using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginTypeCreateDescriptionCommand : AbstractCommand
    {
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private FileCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, idCommand, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        private FileCSharpProjectPluginTypeCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FileCSharpProjectPluginTypeCreateDescriptionCommand InstanceEntityDescription { get; private set; }

        public static FileCSharpProjectPluginTypeCreateDescriptionCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static FileCSharpProjectPluginTypeCreateDescriptionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new FileCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new FileCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new FileCSharpProjectPluginTypeCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeCreateDescriptionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            try
            {
                var listFiles = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();
                helper.ActivateOutputWindow(null);

                foreach (var projectItem in listFiles.OrderBy(d => d.FileNames[1]))
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, projectItem.FileNames[1]);
                }

                VSProject2Info.GetPluginTypes(listFiles, out var pluginTypesNotCompiled, out var projectInfos);

                var task = ExecuteAsync(helper, pluginTypesNotCompiled, projectInfos);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper, string[] pluginTypesNotCompiled, VSProject2Info[] projectInfos)
        {
            try
            {
                string[] pluginTypeArray = await CSharpCodeHelper.GetTypeFullNameListAsync(pluginTypesNotCompiled, projectInfos);

                if (pluginTypeArray.Any())
                {
                    helper.HandleActionOnPluginTypesCommand(null, this._actionOnComponent, this._fieldName, this._fieldTitle, pluginTypeArray);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectAny(applicationObject, menuCommand);

            if (_actionOnComponent == ActionOnComponent.EntityDescription)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectPluginTypeCreateEntityDescriptionCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.SingleXmlField)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.Description)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectPluginTypeCreateDescriptionCommand);
            }
        }
    }
}
