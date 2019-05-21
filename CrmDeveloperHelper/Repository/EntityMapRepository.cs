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
    public class EntityMapRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public EntityMapRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<EntityMap>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        private List<EntityMap> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = EntityMap.EntityLogicalName,

                NoLock = true,

                ColumnSet = new ColumnSet(true),

                Orders =
                {
                    new OrderExpression(EntityMap.Schema.Attributes.sourceentityname, OrderType.Ascending),
                    new OrderExpression(EntityMap.Schema.Attributes.targetentityname, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<EntityMap>(query);
        }

        public Task<List<EntityMap>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<EntityMap> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = EntityMap.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(EntityMap.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(EntityMap.Schema.Attributes.sourceentityname, OrderType.Ascending),
                    new OrderExpression(EntityMap.Schema.Attributes.targetentityname, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<EntityMap>(query);
        }

        public Task<List<EntityMap>> GetListForEntitiesAsync(string[] entities, ColumnSet columnSet)
        {
            return Task.Run(() => GetListForEntities(entities, columnSet));
        }

        private List<EntityMap> GetListForEntities(string[] entities, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = EntityMap.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(EntityMap.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)ComponentState.Published
                            , (int)ComponentState.Unpublished
                        ),
                    },

                    Filters =
                    {
                        new FilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                            {
                                new ConditionExpression(EntityMap.Schema.Attributes.sourceentityname, ConditionOperator.In, entities),
                                new ConditionExpression(EntityMap.Schema.Attributes.targetentityname, ConditionOperator.In, entities),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(EntityMap.Schema.Attributes.sourceentityname, OrderType.Ascending),
                    new OrderExpression(EntityMap.Schema.Attributes.targetentityname, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<EntityMap>(query);
        }
    }
}
