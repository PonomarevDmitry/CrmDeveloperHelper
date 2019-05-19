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
    public class SdkMessageResponseRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageResponseRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageResponse>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        private List<SdkMessageResponse> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageResponse.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageResponse>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageResponse>()));

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

        public Task<List<SdkMessageResponse>> GetListByRequestAsync(Guid idRequest, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByRequest(idRequest, columnSet));
        }

        private List<SdkMessageResponse> GetListByRequest(Guid idRequest, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageResponse.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageResponse.Schema.Attributes.sdkmessagerequestid, ConditionOperator.Equal, idRequest),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageResponse>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageResponse>()));

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

        public Task<List<SdkMessageResponse>> GetListByPairAsync(Guid idPair, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByPair(idPair, columnSet));
        }

        private List<SdkMessageResponse> GetListByPair(Guid idPair, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageResponse.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageResponse.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid,

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

            var result = new List<SdkMessageResponse>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageResponse>()));

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

        public Task<SdkMessageResponse> GetByIdAsync(Guid idSdkMessageResponse, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSdkMessageResponse, columnSet));
        }

        private SdkMessageResponse GetById(Guid idSdkMessageResponse, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageResponse.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageResponse.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSdkMessageResponse),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageResponse>()).SingleOrDefault() : null;
        }
    }
}
