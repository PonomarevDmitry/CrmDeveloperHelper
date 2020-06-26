using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface ISourceSelectedFiles
    {
        IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper);

        void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand);
    }
}
