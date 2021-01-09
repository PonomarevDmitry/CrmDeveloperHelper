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
    class SdkMessageResponseFieldRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageResponseFieldRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageResponseField>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SdkMessageResponseField> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageResponseField.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,
            };

            return _service.RetrieveMultipleAll<SdkMessageResponseField>(query);
        }

        public Task<SdkMessageResponseField> GetByIdAsync(Guid idSdkMessageResponseField, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSdkMessageResponseField, columnSet));
        }

        private SdkMessageResponseField GetById(Guid idSdkMessageResponseField, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageResponseField.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageResponseField.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSdkMessageResponseField),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageResponseField>()).SingleOrDefault() : null;
        }

        public Task<List<SdkMessageResponseField>> GetListByResponseIdAsync(Guid idResponse, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByResponseId(idResponse, columnSet));
        }

        private List<SdkMessageResponseField> GetListByResponseId(Guid idResponse, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageResponseField.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageResponseField.Schema.Attributes.sdkmessageresponseid, ConditionOperator.Equal, idResponse),
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessageResponseField>(query);
        }

        public Task<List<SdkMessageResponseField>> GetListByPairAsync(Guid idPair, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByPair(idPair, columnSet));
        }

        private List<SdkMessageResponseField> GetListByPair(Guid idPair, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageResponseField.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageResponseField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponseField.Schema.Attributes.sdkmessageresponseid,

                        LinkToEntityName = SdkMessageResponse.EntityLogicalName,
                        LinkToAttributeName = SdkMessageResponse.EntityPrimaryIdAttribute,

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
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessageResponseField>(query);
        }

        public Task<List<SdkMessageResponseField>> GetListByMessageAsync(Guid idMessage, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByMessage(idMessage, columnSet));
        }

        private List<SdkMessageResponseField> GetListByMessage(Guid idMessage, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageResponseField.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageResponseField.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageResponseField.Schema.Attributes.sdkmessageresponseid,

                        LinkToEntityName = SdkMessageResponse.EntityLogicalName,
                        LinkToAttributeName = SdkMessageResponse.EntityPrimaryIdAttribute,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageResponse.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageResponse.Schema.Attributes.sdkmessagerequestid,

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
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessageResponseField>(query);
        }
    }
}
