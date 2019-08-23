using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckUnknownFormControlTypes : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckUnknownFormControlTypes(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.CommonCheckUnknownFormControlTypesId
            )
        {

        }

        public static CommonCheckUnknownFormControlTypes Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckUnknownFormControlTypes(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckUnknownFormControlTypes(connectionData);
        }
    }
}
