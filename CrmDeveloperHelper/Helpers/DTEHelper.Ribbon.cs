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

                if (connectionData.IsReadOnly)
                {
                    this.WriteToOutput(null, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                    return;
                }

                string message = string.Format(Properties.MessageBoxStrings.PublishRibbonDiffXmlFormat2, selectedFile.FileName, connectionData.GetDescription());

                var dialog = new WindowConfirmPublish(message, false);

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.StartRibbonDiffXmlUpdate(connectionData, commonConfig, selectedFile);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
        }

        public void HandleRibbonDiffXmlUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

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

                if (connectionData.IsReadOnly)
                {
                    this.WriteToOutput(null, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                    return;
                }

                string message = string.Format(Properties.MessageBoxStrings.PublishRibbonDiffXmlFormat2, Path.GetFileName(filePath), connectionData.GetDescription());

                var dialog = new WindowConfirmPublish(message, false);

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ActivateOutputWindow(connectionData);
                    WriteToOutputEmptyLines(connectionData, commonConfig);

                    CheckWishToChangeCurrentConnection(connectionData);

                    try
                    {
                        Controller.StartRibbonDiffXmlUpdate(connectionData, commonConfig, doc, filePath);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(connectionData, ex);
                    }
                }
            }
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
    }
}
