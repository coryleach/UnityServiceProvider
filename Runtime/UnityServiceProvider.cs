using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameframe.ServiceProvider
{
    
    /// <summary>
    /// UnityServiceProvider provides all the functionality of the basic service provider
    /// but also provides parameterless methods to add services that inherit unity object types
    /// and will automatically create factory methods that instantiate those types for you.
    /// </summary>
    public class UnityServiceProvider : IServiceProvider, IServiceCollection
    {
        private static UnityServiceProvider _sharedInstance = null;
        public static UnityServiceProvider SharedInstance
        {
            get => _sharedInstance ?? (_sharedInstance = new UnityServiceProvider());
            set => _sharedInstance = value;
        }
        
        private readonly Dictionary<Type,ServiceDescription> serviceDictionary = new Dictionary<Type, ServiceDescription>();
        
        /// <summary>
        /// Get Service
        /// </summary>
        /// <typeparam name="TService">Type of service to get.</typeparam>
        /// <returns>Service instance assuming one exists. May return null if service is not provided.</returns>
        public TService Get<TService>() where TService : class
        {
            return GetService(typeof(TService)) as TService;
        }

        /// <summary>
        /// Get all implementations that provide the requested service
        /// </summary>
        /// <param name="list">List to be filled with the service requested</param>
        /// <typeparam name="TService">Type of the service to be requested</typeparam>
        public void GetAll<TService>(IList<TService> list) where TService : class
        {
            list.Clear();
            foreach (var pair in serviceDictionary.Where(pair => typeof(TService).IsAssignableFrom(pair.Key)) )
            {
                list.Add((TService)pair.Value.GetService(this));
            }
        }

        public void AddSingleton(Type serviceType, object service)
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Singleton,
                factory = null,
                service = service
            };
            serviceDictionary[ serviceType ] = serviceDescription;
        }
        
        /// <summary>
        /// Add singleton service instance
        /// </summary>
        /// <param name="service">Service instance</param>
        /// <typeparam name="TService">Service type</typeparam>
        public void AddSingleton<TService>(TService service) where TService : class
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Singleton,
                factory = null,
                service = service
            };
            serviceDictionary[ typeof(TService) ] = serviceDescription;
        }

        /// <summary>
        /// Add a singleton factory that will create the requested service on demand
        /// </summary>
        /// <param name="factory">method that creates a service instance</param>
        /// <typeparam name="TService">Type of the service</typeparam>
        public void AddSingleton<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Singleton,
                factory = factory,
                service = null
            };
            serviceDictionary[ typeof(TService) ] = serviceDescription;
        }

        /// <summary>
        /// Add an existing object as a service
        /// </summary>
        /// <param name="service">Service singleton instance</param>
        /// <typeparam name="TService">Type of the service</typeparam>
        /// <typeparam name="TImplementation">Type that implements the service</typeparam>
        public void AddSingleton<TService, TImplementation>(TImplementation service) where TImplementation : TService where TService : class
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Singleton,
                factory = null,
                service = service
            };
            serviceDictionary[ typeof(TService) ] = serviceDescription;
        }

        /// <summary>
        /// Add a singleton service.
        /// Infers from type how to construct the singleton instance;
        /// </summary>
        /// <typeparam name="TService">Type of the service</typeparam>
        public void AddSingleton<TService>()
        {
            var serviceType = typeof(TService);
            AddSingleton(serviceType,serviceType);
        }
        
        /// <summary>
        /// Add an automatically instantiated Singleton class
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public void AddSingleton<TService,TImplementation>() where TService : class where TImplementation : TService
        {
            var implementationType = typeof(TImplementation);
            var serviceType = typeof(TService);
            AddSingleton(serviceType,implementationType);
        }

        /// <summary>
        /// Add a service type
        /// Infers from service parent classes how object is created
        /// </summary>
        /// <param name="serviceType">Type of the service</param>
        /// <param name="implementationType">Type that implements the service</param>
        private void AddSingleton(Type serviceType, Type implementationType)
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Singleton,
                factory = null,
                service = null
            };

            if (implementationType.IsSubclassOf(typeof(ScriptableObject)) )
            {
                serviceDescription.factory = provider => ScriptableObject.CreateInstance(implementationType);
            }
            else if ( implementationType.IsSubclassOf(typeof(MonoBehaviour) ) )
            {
                serviceDescription.factory = provider =>
                {
                    var obj = new GameObject(implementationType.ToString());
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                    return obj.AddComponent(implementationType);
                };
            }
            else
            {
                serviceDescription.factory = provider => Activator.CreateInstance(implementationType);
            }
            
            serviceDictionary[serviceType] = serviceDescription;
        }
        
        #region Add Transient
        
        public void AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : TService
        {
            AddTransient(typeof(TService),(provider => factory.Invoke(this)));
        }

        public void AddTransient<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            AddTransient(typeof(TService),(provider => factory.Invoke(this)));
        }
        
        private void AddTransient(Type serviceType, Func<IServiceProvider, object> factory)
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Transient,
                factory = factory,
                service = null
            };
            serviceDictionary[ serviceType ] = serviceDescription;
        }
        
        public void AddTransient<TService, TImplementation>() where TService : class where TImplementation : TService
        {
            AddTransient(typeof(TService),typeof(TImplementation));
        }

        public void AddTransient<TService>() where TService : class
        {
            AddTransient(typeof(TService),typeof(TService));
        }
        
        private void AddTransient(Type serviceType, Type implementationType)
        {
            if (implementationType.IsSubclassOf(typeof(ScriptableObject)) )
            {
                AddTransient(serviceType, (serviceProvider) => ScriptableObject.CreateInstance(implementationType));
            }
            else if ( implementationType.IsSubclassOf(typeof(MonoBehaviour) ) )
            {
                AddTransient(serviceType, (serviceProvider) =>
                {
                    var obj = new GameObject(implementationType.ToString());
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                    return obj.AddComponent(implementationType);
                });
            }
            else
            {
                AddTransient(serviceType, provider => Activator.CreateInstance(implementationType));
            }
        }
        
        #endregion
        
        #region System.IServiceProvider
        
        /// <summary>
        /// Get Service
        /// </summary>
        /// <param name="serviceType">Type of service to get.</param>
        /// <returns>Service instance.</returns>
        public object GetService(Type serviceType)
        {
            var serviceDescription = serviceDictionary.TryGetValue(serviceType, out var value) ? value : null;

            if (serviceDescription == null)
            {
                return null;
            }

            switch (serviceDescription.serviceType)
            {
                case ServiceType.Singleton:
                    return serviceDescription.service ?? (serviceDescription.service = serviceDescription.factory.Invoke(this));
                default:
                    return serviceDescription.factory.Invoke(this);
            }
        }
        
        #endregion
    }
}


