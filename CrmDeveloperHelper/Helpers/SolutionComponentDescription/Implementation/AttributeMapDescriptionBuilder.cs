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
    public class AttributeMapDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public AttributeMapDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.AttributeMap)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.AttributeMap;

        public override int ComponentTypeValue => (int)ComponentType.AttributeMap;

        public override string EntityLogicalName => AttributeMap.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => AttributeMap.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    AttributeMap.Schema.Attributes.sourceattributename
                    , AttributeMap.Schema.Attributes.targetattributename
                    , AttributeMap.Schema.Attributes.ismanaged
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
            handler.SetHeader("Source", "Attribute", "", "Target", "Attribute", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<AttributeMap>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.GetAttributeValue<AliasedValue>("entitymap.sourceentityname").Value.ToString()
                , entity.SourceAttributeName
                , "->"
                , entity.GetAttributeValue<AliasedValue>("entitymap.targetentityname").Value.ToString()
                , entity.TargetAttributeName
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<AttributeMap>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0}.{1} -> {2}.{3}"
                    , entity.GetAttributeValue<AliasedValue>("entitymap.sourceentityname")?.Value?.ToString()
                    , entity.SourceAttributeName
                    , entity.GetAttributeValue<AliasedValue>("entitymap.targetentityname")?.Value?.ToString()
                    , entity.TargetAttributeName
                    );
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { "entitymap.sourceentityname", "SourceEntityName" }
                    , { AttributeMap.Schema.Attributes.sourceattributename, "SourceAttributeName" }
                    , { "entitymap.targetentityname", "TargetEntityName" }
                    , { AttributeMap.Schema.Attributes.targetattributename, "TargetAttributeName" }
                    , { AttributeMap.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}