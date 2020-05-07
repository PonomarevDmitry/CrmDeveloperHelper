using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommandId)
        {
        }

        public static CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            System.Threading.Tasks.Task.WaitAll(ExecuteAsync(helper, solutionUniqueName));
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, string solutionUniqueName)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
                helper.ActivateOutputWindow(null);

                string fileType = await PropertiesHelper.GetTypeFullNameAsync(document);

                helper.HandlePluginTypeAddingProcessingStepsByProjectCommand(null, solutionUniqueName, false, fileType);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);
        }
    }
}