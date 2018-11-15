using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        private readonly object _syncObjectOptionSets = new object();

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
            if (!_dictEntity.ContainsKey(metaEntity.MetadataId.Value))
            {
                _dictEntity.TryAdd(metaEntity.MetadataId.Value, metaEntity);
            }


            if (metaEntity.Attributes != null)
            {
                foreach (var metaAttribute in metaEntity.Attributes)
                {
                    if (!_dictAttribute.ContainsKey(metaAttribute.MetadataId.Value))
                    {
                        _dictAttribute.TryAdd(metaAttribute.MetadataId.Value, metaAttribute);
                    }

                    if (metaAttribute is StatusAttributeMetadata statusAttributeMetadata
                            && statusAttributeMetadata.OptionSet != null
                        )
                    {
                        if (!this.AllOptionSetMetadata.ContainsKey(statusAttributeMetadata.OptionSet.MetadataId.Value))
                        {
                            this.AllOptionSetMetadata.TryAdd(statusAttributeMetadata.OptionSet.MetadataId.Value, statusAttributeMetadata.OptionSet);
                        }
                    }

                    if (metaAttribute is StateAttributeMetadata stateAttributeMetadata
                            && stateAttributeMetadata.OptionSet != null
                        )
                    {
                        if (!this.AllOptionSetMetadata.ContainsKey(stateAttributeMetadata.OptionSet.MetadataId.Value))
                        {
                            this.AllOptionSetMetadata.TryAdd(stateAttributeMetadata.OptionSet.MetadataId.Value, stateAttributeMetadata.OptionSet);
                        }
                    }

                    if (metaAttribute is BooleanAttributeMetadata booleanAttributeMetadata
                            && booleanAttributeMetadata.OptionSet != null
                        )
                    {
                        if (!this.AllOptionSetMetadata.ContainsKey(booleanAttributeMetadata.OptionSet.MetadataId.Value))
                        {
                            this.AllOptionSetMetadata.TryAdd(booleanAttributeMetadata.OptionSet.MetadataId.Value, booleanAttributeMetadata.OptionSet);
                        }
                    }

                    if (metaAttribute is PicklistAttributeMetadata picklistAttributeMetadata
                            && picklistAttributeMetadata.OptionSet != null
                        )
                    {
                        if (!this.AllOptionSetMetadata.ContainsKey(picklistAttributeMetadata.OptionSet.MetadataId.Value))
                        {
                            this.AllOptionSetMetadata.TryAdd(picklistAttributeMetadata.OptionSet.MetadataId.Value, picklistAttributeMetadata.OptionSet);
                        }
                    }
                }
            }

            if (metaEntity.OneToManyRelationships != null)
            {
                foreach (var metaRelationship in metaEntity.OneToManyRelationships)
                {
                    if (!_dictRelashionship.ContainsKey(metaRelationship.MetadataId.Value))
                    {
                        _dictRelashionship.TryAdd(metaRelationship.MetadataId.Value, metaRelationship);
                    }
                }
            }

            if (metaEntity.ManyToOneRelationships != null)
            {
                foreach (var metaRelationship in metaEntity.ManyToOneRelationships)
                {
                    if (!_dictRelashionship.ContainsKey(metaRelationship.MetadataId.Value))
                    {
                        _dictRelashionship.TryAdd(metaRelationship.MetadataId.Value, metaRelationship);
                    }
                }
            }

            if (metaEntity.ManyToManyRelationships != null)
            {
                foreach (var metaRelationship in metaEntity.ManyToManyRelationships)
                {
                    if (!_dictRelashionship.ContainsKey(metaRelationship.MetadataId.Value))
                    {
                        _dictRelashionship.TryAdd(metaRelationship.MetadataId.Value, metaRelationship);
                    }
                }
            }

            if (metaEntity.Keys != null)
            {
                foreach (var metaKey in metaEntity.Keys)
                {
                    if (!_dictKey.ContainsKey(metaKey.MetadataId.Value))
                    {
                        _dictKey.TryAdd(metaKey.MetadataId.Value, metaKey);
                    }
                }
            }
        }

        #region Методы получения метаданных.

        public EntityMetadata GetEntityMetadata(Guid idMetadata)
        {
            if (_dictEntity.ContainsKey(idMetadata))
            {
                return _dictEntity[idMetadata];
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

                return _dictEntity[metaEntity.MetadataId.Value];
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetEntityMetadataByGuidFormat1, idMetadata);

                return null;
            }
        }

        public List<EntityMetadata> GetEntityMetadataList(IEnumerable<Guid> idMetadataColl)
        {
            List<EntityMetadata> result = new List<EntityMetadata>();

            foreach (var idMetadata in idMetadataColl.Distinct())
            {
                if (_dictEntity.ContainsKey(idMetadata))
                {
                    result.Add(_dictEntity[idMetadata]);
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

                    result.Add(_dictEntity[metaEntity.MetadataId.Value]);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetEntityMetadataByGuidFormat1, idMetadata);
                }
            }

            return result;
        }

        public EntityMetadata GetEntityMetadata(string entityName)
        {
            EntityMetadata metadata = null;

            metadata = _dictEntity.Values.FirstOrDefault(e => string.Equals(entityName, e.LogicalName, StringComparison.OrdinalIgnoreCase));

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

                return _dictEntity[metaEntity.MetadataId.Value];
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetEntityMetadataByStringFormat1, entityName);

                return null;
            }
        }

        public EntityMetadata GetEntityMetadata(string entityName, string[] columnNames)
        {
            EntityMetadata metadata = null;

            metadata = _dictEntity.Values.FirstOrDefault(e => string.Equals(entityName, e.LogicalName, StringComparison.OrdinalIgnoreCase));

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

                return _dictEntity.Values.FirstOrDefault(e => string.Equals(entityName, e.LogicalName, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetEntityMetadataByStringStringFormat2, entityName, string.Join(",", columnNames));

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
            if (_dictAttribute.ContainsKey(idAttribute))
            {
                return _dictAttribute[idAttribute];
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

                if (!_dictAttribute.ContainsKey(metaAttribute.MetadataId.Value))
                {
                    _dictAttribute.TryAdd(metaAttribute.MetadataId.Value, metaAttribute);
                }

                return _dictAttribute[metaAttribute.MetadataId.Value];
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetAttributeMetadataByGuidFormat1, idAttribute);

                return null;
            }
        }

        public AttributeMetadata GetAttributeMetadata(string entityName, string attributeName)
        {
            AttributeMetadata result = null;

            result = _dictAttribute.Values.FirstOrDefault(a => string.Equals(entityName, a.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase)
                                                                && string.Equals(attributeName, a.LogicalName, StringComparison.InvariantCultureIgnoreCase));

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

                if (!_dictAttribute.ContainsKey(metaAttribute.MetadataId.Value))
                {
                    _dictAttribute.TryAdd(metaAttribute.MetadataId.Value, metaAttribute);
                }

                if (metaAttribute is StatusAttributeMetadata statusAttributeMetadata
                            && statusAttributeMetadata.OptionSet != null
                        )
                {
                    if (!this.AllOptionSetMetadata.ContainsKey(statusAttributeMetadata.OptionSet.MetadataId.Value))
                    {
                        this.AllOptionSetMetadata.TryAdd(statusAttributeMetadata.OptionSet.MetadataId.Value, statusAttributeMetadata.OptionSet);
                    }
                }

                if (metaAttribute is StateAttributeMetadata stateAttributeMetadata
                        && stateAttributeMetadata.OptionSet != null
                    )
                {
                    if (!this.AllOptionSetMetadata.ContainsKey(stateAttributeMetadata.OptionSet.MetadataId.Value))
                    {
                        this.AllOptionSetMetadata.TryAdd(stateAttributeMetadata.OptionSet.MetadataId.Value, stateAttributeMetadata.OptionSet);
                    }
                }

                if (metaAttribute is BooleanAttributeMetadata booleanAttributeMetadata
                        && booleanAttributeMetadata.OptionSet != null
                    )
                {
                    if (!this.AllOptionSetMetadata.ContainsKey(booleanAttributeMetadata.OptionSet.MetadataId.Value))
                    {
                        this.AllOptionSetMetadata.TryAdd(booleanAttributeMetadata.OptionSet.MetadataId.Value, booleanAttributeMetadata.OptionSet);
                    }
                }

                if (metaAttribute is PicklistAttributeMetadata picklistAttributeMetadata
                        && picklistAttributeMetadata.OptionSet != null
                    )
                {
                    if (!this.AllOptionSetMetadata.ContainsKey(picklistAttributeMetadata.OptionSet.MetadataId.Value))
                    {
                        this.AllOptionSetMetadata.TryAdd(picklistAttributeMetadata.OptionSet.MetadataId.Value, picklistAttributeMetadata.OptionSet);
                    }
                }

                return _dictAttribute[metaAttribute.MetadataId.Value];
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetAttributeMetadataByStringStringFormat2, entityName, attributeName);

                return null;
            }
        }

        public EntityKeyMetadata GetEntityKeyMetadata(Guid idKey)
        {
            if (_dictKey.ContainsKey(idKey))
            {
                return _dictKey[idKey];
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

                if (!_dictKey.ContainsKey(metaKey.MetadataId.Value))
                {
                    _dictKey.TryAdd(metaKey.MetadataId.Value, metaKey);
                }

                return _dictKey[metaKey.MetadataId.Value];
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetEntityKeyMetadataByGuidFormat1, idKey);

                return null;
            }
        }

        public EntityKeyMetadata GetEntityKeyMetadata(string entityName, string keyName)
        {
            EntityKeyMetadata result = null;

            result = _dictKey.Values.FirstOrDefault(a => string.Equals(entityName, a.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase)
                                                                && string.Equals(keyName, a.LogicalName, StringComparison.InvariantCultureIgnoreCase));

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

                var request = new RetrieveEntityKeyRequest()
                {
                    EntityLogicalName = entityName,
                    LogicalName = keyName,
                };

                var response = (RetrieveEntityKeyResponse)Service.Execute(request);

                var metaKey = response.EntityKeyMetadata;

                if (!_dictKey.ContainsKey(metaKey.MetadataId.Value))
                {
                    _dictKey.TryAdd(metaKey.MetadataId.Value, metaKey);
                }

                return _dictKey[metaKey.MetadataId.Value];
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetEntityKeyMetadataByStringStringFormat2, entityName, keyName);

                return null;
            }
        }

        public RelationshipMetadataBase GetRelationshipMetadata(Guid idRelation)
        {
            if (_dictRelashionship.ContainsKey(idRelation))
            {
                return _dictRelashionship[idRelation];
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

                if (!_dictRelashionship.ContainsKey(metaRelation.MetadataId.Value))
                {
                    _dictRelashionship.TryAdd(metaRelation.MetadataId.Value, metaRelation);
                }

                return _dictRelashionship[metaRelation.MetadataId.Value];
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetRelationshipMetadataByGuidFormat1, idRelation);

                return null;
            }
        }

        public RelationshipMetadataBase GetRelationshipMetadata(string relationName)
        {
            RelationshipMetadataBase result = null;

            result = _dictRelashionship.Values.FirstOrDefault(a => string.Equals(relationName, a.SchemaName, StringComparison.InvariantCultureIgnoreCase));

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

                var request = new RetrieveRelationshipRequest()
                {
                    Name = relationName,
                };

                var response = (RetrieveRelationshipResponse)Service.Execute(request);

                var metaRelation = response.RelationshipMetadata;

                if (!_dictRelashionship.ContainsKey(metaRelation.MetadataId.Value))
                {
                    _dictRelashionship.TryAdd(metaRelation.MetadataId.Value, metaRelation);
                }

                return _dictRelashionship[metaRelation.MetadataId.Value];
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex, Properties.OutputStrings.GetRelationshipMetadataByStringFormat1, relationName);

                return null;
            }
        }

        #endregion Методы получения метаданных.
    }
}