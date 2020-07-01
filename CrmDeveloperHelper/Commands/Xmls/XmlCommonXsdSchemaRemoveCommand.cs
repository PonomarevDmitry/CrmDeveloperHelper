using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class XmlCommonXsdSchemaRemoveCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private XmlCommonXsdSchemaRemoveCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static XmlCommonXsdSchemaRemoveCommand InstanceCode { get; private set; }

        public static XmlCommonXsdSchemaRemoveCommand InstanceDocuments { get; private set; }

        public static XmlCommonXsdSchemaRemoveCommand InstanceFile { get; private set; }

        public static XmlCommonXsdSchemaRemoveCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new XmlCommonXsdSchemaRemoveCommand(commandService, PackageIds.guidCommandSet.CodeXmlCommonXsdSchemaRemoveCommandId, sourceCode);

            InstanceDocuments = new XmlCommonXsdSchemaRemoveCommand(commandService, PackageIds.guidCommandSet.DocumentsXmlCommonXsdSchemaRemoveCommandId, sourceDocuments);

            InstanceFile = new XmlCommonXsdSchemaRemoveCommand(commandService, PackageIds.guidCommandSet.FileXmlCommonXsdSchemaRemoveCommandId, sourceFile);

            InstanceFolder = new XmlCommonXsdSchemaRemoveCommand(commandService, PackageIds.guidCommandSet.FolderXmlCommonXsdSchemaRemoveCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var listFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.Xml).ToList();

            if (listFiles.Any())
            {
                foreach (var document in listFiles.Where(s => s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible).Select(s => s.Document))
                {
                    ContentComparerHelper.RemoveXsdSchemaInDocument(document);
                }

                foreach (var filePath in listFiles.Where(s => !(s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible)).Select(s => s.FilePath))
                {
                    ContentComparerHelper.RemoveXsdSchemaInFile(filePath);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.Xml);
        }
    }
}