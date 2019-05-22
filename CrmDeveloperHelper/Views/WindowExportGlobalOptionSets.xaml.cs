using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
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

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly string _filePath;
        private readonly bool _isJavaScript;
        private readonly EnvDTE.SelectedItem _selectedItem;

        private readonly ObservableCollection<OptionSetMetadataListViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, IEnumerable<OptionSetMetadata>> _cacheOptionSetMetadata = new Dictionary<Guid, IEnumerable<OptionSetMetadata>>();

        private readonly Popup _optionsPopup;

        public WindowExportGlobalOptionSets(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<OptionSetMetadata> optionSets
            , string filterEntityName
            , string selection
            , string filePath
            , bool isJavaScript
            , EnvDTE.SelectedItem selectedItem
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._filePath = filePath;
            this._isJavaScript = isJavaScript;
            this._selectedItem = selectedItem;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            if (optionSets != null)
            {
                _cacheOptionSetMetadata[service.ConnectionData.ConnectionId] = optionSets;
            }

            InitializeComponent();

            LoadEntityNames(cmBEntityName, service.ConnectionData);

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
            else if (this._selectedItem != null)
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
                };
                BindingOperations.SetBinding(txtBFolder, TextBox.TextProperty, binding);

                txtBFolder.DataContext = _commonConfig;
            }

            cmBEntityName.Text = filterEntityName;

            txtBFilter.Text = selection;
            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            _itemsSource = new ObservableCollection<OptionSetMetadataListViewItem>();

            lstVwOptionSets.ItemsSource = _itemsSource;

            UpdateButtonsEnable();

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

            ShowExistingOptionSets();
        }

        private void LoadFromConfig()
        {
            txtBNamespaceOptionSetsCSharp.DataContext = cmBCurrentConnection;
            txtBNamespaceOptionSetsJavaScript.DataContext = cmBCurrentConnection;

            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

            GetConnectionData()?.Save();

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
                        bool enabled = this.IsControlsEnabled && this.lstVwOptionSets.SelectedItems.Count > 0;

                        UIElement[] list =
                        {
                            tSDDBSingleOptionSet
                            , btnCreateCSharpFileForSingleOptionSet
                            , btnCreateJavaScriptFileForSingleOptionSetJsonObject
                        };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }

                    {
                        bool enabled = this.IsControlsEnabled;

                        UIElement[] list =
                        {
                            tSDDBGlobalOptionSets
                            , tSBCreateCSharpFile
                            , tSBCreateJavaScriptFileJsonObject
                        };

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
            ConnectionData connectionData = GetConnectionData();

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

        private async Task ShowExistingOptionSets()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingOptionSets);

            this._itemsSource.Clear();

            IEnumerable<OptionSetMetadata> list = Enumerable.Empty<OptionSetMetadata>();

            try
            {
                if (service != null)
                {
                    if (!_cacheOptionSetMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        OptionSetRepository repository = new OptionSetRepository(service);

                        var task = repository.GetOptionSetsAsync();

                        var optionSets = await task;

                        _cacheOptionSetMetadata.Add(service.ConnectionData.ConnectionId, optionSets);
                    }

                    list = _cacheOptionSetMetadata[service.ConnectionData.ConnectionId];
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string entityName = string.Empty;
            string textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();

                if (!string.IsNullOrEmpty(cmBEntityName.Text)
                    && cmBEntityName.Items.Contains(cmBEntityName.Text)
                )
                {
                    entityName = cmBEntityName.Text.Trim().ToLower();
                }
            });

            if (!string.IsNullOrEmpty(entityName))
            {
                var entityId = service.ConnectionData.GetEntityMetadataId(entityName);

                if (entityId.HasValue)
                {
                    var source = new SolutionComponentMetadataSource(service);

                    var entityMetadata = source.GetEntityMetadata(entityId.Value);

                    var entityOptionSets = new HashSet<Guid>(entityMetadata
                        .Attributes
                        .OfType<EnumAttributeMetadata>()
                        .Where(a => a.OptionSet != null && a.OptionSet.IsGlobal.GetValueOrDefault())
                        .Select(a => a.OptionSet.MetadataId.Value)
                    );

                    list = list.Where(o => entityOptionSets.Contains(o.MetadataId.Value));
                }
            }

            list = FilterList(list, textName);

            this.lstVwOptionSets.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list)
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

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingOptionSetsCompletedFormat1, list.Count());
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar
                , cmBCurrentConnection
                , btnSetCurrentConnection
                , this.tSBCreateCSharpFile
                , this.tSBCreateJavaScriptFileJsonObject
            );

            UpdateButtonsEnable();
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

        private async void btnCreateCSharpFile_Click(object sender, RoutedEventArgs e)
        {
            var connectionData = GetConnectionData();

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

            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

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

            var service = await GetService();

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.GenerateCommonIndentType, _commonConfig.GenerateCommonSpaceCount);

                string fileName = null;

                if (optionSets.Count() == 1)
                {
                    fileName = string.Format("{0}.{1}.Generated.cs", service.ConnectionData.Name, optionSets.First().Name);
                }
                else
                {
                    fileName = string.Format("{0}.GlobalOptionSets.cs", service.ConnectionData.Name);
                }

                if (this._selectedItem != null)
                {
                    if (optionSets.Count() == 1)
                    {
                        fileName = string.Format("{0}.Generated.cs", optionSets.First().Name);
                    }
                    else
                    {
                        fileName = "GlobalOptionSets.cs";
                    }
                }

                string filePath = Path.Combine(folder, fileName);

                if (!_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                using (var handler = new CreateGlobalOptionSetsFileCSharpHandler(
                    service
                    , _iWriteToOutput
                    , tabSpacer
                    , _commonConfig.GenerateSchemaConstantType
                    , _commonConfig.GenerateSchemaOptionSetExportType
                    , _commonConfig.GenerateSchemaGlobalOptionSetsWithDependentComponents
                    , _commonConfig.SolutionComponentWithManagedInfo
                    , _commonConfig.GenerateCommonAllDescriptions
                    , _commonConfig.GenerateSchemaAddDescriptionAttribute
                ))
                {
                    await handler.CreateFileAsync(filePath, optionSets);
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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private async void btnCreateJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var connectionData = GetConnectionData();

            if (connectionData != null
                && _cacheOptionSetMetadata.ContainsKey(connectionData.ConnectionId)
            )
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

            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

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

            var service = await GetService();

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.GenerateCommonIndentType, _commonConfig.GenerateCommonSpaceCount);

                string fileName = null;

                if (optionSets.Count() == 1)
                {
                    fileName = string.Format("{0}.{1}.generated.js", service.ConnectionData.Name, optionSets.First().Name);
                }
                else
                {
                    fileName = string.Format("{0}.globaloptionsets.js", service.ConnectionData.Name);
                }

                if (this._selectedItem != null)
                {
                    if (optionSets.Count() == 1)
                    {
                        fileName = string.Format("{0}.generated.js", optionSets.First().Name);
                    }
                    else
                    {
                        fileName = "globaloptionsets.js";
                    }
                }

                string filePath = Path.Combine(folder, fileName);

                if (_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                using (var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(
                    service
                    , _iWriteToOutput
                    , tabSpacer
                    , _commonConfig.GenerateSchemaGlobalOptionSetsWithDependentComponents
                ))
                {
                    await handler.CreateFileAsync(filePath, optionSets);
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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedGlobalOptionSetMetadataFileForConnectionFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private void lstVwOptionSets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as OptionSetMetadataListViewItem;

                if (item != null)
                {
                    if (_isJavaScript && !string.IsNullOrEmpty(_filePath))
                    {
                        CreateJavaScriptFile(new[] { item.OptionSetMetadata });
                    }
                    else
                    {
                        CreateCSharpFile(new[] { item.OptionSetMetadata });
                    }
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

        private async void btnCreateJavaScriptFileForSingleOptionSetJsonObject_Click(object sender, RoutedEventArgs e)
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

            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingOptionSetFormat2, service.ConnectionData.Name, optionSetName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingOptionSetFormat2, service.ConnectionData.Name, optionSetName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishOptionSetsAsync(new[] { optionSetName });

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingOptionSetCompletedFormat2, service.ConnectionData.Name, optionSetName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingOptionSetFailedFormat2, service.ConnectionData.Name, optionSetName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingOptionSetFormat2, service.ConnectionData.Name, optionSetName);
        }

        private void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetConnectionData();

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

            ConnectionData connectionData = GetConnectionData();

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
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.OptionSet, new[] { entity.OptionSetMetadata.MetadataId.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetConnectionData();

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
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.LogicalName ?? txtBFilter.Text);
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

        public ConnectionData GetConnectionData()
        {
            ConnectionData connectionData = null;

            this.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.OfType<ConnectionData>())
            {
                removed.Save();
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetConnectionData();

            if (connectionData != null)
            {
                LoadEntityNames(cmBEntityName, connectionData);

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

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetConnectionData());
        }
    }
}