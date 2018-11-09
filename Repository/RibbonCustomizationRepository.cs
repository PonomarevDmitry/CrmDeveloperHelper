using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class RibbonCustomizationRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public RibbonCustomizationRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<HashSet<string>> GetEntitiesWithRibbonCustomizationAsync()
        {
            return Task.Run(() => GetEntitiesWithRibbonCustomization());
        }

        private HashSet<string> GetEntitiesWithRibbonCustomization()
        {
            var result = GetEntitiesInRibbonCustomization();

            foreach (var item in GetEntitiesInRibbonCommand())
            {
                result.Add(item);
            }

            foreach (var item in GetEntitiesInRibbonContextGroup())
            {
                result.Add(item);
            }

            foreach (var item in GetEntitiesInRibbonDiff())
            {
                result.Add(item);
            }

            foreach (var item in GetEntitiesInRibbonRule())
            {
                result.Add(item);
            }

            foreach (var item in GetEntitiesInRibbonTabToCommandMap())
            {
                result.Add(item);
            }

            return result;
        }

        private HashSet<string> GetEntitiesInRibbonCustomization()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = RibbonCustomization.EntityLogicalName,

                ColumnSet = new ColumnSet(RibbonCustomization.Schema.Attributes.entity),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RibbonCustomization.Schema.Attributes.entity, ConditionOperator.NotNull),
                    },
                },

                Orders =
                {
                    new OrderExpression(RibbonCustomization.Schema.Attributes.entity, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    foreach (var item in coll.Entities.Select(e => e.ToEntity<RibbonCustomization>()))
                    {
                        if (!string.IsNullOrEmpty(item.Entity))
                        {
                            result.Add(item.Entity);
                        }
                    }

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

        private HashSet<string> GetEntitiesInRibbonCommand()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = RibbonCommand.EntityLogicalName,

                ColumnSet = new ColumnSet(RibbonCommand.Schema.Attributes.entity),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RibbonCommand.Schema.Attributes.entity, ConditionOperator.NotNull),
                    },
                },

                Orders =
                {
                    new OrderExpression(RibbonCommand.Schema.Attributes.entity, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    foreach (var item in coll.Entities.Select(e => e.ToEntity<RibbonCommand>()))
                    {
                        if (!string.IsNullOrEmpty(item.Entity))
                        {
                            result.Add(item.Entity);
                        }
                    }

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

        private HashSet<string> GetEntitiesInRibbonContextGroup()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = RibbonContextGroup.EntityLogicalName,

                ColumnSet = new ColumnSet(RibbonContextGroup.Schema.Attributes.entity),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RibbonContextGroup.Schema.Attributes.entity, ConditionOperator.NotNull),
                    },
                },

                Orders =
                {
                    new OrderExpression(RibbonContextGroup.Schema.Attributes.entity, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    foreach (var item in coll.Entities.Select(e => e.ToEntity<RibbonContextGroup>()))
                    {
                        if (!string.IsNullOrEmpty(item.Entity))
                        {
                            result.Add(item.Entity);
                        }
                    }

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

        private HashSet<string> GetEntitiesInRibbonDiff()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = RibbonDiff.EntityLogicalName,

                ColumnSet = new ColumnSet(RibbonDiff.Schema.Attributes.entity),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RibbonDiff.Schema.Attributes.entity, ConditionOperator.NotNull),
                    },
                },

                Orders =
                {
                    new OrderExpression(RibbonDiff.Schema.Attributes.entity, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    foreach (var item in coll.Entities.Select(e => e.ToEntity<RibbonDiff>()))
                    {
                        if (!string.IsNullOrEmpty(item.Entity))
                        {
                            result.Add(item.Entity);
                        }
                    }

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

        private HashSet<string> GetEntitiesInRibbonRule()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = RibbonRule.EntityLogicalName,

                ColumnSet = new ColumnSet(RibbonRule.Schema.Attributes.entity),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RibbonRule.Schema.Attributes.entity, ConditionOperator.NotNull),
                    },
                },

                Orders =
                {
                    new OrderExpression(RibbonRule.Schema.Attributes.entity, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    foreach (var item in coll.Entities.Select(e => e.ToEntity<RibbonRule>()))
                    {
                        if (!string.IsNullOrEmpty(item.Entity))
                        {
                            result.Add(item.Entity);
                        }
                    }

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

        private HashSet<string> GetEntitiesInRibbonTabToCommandMap()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = RibbonTabToCommandMap.EntityLogicalName,

                ColumnSet = new ColumnSet(RibbonTabToCommandMap.Schema.Attributes.entity),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RibbonTabToCommandMap.Schema.Attributes.entity, ConditionOperator.NotNull),
                    },
                },

                Orders =
                {
                    new OrderExpression(RibbonTabToCommandMap.Schema.Attributes.entity, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    foreach (var item in coll.Entities.Select(e => e.ToEntity<RibbonTabToCommandMap>()))
                    {
                        if (!string.IsNullOrEmpty(item.Entity))
                        {
                            result.Add(item.Entity);
                        }
                    }

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

        public Task ExportApplicationRibbon(RibbonLocationFilters filter, string filePath, CommonConfiguration commonConfig)
        {
            return Task.Run(() => ExportingApplicationRibbon(filter, filePath, commonConfig));
        }

        private void ExportingApplicationRibbon(RibbonLocationFilters filter, string filePath, CommonConfiguration commonConfig)
        {
            RetrieveApplicationRibbonRequest appribReq = new RetrieveApplicationRibbonRequest();
            RetrieveApplicationRibbonResponse appribResp = (RetrieveApplicationRibbonResponse)_service.Execute(appribReq);

            var byteXml = FileOperations.UnzipRibbon(appribResp.CompressedApplicationRibbonXml);

            if (commonConfig != null && commonConfig.ExportRibbonXmlXmlAttributeOnNewLine)
            {
                byteXml = ContentCoparerHelper.FormatXmlWithXmlAttributeOnNewLine(byteXml);
            }

            File.WriteAllBytes(filePath, byteXml);
        }

        public Task ExportEntityRibbon(string entityName, RibbonLocationFilters filter, string filePath, CommonConfiguration commonConfig)
        {
            return Task.Run(() => ExportingEntityRibbon(entityName, filter, filePath, commonConfig));
        }

        private void ExportingEntityRibbon(string entityName, RibbonLocationFilters filter, string filePath, CommonConfiguration commonConfig)
        {
            RetrieveEntityRibbonRequest entRibReq = new RetrieveEntityRibbonRequest()
            {
                RibbonLocationFilter = filter,
                EntityName = entityName,
            };

            RetrieveEntityRibbonResponse entRibResp = (RetrieveEntityRibbonResponse)_service.Execute(entRibReq);

            var byteXml = FileOperations.UnzipRibbon(entRibResp.CompressedEntityXml);

            if (commonConfig != null && commonConfig.ExportRibbonXmlXmlAttributeOnNewLine)
            {
                byteXml = ContentCoparerHelper.FormatXmlWithXmlAttributeOnNewLine(byteXml);
            }

            File.WriteAllBytes(filePath, byteXml);
        }

        public Task<RibbonCustomization> FindApplicationRibbonCustomizationAsync()
        {
            return Task.Run(() => FindApplicationRibbonCustomization());
        }

        public RibbonCustomization FindApplicationRibbonCustomization()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = RibbonCustomization.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RibbonCustomization.Schema.Attributes.entity, ConditionOperator.Null),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = RibbonCustomization.EntityLogicalName,
                        LinkFromAttributeName = RibbonCustomization.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.Schema.EntityLogicalName,
                        LinkToAttributeName = Solution.Schema.EntityPrimaryIdAttribute,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Equal, Solution.InstancesUniqueNames.Active),
                            },
                        },
                    },
                },

                Orders =
                {
                    new OrderExpression(RibbonCustomization.Schema.Attributes.publishedon, OrderType.Ascending),
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<RibbonCustomization>()).SingleOrDefault() : null;
        }
    }
}
