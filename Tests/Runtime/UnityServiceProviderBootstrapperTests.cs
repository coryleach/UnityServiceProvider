using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Gameframe.ServiceProvider.Tests.Runtime
{
    public class UnityServiceProviderBootstrapperTests
    {
        [Test]
        public void TestCreation()
        {
            var go = new GameObject();
            var bootstrapper = go.AddComponent<UnityServiceProviderBootstrapper>();
            Assert.IsTrue(bootstrapper != null );
            Assert.IsTrue( ServiceProvider.Current is UnityServiceProvider) ;
            Assert.IsTrue( ServiceCollection.Current is UnityServiceProvider );
        }
    }
}
