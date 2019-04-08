﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenSolutionDifferenceImageCommand : AbstractCommand
    {
        private CommonOpenSolutionDifferenceImageCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonOpenSolutionDifferenceImageCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonOpenSolutionDifferenceImageCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonOpenSolutionDifferenceImageCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenSolutionDifferenceImageWindow();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonOpenSolutionDifferenceImageCommand);
        }
    }
}
