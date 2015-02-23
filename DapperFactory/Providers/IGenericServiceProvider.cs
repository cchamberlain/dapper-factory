namespace Chambersoft.DapperFactory.Providers
{
    public interface IGenericServiceProvider
    {
        T GetService<T>();
        TReturn GetService<T, TReturn>();
    }
}