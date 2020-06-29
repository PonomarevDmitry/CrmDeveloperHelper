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
        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, WebResourceType webResourceType)
        {
            if (webResourceType == WebResourceType.Ordinal)
            {
                return helper.GetOpenedDocuments(FileOperations.SupportsWebResourceType).ToList();
            }
            else if (webResourceType == WebResourceType.SupportsText)
            {
                return helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType).ToList();
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, WebResourceType webResourceType)
        {
            if (webResourceType == WebResourceType.Ordinal)
            {
                CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource(applicationObject, menuCommand);
            }
            else if (webResourceType == WebResourceType.SupportsText)
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