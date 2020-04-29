using Microsoft.Xrm.Sdk;
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

                ColumnSet = new ColumnSet(true),

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

                ColumnSet = new ColumnSet(true),

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
                EntityName = SdkMessage.EntityLogicalName,
                ColumnSet = new ColumnSet(true),

                NoLock = true,

                Distinct = true,

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

                ColumnSet = columnSet ?? new ColumnSet(true),

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
    }
}
