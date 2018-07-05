public sealed class Locator_c0683c5c0096479583afff60ea444de3 : Robowire.Core.CompiledLocatorBase
{
    private Robowire.UnitTests.AutoImplPluginTests.IAutoImplemented _f3c53956b85024b31b2e097bedfaf37a3;

    public Locator_c0683c5c0096479583afff60ea444de3(Robowire.IServiceLocator parentLocator, System.Collections.Generic.Dictionary<string, System.Func<Robowire.IServiceLocator, System.Object>> factories) : base(parentLocator)
    {

    }

    protected override System.Object InternalGet(System.Type t)
    {
        if (t == typeof(Robowire.UnitTests.AutoImplPluginTests.IAutoImplemented))
        {
            return Singleton_IAutoImplemented();

        }
        if (t == typeof(Robowire.IServiceLocator))
        {
            return this;
        }
        return TryGetFromParent(t);

    }

    protected override System.Collections.IEnumerable GetCollectionItems(System.Type collectionType)
    {
        yield break;

    }

    private Robowire.UnitTests.AutoImplPluginTests.IAutoImplemented factory_IAutoImplemented_9c25e62a1aa241bc94ebd41339bedf8c()
    {
        return new ReturnMemberNameImplementorPlugin_implements_IAutoImplemented_8786ff613b59495ea29181264b707fd1(this);

    }

    Robowire.UnitTests.AutoImplPluginTests.IAutoImplemented DisposeIAutoImplemented_246cf8565b0248b1bbde2122af632704()
    {
        return TryRegisterDisposable(factory_IAutoImplemented_9c25e62a1aa241bc94ebd41339bedf8c());

    }

    Robowire.UnitTests.AutoImplPluginTests.IAutoImplemented Singleton_IAutoImplemented()
    {
        return (_f3c53956b85024b31b2e097bedfaf37a3 ?? (_f3c53956b85024b31b2e097bedfaf37a3 = DisposeIAutoImplemented_246cf8565b0248b1bbde2122af632704()));

    }

    public override Robowire.IServiceLocator CreateLocatorInstance(Robowire.IServiceLocator parentLocator, System.Collections.Generic.Dictionary<System.String, System.Func<Robowire.IServiceLocator, System.Object>> factories)
    {
        return new Locator_c0683c5c0096479583afff60ea444de3(parentLocator, factories);

    }

    private sealed class ReturnMemberNameImplementorPlugin_implements_IAutoImplemented_8786ff613b59495ea29181264b707fd1 : object, Robowire.UnitTests.AutoImplPluginTests.IAutoImplemented
    {
        Robowire.IServiceLocator _fc0b59931d342400585fb13b66179ffdd;

        public ReturnMemberNameImplementorPlugin_implements_IAutoImplemented_8786ff613b59495ea29181264b707fd1(Robowire.IServiceLocator locator)
        {
            _fc0b59931d342400585fb13b66179ffdd = locator;

        }

        public System.String Property1
        {
            get
            {
                return "Property1";
            }
        }

        public System.String Method1(System.Int32 a, System.Boolean b)
        {
            return "Method1";

        }

    }

}
