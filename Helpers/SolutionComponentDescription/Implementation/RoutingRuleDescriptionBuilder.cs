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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "Workflow", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<RoutingRule>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.WorkflowId?.Name
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
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