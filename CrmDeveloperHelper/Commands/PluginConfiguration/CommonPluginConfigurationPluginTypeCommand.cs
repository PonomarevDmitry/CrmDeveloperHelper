﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonPluginConfigurationPluginTypeCommand : AbstractCommand
    {
        private CommonPluginConfigurationPluginTypeCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonPluginConfigurationPluginTypeCommandId, ActionExecute, null) { }

        public static CommonPluginConfigurationPluginTypeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonPluginConfigurationPluginTypeCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandlePluginConfigurationPluginTypeDescription();
        }
    }
}
