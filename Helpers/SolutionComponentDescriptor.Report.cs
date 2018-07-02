using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private void GenerateDescriptionReport(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<Report>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("ReportName", "FileName", "ReportType", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "ViewableBy", "Owner", "Url");

            foreach (var entity in list)
            {
                string name = entity.Name;
                string filename = entity.FileName;
                string reportType = entity.FormattedValues.ContainsKey(Report.Schema.Attributes.reporttypecode)
                    ? entity.FormattedValues[Report.Schema.Attributes.reporttypecode] : "Empty";

                var ownerRef = entity.OwnerId;
                string owner = string.Empty;

                if (ownerRef != null)
                {
                    owner = ownerRef.Name;
                }

                string ispersonal = entity.FormattedValues.ContainsKey(Report.Schema.Attributes.ispersonal) ? entity.FormattedValues[Report.Schema.Attributes.ispersonal] : "Empty";

                table.AddLine(name
                    , filename
                    , reportType
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , ispersonal
                    , owner
                    , _withUrls ? _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.Report, entity.Id, null, null) : string.Empty
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionReportSingle(Report entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string name = entity.Name;
                string fileName = entity.FileName;

                StringBuilder builder = new StringBuilder();

                builder.AppendFormat("Report {0}", name);

                if (!string.IsNullOrEmpty(fileName))
                {
                    builder.AppendFormat("    {0}", fileName);
                }

                builder.AppendFormat("    IsManaged {0}", entity.IsManaged.ToString());
                builder.AppendFormat("    SolutionName {0}", EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename"));

                if (_withUrls)
                {
                    builder.AppendFormat("    Url {0}", _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.Report, entity.Id, null, null));
                }

                return builder.ToString();
            }

            return component.ToString();
        }

        private void GenerateDescriptionReportEntity(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ReportEntity>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("ReportName", "ReportRelatedEntity", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var reportRef = entity.ReportId;
                string reportName = string.Empty;

                if (reportRef != null)
                {
                    reportName = reportRef.Name;
                }

                table.AddLine(reportName
                    , entity.ObjectTypeCode.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionReportEntitySingle(ReportEntity entity, SolutionComponent component)
        {
            if (entity != null)
            {
                var reportRef = entity.ReportId;
                string reportName = string.Empty;

                if (reportRef != null)
                {
                    reportName = reportRef.Name;
                }

                return string.Format("Report {0}    ReportRelatedEntity {1}    IsManaged {2}    SolutionName {3}"
                    , reportName
                    , entity.ObjectTypeCode.ToString()
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionReportCategory(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ReportCategory>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("ReportName", "Category", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var reportRef = entity.ReportId;
                string reportName = string.Empty;

                if (reportRef != null)
                {
                    reportName = reportRef.Name;
                }

                table.AddLine(reportName
                    , entity.FormattedValues[ReportCategory.Schema.Attributes.categorycode]
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionReportCategorySingle(ReportCategory entity, SolutionComponent component)
        {
            if (entity != null)
            {
                var reportRef = entity.ReportId;
                string reportName = string.Empty;

                if (reportRef != null)
                {
                    reportName = reportRef.Name;
                }

                return string.Format("Report {0}    Category {1}    IsManaged {2}    SolutionName {3}"
                    , reportName
                    , entity.FormattedValues[ReportCategory.Schema.Attributes.categorycode]
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionReportVisibility(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<ReportVisibility>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            table.SetHeader("ReportName", "Visibility", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

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
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionReportVisibilitySingle(ReportVisibility entity, SolutionComponent component)
        {
            if (entity != null)
            {
                var reportRef = entity.ReportId;
                string reportName = string.Empty;

                if (reportRef != null)
                {
                    reportName = reportRef.Name;
                }

                return string.Format("Report {0}    Visibility {1}    IsManaged {2}    SolutionName {3}"
                    , reportName
                    , entity.FormattedValues[ReportVisibility.Schema.Attributes.visibilitycode]
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }
    }
}
