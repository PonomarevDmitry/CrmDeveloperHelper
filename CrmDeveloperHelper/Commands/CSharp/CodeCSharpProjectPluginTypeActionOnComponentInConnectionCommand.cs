using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectPluginTypeActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOnComponent _actionOnComponent;

        private CodeCSharpProjectPluginTypeActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static CodeCSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CodeCSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CodeCSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenDependentComponentsInWebInConnection = new CodeCSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeOpenDependentInWebInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeCSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeOpenDependentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsListWithComponentInExplorerInConnection = new CodeCSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
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

                    var task = ExecuteActionOnPluginTypesAsync(helper, connectionData, pluginTypesNotCompiled, projectInfos, this._actionOnComponent);
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
