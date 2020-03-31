using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private ListForPublishAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.ListForPublishAddToSolutionLastCommandId
            )
        {

        }

        public static ListForPublishAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishAddToSolutionLastCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData != null)
            {
                List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish().ToList();

                if (selectedFiles.Count > 0)
                {
                    helper.ShowListForPublish(connectionConfig.CurrentConnectionData);

                    helper.HandleWebResourceAddingToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
                }
                else
                {
                    helper.WriteToOutput(connectionConfig.CurrentConnectionData, Properties.OutputStrings.PublishListIsEmpty);
                    helper.ActivateOutputWindow(connectionConfig.CurrentConnectionData);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}