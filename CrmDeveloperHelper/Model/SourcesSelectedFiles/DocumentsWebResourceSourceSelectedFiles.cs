using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles
{
    public class DocumentsWebResourceSourceSelectedFiles : ISourceSelectedFiles
    {
        private static DocumentsWebResourceSourceSelectedFiles _source;

        public static DocumentsWebResourceSourceSelectedFiles CreateSource()
        {
            if (_source == null)
            {
                _source = new DocumentsWebResourceSourceSelectedFiles();
            }

            return _source;
        }

        private DocumentsWebResourceSourceSelectedFiles()
        {

        }

        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, SelectedFileType selectedFileType)
        {
            if (selectedFileType == SelectedFileType.WebResource)
            {
                return helper.GetOpenedDocuments(FileOperations.SupportsWebResourceType).ToList();
            }
            else if (selectedFileType == SelectedFileType.WebResourceText)
            {
                return helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType).ToList();
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, SelectedFileType selectedFileType)
        {
            if (selectedFileType == SelectedFileType.WebResource)
            {
                CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource(applicationObject, menuCommand);
            }
            else if (selectedFileType == SelectedFileType.WebResourceText)
            {
                CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(applicationObject, menuCommand);
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}