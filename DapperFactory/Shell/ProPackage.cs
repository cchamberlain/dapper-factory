using System;
using System.Runtime.InteropServices;
using Chambersoft.DapperFactory.Dependencies;
using Chambersoft.DapperFactory.Extensions;
using Chambersoft.DapperFactory.Handlers;
using Chambersoft.DapperFactory.Providers;
using Chambersoft.DapperFactory.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Chambersoft.DapperFactory.Shell
{
    public abstract class ProPackage : Package
    {
        protected IProErrorHandler ProErrorHandler;
        protected IProCommandService ProCommandService;

        protected ProPackage()
        {
            Injection.CreateKernel().InjectServices(new GenericServiceProvider(GetService), new GenericServiceProvider(GetGlobalService));
        }

        protected override void Initialize()
        {
            base.Initialize();
            ProErrorHandler = Injection.Get<IProErrorHandler>();
            ProCommandService = Injection.Get<IProCommandService>();
        }

        public bool IsSingleProjectItemSelection(out IVsHierarchy hierarchy, out uint itemid)
        {
            var vsMonitorSelection = Injection.Get<IVsMonitorSelection>();
            var vsSolution = Injection.Get<IVsSolution>();

            hierarchy = null;
            itemid = VSConstants.VSITEMID_NIL;

            if (vsMonitorSelection == null || vsSolution == null)
            {
                return false;
            }

            IntPtr hierarchyPtr = IntPtr.Zero;
            IntPtr selectionContainerPtr = IntPtr.Zero;

            try
            {
                IVsMultiItemSelect multiItemSelect;
                var hr = vsMonitorSelection.GetCurrentSelection(out hierarchyPtr, out itemid, out multiItemSelect, out selectionContainerPtr);

                if (IsNothingSelected(itemid, hr, hierarchyPtr))
                    return false;

                // multiple items are selected
                if (multiItemSelect != null) return false;

                // there is a hierarchy root node selected, thus it is not a single item inside a project
                if (IsHierarchyRootSelected(itemid)) return false;

                hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;
                if (hierarchy == null) return false;

                Guid guidProjectId;
                return !ProErrorHandler.Failed(vsSolution.GetGuidOfProject(hierarchy, out guidProjectId));
            }
            finally
            {
                if (selectionContainerPtr != IntPtr.Zero)
                {
                    Marshal.Release(selectionContainerPtr);
                }

                if (hierarchyPtr != IntPtr.Zero)
                {
                    Marshal.Release(hierarchyPtr);
                }
            }
        }

        private bool IsHierarchyRootSelected(uint itemId)
        {
            return itemId == VSConstants.VSITEMID_ROOT;
        }

        private bool IsNothingSelected(uint itemid, int hr, IntPtr hierarchyPtr)
        {
            return ProErrorHandler.Failed(hr) || hierarchyPtr == IntPtr.Zero || itemid == VSConstants.VSITEMID_NIL;
        }
    }
}
