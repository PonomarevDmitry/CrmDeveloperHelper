using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand InstanceDocuments { get; private set; }

        public static JavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand InstanceFile { get; private set; }

        public static JavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new JavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommandId, sourceDocuments);

            InstanceFile = new JavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommandId, sourceFile);

            InstanceFolder = new JavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FolderJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm).ToList();

            var hashFormIds = new HashSet<Guid>();

            foreach (var selectedFile in selectedFiles)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    continue;
                }

                string javaScriptCode = File.ReadAllText(selectedFile.FilePath);

                if (CommonHandlers.GetLinkedSystemForm(javaScriptCode, out string entityName, out Guid formId, out int formType))
                {
                    hashFormIds.Add(formId);
                }
            }

            if (hashFormIds.Any())
            {
                helper.HandleLinkedSystemFormAddingToSolutionCommand(null, solutionUniqueName, false, hashFormIds);
            }
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm);
        }
    }
}