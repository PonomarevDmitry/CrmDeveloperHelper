using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleSiteMapDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSiteMapDifference(conn, commonConfig, selectedFile));
        }

        public void HandleSiteMapDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSiteMapDifference(conn, commonConfig, doc, filePath));
        }

        public void HandleSiteMapUpdateCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSiteMapUpdate(conn, commonConfig, selectedFile));
        }

        public void HandleSiteMapUpdateCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSiteMapUpdate(conn, commonConfig, doc, filePath));
        }

        public void HandleSiteMapOpenInWebCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSiteMapOpenInWeb(conn, commonConfig, selectedFile));
        }

        public void HandleSiteMapGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSiteMapGetCurrent(conn, commonConfig, selectedFile));
        }

        public void HandleExplorerSiteMap()
        {
            HandleExplorerSiteMap(null, null);
        }

        public void HandleExplorerSiteMap(string filter)
        {
            HandleExplorerSiteMap(null, filter);
        }

        public void HandleExplorerSiteMap(ConnectionData connectionData)
        {
            HandleExplorerSiteMap(connectionData, null);
        }

        public void HandleExplorerSiteMap(ConnectionData connectionData, string filter)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartExplorerSiteMapXml(conn, commonConfig, filter));
        }

        public void HandleExportDefaultSiteMap(string selectedSiteMap)
        {
            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig != null)
            {
                string fileName = string.Format("SiteMap.{0}.xml", selectedSiteMap);

                var dialog = new Microsoft.Win32.SaveFileDialog()
                {
                    DefaultExt = ".xml",

                    Filter = "SiteMap (.xml)|*.xml",
                    FilterIndex = 1,

                    RestoreDirectory = true,
                    FileName = fileName,

                    InitialDirectory = commonConfig.FolderForExport,
                };

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    ActivateOutputWindow(null);
                    WriteToOutputEmptyLines(null, commonConfig);

                    try
                    {
                        Uri uri = FileOperations.GetSiteMapResourceUri(selectedSiteMap);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        var doc = XDocument.Load(info.Stream);
                        info.Stream.Dispose();

                        var filePath = dialog.FileName;

                        doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);
                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutput(null, "{0} exported.", fileName);

                        this.WriteToOutput(null, string.Empty);

                        this.WriteToOutputFilePathUri(null, filePath);

                        PerformAction(null, filePath, true);
                    }
                    catch (Exception ex)
                    {
                        WriteErrorToOutput(null, ex);
                    }
                }
            }
        }

        public void HandleShowDifferenceWithDefaultSiteMap(SelectedFile selectedFile, string selectedSiteMap)
        {
            if (selectedFile == null || !File.Exists(selectedFile.FilePath))
            {
                return;
            }

            CommonConfiguration commonConfig = CommonConfiguration.Get();

            if (commonConfig == null)
            {
                return;
            }

            ActivateOutputWindow(null);
            WriteToOutputEmptyLines(null, commonConfig);

            try
            {
                Uri uri = FileOperations.GetSiteMapResourceUri(selectedSiteMap);
                StreamResourceInfo info = Application.GetResourceStream(uri);

                var doc = XDocument.Load(info.Stream);
                info.Stream.Dispose();

                string fileName = string.Format("SiteMap.{0}.xml", selectedSiteMap);

                var filePath = Path.Combine(FileOperations.GetTempFileFolder(), fileName);

                doc.Save(filePath, SaveOptions.OmitDuplicateNamespaces);

                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);
                this.WriteToOutput(null, string.Empty);

                this.WriteToOutput(null, "{0} exported.", fileName);

                this.WriteToOutput(null, string.Empty);

                this.WriteToOutputFilePathUri(null, filePath);

                var task = this.ProcessStartProgramComparerAsync(selectedFile.FilePath, filePath, selectedFile.FileName, fileName);
            }
            catch (Exception ex)
            {
                WriteErrorToOutput(null, ex);
            }
        }
    }
}
