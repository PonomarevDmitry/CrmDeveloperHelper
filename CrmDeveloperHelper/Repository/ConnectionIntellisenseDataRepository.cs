using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class ConnectionIntellisenseDataRepository : IDisposable
    {
        private readonly object syncObjectTaskListEntityHeader = new object();

        private readonly ConnectionData _connectionData;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly ConcurrentDictionary<string, Task> _cacheTaskGettingEntity = new ConcurrentDictionary<string, Task>();

        private readonly ConcurrentDictionary<int, Task> _cacheTaskGettingEntityObjectTypeCode = new ConcurrentDictionary<int, Task>();

        private Task _taskListEntityHeader;
        private bool _taskListEntityHeaderCompleted = false;

        private ConnectionIntellisenseDataRepository(ConnectionData connectionData)
        {
            this._connectionData = connectionData ?? throw new ArgumentNullException(nameof(connectionData));

            this._cancellationTokenSource = new CancellationTokenSource();

            var task = Task.Run(() => StartGettingListEntityHeader(), _cancellationTokenSource.Token);
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
        }

        private async Task<IOrganizationServiceExtented> GetServiceAsync()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            IOrganizationServiceExtented service = null;

            try
            {
                service = await QuickConnection.ConnectAsync(this._connectionData);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);

                service = null;
            }

            return service;
        }

        public ConnectionIntellisenseData GetEntitiesIntellisenseData()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            StartGettingListEntityHeader();

            return _connectionData.EntitiesIntellisenseData;
        }

        public EntityIntellisenseData GetEntityAttributeIntellisense(string entityName)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            if (_connectionData.EntitiesIntellisenseData != null
                && _connectionData.EntitiesIntellisenseData.Entities != null
                && _connectionData.EntitiesIntellisenseData.Entities.ContainsKey(entityName)
            )
            {
                var entityData = _connectionData.EntitiesIntellisenseData.Entities[entityName];

                if (entityData.Attributes != null)
                {
                    return entityData;
                }
            }

            var task = Task.Run(() => GetEntityDataForNames(new[] { entityName }), _cancellationTokenSource.Token);

            return null;
        }

        public EntityIntellisenseData GetEntityAttributeIntellisense(int entityTypeCode)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            if (_connectionData.EntitiesIntellisenseData != null
                && _connectionData.EntitiesIntellisenseData.Entities != null
                && _connectionData.EntitiesIntellisenseData.Entities.Values != null
            )
            {
                var list = _connectionData.EntitiesIntellisenseData.Entities.Values.ToList();

                var entityData = list.SingleOrDefault(e => e.ObjectTypeCode.HasValue && e.ObjectTypeCode == entityTypeCode);

                if (entityData != null && entityData.Attributes != null)
                {
                    return entityData;
                }
            }

            var task = Task.Run(() => GetEntityDataForObjectTypeCodes(new[] { entityTypeCode }), _cancellationTokenSource.Token);

            return null;
        }

        public Task GetEntityDataForNamesAsync(IEnumerable<string> entityCollection)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            return Task.Run(() => GetEntityDataForNames(entityCollection), _cancellationTokenSource.Token);
        }

        public Task GetEntityDataForObjectTypeCodesAsync(IEnumerable<int> entityObjectTypeCodes)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            return Task.Run(() => GetEntityDataForObjectTypeCodes(entityObjectTypeCodes), _cancellationTokenSource.Token);
        }

        private async Task GetListEntityHeaderDataAsync(HashSet<string> hashEntities)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            var service = await GetServiceAsync();

            if (service == null)
            {
                return;
            }

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression(_entityMetadataShortFieldsToQuery),
            };

            if (hashEntities.Any())
            {
                entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.NotIn, hashEntities.ToArray()));
            }

            var request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            try
            {
                var response = await service.ExecuteAsync<RetrieveMetadataChangesResponse>(request);

                lock (syncObjectTaskListEntityHeader)
                {
                    _taskListEntityHeaderCompleted = true;
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            var task1 = Task.Run(GetEntitiesFullInfoAsync, _cancellationTokenSource.Token);
            //Task.Run(() => GetEntitiesFullInfoOneByOneAsync(entities), _cancellationTokenSource.Token);
        }

        private const int _entitiesInPackToFullInformation = 20;
        private const int _packDelay = 30;

        private async Task GetEntitiesFullInfoAsync()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            if (_connectionData.EntitiesIntellisenseData.Entities == null)
            {
                return;
            }

            var entities = _connectionData.EntitiesIntellisenseData.Entities.Values.Where(e => !e.IsFullData()).Select(e => e.EntityLogicalName).ToArray();

            while (entities.Length > 0)
            {
                var pack = entities.Take(_entitiesInPackToFullInformation).ToArray();

                entities = entities.Skip(_entitiesInPackToFullInformation).ToArray();

                await LoadPackEntitiesAsync(pack);

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                await Task.Delay(_packDelay * 1000, _cancellationTokenSource.Token);
            }
        }

        public Task LoadPackEntitiesAsync(string[] pack)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return null;
            }

            return Task.Run(() => LoadPackEntities(pack), _cancellationTokenSource.Token);
        }

        private async Task LoadPackEntities(string[] pack)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            EntityQueryExpression entityQueryExpression = GetEntityQueryExpression();

            entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.In, pack));

            var request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            try
            {
                var service = await GetServiceAsync();

                if (service == null)
                {
                    return;
                }

                var response = await service.ExecuteAsync<RetrieveMetadataChangesResponse>(request);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }
        }

        //private async Task GetEntitiesFullInfoOneByOneAsync(HashSet<string> entities)
        //{
        //    foreach (var entityName in entities)
        //    {
        //        Task task = null;

        //        lock (syncObjectTaskGettingEntity)
        //        {
        //            if (!_cacheTaskGettingEntity.ContainsKey(entityName))
        //            {
        //                _cacheTaskGettingEntity.TryAdd(entityName, GetEntityFullInfoAsync(entityName));
        //            }

        //            task = _cacheTaskGettingEntity[entityName];
        //        }

        //        await task;
        //    }
        //}

        private async Task GetEntityDataForNames(IEnumerable<string> entityCollection)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            var hash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            if (_connectionData.EntitiesIntellisenseData.Entities != null)
            {
                foreach (var entityName in entityCollection)
                {
                    if (_connectionData.EntitiesIntellisenseData.Entities.ContainsKey(entityName))
                    {
                        hash.Add(entityName);
                    }
                }
            }

            foreach (var entityName in hash)
            {
                Task task = null;

                if (!_cacheTaskGettingEntity.ContainsKey(entityName))
                {
                    task = Task.Run(() => GetEntityFullDataForNameAsync(entityName), _cancellationTokenSource.Token);
                    _cacheTaskGettingEntity.TryAdd(entityName, task);
                }

                task = _cacheTaskGettingEntity[entityName];

                if (task != null)
                {
                    await task;
                }

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
            }

            var hashRelatedEntities = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var entityName in hash)
            {
                if (_connectionData.EntitiesIntellisenseData.Entities.ContainsKey(entityName))
                {
                    var entityData = _connectionData.EntitiesIntellisenseData.Entities[entityName];

                    var related = entityData.GetRelatedEntities();

                    foreach (var item in related)
                    {
                        hashRelatedEntities.Add(item);
                    }
                }
            }

            foreach (var item in hash)
            {
                hashRelatedEntities.Remove(item);
            }

            foreach (var entityName in hashRelatedEntities)
            {
                Task task = null;

                if (!_cacheTaskGettingEntity.ContainsKey(entityName))
                {
                    _cacheTaskGettingEntity.TryAdd(entityName, Task.Run(() => GetEntityFullDataForNameAsync(entityName), _cancellationTokenSource.Token));
                }

                task = _cacheTaskGettingEntity[entityName];

                if (task != null)
                {
                    await task;
                }

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private async Task GetEntityDataForObjectTypeCodes(IEnumerable<int> entityObjectTypeCodes)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            var hash = new HashSet<int>(entityObjectTypeCodes);

            foreach (var entityObjectTypeCode in hash)
            {
                Task task = null;

                if (!_cacheTaskGettingEntityObjectTypeCode.ContainsKey(entityObjectTypeCode))
                {
                    task = Task.Run(() => GetEntityFullDataForObjectTypeCodeAsync(entityObjectTypeCode), _cancellationTokenSource.Token);
                    _cacheTaskGettingEntityObjectTypeCode.TryAdd(entityObjectTypeCode, task);
                }

                task = _cacheTaskGettingEntityObjectTypeCode[entityObjectTypeCode];

                if (task != null)
                {
                    await task;
                }

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
            }

            var hashRelatedEntities = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var entityObjectTypeCode in hash)
            {
                var entityData = _connectionData.EntitiesIntellisenseData?.Entities?.Values?.FirstOrDefault(e => e.ObjectTypeCode.HasValue && e.ObjectTypeCode == entityObjectTypeCode);

                if (entityData != null)
                {
                    var related = entityData.GetRelatedEntities();

                    foreach (var item in related)
                    {
                        hashRelatedEntities.Add(item);
                    }
                }
            }

            foreach (var entityObjectTypeCode in hash)
            {
                var entityData = _connectionData.EntitiesIntellisenseData?.Entities?.Values?.FirstOrDefault(e => e.ObjectTypeCode.HasValue && e.ObjectTypeCode == entityObjectTypeCode);

                if (entityData != null)
                {
                    hashRelatedEntities.Remove(entityData.EntityLogicalName);
                }
            }

            foreach (var entityName in hashRelatedEntities)
            {
                Task task = null;

                if (!_cacheTaskGettingEntity.ContainsKey(entityName))
                {
                    _cacheTaskGettingEntity.TryAdd(entityName, Task.Run(() => GetEntityFullDataForNameAsync(entityName), _cancellationTokenSource.Token));
                }

                task = _cacheTaskGettingEntity[entityName];

                if (task != null)
                {
                    await task;
                }

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private async Task GetEntityFullDataForNameAsync(string entityName)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            if (string.IsNullOrEmpty(entityName))
            {
                return;
            }

            var service = await GetServiceAsync();

            if (service == null)
            {
                return;
            }

            EntityQueryExpression entityQueryExpression = GetEntityQueryExpression();

            entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.Equals, entityName));

            var request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            try
            {
                var response = await service.ExecuteAsync<RetrieveMetadataChangesResponse>(request);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            if (_cacheTaskGettingEntity.ContainsKey(entityName))
            {
                _cacheTaskGettingEntity.TryRemove(entityName, out _);
            }
        }

        private async Task GetEntityFullDataForObjectTypeCodeAsync(int objectTypeCode)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            var service = await GetServiceAsync();

            if (service == null)
            {
                return;
            }

            EntityQueryExpression entityQueryExpression = GetEntityQueryExpression();

            entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.ObjectTypeCode), MetadataConditionOperator.Equals, objectTypeCode));

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            try
            {
                var response = (RetrieveMetadataChangesResponse)service.Execute(request);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }

            if (_cacheTaskGettingEntityObjectTypeCode.ContainsKey(objectTypeCode))
            {
                _cacheTaskGettingEntityObjectTypeCode.TryRemove(objectTypeCode, out _);
            }
        }

        private void StartGettingListEntityHeader()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            var hashEntities = new HashSet<string>();

            var intellisenseData = this._connectionData.EntitiesIntellisenseData;

            if (intellisenseData != null && intellisenseData.Entities != null)
            {
                hashEntities = new HashSet<string>(intellisenseData.Entities.Keys, StringComparer.InvariantCultureIgnoreCase);
            }

            lock (syncObjectTaskListEntityHeader)
            {
                if (!_taskListEntityHeaderCompleted)
                {
                    if (_taskListEntityHeader != null)
                    {
                        if (_taskListEntityHeader.Status == TaskStatus.RanToCompletion)
                        {
                            _taskListEntityHeader = null;

                            _taskListEntityHeaderCompleted = true;
                        }
                        else if (_taskListEntityHeader.Status == TaskStatus.Faulted)
                        {
                            DTEHelper.WriteExceptionToLog(_taskListEntityHeader.Exception);

                            _taskListEntityHeader = Task.Run(() => GetListEntityHeaderDataAsync(hashEntities), _cancellationTokenSource.Token);
                        }
                    }
                    else
                    {
                        _taskListEntityHeader = Task.Run(() => GetListEntityHeaderDataAsync(hashEntities), _cancellationTokenSource.Token);
                    }
                }
            }
        }

        private static readonly object _staticSyncObjectCommon = new object();
        private static ConcurrentDictionary<Guid, ConnectionIntellisenseDataRepository> _staticCacheRepositories = new ConcurrentDictionary<Guid, ConnectionIntellisenseDataRepository>();

        private static readonly string[] _entityMetadataShortFieldsToQuery = new string[]
        {
            nameof(EntityMetadata.LogicalName)
            , nameof(EntityMetadata.SchemaName)

            , nameof(EntityMetadata.DisplayName)
            , nameof(EntityMetadata.Description)
            , nameof(EntityMetadata.DisplayCollectionName)

            , nameof(EntityMetadata.ObjectTypeCode)
            , nameof(EntityMetadata.IsIntersect)
            , nameof(EntityMetadata.PrimaryIdAttribute)
            , nameof(EntityMetadata.PrimaryNameAttribute)
        };

        private static readonly string[] _entityMetadataFullFieldsToQuery = new string[]
        {
            nameof(EntityMetadata.LogicalName)
            , nameof(EntityMetadata.SchemaName)

            , nameof(EntityMetadata.DisplayName)
            , nameof(EntityMetadata.Description)
            , nameof(EntityMetadata.DisplayCollectionName)

            , nameof(EntityMetadata.ObjectTypeCode)
            , nameof(EntityMetadata.IsIntersect)
            , nameof(EntityMetadata.PrimaryIdAttribute)
            , nameof(EntityMetadata.PrimaryNameAttribute)

            , nameof(EntityMetadata.Attributes)
            , nameof(EntityMetadata.OneToManyRelationships)
            , nameof(EntityMetadata.ManyToOneRelationships)
            , nameof(EntityMetadata.ManyToManyRelationships)
        };

        private static readonly string[] _attributeFieldsToQuery = new string[]
        {
            nameof(AttributeMetadata.LogicalName)
            , nameof(AttributeMetadata.AttributeOf)
            , nameof(AttributeMetadata.AttributeType)
            , nameof(AttributeMetadata.SchemaName)
            , nameof(AttributeMetadata.EntityLogicalName)
            , nameof(AttributeMetadata.DisplayName)
            , nameof(AttributeMetadata.Description)
            , nameof(AttributeMetadata.IsPrimaryId)
            , nameof(AttributeMetadata.IsPrimaryName)

            , nameof(EnumAttributeMetadata.OptionSet)
            , nameof(LookupAttributeMetadata.Targets)
        };

        private static readonly string[] _relationshipFieldsToQuery = new string[]
        {
            nameof(RelationshipMetadataBase.SchemaName)

            , nameof(OneToManyRelationshipMetadata.ReferencedEntity)
            , nameof(OneToManyRelationshipMetadata.ReferencedAttribute)
            , nameof(OneToManyRelationshipMetadata.ReferencingEntity)
            , nameof(OneToManyRelationshipMetadata.ReferencingAttribute)

            , nameof(ManyToManyRelationshipMetadata.IntersectEntityName)
            , nameof(ManyToManyRelationshipMetadata.Entity1LogicalName)
            , nameof(ManyToManyRelationshipMetadata.Entity2LogicalName)
            , nameof(ManyToManyRelationshipMetadata.Entity1IntersectAttribute)
            , nameof(ManyToManyRelationshipMetadata.Entity2IntersectAttribute)
        };

        static ConnectionIntellisenseDataRepository()
        {
            Task.Run(() => LoadIntellisenseCacheFromDisk());
        }

        public static void LoadStaticConstructor()
        {

        }

        private static void LoadIntellisenseCacheFromDisk()
        {
            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData != null)
            {
                try
                {
                    GetRepository(connectionConfig.CurrentConnectionData);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);
                }
            }

            foreach (var connectionData in connectionConfig.Connections)
            {
                if (connectionData != connectionConfig.CurrentConnectionData)
                {
                    try
                    {
                        GetRepository(connectionData);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);
                    }
                }
            }
        }

        public static ConnectionIntellisenseDataRepository GetRepository(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return null;
            }

            lock (_staticSyncObjectCommon)
            {
                if (!_staticCacheRepositories.ContainsKey(connectionData.ConnectionId))
                {
                    var repository = new ConnectionIntellisenseDataRepository(connectionData);

                    _staticCacheRepositories.TryAdd(connectionData.ConnectionId, repository);
                }

                return _staticCacheRepositories[connectionData.ConnectionId];
            }
        }

        public static EntityQueryExpression GetEntityQueryExpression()
        {
            var result = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression(_entityMetadataFullFieldsToQuery),

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression(_attributeFieldsToQuery),
                },

                RelationshipQuery = new RelationshipQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression(_relationshipFieldsToQuery),
                },
            };

            return result;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            disposedValue = true;

            if (disposing)
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _cancellationTokenSource.Cancel();
                }

                _cancellationTokenSource.Dispose();
            }
        }

        ~ConnectionIntellisenseDataRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}