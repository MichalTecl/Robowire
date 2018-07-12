using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.Core
{
    public abstract class CompiledLocatorBase : IServiceLocator, ILocatorFactory
    {
        private readonly IServiceLocator m_parentLocator;

        private readonly List<IDisposable> m_disposables = new List<IDisposable>();

        protected CompiledLocatorBase(IServiceLocator parentLocator)
        {
            m_parentLocator = parentLocator;
        }

        public void Dispose()
        {
            foreach (var disposable in m_disposables)
            {
                disposable.Dispose();
            }
        }

        public IServiceLocator Parent
        {
            get
            {
                return m_parentLocator;
            }
        }
        
        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public object Get(Type t)
        {
            return InternalGet(t);
        }

        public IEnumerable<T> GetCollection<T>()
        {
            if (Parent != null)
            {
                foreach (var parentItem in Parent.GetCollection<T>())
                {
                    yield return parentItem;
                }
            }

            foreach (var item in GetCollectionItems(typeof(T)).OfType<T>())
            {
                yield return item;
            }
        }

        public object InstantiateNow(ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters().Select(p => Get(p.ParameterType)).ToArray();

            return ctor.Invoke(parameters);
        }

        public object InstantiateNow(Type type)
        {
            var ctor =
                type.GetConstructors()
                    .Where(c => !c.GetParameters().Any(p => p.ParameterType.IsPrimitive))
                    .OrderBy(c => c.GetParameters().Length)
                    .FirstOrDefault();
            if (ctor == null)
            {
                throw new InvalidOperationException("No suitable constructor found");
            }

            return InstantiateNow(ctor);
        }

        public T InstantiateNow<T>()
        {
            return (T)InstantiateNow(typeof(T));
        }

        protected abstract IEnumerable GetCollectionItems(Type collectionType);

        public abstract IServiceLocator CreateLocatorInstance(
            IServiceLocator parentLocator,
            Dictionary<string, Func<IServiceLocator, object>> factories);

        protected abstract object InternalGet(Type t);

        protected T TryRegisterDisposable<T>(T d)
        {
            var disposable = d as IDisposable;
            if (disposable != null)
            {
                m_disposables.Add(disposable);
            }

            return d;
        }

        protected object TryGetFromParent(Type t)
        {
            if (Parent == null)
            {
                throw new InvalidOperationException($"No implementation registered for type {t}");
            }

            return Parent.Get(t);
        }

    }


}
