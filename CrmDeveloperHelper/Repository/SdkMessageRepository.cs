﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SdkMessageRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessage>> GetAllSdkMessageWithStepsAsync()
        {
            return Task.Run(() => GetAllSdkMessageWithSteps());
        }

        private List<SdkMessage> GetAllSdkMessageWithSteps()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = SdkMessage.EntityLogicalName,

                ColumnSet = ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessage.Schema.Attributes.isprivate, ConditionOperator.Equal, false),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

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

            return _service.RetrieveMultipleAll<SdkMessage>(query);
        }

        public Task<SdkMessage> FindMessageAsync(string name)
        {
            return Task.Run(() => FindMessage(name));
        }

        private SdkMessage FindMessage(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                ColumnSet = ColumnSetInstances.AllColumns,

                EntityName = SdkMessage.EntityLogicalName,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessage.Schema.Attributes.name, ConditionOperator.Equal, name),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessage>()).SingleOrDefault() : null;
        }

        public Task<List<SdkMessage>> GetMessageByIdsAsync(Guid[] ids)
        {
            return Task.Run(() => GetMessageByIds(ids));
        }

        private List<SdkMessage> GetMessageByIds(Guid[] ids)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessage.EntityLogicalName,

                ColumnSet = ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessage.EntityPrimaryIdAttribute, ConditionOperator.In, ids),
                    },
                },
            };

            return _service.RetrieveMultipleAll<SdkMessage>(query);
        }

        public Task<SdkMessage> GetByIdAsync(Guid idSdkMessage, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSdkMessage, columnSet));
        }

        private SdkMessage GetById(Guid idSdkMessage, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessage.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessage.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSdkMessage),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessage>()).SingleOrDefault() : null;
        }

        public Task<List<SdkMessage>> GetMessagesAsync(string name, ColumnSet columnSet)
        {
            return Task.Run(() => GetMessages(name, columnSet));
        }

        private List<SdkMessage> GetMessages(string name, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                ColumnSet = columnSet ?? ColumnSetInstances.None,

                EntityName = SdkMessage.EntityLogicalName,

                Orders =
                {
                    new OrderExpression(SdkMessage.Schema.Attributes.categoryname, OrderType.Ascending),
                    new OrderExpression(SdkMessage.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(SdkMessage.Schema.Attributes.createdon, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                if (Guid.TryParse(name, out var id))
                {
                    query.Criteria.Filters.Add(new FilterExpression()
                    {
                        FilterOperator = LogicalOperator.Or,
                        Conditions =
                        {
                            new ConditionExpression(SdkMessage.Schema.Attributes.sdkmessageid, ConditionOperator.Equal, id),
                            new ConditionExpression(SdkMessage.Schema.Attributes.sdkmessageidunique, ConditionOperator.Equal, id),
                        },
                    });
                }
                else
                {
                    query.Criteria.AddCondition(SdkMessage.Schema.Attributes.name, ConditionOperator.Like, name + "%");
                }
            }

            return _service.RetrieveMultipleAll<SdkMessage>(query);
        }
    }
}
