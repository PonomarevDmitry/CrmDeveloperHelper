using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    /// <summary>
    /// Контроллер для экспорта риббона
    /// </summary>
    public class EntityMetadataController
    {
        private readonly IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="outputWindow"></param>
        public EntityMetadataController(IWriteToOutput outputWindow)
        {
            this._iWriteToOutput = outputWindow;
        }

        #region Открытие Entity Explorers.

        public async Task ExecuteOpeningEntityAttributeExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityAttributeExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, commonConfig, selection);
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

        public async Task ExecuteOpeningEntityKeyExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityKeyExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, commonConfig, selection);
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

        public async Task ExecuteOpeningEntityRelationshipOneToManyExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityRelationshipOneToManyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, commonConfig, selection);
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

        public async Task ExecuteOpeningEntityRelationshipManyToManyExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityRelationshipManyToManyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, commonConfig, selection);
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

        public async Task ExecuteOpeningEntityPrivilegesExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityPrivilegesExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, commonConfig, selection);
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

        #endregion Открытие Entity Explorers.

        #region Создание файла с мета-данными сущности.

        public async Task ExecuteOpeningEntityMetadataExplorer(string selection, EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityMetadataExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, commonConfig, selection, selectedItem);
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

        #endregion Создание файла с мета-данными сущности.

        #region Opening Entity Metadata File Generation Options

        public void ExecuteOpeningEntityMetadataFileGenerationOptions()
        {
            string operation = Properties.OperationNames.OpeningEntityMetadataFileGenerationOptions;

            this._iWriteToOutput.WriteToOutputStartOperation(null, operation);

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                WindowHelper.OpenEntityMetadataFileGenerationOptions(fileGenerationOptions);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, operation);
            }
        }

        #endregion Opening Entity Metadata File Generation Options

        #region Opening Global OptionSets Metadata File Generation Options

        public void ExecuteOpeningGlobalOptionSetsMetadataFileGenerationOptions()
        {
            string operation = Properties.OperationNames.OpeningGlobalOptionSetsFileGenerationOptions;

            this._iWriteToOutput.WriteToOutputStartOperation(null, operation);

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                WindowHelper.OpenGlobalOptionSetsFileGenerationOptions(fileGenerationOptions);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, operation);
            }
        }

        #endregion Opening Global OptionSets Metadata File Generation Options

        #region Создание файла с глобальными OptionSet-ами.

        public async Task ExecuteCreatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            string operation = string.Format(Properties.OperationNames.OpeningGlobalOptionSetsExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenGlobalOptionSetsExplorer(this._iWriteToOutput, service, commonConfig, selection, selectedItem);
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

        #endregion Создание файла с глобальными OptionSet-ами.

        #region Generating EntityMetadata Files

        private async Task GenerateEntityProxyClassFileOrSchemaAsync(IOrganizationServiceExtented service, IMetadataProviderService metadataProviderService, CommonConfiguration commonConfig, EntityMetadata entityMetadata, string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            bool isProxyClassFile = IsProxyClassFileName(fileName);

            if (isProxyClassFile)
            {
                await GenerateEntityProxyClassFileAsync(service, metadataProviderService, commonConfig, entityMetadata, filePath);
            }
            else
            {
                await GenerateEntitySchemaFileAsync(service, metadataProviderService, commonConfig, entityMetadata, filePath);
            }
        }

        private bool IsProxyClassFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (fileName.EndsWith(".Proxy.cs", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                if (fileName.EndsWith(".Schema.cs", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                if (fileName.EndsWith(".Generated.cs", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return false;
        }

        private async Task GenerateEntitySchemaFileAsync(IOrganizationServiceExtented service, IMetadataProviderService metadataProviderService, CommonConfiguration commonConfig, EntityMetadata entityMetadata, string filePath)
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = CreateFileCSharpConfiguration.CreateForSchemaEntity(fileGenerationOptions);

            string operation = string.Format(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            var repository = new EntityMetadataRepository(service);

            ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
            INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
            ITypeMappingService typeMappingService = new TypeMappingService(fileGenerationOptions.NamespaceClassesCSharp);
            ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);

            ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    var handler = new CreateFileWithEntityMetadataCSharpHandler(streamWriter, config, service, _iWriteToOutput, codeGenerationServiceProvider);

                    await handler.CreateFileAsync(entityMetadata);

                    try
                    {
                        await memoryStream.FlushAsync();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(filePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                    }
                }
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        private async Task GenerateEntityProxyClassFileAsync(IOrganizationServiceExtented service, IMetadataProviderService metadataProviderService, CommonConfiguration commonConfig, EntityMetadata entityMetadata, string filePath)
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = CreateFileCSharpConfiguration.CreateForProxyClass(fileGenerationOptions);

            string operation = string.Format(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
            INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
            ITypeMappingService typeMappingService = new TypeMappingService(fileGenerationOptions.NamespaceClassesCSharp);
            ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);

            ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BlankLinesBetweenMembers = true,
                BracingStyle = "C",
                VerbatimOrder = true,
            };

            await codeGenerationService.WriteEntityFileAsync(entityMetadata, filePath, fileGenerationOptions.NamespaceClassesCSharp, options, codeGenerationServiceProvider);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        private async Task GenerateEntityJavaScriptFileAsync(IOrganizationServiceExtented service, IMetadataProviderService metadataProviderService, CommonConfiguration commonConfig, EntityMetadata entityMetadata, string filePath)
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = new CreateFileJavaScriptConfiguration(
                fileGenerationOptions.GetTabSpacer()
                , fileGenerationOptions.GenerateSchemaEntityOptionSetsWithDependentComponents
                , fileGenerationOptions.GenerateSchemaIntoSchemaClass
                , fileGenerationOptions.GenerateSchemaGlobalOptionSet
                , fileGenerationOptions.NamespaceClassesJavaScript
            );

            string operation = string.Format(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    var handler = new CreateFileWithEntityMetadataJavaScriptHandler(streamWriter, config, service, _iWriteToOutput);

                    await handler.CreateFileAsync(entityMetadata);

                    try
                    {
                        await memoryStream.FlushAsync();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(filePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                    }
                }
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        #endregion Generating EntityMetadata Files

        #region Updating EntityMetadata Files

        private async Task UpdatingFileWithEntityMetadata(
            List<SelectedFile> selectedFiles
            , ConnectionData connectionData
            , CommonConfiguration commonConfig
            , bool selectEntity
            , bool openOptions
            , bool isJavaScript
            , Func<IOrganizationServiceExtented, IMetadataProviderService, CommonConfiguration, EntityMetadata, string, Task> handler
        )
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!selectEntity && openOptions)
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                WindowHelper.OpenEntityMetadataFileGenerationOptions(fileGenerationOptions);
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.SetSettings(commonConfig);

            IMetadataProviderService metadataProviderService = new MetadataProviderService(new EntityMetadataRepository(service));

            var unhandledFiles = new TupleList<string, string>();
            var findedEntityMetadata = new TupleList<string, EntityMetadata>();

            foreach (var selFile in selectedFiles)
            {
                var filePath = selFile.FilePath;

                if (!File.Exists(filePath))
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, filePath);
                    continue;
                }

                var selection = Path.GetFileNameWithoutExtension(filePath);
                selection = selection.Split('.').FirstOrDefault();

                bool tempSelectEntity = selectEntity;

                if (!tempSelectEntity)
                {
                    var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(selection.ToLower());

                    if (entityMetadata != null)
                    {
                        metadataProviderService.StoreEntityMetadata(entityMetadata);

                        findedEntityMetadata.Add(filePath, entityMetadata);

                        continue;
                    }
                    else
                    {
                        tempSelectEntity = true;
                    }
                }

                if (tempSelectEntity)
                {
                    unhandledFiles.Add(filePath, selection);
                }
            }

            if (findedEntityMetadata.Any())
            {
                HashSet<string> linkedEntities = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var item in findedEntityMetadata)
                {
                    var temp = CodeGenerationService.GetLinkedEntities(item.Item2);

                    foreach (var entityName in temp)
                    {
                        linkedEntities.Add(entityName);
                    }
                }

                metadataProviderService.RetrieveEntities(linkedEntities);

                foreach (var item in findedEntityMetadata)
                {
                    await handler(service, metadataProviderService, commonConfig, item.Item2, item.Item1);
                }
            }

            if (unhandledFiles.Any())
            {
                var tempService = await QuickConnection.ConnectAsync(connectionData);

                if (tempService == null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                foreach (var item in unhandledFiles)
                {
                    WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, tempService, commonConfig, item.Item2, item.Item1, isJavaScript);
                }
            }
        }

        public async Task ExecuteUpdateFileWithEntityMetadataCSharpProxyClassOrSchema(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity, bool openOptions)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithEntityMetadataFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithEntityMetadata(selectedFiles, connectionData, commonConfig, selectEntity, openOptions, false, GenerateEntityProxyClassFileOrSchemaAsync);
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

        public async Task ExecuteUpdateFileWithEntityMetadataCSharpSchema(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity, bool openOptions)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithEntityMetadataFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithEntityMetadata(selectedFiles, connectionData, commonConfig, selectEntity, openOptions, false, GenerateEntitySchemaFileAsync);
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

        public async Task ExecuteUpdateFileWithEntityMetadataCSharpProxyClass(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity, bool openOptions)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithEntityMetadataFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithEntityMetadata(selectedFiles, connectionData, commonConfig, selectEntity, openOptions, false, GenerateEntityProxyClassFileAsync);
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

        public async Task ExecuteUpdateFileWithEntityMetadataJavaScript(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity, bool openOptions)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithEntityMetadataFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithEntityMetadata(selectedFiles, connectionData, commonConfig, selectEntity, openOptions, true, GenerateEntityJavaScriptFileAsync);
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

        #endregion Updating EntityMetadata Files

        #region Generating Global OptionSet Files

        private async Task GenerateGlobalOptionSetCSharptFile(IOrganizationServiceExtented service, CommonConfiguration commonConfig, OptionSetMetadata optionSetMetadata, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat2, service.ConnectionData.Name, optionSetMetadata.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = CreateFileCSharpConfiguration.CreateForSchemaGlobalOptionSet(fileGenerationOptions);

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    var handler = new CreateGlobalOptionSetsFileCSharpHandler(streamWriter, service, _iWriteToOutput, config);

                    await handler.CreateFileAsync(new[] { optionSetMetadata });

                    try
                    {
                        await memoryStream.FlushAsync();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(filePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                    }
                }
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetMetadata.Name, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        private async Task GenerateGlobalOptionSetJavaScriptFile(IOrganizationServiceExtented service, CommonConfiguration commonConfig, OptionSetMetadata metadata, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat2, service.ConnectionData.Name, metadata.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(
                        streamWriter
                        , service
                        , _iWriteToOutput
                        , fileGenerationOptions.GetTabSpacer()
                        , fileGenerationOptions.GenerateSchemaGlobalOptionSetsWithDependentComponents
                        , fileGenerationOptions.NamespaceGlobalOptionSetsJavaScript
                    );

                    await handler.CreateFileAsync(new[] { metadata });

                    try
                    {
                        await memoryStream.FlushAsync();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(filePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                    }
                }
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, metadata.Name, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        #endregion Generating Global OptionSet Files

        #region Updating Global OptionSet Files

        private async Task UpdatingFileWithGlobalOptionSet(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , IEnumerable<SelectedFile> selectedFiles
            , bool withSelect
            , bool openOptions
            , bool isJavaScript
            , Func<IOrganizationServiceExtented, CommonConfiguration, OptionSetMetadata, string, Task> handler
        )
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!withSelect && openOptions)
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                WindowHelper.OpenGlobalOptionSetsFileGenerationOptions(fileGenerationOptions);
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var descriptor = new SolutionComponentDescriptor(service);
            descriptor.SetSettings(commonConfig);

            foreach (var selFile in selectedFiles)
            {
                var filePath = selFile.FilePath;

                if (!File.Exists(filePath))
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, filePath);
                    continue;
                }

                var selection = Path.GetFileNameWithoutExtension(filePath);
                selection = selection.Split('.').FirstOrDefault();

                bool tempWithSelect = withSelect;

                if (!tempWithSelect)
                {
                    var optionSetMetadata = descriptor.MetadataSource.GetOptionSetMetadata(selection.ToLower());

                    if (optionSetMetadata != null)
                    {
                        await handler(service, commonConfig, optionSetMetadata, filePath);

                        continue;
                    }
                    else
                    {
                        tempWithSelect = true;
                    }
                }

                if (tempWithSelect)
                {
                    var tempService = await QuickConnection.ConnectAsync(connectionData);

                    if (tempService == null)
                    {
                        _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                        return;
                    }

                    WindowHelper.OpenGlobalOptionSetsExplorer(this._iWriteToOutput, tempService, commonConfig, selection, filePath, isJavaScript);
                }
            }
        }

        public async Task ExecuteUpdatingFileWithGlobalOptionSetCSharp(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, bool withSelect, bool openOptions)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithGlobalOptionSetsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithGlobalOptionSet(connectionData, commonConfig, selectedFiles, withSelect, openOptions, false, GenerateGlobalOptionSetCSharptFile);
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

        public async Task ExecuteUpdatingFileWithGlobalOptionSetSingleJavaScript(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, bool withSelect, bool openOptions)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithGlobalOptionSetsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithGlobalOptionSet(connectionData, commonConfig, selectedFiles, withSelect, openOptions, true, GenerateGlobalOptionSetJavaScriptFile);
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

        #endregion Updating Global OptionSet Files

        #region Обновление файла с глобальными OptionSet-ами JavaScript All.

        public async Task ExecuteUpdatingFileWithGlobalOptionSetAllJavaScript(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool openOptions)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithGlobalOptionSetsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithGlobalOptionSetAllJavaScript(connectionData, commonConfig, selectedFile, openOptions);
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

        private async Task UpdatingFileWithGlobalOptionSetAllJavaScript(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool openOptions)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            if (openOptions)
            {
                WindowHelper.OpenGlobalOptionSetsFileGenerationOptions(fileGenerationOptions);
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            OptionSetRepository repository = new OptionSetRepository(service);

            var optionSets = await repository.GetOptionSetsAsync();

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            string operation = string.Format(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat2, connectionData?.Name, optionSetsName);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(
                        streamWriter
                        , service
                        , _iWriteToOutput
                        , fileGenerationOptions.GetTabSpacer()
                        , fileGenerationOptions.GenerateSchemaGlobalOptionSetsWithDependentComponents
                        , fileGenerationOptions.NamespaceGlobalOptionSetsJavaScript
                    );

                    await handler.CreateFileAsync(optionSets.OrderBy(o => o.Name));

                    try
                    {
                        await memoryStream.FlushAsync();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(selectedFile.FilePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                    }
                }
            }

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetsName, selectedFile.FilePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, selectedFile.FilePath);

            this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
        }

        #endregion Обновление файла с глобальными OptionSet-ами JavaScript All.

        #region Ribbon Showing Difference

        public async Task ExecuteDifferenceRibbon(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceRibbonFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await DifferenceRibbon(selectedFile, connectionData, commonConfig);
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

        private async Task DifferenceRibbon(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentComparerHelper.TryParseXml(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                return;
            }

            var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName.ToString(), selectedFile.FilePath);
                return;
            }

            string entityName = attribute.Value;

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                var entityMetadata = await repository.GetEntityMetadataAsync(entityName);

                if (entityMetadata == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, connectionData.Name);
                    return;
                }

                entityName = entityMetadata.LogicalName;
            }

            string filePath1 = selectedFile.FilePath;
            string fileTitle1 = selectedFile.FileName;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            var repositoryRibbon = new RibbonCustomizationRepository(service);

            if (!string.IsNullOrEmpty(entityName))
            {
                string ribbonXml = await repositoryRibbon.ExportEntityRibbonAsync(entityName, Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.All);

                ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(ribbonXml, commonConfig, XmlOptionsControls.RibbonFull
                   , ribbonEntityName: entityName ?? string.Empty
                );

                fileTitle2 = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityName);

                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityName, filePath2);
            }
            else
            {
                string ribbonXml = await repositoryRibbon.ExportApplicationRibbonAsync();

                ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(ribbonXml, commonConfig, XmlOptionsControls.RibbonFull
                   , ribbonEntityName: entityName ?? string.Empty
                );

                fileTitle2 = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);

                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ExportedAppliationRibbonForConnectionFormat2, service.ConnectionData.Name, filePath2);
            }

            this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, fileTitle1, fileTitle2);
        }

        #endregion Ribbon Showing Difference

        #region RibbonDiffXml Showing Difference

        public async Task ExecuteDifferenceRibbonDiffXml(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceRibbonDiffXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await DifferenceRibbonDiffXml(selectedFile, connectionData, commonConfig);
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

        private async Task DifferenceRibbonDiffXml(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentComparerHelper.TryParseXml(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                return;
            }

            var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName.ToString(), selectedFile.FilePath);
                return;
            }

            string entityName = attribute.Value;

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            EntityMetadata entityMetadata = null;
            RibbonCustomization ribbonCustomization = null;

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                entityMetadata = await repository.GetEntityMetadataAsync(entityName);

                if (entityMetadata == null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, connectionData.Name);
                    _iWriteToOutput.ActivateOutputWindow(connectionData);
                    return;
                }
            }
            else
            {
                ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (ribbonCustomization == null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NotFoundedApplicationRibbonRibbonCustomization);
                    _iWriteToOutput.ActivateOutputWindow(connectionData);
                    return;
                }
            }

            string filePath1 = selectedFile.FilePath;
            string fileTitle1 = selectedFile.FileName;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entityMetadata, ribbonCustomization);

            ribbonDiffXml = ContentComparerHelper.FormatXmlByConfiguration(ribbonDiffXml, commonConfig, XmlOptionsControls.RibbonFull
                , schemaName: AbstractDynamicCommandXsdSchemas.SchemaRibbonXml
                , ribbonEntityName: entityName ?? string.Empty
                );

            if (entityMetadata != null)
            {
                fileTitle2 = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entityMetadata.LogicalName);
                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath2);
            }
            else if (ribbonCustomization != null)
            {
                fileTitle2 = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ExportedAppliationRibbonDiffXmlForConnectionFormat2, service.ConnectionData.Name, filePath2);
            }

            this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, fileTitle1, fileTitle2);
        }

        #endregion RibbonDiffXml Showing Difference

        #region RibbonDiffXml Updating

        public async Task ExecuteUpdateRibbonDiffXml(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingRibbonFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdateRibbonDiffXml(selectedFile, connectionData, commonConfig);
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

        private async Task UpdateRibbonDiffXml(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            //fileText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(fileText);

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingRibbonDiffXml);

            if (!ContentComparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                _iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName.ToString(), selectedFile.FilePath);
                return;
            }

            bool validateResult = await RibbonCustomizationRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingRibbonDiffXmlFailed);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                var dialogResult = MessageBoxResult.Cancel;

                var t = new Thread(() =>
                {
                    dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult != MessageBoxResult.OK)
                {
                    return;
                }
            }

            string entityName = attribute.Value;

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            EntityMetadata entityMetadata = null;
            RibbonCustomization ribbonCustomization = null;

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                entityMetadata = await repository.GetEntityMetadataAsync(entityName);

                if (entityMetadata == null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, connectionData.Name);
                    _iWriteToOutput.ActivateOutputWindow(connectionData);
                    return;
                }
            }
            else
            {
                ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (ribbonCustomization == null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NotFoundedApplicationRibbonRibbonCustomization);
                    _iWriteToOutput.ActivateOutputWindow(connectionData);
                    return;
                }
            }

            await repositoryRibbonCustomization.PerformUpdateRibbonDiffXml(_iWriteToOutput, commonConfig, doc, entityMetadata, ribbonCustomization);
        }

        #endregion RibbonDiffXml Updating

        #region Ribbon Open Explorer

        public async Task ExecuteOpenRibbonExplorer(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningRibbonExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpenRibbonExplorer(selectedFile, connectionData, commonConfig);
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

        private async Task OpenRibbonExplorer(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentComparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                _iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName.ToString(), selectedFile.FilePath);
                return;
            }

            string entityName = attribute.Value;

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            if (!string.IsNullOrEmpty(entityName))
            {
                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityName);
            }
            else
            {
                WindowHelper.OpenApplicationRibbonWindow(_iWriteToOutput, service, commonConfig);
            }
        }

        #endregion Ribbon Open Explorer

        #region Open Entity in Web

        public async Task ExecuteEntityRibbonOpenInWeb(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityInWebFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await EntityRibbonOpenInWeb(selectedFile, connectionData, commonConfig);
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

        private async Task EntityRibbonOpenInWeb(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentComparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig);

                return;
            }

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName.ToString(), selectedFile.FilePath);

                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig);

                return;
            }

            string entityName = attribute.Value;

            if (string.IsNullOrEmpty(entityName))
            {
                this._iWriteToOutput.WriteToOutput(connectionData
                    , Properties.OutputStrings.XmlAttributeIsEmptyFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName.ToString()
                    , selectedFile.FilePath
                );

                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig);

                return;
            }

            var repository = new EntityMetadataRepository(service);

            EntityMetadata entityMetadata = await repository.GetEntityMetadataAsync(entityName);

            if (entityMetadata == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, connectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityName);

                return;
            }

            service.ConnectionData.OpenEntityMetadataInWeb(entityMetadata.MetadataId.Value);
        }

        #endregion Open Entity in Web
    }
}