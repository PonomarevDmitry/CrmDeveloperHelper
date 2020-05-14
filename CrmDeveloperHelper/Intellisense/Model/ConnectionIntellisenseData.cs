using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class ConnectionIntellisenseData
    {
        public Guid ConnectionId { get; set; }

        public ConcurrentDictionary<string, EntityIntellisenseData> Entities { get; private set; }

        public ConnectionIntellisenseData()
        {
            CreateCollectionsIfNeeded();
        }

        private void CreateCollectionsIfNeeded()
        {
            if (this.Entities == null)
            {
                this.Entities = new ConcurrentDictionary<string, EntityIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        public void LoadSomeData(IEnumerable<EntityMetadata> entityMetadataList)
        {
            CreateCollectionsIfNeeded();

            foreach (var entityMetadata in entityMetadataList)
            {
                if (!this.Entities.ContainsKey(entityMetadata.LogicalName))
                {
                    this.Entities.TryAdd(entityMetadata.LogicalName, new EntityIntellisenseData());
                }

                this.Entities[entityMetadata.LogicalName].LoadData(entityMetadata);
                var task = this.Entities[entityMetadata.LogicalName].SaveAsync(this.ConnectionId);
            }
        }

        public void LoadFullData(IEnumerable<EntityMetadata> entityMetadataList)
        {
            CreateCollectionsIfNeeded();

            var hashSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var entityMetadata in entityMetadataList)
            {
                hashSet.Add(entityMetadata.LogicalName);

                if (!this.Entities.ContainsKey(entityMetadata.LogicalName))
                {
                    this.Entities.TryAdd(entityMetadata.LogicalName, new EntityIntellisenseData());
                }

                this.Entities[entityMetadata.LogicalName].LoadData(entityMetadata);
                var task = this.Entities[entityMetadata.LogicalName].SaveAsync(this.ConnectionId);
            }

            foreach (var entityName in this.Entities.Keys.ToList())
            {
                if (!hashSet.Contains(entityName))
                {
                    this.Entities.TryRemove(entityName, out _);
                }
            }
        }

        public void LoadData(EntityMetadata entityMetadata)
        {
            CreateCollectionsIfNeeded();

            if (entityMetadata == null)
            {
                return;
            }

            if (!this.Entities.ContainsKey(entityMetadata.LogicalName))
            {
                this.Entities.TryAdd(entityMetadata.LogicalName, new EntityIntellisenseData());
            }

            this.Entities[entityMetadata.LogicalName].LoadData(entityMetadata);

            var task = this.Entities[entityMetadata.LogicalName].SaveAsync(this.ConnectionId);
        }

        public void GetDataFromDisk()
        {
            CreateCollectionsIfNeeded();

            var directory = FileOperations.GetConnectionIntellisenseDataFolderPathEntities(this.ConnectionId);

            var directoryInfo = new DirectoryInfo(directory);

            if (directoryInfo.Exists)
            {
                var files = directoryInfo.GetFiles("*.xml");

                Parallel.ForEach(files, file =>
                {
                    var entityData = EntityIntellisenseData.Get(file.FullName);

                    if (entityData != null)
                    {
                        if (!this.Entities.ContainsKey(entityData.EntityLogicalName))
                        {
                            this.Entities.TryAdd(entityData.EntityLogicalName, entityData);
                        }
                        else
                        {
                            this.Entities[entityData.EntityLogicalName].MergeDataFromDisk(entityData);
                        }
                    }
                });
            }
        }
    }
}