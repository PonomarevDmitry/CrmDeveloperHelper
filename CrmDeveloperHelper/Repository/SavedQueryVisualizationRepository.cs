using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SavedQueryVisualizationRepository : IEntitySaver
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SavedQueryVisualizationRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SavedQueryVisualization>> GetListAsync(string filterEntity = null, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetList(filterEntity, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SavedQueryVisualization> GetList(string filterEntity, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SavedQueryVisualization.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQueryVisualization.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, OrderType.Ascending),
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, ConditionOperator.Equal, filterEntity));
            }

            return _service.RetrieveMultipleAll<SavedQueryVisualization>(query);
        }

        public Task<SavedQueryVisualization> GetByIdAsync(Guid idChart, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idChart, columnSet));
        }

        public SavedQueryVisualization GetById(Guid idChart, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SavedQueryVisualization.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQueryVisualization.EntityPrimaryIdAttribute, ConditionOperator.Equal, idChart),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SavedQueryVisualization>()).SingleOrDefault() : null;
        }

        public Task<List<SavedQueryVisualization>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<SavedQueryVisualization> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SavedQueryVisualization.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQueryVisualization.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, OrderType.Ascending),
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<SavedQueryVisualization>(query);
        }

        public Task<List<SavedQueryVisualization>> GetListForEntitiesAsync(string[] entities, ColumnSet columnSet)
        {
            return Task.Run(() => GetListForEntities(entities, columnSet));
        }

        private List<SavedQueryVisualization> GetListForEntities(string[] entities, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SavedQueryVisualization.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQueryVisualization.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                        new ConditionExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, ConditionOperator.In, entities),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, OrderType.Ascending),
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<SavedQueryVisualization>(query);
        }

        public async Task<Guid> UpsertAsync(Entity entity, Action<string> updateStatus)
        {
            var idEntity = await _service.UpsertAsync(entity);

            var chart = await GetByIdAsync(idEntity, new ColumnSet(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode));

            if (!string.IsNullOrEmpty(chart.PrimaryEntityTypeCode)
                && !string.Equals(chart.PrimaryEntityTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                updateStatus(string.Format(Properties.WindowStatusStrings.PublishingEntitiesFormat2, _service.ConnectionData.Name, chart.PrimaryEntityTypeCode));

                var repositoryPublish = new PublishActionsRepository(_service);

                await repositoryPublish.PublishEntitiesAsync(new[] { chart.PrimaryEntityTypeCode });

                updateStatus(string.Format(Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, _service.ConnectionData.Name, chart.PrimaryEntityTypeCode));
            }

            return idEntity;
        }
    }
}
