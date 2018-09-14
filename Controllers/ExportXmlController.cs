using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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
        private IWriteToOutput _iWriteToOutput = null;

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
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting Sitemap Xml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

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
                this._iWriteToOutput.WriteToOutput("*********** End Exporting Sitemap Xml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingSitemapXml(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenExportSiteMapWindow(this._iWriteToOutput, service, commonConfig);
        }

        #endregion Экспортирование Sitemap Xml.

        #region Экспортирование RibbonXml.

        public async Task ExecuteExportingRibbonXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting Ribbon Xml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                await ExportingRibbonXml(selection, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Exporting Ribbon Xml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingRibbonXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenEntityRibbonWindow(this._iWriteToOutput, service, commonConfig, selection, null);
        }

        #endregion Экспортирование RibbonXml.

        #region Экспортирование System View (Saved Query) LayoutXml and FethcXml.

        public async Task ExecuteExportingSystemSavedQueryXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting System View (Saved Query) Xml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

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
                this._iWriteToOutput.WriteToOutput("*********** End Exporting System View (Saved Query) Xml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingSystemSavedQueryXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion System View (Saved Query) LayoutXml and FethcXml.

        #region Экспортирование System Form FormXml.

        public async Task ExecuteExportingSystemFormXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting System Form FormXml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

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
                this._iWriteToOutput.WriteToOutput("*********** End Exporting System Form FormXml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingSystemFormXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion Экспортирование System Form FormXml.

        #region Экспортирование System Chart (Saved Query Visualization) Xml.

        public async Task ExecuteExportingSystemSavedQueryVisualizationXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting System Chart (Saved Query Visualization) Xml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

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
                this._iWriteToOutput.WriteToOutput("*********** End Exporting System Chart (Saved Query Visualization) Xml at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingSystemSavedQueryVisualizationXml(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion System Chart (Saved Query Visualization) Xml.

        #region Экспортирование списка событий форм.

        public async Task ExecuteExportingFormsEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting System Forms Events at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

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
                this._iWriteToOutput.WriteToOutput("*********** End Exporting System Forms Events at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingFormsEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            string filePath = await CreateFormsEventsFile(service, commonConfig.FolderForExport, commonConfig.FormsEventsFileName, connectionData.Name, commonConfig.FormsEventsOnlyWithFormLibraries);

            this._iWriteToOutput.PerformAction(filePath, commonConfig);
        }

        private async Task<string> CreateFormsEventsFile(IOrganizationServiceExtented service, string fileFolder, string fileNameFormat, string connectionDataName, bool onlyWithFormLibraries)
        {
            this._iWriteToOutput.WriteToOutput("Start analyzing System Forms.");

            var repository = new SystemFormRepository(service);

            var allForms = (await repository.GetListAsync(string.Empty))
                .OrderBy(ent => ent.ObjectTypeCode)
                .ThenBy(ent => ent.Type.Value)
                .ThenBy(ent => ent.Name);

            SolutionComponentDescriptor descriptor = new SolutionComponentDescriptor(_iWriteToOutput, service, true);

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

        #region Экспортирование зависимостей атрибутов.

        public async Task ExecuteExportingEntityAttributesDependentComponents(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting Entity Attributes Dependent Components at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                await ExportingEntityAttributesDependentComponents(selection, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutput("*********** End Exporting Entity Attributes Dependent Components at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingEntityAttributesDependentComponents(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenAttributesDependentComponentWindow(this._iWriteToOutput, service, commonConfig, selection, null);
        }

        #endregion Экспортирование зависимостей атрибутов.

        #region Экспортирование Workflow.

        public async Task ExecuteExportingWorkflow(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting Workflow at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

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
                this._iWriteToOutput.WriteToOutput("*********** End Exporting Workflow at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingWorkflow(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion Экспортирование Workflow.

        #region Экспортирование информации об организации.

        public async Task ExecuteExportingOrganizationInformation(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutput("*********** Start Exporting Organization Information at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

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
                this._iWriteToOutput.WriteToOutput("*********** End Exporting Organization Information at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ExportingOrganizationInformation(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

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
            this._iWriteToOutput.WriteToOutput("*********** Start Trace Reader at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

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
                this._iWriteToOutput.WriteToOutput("*********** End Trace Reader at {0} *******************************************************", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        private async Task ShowingTraceReader(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput("No current CRM Connection.");
                return;
            }

            this._iWriteToOutput.WriteToOutput("Connect to CRM.");

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

            WindowHelper.OpenTraceReaderWindow(this._iWriteToOutput, service, commonConfig);
        }

        #endregion Trace Reader.
    }
}
