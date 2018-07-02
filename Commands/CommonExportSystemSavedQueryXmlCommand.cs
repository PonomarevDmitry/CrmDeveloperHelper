using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportSystemSavedQueryXmlCommand : AbstractCommand
    {
        private CommonExportSystemSavedQueryXmlCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportSystemSavedQueryXmlCommandId, ActionExecute, null) { }

        public static CommonExportSystemSavedQueryXmlCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportSystemSavedQueryXmlCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportSystemSavedQuery();
        }
    }
}
