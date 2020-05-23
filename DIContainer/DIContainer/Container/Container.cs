using DIContainer.Container.Service;
using System;

namespace DIContainer.Container
{
    public class Container
    {
        private readonly Services _services;

        public Container(Services services)
        {
            _services = services;
        }   

        public T GetSingleton<T>()
        {
            var instance = _services.SingletonServices.Find(type => type.GetType() == typeof(T));

            if (instance == null)
                return default;

            return (T)instance;
        }

        public T GetTransient<T>()
        {
            var obj = _services.TransientServices[typeof(T)];

            if (obj == null)
                return default;

            var instance = Activator.CreateInstance(obj);

            return (T)instance;
        }
    }
}
