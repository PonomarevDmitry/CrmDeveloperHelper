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
    public class DisplayStringMapDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public DisplayStringMapDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.DisplayStringMap)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.DisplayStringMap;

        public override int ComponentTypeValue => (int)ComponentType.DisplayStringMap;

        public override string EntityLogicalName => DisplayStringMap.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => DisplayStringMap.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet(DisplayStringMap.Schema.Attributes.objecttypecode, DisplayStringMap.Schema.Attributes.ismanaged);
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
           var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = DisplayStringMap.EntityLogicalName,

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
            var list = GetEntities<DisplayStringMap>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("DisplayStringKey", "ObjectTypeCode", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var displayStringKey = entity.GetAttributeValue<AliasedValue>("displaystring.displaystringkey").Value.ToString();

                handler.AddLine(displayStringKey
                    , entity.ObjectTypeCode
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
            var displayStringMap = GetEntity<DisplayStringMap>(component.ObjectId.Value);

            if (displayStringMap != null)
            {
                var displayStringKey = displayStringMap.GetAttributeValue<AliasedValue>("displaystring.displaystringkey").Value.ToString();

                return string.Format("DisplayStringKey {0}    ObjectTypeCode {1}    IsManaged {2}    SolutionName {3}"
                    , displayStringKey
                    , displayStringMap.ObjectTypeCode
                    , displayStringMap.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(displayStringMap, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<DisplayStringMap>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}"
                    , EntityDescriptionHandler.GetAttributeString(entity, "displaystring.displaystringkey")
                    , entity.ObjectTypeCode
                    );
            }

            return base.GetName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { "displaystring.displaystringkey", "DisplayStringKey" }
                    , { DisplayStringMap.Schema.Attributes.objecttypecode, "Entity" }
                    , { DisplayStringMap.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}