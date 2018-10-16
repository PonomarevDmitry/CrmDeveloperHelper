using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction
{
    [DataContract]
    public class PluginImage
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int? ImageType { get; set; }

        [DataMember]
        public string ImageTypeName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string EntityAlias { get; set; }

        [DataMember]
        public string ComponentState { get; set; }

        [DataMember]
        public int? ComponentStateCode { get; set; }

        [DataMember]
        public string MessagePropertyName { get; set; }

        [DataMember]
        public string RelatedAttributeName { get; set; }

        [DataMember]
        public int? CustomizationLevel { get; set; }

        [DataMember]
        public DateTime? CreatedOn { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime? ModifiedOn { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public List<string> Attributes { get; set; }

        public PluginImage()
        {
            this.Attributes = new List<string>();
        }

        internal static PluginImage GetObject(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageProcessingStepImage entImage)
        {
            var result = new PluginImage();

            result.Id = entImage.Id;

            result.Name = entImage.Name;
            result.EntityAlias = entImage.EntityAlias;


            result.ImageType = entImage.ImageType.Value;
            result.ImageTypeName = entImage.FormattedValues[Entities.SdkMessageProcessingStepImage.Schema.Attributes.imagetype];

            result.CreatedBy = entImage.CreatedBy.Name;
            result.CreatedOn = entImage.CreatedOn;

            result.ModifiedBy = entImage.ModifiedBy.Name;
            result.ModifiedOn = entImage.ModifiedOn;

            result.MessagePropertyName = entImage.MessagePropertyName;

            result.CustomizationLevel = entImage.CustomizationLevel;

            result.RelatedAttributeName = entImage.RelatedAttributeName;

            result.ComponentState = entImage.FormattedValues[Entities.SdkMessageProcessingStepImage.Schema.Attributes.componentstate];
            result.ComponentStateCode = entImage.ComponentState.Value;

            result.Attributes.AddRange(entImage.Attributes1Strings);

            return result;
        }
    }
}