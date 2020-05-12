using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpPluginTypeExplorerCommand : AbstractCommand
    {
        private FileCSharpPluginTypeExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileCSharpPluginTypeExplorerCommandId)
        {
        }

        public static FileCSharpPluginTypeExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpPluginTypeExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            try
            {
                var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

                if (projectItem != null)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, projectItem.FileNames[1]);
                    helper.ActivateOutputWindow(null);

                    VSProject2Info.GetPluginTypes(new[] { projectItem }, out var pluginTypesNotCompiled, out var projectInfos);

                    var task = ExecuteAsync(helper, pluginTypesNotCompiled, projectInfos);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            try
            {
                string pluginType = await CSharpCodeHelper.GetSingleFileTypeFullNameAsync(pluginTypesNotCompiled, projectInfos);

                helper.HandleOpenPluginTypeExplorer(pluginType);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);
        }
    }
}