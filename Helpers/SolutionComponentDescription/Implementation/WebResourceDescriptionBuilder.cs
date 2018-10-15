using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
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
                    , withUrls ? _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.WebResource, webResource.Id, null, null) : string.Empty
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
                    , withUrls ? string.Format("    Url {0}", _service.ConnectionData.GetSolutionComponentUrl(ComponentType.WebResource, webResource.Id, null, null)) : string.Empty
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