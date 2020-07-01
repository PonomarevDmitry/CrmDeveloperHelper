using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class XmlCommonRemoveCustomAttributesCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private XmlCommonRemoveCustomAttributesCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static XmlCommonRemoveCustomAttributesCommand InstanceCode { get; private set; }

        public static XmlCommonRemoveCustomAttributesCommand InstanceDocuments { get; private set; }

        public static XmlCommonRemoveCustomAttributesCommand InstanceFile { get; private set; }

        public static XmlCommonRemoveCustomAttributesCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new XmlCommonRemoveCustomAttributesCommand(commandService, PackageIds.guidCommandSet.CodeXmlCommonRemoveCustomAttributesCommandId, sourceCode);

            InstanceDocuments = new XmlCommonRemoveCustomAttributesCommand(commandService, PackageIds.guidCommandSet.DocumentsXmlCommonRemoveCustomAttributesCommandId, sourceDocuments);

            InstanceFile = new XmlCommonRemoveCustomAttributesCommand(commandService, PackageIds.guidCommandSet.FileXmlCommonRemoveCustomAttributesCommandId, sourceFile);

            InstanceFolder = new XmlCommonRemoveCustomAttributesCommand(commandService, PackageIds.guidCommandSet.FolderXmlCommonRemoveCustomAttributesCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var listFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.Xml).ToList();

            if (listFiles.Any())
            {
                foreach (var document in listFiles.Where(s => s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible).Select(s => s.Document))
                {
                    ContentComparerHelper.RemoveAllCustomAttributesInDocument(document);
                }

                foreach (var filePath in listFiles.Where(s => !(s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible)).Select(s => s.FilePath))
                {
                    ContentComparerHelper.RemoveAllCustomAttributesInFile(filePath);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.Xml);
        }
    }
}