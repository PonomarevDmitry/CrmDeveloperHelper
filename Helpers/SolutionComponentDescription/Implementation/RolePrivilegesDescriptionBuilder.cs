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

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            return query;
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RolePrivileges>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("Name", "BusinessUnit", "Privilege", "PrivilegeDepthMask", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var rolePriv in list.Select(e => e.ToEntity<RolePrivileges>()))
            {
                var roleName = rolePriv.GetAttributeValue<AliasedValue>("role.name").Value.ToString();
                var businessUnit = ((EntityReference)rolePriv.GetAttributeValue<AliasedValue>("role.businessunitid").Value).Name;

                if (!rolePriv.Attributes.Contains("businessunit.parentbusinessunitid"))
                {
                    businessUnit = "Root Organization";
                }

                table.AddLine(roleName
                    , businessUnit
                    , rolePriv.GetAttributeValue<AliasedValue>("privilege.name").Value.ToString()
                    , SecurityRolePrivilegesRepository.GetPrivilegeDepthMaskName(rolePriv.PrivilegeDepthMask.Value)
                    , rolePriv.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var rolePrivileges = GetEntity<RolePrivileges>(component.ObjectId.Value);

            if (rolePrivileges != null)
            {
                var roleName = rolePrivileges.GetAttributeValue<AliasedValue>("role.name").Value.ToString();
                var businessUnit = ((EntityReference)rolePrivileges.GetAttributeValue<AliasedValue>("role.businessunitid").Value).Name;

                if (!rolePrivileges.Attributes.Contains("businessunit.parentbusinessunitid"))
                {
                    businessUnit = "Root Organization";
                }

                return string.Format("Role {0}    BusinessUnit {1}    Privilege {2}    PrivilegeDepthMask {3}    IsManaged {4}    SolutionName {5}"
                    , roleName
                    , businessUnit
                    , rolePrivileges.GetAttributeValue<AliasedValue>("privilege.name").Value.ToString()
                    , SecurityRolePrivilegesRepository.GetPrivilegeDepthMaskName(rolePrivileges.PrivilegeDepthMask.Value)
                    , rolePrivileges.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(rolePrivileges, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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
                return SecurityRolePrivilegesRepository.GetPrivilegeDepthMaskName(entity.ToEntity<RolePrivileges>().PrivilegeDepthMask.Value);
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