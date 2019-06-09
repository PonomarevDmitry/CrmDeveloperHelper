using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    /// <summary>
    /// Основной контроллер
    /// </summary>
    public class MainController
    {
        private readonly IWriteToOutputAndPublishList _iWriteToOutputAndPublishList;

        private readonly PublishController _publishController;
        private readonly CompareController _compareController;
        private readonly DifferenceController _differenceController;
        private readonly ExplorerController _explorerController;
        private readonly CheckController _checkController;
        private readonly ExportXmlController _exportXmlController;
        private readonly PluginConfigurationController _pluginConfigurationController;
        private readonly SolutionController _solutionController;
        private readonly EntityMetadataController _entityMetadataController;
        private readonly ExportPluginConfigurationController _exportPluginConfigurationController;
        private readonly CheckPluginController _checkPluginController;
        private readonly PluginController _pluginController;
        private readonly CheckManagedEntitiesController _checkManagedEntitiesController;
        private readonly OpenFilesController _openFilesController;
        private readonly ReportController _reportController;
        private readonly LinkController _linkController;
        private readonly SecurityController _securityController;

        /// <summary>
        /// Конструктор контроллера для публикации
        /// </summary>
        /// <param name="outputWindow"></param>
        public MainController(IWriteToOutputAndPublishList outputWindow)
        {
            this._iWriteToOutputAndPublishList = outputWindow;

            this._publishController = new PublishController(outputWindow);
            this._compareController = new CompareController(outputWindow);
            this._explorerController = new ExplorerController(outputWindow);
            this._checkController = new CheckController(outputWindow);
            this._exportXmlController = new ExportXmlController(outputWindow);
            this._pluginConfigurationController = new PluginConfigurationController(outputWindow);
            this._differenceController = new DifferenceController(outputWindow);
            this._solutionController = new SolutionController(outputWindow);
            this._entityMetadataController = new EntityMetadataController(outputWindow);
            this._exportPluginConfigurationController = new ExportPluginConfigurationController(outputWindow);
            this._checkPluginController = new CheckPluginController(outputWindow);
            this._pluginController = new PluginController(outputWindow);
            this._checkManagedEntitiesController = new CheckManagedEntitiesController(outputWindow);
            this._openFilesController = new OpenFilesController(outputWindow);
            this._reportController = new ReportController(outputWindow);
            this._linkController = new LinkController(outputWindow);
            this._securityController = new SecurityController(outputWindow);
        }

        /// <summary>
        /// Старт публикации веб-ресурсов
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartUpdateContentAndPublish(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._publishController.ExecuteUpdateContentAndPublish(selectedFiles, connectionData);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        /// <summary>
        /// Старт публикации веб-ресурсов разных по содержанию, но одинаковых по тексту
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartUpdateContentAndPublishEqualByText(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._publishController.ExecuteUpdateContentAndPublishEqualByText(selectedFiles, connectionData);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        /// <summary>
        /// Старт сравнения
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartComparing(List<SelectedFile> selectedFiles, ConnectionData connectionData, bool withDetails)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._compareController.ExecuteComparingFilesAndWebResources(selectedFiles, connectionData, withDetails);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void ShowingWebResourcesDependentComponents(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteShowingWebResourcesDependentComponents(connectionData, commonConfig, selectedFiles);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void ExecuteCheckingWorkflowsUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingWorkflowsUsedEntities(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void ExecuteCheckingWorkflowsNotExistingUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingWorkflowsNotExistingUsedEntities(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartComparingFilesWithWrongEncoding(List<SelectedFile> selectedFiles, ConnectionData connectionData, bool withDetails)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._compareController.ExecuteComparingFilesWithWrongEncoding(selectedFiles, connectionData, withDetails);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        /// <summary>
        /// Старт различия
        /// </summary>
        /// <param name="selectedFile"></param>
        /// <param name="isCustom"></param>
        /// <param name="crmConfig"></param>
        /// <param name="commonConfig"></param>
        public void StartWebResourceDifference(SelectedFile selectedFile, bool isCustom, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteDifferenceWebResources(selectedFile, isCustom, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartRibbonDifference(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteDifferenceRibbon(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSiteMapDifference(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceSiteMap(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSiteMapUpdate(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateSiteMap(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSiteMapOpenInWeb(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteOpenInWebSiteMap(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSystemFormDifference(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceSystemForm(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSystemFormUpdate(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateSystemForm(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSystemFormOpenInWeb(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteOpenInWebSystemForm(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSavedQueryDifference(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceSavedQuery(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSavedQueryUpdate(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateSavedQuery(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSavedQueryOpenInWeb(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteOpenInWebSavedQuery(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartRibbonDiffXmlDifference(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteDifferenceRibbonDiffXml(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartRibbonDiffXmlUpdate(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdateRibbonDiffXml(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWebResourceThreeFileDifference(SelectedFile selectedFile, ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteThreeFileDifferenceWebResources(selectedFile, connectionData1, connectionData2, differenceType, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartAddingIntoPublishListFilesByType(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._compareController.ExecuteAddingIntoPublishListFilesByType(selectedFiles, openFilesType, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartMultiDifferenceFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteMultiDifferenceFiles(selectedFiles, openFilesType, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        /// <summary>
        /// Старт различия отчетов
        /// </summary>
        /// <param name="selectedFile"></param>
        /// <param name="isCustom"></param>
        /// <param name="crmConfig"></param>
        /// <param name="commonConfig"></param>
        public void StartReportDifference(SelectedFile selectedFile, string fieldName, string fieldTitle, bool isCustom, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteDifferenceReport(selectedFile, fieldName, fieldTitle, isCustom, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartReportThreeFileDifference(SelectedFile selectedFile, string fieldName, string fieldTitle, ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteThreeFileDifferenceReport(selectedFile, fieldName, fieldTitle, connectionData1, connectionData2, differenceType, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartReportUpdate(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._reportController.ExecuteUpdatingReport(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenWebResourceExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningWebResourceExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenReportExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningReportExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        /// <summary>
        /// Старт очистки старых линков
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartClearingLastLink(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._linkController.ExecuteClearingLastLink(selectedFiles, connectionData);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCreatingLastLinkReport(SelectedFile selectedFile, ConnectionData connectionData)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._linkController.ExecuteCreatingLastLinkReport(selectedFile, connectionData);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCreatingLastLinkWebResourceMultiple(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._linkController.ExecuteCreatingLastLinkWebResourceMultiple(selectedFiles, connectionData);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        /// <summary>
        /// Проверка CRM на существование сущностей с префиксом new_.
        /// </summary>
        /// <param name="config"></param>
        public void StartCheckEntitiesNames(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingEntitiesNames(connectionData, commonConfig, prefix);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckEntitiesNamesAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingEntitiesNamesAndShowDependentComponents(connectionData, commonConfig, prefix);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckMarkedToDeleteAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingMarkedToDelete(connectionData, commonConfig, prefix);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckPluginImages(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkPluginController.ExecuteCheckingPluginImages(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckGlobalOptionSetDuplicates(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingGlobalOptionSetDuplicates(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckComponentTypeEnum(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingComponentTypeEnum(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCreateAllDependencyNodesDescription(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCreatingAllDependencyNodesDescription(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckPluginImagesRequiredComponents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkPluginController.ExecuteCheckingPluginImagesRequiredComponents(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckPluginStepsRequiredComponents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkPluginController.ExecuteCheckingPluginStepsRequiredComponents(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckPluginSteps(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkPluginController.ExecuteCheckingPluginSteps(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityElementsByName(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteFindEntityElementsByName(connectionData, commonConfig, name);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityElementsContainsString(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteFindEntityElementsContainsString(connectionData, commonConfig, name);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteFindEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartEditEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteEditEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityByUniqueidentifier(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteFindEntityByUniqueidentifier(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckEntitiesOwnership(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingEntitiesOwnership(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExportingFormEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteExportingFormsEvents(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExportRibbonXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningApplicationRibbonExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExportPluginConfiguration(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportPluginConfigurationController.ExecuteExportingPluginConfigurationXml(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenPluginTypeExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningPluginTypeExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartAddPluginStep(string pluginTypeName, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginController.ExecuteAddingPluginStepForType(connectionData, commonConfig, pluginTypeName);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenPluginAssemblyExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningPluginAssemblyExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCreatingFileWithEntityMetadata(string selection, EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteCreatingFileWithEntityMetadata(selection, selectedItem, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerEntityAttribute(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityAttributeExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerEntityKey(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityKeyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerEntityRelationshipOneToMany(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityRelationshipOneToManyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerEntityRelationshipManyToMany(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityRelationshipManyToManyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerEntityPrivileges(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityPrivilegesExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCreatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteCreatingFileWithGlobalOptionSets(connectionData, commonConfig, selection, selectedItem);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithEntityMetadataCSharpSchema(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity, bool openOptions)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdateFileWithEntityMetadataCSharpSchema(selectedFiles, connectionData, commonConfig, selectEntity, openOptions);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithEntityMetadataCSharpProxyClass(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity, bool openOptions)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdateFileWithEntityMetadataCSharpProxyClass(selectedFiles, connectionData, commonConfig, selectEntity, openOptions);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithEntityMetadataJavaScript(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity, bool openOptions)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdateFileWithEntityMetadataJavaScript(selectedFiles, connectionData, commonConfig, selectEntity, openOptions);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, bool withSelect, bool openOptions)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdatingFileWithGlobalOptionSetCSharp(connectionData, commonConfig, selectedFiles, withSelect, openOptions);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithGlobalOptionSetSingleJavaScript(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity, bool openOptions)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdatingFileWithGlobalOptionSetSingleJavaScript(connectionData, commonConfig, selectedFiles, selectEntity, openOptions);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithGlobalOptionSetAllJavaScript(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool openOptions)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdatingFileWithGlobalOptionSetAllJavaScript(connectionData, commonConfig, selectedFile, openOptions);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerSitemapXml(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningSitemapExplorer(connectionData, commonConfig, filter);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerOrganizationInformation(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningOrganizationExplorer(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerSystemSavedQueryXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningSystemSavedQueryExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerSystemSavedQueryVisualization(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningSystemSavedQueryVisualizationExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningSystemFormExplorer(connectionData, commonConfig, selection, selectedItem);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerCustomControl(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningCustomControlExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerWorkflow(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningWorkflowExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenSolutionExplorerWindow(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteOpeningSolutionExlorerWindow(selectedItem, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenImportJobExplorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteOpeningImportJobExlorerWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenSolutionImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteOpeningSolutionImageWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenSolutionDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteOpeningSolutionDifferenceImageWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenOrganizationDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteOpeningOrganizationDifferenceImageWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartAddingWebResourcesToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingWebResourcesToSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartAddingPluginAssemblyToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingPluginAssemblyToSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartAddingPluginAssemblyProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingPluginAssemblyProcessingStepsToSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartAddingPluginTypeProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> pluginTypeNames, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingPluginTypeProcessingStepsToSolution(connectionData, commonConfig, solutionUniqueName, pluginTypeNames, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartComparingPluginAssemblyAndLocalAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, string projectName, string defaultOutputFilePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginController.ExecuteComparingAssemblyAndCrmSolution(connectionData, commonConfig, projectName, defaultOutputFilePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.Project project, string defaultOutputFilePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginController.ExecuteUpdatingPluginAssembly(connectionData, commonConfig, project, defaultOutputFilePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartRegisterPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.Project project, string defaultOutputFilePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginController.ExecuteRegisterPluginAssembly(connectionData, commonConfig, project, defaultOutputFilePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartAddingReportsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingReportsToSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteShowingPluginTree(connectionData, commonConfig, entityFilter, pluginTypeFilter, messageFilter);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteShowingSdkMessageTree(connectionData, commonConfig, entityFilter, messageFilter);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter, EnvDTE.SelectedItem selectedItem)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteShowingSdkMessageRequestTree(connectionData, commonConfig, entityFilter, messageFilter, selectedItem);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingSystemUserExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._securityController.ExecuteShowingSystemUserExplorer(filter, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingTeamsExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._securityController.ExecuteShowingTeamsExplorer(filter, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingSecurityRolesExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._securityController.ExecuteShowingSecurityRolesExplorer(filter, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingPluginConfigurationTree(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginConfigurationController.ExecuteShowingPluginConfigurationTree(connectionData, commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingPluginConfigurationAssemblyDescription(CommonConfiguration commonConfig, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginConfigurationController.ExecuteShowingPluginConfigurationAssemblyDescription(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingPluginConfigurationTypeDescription(CommonConfiguration commonConfig, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginConfigurationController.ExecuteShowingPluginConfigurationTypeDescription(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartShowingPluginConfigurationComparer(CommonConfiguration commonConfig, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginConfigurationController.ExecuteShowingPluginConfigurationComparer(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartOrganizationComparer(ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._compareController.ExecuteOrganizationComparer(crmConfig, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckFileEncoding(List<SelectedFile> selectedFiles)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingFilesEncoding(selectedFiles);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartOpenFilesWithouUTF8Encoding(List<SelectedFile> selectedFiles)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteOpenFilesWithoutUTF8Encoding(selectedFiles);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartExportPluginConfigurationIntoFolder(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportPluginConfigurationController.ExecuteExportingPluginConfigurationIntoFolder(selectedItem, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCheckManagedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkManagedEntitiesController.ExecuteCheckingManagedEntities(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpeningFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool isTextEditor, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._openFilesController.ExecuteOpenFiles(selectedFiles, openFilesType, isTextEditor, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpeningReport(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOpenComponent action)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._linkController.ExecuteOpeningReport(commonConfig, connectionData, selectedFile, action);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpeningSolution(CommonConfiguration commonConfig, ConnectionData connectionData, string solutionUniqueName, ActionOpenComponent action)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._linkController.ExecuteOpeningSolutionAsync(commonConfig, connectionData, solutionUniqueName, action);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpeningWebResource(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOpenComponent action)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._linkController.ExecuteOpeningWebResource(commonConfig, connectionData, selectedFile, action);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartPublishAll(ConnectionData connectionData)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._publishController.ExecutePublishingAll(connectionData);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartTraceReaderWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._explorerController.ExecuteOpeningTraceReader(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }
    }
}