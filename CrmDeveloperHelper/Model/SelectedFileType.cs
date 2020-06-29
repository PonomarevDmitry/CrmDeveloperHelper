using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public enum SelectedFileType
    {
        WebResource,

        WebResourceText,

        WebResourceJavaScript,

        WebResourceJavaScriptHasLinkedSystemForm,

        Report,

        CSharp,

        Xml
    }

    public static class SelectedFileTypeExtensions
    {
        public static Func<string, bool> GetCheckerFunction(this SelectedFileType selectedFileType)
        {
            switch (selectedFileType)
            {
                case SelectedFileType.WebResource:
                    return Helpers.FileOperations.SupportsWebResourceType;

                case SelectedFileType.WebResourceText:
                    return Helpers.FileOperations.SupportsWebResourceTextType;

                case SelectedFileType.WebResourceJavaScript:
                case SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm:
                    return Helpers.FileOperations.SupportsJavaScriptType;

                case SelectedFileType.Report:
                    return Helpers.FileOperations.SupportsReportType;

                case SelectedFileType.CSharp:
                    return Helpers.FileOperations.SupportsCSharpType;

                case SelectedFileType.Xml:
                    return Helpers.FileOperations.SupportsXmlType;

                default:
                    break;
            }

            return null;
        }
    }
}
