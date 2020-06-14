using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceGetAttributeInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly string _fieldName;
        private readonly string _fieldTitle;

        private FileWebResourceGetAttributeInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, string fieldName, string fieldTitle)
            : base(commandService, baseIdStart)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        public static FileWebResourceGetAttributeInConnectionCommand InstanceContentJson { get; private set; }

        public static FileWebResourceGetAttributeInConnectionCommand InstanceDependencyXml { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceContentJson = new FileWebResourceGetAttributeInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceGetAttributeContentJsonInConnectionCommandId
                , WebResource.Schema.Attributes.contentjson
                , WebResource.Schema.Headers.contentjson
            );

            InstanceDependencyXml = new FileWebResourceGetAttributeInConnectionCommand(
               commandService
               , PackageIds.guidDynamicCommandSet.FileWebResourceGetAttributeDependencyXmlInConnectionCommandId
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
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceSingle(applicationObject, menuCommand);
        }
    }
}