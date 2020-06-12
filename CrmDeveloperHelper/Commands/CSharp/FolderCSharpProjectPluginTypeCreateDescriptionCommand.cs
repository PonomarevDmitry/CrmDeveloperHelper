using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    class FolderCSharpProjectPluginTypeCreateDescriptionCommand : AbstractSingleCommand
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private FolderCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, idCommand, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        private FolderCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FolderCSharpProjectPluginTypeCreateDescriptionCommand InstanceEntityDescription { get; private set; }

        public static FolderCSharpProjectPluginTypeCreateDescriptionCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static FolderCSharpProjectPluginTypeCreateDescriptionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new FolderCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FolderCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new FolderCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FolderCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new FolderCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FolderCSharpProjectPluginTypeCreateDescriptionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            try
            {
                var listFiles = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, true).ToList();
                helper.ActivateOutputWindow(null);

                foreach (var projectItem in listFiles.OrderBy(d => d.FileNames[1]))
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, projectItem.FileNames[1]);
                }

                VSProject2Info.GetPluginTypes(listFiles, out var pluginTypesNotCompiled, out var projectInfos);

                var task = ExecuteActionOnPluginTypesAsync(helper, null, pluginTypesNotCompiled, projectInfos, this._actionOnComponent, this._fieldName, this._fieldTitle);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectRecursive(applicationObject, menuCommand);

            if (_actionOnComponent == ActionOnComponent.EntityDescription)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderCSharpProjectPluginTypeCreateEntityDescriptionCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.SingleXmlField)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.Description)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderCSharpProjectPluginTypeCreateDescriptionCommand);
            }
        }
    }
}
