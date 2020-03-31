using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeStepsAddToSolutionLastCommandId
            )
        {

        }

        public static FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService);
        }

        protected override async void CommandAction(DTEHelper helper, string solutionUniqueName)
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

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectAny(applicationObject, menuCommand);
        }
    }
}