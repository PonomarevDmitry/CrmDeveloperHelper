using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface ISourceSelectedProjects
    {
        IEnumerable<Project> GetSelectedProjects(DTEHelper helper);

        void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand);
    }
}