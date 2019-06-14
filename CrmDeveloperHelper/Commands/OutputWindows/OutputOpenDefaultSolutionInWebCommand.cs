using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenDefaultSolutionInWebCommand : AbstractCommand
    {
        private OutputOpenDefaultSolutionInWebCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputOpenDefaultSolutionInWebCommandId
            )
        {

        }

        public static OutputOpenDefaultSolutionInWebCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenDefaultSolutionInWebCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            connectionData.OpenSolutionInWeb(Solution.Schema.InstancesUniqueId.DefaultId);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}