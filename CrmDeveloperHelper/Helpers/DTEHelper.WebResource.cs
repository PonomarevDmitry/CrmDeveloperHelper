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

                if (commonConfig.DoNotPromtPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.PublishWebResourcesFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPromtPublishMessage = dialog.DoNotPromtPublishMessage;

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

                if (commonConfig.DoNotPromtPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.PublishWebResourcesEqualByTextFormat2, selectedFiles.Count, connectionData.GetDescription());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPromtPublishMessage = dialog.DoNotPromtPublishMessage;

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

                if (commonConfig.DoNotPromtPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.IncludeReferencesToDependencyXmlAndPublishWebResourcesFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPromtPublishMessage = dialog.DoNotPromtPublishMessage;

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

        public void HandleUpdateContentIncludeReferencesToDependencyXmlCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
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

                if (commonConfig.DoNotPromtPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.UpdateContentIncludeReferencesToDependencyXmlAndPublishWebResourcesFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPromtPublishMessage = dialog.DoNotPromtPublishMessage;

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
                        Controller.StartUpdateContentIncludeReferencesToDependencyXml(connectionData, commonConfig, selectedFiles);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleUpdateEqualByTextContentIncludeReferencesToDependencyXmlCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
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

                if (commonConfig.DoNotPromtPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.UpdateEqualByTextContentIncludeReferencesToDependencyXmlAndPublishWebResourcesFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPromtPublishMessage = dialog.DoNotPromtPublishMessage;

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
                        Controller.StartUpdateEqualByTextContentContentIncludeReferencesToDependencyXml(connectionData, commonConfig, selectedFiles);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleIncludeReferencesToLinkedSystemFormsLibrariesCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
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

                if (commonConfig.DoNotPromtPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.IncludeReferencesToLinkedSystemFormsLibrariesAndPublishFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPromtPublishMessage = dialog.DoNotPromtPublishMessage;

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
                        Controller.StartIncludeReferencesToLinkedSystemFormsLibraries(connectionData, commonConfig, selectedFiles);
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

        public void HandleWebResourceDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile, bool withSelect)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceDifference(conn, commonConfig, selectedFile, withSelect));
        }

        public void HandleWebResourceCreateEntityDescriptionCommand(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles)
        {
            if (selectedFiles == null || !selectedFiles.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceCreateEntityDescription(conn, commonConfig, selectedFiles));
        }

        public void HandleWebResourceChangeInEntityEditorCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceChangeInEntityEditor(conn, commonConfig, selectedFile));
        }

        public void HandleWebResourceGetAttributeCommand(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles, string fieldName, string fieldTitle)
        {
            if (selectedFiles == null || !selectedFiles.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceGetAttribute(conn, commonConfig, selectedFiles, fieldName, fieldTitle));
        }

        public void HandleWebResourceThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, SelectedFile selectedFile, ShowDifferenceThreeFileType differenceType)
        {
            if (selectedFile == null)
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartWebResourceThreeFileDifference(connectionData1, connectionData2, commonConfig, selectedFile, differenceType);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleOpenWebResourceInExplorer(ConnectionData connectionData, SelectedFile selectedFile, ActionOnComponent actionOnComponent)
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
                    Controller.StartOpeningWebResourceInExplorer(connectionData, commonConfig, selectedFile, actionOnComponent);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleOpenWebResourceInWeb(ConnectionData connectionData, ActionOnComponent actionOnComponent, IEnumerable<SelectedFile> selectedFiles)
        {
            if (selectedFiles == null || !selectedFiles.Any())
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
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

                var list = new List<SelectedFile>();

                foreach (var selectedFile in selectedFiles)
                {
                    var objectId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                    if (objectId.HasValue)
                    {
                        switch (actionOnComponent)
                        {
                            case ActionOnComponent.OpenInWeb:
                                connectionData.OpenEntityInstanceInWeb(Entities.WebResource.EntityLogicalName, objectId.Value);
                                break;

                            case ActionOnComponent.OpenDependentComponentsInWeb:
                                connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.WebResource, objectId.Value);
                                break;
                        }
                    }
                    else
                    {
                        list.Add(selectedFile);
                    }
                }

                if (list.Any())
                {
                    try
                    {
                        Controller.StartOpeningWebResourceInWeb(connectionData, commonConfig, actionOnComponent, list);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
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

                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
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
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandleWebResourceMultiDifferenceFiles(IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            if (!selectedFiles.Any())
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

        public void HandleOpenWebResourceExplorerCommand(string selection)
        {
            HandleOpenWebResourceExplorerCommand(null, selection);
        }

        private void HandleOpenWebResourceExplorerCommand(ConnectionData connectionData, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenWebResourceExplorer(conn, commonConfig, selection));
        }

        public void HandleOpenWebResourceOrganizationComparerCommand(ConnectionData connectionData1, ConnectionData connectionData2, string filter)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.OpenWebResourceOrganizationComparer(connectionData1, connectionData2, commonConfig, filter);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
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

                Controller.StartWebResourceOpeningFiles(connectionData, commonConfig, selectedFiles, openFilesType, inTextEditor);
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
                    Controller.StartWebResourceCheckFileEncoding(selectedFiles);
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
                    Controller.StartWebResourceOpenFilesWithouUTF8Encoding(selectedFiles);
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

        public void HandleWebResourcesGetContentCommand(ConnectionData connectionData, List<SelectedFile> selectedFiles)
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

                if (commonConfig.DoNotPromtPublishMessage)
                {
                    canPublish = true;
                }
                else
                {
                    string message = string.Format(Properties.MessageBoxStrings.GetWebResourcesContentFormat2, selectedFiles.Count, connectionData.GetDescriptionColumn());

                    var dialog = new WindowConfirmPublish(message);

                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        commonConfig.DoNotPromtPublishMessage = dialog.DoNotPromtPublishMessage;

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
                        Controller.StartWebResourcesGetContent(connectionData, commonConfig, selectedFiles);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleWebResourceCopyToClipboardRibbonObjectsCommand(ConnectionData connectionData, SelectedFile selectedFile, RibbonPlacement ribbonPlacement)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWebResourceCopyToClipboardRibbonObjects(conn, commonConfig, selectedFile, ribbonPlacement));
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
