using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
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
            await CheckEncodingConnectFindWebResourceExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningFilesFormat2
                , selectedFiles
                , openFilesType
                , (compareResult) => OpenFiles(compareResult, inTextEditor)
                , openFilesType.ToString()
            );
        }

        private void OpenFiles(Tuple<IOrganizationServiceExtented, TupleList<SelectedFile, WebResource>> compareResult, bool inTextEditor)
        {
            IOrganizationServiceExtented service = compareResult.Item1;

            var filesToOpen = compareResult.Item2;

            if (filesToOpen.Any())
            {
                var orderEnumrator = filesToOpen.Select(s => s.Item1).OrderBy(s => s.FriendlyFilePath);

                if (inTextEditor)
                {
                    foreach (var item in orderEnumrator)
                    {
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, item.FilePath);
                        this._iWriteToOutput.OpenFileInTextEditor(service.ConnectionData, item.FilePath);
                    }
                }
                else
                {
                    foreach (var item in orderEnumrator)
                    {
                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, item.FilePath);
                        this._iWriteToOutput.OpenFileInVisualStudio(service.ConnectionData, item.FilePath);
                    }
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "No files for open.");
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }
        }
    }
}
