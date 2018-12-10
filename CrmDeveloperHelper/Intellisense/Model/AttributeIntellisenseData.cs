using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class AttributeIntellisenseData
    {
        [DataMember]
        public Guid? MetadataId { get; private set; }

        [DataMember]
        public string LogicalName { get; private set; }

        [DataMember]
        public AttributeTypeCode? AttributeType { get; private set; }

        [DataMember]
        public Label DisplayName { get; private set; }

        [DataMember]
        public Label Description { get; private set; }

        [DataMember]
        public bool? IsPrimaryIdAttribute { get; private set; }

        [DataMember]
        public bool? IsPrimaryNameAttribute { get; private set; }

        [DataMember]
        public bool IsEntityNameAttribute { get; private set; }

        [DataMember]
        public List<string> Targets { get; private set; }

        [DataMember]
        public OptionSetIntellisenseData OptionSet { get; private set; }

        public AttributeIntellisenseData()
        {

        }

        public void LoadData(AttributeMetadata attr, bool isPrimaryIdAttribute, bool isPrimaryNameAttribute)
        {
            this.IsPrimaryIdAttribute = isPrimaryIdAttribute;
            this.IsPrimaryNameAttribute = isPrimaryNameAttribute;

            if (attr.MetadataId.HasValue)
            {
                this.MetadataId = attr.MetadataId;
            }

            if (!string.IsNullOrEmpty(attr.LogicalName))
            {
                this.LogicalName = attr.LogicalName;
            }

            if (attr.DisplayName != null)
            {
                this.DisplayName = attr.DisplayName;
            }

            if (attr.Description != null)
            {
                this.Description = attr.Description;
            }

            if (attr.AttributeType.HasValue)
            {
                this.AttributeType = attr.AttributeType;
            }

            this.IsEntityNameAttribute = attr is EntityNameAttributeMetadata;

            if (attr is BooleanAttributeMetadata boolMetadata)
            {
                this.OptionSet = new OptionSetIntellisenseData(boolMetadata);
            }

            if (attr is StateAttributeMetadata stateMetadata)
            {
                this.OptionSet = new OptionSetIntellisenseData(stateMetadata);
            }

            if (attr is StatusAttributeMetadata statusMetadata)
            {
                this.OptionSet = new OptionSetIntellisenseData(statusMetadata);
            }

            if (attr is PicklistAttributeMetadata picklistMetadata)
            {
                this.OptionSet = new OptionSetIntellisenseData(picklistMetadata);
            }

            if (attr is LookupAttributeMetadata lookupAttributeMetadata)
            {
                if (lookupAttributeMetadata.Targets != null && lookupAttributeMetadata.Targets.Length > 0)
                {
                    this.Targets = new List<string>(lookupAttributeMetadata.Targets);
                }
            }
        }

        public void MergeDataFromDisk(AttributeIntellisenseData attr)
        {
            if (attr == null)
            {
                return;
            }

            if (!this.MetadataId.HasValue
                && attr.MetadataId.HasValue)
            {
                this.MetadataId = attr.MetadataId;
            }

            if (string.IsNullOrEmpty(this.LogicalName)
               && !string.IsNullOrEmpty(attr.LogicalName))
            {
                this.LogicalName = attr.LogicalName;
            }

            if (this.DisplayName == null
                && attr.DisplayName != null)
            {
                this.DisplayName = attr.DisplayName;
            }

            if (this.Description == null
                && attr.Description != null)
            {
                this.Description = attr.Description;
            }

            if (!this.AttributeType.HasValue
                && attr.AttributeType.HasValue)
            {
                this.AttributeType = attr.AttributeType;
            }

            if (!this.IsPrimaryIdAttribute.HasValue
                && attr.IsPrimaryIdAttribute.HasValue)
            {
                this.IsPrimaryIdAttribute = attr.IsPrimaryIdAttribute;
            }

            if (!this.IsPrimaryNameAttribute.HasValue
                && attr.IsPrimaryNameAttribute.HasValue)
            {
                this.IsPrimaryNameAttribute = attr.IsPrimaryNameAttribute;
            }

            if (attr.IsEntityNameAttribute)
            {
                this.IsEntityNameAttribute = attr.IsEntityNameAttribute;
            }

            if (this.OptionSet == null
                && attr.OptionSet != null)
            {
                this.OptionSet = attr.OptionSet;
            }

            if ((this.Targets == null || this.Targets.Count == 0)
                && attr.Targets != null && attr.Targets.Count > 0)
            {
                this.Targets = attr.Targets;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.LogicalName, this.AttributeType.ToString());
        }
    }
}