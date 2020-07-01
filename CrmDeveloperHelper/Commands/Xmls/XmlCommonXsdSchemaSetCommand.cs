using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class XmlCommonXsdSchemaSetCommand : AbstractDynamicCommandXsdSchemas
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private XmlCommonXsdSchemaSetCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static XmlCommonXsdSchemaSetCommand InstanceCode { get; private set; }

        public static XmlCommonXsdSchemaSetCommand InstanceDocuments { get; private set; }

        public static XmlCommonXsdSchemaSetCommand InstanceFile { get; private set; }

        public static XmlCommonXsdSchemaSetCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new XmlCommonXsdSchemaSetCommand(commandService, PackageIds.guidDynamicCommandSet.CodeXmlCommonXsdSchemaSetCommandId, sourceCode);

            InstanceDocuments = new XmlCommonXsdSchemaSetCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsXmlCommonXsdSchemaSetCommandId, sourceDocuments);

            InstanceFile = new XmlCommonXsdSchemaSetCommand(commandService, PackageIds.guidDynamicCommandSet.FileXmlCommonXsdSchemaSetCommandId, sourceFile);

            InstanceFolder = new XmlCommonXsdSchemaSetCommand(commandService, PackageIds.guidDynamicCommandSet.FolderXmlCommonXsdSchemaSetCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, Tuple<string, string[]> schemas)
        {
            var listFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.Xml).ToList();

            if (listFiles.Any())
            {
                foreach (var document in listFiles.Where(s => s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible).Select(s => s.Document))
                {
                    ContentComparerHelper.ReplaceXsdSchemaInDocument(document, schemas.Item2);
                }

                foreach (var filePath in listFiles.Where(s => !(s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible)).Select(s => s.FilePath))
                {
                    ContentComparerHelper.ReplaceXsdSchemaInFile(filePath, schemas.Item2);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<string, string[]> schemas, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.Xml);
        }
    }
}