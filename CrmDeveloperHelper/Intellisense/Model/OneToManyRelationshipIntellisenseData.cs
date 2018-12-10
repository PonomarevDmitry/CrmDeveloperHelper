using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class OneToManyRelationshipIntellisenseData
    {
        [DataMember]
        public Guid? MetadataId { get; private set; }

        [DataMember]
        public string SchemaName { get; private set; }

        [DataMember]
        public string ChildEntityName { get; private set; }

        [DataMember]
        public string ChildEntityAttributeName { get; private set; }

        public OneToManyRelationshipIntellisenseData()
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

            if (!string.IsNullOrEmpty(item.ReferencingEntity))
            {
                this.ChildEntityName = item.ReferencingEntity;
            }
            if (!string.IsNullOrEmpty(item.ReferencingAttribute))
            {
                this.ChildEntityAttributeName = item.ReferencingAttribute;
            }
        }

        internal void MergeDataFromDisk(OneToManyRelationshipIntellisenseData item)
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

            if (string.IsNullOrEmpty(this.ChildEntityName)
                && !string.IsNullOrEmpty(item.ChildEntityName))
            {
                this.ChildEntityName = item.ChildEntityName;
            }

            if (string.IsNullOrEmpty(this.ChildEntityAttributeName)
                && !string.IsNullOrEmpty(item.ChildEntityAttributeName))
            {
                this.ChildEntityAttributeName = item.ChildEntityAttributeName;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", this.SchemaName, this.ChildEntityName, this.ChildEntityAttributeName);
        }
    }
}