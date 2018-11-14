using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportSystemForm : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private string _filterEntity;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private int _init = 0;

        public WindowExportSystemForm(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , string selection
            )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;
            this._filterEntity = filterEntity;

            var descriptor = new SolutionComponentDescriptor(service, true);
            var formDescriptor = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = descriptor;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            if (string.IsNullOrEmpty(_filterEntity))
            {
                btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
                btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSystemForms.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (service != null)
            {
                ShowExistingSystemForms();
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;

            chBSetXmlSchemas.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private async Task<IOrganizationServiceExtented> GetService()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_connectionCache.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task<SolutionComponentDescriptor> GetDescriptor()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_descriptorCache.ContainsKey(connectionData.ConnectionId))
                {
                    var service = await GetService();

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);
                }

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingSystemForms()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingForms);

            this._itemsSource.Clear();

            IEnumerable<SystemForm> list = Enumerable.Empty<SystemForm>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new SystemFormRepository(service);
                    list = await repository.GetListAsync(this._filterEntity, new ColumnSet(SystemForm.Schema.Attributes.name, SystemForm.Schema.Attributes.objecttypecode, SystemForm.Schema.Attributes.type, SystemForm.Schema.Attributes.iscustomizable));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadSystemForms(list);

            ToggleControls(true, Properties.WindowStatusStrings.LoadingFormsCompletedFormat, list.Count());
        }

        private static IEnumerable<SystemForm> FilterList(IEnumerable<SystemForm> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.Id == tempGuid || ent.FormIdUnique == tempGuid);
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var type = ent.ObjectTypeCode.ToLower();
                        var name = ent.Name.ToLower();

                        return type.Contains(textName) || name.Contains(textName);
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string EntityName { get; private set; }

            public string FormType { get; private set; }

            public string FormName { get; private set; }

            public SystemForm SystemForm { get; private set; }

            public EntityViewItem(string entityName, string formName, string formType, SystemForm systemForm)
            {
                this.EntityName = entityName;
                this.FormName = formName;
                this.SystemForm = systemForm;
                this.FormType = formType;
            }
        }

        private void LoadSystemForms(IEnumerable<SystemForm> results)
        {
            this.lstVwSystemForms.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.ObjectTypeCode)
                    .ThenBy(ent => ent.Type?.Value)
                    .ThenBy(ent => ent.Name)
                )
                {
                    var item = new EntityViewItem(entity.ObjectTypeCode, entity.Name, entity.FormattedValues[SystemForm.Schema.Attributes.type], entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSystemForms.Items.Count == 1)
                {
                    this.lstVwSystemForms.SelectedItem = this.lstVwSystemForms.Items[0];
                }
            });
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(cmBCurrentConnection, enabled);

            ToggleProgressBar(enabled);

            UpdateButtonsEnable();
        }

        private void ToggleProgressBar(bool enabled)
        {
            if (tSProgressBar == null)
            {
                return;
            }

            this.tSProgressBar.Dispatcher.Invoke(() =>
            {
                tSProgressBar.IsIndeterminate = !enabled;
            });
        }

        private void ToggleControl(Control c, bool enabled)
        {
            c.Dispatcher.Invoke(() =>
            {
                if (c is TextBox)
                {
                    ((TextBox)c).IsReadOnly = !enabled;
                }
                else
                {
                    c.IsEnabled = enabled;
                }
            });
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSystemForms.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwSystemForms.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportSystemForm, btnExportAll };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSystemForms();
            }
        }

        private SystemForm GetSelectedEntity()
        {
            return this.lstVwSystemForms.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwSystemForms.SelectedItems.OfType<EntityViewItem>().Select(e => e.SystemForm).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null)
                {
                    ExecuteActionAsync(item.SystemForm.Id, item.SystemForm.ObjectTypeCode, item.SystemForm.Name, PerformExportMouseDoubleClickAsync);
                }
            }
        }

        private async Task PerformExportMouseDoubleClickAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            await PerformExportFormDescriptionToFileAsync(folder, idSystemForm, entityName, name);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteActionAsync(Guid idSystemForm, string entityName, string name, Func<string, Guid, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, idSystemForm, entityName, name);
        }

        private Task<string> CreateFileAsync(string folder, string entityName, string name, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, entityName, name, fieldTitle, xmlContent));
        }

        private string CreateFile(string folder, string entityName, string name, string fieldTitle, string xmlContent)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetSystemFormFileName(connectionData.Name, entityName, name, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "FormXml", StringComparison.InvariantCultureIgnoreCase));

                        if (schemasResources != null)
                        {
                            string schemas = ContentCoparerHelper.HandleExportXsdSchemaIntoSchamasFolder(schemasResources.Item2);

                            if (!string.IsNullOrEmpty(schemas))
                            {
                                xmlContent = ContentCoparerHelper.ReplaceXsdSchema(xmlContent, schemas);
                            }
                        }
                    }

                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput("{0} SystemForm {1} {2} exported to {3}", connectionData.Name, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("SystemForm {0} {1} is empty.", name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            return filePath;
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformExportAllXmlAsync);
        }

        private async Task PerformExportAllXmlAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            await PerformExportEntityDescriptionAsync(folder, idSystemForm, entityName, name);

            await PerformExportFormDescriptionToFileAsync(folder, idSystemForm, entityName, name);

            await PerformExportXmlToFileAsync(folder, idSystemForm, entityName, name, SystemForm.Schema.Attributes.formxml, "FormXml");
        }

        private async Task ExecuteActionEntityAsync(Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, idSystemForm, entityName, name, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFileAsync(string folder, Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat, fieldTitle);

            try
            {

                var service = await GetService();

                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(fieldName));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, entityName, name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileFailedFormat, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingFieldFormat, fieldName);

            try
            {
                var service = await GetService();

                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(fieldName));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var tempDoc))
                    {
                        xmlContent = tempDoc.ToString();
                    }
                }

                string filePath = await CreateFileAsync(folder, entityName, name, fieldTitle + " BackUp", xmlContent);

                var newText = string.Empty;

                bool? dialogResult = false;

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + fieldTitle, fieldTitle, xmlContent);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldCanceledFormat, fieldName);
                    return;
                }

                ContentCoparerHelper.ClearXsdSchema(newText, out newText);
                
                UpdateStatus(Properties.WindowStatusStrings.ValidatingXmlForFieldFormat, fieldName);

                if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
                {
                    ToggleControls(true, Properties.WindowStatusStrings.TextIsNotValidXml);

                    _iWriteToOutput.ActivateOutputWindow();
                    return;
                }

                bool validateResult = await ValidateXmlDocumentAsync(doc);

                if (!validateResult)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.ValidatingXmlForFieldFailedFormat, fieldName);

                    return;
                }

                newText = doc.ToString(SaveOptions.DisableFormatting);

                var updateEntity = new SystemForm
                {
                    Id = idSystemForm
                };
                updateEntity.Attributes[fieldName] = newText;

                service.Update(updateEntity);

                UpdateStatus(Properties.WindowStatusStrings.PublishingSystemFormFormat, entityName, name);

                {
                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishDashboardsAsync(new[] { idSystemForm });
                }

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat, fieldName);
            }
        }

        private Task<bool> ValidateXmlDocumentAsync(XDocument doc)
        {
            return Task.Run(() => ValidateXmlDocument(doc));
        }

        private bool ValidateXmlDocument(XDocument doc)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            {
                var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "FormXml", StringComparison.InvariantCultureIgnoreCase));

                if (schemasResources != null)
                {
                    foreach (var fileName in schemasResources.Item2)
                    {
                        Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        using (StreamReader reader = new StreamReader(info.Stream))
                        {
                            schemas.Add("", XmlReader.Create(reader));
                        }
                    }
                }
            }

            List<ValidationEventArgs> errors = new List<ValidationEventArgs>();

            doc.Validate(schemas, (o, e) =>
            {
                errors.Add(e);
            });

            if (errors.Count > 0)
            {
                _iWriteToOutput.WriteToOutput("FormXml is not valid.");

                foreach (var item in errors)
                {
                    _iWriteToOutput.WriteToOutput(string.Empty);
                    _iWriteToOutput.WriteToOutput(string.Empty);
                    _iWriteToOutput.WriteToOutput("Severity: {0}      Message: {1}", item.Severity, item.Message);
                    _iWriteToOutput.WriteErrorToOutput(item.Exception);
                }

                _iWriteToOutput.ActivateOutputWindow();
            }

            return errors.Count == 0;
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformExportEntityDescriptionAsync);
        }

        private async Task PerformExportEntityDescriptionAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                var service = await GetService();

                string fileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, entityName, name, "EntityDescription", "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, systemForm, EntityFileNameFormatter.SystemFormIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput("SystemForm Entity Description exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("End creating file at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private void mIPublishSystemForm_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformPublishSystemFormAsync);
        }

        private async Task PerformPublishSystemFormAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }
            
            ToggleControls(false, Properties.WindowStatusStrings.PublishingSystemFormFormat, entityName, name);

            this._iWriteToOutput.WriteToOutput("Start publishing SystemForm {0} - {1} at {2}", entityName, name, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishDashboardsAsync(new[] { idSystemForm });

                this._iWriteToOutput.WriteToOutput("End publishing SystemForm {0} - {1} at {2}", entityName, name, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                ToggleControls(true, Properties.WindowStatusStrings.PublishingSystemFormCompletedFormat, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingSystemFormFailedFormat, entityName, name);
            }
        }

        private void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                && !string.IsNullOrEmpty(entity.ObjectTypeCode)
                && !string.Equals(entity.ObjectTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformPublishEntityAsync);
        }

        private async Task PerformPublishEntityAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.PublishingEntitiesFormat, entityName);

            this._iWriteToOutput.WriteToOutput("Start publishing Entity {0} at {1}", entityName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                this._iWriteToOutput.WriteToOutput("End publishing Entity {0} at {1}", entityName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat, entityName);
            }
        }

        private void mIExportSystemFormDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformExportFormDescriptionToFileAsync);
        }

        private async Task PerformExportFormDescriptionToFileAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            ToggleControls(false, Properties.WindowStatusStrings.CreatingSystemFormDescriptionFormat, entityName, name);
            
            var service = await GetService();
            var descriptor = await GetDescriptor();
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            string fileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, entityName, name, "FormDescription", "txt");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(SystemForm.Schema.Attributes.type, SystemForm.Schema.Attributes.formxml));

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    XElement doc = XElement.Parse(formXml);

                    string desc = await handler.GetFormDescriptionAsync(doc, entityName, idSystemForm, name, systemForm.FormattedValues[SystemForm.Schema.Attributes.type]);

                    File.WriteAllText(filePath, desc, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput("SystemForm {0} Form Description exported to {0}", name, filePath);

                    this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("SystemForm {0} FormXml is empty.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            this._iWriteToOutput.WriteToOutput("End creating file at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            
            ToggleControls(true, Properties.WindowStatusStrings.CreatingSystemFormDescriptionCompletedFormat, entityName, name);
        }

        private void mIExportSystemFormFormXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formxml, "FormXml", PerformExportXmlToFileAsync);
        }

        private void mIExportSystemFormWebResources_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformDownloadWebResources);
        }

        private async Task PerformDownloadWebResources(string folder, Guid idSystemForm, string entityName, string name)
        {
            ToggleControls(false, Properties.WindowStatusStrings.DownloadingSystemFormWebResourcesFormat, entityName, name);
            
            var service = await GetService();
            var descriptor = await GetDescriptor();
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(SystemForm.Schema.Attributes.formxml));

            string formXml = systemForm.FormXml;

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            List<string> files = new List<string>();

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    XElement doc = XElement.Parse(formXml);

                    List<string> webResources = handler.GetFormLibraries(doc);

                    foreach (var resName in webResources)
                    {
                        var webresource = await webResourceRepository.FindByNameAsync(resName, ".js");

                        var filePath = await CreateWebResourceAsync(folder, service.ConnectionData.Name, resName, webresource);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            files.Add(filePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("System Form {0} FormXml is empty.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            foreach (var filePath in files)
            {
                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            
            ToggleControls(true, Properties.WindowStatusStrings.DownloadingSystemFormWebResourcesCompletedFormat, entityName, name);
        }

        private Task<string> CreateWebResourceAsync(string folder, string connectionName, string resName, WebResource webresource)
        {
            return Task.Run(() => CreateWebResource(folder, connectionName, resName, webresource));
        }

        private string CreateWebResource(string folder, string connectionName, string resName, WebResource webresource)
        {
            if (webresource == null)
            {
                return string.Empty;
            }

            this._iWriteToOutput.WriteToOutput("Web-resource founded by name: {0}", resName);

            this._iWriteToOutput.WriteToOutput("Starting downloading {0}", webresource.Name);

            string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

            var contentWebResource = webresource.Content ?? string.Empty;

            var array = Convert.FromBase64String(contentWebResource);

            string localFileName = string.Format("{0}.{1}", connectionName, webResourceFileName);
            string localFilePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(localFileName));

            File.WriteAllBytes(localFilePath, array);

            this._iWriteToOutput.WriteToOutput("Web-resource '{0}' has downloaded to {1}.", webresource.Name, localFilePath);

            return localFilePath;
        }

        private void mIExportSystemFormEntityJavaScriptFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private async Task PerformCreateEntityJavaScriptFileBasedOnForm(string folder, Guid idSystemForm, string entityName, string name)
        {
            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityJavaScriptFileOnFormFormat, entityName, name);
            
            var service = await GetService();
            var descriptor = await GetDescriptor();
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            string fileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, entityName, name, "EntityMetadata", "js");

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(SystemForm.Schema.Attributes.formxml));

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    _commonConfig.Save();

                    string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

                    var config = new CreateFileWithEntityMetadataJavaScriptConfiguration(
                        entityName
                        , folder
                        , tabSpacer
                        , false
                        );

                    XElement doc = XElement.Parse(formXml);

                    var tabs = handler.GetFormTabs(doc);

                    string filePath = string.Empty;

                    using (var handlerCreate = new CreateFormTabsJavaScriptHandler(config, service))
                    {
                        filePath = await handlerCreate.CreateFileAsync(fileName, tabs);
                    }

                    this._iWriteToOutput.WriteToOutput("System Form {0} Entity Metadata exported to {1}", name, filePath);

                    this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("System Form {0} FormXml is empty.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            this._iWriteToOutput.WriteToOutput("End creating file at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityJavaScriptFileOnFormCompletedFormat, entityName, name);
        }

        private void btnClearEntityFilter_Click(object sender, RoutedEventArgs e)
        {
            this._filterEntity = null;

            btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
            btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;

            ShowExistingSystemForms();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSystemForms();
            }

            base.OnKeyDown(e);
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SystemForm, entity.Id);
            }
        }

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, entity.Id);
            }
        }

        private async void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddIntoSolution(true, null);
        }

        private async void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.SystemForm, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private async void mIAddEntityIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null);
        }

        private async void mIAddEntityIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddEntityIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.ObjectTypeCode)
                || string.Equals(entity.ObjectTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var entityMetadataId = connectionData.GetEntityMetadataId(entity.ObjectTypeCode);

                if (entityMetadataId.HasValue)
                {
                    _commonConfig.Save();

                    var service = await GetService();
                    var descriptor = await GetDescriptor();

                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entityMetadataId.Value }, null, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                }
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            ActivateControls(items, (nodeItem.SystemForm.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
            
            bool hasEntity = !string.IsNullOrEmpty(nodeItem.SystemForm.ObjectTypeCode) && !string.Equals(nodeItem.SystemForm.ObjectTypeCode, "none", StringComparison.InvariantCultureIgnoreCase);
            ActivateControls(items, hasEntity, "contMnEntity");
            FillLastSolutionItems(connectionData, items, hasEntity, mIAddEntityIntoCrmSolutionLast_Click, "contMnAddEntityIntoSolutionLast");
        }

        private void tSDDBExportSystemForm_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            ActivateControls(tSDDBExportSystemForm.Items.OfType<Control>(), (nodeItem.SystemForm.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, null, null);
        }

        private async void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
        }

        private async void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
        }

        private async void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
        }

        private async void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
        }

        private async void btnExportRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRibbonWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, null);
        }

        private async void btnAttributesDependentComponent_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenAttributesDependentComponentWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, null);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty, string.Empty);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.ObjectTypeCode);
        }

        private async void btnCompareRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.ObjectTypeCode);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.ObjectTypeCode);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.ObjectTypeCode);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.ObjectTypeCode);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.ObjectTypeCode);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.ObjectTypeCode);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private async void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , service
                , descriptor
                , _commonConfig
                , (int)ComponentType.SystemForm
                , entity.Id
                , null
                );
        }

        private async void mIOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.SystemForm
                , entity.Id
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ShowExistingSystemForms();
            }
        }

        private void mIUpdateSystemFormFormXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formxml, "FormXml", PerformUpdateEntityField);
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.ObjectTypeCode)
                || string.Equals(entity.ObjectTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.ObjectTypeCode);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.ObjectTypeCode)
                || string.Equals(entity.ObjectTypeCode, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityListInWeb(entity.ObjectTypeCode);
            }
        }
    }
}