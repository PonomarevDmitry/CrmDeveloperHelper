using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class EntityMetadataRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _Service { get; set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску издателей.
        /// </summary>
        /// <param name="service"></param>
        public EntityMetadataRepository(IOrganizationServiceExtented service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<EntityMetadata>> GetEntitiesDisplayNameAsync()
        {
            return Task<List<EntityMetadata>>.Run(() => GetEntitiesDisplayName());
        }

        private List<EntityMetadata> GetEntitiesDisplayName()
        {
            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression("LogicalName", "DisplayName", "Description", "DisplayCollectionName", "OwnershipType", "IsIntersect", "ObjectTypeCode")
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

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_Service.Execute(request);

            return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
        }

        public Task<List<EntityMetadata>> GetEntitiesWithAttributesAndRelationshipsAsync()
        {
            return Task.Run(() => GetEntitiesWithAttributesAndRelationships());
        }

        private List<EntityMetadata> GetEntitiesWithAttributesAndRelationships()
        {
            MetadataPropertiesExpression relationshipProperties = new MetadataPropertiesExpression(
                "SchemaName",
                "ReferencedEntity",
                "ReferencingEntity",
                "ReferencedAttribute",
                "ReferencingAttribute",
                "Entity1LogicalName",
                "Entity2LogicalName",
                "Entity1IntersectAttribute",
                "Entity2IntersectAttribute"
                )
            { AllProperties = false };

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression() { AllProperties = true },

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression("AttributeOf", "LogicalName", "EntityLogicalName")
                },

                RelationshipQuery = new RelationshipQueryExpression()
                {
                    Properties = relationshipProperties
                },
            };

            var isEntityKeyExists = _Service.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

            if (isEntityKeyExists)
            {
                entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression("EntityLogicalName", "LogicalName", "KeyAttributes") };
            }

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_Service.Execute(request);

            var result = response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();

            return result;
        }

        public Task<List<EntityMetadata>> GetEntitiesWithAttributesAsync()
        {
            return Task.Run(() => GetEntitiesWithAttributes());
        }

        private List<EntityMetadata> GetEntitiesWithAttributes()
        {
            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression("LogicalName", "DisplayName", "Description", "DisplayCollectionName", "OwnershipType", "Attributes", "ObjectTypeCode")
            {
                AllProperties = false
            };

            MetadataPropertiesExpression attributeProperties = new MetadataPropertiesExpression("AttributeOf", "LogicalName", "EntityLogicalName", "OptionSet")
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

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_Service.Execute(request);

            return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
        }

        public EntityMetadata GetEntityMetadata(string entityName)
        {
            try
            {
                MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
                entityFilter.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, entityName));

                var entityQueryExpression = new EntityQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression() { AllProperties = true },
                    AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                    RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                    LabelQuery = new LabelQueryExpression(),

                    Criteria = entityFilter,
                };

                var isEntityKeyExists = _Service.IsRequestExists(SdkMessageRequest.Instances.RetrieveEntityKeyRequest);

                if (isEntityKeyExists)
                {
                    entityQueryExpression.KeyQuery = new EntityKeyQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } };
                }

                var response = (RetrieveMetadataChangesResponse)_Service.Execute(
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
                Helpers.DTEHelper.WriteExceptionToOutput(ex);

                return null;
            }
        }

        public bool IsEntityExists(string entityName)
        {
            try
            {
                MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
                entityFilter.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, entityName));

                var entityQueryExpression = new EntityQueryExpression()
                {
                    Criteria = entityFilter,
                };

                var response = (RetrieveMetadataChangesResponse)_Service.Execute(
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
                Helpers.DTEHelper.WriteExceptionToOutput(ex);

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
                    Properties = new MetadataPropertiesExpression("LogicalName", "AttributeType"),
                },
            };

            if (!string.IsNullOrEmpty(entityName) || entityTypeCode.HasValue)
            {
                var criteria = new MetadataFilterExpression(LogicalOperator.Or);

                if (!string.IsNullOrEmpty(entityName))
                {
                    criteria.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, entityName));
                }

                if (entityTypeCode.HasValue)
                {
                    criteria.Conditions.Add(new MetadataConditionExpression("ObjectTypeCode", MetadataConditionOperator.Equals, entityTypeCode.Value));
                }

                entityQueryExpression.Criteria = criteria;
            }

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_Service.Execute(request);

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

            var response = _Service.Execute(request);

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

                var response = (RetrieveEntityResponse)_Service.Execute(request);

                return response.EntityMetadata;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(ex);

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

                var response = (RetrieveAttributeResponse)_Service.Execute(request);

                return response.AttributeMetadata;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(ex);

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

                var response = (RetrieveEntityResponse)_Service.Execute(request);

                return response.EntityMetadata;
            }
            catch (Exception ex)
            {
                Helpers.DTEHelper.WriteExceptionToOutput(ex);

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

            RetrieveAllEntitiesResponse resp = (RetrieveAllEntitiesResponse)_Service.Execute(raer);

            return resp.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
        }

        public Task<List<EntityMetadata>> GetEntitiesWithAttributesForAuditAsync()
        {
            return Task.Run(() => GetEntitiesWithAttributesForAudit());
        }

        private List<EntityMetadata> GetEntitiesWithAttributesForAudit()
        {
            MetadataPropertiesExpression entityProperties = new MetadataPropertiesExpression("LogicalName", "IsAuditEnabled", "Attributes")
            {
                AllProperties = false
            };

            MetadataPropertiesExpression attributeProperties = new MetadataPropertiesExpression("AttributeOf", "EntityLogicalName", "LogicalName", "IsAuditEnabled")
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

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)_Service.Execute(request);

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

            var response = (UpdateEntityResponse)_Service.Execute(request);
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

            var response = (UpdateAttributeResponse)_Service.Execute(request);
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

            var response = (UpdateRelationshipResponse)_Service.Execute(request);
        }
    }
}
