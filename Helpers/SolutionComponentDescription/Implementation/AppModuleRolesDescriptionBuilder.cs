using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class AppModuleRolesDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public AppModuleRolesDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.AppModuleRoles)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.AppModuleRoles;

        public override int ComponentTypeValue => (int)ComponentType.AppModuleRoles;

        public override string EntityLogicalName => AppModuleRoles.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => AppModuleRoles.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    AppModuleRoles.Schema.Attributes.appmoduleid
                    , AppModuleRoles.Schema.Attributes.roleid
                    , AppModuleRoles.Schema.Attributes.ismanaged
                );
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = this.EntityLogicalName,

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
            var list = GetEntities<AppModuleRoles>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("AppModuleName", "RoleName", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.appmoduleid + "." + AppModule.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.roleid + "." + Role.Schema.Attributes.name)
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var appModuleRoles = GetEntity<AppModuleRoles>(component.ObjectId.Value);

            if (appModuleRoles != null)
            {
                return string.Format("AppModuleName {0}    RoleName {1}    IsManaged {2}    SolutionName {3}"
                    , EntityDescriptionHandler.GetAttributeString(appModuleRoles, AppModuleRoles.Schema.Attributes.appmoduleid + "." + AppModule.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(appModuleRoles, AppModuleRoles.Schema.Attributes.roleid + "." + Role.Schema.Attributes.name)
                    , appModuleRoles.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(appModuleRoles, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<AppModuleRoles>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}"
                    , entity.GetAttributeValue<AliasedValue>("entitymap.sourceentityname")?.Value?.ToString()
                    , entity.GetAttributeValue<AliasedValue>("entitymap.targetentityname")?.Value?.ToString()
                    );
            }

            return component.ObjectId.ToString();
        }
    }
}