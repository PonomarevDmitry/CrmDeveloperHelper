﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonApplicationRibbonExplorerCommand : AbstractCommand
    {
        private CommonApplicationRibbonExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportApplicationRibbonXmlCommandId, ActionExecute, null) { }

        public static CommonApplicationRibbonExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonApplicationRibbonExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportRibbon();
        }
    }
}
