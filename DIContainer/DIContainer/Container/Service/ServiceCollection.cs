using System;
using System.Collections.Generic;

namespace DIContainer.Container.Service
{
    public class ServiceCollection
    {
        private Dictionary<Type, Type> transientServices;
        private List<object> singletonServices;

        public ServiceCollection()
        {
            transientServices = new Dictionary<Type, Type>();
            singletonServices = new List<object>();
        }

        public void AddSingleton<T>()
        {
            var instance = Activator.CreateInstance(typeof(T));
            singletonServices.Add(instance);
        }

        public void AddTransient<TService, TImplementaion>() where TImplementaion : TService 
        {
            var key = typeof(TService);
            var value = typeof(TImplementaion);
            transientServices.Add(key, value);
        }

        public Container BuildContainer()
        {
            return new Container(new Services(transientServices, singletonServices));
        }
    }
}
