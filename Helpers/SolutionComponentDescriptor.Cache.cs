using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private ConcurrentDictionary<int, ConcurrentDictionary<Guid, Entity>> _cache = new ConcurrentDictionary<int, ConcurrentDictionary<Guid, Entity>>();

        public List<T> GetEntities<T>(int componentType, IEnumerable<Guid> components) where T : Entity
        {
            return GetEntities<T>(componentType, components.Select(id => (Guid?)id));
        }

        public List<T> GetEntities<T>(int componentType, IEnumerable<Guid?> components) where T : Entity
        {
            List<Guid> idsNotCached = components.Where(c => c.HasValue).Select(comp => comp.Value).ToList();

            var list = GetCachedEntities(componentType, idsNotCached);

            if (idsNotCached.Count > 0)
            {
                string entityName = null;
                string entityIdName = null;

                GetEntityName(componentType, out entityName, out entityIdName);

                if (!string.IsNullOrEmpty(entityName))
                {
                    QueryExpression query = GetQuery(idsNotCached, (ComponentType)componentType, entityName, entityIdName);

                    var result = new List<Entity>();

                    try
                    {
                        while (true)
                        {
                            var coll = _service.RetrieveMultiple(query);

                            result.AddRange(coll.Entities);

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
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }

                    CacheEntities(componentType, result);

                    list.AddRange(result);
                }
            }

            if (componentType == (int)ComponentType.SDKMessageProcessingStep
               || componentType == (int)ComponentType.SDKMessageProcessingStepImage)
            {
                SdkMessageProcessingStepRepository.FullfillEntitiesSteps(list);
            }

            return list.Select(e => e.ToEntity<T>()).ToList();
        }

        private static QueryExpression GetQuery(List<Guid> idsNotCached, ComponentType componentType, string entityName, string entityIdName)
        {
            QueryExpression query = null;

            #region SdkMessageProcessingStep.

            if (entityName == SdkMessageProcessingStep.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = SdkMessageProcessingStep.EntityLogicalName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                            LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                            LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                            LinkToAttributeName = SdkMessageFilter.PrimaryIdAttribute,

                            EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                            Columns = new ColumnSet(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                            LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                            LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            #endregion SdkMessageProcessingStep.

            #region SdkMessageProcessingStepImage

            if (entityName == SdkMessageProcessingStepImage.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = SdkMessageProcessingStepImage.EntityLogicalName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
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

                            EntityAlias = SdkMessageProcessingStep.EntityLogicalName,

                            Columns = new ColumnSet(
                                SdkMessageProcessingStep.Schema.Attributes.sdkmessageid
                                , SdkMessageProcessingStep.Schema.Attributes.stage
                                , SdkMessageProcessingStep.Schema.Attributes.mode
                                , SdkMessageProcessingStep.Schema.Attributes.rank
                                , SdkMessageProcessingStep.Schema.Attributes.statuscode
                                , SdkMessageProcessingStep.Schema.Attributes.eventhandler
                            ),

                            LinkEntities =
                            {
                                new LinkEntity()
                                {
                                    JoinOperator = JoinOperator.LeftOuter,

                                    LinkFromEntityName = SdkMessageProcessingStep.EntityLogicalName,
                                    LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                                    LinkToEntityName = SdkMessageFilter.EntityLogicalName,
                                    LinkToAttributeName = SdkMessageFilter.PrimaryIdAttribute,

                                    EntityAlias = SdkMessageProcessingStep.Schema.Attributes.sdkmessagefilterid,

                                    Columns = new ColumnSet(SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode),
                                },
                            },
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                            LinkFromAttributeName = SdkMessageProcessingStep.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                            LinkFromAttributeName = SdkMessageProcessingStepImage.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            #endregion SdkMessageProcessingStepImage

            #region Role

            if (entityName == Role.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = Role.EntityLogicalName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = Role.EntityLogicalName,
                            LinkFromAttributeName = Role.Schema.Attributes.businessunitid,

                            LinkToEntityName = BusinessUnit.EntityLogicalName,
                            LinkToAttributeName = BusinessUnit.PrimaryIdAttribute,

                            EntityAlias = BusinessUnit.EntityLogicalName,

                            Columns = new ColumnSet(BusinessUnit.Schema.Attributes.parentbusinessunitid, BusinessUnit.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = Role.EntityLogicalName,
                            LinkFromAttributeName = Role.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = Role.EntityLogicalName,
                            LinkFromAttributeName = Role.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    Orders =
                    {
                        new OrderExpression(Role.Schema.Attributes.name, OrderType.Ascending),
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            #endregion Role

            #region RolePrivileges

            if (entityName == RolePrivileges.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = RolePrivileges.EntityLogicalName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = RolePrivileges.EntityLogicalName,
                            LinkFromAttributeName = RolePrivileges.Schema.Attributes.privilegeid,

                            LinkToEntityName = Privilege.EntityLogicalName,
                            LinkToAttributeName = Privilege.PrimaryIdAttribute,

                            EntityAlias = Privilege.EntityLogicalName,

                            Columns = new ColumnSet(Privilege.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = RolePrivileges.EntityLogicalName,
                            LinkFromAttributeName = RolePrivileges.Schema.Attributes.roleid,

                            LinkToEntityName = Role.EntityLogicalName,
                            LinkToAttributeName = Role.PrimaryIdAttribute,

                            EntityAlias = Role.EntityLogicalName,

                            Columns = new ColumnSet(Role.Schema.Attributes.name, Role.Schema.Attributes.businessunitid),

                            LinkEntities =
                            {
                                new LinkEntity()
                                {
                                    JoinOperator = JoinOperator.LeftOuter,

                                    LinkFromEntityName = Role.EntityLogicalName,
                                    LinkFromAttributeName = Role.Schema.Attributes.businessunitid,

                                    LinkToEntityName = BusinessUnit.EntityLogicalName,
                                    LinkToAttributeName = BusinessUnit.PrimaryIdAttribute,

                                    EntityAlias = BusinessUnit.EntityLogicalName,

                                    Columns = new ColumnSet(BusinessUnit.Schema.Attributes.parentbusinessunitid, BusinessUnit.Schema.Attributes.name),
                                },
                            },
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = RolePrivileges.EntityLogicalName,
                            LinkFromAttributeName = RolePrivileges.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = RolePrivileges.EntityLogicalName,
                            LinkFromAttributeName = RolePrivileges.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            #endregion RolePrivileges

            #region fieldpermission

            if (entityName == FieldPermission.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = FieldPermission.EntityLogicalName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = FieldPermission.EntityLogicalName,
                            LinkFromAttributeName = FieldPermission.Schema.Attributes.fieldsecurityprofileid,

                            LinkToEntityName = FieldSecurityProfile.EntityLogicalName,
                            LinkToAttributeName = FieldSecurityProfile.PrimaryIdAttribute,

                            EntityAlias = FieldSecurityProfile.EntityLogicalName,

                            Columns = new ColumnSet(FieldSecurityProfile.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = FieldPermission.EntityLogicalName,
                            LinkFromAttributeName = FieldPermission.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = FieldPermission.EntityLogicalName,
                            LinkFromAttributeName = FieldPermission.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            #endregion fieldpermission

            #region displaystringmap

            if (entityName == DisplayStringMap.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = DisplayStringMap.EntityLogicalName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = DisplayStringMap.EntityLogicalName,
                            LinkFromAttributeName = DisplayStringMap.Schema.Attributes.displaystringid,

                            LinkToEntityName = DisplayString.EntityLogicalName,
                            LinkToAttributeName = DisplayString.PrimaryIdAttribute,

                            EntityAlias = DisplayString.EntityLogicalName,

                            Columns = new ColumnSet(DisplayString.Schema.Attributes.displaystringkey),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = DisplayStringMap.EntityLogicalName,
                            LinkFromAttributeName = DisplayStringMap.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = DisplayStringMap.EntityLogicalName,
                            LinkFromAttributeName = DisplayStringMap.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            #endregion displaystringmap

            #region attributemap

            if (entityName == AttributeMap.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = AttributeMap.EntityLogicalName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.entitymapid,

                            LinkToEntityName = EntityMap.EntityLogicalName,
                            LinkToAttributeName = EntityMap.PrimaryIdAttribute,

                            EntityAlias = EntityMap.EntityLogicalName,

                            Columns = new ColumnSet(EntityMap.Schema.Attributes.sourceentityname, EntityMap.Schema.Attributes.targetentityname),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            #endregion attributemap

            if (entityName == ChannelAccessProfileEntityAccessLevel.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = entityName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            LinkFromEntityName = ChannelAccessProfileEntityAccessLevel.EntityLogicalName,
                            LinkFromAttributeName = ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid,

                            LinkToEntityName = ChannelAccessProfile.EntityLogicalName,
                            LinkToAttributeName = ChannelAccessProfile.PrimaryIdAttribute,

                            EntityAlias = ChannelAccessProfileEntityAccessLevel.Schema.Attributes.channelaccessprofileid,

                            Columns = new ColumnSet(ChannelAccessProfile.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            LinkFromEntityName = ChannelAccessProfileEntityAccessLevel.EntityLogicalName,
                            LinkFromAttributeName = ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid,

                            LinkToEntityName = Privilege.EntityLogicalName,
                            LinkToAttributeName = Privilege.PrimaryIdAttribute,

                            EntityAlias = ChannelAccessProfileEntityAccessLevel.Schema.Attributes.entityaccesslevelid,

                            Columns = new ColumnSet(Privilege.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            if (entityName == AppModuleRoles.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = entityName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            LinkFromEntityName = AppModuleRoles.EntityLogicalName,
                            LinkFromAttributeName = AppModuleRoles.Schema.Attributes.appmoduleid,

                            LinkToEntityName = AppModule.EntityLogicalName,
                            LinkToAttributeName = AppModule.PrimaryIdAttribute,

                            EntityAlias = AppModuleRoles.Schema.Attributes.appmoduleid,

                            Columns = new ColumnSet(AppModule.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            LinkFromEntityName = AppModuleRoles.EntityLogicalName,
                            LinkFromAttributeName = AppModuleRoles.Schema.Attributes.roleid,

                            LinkToEntityName = Role.EntityLogicalName,
                            LinkToAttributeName = Role.PrimaryIdAttribute,

                            EntityAlias = AppModuleRoles.Schema.Attributes.roleid,

                            Columns = new ColumnSet(Role.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            if (entityName == CustomControlResource.EntityLogicalName)
            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = entityName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            LinkFromEntityName = CustomControlResource.EntityLogicalName,
                            LinkFromAttributeName = CustomControlResource.Schema.Attributes.customcontrolid,

                            LinkToEntityName = CustomControl.EntityLogicalName,
                            LinkToAttributeName = CustomControl.PrimaryIdAttribute,

                            EntityAlias = CustomControlResource.Schema.Attributes.customcontrolid,

                            Columns = new ColumnSet(CustomControl.Schema.Attributes.name),
                        },

                        new LinkEntity()
                        {
                            LinkFromEntityName = CustomControlResource.EntityLogicalName,
                            LinkFromAttributeName = CustomControlResource.Schema.Attributes.webresourceid,

                            LinkToEntityName = WebResource.EntityLogicalName,
                            LinkToAttributeName = WebResource.PrimaryIdAttribute,

                            EntityAlias = CustomControlResource.Schema.Attributes.webresourceid,

                            Columns = new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.solutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = AttributeMap.EntityLogicalName,
                            LinkFromAttributeName = AttributeMap.Schema.Attributes.supportingsolutionid,

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }

            {
                query = new QueryExpression()
                {
                    NoLock = true,

                    EntityName = entityName,

                    ColumnSet = GetColumnSet(componentType),

                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(entityIdName, ConditionOperator.In, idsNotCached.ToArray()),
                        },
                    },

                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = entityName,
                            LinkFromAttributeName = "solutionid",

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = Solution.EntityLogicalName,

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },

                        new LinkEntity()
                        {
                            JoinOperator = JoinOperator.LeftOuter,

                            LinkFromEntityName = entityName,
                            LinkFromAttributeName = "supportingsolutionid",

                            LinkToEntityName = Solution.EntityLogicalName,
                            LinkToAttributeName = Solution.PrimaryIdAttribute,

                            EntityAlias = "suppsolution",

                            Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                        },
                    },

                    PageInfo = new PagingInfo()
                    {
                        PageNumber = 1,
                        Count = 5000,
                    }
                };

                return query;
            }
        }

        private static ColumnSet GetColumnSet(ComponentType componentType)
        {
            switch (componentType)
            {
                case ComponentType.Role:
                    return new ColumnSet(Role.Schema.Attributes.name, Role.Schema.Attributes.businessunitid, Role.Schema.Attributes.ismanaged);

                case ComponentType.RolePrivileges:
                    return new ColumnSet(RolePrivileges.Schema.Attributes.privilegedepthmask, RolePrivileges.Schema.Attributes.ismanaged);

                case ComponentType.DisplayString:
                    return new ColumnSet
                        (
                            DisplayString.Schema.Attributes.displaystringkey
                            , DisplayString.Schema.Attributes.languagecode
                            , DisplayString.Schema.Attributes.publisheddisplaystring
                            , DisplayString.Schema.Attributes.customdisplaystring
                            , DisplayString.Schema.Attributes.customcomment
                            , DisplayString.Schema.Attributes.formatparameters
                            , DisplayString.Schema.Attributes.ismanaged
                        );

                case ComponentType.DisplayStringMap:
                    return new ColumnSet(DisplayStringMap.Schema.Attributes.objecttypecode, DisplayStringMap.Schema.Attributes.ismanaged);

                case ComponentType.SavedQuery:
                    return new ColumnSet
                        (
                            SavedQuery.Schema.Attributes.returnedtypecode
                            , SavedQuery.Schema.Attributes.querytype
                            , SavedQuery.Schema.Attributes.name
                            , SavedQuery.Schema.Attributes.ismanaged
                            , SavedQuery.Schema.Attributes.iscustomizable
                        );

                case ComponentType.SavedQueryVisualization:
                    return new ColumnSet
                        (
                            SavedQueryVisualization.Schema.Attributes.primaryentitytypecode
                            , SavedQueryVisualization.Schema.Attributes.name
                            , SavedQueryVisualization.Schema.Attributes.ismanaged
                            , SavedQueryVisualization.Schema.Attributes.iscustomizable
                        );

                case ComponentType.SystemForm:
                    return new ColumnSet
                        (
                            SystemForm.Schema.Attributes.objecttypecode
                            , SystemForm.Schema.Attributes.name
                            , SystemForm.Schema.Attributes.type
                            , SystemForm.Schema.Attributes.ismanaged
                            , SystemForm.Schema.Attributes.iscustomizable
                        );

                case ComponentType.Workflow:
                    return new ColumnSet
                        (
                            Workflow.Schema.Attributes.primaryentity
                            , Workflow.Schema.Attributes.category
                            , Workflow.Schema.Attributes.name
                            , Workflow.Schema.Attributes.uniquename
                            , Workflow.Schema.Attributes.businessprocesstype
                            , Workflow.Schema.Attributes.scope
                            , Workflow.Schema.Attributes.mode
                            , Workflow.Schema.Attributes.statuscode
                            , Workflow.Schema.Attributes.ismanaged
                            , Workflow.Schema.Attributes.iscustomizable
                        );

                case ComponentType.Report:
                    return new ColumnSet
                        (
                            Report.Schema.Attributes.name
                            , Report.Schema.Attributes.filename
                            , Report.Schema.Attributes.reporttypecode
                            , Report.Schema.Attributes.ispersonal
                            , Report.Schema.Attributes.ownerid
                            , Report.Schema.Attributes.ismanaged
                            , Report.Schema.Attributes.iscustomizable
                        );

                case ComponentType.ReportEntity:
                    return new ColumnSet
                        (
                            ReportEntity.Schema.Attributes.reportid
                            , ReportEntity.Schema.Attributes.objecttypecode
                            , ReportEntity.Schema.Attributes.ismanaged
                            , ReportEntity.Schema.Attributes.iscustomizable
                        );

                case ComponentType.ReportCategory:
                    return new ColumnSet
                        (
                            ReportCategory.Schema.Attributes.reportid
                            , ReportCategory.Schema.Attributes.categorycode
                            , ReportCategory.Schema.Attributes.ismanaged
                            , ReportCategory.Schema.Attributes.iscustomizable
                        );

                case ComponentType.ReportVisibility:
                    return new ColumnSet
                        (
                            ReportVisibility.Schema.Attributes.reportid
                            , ReportVisibility.Schema.Attributes.visibilitycode
                            , ReportVisibility.Schema.Attributes.ismanaged
                            , ReportVisibility.Schema.Attributes.iscustomizable
                        );

                case ComponentType.EmailTemplate:
                    return new ColumnSet
                        (
                            Template.Schema.Attributes.templatetypecode
                            , Template.Schema.Attributes.title
                            , Template.Schema.Attributes.ismanaged
                            , Template.Schema.Attributes.iscustomizable
                        );

                case ComponentType.ContractTemplate:
                    return new ColumnSet
                        (
                            ContractTemplate.Schema.Attributes.name
                            , ContractTemplate.Schema.Attributes.ismanaged
                            , ContractTemplate.Schema.Attributes.iscustomizable
                        );

                case ComponentType.KBArticleTemplate:
                    return new ColumnSet
                        (
                            KbArticleTemplate.Schema.Attributes.title
                            , KbArticleTemplate.Schema.Attributes.ismanaged
                            , KbArticleTemplate.Schema.Attributes.iscustomizable
                        );

                case ComponentType.MailMergeTemplate:
                    return new ColumnSet
                        (
                            MailMergeTemplate.Schema.Attributes.templatetypecode
                            , MailMergeTemplate.Schema.Attributes.name
                            , MailMergeTemplate.Schema.Attributes.mailmergetype
                            , MailMergeTemplate.Schema.Attributes.ismanaged
                            , MailMergeTemplate.Schema.Attributes.iscustomizable
                        );

                //case ComponentType.DuplicateRule:
                //    return new ColumnSet
                //        (
                //            DuplicateRule.Schema.Attributes.name
                //            , DuplicateRule.Schema.Attributes.ismanaged
                //            , DuplicateRule.Schema.Attributes.ismanaged
                //            , DuplicateRule.Schema.Attributes.ismanaged
                //            , DuplicateRule.Schema.Attributes.ismanaged
                //            , DuplicateRule.Schema.Attributes.ismanaged
                //            , DuplicateRule.Schema.Attributes.ismanaged
                //            , DuplicateRule.Schema.Attributes.ismanaged
                //        );

                //case ComponentType.DuplicateRuleCondition:
                //    return new ColumnSet
                //        (
                //            Workflow.Schema.Attributes.name
                //            , Workflow.Schema.Attributes.ismanaged
                //            , Workflow.Schema.Attributes.ismanaged
                //            , Workflow.Schema.Attributes.ismanaged
                //            , Workflow.Schema.Attributes.ismanaged
                //            , Workflow.Schema.Attributes.ismanaged
                //            , Workflow.Schema.Attributes.ismanaged
                //            , Workflow.Schema.Attributes.ismanaged
                //        );

                case ComponentType.EntityMap:
                    return new ColumnSet
                        (
                            EntityMap.Schema.Attributes.sourceentityname
                            , EntityMap.Schema.Attributes.targetentityname
                            , EntityMap.Schema.Attributes.ismanaged
                        );

                case ComponentType.AttributeMap:
                    return new ColumnSet
                        (
                            AttributeMap.Schema.Attributes.sourceattributename
                            , AttributeMap.Schema.Attributes.targetattributename
                            , AttributeMap.Schema.Attributes.ismanaged
                        );

                case ComponentType.RibbonCommand:
                    return new ColumnSet
                        (
                            RibbonCommand.Schema.Attributes.entity
                            , RibbonCommand.Schema.Attributes.command
                            , RibbonCommand.Schema.Attributes.ribboncommandid
                            , RibbonCommand.Schema.Attributes.ismanaged
                        );

                case ComponentType.RibbonContextGroup:
                    return new ColumnSet
                        (
                            RibbonContextGroup.Schema.Attributes.entity
                            , RibbonContextGroup.Schema.Attributes.contextgroupid
                            , RibbonContextGroup.Schema.Attributes.ismanaged
                        );

                case ComponentType.RibbonCustomization:
                    return new ColumnSet
                        (
                            RibbonCustomization.Schema.Attributes.entity
                            , RibbonCustomization.Schema.Attributes.ismanaged
                        );

                case ComponentType.RibbonRule:
                    return new ColumnSet
                        (
                            RibbonRule.Schema.Attributes.entity
                            , RibbonRule.Schema.Attributes.ruletype
                            , RibbonRule.Schema.Attributes.ruleid
                            , RibbonRule.Schema.Attributes.ismanaged
                        );

                case ComponentType.RibbonTabToCommandMap:
                    return new ColumnSet
                        (
                            RibbonTabToCommandMap.Schema.Attributes.entity
                            , RibbonTabToCommandMap.Schema.Attributes.tabid
                            , RibbonTabToCommandMap.Schema.Attributes.controlid
                            , RibbonTabToCommandMap.Schema.Attributes.command
                            , RibbonTabToCommandMap.Schema.Attributes.ismanaged
                        );

                case ComponentType.RibbonDiff:
                    return new ColumnSet
                        (
                            RibbonDiff.Schema.Attributes.entity
                            , RibbonDiff.Schema.Attributes.diffid
                            , RibbonDiff.Schema.Attributes.tabid
                            , RibbonDiff.Schema.Attributes.contextgroupid
                            , RibbonDiff.Schema.Attributes.sequence
                            , RibbonDiff.Schema.Attributes.ismanaged
                        );

                case ComponentType.WebResource:
                    return new ColumnSet
                        (
                            WebResource.Schema.Attributes.name
                            , WebResource.Schema.Attributes.displayname
                            , WebResource.Schema.Attributes.webresourcetype
                            , WebResource.Schema.Attributes.ismanaged
                            , WebResource.Schema.Attributes.iscustomizable
                        );

                case ComponentType.SiteMap:
                    return new ColumnSet
                        (
                            SiteMap.Schema.Attributes.sitemapname
                            , SiteMap.Schema.Attributes.sitemapnameunique
                            , SiteMap.Schema.Attributes.sitemapid
                            , SiteMap.Schema.Attributes.isappaware
                            , SiteMap.Schema.Attributes.ismanaged
                        );

                case ComponentType.ConnectionRole:
                    return new ColumnSet
                        (
                            ConnectionRole.Schema.Attributes.name
                            , ConnectionRole.Schema.Attributes.ismanaged
                            , ConnectionRole.Schema.Attributes.iscustomizable
                        );

                case ComponentType.FieldSecurityProfile:
                    return new ColumnSet
                        (
                            FieldSecurityProfile.Schema.Attributes.name
                            , FieldSecurityProfile.Schema.Attributes.description
                            , FieldSecurityProfile.Schema.Attributes.ismanaged
                        );

                case ComponentType.FieldPermission:
                    return new ColumnSet
                        (
                            FieldPermission.Schema.Attributes.entityname
                            , FieldPermission.Schema.Attributes.attributelogicalname
                            , FieldPermission.Schema.Attributes.ismanaged
                        );

                case ComponentType.PluginType:
                    return new ColumnSet
                        (
                            PluginType.Schema.Attributes.assemblyname
                            , PluginType.Schema.Attributes.typename
                            , PluginType.Schema.Attributes.ismanaged
                        );

                case ComponentType.PluginAssembly:
                    return new ColumnSet(PluginAssembly.Schema.Attributes.name, PluginAssembly.Schema.Attributes.ismanaged, PluginAssembly.Schema.Attributes.iscustomizable);

                case ComponentType.ServiceEndpoint:
                    return new ColumnSet
                        (
                            ServiceEndpoint.Schema.Attributes.name
                            , ServiceEndpoint.Schema.Attributes.connectionmode
                            , ServiceEndpoint.Schema.Attributes.contract
                            , ServiceEndpoint.Schema.Attributes.messageformat
                            , ServiceEndpoint.Schema.Attributes.ismanaged
                            , ServiceEndpoint.Schema.Attributes.iscustomizable
                        );

                case ComponentType.RoutingRule:
                    return new ColumnSet
                        (
                            RoutingRule.Schema.Attributes.name
                            , RoutingRule.Schema.Attributes.workflowid
                            , RoutingRule.Schema.Attributes.ismanaged
                        );

                case ComponentType.RoutingRuleItem:
                    return new ColumnSet
                        (
                            RoutingRuleItem.Schema.Attributes.routingruleid
                            , RoutingRuleItem.Schema.Attributes.name
                            , RoutingRuleItem.Schema.Attributes.ismanaged
                        );

                case ComponentType.SLA:
                    return new ColumnSet
                        (
                            SLA.Schema.Attributes.objecttypecode
                            , SLA.Schema.Attributes.name
                            , SLA.Schema.Attributes.slaid
                            , SLA.Schema.Attributes.ismanaged
                        );

                case ComponentType.SLAItem:
                    return new ColumnSet
                        (
                            SLAItem.Schema.Attributes.slaid
                            , SLAItem.Schema.Attributes.name
                            , SLAItem.Schema.Attributes.relatedfield
                            , SLAItem.Schema.Attributes.slaitemid
                            , SLAItem.Schema.Attributes.ismanaged
                        );

                case ComponentType.ConvertRule:
                    return new ColumnSet
                        (
                            ConvertRule.Schema.Attributes.name
                            , ConvertRule.Schema.Attributes.ismanaged
                        );

                case ComponentType.ConvertRuleItem:
                    return new ColumnSet
                        (
                            ConvertRuleItem.Schema.Attributes.convertruleid
                            , ConvertRuleItem.Schema.Attributes.name
                            , ConvertRuleItem.Schema.Attributes.ismanaged
                        );

                case ComponentType.HierarchyRule:
                    return new ColumnSet
                        (
                            HierarchyRule.Schema.Attributes.primaryentitylogicalname
                            , HierarchyRule.Schema.Attributes.name
                            , HierarchyRule.Schema.Attributes.description
                            , HierarchyRule.Schema.Attributes.ismanaged
                            , HierarchyRule.Schema.Attributes.iscustomizable
                        );

                case ComponentType.MobileOfflineProfile:
                    return new ColumnSet
                        (
                            MobileOfflineProfile.Schema.Attributes.name
                            , MobileOfflineProfile.Schema.Attributes.selectedentitymetadata
                            , MobileOfflineProfile.Schema.Attributes.mobileofflineprofileid
                            , MobileOfflineProfile.Schema.Attributes.ismanaged
                        );

                case ComponentType.MobileOfflineProfileItem:
                    return new ColumnSet
                        (
                            MobileOfflineProfileItem.Schema.Attributes.name
                            , MobileOfflineProfileItem.Schema.Attributes.selectedentitytypecode
                            , MobileOfflineProfileItem.Schema.Attributes.entityobjecttypecode
                            , MobileOfflineProfileItem.Schema.Attributes.mobileofflineprofileitemid
                            , MobileOfflineProfileItem.Schema.Attributes.ismanaged
                        );

                case ComponentType.SimilarityRule:
                    return new ColumnSet
                        (
                            SimilarityRule.Schema.Attributes.baseentityname
                            , SimilarityRule.Schema.Attributes.name
                            , SimilarityRule.Schema.Attributes.matchingentityname
                            , SimilarityRule.Schema.Attributes.similarityruleid
                            , SimilarityRule.Schema.Attributes.ismanaged
                        );

                case ComponentType.CustomControl:
                    return new ColumnSet
                        (
                            CustomControl.Schema.Attributes.name
                            , CustomControl.Schema.Attributes.compatibledatatypes
                            , CustomControl.Schema.Attributes.manifest
                            , CustomControl.Schema.Attributes.ismanaged
                        );

                case ComponentType.CustomControlDefaultConfig:
                    return new ColumnSet
                        (
                            CustomControlDefaultConfig.Schema.Attributes.primaryentitytypecode
                            , CustomControlDefaultConfig.Schema.Attributes.customcontroldefaultconfigid
                            , CustomControlDefaultConfig.Schema.Attributes.ismanaged
                        );

                case ComponentType.CustomControlResource:
                    return new ColumnSet
                        (
                            CustomControlResource.Schema.Attributes.name
                            , CustomControlResource.Schema.Attributes.customcontrolid
                            , CustomControlResource.Schema.Attributes.webresourceid
                            , CustomControlResource.Schema.Attributes.ismanaged
                        );

                case ComponentType.AppModule:
                    return new ColumnSet
                        (
                            AppModule.Schema.Attributes.name
                            , AppModule.Schema.Attributes.uniquename
                            , AppModule.Schema.Attributes.url
                            , AppModule.Schema.Attributes.appmoduleversion
                            , AppModule.Schema.Attributes.ismanaged
                        );

                case ComponentType.AppModuleRoles:
                    return new ColumnSet
                        (
                            AppModuleRoles.Schema.Attributes.appmoduleid
                            , AppModuleRoles.Schema.Attributes.roleid
                            , AppModuleRoles.Schema.Attributes.ismanaged
                        );

                case ComponentType.ChannelAccessProfile:
                    return new ColumnSet
                        (
                            ChannelAccessProfile.Schema.Attributes.name
                            , ChannelAccessProfile.Schema.Attributes.statuscode
                            , ChannelAccessProfile.Schema.Attributes.ismanaged
                        );

                case ComponentType.ProcessTrigger:
                    return new ColumnSet
                        (
                            ProcessTrigger.Schema.Attributes.primaryentitytypecode
                            , ProcessTrigger.Schema.Attributes.processid
                            , ProcessTrigger.Schema.Attributes.Event
                            , ProcessTrigger.Schema.Attributes.pipelinestage
                            , ProcessTrigger.Schema.Attributes.formid
                            , ProcessTrigger.Schema.Attributes.scope
                            , ProcessTrigger.Schema.Attributes.methodid
                            , ProcessTrigger.Schema.Attributes.controlname
                            , ProcessTrigger.Schema.Attributes.controltype
                            , ProcessTrigger.Schema.Attributes.iscustomizable
                            , ProcessTrigger.Schema.Attributes.ismanaged
                        );
            }

            return new ColumnSet(true);
        }

        public static void GetEntityName(int type, out string entityName, out string entityIdName)
        {
            entityName = string.Empty;
            entityIdName = string.Empty;

            if (!SolutionComponent.IsDefinedComponentType(type))
            {
                return;
            }

            ComponentType componentType = (ComponentType)type;

            if (SolutionComponent.IsComponentTypeMetadata(componentType))
            {
                return;
            }

            switch (componentType)
            {
                case ComponentType.EmailTemplate:
                    entityName = Template.EntityLogicalName;
                    entityIdName = Template.PrimaryIdAttribute;
                    break;

                case ComponentType.RolePrivileges:
                    entityName = RolePrivileges.EntityLogicalName;
                    entityIdName = RolePrivileges.PrimaryIdAttribute;
                    break;

                case ComponentType.SystemForm:
                    entityName = SystemForm.EntityLogicalName;
                    entityIdName = SystemForm.PrimaryIdAttribute;
                    break;

                case ComponentType.DependencyFeature:
                    break;

                default:
                    entityName = componentType.ToString().ToLower();
                    entityIdName = entityName + "id";
                    break;
            }
        }

        private List<Entity> GetCachedEntities(int componentType, List<Guid> idsNotCached)
        {
            var result = new List<Entity>();

            lock (_syncObjectEntityCache)
            {
                if (idsNotCached.Count > 0 && _cache.ContainsKey(componentType))
                {
                    var listToDelete = new List<Guid>();

                    var componentCache = _cache[componentType];

                    foreach (var objectId in idsNotCached)
                    {
                        if (componentCache.ContainsKey(objectId))
                        {
                            listToDelete.Add(objectId);

                            result.Add(componentCache[objectId]);
                        }
                    }

                    listToDelete.ForEach(id => idsNotCached.Remove(id));
                }
            }

            return result;
        }

        private void CacheEntities(int componentType, List<Entity> listEntities)
        {
            ConcurrentDictionary<Guid, Entity> componentCache = null;

            lock (_syncObjectEntityCache)
            {
                if (_cache.ContainsKey(componentType))
                {
                    componentCache = _cache[componentType];
                }
                else
                {
                    componentCache = new ConcurrentDictionary<Guid, Entity>();

                    _cache.TryAdd(componentType, componentCache);
                }
            }

            lock (_syncObjectEntityCache)
            {
                foreach (var entity in listEntities)
                {
                    componentCache.TryAdd(entity.Id, entity);
                }
            }
        }

        public T GetEntity<T>(int type, Guid idEntity) where T : Entity
        {
            var listEntities = GetEntities<T>(type, new Guid?[] { idEntity });

            var entity = listEntities.FirstOrDefault();

            if (entity != null)
            {
                return entity.ToEntity<T>();
            }

            return null;
        }
    }
}