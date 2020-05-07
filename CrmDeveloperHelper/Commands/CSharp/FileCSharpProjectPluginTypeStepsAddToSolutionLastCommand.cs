using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeStepsAddToSolutionLastCommandId)
        {
        }

        public static FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            System.Threading.Tasks.Task.WaitAll(ExecuteAsync(helper, solutionUniqueName));
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, string solutionUniqueName)
        {
            try
            {
                var listFiles = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();

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
                    helper.HandlePluginTypeAddingProcessingStepsByProjectCommand(null, solutionUniqueName, false, pluginTypeNames.ToArray());
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectAny(applicationObject, menuCommand);
        }
    }
}