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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("RoutingRuleName", "Name", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<RoutingRuleItem>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.RoutingRuleId?.Name
                , entity.Name
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
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