using System.Collections.Generic;

namespace Gameframe.ServiceProvider
{
    public interface IServiceProvider : System.IServiceProvider
    { 
        T Get<T>() where T : class;
        void GetAll<T>(IList<T> list) where T : class;
    }
}
