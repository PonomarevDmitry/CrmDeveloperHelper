using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

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
        public bool? IsIntersectEntity { get; private set; }

        [DataMember]
        public ConcurrentDictionary<string, AttributeIntellisenseData> Attributes { get; private set; }

        [DataMember]
        public ConcurrentDictionary<string, ManyToOneRelationshipIntellisenseData> ManyToOneRelationships { get; private set; }

        [DataMember]
        public ConcurrentDictionary<string, OneToManyRelationshipIntellisenseData> OneToManyRelationships { get; private set; }

        [DataMember]
        public ConcurrentDictionary<string, ManyToManyRelationshipIntellisenseData> ManyToManyRelationships { get; private set; }

        [DataMember]
        public ConcurrentDictionary<string, EntityKeyIntellisenseData> Keys { get; private set; }

        public EntityIntellisenseData()
        {
            this.Attributes = new ConcurrentDictionary<string, AttributeIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            this.ManyToOneRelationships = new ConcurrentDictionary<string, ManyToOneRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            this.OneToManyRelationships = new ConcurrentDictionary<string, OneToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            this.ManyToManyRelationships = new ConcurrentDictionary<string, ManyToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            this.Keys = new ConcurrentDictionary<string, EntityKeyIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
        }

        public IEnumerable<AttributeIntellisenseData> AttributesOrdered()
        {
            if (this.Attributes == null)
            {
                yield break;
            }

            if (!string.IsNullOrEmpty(this.EntityPrimaryIdAttribute))
            {
                if (this.Attributes.ContainsKey(this.EntityPrimaryIdAttribute))
                {
                    yield return this.Attributes[this.EntityPrimaryIdAttribute];
                }
            }

            if (!string.IsNullOrEmpty(this.EntityPrimaryNameAttribute))
            {
                if (this.Attributes.ContainsKey(this.EntityPrimaryNameAttribute))
                {
                    yield return this.Attributes[this.EntityPrimaryNameAttribute];
                }
            }

            foreach (var attribute in this.Attributes.Values.OrderBy(e => e.LogicalName))
            {
                if (string.Equals(attribute.LogicalName, this.EntityPrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (string.Equals(attribute.LogicalName, this.EntityPrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                yield return attribute;
            }
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (Attributes == null)
            {
                this.Attributes = new ConcurrentDictionary<string, AttributeIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (ManyToOneRelationships == null)
            {
                this.ManyToOneRelationships = new ConcurrentDictionary<string, ManyToOneRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (OneToManyRelationships == null)
            {
                this.OneToManyRelationships = new ConcurrentDictionary<string, OneToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (ManyToManyRelationships == null)
            {
                this.ManyToManyRelationships = new ConcurrentDictionary<string, ManyToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (Keys == null)
            {
                this.Keys = new ConcurrentDictionary<string, EntityKeyIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
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
                var temp = new ConcurrentDictionary<string, AttributeIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var attr in entityMetadata.Attributes)
                {
                    if (!temp.ContainsKey(attr.LogicalName))
                    {
                        temp.TryAdd(attr.LogicalName, new AttributeIntellisenseData());
                    }

                    temp[attr.LogicalName].LoadData(attr
                        , string.Equals(attr.LogicalName, entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase)
                        , string.Equals(attr.LogicalName, entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase)
                    );
                }

                this.Attributes = temp;
            }

            if (entityMetadata.OneToManyRelationships != null)
            {
                var temp = new ConcurrentDictionary<string, OneToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var item in entityMetadata.OneToManyRelationships)
                {
                    if (!temp.ContainsKey(item.SchemaName))
                    {
                        temp.TryAdd(item.SchemaName, new OneToManyRelationshipIntellisenseData());
                    }

                    temp[item.SchemaName].LoadData(item);
                }

                this.OneToManyRelationships = temp;
            }

            if (entityMetadata.ManyToOneRelationships != null)
            {
                var temp = new ConcurrentDictionary<string, ManyToOneRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var item in entityMetadata.ManyToOneRelationships)
                {
                    if (!temp.ContainsKey(item.SchemaName))
                    {
                        temp.TryAdd(item.SchemaName, new ManyToOneRelationshipIntellisenseData());
                    }

                    temp[item.SchemaName].LoadData(item);
                }

                this.ManyToOneRelationships = temp;
            }

            if (entityMetadata.ManyToManyRelationships != null)
            {
                var temp = new ConcurrentDictionary<string, ManyToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var item in entityMetadata.ManyToManyRelationships)
                {
                    if (!temp.ContainsKey(item.SchemaName))
                    {
                        temp.TryAdd(item.SchemaName, new ManyToManyRelationshipIntellisenseData());
                    }

                    temp[item.SchemaName].LoadData(item);
                }

                this.ManyToManyRelationships = temp;
            }

            if (entityMetadata.Keys != null)
            {
                var temp = new ConcurrentDictionary<string, EntityKeyIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var item in entityMetadata.Keys)
                {
                    if (!temp.ContainsKey(item.LogicalName))
                    {
                        temp.TryAdd(item.LogicalName, new EntityKeyIntellisenseData());
                    }

                    temp[item.SchemaName].LoadData(item);
                }

                this.Keys = temp;
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

            if (entityData.IsIntersectEntity.HasValue)
            {
                this.IsIntersectEntity = entityData.IsIntersectEntity;
            }

            if (entityData.Attributes != null)
            {
                if (this.Attributes == null)
                {
                    this.Attributes = new ConcurrentDictionary<string, AttributeIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }

                foreach (var attr in entityData.Attributes.Values)
                {
                    if (!this.Attributes.ContainsKey(attr.LogicalName))
                    {
                        this.Attributes.TryAdd(attr.LogicalName, attr);
                    }
                    else
                    {
                        this.Attributes[attr.LogicalName].MergeDataFromDisk(attr);
                    }
                }
            }

            if (entityData.OneToManyRelationships != null)
            {
                if (this.OneToManyRelationships == null)
                {
                    this.OneToManyRelationships = new ConcurrentDictionary<string, OneToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }

                foreach (var item in entityData.OneToManyRelationships.Values)
                {
                    if (!this.OneToManyRelationships.ContainsKey(item.SchemaName))
                    {
                        this.OneToManyRelationships.TryAdd(item.SchemaName, item);
                    }
                    else
                    {
                        this.OneToManyRelationships[item.SchemaName].MergeDataFromDisk(item);
                    }
                }
            }

            if (entityData.ManyToOneRelationships != null)
            {
                if (this.ManyToOneRelationships == null)
                {
                    this.ManyToOneRelationships = new ConcurrentDictionary<string, ManyToOneRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }

                foreach (var item in entityData.ManyToOneRelationships.Values)
                {
                    if (!this.ManyToOneRelationships.ContainsKey(item.SchemaName))
                    {
                        this.ManyToOneRelationships.TryAdd(item.SchemaName, item);
                    }
                    else
                    {
                        this.ManyToOneRelationships[item.SchemaName].MergeDataFromDisk(item);
                    }
                }
            }

            if (entityData.ManyToManyRelationships != null)
            {
                if (this.ManyToManyRelationships == null)
                {
                    this.ManyToManyRelationships = new ConcurrentDictionary<string, ManyToManyRelationshipIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }

                foreach (var item in entityData.ManyToManyRelationships.Values)
                {
                    if (!this.ManyToManyRelationships.ContainsKey(item.SchemaName))
                    {
                        this.ManyToManyRelationships.TryAdd(item.SchemaName, item);
                    }
                    else
                    {
                        this.ManyToManyRelationships[item.SchemaName].MergeDataFromDisk(item);
                    }
                }
            }

            if (entityData.Keys != null)
            {
                if (this.Keys == null)
                {
                    this.Keys = new ConcurrentDictionary<string, EntityKeyIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }

                foreach (var item in entityData.Keys.Values)
                {
                    if (!this.Keys.ContainsKey(item.LogicalName))
                    {
                        this.Keys.TryAdd(item.LogicalName, item);
                    }
                    else
                    {
                        this.Keys[item.LogicalName].MergeDataFromDisk(item);
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
                    if (entityData.IsIntersectEntity.GetValueOrDefault())
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

        public static EntityIntellisenseData Get(string filePath)
        {
            EntityIntellisenseData result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(EntityIntellisenseData));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as EntityIntellisenseData;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);

                        FileOperations.CreateBackUpFile(filePath, ex);
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }

            return result;
        }

        public Task SaveAsync(Guid connectionId)
        {
            return Task.Run(() => Save(connectionId));
        }

        public void Save(Guid connectionId)
        {
            string directory = FileOperations.GetConnectionIntellisenseDataFolderPathEntities(connectionId);

            string filePath = Path.Combine(directory, this.EntityLogicalName.ToLower() + ".xml");

            this.SaveInternal(filePath);
        }

        private void SaveInternal(string filePath)
        {
            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(EntityIntellisenseData));

                    ser.WriteObject(memoryStream, this);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    fileBody = memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);

                    fileBody = null;
                }
            }

            if (fileBody != null)
            {
                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        try
                        {
                            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                stream.Write(fileBody, 0, fileBody.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToLog(ex);
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }
    }
}