using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportPluginTypeDescriptionCommand : AbstractCommand
    {
        private CommonExportPluginTypeDescriptionCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportPluginTypeDescriptionCommandId, ActionExecute, null) { }

        public static CommonExportPluginTypeDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportPluginTypeDescriptionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            string selection = helper.GetSelectedText();

            helper.HandleExportPluginTypeDescription(selection);
        }
    }
}
