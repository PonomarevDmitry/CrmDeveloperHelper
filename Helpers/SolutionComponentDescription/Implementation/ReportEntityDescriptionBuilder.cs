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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("ReportName", "ReportRelatedEntity", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ReportEntity>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.ReportId?.Name
                , entity.ObjectTypeCode.ToString()
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
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