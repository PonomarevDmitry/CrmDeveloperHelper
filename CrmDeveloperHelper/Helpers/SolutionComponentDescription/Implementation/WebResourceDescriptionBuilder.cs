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
    public class WebResourceDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public WebResourceDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.WebResource)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.WebResource;

        public override int ComponentTypeValue => (int)ComponentType.WebResource;

        public override string EntityLogicalName => WebResource.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => WebResource.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    WebResource.Schema.Attributes.name
                    , WebResource.Schema.Attributes.displayname
                    , WebResource.Schema.Attributes.webresourcetype
                    , WebResource.Schema.Attributes.ismanaged
                    , WebResource.Schema.Attributes.iscustomizable
                );
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("WebResourceType", "Name", "DisplayName", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<WebResource>();

            List<string> values = new List<string>();

            entity.FormattedValues.TryGetValue(WebResource.Schema.Attributes.webresourcetype, out var webresourcetype);

            values.AddRange(new[]
            {
                webresourcetype
                , entity.Name
                , entity.DisplayName
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<WebResource>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.DisplayName;
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

            var entity = GetEntity<WebResource>(solutionComponent.ObjectId.Value);

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

        private void FillSolutionComponentFromSchemaName(ICollection<SolutionComponent> result, string webResourceName, int? behavior)
        {
            if (string.IsNullOrEmpty(webResourceName))
            {
                return;
            }

            var repository = new WebResourceRepository(_service);

            var entity = repository.FindByExactName(webResourceName, new ColumnSet(false));

            if (entity != null)
            {
                FillSolutionComponentInternal(result, entity.Id, behavior);
            }
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
            {
                { WebResource.Schema.Attributes.name, "Name" }
                , { WebResource.Schema.Attributes.displayname, "DisplayName" }
                , { WebResource.Schema.Attributes.webresourcetype, "Type" }
                , { WebResource.Schema.Attributes.languagecode, "LanguageCode" }
                , { WebResource.Schema.Attributes.ismanaged, "IsManaged" }
                , { WebResource.Schema.Attributes.iscustomizable, "IsCustomizable" }
                , { "solution.uniquename", "SolutionName" }
                , { "solution.ismanaged", "SolutionIsManaged" }
                , { "suppsolution.uniquename", "SupportingName" }
                , { "suppsolution.ismanaged", "SupportingIsManaged" }
            };
        }
    }
}