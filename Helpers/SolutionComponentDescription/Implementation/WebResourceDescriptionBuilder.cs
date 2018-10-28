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

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<WebResource>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("WebResourceType", "Name", "DisplayName", "IsManaged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

            foreach (var webResource in list)
            {
                string webTypeName = string.Format("'{0}'", webResource.FormattedValues[WebResource.Schema.Attributes.webresourcetype]);

                handler.AddLine(webTypeName
                    , webResource.Name
                    , webResource.DisplayName
                    , webResource.IsManaged.ToString()
                    , webResource.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(webResource, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(webResource, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(webResource, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(webResource, "suppsolution.ismanaged")
                    , withUrls ? _service.UrlGenerator?.GetSolutionComponentUrl(ComponentType.WebResource, webResource.Id) : string.Empty
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var webResource = GetEntity<WebResource>(component.ObjectId.Value);

            if (webResource != null)
            {
                string webTypeName = string.Format("'{0}'", webResource.FormattedValues[WebResource.Schema.Attributes.webresourcetype]);

                return string.Format("WebResource     '{0}'    WebResourceType '{1}'    DisplayName     '{2}'    IsManaged {3}    IsCustomizable {4}    SolutionName {5}{6}"
                    , webResource.Name
                    , webTypeName
                    , webResource.DisplayName
                    , webResource.IsManaged.ToString()
                    , webResource.IsCustomizable?.Value.ToString()
                    , EntityDescriptionHandler.GetAttributeString(webResource, "solution.uniquename")
                    , withUrls ? string.Format("    Url {0}", _service.UrlGenerator.GetSolutionComponentUrl(ComponentType.WebResource, webResource.Id)) : string.Empty
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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

                    Description = GenerateDescriptionSingle(solutionComponent, false),
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

            if (!string.IsNullOrEmpty(solutionImageComponent.SchemaName))
            {
                string webResourceName = solutionImageComponent.SchemaName;

                var repository = new WebResourceRepository(_service);

                var entity = repository.FindByExactName(webResourceName, new ColumnSet(false));

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