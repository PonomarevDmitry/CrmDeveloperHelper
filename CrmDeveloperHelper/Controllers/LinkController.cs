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
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class LinkController : BaseController<IWriteToOutput>
    {
        public LinkController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Очищение связей.

        internal void ExecuteClearingLastLink(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            string operation = string.Format(Properties.OperationNames.ClearingLastLinkFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

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

        public async Task ExecuteCreatingLastLinkReport(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            await CheckEncodingCheckReadOnlyConnectExecuteActionAsync(connectionData
                , Properties.OperationNames.CreatingLastLinkForReportFormat1
                , new[] { selectedFile }
                , false
                , (service) => CreatingLastLinkReport(service, selectedFile)
            );
        }

        private async Task CreatingLastLinkReport(IOrganizationServiceExtented service, SelectedFile selectedFile)
        {
            Guid? idLastLink = service.ConnectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

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
                    DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                }
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (!dialogResult.GetValueOrDefault())
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatingLastLinkWasCanceled);
                return;
            }

            if (!selectedReportId.HasValue)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ReportNotFoundedByNameFormat1, selectedFile.Name);
                return;
            }

            ReportRepository reportRepository = new ReportRepository(service);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ReportIsSelected);

            var webresource = await reportRepository.GetByIdAsync(selectedReportId.Value);

            service.ConnectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

            service.ConnectionData.Save();
        }

        #endregion Создание связи отчета.

        #region Создание связи веб-ресурсов.

        public async Task ExecuteCreatingLastLinkWebResourceMultiple(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            await CheckEncodingCheckReadOnlyConnectExecuteActionAsync(connectionData
                , Properties.OperationNames.CreatingLastLinkForWebResourcesFormat1
                , selectedFiles
                , false
                , (service) => CreatingLastLinkWebResourceMultiple(service, selectedFiles)
            );
        }

        private async Task CreatingLastLinkWebResourceMultiple(IOrganizationServiceExtented service, List<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            foreach (var selectedFile in selectedFiles)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                    continue;
                }

                var idLastLink = service.ConnectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

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
                        DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                    }
                }));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult.GetValueOrDefault())
                {
                    if (selectedWebResourceId.HasValue)
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.WebResourceIsSelected);

                        var webresource = await webResourceRepository.GetByIdAsync(selectedWebResourceId.Value);

                        service.ConnectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                        service.ConnectionData.Save();
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.WebResourceNotFoundedByNameFormat1, selectedFile.Name);
                    }
                }
                else if (!showNext)
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatingLastLinkWasCanceled);
                    return;
                }
            }
        }

        #endregion Создание связи веб-ресурсов.

        #region Открытие отчетов.

        public async Task ExecuteOpeningReport(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , Report.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(actionOnComponent)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningReport(commonConfig, connectionData, selectedFile, actionOnComponent);
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

        private async Task OpeningReport(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
        {
            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

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

            if (actionOnComponent == ActionOnComponent.OpenInWeb)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(Entities.ComponentType.Report, reportEntity.Id);
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.Report, reportEntity.Id);
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInExplorer)
            {
                WindowHelper.OpenSolutionComponentDependenciesExplorer(
                    _iWriteToOutput
                    , service
                    , null
                    , commonConfig
                    , (int)ComponentType.Report
                    , reportEntity.Id
                    , null);
            }
            else if (actionOnComponent == ActionOnComponent.OpenSolutionsContainingComponentInExplorer)
            {
                WindowHelper.OpenExplorerSolutionExplorer(
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

        public async Task ExecuteOpeningWebResource(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , WebResource.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(actionOnComponent)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await OpeningWebResource(commonConfig, connectionData, selectedFile, actionOnComponent);
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

        private async Task OpeningWebResource(CommonConfiguration commonConfig, ConnectionData connectionData, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
        {
            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

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

            if (actionOnComponent == ActionOnComponent.OpenInWeb)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(Entities.ComponentType.WebResource, webresource.Id);
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.WebResource, webresource.Id);
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInExplorer)
            {
                WindowHelper.OpenSolutionComponentDependenciesExplorer(
                    _iWriteToOutput
                    , service
                    , null
                    , commonConfig
                    , (int)ComponentType.WebResource
                    , webresource.Id
                    , null);
            }
            else if (actionOnComponent == ActionOnComponent.OpenSolutionsContainingComponentInExplorer)
            {
                WindowHelper.OpenExplorerSolutionExplorer(
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

        public async Task ExecuteOpeningSolutionAsync(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, ActionOnComponent actionOnComponent)
        {
            string operation = string.Format(
                Properties.OperationNames.ActionOnComponentFormat3
                , connectionData?.Name
                , Solution.EntitySchemaName
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(actionOnComponent)
            );

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSolutionAsync(commonConfig, connectionData, solutionUniqueName, actionOnComponent);
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

        private async Task OpeningSolutionAsync(CommonConfiguration commonConfig, ConnectionData connectionData, string solutionUniqueName, ActionOnComponent actionOnComponent)
        {
            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            var repository = new SolutionRepository(service);

            var solution = await repository.GetSolutionByUniqueNameAsync(solutionUniqueName);

            if (solution == null)
            {
                WindowHelper.OpenExplorerSolutionExplorer(
                    _iWriteToOutput
                    , service
                    , commonConfig
                    , null
                    , null
                    , null
                );

                return;
            }

            if (actionOnComponent == ActionOnComponent.OpenInWeb)
            {
                connectionData.OpenSolutionInWeb(solution.Id);
            }
            else if (actionOnComponent == ActionOnComponent.OpenInExplorer)
            {
                WindowHelper.OpenSolutionComponentsExplorer(this._iWriteToOutput, service, null, commonConfig, solution.UniqueName, null);
            }
        }
    }
}