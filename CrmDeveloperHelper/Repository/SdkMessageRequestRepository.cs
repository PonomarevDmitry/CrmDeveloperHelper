﻿using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SdkMessageRequestRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageRequestRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageRequest>> GetListAsync(string filterEntity, string messageName, string endpointName, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filterEntity, messageName, endpointName, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SdkMessageRequest> GetList(string filterEntity, string messageName, string endpointName, ColumnSet columnSet)
        {
            var linkEntityMessage = new LinkEntity()
            {
                LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                LinkToEntityName = SdkMessage.EntityLogicalName,
                LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                Columns = new ColumnSet(SdkMessage.Schema.Attributes.name, SdkMessage.Schema.Attributes.categoryname),
            };

            var linkEntityPair = new LinkEntity()
            {
                LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                LinkToEntityName = SdkMessagePair.EntityLogicalName,
                LinkToAttributeName = SdkMessagePair.EntityPrimaryIdAttribute,

                EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                Columns = new ColumnSet(SdkMessagePair.Schema.Attributes.@namespace, SdkMessagePair.Schema.Attributes.endpoint, SdkMessagePair.Schema.Attributes.sdkmessageid),

                LinkEntities =
                {
                    linkEntityMessage,
                },
            };

            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageRequest.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                LinkEntities =
                {
                    linkEntityPair,
                },
            };

            if (!string.IsNullOrEmpty(messageName))
            {
                linkEntityMessage.LinkCriteria.Conditions.Add(new ConditionExpression(SdkMessage.Schema.Attributes.name, ConditionOperator.Like, messageName + "%"));
            }

            if (!string.IsNullOrEmpty(endpointName))
            {
                linkEntityPair.LinkCriteria.Conditions.Add(new ConditionExpression(SdkMessagePair.Schema.Attributes.endpoint, ConditionOperator.Like, endpointName + "%"));
            }

            var result = _service.RetrieveMultipleAll<SdkMessageRequest>(query).AsEnumerable();

            if (!string.IsNullOrEmpty(filterEntity))
            {
                filterEntity = filterEntity.ToLower();

                result = result.Where(ent => ent.PrimaryObjectTypeCode != null && ent.PrimaryObjectTypeCode.IndexOf(filterEntity, StringComparison.InvariantCultureIgnoreCase) > -1);
            }

            return result.ToList();
        }

        public Task<SdkMessageRequest> GetByIdAsync(Guid idSdkMessageRequest, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSdkMessageRequest, columnSet));
        }

        private SdkMessageRequest GetById(Guid idSdkMessageRequest, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageRequest.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageRequest.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSdkMessageRequest),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        LinkToEntityName = SdkMessagePair.EntityLogicalName,
                        LinkToAttributeName = SdkMessagePair.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        Columns = ColumnSetInstances.AllColumns,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                LinkToEntityName = SdkMessage.EntityLogicalName,
                                LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                                EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                                Columns = ColumnSetInstances.AllColumns,
                            },
                        },
                    }
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageRequest>()).SingleOrDefault() : null;
        }

        public Task<SdkMessageRequest> FindByRequestNameAsync(string requestName, ColumnSet columnSet)
        {
            return Task.Run(() => FindByRequestName(requestName, columnSet));
        }

        public SdkMessageRequest FindByRequestName(string requestName, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageRequest.EntityLogicalName,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageRequest.Schema.Attributes.name, ConditionOperator.Equal, requestName),
                    },
                },

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageRequest>()).SingleOrDefault() : null;
        }

        public Task<List<SdkMessageRequest>> GetListByPairAsync(Guid idPair, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByPair(idPair, columnSet));
        }

        private List<SdkMessageRequest> GetListByPair(Guid idPair, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageRequest.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageRequest.Schema.Attributes.sdkmessagepairid, ConditionOperator.Equal, idPair),
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessageRequest>(query);
        }

        public Task<List<SdkMessageRequest>> GetListByMessageAsync(Guid idMessage, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByMessage(idMessage, columnSet));
        }

        private List<SdkMessageRequest> GetListByMessage(Guid idMessage, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageRequest.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

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
            };

            return _service.RetrieveMultipleAll<SdkMessageRequest>(query);
        }
    }
}