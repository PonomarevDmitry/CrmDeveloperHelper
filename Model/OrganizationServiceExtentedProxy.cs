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
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    internal class OrganizationServiceExtentedProxy : IOrganizationServiceExtented
    {
        private IOrganizationService _service;

        public string CurrentServiceEndpoint { get; private set; }

        public ConnectionData ConnectionData { get; private set; }

        public ConnectionDataUrlGenerator UrlGenerator { get; private set; }

        public OrganizationServiceExtentedProxy(OrganizationServiceProxy service, ConnectionData connectionData)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
            this.ConnectionData = connectionData ?? throw new ArgumentNullException(nameof(connectionData));
            this.UrlGenerator = new ConnectionDataUrlGenerator(this);

            this.CurrentServiceEndpoint = service.ServiceManagement?.CurrentServiceEndpoint?.Address?.Uri?.ToString();
        }

        public Task<Guid> CreateAsync(Entity entity)
        {
            return Task.Run(() => Create(entity));
        }

        public Guid Create(Entity entity)
        {
            try
            {
                FilterAttributes(entity);

                return _service.Create(entity);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            try
            {
                columnSet = FilterColumns(entityName, columnSet);

                return _service.Retrieve(entityName, id, columnSet);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public Task<T> RetrieveByQueryAsync<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity
        {
            return Task.Run(() => RetrieveByQuery<T>(entityName, id, columnSet));
        }

        public T RetrieveByQuery<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity
        {
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
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }

            return null;
        }

        public Task UpdateAsync(Entity entity)
        {
            return Task.Run(() => Update(entity));
        }

        public void Update(Entity entity)
        {
            try
            {
                FilterAttributes(entity);

                _service.Update(entity);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public Task DeleteAsync(string entityName, Guid id)
        {
            return Task.Run(() => Delete(entityName, id));
        }

        public void Delete(string entityName, Guid id)
        {
            try
            {
                _service.Delete(entityName, id);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            try
            {
                var response = _service.Execute(request);

                if (response is RetrieveEntityResponse retrieveEntityResponse && retrieveEntityResponse.EntityMetadata != null)
                {
                    Task.Run(() => this.ConnectionData.IntellisenseData.LoadData(retrieveEntityResponse.EntityMetadata));
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
                    Task.Run(() => this.ConnectionData.IntellisenseData.LoadFullData(retrieveMetadataChangesResponse.EntityMetadata));
                }

                if (response is RetrieveAllEntitiesResponse retrieveAllEntitiesResponse && retrieveAllEntitiesResponse.EntityMetadata != null)
                {
                    Task.Run(() => this.ConnectionData.IntellisenseData.LoadFullData(retrieveAllEntitiesResponse.EntityMetadata));
                }

                return response;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        private OrganizationResponse ExecuteWithSyncMetadataHandling(OrganizationRequest request)
        {
            try
            {
                var response = _service.Execute(request);

                if (response is RetrieveEntityResponse retrieveEntityResponse && retrieveEntityResponse.EntityMetadata != null)
                {
                    this.ConnectionData.IntellisenseData.LoadData(retrieveEntityResponse.EntityMetadata);
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
                    this.ConnectionData.IntellisenseData.LoadFullData(retrieveMetadataChangesResponse.EntityMetadata);
                }

                if (response is RetrieveAllEntitiesResponse retrieveAllEntitiesResponse && retrieveAllEntitiesResponse.EntityMetadata != null)
                {
                    this.ConnectionData.IntellisenseData.LoadFullData(retrieveAllEntitiesResponse.EntityMetadata);
                }

                return response;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            try
            {
                _service.Associate(entityName, entityId, relationship, relatedEntities);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            try
            {
                _service.Disassociate(entityName, entityId, relationship, relatedEntities);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

        public EntityCollection RetrieveMultiple(QueryBase queryBase)
        {
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

                return _service.RetrieveMultiple(queryBase);
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToLog(ex);
                throw;
            }
        }

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

            entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, entityName));

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

            entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, entityName));

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

        public bool IsRequestExists(string requestName)
        {
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
    }
}