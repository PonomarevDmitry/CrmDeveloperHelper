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
    public class ReportEntityDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ReportEntityDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ReportEntity)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ReportEntity;

        public override int ComponentTypeValue => (int)ComponentType.ReportEntity;

        public override string EntityLogicalName => ReportEntity.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ReportEntity.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ReportEntity.Schema.Attributes.reportid
                    , ReportEntity.Schema.Attributes.objecttypecode
                    , ReportEntity.Schema.Attributes.ismanaged
                    , ReportEntity.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ReportEntity>(components.Select(c => c.ObjectId));

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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var reportEntity = GetEntity<ReportEntity>(component.ObjectId.Value);

            if (reportEntity != null)
            {
                var reportRef = reportEntity.ReportId;
                string reportName = string.Empty;

                if (reportRef != null)
                {
                    reportName = reportRef.Name;
                }

                return string.Format("Report {0}    ReportRelatedEntity {1}    IsManaged {2}    SolutionName {3}"
                    , reportName
                    , reportEntity.ObjectTypeCode.ToString()
                    , reportEntity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(reportEntity, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<ReportEntity>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.ReportId?.Name, entity.ObjectTypeCode);
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { ReportEntity.Schema.Attributes.reportid, "ReportName" }
                    , { ReportEntity.Schema.Attributes.objecttypecode, "ReportRelatedEntity" }
                    , { ReportEntity.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { ReportEntity.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}