using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId
            )
        {

        }

        public static FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService);
        }

        protected override async void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var listFiles = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, true).ToList();

            var pluginTypeNames = new List<string>();
            var handledFilePaths = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            helper.ActivateOutputWindow(null);

            foreach (var item in listFiles)
            {
                string filePath = item.FileNames[1];

                if (handledFilePaths.Add(filePath))
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, filePath);
                    var typeName = await PropertiesHelper.GetTypeFullNameAsync(item);

                    if (!string.IsNullOrEmpty(typeName))
                    {
                        pluginTypeNames.Add(typeName);
                    }
                }
            }

            if (pluginTypeNames.Any())
            {
                helper.HandleAddingPluginTypeProcessingStepsByProjectCommand(connectionData, null, true, pluginTypeNames.ToArray());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(applicationObject, menuCommand, true);
        }
    }
}
