using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameframe.ServiceProvider
{
    public class BasicServiceProvider : IServiceProvider, IServiceCollection
    {
        private readonly Dictionary<Type, object> _serviceDictionary = new Dictionary<Type, object>();

        public T Get<T>() where T : class
        {
            return GetService(typeof(T)) as T;
        }

        public void GetAll<T>(IList<T> list) where T : class
        {
            foreach (var pair in _serviceDictionary.Where(pair => pair.Value is T))
            {
                list.Add((T)pair.Value);
            }
        }

        public void AddSingleton<T>(T service) where T : class
        {
            _serviceDictionary[ typeof(T) ] = service;
        }

        #region IServiceProvider
        
        public object GetService(Type serviceType)
        {
            return _serviceDictionary.TryGetValue(serviceType, out var value) ? value : null;
        }
        
        #endregion
        
    }
}


