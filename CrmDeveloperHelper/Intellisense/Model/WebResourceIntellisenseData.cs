using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class WebResourceIntellisenseData
    {
        [DataMember]
        public Guid? WebResourceId { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string DisplayName { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public OptionSetValue WebResourceType { get; private set; }

        [DataMember]
        public int? LanguageCode { get; private set; }

        public void LoadData(WebResource webResource)
        {
            if (webResource.WebResourceId.HasValue)
            {
                this.WebResourceId = webResource.WebResourceId;
            }

            this.Name = webResource.Name;
            this.DisplayName = webResource.DisplayName;
            this.Description = webResource.Description;
            this.WebResourceType = webResource.WebResourceType;
            this.LanguageCode = webResource.LanguageCode;
        }

        public void MergeDataFromDisk(WebResourceIntellisenseData webResource)
        {
            if (webResource == null)
            {
                return;
            }

            if (!this.WebResourceId.HasValue
                && webResource.WebResourceId.HasValue
            )
            {
                this.WebResourceId = webResource.WebResourceId;
            }

            if (string.IsNullOrEmpty(this.Name)
                && !string.IsNullOrEmpty(webResource.Name))
            {
                this.Name = webResource.Name;
            }

            if (string.IsNullOrEmpty(this.DisplayName)
                && !string.IsNullOrEmpty(webResource.DisplayName))
            {
                this.DisplayName = webResource.DisplayName;
            }

            if (string.IsNullOrEmpty(this.Description)
                && !string.IsNullOrEmpty(webResource.Description))
            {
                this.Description = webResource.Description;
            }

            if (this.WebResourceType == null
                && webResource.WebResourceType != null)
            {
                this.WebResourceType = webResource.WebResourceType;
            }

            if (!this.LanguageCode.HasValue
                && webResource.LanguageCode.HasValue
            )
            {
                this.LanguageCode = webResource.LanguageCode;
            }
        }
    }
}