using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class AppModuleDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public AppModuleDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.AppModule)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.AppModule;

        public override int ComponentTypeValue => (int)ComponentType.AppModule;

        public override string EntityLogicalName => AppModule.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => AppModule.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    AppModule.Schema.Attributes.name
                    , AppModule.Schema.Attributes.uniquename
                    , AppModule.Schema.Attributes.url
                    , AppModule.Schema.Attributes.appmoduleversion
                    , AppModule.Schema.Attributes.ismanaged
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "UniqueName", "URL", "AppModuleVersion", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<AppModule>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                entity.Name
                , entity.UniqueName
                , entity.URL
                , entity.AppModuleVersion
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<AppModule>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.UniqueName ?? entity.Id.ToString();
            }

            return base.GetName(component);
        }

        public override string GetDisplayName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<AppModule>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return entity.Name;
            }

            return null;
        }

        public override void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            if (solutionComponent == null
                || !solutionComponent.ObjectId.HasValue
                )
            {
                return;
            }

            var entity = GetEntity<AppModule>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                var imageComponent = new SolutionImageComponent()
                {
                    ComponentType = this.ComponentTypeValue,
                    SchemaName = entity.Name,

                    RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                    Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
                };

                result.Add(imageComponent);
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

        private void FillSolutionComponentFromSchemaName(ICollection<SolutionComponent> result, string AppModuleName, int? behavior)
        {
            if (string.IsNullOrEmpty(AppModuleName))
            {
                return;
            }

            var repository = new AppModuleRepository(_service);

            var entity = repository.FindByExactName(AppModuleName, new ColumnSet(false));

            if (entity != null)
            {
                FillSolutionComponentInternal(result, entity.Id, behavior);
            }
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { AppModule.Schema.Attributes.name, "Name" }
                    , { AppModule.Schema.Attributes.uniquename, "UniqueName" }
                    , { AppModule.Schema.Attributes.url, "URL" }
                    , { AppModule.Schema.Attributes.appmoduleversion, "AppModuleVersion" }
                    , { AppModule.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}