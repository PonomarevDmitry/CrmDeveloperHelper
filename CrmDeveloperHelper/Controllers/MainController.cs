using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;

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
        private readonly FindsController _findsController;
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
            this._findsController = new FindsController(outputWindow);
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

        #region SiteMap

        public void StartSiteMapDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartSiteMapDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceSiteMap(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSiteMapUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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
        public void StartSiteMapUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateSiteMap(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }


        public void StartSiteMapOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        #endregion SiteMap

        #region SystemForm

        public void StartSystemFormDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartSystemFormDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceSystemForm(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSystemFormUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartSystemFormUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateSystemForm(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSystemFormOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        #endregion SystemForm

        #region SavedQuery

        public void StartSavedQueryDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartSavedQueryDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceSavedQuery(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSavedQueryUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartSavedQueryUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateSavedQuery(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartSavedQueryOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        #endregion SavedQuery

        #region Workflow

        public void StartWorkflowDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceWorkflow(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWorkflowDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceWorkflow(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWorkflowUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateWorkflow(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWorkflowUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateWorkflow(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWorkflowOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteOpenInWebWorkflow(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        #endregion Workflow

        #region RibbonDiff

        public void StartRibbonDiffXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartRibbonDiffXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteDifferenceRibbonDiffXml(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartRibbonDiffXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartRibbonDiffXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdateRibbonDiffXml(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        #endregion RibbonDiff

        #region Ribbon

        public void StartRibbonDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartRibbonDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteDifferenceRibbon(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartEntityRibbonOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteEntityRibbonOpenInWeb(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartRibbonExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpenRibbonExplorer(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        #endregion Ribbon

        #region WebResource

        /// <summary>
        /// Старт публикации веб-ресурсов
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartUpdateContentAndPublish(ConnectionData connectionData, List<SelectedFile> selectedFiles)
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
        public void StartUpdateContentAndPublishEqualByText(ConnectionData connectionData, List<SelectedFile> selectedFiles)
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
        public void StartComparing(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
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

        public void ShowingWebResourcesDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
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

        public void StartComparingFilesWithWrongEncoding(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
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
        public void StartWebResourceDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool isCustom)
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

        public void StartWebResourceCreateEntityDescription(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteCreatingWebResourceEntityDescription(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWebResourceGetAttribute(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteWebResourceGettingAttribute(selectedFile, fieldName, fieldTitle, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWebResourceChangeInEntityEditor(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteChangingWebResourceInEntityEditor(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWebResourceThreeFileDifference(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, ShowDifferenceThreeFileType differenceType)
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

        public void StartWebResourceMultiDifferenceFiles(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._differenceController.ExecuteWebResourceMultiDifferenceFiles(selectedFiles, openFilesType, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartCreatingLastLinkWebResourceMultiple(ConnectionData connectionData, List<SelectedFile> selectedFiles)
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

        public void StartWebResourceDependencyXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceWebResourceDependencyXml(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWebResourceDependencyXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteDifferenceWebResourceDependencyXml(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWebResourceDependencyXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateWebResourceDependencyXml(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWebResourceDependencyXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteUpdateWebResourceDependencyXml(doc, filePath, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartWebResourceDependencyXmlOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._exportXmlController.ExecuteOpenInWebWebResourceDependencyXml(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        #endregion WebResource

        public void StartAddingIntoPublishListFilesByType(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
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

        #region Report

        /// <summary>
        /// Старт различия отчетов
        /// </summary>
        /// <param name="selectedFile"></param>
        /// <param name="isCustom"></param>
        /// <param name="crmConfig"></param>
        /// <param name="commonConfig"></param>
        public void StartReportDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle, bool isCustom)
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

        public void StartReportThreeFileDifference(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle, ShowDifferenceThreeFileType differenceType)
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

        public void StartReportUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
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

        public void StartCreatingLastLinkReport(ConnectionData connectionData, SelectedFile selectedFile)
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

        #endregion Report

        /// <summary>
        /// Старт очистки старых линков
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartClearingLastLink(ConnectionData connectionData, List<SelectedFile> selectedFiles)
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

        #region Finds

        /// <summary>
        /// Проверка CRM на существование сущностей с префиксом new_.
        /// </summary>
        public void StartFindEntityObjectsByPrefix(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindingEntityObjectsByPrefix(connectionData, commonConfig, prefix);
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
        public void StartFindEntityObjectsByPrefixInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindingEntityObjectsByPrefixInExplorer(connectionData, commonConfig, prefix);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityObjectsByPrefixAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindingEntityObjectsByPrefixAndShowDependentComponents(connectionData, commonConfig, prefix);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindMarkedToDeleteAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindingMarkedToDeleteAndShowDependentComponents(connectionData, commonConfig, prefix);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindMarkedToDeleteInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindingMarkedToDeleteInExplorer(connectionData, commonConfig, prefix);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityObjectsByName(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindEntityElementsByName(connectionData, commonConfig, name);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityObjectsByNameInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindEntityElementsByNameInExplorer(connectionData, commonConfig, name);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityObjectsContainsString(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindEntityElementsContainsString(connectionData, commonConfig, name);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartFindEntityObjectsContainsStringInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._findsController.ExecuteFindEntityElementsContainsStringInExplorer(connectionData, commonConfig, name);
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
                    this._findsController.ExecuteFindEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
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
                    this._findsController.ExecuteEditEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
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
                    this._findsController.ExecuteFindEntityByUniqueidentifier(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        #endregion Finds

        #region Checks

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

        public void StartCheckUnknownFormControlTypes(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._checkController.ExecuteCheckingUnknownFormControlType(connectionData, commonConfig);
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

        #endregion Checks

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

        public void StartExportRibbonXml(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartOpenPluginTypeExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartAddPluginStep(ConnectionData connectionData, CommonConfiguration commonConfig, string pluginTypeName)
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

        public void StartOpenPluginAssemblyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartOpeningEntityMetadataExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityMetadataExplorer(selection, selectedItem, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartOpeningEntityMetadataFileGenerationOptions()
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningEntityMetadataFileGenerationOptions();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartOpeningGlobalOptionSetsMetadataFileGenerationOptions()
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningGlobalOptionSetsMetadataFileGenerationOptions();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        public void StartExplorerEntityAttribute(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartExplorerEntityKey(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartExplorerEntityRelationshipOneToMany(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartExplorerEntityRelationshipManyToMany(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartExplorerEntityPrivileges(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartExplorerOtherPrivileges(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteOpeningOtherPrivilegesExplorer(selection, connectionData, commonConfig);
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

        public void StartUpdatingFileWithEntityMetadataCSharpSchema(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
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

        public void StartUpdatingFileWithEntityMetadataCSharpProxyClassOrSchema(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._entityMetadataController.ExecuteUpdateFileWithEntityMetadataCSharpProxyClassOrSchema(selectedFiles, connectionData, commonConfig, selectEntity, openOptions);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartUpdatingFileWithEntityMetadataCSharpProxyClass(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
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

        public void StartUpdatingFileWithEntityMetadataJavaScript(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
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

        public void StartUpdatingFileWithGlobalOptionSetSingleJavaScript(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
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

        public void StartExplorerSystemSavedQueryXml(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartExplorerSystemSavedQueryVisualization(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartExplorerCustomControl(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartExplorerWorkflow(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
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

        public void StartOpenSolutionExplorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.SelectedItem selectedItem)
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

        public void StartUpdatingPluginAssembliesInWindow(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginController.ExecuteUpdatingPluginAssembliesInWindow(connectionData, commonConfig, projectList);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartBuildProjectUpdatePluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList, bool registerPlugins)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginController.ExecuteBuildingProjectAndUpdatingPluginAssembly(connectionData, commonConfig, projectList, registerPlugins);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        public void StartRegisterPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, List<EnvDTE.Project> projectList)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    this._pluginController.ExecuteRegisterPluginAssembly(connectionData, commonConfig, projectList);
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

        public void StartExportPluginConfigurationIntoFolder(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.SelectedItem selectedItem)
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

        public void StartOpeningFiles(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool isTextEditor)
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

        public void StartOpeningReport(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, ActionOpenComponent action)
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

        public void StartOpeningSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, ActionOpenComponent action)
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

        public void StartOpeningWebResource(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, ActionOpenComponent action)
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