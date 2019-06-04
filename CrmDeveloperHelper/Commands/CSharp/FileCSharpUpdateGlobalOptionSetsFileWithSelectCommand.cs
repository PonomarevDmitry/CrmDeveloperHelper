using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand : AbstractCommand
    {
        private FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpUpdateGlobalOptionSetsFileWithSelectCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();

            helper.HandleUpdateGlobalOptionSetsFile(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand);
        }
    }
}