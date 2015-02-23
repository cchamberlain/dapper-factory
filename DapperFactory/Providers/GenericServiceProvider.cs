using System;

namespace Chambersoft.DapperFactory.Providers
{
    public class GenericServiceProvider : IGenericServiceProvider
    {
        private readonly Func<Type, object> _serviceProvider;

        public GenericServiceProvider(Func<Type, object> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>() 
        {
            return GetService<T, T>();
        }

        public TReturn GetService<T, TReturn>()
        {
            return (TReturn)_serviceProvider(typeof(T));
        }
    }
}