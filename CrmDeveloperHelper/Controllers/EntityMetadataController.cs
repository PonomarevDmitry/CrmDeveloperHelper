using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Implementations;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces;
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
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    /// <summary>
    /// Контроллер для экспорта риббона
    /// </summary>
    public class EntityMetadataController : BaseController<IWriteToOutput>
    {
        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public EntityMetadataController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Generating EntityMetadata Files

        private async Task GenerateEntityProxyClassFileOrSchemaAsync(
            IOrganizationServiceExtented service
            , IMetadataProviderService metadataProviderService
            , CommonConfiguration commonConfig
            , EntityMetadata entityMetadata
            , string filePath
            , int number
            , int totalCount
        )
        {
            string fileName = Path.GetFileName(filePath);

            bool isProxyClassFile = IsProxyClassFileName(fileName);

            if (isProxyClassFile)
            {
                await GenerateEntityProxyClassFileAsync(service, metadataProviderService, commonConfig, entityMetadata, filePath, number, totalCount);
            }
            else
            {
                await GenerateEntitySchemaFileAsync(service, metadataProviderService, commonConfig, entityMetadata, filePath, number, totalCount);
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

        private async Task GenerateEntitySchemaFileAsync(
            IOrganizationServiceExtented service
            , IMetadataProviderService metadataProviderService
            , CommonConfiguration commonConfig
            , EntityMetadata entityMetadata
            , string filePath
            , int number
            , int totalCount
        )
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = CreateFileCSharpConfiguration.CreateForSchemaEntity(fileGenerationOptions);

            string operation = string.Format(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityListFormat4, service.ConnectionData.Name, number, totalCount, entityMetadata.LogicalName);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            var repository = new EntityMetadataRepository(service);

            ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
            INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
            ITypeMappingService typeMappingService = new TypeMappingService(fileGenerationOptions.NamespaceClassesCSharp);
            ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);

            ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
            {
                var handler = new CreateFileWithEntityMetadataCSharpHandler(stringWriter, config, service, _iWriteToOutput, codeGenerationServiceProvider);

                await handler.CreateFileAsync(entityMetadata);
            }

            File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        private async Task GenerateEntityProxyClassFileAsync(
            IOrganizationServiceExtented service
            , IMetadataProviderService metadataProviderService
            , CommonConfiguration commonConfig
            , EntityMetadata entityMetadata
            , string filePath
            , int number
            , int totalCount
        )
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = CreateFileCSharpConfiguration.CreateForProxyClass(fileGenerationOptions);

            string operation = string.Format(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityListFormat4, service.ConnectionData.Name, number, totalCount, entityMetadata.LogicalName);

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

            await codeGenerationService.WriteEntityFileAsync(entityMetadata, filePath, fileGenerationOptions.NamespaceClassesCSharp, options);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        private async Task GenerateEntityJavaScriptFileAsync(
            IOrganizationServiceExtented service
            , IMetadataProviderService metadataProviderService
            , CommonConfiguration commonConfig
            , EntityMetadata entityMetadata
            , string filePath
            , int number
            , int totalCount
        )
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = new CreateFileJavaScriptConfiguration(fileGenerationOptions);

            string operation = string.Format(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityListFormat4, service.ConnectionData.Name, number, totalCount, entityMetadata.LogicalName);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
            {
                var handler = new CreateFileWithEntityMetadataJavaScriptHandler(stringWriter, config, service, _iWriteToOutput);

                await handler.CreateFileAsync(entityMetadata);
            }

            File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

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
            , Func<IOrganizationServiceExtented, IMetadataProviderService, CommonConfiguration, EntityMetadata, string, int, int, Task> handler
        )
        {
            if (!selectEntity && openOptions)
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                if (isJavaScript)
                {
                    WindowHelper.OpenJavaScriptFileGenerationOptions(fileGenerationOptions);
                }
                else
                {
                    WindowHelper.OpenEntityMetadataFileGenerationOptions(fileGenerationOptions);
                }
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var unhandledFiles = new TupleList<string, string>();

            using (service.Lock())
            {
                var descriptor = new SolutionComponentDescriptor(service);

                IMetadataProviderService metadataProviderService = new MetadataProviderService(new EntityMetadataRepository(service));

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
                        unhandledFiles.Add(selection, filePath);
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

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.GettingEntitiesMetadataFormat1, linkedEntities.Count);

                    metadataProviderService.RetrieveEntities(linkedEntities);

                    int totalCount = findedEntityMetadata.Count;
                    int number = 0;

                    foreach (var item in findedEntityMetadata)
                    {
                        number++;

                        await handler(service, metadataProviderService, commonConfig, item.Item2, item.Item1, number, totalCount);
                    }
                }
            }

            if (unhandledFiles.Any())
            {
                var tabSpacer = "    ";

                var tableUnhandledFiles = new FormatTextTableHandler();
                tableUnhandledFiles.SetHeader("Entity Name", "FilePath");

                foreach (var item in unhandledFiles)
                {
                    tableUnhandledFiles.AddLine(item.Item1, item.Item2);
                }

                _iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                _iWriteToOutput.WriteToOutput(connectionData, "NOT FINDED Entities: {0}", tableUnhandledFiles.Count);

                tableUnhandledFiles.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));

                var tempService = await QuickConnection.ConnectAsync(connectionData);

                if (tempService == null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                var firstUnhandledFile = unhandledFiles.OrderBy(i => i.Item1).FirstOrDefault();

                WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, tempService, commonConfig, firstUnhandledFile.Item1, firstUnhandledFile.Item2, isJavaScript);
            }
        }

        public async Task ExecuteUpdateFileWithEntityMetadataCSharpProxyClassOrSchema(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
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

        public async Task ExecuteUpdateFileWithEntityMetadataCSharpSchema(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
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

        public async Task ExecuteUpdateFileWithEntityMetadataCSharpProxyClass(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
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

        public async Task ExecuteUpdateFileWithEntityMetadataJavaScript(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
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

        private async Task GenerateGlobalOptionSetCSharptFile(
            IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , OptionSetMetadata optionSetMetadata
            , string filePath
            , int number
            , int count
        )
        {
            string operation = string.Format(Properties.OperationNames.CreatingFileWithGlobalOptionSetsListFormat4, service.ConnectionData.Name, number, count, optionSetMetadata.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = CreateFileCSharpConfiguration.CreateForSchemaGlobalOptionSet(fileGenerationOptions);

            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
            {
                var handler = new CreateGlobalOptionSetsFileCSharpHandler(stringWriter, service, _iWriteToOutput, descriptor, config);

                await handler.CreateFileAsync(new[] { optionSetMetadata });
            }

            File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetMetadata.Name, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        private async Task GenerateGlobalOptionSetJavaScriptFile(
            IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , OptionSetMetadata metadata
            , string filePath
            , int number
            , int count
        )
        {
            string operation = string.Format(Properties.OperationNames.CreatingFileWithGlobalOptionSetsListFormat4, service.ConnectionData.Name, number, count, metadata.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
            {
                var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(
                    stringWriter
                    , service
                    , descriptor
                    , _iWriteToOutput
                    , fileGenerationOptions.GetTabSpacer()
                    , fileGenerationOptions.GenerateSchemaGlobalOptionSetsWithDependentComponents
                    , fileGenerationOptions.NamespaceGlobalOptionSetsJavaScript
                );

                await handler.CreateFileAsync(new[] { metadata });
            }

            File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, metadata.Name, filePath);

            this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);

            service.TryDispose();
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
            , Func<IOrganizationServiceExtented, SolutionComponentDescriptor, CommonConfiguration, OptionSetMetadata, string, int, int, Task> handler
        )
        {
            if (!withSelect && openOptions)
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                WindowHelper.OpenGlobalOptionSetsFileGenerationOptions(fileGenerationOptions);
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var unhandledFiles = new TupleList<string, string>();

            using (service.Lock())
            {
                var descriptor = new SolutionComponentDescriptor(service);

                var totalCount = selectedFiles.Count();

                int number = 0;

                foreach (var selFile in selectedFiles)
                {
                    number++;

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
                            await handler(service, descriptor, commonConfig, optionSetMetadata, filePath, number, totalCount);

                            continue;
                        }
                        else
                        {
                            tempWithSelect = true;
                        }
                    }

                    if (tempWithSelect)
                    {
                        unhandledFiles.Add(selection, filePath);
                    }
                }

            }

            if (unhandledFiles.Any())
            {
                var tabSpacer = "    ";

                FormatTextTableHandler tableUnhandledFiles = new FormatTextTableHandler();
                tableUnhandledFiles.SetHeader("Global OptionSet Name", "FilePath");

                foreach (var item in unhandledFiles)
                {
                    tableUnhandledFiles.AddLine(item.Item1, item.Item2);
                }

                _iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                _iWriteToOutput.WriteToOutput(connectionData, "NOT FINDED Global OptionSets: {0}", tableUnhandledFiles.Count);

                tableUnhandledFiles.GetFormatedLines(true).ForEach(item => _iWriteToOutput.WriteToOutput(connectionData, tabSpacer + item));

                var tempService = await QuickConnection.ConnectAsync(connectionData);

                if (tempService == null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                    return;
                }

                var firstUnhandledFile = unhandledFiles.OrderBy(i => i.Item1).First();

                WindowHelper.OpenGlobalOptionSetsExplorer(this._iWriteToOutput, tempService, commonConfig, firstUnhandledFile.Item1, firstUnhandledFile.Item2, isJavaScript);
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
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            if (openOptions)
            {
                WindowHelper.OpenGlobalOptionSetsFileGenerationOptions(fileGenerationOptions);
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            using (service.Lock())
            {
                var repository = new OptionSetRepository(service);

                var optionSets = await repository.GetOptionSetsAsync();

                string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

                string operation = string.Format(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat2, connectionData?.Name, optionSetsName);

                this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

                SolutionComponentDescriptor descriptor = new SolutionComponentDescriptor(service);

                var stringBuilder = new StringBuilder();

                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(
                        stringWriter
                        , service
                        , descriptor
                        , _iWriteToOutput
                        , fileGenerationOptions.GetTabSpacer()
                        , fileGenerationOptions.GenerateSchemaGlobalOptionSetsWithDependentComponents
                        , fileGenerationOptions.NamespaceGlobalOptionSetsJavaScript
                    );

                    await handler.CreateFileAsync(optionSets.OrderBy(o => o.Name));
                }

                File.WriteAllText(selectedFile.FilePath, stringBuilder.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetsName, selectedFile.FilePath);

                this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, selectedFile.FilePath);

                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        #endregion Обновление файла с глобальными OptionSet-ами JavaScript All.

        private Task CheckAttributeValidateGetEntityNameExecuteAction(
            ConnectionData connectionData
            , CommonConfiguration commonConfig
            , XDocument doc
            , string filePath
            , Func<ConnectionData, XDocument, Task<bool>> validatorDocument
            , Func<IOrganizationServiceExtented, CommonConfiguration, XDocument, string, string, Task> continueAction
        )
        {
            return CheckAttributeValidateGetEntityExecuteAction(
                connectionData
                , commonConfig
                , doc
                , filePath
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName
                , null
                , validatorDocument
                , (service, _, entityName) => Task.FromResult(Tuple.Create(true, entityName))
                , continueAction
            );
        }

        #region Ribbon Showing Difference

        public async Task ExecuteRibbonDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceRibbonFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, DifferenceRibbon);
                }
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

        public async Task ExecuteRibbonDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceRibbonFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, filePath, null, DifferenceRibbon);
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

        private async Task DifferenceRibbon(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, string entityName)
        {
            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                var entityMetadata = await repository.GetEntityMetadataAsync(entityName);

                if (entityMetadata == null)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, service.ConnectionData.Name);
                    return;
                }

                entityName = entityMetadata.LogicalName;
            }

            string fileLocalPath = filePath;
            string fileLocalTitle = Path.GetFileName(filePath);

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            var repositoryRibbon = new RibbonCustomizationRepository(service);

            if (!string.IsNullOrEmpty(entityName))
            {
                string ribbonXml = await repositoryRibbon.ExportEntityRibbonAsync(entityName, Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.All);

                ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(
                    ribbonXml
                    , commonConfig
                    , XmlOptionsControls.RibbonXmlOptions
                    , entityName: entityName ?? string.Empty
                );

                fileTitle2 = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityName);

                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityName, filePath2);
            }
            else
            {
                string ribbonXml = await repositoryRibbon.ExportApplicationRibbonAsync();

                ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(
                    ribbonXml
                    , commonConfig
                    , XmlOptionsControls.RibbonXmlOptions
                    , entityName: entityName ?? string.Empty
                );

                fileTitle2 = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);

                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedAppliationRibbonForConnectionFormat2, service.ConnectionData.Name, filePath2);
            }

            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service.ConnectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);

            service.TryDispose();
        }

        #endregion Ribbon Showing Difference

        #region RibbonDiffXml Showing Difference

        public async Task ExecuteRibbonDiffXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceRibbonDiffXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, DifferenceRibbonDiffXml);
                }
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

        public async Task ExecuteRibbonDiffXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceRibbonDiffXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, filePath, null, DifferenceRibbonDiffXml);
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

        private async Task DifferenceRibbonDiffXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, string entityName)
        {
            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            EntityMetadata entityMetadata = null;
            RibbonCustomization ribbonCustomization = null;

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                entityMetadata = await repository.GetEntityMetadataAsync(entityName);

                if (entityMetadata == null)
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, service.ConnectionData.Name);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }
            else
            {
                ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (ribbonCustomization == null)
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NotFoundedApplicationRibbonRibbonCustomization);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }

            string fileLocalPath = filePath;
            string fileLocalTitle = Path.GetFileName(filePath);

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entityMetadata, ribbonCustomization);

            ribbonDiffXml = ContentComparerHelper.FormatXmlByConfiguration(
                ribbonDiffXml
                , commonConfig
                , XmlOptionsControls.RibbonXmlOptions
                , schemaName: AbstractDynamicCommandXsdSchemas.RibbonSchema
                , entityName: entityName ?? string.Empty
            );

            if (entityMetadata != null)
            {
                fileTitle2 = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entityMetadata.LogicalName);
                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath2);
            }
            else if (ribbonCustomization != null)
            {
                fileTitle2 = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonDiffXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedAppliationRibbonDiffXmlForConnectionFormat2, service.ConnectionData.Name, filePath2);
            }

            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service.ConnectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);

            service.TryDispose();
        }

        #endregion RibbonDiffXml Showing Difference

        #region RibbonDiffXml Updating

        public async Task ExecuteRibbonDiffXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingRibbonFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, ValidateDocumentRibbonDiffXml, UpdateRibbonDiffXml);
                }
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

        public async Task ExecuteRibbonDiffXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingRibbonFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, filePath, ValidateDocumentRibbonDiffXml, UpdateRibbonDiffXml);
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

        private async Task<bool> ValidateDocumentRibbonDiffXml(ConnectionData connectionData, XDocument doc)
        {
            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ValidatingRibbonDiffXml);

            ContentComparerHelper.ClearRoot(doc);

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
                    return false;
                }
            }

            return true;
        }

        private async Task UpdateRibbonDiffXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, string entityName)
        {
            EntityMetadata entityMetadata = null;
            RibbonCustomization ribbonCustomization = null;

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                entityMetadata = await repository.GetEntityMetadataAsync(entityName);

                if (entityMetadata == null)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, service.ConnectionData.Name);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }
            else
            {
                ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (ribbonCustomization == null)
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NotFoundedApplicationRibbonRibbonCustomization);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }

            await repositoryRibbonCustomization.PerformUpdateRibbonDiffXml(_iWriteToOutput, commonConfig, doc, entityMetadata, ribbonCustomization);

            service.TryDispose();
        }

        #endregion RibbonDiffXml Updating

        #region Ribbon Open Explorer

        public async Task ExecuteOpenRibbonExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.OpeningRibbonExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, OpenRibbonExplorer);
                }
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

        private Task OpenRibbonExplorer(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, string entityName)
        {
            return Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(entityName))
                {
                    WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityName);
                }
                else
                {
                    WindowHelper.OpenApplicationRibbonExplorer(_iWriteToOutput, service, commonConfig);
                }
            });
        }

        #endregion Ribbon Open Explorer

        #region Open Entity in Browser

        public async Task ExecuteEntityRibbonOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityInWebFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, EntityRibbonOpenInWeb);
                }
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

        private async Task EntityRibbonOpenInWeb(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, string entityName)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                this._iWriteToOutput.WriteToOutput(
                    service.ConnectionData
                    , Properties.OutputStrings.XmlAttributeIsEmptyFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName.ToString()
                    , filePath
                );

                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig);

                return;
            }

            await OpenEntityInWeb(service, commonConfig, entityName, ActionOnComponent.OpenInWeb);
        }

        public async Task ExecuteEntityMetadataOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, ActionOnComponent actionOnComponent)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningEntityMetadataExplorerFormat1
                , (service) => OpenEntityInWeb(service, commonConfig, entityName, actionOnComponent)
            );
        }

        private async Task OpenEntityInWeb(IOrganizationServiceExtented service, CommonConfiguration commonConfig, string entityName, ActionOnComponent actionOnComponent)
        {
            var repository = new EntityMetadataRepository(service);

            EntityMetadata entityMetadata = await repository.GetEntityMetadataAsync(entityName);

            if (entityMetadata == null)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, service.ConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityName);

                return;
            }

            switch (actionOnComponent)
            {
                case ActionOnComponent.OpenInWeb:
                    service.ConnectionData.OpenEntityMetadataInWeb(entityMetadata.MetadataId.Value);
                    service.TryDispose();
                    break;

                case ActionOnComponent.OpenInExplorer:
                    WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityName);
                    break;

                case ActionOnComponent.OpenDependentComponentsInWeb:
                    service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Entity, entityMetadata.MetadataId.Value);
                    service.TryDispose();
                    break;

                case ActionOnComponent.OpenDependentComponentsInExplorer:
                    WindowHelper.OpenSolutionComponentDependenciesExplorer(
                        _iWriteToOutput
                        , service
                        , null
                        , commonConfig
                        , (int)ComponentType.Entity
                        , entityMetadata.MetadataId.Value
                        , null
                    );
                    break;

                case ActionOnComponent.OpenSolutionsListWithComponentInExplorer:
                    WindowHelper.OpenExplorerSolutionExplorer(
                        _iWriteToOutput
                        , service
                        , commonConfig
                        , (int)ComponentType.Entity
                        , entityMetadata.MetadataId.Value
                        , null
                    );
                    break;

                case ActionOnComponent.OpenListInWeb:
                    service.ConnectionData.OpenEntityInstanceListInWeb(entityName);
                    service.TryDispose();
                    break;

                case ActionOnComponent.None:
                default:
                    break;
            }
        }

        #endregion Open Entity in Browser

        #region Ribbon Get Current

        public async Task ExecuteRibbonGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.GettingRibbonCurrentXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, GetCurrentRibbon);
                }
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

        public async Task ExecuteRibbonGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.GettingRibbonCurrentXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, filePath, null, GetCurrentRibbon);
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

        private async Task GetCurrentRibbon(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, string entityName)
        {
            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                var entityMetadata = await repository.GetEntityMetadataAsync(entityName);

                if (entityMetadata == null)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, service.ConnectionData.Name);
                    return;
                }

                entityName = entityMetadata.LogicalName;
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string currentFilePath = string.Empty;

            var repositoryRibbon = new RibbonCustomizationRepository(service);

            if (!string.IsNullOrEmpty(entityName))
            {
                string ribbonXml = await repositoryRibbon.ExportEntityRibbonAsync(entityName, Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.All);

                ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(
                    ribbonXml
                    , commonConfig
                    , XmlOptionsControls.RibbonXmlOptions
                    , entityName: entityName ?? string.Empty
                );

                var fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityName);
                currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(currentFilePath, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityName, currentFilePath);
            }
            else
            {
                string ribbonXml = await repositoryRibbon.ExportApplicationRibbonAsync();

                ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(
                    ribbonXml
                    , commonConfig
                    , XmlOptionsControls.RibbonXmlOptions
                    , entityName: entityName ?? string.Empty
                );

                var fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);
                currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(currentFilePath, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedAppliationRibbonForConnectionFormat2, service.ConnectionData.Name, currentFilePath);
            }

            this._iWriteToOutput.PerformAction(service.ConnectionData, currentFilePath);

            service.TryDispose();
        }

        #endregion Ribbon Showing Difference

        #region RibbonDiffXml Showing Difference

        public async Task ExecuteRibbonDiffXmlGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.GettingRibbonDiffCurrentXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                if (ParseXmlDocument(connectionData, selectedFile, out var doc))
                {
                    await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, selectedFile.FilePath, null, GetCurrentRibbonDiffXml);
                }
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

        public async Task ExecuteRibbonDiffXmlGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            string operation = string.Format(Properties.OperationNames.GettingRibbonDiffCurrentXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await CheckAttributeValidateGetEntityNameExecuteAction(connectionData, commonConfig, doc, filePath, null, GetCurrentRibbonDiffXml);
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

        private async Task GetCurrentRibbonDiffXml(IOrganizationServiceExtented service, CommonConfiguration commonConfig, XDocument doc, string filePath, string entityName)
        {
            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            EntityMetadata entityMetadata = null;
            RibbonCustomization ribbonCustomization = null;

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                entityMetadata = await repository.GetEntityMetadataAsync(entityName);

                if (entityMetadata == null)
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, service.ConnectionData.Name);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }
            else
            {
                ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (ribbonCustomization == null)
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.NotFoundedApplicationRibbonRibbonCustomization);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string currentFilePath = string.Empty;

            string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entityMetadata, ribbonCustomization);

            ribbonDiffXml = ContentComparerHelper.FormatXmlByConfiguration(
                ribbonDiffXml
                , commonConfig
                , XmlOptionsControls.RibbonXmlOptions
                , schemaName: AbstractDynamicCommandXsdSchemas.RibbonSchema
                , entityName: entityName ?? string.Empty
            );

            if (entityMetadata != null)
            {
                string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entityMetadata.LogicalName);
                currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(currentFilePath, ribbonDiffXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, currentFilePath);
            }
            else if (ribbonCustomization != null)
            {
                string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                currentFilePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(currentFilePath, ribbonDiffXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedAppliationRibbonDiffXmlForConnectionFormat2, service.ConnectionData.Name, currentFilePath);
            }

            this._iWriteToOutput.PerformAction(service.ConnectionData, currentFilePath);

            service.TryDispose();
        }

        #endregion RibbonDiffXml Showing Difference

        public async Task ExecuteOpeningEntityMetadataInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityInWebFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningEntityMetadataInWeb(connectionData, commonConfig, entityName, entityTypeCode);
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

        private async Task OpeningEntityMetadataInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.FindEntitiesPropertiesOrEmptyAsync(entityName, entityTypeCode
                , nameof(EntityMetadata.LogicalName)
            );

            if (!entityMetadataList.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);

                return;
            }

            foreach (var entityMetadata in entityMetadataList)
            {
                service.ConnectionData.OpenEntityMetadataInWeb(entityMetadata.MetadataId.Value);
            }

            service.TryDispose();
        }

        public async Task ExecuteOpeningEntityListInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityListInWebFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningEntityListInWeb(connectionData, commonConfig, entityName, entityTypeCode);
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

        private async Task OpeningEntityListInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.FindEntitiesPropertiesOrEmptyAsync(entityName, entityTypeCode
                , nameof(EntityMetadata.LogicalName)
            );

            if (!entityMetadataList.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);

                return;
            }

            foreach (var entityMetadata in entityMetadataList)
            {
                service.ConnectionData.OpenEntityInstanceListInWeb(entityMetadata.LogicalName);
            }

            service.TryDispose();
        }

        public async Task ExecuteOpeningEntityFetchXmlFile(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntityFetchXmlFileFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningEntityFetchXmlFile(connectionData, commonConfig, entityName, entityTypeCode);
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

        private async Task OpeningEntityFetchXmlFile(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.FindEntitiesPropertiesOrEmptyAsync(entityName, entityTypeCode
                , nameof(EntityMetadata.LogicalName)
                , nameof(EntityMetadata.PrimaryIdAttribute)
                , nameof(EntityMetadata.PrimaryNameAttribute)
            );

            service.TryDispose();

            if (!entityMetadataList.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoObjectsInCRMWereFounded);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);

                return;
            }

            foreach (var entityMetadata in entityMetadataList)
            {
                this._iWriteToOutput.OpenFetchXmlFile(connectionData, commonConfig, entityMetadata.LogicalName);
            }
        }

        public async Task ExecutePublishEntity(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
        {
            var correctedName = new StringBuilder(entityName);

            if (entityTypeCode.HasValue)
            {
                if (correctedName.Length > 0)
                {
                    correctedName.Append(" - ");
                }

                correctedName.Append(entityTypeCode.Value.ToString());
            }

            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.PublishingEntitiesFormat2
                , (service) => PublishEntityAsync(service, commonConfig, entityName, entityTypeCode)
                , correctedName.ToString()
            );
        }

        private async Task PublishEntityAsync(IOrganizationServiceExtented service, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
        {
            var repository = new EntityMetadataRepository(service);

            var entityMetadataList = await repository.FindEntitiesPropertiesOrEmptyAsync(entityName, entityTypeCode
                , nameof(EntityMetadata.LogicalName)
            );

            if (!entityMetadataList.Any())
            {
                string entityNameCorrected = (!string.IsNullOrEmpty(entityName)) ? entityName : entityTypeCode.ToString();

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityNameCorrected, service.ConnectionData.Name);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                WindowHelper.OpenEntityMetadataExplorer(_iWriteToOutput, service, commonConfig, entityNameCorrected);

                return;
            }

            var publishRepository = new PublishActionsRepository(service);

            await publishRepository.PublishEntitiesAsync(entityMetadataList.Select(e => e.LogicalName));

            service.TryDispose();
        }
    }
}