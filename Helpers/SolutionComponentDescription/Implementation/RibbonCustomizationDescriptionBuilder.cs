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
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class RibbonCustomizationDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public RibbonCustomizationDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.RibbonCustomization)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.RibbonCustomization;

        public override int ComponentTypeValue => (int)ComponentType.RibbonCustomization;

        public override string EntityLogicalName => RibbonCustomization.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => RibbonCustomization.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    RibbonCustomization.Schema.Attributes.entity
                    , RibbonCustomization.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Entity", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<RibbonCustomization>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Entity ?? "ApplicationRibbon"
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<RibbonCustomization>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.ToEntity<RibbonCustomization>().Entity ?? "ApplicationRibbon";
            }

            return base.GetName(component);
        }

        public override void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            if (solutionComponent == null
                || !solutionComponent.ObjectId.HasValue
                )
            {
                return;
            }

            var entity = GetEntity<RibbonCustomization>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                var repository = new RibbonCustomizationRepository(_service);

                result.Add(new SolutionImageComponent()
                {
                    SchemaName = string.Format("{0}:RibbonDiffXml", entity.Entity),
                    ComponentType = (solutionComponent.ComponentType?.Value).GetValueOrDefault(),
                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                });
            }
        }

        public override void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null)
            {
                return;
            }

            string schemaName = solutionImageComponent.SchemaName;
            int? behavior = solutionImageComponent.RootComponentBehavior;

            FillSolutionComponentFromSchemaName(result, schemaName, behavior);
        }

        public override void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            var schemaName = GetSchemaNameFromXml(elementRootComponent);
            var behavior = GetBehaviorFromXml(elementRootComponent);

            FillSolutionComponentFromSchemaName(result, schemaName, behavior);
        }

        private void FillSolutionComponentFromSchemaName(ICollection<SolutionComponent> result, string schemaName, int? behavior)
        {
            if (string.IsNullOrEmpty(schemaName))
            {
                return;
            }

            if (string.Equals(schemaName, ":RibbonDiffXml", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = new RibbonCustomizationRepository(_service);

                var entity = repository.FindApplicationRibbonCustomization();

                if (entity != null)
                {
                    FillSolutionComponentInternal(result, entity.Id, behavior);
                }
            }
        }

        public override string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<RibbonCustomization>(solutionComponent.ObjectId.Value);

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
                    { RibbonCustomization.Schema.Attributes.entity, "Entity" }
                    , { RibbonCustomization.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}