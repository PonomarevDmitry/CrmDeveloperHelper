using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly JavaScriptObjectType _jsObjectType;

        private CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , JavaScriptObjectType jsObjectType
        ) : base(commandService, baseIdStart)
        {
            this._jsObjectType = jsObjectType;
        }

        public static CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionCommand InstanceTypeConstructor { get; private set; }

        public static CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionCommand InstanceJsonObject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceTypeConstructor = new CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionTypeConstructorCommandId
                , JavaScriptObjectType.TypeConstructor
            );

            InstanceJsonObject = new CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionCommand(
                commandService
               , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormCopyToClipboardTabsAndSectionsInConnectionJsonObjectCommandId
               , JavaScriptObjectType.JsonObject
           );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType))
            {
                helper.HandleSystemFormCopyToClipboardTabsAndSectionsCommand(connectionData, this._jsObjectType, entityName, formId, formType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}