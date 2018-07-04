using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class EntityIntellisenseData
    {
        [DataMember]
        public Guid? MetadataId { get; private set; }

        [DataMember]
        public string EntityLogicalName { get; private set; }

        [DataMember]
        public int? ObjectTypeCode { get; private set; }

        [DataMember]
        public string EntityPrimaryIdAttribute { get; private set; }

        [DataMember]
        public string EntityPrimaryNameAttribute { get; private set; }

        [DataMember]
        public Label DisplayName { get; private set; }

        [DataMember]
        public Label DisplayCollectionName { get; private set; }

        [DataMember]
        public Label Description { get; private set; }

        [DataMember]
        public bool IsIntersectEntity { get; private set; }

        private object _syncObjectAttributes = new object();

        private object _syncObjectManyToOneRelationships = new object();

        private object _syncObjectOneToManyRelationships = new object();

        private object _syncObjectManyToManyRelationships = new object();

        [DataMember]
        public Dictionary<string, AttributeIntellisenseData> Attributes { get; private set; }

        [DataMember]
        public Dictionary<string, ManyToOneRelationshipIntellisenseData> ManyToOneRelationships { get; private set; }

        [DataMember]
        public Dictionary<string, OneToManyRelationshipIntellisenseData> OneToManyRelationships { get; private set; }

        [DataMember]
        public Dictionary<string, ManyToManyRelationshipIntellisenseData> ManyToManyRelationships { get; private set; }

        public EntityIntellisenseData()
        {
            this.Attributes = new Dictionary<string, AttributeIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            this.ManyToOneRelationships = new Dictionary<string, ManyToOneRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            this.OneToManyRelationships = new Dictionary<string, OneToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            this.ManyToManyRelationships = new Dictionary<string, ManyToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (_syncObjectAttributes == null) { _syncObjectAttributes = new object(); }
            if (_syncObjectManyToOneRelationships == null) { _syncObjectManyToOneRelationships = new object(); }
            if (_syncObjectOneToManyRelationships == null) { _syncObjectOneToManyRelationships = new object(); }
            if (_syncObjectManyToManyRelationships == null) { _syncObjectManyToManyRelationships = new object(); }

            lock (_syncObjectAttributes)
            {
                if (Attributes == null)
                {
                    this.Attributes = new Dictionary<string, AttributeIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }
            }

            lock (_syncObjectManyToOneRelationships)
            {
                if (ManyToOneRelationships == null)
                {
                    this.ManyToOneRelationships = new Dictionary<string, ManyToOneRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }
            }

            lock (_syncObjectOneToManyRelationships)
            {
                if (OneToManyRelationships == null)
                {
                    this.OneToManyRelationships = new Dictionary<string, OneToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }
            }

            lock (_syncObjectManyToManyRelationships)
            {
                if (ManyToManyRelationships == null)
                {
                    this.ManyToManyRelationships = new Dictionary<string, ManyToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }
            }
        }

        public void LoadData(EntityMetadata entityMetadata)
        {
            if (entityMetadata.MetadataId.HasValue)
            {
                this.MetadataId = entityMetadata.MetadataId;
            }

            if (!string.IsNullOrEmpty(entityMetadata.LogicalName))
            {
                this.EntityLogicalName = entityMetadata.LogicalName;
            }

            if (entityMetadata.ObjectTypeCode.HasValue)
            {
                this.ObjectTypeCode = entityMetadata.ObjectTypeCode;
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryIdAttribute))
            {
                this.EntityPrimaryIdAttribute = entityMetadata.PrimaryIdAttribute;
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute))
            {
                this.EntityPrimaryNameAttribute = entityMetadata.PrimaryNameAttribute;
            }

            if (entityMetadata.DisplayName != null)
            {
                this.DisplayName = entityMetadata.DisplayName;
            }

            if (entityMetadata.Description != null)
            {
                this.Description = entityMetadata.Description;
            }

            if (entityMetadata.DisplayCollectionName != null)
            {
                this.DisplayCollectionName = entityMetadata.DisplayCollectionName;
            }

            if (entityMetadata.IsIntersect.HasValue)
            {
                this.IsIntersectEntity = entityMetadata.IsIntersect.GetValueOrDefault();
            }

            if (entityMetadata.Attributes != null)
            {
                lock (_syncObjectAttributes)
                {
                    if (this.Attributes == null)
                    {
                        this.Attributes = new Dictionary<string, AttributeIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                    }
                }

                foreach (var attr in entityMetadata.Attributes)
                {
                    if (!string.IsNullOrEmpty(attr.AttributeOf))
                    {
                        continue;
                    }

                    lock (_syncObjectAttributes)
                    {
                        if (!this.Attributes.ContainsKey(attr.LogicalName))
                        {
                            this.Attributes.Add(attr.LogicalName, new AttributeIntellisenseData());
                        }
                    }

                    this.Attributes[attr.LogicalName].LoadData(attr);
                }
            }

            if (entityMetadata.OneToManyRelationships != null)
            {
                lock (_syncObjectOneToManyRelationships)
                {
                    if (this.OneToManyRelationships == null)
                    {
                        this.OneToManyRelationships = new Dictionary<string, OneToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                    }
                }

                foreach (var item in entityMetadata.OneToManyRelationships)
                {
                    lock (_syncObjectOneToManyRelationships)
                    {
                        if (!this.OneToManyRelationships.ContainsKey(item.SchemaName))
                        {
                            this.OneToManyRelationships.Add(item.SchemaName, new OneToManyRelationshipIntellisenseData());
                        }
                    }

                    this.OneToManyRelationships[item.SchemaName].LoadData(item);
                }
            }

            if (entityMetadata.ManyToOneRelationships != null)
            {
                lock (_syncObjectManyToOneRelationships)
                {
                    if (this.ManyToOneRelationships == null)
                    {
                        this.ManyToOneRelationships = new Dictionary<string, ManyToOneRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                    }
                }

                foreach (var item in entityMetadata.ManyToOneRelationships)
                {
                    lock (_syncObjectManyToOneRelationships)
                    {
                        if (!this.ManyToOneRelationships.ContainsKey(item.SchemaName))
                        {
                            this.ManyToOneRelationships.Add(item.SchemaName, new ManyToOneRelationshipIntellisenseData());
                        }
                    }

                    this.ManyToOneRelationships[item.SchemaName].LoadData(item);
                }
            }

            if (entityMetadata.ManyToManyRelationships != null)
            {
                lock (_syncObjectManyToManyRelationships)
                {
                    if (this.ManyToManyRelationships == null)
                    {
                        this.ManyToManyRelationships = new Dictionary<string, ManyToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                    }
                }

                foreach (var item in entityMetadata.ManyToManyRelationships)
                {
                    lock (_syncObjectManyToManyRelationships)
                    {
                        if (!this.ManyToManyRelationships.ContainsKey(item.SchemaName))
                        {
                            this.ManyToManyRelationships.Add(item.SchemaName, new ManyToManyRelationshipIntellisenseData());
                        }
                    }

                    this.ManyToManyRelationships[item.SchemaName].LoadData(item);
                }
            }
        }

        public void MergeDataFromDisk(EntityIntellisenseData entityData)
        {
            if (entityData == null)
            {
                return;
            }

            if (!this.MetadataId.HasValue
                && entityData.MetadataId.HasValue)
            {
                this.MetadataId = entityData.MetadataId;
            }

            if (string.IsNullOrEmpty(this.EntityLogicalName)
                && !string.IsNullOrEmpty(entityData.EntityLogicalName))
            {
                this.EntityLogicalName = entityData.EntityLogicalName;
            }

            if (!this.ObjectTypeCode.HasValue
                && entityData.ObjectTypeCode.HasValue)
            {
                this.ObjectTypeCode = entityData.ObjectTypeCode;
            }

            if (string.IsNullOrEmpty(this.EntityPrimaryIdAttribute)
                && !string.IsNullOrEmpty(entityData.EntityPrimaryIdAttribute))
            {
                this.EntityPrimaryIdAttribute = entityData.EntityPrimaryIdAttribute;
            }

            if (string.IsNullOrEmpty(this.EntityPrimaryNameAttribute)
                && !string.IsNullOrEmpty(entityData.EntityPrimaryNameAttribute))
            {
                this.EntityPrimaryNameAttribute = entityData.EntityPrimaryNameAttribute;
            }

            if (this.DisplayName == null
                && entityData.DisplayName != null)
            {
                this.DisplayName = entityData.DisplayName;
            }

            if (this.Description == null
                && entityData.Description != null)
            {
                this.Description = entityData.Description;
            }

            if (this.DisplayCollectionName == null
                && entityData.DisplayCollectionName != null)
            {
                this.DisplayCollectionName = entityData.DisplayCollectionName;
            }

            if (entityData.IsIntersectEntity)
            {
                this.IsIntersectEntity = entityData.IsIntersectEntity;
            }

            if (entityData.Attributes != null)
            {
                lock (_syncObjectAttributes)
                {
                    if (this.Attributes == null)
                    {
                        this.Attributes = new Dictionary<string, AttributeIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                    }
                }

                foreach (var attr in entityData.Attributes.Values)
                {
                    lock (_syncObjectAttributes)
                    {
                        if (!this.Attributes.ContainsKey(attr.LogicalName))
                        {
                            this.Attributes.Add(attr.LogicalName, attr);
                        }
                        else
                        {
                            this.Attributes[attr.LogicalName].MergeDataFromDisk(attr);
                        }
                    }
                }
            }

            if (entityData.OneToManyRelationships != null)
            {
                lock (_syncObjectOneToManyRelationships)
                {
                    if (this.OneToManyRelationships == null)
                    {
                        this.OneToManyRelationships = new Dictionary<string, OneToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                    }
                }

                foreach (var item in entityData.OneToManyRelationships.Values)
                {
                    lock (_syncObjectOneToManyRelationships)
                    {
                        if (!this.OneToManyRelationships.ContainsKey(item.SchemaName))
                        {
                            this.OneToManyRelationships.Add(item.SchemaName, item);
                        }
                        else
                        {
                            this.OneToManyRelationships[item.SchemaName].MergeDataFromDisk(item);
                        }
                    }
                }
            }

            if (entityData.ManyToOneRelationships != null)
            {
                lock (_syncObjectManyToOneRelationships)
                {
                    if (this.ManyToOneRelationships == null)
                    {
                        this.ManyToOneRelationships = new Dictionary<string, ManyToOneRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                    }
                }

                foreach (var item in entityData.ManyToOneRelationships.Values)
                {
                    lock (_syncObjectManyToOneRelationships)
                    {
                        if (!this.ManyToOneRelationships.ContainsKey(item.SchemaName))
                        {
                            this.ManyToOneRelationships.Add(item.SchemaName, item);
                        }
                        else
                        {
                            this.ManyToOneRelationships[item.SchemaName].MergeDataFromDisk(item);
                        }
                    }
                }
            }

            if (entityData.ManyToManyRelationships != null)
            {
                lock (_syncObjectManyToManyRelationships)
                {
                    if (this.ManyToManyRelationships == null)
                    {
                        this.ManyToManyRelationships = new Dictionary<string, ManyToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                    }
                }

                foreach (var item in entityData.ManyToManyRelationships.Values)
                {
                    if (!this.ManyToManyRelationships.ContainsKey(item.SchemaName))
                    {
                        this.ManyToManyRelationships.Add(item.SchemaName, item);
                    }
                    else
                    {
                        this.ManyToManyRelationships[item.SchemaName].MergeDataFromDisk(item);
                    }
                }
            }
        }

        public HashSet<string> GetRelatedEntities()
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            AddReferencingEntities(result, this, true);

            if (this.ManyToOneRelationships != null)
            {
                foreach (var item in this.ManyToOneRelationships.Values.ToList())
                {
                    if (!string.IsNullOrEmpty(item.TargetEntityName))
                    {
                        result.Add(item.TargetEntityName);
                    }
                }
            }

            return result;
        }

        public HashSet<string> GetLinkedEntities(ConnectionIntellisenseData connectionIntellisense)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            AddReferencingEntities(result, this, true);

            if (this.ManyToOneRelationships != null && connectionIntellisense.Entities != null)
            {
                foreach (var item in this.ManyToOneRelationships.Values.ToList())
                {
                    if (!string.IsNullOrEmpty(item.TargetEntityName))
                    {
                        result.Add(item.TargetEntityName);

                        if (connectionIntellisense.Entities.ContainsKey(item.TargetEntityName))
                        {
                            var targetEntity = connectionIntellisense.Entities[item.TargetEntityName];

                            if (!IsCommonAttribute(item.BaseAttributeName))
                            {
                                AddReferencingEntities(result, targetEntity, false);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool IsFullData()
        {
            return this.MetadataId.HasValue && this.Attributes != null && this.Attributes.Any();
        }

        private static HashSet<string> _commonAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            Entities.Report.Schema.Attributes.ownerid
            , Entities.Report.Schema.Attributes.owningbusinessunit
            , Entities.Report.Schema.Attributes.owningteam
            , Entities.Report.Schema.Attributes.owninguser

            , Entities.Report.Schema.Attributes.createdby
            , Entities.Report.Schema.Attributes.createdonbehalfby

            , Entities.Report.Schema.Attributes.modifiedby
            , Entities.Report.Schema.Attributes.modifiedonbehalfby

            , Entities.SystemUser.Schema.Attributes.transactioncurrencyid
        };

        private static bool IsCommonAttribute(string entityName)
        {
            return _commonAttributes.Contains(entityName);
        }

        //private static HashSet<string> _commonEntities = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { Entities.SystemUser.EntityLogicalName, Entities.Team.EntityLogicalName };

        //private static bool IsCommonEntity(string entityName)
        //{
        //    return _commonEntities.Contains(entityName);
        //}

        public override string ToString()
        {
            return this.EntityLogicalName;
        }

        private static void AddReferencingEntities(HashSet<string> result, EntityIntellisenseData entityData, bool all)
        {
            if (entityData.ManyToManyRelationships != null)
            {
                foreach (var item in entityData.ManyToManyRelationships.Values.ToList())
                {
                    if (entityData.IsIntersectEntity)
                    {
                        if (!string.IsNullOrEmpty(item.Entity1Name))
                        {
                            result.Add(item.Entity1Name);
                        }

                        if (!string.IsNullOrEmpty(item.Entity2Name))
                        {
                            result.Add(item.Entity2Name);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.IntersectEntityName))
                        {
                            result.Add(item.IntersectEntityName);
                        }
                    }
                }
            }

            if (entityData.OneToManyRelationships != null)
            {
                foreach (var item in entityData.OneToManyRelationships.Values.ToList())
                {
                    if (!string.IsNullOrEmpty(item.ChildEntityName))
                    {
                        if (all || !IsCommonAttribute(item.ChildEntityAttributeName))
                        {
                            result.Add(item.ChildEntityName);
                        }
                    }
                }
            }
        }
    }
}