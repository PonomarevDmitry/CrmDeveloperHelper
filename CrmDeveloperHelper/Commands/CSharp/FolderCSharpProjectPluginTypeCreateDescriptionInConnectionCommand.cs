using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, baseIdStart, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        private FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceEntityDescription { get; private set; }

        public static FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeCreateEntityDescriptionInConnectionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeGetCustomWorkflowActivityInfoInConnectionCommandId
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            try
            {
                var listFiles = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, true).ToList();
                helper.ActivateOutputWindow(null);

                foreach (var projectItem in listFiles.OrderBy(d => d.FileNames[1]))
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, projectItem.FileNames[1]);
                }

                VSProject2Info.GetPluginTypes(listFiles, out var pluginTypesNotCompiled, out var projectInfos);

                var task = ExecuteAsync(helper, connectionData, pluginTypesNotCompiled, projectInfos);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, ConnectionData connectionData, string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            try
            {
                string[] pluginTypeArray = await CSharpCodeHelper.GetTypeFullNameListAsync(pluginTypesNotCompiled, projectInfos);

                if (pluginTypeArray.Any())
                {
                    helper.HandleActionOnPluginTypesCommand(connectionData, this._actionOnComponent, this._fieldName, this._fieldTitle, pluginTypeArray);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectRecursive(applicationObject, menuCommand);
        }
    }
}
