using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameframe.ServiceProvider
{
    public class UnityServiceProvider : IServiceProvider, IServiceCollection
    {
        private readonly Dictionary<Type,Func<object>> _serviceDictionary = new Dictionary<Type, Func<object>>();
        
        public T Get<T>() where T : class
        {
            return GetService(typeof(T)) as T;
        }

        public void GetAll<T>(IList<T> list) where T : class
        {
            list.Clear();
            foreach (var pair in _serviceDictionary.Where(pair => pair.Key.IsSubclassOf(typeof(T)) || pair.Key == typeof(T)))
            {
                list.Add((T)pair.Value.Invoke());
            }
        }
        
        public void AddSingleton<T>(T service) where T : class
        {
            _serviceDictionary[typeof(T)] = () => service;
        }

        public void AddSingleton<T>() where T : class
        {
            var serviceType = typeof(T);

            if (serviceType.IsSubclassOf(typeof(ScriptableObject)) )
            {
                var scriptableObject = ScriptableObject.CreateInstance(serviceType);
                _serviceDictionary[serviceType] = () => scriptableObject;
            }
            else if ( serviceType.IsSubclassOf(typeof(MonoBehaviour) ) )
            {
                var obj = new GameObject(serviceType.ToString());
                UnityEngine.Object.DontDestroyOnLoad(obj);
                var service = obj.AddComponent(serviceType);
                _serviceDictionary[serviceType] = () => service;
            }
            else
            {
                var service = Activator.CreateInstance(serviceType);
                _serviceDictionary[serviceType] = () => service;
            }
        }
        
        #region System.IServiceProvider
        
        public object GetService(Type serviceType)
        {
            if (_serviceDictionary.TryGetValue(serviceType, out var value))
            {
                return value.Invoke();
            }
            return null;
        }
        
        #endregion
    }
}


