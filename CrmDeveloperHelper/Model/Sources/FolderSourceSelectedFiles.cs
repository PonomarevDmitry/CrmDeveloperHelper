using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources
{
    public class FolderSourceSelectedFiles : ISourceSelectedFiles
    {
        private static FolderSourceSelectedFiles _source;

        public static FolderSourceSelectedFiles CreateSource()
        {
            if (_source == null)
            {
                _source = new FolderSourceSelectedFiles();
            }

            return _source;
        }

        private FolderSourceSelectedFiles()
        {

        }

        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, SelectedFileType selectedFileType)
        {
            Func<string, bool> checkerFunction = selectedFileType.GetCheckerFunction();

            if (checkerFunction != null)
            {
                return helper.GetSelectedFilesInSolutionExplorer(checkerFunction, true).ToList();
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, SelectedFileType selectedFileType)
        {
            switch (selectedFileType)
            {
                case SelectedFileType.WebResource:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceText:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceJavaScript:
                case SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm:
                case SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Report:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportRecursive(applicationObject, menuCommand);
                    break;

                case SelectedFileType.CSharp:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpRecursive(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Xml:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlRecursive(applicationObject, menuCommand);
                    break;

                default:
                    menuCommand.Enabled = menuCommand.Visible = false;
                    break;
            }
        }
    }
}