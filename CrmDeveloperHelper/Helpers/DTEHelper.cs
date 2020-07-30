using EnvDTE80;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public DTE2 ApplicationObject { get; private set; }

        private readonly MainController Controller;

        private static DTEHelper _singleton;

        public static DTEHelper Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    var applicationObject = CrmDeveloperHelperPackage.Singleton?.ApplicationObject;

                    if (applicationObject != null)
                    {
                        _singleton = new DTEHelper(applicationObject);
                    }
                }

                return _singleton;
            }
        }

        private static readonly object syncObject = new object();

        public static DTEHelper Create(DTE2 applicationObject)
        {
            if (applicationObject == null)
            {
                throw new ArgumentNullException(nameof(applicationObject));
            }

            lock (syncObject)
            {
                if (_singleton != null)
                {
                    return _singleton;
                }

                _singleton = new DTEHelper(applicationObject);

                return _singleton;
            }
        }

        private DTEHelper(DTE2 applicationObject)
        {
            this.ApplicationObject = applicationObject ?? throw new ArgumentNullException(nameof(applicationObject));

            this.Controller = new MainController(this);

            System.Threading.Thread clearTempFiles = new System.Threading.Thread(ClearTemporaryFiles);
            clearTempFiles.Start();
        }

        private void ClearTemporaryFiles()
        {
            string directory = FileOperations.GetTempFileFolder();

            if (Directory.Exists(directory))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);

                    var files = dir.GetFiles();

                    foreach (var item in files)
                    {
                        try
                        {
                            item.Delete();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void CheckWishToChangeCurrentConnection(ConnectionData connectionData)
        {
            if (connectionData == null || connectionData.ConnectionConfiguration == null)
            {
                return;
            }

            if (connectionData.ConnectionConfiguration.CurrentConnectionData?.ConnectionId == connectionData.ConnectionId)
            {
                return;
            }

            if ((System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) != 0)
            {
                connectionData.ConnectionConfiguration.SetCurrentConnection(connectionData.ConnectionId);
                connectionData.ConnectionConfiguration.Save();
                this.WriteToOutput(null, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);
                this.ActivateOutputWindow(null);
            }
        }

        public bool HasCurrentCrmConnection(out ConnectionConfiguration connectionConfig)
        {
            bool result = false;

            connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData == null)
            {
                this.WriteToOutput(null, Properties.OutputStrings.CurrentCrmConnectionIsNotSelected);
                this.ActivateOutputWindow(null);

                var crmConnection = new WindowCrmConnectionList(this, connectionConfig, true);

                crmConnection.ShowDialog();

                connectionConfig.Save();
            }

            result = connectionConfig.CurrentConnectionData != null;

            return result;
        }

        private void GetConfigAndExecute(Action<CommonConfiguration> action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    action(commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        private void GetConnectionConfigAndExecute(ConnectionData connectionData, Action<ConnectionData, CommonConfiguration> action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    action(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        private void GetConnectionConfigOpenOptionsAndExecute(ConnectionData connectionData, Action<ConnectionData, CommonConfiguration, bool> action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                bool openOptions = (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != 0;

                try
                {
                    action(connectionData, commonConfig, openOptions);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        #region FetchXml

        public void HandleFetchXmlExecuting(ConnectionData connectionData, SelectedFile selectedFile, bool strictConnection)
        {
            if (selectedFile == null || !File.Exists(selectedFile.FilePath))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => CrmDeveloperHelperPackage.Singleton?.ExecuteFetchXmlQueryAsync(selectedFile.FilePath, conn, this, strictConnection));
        }

        #endregion FetchXml

        #region Connection Operations

        public static ConnectionData GetCurrentConnection()
        {
            ConnectionConfiguration connectionConfig = ConnectionConfiguration.Get();

            return connectionConfig?.CurrentConnectionData;
        }

        public void HandleConnectionOpenList()
        {
            ConnectionConfiguration connectionConfig = ConnectionConfiguration.Get();

            var worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowCrmConnectionList(this, connectionConfig);

                    form.ShowDialog();

                    connectionConfig.Save();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public void HandleConnectionTest(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => QuickConnection.TestConnectAsync(conn, this, null));
        }

        public void HandlerConnectionEdit(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => WindowHelper.OpenCrmConnectionCard(this, connectionData));
        }

        public void HandleConnectionOrganizationComparer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            ConnectionConfiguration crmConfig = ConnectionConfiguration.Get();

            if (commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartOrganizationComparer(crmConfig, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleConnectionOpenCrmWebSiteInWeb(ConnectionData connectionData, OpenCrmWebSiteType crmWebSiteType)
        {
            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    connectionData.OpenCrmWebSite(crmWebSiteType);
                }
                catch (Exception ex)
                {
                    this.WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleConnectionOpenEntityMetadataInWeb(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var dialog = new WindowSelectEntityName(connectionData, "EntityName");

                        if (dialog.ShowDialog().GetValueOrDefault())
                        {
                            string entityName = dialog.EntityTypeName;
                            int? entityTypeCode = dialog.EntityTypeCode;

                            connectionData = dialog.GetConnectionData();

                            CheckWishToChangeCurrentConnection(connectionData);

                            var idEntityMetadata = connectionData.GetEntityMetadataId(entityName);

                            if (idEntityMetadata.HasValue)
                            {
                                connectionData.OpenEntityMetadataInWeb(idEntityMetadata.Value);
                            }
                            else
                            {
                                ActivateOutputWindow(connectionData);
                                WriteToOutputEmptyLines(connectionData, commonConfig);

                                try
                                {
                                    Controller.StartOpeningEntityMetadataInWeb(connectionData, commonConfig, entityName, entityTypeCode);
                                }
                                catch (Exception ex)
                                {
                                    WriteErrorToOutput(connectionData, ex);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandleSelectAndPublishEntityCommand(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var dialog = new WindowSelectEntityName(connectionData, "EntityName");

                        if (dialog.ShowDialog().GetValueOrDefault())
                        {
                            string entityName = dialog.EntityTypeName;
                            int? entityTypeCode = dialog.EntityTypeCode;

                            connectionData = dialog.GetConnectionData();

                            ActivateOutputWindow(connectionData);
                            WriteToOutputEmptyLines(connectionData, commonConfig);

                            CheckWishToChangeCurrentConnection(connectionData);

                            try
                            {
                                Controller.StartPublishEntityMetadata(connectionData, commonConfig, entityName, entityTypeCode);
                            }
                            catch (Exception ex)
                            {
                                WriteErrorToOutput(connectionData, ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandleConnectionOpenEntityListInWeb(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var dialog = new WindowSelectEntityName(connectionData, "EntityName");

                        if (dialog.ShowDialog().GetValueOrDefault())
                        {
                            string entityName = dialog.EntityTypeName;
                            int? entityTypeCode = dialog.EntityTypeCode;

                            connectionData = dialog.GetConnectionData();

                            CheckWishToChangeCurrentConnection(connectionData);

                            var idEntityMetadata = connectionData.GetEntityMetadataId(entityName);

                            if (idEntityMetadata.HasValue)
                            {
                                connectionData.OpenEntityInstanceListInWeb(entityName);
                            }
                            else
                            {
                                ActivateOutputWindow(connectionData);
                                WriteToOutputEmptyLines(connectionData, commonConfig);

                                try
                                {
                                    Controller.StartOpeningEntityMetadataInWeb(connectionData, commonConfig, entityName, entityTypeCode);
                                }
                                catch (Exception ex)
                                {
                                    WriteErrorToOutput(connectionData, ex);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandleConnectionOpenFetchXmlFile(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var dialog = new WindowSelectEntityName(connectionData, "EntityName");

                        if (dialog.ShowDialog().GetValueOrDefault())
                        {
                            string entityName = dialog.EntityTypeName;
                            int? entityTypeCode = dialog.EntityTypeCode;

                            connectionData = dialog.GetConnectionData();

                            CheckWishToChangeCurrentConnection(connectionData);

                            var idEntityMetadata = connectionData.GetEntityMetadataId(entityName);

                            if (idEntityMetadata.HasValue)
                            {
                                this.OpenFetchXmlFile(connectionData, commonConfig, entityName);
                            }
                            else
                            {
                                ActivateOutputWindow(connectionData);
                                WriteToOutputEmptyLines(connectionData, commonConfig);

                                try
                                {
                                    Controller.StartOpeningEntityFetchXmlFile(connectionData, commonConfig, entityName, entityTypeCode);
                                }
                                catch (Exception ex)
                                {
                                    WriteErrorToOutput(connectionData, ex);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandleConnectionOpenFetchXmlFolder(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                string directoryPath = FileOperations.GetConnectionFetchXmlFolderPath(connectionData.ConnectionId);

                this.OpenFolder(connectionData, directoryPath);
            }
        }

        public void HandleConnectionOpenInfoFolder(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                string directoryPath = FileOperations.GetConnectionInformationFolderPath(connectionData.ConnectionId);

                this.OpenFolder(connectionData, directoryPath);
            }
        }

        public void HandleConnectionPublishAll(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                string message = string.Format(Properties.MessageBoxStrings.PublishAllInConnectionFormat1, connectionData.Name);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        this.Controller.StartPublishAll(connectionData);
                    }
                    catch (Exception ex)
                    {
                        this.WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        #endregion Connection Operations

        #region ListForPublish

        public void HandleFileCompareListForPublishCommand(ConnectionData connectionData, bool withDetails)
        {
            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            CheckWishToChangeCurrentConnection(connectionData);

            List<SelectedFile> selectedFiles = this.GetSelectedFilesFromListForPublish(FileOperations.SupportsWebResourceTextType).ToList();

            if (selectedFiles.Count > 0)
            {
                this.ShowListForPublish(connectionData);

                this.HandleWebResourceCompareCommand(connectionData, selectedFiles, withDetails);
            }
            else
            {
                this.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                this.ActivateOutputWindow(connectionData);
            }
        }

        public void HandleAddingIntoPublishListFilesByTypeCommand(IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartAddingIntoPublishListFilesByType(conn, commonConfig, selectedFiles, openFilesType));
        }

        public void HandleRemovingFromPublishListFilesByTypeCommand(IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartRemovingFromPublishListFilesByType(conn, commonConfig, selectedFiles, openFilesType));
        }

        #endregion ListForPublish

        public void HandleFileClearLink(List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartClearingLastLink(conn, selectedFiles));
        }

        public void OpenCommonConfiguration()
        {
            CommonConfiguration config = CommonConfiguration.Get();

            if (config != null)
            {
                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var form = new WindowCommonConfiguration(config);

                        form.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandleExportFormEvents(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var form = new WindowExportFormEvents(commonConfig);

                        if (form.ShowDialog().GetValueOrDefault())
                        {
                            try
                            {
                                Controller.StartExportingFormEvents(connectionData, commonConfig);
                            }
                            catch (Exception ex)
                            {
                                WriteErrorToOutput(connectionData, ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandleXsdSchemaExport(string[] fileNamesColl)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
            {
                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var form = new WindowSelectFolderForExport(null, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                        if (form.ShowDialog().GetValueOrDefault())
                        {
                            commonConfig.FolderForExport = form.SelectedFolder;
                            commonConfig.DefaultFileAction = form.GetFileAction();

                            commonConfig.Save();

                            ActivateOutputWindow(null);
                            WriteToOutputEmptyLines(null, commonConfig);

                            try
                            {
                                foreach (var fileName in fileNamesColl)
                                {
                                    Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                                    StreamResourceInfo info = Application.GetResourceStream(uri);

                                    var doc = XDocument.Load(info.Stream);
                                    info.Stream.Dispose();

                                    var filePath = Path.Combine(commonConfig.FolderForExport, fileName);

                                    doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                                    this.WriteToOutput(null, string.Empty);
                                    this.WriteToOutput(null, string.Empty);
                                    this.WriteToOutput(null, string.Empty);

                                    this.WriteToOutput(null, "{0} exported.", fileName);

                                    this.WriteToOutput(null, string.Empty);

                                    this.WriteToOutputFilePathUri(null, filePath);

                                    PerformAction(null, filePath, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteErrorToOutput(null, ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandleXsdSchemaOpenFolder()
        {
            var folderPath = FileOperations.GetSchemaXsdFolder();

            this.OpenFolder(null, folderPath);
        }

        public void HandleExportTraceEnableFile()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
            {
                string fileName = "TraceEnable.reg";

                var dialog = new Microsoft.Win32.SaveFileDialog()
                {
                    DefaultExt = ".reg",

                    Filter = "Registry Edit (.reg)|*.reg",
                    FilterIndex = 1,

                    RestoreDirectory = true,
                    FileName = fileName,
                };

                if (!string.IsNullOrEmpty(commonConfig.FolderForExport))
                {
                    dialog.InitialDirectory = commonConfig.FolderForExport;
                }

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow(null);
                    WriteToOutputEmptyLines(null, commonConfig);

                    try
                    {
                        Uri uri = FileOperations.GetResourceUri(fileName);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        var filePath = dialog.FileName;

                        byte[] buffer = new byte[16345];
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int read;
                            while ((read = info.Stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fs.Write(buffer, 0, read);
                            }
                        }

                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutput(null, "{0} exported.", fileName);

                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutputFilePathUri(null, filePath);

                        SelectFileInFolder(null, filePath);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(null, ex);
                    }
                }
            }
        }

        public void HandleOpenConfigFolderCommand()
        {
            var directoryPath = FileOperations.GetConfigurationFolder();

            this.OpenFolder(null, directoryPath);
        }

        public static string GetRelativePath(EnvDTE.Project project)
        {
            List<string> names = new List<string>();

            if (project != null)
            {
                AddNamesRecursive(names, project);
            }

            names.Reverse();

            return string.Join(@"\", names);
        }

        private static void AddNamesRecursive(List<string> names, EnvDTE.Project project)
        {
            if (project != null)
            {
                names.Add(project.Name);

                if (project.ParentProjectItem != null && project.ParentProjectItem.ContainingProject != null)
                {
                    AddNamesRecursive(names, project.ParentProjectItem.ContainingProject);
                }
            }
        }
    }
}