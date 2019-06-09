using Microsoft.Crm.Sdk.Messages;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        #region Экспортирование списка событий форм.

        public async Task ExecuteExportingFormsEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemFormsEventsFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ExportingFormsEvents(connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task ExportingFormsEvents(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            string filePath = await CreateFormsEventsFile(service, commonConfig.FolderForExport, commonConfig.FormsEventsFileName, connectionData.Name, commonConfig.FormsEventsOnlyWithFormLibraries);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        private async Task<string> CreateFormsEventsFile(IOrganizationServiceExtented service, string fileFolder, string fileNameFormat, string connectionDataName, bool onlyWithFormLibraries)
        {
            this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Start analyzing System Forms.");

            var repository = new SystemFormRepository(service);

            var allForms = (await repository.GetListAsync(string.Empty))
                .OrderBy(ent => ent.ObjectTypeCode)
                .ThenBy(ent => ent.Type.Value)
                .ThenBy(ent => ent.Name);

            SolutionComponentDescriptor descriptor = new SolutionComponentDescriptor(service);

            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            StringBuilder content = new StringBuilder();

            foreach (var savedQuery in allForms)
            {
                string entityName = savedQuery.ObjectTypeCode;
                string name = savedQuery.Name;

                string typeName = savedQuery.FormattedValues[SystemForm.Schema.Attributes.type];

                string formXml = savedQuery.FormXml;

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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "System Forms Events were exported to {0}", filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, "No Forms Events were founded.");
            }

            return filePath;
        }

        #endregion Экспортирование списка событий форм.

        #region SiteMap

        public async Task ExecuteDifferenceSiteMap(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSiteMapFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await DifferenceSiteMap(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task DifferenceSiteMap(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string siteMapNameUnique = string.Empty;

            {
                string fileText = File.ReadAllText(selectedFile.FilePath);

                if (ContentCoparerHelper.TryParseXml(fileText, out var doc))
                {
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                    if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                    {
                        siteMapNameUnique = attribute.Value;
                    }
                }
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repository = new SitemapRepository(service);

            var siteMap = repository.FindByExactName(siteMapNameUnique, new ColumnSet(true));

            if (siteMap == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SiteMapNotFoundedFormat2, connectionData.Name, siteMapNameUnique);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string siteMapXml = siteMap.GetAttributeValue<string>(SiteMap.Schema.Attributes.sitemapxml);

            string fieldTitle = "SiteMapXml";

            string fileTitle2 = EntityFileNameFormatter.GetSiteMapFileName(connectionData.Name, siteMap.SiteMapNameUnique, siteMap.Id, fieldTitle, "xml");
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (!string.IsNullOrEmpty(siteMapXml))
            {
                try
                {
                    siteMapXml = ContentCoparerHelper.FormatXmlByConfiguration(siteMapXml, commonConfig, WindowExplorerSiteMap._xmlOptions
                       , schemaName: CommonExportXsdSchemasCommand.SchemaSiteMapXml
                       , siteMapUniqueName: siteMap.SiteMapNameUnique ?? string.Empty
                    );

                    File.WriteAllText(filePath2, siteMapXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, fieldTitle, filePath2);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string filePath1 = selectedFile.FilePath;
            string fileTitle1 = selectedFile.FileName;

            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
        }

        public async Task ExecuteUpdateSiteMap(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSiteMapFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdateSiteMap(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task UpdateSiteMap(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentCoparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                _iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string siteMapNameUnique = string.Empty;

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

            if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
            {
                siteMapNameUnique = attribute.Value;
            }

            ContentCoparerHelper.ClearRoot(doc);

            bool validateResult = await SitemapRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ValidatingXmlForFieldFailedFormat1, SiteMap.Schema.Attributes.sitemapxml);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                var dialogResult = MessageBoxResult.Cancel;

                var t = new Thread(() =>
                {
                    dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult != MessageBoxResult.OK)
                {
                    return;
                }
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repositorySiteMap = new SitemapRepository(service);

            var siteMap = repositorySiteMap.FindByExactName(siteMapNameUnique, new ColumnSet(true));

            if (siteMap == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SiteMapNotFoundedFormat2, connectionData.Name, SiteMap.Schema.EntityLogicalName, siteMapNameUnique);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            {
                string siteMapXml = siteMap.GetAttributeValue<string>(SiteMap.Schema.Attributes.sitemapxml);

                string fieldTitle = "SiteMapXml BackUp";

                string fileName = EntityFileNameFormatter.GetSiteMapFileName(connectionData.Name, siteMap.SiteMapName, siteMap.Id, fieldTitle, "xml");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!string.IsNullOrEmpty(siteMapXml))
                {
                    try
                    {
                        siteMapXml = ContentCoparerHelper.FormatXmlByConfiguration(siteMapXml, commonConfig, WindowExplorerSiteMap._xmlOptions
                            , schemaName: CommonExportXsdSchemasCommand.SchemaSiteMapXml
                            , siteMapUniqueName: siteMap.SiteMapNameUnique ?? string.Empty
                        );

                        File.WriteAllText(filePath, siteMapXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, fieldTitle, filePath);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SiteMap.Schema.EntityLogicalName, siteMap.SiteMapNameUnique, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(connectionData);
                }
            }

            var newText = doc.ToString(SaveOptions.DisableFormatting);

            var updateEntity = new SiteMap
            {
                Id = siteMap.Id
            };
            updateEntity.Attributes[SiteMap.Schema.Attributes.sitemapxml] = newText;

            await service.UpdateAsync(updateEntity);

            _iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.PublishingSiteMapFormat3, service.ConnectionData.Name, siteMap.SiteMapName, siteMap.Id.ToString());

            {
                var repositoryPublish = new PublishActionsRepository(service);

                await repositoryPublish.PublishSiteMapsAsync(new[] { siteMap.Id });
            }
        }

        public async Task ExecuteOpenInWebSiteMap(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningSiteMapInWebFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpenInWebSiteMap(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task OpenInWebSiteMap(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentCoparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                WindowHelper.OpenExportSiteMapWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            string siteMapNameUnique = string.Empty;

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

            if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
            {
                siteMapNameUnique = attribute.Value;
            }

            var repositorySiteMap = new SitemapRepository(service);

            var siteMap = repositorySiteMap.FindByExactName(siteMapNameUnique, new ColumnSet(false));

            if (siteMap == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SiteMapNotFoundedFormat2, connectionData.Name, SiteMap.Schema.EntityLogicalName, siteMapNameUnique);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);

                WindowHelper.OpenExportSiteMapWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SiteMap, siteMap.Id);
        }

        #endregion SiteMap

        #region SystemForm

        public async Task ExecuteDifferenceSystemForm(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSystemFormFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await DifferenceSystemForm(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task DifferenceSystemForm(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentCoparerHelper.TryParseXml(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                return;
            }

            var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId.ToString()
                    , selectedFile.FilePath
                    );

                return;
            }

            if (string.IsNullOrEmpty(attribute.Value)
                || !Guid.TryParse(attribute.Value, out var systemFormId)
                )
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId.ToString()
                    , attribute.Value
                    , selectedFile.FilePath
                    );

                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(systemFormId, new ColumnSet(true));

            if (systemForm == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SystemFormNotFoundedFormat2, connectionData.Name, systemFormId);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string formXml = systemForm.GetAttributeValue<string>(SystemForm.Schema.Attributes.formxml);

            string fieldTitle = "FormXml";

            string fileTitle2 = EntityFileNameFormatter.GetSystemFormFileName(connectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, fieldTitle, "xml");
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    formXml = ContentCoparerHelper.FormatXmlByConfiguration(formXml, commonConfig, WindowExplorerSystemForm._xmlOptions
                        , schemaName: CommonExportXsdSchemasCommand.SchemaFormXml
                        , formId: systemForm.Id
                    );

                    File.WriteAllText(filePath2, formXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, fieldTitle, filePath2);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string filePath1 = selectedFile.FilePath;
            string fileTitle1 = selectedFile.FileName;

            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
        }

        public async Task ExecuteUpdateSystemForm(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSystemFormFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdateSystemForm(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task UpdateSystemForm(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentCoparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                return;
            }

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId.ToString()
                    , selectedFile.FilePath
                    );

                return;
            }

            if (string.IsNullOrEmpty(attribute.Value)
                || !Guid.TryParse(attribute.Value, out var formId)
                )
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId.ToString()
                    , attribute.Value
                    , selectedFile.FilePath
                    );

                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ValidatingXmlForFieldFormat1, SystemForm.Schema.Attributes.formxml);

            ContentCoparerHelper.ClearRoot(doc);

            bool validateResult = await SystemFormRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ValidatingXmlForFieldFailedFormat1, SystemForm.Schema.Attributes.formxml);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                var dialogResult = MessageBoxResult.Cancel;

                var t = new Thread(() =>
                {
                    dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult != MessageBoxResult.OK)
                {
                    return;
                }
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repositorySystemForm = new SystemFormRepository(service);

            var systemForm = await repositorySystemForm.GetByIdAsync(formId, new ColumnSet(true));

            if (systemForm == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SystemFormNotFoundedFormat2, connectionData.Name, formId);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            {
                string formXml = systemForm.GetAttributeValue<string>(SystemForm.Schema.Attributes.formxml);

                string fieldTitle = "FormXml BackUp";

                string fileName = EntityFileNameFormatter.GetSystemFormFileName(connectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, fieldTitle, "xml");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!string.IsNullOrEmpty(formXml))
                {
                    try
                    {
                        formXml = ContentCoparerHelper.FormatXmlByConfiguration(formXml, commonConfig, WindowExplorerSystemForm._xmlOptions
                            , schemaName: CommonExportXsdSchemasCommand.SchemaFormXml
                            , formId: systemForm.Id
                        );

                        File.WriteAllText(filePath, formXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, fieldTitle, filePath);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(connectionData);
                }
            }

            var newText = doc.ToString(SaveOptions.DisableFormatting);

            var updateEntity = new SystemForm
            {
                Id = formId
            };
            updateEntity.Attributes[SystemForm.Schema.Attributes.formxml] = newText;

            await service.UpdateAsync(updateEntity);

            var repositoryPublish = new PublishActionsRepository(service);

            _iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.PublishingSystemFormFormat3, service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name);
            await repositoryPublish.PublishDashboardsAsync(new[] { formId });

            if (systemForm.ObjectTypeCode.IsValidEntityName())
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, systemForm.ObjectTypeCode);
                await repositoryPublish.PublishEntitiesAsync(new[] { systemForm.ObjectTypeCode });
            }
        }

        public async Task ExecuteOpenInWebSystemForm(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningSystemFormInWebFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpenInWebSystemForm(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task OpenInWebSystemForm(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentCoparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);

                WindowHelper.OpenSystemFormWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData
                    , Properties.OutputStrings.FileNotContainsXmlAttributeFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId.ToString()
                    , selectedFile.FilePath
                );

                WindowHelper.OpenSystemFormWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            if (string.IsNullOrEmpty(attribute.Value)
                || !Guid.TryParse(attribute.Value, out var formId)
            )
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId.ToString()
                    , attribute.Value
                    , selectedFile.FilePath
                );

                WindowHelper.OpenSystemFormWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            var repositorySystemForm = new SystemFormRepository(service);

            var systemForm = await repositorySystemForm.GetByIdAsync(formId, new ColumnSet(false));

            if (systemForm == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SystemFormNotFoundedFormat2, connectionData.Name, formId);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);

                WindowHelper.OpenSystemFormWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, systemForm.Id);
        }

        #endregion SystemForm

        #region SavedQuery

        public async Task ExecuteDifferenceSavedQuery(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.DifferenceSavedQueryFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await DifferenceSavedQuery(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task DifferenceSavedQuery(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentCoparerHelper.TryParseXml(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                return;
            }

            var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId.ToString()
                    , selectedFile.FilePath
                    );

                return;
            }

            if (string.IsNullOrEmpty(attribute.Value)
                || !Guid.TryParse(attribute.Value, out var savedQueryId)
                )
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId.ToString()
                    , attribute.Value
                    , selectedFile.FilePath
                    );

                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repository = new SavedQueryRepository(service);

            var savedQuery = await repository.GetByIdAsync(savedQueryId, new ColumnSet(true));

            if (savedQuery == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SavedQueryNotFoundedFormat2, connectionData.Name, savedQueryId);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string fieldName = SavedQueryRepository.GetFieldNameByXmlRoot(doc.Name.ToString());
            string fieldTitle = SavedQueryRepository.GetFieldTitleByXmlRoot(doc.Name.ToString());

            string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

            string fileTitle2 = EntityFileNameFormatter.GetSavedQueryFileName(connectionData.Name, savedQuery.ReturnedTypeCode, savedQuery.Name, fieldTitle, "xml");
            string filePath2 = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(fileTitle2), Path.GetExtension(fileTitle2));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    xmlContent = ContentCoparerHelper.FormatXmlByConfiguration(xmlContent, commonConfig, WindowExplorerSavedQuery._xmlOptions
                        , schemaName: CommonExportXsdSchemasCommand.SchemaFetch
                        , savedQueryId: savedQueryId
                    );

                    File.WriteAllText(filePath2, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle, filePath2);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            string filePath1 = selectedFile.FilePath;
            string fileTitle1 = selectedFile.FileName;

            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, fileTitle1, fileTitle2);
        }

        public async Task ExecuteUpdateSavedQuery(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.UpdatingSavedQueryFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await UpdateSavedQuery(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task UpdateSavedQuery(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentCoparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);
                return;
            }

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotContainsXmlAttributeFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId.ToString()
                    , selectedFile.FilePath
                    );

                return;
            }

            if (string.IsNullOrEmpty(attribute.Value)
                || !Guid.TryParse(attribute.Value, out var savedQueryId)
                )
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId.ToString()
                    , attribute.Value
                    , selectedFile.FilePath
                    );

                return;
            }

            string fieldName = SavedQueryRepository.GetFieldNameByXmlRoot(doc.Root.Name.ToString());
            string fieldTitle = SavedQueryRepository.GetFieldTitleByXmlRoot(doc.Root.Name.ToString());

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ValidatingXmlForFieldFormat1, fieldTitle);

            ContentCoparerHelper.ClearRoot(doc);

            bool validateResult = await SavedQueryRepository.ValidateXmlDocumentAsync(connectionData, _iWriteToOutput, doc, fieldTitle);

            if (!validateResult)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ValidatingXmlForFieldFailedFormat1, fieldTitle);
                _iWriteToOutput.ActivateOutputWindow(connectionData);

                var dialogResult = MessageBoxResult.Cancel;

                var t = new Thread(() =>
                {
                    dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();

                if (dialogResult != MessageBoxResult.OK)
                {
                    return;
                }
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            var repositorySavedQuery = new SavedQueryRepository(service);

            var savedQuery = await repositorySavedQuery.GetByIdAsync(savedQueryId, new ColumnSet(true));

            if (savedQuery == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SavedQueryNotFoundedFormat2, connectionData.Name, savedQueryId);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
                return;
            }

            if (string.Equals(fieldName, SavedQuery.Schema.Attributes.layoutxml, StringComparison.InvariantCulture)
                && !string.IsNullOrEmpty(savedQuery.ReturnedTypeCode)
            )
            {
                var entityData = connectionData.GetEntityIntellisenseData(savedQuery.ReturnedTypeCode);

                if (entityData != null && entityData.ObjectTypeCode.HasValue)
                {
                    XAttribute attr = doc.Root.Attribute("object");

                    if (attr != null)
                    {
                        attr.Value = entityData.ObjectTypeCode.ToString();
                    }
                }
            }

            {
                string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

                fieldTitle += " BackUp";

                string fileName = EntityFileNameFormatter.GetSavedQueryFileName(connectionData.Name, savedQuery.ReturnedTypeCode, savedQuery.Name, fieldTitle, "xml");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                if (!string.IsNullOrEmpty(xmlContent))
                {
                    try
                    {
                        xmlContent = ContentCoparerHelper.FormatXmlByConfiguration(xmlContent, commonConfig, WindowExplorerSavedQuery._xmlOptions
                            , schemaName: CommonExportXsdSchemasCommand.SchemaFetch
                            , savedQueryId: savedQueryId
                        );

                        File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle, filePath);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(connectionData);
                }
            }

            var newText = doc.ToString(SaveOptions.DisableFormatting);

            if (string.Equals(fieldName, SavedQuery.Schema.Attributes.fetchxml, StringComparison.InvariantCulture))
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.ExecutingValidateSavedQueryRequest);

                var request = new ValidateSavedQueryRequest()
                {
                    FetchXml = newText,
                    QueryType = savedQuery.QueryType.GetValueOrDefault()
                };

                service.Execute(request);
            }

            var updateEntity = new SavedQuery
            {
                Id = savedQueryId
            };
            updateEntity.Attributes[fieldName] = newText;

            await service.UpdateAsync(updateEntity);

            _iWriteToOutput.WriteToOutput(connectionData, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, savedQuery.ReturnedTypeCode);

            {
                var repositoryPublish = new PublishActionsRepository(service);

                await repositoryPublish.PublishEntitiesAsync(new[] { savedQuery.ReturnedTypeCode });
            }
        }

        public async Task ExecuteOpenInWebSavedQuery(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.OpeningSavedQueryInWebFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpenInWebSavedQuery(selectedFile, connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private async Task OpenInWebSavedQuery(SelectedFile selectedFile, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            if (!File.Exists(selectedFile.FilePath))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileNotExistsFormat1, selectedFile.FilePath);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            string fileText = File.ReadAllText(selectedFile.FilePath);

            if (!ContentCoparerHelper.TryParseXmlDocument(fileText, out var doc))
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.FileTextIsNotXmlFormat1, selectedFile.FilePath);

                WindowHelper.OpenSavedQueryWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            var attribute = doc.Root.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId);

            if (attribute == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData
                    , Properties.OutputStrings.FileNotContainsXmlAttributeFormat2
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId.ToString()
                    , selectedFile.FilePath
                );

                WindowHelper.OpenSavedQueryWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            if (string.IsNullOrEmpty(attribute.Value)
                || !Guid.TryParse(attribute.Value, out var savedQueryId)
                )
            {
                this._iWriteToOutput.WriteToOutput(connectionData
                    , Properties.OutputStrings.XmlAttributeNotValidGuidFormat3
                    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId.ToString()
                    , attribute.Value
                    , selectedFile.FilePath
                );

                WindowHelper.OpenSavedQueryWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            var repositorySavedQuery = new SavedQueryRepository(service);

            var savedQuery = await repositorySavedQuery.GetByIdAsync(savedQueryId, new ColumnSet(false));

            if (savedQuery == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.SavedQueryNotFoundedFormat2, connectionData.Name, savedQueryId);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);

                WindowHelper.OpenSavedQueryWindow(_iWriteToOutput, service, commonConfig);

                return;
            }

            service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQuery, savedQuery.Id);
        }

        #endregion SavedQuery
    }
}