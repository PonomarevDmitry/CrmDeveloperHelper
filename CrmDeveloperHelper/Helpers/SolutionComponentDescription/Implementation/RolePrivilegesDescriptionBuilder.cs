using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RolePrivilegesDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RolePrivilegesDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RolePrivileges)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RolePrivileges;

        public override int ComponentTypeValue => (int)ComponentType.RolePrivileges;

        public override string EntityLogicalName => RolePrivileges.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RolePrivileges.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet(RolePrivileges.Schema.Attributes.privilegedepthmask, RolePrivileges.Schema.Attributes.ismanaged);
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = RolePrivileges.EntityLogicalName,

                ColumnSet = GetColumnSet(),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(this.EntityPrimaryIdAttribute, ConditionOperator.In, idsNotCached.ToArray()),
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

                        EntityAlias = SupportingSolutionAlias,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },
                },
            };

            return query;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "BusinessUnit", "Privilege", "PrivilegeDepthMask", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<RolePrivileges>();

            List<string> values = new List<string>();

            var businessUnit = EntityDescriptionHandler.GetAttributeString(entity, "role.businessunitid");

            if (!entity.Attributes.Contains("businessunit.parentbusinessunitid"))
            {
                businessUnit = "Root Organization";
            }

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, "role.name")
                , businessUnit
                , EntityDescriptionHandler.GetAttributeString(entity, "privilege.name")
                , RolePrivilegesRepository.GetPrivilegeDepthMaskName(entity.PrivilegeDepthMask.Value)
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RolePrivileges>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.GetAttributeValue<AliasedValue>("privilege.name")?.Value?.ToString();
            }

            return base.GetName(component);
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<RolePrivileges>(component.ObjectId.Value);

            if (entity != null)
            {
                return RolePrivilegesRepository.GetPrivilegeDepthMaskName(entity.ToEntity<RolePrivileges>().PrivilegeDepthMask.Value);
            }

            return base.GetDisplayName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { "role.name", "Name" }
                    , { "role.businessunitid", "BusinessUnit" }
                    , { "privilege.name", "Privilege" }
                    , { RolePrivileges.Schema.Attributes.privilegedepthmask, "PrivilegeDepthMask" }
                    , { RolePrivileges.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}