using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RoleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RoleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.Role)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.Role;

        public override int ComponentTypeValue => (int)ComponentType.Role;

        public override string EntityLogicalName => Role.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => Role.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet(Role.Schema.Attributes.name, Role.Schema.Attributes.businessunitid, Role.Schema.Attributes.ismanaged, Role.Schema.Attributes.iscustomizable);
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Role.EntityLogicalName,

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

                        EntityAlias = SupportingSolutionAlias,

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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<Role>(components.Select(c => c.ObjectId));

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
            table.SetHeader("Name", "BusinessUnit", "IsManaged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

            foreach (var role in list)
            {
                var businessUnit = role.BusinessUnitId.Name;

                if (role.BusinessUnitParentBusinessUnit == null)
                {
                    businessUnit = "Root Organization";
                }

                table.AddLine(role.Name
                    , businessUnit
                    , role.IsManaged.ToString()
                    , role.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(role, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(role, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(role, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(role, "suppsolution.ismanaged")
                    , withUrls ? _service.UrlGenerator.GetSolutionComponentUrl(ComponentType.Role, role.Id) : string.Empty
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var role = GetEntity<Role>(component.ObjectId.Value);

            if (role != null)
            {
                var businessUnit = role.BusinessUnitId.Name;

                if (!role.Attributes.Contains("businessunit.parentbusinessunitid"))
                {
                    businessUnit = "Root Organization";
                }

                return string.Format("Role {0}    BusinessUnit {1}    IsManaged {2}    IsManaged {3}    SolutionName {4}"
                    , role.Name
                    , businessUnit
                    , role.IsManaged.ToString()
                    , role.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(role, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  Role.Schema.Attributes.name, "Name" }
                    , { Role.Schema.Attributes.businessunitid, "BusinessUnit" }
                    , { Role.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { Role.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}