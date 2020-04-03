using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonXsdSchemaExportCommand : AbstractDynamicCommandXsdSchemas
    {
        private CommonXsdSchemaExportCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonXsdSchemaExportCommandId)
        {
        }

        public static CommonXsdSchemaExportCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonXsdSchemaExportCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, Tuple<string, string[]> schemas)
        {
            helper.HandleXsdSchemaExport(schemas.Item2);
        }
    }
}
