using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class OpenFilesController
    {
        private IWriteToOutput _iWriteToOutput = null;

        public OpenFilesController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        public async Task ExecuteOpenFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningFilesFormat2, connectionData?.Name, openFilesType.ToString());

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OperationNames.CheckingFilesEncoding);

                    CheckController.CheckingFilesEncoding(this._iWriteToOutput, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                    this._iWriteToOutput.WriteToOutput(string.Empty);
                }

                await OpenFiles(selectedFiles, openFilesType, inTextEditor, connectionData, commonConfig);
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

        private async Task OpenFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            var compareResult = await CompareController.GetWebResourcesWithType(this._iWriteToOutput, selectedFiles, openFilesType, connectionData);

            var filesToOpen = compareResult.Item2;

            if (filesToOpen.Any())
            {
                var orderEnumrator = filesToOpen.Select(s => s.Item1).OrderBy(s => s.FriendlyFilePath);

                if (inTextEditor)
                {
                    foreach (var item in orderEnumrator)
                    {
                        this._iWriteToOutput.WriteToOutputFilePathUri(item.FilePath);
                        this._iWriteToOutput.OpenFileInTextEditor(item.FilePath);
                    }
                }
                else
                {
                    foreach (var item in orderEnumrator)
                    {
                        this._iWriteToOutput.WriteToOutputFilePathUri(item.FilePath);
                        this._iWriteToOutput.OpenFileInVisualStudio(item.FilePath);
                    }
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("No files for open.");
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }       
    }
}
