using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        private void GetConnectionConfigTextAndExecute(ConnectionData connectionData, string windowTitle, string labelTitle, Action<ConnectionData, CommonConfiguration, string> action)
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

                var thread = new System.Threading.Thread(() =>
                {
                    try
                    {
                        var dialog = new WindowSelectFolderAndText(commonConfig, connectionData, windowTitle, labelTitle);

                        if (dialog.ShowDialog().GetValueOrDefault())
                        {
                            connectionData = dialog.GetConnectionData();

                            if (connectionData != null)
                            {
                                ActivateOutputWindow(connectionData);
                                WriteToOutputEmptyLines(connectionData, commonConfig);

                                CheckWishToChangeCurrentConnection(connectionData);

                                string text = dialog.GetText();

                                try
                                {
                                    action(connectionData, commonConfig, text);
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

                thread.SetApartmentState(System.Threading.ApartmentState.STA);

                thread.Start();
            }
        }

        public void HandleFindEntityObjectsByPrefix()
        {
            HandleFindEntityObjectsByPrefix(null);
        }

        public void HandleFindEntityObjectsByPrefix(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select Entity Name Prefix", "Entity Name Prefix"
                , (conn, commonConfig, text) => Controller.StartFindEntityObjectsByPrefix(conn, commonConfig, text)
            );
        }

        public void HandleFindEntityObjectsByPrefixInExplorer()
        {
            HandleFindEntityObjectsByPrefixInExplorer(null);
        }

        public void HandleFindEntityObjectsByPrefixInExplorer(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select Entity Name Prefix", "Entity Name Prefix"
                , (conn, commonConfig, text) => Controller.StartFindEntityObjectsByPrefixInExplorer(conn, commonConfig, text)
            );
        }

        public void HandleFindEntityObjectsByPrefixAndShowDependentComponents()
        {
            HandleFindEntityObjectsByPrefixAndShowDependentComponents(null);
        }

        public void HandleFindEntityObjectsByPrefixAndShowDependentComponents(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select Entity Name Prefix", "Entity Name Prefix"
                , (conn, commonConfig, text) => Controller.StartFindEntityObjectsByPrefixAndShowDependentComponents(conn, commonConfig, text)
            );
        }

        public void HandleFindMarkedToDeleteInExplorer()
        {
            HandleFindMarkedToDeleteInExplorer(null);
        }

        public void HandleFindMarkedToDeleteInExplorer(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select mark to delete", "Mark to delete"
                , (conn, commonConfig, text) => Controller.StartFindMarkedToDeleteInExplorer(conn, commonConfig, text)
            );
        }

        public void HandleFindMarkedToDeleteAndShowDependentComponents()
        {
            HandleFindMarkedToDeleteAndShowDependentComponents(null);
        }

        public void HandleFindMarkedToDeleteAndShowDependentComponents(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select mark to delete", "Mark to delete"
                , (conn, commonConfig, text) => Controller.StartFindMarkedToDeleteInExplorer(conn, commonConfig, text)
            );
        }

        public void HandleFindEntityObjectsByName()
        {
            HandleFindEntityObjectsByName(null);
        }

        public void HandleFindEntityObjectsByName(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select Element Name", "Element Name"
                , (conn, commonConfig, text) => Controller.StartFindEntityObjectsByName(conn, commonConfig, text)
            );
        }

        public void HandleFindEntityObjectsByNameInExplorer()
        {
            HandleFindEntityObjectsByNameInExplorer(null);
        }

        public void HandleFindEntityObjectsByNameInExplorer(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select Element Name", "Element Name"
                , (conn, commonConfig, text) => Controller.StartFindEntityObjectsByNameInExplorer(conn, commonConfig, text)
            );
        }

        public void HandleFindEntityObjectsContainsString()
        {
            HandleFindEntityObjectsContainsString(null);
        }

        public void HandleFindEntityObjectsContainsString(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select String to contain", "String to contain"
                , (conn, commonConfig, text) => Controller.StartFindEntityObjectsContainsString(conn, commonConfig, text)
            );
        }

        public void HandleFindEntityObjectsContainsStringInExplorer()
        {
            HandleFindEntityObjectsContainsStringInExplorer(null);
        }

        public void HandleFindEntityObjectsContainsStringInExplorer(ConnectionData connectionData)
        {
            GetConnectionConfigTextAndExecute(connectionData
                , "Select String to contain", "String to contain"
                , (conn, commonConfig, text) => Controller.StartFindEntityObjectsContainsStringInExplorer(conn, commonConfig, text)
            );
        }

        public void HandleFindEntityById()
        {
            HandleFindEntityById(null);
        }

        private void GetConnectionConfigEntityPropertiesAndExecute(ConnectionData connectionData, string windowTitleFormat1, Action<ConnectionData, CommonConfiguration, string, int?, Guid> action)
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

                var thread = new System.Threading.Thread(() =>
                {
                    try
                    {
                        string windowTitle = string.Format(windowTitleFormat1, connectionData.Name);

                        var dialog = new WindowSelectEntityIdToFind(commonConfig, connectionData, windowTitle);

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
                                action(connectionData, commonConfig, entityName, entityTypeCode, entityId);
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

                thread.SetApartmentState(System.Threading.ApartmentState.STA);

                thread.Start();
            }
        }

        public void HandleFindEntityById(ConnectionData connectionData)
        {
            GetConnectionConfigEntityPropertiesAndExecute(connectionData
                , "Find Entity in {0} by Id"
                , (conn, commonConfig, entityName, entityTypeCode, entityId) => Controller.StartFindEntityById(conn, commonConfig, entityName, entityTypeCode, entityId)
            );
        }

        public void HandleFindEntityByUniqueidentifier()
        {
            HandleFindEntityByUniqueidentifier(null);
        }

        public void HandleFindEntityByUniqueidentifier(ConnectionData connectionData)
        {
            GetConnectionConfigEntityPropertiesAndExecute(connectionData
                , "Find Entity in {0} by Uniqueidentifier"
                , (conn, commonConfig, entityName, entityTypeCode, entityId) => Controller.StartFindEntityByUniqueidentifier(conn, commonConfig, entityName, entityTypeCode, entityId)
            );
        }

        public void HandleEditEntityById()
        {
            HandleEditEntityById(null);
        }

        public void HandleEditEntityById(ConnectionData connectionData)
        {
            GetConnectionConfigEntityPropertiesAndExecute(connectionData
                , "Edit Entity in {0} by Id"
                , (conn, commonConfig, entityName, entityTypeCode, entityId) => Controller.StartEditEntityById(conn, commonConfig, entityName, entityTypeCode, entityId)
            );
        }
    }
}
