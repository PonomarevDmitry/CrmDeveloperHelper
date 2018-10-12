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
    public class RibbonRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonRuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonRule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonRule;

        public override int ComponentTypeValue => (int)ComponentType.RibbonRule;

        public override string EntityLogicalName => RibbonRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonRule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonRule.Schema.Attributes.entity
                    , RibbonRule.Schema.Attributes.ruletype
                    , RibbonRule.Schema.Attributes.ruleid
                    , RibbonRule.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<RibbonRule>(components.Select(c => c.ObjectId));

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

            table.SetHeader("Entity", "RuleType", "RuleId", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string entityName = entity.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                table.AddLine(entityName
                    , entity.FormattedValues[RibbonRule.Schema.Attributes.ruletype]
                    , entity.RuleId
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var ribbonRule = GetEntity<RibbonRule>(component.ObjectId.Value);

            if (ribbonRule != null)
            {
                string entityName = ribbonRule.Entity;

                if (string.IsNullOrEmpty(entityName))
                {
                    entityName = "ApplicationRibbon";
                }

                return string.Format("Entity {0}    RuleType {1}    RuleId {2}    IsManaged {3}    SolutionName {4}"
                    , entityName
                    , ribbonRule.FormattedValues[RibbonRule.Schema.Attributes.ruletype]
                    , ribbonRule.RuleId
                    , ribbonRule.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(ribbonRule, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonRule>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.Entity ?? "ApplicationRibbon", entity.RuleId);
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { RibbonRule.Schema.Attributes.entity, "Entity" }
                    , { RibbonRule.Schema.Attributes.ruletype, "RuleType" }
                    , { RibbonRule.Schema.Attributes.ruleid, "RuleId" }
                    , { RibbonRule.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}