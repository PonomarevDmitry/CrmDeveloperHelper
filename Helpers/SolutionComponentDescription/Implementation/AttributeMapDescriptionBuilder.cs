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
            var list = GetEntities<AttributeMap>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("Source", "Attribute", "", "Target", "Attribute", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.GetAttributeValue<AliasedValue>("entitymap.sourceentityname").Value.ToString()
                    , entity.SourceAttributeName
                    , "->"
                    , entity.GetAttributeValue<AliasedValue>("entitymap.targetentityname").Value.ToString()
                    , entity.TargetAttributeName
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
            var attributeMap = GetEntity<AttributeMap>(component.ObjectId.Value);

            if (attributeMap != null)
            {
                return string.Format("AttributeMap {0}.{1} -> {2}.{3}    IsManaged {4}    SolutionName {5}"
                    , attributeMap.GetAttributeValue<AliasedValue>("entitymap.sourceentityname").Value.ToString()
                    , attributeMap.SourceAttributeName
                    , attributeMap.GetAttributeValue<AliasedValue>("entitymap.targetentityname").Value.ToString()
                    , attributeMap.TargetAttributeName
                    , attributeMap.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(attributeMap, "solution.uniquename")
                    );
            }

            return component.ToString();
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

            return component.ObjectId.ToString();
        }
    }
}