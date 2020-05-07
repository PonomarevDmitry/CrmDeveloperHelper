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
            : base(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId)
        {
        }

        public static FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            System.Threading.Tasks.Task.WaitAll(ExecuteAsync(helper, connectionData));
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, ConnectionData connectionData)
        {
            try
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
                    helper.HandlePluginTypeAddingProcessingStepsByProjectCommand(connectionData, null, true, pluginTypeNames.ToArray());
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectRecursive(applicationObject, menuCommand);
        }
    }
}
