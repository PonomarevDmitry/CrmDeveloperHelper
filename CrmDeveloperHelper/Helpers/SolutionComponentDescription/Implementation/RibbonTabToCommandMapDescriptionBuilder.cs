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
    public class RibbonTabToCommandMapDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonTabToCommandMapDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonTabToCommandMap)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonTabToCommandMap;

        public override int ComponentTypeValue => (int)ComponentType.RibbonTabToCommandMap;

        public override string EntityLogicalName => RibbonTabToCommandMap.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonTabToCommandMap.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonTabToCommandMap.Schema.Attributes.entity
                    , RibbonTabToCommandMap.Schema.Attributes.tabid
                    , RibbonTabToCommandMap.Schema.Attributes.controlid
                    , RibbonTabToCommandMap.Schema.Attributes.command
                    , RibbonTabToCommandMap.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Entity", "TabId", "ControlId", "Command", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<RibbonTabToCommandMap>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Entity ?? "ApplicationRibbon"
                , entity.TabId
                , entity.ControlId
                , entity.Command
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonTabToCommandMap>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.Entity ?? "ApplicationRibbon", entity.TabId);
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
            var entity = GetEntity<RibbonTabToCommandMap>(solutionComponent.ObjectId.Value);

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
                    { RibbonTabToCommandMap.Schema.Attributes.entity, "Entity" }
                    , { RibbonTabToCommandMap.Schema.Attributes.tabid, "TabId" }
                    , { RibbonTabToCommandMap.Schema.Attributes.controlid, "ControlId" }
                    , { RibbonTabToCommandMap.Schema.Attributes.command, "Command" }
                    , { RibbonTabToCommandMap.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}