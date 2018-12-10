using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class PluginTypeRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public PluginTypeRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        private List<PluginType> GetAllPluginTypeWithSteps()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = PluginType.EntityLogicalName,
                ColumnSet = new ColumnSet(true),

                NoLock = true,

                Distinct = true,

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = PluginType.EntityLogicalName,
                        LinkFromAttributeName = PluginType.PrimaryIdAttribute,

                        LinkToEntityName = SdkMessageProcessingStep.EntityLogicalName,
                        LinkToAttributeName = SdkMessageProcessingStep.Schema.Attributes.eventhandler,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.stage, ConditionOperator.In
                                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10
                                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20
                                    , (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40
                                ),
                                new ConditionExpression(SdkMessageProcessingStep.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                            },
                        },
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<PluginType>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(ent => ent.ToEntity<PluginType>()));

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<PluginType>> GetPluginTypesAsync(string name, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetPluginTypes(name, columnSet));
        }

        private List<PluginType> GetPluginTypes(string name, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = PluginType.EntityLogicalName,
                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = PluginType.EntityLogicalName,
                        LinkFromAttributeName = PluginType.Schema.Attributes.pluginassemblyid,

                        LinkToEntityName = PluginAssembly.EntityLogicalName,
                        LinkToAttributeName = PluginAssembly.PrimaryIdAttribute,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(PluginAssembly.Schema.Attributes.ishidden, ConditionOperator.Equal, false),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(PluginType.Schema.Attributes.typename, OrderType.Ascending),
                    new OrderExpression(PluginType.Schema.Attributes.createdon, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(PluginType.Schema.Attributes.typename, ConditionOperator.Like, "%" + name + "%"));
            }

            var result = new List<PluginType>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(ent => ent.ToEntity<PluginType>()));

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<PluginType>> GetPluginTypesAsync(Guid pluginAssemblyId)
        {
            return Task.Run(() => GetPluginTypes(pluginAssemblyId));
        }

        private List<PluginType> GetPluginTypes(Guid pluginAssemblyId)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = PluginType.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginType.Schema.Attributes.pluginassemblyid, ConditionOperator.Equal, pluginAssemblyId),
                    },
                },

                Orders =
                {
                    new OrderExpression(PluginType.Schema.Attributes.typename, OrderType.Ascending),
                    new OrderExpression(PluginType.Schema.Attributes.createdon, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<PluginType>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(ent => ent.ToEntity<PluginType>()));

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
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public PluginType FindTypeByFullName(string name, string assemblyName, string versionString, string cultureString, string publicKeyTokenString, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = PluginType.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginType.Schema.Attributes.name, ConditionOperator.Equal, name),

                        new ConditionExpression(PluginType.Schema.Attributes.assemblyname, ConditionOperator.Equal, assemblyName),
                        new ConditionExpression(PluginType.Schema.Attributes.version, ConditionOperator.Equal, versionString),
                        new ConditionExpression(PluginType.Schema.Attributes.culture, ConditionOperator.Equal, cultureString),
                        new ConditionExpression(PluginType.Schema.Attributes.publickeytoken, ConditionOperator.Equal, publicKeyTokenString),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<PluginType>()).SingleOrDefault() : null;
        }

        internal PluginType FindPluginType(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = PluginType.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginType.Schema.Attributes.typename, ConditionOperator.Equal, name),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<PluginType>()).SingleOrDefault() : null;
        }

        internal Task<PluginType> FindPluginTypeByLikeNameAsync(string name)
        {
            return Task.Run(() => FindPluginTypeByLikeName(name));
        }

        private PluginType FindPluginTypeByLikeName(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = PluginType.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginType.Schema.Attributes.typename, ConditionOperator.Like, "%" + name + "%"),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<PluginType>()).SingleOrDefault() : null;
        }

        public async Task<PluginType> GetPluginTypeByIdAsync(Guid id)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = PluginType.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginType.PrimaryIdAttribute, ConditionOperator.Equal, id),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = PluginType.EntityLogicalName,
                        LinkFromAttributeName = PluginType.Schema.Attributes.pluginassemblyid,

                        LinkToEntityName = PluginAssembly.EntityLogicalName,
                        LinkToAttributeName = PluginAssembly.PrimaryIdAttribute,

                        Columns = new ColumnSet(await PluginAssemblyRepository.GetAttributesAsync(_service)),

                        EntityAlias = PluginType.Schema.Attributes.pluginassemblyid,
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<PluginType>()).SingleOrDefault();
        }
    }
}