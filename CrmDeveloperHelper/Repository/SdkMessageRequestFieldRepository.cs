using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    class SdkMessageRequestFieldRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageRequestFieldRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageRequestField>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SdkMessageRequestField> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageRequestField.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageRequestField>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageRequestField>()));

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

        public Task<SdkMessageRequestField> GetByIdAsync(Guid idSdkMessageRequestField, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSdkMessageRequestField, columnSet));
        }

        private SdkMessageRequestField GetById(Guid idSdkMessageRequestField, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageRequestField.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageRequestField.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSdkMessageRequestField),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageRequestField>()).SingleOrDefault() : null;
        }

        public Task<List<SdkMessageRequestField>> GetListByRequestIdAsync(Guid idRequest, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByRequestId(idRequest, columnSet));
        }

        private List<SdkMessageRequestField> GetListByRequestId(Guid idRequest, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageRequestField.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid, ConditionOperator.Equal, idRequest),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageRequestField>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageRequestField>()));

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

        public Task<List<SdkMessageRequestField>> GetListByPairIdAsync(Guid idPair, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByPairId(idPair, columnSet));
        }

        private List<SdkMessageRequestField> GetListByPairId(Guid idPair, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageRequestField.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageRequestField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid,

                        LinkToEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkToAttributeName = SdkMessageRequest.EntityPrimaryIdAttribute,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SdkMessageRequest.Schema.Attributes.sdkmessagepairid, ConditionOperator.Equal, idPair),
                            },
                        },
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageRequestField>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageRequestField>()));

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
