using System.ComponentModel.Design;
using Chambersoft.DapperFactory.Handlers;
using Chambersoft.DapperFactory.Providers;
using Chambersoft.DapperFactory.Services;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Ninject;

namespace Chambersoft.DapperFactory.Extensions
{
    public static class NinjectExtensions
    {
        public static IKernel InjectServices(this IKernel kernel, IGenericServiceProvider sp, IGenericServiceProvider gsp)
        {
            // handlers
            kernel.Bind<IProErrorHandler>().To<ProErrorHandler>();

            // command services
            var oleMenuCommandService = sp.GetService<IMenuCommandService, OleMenuCommandService>();
            kernel.Bind<IMenuCommandService>().ToConstant(oleMenuCommandService);
            kernel.Bind<IOleCommandTarget>().ToConstant(oleMenuCommandService);
            kernel.Bind<IProCommandService>().To<ProCommandService>();

            // global services
            kernel.Bind<IVsMonitorSelection>().ToMethod(ctx => gsp.GetService<SVsShellMonitorSelection, IVsMonitorSelection>());
            kernel.Bind<IVsSolution>().ToMethod(ctx => gsp.GetService<SVsSolution, IVsSolution>());
            return kernel;
        }
    }
}
