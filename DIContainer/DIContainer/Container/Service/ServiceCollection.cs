using System;
using System.Collections.Generic;
using System.Linq;

namespace DIContainer.Container.Service
{
    public class ServiceCollection
    {
        private Dictionary<object, ServiceInfo> transientServices;
        private Dictionary<object, ServiceInfo> singletonServices;

        public ServiceCollection()
        {
            transientServices = new Dictionary<object, ServiceInfo>();
            singletonServices = new Dictionary<object, ServiceInfo>();
        }


        private T GetService<T>()
        {
           return (T)GetService(typeof(T));
        }

        private object GetService(object type)
        {
            ServiceInfo instance;

           if(singletonServices.TryGetValue(type,out instance))
            {
                instance = singletonServices[type];
            }

            if (instance == null)
                instance = transientServices[type];

            if (instance == null)
                throw new NullReferenceException($"Service of type {instance.ServiceType} not found");

            return instance.ServiceType;
        }

        public void AddSingleton<T>()
        {
            var type = typeof(T);

            if (type.IsAbstract || type.IsInterface)
                throw new Exception("Cannot instantiate abstract classes");

            
            //For now just try to get parametless contructor...
            if(type.GetConstructor(Type.EmptyTypes) != null)
            {
                singletonServices.Add(type, new ServiceInfo
                {
                    ServiceImplementation = Activator.CreateInstance(type),
                    ServiceLifeTime = ServiceLifeTime.Singleton,
                    ServiceType = type
                });

                return;
            }

            var constructor = type.GetConstructors().First();
            var parametersTypes = constructor.GetParameters().Select(x => x.ParameterType);
            var dependencies = parametersTypes.Select(x => GetService(x)).ToArray();

            singletonServices.Add(type, new ServiceInfo
            {
                ServiceImplementation = Activator.CreateInstance(type, dependencies),
                ServiceLifeTime = ServiceLifeTime.Singleton,
                ServiceType = type
            });
        }

        public void AddTransient<TService, TImplementaion>() where TImplementaion : TService
        {
            var key = typeof(TService);
            var value = typeof(TImplementaion);
            transientServices.Add(key, new ServiceInfo
            {
                ServiceImplementation = value,
                ServiceLifeTime = ServiceLifeTime.Transient,
                ServiceType = key
            });
        }

        public Container BuildContainer()
        {
            return new Container(new Services(transientServices, singletonServices));
        }
    }
}
