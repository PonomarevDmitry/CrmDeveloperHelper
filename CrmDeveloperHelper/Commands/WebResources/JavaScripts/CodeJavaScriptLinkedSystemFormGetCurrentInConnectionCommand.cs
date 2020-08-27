using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName;
        private readonly string _fieldTitle;

        private CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , ActionOnComponent actionOnComponent
            , string fieldName
            , string fieldTitle
        ) : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        public static CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand InstanceEntityDescription { get; private set; }

        public static CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand InstanceFormDescription { get; private set; }

        public static CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand InstanceFormXml { get; private set; }

        public static CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand InstanceFormJson { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormGetCurrentEntityDescriptionInConnectionCommandId
                , ActionOnComponent.EntityDescription
                , string.Empty
                , string.Empty
            );

            InstanceFormDescription = new CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormGetCurrentFormDescriptionInConnectionCommandId
                , ActionOnComponent.Description
                , string.Empty
                , string.Empty
            );

            InstanceFormXml = new CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormGetCurrentFormXmlInConnectionCommandId
                , ActionOnComponent.SingleXmlField
                , SystemForm.Schema.Attributes.formxml
                , SystemForm.Schema.Headers.formxml
            );

            InstanceFormJson = new CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand(
                commandService
               , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormGetCurrentFormJsonInConnectionCommandId
               , ActionOnComponent.SingleField
               , SystemForm.Schema.Attributes.formjson
               , SystemForm.Schema.Headers.formjson
           );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType))
            {
                helper.HandleSystemFormGetCurrentAttributeCommand(connectionData, formId, _actionOnComponent, this._fieldName, this._fieldTitle);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}