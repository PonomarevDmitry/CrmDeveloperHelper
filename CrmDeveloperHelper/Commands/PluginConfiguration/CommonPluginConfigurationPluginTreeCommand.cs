﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonPluginConfigurationPluginTreeCommand : AbstractCommand
    {
        private CommonPluginConfigurationPluginTreeCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonPluginConfigurationPluginTreeCommandId, ActionExecute, null) { }

        public static CommonPluginConfigurationPluginTreeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonPluginConfigurationPluginTreeCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandlePluginConfigurationTree();
        }
    }
}
