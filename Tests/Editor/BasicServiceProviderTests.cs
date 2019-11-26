using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.ServiceProvider.Tests.Editor
{
    public class BasicServiceProviderEditorTests
    {
        private class FakeServiceA : IFakeService
        {
            
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
            //Force the test to create a new instance
            BasicServiceProvider.SharedInstance = null;
            ServiceProvider.Current = null;
            ServiceCollection.Current = null;
        }

        [Test]
        public void DefaultServiceProviderExists()
        {
            Assert.True(BasicServiceProvider.SharedInstance.Count == 0, "Service Provider Not Empty");
            Assert.True(ServiceProvider.Current == BasicServiceProvider.SharedInstance,"Not the basic service provider");
            
            // Use the Assert class to test conditions
            Assert.NotNull(ServiceProvider.Current,"ServiceProvider.Current != null");
            Assert.True(ServiceProvider.Current is BasicServiceProvider);
        }

        [Test]
        public void CanGetServiceByInterface()
        {
            Assert.True(BasicServiceProvider.SharedInstance.Count == 0, "Service Provider Not Empty");
            Assert.True(ServiceProvider.Current == BasicServiceProvider.SharedInstance,"Not the basic service provider");
            
            var service = new FakeServiceA();
            ServiceCollection.Current.AddSingleton<IFakeService,FakeServiceA>(service);
            Assert.True(ServiceProvider.Current.Get<IFakeService>() != null);
            Assert.True(ServiceProvider.Current.Get<IFakeService>() == service);
        }
        
        [Test]
        public void CanGetAllServicesForInterface()
        {
            Assert.True(BasicServiceProvider.SharedInstance.Count == 0, "Service Provider Not Empty");
            Assert.True(ServiceProvider.Current == BasicServiceProvider.SharedInstance,"Not the basic service provider");
            
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
        
    }
}
