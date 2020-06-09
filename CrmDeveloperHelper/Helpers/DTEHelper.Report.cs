using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleReportAddingToSolutionCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count == 0)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartReportAddingToSolution(conn, commonConfig, solutionUniqueName, selectedFiles, withSelect));
        }

        public void HandleReportDifferenceCommand(ConnectionData connectionData, string fieldName, string fieldTitle, bool isCustom)
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartReportDifference(conn, commonConfig, selectedFiles[0], fieldName, fieldTitle, isCustom));
        }

        public void HandleReportThreeFileDifferenceCommand(ConnectionData connectionData1, ConnectionData connectionData2, string fieldName, string fieldTitle, ShowDifferenceThreeFileType differenceType)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false).Take(2).ToList();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null && selectedFiles.Count == 1)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.StartReportThreeFileDifference(connectionData1, connectionData2, commonConfig, selectedFiles[0], fieldName, fieldTitle, differenceType);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleReportUpdateCommand(ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartReportUpdate(conn, commonConfig, selectedFiles[0]));
        }

        public void HandleOpenReportExplorerCommand()
        {
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartOpenReportExplorer(conn, commonConfig, selectedFiles[0].FileName));
        }

        public void HandleOpenReportCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent)
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

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsReportType, false).Take(2).ToList();

            if (connectionData != null && commonConfig != null && selectedFiles.Count == 1)
            {
                CheckWishToChangeCurrentConnection(connectionData);

                SelectedFile selectedFile = selectedFiles[0];

                var objectId = connectionData.GetLastLinkForFile(selectedFile.FriendlyFilePath);

                if (objectId.HasValue)
                {
                    switch (actionOnComponent)
                    {
                        case ActionOnComponent.OpenInWeb:
                            connectionData.OpenEntityInstanceInWeb(Entities.Report.EntityLogicalName, objectId.Value);
                            return;

                        case ActionOnComponent.OpenDependentComponentsInWeb:
                            connectionData.OpenSolutionComponentDependentComponentsInWeb(Entities.ComponentType.Report, objectId.Value);
                            return;
                    }
                }

                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                try
                {
                    Controller.StartOpeningReport(connectionData, commonConfig, selectedFile, actionOnComponent);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(connectionData, ex);
                }
            }
        }

        public void HandleReportCreateLaskLinkCommand(List<SelectedFile> selectedFiles)
        {
            if (selectedFiles.Count != 1)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartReportCreatingLastLink(conn, selectedFiles[0]));
        }

        public void HandleExportReport()
        {
            HandleExportReport(null);
        }

        public void HandleExportReport(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenReportExplorer(conn, commonConfig, selection));
        }
    }
}
