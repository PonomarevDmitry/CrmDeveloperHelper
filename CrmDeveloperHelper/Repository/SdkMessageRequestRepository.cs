using Microsoft.Xrm.Sdk.Query;
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
        private IOrganizationServiceExtented _Service { get; set; }

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageRequestRepository(IOrganizationServiceExtented service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageRequest>> GetListAsync(string filterEntity, string messageName, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filterEntity, messageName, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SdkMessageRequest> GetList(string filterEntity, string messageName, ColumnSet columnSet)
        {
            LinkEntity linkEntityMessage = new LinkEntity()
            {
                LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                LinkToEntityName = SdkMessage.EntityLogicalName,
                LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                Columns = new ColumnSet(SdkMessage.Schema.Attributes.name),
            };

            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageRequest.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        LinkToEntityName = SdkMessagePair.EntityLogicalName,
                        LinkToAttributeName = SdkMessagePair.PrimaryIdAttribute,

                        EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        Columns = new ColumnSet(SdkMessagePair.Schema.Attributes.@namespace, SdkMessagePair.Schema.Attributes.endpoint),

                        LinkEntities =
                        {
                            linkEntityMessage,
                        },
                    }
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(messageName))
            {
                linkEntityMessage.LinkCriteria.Conditions.Add(new ConditionExpression(SdkMessage.Schema.Attributes.name, ConditionOperator.Like, messageName + "%"));
            }

            var result = new List<SdkMessageRequest>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageRequest>()));

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

            if (!string.IsNullOrEmpty(filterEntity))
            {
                filterEntity = filterEntity.ToLower();

                result = result.Where(ent => ent.PrimaryObjectTypeCode != null && ent.PrimaryObjectTypeCode.ToLower().Contains(filterEntity)).ToList();
            }

            return result;
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

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageRequest.PrimaryIdAttribute, ConditionOperator.Equal, idSdkMessageRequest),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageRequest.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        LinkToEntityName = SdkMessagePair.EntityLogicalName,
                        LinkToAttributeName = SdkMessagePair.PrimaryIdAttribute,

                        EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid,

                        Columns = new ColumnSet(true),

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessagePair.EntityLogicalName,
                                LinkFromAttributeName = SdkMessagePair.Schema.Attributes.sdkmessageid,

                                LinkToEntityName = SdkMessage.EntityLogicalName,
                                LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                                EntityAlias = SdkMessageRequest.Schema.Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid,

                                Columns = new ColumnSet(true),
                            },
                        },
                    }
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SdkMessageRequest>()).SingleOrDefault();
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

                ColumnSet = columnSet ?? new ColumnSet(true),
            };

            var coll = _Service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageRequest>()).SingleOrDefault() : null;
        }
    }
}