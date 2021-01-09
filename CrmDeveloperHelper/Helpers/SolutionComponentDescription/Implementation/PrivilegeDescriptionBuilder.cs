using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class PrivilegeDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public PrivilegeDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.Privilege)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.Privilege;

        public override int ComponentTypeValue => (int)ComponentType.Privilege;

        public override string EntityLogicalName => Privilege.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => Privilege.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet(
                Privilege.Schema.Attributes.privilegeid
                , Privilege.Schema.Attributes.name
                , Privilege.Schema.Attributes.accessright
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
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Privilege.EntityLogicalName,
                        LinkFromAttributeName = Privilege.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = Privilege.EntityLogicalName,
                        LinkFromAttributeName = Privilege.Schema.Attributes.supportingsolutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

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
            handler.SetHeader(Privilege.Schema.Headers.name, Privilege.Schema.Headers.accessright, "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<Privilege>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.AccessRight.HasValue ? ((Microsoft.Crm.Sdk.Messages.AccessRights)entity.AccessRight.Value).ToString() : string.Empty
                , behavior
            });

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<Privilege>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.Name;
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { Privilege.Schema.Attributes.name, Privilege.Schema.Headers.name }
                    , { Privilege.Schema.Attributes.accessright, Privilege.Schema.Headers.accessright }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}