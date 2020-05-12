using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId)
        {
        }

        public static CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService);
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

                    var task = ExecuteAsync(helper, connectionData, pluginTypesNotCompiled, projectInfos);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, ConnectionData connectionData, string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            try
            {
                string[] pluginTypeArray = await CSharpCodeHelper.GetTypeFullNameListAsync(pluginTypesNotCompiled, projectInfos);

                if (pluginTypeArray.Any())
                {
                    helper.HandlePluginTypeAddingProcessingStepsByProjectCommand(connectionData, null, true, pluginTypeArray);
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
