using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpProjectPluginTypeCreateDescriptionCommand : AbstractCommand
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private DocumentsCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, idCommand, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        private DocumentsCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static DocumentsCSharpProjectPluginTypeCreateDescriptionCommand InstanceEntityDescription { get; private set; }

        public static DocumentsCSharpProjectPluginTypeCreateDescriptionCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static DocumentsCSharpProjectPluginTypeCreateDescriptionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new DocumentsCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new DocumentsCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new DocumentsCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpProjectPluginTypeCreateDescriptionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            try
            {
                var listFiles = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsCSharpType).ToList();
                helper.ActivateOutputWindow(null);

                foreach (var document in listFiles.OrderBy(d => d.FullName))
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document.FullName);
                }

                VSProject2Info.GetPluginTypes(listFiles, out var pluginTypesNotCompiled, out var projectInfos);

                var task = ExecuteAsync(helper, pluginTypesNotCompiled, projectInfos);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            try
            {
                string[] pluginTypeArray = await CSharpCodeHelper.GetTypeFullNameListAsync(pluginTypesNotCompiled, projectInfos);

                if (pluginTypeArray.Any())
                {
                    helper.HandleActionOnPluginTypesCommand(null, this._actionOnComponent, this._fieldName, this._fieldTitle, pluginTypeArray);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);

            if (_actionOnComponent == ActionOnComponent.EntityDescription)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.DocumentsCSharpProjectPluginTypeCreateEntityDescriptionCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.SingleXmlField)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.DocumentsCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.Description)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.DocumentsCSharpProjectPluginTypeCreateDescriptionCommand);
            }
        }
    }
}
