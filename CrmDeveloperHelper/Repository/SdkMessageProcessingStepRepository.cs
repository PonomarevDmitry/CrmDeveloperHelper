using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SdkMessageProcessingStepRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageProcessingStepRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageProcessingStep>> FindSdkMessageProcessingStepWithEntityNameAsync(string entityName, List<PluginStage> listStages, string pluginName, string messageName, SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode)
        {
            return Task.Run(() => FindSdkMessageProcessingStepWithEntityName(entityName, listStages, pluginName, messageName, statuscode));
        }

        private List<SdkMessageProcessingStep> FindSdkMessageProcessingStepWithEntityName(string entityName, List<PluginStage> listStages, string pluginName, string messageName, SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode)
        {
            var listSteps = FindSdkMessageProcessingStep(listStages, pluginName, messageName, statuscode);

            if (string.IsNullOrEmpty(entityName))
            {
                return listSteps;
            }
            else
            {
                return listSteps.Where(ent => ent.PrimaryObjectTypeCodeName.IndexOf(entityName, StringComparison.InvariantCultureIgnoreCase) > -1).ToList();
            }
        }

        public Task<List<SdkMessageProcessingStep>> FindSdkMessageProcessingStepAsync(List<PluginStage> listStages, string pluginName, string messageName, SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode)
        {
            return Task.Run(() => FindSdkMessageProcessingStep(listStages, pluginName, messageName, statuscode));
        }

        private List<SdkMessageProcessingStep> FindSdkMessageProcessingStep(List<PluginStage> listStages, string pluginName, string messageName, SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode)
        {
            var linkPluginType = new LinkEntity()
            {
                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                LinkToEntityName = PluginType.EntityLogicalName,
                LinkToAttributeName = PluginType.EntityPrimaryIdAttribute,

                EntityAlias = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                Columns = new ColumnSet(PluginType.Schema.Attributes.typename, PluginType.Schema.Attributes.assemblyname, PluginType.Schema.Attributes.pluginassemblyid),
            };

            var linkMessage = new LinkEntity()
            {
                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                LinkToEntityName = SdkMessage.EntityLogicalName,
                LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                Columns = new ColumnSet(SdkMessage.Schema.Attributes.categoryname),
            };

            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageProcessingStep.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                    },
                },

                LinkEntities =
                {
                    linkPluginType,

                    linkMessage,

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkToAttributeName = SdkMessageFilter.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        JoinOperator = JoinOperator.LeftOuter,

                        Columns = new ColumnSet(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode),
                    },
                },
            };

            if (statuscode.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.statuscode, ConditionOperator.Equal, (int)statuscode.Value));
            }

            if (!string.IsNullOrEmpty(pluginName))
            {
                linkPluginType.LinkCriteria.Conditions.Add(new ConditionExpression(PluginType.Schema.Attributes.typename, ConditionOperator.Like, "%" + pluginName + "%"));
            }

            if (!string.IsNullOrEmpty(messageName))
            {
                linkMessage.LinkCriteria.Conditions.Add(new ConditionExpression(SdkMessage.Schema.Attributes.name, ConditionOperator.Like, messageName + "%"));
            }

            if (listStages != null && listStages.Count > 0)
            {
                FilterExpression filter = new FilterExpression(LogicalOperator.Or);

                if (listStages.Contains(PluginStage.PreValidation))
                {
                    var temp = new FilterExpression(LogicalOperator.And);

                    temp.AddCondition(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.Equal, (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10);

                    filter.AddFilter(temp);
                }

                if (listStages.Contains(PluginStage.Pre))
                {
                    var temp = new FilterExpression(LogicalOperator.And);

                    temp.AddCondition(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.Equal, (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20);

                    filter.AddFilter(temp);
                }

                if (listStages.Contains(PluginStage.PostSynch))
                {
                    var temp = new FilterExpression(LogicalOperator.And);

                    temp.AddCondition(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.Equal, (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40);
                    temp.AddCondition(SdkMessageProcessingStep.Schema.Attributes.mode, ConditionOperator.Equal, (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Synchronous_0);

                    filter.AddFilter(temp);
                }

                if (listStages.Contains(PluginStage.PostAsych))
                {
                    var temp = new FilterExpression(LogicalOperator.And);

                    temp.AddCondition(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.Equal, (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40);
                    temp.AddCondition(SdkMessageProcessingStep.Schema.Attributes.mode, ConditionOperator.Equal, (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1);

                    filter.AddFilter(temp);
                }

                query.Criteria.Filters.Add(filter);
            }
            else
            {
                query.Criteria.AddCondition(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.In
                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10
                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20
                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40
                );
            }

            var result = _service.RetrieveMultipleAll<SdkMessageProcessingStep>(query);

            FullfillEntitiesSteps(result);

            return result;
        }

        public static void FullfillEntitiesSteps(IEnumerable<Entity> result)
        {
            foreach (var item in result)
            {
                FullfillEntitiesSteps(item);
            }
        }

        public static void FullfillEntitiesSteps(Entity item)
        {
            if (!item.Contains(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode))
            {
                item.Attributes.Add(
                    SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode
                    , new AliasedValue(SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid, SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, "none")
                );
            }

            if (!item.Contains(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode))
            {
                item.Attributes.Add(SdkMessageProcessingStep.Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode
                    , new AliasedValue(SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode, "none")
                );
            }
        }

        public Task<List<SdkMessageProcessingStep>> GetAllStepsByPluginTypeAsync(Guid pluginTypeId)
        {
            return Task.Run(() => GetAllStepsByPluginType(pluginTypeId));
        }

        private List<SdkMessageProcessingStep> GetAllStepsByPluginType(Guid pluginTypeId)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageProcessingStep.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                NoLock = true,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.eventhandler, ConditionOperator.Equal, pluginTypeId),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkToAttributeName = SdkMessageFilter.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        JoinOperator = JoinOperator.LeftOuter,

                        Columns = new ColumnSet(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                        LinkToEntityName = PluginType.EntityLogicalName,
                        LinkToAttributeName = PluginType.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                        Columns = new ColumnSet(PluginType.Schema.Attributes.typename, PluginType.Schema.Attributes.assemblyname, PluginType.Schema.Attributes.pluginassemblyid),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                        LinkToEntityName = SdkMessage.EntityLogicalName,
                        LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                        JoinOperator = JoinOperator.LeftOuter,

                        Columns = new ColumnSet(SdkMessage.Schema.Attributes.name, SdkMessage.Schema.Attributes.categoryname),
                    },
                },
            };

            var result = _service.RetrieveMultipleAll<SdkMessageProcessingStep>(query);

            FullfillEntitiesSteps(result);

            return result;
        }

        public Task<List<SdkMessageProcessingStep>> GetAllStepsByPluginAssemblyAsync(Guid idPluginAssembly)
        {
            return Task.Run(() => GetAllStepsByPluginAssembly(idPluginAssembly));
        }

        private List<SdkMessageProcessingStep> GetAllStepsByPluginAssembly(Guid idPluginAssembly)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageProcessingStep.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                NoLock = true,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkToAttributeName = SdkMessageFilter.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                        LinkToEntityName = PluginType.EntityLogicalName,
                        LinkToAttributeName = PluginType.EntityPrimaryIdAttribute,

                        EntityAlias = PluginType.EntityLogicalName,

                        Columns = new ColumnSet(PluginType.Schema.Attributes.typename, PluginType.Schema.Attributes.assemblyname, PluginType.Schema.Attributes.pluginassemblyid),

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(PluginType.Schema.Attributes.pluginassemblyid, ConditionOperator.Equal, idPluginAssembly),
                            },
                        },
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                        LinkToEntityName = SdkMessage.EntityLogicalName,
                        LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessage.EntityLogicalName,

                        Columns = new ColumnSet(SdkMessage.Schema.Attributes.name, SdkMessage.Schema.Attributes.categoryname),
                    },
                },
            };

            var result = _service.RetrieveMultipleAll<SdkMessageProcessingStep>(query);

            FullfillEntitiesSteps(result);

            return result;
        }

        public Task<List<SdkMessageProcessingStep>> GetPluginStepsByPluginTypeIdAsync(Guid pluginTypeId)
        {
            return Task.Run(() => GetPluginStepsByPluginTypeId(pluginTypeId));
        }

        private List<SdkMessageProcessingStep> GetPluginStepsByPluginTypeId(Guid pluginTypeId)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageProcessingStep.EntityLogicalName,
                ColumnSet = new ColumnSet(true),

                NoLock = true,

                Distinct = true,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.eventhandler, ConditionOperator.Equal, pluginTypeId),

                        new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.In
                            , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10
                            , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20
                            , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40
                        ),

                        new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                    },
                },

                Orders =
                {
                    new OrderExpression(SdkMessageProcessingStep.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStep.Schema.Attributes.createdon, OrderType.Ascending),
                },
            };

            var result = _service.RetrieveMultipleAll<SdkMessageProcessingStep>(query);

            FullfillEntitiesSteps(result);

            return result;
        }

        public async Task<SdkMessageProcessingStep> GetStepByIdAsync(Guid id)
        {
            SdkMessageProcessingStep entity1 = await GetLinked1(id);
            SdkMessageProcessingStep entity2 = GetLinked2(id);

            entity1.Merge(entity2);

            FullfillEntitiesSteps(entity1);

            return entity1;
        }

        private async Task<SdkMessageProcessingStep> GetLinked1(Guid id)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageProcessingStep.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStep.EntityPrimaryIdAttribute, ConditionOperator.Equal, id),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                        LinkToEntityName = SdkMessage.EntityLogicalName,
                        LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(true),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkToAttributeName = SdkMessageFilter.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(true),

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                                LinkToEntityName = SdkMessage.EntityLogicalName,
                                LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid + "." + SdkMessageFilter.Schema.Attributes.sdkmessageid,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(true),
                            },
                        },
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                        LinkToEntityName = PluginType.EntityLogicalName,
                        LinkToAttributeName = PluginType.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.eventhandler
                                        + "." + PluginType.EntityLogicalName
                                        ,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(true),

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = PluginType.EntityLogicalName,
                                LinkFromAttributeName = PluginType.Schema.Attributes.pluginassemblyid,

                                LinkToEntityName = PluginAssembly.EntityLogicalName,
                                LinkToAttributeName = PluginAssembly.EntityPrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStep.Schema.Attributes.eventhandler
                                                + "." + PluginType.EntityLogicalName
                                                + "." + PluginType.Schema.Attributes.pluginassemblyid
                                                ,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(await PluginAssemblyRepository.GetAttributesAsync(_service)),
                            },
                        },
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageProcessingStep>()).SingleOrDefault() : null;
        }

        private SdkMessageProcessingStep GetLinked2(Guid id)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageProcessingStep.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStep.EntityPrimaryIdAttribute, ConditionOperator.Equal, id),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                        LinkToEntityName = ServiceEndpoint.EntityLogicalName,
                        LinkToAttributeName = ServiceEndpoint.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.eventhandler
                                        + "." + ServiceEndpoint.EntityLogicalName
                                        ,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(true),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessageprocessingstepsecureconfigid,

                        LinkToEntityName = SdkMessageProcessingStepSecureConfig.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStepSecureConfig.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessageprocessingstepsecureconfigid,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(true),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.impersonatinguserid,

                        LinkToEntityName = SystemUser.EntityLogicalName,
                        LinkToAttributeName = SystemUser.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.impersonatinguserid,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(true),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.plugintypeid,

                        LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkToAttributeName = SdkMessageFilter.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.plugintypeid,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(true),

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                                LinkToEntityName = SdkMessage.EntityLogicalName,
                                LinkToAttributeName = SdkMessage.EntityPrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStep.Schema.Attributes.plugintypeid + "." + SdkMessageFilter.Schema.Attributes.sdkmessageid,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(true),
                            },
                        },
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageProcessingStep>()).SingleOrDefault() : null;
        }

        public static string GetStageName(int stage, int? mode)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0} - ", stage);

            string name = string.Empty;

            switch (stage)
            {
                case (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10:
                    name = "Pre Validation";
                    break;

                case (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20:
                    name = "Pre";
                    break;

                case (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40:
                case (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_Deprecated_50:
                    name = "Post";
                    break;

                default:
                    name = "Other";
                    break;
            }

            result.Append(name);

            if (mode == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1)
            {
                result.Append(" Asynch");
            }

            return result.ToString();
        }

        public Task<List<SdkMessageProcessingStep>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<SdkMessageProcessingStep> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageProcessingStep.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                        new ConditionExpression(SdkMessageProcessingStep.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                        new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.In
                            , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10
                            , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20
                            , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40
                        )
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                        LinkToAttributeName = SdkMessageFilter.EntityPrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                        Columns = new ColumnSet(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode),
                    },
                },

                Orders =
                {
                    new OrderExpression(SdkMessageProcessingStep.Schema.Attributes.plugintypeid, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStep.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            var result = _service.RetrieveMultipleAll<SdkMessageProcessingStep>(query);

            FullfillEntitiesSteps(result);

            return result;
        }
    }
}
