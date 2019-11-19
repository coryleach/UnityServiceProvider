namespace Gameframe.ServiceProvider
{
    public static class ServiceCollection
    {
        private static IServiceCollection _current;
        public static IServiceCollection Current
        {
            get => _current ?? (_current = BasicServiceProvider.SharedInstance);
            set => _current = value;
        }
    }
}
