using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public sealed class FindsController
    {
        private const string tabSpacer = "    ";

        private readonly IWriteToOutput _iWriteToOutput = null;

        public FindsController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Find components with prefix

        public async Task ExecuteFindingEntityObjectsByPrefix(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            prefix = prefix.TrimEnd(' ', '_').Trim();
            prefix = string.Format("{0}_", prefix);

            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsNamesForPrefixFormat2, connectionData?.Name, prefix);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindingEntityObjectsByPrefix(connectionData, commonConfig, prefix);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindingEntityObjectsByPrefix(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            List<SolutionComponent> wrongEntityNames = new List<SolutionComponent>();
            List<SolutionComponent> wrongEntityAttributes = new List<SolutionComponent>();
            List<SolutionComponent> wrongEntityRelationshipsManyToOne = new List<SolutionComponent>();
            List<SolutionComponent> wrongEntityRelationshipsManyToMany = new List<SolutionComponent>();

            List<SolutionComponent> wrongWebResourceNames = new List<SolutionComponent>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    if (currentEntity.LogicalName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        wrongEntityNames.Add(new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = currentEntity.MetadataId,
                        });
                    }

                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (string.IsNullOrEmpty(currentAttribute.AttributeOf))
                        {
                            if (currentAttribute.LogicalName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                            {
                                wrongEntityAttributes.Add(new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId,
                                });
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            wrongEntityRelationshipsManyToOne.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            wrongEntityRelationshipsManyToMany.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }
                }
            }

            {
                WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingWebResources);

                var coll = await repositoryWebResource.GetListAllAsync(string.Empty, new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.displayname, WebResource.Schema.Attributes.webresourcetype));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingWebResources);

                foreach (var webResource in coll)
                {
                    if (webResource.Name.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        wrongEntityRelationshipsManyToMany.Add(new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                            ObjectId = webResource.Id,
                        });
                    }
                }
            }

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            await WriteToContentList(descriptor, wrongEntityNames, content, Properties.OutputStrings.EntityNamesWithPrefixFormat2, prefix);

            await WriteToContentList(descriptor, wrongEntityAttributes, content, Properties.OutputStrings.EntityAttributesNamesWithPrefixFormat2, prefix);

            await WriteToContentList(descriptor, wrongEntityRelationshipsManyToOne, content, Properties.OutputStrings.ManyToOneRelationshipsNamesWithPrefixFormat2, prefix);

            await WriteToContentList(descriptor, wrongEntityRelationshipsManyToMany, content, Properties.OutputStrings.ManyToManyRelationshipsNamesWithPrefixFormat2, prefix);

            await WriteToContentList(descriptor, wrongWebResourceNames, content, Properties.OutputStrings.WebResourcesWithPrefixFormat2, prefix);

            int totalErrors =
                wrongEntityNames.Count
                + wrongEntityAttributes.Count
                + wrongEntityRelationshipsManyToOne.Count
                + wrongEntityRelationshipsManyToMany.Count
                + wrongWebResourceNames.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat(Properties.OutputStrings.NoObjectsInCRMFoundedWithPrefixFormat1, prefix).AppendLine();
            }

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string fileName = EntityFileNameFormatter.GetCheckEntityNamesForPrefixFileName(connectionData.Name, prefix);

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Find components with prefix

        #region Find components with prefix and show in Explorer

        public async Task ExecuteFindingEntityObjectsByPrefixInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            prefix = prefix.TrimEnd(' ', '_').Trim();
            prefix = string.Format("{0}_", prefix);

            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsNamesForPrefixInExplorerFormat2, connectionData?.Name, prefix);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindingEntityObjectsByPrefixInExplorer(connectionData, commonConfig, prefix);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindingEntityObjectsByPrefixInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            List<SolutionComponent> wrongElements = new List<SolutionComponent>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    if (currentEntity.LogicalName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        wrongElements.Add(new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = currentEntity.MetadataId,
                        });
                    }

                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (string.IsNullOrEmpty(currentAttribute.AttributeOf))
                        {
                            if (currentAttribute.LogicalName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                            {
                                wrongElements.Add(new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId,
                                });
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            wrongElements.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            wrongElements.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }
                }
            }

            {
                WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingWebResources);

                var coll = await repositoryWebResource.GetListAllAsync(string.Empty, new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.displayname, WebResource.Schema.Attributes.webresourcetype));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingWebResources);

                foreach (var webResource in coll)
                {
                    if (webResource.Name.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        wrongElements.Add(new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                            ObjectId = webResource.Id,
                        });
                    }
                }
            }

            if (wrongElements.Count == 0)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMFoundedWithPrefixFormat1, prefix);
                return;
            }

            string name = string.Format("Components with prefix '{0}'", prefix);

            WindowHelper.OpenExplorerComponentsExplorer(_iWriteToOutput, service, null, commonConfig, wrongElements, null, name, null);
        }

        #endregion Find components with prefix and show in Explorer

        #region Find components with prefix and show dependent components

        public async Task ExecuteFindingEntityObjectsByPrefixAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            prefix = prefix.TrimEnd(' ', '_');
            prefix = string.Format("{0}_", prefix);

            string operation = string.Format(Properties.OperationNames.CheckingCRMObjectsNamesForPrefixAndShowDependentComponentsFormat2, connectionData?.Name, prefix);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindingEntityObjectsByPrefixAndShowDependentComponents(connectionData, commonConfig, prefix);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindingEntityObjectsByPrefixAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            List<SolutionComponent> wrongEntityNames = new List<SolutionComponent>();
            List<SolutionComponent> wrongEntityAttributes = new List<SolutionComponent>();
            List<SolutionComponent> wrongEntityRelationshipsManyToOne = new List<SolutionComponent>();
            List<SolutionComponent> wrongEntityRelationshipsManyToMany = new List<SolutionComponent>();

            List<SolutionComponent> wrongWebResourceNames = new List<SolutionComponent>();

            Dictionary<SolutionComponent, string> dictEntityNames = new Dictionary<SolutionComponent, string>();
            Dictionary<SolutionComponent, string> dictEntityAttributes = new Dictionary<SolutionComponent, string>();
            Dictionary<SolutionComponent, string> dictEntityRelationshipsManyToOne = new Dictionary<SolutionComponent, string>();
            Dictionary<SolutionComponent, string> dictEntityRelationshipsManyToMany = new Dictionary<SolutionComponent, string>();

            Dictionary<SolutionComponent, string> dictWebResourceNames = new Dictionary<SolutionComponent, string>();

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            DependencyRepository dependencyRepository = new DependencyRepository(service);

            DependencyDescriptionHandler descriptorHandler = new DependencyDescriptionHandler(descriptor);

            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes.OrderBy(a => a.LogicalName))
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            if (currentAttribute.LogicalName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var component = new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId.Value,
                                };

                                wrongEntityAttributes.Add(component);

                                var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Attribute, currentAttribute.MetadataId.Value);

                                var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                                if (!string.IsNullOrEmpty(desc))
                                {
                                    dictEntityAttributes.Add(component, desc);
                                }
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity
                        .ManyToOneRelationships
                        .OrderBy(a => a.ReferencingEntity)
                        .ThenBy(a => a.ReferencingAttribute)
                        .ThenBy(a => a.SchemaName)
                    )
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var component = new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId.Value,
                            };

                            wrongEntityRelationshipsManyToOne.Add(component);

                            var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.EntityRelationship, currentRelationship.MetadataId.Value);

                            var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                            if (!string.IsNullOrEmpty(desc))
                            {
                                dictEntityRelationshipsManyToOne.Add(component, desc);
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity
                        .ManyToManyRelationships
                        .OrderBy(a => a.Entity1LogicalName)
                        .ThenBy(a => a.Entity2LogicalName)
                        .ThenBy(a => a.Entity1IntersectAttribute)
                        .ThenBy(a => a.Entity2IntersectAttribute)
                        .ThenBy(a => a.SchemaName)
                    )
                    {
                        if (currentRelationship.SchemaName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var component = new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId.Value,
                            };

                            wrongEntityRelationshipsManyToMany.Add(component);

                            var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.EntityRelationship, currentRelationship.MetadataId.Value);

                            var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                            if (!string.IsNullOrEmpty(desc))
                            {
                                dictEntityRelationshipsManyToMany.Add(component, desc);
                            }
                        }
                    }

                    var wrongEntity = currentEntity.LogicalName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase);
                    if (wrongEntity)
                    {
                        var component = new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = currentEntity.MetadataId.Value,
                        };

                        wrongEntityNames.Add(component);

                        var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Entity, currentEntity.MetadataId.Value);

                        var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            dictEntityNames.Add(component, desc);
                        }
                    }
                }
            }

            {
                WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingWebResources);

                var webResources = await repositoryWebResource.GetListAllAsync(null, new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.displayname, WebResource.Schema.Attributes.webresourcetype));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingWebResources);

                foreach (var webResource in webResources)
                {
                    if (webResource.Name.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var component = new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                            ObjectId = webResource.Id,
                        };

                        wrongWebResourceNames.Add(component);

                        var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.WebResource, webResource.Id);

                        var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            dictWebResourceNames.Add(component, desc);
                        }
                    }
                }
            }

            await WriteToContentList(descriptor, wrongEntityNames, content, Properties.OutputStrings.EntityNamesWithPrefixFormat2, prefix);

            await WriteToContentList(descriptor, wrongEntityAttributes, content, Properties.OutputStrings.EntityAttributesNamesWithPrefixFormat2, prefix);

            await WriteToContentList(descriptor, wrongEntityRelationshipsManyToOne, content, Properties.OutputStrings.ManyToOneRelationshipsNamesWithPrefixFormat2, prefix);

            await WriteToContentList(descriptor, wrongEntityRelationshipsManyToMany, content, Properties.OutputStrings.ManyToManyRelationshipsNamesWithPrefixFormat2, prefix);

            await WriteToContentList(descriptor, wrongWebResourceNames, content, Properties.OutputStrings.WebResourcesWithPrefixFormat2, prefix);

            WriteToContentDictionary(descriptor, content, wrongEntityNames, dictEntityNames, Properties.OutputStrings.EntityNamesWithPrefixFormat2, prefix);

            WriteToContentDictionary(descriptor, content, wrongEntityAttributes, dictEntityAttributes, Properties.OutputStrings.EntityAttributesNamesWithPrefixFormat2, prefix);

            WriteToContentDictionary(descriptor, content, wrongEntityRelationshipsManyToOne, dictEntityRelationshipsManyToOne, Properties.OutputStrings.ManyToOneRelationshipsNamesWithPrefixFormat2, prefix);

            WriteToContentDictionary(descriptor, content, wrongEntityRelationshipsManyToMany, dictEntityRelationshipsManyToMany, Properties.OutputStrings.ManyToManyRelationshipsNamesWithPrefixFormat2, prefix);

            WriteToContentDictionary(descriptor, content, wrongWebResourceNames, dictWebResourceNames, Properties.OutputStrings.WebResourcesWithPrefixFormat2, prefix);

            int totalErrors = dictEntityNames.Count
                + dictEntityAttributes.Count
                + dictEntityRelationshipsManyToOne.Count
                + dictEntityRelationshipsManyToMany.Count
                + dictWebResourceNames.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat("No Objects in CRM founded with prefix '{0}'.", prefix).AppendLine();
            }

            string fileName = string.Format("{0}.CRM Objects names for prefix '{1}' and show dependent components at {2}.txt", connectionData.Name, prefix, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, "Created file with CRM Objects names for prefix '{0}' and show dependent components: {1}", prefix, filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Find components with prefix and show dependent components

        private bool IsMakedToDelete(string prefix, string logicalName, Label label)
        {
            if (logicalName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            if (label != null)
            {
                foreach (var item in label.LocalizedLabels)
                {
                    if (item.Label.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #region Finding Marked to Delelete and Show Dependent.

        public async Task ExecuteFindingMarkedToDeleteAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string deleteMark)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsMarkedToDeleteByAndShowDependentComponentsFormat2, connectionData?.Name, deleteMark);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindingMarkedToDeleteAndShowDependentComponents(connectionData, commonConfig, deleteMark);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindingMarkedToDeleteAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string deleteMark)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            List<SolutionComponent> wrongEntityNames = new List<SolutionComponent>();
            List<SolutionComponent> wrongEntityAttributes = new List<SolutionComponent>();

            List<SolutionComponent> wrongWebResourceNames = new List<SolutionComponent>();

            Dictionary<SolutionComponent, string> dictEntityNames = new Dictionary<SolutionComponent, string>();
            Dictionary<SolutionComponent, string> dictEntityAttributes = new Dictionary<SolutionComponent, string>();

            Dictionary<SolutionComponent, string> dictWebResourceNames = new Dictionary<SolutionComponent, string>();

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var dependencyRepository = new DependencyRepository(service);

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities.OrderBy(e => e.LogicalName))
                {
                    foreach (var currentAttribute in currentEntity.Attributes.OrderBy(a => a.LogicalName))
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            bool marked = IsMakedToDelete(deleteMark, currentAttribute.LogicalName, currentAttribute.DisplayName);

                            if (marked)
                            {
                                var component = new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId.Value,
                                };

                                wrongEntityAttributes.Add(component);

                                var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Attribute, currentAttribute.MetadataId.Value);

                                var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                                if (!string.IsNullOrEmpty(desc))
                                {
                                    dictEntityAttributes.Add(component, desc);
                                }
                            }
                        }
                    }

                    var wrongEntity = IsMakedToDelete(deleteMark, currentEntity.LogicalName, currentEntity.DisplayName);
                    if (wrongEntity)
                    {
                        var component = new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = currentEntity.MetadataId.Value,
                        };

                        wrongEntityNames.Add(component);

                        var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.Entity, currentEntity.MetadataId.Value);

                        var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            dictEntityNames.Add(component, desc);
                        }
                    }
                }
            }

            {
                WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingWebResources);

                var collWebResources = await repositoryWebResource.GetListAllAsync(string.Empty, new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.displayname, WebResource.Schema.Attributes.webresourcetype));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingWebResources);

                foreach (var webResource in collWebResources)
                {
                    if (!string.IsNullOrEmpty(webResource.DisplayName)
                        && webResource.DisplayName.StartsWith(deleteMark, StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        var component = new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                            ObjectId = webResource.Id,
                        };

                        wrongWebResourceNames.Add(component);

                        var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.WebResource, webResource.Id);

                        var desc = await descriptorHandler.GetDescriptionDependentAsync(coll);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            dictWebResourceNames.Add(component, desc);
                        }
                    }
                }
            }

            await WriteToContentList(descriptor, wrongEntityNames, content, Properties.OutputStrings.EntityNamesMarkedToDeleteFormat2, deleteMark);

            await WriteToContentList(descriptor, wrongEntityAttributes, content, Properties.OutputStrings.EntityAttributesNamesMarkedToDeleteFormat2, deleteMark);

            await WriteToContentList(descriptor, wrongWebResourceNames, content, Properties.OutputStrings.WebResourcesMarkedToDeleteFormat2, deleteMark);

            WriteToContentDictionary(descriptor, content, wrongEntityNames, dictEntityNames, Properties.OutputStrings.EntityNamesMarkedToDeleteFormat2, deleteMark);

            WriteToContentDictionary(descriptor, content, wrongEntityAttributes, dictEntityAttributes, Properties.OutputStrings.EntityAttributesNamesMarkedToDeleteFormat2, deleteMark);

            WriteToContentDictionary(descriptor, content, wrongWebResourceNames, dictWebResourceNames, Properties.OutputStrings.WebResourcesMarkedToDeleteFormat2, deleteMark);

            int totalErrors = dictEntityAttributes.Count
                + dictEntityNames.Count
                + dictWebResourceNames.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat(Properties.OutputStrings.NoObjectsInCRMFoundedMarkedToDeleteFormat1, deleteMark).AppendLine();
            }

            string fileName = string.Format("{0}.CRM Objects marked to delete by '{1}' and show dependent components at {2}.txt", connectionData.Name, deleteMark, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, "Created file with CRM Objects marked to delete by '{0}' and show dependent components: {1}", deleteMark, filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Finding Marked to Delelete and Show Dependent.

        #region Finding Marked to Delelete in Explorer.

        public async Task ExecuteFindingMarkedToDeleteInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string deleteMark)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsMarkedToDeleteInExplorerFormat2, connectionData?.Name, deleteMark);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindingMarkedToDeleteInExplorer(connectionData, commonConfig, deleteMark);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindingMarkedToDeleteInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string deleteMark)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            List<SolutionComponent> wrongElements = new List<SolutionComponent>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities.OrderBy(e => e.LogicalName))
                {
                    foreach (var currentAttribute in currentEntity.Attributes.OrderBy(a => a.LogicalName))
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            bool marked = IsMakedToDelete(deleteMark, currentAttribute.LogicalName, currentAttribute.DisplayName);

                            if (marked)
                            {
                                var component = new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId.Value,
                                };

                                wrongElements.Add(component);
                            }
                        }
                    }

                    var wrongEntity = IsMakedToDelete(deleteMark, currentEntity.LogicalName, currentEntity.DisplayName);
                    if (wrongEntity)
                    {
                        var component = new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = currentEntity.MetadataId.Value,
                        };

                        wrongElements.Add(component);
                    }
                }
            }

            {
                WebResourceRepository repositoryWebResource = new WebResourceRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingWebResources);

                var collWebResources = await repositoryWebResource.GetListAllAsync(string.Empty, new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.displayname, WebResource.Schema.Attributes.webresourcetype));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingWebResources);

                foreach (var webResource in collWebResources)
                {
                    if (!string.IsNullOrEmpty(webResource.DisplayName)
                        && webResource.DisplayName.StartsWith(deleteMark, StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        var component = new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.WebResource),
                            ObjectId = webResource.Id,
                        };

                        wrongElements.Add(component);
                    }
                }
            }

            if (wrongElements.Count == 0)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMFoundedMarkedToDeleteFormat1, deleteMark);
                return;
            }

            string name = string.Format("Components marked to delete by '{0}'", deleteMark);

            WindowHelper.OpenExplorerComponentsExplorer(_iWriteToOutput, service, null, commonConfig, wrongElements, null, name, null);
        }

        #endregion Finding Marked to Delelete in Explorer.

        #region Finding components with name

        public async Task ExecuteFindEntityElementsByName(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsByNameFormat2, connectionData?.Name, name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindindEntityElementsByName(connectionData, commonConfig, name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindindEntityElementsByName(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            List<SolutionComponent> listEntityAttributes = new List<SolutionComponent>();
            List<SolutionComponent> listEntityRelationshipsManyToOne = new List<SolutionComponent>();
            List<SolutionComponent> listEntityRelationshipsManyToMany = new List<SolutionComponent>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (string.IsNullOrEmpty(currentAttribute.AttributeOf))
                        {
                            if (string.Equals(currentAttribute.LogicalName, name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                listEntityAttributes.Add(new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId,
                                });
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (string.Equals(currentRelationship.SchemaName, name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            listEntityRelationshipsManyToOne.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (string.Equals(currentRelationship.SchemaName, name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            listEntityRelationshipsManyToMany.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }
                }
            }

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            await WriteToContentList(descriptor, listEntityAttributes, content, "Entity Attributes names with name '" + name + "': {0}");

            await WriteToContentList(descriptor, listEntityRelationshipsManyToOne, content, "Many to One Relationships names with name '" + name + "': {0}");

            await WriteToContentList(descriptor, listEntityRelationshipsManyToMany, content, "Many to Many Relationships names with name '" + name + "': {0}");

            int totalErrors =
                listEntityAttributes.Count
                + listEntityRelationshipsManyToOne.Count
                + listEntityRelationshipsManyToMany.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat("No Objects in CRM founded with name '{0}'.", name).AppendLine();
            }

            string fileName = string.Format("{0}.Finding CRM Objects names for {1} at {2}.txt"
                , connectionData.Name
                , name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
            );

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Finding components with name

        #region Finding components with name in Explorer

        public async Task ExecuteFindEntityElementsByNameInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsByNameInExplorerFormat2, connectionData?.Name, name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindindEntityElementsByNameInExplorer(connectionData, commonConfig, name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindindEntityElementsByNameInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            List<SolutionComponent> listComponents = new List<SolutionComponent>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (string.IsNullOrEmpty(currentAttribute.AttributeOf))
                        {
                            if (string.Equals(currentAttribute.LogicalName, name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                listComponents.Add(new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId,
                                });
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (string.Equals(currentRelationship.SchemaName, name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            listComponents.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (string.Equals(currentRelationship.SchemaName, name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            listComponents.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }
                }
            }

            if (listComponents.Count == 0)
            {
                _iWriteToOutput.WriteToOutput(connectionData, "No Entity Objects in CRM founded with name '{0}'.", name);
                return;
            }

            string nameWindow = string.Format("Components with name '{0}'", name);

            WindowHelper.OpenExplorerComponentsExplorer(_iWriteToOutput, service, null, commonConfig, listComponents, null, nameWindow, null);
        }

        #endregion Finding components with name in Explorer

        #region Finding components with name contains string

        public async Task ExecuteFindEntityElementsContainsString(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectscontainsNameFormat2, connectionData?.Name, name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindindEntityElementsContainsString(connectionData, commonConfig, name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindindEntityElementsContainsString(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            List<SolutionComponent> listEntityAttributes = new List<SolutionComponent>();
            List<SolutionComponent> listEntityRelationshipsManyToOne = new List<SolutionComponent>();
            List<SolutionComponent> listEntityRelationshipsManyToMany = new List<SolutionComponent>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            if (Regex.IsMatch(currentAttribute.LogicalName, name, RegexOptions.IgnoreCase))
                            {
                                listEntityAttributes.Add(new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId,
                                });
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (Regex.IsMatch(currentRelationship.SchemaName, name, RegexOptions.IgnoreCase))
                        {
                            listEntityRelationshipsManyToOne.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (Regex.IsMatch(currentRelationship.SchemaName, name, RegexOptions.IgnoreCase))
                        {
                            listEntityRelationshipsManyToMany.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }
                }
            }

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.WithUrls = true;
            descriptor.WithManagedInfo = true;
            descriptor.WithSolutionsInfo = true;

            await WriteToContentList(descriptor, listEntityAttributes, content, "Entity Attributes names contains '" + name + "': {0}");

            await WriteToContentList(descriptor, listEntityRelationshipsManyToOne, content, "Many to One Relationships names contains '" + name + "': {0}");

            await WriteToContentList(descriptor, listEntityRelationshipsManyToMany, content, "Many to Many Relationships names contains '" + name + "': {0}");

            int totalErrors =
                listEntityAttributes.Count
                + listEntityRelationshipsManyToOne.Count
                + listEntityRelationshipsManyToMany.Count
                ;

            if (totalErrors == 0)
            {
                content.AppendLine();
                content.AppendFormat("No Objects in CRM founded that contains '{0}'.", name).AppendLine();
            }
            string fileName = string.Format("{0}.Finding CRM Objects names contains {1} at {2}.txt"
                , connectionData.Name
                , name
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
            );

            if (string.IsNullOrEmpty(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #endregion Finding components with name contains string

        #region Finding components with name contains string in Explorer

        public async Task ExecuteFindEntityElementsContainsStringInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectscontainsNameInExplorerFormat2, connectionData?.Name, name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindindEntityElementsContainsStringInExplorer(connectionData, commonConfig, name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindindEntityElementsContainsStringInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            List<SolutionComponent> listComponents = new List<SolutionComponent>();

            {
                EntityMetadataRepository repositoryEntity = new EntityMetadataRepository(service);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadata);

                var allEntities = await repositoryEntity.GetEntitiesWithAttributesAndRelationshipsAsync();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CheckingEntitiesMetadata);

                foreach (EntityMetadata currentEntity in allEntities)
                {
                    foreach (var currentAttribute in currentEntity.Attributes)
                    {
                        if (currentAttribute.AttributeOf == null)
                        {
                            if (Regex.IsMatch(currentAttribute.LogicalName, name, RegexOptions.IgnoreCase))
                            {
                                listComponents.Add(new SolutionComponent()
                                {
                                    ComponentType = new OptionSetValue((int)ComponentType.Attribute),
                                    ObjectId = currentAttribute.MetadataId,
                                });
                            }
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToOneRelationships)
                    {
                        if (Regex.IsMatch(currentRelationship.SchemaName, name, RegexOptions.IgnoreCase))
                        {
                            listComponents.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }

                    foreach (var currentRelationship in currentEntity.ManyToManyRelationships)
                    {
                        if (Regex.IsMatch(currentRelationship.SchemaName, name, RegexOptions.IgnoreCase))
                        {
                            listComponents.Add(new SolutionComponent()
                            {
                                ComponentType = new OptionSetValue((int)ComponentType.EntityRelationship),
                                ObjectId = currentRelationship.MetadataId,
                            });
                        }
                    }
                }
            }

            if (listComponents.Count == 0)
            {
                _iWriteToOutput.WriteToOutput(connectionData, "No Objects in CRM founded that contains '{0}'.", name);

                return;
            }

            string nameWindow = string.Format("Components with name '{0}'", name);

            WindowHelper.OpenExplorerComponentsExplorer(_iWriteToOutput, service, null, commonConfig, listComponents, null, nameWindow, null);
        }

        #endregion Finding components with name contains string in Explorer

        #region Поиск элементов по идентификатору.

        public async Task ExecuteFindEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsByIdEntityNameEntityTypeCodeFormat4, connectionData?.Name, entityId, entityName, entityTypeCode);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            EntityMetadataRepository repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.GetEntitiesPropertiesAsync(entityName, entityTypeCode, "LogicalName", "PrimaryIdAttribute", "IsIntersect", "Attributes");

            bool finded = false;

            foreach (var item in entityMetadataList.OrderBy(e => e.LogicalName))
            {
                var primaryAttr = item.Attributes.FirstOrDefault(a => string.Equals(a.LogicalName, item.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                if (primaryAttr != null && primaryAttr.AttributeType == AttributeTypeCode.Uniqueidentifier)
                {
                    var generalRepository = new GenericRepository(service, item);

                    Entity entity = await generalRepository.GetEntityByIdAsync(entityId, new ColumnSet(true));

                    if (entity != null)
                    {
                        finded = true;

                        content
                            .AppendLine()
                            .AppendLine()
                            .AppendLine(new string('-', 150))
                            .AppendLine()
                            .AppendLine()
                            .AppendLine(await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, null, connectionData))
                            ;
                    }
                }
            }

            if (finded)
            {
                string fileName = EntityFileNameFormatter.GetFindingCRMObjectsByIdFileName(connectionData.Name, entityId);

                if (string.IsNullOrEmpty(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }
                else if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }
        }

        #endregion Поиск элементов по идентификатору.

        #region Поиск элементов по любому Guid.

        public async Task ExecuteFindEntityByUniqueidentifier(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            string operation = string.Format(Properties.OperationNames.FindingCRMObjectsByUniqueidentifierEntityNameEntityTypeCodeFormat4, connectionData?.Name, entityId, entityName, entityTypeCode);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await FindEntityByUniqueidentifier(connectionData, commonConfig, entityName, entityTypeCode, entityId);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task FindEntityByUniqueidentifier(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            StringBuilder content = new StringBuilder();

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM));

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription()));

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            content.AppendLine(this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint));

            EntityMetadataRepository repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.GetEntitiesPropertiesAsync(entityName, entityTypeCode
                , nameof(EntityMetadata.LogicalName)
                , nameof(EntityMetadata.PrimaryIdAttribute)
                , nameof(EntityMetadata.IsIntersect)
                , nameof(EntityMetadata.Attributes)
            );

            bool finded = false;

            foreach (var item in entityMetadataList.OrderBy(e => e.LogicalName))
            {
                foreach (var field in item.Attributes
                    .Where(a => a.AttributeType == AttributeTypeCode.Uniqueidentifier && a.IsValidForRead.GetValueOrDefault(true))
                    .OrderBy(a => a.LogicalName)
                )
                {
                    var generalRepository = new GenericRepository(service, item);

                    var entityList = await generalRepository.GetEntitiesByFieldAsync(field.LogicalName, entityId, new ColumnSet(true));

                    if (entityList != null)
                    {
                        foreach (var entity in entityList)
                        {
                            finded = true;

                            content
                                .AppendLine()
                                .AppendLine()
                                .AppendLine(new string('-', 150))
                                .AppendLine()
                                .AppendLine()
                                .AppendLine(await EntityDescriptionHandler.GetEntityDescriptionAsync(entity, null, connectionData))
                            ;
                        }
                    }
                }
            }

            if (finded)
            {
                string fileName = EntityFileNameFormatter.GetFindingCRMObjectsByUniqueidentifierFileName(connectionData.Name, entityId);

                if (string.IsNullOrEmpty(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }
                else if (!Directory.Exists(commonConfig.FolderForExport))
                {
                    _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, commonConfig.FolderForExport);
                    commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }

                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsInCRMWereExportedToFormat1, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }
        }

        #endregion Поиск элементов по любому Guid.

        #region Редактирование элементов по идентификатору.

        public async Task ExecuteEditEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            string operation = string.Format(Properties.OperationNames.EditingCRMObjectsByIdEntityNameEntityTypeCodeFormat4, connectionData?.Name, entityId, entityName, entityTypeCode);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await EditEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task EditEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            EntityMetadataRepository repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.GetEntitiesPropertiesAsync(entityName, entityTypeCode, "LogicalName", "PrimaryIdAttribute", "IsIntersect", "Attributes");

            List<EntityReference> listEntities = new List<EntityReference>();

            foreach (var item in entityMetadataList.OrderBy(e => e.LogicalName))
            {
                var primaryAttr = item.Attributes.FirstOrDefault(a => string.Equals(a.LogicalName, item.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                if (primaryAttr != null && primaryAttr.AttributeType == AttributeTypeCode.Uniqueidentifier)
                {
                    var generalRepository = new GenericRepository(service, item);

                    Entity entity = await generalRepository.GetEntityByIdAsync(entityId, new ColumnSet(false));

                    if (entity != null)
                    {
                        listEntities.Add(entity.ToEntityReference());
                    }
                }
            }

            if (!listEntities.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ObjectsFoundedInCRMFormat1, listEntities.Count);

            foreach (var item in listEntities)
            {
                WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, item.LogicalName, item.Id);
            }
        }

        #endregion Редактирование элементов по идентификатору.

        public static void WriteToContentDictionary(
            SolutionComponentDescriptor descriptor
            , StringBuilder content
            , List<SolutionComponent> solutionComponents
            , Dictionary<SolutionComponent, string> dict
            , string formatList
            , params object[] args
        )
        {
            if (!dict.Any())
            {
                return;
            }

            if (content.Length > 0) { content.AppendLine(); }

            List<object> temp = new List<object>(args)
            {
                dict.Count
            };

            content.AppendFormat(formatList, temp.ToArray()).AppendLine();

            foreach (var component in solutionComponents)
            {
                if (dict.ContainsKey(component))
                {
                    var dependentComponents = dict[component];

                    if (!string.IsNullOrEmpty(dependentComponents))
                    {
                        if (content.Length > 0) { content.AppendLine(); }

                        content.AppendLine(new string('-', 150));

                        string componentDescription = descriptor.GetComponentDescription(component.ComponentType.Value, component.ObjectId.Value);

                        content.AppendLine(tabSpacer + componentDescription);
                        content.AppendLine(tabSpacer + Properties.OutputStrings.DependentComponents);

                        var coll = dependentComponents.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var item in coll)
                        {
                            content.AppendLine(tabSpacer + tabSpacer + item);
                        }
                    }
                }
            }
        }

        public static async Task WriteToContentList(SolutionComponentDescriptor descriptor, List<SolutionComponent> list, StringBuilder content, string formatList, params object[] args)
        {
            if (list.Count == 0)
            {
                return;
            }

            if (content.Length > 0) { content.AppendLine(); }

            List<object> temp = new List<object>(args)
            {
                list.Count
            };

            content.AppendFormat(formatList, temp.ToArray()).AppendLine();

            string description = await descriptor.GetSolutionComponentsDescriptionAsync(list);

            content.AppendLine(description);
        }

        public static void WriteToContentList(List<string> list, StringBuilder content, string formatList, params object[] args)
        {
            if (list.Count > 0)
            {
                if (content.Length > 0) { content.AppendLine(); }

                List<object> temp = new List<object>(args)
                {
                    list.Count
                };

                content.AppendFormat(formatList, temp.ToArray()).AppendLine();

                list.Sort();

                list.ForEach(item => content.AppendLine(tabSpacer + item));
            }
        }
    }
}
