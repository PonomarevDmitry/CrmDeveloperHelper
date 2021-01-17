using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources
{
    public class FileSourceSelectedFileSingle : ISourceSelectedFiles
    {
        private static FileSourceSelectedFileSingle _source;

        public static FileSourceSelectedFileSingle CreateSource()
        {
            if (_source == null)
            {
                _source = new FileSourceSelectedFileSingle();
            }

            return _source;
        }

        private FileSourceSelectedFileSingle()
        {

        }

        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, SelectedFileType selectedFileType)
        {
            Func<string, bool> checkerFunction = selectedFileType.GetCheckerFunction();

            if (checkerFunction != null)
            {
                var result = helper.GetSelectedFilesInSolutionExplorer(checkerFunction, false).ToList();

                if (result.Count == 1)
                {
                    return result;
                }
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, SelectedFileType selectedFileType)
        {
            switch (selectedFileType)
            {
                case SelectedFileType.WebResource:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceSingle(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceText:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceJavaScript:
                case SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm:
                case SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptSingle(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Report:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportSingle(applicationObject, menuCommand);
                    break;

                case SelectedFileType.CSharp:
                    CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);
                    break;

                default:
                    menuCommand.Enabled = menuCommand.Visible = false;
                    break;
            }
        }
    }
}