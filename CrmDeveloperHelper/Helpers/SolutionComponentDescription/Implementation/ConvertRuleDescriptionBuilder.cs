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
    public class ConvertRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ConvertRuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ConvertRule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ConvertRule;

        public override int ComponentTypeValue => (int)ComponentType.ConvertRule;

        public override string EntityLogicalName => ConvertRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ConvertRule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ConvertRule.Schema.Attributes.name
                    , ConvertRule.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ConvertRule>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { ConvertRule.Schema.Attributes.name, "Name" }
                    , { ConvertRule.Schema.Attributes.statuscode, "StatusCode" }
                    , { ConvertRule.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}