using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenDefaultSolutionInWebCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonOpenDefaultSolutionInWebCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonOpenDefaultSolutionInWebCommandId
            )
        {

        }

        public static CommonOpenDefaultSolutionInWebCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenDefaultSolutionInWebCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData)
        {
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            connectionData.OpenSolutionInWeb(Solution.Schema.InstancesUniqueId.DefaultId);
        }
    }
}
