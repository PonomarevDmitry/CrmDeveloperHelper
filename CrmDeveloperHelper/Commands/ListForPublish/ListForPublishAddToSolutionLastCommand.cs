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
    internal sealed class ListForPublishAddToSolutionLastCommand : AbstractDynamicCommandAddObjectToSolutionLast
    {
        private ListForPublishAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.ListForPublishAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny
            )
        {

        }

        public static ListForPublishAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish().ToList();

            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish(connectionData);

                helper.HandleAddingWebResourcesToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
            }
            else
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                helper.ActivateOutputWindow(connectionData);
            }
        }
    }
}