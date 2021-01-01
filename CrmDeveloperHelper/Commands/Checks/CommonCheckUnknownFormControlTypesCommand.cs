using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckUnknownFormControlTypesCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckUnknownFormControlTypesCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonCheckUnknownFormControlTypesCommandId)
        {
        }

        public static CommonCheckUnknownFormControlTypesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckUnknownFormControlTypesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckUnknownFormControlTypes(connectionData);
        }
    }
}