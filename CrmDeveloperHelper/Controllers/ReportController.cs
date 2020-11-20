using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class ReportController : BaseController<IWriteToOutput>
    {
        public ReportController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Различия отчета и файла.

        public async Task ExecuteUpdatingReport(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingReportFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdatingReport(selectedFile, connectionData, commonConfig);
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

        private async Task UpdatingReport(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (connectionData.IsReadOnly)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            var service = await ConnectAndWriteToOutputAsync(connectionData);

            if (service == null)
            {
                return;
            }

            bool isconnectionDataDirty = false;

            // Репозиторий для работы с веб-ресурсами
            ReportRepository reportRepository = new ReportRepository(service);

            if (File.Exists(selectedFile.FilePath))
            {
                Report reportEntity = null;

                reportEntity = await reportRepository.FindAsync(selectedFile.FileName);

                if (reportEntity != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Report founded by name.");

                    isconnectionDataDirty = true;
                    connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);
                }
                else
                {
                    Guid? lastReportId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                    this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom Report selection form.");

                    bool? dialogResult = null;
                    Guid? selectedReportId = null;

                    string selectedPath = string.Empty;
                    var t = new Thread((ThreadStart)(() =>
                    {
                        try
                        {
                            var form = new Views.WindowReportSelect(this._iWriteToOutput, service, selectedFile, lastReportId);

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

                            isconnectionDataDirty = true;
                            connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(connectionData, "!Warning. Report not exists. name: {0}.", selectedFile.Name);
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "Updating was cancelled.");
                        return;
                    }
                }

                if (reportEntity != null)
                {
                    var update = new Entity(Report.EntityLogicalName);
                    update.Id = reportEntity.Id;

                    update.Attributes[Report.Schema.Attributes.bodytext] = File.ReadAllText(selectedFile.FilePath);

                    await service.UpdateAsync(update);

                    this._iWriteToOutput.WriteToOutput(connectionData, "Report updated in CRM: {0} - {1} - {2}", reportEntity.Name, reportEntity.FileName, reportEntity.ReportNameOnSRS);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionReportWasNotFoundFormat2, connectionData.Name, selectedFile.FileName);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                connectionData.Save();
            }

            service.TryDispose();
        }

        #endregion Различия отчета и файла.

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

        #region Различия отчета и файла.

        public async Task ExecuteDifferenceReport(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceReportFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await DifferenceReport(connectionData, commonConfig, selectedFile, fieldName, fieldTitle, withSelect);
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

        private async Task DifferenceReport(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle, bool withSelect)
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

            if (string.IsNullOrEmpty(fieldName))
            {
                fieldName = Report.Schema.Attributes.originalbodytext;
                fieldTitle = Report.Schema.Headers.originalbodytext;
            }

            Report reportEntity = null;

            using (service.Lock())
            {
                // Репозиторий для работы с веб-ресурсами
                var reportRepository = new ReportRepository(service);

                if (withSelect)
                {
                    Guid? reportId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                    bool? dialogResult = null;
                    Guid? selectedReportId = null;

                    string selectedPath = string.Empty;
                    var t = new Thread(() =>
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
                    });
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
                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.DifferenceWasCancelled);
                        return;
                    }
                }
                else
                {
                    reportEntity = await reportRepository.FindAsync(selectedFile.FileName);

                    if (reportEntity != null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "Report founded by name.");

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
                            this._iWriteToOutput.WriteToOutput(connectionData, "Report not founded by name. Last link report is selected for difference.");

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
                            var t = new Thread(() =>
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
                            });
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
                                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.DifferenceWasCancelled);
                                return;
                            }
                        }
                    }
                }
            }

            if (reportEntity == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionReportWasNotFoundFormat2, connectionData.Name, selectedFile.FileName);
                return;
            }

            var reportName = EntityFileNameFormatter.GetReportFileName(connectionData.Name, reportEntity.Name, reportEntity.Id, fieldTitle, selectedFile.Extension);

            string temporaryFilePath = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(reportName), selectedFile.Extension);

            var textReport = reportEntity.GetAttributeValue<string>(fieldName);

            if (ContentComparerHelper.TryParseXml(textReport, out var doc))
            {
                textReport = doc.ToString();
            }

            File.WriteAllText(temporaryFilePath, textReport, new UTF8Encoding(false));

            //DownloadReportDefinitionRequest rdlRequest = new DownloadReportDefinitionRequest
            //{
            //    ReportId = reportEntity.Id
            //};

            //DownloadReportDefinitionResponse rdlResponse = (DownloadReportDefinitionResponse)service.Execute(rdlRequest);

            //using (XmlTextWriter reportDefinitionFile = new XmlTextWriter(temporaryFilePath, System.Text.Encoding.UTF8))
            //{
            //    reportDefinitionFile.WriteRaw(rdlResponse.BodyText);
            //    reportDefinitionFile.Close();
            //}

            this._iWriteToOutput.WriteToOutput(connectionData, "Starting Compare Program for {0} and {1}", selectedFile.FriendlyFilePath, reportName);

            string fileLocalPath = selectedFile.FilePath;
            string fileLocalTitle = selectedFile.FileName;

            string filePath2 = temporaryFilePath;
            string fileTitle2 = connectionData.Name + "." + selectedFile.FileName + " - " + temporaryFilePath;

            await this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
        }

        #endregion Различия отчета и файла.

        #region Различия трех файлов отчетов.

        public async Task ExecuteThreeFileDifferenceReport(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle, ShowDifferenceThreeFileType differenceType)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceLocalFileAndTwoReportsFormat3, differenceType, connectionData1?.Name, connectionData2?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(null, operation);

            try
            {
                await ThreeFileDifferenceReport(selectedFile, fieldName, fieldTitle, connectionData1, connectionData2, differenceType, commonConfig);
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

        private async Task ThreeFileDifferenceReport(SelectedFile selectedFile, string fieldName, string fieldTitle, ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType, CommonConfiguration commonConfig)
        {
            if (differenceType == ShowDifferenceThreeFileType.ThreeWay)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                    return;
                }
            }

            var doubleConnection = await ConnectAndWriteToOutputDoubleConnectionAsync(connectionData1, connectionData2);

            if (doubleConnection == null)
            {
                return;
            }

            var service1 = doubleConnection.Item1;
            var service2 = doubleConnection.Item2;

            if (string.IsNullOrEmpty(fieldName))
            {
                fieldName = Report.Schema.Attributes.originalbodytext;
                fieldTitle = Report.Schema.Headers.originalbodytext;
            }

            // Репозиторий для работы с веб-ресурсами
            ReportRepository reportRepository1 = new ReportRepository(service1);
            ReportRepository reportRepository2 = new ReportRepository(service2);

            var taskReport1 = reportRepository1.FindAsync(selectedFile.FileName);
            var taskReport2 = reportRepository2.FindAsync(selectedFile.FileName);

            Report reportEntity1 = await taskReport1;
            Report reportEntity2 = await taskReport2;

            if (reportEntity1 != null)
            {
                this._iWriteToOutput.WriteToOutput(null, "{0}: Report founded by name.", connectionData1.Name);

                connectionData1.AddMapping(reportEntity1.Id, selectedFile.FriendlyFilePath);

                connectionData1.Save();
            }
            else
            {
                Guid? reportId = connectionData1.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (reportId.HasValue)
                {
                    reportEntity1 = await reportRepository1.GetByIdAsync(reportId.Value);

                    if (reportEntity1 != null)
                    {
                        this._iWriteToOutput.WriteToOutput(null, "{0}: Report not founded by name. Last link report is selected for difference.", connectionData1.Name);

                        connectionData1.AddMapping(reportEntity1.Id, selectedFile.FriendlyFilePath);

                        connectionData1.Save();
                    }
                }
            }

            if (reportEntity2 != null)
            {
                this._iWriteToOutput.WriteToOutput(null, "{0}: Report founded by name.", connectionData2.Name);

                connectionData2.AddMapping(reportEntity2.Id, selectedFile.FriendlyFilePath);

                connectionData2.Save();
            }
            else
            {
                Guid? reportId = connectionData2.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (reportId.HasValue)
                {
                    reportEntity2 = await reportRepository2.GetByIdAsync(reportId.Value);

                    if (reportEntity2 != null)
                    {
                        this._iWriteToOutput.WriteToOutput(null, "{0}: Report not founded by name. Last link report is selected for difference.", connectionData2.Name);

                        connectionData2.AddMapping(reportEntity2.Id, selectedFile.FriendlyFilePath);

                        connectionData2.Save();
                    }
                }
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
            }

            if (reportEntity1 == null)
            {
                this._iWriteToOutput.WriteToOutput(null, "{0}: Report not founded in CRM: {1}", connectionData1.Name, selectedFile.FileName);
            }

            if (reportEntity2 == null)
            {
                this._iWriteToOutput.WriteToOutput(null, "{0}: Report not founded in CRM: {1}", connectionData2.Name, selectedFile.FileName);
            }

            string fileLocalPath = selectedFile.FilePath;
            string fileLocalTitle = selectedFile.FileName;

            string filePath1 = string.Empty;
            string fileTitle1 = string.Empty;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            if (reportEntity1 != null)
            {
                var textReport = reportEntity1.GetAttributeValue<string>(fieldName);

                if (ContentComparerHelper.TryParseXml(textReport, out var doc))
                {
                    textReport = doc.ToString();
                }

                var reportName = EntityFileNameFormatter.GetReportFileName(connectionData1.Name, reportEntity1.Name, reportEntity1.Id, fieldTitle, selectedFile.Extension);

                filePath1 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(reportName), selectedFile.Extension);
                fileTitle1 = connectionData1.Name + "." + selectedFile.FileName + " - " + filePath1;

                File.WriteAllText(filePath1, textReport, new UTF8Encoding(false));
            }

            if (reportEntity2 != null)
            {
                var textReport = reportEntity2.GetAttributeValue<string>(fieldName);

                if (ContentComparerHelper.TryParseXml(textReport, out var doc))
                {
                    textReport = doc.ToString();
                }

                var reportName = EntityFileNameFormatter.GetReportFileName(connectionData2.Name, reportEntity2.Name, reportEntity2.Id, fieldTitle, selectedFile.Extension);

                filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(reportName), selectedFile.Extension);
                fileTitle1 = connectionData2.Name + "." + selectedFile.FileName + " - " + filePath2;

                File.WriteAllText(filePath2, textReport, new UTF8Encoding(false));
            }

            switch (differenceType)
            {
                case ShowDifferenceThreeFileType.OneByOne:
                    ShowDifferenceOneByOne(commonConfig, connectionData1, connectionData2, fileLocalPath, fileLocalTitle, filePath1, fileTitle1, filePath2, fileTitle2);
                    break;

                case ShowDifferenceThreeFileType.TwoConnections:
                    await this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData1, filePath1, filePath2, fileTitle1, fileTitle2, connectionData2);
                    break;

                case ShowDifferenceThreeFileType.ThreeWay:
                    ShowDifferenceThreeWay(commonConfig, connectionData1, connectionData2, fileLocalPath, fileLocalTitle, filePath1, fileTitle1, filePath2, fileTitle2);
                    break;
                default:
                    break;
            }
        }

        #endregion Различия трех файлов отчетов.

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
                service.TryDispose();

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportNotFoundedByNameFormat1, selectedFile.FileName);
                return;
            }

            if (actionOnComponent == ActionOnComponent.OpenInWeb)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(Entities.ComponentType.Report, reportEntity.Id);
                service.TryDispose();
            }
            else if (actionOnComponent == ActionOnComponent.OpenDependentComponentsInWeb)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.Report, reportEntity.Id);
                service.TryDispose();
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
            else if (actionOnComponent == ActionOnComponent.OpenSolutionsListWithComponentInExplorer)
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
    }
}
