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
                EntityName = SdkMessage.EntityLogicalName,
                ColumnSet = new ColumnSet(true),

                NoLock = true,

                Distinct = true,

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
                        LinkFromAttributeName = SdkMessage.PrimaryIdAttribute,

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

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessage>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessage>()));

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
                        new ConditionExpression(SdkMessage.PrimaryIdAttribute, ConditionOperator.In, ids),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessage>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessage>()));

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

        public Task<List<SdkMessage>> GetAllSdkMessageWithFiltersAsync(string messageName, string entityName)
        {
            return Task.Run(() => GetAllSdkMessageWithFilters(messageName, entityName));
        }

        private List<SdkMessage> GetAllSdkMessageWithFilters(string messageName, string entityName)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessage.EntityLogicalName,

                ColumnSet = new ColumnSet(SdkMessage.PrimaryIdAttribute, SdkMessage.Schema.Attributes.name),

                Distinct = true,

                //Criteria =
                //{
                //    Conditions =
                //    {
                //        new ConditionExpression(SdkMessage.Schema.Attributes.isprivate, ConditionOperator.Equal, false),
                //    },
                //},

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessage.PrimaryIdAttribute,

                        LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkToAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                        EntityAlias = SdkMessageFilter.Schema.EntityPrimaryIdAttribute,

                        Columns = new ColumnSet(SdkMessageFilter.Schema.EntityPrimaryIdAttribute, SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode),

                        //LinkCriteria =
                        //{
                        //    Conditions =
                        //    {
                        //        new ConditionExpression(SdkMessageFilter.Schema.Attributes.iscustomprocessingstepallowed, ConditionOperator.Equal, true),
                        //        new ConditionExpression(SdkMessageFilter.Schema.Attributes.isvisible, ConditionOperator.Equal, true),
                        //    },
                        //},
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(messageName))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SdkMessage.Schema.Attributes.name, ConditionOperator.Like, messageName + "%"));
            }

            var result = new List<SdkMessage>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessage>()));

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

            SdkMessageProcessingStepRepository.FullfillEntitiesSteps(result);

            if (!string.IsNullOrEmpty(entityName))
            {
                result = result.Where(ent => ent.PrimaryObjectTypeCodeName.IndexOf(entityName, StringComparison.InvariantCultureIgnoreCase) > -1).ToList();
            }

            return result;
        }
    }
}
