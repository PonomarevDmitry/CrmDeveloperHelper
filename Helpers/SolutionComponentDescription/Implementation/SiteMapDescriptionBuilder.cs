using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class SiteMapDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public SiteMapDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.SiteMap)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.SiteMap;

        public override int ComponentTypeValue => (int)ComponentType.SiteMap;

        public override string EntityLogicalName => SiteMap.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => SiteMap.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    SiteMap.Schema.Attributes.sitemapname
                    , SiteMap.Schema.Attributes.sitemapnameunique
                    , SiteMap.Schema.Attributes.sitemapid
                    , SiteMap.Schema.Attributes.isappaware
                    , SiteMap.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<SiteMap>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("SiteMapName", "Id", "IsAppAware", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var sitemap in list)
            {
                string name = sitemap.SiteMapName;

                handler.AddLine(
                    name
                    , sitemap.Id.ToString()
                    , sitemap.IsAppAware.ToString()
                    , sitemap.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(sitemap, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(sitemap, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(sitemap, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(sitemap, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var siteMap = GetEntity<SiteMap>(component.ObjectId.Value);

            if (siteMap != null)
            {
                string name = siteMap.SiteMapName;

                return string.Format("SiteMapName {0}    Id {1}    IsAppAware {2}    IsManaged {3}    SolutionName {4}"
                    , name
                    , siteMap.Id.ToString()
                    , siteMap.IsAppAware.ToString()
                    , siteMap.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(siteMap, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<SdkMessageProcessingStepImage>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.EntityAlias;
            }

            return component.ObjectId.ToString();
        }
    }
}