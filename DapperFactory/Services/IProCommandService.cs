using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace Chambersoft.DapperFactory.Services
{
    public interface IProCommandService : IMenuCommandService, IOleCommandTarget
    {
        /// <summary>
        /// Chainable method to set the current command set that the IProCommandService is operating on.
        /// </summary>
        /// <param name="commandSetId">Sets the command set ID for future commands.</param>
        /// <returns>Chainable instance of IProCommandService.</returns>
        IProCommandService SetContext(Guid commandSetId);

        /// <summary>
        /// Chainable method to act as helper for registering menu items and their callbacks.
        /// </summary>
        /// <param name="commandId">The int id of the command that is being registered (from PkgCmdIDList).</param>
        /// <param name="invokeAction">The callback to be invoked when the command is triggered.</param>
        /// <param name="beforeQueryStatusAction">Optionally registers a callback to be invoked priory to query status being pulled (hide / show here).</param>
        /// <returns>A chainable instance of the IProCommandService.</returns>
        IProCommandService RegisterCommand(uint commandId, Action<OleMenuCommand, EventArgs> invokeAction, Action<OleMenuCommand, EventArgs> beforeQueryStatusAction = null);
    }
}