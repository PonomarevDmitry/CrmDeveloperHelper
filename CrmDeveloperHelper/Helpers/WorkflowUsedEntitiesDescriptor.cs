using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class WorkflowUsedEntitiesDescriptor
    {
        private const string tabspacer = "    ";

        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;
        private readonly SolutionComponentDescriptor _descriptor;

        public WorkflowUsedEntitiesDescriptor(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            )
        {
            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._descriptor = descriptor;
        }

        public Task GetDescriptionWithUsedEntitiesInSolutionWorkflowsAsync(StringBuilder strFile, Guid idSolution)
        {
            return Task.Run(() => GetDescriptionWithUsedEntitiesInSolutionWorkflows(strFile, idSolution));
        }

        private async Task GetDescriptionWithUsedEntitiesInSolutionWorkflows(StringBuilder strFile, Guid idSolution)
        {
            try
            {
                var repositorySolution = new SolutionRepository(_service);

                var solution = await repositorySolution.GetSolutionByIdAsync(idSolution);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputStartOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingSolutionWorkflowsFormat2, _service.ConnectionData.Name, solution.UniqueName));

                SolutionComponentRepository repository = new SolutionComponentRepository(_service);

                var repositoryWorkflow = new WorkflowRepository(_service);

                HashSet<Guid> workflowsWithEntities = new HashSet<Guid>();

                Dictionary<EntityReference, HashSet<Guid>> list = new Dictionary<EntityReference, HashSet<Guid>>();

                var handler = new WorkflowUsedEntitiesHandler();

                {
                    var components = await repository.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.Workflow, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                    _descriptor.GetEntities<Workflow>((int)ComponentType.Workflow, components.Select(c => c.ObjectId));

                    var workflows = await repositoryWorkflow.GetListByIdListAsync(components.Select(en => en.ObjectId.Value), new ColumnSet(Workflow.Schema.Attributes.xaml));

                    foreach (var item in workflows)
                    {
                        if (string.IsNullOrEmpty(item.Xaml))
                        {
                            continue;
                        }

                        var xmlContent = ContentComparerHelper.RemoveDiacritics(item.Xaml);

                        var doc = XElement.Parse(xmlContent);

                        var coll = await handler.GetWorkflowUsedEntitiesAsync(doc);

                        if (coll.Count > 0)
                        {
                            workflowsWithEntities.Add(item.Id);

                            foreach (var entityRef in coll)
                            {
                                HashSet<Guid> linkedWorkflows = null;

                                if (list.ContainsKey(entityRef))
                                {
                                    linkedWorkflows = list[entityRef];
                                }
                                else
                                {
                                    list[entityRef] = linkedWorkflows = new HashSet<Guid>();
                                }

                                linkedWorkflows.Add(item.Id);
                            }
                        }
                    }
                }

                {
                    var components = await repository.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.Entity, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                    var listMetadata = _descriptor.MetadataSource.GetEntityMetadataList(components.Where(e => e.ObjectId.HasValue).Select(e => e.ObjectId.Value));

                    foreach (var entityMetadata in listMetadata)
                    {
                        var workflows = await repositoryWorkflow.GetListAsync(entityMetadata.LogicalName, Workflow.Schema.OptionSets.category.Business_Rule_2, null, null, new ColumnSet(Workflow.Schema.Attributes.xaml));

                        foreach (var item in workflows)
                        {
                            if (!string.IsNullOrEmpty(item.Xaml) && !workflowsWithEntities.Contains(item.Id))
                            {
                                var xmlContent = ContentComparerHelper.RemoveDiacritics(item.Xaml);

                                var doc = XElement.Parse(xmlContent);

                                var coll = await handler.GetWorkflowUsedEntitiesAsync(doc);

                                if (coll.Count > 0)
                                {
                                    workflowsWithEntities.Add(item.Id);

                                    foreach (var entityRef in coll)
                                    {
                                        HashSet<Guid> linkedWorkflows = null;

                                        if (list.ContainsKey(entityRef))
                                        {
                                            linkedWorkflows = list[entityRef];
                                        }
                                        else
                                        {
                                            list[entityRef] = linkedWorkflows = new HashSet<Guid>();
                                        }

                                        linkedWorkflows.Add(item.Id);
                                    }
                                }
                            }
                        }
                    }
                }

                await FillDescriptionUsedEntities(strFile, workflowsWithEntities, list);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputEndOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingSolutionWorkflowsFormat2, _service.ConnectionData.Name, solution.UniqueName));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        public Task GetDescriptionWithUsedNotExistsEntitiesInSolutionWorkflowsAsync(StringBuilder strFile, Guid idSolution)
        {
            return Task.Run(() => GetDescriptionWithUsedNotExistsEntitiesInSolutionWorkflows(strFile, idSolution));
        }

        private async Task GetDescriptionWithUsedNotExistsEntitiesInSolutionWorkflows(StringBuilder strFile, Guid idSolution)
        {
            try
            {
                var repositorySolution = new SolutionRepository(_service);

                var solution = await repositorySolution.GetSolutionByIdAsync(idSolution);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputStartOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingSolutionWorkflowsFormat2, _service.ConnectionData.Name, solution.UniqueName));

                SolutionComponentRepository repository = new SolutionComponentRepository(_service);

                HashSet<Guid> workflowsWithEntities = new HashSet<Guid>();

                Dictionary<EntityReference, HashSet<Guid>> list = new Dictionary<EntityReference, HashSet<Guid>>();

                var handler = new WorkflowUsedEntitiesHandler();

                var repositoryWorkflow = new WorkflowRepository(_service);

                {
                    var components = await repository.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.Workflow, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                    _descriptor.GetEntities<Workflow>((int)ComponentType.Workflow, components.Select(c => c.ObjectId));

                    var workflows = await repositoryWorkflow.GetListByIdListAsync(components.Select(en => en.ObjectId.Value), new ColumnSet(Workflow.Schema.Attributes.xaml));

                    foreach (var item in workflows)
                    {
                        var xmlContent = ContentComparerHelper.RemoveDiacritics(item.Xaml);

                        var doc = XElement.Parse(xmlContent);

                        var coll = await handler.GetWorkflowUsedEntitiesAsync(doc);

                        if (coll.Count > 0)
                        {
                            workflowsWithEntities.Add(item.Id);

                            foreach (var entityRef in coll)
                            {
                                HashSet<Guid> linkedWorkflows = null;

                                if (list.ContainsKey(entityRef))
                                {
                                    linkedWorkflows = list[entityRef];
                                }
                                else
                                {
                                    list[entityRef] = linkedWorkflows = new HashSet<Guid>();
                                }

                                linkedWorkflows.Add(item.Id);
                            }
                        }
                    }
                }

                {
                    var components = await repository.GetSolutionComponentsByTypeAsync(solution.Id, ComponentType.Entity, new ColumnSet(SolutionComponent.Schema.Attributes.objectid));

                    var listMetadata = _descriptor.MetadataSource.GetEntityMetadataList(components.Where(e => e.ObjectId.HasValue).Select(e => e.ObjectId.Value));

                    foreach (var entityMetadata in listMetadata)
                    {
                        var workflows = await repositoryWorkflow.GetListAsync(entityMetadata.LogicalName, Workflow.Schema.OptionSets.category.Business_Rule_2, null, null, new ColumnSet(Workflow.Schema.Attributes.xaml));

                        foreach (var item in workflows)
                        {
                            if (!string.IsNullOrEmpty(item.Xaml) && !workflowsWithEntities.Contains(item.Id))
                            {
                                var xmlContent = ContentComparerHelper.RemoveDiacritics(item.Xaml);

                                var doc = XElement.Parse(xmlContent);

                                var coll = await handler.GetWorkflowUsedEntitiesAsync(doc);

                                if (coll.Count > 0)
                                {
                                    workflowsWithEntities.Add(item.Id);

                                    foreach (var entityRef in coll)
                                    {
                                        HashSet<Guid> linkedWorkflows = null;

                                        if (list.ContainsKey(entityRef))
                                        {
                                            linkedWorkflows = list[entityRef];
                                        }
                                        else
                                        {
                                            list[entityRef] = linkedWorkflows = new HashSet<Guid>();
                                        }

                                        linkedWorkflows.Add(item.Id);
                                    }
                                }
                            }
                        }
                    }
                }

                await FillDescriptionNotExistsEntities(strFile, workflowsWithEntities, list);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputEndOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingSolutionWorkflowsFormat2, _service.ConnectionData.Name, solution.UniqueName));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        public Task GetDescriptionWithUsedEntitiesInAllWorkflowsAsync(StringBuilder strFile)
        {
            return Task.Run(() => GetDescriptionWithUsedEntitiesInAllWorkflows(strFile));
        }

        private async Task GetDescriptionWithUsedEntitiesInAllWorkflows(StringBuilder strFile)
        {
            try
            {
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputStartOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingWorkflowsFormat1, _service.ConnectionData.Name));

                var repositoryWorkflow = new WorkflowRepository(_service);

                HashSet<Guid> workflowsWithEntities = new HashSet<Guid>();

                Dictionary<EntityReference, HashSet<Guid>> list = new Dictionary<EntityReference, HashSet<Guid>>();

                var handler = new WorkflowUsedEntitiesHandler();

                {
                    var workflows = await repositoryWorkflow.GetListAsync(null, null, null, null, new ColumnSet(Workflow.Schema.Attributes.xaml));

                    _descriptor.GetEntities<Workflow>((int)ComponentType.Workflow, workflows.Select(c => c.Id));

                    foreach (var item in workflows)
                    {
                        if (string.IsNullOrEmpty(item.Xaml))
                        {
                            continue;
                        }

                        var xmlContent = ContentComparerHelper.RemoveDiacritics(item.Xaml);

                        var doc = XElement.Parse(xmlContent);

                        List<EntityReference> coll = await handler.GetWorkflowUsedEntitiesAsync(doc);

                        if (coll.Count > 0)
                        {
                            workflowsWithEntities.Add(item.Id);

                            foreach (var entityRef in coll)
                            {
                                HashSet<Guid> linkedWorkflows = null;

                                if (list.ContainsKey(entityRef))
                                {
                                    linkedWorkflows = list[entityRef];
                                }
                                else
                                {
                                    list[entityRef] = linkedWorkflows = new HashSet<Guid>();
                                }

                                linkedWorkflows.Add(item.Id);
                            }
                        }
                    }
                }

                await FillDescriptionUsedEntities(strFile, workflowsWithEntities, list);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputEndOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingWorkflowsFormat1, _service.ConnectionData.Name));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private async Task FillDescriptionUsedEntities(StringBuilder strFile, HashSet<Guid> workflowsWithEntities, Dictionary<EntityReference, HashSet<Guid>> dictUsedEntities)
        {
            string message = string.Empty;

            if (dictUsedEntities.Count == 0)
            {
                strFile
                       .AppendLine()
                       .AppendLine()
                       .AppendLine()
                       .AppendLine(this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "No used entities in workflows."))
                       ;
                return;
            }

            strFile
                .AppendLine()
                .AppendLine()
                .AppendFormat(this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Used Entities {0}", dictUsedEntities.Count)).AppendLine()
                ;

            var orderedList = dictUsedEntities.Keys.OrderBy(i => i.LogicalName).ThenBy(i => i.Name).ThenBy(i => i.Id);

            {
                FormatTextTableHandler table = new FormatTextTableHandler();
                table.SetHeader("LogicalName", "Name", "Id", "Url");

                foreach (var item in orderedList)
                {
                    var values = new List<string>() { item.LogicalName, item.Name, item.Id.ToString() };

                    var url = _service.ConnectionData.GetEntityInstanceUrl(item.LogicalName, item.Id);

                    if (!string.IsNullOrEmpty(url))
                    {
                        values.Add(url);
                    }

                    table.AddLine(values);
                }

                table.GetFormatedLines(false).ForEach(s => strFile.AppendLine(tabspacer + s));
            }

            strFile
                .AppendLine()
                .AppendLine()
                .AppendLine()
                .AppendLine(new string('-', 150))
                .AppendLine()
                .AppendLine()
                .AppendLine()
                ;

            strFile.AppendFormat("Used Entities Full Information {0}", dictUsedEntities.Count).AppendLine();

            foreach (var item in orderedList)
            {
                strFile
                    .AppendLine()
                    .AppendLine()
                    .AppendLine();

                FormatTextTableHandler table = new FormatTextTableHandler();
                table.SetHeader("LogicalName", "Name", "Id");
                table.AddLine(item.LogicalName, item.Name, item.Id.ToString());
                table.GetFormatedLines(false).ForEach(s => strFile.AppendLine(s));

                var url = _service.ConnectionData.GetEntityInstanceUrl(item.LogicalName, item.Id);

                if (!string.IsNullOrEmpty(url))
                {
                    strFile.AppendLine("Url:");
                    strFile.AppendLine(url);
                }

                strFile.AppendLine();

                try
                {
                    var entityMetadata = _descriptor.MetadataSource.GetEntityMetadata(item.LogicalName);

                    if (entityMetadata != null)
                    {
                        var repositoryGeneric = new GenericRepository(_service, entityMetadata);

                        var entity = await repositoryGeneric.GetEntityByIdAsync(item.Id, ColumnSetInstances.AllColumns);

                        if (entity != null)
                        {
                            var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, _service.ConnectionData);

                            strFile
                                .AppendLine(desc)
                                .AppendLine()
                                .AppendLine()
                                ;
                        }
                        else
                        {
                            strFile
                                .AppendFormat("{0} With Id = {1} Does Not Exists", item.LogicalName, item.Id).AppendLine()
                                .AppendLine()
                                .AppendLine()
                                ;
                        }
                    }
                    else
                    {
                        strFile
                                .AppendFormat("Entity with name '{0}' Does Not Exists", item.LogicalName).AppendLine()
                                .AppendFormat("{0} With Id = {1} Does Not Exists", item.LogicalName, item.Id).AppendLine()
                                .AppendLine()
                                .AppendLine()
                                ;
                    }
                }
                catch (Exception ex)
                {
                    var description = DTEHelper.GetExceptionDescription(ex);

                    strFile
                        .AppendLine(description)
                        .AppendLine()
                        .AppendLine()
                        ;
                }

                message = await _descriptor.GetSolutionComponentsDescriptionAsync(dictUsedEntities[item].Select(id => new SolutionComponent()
                {
                    ObjectId = id,
                    ComponentType = new OptionSetValue((int)ComponentType.Workflow),
                }));

                strFile
                    .AppendLine("This entity Used By Workflows:")
                    .AppendLine(message)
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    ;
            }

            if (workflowsWithEntities.Any())
            {
                strFile
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine("These entities Used By Workflows:")
                        ;

                message = await _descriptor.GetSolutionComponentsDescriptionAsync(workflowsWithEntities.Select(id => new SolutionComponent()
                {
                    ObjectId = id,
                    ComponentType = new OptionSetValue((int)ComponentType.Workflow),
                }));

                strFile.AppendLine(message);
            }
        }

        public Task GetDescriptionWithEntityFieldStringsInAllWorkflowsAsync(StringBuilder strFile)
        {
            return Task.Run(() => GetDescriptionWithEntityFieldStringsInAllWorkflows(strFile));
        }

        private async Task GetDescriptionWithEntityFieldStringsInAllWorkflows(StringBuilder strFile)
        {
            try
            {
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputStartOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingWorkflowsFormat1, _service.ConnectionData.Name));

                var repositoryWorkflow = new WorkflowRepository(_service);

                var workflowsWithStrings = new Dictionary<Guid, TupleList<string, string, List<string>>>();

                var handler = new WorkflowUsedEntitiesHandler();

                {
                    var workflowsList = await repositoryWorkflow.GetListAsync(null, null, null, null, new ColumnSet(Workflow.Schema.Attributes.xaml));

                    _descriptor.GetEntities<Workflow>((int)ComponentType.Workflow, workflowsList.Select(c => c.Id));

                    foreach (var workflow in workflowsList)
                    {
                        if (string.IsNullOrEmpty(workflow.Xaml))
                        {
                            continue;
                        }

                        var xmlContent = ContentComparerHelper.RemoveDiacritics(workflow.Xaml);

                        var doc = XElement.Parse(xmlContent);

                        TupleList<string, string, List<string>> coll = await handler.GetWorkflowStringsEntityFieldsAsync(doc);

                        if (coll.Count > 0)
                        {
                            workflowsWithStrings.Add(workflow.Id, coll);
                        }
                    }
                }

                await FillDescriptionUsedEntities(strFile, workflowsWithStrings);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputEndOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingWorkflowsFormat1, _service.ConnectionData.Name));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private async Task FillDescriptionUsedEntities(StringBuilder strFile, Dictionary<Guid, TupleList<string, string, List<string>>> workflowsWithStrings)
        {
            string message = string.Empty;

            if (workflowsWithStrings.Count == 0)
            {
                strFile
                       .AppendLine()
                       .AppendLine()
                       .AppendLine()
                       .AppendLine(this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "No workflows with Entity Field Strings."))
                       ;

                return;
            }

            strFile
                .AppendLine()
                .AppendLine()
                .AppendFormat(this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Workflows with Entity Field Strings: {0}", workflowsWithStrings.Count)).AppendLine()
                .AppendLine()
                ;

            message = await _descriptor.GetSolutionComponentsDescriptionAsync(workflowsWithStrings.Keys.Select(id => new SolutionComponent()
            {
                ObjectId = id,
                ComponentType = new OptionSetValue((int)ComponentType.Workflow),
            }));

            strFile.AppendLine(message);

            var worlflowList = _descriptor.GetEntities<Workflow>((int)ComponentType.Workflow, workflowsWithStrings.Keys);

            foreach (var workflow in worlflowList
                .OrderBy(w => w.PrimaryEntity)
                .ThenBy(w => w.Category?.Value)
                .ThenBy(w => w.Name)
                .ThenBy(w => w.UniqueName)
                .ThenBy(w => w.Id)
            )
            {
                strFile
                    .AppendLine()
                    .AppendLine()
                    .AppendLine();

                var table = new FormatTextTableHandler("Step Name", "Value", "OriginalString");

                var list = workflowsWithStrings[workflow.Id];

                foreach (var step in list)
                {
                    foreach (var entityFieldString in step.Item3)
                    {
                        table.AddLine(step.Item1, entityFieldString, step.Item2);
                    }
                }

                message = _descriptor.GetComponentDescription((int)ComponentType.Workflow, workflow.Id);

                strFile
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    .AppendLine()
                    .AppendLine(message)
                    ;

                table.GetFormatedLines(false).ForEach(s => strFile.AppendLine(s));
            }

            strFile
                   .AppendLine()
                   .AppendLine()
                   .AppendLine()
                   ;
        }

        public Task GetDescriptionWithUsedNotExistsEntitiesInAllWorkflowsAsync(StringBuilder strFile)
        {
            return Task.Run(() => GetDescriptionWithUsedNotExistsEntitiesInAllWorkflows(strFile));
        }

        private async Task GetDescriptionWithUsedNotExistsEntitiesInAllWorkflows(StringBuilder strFile)
        {
            try
            {
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputStartOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingWorkflowsFormat1, _service.ConnectionData.Name));

                SolutionComponentRepository repository = new SolutionComponentRepository(_service);

                HashSet<Guid> workflowsWithEntities = new HashSet<Guid>();

                Dictionary<EntityReference, HashSet<Guid>> list = new Dictionary<EntityReference, HashSet<Guid>>();

                var handler = new WorkflowUsedEntitiesHandler();

                var repositoryWorkflow = new WorkflowRepository(_service);

                {
                    var workflows = await repositoryWorkflow.GetListAsync(null, null, null, null, new ColumnSet(Workflow.Schema.Attributes.xaml));

                    _descriptor.GetEntities<Workflow>((int)ComponentType.Workflow, workflows.Select(c => c.Id));

                    foreach (var item in workflows)
                    {
                        var xmlContent = ContentComparerHelper.RemoveDiacritics(item.Xaml);

                        var doc = XElement.Parse(xmlContent);

                        var coll = await handler.GetWorkflowUsedEntitiesAsync(doc);

                        if (coll.Count > 0)
                        {
                            workflowsWithEntities.Add(item.Id);

                            foreach (var entityRef in coll)
                            {
                                HashSet<Guid> linkedWorkflows = null;

                                if (list.ContainsKey(entityRef))
                                {
                                    linkedWorkflows = list[entityRef];
                                }
                                else
                                {
                                    list[entityRef] = linkedWorkflows = new HashSet<Guid>();
                                }

                                linkedWorkflows.Add(item.Id);
                            }
                        }
                    }
                }

                await FillDescriptionNotExistsEntities(strFile, workflowsWithEntities, list);

                strFile.AppendLine(this._iWriteToOutput.WriteToOutputEndOperation(_service.ConnectionData, Properties.OperationNames.AnalyzingWorkflowsFormat1, _service.ConnectionData.Name));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private async Task FillDescriptionNotExistsEntities(StringBuilder strFile, HashSet<Guid> workflowsWithEntities, Dictionary<EntityReference, HashSet<Guid>> list)
        {
            {
                var listToDelete = new List<EntityReference>();

                foreach (var itemRef in list.Keys)
                {
                    try
                    {
                        var entityMetadata = _descriptor.MetadataSource.GetEntityMetadata(itemRef.LogicalName);

                        if (entityMetadata != null)
                        {
                            var repositoryGeneric = new GenericRepository(_service, entityMetadata);

                            var entity = await repositoryGeneric.GetEntityByIdAsync(itemRef.Id, ColumnSetInstances.AllColumns);

                            if (entity != null)
                            {
                                listToDelete.Add(itemRef);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                foreach (var item in listToDelete)
                {
                    list.Remove(item);
                }

                workflowsWithEntities.Clear();

                foreach (var set in list.Values)
                {
                    foreach (var item in set)
                    {
                        workflowsWithEntities.Add(item);
                    }
                }
            }

            if (list.Count == 0)
            {
                strFile
                       .AppendLine()
                       .AppendLine()
                       .AppendLine()
                       .AppendLine(this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "No used not exists entities in workflows."))
                       ;
                return;
            }

            strFile
                .AppendLine()
                .AppendLine()
                .AppendFormat(this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Used Not Exists Entities {0}", list.Count)).AppendLine()
                ;

            var orderedList = list.Keys.OrderBy(i => i.LogicalName).ThenBy(i => i.Name).ThenBy(i => i.Id);

            {
                FormatTextTableHandler table = new FormatTextTableHandler();
                table.SetHeader("LogicalName", "Name", "Id", "Url");

                foreach (var item in orderedList)
                {
                    var values = new List<string>() { item.LogicalName, item.Name, item.Id.ToString() };

                    var url = _service.ConnectionData.GetEntityInstanceUrl(item.LogicalName, item.Id);

                    if (!string.IsNullOrEmpty(url))
                    {
                        values.Add(url);
                    }

                    table.AddLine(values);
                }

                table.GetFormatedLines(false).ForEach(s => strFile.AppendLine(tabspacer + s));
            }

            strFile
                .AppendLine()
                .AppendLine()
                .AppendLine()
                .AppendLine(new string('-', 150))
                .AppendLine()
                .AppendLine()
                .AppendLine()
                ;

            strFile.AppendFormat("Used Not Exists Entities Full Information {0}", list.Count).AppendLine();

            foreach (var item in orderedList)
            {
                strFile
                    .AppendLine()
                    .AppendLine()
                    .AppendLine();

                FormatTextTableHandler table = new FormatTextTableHandler();
                table.SetHeader("LogicalName", "Name", "Id");
                table.AddLine(item.LogicalName, item.Name, item.Id.ToString());
                table.GetFormatedLines(false).ForEach(s => strFile.AppendLine(s));

                var url = _service.ConnectionData.GetEntityInstanceUrl(item.LogicalName, item.Id);

                if (!string.IsNullOrEmpty(url))
                {
                    strFile.AppendLine("Url:");
                    strFile.AppendLine(url);
                }

                strFile.AppendLine();

                var message = await _descriptor.GetSolutionComponentsDescriptionAsync(list[item].Select(id => new SolutionComponent()
                {
                    ObjectId = id,
                    ComponentType = new OptionSetValue((int)ComponentType.Workflow),
                }));

                strFile
                    .AppendLine("This entity Used By Workflows:")
                    .AppendLine(message)
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(new string('-', 150))
                    ;
            }

            if (workflowsWithEntities.Any())
            {
                strFile
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine("These entities Used By Workflows:")
                        ;

                var desc = await _descriptor.GetSolutionComponentsDescriptionAsync(workflowsWithEntities.Select(id => new SolutionComponent()
                {
                    ObjectId = id,
                    ComponentType = new OptionSetValue((int)ComponentType.Workflow),
                }));

                strFile.AppendLine(desc);
            }
        }

        public async Task GetDescriptionUsedEntitiesInWorkflowAsync(StringBuilder strFile, Guid idWorkflow)
        {
            try
            {
                var repository = new WorkflowRepository(_service);

                var workflow = await repository.GetByIdAsync(idWorkflow, ColumnSetInstances.AllColumns);

                string xmlContent = ContentComparerHelper.RemoveDiacritics(workflow.Xaml);

                var doc = XElement.Parse(xmlContent);

                var handler = new WorkflowUsedEntitiesHandler();

                strFile
                    .AppendFormat("Entity:   {0}", workflow.PrimaryEntity).AppendLine()
                    .AppendFormat("Category: {0}", workflow.FormattedValues[Workflow.Schema.Attributes.category]).AppendLine()
                    .AppendFormat("Name:     {0}", workflow.Name).AppendLine()
                    .AppendFormat("Url:      {0}", _service.UrlGenerator.GetSolutionComponentUrl(ComponentType.Workflow, idWorkflow)).AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    ;

                var workflowsWithEntities = new HashSet<Guid>() { idWorkflow };

                Dictionary<EntityReference, HashSet<Guid>> list = (await handler.GetWorkflowUsedEntitiesAsync(doc)).ToDictionary(e => e, k => new HashSet<Guid>() { idWorkflow });

                await FillDescriptionUsedEntities(strFile, workflowsWithEntities, list);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        public async Task GetDescriptionUsedNotExistsEntitiesInWorkflowAsync(StringBuilder strFile, Guid idWorkflow)
        {
            try
            {
                var repository = new WorkflowRepository(_service);

                var workflow = await repository.GetByIdAsync(idWorkflow, ColumnSetInstances.AllColumns);

                string xmlContent = ContentComparerHelper.RemoveDiacritics(workflow.Xaml);

                var doc = XElement.Parse(xmlContent);

                HashSet<Guid> workflowsWithEntities = new HashSet<Guid>() { idWorkflow };

                var handler = new WorkflowUsedEntitiesHandler();

                Dictionary<EntityReference, HashSet<Guid>> list = (await handler.GetWorkflowUsedEntitiesAsync(doc)).ToDictionary(e => e, k => new HashSet<Guid>() { idWorkflow });

                strFile
                    .AppendFormat("Entity:   {0}", workflow.PrimaryEntity).AppendLine()
                    .AppendFormat("Category: {0}", workflow.FormattedValues[Workflow.Schema.Attributes.category]).AppendLine()
                    .AppendFormat("Name:     {0}", workflow.Name).AppendLine()
                    .AppendFormat("Url:      {0}", _service.UrlGenerator.GetSolutionComponentUrl(ComponentType.Workflow, idWorkflow)).AppendLine()
                    .AppendLine()
                    .AppendLine()
                    .AppendLine()
                    ;

                await FillDescriptionNotExistsEntities(strFile, workflowsWithEntities, list);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        public async Task GetDescriptionEntitesAndAttributesInWorkflowAsync(StringBuilder strFile, Guid idWorkflow)
        {
            try
            {
                var repository = new WorkflowRepository(_service);

                var workflow = await repository.GetByIdAsync(idWorkflow, ColumnSetInstances.AllColumns);

                string xmlContent = ContentComparerHelper.RemoveDiacritics(workflow.Xaml);

                var doc = XElement.Parse(xmlContent);

                var handler = new WorkflowUsedEntitiesHandler();

                strFile
                    .AppendFormat("Entity:   {0}", workflow.PrimaryEntity).AppendLine()
                    .AppendFormat("Category: {0}", workflow.FormattedValues[Workflow.Schema.Attributes.category]).AppendLine()
                    .AppendFormat("Name:     {0}", workflow.Name).AppendLine()
                    .AppendFormat("Url:      {0}", _service.UrlGenerator.GetSolutionComponentUrl(ComponentType.Workflow, idWorkflow)).AppendLine()
                    .AppendLine()
                    ;

                WorkflowAnalysis analysis = handler.GetWorkflowAnalysis(doc);

                if (analysis.UsedEntityAttributes.Any())
                {
                    strFile
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150))
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        ;

                    var tableUsedAttributes = new FormatTextTableHandler();
                    tableUsedAttributes.SetHeader("Entity", "Attribute", "StepName");

                    foreach (var step in analysis.UsedEntityAttributes
                        .OrderBy(e => e.EntityName)
                        .ThenBy(e => e.Attribute)
                        .ThenBy(e => e.DisplayName)
                        )
                    {
                        tableUsedAttributes.AddLine(step.EntityName, step.Attribute, step.DisplayName);
                    }

                    strFile.AppendFormat("Used Attributes: {0}", analysis.UsedEntityAttributes.Count).AppendLine();

                    tableUsedAttributes.GetFormatedLines(false).ForEach(s => strFile.AppendLine(tabspacer + s));
                }

                if (analysis.CreateUpdateEntitySteps.Any())
                {
                    strFile
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(new string('-', 150))
                        .AppendLine()
                        .AppendLine()
                        .AppendLine()
                        ;

                    strFile.AppendFormat("Created or Updated Entities {0}", analysis.CreateUpdateEntitySteps.Count).AppendLine();

                    foreach (var step in analysis.CreateUpdateEntitySteps)
                    {
                        strFile.AppendFormat("Entity {0}    Operation {1}    StepName {2}    With Attributes {2}", step.EntityName, step.StepType, step.DisplayName, step.SetEntityPropertySteps.Count).AppendLine()
                            ;

                        var tableUsedAttributes = new FormatTextTableHandler();
                        tableUsedAttributes.SetHeader("Attribute");

                        foreach (var stepSet in step.SetEntityPropertySteps
                            .OrderBy(e => e.EntityName)
                            .ThenBy(e => e.Attribute)
                            )
                        {
                            tableUsedAttributes.AddLine(stepSet.Attribute);
                        }

                        tableUsedAttributes.GetFormatedLines(false).ForEach(s => strFile.AppendLine(tabspacer + s));

                        strFile.AppendLine().AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }
    }
}