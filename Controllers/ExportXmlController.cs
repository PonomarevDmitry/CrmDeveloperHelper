using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    /// <summary>
    /// Контроллер для экспорта риббона
    /// </summary>
    public class ExportXmlController
    {
        private readonly IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public ExportXmlController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Экспортирование Sitemap Xml.

        public async Task ExecuteExportingSitemapXml(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSitemapXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingSitemapXml(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingSitemapXml(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenExportSiteMapWindow(this._iWriteToOutput, service, commonConfig);
        }

        #endregion Экспортирование Sitemap Xml.

        #region Экспортирование ApplicationRibbon.

        public async Task ExecuteExportingApplicationRibbonXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingRibbonXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingApplicationRibbonXml(selection, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingApplicationRibbonXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, commonConfig);
        }

        #endregion Экспортирование Ribbon.

        #region Экспортирование System View (Saved Query) LayoutXml and FethcXml.

        public async Task ExecuteExportingSystemSavedQueryXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemSavedQueryXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingSystemSavedQueryXml(selection, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingSystemSavedQueryXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion System View (Saved Query) LayoutXml and FethcXml.

        #region Экспортирование System Form FormXml.

        public async Task ExecuteExportingSystemFormXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemFormXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingSystemFormXml(selection, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingSystemFormXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion Экспортирование System Form FormXml.

        #region Экспортирование System Chart (Saved Query Visualization) Xml.

        public async Task ExecuteExportingSystemSavedQueryVisualizationXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemSavedQueryVisualizationXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingSystemSavedQueryVisualizationXml(selection, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingSystemSavedQueryVisualizationXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion System Chart (Saved Query Visualization) Xml.

        #region Экспортирование списка событий форм.

        public async Task ExecuteExportingFormsEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemFormsEventsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingFormsEvents(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingFormsEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            string filePath = await CreateFormsEventsFile(service, commonConfig.FolderForExport, commonConfig.FormsEventsFileName, connectionData.Name, commonConfig.FormsEventsOnlyWithFormLibraries);

            this._iWriteToOutput.PerformAction(filePath);
        }

        private async Task<string> CreateFormsEventsFile(IOrganizationServiceExtented service, string fileFolder, string fileNameFormat, string connectionDataName, bool onlyWithFormLibraries)
        {
            this._iWriteToOutput.WriteToOutput("Start analyzing System Forms.");

            var repository = new SystemFormRepository(service);

            var allForms = (await repository.GetListAsync(string.Empty))
                .OrderBy(ent => ent.ObjectTypeCode)
                .ThenBy(ent => ent.Type.Value)
                .ThenBy(ent => ent.Name);

            SolutionComponentDescriptor descriptor = new SolutionComponentDescriptor(service, true);

            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            StringBuilder content = new StringBuilder();

            foreach (var systemForm in allForms)
            {
                string entityName = systemForm.ObjectTypeCode;
                string name = systemForm.Name;

                string typeName = systemForm.FormattedValues[SystemForm.Schema.Attributes.type];

                string formXml = systemForm.FormXml;

                if (!string.IsNullOrEmpty(formXml))
                {
                    XElement doc = XElement.Parse(formXml);

                    string events = handler.GetEventsDescription(doc);

                    if (!string.IsNullOrEmpty(events))
                    {
                        string desc = handler.GetFormLibrariesDescription(doc);

                        if (onlyWithFormLibraries && string.IsNullOrEmpty(desc))
                        {
                            continue;
                        }

                        if (content.Length > 0)
                        {
                            content
                                .AppendLine()
                                .AppendLine(new string('-', 100))
                                .AppendLine()
                                ;
                        }

                        content.AppendFormat("Entity: {0}", entityName).AppendLine();
                        content.AppendFormat("Form Type: {0}", typeName).AppendLine();
                        content.AppendFormat("Form Name: {0}", name).AppendLine();

                        if (!string.IsNullOrEmpty(desc))
                        {
                            content.AppendLine("FormLibraries:");
                            content.Append(desc);
                        }

                        content.AppendLine("Events:");
                        content.AppendLine(events);
                    }
                }
            }

            string filePath = string.Empty;

            if (content.Length > 0)
            {
                string fileName = string.Format("{0}{1} {2}.txt"
                , (!string.IsNullOrEmpty(connectionDataName) ? connectionDataName + "." : string.Empty)
                , fileNameFormat.Trim()
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));

                filePath = Path.Combine(fileFolder, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(fileFolder))
                {
                    Directory.CreateDirectory(fileFolder);
                }

                File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput("System Forms Events were exported to {0}", filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("No Forms Events were founded.");
            }

            return filePath;
        }

        #endregion Экспортирование списка событий форм.

        #region Экспортирование Workflow.

        public async Task ExecuteExportingWorkflow(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingWorkflowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingWorkflow(selection, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingWorkflow(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion Экспортирование Workflow.

        #region Экспортирование информации об организации.

        public async Task ExecuteExportingOrganizationInformation(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingOrganizationInformationFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ExportingOrganizationInformation(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ExportingOrganizationInformation(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportOrganization(
                        this._iWriteToOutput
                        , service
                        , commonConfig
                        , connectionData.Name
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        #endregion Экспортирование информации об организации.

        #region Trace Reader.

        public async Task ExecuteShowingTraceReader(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.TraceReaderFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await ShowingTraceReader(connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task ShowingTraceReader(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenTraceReaderWindow(this._iWriteToOutput, service, commonConfig);
        }

        #endregion Trace Reader.

        public async Task ExecuteDifferenceSiteMap(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSiteMapFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await DifferenceSiteMap(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task DifferenceSiteMap(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            //string fileText = File.ReadAllText(selectedFile.FilePath);

            //if (!ContentCoparerHelper.TryParseXml(fileText, out var doc))
            //{
            //    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
            //    return;
            //}

            //var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

            //if (attribute == null)
            //{
            //    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.FileNotContainsXmlAttributeFormat2, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName.ToString(), selectedFile.FilePath);
            //    return;
            //}

            //string siteMapUniqueName = attribute.Value;

            string siteMapUniqueName = string.Empty;

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repository = new SitemapRepository(service);

            var siteMap = repository.FindByExactName(siteMapUniqueName, new ColumnSet(true));

            if (siteMap == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.SiteMapNotFoundedFormat2, connectionData.Name, SiteMap.Schema.EntityLogicalName, siteMapUniqueName);
                this._iWriteToOutput.ActivateOutputWindow();
                return;
            }

            string xmlContent = siteMap.GetAttributeValue<string>(SiteMap.Schema.Attributes.sitemapxml);

            string fieldTitle = "SiteMapXml";

            string name = !string.IsNullOrEmpty(siteMap.SiteMapNameUnique) ? " " + siteMap.SiteMapNameUnique : string.Empty;

            string fileTitle2 = EntityFileNameFormatter.GetSiteMapFileName(connectionData.Name, name, siteMap.Id, fieldTitle, "xml");
            string filePath2 = FileOperations.GetNewTempFile(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaSiteMapXml);

                        if (schemasResources != null)
                        {
                            xmlContent = ContentCoparerHelper.ReplaceXsdSchema(xmlContent, schemasResources);
                        }
                    }

                    xmlContent = ContentCoparerHelper.FormatXml(xmlContent, commonConfig.ExportSiteMapXmlAttributeOnNewLine);

                    File.WriteAllText(filePath2, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle, filePath2);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
                return;
            }

            string filePath1 = selectedFile.FilePath;
            string fileTitle1 = selectedFile.FileName;

            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
        }
    }
}