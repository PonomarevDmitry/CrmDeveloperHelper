using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleFindEntityObjectsByPrefix()
        {
            HandleFindEntityObjectsByPrefix(null);
        }

        public void HandleFindEntityObjectsByPrefix(ConnectionData connectionData)
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
                            Controller.StartFindEntityObjectsByPrefix(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindEntityObjectsByPrefixInExplorer()
        {
            HandleFindEntityObjectsByPrefixInExplorer(null);
        }

        public void HandleFindEntityObjectsByPrefixInExplorer(ConnectionData connectionData)
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
                            Controller.StartFindEntityObjectsByPrefixInExplorer(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindEntityObjectsByPrefixAndShowDependentComponents()
        {
            HandleFindEntityObjectsByPrefixAndShowDependentComponents(null);
        }

        public void HandleFindEntityObjectsByPrefixAndShowDependentComponents(ConnectionData connectionData)
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
                            Controller.StartFindEntityObjectsByPrefixAndShowDependentComponents(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindMarkedToDeleteInExplorer()
        {
            HandleFindMarkedToDeleteInExplorer(null);
        }

        public void HandleFindMarkedToDeleteInExplorer(ConnectionData connectionData)
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
                            Controller.StartFindMarkedToDeleteInExplorer(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindMarkedToDeleteAndShowDependentComponents()
        {
            HandleFindMarkedToDeleteAndShowDependentComponents(null);
        }

        public void HandleFindMarkedToDeleteAndShowDependentComponents(ConnectionData connectionData)
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
                            Controller.StartFindMarkedToDeleteAndShowDependentComponents(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindEntityObjectsByName()
        {
            HandleFindEntityObjectsByName(null);
        }

        public void HandleFindEntityObjectsByName(ConnectionData connectionData)
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
                            Controller.StartFindEntityObjectsByName(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindEntityObjectsByNameInExplorer()
        {
            HandleFindEntityObjectsByNameInExplorer(null);
        }

        public void HandleFindEntityObjectsByNameInExplorer(ConnectionData connectionData)
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
                            Controller.StartFindEntityObjectsByNameInExplorer(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindEntityObjectsContainsString()
        {
            HandleFindEntityObjectsContainsString(null);
        }

        public void HandleFindEntityObjectsContainsString(ConnectionData connectionData)
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
                            Controller.StartFindEntityObjectsContainsString(connectionData, commonConfig, dialog.GetText());
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToOutput(connectionData, ex);
                        }
                    }
                }
            }
        }

        public void HandleFindEntityObjectsContainsStringInExplorer()
        {
            HandleFindEntityObjectsContainsStringInExplorer(null);
        }

        public void HandleFindEntityObjectsContainsStringInExplorer(ConnectionData connectionData)
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
                            Controller.StartFindEntityObjectsContainsStringInExplorer(connectionData, commonConfig, dialog.GetText());
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
            HandleFindEntityById(null);
        }

        public void HandleFindEntityById(ConnectionData connectionData)
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

        public void HandleFindEntityByUniqueidentifier()
        {
            HandleFindEntityByUniqueidentifier(null);
        }

        public void HandleFindEntityByUniqueidentifier(ConnectionData connectionData)
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

        public void HandleEditEntityById()
        {
            HandleEditEntityById(null);
        }

        public void HandleEditEntityById(ConnectionData connectionData)
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

                var dialog = new WindowSelectEntityIdToFind(commonConfig, connectionData, string.Format("Edit Entity in {0} by Id", connectionData.Name));

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
    }
}
