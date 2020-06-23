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
                , (conn, service, filesToOpen) => OpenFiles(conn, service, filesToOpen, inTextEditor)
                , openFilesType.ToString()
            );
        }

        private void OpenFiles(ConnectionData connectionData, IOrganizationServiceExtented service, TupleList<SelectedFile, WebResource> filesToOpen, bool inTextEditor)
        {
            if (!filesToOpen.Any())
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "No files for open.");
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

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
    }
}