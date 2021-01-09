using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class PluginAssemblyRepository : IEntitySaver
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public PluginAssemblyRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public static Task<string[]> GetAttributesAsync(IOrganizationServiceExtented service)
        {
            return Task.Run(() => GetAttributes(service));
        }

        private static string[] GetAttributes(IOrganizationServiceExtented service)
        {
            if (service.ConnectionData.PluginAssemblyProperties != null)
            {
                return service.ConnectionData.PluginAssemblyProperties;
            }

            MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.Equals, PluginAssembly.EntityLogicalName));

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression(nameof(EntityMetadata.LogicalName), nameof(EntityMetadata.Attributes)),
                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = new MetadataPropertiesExpression()
                    {
                        PropertyNames =
                        {
                            nameof(AttributeMetadata.LogicalName), nameof(AttributeMetadata.AttributeOf), nameof(AttributeMetadata.IsValidForRead)
                        }
                    }
                },

                Criteria = entityFilter,
            };

            var response = (RetrieveMetadataChangesResponse)service.Execute(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            var metadata = response.EntityMetadata.SingleOrDefault(e => string.Equals(e.LogicalName, PluginAssembly.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase));

            if (metadata == null)
            {
                return null;
            }

            var list = new List<string>();

            if (metadata.Attributes != null)
            {
                foreach (var attr in metadata.Attributes.OrderBy(a => a.LogicalName))
                {
                    if (!string.IsNullOrEmpty(attr.AttributeOf))
                    {
                        continue;
                    }

                    if (string.Equals(attr.LogicalName, PluginAssembly.Schema.Attributes.content, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    if (!attr.IsValidForRead.GetValueOrDefault()
                        //&& !attr.IsValidForCreate.GetValueOrDefault()
                        //&& !attr.IsValidForUpdate.GetValueOrDefault()
                        //&& !attr.IsValidForAdvancedFind.Value
                        )
                    {
                        continue;
                    }

                    list.Add(attr.LogicalName);
                }
            }

            service.ConnectionData.PluginAssemblyProperties = list.ToArray();

            return service.ConnectionData.PluginAssemblyProperties;
        }

        public Task<List<PluginAssembly>> GetPluginAssembliesAsync(string name = null, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetPluginAssemblies(name, columnSet));
        }

        private List<PluginAssembly> GetPluginAssemblies(string name, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                ColumnSet = columnSet ?? new ColumnSet(GetAttributes(_service)),

                EntityName = PluginAssembly.EntityLogicalName,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                    },
                },

                Orders =
                {
                    new OrderExpression(PluginAssembly.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(PluginAssembly.Schema.Attributes.createdon, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                query.Criteria.AddCondition(PluginAssembly.Schema.Attributes.name, ConditionOperator.Like, "%" + name + "%");
            }

            return _service.RetrieveMultipleAll<PluginAssembly>(query);
        }

        public Task<List<PluginAssembly>> GetAllPluginAssemblisWithStepsAsync()
        {
            return Task.Run(() => GetAllPluginAssemblisWithSteps());
        }

        private List<PluginAssembly> GetAllPluginAssemblisWithSteps()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = PluginAssembly.EntityLogicalName,
                ColumnSet = new ColumnSet(GetAttributes(_service)),

                NoLock = true,

                Distinct = true,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                    },
                },

                //LinkEntities =
                //{
                //    new LinkEntity()
                //    {
                //        LinkFromEntityName = PluginAssembly.EntityLogicalName,
                //        LinkFromAttributeName = "pluginassemblyid",

                //        LinkToEntityName = PluginType.EntityLogicalName,
                //        LinkToAttributeName = "pluginassemblyid",

                //        LinkEntities =
                //        {
                //            new LinkEntity()
                //            {
                //                LinkFromEntityName = PluginType.EntityLogicalName,
                //                LinkFromAttributeName = "plugintypeid",

                //                LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                //                LinkToAttributeName = "plugintypeid",

                //                LinkCriteria =
                //                {
                //                    Conditions =
                //                    {
                //                        new ConditionExpression("stage", ConditionOperator.In, 10, 20, 40),
                //                        new ConditionExpression("ishidden", ConditionOperator.Equal, false),
                //                    },
                //                },
                //            },
                //        },
                //    },
                //},

                Orders =
                {
                    new OrderExpression(PluginAssembly.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(PluginAssembly.Schema.Attributes.createdon, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<PluginAssembly>(query);
        }

        public Task<PluginAssembly> FindAssemblyAsync(string name)
        {
            return Task.Run(() => FindAssembly(name));
        }

        private PluginAssembly FindAssembly(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = PluginAssembly.EntityLogicalName,

                ColumnSet = new ColumnSet(GetAttributes(_service)),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                        new ConditionExpression(PluginAssembly.Schema.Attributes.name, ConditionOperator.Equal, name),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<PluginAssembly>()).SingleOrDefault() : null;
        }

        public Task<PluginAssembly> FindAssemblyByLikeNameAsync(string name)
        {
            return Task.Run(() => FindAssemblyByLikeName(name));
        }

        private PluginAssembly FindAssemblyByLikeName(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = PluginAssembly.EntityLogicalName,

                ColumnSet = new ColumnSet(GetAttributes(_service)),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                        new ConditionExpression(PluginAssembly.Schema.Attributes.name, ConditionOperator.Like, "%" + name + "%"),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<PluginAssembly>()).SingleOrDefault() : null;
        }

        public Task<PluginAssembly> GetAssemblyByIdRetrieveRequestAsync(Guid id, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetAssemblyByIdByRetrieveRequest(id, columnSet));
        }

        private PluginAssembly GetAssemblyByIdByRetrieveRequest(Guid id, ColumnSet columnSet)
        {
            var request = new RetrieveRequest()
            {
                Target = new EntityReference(PluginAssembly.EntityLogicalName, id),
                ColumnSet = columnSet ?? new ColumnSet(GetAttributes(_service)),
                ReturnNotifications = true,
            };

            var response = (RetrieveResponse)_service.Execute(request);

            return response.Entity.ToEntity<PluginAssembly>();
        }

        public Task<PluginAssembly> GetByIdAsync(Guid idPluginAssembly, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idPluginAssembly, columnSet));
        }

        public PluginAssembly GetById(Guid idPluginAssembly, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = PluginAssembly.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(GetAttributes(_service)),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.EntityPrimaryIdAttribute, ConditionOperator.Equal, idPluginAssembly),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<PluginAssembly>()).SingleOrDefault() : null;
        }

        public PluginAssembly FindAssemblyByFullName(string name, string versionString, string cultureString, string publicKeyTokenString, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = PluginAssembly.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(GetAttributes(_service)),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.Schema.Attributes.ishidden, ConditionOperator.Equal, false),

                        new ConditionExpression(PluginAssembly.Schema.Attributes.name, ConditionOperator.Equal, name),
                        new ConditionExpression(PluginAssembly.Schema.Attributes.version, ConditionOperator.Equal, versionString),
                        new ConditionExpression(PluginAssembly.Schema.Attributes.culture, ConditionOperator.Equal, cultureString),
                        new ConditionExpression(PluginAssembly.Schema.Attributes.publickeytoken, ConditionOperator.Equal, publicKeyTokenString),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<PluginAssembly>()).SingleOrDefault() : null;
        }

        public Task<List<PluginAssembly>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<PluginAssembly> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = PluginAssembly.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(GetAttributes(_service)),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(PluginAssembly.Schema.Attributes.name, OrderType.Ascending),
                    new OrderExpression(PluginAssembly.Schema.Attributes.createdon, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<PluginAssembly>(query);
        }

        public async Task<Guid> UpsertAsync(Entity entity, Action<string> updateStatus)
        {
            Guid entityId = entity.Id;

            if (entity.Attributes.ContainsKey(PluginAssembly.EntityPrimaryIdAttribute)
                && entity.Attributes[PluginAssembly.EntityPrimaryIdAttribute] != null
                && entity.Attributes[PluginAssembly.EntityPrimaryIdAttribute] is Guid tempId
            )
            {
                entityId = tempId;
            }

            if (entityId == Guid.Empty)
            {
                return await _service.CreateAsync(entity);
            }
            else
            {
                entity.Id = entityId;

                var exists = await GetByIdAsync(entityId, ColumnSetInstances.None);

                if (exists != null)
                {
                    var updateAssembly = await GetAssemblyByIdRetrieveRequestAsync(entityId, ColumnSetInstances.AllColumns);

                    foreach (var attribute in entity.Attributes)
                    {
                        updateAssembly.Attributes[attribute.Key] = attribute.Value;
                    }

                    updateAssembly.Id = entityId;
                    updateAssembly.PluginAssemblyId = entityId;

                    await _service.UpdateAsync(updateAssembly);

                    return entityId;
                }
                else
                {
                    return await _service.CreateAsync(entity);
                }
            }
        }
    }
}
