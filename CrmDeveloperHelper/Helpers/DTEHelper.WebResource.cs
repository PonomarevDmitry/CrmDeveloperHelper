using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        #region WebResource

        public void HandleUpdateContentWebResourcesAndPublishCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData == null)
            {
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
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
                        Controller.StartUpdateContentAndPublish(connectionData, selectedFiles);
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
                if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
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
                        Controller.StartUpdateContentAndPublishEqualByText(connectionData, selectedFiles);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleIncludeReferencesToDependencyXmlCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

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

                bool canPublish = false;

                if (commonConfig.DoNotPropmPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.IncludeReferencesToDependencyXmlAndPublishWebResourcesFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

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
                        Controller.StartIncludeReferencesToDependencyXml(connectionData, commonConfig, selectedFiles);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleWebResourceAddingToSolutionCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartAddingWebResourcesToSolution(conn, commonConfig, solutionUniqueName, selectedFiles, withSelect));
        }

        public void HandleWebResourceDifferenceCommand(ConnectionData connectionData, bool isCustom)
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceDifference(conn, commonConfig, selectedFiles[0], isCustom));
        }

        public void HandleWebResourceCreateEntityDescriptionCommand(ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceCreateEntityDescription(conn, commonConfig, selectedFiles[0]));
        }

        public void HandleWebResourceChangeInEntityEditorCommand(ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceChangeInEntityEditor(conn, commonConfig, selectedFiles[0]));
        }

        public void HandleWebResourceGetAttributeCommand(ConnectionData connectionData, string fieldName, string fieldTitle)
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceGetAttribute(conn, commonConfig, selectedFiles[0], fieldName, fieldTitle));
        }

        public void HandleWebResourceThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, ShowDifferenceThreeFileType differenceType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceTextType, false).Take(2).ToList();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartWebResourceThreeFileDifference(connectionData1, connectionData2, commonConfig, selectedFiles[0], differenceType);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleOpenWebResource(ConnectionData connectionData, ActionOnComponent actionOnComponent)
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

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceType, false).Take(2).ToList();

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                var objectId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (objectId.HasValue)
                {
                    switch (actionOnComponent)
                    {
                        case ActionOnComponent.OpenInWeb:
                            connectionData.OpenEntityInstanceInWeb(Entities.WebResource.EntityLogicalName, objectId.Value);
                            return;

                        case ActionOnComponent.OpenDependentComponentsInWeb:
                            connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.WebResource, objectId.Value);
                            return;
                    }
                }

                try
                {
                    Controller.StartOpeningWebResource(connectionData, commonConfig, selectedFile, actionOnComponent);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenLinkedSystemForm(ConnectionData connectionData, ActionOnComponent actionOnComponent, string entityName, Guid formId, int formType)
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

                switch (actionOnComponent)
                {
                    case ActionOnComponent.OpenInWeb:
                        connectionData.OpenSystemFormInWeb(entityName, formId, formType);
                        return;

                    case ActionOnComponent.OpenDependentComponentsInWeb:
                        connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.SystemForm, formId);
                        return;
                }

                try
                {
                    Controller.StartOpeningLinkedSystemForm(connectionData, commonConfig, actionOnComponent, entityName, formId, formType);
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

            if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
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
                            Controller.ShowingWebResourcesDependentComponents(connectionData, commonConfig, selectedFiles);
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleWebResourceMultiDifferenceFiles(List<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) =>
            {
                if (openFilesType == OpenFilesType.All)
                {
                    WriteToOutput(conn, Properties.OutputStrings.ShowingDifferenceIsNotAllowedForFormat1, openFilesType.ToString());
                    return;
                }

                Controller.StartWebResourceMultiDifferenceFiles(conn, commonConfig, selectedFiles, openFilesType);
            });
        }

        public void HandleExportWebResource()
        {
            HandleExportWebResource(null);
        }

        public void HandleExportWebResource(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            HandleOpenWebResourceExplorerCommand(connectionData, selection);
        }

        public void HandleOpenWebResourceExplorerCommand()
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsWebResourceType, false).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                SelectedFile selectedFile = selectedFiles[0];

                HandleOpenWebResourceExplorerCommand(null, selectedFile.FileName);
            }
        }

        public void HandleOpenWebResourceExplorerCommand(string selection)
        {
            HandleOpenWebResourceExplorerCommand(null, selection);
        }

        private void HandleOpenWebResourceExplorerCommand(ConnectionData connectionData, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenWebResourceExplorer(conn, commonConfig, selection));
        }

        public void HandleWebResourceCreateLaskLinkMultipleCommand(List<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartWebResourceCreatingLastLinkMultiple(conn, selectedFiles));
        }

        public void HandleWebResourceCompareCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceComparing(conn, selectedFiles, withDetails));
        }

        public void HandleWebResourceOpenFilesCommand(List<SelectedFile> selectedFiles, OpenFilesType openFilesType, bool inTextEditor)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (inTextEditor && !File.Exists(commonConfig.TextEditorProgram))
            {
                return;
            }

            if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && commonConfig != null)
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                Controller.StartOpeningFiles(connectionData, commonConfig, selectedFiles, openFilesType, inTextEditor);
            }
        }

        public void HandleWebResourceCheckFileEncodingCommand(List<SelectedFile> selectedFiles)
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

        public void HandleWebResourceCheckOpenFilesWithoutUTF8EncodingCommand(List<SelectedFile> selectedFiles)
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

        public void HandleWebResourceCompareFilesWithoutUTF8EncodingCommand(List<SelectedFile> selectedFiles, bool withDetails)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartComparingFilesWithWrongEncoding(conn, selectedFiles, withDetails));
        }

        #endregion WebResource

        #region WebResourceDependencyXml

        public void HandleWebResourceDependencyXmlDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceDependencyXmlDifference(conn, commonConfig, selectedFile));
        }

        public void HandleWebResourceDependencyXmlDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceDependencyXmlDifference(conn, commonConfig, doc, filePath));
        }

        public void HandleWebResourceDependencyXmlUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceDependencyXmlUpdate(conn, commonConfig, selectedFile));
        }

        public void HandleWebResourceDependencyXmlUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceDependencyXmlUpdate(conn, commonConfig, doc, filePath));
        }

        public void HandleWebResourceDependencyXmlOpenInWebCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceDependencyXmlOpenInWeb(conn, commonConfig, selectedFile));
        }

        public void HandleWebResourceDependencyXmlGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceDependencyXmlGetCurrent(conn, commonConfig, selectedFile));
        }

        #endregion WebResourceDependencyXml
    }
}
