using EnvDTE80;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class ReportController
    {
        private IWriteToOutput _iWriteToOutput = null;

        private const int timeDelay = 2000;

        public ReportController(IWriteToOutput outputWindow)
        {
            this._iWriteToOutput = outputWindow;
        }

        #region Различия отчета и файла.

        public async Task ExecuteUpdatingReport(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingReportFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await UpdatingReport(selectedFile, connectionData, commonConfig);
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

        private async Task UpdatingReport(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (connectionData.IsReadOnly)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            bool isconnectionDataDirty = false;

            // Репозиторий для работы с веб-ресурсами
            ReportRepository reportRepository = new ReportRepository(service);

            if (File.Exists(selectedFile.FilePath))
            {
                Report reportEntity = null;

                reportEntity = await reportRepository.FindAsync(selectedFile.FileName);

                if (reportEntity != null)
                {
                    this._iWriteToOutput.WriteToOutput("Report founded by name.");

                    isconnectionDataDirty = true;
                    connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);
                }
                else
                {
                    Guid? lastReportId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                    this._iWriteToOutput.WriteToOutput("Starting Custom Report selection form.");

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
                            DTEHelper.WriteExceptionToOutput(ex);
                        }
                    }));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    t.Join();

                    if (dialogResult.GetValueOrDefault())
                    {
                        if (selectedReportId.HasValue)
                        {
                            this._iWriteToOutput.WriteToOutput("Custom report is selected.");

                            reportEntity = await reportRepository.GetByIdAsync(selectedReportId.Value);

                            isconnectionDataDirty = true;
                            connectionData.AddMapping(reportEntity.Id, selectedFile.FriendlyFilePath);
                        }
                        else
                        {
                            this._iWriteToOutput.WriteToOutput("!Warning. Report not exists. name: {0}.", selectedFile.Name);
                        }
                    }
                    else
                    {
                        this._iWriteToOutput.WriteToOutput("Updating was cancelled.");
                        return;
                    }
                }

                if (reportEntity != null)
                {
                    var update = new Entity(Report.EntityLogicalName);
                    update.Id = reportEntity.Id;

                    update.Attributes[Report.Schema.Attributes.bodytext] = File.ReadAllText(selectedFile.FilePath);

                    await service.UpdateAsync(update);

                    this._iWriteToOutput.WriteToOutput("Report updated in CRM: {0} - {1} - {2}", reportEntity.Name, reportEntity.FileName, reportEntity.ReportNameOnSRS);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ReportNotFoundedInConnectionFormat2, connectionData.Name, selectedFile.FileName);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
            }

            if (isconnectionDataDirty)
            {
                //Сохранение настроек после публикации
                connectionData.ConnectionConfiguration.Save();
            }
        }

        #endregion Различия отчета и файла.
    }
}
