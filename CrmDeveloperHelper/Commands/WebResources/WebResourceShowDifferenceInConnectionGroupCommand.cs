using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceShowDifferenceInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceShowDifferenceInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceShowDifferenceInConnectionGroupCommand InstanceCode { get; private set; }

        public static WebResourceShowDifferenceInConnectionGroupCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new WebResourceShowDifferenceInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceShowDifferenceInConnectionGroupCommandId
                , sourceCode
            );

            InstanceFile = new WebResourceShowDifferenceInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceShowDifferenceInConnectionGroupCommandId
                , sourceFile
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceDifferenceCommand(connectionData, selectedFiles.FirstOrDefault(), false);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
        }
    }
}