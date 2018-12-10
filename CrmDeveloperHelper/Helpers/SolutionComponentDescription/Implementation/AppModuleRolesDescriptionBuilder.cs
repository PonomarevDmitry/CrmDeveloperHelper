using Microsoft.Xrm.Sdk;
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
            };

            return query;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("AppModuleName", "RoleName", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<AppModuleRoles>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.appmoduleid + "." + AppModule.Schema.Attributes.name)
                , EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.roleid + "." + Role.Schema.Attributes.name)
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<AppModuleRoles>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}"
                    , EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.appmoduleid + "." + AppModule.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, AppModuleRoles.Schema.Attributes.roleid + "." + Role.Schema.Attributes.name)
                );
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { AppModuleRoles.Schema.Attributes.appmoduleid + "." + AppModule.Schema.Attributes.name, "AppModuleName" }
                    , { AppModuleRoles.Schema.Attributes.roleid + "." + Role.Schema.Attributes.name, "RoleName" }
                    , { AppModuleRoles.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}