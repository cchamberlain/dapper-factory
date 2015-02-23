using Ninject;
using Ninject.Modules;
using Ninject.Parameters;

namespace Chambersoft.DapperFactory.Dependencies
{
    public static class Injection
    {
        private static IKernel _kernel;

        public static IKernel CreateKernel(params INinjectModule[] ninjectModules)
        {
            _kernel = new StandardKernel(ninjectModules);
            return _kernel;
        }

        public static T Get<T>(params IParameter[] parameters)
        {
            IParameter[] param = parameters;
            return _kernel.Get<T>(param);
        }
    }
}
