using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckComponentTypeEnumCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckComponentTypeEnumCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonCheckComponentTypeEnumCommandId
            )
        {

        }

        public static CommonCheckComponentTypeEnumCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckComponentTypeEnumCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckComponentTypeEnum(connectionData);
        }
    }
}
