using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportSystemSavedQueryVisualizationXmlCommand : AbstractCommand
    {
        private CommonExportSystemSavedQueryVisualizationXmlCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportSystemSavedQueryVisualizationXmlCommandId, ActionExecute, null) { }

        public static CommonExportSystemSavedQueryVisualizationXmlCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportSystemSavedQueryVisualizationXmlCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportSystemSavedQueryVisualization();
        }
    }
}
