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
        private readonly DownloadController _downloadController;
        private readonly CheckController _checkController;
        private readonly ExportXmlController _exportXmlController;
        private readonly PluginTreeController _pluginTreeController;
        private readonly SolutionController _solutionController;
        private readonly EntityMetadataController _entityMetadataController;
        private readonly ExportPluginConfigurationController _exportPluginConfigurationController;
        private readonly CheckPluginController _checkPluginController;
        private readonly PluginTypeDescriptionController _pluginTypeDescriptionController;
        private readonly CrmSvcUtilController _crmSvcUtilController;
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
            this._downloadController = new DownloadController(outputWindow);
            this._checkController = new CheckController(outputWindow);
            this._exportXmlController = new ExportXmlController(outputWindow);
            this._pluginTreeController = new PluginTreeController(outputWindow);
            this._differenceController = new DifferenceController(outputWindow);
            this._solutionController = new SolutionController(outputWindow);
            this._entityMetadataController = new EntityMetadataController(outputWindow);
            this._exportPluginConfigurationController = new ExportPluginConfigurationController(outputWindow);
            this._checkPluginController = new CheckPluginController(outputWindow);
            this._pluginTypeDescriptionController = new PluginTypeDescriptionController(outputWindow);
            this._crmSvcUtilController = new CrmSvcUtilController(outputWindow);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartDownloadCustomWebResource(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._downloadController.ExecuteDownloadCustomWebResources(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartDownloadCustomReport(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._downloadController.ExecuteDownloadCustomReport(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    this._exportXmlController.ExecuteExportingApplicationRibbonXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportPluginTypeDescription(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginTypeDescriptionController.ExecuteCreatingPluginTypeDescription(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportPluginAssembly(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginTypeDescriptionController.ExecuteExportingPluginAssembly(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartCreatingFileWithEntityMetadata(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteCreatingFileWithEntityMetadata(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartOpenEntityAttributeExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityAttributeExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartOpenEntityKeyExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityKeyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartOpenEntityRelationshipOneToManyExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityRelationshipOneToManyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartOpenEntityRelationshipManyToManyExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityRelationshipManyToManyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartOpenEntitySecurityRolesExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntitySecurityRolesExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartCreatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteCreatingFileWithGlobalOptionSets(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdatingFileWithGlobalOptionSets(connectionData, commonConfig, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithEntityMetadata(List<SelectedFile> selectedFiles, ConnectionData connectionData, CommonConfiguration commonConfig, bool selectEntity)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdateFileWithEntityMetadata(selectedFiles, connectionData, commonConfig, selectEntity);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportSitemapXml(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteExportingSitemapXml(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportOrganizationInformation(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteExportingOrganizationInformation(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportSystemSavedQueryXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteExportingSystemSavedQueryXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportSystemSavedQueryVisualizationXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteExportingSystemSavedQueryVisualizationXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportSystemFormXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteExportingSystemFormXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportWorkflow(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteExportingWorkflow(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartOpenSolutionComponentExplorerWindow(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteOpeningSolutionComponentWindow(selectedItem, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartAddingWebResourcesIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingWebResourcesIntoSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartAddingPluginAssemblyIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingPluginAssemblyIntoSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartAddingPluginAssemblyProcessingStepsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingPluginAssemblyProcessingStepsIntoSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartAddingPluginTypeProcessingStepsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> pluginTypeNames, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingPluginTypeProcessingStepsIntoSolution(connectionData, commonConfig, solutionUniqueName, pluginTypeNames, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartComparingPluginAssemblyAndLocalAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, string projectName, string defaultFolder)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginTypeDescriptionController.ExecuteComparingAssemblyAndCrmSolution(connectionData, commonConfig, projectName, defaultFolder);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartAddingReportsIntoSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteAddingReportsIntoSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    this._pluginTreeController.ExecuteShowingPluginTree(connectionData, commonConfig, entityFilter, pluginTypeFilter, messageFilter);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    this._pluginTreeController.ExecuteShowingSdkMessageTree(connectionData, commonConfig, entityFilter, messageFilter);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginTreeController.ExecuteShowingSdkMessageRequestTree(connectionData, commonConfig, entityFilter, messageFilter);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    this._pluginTreeController.ExecuteShowingPluginConfigurationTree(connectionData, commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    this._pluginTreeController.ExecuteShowingPluginConfigurationAssemblyDescription(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    this._pluginTreeController.ExecuteShowingPluginConfigurationTypeDescription(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    this._pluginTreeController.ExecuteShowingPluginConfigurationComparer(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void UpdateProxyClasses(string filePath, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._crmSvcUtilController.ExecuteUpdatingProxyClasses(filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    DTEHelper.WriteExceptionToOutput(ex);
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
                    this._exportXmlController.ExecuteShowingTraceReader(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }
    }
}