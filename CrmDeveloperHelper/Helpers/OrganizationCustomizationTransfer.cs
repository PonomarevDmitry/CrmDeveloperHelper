using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class OrganizationCustomizationTransfer
    {
        private IOrganizationComparerSource _comparerSource;

        public ConnectionData ConnectionSource => _comparerSource.Connection1;

        public ConnectionData ConnectionTarget => _comparerSource.Connection2;

        private string _folder;

        private const string _tabSpacer = "    ";

        private readonly IWriteToOutput _iWriteToOutput;

        public OrganizationCustomizationTransfer(IOrganizationComparerSource comparerSource, IWriteToOutput iWriteToOutput, string folder)
        {
            this._comparerSource = comparerSource;
            this._iWriteToOutput = iWriteToOutput;
            this._folder = folder;
        }

        public Task<string> TrasnferAuditAsync()
        {
            return Task.Run(async () => await TrasnferAudit());
        }

        private async Task<string> TrasnferAudit()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content, "Connection CRM Source.", "Connection CRM Target.");

            string operation = string.Format(Properties.OperationNames.TransferingAuditFormat2, ConnectionSource.Name, ConnectionTarget.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var repositorySource = new EntityMetadataRepository(_comparerSource.Service1);
            var repositoryTarget = new EntityMetadataRepository(_comparerSource.Service2);

            var taskSource = repositorySource.GetEntitiesWithAttributesForAuditAsync();
            var taskTarget = repositoryTarget.GetEntitiesWithAttributesForAuditAsync();

            var listEntityMetadataSource = await taskSource;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Entities in {0}: {1}", ConnectionSource.Name, listEntityMetadataSource.Count()));

            var listEntityMetadataTarget = await taskTarget;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, "Entities in {0}: {1}", ConnectionTarget.Name, listEntityMetadataTarget.Count()));

            var commonEntityMetadata = new List<LinkedEntities<EntityMetadata>>();

            foreach (var entityMetadata1 in listEntityMetadataSource.OrderBy(e => e.LogicalName))
            {
                {
                    var entityMetadata2 = listEntityMetadataTarget.FirstOrDefault(e => string.Equals(e.LogicalName, entityMetadata1.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                    if (entityMetadata2 != null)
                    {
                        commonEntityMetadata.Add(new LinkedEntities<EntityMetadata>(entityMetadata1, entityMetadata2));
                        continue;
                    }
                }
            }

            HashSet<string> entitiesToPublish = new HashSet<string>();

            var entitiesToEnableAudit = commonEntityMetadata.Where(
                e =>
                e.Entity1.IsAuditEnabled != null
                && e.Entity1.IsAuditEnabled.Value
                && e.Entity2.IsAuditEnabled != null
                && e.Entity2.IsAuditEnabled.CanBeChanged
                && e.Entity2.IsAuditEnabled.Value == false
                ).ToList();

            if (entitiesToEnableAudit.Any())
            {
                content
                    .AppendLine()
                    .AppendFormat("Enabling Audit on Entities: {0}", entitiesToEnableAudit.Count)
                    .AppendLine();

                foreach (var entityLink in entitiesToEnableAudit.OrderBy(e => e.Entity1.LogicalName))
                {
                    content.AppendLine(_tabSpacer + entityLink.Entity1.LogicalName);

                    entitiesToPublish.Add(entityLink.Entity1.LogicalName);

                    try
                    {
                        entityLink.Entity2.IsAuditEnabled.Value = true;

                        await repositoryTarget.UpdateEntityMetadataAsync(entityLink.Entity2);
                    }
                    catch (Exception ex)
                    {
                        var desc = DTEHelper.GetExceptionDescription(ex);

                        content.AppendLine(desc);
                    }
                }
            }

            bool first = true;

            foreach (var entityLink in commonEntityMetadata.OrderBy(e => e.Entity1.LogicalName))
            {
                var query = from source in entityLink.Entity1.Attributes
                            join target in entityLink.Entity2.Attributes on source.LogicalName equals target.LogicalName
                            where source.IsAuditEnabled != null
                                    && string.IsNullOrEmpty(source.AttributeOf)
                                    && string.IsNullOrEmpty(target.AttributeOf)
                                    && source.IsAuditEnabled.Value
                                    && target.IsAuditEnabled != null
                                    && target.IsAuditEnabled.CanBeChanged
                                    && target.IsAuditEnabled.Value == false
                            orderby target.LogicalName
                            select target;

                foreach (var attribute in query)
                {
                    if (first)
                    {
                        content
                            .AppendLine()
                            .AppendLine("Enabling Audit on Attributes:")
                            .AppendLine();

                        first = false;
                    }

                    content
                        .AppendFormat(_tabSpacer + "{0}.{1}", attribute.EntityLogicalName, attribute.LogicalName)
                        .AppendLine();

                    entitiesToPublish.Add(attribute.EntityLogicalName);

                    try
                    {
                        attribute.IsAuditEnabled.Value = true;

                        await repositoryTarget.UpdateAttributeMetadataAsync(attribute);
                    }
                    catch (Exception ex)
                    {
                        var desc = DTEHelper.GetExceptionDescription(ex);

                        content.AppendLine(desc);
                    }
                }
            }

            if (entitiesToPublish.Any())
            {
                content
                    .AppendLine()
                    .AppendFormat("Publish Entities: {0}", entitiesToPublish.Count)
                    .AppendLine();

                foreach (var item in entitiesToPublish.OrderBy(s => s))
                {
                    content.AppendLine(_tabSpacer + item);
                }

                var repositoryPublish = new PublishActionsRepository(_comparerSource.Service2);

                try
                {
                    await repositoryPublish.PublishEntitiesAsync(entitiesToPublish);
                }
                catch (Exception ex)
                {
                    var desc = DTEHelper.GetExceptionDescription(ex);

                    content.AppendLine(desc);
                }
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = string.Format("OrgTransfer Audit from {0} to {1} at {2}.txt"
                , this.ConnectionSource.Name
                , this.ConnectionTarget.Name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public Task<string> TrasnferWorkflowsStatesAsync()
        {
            return Task.Run(async () => await TrasnferWorkflowsStates());
        }

        private async Task<string> TrasnferWorkflowsStates()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content, "Connection CRM Source.", "Connection CRM Target.");

            string operation = string.Format(Properties.OperationNames.TransferingWorkflowsStatesFormat2, ConnectionSource.Name, ConnectionTarget.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var columnsSet = new ColumnSet
            (
                Workflow.Schema.EntityPrimaryIdAttribute
                , Workflow.Schema.Attributes.primaryentity
                , Workflow.Schema.Attributes.category
                , Workflow.Schema.Attributes.name
                , Workflow.Schema.Attributes.statecode
                , Workflow.Schema.Attributes.statuscode
                , Workflow.Schema.Attributes.iscrmuiworkflow
                , Workflow.Schema.Attributes.ismanaged
            );

            var taskSource = _comparerSource.GetWorkflow1Async(columnsSet);
            var taskTarget = _comparerSource.GetWorkflow2Async(columnsSet);

            List<Workflow> listSource = (await taskSource).ToList();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.WorkflowsInConnectionFormat2, ConnectionSource.Name, listSource.Count()));

            List<Workflow> listTarget = (await taskTarget).ToList();

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.WorkflowsInConnectionFormat2, ConnectionTarget.Name, listTarget.Count()));

            List<LinkedEntities<Workflow>> commonList = new List<LinkedEntities<Workflow>>();

            foreach (Workflow workflowSource in listSource)
            {
                Workflow workflowTarget = listTarget.FirstOrDefault(workflow => workflow.Id == workflowSource.Id);

                if (workflowTarget != null)
                {
                    commonList.Add(new LinkedEntities<Workflow>(workflowSource, workflowTarget));
                    continue;
                }
            }

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.WorkflowsCommonFormat3, ConnectionSource.Name, ConnectionTarget.Name, commonList.Count()));

            List<Workflow> workflowsToActivate = new List<Workflow>();
            List<Workflow> workflowsToDeactivate = new List<Workflow>();

            {
                ProgressReporter reporter = new ProgressReporter(_iWriteToOutput, commonList.Count, 5, Properties.OrganizationComparerStrings.WorkflowsProcessingCommon);

                foreach (LinkedEntities<Workflow> workflow in commonList)
                {
                    reporter.Increase();

                    if (workflow.Entity1.StatusCode?.Value != workflow.Entity2.StatusCode?.Value)
                    {
                        List<Workflow> list = null;

                        if (workflow.Entity1.StatusCode.Value == (int)Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2)
                        {
                            list = workflowsToActivate;
                        }
                        else if (workflow.Entity1.StatusCode.Value == (int)Workflow.Schema.OptionSets.statuscode.Draft_0_Draft_1)
                        {
                            list = workflowsToDeactivate;
                        }

                        if (list != null)
                        {
                            list.Add(workflow.Entity2);
                        }
                    }
                }
            }

            var orderedDeactivate = workflowsToDeactivate
                .OrderBy(w => w.PrimaryEntity)
                .ThenBy(w => w.Category?.Value)
                .ThenBy(w => w.Name)
                .ThenBy(w => w.Id);

            var orderedActivate = workflowsToActivate
                    .OrderBy(w => w.PrimaryEntity)
                    .ThenBy(w => w.Category?.Value)
                    .ThenBy(w => w.Name)
                    .ThenBy(w => w.Id);

            FormatTextTableHandler tableDeactivateWorkflows = new FormatTextTableHandler();
            tableDeactivateWorkflows.SetHeader("Entity", "Category", "Name", "StatusCode", "IsCrmUIWorkflow", "IsManaged", "Id", "Url");

            FormatTextTableHandler tableActivateWorkflows = new FormatTextTableHandler();
            tableActivateWorkflows.SetHeader("Entity", "Category", "Name", "StatusCode", "IsCrmUIWorkflow", "IsManaged", "Id", "Url");

            foreach (var workflow in orderedDeactivate)
            {
                workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out string categoryName2);
                workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out string statusCode2);

                tableDeactivateWorkflows.AddLine(
                    workflow.PrimaryEntity
                    , categoryName2
                    , workflow.Name
                    , statusCode2
                    , workflow.IsCrmUIWorkflow.ToString()
                    , workflow.IsManaged.ToString()
                    , workflow.Id.ToString()
                    , _comparerSource.Service2.UrlGenerator.GetSolutionComponentUrl(ComponentType.Workflow, workflow.Id)
                );
            }

            foreach (var workflow in orderedActivate)
            {
                workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out string categoryName2);
                workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out string statusCode2);

                tableActivateWorkflows.AddLine(
                    workflow.PrimaryEntity
                    , categoryName2
                    , workflow.Name
                    , statusCode2
                    , workflow.IsCrmUIWorkflow.ToString()
                    , workflow.IsManaged.ToString()
                    , workflow.Id.ToString()
                    , _comparerSource.Service2.UrlGenerator.GetSolutionComponentUrl(ComponentType.Workflow, workflow.Id)
                );
            }

            if (tableDeactivateWorkflows.Count > 0)
            {
                content
                      .AppendLine()
                      .AppendLine()
                      .AppendLine()
                      .AppendLine(new string('-', 150))
                      .AppendLine()
                      .AppendLine();

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.WorkflowsToDeactivationInConnectionFormat2, ConnectionTarget.Name, tableDeactivateWorkflows.Count);

                tableDeactivateWorkflows.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((_tabSpacer + e).TrimEnd()));
            }

            if (tableActivateWorkflows.Count > 0)
            {
                content
                       .AppendLine()
                       .AppendLine()
                       .AppendLine()
                       .AppendLine(new string('-', 150))
                       .AppendLine()
                       .AppendLine();

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.WorkflowsToActivationInConnectionFormat2, ConnectionTarget.Name, tableActivateWorkflows.Count);

                tableActivateWorkflows.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((_tabSpacer + e).TrimEnd()));
            }

            foreach (var workflow in orderedDeactivate)
            {
                try
                {
                    await _comparerSource.Service2.ExecuteAsync(new SetStateRequest()
                    {
                        EntityMoniker = workflow.ToEntityReference(),

                        State = new Microsoft.Xrm.Sdk.OptionSetValue((int)Workflow.Schema.OptionSets.statecode.Draft_0),
                        Status = new Microsoft.Xrm.Sdk.OptionSetValue((int)Workflow.Schema.OptionSets.statuscode.Draft_0_Draft_1),
                    });
                }
                catch (Exception ex)
                {
                    var desc = DTEHelper.GetExceptionDescription(ex);

                    workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out string categoryName2);
                    workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out string statusCode2);

                    var workflowDescription = tableDeactivateWorkflows.FormatLineWithHeadersInLine(
                        workflow.PrimaryEntity
                        , categoryName2
                        , workflow.Name
                        , statusCode2
                        , workflow.IsCrmUIWorkflow.ToString()
                        , workflow.IsManaged.ToString()
                        , workflow.Id.ToString()
                        , _comparerSource.Service2.UrlGenerator.GetSolutionComponentUrl(ComponentType.Workflow, workflow.Id)
                    );

                    string operationLocal = string.Format(Properties.OperationNames.DeactivatingEntityFormat2, workflow.LogicalName, ConnectionTarget.Name, workflowDescription);

                    content.AppendLine().AppendLine().AppendLine();
                    content.AppendLine(new string('-', 150)).AppendLine();
                    content.AppendFormat(Properties.OutputStrings.ExceptionWhileOperationFormat1, operationLocal).AppendLine();
                    content.AppendLine(desc);
                    content.AppendLine(new string('-', 150)).AppendLine();
                }
            }

            foreach (var workflow in orderedActivate)
            {
                try
                {
                    await _comparerSource.Service2.ExecuteAsync(new SetStateRequest()
                    {
                        EntityMoniker = workflow.ToEntityReference(),

                        State = new Microsoft.Xrm.Sdk.OptionSetValue((int)Workflow.Schema.OptionSets.statecode.Activated_1),
                        Status = new Microsoft.Xrm.Sdk.OptionSetValue((int)Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2),
                    });
                }
                catch (Exception ex)
                {
                    var desc = DTEHelper.GetExceptionDescription(ex);

                    workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out string categoryName2);
                    workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out string statusCode2);

                    var workflowDescription = tableDeactivateWorkflows.FormatLineWithHeadersInLine(
                        workflow.PrimaryEntity
                        , categoryName2
                        , workflow.Name
                        , statusCode2
                        , workflow.IsCrmUIWorkflow.ToString()
                        , workflow.IsManaged.ToString()
                        , workflow.Id.ToString()
                        , _comparerSource.Service2.UrlGenerator.GetSolutionComponentUrl(ComponentType.Workflow, workflow.Id)
                    );

                    string operationLocal = string.Format(Properties.OperationNames.ActivatingEntityFormat3, workflow.LogicalName, ConnectionTarget.Name, workflowDescription);

                    content.AppendLine().AppendLine().AppendLine();
                    content.AppendLine(new string('-', 150)).AppendLine();
                    content.AppendFormat(Properties.OutputStrings.ExceptionWhileOperationFormat1, operationLocal).AppendLine();
                    content.AppendLine(desc);
                    content.AppendLine(new string('-', 150)).AppendLine();
                }
            }

            if (tableActivateWorkflows.Count == 0
                && tableDeactivateWorkflows.Count == 0
                )
            {
                content.AppendLine(Properties.OrganizationComparerStrings.WorkflowsNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = string.Format("OrgTransfer Workflows States from {0} to {1} at {2}.txt"
                , this.ConnectionSource.Name
                , this.ConnectionTarget.Name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }

        public Task<string> TrasnferPluginStepsStatesAsync()
        {
            return Task.Run(async () => await TrasnferPluginStepsStates());
        }

        private async Task<string> TrasnferPluginStepsStates()
        {
            StringBuilder content = new StringBuilder();

            await _comparerSource.InitializeConnection(_iWriteToOutput, content, "Connection CRM Source.", "Connection CRM Target.");

            string operation = string.Format(Properties.OperationNames.TransferingPluginStepsStatesFormat2, ConnectionSource.Name, ConnectionTarget.Name);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(null, operation));

            var taskSource = _comparerSource.GetSdkMessageProcessingStep1Async();
            var taskTarget = _comparerSource.GetSdkMessageProcessingStep2Async();

            List<SdkMessageProcessingStep> listSource = await taskSource;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.PluginStepsInConnectionFormat2, ConnectionSource.Name, listSource.Count()));

            List<SdkMessageProcessingStep> listTarget = await taskTarget;

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.PluginStepsInConnectionFormat2, ConnectionTarget.Name, listTarget.Count()));

            List<LinkedEntities<SdkMessageProcessingStep>> commonList = new List<LinkedEntities<SdkMessageProcessingStep>>();

            foreach (SdkMessageProcessingStep stepSource in listSource)
            {
                SdkMessageProcessingStep stepTarget = listTarget.FirstOrDefault(st => st.Id == stepSource.Id);

                if (stepTarget != null)
                {
                    commonList.Add(new LinkedEntities<SdkMessageProcessingStep>(stepSource, stepTarget));
                    continue;
                }
            }

            content.AppendLine(_iWriteToOutput.WriteToOutput(null, Properties.OrganizationComparerStrings.PluginStepsCommonFormat3, ConnectionSource.Name, ConnectionTarget.Name, commonList.Count()));

            List<SdkMessageProcessingStep> pluginStepsToActivate = new List<SdkMessageProcessingStep>();
            List<SdkMessageProcessingStep> pluginStepsToDeactivate = new List<SdkMessageProcessingStep>();

            foreach (LinkedEntities<SdkMessageProcessingStep> step in commonList
                .OrderBy(s => s.Entity1.EventHandler?.Name ?? "Unknown")
                .ThenBy(s => s.Entity1.PrimaryObjectTypeCodeName)
                .ThenBy(s => s.Entity1.SecondaryObjectTypeCodeName)
                .ThenBy(s => s.Entity1.SdkMessageId?.Name ?? "Unknown", new MessageComparer())
                .ThenBy(s => s.Entity1.Stage.Value)
                .ThenBy(s => s.Entity1.Mode.Value)
                )
            {
                if (step.Entity1.StatusCode?.Value != step.Entity2.StatusCode?.Value)
                {
                    List<SdkMessageProcessingStep> list = null;

                    if (step.Entity1.StatusCode.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.statuscode.Enabled_0_Enabled_1)
                    {
                        list = pluginStepsToActivate;
                    }
                    else if (step.Entity1.StatusCode.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.statuscode.Disabled_1_Disabled_2)
                    {
                        list = pluginStepsToDeactivate;
                    }

                    if (list != null)
                    {
                        list.Add(step.Entity2);
                    }
                }
            }

            var orderedDeactivate = pluginStepsToDeactivate
                .OrderBy(s => s.EventHandler?.Name ?? "Unknown")
                .ThenBy(s => s.PrimaryObjectTypeCodeName)
                .ThenBy(s => s.SecondaryObjectTypeCodeName)
                .ThenBy(s => s.SdkMessageId?.Name ?? "Unknown", new MessageComparer())
                .ThenBy(s => s.Stage.Value)
                .ThenBy(s => s.Mode.Value)
                ;

            var orderedActivate = pluginStepsToActivate
                .OrderBy(s => s.EventHandler?.Name ?? "Unknown")
                .ThenBy(s => s.PrimaryObjectTypeCodeName)
                .ThenBy(s => s.SecondaryObjectTypeCodeName)
                .ThenBy(s => s.SdkMessageId?.Name ?? "Unknown", new MessageComparer())
                .ThenBy(s => s.Stage.Value)
                .ThenBy(s => s.Mode.Value)
                ;

            FormatTextTableHandler tableDeactivatePluginSteps = new FormatTextTableHandler();
            tableDeactivatePluginSteps.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "IsHidden", "IsManaged", "FilteringAttributes");

            FormatTextTableHandler tableActivatePluginSteps = new FormatTextTableHandler();
            tableActivatePluginSteps.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "IsHidden", "IsManaged", "FilteringAttributes");

            foreach (var step in orderedDeactivate)
            {
                tableDeactivatePluginSteps.AddLine(
                    step.EventHandler?.Name ?? "Unknown"
                    , step.PrimaryObjectTypeCodeName
                    , step.SecondaryObjectTypeCodeName
                    , step.SdkMessageId?.Name ?? "Unknown"
                    , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                    , step.Rank.ToString()
                    , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                    , step.IsHidden?.Value.ToString()
                    , step.IsManaged.ToString()
                    , step.FilteringAttributesStringsSorted
                );
            }

            foreach (var step in orderedActivate)
            {
                tableActivatePluginSteps.AddLine(
                    step.EventHandler?.Name ?? "Unknown"
                    , step.PrimaryObjectTypeCodeName
                    , step.SecondaryObjectTypeCodeName
                    , step.SdkMessageId?.Name ?? "Unknown"
                    , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                    , step.Rank.ToString()
                    , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                    , step.IsHidden?.Value.ToString()
                    , step.IsManaged.ToString()
                    , step.FilteringAttributesStringsSorted
                );
            }

            if (tableDeactivatePluginSteps.Count > 0)
            {
                content
                      .AppendLine()
                      .AppendLine()
                      .AppendLine()
                      .AppendLine(new string('-', 150))
                      .AppendLine()
                      .AppendLine();

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.PluginStepsToDeactivationInConnectionFormat2, ConnectionTarget.Name, tableDeactivatePluginSteps.Count);

                tableDeactivatePluginSteps.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((_tabSpacer + e).TrimEnd()));
            }

            if (tableActivatePluginSteps.Count > 0)
            {
                content
                       .AppendLine()
                       .AppendLine()
                       .AppendLine()
                       .AppendLine(new string('-', 150))
                       .AppendLine()
                       .AppendLine();

                content.AppendLine().AppendLine().AppendFormat(Properties.OrganizationComparerStrings.PluginStepsToActivationInConnectionFormat2, ConnectionTarget.Name, tableActivatePluginSteps.Count);

                tableActivatePluginSteps.GetFormatedLines(false).ForEach(e => content.AppendLine().Append((_tabSpacer + e).TrimEnd()));
            }

            foreach (var step in orderedDeactivate)
            {
                try
                {
                    await _comparerSource.Service2.ExecuteAsync(new SetStateRequest()
                    {
                        EntityMoniker = step.ToEntityReference(),

                        State = new Microsoft.Xrm.Sdk.OptionSetValue((int)SdkMessageProcessingStep.Schema.OptionSets.statecode.Disabled_1),
                        Status = new Microsoft.Xrm.Sdk.OptionSetValue((int)SdkMessageProcessingStep.Schema.OptionSets.statuscode.Disabled_1_Disabled_2),
                    });
                }
                catch (Exception ex)
                {
                    var desc = DTEHelper.GetExceptionDescription(ex);

                    var stepDescription = tableDeactivatePluginSteps.FormatLineWithHeadersInLine(
                        step.EventHandler?.Name ?? "Unknown"
                        , step.PrimaryObjectTypeCodeName
                        , step.SecondaryObjectTypeCodeName
                        , step.SdkMessageId?.Name ?? "Unknown"
                        , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                        , step.Rank.ToString()
                        , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                        , step.IsHidden?.Value.ToString()
                        , step.IsManaged.ToString()
                        , step.FilteringAttributesStringsSorted
                    );

                    string operationLocal = string.Format(Properties.OperationNames.DeactivatingEntityFormat2, step.LogicalName, ConnectionTarget.Name, stepDescription);

                    content.AppendLine().AppendLine().AppendLine();
                    content.AppendLine(new string('-', 150)).AppendLine();
                    content.AppendFormat(Properties.OutputStrings.ExceptionWhileOperationFormat1, operationLocal).AppendLine();
                    content.AppendLine(desc);
                    content.AppendLine(new string('-', 150)).AppendLine();
                }
            }

            foreach (var step in orderedActivate)
            {
                try
                {
                    await _comparerSource.Service2.ExecuteAsync(new SetStateRequest()
                    {
                        EntityMoniker = step.ToEntityReference(),

                        State = new Microsoft.Xrm.Sdk.OptionSetValue((int)SdkMessageProcessingStep.Schema.OptionSets.statecode.Enabled_0),
                        Status = new Microsoft.Xrm.Sdk.OptionSetValue((int)SdkMessageProcessingStep.Schema.OptionSets.statuscode.Enabled_0_Enabled_1),
                    });
                }
                catch (Exception ex)
                {
                    var desc = DTEHelper.GetExceptionDescription(ex);

                    var stepDescription = tableActivatePluginSteps.FormatLineWithHeadersInLine(
                        step.EventHandler?.Name ?? "Unknown"
                        , step.PrimaryObjectTypeCodeName
                        , step.SecondaryObjectTypeCodeName
                        , step.SdkMessageId?.Name ?? "Unknown"
                        , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                        , step.Rank.ToString()
                        , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                        , step.IsHidden?.Value.ToString()
                        , step.IsManaged.ToString()
                        , step.FilteringAttributesStringsSorted
                    );

                    string operationLocal = string.Format(Properties.OperationNames.ActivatingEntityFormat3, step.LogicalName, ConnectionTarget.Name, stepDescription);

                    content.AppendLine().AppendLine().AppendLine();
                    content.AppendLine(new string('-', 150)).AppendLine();
                    content.AppendFormat(Properties.OutputStrings.ExceptionWhileOperationFormat1, operationLocal).AppendLine();
                    content.AppendLine(desc);
                    content.AppendLine(new string('-', 150)).AppendLine();
                }
            }

            if (tableActivatePluginSteps.Count == 0
                && tableDeactivatePluginSteps.Count == 0
                )
            {
                content.AppendLine(Properties.OrganizationComparerStrings.PluginStepsStatesNoDifference);
            }

            content.AppendLine().AppendLine().AppendLine(_iWriteToOutput.WriteToOutputEndOperation(null, operation));

            string fileName = string.Format("OrgTransfer Plugin Steps States from {0} to {1} at {2}.txt"
                , this.ConnectionSource.Name
                , this.ConnectionTarget.Name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            string filePath = Path.Combine(_folder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            return filePath;
        }
    }
}