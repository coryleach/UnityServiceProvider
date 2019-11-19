namespace Gameframe.ServiceProvider
{
    public interface IServiceCollection
    {
        void AddSingleton<T>(T service) where T : class;
    }
}
