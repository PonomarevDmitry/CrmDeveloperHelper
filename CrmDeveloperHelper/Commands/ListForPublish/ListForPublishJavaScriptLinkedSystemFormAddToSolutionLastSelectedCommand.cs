using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private ListForPublishJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.ListForPublishJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommandId)
        {
        }

        public static ListForPublishJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var connectionConfig = ConnectionConfiguration.Get();

            var connectionData = connectionConfig.CurrentConnectionData;

            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish(FileOperations.SupportsJavaScriptType).ToList();

            if (selectedFiles.Any())
            {
                helper.ShowListForPublish(connectionData);

                var hashFormIds = new HashSet<Guid>();

                foreach (var selectedFile in selectedFiles)
                {
                    if (!File.Exists(selectedFile.FilePath))
                    {
                        continue;
                    }

                    string javaScriptCode = File.ReadAllText(selectedFile.FilePath);

                    if (CommonHandlers.GetLinkedSystemForm(javaScriptCode, out string entityName, out Guid formId, out int formType))
                    {
                        hashFormIds.Add(formId);
                    }
                }

                if (hashFormIds.Any())
                {
                    helper.HandleLinkedSystemFormAddingToSolutionCommand(null, solutionUniqueName, false, hashFormIds);
                }
            }
            else
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                helper.ActivateOutputWindow(connectionData);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceJavaScriptAny(applicationObject, menuCommand);
        }
    }
}