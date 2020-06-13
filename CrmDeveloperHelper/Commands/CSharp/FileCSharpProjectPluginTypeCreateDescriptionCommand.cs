using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginTypeCreateDescriptionCommand : AbstractSingleCommand
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private FileCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, idCommand, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        private FileCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FileCSharpProjectPluginTypeCreateDescriptionCommand InstanceEntityDescription { get; private set; }

        public static FileCSharpProjectPluginTypeCreateDescriptionCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static FileCSharpProjectPluginTypeCreateDescriptionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new FileCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new FileCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new FileCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeCreateDescriptionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            try
            {
                var listFiles = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();

                if (listFiles.Any())
                {
                    helper.HandleActionOnPluginTypesCommand(null, this._actionOnComponent, this._fieldName, this._fieldTitle, listFiles);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectAny(applicationObject, menuCommand);

            if (_actionOnComponent == ActionOnComponent.EntityDescription)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectPluginTypeCreateEntityDescriptionCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.SingleXmlField)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.Description)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectPluginTypeCreateDescriptionCommand);
            }
        }
    }
}
