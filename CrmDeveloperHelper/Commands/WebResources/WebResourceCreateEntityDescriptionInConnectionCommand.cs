using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceCreateEntityDescriptionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceCreateEntityDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceCreateEntityDescriptionInConnectionCommand InstanceCode { get; private set; }

        public static WebResourceCreateEntityDescriptionInConnectionCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new WebResourceCreateEntityDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceCreateEntityDescriptionInConnectionCommandId
                , sourceCode
            );

            InstanceFile = new WebResourceCreateEntityDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceCreateEntityDescriptionInConnectionCommandId
                , sourceFile
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceCreateEntityDescriptionCommand(connectionData, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}