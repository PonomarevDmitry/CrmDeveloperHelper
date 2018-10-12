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
    public class RoutingRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RoutingRuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RoutingRule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RoutingRule;

        public override int ComponentTypeValue => (int)ComponentType.RoutingRule;

        public override string EntityLogicalName => RoutingRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RoutingRule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RoutingRule.Schema.Attributes.name
                    , RoutingRule.Schema.Attributes.workflowid
                    , RoutingRule.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RoutingRule>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            var table = new FormatTextTableHandler();
            table.SetHeader("Name", "Workflow", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var name = entity.Name;

                table.AddLine(name
                    , entity.WorkflowId?.Name
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
            var routingRule = GetEntity<RoutingRule>(component.ObjectId.Value);

            if (routingRule != null)
            {
                string name = routingRule.Name;

                StringBuilder title = new StringBuilder();

                title.AppendFormat("RoutingRule {0}", name);

                if (routingRule.WorkflowId != null)
                {
                    title.AppendFormat("    Workflow '{0}'", routingRule.WorkflowId?.Name);
                }

                title.AppendFormat("    IsManaged '{0}'", routingRule.IsManaged.ToString());

                title.AppendFormat("    SolutionName '{0}'", EntityDescriptionHandler.GetAttributeString(routingRule, "solution.uniquename"));

                return title.ToString();
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { RoutingRule.Schema.Attributes.name, "Name" }
                    , { RoutingRule.Schema.Attributes.workflowid, "Workflow" }
                    , { RoutingRule.Schema.Attributes.statuscode, "StatusCode" }
                    , { RoutingRule.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}