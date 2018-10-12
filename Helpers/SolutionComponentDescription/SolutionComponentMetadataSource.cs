using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription
{
    public class SolutionComponentMetadataSource
    {
        public IOrganizationServiceExtented Service { get; private set; }

        public SolutionComponentMetadataSource(IOrganizationServiceExtented service)
        {
            this.Service = service;
        }

        private readonly object _syncObjectEntityCache = new object();

        private readonly object _syncObjectOptionSets = new object();

        private readonly object _syncObjectEntityMetadata = new object();

        private readonly object _syncObjectAttributeMetadata = new object();

        private readonly object _syncObjectEntityKeyMetadata = new object();

        private readonly object _syncObjectRelationshipMetadata = new object();

        private ConcurrentDictionary<Guid, EntityMetadata> _dictEntity = new ConcurrentDictionary<Guid, EntityMetadata>();

        private ConcurrentDictionary<Guid, AttributeMetadata> _dictAttribute = new ConcurrentDictionary<Guid, AttributeMetadata>();

        private ConcurrentDictionary<Guid, EntityKeyMetadata> _dictKey = new ConcurrentDictionary<Guid, EntityKeyMetadata>();

        private ConcurrentDictionary<Guid, RelationshipMetadataBase> _dictRelashionship = new ConcurrentDictionary<Guid, RelationshipMetadataBase>();

        private ConcurrentDictionary<Guid, OptionSetMetadataBase> _allOptionSetMetadata;

        private bool _allMetadataDownloaded = false;

        public ConcurrentDictionary<Guid, OptionSetMetadataBase> AllOptionSetMetadata
        {
            get
            {
                lock (_syncObjectOptionSets)
                {
                    if (_allOptionSetMetadata == null)
                    {
                        var request = new RetrieveAllOptionSetsRequest();
                        var response = (RetrieveAllOptionSetsResponse)this.Service.Execute(request);

                        _allOptionSetMetadata = new ConcurrentDictionary<Guid, OptionSetMetadataBase>();

                        foreach (var item in response.OptionSetMetadata)
                        {
                            if (!_allOptionSetMetadata.ContainsKey(item.MetadataId.Value))
                            {
                                _allOptionSetMetadata.TryAdd(item.MetadataId.Value, item);
                            }
                        }
                    }
                }

                return _allOptionSetMetadata;
            }
        }

        public void DownloadEntityMetadata()
        {
            if (this._allMetadataDownloaded)
            {
                return;
            }

            this._allMetadataDownloaded = true;

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression() { AllProperties = true },
                AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                LabelQuery = new LabelQueryExpression(),
            };

            var isEntityKeyExists = Service.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
            }

            var response = (RetrieveMetadataChangesResponse)Service.Execute(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            foreach (var metaEntity in response.EntityMetadata)
            {
                HandleEntityMetadata(metaEntity);
            }
        }

        public void DownloadEntityMetadataForNames(string[] entityNames, bool changeAllMetadataDownloaded)
        {
            if (changeAllMetadataDownloaded)
            {
                if (this._allMetadataDownloaded)
                {
                    return;
                }

                this._allMetadataDownloaded = true;
            }

            MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.In, entityNames));

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression() { AllProperties = true },
                AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                LabelQuery = new LabelQueryExpression(),

                Criteria = entityFilter,
            };

            var isEntityKeyExists = Service.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
            }

            var response = (RetrieveMetadataChangesResponse)Service.Execute(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            foreach (var metaEntity in response.EntityMetadata)
            {
                HandleEntityMetadata(metaEntity);
            }
        }

        public Task DownloadEntityMetadataOnlyForNamesAsync(string[] entityNames, string[] columnNames, bool changeAllMetadataDownloaded)
        {
            return Task.Run(() => DownloadEntityMetadataOnlyForNames(entityNames, columnNames, changeAllMetadataDownloaded));
        }

        private void DownloadEntityMetadataOnlyForNames(string[] entityNames, string[] columnNames, bool changeAllMetadataDownloaded)
        {
            if (changeAllMetadataDownloaded)
            {
                if (this._allMetadataDownloaded)
                {
                    return;
                }

                this._allMetadataDownloaded = true;
            }

            MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.In, entityNames));

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression(columnNames),
                LabelQuery = new LabelQueryExpression(),

                Criteria = entityFilter,
            };

            var response = (RetrieveMetadataChangesResponse)Service.Execute(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            foreach (var metaEntity in response.EntityMetadata)
            {
                HandleEntityMetadata(metaEntity);
            }
        }

        private void HandleEntityMetadata(EntityMetadata metaEntity)
        {
            lock (_syncObjectEntityMetadata)
            {
                if (!_dictEntity.ContainsKey(metaEntity.MetadataId.Value))
                {
                    _dictEntity.TryAdd(metaEntity.MetadataId.Value, metaEntity);
                }
            }


            if (metaEntity.Attributes != null)
            {
                foreach (var metaAttribute in metaEntity.Attributes)
                {
                    lock (_syncObjectAttributeMetadata)
                    {
                        if (!_dictAttribute.ContainsKey(metaAttribute.MetadataId.Value))
                        {
                            _dictAttribute.TryAdd(metaAttribute.MetadataId.Value, metaAttribute);
                        }
                    }

                    if (metaAttribute is StatusAttributeMetadata statusAttributeMetadata
                            && statusAttributeMetadata.OptionSet != null
                        )
                    {
                        lock (_syncObjectOptionSets)
                        {
                            if (!this.AllOptionSetMetadata.ContainsKey(statusAttributeMetadata.OptionSet.MetadataId.Value))
                            {
                                this.AllOptionSetMetadata.TryAdd(statusAttributeMetadata.OptionSet.MetadataId.Value, statusAttributeMetadata.OptionSet);
                            }
                        }
                    }

                    if (metaAttribute is StateAttributeMetadata stateAttributeMetadata
                            && stateAttributeMetadata.OptionSet != null
                        )
                    {
                        lock (_syncObjectOptionSets)
                        {
                            if (!this.AllOptionSetMetadata.ContainsKey(stateAttributeMetadata.OptionSet.MetadataId.Value))
                            {
                                this.AllOptionSetMetadata.TryAdd(stateAttributeMetadata.OptionSet.MetadataId.Value, stateAttributeMetadata.OptionSet);
                            }
                        }
                    }

                    if (metaAttribute is BooleanAttributeMetadata booleanAttributeMetadata
                            && booleanAttributeMetadata.OptionSet != null
                        )
                    {
                        lock (_syncObjectOptionSets)
                        {
                            if (!this.AllOptionSetMetadata.ContainsKey(booleanAttributeMetadata.OptionSet.MetadataId.Value))
                            {
                                this.AllOptionSetMetadata.TryAdd(booleanAttributeMetadata.OptionSet.MetadataId.Value, booleanAttributeMetadata.OptionSet);
                            }
                        }
                    }

                    if (metaAttribute is PicklistAttributeMetadata picklistAttributeMetadata
                            && picklistAttributeMetadata.OptionSet != null
                        )
                    {
                        lock (_syncObjectOptionSets)
                        {
                            if (!this.AllOptionSetMetadata.ContainsKey(picklistAttributeMetadata.OptionSet.MetadataId.Value))
                            {
                                this.AllOptionSetMetadata.TryAdd(picklistAttributeMetadata.OptionSet.MetadataId.Value, picklistAttributeMetadata.OptionSet);
                            }
                        }
                    }
                }
            }

            if (metaEntity.OneToManyRelationships != null)
            {
                foreach (var metaRelationship in metaEntity.OneToManyRelationships)
                {
                    lock (_syncObjectRelationshipMetadata)
                    {
                        if (!_dictRelashionship.ContainsKey(metaRelationship.MetadataId.Value))
                        {
                            _dictRelashionship.TryAdd(metaRelationship.MetadataId.Value, metaRelationship);
                        }
                    }
                }
            }

            if (metaEntity.ManyToOneRelationships != null)
            {
                foreach (var metaRelationship in metaEntity.ManyToOneRelationships)
                {
                    lock (_syncObjectRelationshipMetadata)
                    {
                        if (!_dictRelashionship.ContainsKey(metaRelationship.MetadataId.Value))
                        {
                            _dictRelashionship.TryAdd(metaRelationship.MetadataId.Value, metaRelationship);
                        }
                    }
                }
            }

            if (metaEntity.ManyToManyRelationships != null)
            {
                foreach (var metaRelationship in metaEntity.ManyToManyRelationships)
                {
                    lock (_syncObjectRelationshipMetadata)
                    {
                        if (!_dictRelashionship.ContainsKey(metaRelationship.MetadataId.Value))
                        {
                            _dictRelashionship.TryAdd(metaRelationship.MetadataId.Value, metaRelationship);
                        }
                    }
                }
            }

            if (metaEntity.Keys != null)
            {
                foreach (var metaKey in metaEntity.Keys)
                {
                    lock (_syncObjectEntityKeyMetadata)
                    {
                        if (!_dictKey.ContainsKey(metaKey.MetadataId.Value))
                        {
                            _dictKey.TryAdd(metaKey.MetadataId.Value, metaKey);
                        }
                    }
                }
            }
        }

        #region Методы получения метаданных.

        public EntityMetadata GetEntityMetadata(Guid idMetadata)
        {
            lock (_syncObjectEntityMetadata)
            {
                if (_dictEntity.ContainsKey(idMetadata))
                {
                    return _dictEntity[idMetadata];
                }
            }

            try
            {
                if (this._allMetadataDownloaded)
                {
                    return null;
                }

                var request = new RetrieveEntityRequest()
                {
                    EntityFilters = EntityFilters.All,
                    MetadataId = idMetadata,
                };

                var response = (RetrieveEntityResponse)Service.Execute(request);

                var metaEntity = response.EntityMetadata;

                HandleEntityMetadata(metaEntity);

                lock (_syncObjectEntityMetadata)
                {
                    return _dictEntity[metaEntity.MetadataId.Value];
                }
            }
            catch (Exception ex)
            {
                DTEHelper.Singleton.WriteToOutput("GetEntityMetadata(Guid {0})", idMetadata);
                DTEHelper.Singleton.WriteErrorToOutput(ex);

                return null;
            }
        }

        public List<EntityMetadata> GetEntityMetadataList(IEnumerable<Guid> idMetadataColl)
        {
            List<EntityMetadata> result = new List<EntityMetadata>();

            foreach (var idMetadata in idMetadataColl.Distinct())
            {
                lock (_syncObjectEntityMetadata)
                {
                    if (_dictEntity.ContainsKey(idMetadata))
                    {
                        result.Add(_dictEntity[idMetadata]);
                    }
                }

                try
                {
                    if (this._allMetadataDownloaded)
                    {
                        return null;
                    }

                    var request = new RetrieveEntityRequest()
                    {
                        EntityFilters = EntityFilters.All,
                        MetadataId = idMetadata,
                    };

                    var response = (RetrieveEntityResponse)Service.Execute(request);

                    var metaEntity = response.EntityMetadata;

                    HandleEntityMetadata(metaEntity);

                    lock (_syncObjectEntityMetadata)
                    {
                        result.Add(_dictEntity[metaEntity.MetadataId.Value]);
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.Singleton.WriteToOutput("GetEntityMetadataList(Guid {0})", idMetadata);
                    DTEHelper.Singleton.WriteErrorToOutput(ex);
                }
            }

            return result;
        }

        public EntityMetadata GetEntityMetadata(string entityName)
        {
            EntityMetadata metadata = null;

            lock (_syncObjectEntityMetadata)
            {
                metadata = _dictEntity.Values.FirstOrDefault(e => string.Equals(entityName, e.LogicalName, StringComparison.OrdinalIgnoreCase));
            }

            if (metadata != null)
            {
                return metadata;
            }

            try
            {
                if (this._allMetadataDownloaded)
                {
                    return null;
                }

                var request = new RetrieveEntityRequest()
                {
                    EntityFilters = EntityFilters.All,
                    LogicalName = entityName.ToLower(),
                };

                var response = (RetrieveEntityResponse)Service.Execute(request);

                var metaEntity = response.EntityMetadata;

                HandleEntityMetadata(metaEntity);

                lock (_syncObjectEntityMetadata)
                {
                    return _dictEntity[metaEntity.MetadataId.Value];
                }
            }
            catch (Exception ex)
            {
                DTEHelper.Singleton.WriteToOutput("GetEntityMetadata(string {0})", entityName);
                DTEHelper.Singleton.WriteErrorToOutput(ex);

                return null;
            }
        }

        public EntityMetadata GetEntityMetadata(string entityName, string[] columnNames)
        {
            EntityMetadata metadata = null;

            lock (_syncObjectEntityMetadata)
            {
                metadata = _dictEntity.Values.FirstOrDefault(e => string.Equals(entityName, e.LogicalName, StringComparison.OrdinalIgnoreCase));
            }

            if (metadata != null)
            {
                return metadata;
            }

            try
            {
                if (this._allMetadataDownloaded)
                {
                    return null;
                }

                MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
                entityFilter.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, entityName));

                var entityQueryExpression = new EntityQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression(columnNames),
                    LabelQuery = new LabelQueryExpression(),

                    Criteria = entityFilter,
                };

                var response = (RetrieveMetadataChangesResponse)Service.Execute(
                    new RetrieveMetadataChangesRequest()
                    {
                        ClientVersionStamp = null,
                        Query = entityQueryExpression,
                    }
                );

                foreach (var metaEntity in response.EntityMetadata)
                {
                    HandleEntityMetadata(metaEntity);
                }

                lock (_syncObjectEntityMetadata)
                {
                    return _dictEntity.Values.FirstOrDefault(e => string.Equals(entityName, e.LogicalName, StringComparison.OrdinalIgnoreCase));
                }
            }
            catch (Exception ex)
            {
                DTEHelper.Singleton.WriteToOutput("GetEntityMetadata(string {0}, string[] {1})", entityName, string.Join(",", columnNames));
                DTEHelper.Singleton.WriteErrorToOutput(ex);

                return null;
            }
        }

        public void StoreEntityMetadata(EntityMetadata entityMetadata)
        {
            HandleEntityMetadata(entityMetadata);
        }

        public OptionSetMetadata GetOptionSetMetadata(string name)
        {
            return this.AllOptionSetMetadata?.Values.OfType<OptionSetMetadata>().FirstOrDefault(o => string.Equals(name, o.Name, StringComparison.OrdinalIgnoreCase));
        }

        public AttributeMetadata GetAttributeMetadata(Guid idAttribute)
        {
            lock (_syncObjectAttributeMetadata)
            {
                if (_dictAttribute.ContainsKey(idAttribute))
                {
                    return _dictAttribute[idAttribute];
                }
            }

            try
            {
                if (this._allMetadataDownloaded)
                {
                    return null;
                }

                var request = new RetrieveAttributeRequest()
                {
                    MetadataId = idAttribute,
                };

                var response = (RetrieveAttributeResponse)Service.Execute(request);

                var metaAttribute = response.AttributeMetadata;

                lock (_syncObjectAttributeMetadata)
                {
                    if (!_dictAttribute.ContainsKey(metaAttribute.MetadataId.Value))
                    {
                        _dictAttribute.TryAdd(metaAttribute.MetadataId.Value, metaAttribute);
                    }
                }

                lock (_syncObjectAttributeMetadata)
                {
                    return _dictAttribute[metaAttribute.MetadataId.Value];
                }
            }
            catch (Exception ex)
            {
                DTEHelper.Singleton.WriteToOutput("GetAttributeMetadata(Guid {0})", idAttribute);
                DTEHelper.Singleton.WriteErrorToOutput(ex);

                return null;
            }
        }

        public AttributeMetadata GetAttributeMetadata(string entityName, string attributeName)
        {
            AttributeMetadata result = null;

            lock (_syncObjectAttributeMetadata)
            {
                result = _dictAttribute.Values.FirstOrDefault(a => string.Equals(entityName, a.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase)
                                                                    && string.Equals(attributeName, a.LogicalName, StringComparison.InvariantCultureIgnoreCase));
            }

            if (result != null)
            {
                return result;
            }

            try
            {
                if (this._allMetadataDownloaded)
                {
                    return null;
                }

                var request = new RetrieveAttributeRequest()
                {
                    EntityLogicalName = entityName,
                    LogicalName = attributeName,
                };

                var response = (RetrieveAttributeResponse)Service.Execute(request);

                var metaAttribute = response.AttributeMetadata;

                lock (_syncObjectAttributeMetadata)
                {
                    if (!_dictAttribute.ContainsKey(metaAttribute.MetadataId.Value))
                    {
                        _dictAttribute.TryAdd(metaAttribute.MetadataId.Value, metaAttribute);
                    }
                }

                if (metaAttribute is StatusAttributeMetadata statusAttributeMetadata
                            && statusAttributeMetadata.OptionSet != null
                        )
                {
                    lock (_syncObjectOptionSets)
                    {
                        if (!this.AllOptionSetMetadata.ContainsKey(statusAttributeMetadata.OptionSet.MetadataId.Value))
                        {
                            this.AllOptionSetMetadata.TryAdd(statusAttributeMetadata.OptionSet.MetadataId.Value, statusAttributeMetadata.OptionSet);
                        }
                    }
                }

                if (metaAttribute is StateAttributeMetadata stateAttributeMetadata
                        && stateAttributeMetadata.OptionSet != null
                    )
                {
                    lock (_syncObjectOptionSets)
                    {
                        if (!this.AllOptionSetMetadata.ContainsKey(stateAttributeMetadata.OptionSet.MetadataId.Value))
                        {
                            this.AllOptionSetMetadata.TryAdd(stateAttributeMetadata.OptionSet.MetadataId.Value, stateAttributeMetadata.OptionSet);
                        }
                    }
                }

                if (metaAttribute is BooleanAttributeMetadata booleanAttributeMetadata
                        && booleanAttributeMetadata.OptionSet != null
                    )
                {
                    lock (_syncObjectOptionSets)
                    {
                        if (!this.AllOptionSetMetadata.ContainsKey(booleanAttributeMetadata.OptionSet.MetadataId.Value))
                        {
                            this.AllOptionSetMetadata.TryAdd(booleanAttributeMetadata.OptionSet.MetadataId.Value, booleanAttributeMetadata.OptionSet);
                        }
                    }
                }

                if (metaAttribute is PicklistAttributeMetadata picklistAttributeMetadata
                        && picklistAttributeMetadata.OptionSet != null
                    )
                {
                    lock (_syncObjectOptionSets)
                    {
                        if (!this.AllOptionSetMetadata.ContainsKey(picklistAttributeMetadata.OptionSet.MetadataId.Value))
                        {
                            this.AllOptionSetMetadata.TryAdd(picklistAttributeMetadata.OptionSet.MetadataId.Value, picklistAttributeMetadata.OptionSet);
                        }
                    }
                }

                lock (_syncObjectAttributeMetadata)
                {
                    return _dictAttribute[metaAttribute.MetadataId.Value];
                }
            }
            catch (Exception ex)
            {
                DTEHelper.Singleton.WriteToOutput("GetAttributeMetadata(string {0}, string {1})", entityName, attributeName);
                DTEHelper.Singleton.WriteErrorToOutput(ex);

                return null;
            }
        }

        public EntityKeyMetadata GetEntityKeyMetadata(Guid idKey)
        {
            lock (_syncObjectEntityKeyMetadata)
            {
                if (_dictKey.ContainsKey(idKey))
                {
                    return _dictKey[idKey];
                }
            }

            try
            {
                if (this._allMetadataDownloaded)
                {
                    return null;
                }

                var request = new RetrieveEntityKeyRequest()
                {
                    MetadataId = idKey,
                };

                var response = (RetrieveEntityKeyResponse)Service.Execute(request);

                var metaKey = response.EntityKeyMetadata;

                lock (_syncObjectEntityKeyMetadata)
                {
                    if (!_dictKey.ContainsKey(metaKey.MetadataId.Value))
                    {
                        _dictKey.TryAdd(metaKey.MetadataId.Value, metaKey);
                    }
                }

                lock (_syncObjectEntityKeyMetadata)
                {
                    return _dictKey[metaKey.MetadataId.Value];
                }
            }
            catch (Exception ex)
            {
                DTEHelper.Singleton.WriteToOutput("GetEntityKeyMetadata(Guid {0})", idKey);
                DTEHelper.Singleton.WriteErrorToOutput(ex);

                return null;
            }
        }

        public RelationshipMetadataBase GetRelationshipMetadata(Guid idRelation)
        {
            lock (_syncObjectRelationshipMetadata)
            {
                if (_dictRelashionship.ContainsKey(idRelation))
                {
                    return _dictRelashionship[idRelation];
                }
            }

            try
            {
                if (this._allMetadataDownloaded)
                {
                    return null;
                }

                var request = new RetrieveRelationshipRequest()
                {
                    MetadataId = idRelation,
                };

                var response = (RetrieveRelationshipResponse)Service.Execute(request);

                var metaRelation = response.RelationshipMetadata;

                lock (_syncObjectRelationshipMetadata)
                {
                    if (!_dictRelashionship.ContainsKey(metaRelation.MetadataId.Value))
                    {
                        _dictRelashionship.TryAdd(metaRelation.MetadataId.Value, metaRelation);
                    }
                }

                lock (_syncObjectRelationshipMetadata)
                {
                    return _dictRelashionship[metaRelation.MetadataId.Value];
                }
            }
            catch (Exception ex)
            {
                DTEHelper.Singleton.WriteToOutput("GetRelationshipMetadata(Guid {0})", idRelation);
                DTEHelper.Singleton.WriteErrorToOutput(ex);

                return null;
            }
        }

        #endregion Методы получения метаданных.
    }
}
