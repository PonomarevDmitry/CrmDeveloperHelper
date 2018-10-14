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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ReportVisibility>(components.Select(c => c.ObjectId));

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
            table.SetHeader("ReportName", "Visibility", "IsManaged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

            foreach (var entity in list)
            {
                var reportRef = entity.ReportId;
                string reportName = string.Empty;

                if (reportRef != null)
                {
                    reportName = reportRef.Name;
                }

                table.AddLine(reportName
                    , entity.FormattedValues[ReportVisibility.Schema.Attributes.visibilitycode]
                    , entity.IsManaged.ToString()
                    , entity.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , withUrls && reportRef != null ? _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.Report, reportRef.Id, null, null) : string.Empty
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var reportVisibility = GetEntity<ReportVisibility>(component.ObjectId.Value);

            if (reportVisibility != null)
            {
                var reportRef = reportVisibility.ReportId;
                string reportName = string.Empty;

                if (reportRef != null)
                {
                    reportName = reportRef.Name;
                }

                return string.Format("Report {0}    Visibility {1}    IsManaged {2}    IsManaged {3}    SolutionName {4}{5}"
                    , reportName
                    , reportVisibility.FormattedValues[ReportVisibility.Schema.Attributes.visibilitycode]
                    , reportVisibility.IsManaged.ToString()
                    , reportVisibility.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(reportVisibility, "solution.uniquename")
                    , withUrls && reportRef != null ? string.Format("    Url {0}", _service.ConnectionData.GetSolutionComponentUrl(ComponentType.Report, reportRef.Id, null, null)) : string.Empty
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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