using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenDefaultSolutionInWebCommand : AbstractOutputWindowCommand
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

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            connectionData.OpenSolutionInWeb(Solution.Schema.InstancesUniqueId.DefaultId);
        }
    }
}