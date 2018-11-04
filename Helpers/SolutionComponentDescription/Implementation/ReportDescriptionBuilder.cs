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
using System.Xml.Linq;

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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("ReportName", "FileName", "ReportType", "SignatureLcid", "SignatureId", "IsCustomizable", "ViewableBy", "Owner", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<Report>();

            List<string> values = new List<string>();

            entity.FormattedValues.TryGetValue(Report.Schema.Attributes.reporttypecode, out string reporttypecode);

            entity.FormattedValues.TryGetValue(Report.Schema.Attributes.ispersonal, out string ispersonal);

            values.AddRange(new[]
            {
                entity.Name
                , entity.FileName
                , reporttypecode
                , entity.SignatureLcid.ToString()
                , entity.SignatureId.ToString()
                , entity.IsCustomizable?.Value.ToString()
                , ispersonal
                , entity.OwnerId?.Name
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
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

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                };

                if (entity.SignatureId.HasValue && entity.SignatureLcid.HasValue)
                {
                    imageComponent.SchemaName = string.Format("{0}{1:B}", entity.SignatureLcid, entity.SignatureId);
                }

                result.Add(imageComponent);
            }
        }

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null)
            {
                return;
            }

            if (FillSolutionComponentFromSchemaName(result, solutionImageComponent.SchemaName, solutionImageComponent.RootComponentBehavior))
            {
                return;
            }

            base.FillSolutionComponent(result, solutionImageComponent);


        }

        public override void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            var schemaName = GetSchemaNameFromXml(elementRootComponent);
            var behavior = GetBehaviorFromXml(elementRootComponent);

            if (FillSolutionComponentFromSchemaName(result, schemaName, behavior))
            {
                return;
            }

            base.FillSolutionComponentFromXml(result, elementRootComponent, docCustomizations);
        }

        private static readonly Regex _regexSchemaName = new Regex(@"^([0-9]{1,4})({[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}})$", RegexOptions.Compiled);

        private bool FillSolutionComponentFromSchemaName(ICollection<SolutionComponent> result, string schemaName, int? behavior)
        {
            if (string.IsNullOrEmpty(schemaName))
            {
                return false;
            }

            var match = _regexSchemaName.Match(schemaName);

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
                        FillSolutionComponentInternal(result, entity.Id, behavior);

                        return true;
                    }
                }
            }

            return false;
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