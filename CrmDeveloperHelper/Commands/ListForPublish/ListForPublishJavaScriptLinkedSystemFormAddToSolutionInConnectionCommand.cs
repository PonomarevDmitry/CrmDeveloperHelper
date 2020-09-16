using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishJavaScriptLinkedSystemFormAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private ListForPublishJavaScriptLinkedSystemFormAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.ListForPublishJavaScriptLinkedSystemFormAddToSolutionInConnectionCommandId)
        {
        }

        public static ListForPublishJavaScriptLinkedSystemFormAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishJavaScriptLinkedSystemFormAddToSolutionInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish(FileOperations.SupportsJavaScriptType).ToList();

            if (selectedFiles.Count > 0)
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
                    helper.HandleLinkedSystemFormAddingToSolutionCommand(connectionData, null, true, hashFormIds);
                }
            }
            else
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                helper.ActivateOutputWindow(connectionData);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceJavaScriptAny(applicationObject, menuCommand);
        }
    }
}
