using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class EntityKeyIntellisenseData
    {
        [DataMember]
        public Guid? MetadataId { get; private set; }

        [DataMember]
        public string LogicalName { get; private set; }

        [DataMember]
        public string EntityLogicalName { get; private set; }

        [DataMember]
        public string[] KeyAttributes { get; private set; }

        public EntityKeyIntellisenseData()
        {

        }

        public void LoadData(EntityKeyMetadata item)
        {
            if (item.MetadataId.HasValue)
            {
                this.MetadataId = item.MetadataId;
            }

            if (!string.IsNullOrEmpty(item.LogicalName))
            {
                this.LogicalName = item.LogicalName;
            }

            if (!string.IsNullOrEmpty(item.EntityLogicalName))
            {
                this.EntityLogicalName = item.EntityLogicalName;
            }

            if (item.KeyAttributes != null)
            {
                this.KeyAttributes = item.KeyAttributes;
            }
        }

        internal void MergeDataFromDisk(EntityKeyIntellisenseData item)
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

            if (string.IsNullOrEmpty(this.LogicalName)
                && !string.IsNullOrEmpty(item.LogicalName))
            {
                this.LogicalName = item.LogicalName;
            }

            if (string.IsNullOrEmpty(this.EntityLogicalName)
                && !string.IsNullOrEmpty(item.EntityLogicalName))
            {
                this.EntityLogicalName = item.EntityLogicalName;
            }

            if (this.KeyAttributes == null
                && item.KeyAttributes != null)
            {
                this.KeyAttributes = item.KeyAttributes;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", this.EntityLogicalName, this.LogicalName, string.Join(",", this.KeyAttributes));
        }
    }
}
