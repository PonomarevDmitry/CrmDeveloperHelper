using System;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit
{
    internal sealed class OutputFindEntityObjectsByPrefixInExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntityObjectsByPrefixInExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntityObjectsByPrefixInExplorerCommandId) { }

        public static OutputFindEntityObjectsByPrefixInExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsByPrefixInExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindEntityObjectsByPrefixInExplorer(connectionData);
        }
    }
}
