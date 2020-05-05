using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Concurrent;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class WebResourceIntellisenseData
    {
        private string FilePath { get; set; }

        public DateTime? NextLoadFileDate { get; set; }

        [DataMember]
        public Guid ConnectionId { get; set; }

        [DataMember]
        public ConcurrentDictionary<Guid, WebResource> WebResourcesAll { get; private set; }

        [DataMember]
        public ConcurrentDictionary<Guid, WebResource> WebResourcesHtml { get; private set; }

        [DataMember]
        public ConcurrentDictionary<Guid, WebResource> WebResourcesIcon { get; private set; }

        [DataMember]
        public ConcurrentDictionary<Guid, WebResource> WebResourcesJavaScript { get; private set; }

        public WebResourceIntellisenseData()
        {
            this.WebResourcesAll = new ConcurrentDictionary<Guid, WebResource>();
            this.WebResourcesHtml = new ConcurrentDictionary<Guid, WebResource>();
            this.WebResourcesIcon = new ConcurrentDictionary<Guid, WebResource>();
            this.WebResourcesJavaScript = new ConcurrentDictionary<Guid, WebResource>();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (WebResourcesAll == null)
            {
                this.WebResourcesAll = new ConcurrentDictionary<Guid, WebResource>();
            }

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
    }
}