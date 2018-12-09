using Microsoft.Xrm.Sdk.Metadata;
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
    public partial class WindowExportGlobalOptionSets : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private readonly string _filePath;

        private bool _controlsEnabled = true;

        private ObservableCollection<OptionSetMetadataListViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, IEnumerable<OptionSetMetadata>> _cacheOptionSetMetadata = new Dictionary<Guid, IEnumerable<OptionSetMetadata>>();

        private Popup _optionsPopup;

        private int _init = 0;

        private HashSet<string> _selectedOptionSets;

        public WindowExportGlobalOptionSets(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<OptionSetMetadata> optionSets
            , string filePath
            , string selection
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;
            this._filePath = filePath;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            if (optionSets != null)
            {
                _cacheOptionSetMetadata[service.ConnectionData.ConnectionId] = optionSets;

                this._selectedOptionSets = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

                foreach (var item in optionSets)
                {
                    _selectedOptionSets.Add(item.Name);
                }
            }

            InitializeComponent();

            var child = new ExportGlobalOptionSetMetadataOptionsControl(_commonConfig);
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

            if (!string.IsNullOrEmpty(_filePath))
            {
                txtBFolder.IsReadOnly = true;
                txtBFolder.Background = SystemColors.InactiveSelectionHighlightBrush;
                txtBFolder.Text = Path.GetDirectoryName(_filePath);
            }

            if (optionSets != null)
            {
                btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = true;
                btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Visible;
            }
            else
            {
                btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
                btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;
            }

            txtBFilter.Text = selection;
            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            _itemsSource = new ObservableCollection<OptionSetMetadataListViewItem>();

            lstVwOptionSets.ItemsSource = _itemsSource;

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

            ShowExistingOptionSets();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwOptionSets.Dispatcher.Invoke(() =>
            {
                try
                {
                    {
                        bool enabled = this._controlsEnabled && this.lstVwOptionSets.SelectedItems.Count > 0;

                        UIElement[] list = { tSDDBSingleOptionSet, btnCreateJavaScriptFileForSingleOptionSet, btnCreateCSharpFileForSingleOptionSet };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }

                    {
                        bool enabled = _controlsEnabled;

                        UIElement[] list = { tSDDBGlobalOptionSets, tSBCreateCSharpFile, tSBCreateJavaScriptFile };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingOptionSets();
            }
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

        private async Task ShowExistingOptionSets()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }
            
            ToggleControls(false, Properties.WindowStatusStrings.LoadingOptionSets);

            this._itemsSource.Clear();

            IEnumerable<OptionSetMetadata> list = Enumerable.Empty<OptionSetMetadata>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    if (!_cacheOptionSetMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        OptionSetRepository repository = new OptionSetRepository(service);

                        var task = repository.GetOptionSetsAsync();

                        var optionSets = await task;

                        if (this._selectedOptionSets != null)
                        {
                            optionSets = optionSets.Where(o => _selectedOptionSets.Contains(o.Name)).ToList();
                        }

                        _cacheOptionSetMetadata.Add(service.ConnectionData.ConnectionId, optionSets);
                    }

                    list = _cacheOptionSetMetadata[service.ConnectionData.ConnectionId];
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

            LoadEntities(list);
            
            ToggleControls(true, Properties.WindowStatusStrings.LoadingOptionSetsCompletedFormat1, list.Count());
        }

        private static IEnumerable<OptionSetMetadata> FilterList(IEnumerable<OptionSetMetadata> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.MetadataId == tempGuid);
                }
                else
                {
                    list = list
                    .Where(ent =>
                        ent.Name.ToLower().Contains(textName)
                        || (ent.DisplayName != null && ent.DisplayName.LocalizedLabels
                            .Where(l => !string.IsNullOrEmpty(l.Label))
                            .Any(lbl => lbl.Label.ToLower().Contains(textName)))
                    );
                }
            }

            return list;
        }

        private void LoadEntities(IEnumerable<OptionSetMetadata> results)
        {
            this.lstVwOptionSets.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results)
                {
                    string name = entity.Name;
                    string displayName = CreateFileHandler.GetLocalizedLabel(entity.DisplayName);

                    OptionSetMetadataListViewItem item = new OptionSetMetadataListViewItem(name, displayName, entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwOptionSets.Items.Count == 1)
                {
                    this.lstVwOptionSets.SelectedItem = this.lstVwOptionSets.Items[0];
                }
            });
        }

        private bool FolderExists()
        {
            bool result = true;
            StringBuilder message = new StringBuilder();

            string folder = string.Empty;

            txtBFolder.Dispatcher.Invoke(() =>
            {
                folder = txtBFolder.Text.Trim();
            });

            if (!string.IsNullOrEmpty(folder))
            {
                if (!Directory.Exists(folder))
                {
                    result = false;

                    if (message.Length > 0)
                    {
                        message.AppendLine();
                    }

                    message.Append(Properties.MessageBoxStrings.FolderDoesNotExists);
                }
            }
            else
            {
                result = false;

                if (message.Length > 0)
                {
                    message.AppendLine();
                }

                message.Append(Properties.MessageBoxStrings.FolderDoesNotExists);
            }

            if (!result)
            {
                MessageBox.Show(message.ToString(), Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSBCreateCSharpFile, enabled);
            ToggleControl(this.tSBCreateJavaScriptFile, enabled);

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

        private async void btnCreateCSharpFile_Click(object sender, RoutedEventArgs e)
        {
            var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null && _cacheOptionSetMetadata.ContainsKey(connectionData.ConnectionId))
            {
                await CreateCSharpFile(_cacheOptionSetMetadata[connectionData.ConnectionId]);
            }
        }

        private async Task CreateCSharpFile(IEnumerable<OptionSetMetadata> optionSets)
        {
            if (optionSets == null || !optionSets.Any())
            {
                return;
            }

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            bool allGood = FolderExists();

            if (!allGood)
            {
                return;
            }

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);
                var constantType = _commonConfig.ConstantType;
                var optionSetExportType = _commonConfig.OptionSetExportType;

                string folder = txtBFolder.Text.Trim();
                string nameSpace = txtBNameSpace.Text.Trim();

                bool withDependentComponents = _commonConfig.GlobalOptionSetsWithDependentComponents;
                bool allDescriptions = _commonConfig.AllDescriptions;
                bool withManagedInfo = _commonConfig.WithManagedInfo;

                string filePath = null;

                var service = await GetService();

                if (!string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }
                else
                {
                    if (optionSets.Count() == 1)
                    {
                        string fileName = string.Format("{0}.{1}.Generated.cs", service.ConnectionData.Name, optionSets.First().Name);
                        filePath = Path.Combine(folder, fileName);
                    }
                    else
                    {
                        string fileName = string.Format("{0}.GlobalOptionSets.cs", service.ConnectionData.Name);
                        filePath = Path.Combine(folder, fileName);
                    }
                }

                service.ConnectionData.NameSpaceOptionSets = nameSpace;

                using (var handler = new CreateGlobalOptionSetsFileCSharpHandler(
                                    service
                                    , _iWriteToOutput
                                    , tabSpacer
                                    , constantType
                                    , optionSetExportType
                                    , withDependentComponents
                                    , withManagedInfo
                                    , allDescriptions
                                    ))
                {
                    await handler.CreateFileAsync(filePath, optionSets);
                }

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetsName, filePath);
                
                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private async void btnCreateJavaScriptFile_Click(object sender, RoutedEventArgs e)
        {
            var connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null && _cacheOptionSetMetadata.ContainsKey(connectionData.ConnectionId))
            {
                await CreateJavaScriptFile(_cacheOptionSetMetadata[connectionData.ConnectionId]);
            }
        }

        private async Task CreateJavaScriptFile(IEnumerable<OptionSetMetadata> optionSets)
        {
            if (optionSets == null)
            {
                return;
            }

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            bool allGood = FolderExists();

            if (!allGood)
            {
                return;
            }

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

                string folder = txtBFolder.Text.Trim();
                string nameSpace = txtBNameSpace.Text.Trim();

                bool withDependentComponents = _commonConfig.GlobalOptionSetsWithDependentComponents;

                string filePath = null;

                var service = await GetService();

                if (optionSets.Count() == 1)
                {
                    string fileName = string.Format("{0}.{1}.Generated.js", service.ConnectionData.Name, optionSets.First().Name);
                    filePath = Path.Combine(folder, fileName);
                }
                else
                {
                    string fileName = string.Format("{0}.GlobalOptionSets.js", service.ConnectionData.Name);
                    filePath = Path.Combine(folder, fileName);
                }

                service.ConnectionData.NameSpaceOptionSets = nameSpace;

                using (var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(
                    service
                    , _iWriteToOutput
                    , tabSpacer
                    , withDependentComponents
                    ))
                {
                    await handler.CreateFileAsync(filePath, optionSets);
                }

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private void lstVwOptionSets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as OptionSetMetadataListViewItem;

                if (item != null)
                {
                    CreateCSharpFile(new[] { item.OptionSetMetadata });
                }
            }
        }

        private void lstVwOptionSets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingOptionSets();
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

        private OptionSetMetadataListViewItem GetSelectedEntity()
        {
            return this.lstVwOptionSets.SelectedItems.OfType<OptionSetMetadataListViewItem>().Count() == 1
                ? this.lstVwOptionSets.SelectedItems.OfType<OptionSetMetadataListViewItem>().SingleOrDefault() : null;
        }

        private async void btnCreateCSharpFileForSingleOptionSet_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await CreateCSharpFile(new[] { entity.OptionSetMetadata });
        }

        private async void btnCreateJavaScriptFileForSingleOptionSet_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await CreateJavaScriptFile(new[] { entity.OptionSetMetadata });
        }

        private async void btnPublishOptionSet_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await PublishOptionSetAsync(entity.OptionSetMetadata.Name);
        }

        private async Task PublishOptionSetAsync(string optionSetName)
        {
            if (string.IsNullOrEmpty(optionSetName))
            {
                return;
            }

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PublishingOptionSetFormat2, service.ConnectionData.Name, optionSetName);

            ToggleControls(false, Properties.WindowStatusStrings.PublishingOptionSetFormat2, service.ConnectionData.Name, optionSetName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishOptionSetsAsync(new[] { optionSetName });

                ToggleControls(true, Properties.WindowStatusStrings.PublishingOptionSetCompletedFormat2, service.ConnectionData.Name, optionSetName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingOptionSetFailedFormat2, service.ConnectionData.Name, optionSetName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PublishingOptionSetFormat2, service.ConnectionData.Name, optionSetName);
        }

        private void btnClearEntityFilter_Click(object sender, RoutedEventArgs e)
        {
            this._cacheOptionSetMetadata.Clear();
            this._selectedOptionSets = null;

            btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
            btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;

            ShowExistingOptionSets();
        }

        private void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenGlobalOptionSetInWeb(entity.OptionSetMetadata.MetadataId.Value);
            }
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.OptionSet, entity.OptionSetMetadata.MetadataId.Value);
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

            try
            {
                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.OptionSet, new[] { entity.OptionSetMetadata.MetadataId.Value }, null, withSelect);
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

        private async void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , service
                , null
                , _commonConfig
                , (int)ComponentType.OptionSet
                , entity.OptionSetMetadata.MetadataId.Value
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
                , (int)ComponentType.OptionSet
                , entity.OptionSetMetadata.MetadataId.Value
                , null
            );
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

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, null);
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
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, null);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, null);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, null);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, null);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, null);
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
                ShowExistingOptionSets();
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
    }
}