using System;

namespace Gameframe.ServiceProvider
{
    /// <summary>
    /// TODO: Implement AddTransient
    /// TODO: Implement AddScoped
    /// </summary>
    public interface IServiceCollection
    {
        void AddSingleton<TService>(TService service) where TService : class;
        void AddSingleton<TService>(Func<IServiceProvider,TService> factory) where TService : class;
        void AddSingleton<TService, TImplementation>(TImplementation service) where TImplementation : TService where TService : class;

        void AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> factory) where TImplementation : TService where TService : class;
        void AddTransient<TService>(Func<IServiceProvider, TService> factory) where TService : class;

    }
}
