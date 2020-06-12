using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, baseIdStart, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        private CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceEntityDescription { get; private set; }

        public static CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeCreateEntityDescriptionInConnectionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeGetCustomWorkflowActivityInfoInConnectionCommandId
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                if (document != null)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
                    helper.ActivateOutputWindow(null);

                    VSProject2Info.GetPluginTypes(new[] { document }, out var pluginTypesNotCompiled, out var projectInfos);

                    var task = ExecuteActionOnPluginTypesAsync(helper, connectionData, pluginTypesNotCompiled, projectInfos, this._actionOnComponent, this._fieldName, this._fieldTitle);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);
        }
    }
}
