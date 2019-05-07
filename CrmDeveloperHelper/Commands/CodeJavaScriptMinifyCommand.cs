using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeJavaScriptMinifyCommand : AbstractCommand
    {
        private CodeJavaScriptMinifyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeJavaScriptMinifyCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeJavaScriptMinifyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeJavaScriptMinifyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsJavaScriptType);

            if (document != null 
                && document.ProjectItem != null
                //&& document.ProjectItem.ContainingProject != null
                //&& document.ProjectItem.ContainingProject.ProjectItems != null
            )
            {
                string newFileName = Path.GetFileNameWithoutExtension(document.FullName) + ".min" + Path.GetExtension(document.FullName);
                string newFilePath = Path.Combine(Path.GetDirectoryName(document.FullName), newFileName);

                string fileContent = File.ReadAllText(document.FullName);

                var minifier = new Microsoft.Ajax.Utilities.Minifier();
                var minifiedString = minifier.MinifyJavaScript(fileContent);

                File.WriteAllText(newFilePath, minifiedString, new UTF8Encoding(false));

                //document.ProjectItem.ContainingProject.ProjectItems.AddFromFile(newFilePath);

                document.ProjectItem.Collection.AddFromFile(newFilePath);

                helper.WriteToOutputFilePathUri(null, newFilePath);
                helper.ActivateOutputWindow(null);
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(command, menuCommand);
        }
    }
}
