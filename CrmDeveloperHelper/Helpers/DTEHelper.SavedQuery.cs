using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
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

            Func<ConnectionData, string> message = (conn) => string.Format(Properties.MessageBoxStrings.PublishSavedQueryFormat2, selectedFile.FilePath, conn.GetDescriptionColumn());
            string title = Properties.MessageBoxStrings.ConfirmPublishSavedQuery;

            GetConnectionConfigConfirmActionAndExecute(connectionData, message, title, (conn, commonConfig) => Controller.StartSavedQueryUpdate(conn, commonConfig, selectedFile));
        }

        public void HandleSavedQueryUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            Func<ConnectionData, string> message = (conn) => string.Format(Properties.MessageBoxStrings.PublishSavedQueryFormat2, filePath, conn.GetDescriptionColumn());
            string title = Properties.MessageBoxStrings.ConfirmPublishSavedQuery;

            GetConnectionConfigConfirmActionAndExecute(connectionData, message, title, (conn, commonConfig) => Controller.StartSavedQueryUpdate(conn, commonConfig, doc, filePath));
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

        public void HandleOpenSavedQueryOrganizationComparerCommand(ConnectionData connectionData1, ConnectionData connectionData2, string filter)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.OpenSavedQueryOrganizationComparer(connectionData1, connectionData2, commonConfig, filter);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
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
