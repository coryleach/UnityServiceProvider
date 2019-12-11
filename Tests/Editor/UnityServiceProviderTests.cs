using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections.Generic;

namespace Gameframe.ServiceProvider.Tests.Editor
{
    public class UnityServiceProviderTests
    {
        private class FakeServiceA : IFakeService
        {
            public static int instanceCount = 0;

            public FakeServiceA()
            {
                instanceCount++;
            }
        }
        
        private class FakeServiceB : IFakeService
        {
            
        }
        
        private class FakeServiceC : object
        {
            
        }

        private interface IFakeService
        {
            
        }
        
        [SetUp]
        public void Setup()
        {
            var unityServiceProvider = new UnityServiceProvider();
            ServiceProvider.Current = unityServiceProvider;
            ServiceCollection.Current = unityServiceProvider;
        }
        
        [Test]
        public void TestSetupUnityServiceProvider()
        {
            Assert.IsTrue(ServiceProvider.Current is UnityServiceProvider);
        }
        
        [Test]
        public void TestSetupUnityServiceCollection()
        {
            Assert.IsTrue(ServiceCollection.Current is UnityServiceProvider);
        }
        
        [Test]
        public void CanGetServiceByInterface()
        {
            Assert.True(ServiceProvider.Current is UnityServiceProvider,"Not the Unity service provider");
            
            var service = new FakeServiceA();
            ServiceCollection.Current.AddSingleton<IFakeService,FakeServiceA>(service);
            Assert.True(ServiceProvider.Current.Get<IFakeService>() != null);
            Assert.True(ServiceProvider.Current.Get<IFakeService>() == service);
        }
        
        [Test]
        public void CanGetAllServicesForInterface()
        {
            Assert.True(ServiceProvider.Current is UnityServiceProvider,"Not the Unity service provider");
            
            var serviceA = new FakeServiceA();
            ServiceCollection.Current.AddSingleton(serviceA);
            
            var serviceB = new FakeServiceB();
            ServiceCollection.Current.AddSingleton(serviceB);
            
            var serviceC = new FakeServiceC();
            ServiceCollection.Current.AddSingleton(serviceC);
            
            List<IFakeService> list = new List<IFakeService>();
            ServiceProvider.Current.GetAll(list);
            
            Assert.True(list.Count == 2,$"IFakeService count = {list.Count} but we expected 2");
        }

        [Test]
        public void CanAddSingletonServiceWithFactory()
        {
            FakeServiceA.instanceCount = 0;
            ServiceCollection.Current.AddSingleton((serviceProvider) => new FakeServiceA());
            
            //Service should not be instantiated yet
            Assert.True(FakeServiceA.instanceCount == 0);

            var service = ServiceProvider.Current.Get<FakeServiceA>();
            Assert.NotNull(service);
            
            //Should have instantiated when it was asked for
            Assert.True(FakeServiceA.instanceCount == 1);

            var serviceAgain = ServiceProvider.Current.Get<FakeServiceA>();
            
            //Assert the second time we get the service it's the same singleton instance and the count hasn't gone up
            Assert.True(service==serviceAgain);
            Assert.True(FakeServiceA.instanceCount == 1);
        }

        [Test]
        public void CanAddAndGetTransientService()
        {
            FakeServiceA.instanceCount = 0;
            ServiceCollection.Current.AddTransient((serviceProvider) => new FakeServiceA());
            Assert.True(FakeServiceA.instanceCount == 0);

            var service1 = ServiceProvider.Current.Get<FakeServiceA>();
            Assert.True(FakeServiceA.instanceCount == 1);
            var service2 = ServiceProvider.Current.Get<FakeServiceA>();
            Assert.True(FakeServiceA.instanceCount == 2);
            Assert.False(service1 == service2);
        }
    }
}
