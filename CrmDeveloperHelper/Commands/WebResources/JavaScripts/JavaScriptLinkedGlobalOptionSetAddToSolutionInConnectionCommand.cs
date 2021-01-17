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
    internal sealed class JavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand InstanceDocuments { get; private set; }

        public static JavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand InstanceFile { get; private set; }

        public static JavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new JavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsJavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommandId, sourceDocuments);

            InstanceFile = new JavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileJavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommandId, sourceFile);

            InstanceFolder = new JavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderJavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
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
                helper.HandleAddingGlobalOptionSetToSolutionCommand(connectionData, null, true, hashOpitonSetNames);
            }
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet);
        }
    }
}
