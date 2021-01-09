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
    public class AttributeMapRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public AttributeMapRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<AttributeMap>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        private List<AttributeMap> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = AttributeMap.EntityLogicalName,

                NoLock = true,

                ColumnSet = ColumnSetInstances.AllColumns,

                Orders =
                {
                    new OrderExpression(AttributeMap.Schema.Attributes.sourceattributename, OrderType.Ascending),
                    new OrderExpression(AttributeMap.Schema.Attributes.targetattributename, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<AttributeMap>(query);
        }

        public Task<List<AttributeMap>> GetListWithEntityMapAsync(string entityName)
        {
            return Task.Run(() => GetListWithEntityMap(entityName));
        }

        private List<AttributeMap> GetListWithEntityMap(string entityName)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = AttributeMap.EntityLogicalName,

                NoLock = true,

                ColumnSet = ColumnSetInstances.AllColumns,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = AttributeMap.EntityLogicalName,
                        LinkFromAttributeName = AttributeMap.Schema.Attributes.entitymapid,

                        LinkToEntityName = EntityMap.EntityLogicalName,
                        LinkToAttributeName = EntityMap.EntityPrimaryIdAttribute,

                        EntityAlias = AttributeMap.Schema.Attributes.entitymapid,

                        Columns = new ColumnSet(EntityMap.Schema.Attributes.sourceentityname, EntityMap.Schema.Attributes.targetentityname),

                        LinkCriteria = new FilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                            {
                                new ConditionExpression(EntityMap.Schema.Attributes.sourceentityname, ConditionOperator.Equal, entityName),
                                new ConditionExpression(EntityMap.Schema.Attributes.targetentityname, ConditionOperator.Equal, entityName),
                            },
                        },
                    },
                },
            };

            return _service.RetrieveMultipleAll<AttributeMap>(query);
        }
    }
}
