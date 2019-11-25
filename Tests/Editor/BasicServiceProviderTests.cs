using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Gameframe.ServiceProvider.Tests.Editor
{
    public class BasicServiceProviderEditorTests
    {
        private class FakeService : IFakeService
        {
            
        }

        private interface IFakeService
        {
            
        }

        [Test]
        public void DefaultServiceProviderExists()
        {
            // Use the Assert class to test conditions
            Assert.NotNull(ServiceProvider.Current,"ServiceProvider.Current != null");
            Assert.True(ServiceProvider.Current is BasicServiceProvider);
        }

        [Test]
        public void CanGetServiceByInterface()
        {
            var service = new FakeService();
            ServiceCollection.Current.AddSingleton(service);
            Assert.True(ServiceProvider.Current.Get<IFakeService>() == service);
        }
        
    }
}
