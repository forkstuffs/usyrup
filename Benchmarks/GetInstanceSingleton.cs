using NUnit.Framework;
using Syrup.Framework;
using Syrup.Framework.Declarative;
using Unity.PerformanceTesting;

public class GetInstanceSingleton {
    private const int Iteration = 10_000;

    [Test]
    [Performance]
    public void Test() {
        SyrupInjectorOptions options = new SyrupInjectorOptions {
            EnableAutomaticConstructorSelection = true
        };
        TestModule module = new TestModule();
        SyrupInjector injector = new SyrupInjector(options, module);

        Measure.Method(() => {
            for (int i = 0; i < Iteration; i++) {
                injector.GetInstance<ITestClass1>();
                injector.GetInstance<ITestClass2>();
                injector.GetInstance<ITestClass3>();
            }
        }).SampleGroup("syrup").GC().Run();
    }

    [Test]
    [Performance]
    public void Test2() {
        SyrupInjectorOptions options = new SyrupInjectorOptions {
            EnableAutomaticConstructorSelection = true
        };
        Test2Module module = new Test2Module();
        SyrupInjector injector = new SyrupInjector(options, module);

        Measure.Method(() => {
            for (int i = 0; i < Iteration; i++) {
                injector.GetInstance<ITestClass1>();
                injector.GetInstance<ITestClass2>();
                injector.GetInstance<ITestClass3>();
            }
        }).SampleGroup("syrup").GC().Run();
    }

    private class TestClass1 : ITestClass1 { }
    private interface ITestClass1 { }

    private class TestClass2 : ITestClass2 { }
    private interface ITestClass2 { }

    private class TestClass3 : ITestClass3 { }
    private interface ITestClass3 { }

    private class TestModule : ISyrupModule {
        public void Configure(IBinder binder) {
            binder.Bind<ITestClass1>().To<TestClass1>().AsSingleton();
            binder.Bind<ITestClass2>().To<TestClass2>().AsSingleton();
            binder.Bind<ITestClass3>().To<TestClass3>().AsSingleton();
        }
    }

    private class Test2Module : ISyrupModule {
        public void Configure(IBinder binder) {
            binder.Bind<ITestClass1>().To<TestClass1>();
            binder.Bind<ITestClass2>().To<TestClass2>();
            binder.Bind<ITestClass3>().To<TestClass3>();
        }
    }
}
