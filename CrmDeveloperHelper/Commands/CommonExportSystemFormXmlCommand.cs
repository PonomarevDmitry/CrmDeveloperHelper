using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportSystemFormXmlCommand : AbstractCommand
    {
        private CommonExportSystemFormXmlCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportSystemFormXmlCommandId, ActionExecute, null) { }

        public static CommonExportSystemFormXmlCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportSystemFormXmlCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportSystemForm();
        }
    }
}
