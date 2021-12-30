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
using System.ServiceModel;
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

        private static string[] _baseEntityMetadataAttributes =
        {
            nameof(EntityMetadata.IsQuickCreateEnabled)
            , nameof(EntityMetadata.IsReadingPaneEnabled)
            , nameof(EntityMetadata.IsValidForAdvancedFind)
            , nameof(EntityMetadata.IsEnabledForTrace)
            , nameof(EntityMetadata.IsEnabledForCharts)
            , nameof(EntityMetadata.IsManaged)
            , nameof(EntityMetadata.IsIntersect)
            , nameof(EntityMetadata.IsImportable)
            , nameof(EntityMetadata.ChangeTrackingEnabled)
            , nameof(EntityMetadata.IsOptimisticConcurrencyEnabled)
            , nameof(EntityMetadata.HasActivities)
            , nameof(EntityMetadata.HasNotes)
            , nameof(EntityMetadata.IsLogicalEntity)
            , nameof(EntityMetadata.UsesBusinessDataLabelTable)
            , nameof(EntityMetadata.IsPrivate)
            , nameof(EntityMetadata.IsEnabledForExternalChannels)
            , nameof(EntityMetadata.EnforceStateTransitions)
            , nameof(EntityMetadata.IsStateModelAware)
            , nameof(EntityMetadata.SyncToExternalSearchIndex)
            , nameof(EntityMetadata.IsActivity)
            , nameof(EntityMetadata.AutoCreateAccessTeams)
            //, nameof(EntityMetadata.IsMSTeamsIntegrationEnabled)
            , nameof(EntityMetadata.IsDocumentRecommendationsEnabled)
            , nameof(EntityMetadata.IsBPFEntity)
            , nameof(EntityMetadata.IsSLAEnabled)
            , nameof(EntityMetadata.IsKnowledgeManagementEnabled)
            , nameof(EntityMetadata.IsInteractionCentricEnabled)
            , nameof(EntityMetadata.IsOneNoteIntegrationEnabled)
            , nameof(EntityMetadata.IsDocumentManagementEnabled)
            , nameof(EntityMetadata.EntityHelpUrlEnabled)
            , nameof(EntityMetadata.CanTriggerWorkflow)
            , nameof(EntityMetadata.AutoRouteToOwnerQueue)
            , nameof(EntityMetadata.IsActivityParty)
            , nameof(EntityMetadata.IsAvailableOffline)
            , nameof(EntityMetadata.IsChildEntity)
            , nameof(EntityMetadata.HasFeedback)
            , nameof(EntityMetadata.IsBusinessProcessEnabled)
            , nameof(EntityMetadata.IsCustomEntity)
            , nameof(EntityMetadata.IsAIRUpdated)
            //, nameof(EntityMetadata.IsSolutionAware)

            , nameof(EntityMetadata.IsOfflineInMobileClient)
            , nameof(EntityMetadata.IsReadOnlyInMobileClient)
            , nameof(EntityMetadata.IsVisibleInMobileClient)
            , nameof(EntityMetadata.IsVisibleInMobile)
            , nameof(EntityMetadata.IsMailMergeEnabled)
            , nameof(EntityMetadata.CanChangeTrackingBeEnabled)
            , nameof(EntityMetadata.CanChangeHierarchicalRelationship)
            , nameof(EntityMetadata.CanModifyAdditionalSettings)
            , nameof(EntityMetadata.IsAuditEnabled)
            , nameof(EntityMetadata.CanEnableSyncToExternalSearchIndex)
            , nameof(EntityMetadata.CanBeInCustomEntityAssociation)
            , nameof(EntityMetadata.CanBeInManyToMany)
            , nameof(EntityMetadata.CanBePrimaryEntityInRelationship)
            , nameof(EntityMetadata.CanBeRelatedEntityInRelationship)
            , nameof(EntityMetadata.CanCreateCharts)
            , nameof(EntityMetadata.CanCreateViews)
            , nameof(EntityMetadata.CanCreateForms)
            , nameof(EntityMetadata.CanCreateAttributes)
            , nameof(EntityMetadata.IsDuplicateDetectionEnabled)
            , nameof(EntityMetadata.IsMappable)
            , nameof(EntityMetadata.IsCustomizable)
            , nameof(EntityMetadata.IsConnectionsEnabled)
            , nameof(EntityMetadata.IsValidForQueue)
            , nameof(EntityMetadata.IsRenameable)
        };

        private static void FillAdditionalEntityProperties(IOrganizationServiceExtented service, MetadataPropertiesExpression entityProperties)
        {
            if (service.ConnectionData.EntityMetadataProperties != null)
            {
                entityProperties.PropertyNames.AddRange(service.ConnectionData.EntityMetadataProperties);
            }

            var list = new List<string>(_baseEntityMetadataAttributes);

            if (RemoveWrongEntityMetadataAttributes(service, list))
            {
                service.ConnectionData.EntityMetadataProperties = list.ToArray();

                entityProperties.PropertyNames.AddRange(service.ConnectionData.EntityMetadataProperties);
            }
        }

        private static bool RemoveWrongEntityMetadataAttributes(IOrganizationServiceExtented service, List<string> list)
        {
            bool executeAgain = true;

            while (executeAgain)
            {
                executeAgain = false;

                try
                {
                    MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
                    entityFilter.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.Equals, SystemUser.EntityLogicalName));

                    var entityQueryExpression = new EntityQueryExpression()
                    {
                        Properties = new MetadataPropertiesExpression(nameof(EntityMetadata.LogicalName)),

                        Criteria = entityFilter,
                    };

                    entityQueryExpression.Properties.PropertyNames.AddRange(list);

                    var response = (RetrieveMetadataChangesResponse)service.Execute(
                        new RetrieveMetadataChangesRequest()
                        {
                            ClientVersionStamp = null,
                            Query = entityQueryExpression,
                        }
                    );

                    return true;
                }
                catch (FaultException<OrganizationServiceFault> fex)
                {
                    if (fex.Detail != null
                        && fex.Detail.ErrorCode == -2147204733
                        && fex.Detail.InnerFault != null
                        && fex.Detail.InnerFault.InnerFault != null
                        && fex.Detail.InnerFault.InnerFault.Message.StartsWith("The type EntityMetadata does not have a property named ", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        var propertyName = fex.Detail.InnerFault.InnerFault.Message.Replace("The type EntityMetadata does not have a property named ", string.Empty);

                        list.Remove(propertyName);

                        executeAgain = true;
                    }
                    else
                    {
                        DTEHelper.WriteExceptionToLog(fex);
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);
                }
            }

            return false;
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
                , nameof(EntityMetadata.ObjectTypeCode)
                , nameof(EntityMetadata.IsIntersect)
                , nameof(EntityMetadata.IsCustomEntity)
                , nameof(EntityMetadata.IsActivity)

                , nameof(EntityMetadata.OwnershipType)
            )
            {
                AllProperties = false,
            };

            FillAdditionalEntityProperties(_service, entityProperties);

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
                , nameof(EntityMetadata.ObjectTypeCode)
                , nameof(EntityMetadata.IsIntersect)
                , nameof(EntityMetadata.IsCustomEntity)
                , nameof(EntityMetadata.Attributes)

                , nameof(EntityMetadata.OwnershipType)
            )
            {
                AllProperties = false,
            };

            FillAdditionalEntityProperties(_service, entityProperties);

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

        public Task<List<EntityMetadata>> GetEntitiesWithAttributesFullAsync(IEnumerable<string> entityList)
        {
            return Task.Run(() => GetEntitiesWithAttributesFull(entityList));
        }

        private List<EntityMetadata> GetEntitiesWithAttributesFull(IEnumerable<string> entityList)
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
                , nameof(EntityMetadata.IsIntersect)
                , nameof(EntityMetadata.IsCustomEntity)
                , nameof(EntityMetadata.Attributes)
            )
            {
                AllProperties = false,
            };

            FillAdditionalEntityProperties(_service, entityProperties);

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,

                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression()
                    {
                        AllProperties = true,
                    },
                },

                Criteria = new MetadataFilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.In, entityList.ToArray()),
                    },
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
                DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);

                return null;
            }
        }

        public List<EntityMetadata> GetEntityMetadataList(IEnumerable<string> entityList)
        {
            try
            {
                var entityQueryExpression = new EntityQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression() { AllProperties = true },
                    AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                    RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },
                    LabelQuery = new LabelQueryExpression(),

                    Criteria = new MetadataFilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                        {
                            new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.In, entityList.ToArray()),
                        },
                    },
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

                return response.EntityMetadata.ToList();
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
                , nameof(EntityMetadata.IsIntersect)
                , nameof(EntityMetadata.IsCustomEntity)
                , nameof(EntityMetadata.Attributes)
            )
            {
                AllProperties = false,
            };

            FillAdditionalEntityProperties(_service, entityProperties);

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

        public Task<List<EntityMetadata>> FindEntitiesPropertiesOrAllAsync(string entityName, int? entityTypeCode, params string[] properties)
        {
            return Task<List<EntityMetadata>>.Run(() => FindEntitiesPropertiesOrAll(entityName, entityTypeCode, properties));
        }

        private List<EntityMetadata> FindEntitiesPropertiesOrAll(string entityName, int? entityTypeCode, params string[] properties)
        {
            var entityProperties = new MetadataPropertiesExpression(properties)
            {
                AllProperties = false
            };

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,
            };

            if (properties != null && properties.Contains(nameof(EntityMetadata.Attributes), StringComparer.InvariantCultureIgnoreCase))
            {
                entityQueryExpression.AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression
                    (
                        nameof(AttributeMetadata.LogicalName)
                        , nameof(AttributeMetadata.AttributeType)
                        , nameof(AttributeMetadata.IsValidForRead)
                    ),
                };
            }

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

            var request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            var response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            if (response.EntityMetadata.Any())
            {
                return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
            }
            else if (!string.IsNullOrEmpty(entityName) || entityTypeCode.HasValue)
            {
                return FindEntitiesPropertiesOrAll(null, null, properties);
            }

            return new List<EntityMetadata>();
        }

        public Task<List<EntityMetadata>> FindEntitiesPropertiesOrEmptyAsync(string entityName, int? entityTypeCode, params string[] properties)
        {
            return Task<List<EntityMetadata>>.Run(() => FindEntitiesPropertiesOrEmpty(entityName, entityTypeCode, properties));
        }

        private List<EntityMetadata> FindEntitiesPropertiesOrEmpty(string entityName, int? entityTypeCode, params string[] properties)
        {
            var entityProperties = new MetadataPropertiesExpression(properties)
            {
                AllProperties = false
            };

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = entityProperties,
            };

            if (properties != null && properties.Contains(nameof(EntityMetadata.Attributes), StringComparer.InvariantCultureIgnoreCase))
            {
                entityQueryExpression.AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression
                    (
                        nameof(AttributeMetadata.LogicalName)
                        , nameof(AttributeMetadata.AttributeType)
                        , nameof(AttributeMetadata.IsValidForRead)
                    ),
                };
            }

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

            var request = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
            };

            var response = (RetrieveMetadataChangesResponse)_service.Execute(request);

            return response.EntityMetadata.OrderBy(ent => ent.LogicalName).ToList();
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

                if (ContentComparerHelper.TryParseXml(text, out var doc))
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
                , nameof(EntityMetadata.DisplayCollectionName)
                , nameof(EntityMetadata.ObjectTypeCode)
                , nameof(EntityMetadata.IsIntersect)
                , nameof(EntityMetadata.IsCustomEntity)
                , nameof(EntityMetadata.IsActivity)

                , nameof(EntityMetadata.Privileges)
            )
            {
                AllProperties = false,
            };

            FillAdditionalEntityProperties(_service, entityProperties);

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
                AllProperties = false,
            };

            FillAdditionalEntityProperties(_service, entityProperties);

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
