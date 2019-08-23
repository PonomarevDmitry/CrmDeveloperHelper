using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportXsdSchemasCommand : AbstractDynamicCommandXsdSchemas
    {
        private CommonExportXsdSchemasCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.CommonExportXsdSchemasCommandId
            )
        {

        }

        public static CommonExportXsdSchemasCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonExportXsdSchemasCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, Tuple<string, string[]> schemas)
        {
            helper.HandleExportXsdSchema(schemas.Item2);
        }
    }
}
