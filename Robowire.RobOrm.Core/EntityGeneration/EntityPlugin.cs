﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using CodeGeneration;
using CodeGeneration.Primitives;

using Robowire.Behavior;
using Robowire.Core;
using Robowire.Plugin;
using Robowire.RobOrm.Core.EntityModel;
using Robowire.RobOrm.Core.Internal;

namespace Robowire.RobOrm.Core.EntityGeneration
{
    public class EntityPlugin : IPlugin
    {
        private readonly IRobOrmSetup m_setup;

        public EntityPlugin(IRobOrmSetup setup)
        {
            m_setup = setup;
        }

        public bool IsApplicable(IServiceSetupRecord setup)
        {
            return setup.GetBehavior<EntityBehavior>() != null;
        }

        public INamedReference GenerateFactoryMethod(
            IServiceSetupRecord setup,
            Dictionary<string, INamedReference> ctorParamValueFactoryFields,
            INamedReference valueFactoryField,
            IClassBuilder locatorBuilder,
            INamedReference previousPluginMethod)
        {
            if (!IsApplicable(setup))
            {
                return previousPluginMethod;
            }

            var behavior = setup.Behaviors.OfType<EntityBehavior>().Last();
            
            var entityClass =
                locatorBuilder.HasNestedClass($"{TypeNameHelper.GetTypeMark(setup.InterfaceType)}_Impl_{Guid.NewGuid():N}")
                    .WithModifier("private")
                    .WithModifier("sealed")
                    .Implements<IEntity>()
                    .Implements(setup.InterfaceType);

            var locatorField = entityClass.HasField<IServiceLocator>();

            var ctor = entityClass.WithConstructor().WithModifier("public");
            var locatorParam = ctor.WithParam<IServiceLocator>("locator");
            ctor.Body.Assign(locatorField, a => a.Write(locatorParam)).EndStatement();
            ctor.Body.Write("IsDirty = true").EndStatement();

            var entityProperties = ImplementProperties(entityClass, setup.InterfaceType, behavior).ToList();
            
            ImplementIsDirtyProperty(entityClass);
            var pkProperty = ImplementReadPrimaryKeyAndGetPkProperty(entityClass, behavior, setup.InterfaceType);
            ImplementGetSaveMethodTypeMethod(entityClass, pkProperty);
            ImplementGetValuesMethod(entityClass, entityProperties, pkProperty);
            ImplementReadSelfMethod(entityClass, locatorField, entityProperties);
            ImplementReferenceEntityProperties(entityClass, setup.InterfaceType, m_setup.EntityNamingConvention);
            ImplementDbEntityNameProperty(entityClass, setup.InterfaceType);
            ImplementPkTypeProperty(entityClass, pkProperty);
            ImplementIsPkAuto(entityClass, pkProperty);

            var collectionMethod = locatorBuilder.HasMethod("GetEntitiesCollection").Returns(typeof(IEnumerable<Type>));
            collectionMethod.Body.Write("yield return typeof(")
                .Write(setup.InterfaceType)
                .Write(")")
                .EndStatement()
                .NewLine();

            var factoryMethod = locatorBuilder.HasMethod($"{TypeNameHelper.GetTypeMark(setup.InterfaceType)}_{Guid.NewGuid():N}").Returns(setup.InterfaceType);
            factoryMethod.Body.Write("return ")
                .InvokeConstructor(entityClass, inv => inv.WithParam("this"))
                .EndStatement();

            return factoryMethod;
        }

        private void ImplementGetSaveMethodTypeMethod(IClassBuilder entityClass, PropertyInfo pkProperty)
        {
            var method =
                entityClass.ImplementsMethod(typeof(IEntity).GetMethod(nameof(IEntity.GetSaveMethodType)))
                    .WithModifier("public");

            if (!PkHandlingHelper.IsPkAutogenerated(pkProperty))
            {
                method.Body.Write("return ")
                    .Write(typeof(SaveMethodType))
                    .Write(".")
                    .Write(nameof(SaveMethodType.Merge))
                    .EndStatement();

                return;
            }

            method.Body.Write("return ").Write(pkProperty.Name).Write(" == ").Write("default(").Write(pkProperty.PropertyType).Write(") ?")
                .Write(typeof(SaveMethodType)).Write(".").Write(nameof(SaveMethodType.Insert)).Write(" : ")
                .Write(typeof(SaveMethodType)).Write(".").Write(nameof(SaveMethodType.Update)).EndStatement();
        }

        private void ImplementGetValuesMethod(IClassBuilder entityClass, IEnumerable<PropertyInfo> entityProperties, PropertyInfo pkProperty)
        {
            var method = entityClass.ImplementsMethod(typeof(IEntity).GetMethod(nameof(IEntity.GetValues))).WithModifier("public");

            method.Body.Write("yield return ")
                .InvokeConstructor(
                    typeof(EntityColumnValue),
                    inv =>
                        inv.WithParam(p => p.String(pkProperty.Name))
                            .WithParam(p => p.Write(true))
                            .WithParam(p => p.Write(pkProperty.Name)))
                .EndStatement();

            foreach (var prop in entityProperties)
            {
                method.Body.Write("yield return ")
                    .InvokeConstructor(
                        typeof(EntityColumnValue),
                        inv =>
                            inv.WithParam(p => p.String(prop.Name))
                                .WithParam(p => p.Write(false))
                                .WithParam(p => p.Write(prop.Name)))
                    .EndStatement();
            }
        }

        private void ImplementIsDirtyProperty(IClassBuilder entityClass)
        {
            var isDirtyField = entityClass.HasField<bool>("isDirty");

            var prop = entityClass.HasProperty<bool>("IsDirty").WithModifier("public");

            prop.HasGetter().Write("return ").Write(isDirtyField).EndStatement();

            var setter = prop.HasSetter();
            setter.Assign(isDirtyField, asi => asi.Write(setter.ValueParameter));
        }
        
        private PropertyInfo ImplementReadPrimaryKeyAndGetPkProperty(
            IClassBuilder entityClass,
            EntityBehavior behavior,
            Type entityType)
        {
            var pkProperty = ReflectionUtil.GetProperty(entityType, behavior.PrimaryKeyProperty);
            if (pkProperty == null)
            {
                throw new InvalidOperationException($"{entityType.Name} is missing {behavior.PrimaryKeyProperty} property");
            }

            var idField = entityClass.HasField(pkProperty.PropertyType);

            var idProp = entityClass.HasProperty(pkProperty.Name, pkProperty.PropertyType).WithModifier("public");
            idProp.HasGetter().Write("return ").Write(idField).EndStatement();
            idProp.HasSetter().Assign(idField, a => a.Write("value")).EndStatement();

            var primaryKeyValueProperty = entityClass.HasProperty(nameof(IEntity.PrimaryKeyValue), typeof(object))
                .WithModifier("public");
            primaryKeyValueProperty.HasGetter().Write("return ").Write(idProp).EndStatement();

            var primaryKeyValueSetter = primaryKeyValueProperty.HasSetter();
            primaryKeyValueSetter.Write(idProp).Write(" = ").Write("(").Write(pkProperty.PropertyType).Write(")").Write(primaryKeyValueSetter.ValueParameter).EndStatement();

            var readPkMethod = entityClass.HasMethod(nameof(IEntity.ReadPrimaryKey)).WithModifier("public").Returns<bool>();
            var readerParam = readPkMethod.WithParam<IDataRecord>("reader");

            readPkMethod.Body.If(
                c =>
                    c.Write(readerParam)
                        .Write(".")
                        .Invoke(
                            nameof(IDataRecord.IsNull),
                            p => p.WithParam(code => code.Write("\"").Write(pkProperty.Name).Write("\""))),
                then => then.Write("return false").EndStatement()).NewLine();

            readPkMethod.Body.Assign(
                idField,
                a =>
                    a.Write(readerParam)
                        .Write(".Get<")
                        .Write(pkProperty.PropertyType)
                        .Write(">(\"")
                        .Write(pkProperty.Name)
                        .Write("\")")).EndStatement();

            readPkMethod.Body.Write("return true").EndStatement();

            return pkProperty;
        }

        private void ImplementReadSelfMethod(IClassBuilder entityClass, IClassFieldBuilder locatorField, List<PropertyInfo> columnProperties)
        {
            var readMethod = entityClass.HasMethod("ReadSelf").WithModifier("public").ReturnsVoid();
            var rowParam = readMethod.WithParam<IDataRecord>("dataRow");

            readMethod.Body.Write("if(!ReadPrimaryKey(").Write(rowParam).Write(")){ return; }").NewLine();

            foreach (var columnProp in columnProperties)
            {
                readMethod.Body.If(
                    c => c.Write("!").Write(rowParam).Write(".IsNull(\"").Write(columnProp.Name).Write("\")"),
                    then =>
                        {
                            then.Write(columnProp.Name)
                                .Write(" = ")
                                .Write(rowParam)
                                .Write(".Get<")
                                .Write(columnProp.PropertyType)
                                .Write(">(\"").Write(columnProp.Name).Write("\")")
                                .EndStatement();
                        },
                    els =>
                        {
                            els.Write(columnProp.Name)
                                .Write(" = default(")
                                .Write(columnProp.PropertyType)
                                .Write(")")
                                .EndStatement();
                        });
            }
        }

        private IEnumerable<PropertyInfo> ImplementProperties(IClassBuilder entityClass, Type setupInterfaceType, EntityBehavior behavior)
        {
            foreach (var sourceProperty in ReflectionUtil.GetAllProperties(setupInterfaceType))
            {
                if (sourceProperty.Name == behavior.PrimaryKeyProperty)
                {
                    continue;
                }

                if (!sourceProperty.CanRead)
                {
                    continue;
                }

                try
                {
                    if (string.IsNullOrWhiteSpace(m_setup.EntityNamingConvention.GetColumnName(sourceProperty)))
                    {
                        if (m_setup.EntityNamingConvention.TryGetRefEntityType(sourceProperty) == null)
                        {
                            ImplementDefaultProperty(entityClass, sourceProperty);
                        }

                        continue;
                    }
                    else
                    {
                        if (!sourceProperty.CanWrite)
                        {
                            throw new InvalidOperationException(
                                      $"Property {sourceProperty.DeclaringType?.Name}.{sourceProperty.Name} seems to be a column, but has no setter");
                        }

                        if ((sourceProperty.Name.Length > 2) && sourceProperty.Name.EndsWith("Id") && !Attribute.IsDefined(sourceProperty, typeof(NotFkAttribute)))
                        {
                            var typedPropName = sourceProperty.Name.Substring(0, sourceProperty.Name.Length - 2);

                            var typedProp = ReflectionUtil.GetProperty(setupInterfaceType, typedPropName);
                            if ((typedProp == null)
                                || (m_setup.EntityNamingConvention.TryGetRefEntityType(typedProp) == null))
                            {
                                throw new InvalidOperationException($"Property {sourceProperty.DeclaringType}.{sourceProperty.Name} seems to be a foreign key, but there is not any property {typedPropName} of Entity type to describe the reference. Add {typedPropName} or mark the property with {typeof(NotFkAttribute)}");
                            }
                        }

                        ImplementColumnProperty(entityClass, sourceProperty);
                       
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to map property {sourceProperty.DeclaringType}.{sourceProperty.Name}", ex);
                }

                yield return sourceProperty;
            }
        }

        private void ImplementColumnProperty(IClassBuilder entityClass, PropertyInfo sourceProperty)
        {
            ImplementDefaultProperty(entityClass, sourceProperty, true);
        }

        private void ImplementDefaultProperty(IClassBuilder entityClass, PropertyInfo sourceProperty, bool setDirtyFlag = false)
        {
            var backingField = entityClass.HasField(sourceProperty.PropertyType);

            var property = entityClass.HasProperty(sourceProperty.Name, sourceProperty.PropertyType).WithModifier("public");

            property.HasGetter().Write("return ").Write(backingField).EndStatement();

            var setter = property.HasSetter();

            if (setDirtyFlag)
            {
                setter.If(
                    c => c.Write(backingField).Write(" == ").Write(setter.ValueParameter),
                    then => then.Write("return").EndStatement());

                setter.Write("IsDirty = true").EndStatement();
            }

            setter.Assign(backingField, a => a.Write(setter.ValueParameter)).EndStatement();
        }

        private void ImplementReferenceEntityProperties(IClassBuilder entityClass, Type entityType, IEntityNamingConvention convention)
        {
            var getReferencePropertiesMethod =
                entityClass.HasMethod(nameof(IEntity.GetReferenceProperties))
                    .Returns<IEnumerable<Tuple<string, Type>>>()
                    .WithModifier("public");

            var openPropertyMethod =
                entityClass.HasMethod(nameof(IEntity.OpenProperty)).Returns<IEntitySet>().WithModifier("public");
            var openPropertyNameParam = openPropertyMethod.WithParam<string>("propertyName");
            
            foreach (var property in ReflectionUtil.GetAllProperties(entityType))
            {
                var foreignEntityType = convention.TryGetRefEntityType(property);
                if (foreignEntityType == null)
                {
                    continue;
                }

                var refNameTypeField =
                    entityClass.HasField<Tuple<string, Type>>().WithModifier("private").WithModifier("readonly").WithAssignment(a =>
                        a.InvokeConstructor(
                            typeof(Tuple<string, Type>),
                            inv =>
                                inv.WithParam(p => p.String(property.Name))
                                    .WithParam(p => p.Typeof(foreignEntityType))));
                
                getReferencePropertiesMethod.Body.Write("yield return ").Write(refNameTypeField).EndStatement();
                
                INamedReference backingField;
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    backingField =
                        entityClass.HasField<EntityList>()
                            .WithAssignment(a => a.InvokeConstructor(typeof(EntityList), x => { }));
                    


                    entityClass.HasProperty(property.Name, property.PropertyType)
                        .WithModifier("public")
                        .HasGetter()
                        .Write("return ")
                        .Write(typeof(System.Linq.Enumerable))
                        .Write(".")
                        .Write("OfType<")
                        .Write(foreignEntityType)
                        .Write(">(")
                        .Write(backingField)
                        .Write(")")
                        .EndStatement();
                }
                else
                {
                    backingField =
                        entityClass.HasField<EntityHolder>()
                            .WithAssignment(a => a.InvokeConstructor(typeof(EntityHolder), x => { }));

                    entityClass.HasProperty(property.Name, property.PropertyType)
                        .WithModifier("public")
                        .HasGetter()
                        .Write("return ")
                        .Write(backingField)
                        .Write(".Value")
                        .Write(" as ")
                        .Write(foreignEntityType)
                        .EndStatement();
                }

                openPropertyMethod.Body.If(
                    c => c.Compare(openPropertyNameParam, b => b.String(property.Name)),
                    then => then.Write("return ").Write(backingField).EndStatement()).NewLine();
            }

            openPropertyMethod.Body.Write("throw ")
                .InvokeConstructor(
                    typeof(InvalidOperationException),
                    i => i.WithParam(p => p.String("Unknown property requested")))
                .EndStatement();

            getReferencePropertiesMethod.Body.Write("yield break").EndStatement();
        }

        private void ImplementDbEntityNameProperty(IClassBuilder entityClass, Type entityType)
        {
            var entityName = NamingHelper.GetEntityName(entityType);

            var getter = entityClass.HasProperty<string>(nameof(IEntity.DbEntityName)).WithModifier("public").HasGetter();
            getter.Write("return ").String(entityName).EndStatement();
        }

        private void ImplementPkTypeProperty(IClassBuilder entityClass, PropertyInfo pkProperty)
        {
            entityClass.HasProperty<Type>(nameof(IEntity.PrimaryKeyType))
                .WithModifier("public")
                .HasGetter()
                .Write("return typeof(")
                .Write(pkProperty.PropertyType)
                .Write(")")
                .EndStatement();
        }

        private void ImplementIsPkAuto(IClassBuilder entityClass, PropertyInfo pkProperty)
        {
            var value = PkHandlingHelper.IsPkAutogenerated(pkProperty);

            entityClass.HasPublicProperty<bool>(nameof(IEntity.IsPrimaryKeyAutogenerated)).Returns(p => p.Write(value));
        }

        public IPlugin InheritToChildContainer()
        {
            return this;
        }

    }
}
