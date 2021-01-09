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
                NoLock = true,

                EntityName = SdkMessageRequestField.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,
            };

            return _service.RetrieveMultipleAll<SdkMessageRequestField>(query);
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

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

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

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid, ConditionOperator.Equal, idRequest),
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessageRequestField>(query);
        }

        public Task<List<SdkMessageRequestField>> GetListByPairAsync(Guid idPair, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByPair(idPair, columnSet));
        }

        private List<SdkMessageRequestField> GetListByPair(Guid idPair, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageRequestField.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

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
            };

            return _service.RetrieveMultipleAll<SdkMessageRequestField>(query);
        }

        public Task<List<SdkMessageRequestField>> GetListByMessageAsync(Guid idMessage, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByMessage(idMessage, columnSet));
        }

        private List<SdkMessageRequestField> GetListByMessage(Guid idMessage, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageRequestField.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageRequestField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid,

                        LinkToEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkToAttributeName = SdkMessageRequest.EntityPrimaryIdAttribute,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                                LinkToEntityName = SdkMessagePair.EntityLogicalName,
                                LinkToAttributeName = SdkMessagePair.EntityPrimaryIdAttribute,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(SdkMessagePair.Schema.Attributes.sdkmessageid, ConditionOperator.Equal, idMessage),
                                    },
                                },
                            }
                        },
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessageRequestField>(query);
        }
    }
}
