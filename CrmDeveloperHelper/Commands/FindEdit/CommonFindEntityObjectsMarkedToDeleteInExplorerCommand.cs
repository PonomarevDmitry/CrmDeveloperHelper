﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntityObjectsMarkedToDeleteInExplorerCommand : AbstractSingleCommand
    {
        private CommonFindEntityObjectsMarkedToDeleteInExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonFindEntityObjectsMarkedToDeleteInExplorerCommandId) { }

        public static CommonFindEntityObjectsMarkedToDeleteInExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsMarkedToDeleteInExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindMarkedToDeleteInExplorer();
        }
    }
}
