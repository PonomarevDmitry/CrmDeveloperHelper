using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandlePluginConfigurationCreate()
        {
            HandlePluginConfigurationCreate(null);
        }

        public void HandlePluginConfigurationCreate(ConnectionData connectionData)
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
                        var form = new WindowPluginConfiguration(commonConfig, connectionData, true);

                        if (form.ShowDialog().GetValueOrDefault())
                        {
                            connectionData = form.GetConnectionData();

                            ActivateOutputWindow(connectionData);
                            WriteToOutputEmptyLines(connectionData, commonConfig);

                            CheckWishToChangeCurrentConnection(connectionData);

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
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(connectionData, ex);
                    }
                });

                worker.SetApartmentState(System.Threading.ApartmentState.STA);

                worker.Start();
            }
        }

        public void HandlePluginConfigurationPluginAssemblyDescription()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false).Take(2).ToList();

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
            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false).Take(2).ToList();

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
            HandlePluginConfigurationTree(null);
        }

        public void HandlePluginConfigurationTree(ConnectionData connectionData)
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

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false).Take(2).ToList();

            string filePath = string.Empty;

            if (selectedFiles.Count == 1)
            {
                filePath = selectedFiles[0].FilePath;
            }

            if (connectionData != null && commonConfig != null && Directory.Exists(commonConfig.FolderForExport))
            {
                ActivateOutputWindow(connectionData);
                WriteToOutputEmptyLines(connectionData, commonConfig);

                CheckWishToChangeCurrentConnection(connectionData);

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

            List<SelectedFile> selectedFiles = GetSelectedFilesAll(FileOperations.SupportsXmlType, false).Take(2).ToList();

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

        public void HandleExportPluginConfigurationInfoFolder()
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (!HasCurrentCrmConnection(out ConnectionConfiguration crmConfig))
            {
                return;
            }

            var selectedItem = GetSelectedProjectItem();

            var connectionData = crmConfig.CurrentConnectionData;

            if (connectionData != null && selectedItem != null)
            {
                var worker = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var form = new WindowPluginConfiguration(commonConfig, connectionData, true);

                        if (form.ShowDialog().GetValueOrDefault())
                        {
                            connectionData = form.GetConnectionData();

                            ActivateOutputWindow(connectionData);
                            WriteToOutputEmptyLines(connectionData, commonConfig);

                            CheckWishToChangeCurrentConnection(connectionData);

                            try
                            {
                                Controller.StartExportPluginConfigurationIntoFolder(connectionData, commonConfig, selectedItem);
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
    }
}