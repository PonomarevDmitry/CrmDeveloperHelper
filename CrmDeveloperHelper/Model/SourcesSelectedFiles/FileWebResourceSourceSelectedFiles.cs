using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles
{
    public class FileWebResourceSourceSelectedFiles : ISourceSelectedFiles
    {
        private static FileWebResourceSourceSelectedFiles _source;

        public static FileWebResourceSourceSelectedFiles CreateSource()
        {
            if (_source == null)
            {
                _source = new FileWebResourceSourceSelectedFiles();
            }

            return _source;
        }

        private FileWebResourceSourceSelectedFiles()
        {

        }

        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, SelectedFileType selectedFileType)
        {
            if (selectedFileType == SelectedFileType.WebResource)
            {
                return helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, false).ToList();
            }
            else if (selectedFileType == SelectedFileType.WebResourceText)
            {
                return helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, SelectedFileType selectedFileType)
        {
            if (selectedFileType == SelectedFileType.WebResource)
            {
                CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny(applicationObject, menuCommand);
            }
            else if (selectedFileType == SelectedFileType.WebResourceText)
            {
                CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(applicationObject, menuCommand);
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}