using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using EnvDTE80;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class LinkController
    {
        private readonly IWriteToOutput _iWriteToOutput = null;

        public LinkController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Очищение связей.

        internal void ExecuteClearingLastLink(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            string operation = string.Format(Properties.OperationNames.ClearingLastLinkFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, connectionData, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                }

                ClearingWebResourcesLinks(selectedFiles, connectionData);
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

        private void ClearingWebResourcesLinks(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            int count = 0;

            foreach (SelectedFile selectedFile in selectedFiles)
            {
                if (connectionData.RemoveMapping(selectedFile.FriendlyFilePath))
                {
                    count++;
                }
            }

            if (count > 0)
            {
                //Сохранение настроек после публикации
                connectionData.Save();
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.DeletedLastLinksFormat1, count);
        }

        #endregion Очищение связей.

        #region Создание связи отчета.

        public async Task ExecuteCreatingLastLinkReport(SelectedFile selectedFile, ConnectionData connectionData)
        {
            string operation = string.Format(Properties.OperationNames.CreatingLastLinkForReportFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, connectionData, new List<SelectedFile>() { selectedFile }, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                }

                await CreatingLastLinkReport(selectedFile, connectionData);
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

        private async Task CreatingLastLinkReport(SelectedFile selectedFile, ConnectionData connectionData)
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

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            Guid? idLastLink = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

            bool? dialogResult = null;
            Guid? selectedReportId = null;

            string selectedPath = string.Empty;
            var t = new Thread((ThreadStart)(() =>
            {
                try
                {
                    var form = new Views.WindowReportSelect(this._iWriteToOutput, service, selectedFile, idLastLink);

                    dialogResult = form.ShowDialog();
                    selectedReportId = form.SelectedReportId;
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (dialogResult.GetValueOrDefault())
            {
                if (selectedReportId.HasValue)
                {
                    ReportRepository reportRepository = new ReportRepository(service);

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportIsSelected);

                    var webresource = await reportRepository.GetByIdAsync(selectedReportId.Value);

                    connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                    connectionData.Save();
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportNotFoundedByNameFormat1, selectedFile.Name);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CreatingLastLinkWasCanceled);
                return;
            }
        }

        #endregion Создание связи отчета.

        #region Создание связи веб-ресурсов.

        public async Task ExecuteCreatingLastLinkWebResourceMultiple(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            string operation = string.Format(Properties.OperationNames.CreatingLastLinkForWebResourcesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, connectionData, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                }

                await CreatingLastLinkWebResourceMultiple(selectedFiles, connectionData);
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

        private async Task CreatingLastLinkWebResourceMultiple(List<SelectedFile> selectedFiles, ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!selectedFiles.Any())
            {
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            foreach (var selectedFile in selectedFiles)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                    continue;
                }

                var idLastLink = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                bool? dialogResult = null;
                Guid? selectedWebResourceId = null;

                bool showNext = false;

                var t = new Thread((ThreadStart)(() =>
                {
                    try
                    {
                        var form = new Views.WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, selectedFile, idLastLink);
                        form.ShowSkipButton();

                        dialogResult = form.ShowDialog();
                        selectedWebResourceId = form.SelectedWebResourceId;
                        showNext = form.ShowNext;
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult.GetValueOrDefault())
                {
                    if (selectedWebResourceId.HasValue)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourceIsSelected);

                        var webresource = await webResourceRepository.GetByIdAsync(selectedWebResourceId.Value);

                        connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                        connectionData.Save();
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourceNotFoundedByNameFormat1, selectedFile.Name);
                    }
                }
                else if (!showNext)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CreatingLastLinkWasCanceled);
                    return;
                }
            }
        }

        #endregion Создание связи веб-ресурсов.

        #region Открытие отчетов.

        public async Task ExecuteOpeningReport(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOpenComponent action)
        {
            string operation = string.Format(Properties.OperationNames.OpeningReportFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningReport(commonConfig, connectionData, selectedFile, action);
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

        private async Task OpeningReport(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOpenComponent action)
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

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            // Репозиторий для работы с веб-ресурсами
            ReportRepository reportRepository = new ReportRepository(service);

            Report reportEntity = await reportRepository.FindAsync(selectedFile.FileName);

            if (reportEntity != null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportFoundedByNameFormat2, reportEntity.Id.ToString(), reportEntity.Name);

                connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);

                connectionData.Save();
            }
            else
            {
                Guid? reportId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (reportId.HasValue)
                {
                    reportEntity = await reportRepository.GetByIdAsync(reportId.Value);
                }

                if (reportEntity != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Report not founded by name. Last link report is selected for opening.");

                    connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);

                    connectionData.Save();
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Report not founded by name and has not Last link.");
                    this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom Report selection form.");

                    bool? dialogResult = null;
                    Guid? selectedReportId = null;

                    string selectedPath = string.Empty;
                    var t = new Thread((ThreadStart)(() =>
                    {
                        try
                        {
                            var form = new Views.WindowReportSelect(this._iWriteToOutput, service, selectedFile, reportId);

                            dialogResult = form.ShowDialog();
                            selectedReportId = form.SelectedReportId;
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(connectionData, ex);
                        }
                    }));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    t.Join();

                    if (dialogResult.GetValueOrDefault())
                    {
                        if (selectedReportId.HasValue)
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "Custom report is selected.");

                            reportEntity = await reportRepository.GetByIdAsync(selectedReportId.Value);

                            connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);

                            connectionData.Save();
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "!Warning. Report not exists. name: {0}.", selectedFile.Name);
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "Opening was cancelled.");
                        return;
                    }
                }
            }

            if (reportEntity == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportNotFoundedByNameFormat1, selectedFile.FileName);
                return;
            }

            if (action == ActionOpenComponent.OpenInWeb)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(Entities.ComponentType.Report, reportEntity.Id);
            }
            else if (action == ActionOpenComponent.OpenDependentComponentsInWeb)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.Report, reportEntity.Id);
            }
            else if (action == ActionOpenComponent.OpenDependentComponentsInExplorer)
            {
                WindowHelper.OpenSolutionComponentDependenciesWindow(
                    _iWriteToOutput
                    , service
                    , null
                    , commonConfig
                    , (int)ComponentType.Report
                    , reportEntity.Id
                    , null);
            }
            else if (action == ActionOpenComponent.OpenSolutionsContainingComponentInExplorer)
            {
                WindowHelper.OpenExplorerSolutionWindow(
                    _iWriteToOutput
                    , service
                    , commonConfig
                    , (int)ComponentType.Report
                    , reportEntity.Id
                    , null
                );
            }
        }

        #endregion Открытие отчетов.

        #region Открытие веб-ресурсов.

        public async Task ExecuteOpeningWebResource(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOpenComponent action)
        {
            string operation = string.Format(Properties.OperationNames.OpeningWebResourceFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, connectionData, new List<SelectedFile>() { selectedFile }, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                }

                await OpeningWebResource(commonConfig, connectionData, selectedFile, action);
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

        private async Task OpeningWebResource(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOpenComponent action)
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

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            WebResource webresource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

            if (webresource != null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourceFoundedByNameFormat2, webresource.Id.ToString(), webresource.Name);

                connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                connectionData.Save();
            }
            else
            {
                Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webId.HasValue)
                {
                    webresource = await webResourceRepository.GetByIdAsync(webId.Value);
                }

                if (webresource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Web-resource not founded by name. Last link web-resource is selected for opening.");

                    connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                    connectionData.Save();
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Web-resource not founded by name and has not Last link.");
                    this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom Web-resource selection form.");

                    bool? dialogResult = null;
                    Guid? selectedWebResourceId = null;

                    string selectedPath = string.Empty;
                    var t = new Thread((ThreadStart)(() =>
                    {
                        try
                        {
                            var form = new Views.WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, selectedFile, webId);

                            dialogResult = form.ShowDialog();
                            selectedWebResourceId = form.SelectedWebResourceId;
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(connectionData, ex);
                        }
                    }));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    t.Join();

                    if (dialogResult.GetValueOrDefault())
                    {
                        if (selectedWebResourceId.HasValue)
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "Custom Web-resource is selected.");

                            webresource = await webResourceRepository.GetByIdAsync(selectedWebResourceId.Value);

                            connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                            connectionData.Save();
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "!Warning. WebResource not exists. name: {0}.", selectedFile.Name);
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "Opening was cancelled.");
                        return;
                    }
                }
            }

            if (webresource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.WebResourceNotFoundedByNameFormat1, selectedFile.FileName);
                return;
            }

            if (action == ActionOpenComponent.OpenInWeb)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(Entities.ComponentType.WebResource, webresource.Id);
            }
            else if (action == ActionOpenComponent.OpenDependentComponentsInWeb)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.WebResource, webresource.Id);
            }
            else if (action == ActionOpenComponent.OpenDependentComponentsInExplorer)
            {
                WindowHelper.OpenSolutionComponentDependenciesWindow(
                    _iWriteToOutput
                    , service
                    , null
                    , commonConfig
                    , (int)ComponentType.WebResource
                    , webresource.Id
                    , null);
            }
            else if (action == ActionOpenComponent.OpenSolutionsContainingComponentInExplorer)
            {
                WindowHelper.OpenExplorerSolutionWindow(
                    _iWriteToOutput
                    , service
                    , commonConfig
                    , (int)ComponentType.WebResource
                    , webresource.Id
                    , null
                );
            }
        }

        #endregion Открытие веб-ресурсов.

        public async Task ExecuteOpeningSolutionAsync(CommonConfiguration commonConfig, ConnectionData connectionData, string solutionUniqueName, ActionOpenComponent action)
        {
            string operation = string.Format(Properties.OperationNames.OpeningSolutionFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSolutionAsync(commonConfig, connectionData, solutionUniqueName, action);
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

        private async Task OpeningSolutionAsync(CommonConfiguration commonConfig, ConnectionData connectionData, string solutionUniqueName, ActionOpenComponent action)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repository = new SolutionRepository(service);

            var solution = await repository.GetSolutionByUniqueNameAsync(solutionUniqueName);

            if (solution == null)
            {
                WindowHelper.OpenExplorerSolutionWindow(
                    _iWriteToOutput
                    , service
                    , commonConfig
                    , null
                    , null
                    , null
                );

                return;
            }

            if (action == ActionOpenComponent.OpenInWeb)
            {
                connectionData.OpenSolutionInWeb(solution.Id);
            }
            else if (action == ActionOpenComponent.OpenInExplorer)
            {
                WindowHelper.OpenSolutionComponentDependenciesWindow(this._iWriteToOutput, service, null, commonConfig, solution.UniqueName, null);
            }
        }
    }
}