using EnvDTE;
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

            connectionConfig = Model.ConnectionConfiguration.Get();

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

        public void HandleFileCompareCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartComparing(conn, selectedFiles, withDetails));
        }

        #region Plugin and Messages Trees

        public void HandleOpenPluginTree(string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            HandleOpenPluginTree(null, entityFilter, pluginTypeFilter, messageFilter);
        }

        public void HandleOpenPluginTree(ConnectionData connectionData, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingPluginTree(conn, commonConfig, entityFilter, pluginTypeFilter, messageFilter));
        }

        public void HandleSdkMessageTree()
        {
            HandleSdkMessageTree(null);
        }

        public void HandleSdkMessageTree(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingSdkMessageTree(conn, commonConfig, selection, null));
        }

        public void HandleSdkMessageRequestTree()
        {
            HandleSdkMessageRequestTree(null, null);
        }

        public void HandleSdkMessageRequestTree(ConnectionData connectionData)
        {
            HandleSdkMessageRequestTree(connectionData, null);
        }

        public void HandleSdkMessageRequestTree(ConnectionData connectionData, SelectedItem selectedItem)
        {
            string selection = string.Empty;

            if (selectedItem == null)
            {
                selection = GetSelectedText();
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingSdkMessageRequestTree(conn, commonConfig, selection, null, selectedItem));
        }

        #endregion Plugin and Messages Trees

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

        #region Solution

        public void HandleSolutionOpenLastSelected(ConnectionData connectionData, string solutionUniqueName, ActionOpenComponent action)
        {
            if (string.IsNullOrEmpty(solutionUniqueName))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSolutionOpening(conn, commonConfig, solutionUniqueName, action));
        }

        public string GetLastSolutionUniqueName()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            return connectionConfig?.CurrentConnectionData?.LastSelectedSolutionsUniqueName?.FirstOrDefault();
        }

        public void HandleOpenSolutionExplorerWindow(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenSolutionExplorerWindow(conn, commonConfig, null));
        }

        public void HandleSolutionAddFileToFolder()
        {
            SelectedItem selectedItem = GetSelectedProjectItem();

            if (selectedItem == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartOpenSolutionExplorerWindow(conn, commonConfig, selectedItem));
        }

        #endregion Solution

        public void HandleFileClearLink(List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartClearingLastLink(conn, selectedFiles));
        }

        public void HandleCheckFileEncodingCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartCheckFileEncoding(selectedFiles);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleOpenFilesCommand(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                if (inTextEditor && !File.Exists(commonConfig.TextEditorProgram))
                {
                    return;
                }

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                Controller.StartOpeningFiles(connectionData, commonConfig, selectedFiles, openFilesType, inTextEditor);
            }
        }

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

            List<SelectedFile> selectedFiles = this.GetSelectedFilesFromListForPublish().ToList();

            if (selectedFiles.Count > 0)
            {
                this.ShowListForPublish(connectionData);

                this.HandleFileCompareCommand(connectionData, selectedFiles, withDetails);
            }
            else
            {
                this.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                this.ActivateOutputWindow(connectionData);
            }
        }

        public void OpenConnectionList()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            System.Threading.Thread worker = new System.Threading.Thread(() =>
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

        public void TestConnection(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => QuickConnection.TestConnectAsync(conn, this, null));
        }

        public void EditConnection(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => WindowHelper.OpenCrmConnectionCard(this, connectionData));
        }

        public void OpenCommonConfiguration()
        {
            CommonConfiguration config = CommonConfiguration.Get();

            if (config != null)
            {
                var form = new WindowCommonConfiguration(config);

                form.ShowDialog();
            }
        }

        public ConnectionData GetCurrentConnection()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            return connectionConfig?.CurrentConnectionData;
        }

        #region Security

        public void HandleOpenSystemUsersExplorer()
        {
            HandleOpenSystemUsersExplorer(null);
        }

        public void HandleOpenSystemUsersExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingSystemUserExplorer(conn, commonConfig, selection));
        }

        public void HandleOpenTeamsExplorer()
        {
            HandleOpenTeamsExplorer(null);
        }

        public void HandleOpenTeamsExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingTeamsExplorer(conn, commonConfig, selection));
        }

        public void HandleOpenSecurityRolesExplorer()
        {
            HandleOpenSecurityRolesExplorer(null);
        }

        public void HandleOpenSecurityRolesExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingSecurityRolesExplorer(conn, commonConfig, selection));
        }

        #endregion Security

        public void HandleOrganizationComparer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            ConnectionConfiguration crmConfig = Model.ConnectionConfiguration.Get();

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

        public void HandleTraceReaderWindow(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartTraceReaderWindow(conn, commonConfig));
        }

        public void HandleExportFileWithEntityMetadata()
        {
            HandleExportFileWithEntityMetadata(null, null);
        }

        public void HandleExportFileWithEntityMetadata(ConnectionData connectionData)
        {
            HandleExportFileWithEntityMetadata(connectionData, null);
        }

        public void HandleExportFileWithEntityMetadata(ConnectionData connectionData, SelectedItem selectedItem)
        {
            string selection = string.Empty;

            if (selectedItem == null)
            {
                selection = GetSelectedText();
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpeningEntityMetadataExplorer(conn, commonConfig, selection, selectedItem));
        }

        public void HandleEntityMetadataFileGenerationOptions()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            ActivateOutputWindow(null);
            WriteToOutputEmptyLines(null, commonConfig);

            try
            {
                Controller.StartOpeningEntityMetadataFileGenerationOptions();
            }
            catch (Exception ex)
            {
                WriteErrorToOutput(null, ex);
            }
        }

        public void HandleGlobalOptionSetsMetadataFileGenerationOptions()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            ActivateOutputWindow(null);
            WriteToOutputEmptyLines(null, commonConfig);

            try
            {
                Controller.StartOpeningGlobalOptionSetsMetadataFileGenerationOptions();
            }
            catch (Exception ex)
            {
                WriteErrorToOutput(null, ex);
            }
        }

        public void HandleOpenEntityAttributeExplorer()
        {
            HandleOpenEntityAttributeExplorer(null);
        }

        public void HandleOpenEntityAttributeExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerEntityAttribute(conn, commonConfig, selection));
        }

        public void HandleOpenEntityKeyExplorer()
        {
            HandleOpenEntityKeyExplorer(null);
        }

        public void HandleOpenEntityKeyExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerEntityKey(conn, commonConfig, selection));
        }

        public void HandleOpenEntityRelationshipOneToManyExplorer()
        {
            HandleOpenEntityRelationshipOneToManyExplorer(null);
        }

        public void HandleOpenEntityRelationshipOneToManyExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerEntityRelationshipOneToMany(conn, commonConfig, selection));
        }

        public void HandleOpenEntityRelationshipManyToManyExplorer()
        {
            HandleOpenEntityRelationshipManyToManyExplorer(null);
        }

        public void HandleOpenEntityRelationshipManyToManyExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerEntityRelationshipManyToMany(conn, commonConfig, selection));
        }

        public void HandleOpenEntityPrivilegesExplorer()
        {
            HandleOpenEntityPrivilegesExplorer(null);
        }

        public void HandleOpenEntityPrivilegesExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerEntityPrivileges(conn, commonConfig, selection));
        }

        public void HandleOpenOtherPrivilegesExplorer()
        {
            HandleOpenOtherPrivilegesExplorer(null);
        }

        public void HandleOpenOtherPrivilegesExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerOtherPrivileges(conn, commonConfig, selection));
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
        }

        public void HandleExportGlobalOptionSets()
        {
            HandleExportGlobalOptionSets(null, null);
        }

        public void HandleExportGlobalOptionSets(ConnectionData connectionData)
        {
            HandleExportGlobalOptionSets(connectionData, null);
        }

        public void HandleExportGlobalOptionSets(ConnectionData connectionData, SelectedItem selectedItem)
        {
            string selection = string.Empty;

            if (selectedItem == null)
            {
                selection = GetSelectedText();
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCreatingFileWithGlobalOptionSets(conn, commonConfig, selection, selectedItem));
        }

        public void HandleExportOrganizationInformation()
        {
            HandleExportOrganizationInformation(null);
        }

        public void HandleExportOrganizationInformation(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerOrganizationInformation(conn, commonConfig));
        }

        public void HandleOpenImportJobExplorerWindow(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenImportJobExplorerWindow(conn, commonConfig));
        }

        public void HandleOpenSolutionImageWindow()
        {
            HandleOpenSolutionImageWindow(null);
        }

        public void HandleOpenSolutionImageWindow(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenSolutionImageWindow(conn, commonConfig));
        }

        public void HandleOpenSolutionDifferenceImageWindow()
        {
            HandleOpenSolutionDifferenceImageWindow(null);
        }

        public void HandleOpenSolutionDifferenceImageWindow(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenSolutionDifferenceImageWindow(conn, commonConfig));
        }

        public void HandleOpenOrganizationDifferenceImageWindow()
        {
            HandleOpenOrganizationDifferenceImageWindow(null);
        }

        public void HandleOpenOrganizationDifferenceImageWindow(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenOrganizationDifferenceImageWindow(conn, commonConfig));
        }

        public void HandleExportCustomControl()
        {
            HandleExportCustomControl(null);
        }

        public void HandleExportCustomControl(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerCustomControl(conn, commonConfig, selection));
        }

        public void HandleAddingIntoPublishListFilesByTypeCommand(List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartAddingIntoPublishListFilesByType(conn, commonConfig, selectedFiles, openFilesType));
        }

        public void HandleCheckOpenFilesWithoutUTF8EncodingCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartOpenFilesWithouUTF8Encoding(selectedFiles);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleCompareFilesWithoutUTF8EncodingCommand(List<SelectedFile> selectedFiles, bool withDetails)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartComparingFilesWithWrongEncoding(conn, selectedFiles, withDetails));
        }

        public void HandleOpenCrmInWeb(ConnectionData connectionData, OpenCrmWebSiteType crmWebSiteType)
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

        public void HandlePublishAll(ConnectionData connectionData)
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

        public void HandleXsdSchemaExport(string[] fileNamesColl)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
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

        public void HandleXsdSchemaOpenFolder()
        {
            var folder = FileOperations.GetSchemaXsdFolder();

            this.OpenFolder(folder);
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