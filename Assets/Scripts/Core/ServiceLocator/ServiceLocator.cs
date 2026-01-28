using System;
using System.Collections.Generic;
using UnityEngine;

namespace CFD.Core
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, IService> services = new Dictionary<Type, IService>();
        
        public static void Register<T>(IService service) where T : IService
        {
            if (services.ContainsKey(typeof(T)))
                Debug.LogError($"Service {typeof(T).Name} already registered");
            else
                services.Add(typeof(T), service);
        }
        
        public static T Resolve<T>() where T : IService
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                return (T) service;
            }

            Debug.LogError($"Service {typeof(T).Name} not registered");
            throw new Exception($"Service {typeof(T).Name} not registered");
        }
    }
}