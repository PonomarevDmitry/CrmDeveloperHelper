using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        private void ExecuteWithConnectionInThreadVoid<T1>(ConnectionData connectionData, Action<ConnectionData, T1> action, T1 arg1)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData, arg1);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteWithConnectionInThreadVoid<T1, T2>(ConnectionData connectionData, Action<ConnectionData, T1, T2> action, T1 arg1, T2 arg2)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData, arg1, arg2);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteWithConnectionInThread(ConnectionData connectionData, Func<ConnectionData, Task> action)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteWithConnectionInThread<T1>(ConnectionData connectionData, Func<ConnectionData, T1, Task> action, T1 arg1)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData, arg1);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteWithConnectionInThread<T1, T2>(ConnectionData connectionData, Func<ConnectionData, T1, T2, Task> action, T1 arg1, T2 arg2)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData, arg1, arg2);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteWithConnectionInThread<T1, T2, T3>(ConnectionData connectionData, Func<ConnectionData, T1, T2, T3, Task> action, T1 arg1, T2 arg2, T3 arg3)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData, arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteWithConnectionInThread<T1, T2, T3, T4>(ConnectionData connectionData, Func<ConnectionData, T1, T2, T3, T4, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData, arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteWithConnectionInThread<T1, T2, T3, T4, T5>(ConnectionData connectionData, Func<ConnectionData, T1, T2, T3, T4, T5, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData, arg1, arg2, arg3, arg4, arg5);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteWithConnectionInThread<T1, T2, T3, T4, T5, T6>(ConnectionData connectionData, Func<ConnectionData, T1, T2, T3, T4, T5, T6, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(connectionData, arg1, arg2, arg3, arg4, arg5, arg6);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThreadVoid(Action action)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThreadVoid<T1>(Action<T1> action, T1 art1)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(art1);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThreadVoid<T1, T2>(Action<T1, T2> action, T1 art1, T2 arg2)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(art1, arg2);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThread<T1>(Func<T1, Task> action, T1 arg1)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(arg1);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThread<T1, T2>(Func<T1, T2, Task> action, T1 arg1, T2 arg2)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(arg1, arg2);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThread<T1, T2, T3>(Func<T1, T2, T3, Task> action, T1 arg1, T2 arg2, T3 arg3)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThread<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThread<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThread<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        private void ExecuteInThread<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.Start();
        }

        #region SiteMap

        public void StartSiteMapDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceSiteMap, commonConfig, selectedFile);

        public void StartSiteMapDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceSiteMap, commonConfig, doc, filePath);

        public void StartSiteMapUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateSiteMap, commonConfig, selectedFile);

        public void StartSiteMapUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateSiteMap, commonConfig, doc, filePath);

        public void StartSiteMapOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteOpenInWebSiteMap, commonConfig, selectedFile);

        public void StartSiteMapGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteGetSiteMapCurrentXml, commonConfig, selectedFile);

        #endregion SiteMap

        #region SystemForm

        public void StartSystemFormDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceSystemForm, commonConfig, selectedFile);

        public void StartSystemFormDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceSystemForm, commonConfig, doc, filePath);

        public void StartSystemFormUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateSystemForm, commonConfig, selectedFile);

        public void StartSystemFormUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateSystemForm, commonConfig, doc, filePath);

        public void StartSystemFormOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteOpenInWebSystemForm, commonConfig, selectedFile);

        public void StartSystemFormGetCurrentFormXml(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteGetSystemFormCurrentXml, commonConfig, selectedFile);

        public void StartSystemFormGetCurrentAttribute(ConnectionData connectionData, CommonConfiguration commonConfig, Guid formId, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteGetSystemFormCurrentAttribute, commonConfig, formId, actionOnComponent, fieldName, fieldTitle);

        public void StartOpeningLinkedSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, ActionOnComponent actionOnComponent, string entityName, Guid formId, int formType)
             => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteOpeningLinkedSystemForm, commonConfig, actionOnComponent, entityName, formId, formType);

        public void StartAddingLinkedSystemFormToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool withSelect, string entityName, Guid formId, int formType)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteAddingLinkedSystemFormToSolution, commonConfig, solutionUniqueName, withSelect, entityName, formId, formType);
        public void StartLinkedSystemFormChangeInEntityEditor(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, Guid formId, int formType)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteChangingLinkedSystemFormInEntityEditor, commonConfig, entityName, formId, formType);

        public void StartSystemFormGetCurrentTabsAndSections(ConnectionData connectionData, CommonConfiguration commonConfig, JavaScriptObjectType jsObjectType, string entityName, Guid formId, int formType)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteGettingSystemFormCurrentTabsAndSections, commonConfig, jsObjectType, entityName, formId, formType);

        #endregion SystemForm

        #region SavedQuery

        public void StartSavedQueryDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceSavedQuery, commonConfig, selectedFile);

        public void StartSavedQueryDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceSavedQuery, commonConfig, doc, filePath);

        public void StartSavedQueryUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateSavedQuery, commonConfig, selectedFile);

        public void StartSavedQueryUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateSavedQuery, commonConfig, doc, filePath);

        public void StartSavedQueryOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteOpenInWebSavedQuery, commonConfig, selectedFile);

        public void StartSavedQueryGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteGetSavedQueryCurrentXml, commonConfig, selectedFile);

        #endregion SavedQuery

        #region Workflow

        public void StartWorkflowDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceWorkflow, commonConfig, selectedFile);

        public void StartWorkflowDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceWorkflow, commonConfig, doc, filePath);

        public void StartWorkflowUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateWorkflow, commonConfig, selectedFile);

        public void StartWorkflowUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateWorkflow, commonConfig, doc, filePath);

        public void StartWorkflowOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteOpenInWebWorkflow, commonConfig, selectedFile);

        public void StartWorkflowGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteGetWorkflowCurrentXaml, commonConfig, selectedFile);

        #endregion Workflow

        #region RibbonDiff

        public void StartRibbonDiffXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteRibbonDiffXmlDifference, commonConfig, selectedFile);

        public void StartRibbonDiffXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteRibbonDiffXmlDifference, commonConfig, doc, filePath);

        public void StartRibbonDiffXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteRibbonDiffXmlUpdate, commonConfig, selectedFile);

        public void StartRibbonDiffXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteRibbonDiffXmlUpdate, commonConfig, doc, filePath);

        public void StartRibbonDiffXmlGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteRibbonDiffXmlGetCurrent, commonConfig, selectedFile);

        public void StartOpeningEntityMetadataInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteOpeningEntityMetadataInWeb, commonConfig, entityName, entityTypeCode);

        public void StartOpeningEntityListInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteOpeningEntityListInWeb, commonConfig, entityName, entityTypeCode);

        public void StartOpeningEntityFetchXmlFile(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteOpeningEntityFetchXmlFile, commonConfig, entityName, entityTypeCode);

        #endregion RibbonDiff

        #region Ribbon

        public void StartRibbonDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteRibbonDifference, commonConfig, selectedFile);

        public void StartRibbonDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteRibbonDifference, commonConfig, doc, filePath);

        public void StartEntityRibbonOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteEntityRibbonOpenInWeb, commonConfig, selectedFile);

        public void StartRibbonExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteOpenRibbonExplorer, commonConfig, selectedFile);

        public void StartRibbonGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteRibbonGetCurrent, commonConfig, selectedFile);

        #endregion Ribbon

        public void StartEntityMetadataOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, ActionOnComponent actionOnComponent)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteEntityMetadataOpenInWeb, commonConfig, entityName, actionOnComponent);

        public void StartPublishEntityMetadata(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecutePublishEntity, commonConfig, entityName);

        public void StartAddingEntityToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool withSelect, string entityName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteAddingEntityToSolution, commonConfig, solutionUniqueName, withSelect, entityName, rootComponentBehavior);

        #region WebResource

        /// <summary>
        /// Старт публикации веб-ресурсов
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartUpdateContentAndPublish(ConnectionData connectionData, List<SelectedFile> selectedFiles)
            => ExecuteWithConnectionInThread(connectionData, this._publishController.ExecuteUpdateContentAndPublish, selectedFiles);

        /// <summary>
        /// Старт публикации веб-ресурсов разных по содержанию, но одинаковых по тексту
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartUpdateContentAndPublishEqualByText(ConnectionData connectionData, List<SelectedFile> selectedFiles)
            => ExecuteWithConnectionInThread(connectionData, this._publishController.ExecuteUpdateContentAndPublishEqualByText, selectedFiles);

        public void StartIncludeReferencesToDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
            => ExecuteWithConnectionInThread(connectionData, this._publishController.ExecuteIncludeReferencesToDependencyXml, commonConfig, selectedFiles);

        public void StartUpdateContentIncludeReferencesToDependencyXml(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
            => ExecuteWithConnectionInThread(connectionData, this._publishController.ExecuteUpdateContentIncludeReferencesToDependencyXml, commonConfig, selectedFiles);

        public void StartIncludeReferencesToLinkedSystemFormsLibraries(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
            => ExecuteWithConnectionInThread(connectionData, this._publishController.ExecuteIncludeReferencesToLinkedSystemFormsLibraries, commonConfig, selectedFiles);

        /// <summary>
        /// Старт сравнения
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartWebResourceComparing(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
            => ExecuteWithConnectionInThread(connectionData, this._compareController.ExecuteComparingFilesAndWebResources, selectedFiles, withDetails);

        public void ShowingWebResourcesDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles)
            => ExecuteWithConnectionInThread(connectionData, this._checkController.ExecuteShowingWebResourcesDependentComponents, commonConfig, selectedFiles);

        public void StartComparingFilesWithWrongEncoding(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
            => ExecuteWithConnectionInThread(connectionData, this._compareController.ExecuteComparingFilesWithWrongEncoding, selectedFiles, withDetails);

        /// <summary>
        /// Старт различия
        /// </summary>
        /// <param name="selectedFile"></param>
        /// <param name="isCustom"></param>
        /// <param name="crmConfig"></param>
        /// <param name="commonConfig"></param>
        public void StartWebResourceDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool isCustom)
            => ExecuteWithConnectionInThread(connectionData, this._differenceController.ExecuteDifferenceWebResources, commonConfig, selectedFile, isCustom);

        public void StartWebResourceCreateEntityDescription(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._differenceController.ExecuteCreatingWebResourceEntityDescription, commonConfig, selectedFile);

        public void StartWebResourceGetAttribute(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle)
            => ExecuteWithConnectionInThread(connectionData, this._differenceController.ExecuteWebResourceGettingAttribute, commonConfig, selectedFile, fieldName, fieldTitle);

        public void StartWebResourceChangeInEntityEditor(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._differenceController.ExecuteChangingWebResourceInEntityEditor, commonConfig, selectedFile);

        public void StartWebResourceThreeFileDifference(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, ShowDifferenceThreeFileType differenceType)
            => ExecuteInThread(this._differenceController.ExecuteThreeFileDifferenceWebResources, connectionData1, connectionData2, commonConfig, selectedFile, differenceType);

        public void StartOpenWebResourceExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningWebResourceExplorer, commonConfig, selection);

        public void StartWebResourceMultiDifferenceFiles(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
            => ExecuteWithConnectionInThread(connectionData, this._differenceController.ExecuteWebResourceMultiDifferenceFiles, commonConfig, selectedFiles, openFilesType);

        public void StartWebResourceCreatingLastLinkMultiple(ConnectionData connectionData, List<SelectedFile> selectedFiles)
            => ExecuteWithConnectionInThread(connectionData, this._linkController.ExecuteCreatingLastLinkWebResourceMultiple, selectedFiles);

        public void StartWebResourceDependencyXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceWebResourceDependencyXml, commonConfig, selectedFile);

        public void StartWebResourceDependencyXmlDifference(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteDifferenceWebResourceDependencyXml, commonConfig, doc, filePath);

        public void StartWebResourceDependencyXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateWebResourceDependencyXml, commonConfig, selectedFile);

        public void StartWebResourceDependencyXmlUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, XDocument doc, string filePath)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteUpdateWebResourceDependencyXml, commonConfig, doc, filePath);

        public void StartWebResourceDependencyXmlOpenInWeb(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteOpenInWebWebResourceDependencyXml, commonConfig, selectedFile);

        public void StartWebResourceDependencyXmlGetCurrent(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteGetWebResourceCurrentDependencyXml, commonConfig, selectedFile);

        #endregion WebResource

        public void StartAddingIntoPublishListFilesByType(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
            => ExecuteWithConnectionInThread(connectionData, this._compareController.ExecuteAddingIntoPublishListFilesByType, commonConfig, selectedFiles, openFilesType);

        #region Report

        /// <summary>
        /// Старт различия отчетов
        /// </summary>
        /// <param name="selectedFile"></param>
        /// <param name="isCustom"></param>
        /// <param name="crmConfig"></param>
        /// <param name="commonConfig"></param>
        public void StartReportDifference(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle, bool isCustom)
            => ExecuteWithConnectionInThread(connectionData, this._differenceController.ExecuteDifferenceReport, commonConfig, selectedFile, fieldName, fieldTitle, isCustom);

        public void StartReportThreeFileDifference(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle, ShowDifferenceThreeFileType differenceType)
            => ExecuteInThread(this._differenceController.ExecuteThreeFileDifferenceReport, connectionData1, connectionData2, commonConfig, selectedFile, fieldName, fieldTitle, differenceType);

        public void StartReportUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._reportController.ExecuteUpdatingReport, commonConfig, selectedFile);

        public void StartOpenReportExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningReportExplorer, commonConfig, selection);

        public void StartReportCreatingLastLink(ConnectionData connectionData, SelectedFile selectedFile)
            => ExecuteWithConnectionInThread(connectionData, this._linkController.ExecuteCreatingLastLinkReport, selectedFile);

        #endregion Report

        /// <summary>
        /// Старт очистки старых линков
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="config"></param>
        public void StartClearingLastLink(ConnectionData connectionData, List<SelectedFile> selectedFiles)
            => ExecuteWithConnectionInThreadVoid(connectionData, this._linkController.ExecuteClearingLastLink, selectedFiles);

        #region Finds

        /// <summary>
        /// Проверка CRM на существование сущностей с префиксом new_.
        /// </summary>
        public void StartFindEntityObjectsByPrefix(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindingEntityObjectsByPrefix, commonConfig, prefix);

        /// <summary>
        /// Проверка CRM на существование сущностей с префиксом new_.
        /// </summary>
        public void StartFindEntityObjectsByPrefixInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindingEntityObjectsByPrefixInExplorer, commonConfig, prefix);

        public void StartFindEntityObjectsByPrefixAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindingEntityObjectsByPrefixAndShowDependentComponents, commonConfig, prefix);

        public void StartFindMarkedToDeleteAndShowDependentComponents(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindingMarkedToDeleteAndShowDependentComponents, commonConfig, prefix);

        public void StartFindMarkedToDeleteInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string prefix)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindingMarkedToDeleteInExplorer, commonConfig, prefix);

        public void StartFindEntityObjectsByName(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindEntityElementsByName, commonConfig, name);

        public void StartFindEntityObjectsByNameInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindEntityElementsByNameInExplorer, commonConfig, name);

        public void StartFindEntityObjectsContainsString(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindEntityElementsContainsString, commonConfig, name);

        public void StartFindEntityObjectsContainsStringInExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string name)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindEntityElementsContainsStringInExplorer, commonConfig, name);

        public void StartFindEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindEntityById, commonConfig, entityName, entityTypeCode, entityId);

        public void StartEditEntityById(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteEditEntityById, commonConfig, entityName, entityTypeCode, entityId);

        public void StartFindEntityByUniqueidentifier(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, int? entityTypeCode, Guid entityId)
            => ExecuteWithConnectionInThread(connectionData, this._findsController.ExecuteFindEntityByUniqueidentifier, commonConfig, entityName, entityTypeCode, entityId);

        #endregion Finds

        #region Checks

        public void StartCheckPluginImages(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkPluginController.ExecuteCheckingPluginImages, commonConfig);

        public void StartCheckGlobalOptionSetDuplicates(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkController.ExecuteCheckingGlobalOptionSetDuplicates, commonConfig);

        public void StartCheckComponentTypeEnum(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkController.ExecuteCheckingComponentTypeEnum, commonConfig);

        public void StartCheckUnknownFormControlTypes(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkController.ExecuteCheckingUnknownFormControlType, commonConfig);

        public void StartCreateAllDependencyNodesDescription(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkController.ExecuteCreatingAllDependencyNodesDescription, commonConfig);

        public void StartCheckPluginImagesRequiredComponents(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkPluginController.ExecuteCheckingPluginImagesRequiredComponents, commonConfig);

        public void StartCheckPluginStepsRequiredComponents(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkPluginController.ExecuteCheckingPluginStepsRequiredComponents, commonConfig);

        public void StartCheckPluginSteps(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkPluginController.ExecuteCheckingPluginSteps, commonConfig);

        public void StartCheckEntitiesOwnership(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkController.ExecuteCheckingEntitiesOwnership, commonConfig);

        public void StartCheckingWorkflowsUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkController.ExecuteCheckingWorkflowsUsedEntities, commonConfig);

        public void ExecuteCheckingWorkflowsNotExistingUsedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkController.ExecuteCheckingWorkflowsNotExistingUsedEntities, commonConfig);

        #endregion Checks

        public void StartExportingFormEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._exportXmlController.ExecuteExportingFormsEvents, commonConfig);

        public void StartOpenApplicationRibbonExplorer(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningApplicationRibbonExplorer, commonConfig);

        public void StartExportPluginConfiguration(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._exportPluginConfigurationController.ExecuteExportingPluginConfigurationXml, commonConfig);

        public void StartOpenPluginTypeExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningPluginTypeExplorer, commonConfig, selection);

        public void StartAddPluginStep(ConnectionData connectionData, CommonConfiguration commonConfig, string pluginTypeName)
            => ExecuteWithConnectionInThread(connectionData, this._pluginController.ExecuteAddingPluginStepForType, commonConfig, pluginTypeName);

        public void StartOpenPluginAssemblyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningPluginAssemblyExplorer, commonConfig, selection);

        public void StartOpeningEntityMetadataExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningEntityMetadataExplorer, commonConfig, selection, selectedItem);

        public void StartOpeningEntityMetadataFileGenerationOptions()
            => ExecuteInThreadVoid(this._explorerController.ExecuteOpeningEntityMetadataFileGenerationOptions);

        public void StartOpeningJavaScriptFileGenerationOptions()
            => ExecuteInThreadVoid(this._explorerController.ExecuteOpeningJavaScriptFileGenerationOptions);

        public void StartOpeningGlobalOptionSetsMetadataFileGenerationOptions()
            => ExecuteInThreadVoid(this._explorerController.ExecuteOpeningGlobalOptionSetsMetadataFileGenerationOptions);

        public void StartOpeningFileGenerationOptions()
            => ExecuteInThreadVoid(this._explorerController.ExecuteOpeningFileGenerationOptions);

        public void StartOpeningFileGenerationConfiguration()
            => ExecuteInThreadVoid(this._explorerController.ExecuteOpeningFileGenerationConfiguration);

        public void StartExplorerEntityAttribute(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningEntityAttributeExplorer, commonConfig, selection);

        public void StartExplorerEntityKey(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningEntityKeyExplorer, commonConfig, selection);

        public void StartExplorerEntityRelationshipOneToMany(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningEntityRelationshipOneToManyExplorer, commonConfig, selection);

        public void StartExplorerEntityRelationshipManyToMany(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningEntityRelationshipManyToManyExplorer, commonConfig, selection);

        public void StartExplorerEntityPrivileges(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningEntityPrivilegesExplorer, commonConfig, selection);

        public void StartExplorerOtherPrivileges(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningOtherPrivilegesExplorer, commonConfig, selection);

        public void StartCreatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteCreatingFileWithGlobalOptionSets, commonConfig, selection, selectedItem);

        public void StartCSharpEntityMetadataUpdatingFileWithSchema(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteUpdateFileWithEntityMetadataCSharpSchema, commonConfig, selectedFiles, selectEntity, openOptions);

        public void StartCSharpEntityMetadataUpdatingFileWithProxyClassOrSchema(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteUpdateFileWithEntityMetadataCSharpProxyClassOrSchema, commonConfig, selectedFiles, selectEntity, openOptions);

        public void StartCSharpEntityMetadataUpdatingFileWithProxyClass(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteUpdateFileWithEntityMetadataCSharpProxyClass, commonConfig, selectedFiles, selectEntity, openOptions);

        public void StartJavaScriptEntityMetadataFileUpdatingSchema(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteUpdateFileWithEntityMetadataJavaScript, commonConfig, selectedFiles, selectEntity, openOptions);

        public void StartCSharpGlobalOptionSetsFileUpdatingSchema(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, bool withSelect, bool openOptions)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteUpdatingFileWithGlobalOptionSetCSharp, commonConfig, selectedFiles, withSelect, openOptions);

        public void StartJavaScriptGlobalOptionSetFileUpdatingSingle(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, bool selectEntity, bool openOptions)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteUpdatingFileWithGlobalOptionSetSingleJavaScript, commonConfig, selectedFiles, selectEntity, openOptions);

        public void StartJavaScriptGlobalOptionSetFileUpdatingAll(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool openOptions)
            => ExecuteWithConnectionInThread(connectionData, this._entityMetadataController.ExecuteUpdatingFileWithGlobalOptionSetAllJavaScript, commonConfig, selectedFile, openOptions);

        public void StartExplorerSiteMapXml(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningSiteMapExplorer, commonConfig, filter);

        public void StartExplorerOrganizationInformation(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningOrganizationExplorer, commonConfig);

        public void StartExplorerSystemSavedQueryXml(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningSystemSavedQueryExplorer, commonConfig, selection);

        public void StartExplorerSystemSavedQueryVisualization(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningSystemSavedQueryVisualizationExplorer, commonConfig, selection);

        public void StartExplorerSystemForm(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, string selection, EnvDTE.SelectedItem selectedItem)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningSystemFormExplorer, commonConfig, entityName, selection, selectedItem);

        public void StartExplorerCustomControl(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningCustomControlExplorer, commonConfig, selection);

        public void StartExplorerWorkflow(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningWorkflowExplorer, commonConfig, selection);

        public void StartOpenSolutionExplorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.SelectedItem selectedItem)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteOpeningSolutionExlorerWindow, commonConfig, selectedItem);

        public void StartOpenImportJobExplorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteOpeningImportJobExlorerWindow, commonConfig);

        public void StartOpenSolutionImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThreadVoid(connectionData, this._solutionController.ExecuteOpeningSolutionImageWindow, commonConfig);

        public void StartOpenSolutionDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThreadVoid(connectionData, this._solutionController.ExecuteOpeningSolutionDifferenceImageWindow, commonConfig);

        public void StartOpenOrganizationDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThreadVoid(connectionData, this._solutionController.ExecuteOpeningOrganizationDifferenceImageWindow, commonConfig);

        public void StartAddingWebResourcesToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteAddingWebResourcesToSolution, commonConfig, solutionUniqueName, selectedFiles, withSelect);

        public void StartPluginAssemblyAddingToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteAddingPluginAssemblyToSolution, commonConfig, solutionUniqueName, projectNames, withSelect);

        public void StartPluginAssemblyAddingProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> projectNames, bool withSelect)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteAddingPluginAssemblyProcessingStepsToSolution, commonConfig, solutionUniqueName, projectNames, withSelect);

        public void StartPluginTypeAddingProcessingStepsToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<string> pluginTypeNames, bool withSelect)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteAddingPluginTypeProcessingStepsToSolution, commonConfig, solutionUniqueName, pluginTypeNames, withSelect);

        public void StartPluginAssemblyComparingWithLocalAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, string projectName, string defaultOutputFilePath)
            => ExecuteWithConnectionInThread(connectionData, this._pluginController.ExecuteComparingAssemblyAndCrmSolution, commonConfig, projectName, defaultOutputFilePath);

        public void StartPluginAssemblyUpdatingInWindow(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
            => ExecuteWithConnectionInThread(connectionData, this._pluginController.ExecuteUpdatingPluginAssembliesInWindow, commonConfig, projectList);

        public void StartPluginAssemblyBuildProjectUpdate(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList, bool registerPlugins)
            => ExecuteWithConnectionInThread(connectionData, this._pluginController.ExecuteBuildingProjectAndUpdatingPluginAssembly, commonConfig, projectList, registerPlugins);

        public void StartActionOnPluginAssembly(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList, ActionOnComponent actionOnComponent)
            => ExecuteWithConnectionInThread(connectionData, this._pluginController.ExecuteActionOnProjectPluginAssembly, commonConfig, projectList, actionOnComponent);

        public void StartActionOnPluginTypes(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<string> pluginTypeNames, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            => ExecuteWithConnectionInThread(connectionData, this._pluginController.ExecuteActionOnPluginTypes, commonConfig, pluginTypeNames, actionOnComponent, fieldName, fieldTitle);

        public void StartPluginAssemblyRegister(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<EnvDTE.Project> projectList)
            => ExecuteWithConnectionInThread(connectionData, this._pluginController.ExecuteRegisterPluginAssembly, commonConfig, projectList);

        public void StartReportAddingToSolution(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, IEnumerable<SelectedFile> selectedFiles, bool withSelect)
            => ExecuteWithConnectionInThread(connectionData, this._solutionController.ExecuteAddingReportsToSolution, commonConfig, solutionUniqueName, selectedFiles, withSelect);

        public void StartShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteShowingPluginTree, commonConfig, entityFilter, pluginTypeFilter, messageFilter);

        public void StartShowingSdkMessageExplorer(ConnectionData connectionData, CommonConfiguration commonConfig,  string messageFilter)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteShowingSdkMessageExplorer, commonConfig,  messageFilter);

        public void StartShowingSdkMessageFilterExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string messageFilter)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteShowingSdkMessageFilterExplorer, commonConfig, messageFilter);

        public void StartShowingSdkMessageFilterTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteShowingSdkMessageFilterTree, commonConfig, entityFilter, messageFilter);

        public void StartShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter, EnvDTE.SelectedItem selectedItem)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteShowingSdkMessageRequestTree, commonConfig, entityFilter, messageFilter, selectedItem);

        public void StartShowingSystemUserExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
            => ExecuteWithConnectionInThread(connectionData, this._securityController.ExecuteShowingSystemUserExplorer, commonConfig, filter);

        public void StartShowingTeamsExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
            => ExecuteWithConnectionInThread(connectionData, this._securityController.ExecuteShowingTeamsExplorer, commonConfig, filter);

        public void StartShowingSecurityRolesExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
            => ExecuteWithConnectionInThread(connectionData, this._securityController.ExecuteShowingSecurityRolesExplorer, commonConfig, filter);

        public void StartShowingPluginConfigurationTree(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
            => ExecuteWithConnectionInThreadVoid(connectionData, this._pluginConfigurationController.ExecuteShowingPluginConfigurationTree, commonConfig, filePath);

        public void StartShowingPluginConfigurationAssemblyDescription(CommonConfiguration commonConfig, string filePath)
            => ExecuteInThreadVoid(this._pluginConfigurationController.ExecuteShowingPluginConfigurationAssemblyDescription, commonConfig, filePath);

        public void StartShowingPluginConfigurationTypeDescription(CommonConfiguration commonConfig, string filePath)
            => ExecuteInThreadVoid(this._pluginConfigurationController.ExecuteShowingPluginConfigurationTypeDescription, commonConfig, filePath);

        public void StartShowingPluginConfigurationComparer(CommonConfiguration commonConfig, string filePath)
            => ExecuteInThreadVoid(this._pluginConfigurationController.ExecuteShowingPluginConfigurationComparer, commonConfig, filePath);

        public void StartOrganizationComparer(ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
            => ExecuteInThreadVoid(this._compareController.ExecuteOrganizationComparer, crmConfig, commonConfig);

        public void StartCheckFileEncoding(List<SelectedFile> selectedFiles)
            => ExecuteInThreadVoid(this._checkController.ExecuteCheckingFilesEncoding, selectedFiles);

        public void StartOpenFilesWithouUTF8Encoding(List<SelectedFile> selectedFiles)
            => ExecuteInThreadVoid(this._checkController.ExecuteOpenFilesWithoutUTF8Encoding, selectedFiles);

        public void StartExportPluginConfigurationIntoFolder(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.SelectedItem selectedItem)
            => ExecuteWithConnectionInThread(connectionData, this._exportPluginConfigurationController.ExecuteExportingPluginConfigurationIntoFolder, commonConfig, selectedItem);

        public void StartCheckManagedEntities(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._checkManagedEntitiesController.ExecuteCheckingManagedEntities, commonConfig);

        public void StartOpeningFiles(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool isTextEditor)
            => ExecuteWithConnectionInThread(connectionData, this._openFilesController.ExecuteOpenFiles, commonConfig, selectedFiles, openFilesType, isTextEditor);

        public void StartOpeningReport(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
            => ExecuteWithConnectionInThread(connectionData, this._linkController.ExecuteOpeningReport, commonConfig, selectedFile, actionOnComponent);

        public void StartSolutionOpening(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, ActionOnComponent actionOnComponent)
            => ExecuteWithConnectionInThread(connectionData, this._linkController.ExecuteOpeningSolutionAsync, commonConfig, solutionUniqueName, actionOnComponent);

        public void StartOpeningWebResource(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
            => ExecuteWithConnectionInThread(connectionData, this._linkController.ExecuteOpeningWebResource, commonConfig, selectedFile, actionOnComponent);

        public void StartPublishAll(ConnectionData connectionData)
            => ExecuteWithConnectionInThread(connectionData, this._publishController.ExecutePublishingAll);

        public void StartTraceReaderOpenWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
            => ExecuteWithConnectionInThread(connectionData, this._explorerController.ExecuteOpeningTraceReader, commonConfig);
    }
}