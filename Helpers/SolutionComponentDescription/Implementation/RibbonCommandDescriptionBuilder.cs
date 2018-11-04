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
    public class RibbonCommandDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonCommandDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonCommand)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonCommand;

        public override int ComponentTypeValue => (int)ComponentType.RibbonCommand;

        public override string EntityLogicalName => RibbonCommand.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonCommand.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonCommand.Schema.Attributes.entity
                    , RibbonCommand.Schema.Attributes.command
                    , RibbonCommand.Schema.Attributes.ribboncommandid
                    , RibbonCommand.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Entity", "Command", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<RibbonCommand>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Entity ?? "ApplicationRibbon"
                , entity.Command
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonCommand>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}", entity.Entity ?? "ApplicationRibbon", entity.Command);
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
            var entity = GetEntity<RibbonCommand>(solutionComponent.ObjectId.Value);

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
                    { RibbonCommand.Schema.Attributes.entity, "Entity" }
                    , { RibbonCommand.Schema.Attributes.command, "Command" }
                    , { RibbonCommand.Schema.Attributes.ismanaged, "IsManaged" }
                    , { RibbonCommand.Schema.EntityPrimaryIdAttribute, "" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}