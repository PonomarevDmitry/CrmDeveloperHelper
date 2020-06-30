using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptUpdateGlobalOptionSetAllFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptUpdateGlobalOptionSetAllFileCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptUpdateGlobalOptionSetAllFileCommand InstanceCode { get; private set; }

        public static JavaScriptUpdateGlobalOptionSetAllFileCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new JavaScriptUpdateGlobalOptionSetAllFileCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptUpdateGlobalOptionSetAllFileCommandId
                , sourceCode
            );

            InstanceFile = new JavaScriptUpdateGlobalOptionSetAllFileCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileJavaScriptUpdateGlobalOptionSetAllFileCommandId
                , sourceFile
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleJavaScriptUpdateGlobalOptionSetFileAll(connectionData, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);
        }
    }
}
