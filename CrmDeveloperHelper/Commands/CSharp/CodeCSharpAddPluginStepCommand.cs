using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpAddPluginStepCommand : AbstractCommand
    {
        private CodeCSharpAddPluginStepCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpAddPluginStepCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeCSharpAddPluginStepCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpAddPluginStepCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeCSharpAddPluginStepCommand);
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
                helper.ActivateOutputWindow(null);
                string fileType = await PropertiesHelper.GetTypeFullNameAsync(document);

                helper.HandleAddPluginStep(fileType, null);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
