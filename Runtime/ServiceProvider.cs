using UnityEngine;

namespace Gameframe.ServiceProvider
{
    public static class ServiceProvider
    {
        private static IServiceProvider _current;
        public static IServiceProvider Current
        {
            get => _current ?? (_current = BasicServiceProvider.SharedInstance);
            set => _current = value;
        }
    }
}

