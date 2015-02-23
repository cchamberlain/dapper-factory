using System;
using System.Runtime.InteropServices;
using Chambersoft.DapperFactory.Dependencies;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Chambersoft.DapperFactory.Extensions
{
    public static class PackageExtensions
    {
        public static bool IsSingleProjectItemSelection(out IVsHierarchy hierarchy, out uint itemid)
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

                if (ErrorHandler.Failed(hr) || hierarchyPtr == IntPtr.Zero || itemid == VSConstants.VSITEMID_NIL)
                {
                    // there is no selection
                    return false;
                }

                // multiple items are selected
                if (multiItemSelect != null) return false;

                // there is a hierarchy root node selected, thus it is not a single item inside a project
                if (itemid == VSConstants.VSITEMID_ROOT) return false;

                hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;
                if (hierarchy == null) return false;

                Guid guidProjectId;
                return !ErrorHandler.Failed(vsSolution.GetGuidOfProject(hierarchy, out guidProjectId));
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

    }
}
