using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonSystemSavedQueryVisualizationExplorerCommand : AbstractCommand
    {
        private CommonSystemSavedQueryVisualizationExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportSystemSavedQueryVisualizationXmlCommandId, ActionExecute, null) { }

        public static CommonSystemSavedQueryVisualizationExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonSystemSavedQueryVisualizationExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportSystemSavedQueryVisualization();
        }
    }
}
