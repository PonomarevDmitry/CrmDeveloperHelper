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
    public class FileSourceSelectedProjectSingle : ISourceSelectedProjects
    {
        private static FileSourceSelectedProjectSingle _source;

        public static FileSourceSelectedProjectSingle CreateSource()
        {
            if (_source == null)
            {
                _source = new FileSourceSelectedProjectSingle();
            }

            return _source;
        }

        private FileSourceSelectedProjectSingle()
        {

        }

        public IEnumerable<Project> GetSelectedProjects(DTEHelper helper)
        {
            var projectItemList = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, false).Where(p => p.ContainingProject != null).ToList();

            var projectDict = new Dictionary<string, Project>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var projectItem in projectItemList)
            {
                if (!projectDict.ContainsKey(projectItem.ContainingProject.UniqueName))
                {
                    projectDict.Add(projectItem.ContainingProject.UniqueName, projectItem.ContainingProject);
                }
            }

            if (projectDict.Count == 1)
            {
                yield return projectDict.Values.First();
            }
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectSingle(applicationObject, menuCommand);
        }
    }
}