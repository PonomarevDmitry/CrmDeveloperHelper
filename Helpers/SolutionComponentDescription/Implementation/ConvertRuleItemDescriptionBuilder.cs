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
    public class ConvertRuleItemDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ConvertRuleItemDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ConvertRuleItem)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ConvertRuleItem;

        public override int ComponentTypeValue => (int)ComponentType.ConvertRuleItem;

        public override string EntityLogicalName => ConvertRuleItem.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ConvertRuleItem.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ConvertRuleItem.Schema.Attributes.convertruleid
                    , ConvertRuleItem.Schema.Attributes.name
                    , ConvertRuleItem.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("ConvertRuleName", "Name", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ConvertRuleItem>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.ConvertRuleId?.Name
                , entity.Name
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { ConvertRuleItem.Schema.Attributes.convertruleid, "ConvertRuleName" }
                    , { ConvertRuleItem.Schema.Attributes.name, "Name" }
                    , { ConvertRuleItem.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}