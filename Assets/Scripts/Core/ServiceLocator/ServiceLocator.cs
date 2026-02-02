using System;
using System.Collections.Generic;
using UnityEngine;

namespace CFD.Core
{
    /// <summary>
    /// Service locator for dependency injection
    /// </summary>
    public static class ServiceLocator
    {
        private static Dictionary<Type, IService> services = new Dictionary<Type, IService>();
        
        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="T">IService type to bind to <see cref="service"/></typeparam>
        /// <param name="service">Instance of the service to register</param>
        public static void Register<T>(IService service) where T : IService
        {
            if (services.ContainsKey(typeof(T)))
                Debug.LogError($"Service {typeof(T).Name} already registered");
            else
                services.Add(typeof(T), service);
        }
        
        /// <summary>
        /// Get an instance of the service registered for IService type
        /// </summary>
        /// <typeparam name="T">IService type to resolve <see cref="service"/></typeparam>
        /// <returns>Instance of the service</returns>
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