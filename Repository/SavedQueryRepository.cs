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
    /// <summary>
    /// Репозиторий для работы с SavedQuery
    /// </summary>
    public class SavedQueryRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SavedQueryRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SavedQuery>> GetListAsync(string filterEntity, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filterEntity, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SavedQuery> GetList(string filterEntity, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SavedQuery.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQuery.Schema.Attributes.componentstate, ConditionOperator.In, 0, 1),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQuery.Schema.Attributes.returnedtypecode, OrderType.Ascending),
                    new OrderExpression(SavedQuery.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SavedQuery.Schema.Attributes.returnedtypecode, ConditionOperator.Equal, filterEntity));
            }

            var result = new List<SavedQuery>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SavedQuery>()));

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            result = result.Where(ent =>
                !ent.Contains(SavedQuery.Schema.Attributes.statecode) || (ent.Contains(SavedQuery.Schema.Attributes.statecode) && ent.GetAttributeValue<OptionSetValue>(SavedQuery.Schema.Attributes.statecode).Value == 0)
                ).ToList();

            return result;
        }

        public Task<List<SavedQuery>> GetListCustomableAsync(string filterEntity = null)
        {
            return Task.Run(() => GetListCustomable(filterEntity));
        }

        private List<SavedQuery> GetListCustomable(string filterEntity = null)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SavedQuery.EntityLogicalName,

                NoLock = true,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQuery.Schema.Attributes.componentstate, ConditionOperator.In, 0, 1),
                        new ConditionExpression(SavedQuery.Schema.Attributes.querytype, ConditionOperator.In, 0, 1, 2, 4, 64),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQuery.Schema.Attributes.returnedtypecode, OrderType.Ascending),
                    new OrderExpression(SavedQuery.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SavedQuery.Schema.Attributes.returnedtypecode, ConditionOperator.Equal, filterEntity));
            }

            var list = this._service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SavedQuery>()).ToList();

            list = list.Where(ent =>
                !ent.Contains(SavedQuery.Schema.Attributes.statecode) || (ent.Contains(SavedQuery.Schema.Attributes.statecode) && ent.GetAttributeValue<OptionSetValue>(SavedQuery.Schema.Attributes.statecode).Value == 0)
                ).ToList();

            return list;
        }

        internal static string GetQueryTypeName(int queryType)
        {
            string result = queryType.ToString();

            switch (queryType)
            {
                case 0:
                    result = "Public";
                    break;

                case 1:
                    result = "Advanced Find";
                    break;

                case 2:
                    result = "Associated";
                    break;

                case 4:
                    result = "Quick Find";
                    break;

                case 64:
                    result = "Lookup";
                    break;

                default:
                    break;
            }

            return result;
        }

        public Task<SavedQuery> GetByIdAsync(Guid idSavedQuery, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSavedQuery, columnSet));
        }

        private SavedQuery GetById(Guid idSavedQuery, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SavedQuery.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQuery.PrimaryIdAttribute, ConditionOperator.Equal, idSavedQuery),
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SavedQuery>()).FirstOrDefault();
        }
    }
}
