using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleSavedQueryDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSavedQueryDifference(conn, commonConfig, selectedFile));
        }

        public void HandleSavedQueryDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSavedQueryDifference(conn, commonConfig, doc, filePath));
        }

        public void HandleSavedQueryUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSavedQueryUpdate(conn, commonConfig, selectedFile));
        }

        public void HandleSavedQueryUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSavedQueryUpdate(conn, commonConfig, doc, filePath));
        }

        public void HandleSavedQueryOpenInWebCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSavedQueryOpenInWeb(conn, commonConfig, selectedFile));
        }

        public void HandleSavedQueryGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSavedQueryGetCurrent(conn, commonConfig, selectedFile));
        }

        public void HandleExplorerSystemSavedQuery()
        {
            string selection = GetSelectedText();

            HandleExplorerSystemSavedQuery(null, selection);
        }

        public void HandleExplorerSystemSavedQuery(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            HandleExplorerSystemSavedQuery(connectionData, selection);
        }

        public void HandleExplorerSystemSavedQuery(string selection)
        {
            HandleExplorerSystemSavedQuery(null, selection);
        }

        public void HandleExplorerSystemSavedQuery(ConnectionData connectionData, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerSystemSavedQueryXml(conn, commonConfig, selection));
        }

        public void HandleExportSystemSavedQueryVisualization()
        {
            HandleExportSystemSavedQueryVisualization(null);
        }

        public void HandleExportSystemSavedQueryVisualization(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerSystemSavedQueryVisualization(conn, commonConfig, selection));
        }
    }
}
