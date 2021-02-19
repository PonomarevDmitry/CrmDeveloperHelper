using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedEntityAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly SolutionComponent.Schema.OptionSets.rootcomponentbehavior _rootcomponentbehavior;

        private CodeJavaScriptLinkedEntityAddToSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootcomponentbehavior)
            : base(commandService, baseIdStart)
        {
            this._rootcomponentbehavior = rootcomponentbehavior;
        }

        public static CodeJavaScriptLinkedEntityAddToSolutionLastSelectedCommand InstanceIncludeSubcomponents { get; private set; }

        public static CodeJavaScriptLinkedEntityAddToSolutionLastSelectedCommand InstanceDoNotIncludeSubcomponents { get; private set; }

        public static CodeJavaScriptLinkedEntityAddToSolutionLastSelectedCommand InstanceIncludeAsShellOnly { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceIncludeSubcomponents = new CodeJavaScriptLinkedEntityAddToSolutionLastSelectedCommand(
                commandService
                , PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeJavaScriptLinkedEntityAddToSolutionLastIncludeSubcomponentsCommandId
                , SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0
            );

            InstanceDoNotIncludeSubcomponents = new CodeJavaScriptLinkedEntityAddToSolutionLastSelectedCommand(
                commandService
                , PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeJavaScriptLinkedEntityAddToSolutionLastDoNotIncludeSubcomponentsCommandId
                , SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1
            );

            InstanceIncludeAsShellOnly = new CodeJavaScriptLinkedEntityAddToSolutionLastSelectedCommand(
                commandService
                , PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeJavaScriptLinkedEntityAddToSolutionLastIncludeAsShellOnlyCommandId
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