using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, baseIdStart, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        private DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceEntityDescription { get; private set; }

        public static DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeCreateEntityDescriptionInConnectionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeGetCustomWorkflowActivityInfoInConnectionCommandId
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
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

                var task = ExecuteActionOnPluginTypesAsync(helper, connectionData, pluginTypesNotCompiled, projectInfos, this._actionOnComponent, this._fieldName, this._fieldTitle);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);
        }
    }
}
