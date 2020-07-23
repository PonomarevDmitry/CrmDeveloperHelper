using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    internal class OrganizationServiceExtentedProxy : IOrganizationServiceExtented, IDisposable
    {
        private OrganizationServiceProxy _serviceProxy;

        public string CurrentServiceEndpoint { get; private set; }

        public ConnectionData ConnectionData { get; private set; }

        public ConnectionDataUrlGenerator UrlGenerator { get; private set; }

        public OrganizationServiceExtentedProxy(OrganizationServiceProxy serviceProxy, ConnectionData connectionData, string currentServiceEndpoint)
        {
            this._serviceProxy = serviceProxy ?? throw new ArgumentNullException(nameof(serviceProxy));
            this.ConnectionData = connectionData ?? throw new ArgumentNullException(nameof(connectionData));
            this.UrlGenerator = new ConnectionDataUrlGenerator(this);

            this.CurrentServiceEndpoint = currentServiceEndpoint;

            this.ConnectionData.StoreServiceInUse(serviceProxy);
        }

        public event EventHandler<TryDisposeOrganizationServiceExtentedEventArgs> TryingDispose;

        public void TryDispose()
        {
            if (disposedValue)
            {
                return;
            }

            var eventArgs = new TryDisposeOrganizationServiceExtentedEventArgs();

            TryingDispose?.Invoke(this, eventArgs);

            if (eventArgs.IsDisposingCanceled)
            {
                return;
            }

            this.Dispose();
        }

        public Task<Guid> CreateAsync(Entity entity)
        {
            ThrowExceptionIfDisposed();

            return Task.Run(() => Create(entity));
        }

        public Guid Create(Entity entity)
        {
            ThrowExceptionIfDisposed();

            try
            {
                FilterAttributes(entity);

                return _serviceProxy.Create(entity);
            }
            catch (Exception ex)
            {
                var serializer = new DataContractSerializer(entity.GetType());

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                var serializeString = new StringBuilder();

                using (var stringWriter = new StringWriter(serializeString))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, entity);
                        xmlWriter.Flush();
                    }
                }

                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.Create{Environment.NewLine}{serializeString.ToString()}");
                throw;
            }
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            ThrowExceptionIfDisposed();

            try
            {
                columnSet = FilterColumns(entityName, columnSet);

                return _serviceProxy.Retrieve(entityName, id, columnSet);
            }
            catch (Exception ex)
            {
                var serializer = new DataContractSerializer(typeof(ColumnSet));

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                var serializeString = new StringBuilder();

                using (var stringWriter = new StringWriter(serializeString))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, columnSet);
                        xmlWriter.Flush();
                    }
                }

                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.Retrieve({entityName}, {id}{Environment.NewLine}{serializeString.ToString()}");
                throw;
            }
        }

        public T Retrieve<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity
        {
            ThrowExceptionIfDisposed();

            return this.Retrieve(entityName, id, columnSet).ToEntity<T>();
        }

        public Task<T> RetrieveAsync<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity
        {
            ThrowExceptionIfDisposed();

            return Task.Run(() => Retrieve<T>(entityName, id, columnSet));
        }

        public Task<T> RetrieveByQueryAsync<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity
        {
            ThrowExceptionIfDisposed();

            return Task.Run(() => RetrieveByQuery<T>(entityName, id, columnSet));
        }

        public T RetrieveByQuery<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity
        {
            ThrowExceptionIfDisposed();

            try
            {
                var entityData = GetEntityIntellisenseData(entityName);

                if (entityData != null && !string.IsNullOrEmpty(entityData.EntityPrimaryIdAttribute))
                {
                    var query = new QueryExpression()
                    {
                        NoLock = true,

                        EntityName = entityName,

                        ColumnSet = columnSet,

                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(entityData.EntityPrimaryIdAttribute, ConditionOperator.Equal, id),
                            },
                        },
                    };

                    return this.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<T>()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                var serializer = new DataContractSerializer(typeof(ColumnSet));

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                var serializeString = new StringBuilder();

                using (var stringWriter = new StringWriter(serializeString))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, columnSet);
                        xmlWriter.Flush();
                    }
                }

                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.RetrieveByQuery({entityName}, {id}{Environment.NewLine}{serializeString.ToString()}");
                throw;
            }

            return null;
        }

        public Task UpdateAsync(Entity entity)
        {
            ThrowExceptionIfDisposed();

            return Task.Run(() => Update(entity));
        }

        public void Update(Entity entity)
        {
            ThrowExceptionIfDisposed();

            try
            {
                FilterAttributes(entity);

                _serviceProxy.Update(entity);
            }
            catch (Exception ex)
            {
                var serializer = new DataContractSerializer(entity.GetType());

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                var serializeString = new StringBuilder();

                using (var stringWriter = new StringWriter(serializeString))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, entity);
                        xmlWriter.Flush();
                    }
                }

                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.Update{Environment.NewLine}{serializeString.ToString()}");
                throw;
            }
        }

        public Task DeleteAsync(string entityName, Guid id)
        {
            ThrowExceptionIfDisposed();

            return Task.Run(() => Delete(entityName, id));
        }

        public void Delete(string entityName, Guid id)
        {
            ThrowExceptionIfDisposed();

            try
            {
                _serviceProxy.Delete(entityName, id);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.Delete({entityName}, {id})");
                throw;
            }
        }

        public async Task<Guid> UpsertAsync(Entity entity)
        {
            ThrowExceptionIfDisposed();

            Guid entityId = entity.Id;

            var entityData = GetEntityIntellisenseData(entity.LogicalName);

            if (!string.IsNullOrEmpty(entityData.EntityPrimaryIdAttribute)
                && entity.Attributes.ContainsKey(entityData.EntityPrimaryIdAttribute)
                && entity.Attributes[entityData.EntityPrimaryIdAttribute] != null
                && entity.Attributes[entityData.EntityPrimaryIdAttribute] is Guid tempId
            )
            {
                entityId = tempId;
            }

            if (entityId == Guid.Empty)
            {
                return await CreateAsync(entity);
            }
            else
            {
                entity.Id = entityId;

                var exists = await RetrieveByQueryAsync<Entity>(entity.LogicalName, entityId, new ColumnSet(false));

                if (exists != null)
                {
                    await UpdateAsync(entity);

                    return entityId;
                }
                else
                {
                    return await CreateAsync(entity);
                }
            }
        }

        public Task<T> ExecuteAsync<T>(OrganizationRequest request) where T : OrganizationResponse
        {
            ThrowExceptionIfDisposed();

            return Task.Run(() => (T)Execute(request));
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            ThrowExceptionIfDisposed();

            try
            {
                var response = _serviceProxy.Execute(request);

                if (response is RetrieveEntityResponse retrieveEntityResponse && retrieveEntityResponse.EntityMetadata != null)
                {
                    Task.Run(() => this.ConnectionData.EntitiesIntellisenseData.LoadData(retrieveEntityResponse.EntityMetadata));
                }

                //if (response is RetrieveAttributeResponse retrieveAttributeResponse && retrieveAttributeResponse.AttributeMetadata != null)
                //{
                //    this.ConnectionData.IntellisenseData.LoadData(retrieveAttributeResponse.AttributeMetadata);
                //}

                //if (response is RetrieveRelationshipResponse retrieveRelationshipResponse && retrieveRelationshipResponse.RelationshipMetadata != null)
                //{
                //    this.ConnectionData.IntellisenseData.LoadData(retrieveRelationshipResponse.RelationshipMetadata);
                //}

                if (response is RetrieveMetadataChangesResponse retrieveMetadataChangesResponse && retrieveMetadataChangesResponse.EntityMetadata != null)
                {
                    Task.Run(() => this.ConnectionData.EntitiesIntellisenseData.LoadSomeData(retrieveMetadataChangesResponse.EntityMetadata));
                }

                if (response is RetrieveAllEntitiesResponse retrieveAllEntitiesResponse && retrieveAllEntitiesResponse.EntityMetadata != null)
                {
                    Task.Run(() => this.ConnectionData.EntitiesIntellisenseData.LoadFullData(retrieveAllEntitiesResponse.EntityMetadata));
                }

                return response;
            }
            catch (Exception ex)
            {
                var serializer = new DataContractSerializer(request.GetType());

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                var serializeString = new StringBuilder();

                using (var stringWriter = new StringWriter(serializeString))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, request);
                        xmlWriter.Flush();
                    }
                }

                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.Execute{Environment.NewLine}{serializeString.ToString()}");
                throw;
            }
        }

        private OrganizationResponse ExecuteWithSyncMetadataHandling(OrganizationRequest request)
        {
            ThrowExceptionIfDisposed();

            try
            {
                var response = _serviceProxy.Execute(request);

                if (response is RetrieveEntityResponse retrieveEntityResponse && retrieveEntityResponse.EntityMetadata != null)
                {
                    this.ConnectionData.EntitiesIntellisenseData.LoadData(retrieveEntityResponse.EntityMetadata);
                }

                //if (response is RetrieveAttributeResponse retrieveAttributeResponse && retrieveAttributeResponse.AttributeMetadata != null)
                //{
                //    this.ConnectionData.IntellisenseData.LoadData(retrieveAttributeResponse.AttributeMetadata);
                //}

                //if (response is RetrieveRelationshipResponse retrieveRelationshipResponse && retrieveRelationshipResponse.RelationshipMetadata != null)
                //{
                //    this.ConnectionData.IntellisenseData.LoadData(retrieveRelationshipResponse.RelationshipMetadata);
                //}

                if (response is RetrieveMetadataChangesResponse retrieveMetadataChangesResponse && retrieveMetadataChangesResponse.EntityMetadata != null)
                {
                    this.ConnectionData.EntitiesIntellisenseData.LoadSomeData(retrieveMetadataChangesResponse.EntityMetadata);
                }

                if (response is RetrieveAllEntitiesResponse retrieveAllEntitiesResponse && retrieveAllEntitiesResponse.EntityMetadata != null)
                {
                    this.ConnectionData.EntitiesIntellisenseData.LoadFullData(retrieveAllEntitiesResponse.EntityMetadata);
                }

                return response;
            }
            catch (Exception ex)
            {
                var serializer = new DataContractSerializer(request.GetType());

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                var serializeString = new StringBuilder();

                using (var stringWriter = new StringWriter(serializeString))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, request);
                        xmlWriter.Flush();
                    }
                }

                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.ExecuteWithSyncMetadataHandling{Environment.NewLine}{serializeString.ToString()}");
                throw;
            }
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            ThrowExceptionIfDisposed();

            try
            {
                _serviceProxy.Associate(entityName, entityId, relationship, relatedEntities);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            ThrowExceptionIfDisposed();

            try
            {
                _serviceProxy.Disassociate(entityName, entityId, relationship, relatedEntities);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public EntityCollection RetrieveMultiple(QueryBase queryBase)
        {
            ThrowExceptionIfDisposed();

            try
            {
                if (queryBase is QueryExpression query)
                {
                    Dictionary<string, string> aliases = GetAliases(query);

                    query.ColumnSet = FilterColumns(query.EntityName, query.ColumnSet);

                    FilterFilterExpression(query.EntityName, query.Criteria, aliases);

                    FilterLinkEntities(query.LinkEntities, aliases);

                    FilterOrders(query.EntityName, query.Orders);
                }

                return _serviceProxy.RetrieveMultiple(queryBase);
            }
            catch (Exception ex)
            {
                var serializer = new DataContractSerializer(queryBase.GetType());

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                var serializeString = new StringBuilder();

                using (var stringWriter = new StringWriter(serializeString))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, queryBase);
                        xmlWriter.Flush();
                    }
                }

                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.RetrieveMultiple{Environment.NewLine}{serializeString.ToString()}");
                throw;
            }
        }

        public bool IsRequestExists(string requestName)
        {
            ThrowExceptionIfDisposed();

            var result = this.ConnectionData.IsRequestExists(requestName);

            if (result.HasValue)
            {
                return result.Value;
            }

            var repository = new SdkMessageRequestRepository(this);

            var request = repository.FindByRequestName(requestName, new ColumnSet(false));

            bool isRequestExists = request != null;

            this.ConnectionData.SetRequestExistance(requestName, isRequestExists);

            return isRequestExists;
        }

        public List<T> RetrieveMultipleAll<T>(QueryExpression query) where T : Entity
        {
            ThrowExceptionIfDisposed();

            {
                Dictionary<string, string> aliases = GetAliases(query);

                query.ColumnSet = FilterColumns(query.EntityName, query.ColumnSet);

                FilterFilterExpression(query.EntityName, query.Criteria, aliases);

                FilterLinkEntities(query.LinkEntities, aliases);

                FilterOrders(query.EntityName, query.Orders);
            }

            query.PageInfo = new PagingInfo()
            {
                PageNumber = 1,
                Count = 5000,
            };

            var result = new List<T>();

            try
            {
                while (true)
                {
                    var coll = _serviceProxy.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<T>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                var serializer = new DataContractSerializer(query.GetType());

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                var serializeString = new StringBuilder();

                using (var stringWriter = new StringWriter(serializeString))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, query);
                        xmlWriter.Flush();
                    }
                }

                Helpers.DTEHelper.WriteExceptionToLog(ex, $"OrganizationServiceExtentedProxy.RetrieveMultipleAll{Environment.NewLine}{serializeString.ToString()}");
                Helpers.DTEHelper.WriteExceptionToOutput(ConnectionData, ex);
            }

            return result;
        }

        public Task<List<T>> RetrieveMultipleAllAsync<T>(QueryExpression query) where T : Entity
        {
            ThrowExceptionIfDisposed();

            return Task.Run(() => RetrieveMultipleAll<T>(query));
        }

        #region Private Members

        private Dictionary<string, string> GetAliases(QueryExpression query)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            FillAliases(result, query.LinkEntities);

            return result;
        }

        private void FillAliases(Dictionary<string, string> result, DataCollection<LinkEntity> linkEntities)
        {
            foreach (var link in linkEntities)
            {
                if (!string.IsNullOrEmpty(link.EntityAlias) && !result.ContainsKey(link.EntityAlias))
                {
                    result.Add(link.EntityAlias, link.LinkToEntityName);
                }

                FillAliases(result, link.LinkEntities);
            }
        }

        private void FilterLinkEntities(DataCollection<LinkEntity> linkEntities, Dictionary<string, string> aliases)
        {
            if (linkEntities == null)
            {
                return;
            }

            foreach (var link in linkEntities)
            {
                if (!string.IsNullOrEmpty(link.LinkToEntityName)
                    && !string.IsNullOrEmpty(link.LinkToAttributeName)
                )
                {
                    link.Columns = FilterColumns(link.LinkToEntityName, link.Columns);

                    FilterFilterExpression(link.LinkToEntityName, link.LinkCriteria, aliases);

                    FilterLinkEntities(link.LinkEntities, aliases);
                }
            }
        }

        private void FilterFilterExpression(string entityName, FilterExpression criteria, Dictionary<string, string> aliases)
        {
            if (criteria.Conditions != null)
            {
                var toDelete = new List<ConditionExpression>();

                foreach (var condition in criteria.Conditions)
                {
                    string conditionEntity = entityName;

                    if (!string.IsNullOrEmpty(condition.EntityName))
                    {
                        if (aliases.ContainsKey(condition.EntityName))
                        {
                            conditionEntity = aliases[condition.EntityName];
                        }
                        else
                        {
                            conditionEntity = condition.EntityName;
                        }
                    }

                    HashSet<string> attributes = GetEntityAttributes(conditionEntity);

                    if (attributes != null)
                    {
                        if (!attributes.Contains(condition.AttributeName))
                        {
                            toDelete.Add(condition);
                        }
                    }
                }

                foreach (var item in toDelete)
                {
                    criteria.Conditions.Remove(item);
                }
            }

            if (criteria.Filters != null)
            {
                foreach (var item in criteria.Filters)
                {
                    FilterFilterExpression(entityName, item, aliases);
                }
            }
        }

        private void FilterOrders(string entityName, DataCollection<OrderExpression> orders)
        {
            if (orders == null)
            {
                return;
            }

            HashSet<string> attributes = GetEntityAttributes(entityName);

            if (attributes != null)
            {
                var toDelete = orders.Where(s => !attributes.Contains(s.AttributeName)).ToList();

                foreach (var item in toDelete)
                {
                    orders.Remove(item);
                }
            }
        }

        private HashSet<string> GetEntityAttributes(string entityName)
        {
            var result = this.ConnectionData.GetEntityAttributes(entityName);

            if (result != null)
            {
                return result;
            }

            EntityQueryExpression entityQueryExpression = ConnectionIntellisenseDataRepository.GetEntityQueryExpression();

            entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression(nameof(Entity.LogicalName), MetadataConditionOperator.Equals, entityName));

            var response = (RetrieveMetadataChangesResponse)this.ExecuteWithSyncMetadataHandling(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            return this.ConnectionData.GetEntityAttributes(entityName);
        }

        private EntityIntellisenseData GetEntityIntellisenseData(string entityName)
        {
            var result = this.ConnectionData.GetEntityIntellisenseData(entityName);

            if (result != null)
            {
                return result;
            }

            EntityQueryExpression entityQueryExpression = ConnectionIntellisenseDataRepository.GetEntityQueryExpression();

            entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression(nameof(Entity.LogicalName), MetadataConditionOperator.Equals, entityName));

            var response = (RetrieveMetadataChangesResponse)this.ExecuteWithSyncMetadataHandling(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            return this.ConnectionData.GetEntityIntellisenseData(entityName);
        }

        private ColumnSet FilterColumns(string entityName, ColumnSet columnSet)
        {
            if (columnSet == null)
            {
                return null;
            }

            if (columnSet.AllColumns)
            {
                return columnSet;
            }

            ColumnSet result = new ColumnSet();
            result.Columns.AddRange(columnSet.Columns);

            HashSet<string> attributes = GetEntityAttributes(entityName);

            if (attributes != null)
            {
                var toDelete = result.Columns.Where(s => !attributes.Contains(s)).ToList();

                foreach (var item in toDelete)
                {
                    result.Columns.Remove(item);
                }
            }

            return result;
        }

        private void FilterAttributes(Entity entity)
        {
            HashSet<string> attributes = GetEntityAttributes(entity.LogicalName);

            if (attributes != null)
            {
                var toDelete = entity.Attributes.Keys.Where(s => !attributes.Contains(s)).ToList();

                foreach (var item in toDelete)
                {
                    entity.Attributes.Remove(item);
                }
            }
        }

        private void ThrowExceptionIfDisposed()
        {
            if (!disposedValue)
            {
                return;
            }

            throw new ObjectDisposedException(nameof(OrganizationServiceExtentedProxy));
        }

        #endregion Private Members

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            this.ConnectionData.ReturnServiceToFree(this._serviceProxy);

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).

                this._serviceProxy = null;
                this.TryingDispose = null;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }

        ~OrganizationServiceExtentedProxy()
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