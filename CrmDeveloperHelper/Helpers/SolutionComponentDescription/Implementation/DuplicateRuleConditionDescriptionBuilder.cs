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
    public class DuplicateRuleConditionDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public DuplicateRuleConditionDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.DuplicateRuleCondition)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.DuplicateRuleCondition;

        public override int ComponentTypeValue => (int)ComponentType.DuplicateRuleCondition;

        public override string EntityLogicalName => DuplicateRuleCondition.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => DuplicateRuleCondition.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    DuplicateRuleCondition.Schema.Attributes.regardingobjectid
                    , DuplicateRuleCondition.Schema.Attributes.baseattributename
                    , DuplicateRuleCondition.Schema.Attributes.matchingattributename
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("DuplicateRule", "BaseAttributeName", "MatchingAttributeName", "Behavior");

            action(handler, withUrls, false, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<DuplicateRuleCondition>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.RegardingObjectId?.Name
                , entity.BaseAttributeName
                , entity.MatchingAttributeName
                , behavior
            });

            AppendIntoValues(values, entity, withUrls, false, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { DuplicateRuleCondition.Schema.Attributes.regardingobjectid, "DuplicateRule" }
                    , { DuplicateRuleCondition.Schema.Attributes.baseattributename, "BaseAttributeName" }
                    , { DuplicateRuleCondition.Schema.Attributes.matchingattributename, "MatchingAttributeName" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}