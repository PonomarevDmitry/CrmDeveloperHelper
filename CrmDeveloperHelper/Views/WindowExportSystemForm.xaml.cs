using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportSystemForm : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private string _filterEntity;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private readonly Popup _optionsPopup;

        public static readonly XmlOptionsControls _xmlOptions = XmlOptionsControls.FormXml;

        public WindowExportSystemForm(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , string selection
            )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._filterEntity = filterEntity;


            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportXmlOptionsControl(_commonConfig, _xmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            LoadFromConfig();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            SetButtonClearFilterVisibility(_filterEntity, btnClearEntityFilter, sepClearEntityFilter);

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSystemForms.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

            if (service != null)
            {
                ShowExistingSystemForms();
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;

            txtBNamespaceClassesJavaScript.DataContext = cmBCurrentConnection;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

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
                    ToggleControls(connectionData, false, string.Empty);

                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    ToggleControls(connectionData, true, string.Empty);
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

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service);
                }

                _descriptorCache[connectionData.ConnectionId].SetSettings(_commonConfig);

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingSystemForms()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingForms);

            this._itemsSource.Clear();

            IEnumerable<SystemForm> list = Enumerable.Empty<SystemForm>();

            try
            {
                if (service != null)
                {
                    var repository = new SystemFormRepository(service);
                    list = await repository.GetListAsync(this._filterEntity
                        , new ColumnSet(
                            SystemForm.Schema.Attributes.name
                            , SystemForm.Schema.Attributes.objecttypecode
                            , SystemForm.Schema.Attributes.type
                            , SystemForm.Schema.Attributes.iscustomizable
                            , SystemForm.Schema.Attributes.formactivationstate
                        ));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadSystemForms(list);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingFormsCompletedFormat1, list.Count());
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

            public string FormState { get; private set; }

            public SystemForm SystemForm { get; private set; }

            public EntityViewItem(string entityName, string formName, string formType, string formState, SystemForm systemForm)
            {
                this.EntityName = entityName;
                this.FormName = formName;
                this.SystemForm = systemForm;
                this.FormType = formType;
                this.FormState = formState;
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
                    entity.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out var formType);
                    entity.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.formactivationstate, out var formState);

                    var item = new EntityViewItem(entity.ObjectTypeCode, entity.Name, formType, formState, entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSystemForms.Items.Count == 1)
                {
                    this.lstVwSystemForms.SelectedItem = this.lstVwSystemForms.Items[0];
                }
            });
        }

        private void UpdateStatus(ConnectionData connectionData, string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(connectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSystemForms.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSystemForms.SelectedItems.Count > 0;

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

        private async void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null)
                {
                    var service = await GetService();

                    if (service != null)
                    {
                        service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, item.SystemForm.Id);
                    }
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

            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await action(folder, idSystemForm, entityName, name);
        }

        private async Task ExecuteJavaScriptObjectTypeAsync(Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType, Func<string, Guid, string, string, JavaScriptObjectType, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await action(folder, idSystemForm, entityName, name, javaScriptObjectType);
        }

        private Task<string> CreateFileAsync(string folder, Guid formId, string entityName, string name, string fieldTitle, string extension, string formXml)
        {
            return Task.Run(() => CreateFile(folder, formId, entityName, name, fieldTitle, extension, formXml));
        }

        private string CreateFile(string folder, Guid formId, string entityName, string name, string fieldTitle, string extension, string formXml)
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

            string fileName = EntityFileNameFormatter.GetSystemFormFileName(connectionData.Name, entityName, name, fieldTitle, extension);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    //if (_commonConfig.SetXmlSchemasDuringExport)
                    //{
                    //    var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaFormXml);

                    //    if (schemasResources != null)
                    //    {
                    //        formXml = ContentCoparerHelper.SetXsdSchema(formXml, schemasResources);
                    //    }
                    //}

                    //if (_commonConfig.SetIntellisenseContext)
                    //{
                    //    formXml = ContentCoparerHelper.SetIntellisenseContextFormId(formXml, formId);
                    //}

                    //if (_commonConfig.SortXmlAttributes)
                    //{
                    //    formXml = ContentCoparerHelper.SortXmlAttributes(formXml);
                    //}

                    //formXml = ContentCoparerHelper.FormatXml(formXml, _commonConfig.ExportXmlAttributeOnNewLine);

                    formXml = ContentCoparerHelper.FormatXmlByConfiguration(formXml, _commonConfig, _xmlOptions
                       , schemaName: CommonExportXsdSchemasCommand.SchemaFormXml
                       , formId: formId
                       );

                    File.WriteAllText(filePath, formXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SystemForm.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SystemForm.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
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

            await PerformExportXmlToFileAsync(folder, idSystemForm, entityName, name, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, "xml");

            await PerformExportXmlToFileAsync(folder, idSystemForm, entityName, name, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, "json");
        }

        private async Task ExecuteActionEntityAsync(Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, string extension, Func<string, Guid, string, string, string, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await action(folder, idSystemForm, entityName, name, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportXmlToFileAsync(string folder, Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(fieldName));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, idSystemForm, entityName, name, fieldTitle, extension, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(fieldName));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var tempDoc))
                    {
                        xmlContent = tempDoc.ToString();
                    }
                }

                string filePath = await CreateFileAsync(folder, idSystemForm, entityName, name, fieldTitle + " BackUp", extension, xmlContent);

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
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml))
                {
                    newText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

                    UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.ValidatingXmlForFieldFormat1, fieldName);

                    if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
                    {
                        ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.TextIsNotValidXml);

                        _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                        return;
                    }

                    bool validateResult = await SystemFormRepository.ValidateXmlDocumentAsync(service.ConnectionData, _iWriteToOutput, doc);

                    if (!validateResult)
                    {
                        ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ValidatingXmlForFieldFailedFormat1, fieldName);

                        return;
                    }

                    newText = doc.ToString(SaveOptions.DisableFormatting);
                }

                var updateEntity = new SystemForm
                {
                    Id = idSystemForm
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                var repositoryPublish = new PublishActionsRepository(service);

                UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);
                await repositoryPublish.PublishDashboardsAsync(new[] { idSystemForm });

                if (!string.IsNullOrEmpty(entityName)
                    && !string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
                    await repositoryPublish.PublishEntitiesAsync(new[] { entityName });
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
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

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformEntityEditor);
        }

        private void mIChangeStateSystemForm_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformChangeStateSystemForm);
        }

        private void mIDeleteSystemForm_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformDeleteEntity);
        }

        private async Task PerformExportEntityDescriptionAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, entityName, name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, systemForm, EntityFileNameFormatter.SystemFormIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , systemForm.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private async Task PerformEntityEditor(string folder, Guid idSystemForm, string entityName, string name)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, SystemForm.EntityLogicalName, idSystemForm);
        }

        private async Task PerformChangeStateSystemForm(string folder, Guid idSystemForm, string entityName, string name)
        {
            var service = await GetService();

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

            int state = systemForm.FormActivationStateEnum == SystemForm.Schema.OptionSets.formactivationstate.Active_1 ? (int)SystemForm.Schema.OptionSets.formactivationstate.Inactive_0 : (int)SystemForm.Schema.OptionSets.formactivationstate.Active_1;

            try
            {
                var updateEntity = new Entity(SystemForm.EntityLogicalName)
                {
                    Id = idSystemForm,
                };

                updateEntity.Attributes[SystemForm.Schema.Attributes.formactivationstate] = new OptionSetValue(state);
                await service.UpdateAsync(updateEntity);

                var repositoryPublish = new PublishActionsRepository(service);

                UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);
                await repositoryPublish.PublishDashboardsAsync(new[] { idSystemForm });

                if (!string.IsNullOrEmpty(entityName)
                    && !string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
                    await repositoryPublish.PublishEntitiesAsync(new[] { entityName });
                }
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ShowExistingSystemForms();
        }

        private async Task PerformDeleteEntity(string folder, Guid idSystemForm, string entityName, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, SystemForm.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.DeletingEntitiesFormat2, service.ConnectionData.Name, SystemForm.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, SystemForm.EntityLogicalName, idSystemForm);

                    await service.DeleteAsync(SystemForm.EntityLogicalName, idSystemForm);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.DeletingEntitiesCompletedFormat2, service.ConnectionData.Name, SystemForm.EntityLogicalName);

                ShowExistingSystemForms();
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
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);

            var repository = new PublishActionsRepository(service);

            try
            {
                await repository.PublishDashboardsAsync(new[] { idSystemForm });

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingSystemFormCompletedFormat3, service.ConnectionData.Name, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingSystemFormFailedFormat3, service.ConnectionData.Name, entityName, name);
            }

            if (!string.IsNullOrEmpty(entityName)
                    && !string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                try
                {
                    await repository.PublishEntitiesAsync(new[] { entityName });

                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
                }
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);
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
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
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
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingSystemFormDescriptionFormat2, entityName, name);

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

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "FormDescription", filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "FormXml");
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingSystemFormDescriptionCompletedFormat2, entityName, name);
        }

        private void mIExportSystemFormFormXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, "xml", PerformExportXmlToFileAsync);
        }

        private void mIExportSystemFormFormJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, "json", PerformExportXmlToFileAsync);
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
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.DownloadingSystemFormWebResourcesFormat2, entityName, name);

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

                        var filePath = await CreateWebResourceAsync(folder, service.ConnectionData, resName, webresource);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            files.Add(filePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "FormXml");
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            foreach (var filePath in files)
            {
                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.DownloadingSystemFormWebResourcesCompletedFormat2, entityName, name);
        }

        private Task<string> CreateWebResourceAsync(string folder, ConnectionData connectionData, string resName, WebResource webresource)
        {
            return Task.Run(() => CreateWebResource(folder, connectionData, resName, webresource));
        }

        private string CreateWebResource(string folder, ConnectionData connectionData, string resName, WebResource webresource)
        {
            if (webresource == null)
            {
                return string.Empty;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, "Web-resource founded by name: {0}", resName);

            this._iWriteToOutput.WriteToOutput(connectionData, "Starting downloading {0}", webresource.Name);

            string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

            var contentWebResource = webresource.Content ?? string.Empty;

            var array = Convert.FromBase64String(contentWebResource);

            string localFileName = string.Format("{0}.{1}", connectionData.Name, webResourceFileName);
            string localFilePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(localFileName));

            File.WriteAllBytes(localFilePath, array);

            this._iWriteToOutput.WriteToOutput(connectionData, "Web-resource '{0}' has downloaded to {1}.", webresource.Name, localFilePath);

            return localFilePath;
        }

        private void mIExportSystemFormEntityJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.JsonObject, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private void mIExportSystemFormEntityJavaScriptFileAnonymousConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.AnonymousConstructor, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private void mIExportSystemFormEntityJavaScriptFileTypeConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.TypeConstructor, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private async Task PerformCreateEntityJavaScriptFileBasedOnForm(string folder, Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingEntityJavaScriptFileOnFormFormat2, entityName, name);

            var descriptor = await GetDescriptor();
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            string fileName = string.Format("{0}.{1}_form_main.js", service.ConnectionData.Name, entityName);

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
                        , tabSpacer
                        , _commonConfig.EntityMetadaOptionSetDependentComponents
                        , _commonConfig.GenerateIntoSchemaClass
                    );

                    XElement doc = XElement.Parse(formXml);

                    var tabs = handler.GetFormTabs(doc);

                    string filePath = Path.Combine(folder, fileName);

                    using (var handlerCreate = new CreateFormTabsJavaScriptHandler(config, javaScriptObjectType, service))
                    {
                        await handlerCreate.CreateFileAsync(filePath, tabs);
                    }

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "Entity Metadata", filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "FormXml");
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityJavaScriptFileOnFormCompletedFormat2, entityName, name);
        }

        private void btnClearEntityFilter_Click(object sender, RoutedEventArgs e)
        {
            this._filterEntity = null;

            SetButtonClearFilterVisibility(_filterEntity, btnClearEntityFilter, sepClearEntityFilter);

            ShowExistingSystemForms();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSystemForms();
            }

            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    if (_optionsPopup.IsOpen)
                    {
                        _optionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                }
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
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.SystemForm, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void AddIntoCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null, RootComponentBehavior.IncludeSubcomponents);
        }

        private async void AddIntoCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null, RootComponentBehavior.DoNotIncludeSubcomponents);
        }

        private async void AddIntoCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null, RootComponentBehavior.IncludeAsShellOnly);
        }

        private async void AddIntoCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName, RootComponentBehavior.IncludeSubcomponents);
            }
        }

        private async void AddIntoCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName, RootComponentBehavior.DoNotIncludeSubcomponents);
            }
        }

        private async void AddIntoCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName, RootComponentBehavior.IncludeAsShellOnly);
            }
        }

        private async Task AddEntityIntoSolution(bool withSelect, string solutionUniqueName, RootComponentBehavior rootComponentBehavior)
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
                        this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                        await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entityMetadataId.Value }, rootComponentBehavior, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
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

            FillLastSolutionItems(connectionData, items, hasEntity, AddIntoCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityIntoSolutionLastIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddIntoCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityIntoSolutionLastDoNotIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddIntoCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityIntoSolutionLastIncludeAsShellOnly");

            ActivateControls(items, hasEntity && connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityIntoSolutionLast");

            SetControlsName(items, GetChangeStateName(nodeItem.SystemForm), "contMnChangeState");
        }

        private string GetChangeStateName(SystemForm systemForm)
        {
            if (systemForm == null)
            {
                return "ChangeState";
            }

            return systemForm.FormActivationStateEnum == SystemForm.Schema.OptionSets.formactivationstate.Active_1 ? "Deactivate SystemForm" : "Activate SystemForm";
        }

        private void tSDDBExportSystemForm_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var systemForm = GetSelectedEntity();

            ActivateControls(tSDDBExportSystemForm.Items.OfType<Control>(), (systemForm?.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            SetControlsName(tSDDBExportSystemForm.Items.OfType<Control>(), GetChangeStateName(systemForm), "contMnChangeState");
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
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

        private async void miEntitySecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
        }

        private async void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
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

        private async void btnCompareApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.ObjectTypeCode, entity?.Name ?? txtBFilter.Text);
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
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (!this.IsControlsEnabled)
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

            ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, "xml", PerformUpdateEntityField);
        }

        private void mIUpdateSystemFormFormJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, "json", PerformUpdateEntityField);
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
                connectionData.OpenEntityInstanceListInWeb(entity.ObjectTypeCode);
            }
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
        {
            this._optionsPopup.IsOpen = true;
            this._optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
                this.Focus();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}