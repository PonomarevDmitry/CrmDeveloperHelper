using Microsoft.Crm.Sdk.Messages;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class PublishActionsRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public PublishActionsRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task PublishAllXmlAsync()
        {
            return Task.Run(() => PublishAllXml());
        }

        private void PublishAllXml()
        {
            var request = new PublishAllXmlRequest();

            var response = (PublishAllXmlResponse)_service.Execute(request);
        }

        private static string FormatElement(object value, string element)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()) || string.IsNullOrEmpty(element))
            {
                return string.Empty;
            }

            return string.Format("<{0}>{1}</{0}>", element, value.ToString());
        }

        private const string _formatPublishWebResources =
@"<importexportxml>
    {0}
</importexportxml>";

        private void ExecutePublishXml(
            string contentEntities = null
            , string contentRibbons = null
            , string contentDashboards = null
            , string contentOptionSets = null
            , string contentSiteMaps = null
            , string contentWebResources = null
            )
        {
            StringBuilder parameterXml = new StringBuilder();

            if (!string.IsNullOrEmpty(contentEntities))
            {
                parameterXml.AppendFormat("<entities>{0}</entities>", contentEntities).AppendLine();
            }

            if (!string.IsNullOrEmpty(contentRibbons))
            {
                parameterXml.AppendFormat("<ribbons>{0}</ribbons>", contentRibbons).AppendLine();
            }

            if (!string.IsNullOrEmpty(contentDashboards))
            {
                parameterXml.AppendFormat("<dashboards>{0}</dashboards>", contentDashboards).AppendLine();
            }

            if (!string.IsNullOrEmpty(contentOptionSets))
            {
                parameterXml.AppendFormat("<optionsets>{0}</optionsets>", contentOptionSets).AppendLine();
            }

            if (!string.IsNullOrEmpty(contentSiteMaps))
            {
                parameterXml.AppendFormat("<sitemaps>{0}</sitemaps>", contentSiteMaps).AppendLine();
            }

            if (!string.IsNullOrEmpty(contentWebResources))
            {
                parameterXml.AppendFormat("<webresources>{0}</webresources>", contentWebResources).AppendLine();
            }

            if (parameterXml.Length == 0)
            {
                return;
            }

            var request = new PublishXmlRequest
            {
                ParameterXml = string.Format(_formatPublishWebResources, parameterXml.ToString()),
            };

            _service.Execute(request);
        }

        public Task PublishWebResourcesAsync(IEnumerable<Guid> idWebResources)
        {
            return Task.Run(() => PublishWebResources(idWebResources));
        }

        public void PublishWebResources(IEnumerable<Guid> idWebResources)
        {
            if (!idWebResources.Any())
            {
                return;
            }

            var content = string.Join("", idWebResources.Select(a => FormatElement(a, "webresource")));

            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            ExecutePublishXml(contentWebResources: content);
        }

        public Task PublishEntitiesAsync(IEnumerable<string> entityNames)
        {
            return Task.Run(() => PublishEntities(entityNames));
        }

        private void PublishEntities(IEnumerable<string> entityNames)
        {
            if (!entityNames.Any())
            {
                return;
            }

            var content = string.Join("", entityNames.Select(a => FormatElement(a, "entity")));

            ExecutePublishXml(contentEntities: content);
        }

        public Task PublishOptionSetsAsync(IEnumerable<string> optionSetNames)
        {
            return Task.Run(() => PublishOptionSets(optionSetNames));
        }

        private void PublishOptionSets(IEnumerable<string> optionSetNames)
        {
            if (!optionSetNames.Any())
            {
                return;
            }

            var content = string.Join("", optionSetNames.Select(a => FormatElement(a, "optionset")));

            ExecutePublishXml(contentOptionSets: content);
        }

        public Task PublishApplicationRibbonAsync()
        {
            return Task.Run(() => PublishApplicationRibbon());
        }

        private void PublishApplicationRibbon()
        {
            var content = FormatElement("ApplicationRibbon", "ribbon");

            ExecutePublishXml(contentRibbons: content);
        }

        public Task PublishSiteMapsAsync(IEnumerable<Guid> idSiteMaps)
        {
            return Task.Run(() => PublishSiteMaps(idSiteMaps));
        }

        private void PublishSiteMaps(IEnumerable<Guid> idSiteMaps)
        {
            if (!idSiteMaps.Any())
            {
                return;
            }

            var content = string.Join("", idSiteMaps.Select(a => FormatElement(a, "sitemap")));

            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            ExecutePublishXml(contentSiteMaps: content);
        }

        public Task PublishDashboardsAsync(IEnumerable<Guid> idDashboards)
        {
            return Task.Run(() => PublishDashboards(idDashboards));
        }

        private void PublishDashboards(IEnumerable<Guid> idDashboards)
        {
            if (!idDashboards.Any())
            {
                return;
            }

            var content = string.Join("", idDashboards.Select(a => FormatElement(a, "dashboard")));

            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            ExecutePublishXml(contentDashboards: content);
        }
    }
}