using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpPluginTypeExplorerCommand : AbstractSingleCommand
    {
        private CodeCSharpPluginTypeExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeCSharpPluginTypeExplorerCommandId)
        {
        }

        public static CodeCSharpPluginTypeExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpPluginTypeExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
                helper.ActivateOutputWindow(null);

                VSProject2Info.GetPluginTypes(new[] { document }, out var pluginTypesNotCompiled, out var projectInfos);

                var task = ExecuteAsync(helper, pluginTypesNotCompiled, projectInfos);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            try
            {
                string pluginType = await CSharpCodeHelper.GetSingleFileTypeFullNameAsync(pluginTypesNotCompiled, projectInfos);

                helper.HandleOpenPluginTypeExplorer(pluginType);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);
        }
    }
}