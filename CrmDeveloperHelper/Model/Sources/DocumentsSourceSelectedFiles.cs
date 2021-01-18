using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources
{
    public class DocumentsSourceSelectedFiles : ISourceSelectedFiles
    {
        private static DocumentsSourceSelectedFiles _source;

        public static DocumentsSourceSelectedFiles CreateSource()
        {
            if (_source == null)
            {
                _source = new DocumentsSourceSelectedFiles();
            }

            return _source;
        }

        private DocumentsSourceSelectedFiles()
        {

        }

        public IEnumerable<SelectedFile> GetSelectedFiles(DTEHelper helper, SelectedFileType selectedFileType)
        {
            Func<string, bool> checkerFunction = selectedFileType.GetCheckerFunction();

            if (checkerFunction != null)
            {
                return helper.GetOpenedDocuments(checkerFunction).ToList();
            }

            return Enumerable.Empty<SelectedFile>();
        }

        public void CommandBeforeQueryStatus(DTE2 applicationObject, OleMenuCommand menuCommand, SelectedFileType selectedFileType)
        {
            switch (selectedFileType)
            {
                case SelectedFileType.WebResource:
                    CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceText:
                    CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(applicationObject, menuCommand);
                    break;

                case SelectedFileType.WebResourceJavaScript:
                case SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm:
                case SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet:
                    CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsJavaScript(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Report:
                    CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsReport(applicationObject, menuCommand);
                    break;

                case SelectedFileType.CSharp:
                case SelectedFileType.CSharpHasLinkedGlobalOptionSet:
                    CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);
                    break;

                case SelectedFileType.Xml:
                    CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsXml(applicationObject, menuCommand);
                    break;

                default:
                    menuCommand.Enabled = menuCommand.Visible = false;
                    break;
            }
        }
    }
}