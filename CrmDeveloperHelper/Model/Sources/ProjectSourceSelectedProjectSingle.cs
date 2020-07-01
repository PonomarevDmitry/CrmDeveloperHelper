using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources
{
    public class ProjectSourceSelectedProjectSingle : ISourceSelectedProjects
    {
        private static ProjectSourceSelectedProjectSingle _source;

        public static ProjectSourceSelectedProjectSingle CreateSource()
        {
            if (_source == null)
            {
                _source = new ProjectSourceSelectedProjectSingle();
            }

            return _source;
        }

        private ProjectSourceSelectedProjectSingle()
        {

        }

        public IEnumerable<Project> GetSelectedProjects(DTEHelper helper)
        {
            var selectedProjects = helper.GetSelectedProjects().Take(2).ToList();

            if (selectedProjects.Count == 1)
            {
                return selectedProjects;
            }

            return Enumerable.Empty<Project>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectSingle(applicationObject, menuCommand);
        }
    }
}