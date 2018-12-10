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
    public class HierarchyRuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public HierarchyRuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.HierarchyRule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.HierarchyRule;

        public override int ComponentTypeValue => (int)ComponentType.HierarchyRule;

        public override string EntityLogicalName => HierarchyRule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => HierarchyRule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    HierarchyRule.Schema.Attributes.primaryentitylogicalname
                    , HierarchyRule.Schema.Attributes.name
                    , HierarchyRule.Schema.Attributes.description
                    , HierarchyRule.Schema.Attributes.ismanaged
                    , HierarchyRule.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("PrimaryEntityLogicalName", "Name", "Description", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<HierarchyRule>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.PrimaryEntityLogicalName
                , entity.Name
                , entity.Description
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { HierarchyRule.Schema.Attributes.primaryentitylogicalname, "Entity" }
                    , { HierarchyRule.Schema.Attributes.name, "Name" }
                    , { HierarchyRule.Schema.Attributes.description, "Description" }
                    , { HierarchyRule.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { HierarchyRule.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}