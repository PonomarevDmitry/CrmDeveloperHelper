using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class EntityMetadataRepository
    {
        private readonly IOrganizationServiceExtented _service;

        public EntityMetadataRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<EntityMetadata>> GetEntitiesDisplayNameAsync()
        {
            return Task<List<EntityMetadata>>.Run(() => GetEntitiesDisplayName());
        }

        private List<EntityMetadata> GetEntitiesDisplayName()
        {
            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression
            (
                nameof(EntityMetadata.LogicalName)
                , nameof(EntityMetadata.DisplayName)
                , nameof(EntityMetadata.SchemaName)
                , nameof(EntityMetadata.Description)
                , nameof(EntityMetadata.DisplayCollectionName)
                , nameof(EntityMetadata.OwnershipType)
                , nameof(EntityMetadata.IsIntersect)
                , nameof(EntityMetadata.ObjectTypeCode)
            )
            {
                AllProperties = false
            };

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,
            };

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
        }

        public Task<List<EntityMetadata>> GetEntitiesWithAttributesAndRelationshipsAsync()
        {
            return Task.Run(() => GetEntitiesWithAttributesAndRelationships());
        }

        private List<EntityMetadata> GetEntitiesWithAttributesAndRelationships()
        {
            MetadataPropertiesExpression relationshipProperties = new MetadataPropertiesExpression
            (
                nameof(RelationshipMetadataBase.SchemaName)

                , nameof(OneToManyRelationshipMetadata.ReferencedEntity)
                , nameof(OneToManyRelationshipMetadata.ReferencingEntity)
                , nameof(OneToManyRelationshipMetadata.ReferencedAttribute)
                , nameof(OneToManyRelationshipMetadata.ReferencingAttribute)

                , nameof(ManyToManyRelationshipMetadata.Entity1LogicalName)
                , nameof(ManyToManyRelationshipMetadata.Entity2LogicalName)
                , nameof(ManyToManyRelationshipMetadata.Entity1IntersectAttribute)
                , nameof(ManyToManyRelationshipMetadata.Entity2IntersectAttribute)

            )
            { AllProperties = false };

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression() { AllProperties = true },

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression
                    (
                        nameof(AttributeMetadata.LogicalName)
                        , nameof(AttributeMetadata.AttributeOf)
                        , nameof(AttributeMetadata.EntityLogicalName)
                        , nameof(AttributeMetadata.DisplayName)
                        , nameof(AttributeMetadata.Description)
                    )
                },

                RelationshipQuery = new RelationshipQueryExpression()
                {
                    Properties = relationshipProperties
                },
            };

            var isEntityKeyExists = _service.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression
                    (
                        nameof(EntityKeyMetadata.LogicalName)
                        , nameof(EntityKeyMetadata.EntityLogicalName)
                        , nameof(EntityKeyMetadata.KeyAttributes)
                    )
                };
            }

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            var result = response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();

            return result;
        }

        public Task<List<EntityMetadata>> GetEntitiesWithAttributesAsync()
        {
            return Task.Run(() => GetEntitiesWithAttributes());
        }

        private List<EntityMetadata> GetEntitiesWithAttributes()
        {
            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression
            (
                nameof(EntityMetadata.LogicalName)
                , nameof(EntityMetadata.DisplayName)
                , nameof(EntityMetadata.SchemaName)
                , nameof(EntityMetadata.Description)
                , nameof(EntityMetadata.DisplayCollectionName)
                , nameof(EntityMetadata.OwnershipType)
                , nameof(EntityMetadata.ObjectTypeCode)
                , nameof(EntityMetadata.Attributes)
            )
            {
                AllProperties = false
            };

            MetadataPropertiesExpression attributeProperties = new MetadataPropertiesExpression
            (
                nameof(AttributeMetadata.LogicalName)
                , nameof(AttributeMetadata.AttributeOf)
                , nameof(AttributeMetadata.EntityLogicalName)
                , nameof(AttributeMetadata.SchemaName)
                , nameof(AttributeMetadata.DisplayName)
                , nameof(EnumAttributeMetadata.OptionSet)
            )
            {
                AllProperties = false
            };

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = attributeProperties
                },
            };

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
        }

        public Task<EntityMetadata> GetEntityMetadataAsync(string entityName)
        {
            return Task.Run(() => GetEntityMetadata(entityName));
        }

        public EntityMetadata GetEntityMetadata(string entityName)
        {
            try
            {
                MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
                entityFilter.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.Equals, entityName));

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

                return response.EntityMetadata.SingleOrDefault();
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);

                return null;
            }
        }

        public Task<EntityMetadata> GetEntityMetadataWithAttributesAsync(string entityName)
        {
            return Task.Run(() => GetEntityMetadataWithAttributes(entityName));
        }

        private EntityMetadata GetEntityMetadataWithAttributes(string entityName)
        {
            MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.Equals, entityName));

            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression
            (
                nameof(EntityMetadata.LogicalName)
                , nameof(EntityMetadata.DisplayName)
                , nameof(EntityMetadata.SchemaName)
                , nameof(EntityMetadata.Description)
                , nameof(EntityMetadata.PrimaryIdAttribute)
                , nameof(EntityMetadata.PrimaryNameAttribute)
                , nameof(EntityMetadata.ObjectTypeCode)
                , nameof(EntityMetadata.Attributes)
            )
            {
                AllProperties = false
            };

            MetadataPropertiesExpression attributeProperties = new MetadataPropertiesExpression()
            {
                AllProperties = true,
            };

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = attributeProperties
                },

                Criteria = entityFilter,
            };

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            return response.EntityMetadata.SingleOrDefault();
        }

        public bool IsEntityExists(string entityName)
        {
            try
            {
                MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
                entityFilter.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.Equals, entityName));

                var entityQueryExpression = new EntityQueryExpression()
                {
                    Criteria = entityFilter,
                };

                var response = (RetrieveMetadataChangesResponse)_service.Execute(
                    new RetrieveMetadataChangesRequest()
                    {
                        ClientVersionStamp = null,
                        Query = entityQueryExpression,
                    }
                );

                return response.EntityMetadata.SingleOrDefault() != null;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);

                return false;
            }
        }

        public Task<List<EntityMetadata>> GetEntitiesPropertiesAsync(string entityName, int? entityTypeCode, params string[] properties)
        {
            return Task<List<EntityMetadata>>.Run(() => GetEntitiesProperties(entityName, entityTypeCode, properties));
        }

        private List<EntityMetadata> GetEntitiesProperties(string entityName, int? entityTypeCode, params string[] properties)
        {
            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression(properties)
            {
                AllProperties = false
            };

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression
                    (
                        nameof(AttributeMetadata.LogicalName)
                        , nameof(AttributeMetadata.AttributeType)
                    ),
                },
            };

            if (!string.IsNullOrEmpty(entityName) || entityTypeCode.HasValue)
            {
                var criteria = new MetadataFilterExpression(LogicalOperator.Or);

                if (!string.IsNullOrEmpty(entityName))
                {
                    criteria.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.Equals, entityName));
                }

                if (entityTypeCode.HasValue)
                {
                    criteria.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.ObjectTypeCode), MetadataConditionOperator.Equals, entityTypeCode.Value));
                }

                entityQueryExpression.Criteria = criteria;
            }

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            if (response.EntityMetadata.Any())
            {
                return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
            }
            else if (!string.IsNullOrEmpty(entityName) || entityTypeCode.HasValue)
            {
                return GetEntitiesProperties(null, null, properties);
            }

            return new List<EntityMetadata>();
        }

        public Task ExportEntityXmlAsync(string entityName, string filePath)
        {
            return Task.Run(() => ExportEntityXml(entityName, filePath));
        }

        private void ExportEntityXml(string entityName, string filePath)
        {
            var request = new OrganizationRequest("RetrieveEntityXml");
            request.Parameters["EntityName"] = entityName;

            var response = _service.Execute(request);

            if (response.Results.ContainsKey("EntityXml"))
            {
                var text = response.Results["EntityXml"].ToString();

                if (ContentCoparerHelper.TryParseXml(text, out var doc))
                {
                    text = doc.ToString();
                }

                File.WriteAllText(filePath, text, new UTF8Encoding(false));
            }
        }

        public Task<EntityMetadata> GetEntityMetadataAttributesAsync(string entityName, EntityFilters filters)
        {
            return Task.Run(() => GetEntityMetadataAttributes(entityName, filters));
        }

        private EntityMetadata GetEntityMetadataAttributes(string entityName, EntityFilters filters)
        {
            try
            {
                RetrieveEntityRequest request = new RetrieveEntityRequest()
                {
                    LogicalName = entityName,
                    EntityFilters = filters,
                };

                var response = (RetrieveEntityResponse)_service.Execute(request);

                return response.EntityMetadata;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);

                return null;
            }
        }

        public Task<AttributeMetadata> GetAttributeMetadataAsync(Guid idAttributeMetadata)
        {
            return Task.Run(() => GetAttributeMetadata(idAttributeMetadata));
        }

        private AttributeMetadata GetAttributeMetadata(Guid idAttributeMetadata)
        {
            try
            {
                RetrieveAttributeRequest request = new RetrieveAttributeRequest()
                {
                    MetadataId = idAttributeMetadata,
                };

                var response = (RetrieveAttributeResponse)_service.Execute(request);

                return response.AttributeMetadata;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);

                return null;
            }
        }

        public Task<EntityMetadata> GetEntityMetadataAttributesAsync(Guid idEntityMetadata, EntityFilters filters)
        {
            return Task.Run(() => GetEntityMetadataAttributes(idEntityMetadata, filters));
        }

        private EntityMetadata GetEntityMetadataAttributes(Guid idEntityMetadata, EntityFilters filters)
        {
            try
            {
                RetrieveEntityRequest request = new RetrieveEntityRequest()
                {
                    MetadataId = idEntityMetadata,
                    EntityFilters = filters,
                };

                var response = (RetrieveEntityResponse)_service.Execute(request);

                return response.EntityMetadata;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);

                return null;
            }
        }

        public Task<List<EntityMetadata>> GetEntitiesForEntityAttributeExplorerAsync(EntityFilters filters)
        {
            return Task<List<EntityMetadata>>.Run(() => GetEntitiesForEntityAttributeExplorer(filters));
        }

        private List<EntityMetadata> GetEntitiesForEntityAttributeExplorer(EntityFilters filters)
        {
            RetrieveAllEntitiesRequest raer = new RetrieveAllEntitiesRequest() { EntityFilters = filters };

            RetrieveAllEntitiesResponse resp = (RetrieveAllEntitiesResponse)_service.Execute(raer);

            return resp.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
        }

        public Task<List<EntityMetadata>> GetEntitiesDisplayNameWithPrivilegesAsync()
        {
            return Task<List<EntityMetadata>>.Run(() => GetEntitiesDisplayNameWithPrivileges());
        }

        private List<EntityMetadata> GetEntitiesDisplayNameWithPrivileges()
        {
            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression
            (
                nameof(EntityMetadata.LogicalName)
                , nameof(EntityMetadata.DisplayName)
                , nameof(EntityMetadata.SchemaName)
                , nameof(EntityMetadata.Description)
                , nameof(EntityMetadata.ObjectTypeCode)
                , nameof(EntityMetadata.Privileges)
            )
            {
                AllProperties = false
            };

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,
            };

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
        }

        public Task<List<EntityMetadata>> GetEntitiesWithAttributesForAuditAsync()
        {
            return Task.Run(() => GetEntitiesWithAttributesForAudit());
        }

        private List<EntityMetadata> GetEntitiesWithAttributesForAudit()
        {
            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression
            (
                nameof(EntityMetadata.LogicalName)
                , nameof(EntityMetadata.IsAuditEnabled)
                , nameof(EntityMetadata.Attributes)
            )
            {
                AllProperties = false
            };

            MetadataPropertiesExpression attributeProperties = new MetadataPropertiesExpression
            (
                nameof(AttributeMetadata.LogicalName)
                , nameof(AttributeMetadata.EntityLogicalName)
                , nameof(AttributeMetadata.IsAuditEnabled)
                , nameof(AttributeMetadata.AttributeOf)
            )
            {
                AllProperties = false
            };

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = attributeProperties
                },
            };

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
        }

        public Task UpdateEntityMetadataAsync(EntityMetadata entityMetadata)
        {
            return Task.Run(() => UpdateEntityMetadata(entityMetadata));
        }

        private void UpdateEntityMetadata(EntityMetadata entityMetadata)
        {
            var request = new UpdateEntityRequest()
            {
                Entity = entityMetadata,
            };

            var response = (UpdateEntityResponse)_service.Execute(request);
        }

        public Task UpdateAttributeMetadataAsync(AttributeMetadata attributeMetadata)
        {
            return Task.Run(() => UpdateAttributeMetadata(attributeMetadata));
        }

        private void UpdateAttributeMetadata(AttributeMetadata attributeMetadata)
        {
            var request = new UpdateAttributeRequest()
            {
                Attribute = attributeMetadata,
                EntityName = attributeMetadata.EntityLogicalName,
                MergeLabels = false,
            };

            var response = (UpdateAttributeResponse)_service.Execute(request);
        }

        public Task UpdateRelationshipMetadataAsync(RelationshipMetadataBase relationshipMetadataBase)
        {
            return Task.Run(() => UpdateRelationshipMetadata(relationshipMetadataBase));
        }

        private void UpdateRelationshipMetadata(RelationshipMetadataBase relationshipMetadataBase)
        {
            var request = new UpdateRelationshipRequest()
            {
                Relationship = relationshipMetadataBase,
                MergeLabels = false,
            };

            var response = (UpdateRelationshipResponse)_service.Execute(request);
        }
    }
}
