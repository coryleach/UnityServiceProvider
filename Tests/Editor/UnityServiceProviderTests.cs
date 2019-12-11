using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Gameframe.ServiceProvider.Tests.Editor
{
    public class UnityServiceProviderTests
    {
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
    }
}
