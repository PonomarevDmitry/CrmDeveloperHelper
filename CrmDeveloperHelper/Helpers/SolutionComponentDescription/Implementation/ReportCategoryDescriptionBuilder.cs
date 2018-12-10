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
    public class ReportCategoryDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ReportCategoryDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ReportCategory)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ReportCategory;

        public override int ComponentTypeValue => (int)ComponentType.ReportCategory;

        public override string EntityLogicalName => ReportCategory.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ReportCategory.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ReportCategory.Schema.Attributes.reportid
                    , ReportCategory.Schema.Attributes.categorycode
                    , ReportCategory.Schema.Attributes.ismanaged
                    , ReportCategory.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("ReportName", "Category", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ReportCategory>();

            List<string> values = new List<string>();

            entity.FormattedValues.TryGetValue(ReportCategory.Schema.Attributes.categorycode, out string categorycode);

            values.AddRange(new[]
            {
                entity.ReportId?.Name
                , categorycode
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<ReportCategory>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.ReportId?.Name, entity.FormattedValues.ContainsKey(ReportCategory.Schema.Attributes.categorycode) ? entity.FormattedValues[ReportCategory.Schema.Attributes.categorycode] : entity.CategoryCode?.Value.ToString());
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
                    { ReportCategory.Schema.Attributes.reportid, "ReportName" }
                    , { ReportCategory.Schema.Attributes.categorycode, "Category" }
                    , { ReportCategory.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { ReportCategory.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}