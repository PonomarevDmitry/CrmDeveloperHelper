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
        private IWriteToOutputAndPublishList _iWriteToOutputAndPublishList = null;

        private PublishController _publishController = null;
        private CompareController _compareController = null;
        private DifferenceController _differenceController = null;
        private DownloadController _downloadController = null;
        private CheckController _checkController = null;
        private ExportXmlController _exportXmlController = null;
        private PluginTreeController _pluginTreeController = null;
        private SolutionController _solutionController = null;
        private EntityMetadataController _entityMetadataController = null;
        private ExportPluginConfigurationController _exportPluginConfigurationController = null;
        private CheckPluginController _checkPluginController = null;
        private PluginTypeDescriptionController _pluginTypeDescriptionController = null;
        private ExportSolutionController _exportSolutionController = null;
        private CrmSvcUtilController _crmSvcUtilController = null;
        private CheckManagedEntitiesController _checkManagedEntitiesController = null;
        private OpenFilesController _openFilesController;
        private ReportController _reportController;
        private LinkController _linkController;

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
            this._exportSolutionController = new ExportSolutionController(outputWindow);
            this._crmSvcUtilController = new CrmSvcUtilController(outputWindow);
            this._checkManagedEntitiesController = new CheckManagedEntitiesController(outputWindow);
            this._openFilesController = new OpenFilesController(outputWindow);
            this._reportController = new ReportController(outputWindow);
            this._linkController = new LinkController(outputWindow);
        }

        /// <summary>
        /// Старт публикации веб-ресурсов
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartUpdateContentAndPublish(List<SelectedFile> selectedFiles, ConnectionConfiguration config)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._publishController.ExecuteUpdateContentAndPublish(selectedFiles, config);
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
        public void StartUpdateContentAndPublishEqualByText(List<SelectedFile> selectedFiles, ConnectionConfiguration config)
        {
            var worker = new Thread(() =>
            {
                try { this._publishController.ExecuteUpdateContentAndPublishEqualByText(selectedFiles, config); }
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
        public void StartComparing(List<SelectedFile> selectedFiles, ConnectionConfiguration config, bool withDetails)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._compareController.ExecuteComparingFilesAndWebResources(selectedFiles, config, withDetails);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void ShowingWebResourcesDependentComponents(List<SelectedFile> selectedFiles, ConnectionConfiguration crmConfig, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteShowingWebResourcesDependentComponents(crmConfig, connectionData, commonConfig, selectedFiles);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartComparingFilesWithWrongEncoding(List<SelectedFile> selectedFiles, ConnectionConfiguration config, bool withDetails)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._compareController.ExecuteComparingFilesWithWrongEncoding(selectedFiles, config, withDetails);
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

        public void StartAddingIntoPublishListFilesByType(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._compareController.ExecuteAddingIntoPublishListFilesByType(selectedFiles, openFilesType, crmConfig, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartMultiDifferenceFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteMultiDifferenceFiles(selectedFiles, openFilesType, crmConfig, commonConfig);
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
        public void StartReportDifference(SelectedFile selectedFile, bool isCustom, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteDifferenceReport(selectedFile, isCustom, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartReportThreeFileDifference(SelectedFile selectedFile, ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteThreeFileDifferenceReport(selectedFile, connectionData1, connectionData2, differenceType, commonConfig);
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
        public void StartClearingLastLink(List<SelectedFile> selectedFiles, ConnectionConfiguration config)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._linkController.ExecuteClearingLastLink(selectedFiles, config);
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
                    this._exportXmlController.ExecuteExportingRibbonXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartExportEntityAttributesDependentComponents(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteExportingEntityAttributesDependentComponents(selection, connectionData, commonConfig);
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

        public void StartCreatingFileWithGlobalOptionSets(ConnectionConfiguration crmConfig, ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteCreatingFileWithGlobalOptionSets(crmConfig, connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithGlobalOptionSets(ConnectionConfiguration crmConfig, ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdatingFileWithGlobalOptionSets(crmConfig, connectionData, commonConfig, selectedFiles, withSelect);
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

        public void StartOpenSolutionComponentExplorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteOpeningSolutionComponentWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartOpenExportSolutionWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._solutionController.ExecuteOpeningExportSolutionWindow(connectionData, commonConfig);
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

        public void StartShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginTreeController.ExecuteShowingPluginTree(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginTreeController.ExecuteShowingSdkMessageTree(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.Start();
        }

        public void StartShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginTreeController.ExecuteShowingSdkMessageRequestTree(connectionData, commonConfig, selection);
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

        public void StartExportSolution(EnvDTE.SelectedItem selectedItem, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportSolutionController.ExecuteExportingSolution(selectedItem, string.Empty, connectionData, commonConfig);
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

        public void UpdateProxyClasses(string filePath, ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._crmSvcUtilController.ExecuteUpdatingProxyClasses(filePath, crmConfig, commonConfig);
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

        public void StartOpeningFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool isTextEditor, ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._openFilesController.ExecuteOpenFiles(selectedFiles, openFilesType, isTextEditor, crmConfig, commonConfig);
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
    }
}