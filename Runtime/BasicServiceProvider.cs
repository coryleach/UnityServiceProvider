using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Gameframe.ServiceProvider
{
    internal enum ServiceType
    {
        Singleton,
        Transient,
    }
    
    internal class ServiceDescription
    {
        public ServiceType serviceType = ServiceType.Singleton;
        public Func<IServiceProvider, object> factory = null;
        public object service = null;

        public object GetService(IServiceProvider serviceProvider)
        {
            switch (serviceType)
            {
                case ServiceType.Singleton:
                    return service ?? (service = factory.Invoke(serviceProvider));
                default:
                    return factory.Invoke(serviceProvider);
            }
        }
    }
    
    public class BasicServiceProvider : IServiceProvider, IServiceCollection
    {
        private static BasicServiceProvider _sharedInstance = null;
        public static BasicServiceProvider SharedInstance
        {
            get => _sharedInstance ?? (_sharedInstance = new BasicServiceProvider());
            set => _sharedInstance = value;
        }

        private readonly Dictionary<Type, ServiceDescription> serviceDictionary = new Dictionary<Type, ServiceDescription>();
        
        public T Get<T>() where T : class
        {
            return GetService(typeof(T)) as T;
        }

        public void GetAll<T>(IList<T> list) where T : class
        {
            foreach (var pair in serviceDictionary.Where(pair => pair.Value is T))
            {
                list.Add((T)pair.Value.GetService(this));
            }
        }

        #region Add Singleton
        
        public void AddSingleton<T>(T service) where T : class
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Singleton,
                factory = null,
                service = service
            };
            serviceDictionary[ typeof(T) ] = serviceDescription;
        }
        
        public void AddSingleton<T>(Func<IServiceProvider,T> factory) where T : class
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Singleton, 
                factory = factory,
                service = null
            };
            serviceDictionary[ typeof(T) ] = serviceDescription;
        }

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
        
        #endregion

        #region Add Transient
        
        public void AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : TService
        {
            AddTransient(typeof(TService),(provider => factory.Invoke(this)));
        }

        public void AddTransient<TService>(Func<IServiceProvider, TService> factory)
        {
            AddTransient(typeof(TService),(provider => factory.Invoke(this)));
        }
        
        private void AddTransient(Type serviceType, Func<IServiceProvider, Object> factory)
        {
            var serviceDescription = new ServiceDescription
            {
                serviceType = ServiceType.Transient,
                factory = factory,
                service = null
            };
            serviceDictionary[ serviceType ] = serviceDescription;
        }
        
        #endregion

        #region IServiceProvider
        
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


