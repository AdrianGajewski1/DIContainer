using DIContainer.Container.Service;
using System;
using System.Linq;

namespace DIContainer.Container
{
    public class Container
    {
        private readonly Services _services;

        public Container(Services services)
        {
            _services = services;
        }
        private object GetService(object type)
        {
            ServiceInfo instance;

            instance = _services.SingletonServices[type];

            if (instance == null)
                instance = _services.TransientServices[type];

            if (instance == null)
                throw new NullReferenceException($"Service of type {instance.ServiceType} not found");

            return instance.ServiceImplementation;
        }

        public T GetSingleton<T>()
        {
            var service = _services.SingletonServices[typeof(T)];

            return (T)service.ServiceImplementation;
        }

        public T GetTransient<T>()
        {
            var obj = _services.TransientServices[typeof(T)];

            if (obj == null)
                return default;

            var contructor = (Type)obj.ServiceType;
            var consructorParams = contructor.GetConstructors().First().GetParameters().Select(x => GetService(x));

            var instance = Activator.CreateInstance(obj.ServiceImplementation as Type);

            return (T)instance;
        }
    }
}
