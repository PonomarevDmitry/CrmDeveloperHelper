using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            try
            {
                var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

                helper.HandlePluginTypeAddingProcessingStepsByProjectCommand(connectionData, null, true, selectedFiles);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}