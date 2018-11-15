using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class WebResourceIntellisenseData
    {
        private const int _loadPeriodInMinutes = 5;

        private string FilePath { get; set; }

        public DateTime? NextLoadFileDate { get; set; }

        [DataMember]
        public Guid ConnectionId { get; set; }

        [DataMember]
        public ConcurrentDictionary<Guid, WebResource> WebResourcesHtml { get; private set; }

        [DataMember]
        public ConcurrentDictionary<Guid, WebResource> WebResourcesIcon { get; private set; }

        [DataMember]
        public ConcurrentDictionary<Guid, WebResource> WebResourcesJavaScript { get; private set; }

        public WebResourceIntellisenseData()
        {
            ClearData();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (WebResourcesHtml == null)
            {
                this.WebResourcesHtml = new ConcurrentDictionary<Guid, WebResource>();
            }

            if (WebResourcesIcon == null)
            {
                this.WebResourcesIcon = new ConcurrentDictionary<Guid, WebResource>();
            }

            if (WebResourcesJavaScript == null)
            {
                this.WebResourcesJavaScript = new ConcurrentDictionary<Guid, WebResource>();
            }
        }

        public void ClearData()
        {
            this.WebResourcesHtml = new ConcurrentDictionary<Guid, WebResource>();
            this.WebResourcesIcon = new ConcurrentDictionary<Guid, WebResource>();
            this.WebResourcesJavaScript = new ConcurrentDictionary<Guid, WebResource>();
        }

        public void LoadWebResources(IEnumerable<WebResource> webResources, ConcurrentDictionary<Guid, WebResource> container)
        {
            if (!webResources.Any())
            {
                return;
            }

            this.NextLoadFileDate = DateTime.Now.AddMinutes(_loadPeriodInMinutes);

            foreach (var item in webResources)
            {
                if (!container.ContainsKey(item.Id))
                {
                    container.TryAdd(item.Id, item);
                }
            }
        }
    }
}