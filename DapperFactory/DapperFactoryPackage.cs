using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Chambersoft.DapperFactory.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Chambersoft.DapperFactory
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidDapperFactoryPkgString)]
    public sealed class DapperFactoryPackage : ProPackage
    {
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            ProCommandService.SetContext(GuidList.guidDapperFactoryCmdSet)
                .RegisterCommand(PkgCmdIDList.dfGenerateServiceInterfaces,
                    (menuCommand, e) => { })
                .RegisterCommand(PkgCmdIDList.dfGenerateDapperServiceClass,
                    (menuCommand, e) => { },
                    MenuInterfaceBeforeQueryStatus);
        }

        private void MenuInterfaceBeforeQueryStatus(OleMenuCommand menuCommand, EventArgs e)
        {
            // start by assuming that the menu will not be shown
            menuCommand.Visible = false;
            menuCommand.Enabled = false;

            IVsHierarchy hierarchy;
            uint itemid;
            if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return;
            // Get the file path
            string itemFullPath = null;
            ((IVsProject)hierarchy).GetMkDocument(itemid, out itemFullPath);
            var fileInfo = new FileInfo(itemFullPath);

            // then check if the file is named 'web.config'
            bool isInterface = fileInfo.Name.StartsWith("I") && fileInfo.Name.EndsWith(".cs") &&
                               Char.IsUpper(fileInfo.Name, 1);

            // if not leave the menu hidden
            if (!isInterface) return;

            menuCommand.Visible = true;
            menuCommand.Enabled = true;
        }


        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            // Show a Message Box to prove we were here
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            int result;
            ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                       0,
                       ref clsid,
                       "DapperFactory",
                       string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this),
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_INFO,
                       0,        // false
                       out result));
        }

    }
}
