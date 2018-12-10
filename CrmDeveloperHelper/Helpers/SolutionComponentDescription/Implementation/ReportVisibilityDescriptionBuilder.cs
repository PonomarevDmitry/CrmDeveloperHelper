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
    public class ReportVisibilityDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ReportVisibilityDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ReportVisibility)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ReportVisibility;

        public override int ComponentTypeValue => (int)ComponentType.ReportVisibility;

        public override string EntityLogicalName => ReportVisibility.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ReportVisibility.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ReportVisibility.Schema.Attributes.reportid
                    , ReportVisibility.Schema.Attributes.visibilitycode
                    , ReportVisibility.Schema.Attributes.ismanaged
                    , ReportVisibility.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("ReportName", "Visibility", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ReportVisibility>();

            List<string> values = new List<string>();

            entity.FormattedValues.TryGetValue(ReportVisibility.Schema.Attributes.visibilitycode, out string visibilitycode);

            values.AddRange(new[]
            {
                entity.ReportId?.Name
                , visibilitycode
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<ReportVisibility>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.ReportId?.Name, entity.FormattedValues.ContainsKey(ReportVisibility.Schema.Attributes.visibilitycode) ? entity.FormattedValues[ReportVisibility.Schema.Attributes.visibilitycode] : entity.VisibilityCode?.Value.ToString());
            }

            return base.GetName(component);
        }

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            
        }

        public override void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {

        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  ReportVisibility.Schema.Attributes.reportid, "ReportName" }
                    , { ReportVisibility.Schema.Attributes.visibilitycode, "Visibility" }
                    , { ReportVisibility.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { ReportVisibility.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}