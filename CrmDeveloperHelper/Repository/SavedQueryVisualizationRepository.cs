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
    public class SavedQueryVisualizationRepository
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
                            , (int)ComponentState.Published
                            , (int)ComponentState.Unpublished
                        ),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, OrderType.Ascending),
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, ConditionOperator.Equal, filterEntity));
            }

            var result = new List<SavedQueryVisualization>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SavedQueryVisualization>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
            }

            return result;
        }

        public Task<SavedQueryVisualization> GetByIdAsync(Guid idSavedQueryVisualization, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSavedQueryVisualization, columnSet));
        }

        public SavedQueryVisualization GetById(Guid idSavedQueryVisualization, ColumnSet columnSet)
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
                        new ConditionExpression(SavedQueryVisualization.PrimaryIdAttribute, ConditionOperator.Equal, idSavedQueryVisualization),
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SavedQueryVisualization>()).SingleOrDefault();
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

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SavedQueryVisualization>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SavedQueryVisualization>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
            }

            return result;
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
                            , (int)ComponentState.Published
                            , (int)ComponentState.Unpublished
                        ),
                        new ConditionExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, ConditionOperator.In, entities),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode, OrderType.Ascending),
                    new OrderExpression(SavedQueryVisualization.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SavedQueryVisualization>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SavedQueryVisualization>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
            }

            return result;
        }
    }
}
