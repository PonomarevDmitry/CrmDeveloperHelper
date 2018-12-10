using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Entity", "RuleType", "RuleId", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<RibbonRule>();

            List<string> values = new List<string>();

            entity.FormattedValues.TryGetValue(RibbonRule.Schema.Attributes.ruletype, out string ruletype);

            values.AddRange(new[]
            {
                entity.Entity ?? "ApplicationRibbon"
                , ruletype
                , entity.RuleId
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
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

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {

        }

        public override void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {

        }

        public override string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<RibbonRule>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.Entity;
            }

            return base.GetLinkedEntityName(solutionComponent);
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