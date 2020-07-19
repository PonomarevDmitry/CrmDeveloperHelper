using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class ReportController : BaseController<IWriteToOutput>
    {
        private const int timeDelay = 2000;

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
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ReportNotFoundedInConnectionFormat2, connectionData.Name, selectedFile.FileName);
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
    }
}
