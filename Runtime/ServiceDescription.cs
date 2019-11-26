using System;

namespace Gameframe.ServiceProvider
{
    internal class ServiceDescription
    {
        public ServiceType serviceType = ServiceType.Singleton;
        public Func<IServiceProvider, object> factory = null;
        public object service = null;

        public object GetService(IServiceProvider serviceProvider)
        {
            switch (serviceType)
            {
                case ServiceType.Singleton:
                    //Return the service if we have one created. Otherwise create and hold onto an instance
                    return service ?? (service = factory.Invoke(serviceProvider));
                default:
                    return factory.Invoke(serviceProvider);
            }
        }
    }
}