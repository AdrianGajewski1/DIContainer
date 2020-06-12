using System;
using System.Collections.Generic;

namespace DIContainer.Container.Service
{
    public class Services
    {
        public Services(Dictionary<object, ServiceInfo> transientServices, Dictionary<object, ServiceInfo> singletonServices)
        {
            TransientServices = transientServices;
            SingletonServices = singletonServices;
        }

        public Dictionary<object, ServiceInfo> TransientServices { get; private set; }
        public Dictionary<object, ServiceInfo> SingletonServices { get; private set; }
    }
}
