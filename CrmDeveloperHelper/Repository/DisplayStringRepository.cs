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
    public class DisplayStringRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public DisplayStringRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<DisplayString>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<DisplayString> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = DisplayString.EntityLogicalName,

                NoLock = true,

                ColumnSet = ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(DisplayString.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                    },
                },

                Orders =
                {
                    new OrderExpression(DisplayString.Schema.Attributes.displaystringkey, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<DisplayString>(query);
        }

        public DisplayString GetByKeyAndLanguage(string key, int langCode, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = DisplayString.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(DisplayString.Schema.Attributes.displaystringkey, ConditionOperator.Equal, key),
                        new ConditionExpression(DisplayString.Schema.Attributes.languagecode, ConditionOperator.Equal, langCode),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<DisplayString>()).SingleOrDefault() : null;
        }

        public Task<List<DisplayString>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<DisplayString> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = DisplayString.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(DisplayString.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(DisplayString.Schema.Attributes.displaystringkey, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<DisplayString>(query);
        }

        public Task<List<DisplayString>> GetListForEntitiesAsync(string[] entities, ColumnSet columnSet)
        {
            return Task.Run(() => GetListForEntities(entities, columnSet));
        }

        private List<DisplayString> GetListForEntities(string[] entities, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = DisplayString.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(DisplayString.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = DisplayString.EntityLogicalName,
                        LinkFromAttributeName = DisplayString.EntityPrimaryIdAttribute,

                        LinkToEntityName = DisplayStringMap.EntityLogicalName,
                        LinkToAttributeName = DisplayStringMap.Schema.Attributes.displaystringid,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(DisplayStringMap.Schema.Attributes.objecttypecode, ConditionOperator.In, entities),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(DisplayString.Schema.Attributes.displaystringkey, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<DisplayString>(query);
        }
    }
}
