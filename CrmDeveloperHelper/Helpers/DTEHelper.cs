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
                    var applicationObject = CrmDeveloperHelperPackage.ServiceProvider?.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;

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

            if ((System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) == System.Windows.Input.ModifierKeys.Shift)
            {
                connectionData.ConnectionConfiguration.SetCurrentConnection(connectionData.ConnectionId);
                connectionData.ConnectionConfiguration.Save();
                this.WriteToOutput(null, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);
                this.ActivateOutputWindow(null);
            }
        }

        public bool HasCRMConnection(out ConnectionConfiguration connectionConfig)
        {
            bool result = false;

            connectionConfig = Model.ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData == null)
            {
                var crmConnection = new WindowCrmConnectionList(this, connectionConfig);

                crmConnection.ShowDialog();

                connectionConfig.Save();
            }

            result = connectionConfig.CurrentConnectionData != null;

            return result;
        }

        public void HandleFileCompareCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartComparing(selectedFiles, connectionData, withDetails);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleUpdateContentWebResourcesAndPublishCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                bool canPublish = false;

                if (commonConfig.DoNotPropmPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.PublishWebResourcesFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPropmPublishMessage = dialog.DoNotPromtPublishMessage;

                        commonConfig.Save();

                        canPublish = true;
                    }
                }

                if (canPublish)
                {
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.StartUpdateContentAndPublish(selectedFiles, connectionData);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                bool canPublish = false;

                if (commonConfig.DoNotPropmPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.PublishWebResourcesEqualByTextFormat2, selectedFiles.Count, connectionData.GetDescription());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPropmPublishMessage = dialog.DoNotPromtPublishMessage;

                        commonConfig.Save();

                        canPublish = true;
                    }
                }

                if (canPublish)
                {
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.StartUpdateContentAndPublishEqualByText(selectedFiles, connectionData);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleAddingWebResourcesIntoSolutionCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartAddingWebResourcesIntoSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddingPluginAssemblyIntoSolutionByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, params string[] projectNames)
        {
            if (projectNames == null || !projectNames.Any())
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartAddingPluginAssemblyIntoSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddingPluginAssemblyProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, params string[] projectNames)
        {
            if (projectNames == null || !projectNames.Any())
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartAddingPluginAssemblyProcessingStepsIntoSolution(connectionData, commonConfig, solutionUniqueName, projectNames, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddingPluginTypeProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, params string[] pluginTypeNames)
        {
            if (pluginTypeNames == null || !pluginTypeNames.Any())
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartAddingPluginTypeProcessingStepsIntoSolution(connectionData, commonConfig, solutionUniqueName, pluginTypeNames, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddingReportsIntoSolutionCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartAddingReportsIntoSolution(connectionData, commonConfig, solutionUniqueName, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleComparingPluginAssemblyAndLocalAssemblyCommand(ConnectionData connectionData, EnvDTE.Project project)
        {
            if (project == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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

                var defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);

                try
                {
                    Controller.StartComparingPluginAssemblyAndLocalAssembly(connectionData, commonConfig, project.Name, defaultOutputFilePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleUpdatingPluginAssemblyCommand(ConnectionData connectionData, EnvDTE.Project project)
        {
            if (project == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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

                var defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);

                try
                {
                    Controller.StartUpdatingPluginAssembly(connectionData, commonConfig, project, defaultOutputFilePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleRegisterPluginAssemblyCommand(ConnectionData connectionData, EnvDTE.Project project)
        {
            if (project == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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

                var defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);

                try
                {
                    Controller.StartRegisterPluginAssembly(connectionData, commonConfig, project, defaultOutputFilePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExecutingFetchXml(ConnectionData connectionData, SelectedFile selectedFile, bool strictConnection)
        {
            if (selectedFile == null || !File.Exists(selectedFile.FilePath))
            {
                return;
            }

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                CrmDeveloperHelperPackage.Singleton?.ExecuteFetchXmlQueryAsync(selectedFile.FilePath, connectionData, this, strictConnection);
            }
        }

        public void HandleUpdateGlobalOptionSetsFile(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withSelect)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartUpdatingFileWithGlobalOptionSets(connectionData, commonConfig, selectedFiles, withSelect);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleUpdateEntityMetadataFileCSharpSchema(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartUpdatingFileWithEntityMetadataCSharpSchema(selectedFiles, connectionData, commonConfig, selectEntity);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleUpdateEntityMetadataFileCSharpProxyClass(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartUpdatingFileWithEntityMetadataCSharpProxyClass(selectedFiles, connectionData, commonConfig, selectEntity);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleUpdateEntityMetadataFileJavaScript(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool selectEntity)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartUpdatingFileWithEntityMetadataJavaScript(selectedFiles, connectionData, commonConfig, selectEntity);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleUpdateProxyClasses()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsCSharpType, false);

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && selectedFiles.Count == 1)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                string filePath = selectedFiles[0].FilePath;

                var form = new WindowCreateProxyClasses(commonConfig, crmConfig, filePath);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.UpdateProxyClasses(filePath, connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleReportDifferenceCommand(ConnectionData connectionData, string fieldName, string fieldTitle, bool isCustom)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartReportDifference(selectedFiles[0], fieldName, fieldTitle, isCustom, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleReportThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, string fieldName, string fieldTitle, ShowDifferenceThreeFileType differenceType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartReportThreeFileDifference(selectedFiles[0], fieldName, fieldTitle, connectionData1, connectionData2, differenceType, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleReportUpdateCommand()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartReportUpdate(selectedFiles[0], connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleReportDownloadCommand()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartDownloadCustomReport(connectionData, commonConfig, selectedFile.FileName);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenReportCommand(ConnectionData connectionData, ActionOpenComponent action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false);

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                SelectedFile selectedFile = selectedFiles[0];

                var objectId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (objectId.HasValue)
                {
                    switch (action)
                    {
                        case ActionOpenComponent.OpenInWeb:
                            connectionData.OpenEntityInstanceInWeb(Entities.Report.EntityLogicalName, objectId.Value);
                            return;

                        case ActionOpenComponent.OpenDependentComponentsInWeb:
                            connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.Report, objectId.Value);
                            return;
                    }
                }

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpeningReport(commonConfig, connectionData, selectedFile, action);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenLastSelectedSolution(ConnectionData connectionData, string solutionUniqueName, ActionOpenComponent action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && !string.IsNullOrEmpty(solutionUniqueName) && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartOpeningSolution(commonConfig, connectionData, solutionUniqueName, action);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCreateLaskLinkReportCommand(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCreatingLastLinkReport(selectedFile, connectionData);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleFileClearLink(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartClearingLastLink(selectedFiles, connectionData);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleWebResourceDifferenceCommand(ConnectionData connectionData, bool isCustom)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false);

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartWebResourceDifference(selectedFiles[0], isCustom, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleRibbonDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartRibbonDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSiteMapDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartSiteMapDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSiteMapUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartSiteMapUpdate(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSystemFormDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartSystemFormDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSystemFormUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartSystemFormUpdate(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSavedQueryDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartSavedQueryDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSavedQueryUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartSavedQueryUpdate(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleRibbonDiffXmlDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartRibbonDiffXmlDifference(selectedFile, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleRibbonDiffXmlUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null && commonConfig != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                if (connectionData.IsReadOnly)
                {
                    this.WriteToOutput(null, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                    return;
                }

                string message = string.Format(Properties.MessageBoxStrings.PublishRibbonDiffXmlFormat2, selectedFile.FileName, connectionData.GetDescription());

                var dialog = new WindowConfirmPublish(message, false);

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.StartRibbonDiffXmlUpdate(selectedFile, connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleWebResourceThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false);

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartWebResourceThreeFileDifference(selectedFiles[0], connectionData1, connectionData2, differenceType, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
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

        public void HandleWebResourceDownloadCommand()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceType, false);

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartDownloadCustomWebResource(connectionData, commonConfig, selectedFile.FileName);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenWebResource(ConnectionData connectionData, ActionOpenComponent action)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceType, false);

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                var objectId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (objectId.HasValue)
                {
                    switch (action)
                    {
                        case ActionOpenComponent.OpenInWeb:
                            connectionData.OpenEntityInstanceInWeb(Entities.WebResource.EntityLogicalName, objectId.Value);
                            return;

                        case ActionOpenComponent.OpenDependentComponentsInWeb:
                            connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.WebResource, objectId.Value);
                            return;
                    }
                }

                try
                {
                    Controller.StartOpeningWebResource(commonConfig, connectionData, selectedFile, action);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleShowingWebResourcesDependentComponents(List<SelectedFile> selectedFiles)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                var form = new WindowSelectFolderForExport(connectionData, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.FolderForExport = form.SelectedFolder;
                    commonConfig.DefaultFileAction = form.GetFileAction();

                    connectionData = form.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        CheckWishToChangeCurrentConnection(connectionData);

                        try
                        {
                            Controller.ShowingWebResourcesDependentComponents(selectedFiles, connectionData, commonConfig);
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleCheckingWorkflowsUsedEntities()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                var form = new WindowSelectFolderForExport(connectionData, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.FolderForExport = form.SelectedFolder;
                    commonConfig.DefaultFileAction = form.GetFileAction();

                    connectionData = form.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        CheckWishToChangeCurrentConnection(connectionData);

                        try
                        {
                            Controller.ExecuteCheckingWorkflowsUsedEntities(connectionData, commonConfig);
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleCheckingWorkflowsNotExistingUsedEntities()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                var form = new WindowSelectFolderForExport(connectionData, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.FolderForExport = form.SelectedFolder;
                    commonConfig.DefaultFileAction = form.GetFileAction();

                    connectionData = form.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        CheckWishToChangeCurrentConnection(connectionData);

                        try
                        {
                            Controller.ExecuteCheckingWorkflowsNotExistingUsedEntities(connectionData, commonConfig);
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleOpenFilesCommand(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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

                Controller.StartOpeningFiles(selectedFiles, openFilesType, inTextEditor, connectionData, commonConfig);
            }
        }

        public void HandleFileCompareListForPublishCommand(ConnectionData connectionData, bool withDetails)
        {
            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            CheckWishToChangeCurrentConnection(connectionData);

            List<SelectedFile> selectedFiles = this.GetSelectedFilesFromListForPublish();

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

        public void HandleMultiDifferenceFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartMultiDifferenceFiles(selectedFiles, openFilesType, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
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
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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

                QuickConnection.TestConnectAsync(connectionData, this);
            }
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

        public string GetCurrentConnectionName()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            return connectionConfig?.CurrentConnectionData?.Name;
        }

        public string GetLastSolutionUniqueName()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            return connectionConfig?.CurrentConnectionData?.LastSelectedSolutionsUniqueName?.FirstOrDefault();
        }

        public void HandleCheckEntitiesNames()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select Entity Name Prefix", "Entity Name Prefix");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        CheckWishToChangeCurrentConnection(connectionData);

                        try
                        {
                            Controller.StartCheckEntitiesNames(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleCheckEntitiesNamesAndShowDependentComponents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select Entity Name Prefix", "Entity Name Prefix");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        CheckWishToChangeCurrentConnection(connectionData);

                        try
                        {
                            Controller.StartCheckEntitiesNamesAndShowDependentComponents(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleCheckMarkedAndShowDependentComponents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select mark to delete", "Mark to delete");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        CheckWishToChangeCurrentConnection(connectionData);

                        try
                        {
                            Controller.StartCheckMarkedToDeleteAndShowDependentComponents(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleCheckEntitiesOwnership(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCheckEntitiesOwnership(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckGlobalOptionSetDuplicates(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCheckGlobalOptionSetDuplicates(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckComponentTypeEnum(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCheckComponentTypeEnum(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCreateAllDependencyNodesDescription(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCreateAllDependencyNodesDescription(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckManagedEntities(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCheckManagedEntities(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckPluginSteps(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCheckPluginSteps(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckPluginImages(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCheckPluginImages(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckPluginStepsRequiredComponents(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCheckPluginStepsRequiredComponents(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCheckPluginImagesRequiredComponents(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCheckPluginImagesRequiredComponents(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleFindByName()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select Element Name", "Element Name");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        CheckWishToChangeCurrentConnection(connectionData);

                        try
                        {
                            Controller.StartFindEntityElementsByName(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindContainsString()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, "Select String for contain", "String for contain");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        ActivateOutputWindow(connectionData);
                        WriteToOutputEmptyLines(connectionData, commonConfig);

                        CheckWishToChangeCurrentConnection(connectionData);

                        try
                        {
                            Controller.StartFindEntityElementsContainsString(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindEntityById()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, connectionData, string.Format("Find Entity in {0} by Id", connectionData.Name));

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    connectionData = dialog.GetConnectionData();

                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.StartFindEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleEditEntityById()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, connectionData, string.Format("Find Entity in {0} by Id", connectionData.Name));

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    connectionData = dialog.GetConnectionData();

                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.StartEditEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleFindEntityByUniqueidentifier()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, connectionData, string.Format("Find Entity in {0} by Uniqueidentifier", connectionData.Name));

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    connectionData = dialog.GetConnectionData();

                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.StartFindEntityByUniqueidentifier(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleOpenPluginTree(string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingPluginTree(connectionData, commonConfig, entityFilter, pluginTypeFilter, messageFilter);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSdkMessageTree()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingSdkMessageTree(connectionData, commonConfig, selection, null);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleSdkMessageRequestTree()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingSdkMessageRequestTree(connectionData, commonConfig, selection, null);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSystemUsersExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingSystemUserExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenTeamsExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingTeamsExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSecurityRolesExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            string selection = GetSelectedText();

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingSecurityRolesExplorer(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandlePluginConfigurationCreate()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                var form = new WindowPluginConfiguration(commonConfig, true);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartExportPluginConfiguration(connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandlePluginConfigurationPluginAssemblyDescription()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false);

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            if (commonConfig != null && Directory.Exists(commonConfig.FolderForExport))
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationAssemblyDescription(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandlePluginConfigurationPluginTypeDescription()
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false);

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null && Directory.Exists(commonConfig.FolderForExport))
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationTypeDescription(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandlePluginConfigurationTree()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false);

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && Directory.Exists(commonConfig.FolderForExport))
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationTree(connectionData, commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandlePluginConfigurationComparerPluginAssembly()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            ConnectionConfiguration crmConfig = Model.ConnectionConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false);

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            if (commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartShowingPluginConfigurationComparer(commonConfig, filePath);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

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
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    this.Controller.StartTraceReaderWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    this.WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportFileWithEntityMetadata()
        {
            HandleExportFileWithEntityMetadata(null, null);
        }

        public void HandleExportFileWithEntityMetadata(ConnectionData connectionData, SelectedItem selectedItem)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            string selection = GetSelectedText();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartCreatingFileWithEntityMetadata(selection, selectedItem, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntityAttributeExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntityAttributeExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntityKeyExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntityKeyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntityRelationshipOneToManyExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntityRelationshipOneToManyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntityRelationshipManyToManyExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntityRelationshipManyToManyExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenEntitySecurityRolesExplorer()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenEntitySecurityRolesExplorer(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportFormEvents()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

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
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCreatingFileWithGlobalOptionSets(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportOrganizationInformation()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportOrganizationInformation(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportPluginAssembly()
        {
            string selection = GetSelectedText();

            HandleExportPluginAssembly(selection);
        }

        public void HandleExportPluginAssembly(string selection)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportPluginAssembly(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportPluginTypeDescription(string selection)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportPluginTypeDescription(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddPluginStep(string pluginTypeName, ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                    Controller.StartAddPluginStep(pluginTypeName, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportReport()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartDownloadCustomReport(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportRibbon()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportRibbonXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportSitemap()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportSitemapXml(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSolutionExplorerWindow(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartOpenSolutionExplorerWindow(null, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenImportJobExplorerWindow(ConnectionData connectionData)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
                {
                    return;
                }

                connectionData = crmConfig.CurrentConnectionData;
            }

            if (connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                try
                {
                    Controller.StartOpenImportJobExplorerWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSolutionImageWindow()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenSolutionImageWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenSolutionDifferenceImageWindow()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenSolutionDifferenceImageWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenOrganizationDifferenceImageWindow()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenOrganizationDifferenceImageWindow(connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportSystemForm()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportSystemFormXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportSystemSavedQuery()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportSystemSavedQueryXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportSystemSavedQueryVisualization()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportSystemSavedQueryVisualizationXml(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportWebResource()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartDownloadCustomWebResource(connectionData, commonConfig, selection);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportWorkflows()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();
            string selection = GetSelectedText();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartExportWorkflow(selection, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleAddingIntoPublishListFilesByTypeCommand(List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartAddingIntoPublishListFilesByType(selectedFiles, openFilesType, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
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
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null && selectedFiles.Count > 0)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartComparingFilesWithWrongEncoding(selectedFiles, connectionData, withDetails);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleCreateLaskLinkWebResourcesMultipleCommand(List<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartCreatingLastLinkWebResourceMultiple(selectedFiles, connectionData);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportSolution()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            SelectedItem selectedItem = GetSelectedProjectItem();

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && selectedItem != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpenSolutionExplorerWindow(selectedItem, connectionData, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleExportPluginConfigurationInfoFolder()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var selectedItem = GetSelectedProjectItem();

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && selectedItem != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                var form = new WindowPluginConfiguration(commonConfig, true);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    try
                    {
                        Controller.StartExportPluginConfigurationIntoFolder(selectedItem, connectionData, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleOpenCrmInWeb(ConnectionData connectionData, OpenCrmWebSiteType crmWebSiteType)
        {
            if (connectionData == null)
            {
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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
                if (!HasCRMConnection(out ConnectionConfiguration crmConfig))
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

        public void HandleExportDefaultSitemap(string selectedSitemap)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
            {
                string fileName = string.Format("SiteMap.{0}.xml", selectedSitemap);

                var dialog = new Microsoft.Win32.SaveFileDialog()
                {
                    DefaultExt = ".xml",

                    Filter = "SiteMap (.xml)|*.xml",
                    FilterIndex = 1,

                    RestoreDirectory = true,
                    FileName = fileName,

                    InitialDirectory = commonConfig.FolderForExport,
                };

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    ActivateOutputWindow(null);
                    WriteToOutputEmptyLines(null, commonConfig);

                    try
                    {
                        Uri uri = FileOperations.GetSiteMapResourceUri(selectedSitemap);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        var doc = XDocument.Load(info.Stream);
                        info.Stream.Dispose();

                        var filePath = dialog.FileName;

                        doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutput(null, "{0} exported.", fileName);

                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutputFilePathUri(null, filePath);

                        PerformAction(null, filePath, true);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(null, ex);
                    }
                }
            }
        }

        public void HandleShowDifferenceWithDefaultSitemap(SelectedFile selectedFile, string selectedSitemap)
        {
            if (selectedFile == null || !File.Exists(selectedFile.FilePath))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig == null)
            {
                return;
            }

            ActivateOutputWindow(null);
            WriteToOutputEmptyLines(null, commonConfig);

            try
            {
                Uri uri = FileOperations.GetSiteMapResourceUri(selectedSitemap);
                StreamResourceInfo info = Application.GetResourceStream(uri);

                var doc = XDocument.Load(info.Stream);
                info.Stream.Dispose();

                string fileName = string.Format("SiteMap.{0}.xml", selectedSitemap);

                var filePath = Path.Combine(FileOperations.GetTempFileFolder(), fileName);

                doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);

                this.WriteToOutput(null, "{0} exported.", fileName);

                this.WriteToOutput(null, string.Empty);

                this.WriteToOutputFilePathUri(null, filePath);

                this.ProcessStartProgramComparer(selectedFile.FilePath, filePath, selectedFile.FileName, fileName);
            }
            catch (Exception ex)
            {
                WriteErrorToOutput(null, ex);
            }
        }

        public void HandleExportXsdSchema(string[] fileNamesColl)
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

        public void HandleOpenXsdSchemaFolder()
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