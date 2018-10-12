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
    public class RoutingRuleItemDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RoutingRuleItemDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RoutingRuleItem)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RoutingRuleItem;

        public override int ComponentTypeValue => (int)ComponentType.RoutingRuleItem;

        public override string EntityLogicalName => RoutingRuleItem.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RoutingRuleItem.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RoutingRuleItem.Schema.Attributes.routingruleid
                    , RoutingRuleItem.Schema.Attributes.name
                    , RoutingRuleItem.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RoutingRuleItem>(components.Select(c => c.ObjectId));

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
            table.SetHeader("RoutingRuleName", "Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string ruleName = entity.RoutingRuleId.Name;
                string name = entity.Name;

                table.AddLine(
                    ruleName
                    , name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var routingRuleItem = GetEntity<RoutingRuleItem>(component.ObjectId.Value);

            if (routingRuleItem != null)
            {
                string ruleName = routingRuleItem.RoutingRuleId.Name;
                string name = routingRuleItem.Name;

                return string.Format("RoutingRuleName {0}    Name {1}    IsManaged {2}    SolutionName {3}"
                    , ruleName
                    , name
                    , routingRuleItem.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(routingRuleItem, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RoutingRuleItem>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.RoutingRuleId?.Name, entity.Name);
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  RoutingRuleItem.Schema.Attributes.routingruleid, "RoutingRuleName" }
                    , { RoutingRuleItem.Schema.Attributes.name, "Name" }
                    , { RoutingRuleItem.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}