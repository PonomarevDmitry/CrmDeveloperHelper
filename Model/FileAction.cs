using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [TypeConverter("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum FileAction
    {
        [Description("")]
        None = 0,

        [Description("Open file in Visual Studio")]
        OpenFileInVisualStudio = 1,

        [Description("Open file in Text Editor")]
        OpenFileInTextEditor = 2,

        [Description("Select file in folder")]
        SelectFileInFolder = 3,
    }

    public static class FileActionExtensions
    {
        public static string GetDescription(this FileAction fileAction)
        {
            switch (fileAction)
            {
                case FileAction.None:
                    return string.Empty;

                case FileAction.OpenFileInVisualStudio:
                    return "Open file in Visual Studio";

                case FileAction.OpenFileInTextEditor:
                    return "Open file in Text Editor";

                case FileAction.SelectFileInFolder:
                    return "Select file in folder";
            }

            return fileAction.ToString();
        }
    }
}