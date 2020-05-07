using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpPluginTypeExplorerCommand : AbstractCommand
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
            System.Threading.Tasks.Task.WaitAll(ExecuteAsync(helper));
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
                helper.ActivateOutputWindow(null);

                string fileType = await PropertiesHelper.GetTypeFullNameAsync(document);

                helper.HandleOpenPluginTypeExplorer(fileType);
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