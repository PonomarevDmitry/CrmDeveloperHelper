using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeJavaScriptLinkedEntityAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly SolutionComponent.Schema.OptionSets.rootcomponentbehavior _rootcomponentbehavior;

        private CodeJavaScriptLinkedEntityAddToSolutionLastCommand(OleMenuCommandService commandService, int baseIdStart, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootcomponentbehavior)
            : base(commandService, baseIdStart)
        {
            this._rootcomponentbehavior = rootcomponentbehavior;
        }

        public static CodeJavaScriptLinkedEntityAddToSolutionLastCommand InstanceIncludeSubcomponents { get; private set; }

        public static CodeJavaScriptLinkedEntityAddToSolutionLastCommand InstanceDoNotIncludeSubcomponents { get; private set; }

        public static CodeJavaScriptLinkedEntityAddToSolutionLastCommand InstanceIncludeAsShellOnly { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceIncludeSubcomponents = new CodeJavaScriptLinkedEntityAddToSolutionLastCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityAddToSolutionLastIncludeSubcomponentsCommandId
                , SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
            );

            InstanceDoNotIncludeSubcomponents = new CodeJavaScriptLinkedEntityAddToSolutionLastCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityAddToSolutionLastDoNotIncludeSubcomponentsCommandId
                , SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1
            );

            InstanceIncludeAsShellOnly = new CodeJavaScriptLinkedEntityAddToSolutionLastCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityAddToSolutionLastIncludeAsShellOnlyCommandId
                , SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2
            );
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            if (helper.TryGetLinkedEntityName(out string entityName))
            {
                helper.HandleAddingEntityToSolutionCommand(null, solutionUniqueName, false, entityName, _rootcomponentbehavior);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedEntityName(applicationObject, menuCommand);
        }
    }
}