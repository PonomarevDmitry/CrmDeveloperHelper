using EnvDTE;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleSystemFormDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormDifference(conn, commonConfig, selectedFile));
        }

        public void HandleSystemFormDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormDifference(conn, commonConfig, doc, filePath));
        }

        public void HandleSystemFormUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormUpdate(conn, commonConfig, selectedFile));
        }

        public void HandleSystemFormUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormUpdate(conn, commonConfig, doc, filePath));
        }

        public void HandleSystemFormOpenInWebCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormOpenInWeb(conn, commonConfig, selectedFile));
        }

        public void HandleSystemFormGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSystemFormGetCurrent(conn, commonConfig, selectedFile));
        }

        public void HandleExplorerSystemForm()
        {
            string selection = GetSelectedText();

            HandleExplorerSystemForm(null, null, selection);
        }

        public void HandleExplorerSystemForm(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            HandleExplorerSystemForm(connectionData, null, selection);
        }

        public void HandleExplorerSystemForm(string selection)
        {
            HandleExplorerSystemForm(null, null, selection);
        }

        public void HandleExplorerSystemForm(ConnectionData connectionData, string selection)
        {
            HandleExplorerSystemForm(connectionData, null, selection);
        }

        public void HandleExplorerSystemForm(ConnectionData connectionData, SelectedItem selectedItem)
        {
            HandleExplorerSystemForm(connectionData, selectedItem, string.Empty);
        }

        private void HandleExplorerSystemForm(ConnectionData connectionData, SelectedItem selectedItem, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerSystemForm(conn, commonConfig, selection, selectedItem));
        }
    }
}
