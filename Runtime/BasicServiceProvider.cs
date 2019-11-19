using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameframe.ServiceProvider
{
    public class BasicServiceProvider : IServiceProvider, IServiceCollection
    {
        private static BasicServiceProvider _sharedInstance = null;
        public static BasicServiceProvider SharedInstance
        {
            get => _sharedInstance ?? (_sharedInstance = new BasicServiceProvider());
            set => _sharedInstance = value;
        }

        private readonly Dictionary<Type, object> serviceDictionary = new Dictionary<Type, object>();

        public T Get<T>() where T : class
        {
            return GetService(typeof(T)) as T;
        }

        public void GetAll<T>(IList<T> list) where T : class
        {
            foreach (var pair in serviceDictionary.Where(pair => pair.Value is T))
            {
                list.Add((T)pair.Value);
            }
        }

        public void AddSingleton<T>(T service) where T : class
        {
            serviceDictionary[ typeof(T) ] = service;
        }

        #region IServiceProvider
        
        public object GetService(Type serviceType)
        {
            return serviceDictionary.TryGetValue(serviceType, out var value) ? value : null;
        }
        
        #endregion
        
    }
}


