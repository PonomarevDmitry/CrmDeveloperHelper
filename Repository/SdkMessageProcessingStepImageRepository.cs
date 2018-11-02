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
    public class SdkMessageProcessingStepImageRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageProcessingStepImageRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageProcessingStepImage>> GetAllSdkMessageProcessingStepImageAsync()
        {
            return Task.Run(() => GetAllSdkMessageProcessingStepImage());
        }

        private List<SdkMessageProcessingStepImage> GetAllSdkMessageProcessingStepImage()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Orders =
                {
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.imagetype, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.entityalias, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.createdon, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageProcessingStepImage>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageProcessingStepImage>()));

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

        public Task<List<SdkMessageProcessingStepImage>> GetAllImagesAsync()
        {
            return Task.Run(() => GetAllImages());
        }

        private List<SdkMessageProcessingStepImage> GetAllImages()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.PrimaryIdAttribute,

                        EntityAlias = SdkMessageProcessingStep.EntityLogicalName,

                        Columns = new ColumnSet(
                            SdkMessageProcessingStep.Schema.Attributes.plugintypeid
                            , SdkMessageProcessingStep.Schema.Attributes.sdkmessageid
                            , SdkMessageProcessingStep.Schema.Attributes.stage
                            , SdkMessageProcessingStep.Schema.Attributes.mode
                            , SdkMessageProcessingStep.Schema.Attributes.rank
                            , SdkMessageProcessingStep.Schema.Attributes.statuscode
                            , SdkMessageProcessingStep.Schema.Attributes.eventhandler
                        ),

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

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                                LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                                LinkToAttributeName = SdkMessageFilter.PrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(
                                    SdkMessageFilter.Schema.Attributes.primaryobjecttypecode
                                    , SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode
                                ),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.imagetype, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.entityalias, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.createdon, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageProcessingStepImage>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageProcessingStepImage>()));

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

            return result;
        }

        public Task<List<SdkMessageProcessingStepImage>> GetStepImagesAsync(Guid stepId)
        {
            return Task.Run(() => GetStepImages(stepId));
        }

        private List<SdkMessageProcessingStepImage> GetStepImages(Guid stepId)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid, ConditionOperator.Equal, stepId),
                    },
                },

                Orders =
                {
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.imagetype, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.entityalias, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.createdon, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageProcessingStepImage>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageProcessingStepImage>()));

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

        public Task<List<SdkMessageProcessingStepImage>> GetImagesByPluginTypeAsync(Guid idPluginType)
        {
            return Task.Run(() => GetImagesByPluginType(idPluginType));
        }

        private List<SdkMessageProcessingStepImage> GetImagesByPluginType(Guid idPluginType)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.PrimaryIdAttribute,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.eventhandler, ConditionOperator.Equal, idPluginType),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.imagetype, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.entityalias, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.createdon, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageProcessingStepImage>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageProcessingStepImage>()));

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

        public Task<List<SdkMessageProcessingStepImage>> GetImagesByPluginAssemblyAsync(Guid idPluginAssembly)
        {
            return Task.Run(() => GetImagesByPluginAssembly(idPluginAssembly));
        }

        private List<SdkMessageProcessingStepImage> GetImagesByPluginAssembly(Guid idPluginAssembly)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                ColumnSet = new ColumnSet(true),

                NoLock = true,

                Distinct = true,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.PrimaryIdAttribute,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                                LinkToEntityName = PluginType.EntityLogicalName,
                                LinkToAttributeName = PluginType.PrimaryIdAttribute,

                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(PluginType.Schema.Attributes.pluginassemblyid, ConditionOperator.Equal, idPluginAssembly),
                                    },
                                },
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.imagetype, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.entityalias, OrderType.Ascending),
                    new OrderExpression(SdkMessageProcessingStepImage.Schema.Attributes.createdon, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageProcessingStepImage>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageProcessingStepImage>()));

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

        public Task<SdkMessageProcessingStepImage> GetStepImageByIdAsync(Guid id)
        {
            return Task.Run(async () => await GetStepImageById(id));
        }

        private async Task<SdkMessageProcessingStepImage> GetStepImageById(Guid id)
        {
            SdkMessageProcessingStepImage entity1 = GetLinked1(id);
            SdkMessageProcessingStepImage entity2 = await GetLinked2(id);
            SdkMessageProcessingStepImage entity3 = GetLinked3(id);

            entity1 = EntityExtensions.Merge(entity1, entity2, entity3);

            return entity1;
        }

        private SdkMessageProcessingStepImage GetLinked1(Guid id)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStepImage.PrimaryIdAttribute, ConditionOperator.Equal, id),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.PrimaryIdAttribute,

                        Columns = new ColumnSet(true),

                        EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                                LinkToEntityName = SdkMessage.EntityLogicalName,
                                LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.sdkmessageid,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(true),
                            },

                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                                LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                                LinkToAttributeName = SdkMessageFilter.PrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid
                                                ,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(true),

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                                        LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                                        LinkToEntityName = SdkMessage.EntityLogicalName,
                                        LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                                        EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                        + "." + SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid
                                                        + "." + SdkMessageFilter.Schema.Attributes.sdkmessageid
                                                        ,

                                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                        Columns = new ColumnSet(true),
                                    },
                                },
                            },
                        },
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SdkMessageProcessingStepImage>()).SingleOrDefault();
        }

        private async Task<SdkMessageProcessingStepImage> GetLinked2(Guid id)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStepImage.PrimaryIdAttribute, ConditionOperator.Equal, id),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.PrimaryIdAttribute,

                        Columns = new ColumnSet(true),

                        EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.impersonatinguserid,

                                LinkToEntityName = SystemUser.EntityLogicalName,
                                LinkToAttributeName = SystemUser.PrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.impersonatinguserid,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(true),
                            },

                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                                LinkToEntityName = PluginType.EntityLogicalName,
                                LinkToAttributeName = PluginType.PrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.eventhandler
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
                                        LinkToAttributeName = PluginAssembly.PrimaryIdAttribute,

                                        EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.eventhandler
                                                + "." + PluginType.EntityLogicalName
                                                + "." + PluginType.Schema.Attributes.pluginassemblyid,

                                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                        Columns = new ColumnSet(await PluginAssemblyRepository.GetAttributesAsync(_service)),
                                    },
                                },
                            },

                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                                LinkToEntityName = ServiceEndpoint.EntityLogicalName,
                                LinkToAttributeName = ServiceEndpoint.PrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.eventhandler
                                                + "." + ServiceEndpoint.EntityLogicalName
                                                ,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(true),
                            },
                        },
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SdkMessageProcessingStepImage>()).SingleOrDefault();
        }

        private SdkMessageProcessingStepImage GetLinked3(Guid id)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStepImage.PrimaryIdAttribute, ConditionOperator.Equal, id),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                        LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.PrimaryIdAttribute,

                        Columns = new ColumnSet(true),

                        EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid,

                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessageprocessingstepsecureconfigid,

                                LinkToEntityName = SdkMessageProcessingStepSecureConfig.EntityLogicalName,
                                LinkToAttributeName = SdkMessageProcessingStepSecureConfig.PrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.sdkmessageprocessingstepsecureconfigid
                                                ,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(true),
                            },

                            new LinkEntity()
                            {
                                LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.plugintypeid,

                                LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                                LinkToAttributeName = SdkMessageFilter.PrimaryIdAttribute,

                                EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.plugintypeid
                                                ,

                                JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                Columns = new ColumnSet(true),

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = SdkMessageFilter.EntityLogicalName,
                                        LinkFromAttributeName = SdkMessageFilter.Schema.Attributes.sdkmessageid,

                                        LinkToEntityName = SdkMessage.EntityLogicalName,
                                        LinkToAttributeName = SdkMessage.PrimaryIdAttribute,

                                        EntityAlias = SdkMessageProcessingStepImage.Schema.Attributes.sdkmessageprocessingstepid
                                                + "." + SdkMessageProcessingStep.Schema.Attributes.plugintypeid
                                                + "." + SdkMessageFilter.Schema.Attributes.sdkmessageid
                                                ,

                                        JoinOperator = Microsoft.Xrm.Sdk.Query.JoinOperator.LeftOuter,

                                        Columns = new ColumnSet(true),
                                    },
                                },
                            },
                        },
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SdkMessageProcessingStepImage>()).SingleOrDefault();
        }
    }
}
