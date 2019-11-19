namespace Gameframe.ServiceProvider
{
    /// <summary>
    /// TODO: Implement AddTransient
    /// TODO: Implement AddScoped
    /// </summary>
    public interface IServiceCollection
    {
        void AddSingleton<T>(T service) where T : class;
    }
}
