using EnvDTE;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        #region Plugin and Messages Trees

        public void HandleOpenPluginTree(string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            HandleOpenPluginTree(null, entityFilter, pluginTypeFilter, messageFilter);
        }

        public void HandleOpenPluginTree(ConnectionData connectionData, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingPluginTree(conn, commonConfig, entityFilter, pluginTypeFilter, messageFilter));
        }

        public void HandleSdkMessageExplorer()
        {
            HandleSdkMessageExplorer(null);
        }

        public void HandleSdkMessageExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingSdkMessageExplorer(conn, commonConfig, selection));
        }

        public void HandleSdkMessageFilterTree()
        {
            HandleSdkMessageFilterTree(null);
        }

        public void HandleSdkMessageFilterTree(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartShowingSdkMessageFilterTree(conn, commonConfig, selection, null));
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

        public void HandleTraceReaderOpenWindow(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartTraceReaderOpenWindow(conn, commonConfig));
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

        public void HandleExportCustomControl()
        {
            HandleExportCustomControl(null);
        }

        public void HandleExportCustomControl(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerCustomControl(conn, commonConfig, selection));
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

        public void HandleEntityMetadataFileGenerationOptions()
        {
            GetConfigAndExecute((commonConfig) => Controller.StartOpeningEntityMetadataFileGenerationOptions());
        }

        public void HandleJavaScriptFileGenerationOptions()
        {
            GetConfigAndExecute((commonConfig) => Controller.StartOpeningJavaScriptFileGenerationOptions());
        }

        public void HandleGlobalOptionSetsMetadataFileGenerationOptions()
        {
            GetConfigAndExecute((commonConfig) => Controller.StartOpeningGlobalOptionSetsMetadataFileGenerationOptions());
        }

        public void HandleFileGenerationOptions()
        {
            GetConfigAndExecute((commonConfig) => Controller.StartOpeningFileGenerationOptions());
        }

        public void HandleFileGenerationConfiguration()
        {
            GetConfigAndExecute((commonConfig) => Controller.StartOpeningFileGenerationConfiguration());
        }
    }
}
