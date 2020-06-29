using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles
{
    public class CodeSourceSelectedFiles : ISourceSelectedFiles
    {
        private static CodeSourceSelectedFiles _source;

        public static CodeSourceSelectedFiles CreateSource()
        {
            if (_source == null)
            {
                _source = new CodeSourceSelectedFiles();
            }

            return _source;
        }

        private CodeSourceSelectedFiles()
        {

        }

        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, SelectedFileType selectedFileType)
        {
            Func<string, bool> checkerFunction = selectedFileType.GetCheckerFunction();

            if (checkerFunction != null)
            {
                return helper.GetOpenedFileInCodeWindow(checkerFunction, true).ToList();
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, SelectedFileType selectedFileType)
        {
            switch (selectedFileType)
            {
                case SelectedFileType.WebResource:
                    CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceText:
                    CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResourceText(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceJavaScript:
                    CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm:
                    CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Report:
                    CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(applicationObject, menuCommand);
                    break;

                case SelectedFileType.CSharp:
                    CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Xml:
                    CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);
                    break;

                default:
                    menuCommand.Enabled = menuCommand.Visible = false;
                    break;
            }
        }
    }
}
