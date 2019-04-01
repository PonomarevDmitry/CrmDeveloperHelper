using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
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
using System.Threading.Tasks;

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

        #region Создание файла с мета-данными сущности.

        public async Task ExecuteCreatingFileWithEntityMetadata(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.CreatingFileWithEntityMetadataFormat1, connectionData?.Name);

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

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, commonConfig, null, selection, null);
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

        public async Task ExecuteOpeningEntitySecurityRolesExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningEntitySecurityRolesFormat1, connectionData?.Name);

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

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, commonConfig, selection);
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

        #region Создание файла с глобальными OptionSet-ами.

        public async Task ExecuteCreatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            string operation = string.Format(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, connectionData?.Name);

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

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                WindowHelper.OpenGlobalOptionSetsWindow(
                    this._iWriteToOutput
                    , service
                    , commonConfig
                    , null
                    , null
                    , selection
                    );
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

        #region Обновление файла с мета-данными сущности.

        public async Task ExecuteUpdateFileWithEntityMetadata(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithEntityMetadataFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithEntityMetadata(selectedFiles, connectionData, commonConfig, selectEntity);
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

        private async Task UpdatingFileWithEntityMetadata(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity)
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

                bool tempSelectEntity = selectEntity;

                if (!tempSelectEntity)
                {
                    var metadata = descriptor.MetadataSource.GetEntityMetadata(selection.ToLower());

                    if (metadata != null)
                    {
                        string tabSpacer = CreateFileHandler.GetTabSpacer(commonConfig.IndentType, commonConfig.SpaceCount);

                        var config = new CreateFileWithEntityMetadataCSharpConfiguration(
                            metadata.LogicalName
                            , Path.GetDirectoryName(filePath)
                            , tabSpacer
                            , commonConfig.GenerateAttributes
                            , commonConfig.GenerateStatus
                            , commonConfig.GenerateLocalOptionSet
                            , commonConfig.GenerateGlobalOptionSet
                            , commonConfig.GenerateOneToMany
                            , commonConfig.GenerateManyToOne
                            , commonConfig.GenerateManyToMany
                            , commonConfig.GenerateKeys
                            , commonConfig.AllDescriptions
                            , commonConfig.EntityMetadaOptionSetDependentComponents
                            , commonConfig.GenerateIntoSchemaClass
                            , commonConfig.SolutionComponentWithManagedInfo
                            , commonConfig.ConstantType
                            , commonConfig.OptionSetExportType
                            )
                        {
                            EntityMetadata = metadata
                        };

                        string operation = string.Format(Properties.OperationNames.CreatingFileWithEntityMetadataForEntityFormat2, connectionData?.Name, config.EntityName);

                        this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

                        using (var handler = new CreateFileWithEntityMetadataCSharpHandler(config, service, _iWriteToOutput))
                        {
                            string fileName = Path.GetFileName(filePath);

                            await handler.CreateFileAsync(fileName);
                        }

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, connectionData.Name, config.EntityName, filePath);

                        this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, filePath);

                        this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);

                        continue;
                    }
                    else
                    {
                        tempSelectEntity = true;
                    }
                }

                if (tempSelectEntity)
                {
                    var tempService = await QuickConnection.ConnectAsync(connectionData);

                    WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, tempService, commonConfig, null, selection, filePath);
                }
            }
        }

        #endregion Обновление файла с мета-данными сущности.

        #region Обновление файла с глобальными OptionSet-ами.

        public async Task ExecuteUpdatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingFileWithGlobalOptionSetsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingFileWithGlobalOptionSets(connectionData, commonConfig, selectedFiles, withSelect);
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

        private async Task UpdatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
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
                    var metadata = descriptor.MetadataSource.GetOptionSetMetadata(selection.ToLower());

                    if (metadata != null)
                    {
                        string operation = string.Format(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat2, connectionData?.Name, metadata.Name);

                        this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

                        string tabSpacer = CreateFileHandler.GetTabSpacer(commonConfig.IndentType, commonConfig.SpaceCount);

                        using (var handler = new CreateGlobalOptionSetsFileCSharpHandler(
                                   service
                                   , _iWriteToOutput
                                   , tabSpacer
                                   , commonConfig.ConstantType
                                   , commonConfig.OptionSetExportType
                                   , commonConfig.GlobalOptionSetsWithDependentComponents
                                   , commonConfig.SolutionComponentWithManagedInfo
                                   , commonConfig.AllDescriptions
                                   ))
                        {
                            await handler.CreateFileAsync(filePath, new[] { metadata });
                        }

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, metadata.Name, filePath);

                        this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, filePath);

                        this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);

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

                    WindowHelper.OpenGlobalOptionSetsWindow(
                        this._iWriteToOutput
                        , tempService
                        , commonConfig
                        , null
                        , filePath
                        , selection
                        );
                }
            }
        }

        #endregion Обновление файла с глобальными OptionSet-ами.

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

            if (!ContentCoparerHelper.TryParseXml(fileText, out var doc))
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

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                var entityMetadata = repository.GetEntityMetadata(entityName);

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

                ribbonXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonXml, commonConfig, XmlOptionsControls.RibbonFull
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

                ribbonXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonXml, commonConfig, XmlOptionsControls.RibbonFull
                   , ribbonEntityName: entityName ?? string.Empty
                   );

                fileTitle2 = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);

                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

                File.WriteAllText(filePath2, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ExportedAppliationRibbonForConnectionFormat2, service.ConnectionData.Name, filePath2);
            }

            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
        }

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

            if (!ContentCoparerHelper.TryParseXml(fileText, out var doc))
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

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            EntityMetadata entityMetadata = null;
            RibbonCustomization ribbonCustomization = null;

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                entityMetadata = repository.GetEntityMetadata(entityName);

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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.NotFoundedApplicationRibbonRibbonCustomization);
                    _iWriteToOutput.ActivateOutputWindow(connectionData);
                    return;
                }
            }

            string filePath1 = selectedFile.FilePath;
            string fileTitle1 = selectedFile.FileName;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entityMetadata, ribbonCustomization);

            ribbonDiffXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonDiffXml, commonConfig, XmlOptionsControls.RibbonFull
                , schemaName: CommonExportXsdSchemasCommand.SchemaRibbonXml
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

            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
        }

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

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ValidatingRibbonDiffXml);

            if (!ContentCoparerHelper.TryParseXmlDocument(fileText, out var doc))
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
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ValidatingRibbonDiffXmlFailed);
                _iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string entityName = attribute.Value;

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            EntityMetadata entityMetadata = null;
            RibbonCustomization ribbonCustomization = null;

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            if (!string.IsNullOrEmpty(entityName))
            {
                var repository = new EntityMetadataRepository(service);

                entityMetadata = repository.GetEntityMetadata(entityName);

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
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.NotFoundedApplicationRibbonRibbonCustomization);
                    _iWriteToOutput.ActivateOutputWindow(connectionData);
                    return;
                }
            }

            await repositoryRibbonCustomization.PerformUpdateRibbonDiffXml(_iWriteToOutput, commonConfig, doc, entityMetadata, ribbonCustomization);
        }
    }
}