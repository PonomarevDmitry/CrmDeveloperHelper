using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class WorkflowRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public WorkflowRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<IEnumerable<Workflow>> GetListAsync(
            string filterEntity
            , Workflow.Schema.OptionSets.category? category
            , Workflow.Schema.OptionSets.mode? mode
            , Workflow.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<Guid> selectedWorkflows
            , ColumnSet columnSet
        )
        {
            return Task.Run(() => GetList(filterEntity, category, mode, statuscode, selectedWorkflows, columnSet));
        }

        public Task<IEnumerable<Workflow>> GetListAsync(
            string filterEntity
            , Workflow.Schema.OptionSets.category? category
            , Workflow.Schema.OptionSets.mode? mode
            , Workflow.Schema.OptionSets.statuscode? statuscode
            , ColumnSet columnSet
        )
        {
            return Task.Run(() => GetList(filterEntity, category, mode, statuscode, null, columnSet));
        }

        private IEnumerable<Workflow> GetList(
            string filterEntity
            , Workflow.Schema.OptionSets.category? category
            , Workflow.Schema.OptionSets.mode? mode
            , Workflow.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<Guid> selectedWorkflows
            , ColumnSet columnSet
        )
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Workflow.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.None,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Workflow.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                        new ConditionExpression(Workflow.Schema.Attributes.parentworkflowid, ConditionOperator.Null),
                    },
                },

                Orders =
                {
                    new OrderExpression(Workflow.Schema.Attributes.primaryentity, OrderType.Ascending),
                    new OrderExpression(Workflow.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Workflow.Schema.Attributes.primaryentity, ConditionOperator.Equal, filterEntity));
            }

            if (category.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Workflow.Schema.Attributes.category, ConditionOperator.Equal, (int)category.Value));
            }

            if (mode.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Workflow.Schema.Attributes.mode, ConditionOperator.Equal, (int)mode.Value));
            }

            if (statuscode.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(Workflow.Schema.Attributes.statuscode, ConditionOperator.Equal, (int)statuscode.Value));
            }

            if (selectedWorkflows != null && selectedWorkflows.Any())
            {
                var hashIds = new HashSet<Guid>(selectedWorkflows);

                query.Criteria.Conditions.Add(new ConditionExpression(Workflow.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, hashIds.ToArray()));
            }

            return _service.RetrieveMultipleAll<Workflow>(query);
        }

        public Task<List<Workflow>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<Workflow> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Workflow.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Workflow.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                        new ConditionExpression(Workflow.Schema.Attributes.parentworkflowid, ConditionOperator.Null),

                        new ConditionExpression(Workflow.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(Workflow.Schema.Attributes.primaryentity, OrderType.Ascending),
                    new OrderExpression(Workflow.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<Workflow>(query);
        }

        public Task<Workflow> GetByIdAsync(Guid idWorkflow, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetById(idWorkflow, columnSet));
        }

        private Workflow GetById(Guid idWorkflow, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Workflow.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Workflow.EntityPrimaryIdAttribute, ConditionOperator.Equal, idWorkflow),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Workflow>()).SingleOrDefault() : null;
        }

        public Workflow FindLinkedWorkflow(Guid idSystemForm, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Workflow.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Workflow.Schema.Attributes.parentworkflowid, ConditionOperator.Null),
                        new ConditionExpression(Workflow.Schema.Attributes.formid, ConditionOperator.Equal, idSystemForm),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Workflow>()).SingleOrDefault() : null;
        }

        public Task<List<Workflow>> GetListForEntitiesAsync(string[] entities, ColumnSet columnSet)
        {
            return Task.Run(() => GetListForEntities(entities, columnSet));
        }

        private List<Workflow> GetListForEntities(string[] entities, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Workflow.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Workflow.Schema.Attributes.parentworkflowid, ConditionOperator.Null),
                        new ConditionExpression(Workflow.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                        new ConditionExpression(Workflow.Schema.Attributes.primaryentity, ConditionOperator.In, entities),

                        new ConditionExpression(Workflow.Schema.Attributes.category, ConditionOperator.Equal, (int)Workflow.Schema.OptionSets.category.Business_Rule_2),
                    },
                },

                Orders =
                {
                    new OrderExpression(Workflow.Schema.Attributes.primaryentity, OrderType.Ascending),
                    new OrderExpression(Workflow.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<Workflow>(query);
        }

        public static IEnumerable<DataGridColumn> GetDataGridColumn()
        {
            //<DataGridTextColumn Header="Entity Name" Width="260" Binding="{Binding EntityName}" />

            //<DataGridTextColumn Header="Category" Width="200" Binding="{Binding Category}" />

            //<DataGridTextColumn Header="Workflow Name" Width="200" Binding="{Binding WorkflowName}" />

            //<DataGridTextColumn Header="Mode" Width="150" Binding="{Binding ModeName}" />

            //<DataGridTextColumn Header="Status" Width="150" Binding="{Binding StatusName}" />

            return new List<DataGridColumn>()
            {
                new DataGridTextColumn()
                {
                    Header = "Entity Name",
                    Width = new DataGridLength(260),
                    Binding = new Binding
                    {
                        Path = new PropertyPath(nameof(Workflow.PrimaryEntity)),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "Category",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath(string.Format("{0}.{1}", nameof(Entity.FormattedValues), Workflow.Schema.Attributes.category)),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "Name",
                    Width = new DataGridLength(200),
                    Binding = new Binding
                    {
                        Path = new PropertyPath(nameof(Workflow.Name)),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "Mode",
                    Width = new DataGridLength(150),
                    Binding = new Binding
                    {
                        Path = new PropertyPath(string.Format("{0}.{1}", nameof(Entity.FormattedValues), Workflow.Schema.Attributes.mode)),
                        Mode = BindingMode.OneTime,
                    },
                },

                new DataGridTextColumn()
                {
                    Header = "Status",
                    Width = new DataGridLength(150),
                    Binding = new Binding
                    {
                        Path = new PropertyPath(string.Format("{0}.{1}", nameof(Entity.FormattedValues), Workflow.Schema.Attributes.statuscode)),
                        Mode = BindingMode.OneTime,
                    },
                },
            };
        }
    }
}
