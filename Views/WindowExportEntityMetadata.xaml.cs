using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
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

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportEntityMetadata : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private Popup _optionsPopup;

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        public string _filePath;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityMetadataListViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();
        private Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();

        private int _init = 0;

        public WindowExportEntityMetadata(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , IEnumerable<EntityMetadata> allEntities
            , string filePath
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;
            this._filePath = filePath;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);

            if (allEntities != null)
            {
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = allEntities;
            }

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportEntityMetadataOptionsControl(_commonConfig);
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

            txtBFilterEnitity.Text = filterEntity;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            _itemsSource = new ObservableCollection<EntityMetadataListViewItem>();

            lstVwEntities.ItemsSource = _itemsSource;

            UpdateButtonsEnable();

            if (!string.IsNullOrEmpty(_filePath))
            {
                txtBFolder.IsReadOnly = true;
                txtBFolder.Background = SystemColors.InactiveSelectionHighlightBrush;
                txtBFolder.Text = Path.GetDirectoryName(_filePath);
            }

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (allEntities != null)
            {
                var list = allEntities.AsEnumerable();

                list = FilterList(list, filterEntity);

                LoadEntities(list);
            }
            else if (service != null)
            {
                ShowExistingEntities();
            }
        }

        private void LoadFromConfig()
        {
            txtBNameSpace.DataContext = cmBCurrentConnection;

            cmBFileAction.DataContext = _commonConfig;

            if (string.IsNullOrEmpty(_filePath))
            {
                txtBFolder.DataContext = _commonConfig;
            }
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
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

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

        private async Task ShowExistingEntities()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingEntities);

            _itemsSource.Clear();

            IEnumerable<EntityMetadata> list = Enumerable.Empty<EntityMetadata>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    if (!_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        EntityMetadataRepository repository = new EntityMetadataRepository(service);

                        var task = repository.GetEntitiesDisplayNameAsync();

                        _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, await task);
                    }

                    list = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            string textName = string.Empty;

            txtBFilterEnitity.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadEntities(list);
        }

        private static IEnumerable<EntityMetadata> FilterList(IEnumerable<EntityMetadata> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (int.TryParse(textName, out int tempInt))
                {
                    list = list.Where(ent => ent.ObjectTypeCode == tempInt);
                }
                else
                {
                    if (Guid.TryParse(textName, out Guid tempGuid))
                    {
                        list = list.Where(ent => ent.MetadataId == tempGuid);
                    }
                    else
                    {
                        list = list
                        .Where(ent =>
                            ent.LogicalName.ToLower().Contains(textName)
                            || (ent.DisplayName != null && ent.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                        //|| (ent.Description != null && ent.Description.LocalizedLabels
                        //    .Where(l => !string.IsNullOrEmpty(l.Label))
                        //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                        //|| (ent.DisplayCollectionName != null && ent.DisplayCollectionName.LocalizedLabels
                        //    .Where(l => !string.IsNullOrEmpty(l.Label))
                        //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))
                        );
                    }
                }
            }

            return list;
        }

        private void LoadEntities(IEnumerable<EntityMetadata> results)
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results)
                {
                    string name = entity.LogicalName;
                    string displayName = CreateFileHandler.GetLocalizedLabel(entity.DisplayName);

                    EntityMetadataListViewItem item = new EntityMetadataListViewItem(name, displayName, entity);

                    _itemsSource.Add(item);
                }

                if (this.lstVwEntities.Items.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingEntitiesCompletedFormat1, results.Count());
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
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwEntities.SelectedItems.Count > 0;

                    UIElement[] list = { miEntityOperations };

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
                ShowExistingEntities();
            }
        }

        private EntityMetadataListViewItem GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().Count() == 1
                ? this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().SingleOrDefault() : null;
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void miEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void miEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void miEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void miEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void miEntitySecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, entityMetadataList, entity?.EntityLogicalName);
        }

        private async void miGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();
            var descriptor = await GetDescriptor();

            var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(entity.EntityMetadata.MetadataId.Value);

            IEnumerable<OptionSetMetadata> optionSets =
                entityMetadata
                ?.Attributes
                ?.OfType<PicklistAttributeMetadata>()
                .Where(a => a.OptionSet.IsGlobal.GetValueOrDefault())
                .Select(a => a.OptionSet)
                .GroupBy(o => o.MetadataId)
                .Select(g => g.FirstOrDefault())
                ?? new OptionSetMetadata[0]
                ;

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , optionSets
                , string.Empty
                , string.Empty
                );
        }

        private async void miSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void miSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void miSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void miWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty, string.Empty);
        }

        private async void miMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void miMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void miOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void miCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void miCompareApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void miCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void miCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void miCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void miCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void miCreateCSharpFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, CreateEntityMetadataFileAsync);
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityMetadataListViewItem;

                if (item != null)
                {
                    ExecuteActionAsync(item, CreateEntityMetadataFileAsync);
                }
            }
        }

        private async Task ExecuteActionAsync(EntityMetadataListViewItem entityMetadata, Func<EntityMetadataListViewItem, Task> action)
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

            await action(entityMetadata);
        }

        private async Task CreateEntityMetadataFileAsync(EntityMetadataListViewItem entityMetadata)
        {
            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

                var config = new CreateFileWithEntityMetadataCSharpConfiguration(
                    entityMetadata.EntityLogicalName
                    , txtBFolder.Text.Trim()
                    , tabSpacer
                    , _commonConfig.GenerateAttributes
                    , _commonConfig.GenerateStatus
                    , _commonConfig.GenerateLocalOptionSet
                    , _commonConfig.GenerateGlobalOptionSet
                    , _commonConfig.GenerateOneToMany
                    , _commonConfig.GenerateManyToOne
                    , _commonConfig.GenerateManyToMany
                    , _commonConfig.GenerateKeys
                    , _commonConfig.AllDescriptions
                    , _commonConfig.EntityMetadaOptionSetDependentComponents
                    , _commonConfig.GenerateIntoSchemaClass
                    , _commonConfig.WithManagedInfo
                    , _commonConfig.ConstantType
                    , _commonConfig.OptionSetExportType
                    );

                string filePath = string.Empty;

                using (var handler = new CreateFileWithEntityMetadataCSharpHandler(config, service, _iWriteToOutput))
                {
                    string fileName = null;

                    if (!string.IsNullOrEmpty(_filePath))
                    {
                        fileName = Path.GetFileName(_filePath);
                    }

                    filePath = await handler.CreateFileAsync(fileName);
                }

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, config.EntityName, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                this._iWriteToOutput.WriteToOutput(string.Empty);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private void miCreateJavaScriptFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, CreateEntityMetadataFileJSAsync);
        }

        private async Task CreateEntityMetadataFileJSAsync(EntityMetadataListViewItem entityMetadata)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

            var config = new CreateFileWithEntityMetadataJavaScriptConfiguration(
                entityMetadata.EntityLogicalName
                , txtBFolder.Text.Trim()
                , tabSpacer
                , _commonConfig.EntityMetadaOptionSetDependentComponents
                );

            try
            {
                using (var handler = new CreateFileWithEntityMetadataJavaScriptHandler(config, service, _iWriteToOutput))
                {
                    string filePath = await handler.CreateFileAsync();

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, config.EntityName, filePath);

                    this._iWriteToOutput.PerformAction(filePath);
                }

                this._iWriteToOutput.WriteToOutput(string.Empty);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private void miCreateFileAttributesDependentComponents_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, StartCreateFileWithAttibuteDependentAsync);
        }

        private async Task StartCreateFileWithAttibuteDependentAsync(EntityMetadataListViewItem entityMetadata)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            string operation = string.Format(Properties.OperationNames.CreatingFileWithAttributesDependentComponentsFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            _iWriteToOutput.WriteToOutputStartOperation(operation);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                string folder = txtBFolder.Text.Trim();
                bool allComponents = _commonConfig.AllDependentComponentsForAttributes;

                string fileName = string.Format("{0}.{1} attributes dependent components at {2}.txt", service.ConnectionData.Name, entityMetadata.EntityLogicalName, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var dependencyRepository = new DependencyRepository(service);
                var descriptor = new SolutionComponentDescriptor(service, true);
                var descriptorHandler = new DependencyDescriptionHandler(descriptor);

                var handler = new EntityAttributesDependentComponentsHandler(dependencyRepository, descriptor, descriptorHandler);

                string message = await handler.CreateFileAsync(entityMetadata.EntityLogicalName, filePath, allComponents);

                this._iWriteToOutput.WriteToOutput(message);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            _iWriteToOutput.WriteToOutputEndOperation(operation);
        }

        private void miExportEntityXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, CreateEntityXmlFileAsync);
        }

        private void miPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, PublishEntityAsync);
        }

        private async Task CreateEntityXmlFileAsync(EntityMetadataListViewItem entityMetadata)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.GettingEntityXmlFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                var repository = new EntityMetadataRepository(service);

                var fileName = string.Format("{0}.{1} - EntityXml at {2}.xml", service.ConnectionData.Name, entityMetadata.EntityLogicalName, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));
                string filePath = Path.Combine(txtBFolder.Text.Trim(), FileOperations.RemoveWrongSymbols(fileName));

                await repository.ExportEntityXmlAsync(entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedEntityXmlFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                this._iWriteToOutput.WriteToOutput(string.Empty);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.GettingEntityXmlFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private async Task PublishEntityAsync(EntityMetadataListViewItem entityMetadata)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityMetadata.EntityLogicalName });

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingEntities();

                return;
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

        private void miOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.EntityMetadata.MetadataId.Value);
            }
        }

        private void miOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityListInWeb(entity.EntityLogicalName);
            }
        }

        private void miOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Entity, entity.EntityMetadata.MetadataId.Value);
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

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entity.EntityMetadata.MetadataId.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");
            }
        }

        private async void miOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, descriptor, _commonConfig, (int)ComponentType.Entity, entity.EntityMetadata.MetadataId.Value, null);
        }

        private async void miOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
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
                , (int)ComponentType.Entity
                , entity.EntityMetadata.MetadataId.Value
                , null
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
                UpdateButtonsEnable();

                ShowExistingEntities();
            }
        }

        private void miExportEntityMetadataOptions_Click(object sender, RoutedEventArgs e)
        {
            _optionsPopup.IsOpen = true;
            _optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
                this.Focus();
            }
        }

        private void miExportEntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, PerformExportEntityRibbon);
        }

        private async Task PerformExportEntityRibbon(EntityMetadataListViewItem entityMetadata)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat1, entityMetadata.EntityLogicalName);

            var service = await GetService();

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                string ribbonXml = await repository.ExportEntityRibbonAsync(entityMetadata.EntityLogicalName, _commonConfig.GetRibbonLocationFilters());

                if (_commonConfig.SetIntellisenseContext)
                {
                    ribbonXml = ContentCoparerHelper.SetIntellisenseContextRibbonDiffXmlEntityName(ribbonXml, entityMetadata.EntityLogicalName);
                }

                ribbonXml = ContentCoparerHelper.FormatXml(ribbonXml, _commonConfig.ExportXmlAttributeOnNewLine);

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityMetadata.EntityLogicalName);
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
        }

        private void miExportEntityRibbonArchive_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, PerformExportEntityRibbonArchive);
        }

        private async Task PerformExportEntityRibbonArchive(EntityMetadataListViewItem entityMetadata)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat1, entityMetadata.EntityLogicalName);

            var service = await GetService();

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                var bodyEntityRibbon = await repository.ExportEntityRibbonByteArrayAsync(entityMetadata.EntityLogicalName, _commonConfig.GetRibbonLocationFilters());

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityMetadata.EntityLogicalName, "zip");
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllBytes(filePath, bodyEntityRibbon);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
        }

        private void miExportEntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, PerformExportEntityRibbonDiffXml);
        }

        private async Task PerformExportEntityRibbonDiffXml(EntityMetadataListViewItem entityMetadata)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFormat1, entityMetadata.EntityLogicalName);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            try
            {
                string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entityMetadata.EntityMetadata, null);

                if (string.IsNullOrEmpty(ribbonDiffXml))
                {
                    ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entityMetadata.EntityLogicalName);
                    return;
                }

                if (_commonConfig.SetXmlSchemasDuringExport)
                {
                    var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaRibbonXml);

                    if (schemasResources != null)
                    {
                        ribbonDiffXml = ContentCoparerHelper.SetXsdSchema(ribbonDiffXml, schemasResources);
                    }
                }

                if (_commonConfig.SetIntellisenseContext)
                {
                    ribbonDiffXml = ContentCoparerHelper.SetIntellisenseContextRibbonDiffXmlEntityName(ribbonDiffXml, entityMetadata.EntityLogicalName);
                }

                ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportXmlAttributeOnNewLine);

                {
                    string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entityMetadata.EntityLogicalName);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                    this._iWriteToOutput.PerformAction(filePath);
                }

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private void miUpdateEntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, PerformUpdateEntityRibbonDiffXml);
        }

        private async Task PerformUpdateEntityRibbonDiffXml(EntityMetadataListViewItem entity)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var newText = string.Empty;
            bool? dialogResult = false;

            var title = string.Format("RibbonDiffXml for {0}", entity.EntityLogicalName);

            this.Dispatcher.Invoke(() =>
            {
                var form = new WindowTextField("Enter " + title, title, string.Empty);

                dialogResult = form.ShowDialog();

                newText = form.FieldText;
            });

            var service = await GetService();

            if (dialogResult.GetValueOrDefault() == false)
            {
                ToggleControls(true, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityCanceledFormat2, service.ConnectionData.Name, entity.EntityLogicalName);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            newText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

            UpdateStatus(Properties.WindowStatusStrings.ValidatingRibbonDiffXmlForEntityFormat1, entity.EntityLogicalName);

            if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
            {
                ToggleControls(true, Properties.WindowStatusStrings.TextIsNotValidXml);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            bool validateResult = await RibbonCustomizationRepository.ValidateXmlDocumentAsync(_iWriteToOutput, doc);

            if (!validateResult)
            {
                ToggleControls(true, Properties.WindowStatusStrings.ValidatingRibbonDiffXmlForEntityFailedFormat1, entity.EntityLogicalName);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            var repositoryPublisher = new PublisherRepository(service);
            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            if (publisherDefault == null)
            {
                ToggleControls(true, Properties.WindowStatusStrings.NotFoundedDefaultPublisher);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entity.EntityLogicalName);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            try
            {
                await repositoryRibbonCustomization.PerformUpdateRibbonDiffXml(_iWriteToOutput, _commonConfig, doc, entity.EntityMetadata, null);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityCompletedFormat2, service.ConnectionData.Name, entity.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityFailedFormat2, service.ConnectionData.Name, entity.EntityLogicalName);
            }
        }
    }
}