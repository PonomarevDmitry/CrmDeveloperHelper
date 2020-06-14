using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceGetAttributeInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly string _fieldName;
        private readonly string _fieldTitle;

        private CodeWebResourceGetAttributeInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, string fieldName, string fieldTitle)
            : base(commandService, baseIdStart)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        public static CodeWebResourceGetAttributeInConnectionCommand InstanceContentJson { get; private set; }

        public static CodeWebResourceGetAttributeInConnectionCommand InstanceDependencyXml { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceContentJson = new CodeWebResourceGetAttributeInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceGetAttributeContentJsonInConnectionCommandId
                , WebResource.Schema.Attributes.contentjson
                , WebResource.Schema.Headers.contentjson
            );

            InstanceDependencyXml = new CodeWebResourceGetAttributeInConnectionCommand(
               commandService
               , PackageIds.guidDynamicCommandSet.CodeWebResourceGetAttributeDependencyXmlInConnectionCommandId
               , WebResource.Schema.Attributes.dependencyxml
               , WebResource.Schema.Headers.dependencyxml
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleWebResourceGetAttributeCommand(connectionData, this._fieldName, this._fieldTitle);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResourceText(applicationObject, menuCommand);
        }
    }
}