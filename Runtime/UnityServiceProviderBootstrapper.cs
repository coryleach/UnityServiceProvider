using UnityEngine;

namespace Gameframe.ServiceProvider
{
    /// <summary>
    /// Creates a UnityServiceProvider instance
    /// Assigns that instance to ServiceProvider.Current and ServiceCollection.Current
    /// Adds a list of singleton UnityEngine.Object instances to the service provider
    /// </summary>
    public class UnityServiceProviderBootstrapper : MonoBehaviour
    {
        /// <summary>
        /// List of singleton service objects to add to the service collection
        /// To add non-unity object services or transient services override ConfigureServices
        /// </summary>
        [SerializeField]
        protected Object[] singletonServices = new Object[0];
        
        /// <summary>
        /// Default implementation instantiates UnityServiceProvider
        /// Sets up public static singleton references in ServiceProvider and ServiceCollection
        /// Then calls ConfigureServices 
        /// </summary>
        protected virtual void Awake()
        {
            //Setup service provider with our unity service provider instance
            var unityServiceProvider = new UnityServiceProvider();
            ServiceProvider.Current = unityServiceProvider;
            ServiceCollection.Current = unityServiceProvider;
            ConfigureServices(unityServiceProvider);
        }

        /// <summary>
        /// Default implementation just adds all singleton services to the configured UnityServiceProvider instance
        /// </summary>
        /// <param name="unityServiceProvider"></param>
        protected virtual void ConfigureServices(UnityServiceProvider unityServiceProvider)
        {
            foreach (var service in singletonServices)
            {
                if (service == null)
                {
                    continue;
                }
                unityServiceProvider.AddSingleton(service.GetType(),service);
            }
        }
    }
}
