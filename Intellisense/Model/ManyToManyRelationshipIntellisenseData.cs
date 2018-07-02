using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class ManyToManyRelationshipIntellisenseData
    {
        [DataMember]
        public Guid? MetadataId { get; private set; }

        [DataMember]
        public string SchemaName { get; private set; }

        [DataMember]
        public string IntersectEntityName { get; private set; }

        [DataMember]
        public string Entity1IntersectAttributeName { get; private set; }

        [DataMember]
        public string Entity2IntersectAttributeName { get; private set; }

        [DataMember]
        public string Entity1Name { get; private set; }

        [DataMember]
        public string Entity2Name { get; private set; }

        public ManyToManyRelationshipIntellisenseData()
        {

        }

        public void LoadData(ManyToManyRelationshipMetadata item)
        {
            if (item.MetadataId.HasValue)
            {
                this.MetadataId = item.MetadataId;
            }

            if (!string.IsNullOrEmpty(item.SchemaName))
            {
                this.SchemaName = item.SchemaName;
            }

            if (!string.IsNullOrEmpty(item.IntersectEntityName))
            {
                this.IntersectEntityName = item.IntersectEntityName;
            }

            if (!string.IsNullOrEmpty(item.Entity1LogicalName))
            {
                this.Entity1Name = item.Entity1LogicalName;
            }

            if (!string.IsNullOrEmpty(item.Entity2LogicalName))
            {
                this.Entity2Name = item.Entity2LogicalName;
            }

            if (!string.IsNullOrEmpty(item.Entity1IntersectAttribute))
            {
                this.Entity1IntersectAttributeName = item.Entity1IntersectAttribute;
            }

            if (!string.IsNullOrEmpty(item.Entity2IntersectAttribute))
            {
                this.Entity2IntersectAttributeName = item.Entity2IntersectAttribute;
            }
        }

        internal void MergeDataFromDisk(ManyToManyRelationshipIntellisenseData item)
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

            if (string.IsNullOrEmpty(this.IntersectEntityName)
               && !string.IsNullOrEmpty(item.IntersectEntityName))
            {
                this.IntersectEntityName = item.IntersectEntityName;
            }

            if (string.IsNullOrEmpty(this.Entity1Name)
               && !string.IsNullOrEmpty(item.Entity1Name))
            {
                this.Entity1Name = item.Entity1Name;
            }

            if (string.IsNullOrEmpty(this.Entity2Name)
               && !string.IsNullOrEmpty(item.Entity2Name))
            {
                this.Entity2Name = item.Entity2Name;
            }

            if (string.IsNullOrEmpty(this.Entity1IntersectAttributeName)
               && !string.IsNullOrEmpty(item.Entity1IntersectAttributeName))
            {
                this.Entity1IntersectAttributeName = item.Entity1IntersectAttributeName;
            }

            if (string.IsNullOrEmpty(this.Entity2IntersectAttributeName)
               && !string.IsNullOrEmpty(item.Entity2IntersectAttributeName))
            {
                this.Entity2IntersectAttributeName = item.Entity2IntersectAttributeName;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} - {3}", this.SchemaName, this.IntersectEntityName, this.Entity1Name, this.Entity2Name);
        }
    }
}