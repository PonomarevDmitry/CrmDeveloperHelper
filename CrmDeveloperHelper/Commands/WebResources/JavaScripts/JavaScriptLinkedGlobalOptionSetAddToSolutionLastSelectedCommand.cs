using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand InstanceDocuments { get; private set; }

        public static JavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand InstanceFile { get; private set; }

        public static JavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new JavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsJavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommandId, sourceDocuments);

            InstanceFile = new JavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileJavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommandId, sourceFile);

            InstanceFolder = new JavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FolderJavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet).ToList();

            var hashOpitonSetNames = new HashSet<string>();

            foreach (var selectedFile in selectedFiles)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    continue;
                }

                string javaScriptCode = File.ReadAllText(selectedFile.FilePath);

                if (CommonHandlers.GetLinkedGlobalOptionSetName(javaScriptCode, out string optionSetName))
                {
                    hashOpitonSetNames.Add(optionSetName);
                }
            }

            if (hashOpitonSetNames.Any())
            {
                helper.HandleAddingGlobalOptionSetToSolutionCommand(null, solutionUniqueName, false, hashOpitonSetNames);
            }
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet);
        }
    }
}
