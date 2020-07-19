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
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class DifferenceController : BaseController<IWriteToOutput>
    {
        public DifferenceController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Различия файла и веб-ресурса.

        public async Task ExecuteDifferenceWebResources(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool withSelect)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceWebResourceFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await DifferenceWebResources(connectionData, commonConfig, selectedFile, withSelect);
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

        private async Task DifferenceWebResources(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, bool withSelect)
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

            WebResource webResource = null;
            Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

            if (!withSelect)
            {
                webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

                if (webResource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
                }
                else if (lastLinkedWebResourceId.HasValue)
                {
                    if (webResource != null)
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for difference.");
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                        this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");
                    }
                }
            }

            if (webResource == null)
            {
                if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                    webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
            connectionData.Save();

            string fileLocalPath = selectedFile.FilePath;
            string fileLocalTitle = selectedFile.FileName;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            {
                var contentWebResource = webResource.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource);

                filePath2 = FileOperations.GetNewTempFilePath(webResource.Name, selectedFile.Extension);

                File.WriteAllBytes(filePath2, array);

                fileTitle2 = connectionData.Name + "." + selectedFile.FileName + " - " + filePath2;
            }

            await this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
        }

        #endregion Различия файла и веб-ресурса.

        #region Сравнение трех файлов вебресурсов.

        public async Task ExecuteThreeFileDifferenceWebResources(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, ShowDifferenceThreeFileType differenceType)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceLocalFileAndTwoWebResourcesFormat3, differenceType, connectionData1?.Name, connectionData2?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(null, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(null, new[] { selectedFile }, out _);

                await ThreeFileDifferenceWebResources(connectionData1, connectionData2, commonConfig, selectedFile, differenceType);
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

        private async Task ThreeFileDifferenceWebResources(ConnectionData connectionData1, ConnectionData connectionData2, CommonConfiguration commonConfig, SelectedFile selectedFile, ShowDifferenceThreeFileType differenceType)
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

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository1 = new WebResourceRepository(service1);
            WebResourceRepository webResourceRepository2 = new WebResourceRepository(service2);

            var taskWebResource1 = webResourceRepository1.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
            var taskWebResource2 = webResourceRepository2.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

            WebResource webresource1 = await taskWebResource1;
            WebResource webresource2 = await taskWebResource2;

            if (webresource1 != null)
            {
                this._iWriteToOutput.WriteToOutput(null, "{0}: WebResource founded by name.", connectionData1.Name);

                connectionData1.AddMapping(webresource1.Id, selectedFile.FriendlyFilePath);

                connectionData1.Save();
            }
            else
            {
                Guid? webId = connectionData1.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webId.HasValue)
                {
                    webresource1 = await webResourceRepository1.GetByIdAsync(webId.Value);

                    if (webresource1 != null)
                    {
                        this._iWriteToOutput.WriteToOutput(null, "{0}: WebResource not founded by name. Last link WebResource is selected for difference.", connectionData1.Name);

                        connectionData1.AddMapping(webresource1.Id, selectedFile.FriendlyFilePath);

                        connectionData1.Save();
                    }
                }
            }

            if (webresource2 != null)
            {
                this._iWriteToOutput.WriteToOutput(null, "{0}: WebResource founded by name.", connectionData2.Name);

                connectionData2.AddMapping(webresource2.Id, selectedFile.FriendlyFilePath);

                connectionData2.Save();
            }
            else
            {
                Guid? webId = connectionData2.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webId.HasValue)
                {
                    webresource2 = await webResourceRepository2.GetByIdAsync(webId.Value);

                    if (webresource2 != null)
                    {
                        this._iWriteToOutput.WriteToOutput(null, "{0}: WebResource not founded by name. Last link WebResource is selected for difference.", connectionData2.Name);

                        connectionData2.AddMapping(webresource2.Id, selectedFile.FriendlyFilePath);

                        connectionData2.Save();
                    }
                }
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
            }

            if (webresource1 == null)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.WebResourceNotFoundedInConnectionFormat2, connectionData1.Name, selectedFile.FileName);
            }

            if (webresource2 == null)
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.WebResourceNotFoundedInConnectionFormat2, connectionData2.Name, selectedFile.FileName);
            }

            // string fileLocalPath, string fileLocalTitle, string filePath1, string fileTitle1, string filePath2, string fileTitle2,
            string fileLocalPath = selectedFile.FilePath;
            string fileLocalTitle = selectedFile.FileName;

            string filePath1 = string.Empty;
            string fileTitle1 = string.Empty;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            if (webresource1 != null)
            {
                var contentWebResource1 = webresource1.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource1);

                filePath1 = FileOperations.GetNewTempFilePath(webresource1.Name, selectedFile.Extension);
                fileTitle1 = connectionData1.Name + "." + selectedFile.FileName + " - " + filePath1;

                File.WriteAllBytes(filePath1, array);
            }

            if (webresource2 != null)
            {
                var contentWebResource2 = webresource2.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource2);

                filePath2 = FileOperations.GetNewTempFilePath(webresource2.Name, selectedFile.Extension);
                fileTitle2 = connectionData2.Name + "." + selectedFile.FileName + " - " + filePath2;

                File.WriteAllBytes(filePath2, array);
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

        private void ShowDifferenceThreeWay(
            CommonConfiguration commonConfig
            , ConnectionData connectionData1
            , ConnectionData connectionData2
            , string fileLocalPath
            , string fileLocalTitle
            , string filePath1
            , string fileTitle1
            , string filePath2
            , string fileTitle2
        )
        {
            if (!File.Exists(fileLocalPath))
            {
                this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FileNotExistsFormat1, fileLocalPath);
                return;
            }

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparerThreeWayFile(fileLocalPath, filePath1, filePath2, fileLocalTitle, fileTitle1, fileTitle2);
            }
            else
            {
                if (File.Exists(filePath1))
                {
                    this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData1, fileLocalPath, filePath1, fileLocalTitle, fileTitle1);
                }

                if (File.Exists(filePath2))
                {
                    this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData2, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
                }
            }
        }

        private void ShowDifferenceOneByOne(
            CommonConfiguration commonConfig
            , ConnectionData connectionData1
            , ConnectionData connectionData2
            , string fileLocalPath
            , string fileLocalTitle
            , string filePath1
            , string fileTitle1
            , string filePath2
            , string fileTitle2
        )
        {
            bool existsFileLocal = File.Exists(fileLocalPath);
            bool existsFile1 = File.Exists(filePath1);
            bool existsFile2 = File.Exists(filePath2);

            if (existsFileLocal && existsFile1)
            {
                this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData1, fileLocalPath, filePath1, fileLocalTitle, fileTitle1);
            }

            if (existsFileLocal && existsFile2)
            {
                this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData2, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
            }

            if (existsFile1 && existsFile2)
            {
                this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData1, filePath1, filePath2, fileTitle1, fileTitle2, connectionData2);
            }

            int total = Convert.ToInt32(existsFileLocal) + Convert.ToInt32(existsFile1) + Convert.ToInt32(existsFile2);

            if (total == 1)
            {
                _iWriteToOutput.WriteToOutputFilePathUri(null, fileLocalPath);
                _iWriteToOutput.OpenFile(null, fileLocalPath);

                _iWriteToOutput.WriteToOutputFilePathUri(connectionData1, filePath1);
                _iWriteToOutput.OpenFile(connectionData1, filePath1);

                _iWriteToOutput.WriteToOutputFilePathUri(connectionData2, filePath2);
                _iWriteToOutput.OpenFile(connectionData2, filePath2);
            }
        }

        #endregion Сравнение трех файлов вебресурсов.

        #region Сравнение файлов и веб-ресурсов по разным параметрам.

        public async Task ExecuteWebResourceMultiDifferenceFiles(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            await CheckEncodingConnectFindWebResourceExecuteActionTaskAsync(connectionData
                , Properties.OperationNames.MultiDifferenceFormat2
                , selectedFiles
                , openFilesType
                , MultiDifferenceFiles
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(openFilesType)
            );
        }

        private async Task MultiDifferenceFiles(ConnectionData connectionData, IOrganizationServiceExtented service, TupleList<SelectedFile, WebResource> listFilesToDifference)
        {
            if (!listFilesToDifference.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoFilesForDifference);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);
            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.StartingCompareProgramForCountFilesFormat1, listFilesToDifference.Count());

            foreach (var item in listFilesToDifference.OrderBy(file => file.Item1.FilePath))
            {
                var selectedFile = item.Item1;
                var webresource = item.Item2;

                if (webresource != null)
                {
                    var contentWebResource = webresource.Content;

                    var webResourceName = webresource.Name;

                    var array = Convert.FromBase64String(contentWebResource);

                    string tempFilePath = FileOperations.GetNewTempFilePath(webResourceName, selectedFile.Extension);

                    File.WriteAllBytes(tempFilePath, array);

                    string fileLocalPath = selectedFile.FilePath;
                    string fileLocalTitle = selectedFile.FileName;

                    string filePath2 = tempFilePath;
                    string fileTitle2 = connectionData.Name + "." + selectedFile.FileName + " - " + tempFilePath;

                    await this._iWriteToOutput.ProcessStartProgramComparerAsync(connectionData, fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
                }
            }
        }

        #endregion Сравнение файлов и веб-ресурсов по разным параметрам.

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

            // Репозиторий для работы с веб-ресурсами
            ReportRepository reportRepository = new ReportRepository(service);

            Report reportEntity = null;

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

            if (reportEntity == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportNotFoundedInConnectionFormat2, connectionData.Name, selectedFile.FileName);
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

        public async Task ExecuteCreatingWebResourceEntityDescription(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.CreatingWebResourceEntityDescriptionFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await CreatingWebResourceEntityDescription(selectedFile, connectionData, commonConfig);
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

        private async Task CreatingWebResourceEntityDescription(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
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

            WebResource webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
            Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

            if (webResource != null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
            }
            else if (lastLinkedWebResourceId.HasValue)
            {
                webResource = await webResourceRepository.GetByIdAsync(lastLinkedWebResourceId.Value);

                if (webResource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for difference.");
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");

                if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                    webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
            connectionData.Save();

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string fileName = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, webResource.Name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, webResource, service.ConnectionData);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData
                , Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                , service.ConnectionData.Name
                , webResource.LogicalName
                , filePath
            );

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        public async Task ExecuteChangingWebResourceInEntityEditor(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string operation = string.Format(Properties.OperationNames.ChangingWebResourceInEntityEditorFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await ChangingWebResourceInEntityEditor(selectedFile, connectionData, commonConfig);
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

        private async Task ChangingWebResourceInEntityEditor(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
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
            var webResourceRepository = new WebResourceRepository(service);

            WebResource webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

            Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

            if (webResource != null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
            }
            else if (lastLinkedWebResourceId.HasValue)
            {
                webResource = await webResourceRepository.GetByIdAsync(lastLinkedWebResourceId.Value);

                if (webResource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for difference.");
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");

                if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                    webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
            connectionData.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, WebResource.EntityLogicalName, webResource.Id);
        }

        private bool SelecteWebResourceInWindow(IOrganizationServiceExtented service, SelectedFile selectedFile, Guid? lastLinkedWebResourceId, out Guid selectedWebResourceId)
        {
            selectedWebResourceId = Guid.Empty;

            bool? dialogResult = null;

            Guid? webResourceId = null;

            var t = new Thread(() =>
            {
                try
                {
                    var form = new WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, selectedFile, lastLinkedWebResourceId);

                    dialogResult = form.ShowDialog();

                    webResourceId = form.SelectedWebResourceId;
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (dialogResult.GetValueOrDefault() == false)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DifferenceWasCancelled);
                return false;
            }

            if (!webResourceId.HasValue)
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "!Warning. WebResource not exists. name: {0}.", selectedFile.Name);
                return false;
            }

            selectedWebResourceId = webResourceId.Value;

            return true;
        }

        public async Task ExecuteWebResourceGettingAttribute(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile, string fieldName, string fieldTitle)
        {
            string operation = string.Format(Properties.OperationNames.GettingWebResourceAttributeFormat2, connectionData?.Name, fieldTitle);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, new[] { selectedFile }, out _);

                await WebResourceGettingAttribute(selectedFile, fieldName, fieldTitle, connectionData, commonConfig);
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

        private async Task WebResourceGettingAttribute(SelectedFile selectedFile, string fieldName, string fieldTitle, ConnectionData connectionData, CommonConfiguration commonConfig)
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

            WebResource webResource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
            Guid? lastLinkedWebResourceId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

            if (webResource != null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource founded by name.");
            }
            else if (lastLinkedWebResourceId.HasValue)
            {
                webResource = await webResourceRepository.GetByIdAsync(lastLinkedWebResourceId.Value);

                if (webResource != null)
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name. Last link WebResource is selected for difference.");
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded by name and has not Last link.");
                this._iWriteToOutput.WriteToOutput(connectionData, "Starting Custom WebResource selection form.");

                if (SelecteWebResourceInWindow(service, selectedFile, lastLinkedWebResourceId, out Guid selectedWebResourceId))
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, "Custom WebResource is selected.");

                    webResource = await webResourceRepository.GetByIdAsync(selectedWebResourceId);
                }
            }

            if (webResource == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "WebResource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            connectionData.AddMapping(webResource.Id, selectedFile.FriendlyFilePath);
            connectionData.Save();

            string xmlContent = webResource.GetAttributeValue<string>(fieldName);

            if (string.IsNullOrEmpty(xmlContent))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string extension = "xml";

            if (string.Equals(fieldName, WebResource.Schema.Attributes.dependencyxml, StringComparison.InvariantCultureIgnoreCase))
            {
                xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                    xmlContent
                    , commonConfig
                    , XmlOptionsControls.WebResourceDependencyXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                    , webResourceName: webResource.Name
                );
            }
            else if (string.Equals(fieldName, WebResource.Schema.Attributes.contentjson, StringComparison.InvariantCultureIgnoreCase))
            {
                xmlContent = ContentComparerHelper.FormatJson(xmlContent);
            }

            commonConfig.CheckFolderForExportExists(this._iWriteToOutput);

            string fileName = EntityFileNameFormatter.GetWebResourceFileName(connectionData.Name, webResource.Name, fieldTitle, extension);
            string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, WebResource.Schema.EntityLogicalName, webResource.Name, fieldTitle, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }
    }
}