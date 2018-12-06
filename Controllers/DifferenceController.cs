using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class DifferenceController
    {
        private IWriteToOutput _iWriteToOutput = null;

        public DifferenceController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Различия файла и веб-ресурса.

        public async Task ExecuteDifferenceWebResources(SelectedFile selectedFile, bool isCustom, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceWebResourceFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, new List<SelectedFile>() { selectedFile }, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                }

                await DifferenceWebResources(selectedFile, isCustom, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task DifferenceWebResources(SelectedFile selectedFile, bool isCustom, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            WebResource webresource = null;

            if (isCustom)
            {
                Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                bool? dialogResult = null;
                Guid? selectedWebResourceId = null;

                string selectedPath = string.Empty;
                var t = new Thread(() =>
                {
                    try
                    {
                        var form = new Views.WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, connectionData, selectedFile, webId);

                        dialogResult = form.ShowDialog();
                        selectedWebResourceId = form.SelectedWebResourceId;
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(ex);
                    }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult.GetValueOrDefault() == false)
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.DifferenceWasCancelled);
                    return;
                }

                if (selectedWebResourceId.HasValue)
                {
                    this._iWriteToOutput.WriteToOutput("Custom Web-resource is selected.");

                    webresource = await webResourceRepository.FindByIdAsync(selectedWebResourceId.Value);

                    connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                    connectionData.ConnectionConfiguration.Save();
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput("!Warning. WebResource not exists. name: {0}.", selectedFile.Name);
                }
            }
            else
            {
                webresource = await webResourceRepository.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

                if (webresource != null)
                {
                    this._iWriteToOutput.WriteToOutput("Web-resource founded by name.");

                    connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                    connectionData.ConnectionConfiguration.Save();
                }
                else
                {
                    Guid? webId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                    if (webId.HasValue)
                    {
                        webresource = await webResourceRepository.FindByIdAsync(webId.Value);
                    }

                    if (webresource != null)
                    {
                        this._iWriteToOutput.WriteToOutput("Web-resource not founded by name. Last link web-resource is selected for difference.");

                        connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                        connectionData.ConnectionConfiguration.Save();
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput("Web-resource not founded by name and has not Last link.");
                        this._iWriteToOutput.WriteToOutput("Starting Custom Web-resource selection form.");

                        bool? dialogResult = null;
                        Guid? selectedWebResourceId = null;

                        string selectedPath = string.Empty;
                        var t = new Thread(() =>
                        {
                            try
                            {
                                var form = new Views.WindowWebResourceSelectOrCreate(this._iWriteToOutput, service, connectionData, selectedFile, webId);

                                dialogResult = form.ShowDialog();
                                selectedWebResourceId = form.SelectedWebResourceId;
                            }
                            catch (Exception ex)
                            {
                                DTEHelper.WriteExceptionToOutput(ex);
                            }
                        });
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();

                        t.Join();

                        if (dialogResult.GetValueOrDefault())
                        {
                            if (selectedWebResourceId.HasValue)
                            {
                                this._iWriteToOutput.WriteToOutput("Custom Web-resource is selected.");

                                webresource = await webResourceRepository.FindByIdAsync(selectedWebResourceId.Value);

                                connectionData.AddMapping(webresource.Id, selectedFile.FriendlyFilePath);

                                connectionData.ConnectionConfiguration.Save();
                            }
                            else
                            {
                                this._iWriteToOutput.WriteToOutput("!Warning. WebResource not exists. name: {0}.", selectedFile.Name);
                            }
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.DifferenceWasCancelled);
                            return;
                        }
                    }
                }
            }

            if (webresource == null)
            {
                this._iWriteToOutput.WriteToOutput("Web-resource not founded in CRM: {0}", selectedFile.FileName);
                return;
            }

            string filePath1 = selectedFile.FilePath;
            string fileTitle1 = selectedFile.FileName;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            {
                var contentWebResource = webresource.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource);

                filePath2 = FileOperations.GetNewTempFile(webresource.Name, selectedFile.Extension);

                File.WriteAllBytes(filePath2, array);

                fileTitle2 = connectionData.Name + "." + selectedFile.FileName + " - " + filePath2;
            }

            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
        }

        #endregion Различия файла и веб-ресурса.

        #region Сравнение трех файлов вебресурсов.

        public async Task ExecuteThreeFileDifferenceWebResources(SelectedFile selectedFile, ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceLocalFileAndTwoWebResourcesFormat3, differenceType, connectionData1?.Name, connectionData2?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                this._iWriteToOutput.WriteToOutput(Properties.OperationNames.CheckingFilesEncoding);

                CheckController.CheckingFilesEncoding(this._iWriteToOutput, new List<SelectedFile>() { selectedFile }, out List<SelectedFile> filesWithoutUTF8Encoding);

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);

                await ThreeFileDifferenceWebResources(selectedFile, connectionData1, connectionData2, differenceType, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ThreeFileDifferenceWebResources(SelectedFile selectedFile, ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType, CommonConfiguration commonConfig)
        {
            if (connectionData1 == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCRMConnection1);
                return;
            }

            if (connectionData2 == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCRMConnection2);
                return;
            }

            if (differenceType == ShowDifferenceThreeFileType.ThreeWay)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                    return;
                }
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
            this._iWriteToOutput.WriteToOutput(string.Empty);
            this._iWriteToOutput.WriteToOutput(connectionData1.GetConnectionDescription());
            this._iWriteToOutput.WriteToOutput(string.Empty);
            this._iWriteToOutput.WriteToOutput(connectionData2.GetConnectionDescription());

            var task1 = QuickConnection.ConnectAsync(connectionData1);
            var task2 = QuickConnection.ConnectAsync(connectionData2);

            var service1 = await task1;
            var service2 = await task2;

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointConnectionFormat2, connectionData1.Name, service1.CurrentServiceEndpoint);
            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointConnectionFormat2, connectionData2.Name, service2.CurrentServiceEndpoint);

            // Репозиторий для работы с веб-ресурсами
            WebResourceRepository webResourceRepository1 = new WebResourceRepository(service1);
            WebResourceRepository webResourceRepository2 = new WebResourceRepository(service2);

            var taskWebResource1 = webResourceRepository1.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);
            var taskWebResource2 = webResourceRepository2.FindByNameAsync(selectedFile.FriendlyFilePath, selectedFile.Extension);

            WebResource webresource1 = await taskWebResource1;
            WebResource webresource2 = await taskWebResource2;

            if (webresource1 != null)
            {
                this._iWriteToOutput.WriteToOutput("{0}: WebResource founded by name.", connectionData1.Name);

                connectionData1.AddMapping(webresource1.Id, selectedFile.FriendlyFilePath);

                connectionData1.ConnectionConfiguration.Save();
            }
            else
            {
                Guid? webId = connectionData1.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webId.HasValue)
                {
                    webresource1 = await webResourceRepository1.FindByIdAsync(webId.Value);

                    if (webresource1 != null)
                    {
                        this._iWriteToOutput.WriteToOutput("{0}: WebResource not founded by name. Last link WebResource is selected for difference.", connectionData1.Name);

                        connectionData1.AddMapping(webresource1.Id, selectedFile.FriendlyFilePath);

                        connectionData1.ConnectionConfiguration.Save();
                    }
                }
            }

            if (webresource2 != null)
            {
                this._iWriteToOutput.WriteToOutput("{0}: WebResource founded by name.", connectionData2.Name);

                connectionData2.AddMapping(webresource2.Id, selectedFile.FriendlyFilePath);

                connectionData2.ConnectionConfiguration.Save();
            }
            else
            {
                Guid? webId = connectionData2.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (webId.HasValue)
                {
                    webresource2 = await webResourceRepository2.FindByIdAsync(webId.Value);

                    if (webresource2 != null)
                    {
                        this._iWriteToOutput.WriteToOutput("{0}: WebResource not founded by name. Last link WebResource is selected for difference.", connectionData2.Name);

                        connectionData2.AddMapping(webresource2.Id, selectedFile.FriendlyFilePath);

                        connectionData2.ConnectionConfiguration.Save();
                    }
                }
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
            }
            
            if (webresource1 == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.WebResourceNotFoundedInConnectionFormat2, connectionData1.Name, selectedFile.FileName);
            }

            if (webresource2 == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.WebResourceNotFoundedInConnectionFormat2, connectionData2.Name, selectedFile.FileName);
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

                filePath1 = FileOperations.GetNewTempFile(webresource1.Name, selectedFile.Extension);
                fileTitle1 = connectionData1.Name + "." + selectedFile.FileName + " - " + filePath1;

                File.WriteAllBytes(filePath1, array);
            }

            if (webresource2 != null)
            {
                var contentWebResource2 = webresource2.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource2);

                filePath2 = FileOperations.GetNewTempFile(webresource2.Name, selectedFile.Extension);
                fileTitle2 = connectionData2.Name + "." + selectedFile.FileName + " - " + filePath2;

                File.WriteAllBytes(filePath2, array);
            }

            switch (differenceType)
            {
                case ShowDifferenceThreeFileType.OneByOne:
                    ShowDifferenceOneByOne(commonConfig, fileLocalPath, fileLocalTitle, filePath1, fileTitle1, filePath2, fileTitle2);
                    break;

                case ShowDifferenceThreeFileType.TwoConnections:
                    this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
                    break;

                case ShowDifferenceThreeFileType.ThreeWay:
                    ShowDifferenceThreeWay(commonConfig, fileLocalPath, fileLocalTitle, filePath1, fileTitle1, filePath2, fileTitle2);
                    break;
                default:
                    break;
            }
        }

        private void ShowDifferenceThreeWay(CommonConfiguration commonConfig, string fileLocalPath, string fileLocalTitle, string filePath1, string fileTitle1, string filePath2, string fileTitle2)
        {
            if (!File.Exists(fileLocalPath))
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, fileLocalPath);
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
                    this._iWriteToOutput.ProcessStartProgramComparer(fileLocalPath, filePath1, fileLocalTitle, fileTitle1);
                }

                if (File.Exists(filePath2))
                {
                    this._iWriteToOutput.ProcessStartProgramComparer(fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
                }
            }
        }

        private void ShowDifferenceOneByOne(CommonConfiguration commonConfig, string fileLocalPath, string fileLocalTitle, string filePath1, string fileTitle1, string filePath2, string fileTitle2)
        {
            bool existsFileLocal = File.Exists(fileLocalPath);
            bool existsFile1 = File.Exists(filePath1);
            bool existsFile2 = File.Exists(filePath2);

            if (existsFileLocal && existsFile1)
            {
                this._iWriteToOutput.ProcessStartProgramComparer(fileLocalPath, filePath1, fileLocalTitle, fileTitle1);
            }

            if (existsFileLocal && existsFile2)
            {
                this._iWriteToOutput.ProcessStartProgramComparer(fileLocalPath, filePath2, fileLocalTitle, fileTitle2);
            }

            if (existsFile1 && existsFile2)
            {
                this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
            }

            int total = Convert.ToInt32(existsFileLocal) + Convert.ToInt32(existsFile1) + Convert.ToInt32(existsFile2);

            if (total == 1)
            {
                _iWriteToOutput.WriteToOutputFilePathUri(fileLocalPath);
                _iWriteToOutput.OpenFile(fileLocalPath);

                _iWriteToOutput.WriteToOutputFilePathUri(fileLocalPath);
                _iWriteToOutput.OpenFile(filePath1);

                _iWriteToOutput.WriteToOutputFilePathUri(fileLocalPath);
                _iWriteToOutput.OpenFile(filePath2);
            }
        }

        #endregion Сравнение трех файлов вебресурсов.

        #region Сравнение файлов и веб-ресурсов по разным параметрам.

        public async Task ExecuteMultiDifferenceFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.MultiDifferenceFormat2, connectionData?.Name, openFilesType.ToString());

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                this._iWriteToOutput.WriteToOutput(Properties.OperationNames.CheckingFilesEncoding);

                CheckController.CheckingFilesEncoding(this._iWriteToOutput, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput(string.Empty);

                await MultiDifferenceFiles(selectedFiles, openFilesType, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task MultiDifferenceFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (openFilesType == OpenFilesType.All)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ShowingDifferenceIsNotAllowedForFormat1, openFilesType.ToString());
                return;
            }

            var compareResult = await CompareController.GetWebResourcesWithType(this._iWriteToOutput, selectedFiles, openFilesType, connectionData);

            var listFilesToDifference = compareResult.Item2.Where(f => f.Item2 != null);

            if (!listFilesToDifference.Any())
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoFilesForDifference);
                return;
            }
            
            this._iWriteToOutput.WriteToOutput(string.Empty);
            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.StartingCompareProgramForCountFilesFormat1, listFilesToDifference.Count());

            foreach (var item in listFilesToDifference.OrderBy(file => file.Item1.FilePath))
            {
                var selectedFile = item.Item1;
                var webresource = item.Item2;

                if (webresource != null)
                {
                    var contentWebResource = webresource.Content;

                    var webResourceName = webresource.Name;

                    var array = Convert.FromBase64String(contentWebResource);

                    string tempFilePath = FileOperations.GetNewTempFile(webResourceName, selectedFile.Extension);

                    File.WriteAllBytes(tempFilePath, array);

                    string file1 = selectedFile.FilePath;
                    string fileTitle1 = selectedFile.FileName;

                    string file2 = tempFilePath;
                    string fileTitle2 = connectionData.Name + "." + selectedFile.FileName + " - " + tempFilePath;

                    this._iWriteToOutput.ProcessStartProgramComparer(file1, file2, fileTitle1, fileTitle2);
                }
            }
        }

        #endregion Сравнение файлов и веб-ресурсов по разным параметрам.

        #region Различия отчета и файла.

        public async Task ExecuteDifferenceReport(SelectedFile selectedFile, string fieldName, string fieldTitle, bool isCustom, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceReportFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await DifferenceReport(selectedFile, fieldName, fieldTitle, isCustom, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task DifferenceReport(SelectedFile selectedFile, string fieldName, string fieldTitle, bool isCustom, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                fieldName = Report.Schema.Attributes.originalbodytext;
                fieldTitle = "OriginalBodyText";
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            // Репозиторий для работы с веб-ресурсами
            ReportRepository reportRepository = new ReportRepository(service);

            Report reportEntity = null;

            if (isCustom)
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
                        DTEHelper.WriteExceptionToOutput(ex);
                    }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult.GetValueOrDefault())
                {
                    if (selectedReportId.HasValue)
                    {
                        this._iWriteToOutput.WriteToOutput("Custom report is selected.");

                        reportEntity = await reportRepository.GetByIdAsync(selectedReportId.Value);

                        connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);

                        connectionData.ConnectionConfiguration.Save();
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput("!Warning. Report not exists. name: {0}.", selectedFile.Name);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.DifferenceWasCancelled);
                    return;
                }
            }
            else
            {
                reportEntity = await reportRepository.FindAsync(selectedFile.FileName);

                if (reportEntity != null)
                {
                    this._iWriteToOutput.WriteToOutput("Report founded by name.");

                    connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);

                    connectionData.ConnectionConfiguration.Save();
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
                        this._iWriteToOutput.WriteToOutput("Report not founded by name. Last link report is selected for difference.");

                        connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);

                        connectionData.ConnectionConfiguration.Save();
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput("Report not founded by name and has not Last link.");
                        this._iWriteToOutput.WriteToOutput("Starting Custom Report selection form.");

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
                                DTEHelper.WriteExceptionToOutput(ex);
                            }
                        });
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();

                        t.Join();

                        if (dialogResult.GetValueOrDefault())
                        {
                            if (selectedReportId.HasValue)
                            {
                                this._iWriteToOutput.WriteToOutput("Custom report is selected.");

                                reportEntity = await reportRepository.GetByIdAsync(selectedReportId.Value);

                                connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);

                                connectionData.ConnectionConfiguration.Save();
                            }
                            else
                            {
                                this._iWriteToOutput.WriteToOutput("!Warning. Report not exists. name: {0}.", selectedFile.Name);
                            }
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.DifferenceWasCancelled);
                            return;
                        }
                    }
                }
            }
            
            if (reportEntity == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ReportNotFoundedInConnectionFormat2, connectionData.Name, selectedFile.FileName);
                return;
            }

            var reportName = EntityFileNameFormatter.GetReportFileName(connectionData.Name, reportEntity.Name, reportEntity.Id, fieldTitle, selectedFile.Extension);

            string temporaryFilePath = FileOperations.GetNewTempFile(Path.GetFileNameWithoutExtension(reportName), selectedFile.Extension);

            var textReport = reportEntity.GetAttributeValue<string>(fieldName);

            if (ContentCoparerHelper.TryParseXml(textReport, out var doc))
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

            this._iWriteToOutput.WriteToOutput("Starting Compare Program for {0} and {1}", selectedFile.FriendlyFilePath, reportName);

            string file1 = selectedFile.FilePath;
            string file2 = temporaryFilePath;

            string fileTitle1 = selectedFile.FileName;
            string fileTitle2 = connectionData.Name + "." + selectedFile.FileName + " - " + temporaryFilePath;

            this._iWriteToOutput.ProcessStartProgramComparer(file1, file2, fileTitle1, fileTitle2);
        }

        #endregion Различия отчета и файла.

        #region Различия трех файлов отчетов.

        public async Task ExecuteThreeFileDifferenceReport(SelectedFile selectedFile, string fieldName, string fieldTitle, ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceLocalFileAndTwoReportsFormat3, differenceType, connectionData1?.Name, connectionData2?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ThreeFileDifferenceReport(selectedFile, fieldName, fieldTitle, connectionData1, connectionData2, differenceType, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ThreeFileDifferenceReport(SelectedFile selectedFile, string fieldName, string fieldTitle, ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType, CommonConfiguration commonConfig)
        {
            if (connectionData1 == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCRMConnection1);
                return;
            }

            if (connectionData2 == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCRMConnection2);
                return;
            }

            if (differenceType == ShowDifferenceThreeFileType.ThreeWay)
            {
                if (!File.Exists(selectedFile.FilePath))
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                    return;
                }
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                fieldName = Report.Schema.Attributes.originalbodytext;
                fieldTitle = "OriginalBodyText";
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
            this._iWriteToOutput.WriteToOutput(string.Empty);
            this._iWriteToOutput.WriteToOutput(connectionData1.GetConnectionDescription());
            this._iWriteToOutput.WriteToOutput(string.Empty);
            this._iWriteToOutput.WriteToOutput(connectionData2.GetConnectionDescription());

            var task1 = QuickConnection.ConnectAsync(connectionData1);
            var task2 = QuickConnection.ConnectAsync(connectionData2);

            var service1 = await task1;
            var service2 = await task2;

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointConnectionFormat2, connectionData1.Name, service1.CurrentServiceEndpoint);
            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointConnectionFormat2, connectionData2.Name, service2.CurrentServiceEndpoint);

            // Репозиторий для работы с веб-ресурсами
            ReportRepository reportRepository1 = new ReportRepository(service1);
            ReportRepository reportRepository2 = new ReportRepository(service2);

            var taskReport1 = reportRepository1.FindAsync(selectedFile.FileName);
            var taskReport2 = reportRepository2.FindAsync(selectedFile.FileName);

            Report reportEntity1 = await taskReport1;
            Report reportEntity2 = await taskReport2;

            if (reportEntity1 != null)
            {
                this._iWriteToOutput.WriteToOutput("{0}: Report founded by name.", connectionData1.Name);

                connectionData1.AddMapping(reportEntity1.Id, selectedFile.FriendlyFilePath);

                connectionData1.ConnectionConfiguration.Save();
            }
            else
            {
                Guid? reportId = connectionData1.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (reportId.HasValue)
                {
                    reportEntity1 = await reportRepository1.GetByIdAsync(reportId.Value);

                    if (reportEntity1 != null)
                    {
                        this._iWriteToOutput.WriteToOutput("{0}: Report not founded by name. Last link report is selected for difference.", connectionData1.Name);

                        connectionData1.AddMapping(reportEntity1.Id, selectedFile.FriendlyFilePath);

                        connectionData1.ConnectionConfiguration.Save();
                    }
                }
            }

            if (reportEntity2 != null)
            {
                this._iWriteToOutput.WriteToOutput("{0}: Report founded by name.", connectionData2.Name);

                connectionData2.AddMapping(reportEntity2.Id, selectedFile.FriendlyFilePath);

                connectionData2.ConnectionConfiguration.Save();
            }
            else
            {
                Guid? reportId = connectionData2.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (reportId.HasValue)
                {
                    reportEntity2 = await reportRepository2.GetByIdAsync(reportId.Value);

                    if (reportEntity2 != null)
                    {
                        this._iWriteToOutput.WriteToOutput("{0}: Report not founded by name. Last link report is selected for difference.", connectionData2.Name);

                        connectionData2.AddMapping(reportEntity2.Id, selectedFile.FriendlyFilePath);

                        connectionData2.ConnectionConfiguration.Save();
                    }
                }
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
            }

            if (reportEntity1 == null)
            {
                this._iWriteToOutput.WriteToOutput("{0}: Report not founded in CRM: {1}", connectionData1.Name, selectedFile.FileName);
            }

            if (reportEntity2 == null)
            {
                this._iWriteToOutput.WriteToOutput("{0}: Report not founded in CRM: {1}", connectionData2.Name, selectedFile.FileName);
            }

            string fileLocalPath = selectedFile.FilePath;
            string fileLocalTitle = selectedFile.FileName;

            string filePath1 = string.Empty;
            string fileTitle1 = string.Empty;

            string filePath2 = string.Empty;
            string fileTitle2 = string.Empty;

            if (reportEntity1 != null)
            {
                var textReport = reportEntity1.OriginalBodyText;

                if (ContentCoparerHelper.TryParseXml(textReport, out var doc))
                {
                    textReport = doc.ToString();
                }

                var reportName = EntityFileNameFormatter.GetReportFileName(connectionData1.Name, reportEntity1.Name, reportEntity1.Id, fieldTitle, selectedFile.Extension);

                filePath1 = FileOperations.GetNewTempFile(Path.GetFileNameWithoutExtension(reportName), selectedFile.Extension);
                fileTitle1 = connectionData1.Name + "." + selectedFile.FileName + " - " + filePath1;

                File.WriteAllText(filePath1, textReport, new UTF8Encoding(false));
            }

            if (reportEntity2 != null)
            {
                var textReport = reportEntity2.OriginalBodyText;

                if (ContentCoparerHelper.TryParseXml(textReport, out var doc))
                {
                    textReport = doc.ToString();
                }

                var reportName = EntityFileNameFormatter.GetReportFileName(connectionData2.Name, reportEntity2.Name, reportEntity2.Id, fieldTitle, selectedFile.Extension);

                filePath2 = FileOperations.GetNewTempFile(Path.GetFileNameWithoutExtension(reportName), selectedFile.Extension);
                fileTitle1 = connectionData2.Name + "." + selectedFile.FileName + " - " + filePath2;

                File.WriteAllText(filePath2, textReport, new UTF8Encoding(false));
            }

            switch (differenceType)
            {
                case ShowDifferenceThreeFileType.OneByOne:
                    ShowDifferenceOneByOne(commonConfig, fileLocalPath, fileLocalTitle, filePath1, fileTitle1, filePath2, fileTitle2);
                    break;

                case ShowDifferenceThreeFileType.TwoConnections:
                    this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
                    break;

                case ShowDifferenceThreeFileType.ThreeWay:
                    ShowDifferenceThreeWay(commonConfig, fileLocalPath, fileLocalTitle, filePath1, fileTitle1, filePath2, fileTitle2);
                    break;
                default:
                    break;
            }
        }

        #endregion Различия трех файлов отчетов.
    }
}