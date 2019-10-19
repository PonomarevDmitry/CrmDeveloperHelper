using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
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
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

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

            var list = _service.RetrieveMultipleAll<RibbonCustomization>(query);

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Entity))
                {
                    result.Add(item.Entity);
                }
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

            var list = _service.RetrieveMultipleAll<RibbonCommand>(query);

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Entity))
                {
                    result.Add(item.Entity);
                }
            }

            return result;
        }

        public Task<XDocument> GetEntityRibbonAsync(string entityName)
        {
            return Task.Run(() => GetEntityRibbon(entityName));
        }

        private XDocument GetEntityRibbon(string entityName)
        {
            byte[] arrayXml = null;

            if (!string.IsNullOrEmpty(entityName))
            {
                arrayXml = this.ExportEntityRibbonByteArray(entityName, RibbonLocationFilters.All);
            }
            else
            {
                arrayXml = this.ExportApplicationRibbonByteArray();
            }

            arrayXml = FileOperations.UnzipRibbon(arrayXml);

            XDocument doc = null;

            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(arrayXml, 0, arrayXml.Length);

                memStream.Position = 0;

                doc = XDocument.Load(memStream);
            }

            return doc;
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

            var list = _service.RetrieveMultipleAll<RibbonContextGroup>(query);

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Entity))
                {
                    result.Add(item.Entity);
                }
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

            var list = _service.RetrieveMultipleAll<RibbonDiff>(query);

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Entity))
                {
                    result.Add(item.Entity);
                }
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

            var list = _service.RetrieveMultipleAll<RibbonRule>(query);

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Entity))
                {
                    result.Add(item.Entity);
                }
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

            var list = _service.RetrieveMultipleAll<RibbonTabToCommandMap>(query);

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Entity))
                {
                    result.Add(item.Entity);
                }
            }

            return result;
        }

        public Task<string> ExportApplicationRibbonAsync()
        {
            return Task.Run(() => ExportingApplicationRibbon());
        }

        private string ExportingApplicationRibbon()
        {
            byte[] byteXml = ExportApplicationRibbonByteArray();

            byteXml = FileOperations.UnzipRibbon(byteXml);

            XElement doc = null;

            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(byteXml, 0, byteXml.Length);

                memStream.Position = 0;

                doc = XElement.Load(memStream);
            }

            return doc.ToString();
        }

        public Task<string> ExportEntityRibbonAsync(string entityName, RibbonLocationFilters filter)
        {
            return Task.Run(() => ExportingEntityRibbon(entityName, filter));
        }

        private string ExportingEntityRibbon(string entityName, RibbonLocationFilters filter)
        {
            byte[] byteXml = ExportEntityRibbonByteArray(entityName, filter);

            byteXml = FileOperations.UnzipRibbon(byteXml);

            XElement doc = null;

            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(byteXml, 0, byteXml.Length);

                memStream.Position = 0;

                doc = XElement.Load(memStream);
            }

            return doc.ToString();
        }

        public Task<byte[]> ExportApplicationRibbonByteArrayAsync()
        {
            return Task.Run(() => ExportApplicationRibbonByteArray());
        }

        private byte[] ExportApplicationRibbonByteArray()
        {
            RetrieveApplicationRibbonRequest appribReq = new RetrieveApplicationRibbonRequest();
            RetrieveApplicationRibbonResponse appribResp = (RetrieveApplicationRibbonResponse)_service.Execute(appribReq);

            return appribResp.CompressedApplicationRibbonXml;
        }

        public Task<byte[]> ExportEntityRibbonByteArrayAsync(string entityName, RibbonLocationFilters filter)
        {
            return Task.Run(() => ExportEntityRibbonByteArray(entityName, filter));
        }

        private byte[] ExportEntityRibbonByteArray(string entityName, RibbonLocationFilters filter)
        {
            RetrieveEntityRibbonRequest entRibReq = new RetrieveEntityRibbonRequest()
            {
                RibbonLocationFilter = filter,
                EntityName = entityName,
            };

            RetrieveEntityRibbonResponse entRibResp = (RetrieveEntityRibbonResponse)_service.Execute(entRibReq);

            return entRibResp.CompressedEntityXml;
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
                                new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Equal, Solution.Schema.InstancesUniqueNames.Active),
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

        public static Task<bool> ValidateXmlDocumentAsync(ConnectionData connectionData, IWriteToOutput iWriteToOutput, XDocument doc)
        {
            return Task.Run(() => ValidateXmlDocument(connectionData, iWriteToOutput, doc));
        }

        private static bool ValidateXmlDocument(ConnectionData connectionData, IWriteToOutput iWriteToOutput, XDocument doc)
        {
            ContentCoparerHelper.ClearRoot(doc);

            XmlSchemaSet schemas = new XmlSchemaSet();

            {
                var schemasResources = AbstractDynamicCommandXsdSchemas.GetXsdSchemas(AbstractDynamicCommandXsdSchemas.SchemaRibbonXml);

                if (schemasResources != null)
                {
                    foreach (var fileName in schemasResources)
                    {
                        Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        using (StreamReader reader = new StreamReader(info.Stream))
                        {
                            schemas.Add(string.Empty, XmlReader.Create(reader));
                        }
                    }
                }
            }

            List<ValidationEventArgs> errors = new List<ValidationEventArgs>();

            doc.Validate(schemas, (o, e) =>
            {
                errors.Add(e);
            });

            if (errors.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.TextIsNotValidForFieldFormat1, AbstractDynamicCommandXsdSchemas.SchemaRibbonXml);

                foreach (var item in errors)
                {
                    iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.XmlValidationMessageFormat2, item.Severity, item.Message);
                    iWriteToOutput.WriteErrorToOutput(connectionData, item.Exception);
                }

                iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return errors.Count == 0;
        }

        public async Task PerformUpdateRibbonDiffXml(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, XDocument doc, EntityMetadata entityMetadata, RibbonCustomization ribbonCustomization)
        {
            if (entityMetadata == null && ribbonCustomization == null)
            {
                throw new ArgumentException("entityMetadata or ribbonCustomization");
            }

            ContentCoparerHelper.ClearRoot(doc);

            Publisher publisherDefault = null;

            {
                var repositoryPublisher = new PublisherRepository(_service);
                publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                if (publisherDefault == null)
                {
                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.NotFoundedDefaultPublisher);
                    iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                    return;
                }
            }

            var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());
            solutionUniqueName = solutionUniqueName.Replace("-", "_");

            var solution = new Solution()
            {
                UniqueName = solutionUniqueName,
                FriendlyName = solutionUniqueName,

                Description = "Temporary solution for exporting RibbonDiffXml.",

                PublisherId = publisherDefault.ToEntityReference(),

                Version = "1.0.0.0",
            };

            iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.CreatingNewSolutionFormat1, solutionUniqueName);

            solution.Id = await _service.CreateAsync(solution);

            iWriteToOutput.WriteToOutputSolutionUri(_service.ConnectionData, solution.UniqueName, solution.Id);

            try
            {
                if (entityMetadata != null)
                {

                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.AddingInSolutionEntityFormat3, _service.ConnectionData.Name, solutionUniqueName, entityMetadata.LogicalName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(_service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = entityMetadata.MetadataId.Value,
                            RootComponentBehaviorEnum =  SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0,
                        }});
                    }

                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.ExportingSolutionAndExtractingRibbonDiffXmlForEntityFormat2, solutionUniqueName, entityMetadata.LogicalName);
                }
                else if (ribbonCustomization != null)
                {
                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.AddingInSolutionApplicationRibbonFormat2, _service.ConnectionData.Name, solutionUniqueName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(_service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                            ObjectId = ribbonCustomization.Id,
                            RootComponentBehaviorEnum =  SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0,
                        }});
                    }

                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.ExportingSolutionAndExtractingApplicationRibbonDiffXmlFormat1, solutionUniqueName);
                }



                var repository = new ExportSolutionHelper(_service);

                var solutionBodyBinary = await repository.ExportSolutionAndGetBodyBinaryAsync(solutionUniqueName);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(_service.ConnectionData.Name, solution.UniqueName, "Solution Backup", "zip");

                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, solutionBodyBinary);

                    iWriteToOutput.WriteToOutput(_service.ConnectionData, "Solution {0} Backup exported to {1}", solution.UniqueName, filePath);

                    iWriteToOutput.WriteToOutputFilePathUri(_service.ConnectionData, filePath);
                }




                string ribbonDiffXml = string.Empty;

                if (entityMetadata != null)
                {
                    ribbonDiffXml = ExportSolutionHelper.GetRibbonDiffXmlForEntityFromSolutionBody(entityMetadata.LogicalName, solutionBodyBinary);
                }
                else if (ribbonCustomization != null)
                {
                    ribbonDiffXml = ExportSolutionHelper.GetApplicationRibbonDiffXmlFromSolutionBody(solutionBodyBinary);
                }

                ribbonDiffXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonDiffXml, commonConfig, XmlOptionsControls.RibbonFull
                    , schemaName: AbstractDynamicCommandXsdSchemas.SchemaRibbonXml
                    , ribbonEntityName: entityMetadata?.LogicalName ?? string.Empty
                    );

                {
                    string filePath = string.Empty;

                    if (entityMetadata != null)
                    {
                        string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(_service.ConnectionData.Name, entityMetadata.LogicalName, "BackUp", "xml");
                        filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));
                        iWriteToOutput.WriteToOutput(_service.ConnectionData, "{0} RibbonDiffXml BackUp exported to {1}", entityMetadata.LogicalName, filePath);
                    }
                    else if (ribbonCustomization != null)
                    {
                        string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(_service.ConnectionData.Name, "BackUp", "xml");
                        filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));
                        iWriteToOutput.WriteToOutput(_service.ConnectionData, "Application RibbonDiffXml BackUp exported to {0}", filePath);
                    }

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    iWriteToOutput.WriteToOutputFilePathUri(_service.ConnectionData, filePath);
                }

                if (entityMetadata != null)
                {
                    solutionBodyBinary = ExportSolutionHelper.ReplaceRibbonDiffXmlForEntityInSolutionBody(entityMetadata.LogicalName, solutionBodyBinary, doc.Root);
                }
                else if (ribbonCustomization != null)
                {
                    solutionBodyBinary = ExportSolutionHelper.ReplaceApplicationRibbonDiffXmlInSolutionBody(solutionBodyBinary, doc.Root);
                }

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(_service.ConnectionData.Name, solution.UniqueName, "Changed Solution Backup", "zip");

                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, solutionBodyBinary);

                    iWriteToOutput.WriteToOutput(_service.ConnectionData, "Changed Solution {0} Backup exported to {1}", solution.UniqueName, filePath);

                    iWriteToOutput.WriteToOutputFilePathUri(_service.ConnectionData, filePath);
                }

                iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.ImportingSolutionFormat1, solutionUniqueName);

                await repository.ImportSolutionAsync(solutionBodyBinary);

                await DeleteSolution(iWriteToOutput, solution);

                {
                    var repositoryPublish = new PublishActionsRepository(_service);

                    if (entityMetadata != null)
                    {
                        iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.PublishingEntitiesFormat2, _service.ConnectionData.Name, entityMetadata.LogicalName);

                        await repositoryPublish.PublishEntitiesAsync(new[] { entityMetadata.LogicalName });
                    }
                    else if (ribbonCustomization != null)
                    {
                        iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.PublishingApplicationRibbonFormat1, _service.ConnectionData.Name);

                        await repositoryPublish.PublishApplicationRibbonAsync();
                    }
                }
            }
            finally
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                DeleteSolution(iWriteToOutput, solution);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        public async Task<string> GetRibbonDiffXmlAsync(IWriteToOutput iWriteToOutput, EntityMetadata entityMetadata, RibbonCustomization ribbonCustomization)
        {
            if (entityMetadata == null && ribbonCustomization == null)
            {
                throw new ArgumentException("entityMetadata or ribbonCustomization");
            }

            Publisher publisherDefault = null;

            {
                var repositoryPublisher = new PublisherRepository(_service);
                publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                if (publisherDefault == null)
                {
                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.NotFoundedDefaultPublisher);
                    iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                    return null;
                }
            }

            var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());
            solutionUniqueName = solutionUniqueName.Replace("-", "_");

            var solution = new Solution()
            {
                UniqueName = solutionUniqueName,
                FriendlyName = solutionUniqueName,

                Description = "Temporary solution for exporting RibbonDiffXml.",

                PublisherId = publisherDefault.ToEntityReference(),

                Version = "1.0.0.0",
            };

            iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.CreatingNewSolutionFormat1, solutionUniqueName);

            solution.Id = await _service.CreateAsync(solution);

            iWriteToOutput.WriteToOutputSolutionUri(_service.ConnectionData, solution.UniqueName, solution.Id);

            try
            {
                if (entityMetadata != null)
                {
                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.AddingInSolutionEntityFormat3, _service.ConnectionData.Name, solutionUniqueName, entityMetadata.LogicalName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(_service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.Entity),
                                ObjectId = entityMetadata.MetadataId.Value,
                                RootComponentBehaviorEnum =  SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0,
                            }});
                    }

                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.ExportingSolutionAndExtractingRibbonDiffXmlForEntityFormat2, solutionUniqueName, entityMetadata.LogicalName);
                }
                else if (ribbonCustomization != null)
                {
                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.AddingInSolutionApplicationRibbonFormat2, _service.ConnectionData.Name, solutionUniqueName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(_service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                            ObjectId = ribbonCustomization.Id,
                            RootComponentBehaviorEnum =  SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0,
                        }});
                    }

                    iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.ExportingSolutionAndExtractingApplicationRibbonDiffXmlFormat1, solutionUniqueName);
                }

                var repository = new ExportSolutionHelper(_service);

                var solutionBodyBinary = await repository.ExportSolutionAndGetBodyBinaryAsync(solutionUniqueName);

                string ribbonDiffXml = null;

                if (entityMetadata != null)
                {
                    ribbonDiffXml = ExportSolutionHelper.GetRibbonDiffXmlForEntityFromSolutionBody(entityMetadata.LogicalName, solutionBodyBinary);
                }
                else if (ribbonCustomization != null)
                {
                    ribbonDiffXml = ExportSolutionHelper.GetApplicationRibbonDiffXmlFromSolutionBody(solutionBodyBinary);
                }

                return ribbonDiffXml;
            }
            finally
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                DeleteSolution(iWriteToOutput, solution);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private async Task DeleteSolution(IWriteToOutput iWriteToOutput, Solution solution)
        {
            if (solution.Id == Guid.Empty)
            {
                return;
            }

            try
            {
                iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.DeletingSolutionFormat1, solution.UniqueName);
                await _service.DeleteAsync(solution.LogicalName, solution.Id);
                solution.Id = Guid.Empty;
            }
            catch (Exception ex)
            {
                iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex, Properties.OutputStrings.DeletingSolutionFailedFormat1, solution.UniqueName);
            }
        }
    }
}