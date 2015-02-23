using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace Chambersoft.DapperFactory.Services
{
    /// <summary>
    /// Replacement for OleMenuCommandService meant to cut down on boilerplate code and make this API more usable.  Shame on you Microsoft...
    /// </summary>
    public sealed class ProCommandService : IProCommandService
    {
        private readonly IMenuCommandService _menuCommandService;
        private readonly IOleCommandTarget _oleCommandTarget;
        private Guid _commandSetId;

        public ProCommandService(IMenuCommandService menuCommandService, IOleCommandTarget oleCommandTarget)
        {
            _menuCommandService = menuCommandService;
            _oleCommandTarget = oleCommandTarget;
        }

        /// <summary>
        /// Chainable method to set the current command set that the IProCommandService is operating on.
        /// </summary>
        /// <param name="commandSetId">Sets the command set ID for future commands.</param>
        /// <returns>Chainable instance of IProCommandService.</returns>
        public IProCommandService SetContext(Guid commandSetId)
        {
            _commandSetId = commandSetId;
            return this;
        }

        /// <summary>
        /// Chainable method to act as helper for registering menu items and their callbacks.
        /// </summary>
        /// <param name="commandId">The int id of the command that is being registered (from PkgCmdIDList).</param>
        /// <param name="invokeAction">The callback to be invoked when the command is triggered.</param>
        /// <param name="beforeQueryStatusAction">Optionally registers a callback to be invoked priory to query status being pulled (hide / show here).</param>
        /// <returns>A chainable instance of the IProCommandService.</returns>
        public IProCommandService RegisterCommand(uint commandId, Action<OleMenuCommand, EventArgs> invokeAction, Action<OleMenuCommand, EventArgs> beforeQueryStatusAction = null)
        {
            var menuItem = CreateMenuCommand((int)commandId, invokeAction);
            if (beforeQueryStatusAction != null)
            {
                menuItem.BeforeQueryStatus += (sender, args) =>
                {
                    var menuCommand = sender as OleMenuCommand;
                    if (menuCommand != null)
                        beforeQueryStatusAction(menuCommand, args);
                };
            }
            _menuCommandService.AddCommand(menuItem);
            return this;
        }

        private OleMenuCommand CreateMenuCommand(int commandId, Action<OleMenuCommand, EventArgs> invokeAction)
        {
            return new OleMenuCommand((o, e) => invokeAction(o as OleMenuCommand, e), CreateCommandId(commandId));
        }

        private CommandID CreateCommandId(int commandId)
        {
            return new CommandID(_commandSetId, commandId);
        }

        public void AddCommand(MenuCommand command)
        {
            _menuCommandService.AddCommand(command);
        }

        public void AddVerb(DesignerVerb verb)
        {
            _menuCommandService.AddVerb(verb);
        }

        public MenuCommand FindCommand(CommandID commandId)
        {
            return _menuCommandService.FindCommand(commandId);
        }

        public bool GlobalInvoke(CommandID commandId)
        {
            return _menuCommandService.GlobalInvoke(commandId);
        }

        public void RemoveCommand(MenuCommand command)
        {
            _menuCommandService.RemoveCommand(command);
        }

        public void RemoveVerb(DesignerVerb verb)
        {
            _menuCommandService.RemoveVerb(verb);
        }

        public void ShowContextMenu(CommandID menuId, int x, int y)
        {
            _menuCommandService.ShowContextMenu(menuId, x, y);
        }

        public DesignerVerbCollection Verbs
        {
            get { return _menuCommandService.Verbs; }
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return _oleCommandTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdId, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            return _oleCommandTarget.Exec(ref pguidCmdGroup, nCmdId, nCmdexecopt, pvaIn, pvaOut);
        }
    }
}
