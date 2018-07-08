using Microsoft.Xrm.Sdk.Metadata;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        private int _init = 0;

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
            }

            InitializeComponent();

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
            txtBSpaceCount.DataContext = _commonConfig;

            rBTab.DataContext = _commonConfig;
            rBSpaces.DataContext = _commonConfig;

            rBClasses.DataContext = _commonConfig;
            rBEnums.DataContext = _commonConfig;

            rBReadOnly.DataContext = _commonConfig;
            rBConst.DataContext = _commonConfig;

            txtBNameSpace.DataContext = cmBCurrentConnection;

            chBAllDescriptions.DataContext = _commonConfig;
            chBWithDependentComponents.DataContext = _commonConfig;
            chBWithManagedInfo.DataContext = _commonConfig;

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
                    _iWriteToOutput.WriteToOutput("Connection to CRM.");
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async void ShowExistingOptionSets()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading optionsets...");

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

                        _cacheOptionSetMetadata.Add(service.ConnectionData.ConnectionId, await task);
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

            this._iWriteToOutput.WriteToOutput("Found {0} optionsets.", list.Count());

            LoadEntities(list);

            UpdateStatus(string.Format("{0} optionsets loaded.", list.Count()));

            ToggleControls(true);
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

                    message.Append("Folder does not exists.");
                }
            }
            else
            {
                result = false;

                if (message.Length > 0)
                {
                    message.AppendLine();
                }

                message.Append("Folder does not exists.");
            }

            if (!result)
            {
                MessageBox.Show(message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        private void ToggleControls(bool enabled)
        {
            this._controlsEnabled = enabled;

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

        private void UpdateStatus(string msg)
        {
            this.statusBar.Dispatcher.Invoke(() =>
            {
                this.tSSLStatusMessage.Content = msg;
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
            if (optionSets == null)
            {
                return;
            }

            if (!_controlsEnabled)
            {
                return;
            }

            bool allGood = FolderExists();

            if (!allGood)
            {
                return;
            }

            ToggleControls(false);
            UpdateStatus("Creating File...");

            this._iWriteToOutput.WriteToOutput("Start creating file with OptionSets at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);
            var constantType = _commonConfig.ConstantType;
            var optionSetExportType = _commonConfig.OptionSetExportType;

            string folder = txtBFolder.Text.Trim();
            string nameSpace = txtBNameSpace.Text.Trim();

            bool withDependentComponents = chBWithDependentComponents.IsChecked.GetValueOrDefault();
            bool allDescriptions = chBAllDescriptions.IsChecked.GetValueOrDefault();
            bool withManagedInfo = chBWithManagedInfo.IsChecked.GetValueOrDefault();

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

            this._iWriteToOutput.WriteToOutput("Created file with OptionSets: {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath, _commonConfig);

            this._iWriteToOutput.WriteToOutput("End creating file with OptionSets at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            UpdateStatus("File is created.");

            ToggleControls(true);
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

            if (!_controlsEnabled)
            {
                return;
            }

            bool allGood = FolderExists();

            if (!allGood)
            {
                return;
            }

            ToggleControls(false);
            UpdateStatus("Creating File...");

            this._iWriteToOutput.WriteToOutput("Start creating file with Global OptionSets at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.IndentType, _commonConfig.SpaceCount);

                string folder = txtBFolder.Text.Trim();
                string nameSpace = txtBNameSpace.Text.Trim();

                bool withDependentComponents = chBWithDependentComponents.IsChecked.GetValueOrDefault();

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

                this._iWriteToOutput.WriteToOutput("Created file with Global OptionSets: {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("End creating file with Global OptionSets at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                UpdateStatus("File is created.");
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus("Operation failed.");
            }
            finally
            {
                ToggleControls(true);
            }
        }

        private async void lstVwOptionSets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as OptionSetMetadataListViewItem;

                if (item != null)
                {
                    await CreateCSharpFile(new[] { item.OptionSetMetadata });
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

            base.OnKeyDown(e);
        }

        private OptionSetMetadataListViewItem GetSelectedEntity()
        {
            OptionSetMetadataListViewItem result = null;

            if (this.lstVwOptionSets.SelectedItems.Count == 1
                && this.lstVwOptionSets.SelectedItems[0] != null
                && this.lstVwOptionSets.SelectedItems[0] is OptionSetMetadataListViewItem
                )
            {
                result = (this.lstVwOptionSets.SelectedItems[0] as OptionSetMetadataListViewItem);
            }

            return result;
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

            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus(string.Format("Publishing OptionSet {0}...", optionSetName));

            this._iWriteToOutput.WriteToOutput("Start publishing OptionSet {0} at {1}", optionSetName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishOptionSetsAsync(new[] { optionSetName });

                this._iWriteToOutput.WriteToOutput("End publishing OptionSet {0} at {1}", optionSetName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                UpdateStatus(string.Format("OptionSet {0} published.", optionSetName));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus(string.Format("Publishing OptionSet {0} failed.", optionSetName));
            }
            finally
            {
                ToggleControls(true);
            }
        }

        private void btnClearEntityFilter_Click(object sender, RoutedEventArgs e)
        {
            this._cacheOptionSetMetadata.Clear();

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

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

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

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.OptionSet, entity.OptionSetMetadata.MetadataId.Value);
            }
        }

        private void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            AddIntoSolution(true, null);
        }

        private void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                AddIntoSolution(false, solutionUniqueName);
            }
        }

        private void AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                var backWorker = new Thread(() =>
                {
                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        var contr = new SolutionController(this._iWriteToOutput);

                        contr.ExecuteAddingComponentesIntoSolution(connectionData, _commonConfig, solutionUniqueName, ComponentType.OptionSet, new[] { entity.OptionSetMetadata.MetadataId.Value }, null, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                });

                backWorker.Start();
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                var lastSolution = items.FirstOrDefault(i => string.Equals(i.Uid, "contMnAddIntoSolutionLast", StringComparison.InvariantCultureIgnoreCase));

                if (lastSolution != null)
                {
                    lastSolution.Items.Clear();

                    lastSolution.IsEnabled = false;
                    lastSolution.Visibility = Visibility.Collapsed;

                    ConnectionData connectionData = null;

                    cmBCurrentConnection.Dispatcher.Invoke(() =>
                    {
                        connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                    });

                    if (connectionData != null
                        && connectionData.LastSelectedSolutionsUniqueName != null
                        && connectionData.LastSelectedSolutionsUniqueName.Any()
                        )
                    {
                        lastSolution.IsEnabled = true;
                        lastSolution.Visibility = Visibility.Visible;

                        foreach (var uniqueName in connectionData.LastSelectedSolutionsUniqueName)
                        {
                            var menuItem = new MenuItem()
                            {
                                Header = uniqueName.Replace("_", "__"),
                                Tag = uniqueName,
                            };

                            menuItem.Click += AddIntoCrmSolutionLast_Click;

                            lastSolution.Items.Add(menuItem);
                        }
                    }
                }
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

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
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
            if (_init > 0)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();
            });

            if (!_controlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                ShowExistingOptionSets();
            }
        }
    }
}