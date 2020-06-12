using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectPluginTypeCreateDescriptionCommand : AbstractSingleCommand
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private CodeCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, idCommand, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        private CodeCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static CodeCSharpProjectPluginTypeCreateDescriptionCommand InstanceEntityDescription { get; private set; }

        public static CodeCSharpProjectPluginTypeCreateDescriptionCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static CodeCSharpProjectPluginTypeCreateDescriptionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new CodeCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new CodeCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new CodeCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginTypeCreateDescriptionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                if (document != null)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
                    helper.ActivateOutputWindow(null);

                    VSProject2Info.GetPluginTypes(new[] { document }, out var pluginTypesNotCompiled, out var projectInfos);

                    var task = ExecuteActionOnPluginTypesAsync(helper, null, pluginTypesNotCompiled, projectInfos, this._actionOnComponent, this._fieldName, this._fieldTitle);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);

            if (_actionOnComponent == ActionOnComponent.EntityDescription)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeCSharpProjectPluginTypeCreateEntityDescriptionCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.SingleXmlField)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.Description)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeCSharpProjectPluginTypeCreateDescriptionCommand);
            }
        }
    }
}
