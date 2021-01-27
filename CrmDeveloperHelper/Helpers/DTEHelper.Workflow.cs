using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleWorkflowDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWorkflowDifference(conn, commonConfig, selectedFile));
        }

        public void HandleWorkflowDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWorkflowDifference(conn, commonConfig, doc, filePath));
        }

        public void HandleWorkflowUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWorkflowUpdate(conn, commonConfig, selectedFile));
        }

        public void HandleWorkflowUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWorkflowUpdate(conn, commonConfig, doc, filePath));
        }

        public void HandleWorkflowOpenInWebCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWorkflowOpenInWeb(conn, commonConfig, selectedFile));
        }

        public void HandleWorkflowGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartWorkflowGetCurrent(conn, commonConfig, selectedFile));
        }

        public void HandleCheckingWorkflowsUsedEntities(bool openExplorer)
        {
            HandleCheckingWorkflowsUsedEntities(null, openExplorer);
        }

        public void HandleCheckingWorkflowsUsedEntities(ConnectionData connectionData, bool openExplorer)
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
                                    Controller.StartCheckingWorkflowsUsedEntities(connectionData, commonConfig, openExplorer);
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

        public void HandleCheckingWorkflowsNotExistingUsedEntities(bool openExplorer)
        {
            HandleCheckingWorkflowsNotExistingUsedEntities(null, openExplorer);
        }

        public void HandleCheckingWorkflowsNotExistingUsedEntities(ConnectionData connectionData, bool openExplorer)
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
                                    Controller.ExecuteCheckingWorkflowsNotExistingUsedEntities(connectionData, commonConfig, openExplorer);
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

        public void HandleCheckingWorkflowsWithEntityFieldStrings(bool openExplorer)
        {
            HandleCheckingWorkflowsWithEntityFieldStrings(null, openExplorer);
        }

        public void HandleCheckingWorkflowsWithEntityFieldStrings(ConnectionData connectionData, bool openExplorer)
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
                                    Controller.ExecuteCheckingWorkflowsWithEntityFieldStrings(connectionData, commonConfig, openExplorer);
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

        public void HandleExplorerWorkflows()
        {
            string selection = GetSelectedText();

            HandleExplorerWorkflows(null, selection);
        }

        public void HandleExplorerWorkflows(string selection)
        {
            HandleExplorerWorkflows(null, selection);
        }

        public void HandleExplorerWorkflows(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            HandleExplorerWorkflows(connectionData, selection);
        }

        public void HandleExplorerWorkflows(ConnectionData connectionData, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerWorkflow(conn, commonConfig, selection));
        }

        public void HandleOpenWorkflowOrganizationComparerCommand(ConnectionData connectionData1, ConnectionData connectionData2, string filter)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.OpenWorkflowOrganizationComparer(connectionData1, connectionData2, commonConfig, filter);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }
    }
}
