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
    public class SdkMessageFilterRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageFilterRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageFilter>> GetAllSdkMessageFilterWithStepsAsync()
        {
            return Task.Run(() => GetAllSdkMessageFilterWithSteps());
        }

        private List<SdkMessageFilter> GetAllSdkMessageFilterWithSteps()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = SdkMessageFilter.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageFilter.PrimaryIdAttribute,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.In
                                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10
                                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20
                                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40
                                ),
                                new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
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

            var result = new List<SdkMessageFilter>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageFilter>()));

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

            return result;
        }

        public Task<EntityReference> FindFilterAsync(Guid idMessage, string primaryEntity, string secondaryEntity)
        {
            return Task.Run(() => FindFilter(idMessage, primaryEntity, secondaryEntity));
        }

        private EntityReference FindFilter(Guid idMessage, string primaryEntity, string secondaryEntity)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageFilter.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageFilter.Schema.Attributes.sdkmessageid, ConditionOperator.Equal, idMessage),
                        new ConditionExpression(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, ConditionOperator.Equal, primaryEntity),
                        new ConditionExpression(SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode, ConditionOperator.Equal, secondaryEntity),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntityReference()).SingleOrDefault() : null;
        }

        public Task<List<SdkMessageFilter>> GetMessageFiltersByIdsAsync(Guid[] ids)
        {
            return Task.Run(() => GetMessageFiltersByIds(ids));
        }

        private List<SdkMessageFilter> GetMessageFiltersByIds(Guid[] ids)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageFilter.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageFilter.PrimaryIdAttribute, ConditionOperator.In, ids),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                        LinkToEntityName = SdkMessage.EntityLogicalName,
                        LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                        Columns = new ColumnSet(true),

                        EntityAlias = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                        JoinOperator = JoinOperator.LeftOuter,
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageFilter>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageFilter>()));

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

            return result;
        }

        public Task<SdkMessageFilter> FindByEntityAndMessageAsync(string entityName, string messageName, ColumnSet columnSet)
        {
            return Task.Run(() => FindByEntityAndMessage(entityName, messageName, columnSet));
        }

        public SdkMessageFilter FindByEntityAndMessage(string entityName, string messageName, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageFilter.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, ConditionOperator.Equal, entityName),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                        LinkToEntityName = SdkMessage.EntityLogicalName,
                        LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SdkMessage.Schema.Attributes.name, ConditionOperator.Equal, messageName),
                            },
                        },
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageFilter>()).SingleOrDefault() : null;
        }
    }
}
