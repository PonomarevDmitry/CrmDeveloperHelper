using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources
{
    public class CodeSourceSelectedProjects : ISourceSelectedProjects
    {
        private static CodeSourceSelectedProjects _source;

        public static CodeSourceSelectedProjects CreateSource()
        {
            if (_source == null)
            {
                _source = new CodeSourceSelectedProjects();
            }

            return _source;
        }

        private CodeSourceSelectedProjects()
        {

        }

        public IEnumerable<Project> GetSelectedProjects(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
            )
            {
                yield return document.ProjectItem.ContainingProject;
            }
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);
        }
    }
}