using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly SolutionComponent.Schema.OptionSets.rootcomponentbehavior _rootcomponentbehavior;

        private CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootcomponentbehavior)
            : base(commandService, baseIdStart)
        {
            this._rootcomponentbehavior = rootcomponentbehavior;
        }

        public static CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand InstanceIncludeSubcomponents { get; private set; }

        public static CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand InstanceDoNotIncludeSubcomponents { get; private set; }

        public static CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand InstanceIncludeAsShellOnly { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceIncludeSubcomponents = new CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityAddToSolutionInConnectionIncludeSubcomponentsCommandId
                , SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
            );

            InstanceDoNotIncludeSubcomponents = new CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityAddToSolutionInConnectionDoNotIncludeSubcomponentsCommandId
                , SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1
            );

            InstanceIncludeAsShellOnly = new CodeJavaScriptLinkedEntityAddToSolutionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityAddToSolutionInConnectionIncludeAsShellOnlyCommandId
                , SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedEntityName(out string entityName))
            {
                helper.HandleAddingEntityToSolutionCommand(connectionData, null, true, entityName, _rootcomponentbehavior);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedEntityName(applicationObject, menuCommand);
        }
    }
}