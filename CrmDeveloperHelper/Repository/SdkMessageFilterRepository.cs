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

                EntityName = SdkMessageFilter.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageFilter.EntityPrimaryIdAttribute,

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
            };

            return _service.RetrieveMultipleAll<SdkMessageFilter>(query);
        }

        public Task<List<SdkMessageFilter>> GetAllAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetAll(columnSet));
        }

        private List<SdkMessageFilter> GetAll(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageFilter.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),
            };

            return _service.RetrieveMultipleAll<SdkMessageFilter>(query);
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
                        new ConditionExpression(SdkMessageFilter.EntityPrimaryIdAttribute, ConditionOperator.In, ids),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                        LinkToEntityName = SdkMessage.EntityLogicalName,
                        LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                        Columns = new ColumnSet(true),

                        EntityAlias = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                        JoinOperator = JoinOperator.LeftOuter,
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessageFilter>(query);
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
                        LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

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

        public Task<List<SdkMessageFilter>> GetListByMessageAsync(Guid idMessage, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByMessage(idMessage, columnSet));
        }

        private List<SdkMessageFilter> GetListByMessage(Guid idMessage, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageFilter.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageFilter.Schema.Attributes.sdkmessageid, ConditionOperator.Equal, idMessage),
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessageFilter>(query);
        }

        public Task<List<SdkMessageFilter>> GetAllSdkMessageFiltersWithMessageAsync(string messageName, string entityName, ColumnSet columnSet)
        {
            return Task.Run(() => GetAllSdkMessageFiltersWithMessage(messageName, entityName, columnSet));
        }

        private List<SdkMessageFilter> GetAllSdkMessageFiltersWithMessage(string messageName, string entityName, ColumnSet columnSet)
        {
            var linkMessage = new LinkEntity()
            {
                LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                LinkToEntityName = SdkMessage.EntityLogicalName,
                LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                EntityAlias = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                Columns = new ColumnSet(SdkMessage.EntityPrimaryIdAttribute, SdkMessage.Schema.Attributes.name, SdkMessage.Schema.Attributes.categoryname),
            };

            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageFilter.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(false),

                LinkEntities =
                {
                    linkMessage,
                },
            };

            if (!string.IsNullOrEmpty(messageName))
            {
                linkMessage.LinkCriteria.Conditions.Add(new ConditionExpression(SdkMessage.Schema.Attributes.name, ConditionOperator.Like, messageName + "%"));
            }

            var result = _service.RetrieveMultipleAll<SdkMessageFilter>(query);

            if (!string.IsNullOrEmpty(entityName))
            {
                result = result.Where(ent => ent.PrimaryObjectTypeCode.IndexOf(entityName, StringComparison.InvariantCultureIgnoreCase) > -1).ToList();
            }

            return result;
        }
    }
}
