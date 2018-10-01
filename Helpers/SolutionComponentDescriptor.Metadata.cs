using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private readonly object _syncObjectEntityCache = new object();

        private readonly object _syncObjectOptionSets = new object();

        private readonly object _syncObjectEntityMetadata = new object();

        private readonly object _syncObjectAttributeMetadata = new object();

        private readonly object _syncObjectEntityKeyMetadata = new object();

        private readonly object _syncObjectRelationshipMetadata = new object();

        private readonly object _syncObjectManagedPropertyMetadata = new object();

        private ConcurrentDictionary<Guid, EntityMetadata> _dictEntity = new ConcurrentDictionary<Guid, EntityMetadata>();

        private ConcurrentDictionary<Guid, AttributeMetadata> _dictAttribute = new ConcurrentDictionary<Guid, AttributeMetadata>();

        private ConcurrentDictionary<Guid, EntityKeyMetadata> _dictKey = new ConcurrentDictionary<Guid, EntityKeyMetadata>();

        private ConcurrentDictionary<Guid, RelationshipMetadataBase> _dictRelashionship = new ConcurrentDictionary<Guid, RelationshipMetadataBase>();

        private ConcurrentDictionary<Guid, ManagedPropertyMetadata> _allManagedProperties;

        private ConcurrentDictionary<Guid, OptionSetMetadataBase> _allOptionSetMetadata;

        private bool _allMetadataDownloaded = false;

        private ConcurrentDictionary<Guid, OptionSetMetadataBase> AllOptionSetMetadata
        {
            get
            {
                lock (_syncObjectOptionSets)
                {
                    if (_allOptionSetMetadata == null)
                    {
                        var request = new RetrieveAllOptionSetsRequest();
                        var response = (RetrieveAllOptionSetsResponse)this._service.Execute(request);

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

        private ConcurrentDictionary<Guid, ManagedPropertyMetadata> AllManagedProperties
        {
            get
            {
                lock (_syncObjectManagedPropertyMetadata)
                {
                    if (_allManagedProperties == null)
                    {
                        var request = new RetrieveAllManagedPropertiesRequest();
                        var response = (RetrieveAllManagedPropertiesResponse)this._service.Execute(request);

                        _allManagedProperties = new ConcurrentDictionary<Guid, ManagedPropertyMetadata>();

                        foreach (var item in response.ManagedPropertyMetadata)
                        {
                            if (!_allManagedProperties.ContainsKey(item.MetadataId.Value))
                            {
                                _allManagedProperties.TryAdd(item.MetadataId.Value, item);
                            }
                        }
                    }
                }

                return _allManagedProperties;
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

            var isEntityKeyExists = _service.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
            }

            var response = (RetrieveMetadataChangesResponse)_service.Execute(
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

            var isEntityKeyExists = _service.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
            }

            var response = (RetrieveMetadataChangesResponse)_service.Execute(
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

            var response = (RetrieveMetadataChangesResponse)_service.Execute(
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

                var response = (RetrieveEntityResponse)_service.Execute(request);

                var metaEntity = response.EntityMetadata;

                HandleEntityMetadata(metaEntity);

                lock (_syncObjectEntityMetadata)
                {
                    return _dictEntity[metaEntity.MetadataId.Value];
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

                    var response = (RetrieveEntityResponse)_service.Execute(request);

                    var metaEntity = response.EntityMetadata;

                    HandleEntityMetadata(metaEntity);

                    lock (_syncObjectEntityMetadata)
                    {
                        result.Add(_dictEntity[metaEntity.MetadataId.Value]);
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
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

                var response = (RetrieveEntityResponse)_service.Execute(request);

                var metaEntity = response.EntityMetadata;

                HandleEntityMetadata(metaEntity);

                lock (_syncObjectEntityMetadata)
                {
                    return _dictEntity[metaEntity.MetadataId.Value];
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

                var response = (RetrieveMetadataChangesResponse)_service.Execute(
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
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

                var response = (RetrieveAttributeResponse)_service.Execute(request);

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
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

                var response = (RetrieveAttributeResponse)_service.Execute(request);

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
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

                var response = (RetrieveEntityKeyResponse)_service.Execute(request);

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
                this._iWriteToOutput.WriteErrorToOutput(ex);

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

                var response = (RetrieveRelationshipResponse)_service.Execute(request);

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
                this._iWriteToOutput.WriteErrorToOutput(ex);

                return null;
            }
        }

        #endregion Методы получения метаданных.

        private void GenerateDescriptionEntity(StringBuilder builder, List<SolutionComponent> components)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("EntityName", "IsManaged", "Behaviour", "Url");

            foreach (var comp in components)
            {
                string behaviorName = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behaviorName = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                EntityMetadata metaData = GetEntityMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    string logicalName = metaData.LogicalName;

                    handler.AddLine(logicalName, metaData.IsManaged.ToString(), behaviorName, _withUrls ? _service.ConnectionData?.GetEntityMetadataUrl(metaData.MetadataId.Value) : string.Empty);
                }
                else
                {
                    handler.AddLine(comp.ObjectId.ToString(), string.Empty, behaviorName);
                }
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private void FillEntityComponent(List<SolutionImageComponent> result, IEnumerable<SolutionComponent> components)
        {
            foreach (var comp in components)
            {
                EntityMetadata metaData = GetEntityMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.Entity,
                        SchemaName = metaData.LogicalName,
                        RootComponentBehavior = comp.RootComponentBehavior?.Value,
                    });
                }
            }
        }

        private string GenerateDescriptionEntitySingle(SolutionComponent component)
        {
            EntityMetadata metaData = GetEntityMetadata(component.ObjectId.Value);

            if (metaData != null)
            {
                return string.Format("Entity {0}    IsManaged {1}{2}"
                    , metaData.LogicalName
                    , metaData.IsManaged.ToString()
                    , _withUrls ? string.Format("    Url {0}", _service.ConnectionData.GetEntityMetadataUrl(metaData.MetadataId.Value)) : string.Empty
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionAttribute(StringBuilder builder, List<SolutionComponent> components)
        {
            FormatTextTableHandler handlerUnknowed = new FormatTextTableHandler();

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("AttributeName", "IsManaged", "Behaviour", "Url");

            foreach (var comp in components)
            {
                string behavior = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                AttributeMetadata metaData = GetAttributeMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    var entityMetadata = GetEntityMetadata(metaData.EntityLogicalName);

                    if (entityMetadata != null)
                    {
                        string name = string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName);

                        handler.AddLine(name, metaData.IsManaged.ToString(), behavior, _withUrls ? _service.ConnectionData?.GetAttributeMetadataUrl(entityMetadata.MetadataId.Value, metaData.MetadataId.Value) : string.Empty);

                        continue;
                    }
                }

                handlerUnknowed.AddLine(comp.ObjectId.ToString(), string.Empty, behavior);
            }

            if (handlerUnknowed.Count > 0)
            {
                List<string> lines = handlerUnknowed.GetFormatedLines(true);
                builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }

            {
                List<string> lines = handler.GetFormatedLines(true);

                lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }
        }

        private void FillAttributeComponent(List<SolutionImageComponent> result, IEnumerable<SolutionComponent> components)
        {
            foreach (var comp in components)
            {
                AttributeMetadata metaData = GetAttributeMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.Attribute,
                        SchemaName = metaData.LogicalName,
                        ParentSchemaName = metaData.EntityLogicalName,
                        RootComponentBehavior = comp.RootComponentBehavior?.Value,
                    });
                }
            }
        }

        private string GenerateDescriptionAttributeSingle(SolutionComponent component)
        {
            AttributeMetadata metaData = GetAttributeMetadata(component.ObjectId.Value);

            if (metaData != null)
            {
                var entityMetadata = GetEntityMetadata(metaData.EntityLogicalName);

                if (entityMetadata != null)
                {
                    string name = string.Format("Attribute {0}.{1}     IsManaged {2}{3}"
                        , metaData.EntityLogicalName
                        , metaData.LogicalName
                        , metaData.IsManaged.ToString()
                        , _withUrls ? string.Format("     Url {0}", _service.ConnectionData.GetAttributeMetadataUrl(entityMetadata.MetadataId.Value, metaData.MetadataId.Value)) : string.Empty
                    );

                    return name;
                }
            }

            return component.ToString();
        }

        private void GenerateDescriptionEntityRelationship(StringBuilder builder, List<SolutionComponent> components)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();

            FormatTextTableHandler handlerManyToOne = new FormatTextTableHandler();
            FormatTextTableHandler handlerManyToMany = new FormatTextTableHandler();

            foreach (var comp in components)
            {
                RelationshipMetadataBase metaData = GetRelationshipMetadata(comp.ObjectId.GetValueOrDefault());

                string behavior = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behavior = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                if (metaData != null)
                {
                    if (metaData is OneToManyRelationshipMetadata)
                    {
                        var relationship = metaData as OneToManyRelationshipMetadata;

                        string relName = string.Format("{0}.{1}", relationship.ReferencingEntity, relationship.ReferencingAttribute);
                        string refEntity = relationship.ReferencedEntity;

                        string url = null;

                        if (_withUrls)
                        {
                            var entityMetadata = GetEntityMetadata(refEntity);

                            if (entityMetadata != null)
                            {
                                url = _service.ConnectionData?.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value);
                            }
                        }

                        handlerManyToOne.AddLine(relName
                            , "Many to One"
                            , refEntity
                            , relationship.SchemaName
                            , behavior
                            , relationship.IsManaged.ToString()
                            , url
                            );

                        continue;
                    }
                    else if (metaData is ManyToManyRelationshipMetadata)
                    {
                        var relationship = metaData as ManyToManyRelationshipMetadata;

                        string relName = string.Format("{0} - {1}", relationship.Entity1LogicalName, relationship.Entity2LogicalName);

                        string refEntity = relationship.SchemaName;

                        string url = null;

                        if (_withUrls)
                        {
                            var entityMetadata = GetEntityMetadata(refEntity);

                            if (entityMetadata != null)
                            {
                                url = _service.ConnectionData?.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value);
                            }
                        }

                        handlerManyToMany.AddLine(relName
                            , "Many to Many"
                            , refEntity
                            , behavior
                            , relationship.IsManaged.ToString()
                            , url
                        );

                        continue;
                    }
                }

                handler.AddLine(comp.ObjectId.ToString(), string.Empty, behavior);
            }

            List<string> linesUnknowed = handler.GetFormatedLines(true);

            List<string> linesOne = handlerManyToOne.GetFormatedLines(true);

            List<string> linesMany = handlerManyToMany.GetFormatedLines(true);

            if (linesUnknowed.Any())
            {
                builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                linesUnknowed.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }

            linesOne.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            linesMany.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private void FillEntityRelationshipComponent(List<SolutionImageComponent> result, IEnumerable<SolutionComponent> components)
        {
            foreach (var comp in components)
            {
                RelationshipMetadataBase metaData = GetRelationshipMetadata(comp.ObjectId.GetValueOrDefault());

                if (metaData != null)
                {
                    if (metaData is OneToManyRelationshipMetadata)
                    {
                        var relationship = metaData as OneToManyRelationshipMetadata;

                        result.Add(new SolutionImageComponent()
                        {
                            ComponentType = (int)ComponentType.EntityRelationship,
                            SchemaName = relationship.SchemaName,
                            ParentSchemaName = relationship.ReferencingEntity,
                            RootComponentBehavior = comp.RootComponentBehavior?.Value,
                        });

                        result.Add(new SolutionImageComponent()
                        {
                            ComponentType = (int)ComponentType.EntityRelationship,
                            SchemaName = relationship.SchemaName,
                            ParentSchemaName = relationship.ReferencedEntity,
                            RootComponentBehavior = comp.RootComponentBehavior?.Value,
                        });
                    }
                    else if (metaData is ManyToManyRelationshipMetadata)
                    {
                        var relationship = metaData as ManyToManyRelationshipMetadata;

                        result.Add(new SolutionImageComponent()
                        {
                            ComponentType = (int)ComponentType.EntityRelationship,
                            SchemaName = relationship.SchemaName,
                            ParentSchemaName = relationship.Entity1LogicalName,
                            RootComponentBehavior = comp.RootComponentBehavior?.Value,
                        });

                        result.Add(new SolutionImageComponent()
                        {
                            ComponentType = (int)ComponentType.EntityRelationship,
                            SchemaName = relationship.SchemaName,
                            ParentSchemaName = relationship.Entity2LogicalName,
                            RootComponentBehavior = comp.RootComponentBehavior?.Value,
                        });
                    }
                }
            }
        }

        private string GenerateDescriptionEntityRelationshipSingle(SolutionComponent component)
        {
            RelationshipMetadataBase metaData = GetRelationshipMetadata(component.ObjectId.Value);

            if (metaData != null)
            {
                if (metaData is OneToManyRelationshipMetadata)
                {
                    var relationship = metaData as OneToManyRelationshipMetadata;

                    string relName = string.Format("{0}.{1}", relationship.ReferencingEntity, relationship.ReferencingAttribute);
                    string refEntity = relationship.ReferencedEntity;

                    var entityMetadata = GetEntityMetadata(refEntity);

                    return string.Format("EntityRelationship {0} - {1} - {2} - {3} - {4}{5}"
                        , relName
                        , "Many to One"
                        , refEntity
                        , relationship.SchemaName
                        , relationship.IsManaged.ToString()
                        , _withUrls ? string.Format("    Url {0}", _service.ConnectionData.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value)) : string.Empty
                        );
                }
                else if (metaData is ManyToManyRelationshipMetadata)
                {
                    var relationship = metaData as ManyToManyRelationshipMetadata;

                    string relName = string.Format("{0} - {1}", relationship.Entity1LogicalName, relationship.Entity2LogicalName);

                    string refEntity = relationship.SchemaName;

                    var entityMetadata = GetEntityMetadata(refEntity);

                    return string.Format("EntityRelationship {0} - {1} - {2} - {3}{4}"
                        , relName
                        , "Many to Many"
                        , refEntity
                        , relationship.IsManaged.ToString()
                        , _withUrls ? string.Format("    Url {0}", _service.ConnectionData.GetRelationshipMetadataUrl(entityMetadata.MetadataId.Value, relationship.MetadataId.Value)) : string.Empty
                        );
                }
            }

            return component.ToString();
        }

        private void GenerateDescriptionEntityKey(StringBuilder builder, List<SolutionComponent> components)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("Name", "IsManaged", "KeyAttributes");

            foreach (var comp in components)
            {
                EntityKeyMetadata metaData = GetEntityKeyMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    handler.AddLine(
                        string.Format("{0}.{1}", metaData.EntityLogicalName, metaData.LogicalName)
                        , metaData.IsManaged.ToString()
                        , string.Join(",", metaData.KeyAttributes.OrderBy(s => s))
                        );
                }
                else
                {
                    handler.AddLine(comp.ObjectId.ToString());
                }
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private void FillEntityKeyComponent(List<SolutionImageComponent> result, IEnumerable<SolutionComponent> components)
        {
            foreach (var comp in components)
            {
                EntityKeyMetadata metaData = GetEntityKeyMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.EntityKey,
                        SchemaName = metaData.LogicalName,
                        ParentSchemaName = metaData.EntityLogicalName,
                        RootComponentBehavior = comp.RootComponentBehavior?.Value,
                    });
                }
            }
        }

        private string GenerateDescriptionEntityKeySingle(SolutionComponent component)
        {
            EntityKeyMetadata metaData = GetEntityKeyMetadata(component.ObjectId.Value);

            if (metaData != null)
            {
                string name = string.Format("EntityKey {0}.{1}    IsManaged {2}", metaData.EntityLogicalName, metaData.LogicalName, metaData.IsManaged.ToString());

                return name;
            }

            return component.ToString();
        }

        private void GenerateDescriptionOptionSet(StringBuilder builder, List<SolutionComponent> components)
        {
            FormatTextTableHandler handlerUnknowed = new FormatTextTableHandler();

            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("OptionSetName", "IsManaged", "Behaviour", "Url");

            foreach (var comp in components)
            {
                string behaviorName = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behaviorName = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                if (this.AllOptionSetMetadata.ContainsKey(comp.ObjectId.Value))
                {
                    var optionSet = this.AllOptionSetMetadata[comp.ObjectId.Value];

                    handler.AddLine(optionSet.Name, optionSet.IsManaged.ToString(), behaviorName, _withUrls ? _service.ConnectionData?.GetGlobalOptionSetUrl(optionSet.MetadataId.Value) : string.Empty);
                }
                else
                {
                    handlerUnknowed.AddLine(comp.ObjectId.ToString(), string.Empty, behaviorName);
                }
            }

            if (handlerUnknowed.Count > 0)
            {
                List<string> lines = handlerUnknowed.GetFormatedLines(true);
                builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }

            {
                List<string> lines = handler.GetFormatedLines(true);

                lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
            }
        }

        private void FillOptionSetComponent(List<SolutionImageComponent> result, IEnumerable<SolutionComponent> components)
        {
            foreach (var comp in components)
            {
                string behaviorName = string.Empty;

                if (comp.RootComponentBehavior != null)
                {
                    behaviorName = SolutionComponent.GetRootComponentBehaviorName(comp.RootComponentBehavior.Value);
                }

                if (this.AllOptionSetMetadata.ContainsKey(comp.ObjectId.Value))
                {
                    var optionSet = this.AllOptionSetMetadata[comp.ObjectId.Value];

                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.OptionSet,
                        SchemaName = optionSet.Name,
                        RootComponentBehavior = comp.RootComponentBehavior?.Value,
                    });
                }
            }

            foreach (var comp in components)
            {
                EntityKeyMetadata metaData = GetEntityKeyMetadata(comp.ObjectId.Value);

                if (metaData != null)
                {
                    result.Add(new SolutionImageComponent()
                    {
                        ComponentType = (int)ComponentType.EntityKey,
                        SchemaName = metaData.LogicalName,
                        ParentSchemaName = metaData.EntityLogicalName,
                        RootComponentBehavior = comp.RootComponentBehavior?.Value,
                    });
                }
            }
        }

        private string GenerateDescriptionOptionSetSingle(SolutionComponent component)
        {
            if (this.AllOptionSetMetadata.Any())
            {
                if (this.AllOptionSetMetadata.ContainsKey(component.ObjectId.Value))
                {
                    var optionSet = this.AllOptionSetMetadata[component.ObjectId.Value];

                    return string.Format("OptionSet {0}    IsManaged {1}{2}"
                        , optionSet.Name
                        , optionSet.IsManaged.ToString()
                        , _withUrls ? string.Format("    Url {0}", _service.ConnectionData.GetGlobalOptionSetUrl(optionSet.MetadataId.Value)) : string.Empty
                        );
                }
            }

            return component.ToString();
        }

        private void GenerateDescriptionManagedProperty(StringBuilder builder, List<SolutionComponent> components)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader(
                "LogicalName"
                , "DisplayName"
                , "Description"
                , "EnablesEntityName"
                , "EnablesAttributeName"
                , "ErrorCode"
                , "EvaluationPriority"
                , "IsPrivate"
                , "IsGlobalForOperation"
                , "ManagedPropertyType"
                , "Operation"
                //, "Description"
                //, "Description"
                );

            //public Label Description { get; }
            //public Label DisplayName { get; }
            //public string EnablesAttributeName { get; }
            //public string EnablesEntityName { get; }
            //public int? ErrorCode { get; }
            //public ManagedPropertyEvaluationPriority? EvaluationPriority { get; }
            //public string IntroducedVersion { get; }
            //public bool? IsGlobalForOperation { get; }
            //public bool? IsPrivate { get; }
            //public string LogicalName { get; }
            //public ManagedPropertyType? ManagedPropertyType { get; }
            //public ManagedPropertyOperation? Operation { get; }

            foreach (var comp in components)
            {

                if (this.AllManagedProperties.ContainsKey(comp.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[comp.ObjectId.Value];

                    handler.AddLine(
                        managedProperty.LogicalName
                        , CreateFileHandler.GetLocalizedLabel(managedProperty.DisplayName)
                        , CreateFileHandler.GetLocalizedLabel(managedProperty.Description)
                        , managedProperty.EnablesEntityName
                        , managedProperty.EnablesAttributeName
                        , managedProperty.ErrorCode.ToString()
                        , managedProperty.EvaluationPriority.ToString()
                        , managedProperty.IsPrivate.ToString()
                        , managedProperty.IsGlobalForOperation.ToString()
                        , managedProperty.ManagedPropertyType.ToString()
                        , managedProperty.Operation.ToString()
                        //, managedProperty.IsManaged.ToString()
                        //, managedProperty.IsManaged.ToString()
                        //, managedProperty.IsManaged.ToString()
                        //, managedProperty.IsManaged.ToString()
                        //, managedProperty.IsManaged.ToString()
                        );
                }
                else
                {
                    handler.AddLine(comp.ObjectId.ToString());
                }
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionManagedPropertySingle(SolutionComponent component)
        {
            if (this.AllManagedProperties.Any())
            {
                if (this.AllManagedProperties.ContainsKey(component.ObjectId.Value))
                {
                    var managedProperty = this.AllManagedProperties[component.ObjectId.Value];

                    return $"LogicalName {managedProperty.LogicalName}    DisplayName {CreateFileHandler.GetLocalizedLabel(managedProperty.DisplayName)}"
                        + $"    Description {CreateFileHandler.GetLocalizedLabel(managedProperty.Description)}    EnablesEntityName {managedProperty.EnablesEntityName}"
                        + $"    EnablesAttributeName {managedProperty.EnablesAttributeName}    ErrorCode {managedProperty.ErrorCode.ToString()}"
                        + $"    EvaluationPriority {managedProperty.EvaluationPriority.ToString()}    IsPrivate {managedProperty.IsPrivate.ToString()}"
                        + $"    IsGlobalForOperation {managedProperty.IsGlobalForOperation.ToString()}    ManagedPropertyType {managedProperty.ManagedPropertyType.ToString()}"
                        + $"    Operation {managedProperty.Operation.ToString()}"
                        //+ $"    IsManaged {1}    IsManaged {1}"
                        ;
                    //, managedProperty.EnablesAttributeName
                    //, managedProperty.ErrorCode.ToString()
                    //, managedProperty.EvaluationPriority.ToString()
                    //, managedProperty.IsPrivate.ToString()
                    //, managedProperty.IsGlobalForOperation.ToString()
                    //, managedProperty.ManagedPropertyType.ToString()
                    //, managedProperty.Operation.ToString()
                }
            }

            return component.ToString();
        }
    }
}