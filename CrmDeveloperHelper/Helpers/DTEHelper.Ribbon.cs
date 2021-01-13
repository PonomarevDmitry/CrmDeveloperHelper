using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.IO;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        #region RibbonDiffXml

        public void HandleRibbonDiffXmlDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartRibbonDiffXmlDifference(conn, commonConfig, selectedFile));
        }

        public void HandleRibbonDiffXmlDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartRibbonDiffXmlDifference(conn, commonConfig, doc, filePath));
        }

        public void HandleRibbonDiffXmlUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            Func<ConnectionData, string> message = (conn) => string.Format(Properties.MessageBoxStrings.PublishRibbonDiffXmlFormat2, selectedFile.FileName, conn.GetDescription());
            string title = Properties.MessageBoxStrings.ConfirmPublishRibbonDiffXml;

            GetConnectionConfigConfirmActionAndExecute(connectionData, message, title, (conn, commonConfig) => Controller.StartRibbonDiffXmlUpdate(conn, commonConfig, selectedFile));
        }

        public void HandleRibbonDiffXmlUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            Func<ConnectionData, string> message = (conn) => string.Format(Properties.MessageBoxStrings.PublishRibbonDiffXmlFormat2, Path.GetFileName(filePath), conn.GetDescription());
            string title = Properties.MessageBoxStrings.ConfirmPublishRibbonDiffXml;

            GetConnectionConfigConfirmActionAndExecute(connectionData, message, title, (conn, commonConfig) => Controller.StartRibbonDiffXmlUpdate(conn, commonConfig, doc, filePath));
        }

        public void HandleRibbonDiffXmlGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartRibbonDiffXmlGetCurrent(conn, commonConfig, selectedFile));
        }

        #endregion RibbonDiffXml

        #region Ribbon

        public void HandleRibbonDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartRibbonDifference(conn, commonConfig, selectedFile));
        }

        public void HandleRibbonDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartRibbonDifference(conn, commonConfig, doc, filePath));
        }

        public void HandleEntityRibbonOpenInWeb(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartEntityRibbonOpenInWeb(conn, commonConfig, selectedFile));
        }

        public void HandleRibbonExplorerCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartRibbonExplorer(conn, commonConfig, selectedFile));
        }

        public void HandleOpenEntityMetadataOrganizationComparerCommand(ConnectionData connectionData1, ConnectionData connectionData2, string filter)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.OpenEntityMetadataOrganizationComparer(connectionData1, connectionData2, commonConfig, filter);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleOpenApplicationRibbonOrganizationComparerCommand(ConnectionData connectionData1, ConnectionData connectionData2)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (connectionData1 != null && connectionData2 != null && connectionData1 != connectionData2 && commonConfig != null)
            {
                ActivateOutputWindow(null);
                WriteToOutputEmptyLines(null, commonConfig);

                try
                {
                    Controller.OpenApplicationRibbonOrganizationComparer(connectionData1, connectionData2, commonConfig);
                }
                catch (Exception ex)
                {
                    WriteErrorToOutput(null, ex);
                }
            }
        }

        public void HandleOpenApplicationRibbonExplorer()
        {
            HandleOpenApplicationRibbonExplorer(null);
        }

        public void HandleOpenApplicationRibbonExplorer(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenApplicationRibbonExplorer(conn, commonConfig));
        }

        public void HandleRibbonGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartRibbonGetCurrent(conn, commonConfig, selectedFile));
        }

        #endregion Ribbon

        public void HandleRibbonAndRibbonDiffXmlGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartRibbonAndRibbonDiffXmlGetCurrent(conn, commonConfig, selectedFile));
        }
    }
}
