using System;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk.Metadata;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class ManyToOneRelationshipIntellisenseData
    {
        [DataMember]
        public Guid? MetadataId { get; private set; }

        [DataMember]
        public string SchemaName { get; private set; }

        [DataMember]
        public string BaseAttributeName { get; private set; }

        [DataMember]
        public string TargetEntityName { get; private set; }

        [DataMember]
        public string TargetAttributeName { get; private set; }

        public ManyToOneRelationshipIntellisenseData()
        {

        }

        public void LoadData(OneToManyRelationshipMetadata item)
        {
            if (item.MetadataId.HasValue)
            {
                this.MetadataId = item.MetadataId;
            }

            if (!string.IsNullOrEmpty(item.SchemaName))
            {
                this.SchemaName = item.SchemaName;
            }

            if (!string.IsNullOrEmpty(item.ReferencingAttribute))
            {
                this.BaseAttributeName = item.ReferencingAttribute;
            }

            if (!string.IsNullOrEmpty(item.ReferencedEntity))
            {
                this.TargetEntityName = item.ReferencedEntity;
            }
            if (!string.IsNullOrEmpty(item.ReferencedAttribute))
            {
                this.TargetAttributeName = item.ReferencedAttribute;
            }
        }

        internal void MergeDataFromDisk(ManyToOneRelationshipIntellisenseData item)
        {
            if (item == null)
            {
                return;
            }

            if (!this.MetadataId.HasValue
               && item.MetadataId.HasValue)
            {
                this.MetadataId = item.MetadataId;
            }

            if (string.IsNullOrEmpty(this.SchemaName)
                && !string.IsNullOrEmpty(item.SchemaName))
            {
                this.SchemaName = item.SchemaName;
            }

            if (string.IsNullOrEmpty(this.BaseAttributeName)
                && !string.IsNullOrEmpty(item.BaseAttributeName))
            {
                this.BaseAttributeName = item.BaseAttributeName;
            }

            if (string.IsNullOrEmpty(this.TargetEntityName)
                && !string.IsNullOrEmpty(item.TargetEntityName))
            {
                this.TargetEntityName = item.TargetEntityName;
            }

            if (string.IsNullOrEmpty(this.TargetAttributeName)
                && !string.IsNullOrEmpty(item.TargetAttributeName))
            {
                this.TargetAttributeName = item.TargetAttributeName;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} - {3}", this.SchemaName, this.BaseAttributeName, this.TargetEntityName, this.TargetAttributeName);
        }
    }
}