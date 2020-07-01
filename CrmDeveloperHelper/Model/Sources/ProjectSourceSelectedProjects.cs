using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources
{
    public class ProjectSourceSelectedProjects : ISourceSelectedProjects
    {
        private static ProjectSourceSelectedProjects _source;

        public static ProjectSourceSelectedProjects CreateSource()
        {
            if (_source == null)
            {
                _source = new ProjectSourceSelectedProjects();
            }

            return _source;
        }

        private ProjectSourceSelectedProjects()
        {

        }

        public IEnumerable<Project> GetSelectedProjects(DTEHelper helper)
        {
            return helper.GetSelectedProjects();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}