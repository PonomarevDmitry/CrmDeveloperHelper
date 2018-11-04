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
    public class RibbonDiffDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonDiffDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonDiff)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonDiff;

        public override int ComponentTypeValue => (int)ComponentType.RibbonDiff;

        public override string EntityLogicalName => RibbonDiff.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonDiff.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonDiff.Schema.Attributes.entity
                    , RibbonDiff.Schema.Attributes.diffid
                    , RibbonDiff.Schema.Attributes.tabid
                    , RibbonDiff.Schema.Attributes.contextgroupid
                    , RibbonDiff.Schema.Attributes.sequence
                    , RibbonDiff.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Entity", "DiffId", "DiffType", "TabId", "ContextGroupId", "Sequence", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<RibbonDiff>();

            List<string> values = new List<string>();

            entity.FormattedValues.TryGetValue(RibbonDiff.Schema.Attributes.difftype, out string difftype);

            values.AddRange(new[]
            {
                entity.Entity ?? "ApplicationRibbon"
                , entity.DiffId
                , difftype
                , entity.TabId
                , entity.ContextGroupId?.ToString()
                , entity.Sequence?.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonDiff>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.Entity ?? "ApplicationRibbon", entity.DiffId);
            }

            return base.GetName(component);
        }

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            
        }

        public override void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {

        }

        public override string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<RibbonDiff>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.Entity;
            }

            return base.GetLinkedEntityName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { RibbonDiff.Schema.Attributes.entity, "Entity" }
                    , { RibbonDiff.Schema.Attributes.diffid, "DiffId" }
                    , { RibbonDiff.Schema.Attributes.difftype, "DiffType" }
                    , { RibbonDiff.Schema.Attributes.tabid, "TabId" }
                    , { RibbonDiff.Schema.Attributes.contextgroupid, "ContextGroupId" }
                    , { RibbonDiff.Schema.Attributes.sequence, "Sequence" }
                    , { RibbonDiff.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}