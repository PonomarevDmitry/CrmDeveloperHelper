using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleCheckEntitiesOwnership(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckEntitiesOwnership(conn, commonConfig));
        }

        public void HandleCheckGlobalOptionSetDuplicates(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckGlobalOptionSetDuplicates(conn, commonConfig));
        }

        public void HandleCheckComponentTypeEnum(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckComponentTypeEnum(conn, commonConfig));
        }

        public void HandleCheckUnknownFormControlTypes(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckUnknownFormControlTypes(conn, commonConfig));
        }

        public void HandleCheckCreateAllDependencyNodesDescription(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCreateAllDependencyNodesDescription(conn, commonConfig));
        }

        public void HandleCheckManagedEntities(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckManagedEntities(conn, commonConfig));
        }

        public void HandleCheckPluginSteps(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckPluginSteps(conn, commonConfig));
        }

        public void HandleCheckPluginImages(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckPluginImages(conn, commonConfig));
        }

        public void HandleCheckPluginStepsRequiredComponents(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckPluginStepsRequiredComponents(conn, commonConfig));
        }

        public void HandleCheckPluginImagesRequiredComponents(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartCheckPluginImagesRequiredComponents(conn, commonConfig));
        }
    }
}
