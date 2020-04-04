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
    public partial class WindowExplorerCustomControl : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private readonly Popup _optionsPopup;

        public WindowExplorerCustomControl(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.CustomControlXmlOptions);
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

            if (!string.IsNullOrEmpty(filter))
            {
                txtBFilter.Text = filter;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwCustomControls.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

            if (service != null)
            {
                ShowExistingCustomControls();
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
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

        private async Task ShowExistingCustomControls()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingForms);

            this._itemsSource.Clear();

            IEnumerable<CustomControl> list = Enumerable.Empty<CustomControl>();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            try
            {
                if (service != null)
                {
                    var repository = new CustomControlRepository(service);
                    list = await repository.GetListAsync(textName
                        , new ColumnSet
                        (
                            CustomControl.Schema.Attributes.customcontrolid
                            , CustomControl.Schema.Attributes.name
                            , CustomControl.Schema.Attributes.compatibledatatypes
                            , CustomControl.Schema.Attributes.ismanaged
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadCustomControls(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingFormsCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string Name => CustomControl.Name;

            public string CompatibleDataTypes => CustomControl.CompatibleDataTypes;

            public bool IsManaged => CustomControl.IsManaged.GetValueOrDefault();

            public CustomControl CustomControl { get; private set; }

            public EntityViewItem(CustomControl CustomControl)
            {
                this.CustomControl = CustomControl;
            }
        }

        private void LoadCustomControls(IEnumerable<CustomControl> results)
        {
            this.lstVwCustomControls.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.Name)
                    .ThenBy(ent => ent.CompatibleDataTypes)
                )
                {
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwCustomControls.Items.Count == 1)
                {
                    this.lstVwCustomControls.SelectedItem = this.lstVwCustomControls.Items[0];
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
            this.lstVwCustomControls.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwCustomControls.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportCustomControl, btnExportAll };

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
                ShowExistingCustomControls();
            }
        }

        private CustomControl GetSelectedEntity()
        {
            return this.lstVwCustomControls.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwCustomControls.SelectedItems.OfType<EntityViewItem>().Select(e => e.CustomControl).SingleOrDefault() : null;
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
                        service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.CustomControl, item.CustomControl.Id);
                    }
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteActionAsync(Guid idCustomControl, string name, Func<string, Guid, string, Task> action)
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

            await action(folder, idCustomControl, name);
        }

        private Task<string> CreateFileAsync(string folder, Guid idCustomControl, string name, string fieldTitle, string extension, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, idCustomControl, name, fieldTitle, extension, xmlContent));
        }

        private string CreateFile(string folder, Guid idCustomControl, string name, string fieldTitle, string extension, string xmlContent)
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

            string fileName = EntityFileNameFormatter.GetCustomControlFileName(connectionData.Name, name, fieldTitle, extension);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, CustomControl.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, CustomControl.Schema.EntityLogicalName, name, fieldTitle);
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

            ExecuteActionAsync(entity.Id, entity.Name, PerformExportAllXmlAsync);
        }

        private async Task PerformExportAllXmlAsync(string folder, Guid idCustomControl, string name)
        {
            await PerformExportEntityDescriptionAsync(folder, idCustomControl, name);

            await PerformExportXmlToFileAsync(folder, idCustomControl, name, CustomControl.Schema.Attributes.manifest, CustomControl.Schema.Headers.manifest, "xml");

            await PerformExportXmlToFileAsync(folder, idCustomControl, name, CustomControl.Schema.Attributes.clientjson, CustomControl.Schema.Headers.clientjson, "json");
        }

        private async Task ExecuteActionEntityAsync(Guid idCustomControl, string name, string fieldName, string fieldTitle, string extension, Func<string, Guid, string, string, string, string, Task> action)
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

            await action(folder, idCustomControl, name, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportXmlToFileAsync(string folder, Guid idCustomControl, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new CustomControlRepository(service);

                var customControl = await repository.GetByIdAsync(idCustomControl, new ColumnSet(fieldName));

                string xmlContent = customControl.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent))
                {
                    if (string.Equals(fieldName, CustomControl.Schema.Attributes.manifest, StringComparison.InvariantCultureIgnoreCase))
                    {
                        xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            xmlContent
                            , _commonConfig
                            , XmlOptionsControls.CustomControlXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.SchemaManifest
                            , customControlId: idCustomControl
                        );
                    }
                    else if (string.Equals(fieldName, CustomControl.Schema.Attributes.clientjson, StringComparison.InvariantCultureIgnoreCase))
                    {
                        xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                    }
                }

                string filePath = await CreateFileAsync(folder, idCustomControl, name, fieldTitle, extension, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idCustomControl, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new CustomControlRepository(service);

                var customControl = await repository.GetByIdAsync(idCustomControl, new ColumnSet(fieldName));

                string xmlContent = customControl.GetAttributeValue<string>(fieldName);

                if (string.Equals(fieldName, CustomControl.Schema.Attributes.manifest, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var tempDoc))
                    {
                        xmlContent = tempDoc.ToString();
                    }
                }
                else if (string.Equals(fieldName, CustomControl.Schema.Attributes.clientjson, StringComparison.InvariantCultureIgnoreCase))
                {
                    xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                }

                {
                    string backUpXmlContent = xmlContent;

                    if (string.Equals(fieldName, CustomControl.Schema.Attributes.manifest, StringComparison.InvariantCultureIgnoreCase))
                    {
                        backUpXmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            backUpXmlContent
                            , _commonConfig
                            , XmlOptionsControls.CustomControlXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.SchemaManifest
                            , customControlId: idCustomControl
                        );
                    }
                    else if (string.Equals(fieldName, CustomControl.Schema.Attributes.clientjson, StringComparison.InvariantCultureIgnoreCase))
                    {
                        backUpXmlContent = ContentComparerHelper.FormatJson(backUpXmlContent);
                    }

                    await CreateFileAsync(folder, idCustomControl, name, fieldTitle + " BackUp", extension, backUpXmlContent);
                }

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
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                newText = ContentComparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, fieldName);

                if (!ContentComparerHelper.TryParseXmlDocument(newText, out var doc))
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.TextIsNotValidXml);

                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }

                newText = doc.ToString(SaveOptions.DisableFormatting);

                var updateEntity = new CustomControl
                {
                    Id = idCustomControl
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

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

            ExecuteActionAsync(entity.Id, entity.Name, PerformExportEntityDescriptionAsync);
        }

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.Name, PerformEntityEditor);
        }

        private void mIDeleteCustomControl_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity.Id, entity.Name, PerformDeleteEntity);
        }

        private async Task PerformExportEntityDescriptionAsync(string folder, Guid idCustomControl, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetCustomControlFileName(service.ConnectionData.Name, name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new CustomControlRepository(service);

                var customControl = await repository.GetByIdAsync(idCustomControl, new ColumnSet(true));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, customControl, EntityFileNameFormatter.CustomControlIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , customControl.LogicalName
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

        private async Task PerformEntityEditor(string folder, Guid idCustomControl, string name)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, CustomControl.EntityLogicalName, idCustomControl);
        }

        private async Task PerformDeleteEntity(string folder, Guid idCustomControl, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, CustomControl.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, CustomControl.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, CustomControl.EntityLogicalName, idCustomControl);

                    await service.DeleteAsync(CustomControl.EntityLogicalName, idCustomControl);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, CustomControl.EntityLogicalName);

                ShowExistingCustomControls();
            }
        }

        private void mIExportCustomControlManifest_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.manifest, CustomControl.Schema.Headers.manifest, "xml", PerformExportXmlToFileAsync);
        }

        private void mIExportCustomControlClientJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.clientjson, CustomControl.Schema.Headers.clientjson, "json", PerformExportXmlToFileAsync);
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingCustomControls();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.CustomControl, entity.Id);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.CustomControl, entity.Id);
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

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.CustomControl, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
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
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miEntityPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig);
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

        private async void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
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
                , (int)ComponentType.CustomControl
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
                , (int)ComponentType.CustomControl
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
                ShowExistingCustomControls();
            }
        }

        private void mIUpdateCustomControlManifest_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.manifest, CustomControl.Schema.Headers.manifest, "xml", PerformUpdateEntityField);
        }

        private void mIUpdateCustomControlClientJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.clientjson, CustomControl.Schema.Headers.clientjson, "json", PerformUpdateEntityField);
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

        private void hyperlinkManifest_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.CustomControl;

            ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.manifest, CustomControl.Schema.Headers.manifest, "xml", PerformExportXmlToFileAsync);
        }

        private void hyperlinkClientJson_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.CustomControl;

            ExecuteActionEntityAsync(entity.Id, entity.Name, CustomControl.Schema.Attributes.clientjson, CustomControl.Schema.Headers.clientjson, "json", PerformExportXmlToFileAsync);
        }

        private void lstVwCustomControls_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.ContinueRouting = false;
        }

        private void lstVwCustomControls_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteCustomControl_Click(sender, e);
        }
    }
}