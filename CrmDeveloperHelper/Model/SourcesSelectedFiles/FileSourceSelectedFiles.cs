using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles
{
    public class FileSourceSelectedFiles : ISourceSelectedFiles
    {
        private static FileSourceSelectedFiles _source;

        public static FileSourceSelectedFiles CreateSource()
        {
            if (_source == null)
            {
                _source = new FileSourceSelectedFiles();
            }

            return _source;
        }

        private FileSourceSelectedFiles()
        {

        }

        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, SelectedFileType selectedFileType)
        {
            Func<string, bool> checkerFunction = selectedFileType.GetCheckerFunction();

            if (checkerFunction != null)
            {
                helper.GetSelectedFilesInSolutionExplorer(checkerFunction, false).ToList();
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, SelectedFileType selectedFileType)
        {
            switch (selectedFileType)
            {
                case SelectedFileType.WebResource:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceText:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceJavaScript:
                case SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptAny(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Report:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportAny(applicationObject, menuCommand);
                    break;

                case SelectedFileType.CSharp:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpAny(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Xml:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlAny(applicationObject, menuCommand);
                    break;

                default:
                    menuCommand.Enabled = menuCommand.Visible = false;
                    break;
            }
        }
    }
}