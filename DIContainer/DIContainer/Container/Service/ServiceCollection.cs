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


        private object GetService<T>()
        {
            var service = GetService(typeof(T));

            return service;
         
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

            return instance.ServiceImplementation;
        }

        public void AddSingleton<T>()
        {
            var type = typeof(T);

            if (type.IsAbstract || type.IsInterface)
                throw new Exception("Cannot instantiate abstract classes");

            var constructor = type.GetConstructors().First();

            if (constructor != null)
            {
                
                var constructorParams = constructor.GetParameters();

                if(constructorParams != null)
                {
                    List<object> serviceParams = new List<object>();

                    foreach (var item in constructorParams)
                    {
                        serviceParams.Add(GetService(item.ParameterType));
                    }


                    singletonServices.Add(type, new ServiceInfo
                    {
                        ServiceImplementation = Activator.CreateInstance(type, serviceParams),
                        ServiceType = type,
                        ServiceLifeTime = ServiceLifeTime.Singleton
                    });

                    return;
                }
                
            }

            singletonServices.Add(type, new ServiceInfo
            {
                ServiceImplementation = Activator.CreateInstance(type),
                ServiceType = type,
                ServiceLifeTime = ServiceLifeTime.Singleton
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
