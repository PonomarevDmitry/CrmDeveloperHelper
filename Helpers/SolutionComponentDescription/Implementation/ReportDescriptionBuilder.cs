using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class ReportDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ReportDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.Report)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.Report;

        public override int ComponentTypeValue => (int)ComponentType.Report;

        public override string EntityLogicalName => Report.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => Report.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    Report.Schema.Attributes.name
                    , Report.Schema.Attributes.filename
                    , Report.Schema.Attributes.reporttypecode
                    , Report.Schema.Attributes.ispersonal
                    , Report.Schema.Attributes.ownerid
                    , Report.Schema.Attributes.ismanaged
                    , Report.Schema.Attributes.iscustomizable
                    , Report.Schema.Attributes.signaturelcid
                    , Report.Schema.Attributes.signatureid
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<Report>(components.Select(c => c.ObjectId));

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
            table.SetHeader("ReportName", "FileName", "ReportType", "SignatureLcid", "SignatureLcid", "IsManaged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "ViewableBy", "Owner", "Url");

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
                    , entity.SignatureLcid.ToString()
                    , entity.SignatureId.ToString()
                    , entity.IsManaged.ToString()
                    , entity.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , ispersonal
                    , owner
                    , withUrls ? _service.UrlGenerator.GetSolutionComponentUrl(ComponentType.Report, entity.Id) : string.Empty
                    );
            }

            List<string> lines = table.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var report = GetEntity<Report>(component.ObjectId.Value);

            if (report != null)
            {
                string name = report.Name;
                string fileName = report.FileName;

                StringBuilder builder = new StringBuilder();

                builder.AppendFormat("Report {0}", name);

                if (!string.IsNullOrEmpty(fileName))
                {
                    builder.AppendFormat("    {0}", fileName);
                }

                builder.AppendFormat("    SignatureLcid {0}", report.SignatureLcid.ToString());
                builder.AppendFormat("    SignatureId {0}", report.SignatureId.ToString());

                builder.AppendFormat("    IsManaged {0}", report.IsManaged.ToString());
                builder.AppendFormat("    IsCustomizable {0}", report.IsCustomizable?.Value.ToString());
                builder.AppendFormat("    SolutionName {0}", EntityDescriptionHandler.GetAttributeString(report, "solution.uniquename"));

                if (withUrls)
                {
                    builder.AppendFormat("    Url {0}", _service.UrlGenerator.GetSolutionComponentUrl(ComponentType.Report, report.Id));
                }

                return builder.ToString();
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<Report>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.FileName;
            }

            return base.GetDisplayName(component);
        }

        public override void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            if (solutionComponent == null
                || !solutionComponent.ObjectId.HasValue
                )
            {
                return;
            }

            var entity = GetEntity<Report>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                var imageComponent = new SolutionImageComponent()
                {
                    ComponentType = (int)ComponentType.Report,
                    ObjectId = solutionComponent.ObjectId.Value,

                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, false),
                };

                if (entity.SignatureId.HasValue && entity.SignatureLcid.HasValue)
                {
                    imageComponent.SchemaName = string.Format("{0}{1:B}", entity.SignatureLcid, entity.SignatureId);
                }

                result.Add(imageComponent);
            }
        }

        private static readonly Regex _regexSchemaName = new Regex(@"^([0-9]{1,4})({[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}})$", RegexOptions.Compiled);

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null)
            {
                return;
            }

            if (!String.IsNullOrEmpty(solutionImageComponent.SchemaName))
            {
                var match = _regexSchemaName.Match(solutionImageComponent.SchemaName);

                if (match.Success && match.Groups.Count == 3)
                {
                    string lcidString = match.Groups[1].Value;
                    string signatureIdString = match.Groups[2].Value;

                    if (!string.IsNullOrEmpty(lcidString)
                        && int.TryParse(lcidString, out var lcid)
                        && !string.IsNullOrEmpty(signatureIdString)
                        && Guid.TryParse(signatureIdString, out var signatureId)
                        )
                    {
                        var repository = new ReportRepository(_service);

                        var entity = repository.FindReportBySignature(lcid, signatureId, new ColumnSet(false));

                        if (entity != null)
                        {
                            var component = new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue(this.ComponentTypeValue),
                                ObjectId = entity.Id,
                                RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                            };

                            if (solutionImageComponent.RootComponentBehavior.HasValue)
                            {
                                component.RootComponentBehavior = new OptionSetValue(solutionImageComponent.RootComponentBehavior.Value);
                            }

                            result.Add(component);

                            return;
                        }
                    }
                }
            }

            base.FillSolutionComponent(result, solutionImageComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { Report.Schema.Attributes.name, "Name" }
                    , { Report.Schema.Attributes.filename, "FileName" }
                    , { Report.Schema.Attributes.reporttypecode, "ReportType" }
                    , { Report.Schema.Attributes.ispersonal, "ViewableBy" }
                    , { Report.Schema.Attributes.ownerid, "Owner" }
                    , { Report.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { Report.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}