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
using System.Collections;
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
    public partial class WindowExplorerSystemForm : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly EnvDTE.SelectedItem _selectedItem;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private readonly Popup _optionsPopup;

        public WindowExplorerSystemForm(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , EnvDTE.SelectedItem selectedItem
            , string selection
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._selectedItem = selectedItem;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            LoadEntityNames(cmBEntityName, service.ConnectionData);

            cmBFormActivationState.ItemsSource = new EnumBindingSourceExtension(typeof(SystemForm.Schema.OptionSets.formactivationstate?)).ProvideValue(null) as IEnumerable;
            cmBFormActivationState.SelectedItem = SystemForm.Schema.OptionSets.formactivationstate.Active_1;

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.FormXmlOptions);
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

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            cmBEntityName.Text = filterEntity;

            if (this._selectedItem != null)
            {
                string exportFolder = string.Empty;

                if (_selectedItem.ProjectItem != null)
                {
                    exportFolder = _selectedItem.ProjectItem.FileNames[1];
                }
                else if (_selectedItem.Project != null)
                {
                    string relativePath = DTEHelper.GetRelativePath(_selectedItem.Project);

                    string solutionPath = Path.GetDirectoryName(_selectedItem.DTE.Solution.FullName);

                    exportFolder = Path.Combine(solutionPath, relativePath);
                }

                if (!Directory.Exists(exportFolder))
                {
                    Directory.CreateDirectory(exportFolder);
                }

                txtBFolder.IsReadOnly = true;
                txtBFolder.Background = SystemColors.InactiveSelectionHighlightBrush;
                txtBFolder.Text = exportFolder;
            }
            else
            {
                Binding binding = new Binding
                {
                    Path = new PropertyPath(nameof(CommonConfiguration.FolderForExport)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                };
                BindingOperations.SetBinding(txtBFolder, TextBox.TextProperty, binding);

                txtBFolder.DataContext = _commonConfig;
            }

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
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            FileGenerationConfiguration.SaveConfiguration();

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

            if (connectionData == null)
            {
                return null;
            }

            if (_connectionCache.ContainsKey(connectionData.ConnectionId))
            {
                return _connectionCache[connectionData.ConnectionId];
            }

            ToggleControls(connectionData, false, string.Empty);

            try
            {
                var service = await QuickConnection.ConnectAndWriteToOutputAsync(_iWriteToOutput, connectionData);

                if (service != null)
                {
                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return service;
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                ToggleControls(connectionData, true, string.Empty);
            }

            return null;
        }

        private SolutionComponentDescriptor GetDescriptor(IOrganizationServiceExtented service)
        {
            if (service != null)
            {
                if (!_descriptorCache.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service);
                }

                _descriptorCache[service.ConnectionData.ConnectionId].SetSettings(_commonConfig);

                return _descriptorCache[service.ConnectionData.ConnectionId];
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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingForms);

            this._itemsSource.Clear();

            string filterEntity = null;

            string entityName = string.Empty;
            SystemForm.Schema.OptionSets.formactivationstate? state = null;

            this.Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(cmBEntityName.Text)
                    && cmBEntityName.Items.Contains(cmBEntityName.Text)
                )
                {
                    entityName = cmBEntityName.Text.Trim().ToLower();
                }

                if (cmBFormActivationState.SelectedItem is SystemForm.Schema.OptionSets.formactivationstate comboBoxItem)
                {
                    state = comboBoxItem;
                }
            });

            if (service.ConnectionData.IsValidEntityName(entityName))
            {
                filterEntity = entityName;
            }

            IEnumerable<SystemForm> list = Enumerable.Empty<SystemForm>();

            try
            {
                if (service != null)
                {
                    var repository = new SystemFormRepository(service);

                    list = await repository.GetListAsync(filterEntity
                        , state
                        , new ColumnSet
                        (
                            SystemForm.Schema.Attributes.name
                            , SystemForm.Schema.Attributes.objecttypecode
                            , SystemForm.Schema.Attributes.type
                            , SystemForm.Schema.Attributes.iscustomizable
                            , SystemForm.Schema.Attributes.formactivationstate
                        )
                    );
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

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingFormsCompletedFormat1, list.Count());
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
                        return ent.ObjectTypeCode.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1 || ent.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string ObjectTypeCode => SystemForm.ObjectTypeCode;

            public string FormType { get; }

            public string Name => SystemForm.Name;

            public string FormActivationState { get; }

            public SystemForm SystemForm { get; }

            public EntityViewItem(SystemForm systemForm)
            {
                systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out var formType);
                systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.formactivationstate, out var formactivationstate);

                this.FormType = formType;
                this.FormActivationState = formactivationstate;

                this.SystemForm = systemForm;
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
                    var item = new EntityViewItem(entity);

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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingSystemForms();
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
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

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

        //private async Task PerformExportMouseDoubleClickAsync(string folder, Guid idSystemForm, string entityName, string name)
        //{
        //    await PerformExportFormDescriptionToFileAsync(folder, idSystemForm, entityName, name);
        //}

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteActionAsync(Guid idSystemForm, string entityName, string name, Func<string, Guid, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSystemForm, entityName, name);
        }

        private async Task ExecuteJavaScriptObjectTypeAsync(Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType, Func<string, Guid, string, string, JavaScriptObjectType, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

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
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSystemForm, entityName, name, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportXmlToFileAsync(string folder, Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(fieldName));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml, StringComparison.InvariantCultureIgnoreCase))
                {
                    xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                        xmlContent
                        , _commonConfig
                        , XmlOptionsControls.FormXmlOptions
                        , schemaName: AbstractDynamicCommandXsdSchemas.SchemaFormXml
                        , formId: idSystemForm
                        , entityName: entityName
                    );
                }
                else if (string.Equals(fieldName, SystemForm.Schema.Attributes.formjson, StringComparison.InvariantCultureIgnoreCase))
                {
                    xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                }

                string filePath = await CreateFileAsync(folder, idSystemForm, entityName, name, fieldTitle, extension, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(fieldName));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var tempDoc))
                    {
                        xmlContent = tempDoc.ToString();
                    }
                }
                else if (string.Equals(fieldName, SystemForm.Schema.Attributes.formjson, StringComparison.InvariantCultureIgnoreCase))
                {
                    xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                }

                {
                    string backUpXmlContent = xmlContent;

                    if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml, StringComparison.InvariantCultureIgnoreCase))
                    {
                        backUpXmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            backUpXmlContent
                            , _commonConfig
                            , XmlOptionsControls.FormXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.SchemaFormXml
                            , formId: idSystemForm
                            , entityName: entityName
                        );
                    }
                    else if (string.Equals(fieldName, SystemForm.Schema.Attributes.formjson, StringComparison.InvariantCultureIgnoreCase))
                    {
                        backUpXmlContent = ContentComparerHelper.FormatJson(backUpXmlContent);
                    }

                    await CreateFileAsync(folder, idSystemForm, entityName, name, fieldTitle + " BackUp", extension, backUpXmlContent);
                }

                var newText = string.Empty;

                {
                    bool? dialogResult = false;

                    this.Dispatcher.Invoke(() =>
                    {
                        var form = new WindowTextField("Enter " + fieldTitle, fieldTitle, xmlContent);

                        dialogResult = form.ShowDialog();

                        newText = form.FieldText;
                    });

                    if (dialogResult.GetValueOrDefault() == false)
                    {
                        ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                        return;
                    }
                }

                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml))
                {
                    newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, fieldName);

                    if (!ContentComparerHelper.TryParseXmlDocument(newText, out var doc))
                    {
                        ToggleControls(service.ConnectionData, true, Properties.OutputStrings.TextIsNotValidXml);

                        _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                        return;
                    }

                    bool validateResult = await SystemFormRepository.ValidateXmlDocumentAsync(service.ConnectionData, _iWriteToOutput, doc);

                    if (!validateResult)
                    {
                        var dialogResult = MessageBoxResult.Cancel;

                        this.Dispatcher.Invoke(() =>
                        {
                            dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        });

                        if (dialogResult != MessageBoxResult.OK)
                        {
                            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ValidatingXmlForFieldFailedFormat1, fieldName);
                            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                            return;
                        }
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

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);
                await repositoryPublish.PublishDashboardsAsync(new[] { idSystemForm });

                if (entityName.IsValidEntityName())
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

                    await repositoryPublish.PublishEntitiesAsync(new[] { entityName });
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

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

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionFailed);
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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ChangingEntityStateFormat1, SystemForm.EntityLogicalName);

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

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);
                await repositoryPublish.PublishDashboardsAsync(new[] { idSystemForm });

                if (entityName.IsValidEntityName())
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

                    try
                    {
                        await repositoryPublish.PublishEntitiesAsync(new[] { entityName });
                    }
                    catch (Exception ex)
                    {
                        _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateCompletedFormat1, SystemForm.EntityLogicalName);
            }
            catch (Exception ex)
            {
                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateFailedFormat1, SystemForm.EntityLogicalName);

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

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, SystemForm.EntityLogicalName);

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

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, SystemForm.EntityLogicalName);

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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);

            var repository = new PublishActionsRepository(service);

            try
            {
                await repository.PublishDashboardsAsync(new[] { idSystemForm });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingSystemFormCompletedFormat3, service.ConnectionData.Name, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingSystemFormFailedFormat3, service.ConnectionData.Name, entityName, name);
            }

            if (entityName.IsValidEntityName())
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

                try
                {
                    await repository.PublishEntitiesAsync(new[] { entityName });

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
                }
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);
        }

        private void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ObjectTypeCode.IsValidEntityName()
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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingSystemFormDescriptionFormat2, entityName, name);

            var descriptor = GetDescriptor(service);
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
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingSystemFormDescriptionCompletedFormat2, entityName, name);
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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DownloadingSystemFormWebResourcesFormat2, entityName, name);

            var descriptor = GetDescriptor(service);
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
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            foreach (var filePath in files)
            {
                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DownloadingSystemFormWebResourcesCompletedFormat2, entityName, name);
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

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityJavaScriptFileOnFormFormat2, entityName, name);

            var descriptor = GetDescriptor(service);
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

            SystemFormRepository.GetTypeName(systemForm.TypeEnum, out var formTypeName, out var formTypeConstructorName);

            string objectName = string.Format("{0}_form_{1}", entityName, formTypeName);
            string constructorName = string.Format("{0}Form{1}", entityName, formTypeConstructorName);

            string fileName = string.Format("{0}.{1}_form_{2}.js", service.ConnectionData.Name, entityName, formTypeName);

            if (this._selectedItem != null)
            {
                fileName = string.Format("{0}_form_{1}.js", entityName, formTypeName);
            }

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                    var config = new CreateFileJavaScriptConfiguration(
                        fileGenerationOptions.GetTabSpacer()
                        , fileGenerationOptions.GenerateSchemaEntityOptionSetsWithDependentComponents
                        , fileGenerationOptions.GenerateSchemaIntoSchemaClass
                        , fileGenerationOptions.GenerateSchemaGlobalOptionSet
                        , fileGenerationOptions.NamespaceClassesJavaScript
                    );

                    XElement doc = XElement.Parse(formXml);

                    var tabs = handler.GetFormTabs(doc);

                    string filePath = Path.Combine(folder, fileName);

                    if (this._selectedItem != null)
                    {
                        filePath = FileOperations.CheckFilePathUnique(filePath);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                        {
                            var handlerCreate = new CreateFormTabsJavaScriptHandler(streamWriter, config, javaScriptObjectType, service);

                            systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out string typeName);

                            await handlerCreate.WriteContentAsync(entityName, objectName, constructorName, tabs, systemForm.Id, systemForm.Name, systemForm.Type?.Value, typeName);

                            try
                            {
                                await streamWriter.FlushAsync();
                                await memoryStream.FlushAsync();

                                memoryStream.Seek(0, SeekOrigin.Begin);

                                var fileBody = memoryStream.ToArray();

                                File.WriteAllBytes(filePath, fileBody);
                            }
                            catch (Exception ex)
                            {
                                DTEHelper.WriteExceptionToOutput(service.ConnectionData, ex);
                            }
                        }
                    }

                    if (this._selectedItem != null)
                    {
                        if (_selectedItem.ProjectItem != null)
                        {
                            _selectedItem.ProjectItem.ProjectItems.AddFromFileCopy(filePath);

                            _selectedItem.ProjectItem.ContainingProject.Save();
                        }
                        else if (_selectedItem.Project != null)
                        {
                            _selectedItem.Project.ProjectItems.AddFromFile(filePath);

                            _selectedItem.Project.Save();
                        }
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
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityJavaScriptFileOnFormCompletedFormat2, entityName, name);
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingSystemForms();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            Popup[] _popupArray = new Popup[] { _optionsPopup };

            foreach (var popup in _popupArray)
            {
                if (popup.IsOpen)
                {
                    popup.IsOpen = false;
                    e.Handled = true;

                    return false;
                }
            }

            return true;
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

        private async void AddToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null);
        }

        private async void AddToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddToSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = GetDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.SystemForm, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void AddToCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void AddToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void AddToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async Task AddEntityToSolution(bool withSelect, string solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ObjectTypeCode.IsValidEntityName()
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
                    var descriptor = GetDescriptor(service);

                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                        await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entityMetadataId.Value }, rootComponentBehavior, withSelect);
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

            FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            EntityViewItem nodeItem = GetItemFromRoutedDataContext<EntityViewItem>(e);

            ActivateControls(items, (nodeItem.SystemForm.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            bool hasEntity = nodeItem.SystemForm.ObjectTypeCode.IsValidEntityName();
            ActivateControls(items, hasEntity, "contMnEntity");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastDoNotIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityToSolutionLastIncludeAsShellOnly");

            ActivateControls(items, hasEntity && connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityToSolutionLast");

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

        private async void btnEntityMetadataExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
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

        private async void miEntityPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
        }

        private async void miOtherPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOtherPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig);
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

            WindowHelper.OpenApplicationRibbonExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTree(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty, string.Empty);
        }

        private async void btnMessageFilterTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageFilterTree(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, string.Empty);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTree(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode);
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

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = GetDescriptor(service);

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , descriptor
                , _commonConfig
                , (int)ComponentType.SystemForm
                , entity.Id
                , null
                );
        }

        private async void mIOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionExplorer(
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
                || !entity.ObjectTypeCode.IsValidEntityName()
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
                || !entity.ObjectTypeCode.IsValidEntityName()
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

        private void mISystemFormCopyJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.JsonObject, PerformCopyBasedOnForm);
        }

        private void mISystemFormCopyConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.TypeConstructor, PerformCopyBasedOnForm);
        }

        private async Task PerformCopyBasedOnForm(string folder, Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormFormat2, entityName, name);

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(SystemForm.Schema.Attributes.formxml));

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                    var config = new CreateFileJavaScriptConfiguration(
                        fileGenerationOptions.GetTabSpacer()
                        , fileGenerationOptions.GenerateSchemaEntityOptionSetsWithDependentComponents
                        , fileGenerationOptions.GenerateSchemaIntoSchemaClass
                        , fileGenerationOptions.GenerateSchemaGlobalOptionSet
                        , fileGenerationOptions.NamespaceClassesJavaScript
                    );

                    var doc = XElement.Parse(formXml);

                    var descriptor = GetDescriptor(service);
                    var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

                    var tabs = handler.GetFormTabs(doc);

                    var text = new StringBuilder();

                    using (var writer = new StringWriter(text))
                    {
                        var handlerCreate = new CreateFormTabsJavaScriptHandler(writer, config, javaScriptObjectType, service);

                        handlerCreate.WriteContentOnlyForm(tabs);
                    }

                    ClipboardHelper.SetText(text.ToString().Trim(' ', '\r', '\n'));

                    //this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "Entity Metadata", filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormCompletedFormat2, entityName, name);
        }

        private void mISystemFormCopyFormProperties_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.TypeConstructor, PerformCopyFormPropertiesBasedOnForm);
        }

        private async Task PerformCopyFormPropertiesBasedOnForm(string folder, Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormFormat2, entityName, name);

            var descriptor = GetDescriptor(service);
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                    var config = new CreateFileJavaScriptConfiguration(
                        fileGenerationOptions.GetTabSpacer()
                        , fileGenerationOptions.GenerateSchemaEntityOptionSetsWithDependentComponents
                        , fileGenerationOptions.GenerateSchemaIntoSchemaClass
                        , fileGenerationOptions.GenerateSchemaGlobalOptionSet
                        , fileGenerationOptions.NamespaceClassesJavaScript
                    );

                    var text = new StringBuilder();

                    using (var writer = new StringWriter(text))
                    {
                        var handlerCreate = new CreateFormTabsJavaScriptHandler(writer, config, javaScriptObjectType, service);

                        systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out string typeName);

                        handlerCreate.WriteFormProperties(systemForm.ObjectTypeCode, systemForm.Id, systemForm.Name, systemForm.Type?.Value, typeName);
                    }

                    ClipboardHelper.SetText(text.ToString().Trim(' ', '\r', '\n'));

                    //this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "Entity Metadata", filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormCompletedFormat2, entityName, name);
        }

        private void hyperlinkFormDescription_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformExportFormDescriptionToFileAsync);
        }

        private void hyperlinkFormXml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, "xml", PerformExportXmlToFileAsync);
        }

        private void hyperlinkFormJson_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, "json", PerformExportXmlToFileAsync);
        }

        private void hyperlinkJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.JsonObject, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private void hyperlinkJavaScriptFileAnonymousConstructor_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.AnonymousConstructor, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private void hyperlinkJavaScriptFileTypeConstructor_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.TypeConstructor, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private void hyperlinkPublishSystemForm_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformPublishSystemFormAsync);
        }

        private void hyperlinkPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            if (entity == null
                || !entity.ObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformPublishEntityAsync);
        }

        private async void btnEntityMetadataExplorerSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ObjectTypeCode, _selectedItem);
        }

        private void lstVwSystemForms_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.ContinueRouting = false;
        }

        private void lstVwSystemForms_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteSystemForm_Click(sender, e);
        }
    }
}