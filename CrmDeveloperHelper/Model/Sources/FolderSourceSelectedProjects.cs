using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources
{
    public class FolderSourceSelectedProjects : ISourceSelectedProjects
    {
        private static FolderSourceSelectedProjects _source;

        public static FolderSourceSelectedProjects CreateSource()
        {
            if (_source == null)
            {
                _source = new FolderSourceSelectedProjects();
            }

            return _source;
        }

        private FolderSourceSelectedProjects()
        {

        }

        public IEnumerable<Project> GetSelectedProjects(DTEHelper helper)
        {
            var projectItemList = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, true).Where(p => p.ContainingProject != null).ToList();

            var projectDict = new Dictionary<string, Project>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var projectItem in projectItemList)
            {
                if (!projectDict.ContainsKey(projectItem.ContainingProject.UniqueName))
                {
                    projectDict.Add(projectItem.ContainingProject.UniqueName, projectItem.ContainingProject);
                }
            }

            return projectDict.Values.ToList();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectRecursive(applicationObject, menuCommand);
        }
    }
}