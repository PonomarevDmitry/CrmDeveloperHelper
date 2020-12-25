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
    internal sealed class CSharpAddPluginStepInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpAddPluginStepInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpAddPluginStepInConnectionCommand InstanceCode { get; private set; }

        public static CSharpAddPluginStepInConnectionCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new CSharpAddPluginStepInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpAddPluginStepInConnectionCommandId, sourceCode);

            InstanceFile = new CSharpAddPluginStepInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpAddPluginStepInConnectionCommandId, sourceFile);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            try
            {
                var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).Take(2).ToList();

                if (selectedFiles.Count == 1)
                {
                    helper.HandleAddPluginStep(connectionData, selectedFiles.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(connectionData, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}