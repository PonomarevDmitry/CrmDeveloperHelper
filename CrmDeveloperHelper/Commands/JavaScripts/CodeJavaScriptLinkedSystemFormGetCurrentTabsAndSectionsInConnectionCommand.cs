using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedSystemFormGetCurrentTabsAndSectionsInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly JavaScriptObjectType _jsObjectType;

        private CodeJavaScriptLinkedSystemFormGetCurrentTabsAndSectionsInConnectionCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , JavaScriptObjectType jsObjectType
        ) : base(commandService, baseIdStart)
        {
            this._jsObjectType = jsObjectType;
        }

        public static CodeJavaScriptLinkedSystemFormGetCurrentTabsAndSectionsInConnectionCommand InstanceTypeConstructor { get; private set; }

        public static CodeJavaScriptLinkedSystemFormGetCurrentTabsAndSectionsInConnectionCommand InstanceJsonObject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceTypeConstructor = new CodeJavaScriptLinkedSystemFormGetCurrentTabsAndSectionsInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormGetCurrentTabsAndSectionsInConnectionTypeConstructorCommandId
                , JavaScriptObjectType.TypeConstructor
            );

            InstanceJsonObject = new CodeJavaScriptLinkedSystemFormGetCurrentTabsAndSectionsInConnectionCommand(
                commandService
               , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormGetCurrentTabsAndSectionsInConnectionJsonObjectCommandId
               , JavaScriptObjectType.JsonObject
           );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType))
            {
                helper.HandleSystemFormGetCurrentTabsAndSectionsCommand(connectionData, this._jsObjectType, entityName, formId, formType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}