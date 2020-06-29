using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles
{
    public class FolderWebResourceSourceSelectedFiles : ISourceSelectedFiles
    {
        private static FolderWebResourceSourceSelectedFiles _source;

        public static FolderWebResourceSourceSelectedFiles CreateSource()
        {
            if (_source == null)
            {
                _source = new FolderWebResourceSourceSelectedFiles();
            }

            return _source;
        }

        private FolderWebResourceSourceSelectedFiles()
        {

        }

        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, WebResourceType webResourceType)
        {
            if (webResourceType == WebResourceType.Ordinal)
            {
                return helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, true).ToList();
            }
            else if (webResourceType == WebResourceType.SupportsText)
            {
                return helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, WebResourceType webResourceType)
        {
            if (webResourceType == WebResourceType.Ordinal)
            {
                CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(applicationObject, menuCommand);
            }
            else if (webResourceType == WebResourceType.SupportsText)
            {
                CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(applicationObject, menuCommand);
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}