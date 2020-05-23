using System;
using System.Collections.Generic;

namespace DIContainer.Container.Service
{
    public class Services
    {
        public Services(Dictionary<Type, Type> transientServices, List<object> singletonServices)
        {
            TransientServices = transientServices;
            SingletonServices = singletonServices;
        }

        public Dictionary<Type, Type> TransientServices { get; private set; }
        public List<object> SingletonServices { get; private set; }
    }
}
