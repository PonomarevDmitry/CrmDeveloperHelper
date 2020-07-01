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
    public class DocumentsSourceSelectedProjects : ISourceSelectedProjects
    {
        private static DocumentsSourceSelectedProjects _source;

        public static DocumentsSourceSelectedProjects CreateSource()
        {
            if (_source == null)
            {
                _source = new DocumentsSourceSelectedProjects();
            }

            return _source;
        }

        private DocumentsSourceSelectedProjects()
        {

        }

        public IEnumerable<Project> GetSelectedProjects(DTEHelper helper)
        {
            var documentList = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsCSharpType).ToList();

            var projectDict = new Dictionary<string, Project>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var document in documentList.Where(d => d.ProjectItem != null && d.ProjectItem.ContainingProject != null))
            {
                if (!projectDict.ContainsKey(document.ProjectItem.ContainingProject.UniqueName))
                {
                    projectDict.Add(document.ProjectItem.ContainingProject.UniqueName, document.ProjectItem.ContainingProject);
                }
            }

            return projectDict.Values.ToList();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsContainingProject(applicationObject, menuCommand);
        }
    }
}