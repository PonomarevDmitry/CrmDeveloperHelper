using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class OpenFilesController : BaseController<IWriteToOutput>
    {
        public OpenFilesController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        public async Task ExecuteOpenFiles(ConnectionData connectionData, CommonConfiguration commonConfig, List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor)
        {
            string operation = string.Format(Properties.OperationNames.OpeningFilesFormat2, connectionData?.Name, openFilesType.ToString());

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                await OpenFiles(selectedFiles, openFilesType, inTextEditor, connectionData, commonConfig);
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

        private async Task OpenFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            var compareResult = await CompareController.GetWebResourcesWithType(this._iWriteToOutput, selectedFiles, openFilesType, connectionData);

            if (compareResult == null || compareResult.Item1 == null)
            {
                return;
            }

            var filesToOpen = compareResult.Item2;

            if (filesToOpen.Any())
            {
                var orderEnumrator = filesToOpen.Select(s => s.Item1).OrderBy(s => s.FriendlyFilePath);

                if (inTextEditor)
                {
                    foreach (var item in orderEnumrator)
                    {
                        this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, item.FilePath);
                        this._iWriteToOutput.OpenFileInTextEditor(connectionData, item.FilePath);
                    }
                }
                else
                {
                    foreach (var item in orderEnumrator)
                    {
                        this._iWriteToOutput.WriteToOutputFilePathUri(connectionData, item.FilePath);
                        this._iWriteToOutput.OpenFileInVisualStudio(connectionData, item.FilePath);
                    }
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "No files for open.");
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }
        }
    }
}
