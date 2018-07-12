public sealed class Locator_220b15574045425884e18792574dec06 : Robowire.Core.CompiledLocatorBase
{
    private Robowire.UnitTests.RoboApiBasicTests.SomeDependency _f73b580dc01f441d49a6855725fe9ddbc;
    private Robowire.RoboApi.Internal.IControllerNameExtractor _febbaf2defd81474c9d43877cfaed0b07;
    private Robowire.UnitTests.RoboApiBasicTests.Controller1 _f202ea5aaaf7643ee8b804b6ed49e2b95;
    private Robowire.RoboApi.Internal.ControllerIndex _f070c67cd6d20400892d6e8c31a96fe56;

    public Locator_220b15574045425884e18792574dec06(Robowire.IServiceLocator parentLocator, System.Collections.Generic.Dictionary<System.String, System.Func<Robowire.IServiceLocator, System.Object>> factories) : base(parentLocator)
    {

    }

    protected override System.Object InternalGet(System.Type t)
    {
        if (t == typeof(Robowire.UnitTests.RoboApiBasicTests.SomeDependency))
        {
            return Singleton_SomeDependency_9da6f6809a5d4e1a8341d8cc0c6fb8b2();

        }
        if (t == typeof(Robowire.RoboApi.Internal.IControllerNameExtractor))
        {
            return Singleton_IControllerNameExtractor_b9e2645e22c94442a24a78f5b9bc775c();

        }
        if (t == typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1))
        {
            return Singleton_Controller1_1939effb93c14ec5a5397743dc4c58b0();

        }
        if (t == typeof(Robowire.RoboApi.Internal.ControllerIndex))
        {
            return Singleton_ControllerIndex_5904f15aa6c64b42af02e0b6bc3fa48f();

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

    private Robowire.UnitTests.RoboApiBasicTests.SomeDependency FactorySomeDependency_682897081ea147958736969285e7cc86()
    {
        return new Robowire.UnitTests.RoboApiBasicTests.SomeDependency();

    }

    Robowire.UnitTests.RoboApiBasicTests.SomeDependency Singleton_SomeDependency_9da6f6809a5d4e1a8341d8cc0c6fb8b2()
    {
        return (_f73b580dc01f441d49a6855725fe9ddbc ?? (_f73b580dc01f441d49a6855725fe9ddbc = FactorySomeDependency_682897081ea147958736969285e7cc86()));

    }

    private Robowire.RoboApi.Internal.IControllerNameExtractor FactoryIControllerNameExtractor_7b41e4e07f884652a3020548faa0d5d1()
    {
        return new Robowire.RoboApi.Internal.DefaultControllerNameExtractor();

    }

    Robowire.RoboApi.Internal.IControllerNameExtractor Singleton_IControllerNameExtractor_b9e2645e22c94442a24a78f5b9bc775c()
    {
        return (_febbaf2defd81474c9d43877cfaed0b07 ?? (_febbaf2defd81474c9d43877cfaed0b07 = FactoryIControllerNameExtractor_7b41e4e07f884652a3020548faa0d5d1()));

    }

    private System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, System.Type>> GetRoboapiControllerIndexFactory()
    {
        yield return new System.Collections.Generic.KeyValuePair<System.String, System.Type>("controller1", typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1));

    }

    Robowire.UnitTests.RoboApiBasicTests.Controller1 factoryController1()
    {
        return new ControllerProxy_Controller1_9660f4ce0f294faf95718dd7fe9e99d6(this);

    }

    Robowire.UnitTests.RoboApiBasicTests.Controller1 Singleton_Controller1_1939effb93c14ec5a5397743dc4c58b0()
    {
        return (_f202ea5aaaf7643ee8b804b6ed49e2b95 ?? (_f202ea5aaaf7643ee8b804b6ed49e2b95 = factoryController1()));

    }

    private Robowire.RoboApi.Internal.ControllerIndex ControllerIndexFactory_6356c579765241d297c6a2fd797cd51a()
    {
        return new Robowire.RoboApi.Internal.ControllerIndex(this.GetRoboapiControllerIndexFactory());

    }

    Robowire.RoboApi.Internal.ControllerIndex Singleton_ControllerIndex_5904f15aa6c64b42af02e0b6bc3fa48f()
    {
        return (_f070c67cd6d20400892d6e8c31a96fe56 ?? (_f070c67cd6d20400892d6e8c31a96fe56 = ControllerIndexFactory_6356c579765241d297c6a2fd797cd51a()));

    }

    public override Robowire.IServiceLocator CreateLocatorInstance(Robowire.IServiceLocator parentLocator, System.Collections.Generic.Dictionary<System.String, System.Func<Robowire.IServiceLocator, System.Object>> factories)
    {
        return new Locator_220b15574045425884e18792574dec06(parentLocator, factories);

    }

    class ControllerProxy_Controller1_9660f4ce0f294faf95718dd7fe9e99d6 : Robowire.UnitTests.RoboApiBasicTests.Controller1, Robowire.RoboApi.Internal.ILocatorBoundController
    {
        private readonly Robowire.IServiceLocator _f8706244d2698400c895bdd585c7b0724;
        private readonly Robowire.RoboApi.Extensibility.IControllerInterceptor _ff2788ec3b9ee4da28d8698458c388a47;
        private static readonly System.Reflection.MethodInfo _f6529001dab1a4032bab95d6d57f1a3d3 = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetMethodInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "Method1");
        private static readonly System.Reflection.ParameterInfo _f340573a9accb43c0a01f6e29556947ff = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetParameterInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "Method1", "parameter1");
        private static readonly Robowire.RoboApi.Convention.Default.DefaultJsonSerializer _f2f9c143ea88f4db7a445a8928628cdbd = new Robowire.RoboApi.Convention.Default.DefaultJsonSerializer();
        private static readonly System.Reflection.ParameterInfo _fb564526f3b68416a9eee961c5dbc6fa9 = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetParameterInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "Method1", "parameter2");
        private static readonly System.Reflection.ParameterInfo _ff5c7f4b1ccf94e4e83abbbf11633833c = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetParameterInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "Method1", "parameter3");
        private static readonly System.Reflection.MethodInfo _f58c3eb82c83a43718ca5a571a634cdba = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetMethodInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "Method2");
        private static readonly System.Reflection.ParameterInfo _fd4cc515f10f84c0294a8946350f5b29e = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetParameterInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "Method2", "parameter1");
        private static readonly System.Reflection.ParameterInfo _f6161601ca5a34ef6b8a3e8b3f77dc048 = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetParameterInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "Method2", "parameter2");
        private static readonly System.Reflection.ParameterInfo _f069148422d77416aa112c9b1a9037214 = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetParameterInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "Method2", "parameter3");
        private static readonly System.Reflection.MethodInfo _f4ea7240383f740e489f3dd6c6eadaa39 = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetMethodInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "SomeVoidMethod");
        private static readonly System.Reflection.ParameterInfo _fc1537edd6f864204b12491df1df83074 = Robowire.RoboApi.Convention.Default.ReflectonWrapper.GetParameterInfo(typeof(Robowire.UnitTests.RoboApiBasicTests.Controller1), "SomeVoidMethod", "param");

        public ControllerProxy_Controller1_9660f4ce0f294faf95718dd7fe9e99d6(Robowire.IServiceLocator __locator) : base(__locator.Get<Robowire.UnitTests.RoboApiBasicTests.SomeDependency>())
        {
            _f8706244d2698400c895bdd585c7b0724 = __locator;
            _ff2788ec3b9ee4da28d8698458c388a47 = Robowire.RoboApi.Extensibility.InterceptorProvider.GetInterceptor(this);

        }

        public Robowire.IServiceLocator Locator
        {
            get
            {
                return _f8706244d2698400c895bdd585c7b0724;
            }
        }

        public void Execute(System.Web.Routing.RequestContext __context)
        {
            if (_ff2788ec3b9ee4da28d8698458c388a47.OnRequest(this, __context))
            {
                return;
            }
            var vea6d5e847fbf4e4cacea75b4ababcc8d = Robowire.RoboApi.Convention.Default.MethodNameExtractor.ExtractMethodName(__context);
            System.Object __methodCallResult;
            __methodCallResult = null;
            if (vea6d5e847fbf4e4cacea75b4ababcc8d == "method1")
            {
                var v3ea830c0515a4fecacc4a97238cc2646 = new System.Object[3];
                try
                {
                    var vdadffbae741e45a5a474dccbfd174f88 = _ff2788ec3b9ee4da28d8698458c388a47.ObtainParameterValue<System.Int32>(this, _f6529001dab1a4032bab95d6d57f1a3d3, _f340573a9accb43c0a01f6e29556947ff, __context, () => _f2f9c143ea88f4db7a445a8928628cdbd.Read<System.Int32>(_f340573a9accb43c0a01f6e29556947ff, __context));
                    v3ea830c0515a4fecacc4a97238cc2646[0] = vdadffbae741e45a5a474dccbfd174f88;
                    var vfa0ecb06c4fa48c2b574b3ee9a944523 = _ff2788ec3b9ee4da28d8698458c388a47.ObtainParameterValue<System.String>(this, _f6529001dab1a4032bab95d6d57f1a3d3, _fb564526f3b68416a9eee961c5dbc6fa9, __context, () => _f2f9c143ea88f4db7a445a8928628cdbd.Read<System.String>(_fb564526f3b68416a9eee961c5dbc6fa9, __context));
                    v3ea830c0515a4fecacc4a97238cc2646[1] = vfa0ecb06c4fa48c2b574b3ee9a944523;
                    var vfa214e7a32c846cdbabf48e923bf0d5c = _ff2788ec3b9ee4da28d8698458c388a47.ObtainParameterValue<Robowire.UnitTests.RoboApiBasicTests.Controller1>(this, _f6529001dab1a4032bab95d6d57f1a3d3, _ff5c7f4b1ccf94e4e83abbbf11633833c, __context, () => _f2f9c143ea88f4db7a445a8928628cdbd.Read<Robowire.UnitTests.RoboApiBasicTests.Controller1>(_ff5c7f4b1ccf94e4e83abbbf11633833c, __context));
                    v3ea830c0515a4fecacc4a97238cc2646[2] = vfa214e7a32c846cdbabf48e923bf0d5c;
                    _ff2788ec3b9ee4da28d8698458c388a47.Call(this, _f6529001dab1a4032bab95d6d57f1a3d3, __context, v3ea830c0515a4fecacc4a97238cc2646, false, () => {
                        return Method1(vdadffbae741e45a5a474dccbfd174f88, vfa0ecb06c4fa48c2b574b3ee9a944523, vfa214e7a32c846cdbabf48e923bf0d5c);
                    }, __r => _f2f9c143ea88f4db7a445a8928628cdbd.WriteResult(_f6529001dab1a4032bab95d6d57f1a3d3, __context, __r, false));
                }
                catch (System.Exception callException)
                {
                    _ff2788ec3b9ee4da28d8698458c388a47.OnException(this, _f6529001dab1a4032bab95d6d57f1a3d3, __context, v3ea830c0515a4fecacc4a97238cc2646, callException);
                }
                return;
            }
            if (vea6d5e847fbf4e4cacea75b4ababcc8d == "method2")
            {
                var v5afd0df9dd3147c39603cc4551c9e030 = new System.Object[3];
                try
                {
                    var v5e580593333a4d1d8f47b66a6c80c2c0 = _ff2788ec3b9ee4da28d8698458c388a47.ObtainParameterValue<System.Int32>(this, _f58c3eb82c83a43718ca5a571a634cdba, _fd4cc515f10f84c0294a8946350f5b29e, __context, () => _f2f9c143ea88f4db7a445a8928628cdbd.Read<System.Int32>(_fd4cc515f10f84c0294a8946350f5b29e, __context));
                    v5afd0df9dd3147c39603cc4551c9e030[0] = v5e580593333a4d1d8f47b66a6c80c2c0;
                    var v6358654e6d474ad98789a7f59fda1465 = _ff2788ec3b9ee4da28d8698458c388a47.ObtainParameterValue<System.String>(this, _f58c3eb82c83a43718ca5a571a634cdba, _f6161601ca5a34ef6b8a3e8b3f77dc048, __context, () => _f2f9c143ea88f4db7a445a8928628cdbd.Read<System.String>(_f6161601ca5a34ef6b8a3e8b3f77dc048, __context));
                    v5afd0df9dd3147c39603cc4551c9e030[1] = v6358654e6d474ad98789a7f59fda1465;
                    var v428101943bf54ce592ba9430deb5da34 = _ff2788ec3b9ee4da28d8698458c388a47.ObtainParameterValue<Robowire.UnitTests.RoboApiBasicTests.Controller1>(this, _f58c3eb82c83a43718ca5a571a634cdba, _f069148422d77416aa112c9b1a9037214, __context, () => _f2f9c143ea88f4db7a445a8928628cdbd.Read<Robowire.UnitTests.RoboApiBasicTests.Controller1>(_f069148422d77416aa112c9b1a9037214, __context));
                    v5afd0df9dd3147c39603cc4551c9e030[2] = v428101943bf54ce592ba9430deb5da34;
                    _ff2788ec3b9ee4da28d8698458c388a47.Call(this, _f58c3eb82c83a43718ca5a571a634cdba, __context, v5afd0df9dd3147c39603cc4551c9e030, false, () => {
                        return Method2(v5e580593333a4d1d8f47b66a6c80c2c0, v6358654e6d474ad98789a7f59fda1465, v428101943bf54ce592ba9430deb5da34);
                    }, __r => _f2f9c143ea88f4db7a445a8928628cdbd.WriteResult(_f58c3eb82c83a43718ca5a571a634cdba, __context, __r, false));
                }
                catch (System.Exception callException)
                {
                    _ff2788ec3b9ee4da28d8698458c388a47.OnException(this, _f58c3eb82c83a43718ca5a571a634cdba, __context, v5afd0df9dd3147c39603cc4551c9e030, callException);
                }
                return;
            }
            if (vea6d5e847fbf4e4cacea75b4ababcc8d == "somevoidmethod")
            {
                var vf24aa33184ef4d5ab933ac638257a96d = new System.Object[1];
                try
                {
                    var v22e5bbd8eebe4478bb36a70911542101 = _ff2788ec3b9ee4da28d8698458c388a47.ObtainParameterValue<Robowire.UnitTests.RoboApiBasicTests.Controller1>(this, _f4ea7240383f740e489f3dd6c6eadaa39, _fc1537edd6f864204b12491df1df83074, __context, () => _f2f9c143ea88f4db7a445a8928628cdbd.Read<Robowire.UnitTests.RoboApiBasicTests.Controller1>(_fc1537edd6f864204b12491df1df83074, __context));
                    vf24aa33184ef4d5ab933ac638257a96d[0] = v22e5bbd8eebe4478bb36a70911542101;
                    _ff2788ec3b9ee4da28d8698458c388a47.Call(this, _f4ea7240383f740e489f3dd6c6eadaa39, __context, vf24aa33184ef4d5ab933ac638257a96d, true, () => {
                        SomeVoidMethod(v22e5bbd8eebe4478bb36a70911542101);
                        return null;
                    }, __r => _f2f9c143ea88f4db7a445a8928628cdbd.WriteResult(_f4ea7240383f740e489f3dd6c6eadaa39, __context, __r, true));
                }
                catch (System.Exception callException)
                {
                    _ff2788ec3b9ee4da28d8698458c388a47.OnException(this, _f4ea7240383f740e489f3dd6c6eadaa39, __context, vf24aa33184ef4d5ab933ac638257a96d, callException);
                }
                return;
            }
            throw new System.InvalidOperationException("Unknown method");

        }

    }

}
