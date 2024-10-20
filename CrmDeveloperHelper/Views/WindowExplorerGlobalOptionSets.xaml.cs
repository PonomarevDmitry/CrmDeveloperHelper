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
    public partial class WindowExplorerGlobalOptionSets : WindowWithConnectionList
    {
        private readonly string _filePath;
        private readonly bool _isJavaScript;
        private readonly EnvDTE.SelectedItem _selectedItem;

        private readonly ObservableCollection<OptionSetMetadataListViewItem> _itemsSource;

        private readonly Dictionary<Guid, SolutionComponentMetadataSource> _cacheMetadataSource = new Dictionary<Guid, SolutionComponentMetadataSource>();
        private readonly Dictionary<Guid, IEnumerable<OptionSetMetadata>> _cacheOptionSetMetadata = new Dictionary<Guid, IEnumerable<OptionSetMetadata>>();

        private readonly Popup _optionsPopup;
        private readonly FileGenerationGlobalOptionSetMetadataOptionsControl _optionsControl;

        public WindowExplorerGlobalOptionSets(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , IEnumerable<OptionSetMetadata> optionSetMetadataEnum
            , string filterEntityName
            , string filter
            , string filePath
            , bool isJavaScript
            , EnvDTE.SelectedItem selectedItem
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            this._filePath = filePath;
            this._isJavaScript = isJavaScript;
            this._selectedItem = selectedItem;

            if (optionSetMetadataEnum != null)
            {
                _cacheOptionSetMetadata[service.ConnectionData.ConnectionId] = optionSetMetadataEnum;
            }

            LoadEntityNames(cmBEntityName, service.ConnectionData);

            _optionsControl = new FileGenerationGlobalOptionSetMetadataOptionsControl();
            _optionsControl.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = _optionsControl,

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
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                };
                BindingOperations.SetBinding(txtBFolder, TextBox.TextProperty, binding);

                txtBFolder.DataContext = _commonConfig;
            }

            cmBEntityName.Text = filterEntityName;

            txtBFilter.Text = filter;
            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            _itemsSource = new ObservableCollection<OptionSetMetadataListViewItem>();

            lstVwOptionSets.ItemsSource = _itemsSource;

            UpdateButtonsEnable();

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingOptionSets();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService, _selectedItem
                , getGlobalOptionSetName: GetGlobalOptionSetName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getGlobalOptionSetName: GetGlobalOptionSetName
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));
            }
        }

        private string GetGlobalOptionSetName()
        {
            var entity = GetSelectedEntity();

            return entity?.LogicalName ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            FileGenerationConfiguration.SaveConfiguration();

            GetSelectedConnection()?.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;
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

        private async void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingOptionSets();
            }
        }

        private ConnectionData GetSelectedConnection()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService()
        {
            return GetOrganizationService(GetSelectedConnection());
        }

        private SolutionComponentMetadataSource GetMetadataSource(IOrganizationServiceExtented serivce)
        {
            if (serivce != null)
            {
                if (!_cacheMetadataSource.ContainsKey(serivce.ConnectionData.ConnectionId))
                {
                    var source = new SolutionComponentMetadataSource(serivce);

                    _cacheMetadataSource[serivce.ConnectionData.ConnectionId] = source;
                }

                return _cacheMetadataSource[serivce.ConnectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingOptionSets()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingOptionSets);

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();
            });

            IEnumerable<OptionSetMetadata> list = Enumerable.Empty<OptionSetMetadata>();

            var service = await GetService();

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
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
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

            string filterEntity = null;

            if (connectionData.IsValidEntityName(entityName))
            {
                filterEntity = entityName;
            }

            if (service != null && !string.IsNullOrEmpty(filterEntity))
            {
                var entityId = connectionData.GetEntityMetadataId(filterEntity);

                if (entityId.HasValue)
                {
                    var source = GetMetadataSource(service);

                    var entityMetadata = await source.GetEntityMetadataAsync(entityId.Value);

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

                    var item = new OptionSetMetadataListViewItem(name, displayName, entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwOptionSets.Items.Count == 1)
                {
                    this.lstVwOptionSets.SelectedItem = this.lstVwOptionSets.Items[0];
                }
            });

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingOptionSetsCompletedFormat1, list.Count());
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
                        ent.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                        ||
                        (
                            ent.DisplayName != null
                            && ent.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )
                    );
                }
            }

            return list;
        }

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
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
            ConnectionData connectionData = GetSelectedConnection();

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

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                string fileName = CreateGlobalOptionSetsFileCSharpHandler.CreateFileNameCSharp(service.ConnectionData, optionSets, this._selectedItem != null);

                string filePath = Path.Combine(folder, fileName);

                if (!_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var config = CreateFileCSharpConfiguration.CreateForSchemaGlobalOptionSet(fileGenerationOptions);

                var stringBuilder = new StringBuilder();

                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    var descriptor = new SolutionComponentDescriptor(service);

                    var handler = new CreateGlobalOptionSetsFileCSharpHandler(stringWriter, service, descriptor, config);

                    await handler.CreateFileAsync(optionSets);
                }

                File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

                AddFileToVSProject(_selectedItem, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedGlobalOptionSetMetadataFileFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private async void btnCreateJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

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

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            string optionSetsName = string.Join(",", optionSets.Select(o => o.Name).OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForOptionSetsFormat1, optionSetsName);

            try
            {
                string fileName = CreateGlobalOptionSetsFileCSharpHandler.CreateFileNameJavaScript(service.ConnectionData, optionSets, this._selectedItem != null);

                string filePath = Path.Combine(folder, fileName);

                if (_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var stringBuilder = new StringBuilder();

                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    SolutionComponentDescriptor descriptor = new SolutionComponentDescriptor(service);

                    var handler = new CreateGlobalOptionSetsFileJavaScriptHandler(
                        stringWriter
                        , service
                        , descriptor
                        , fileGenerationOptions.GetTabSpacer()
                        , fileGenerationOptions.GenerateSchemaGlobalOptionSetsWithDependentComponents
                        , fileGenerationOptions.NamespaceGlobalOptionSetsJavaScript
                    );

                    await handler.CreateFileAsync(optionSets);
                }

                File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

                AddFileToVSProject(_selectedItem, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedGlobalOptionSetMetadataFileFormat3, service.ConnectionData.Name, optionSetsName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForOptionSetsCompletedFormat1, optionSetsName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForOptionSetsFailedFormat1, optionSetsName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithGlobalOptionSetsFormat1, optionSetsName);
        }

        private void lstVwOptionSets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = GetItemFromRoutedDataContext<OptionSetMetadataListViewItem>(e);

                if (item != null)
                {
                    ConnectionData connectionData = GetSelectedConnection();

                    if (connectionData != null)
                    {
                        connectionData.OpenGlobalOptionSetInWeb(item.OptionSetMetadata.MetadataId.Value);
                    }
                }
            }
        }

        private void lstVwOptionSets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingOptionSets();
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

        private OptionSetMetadataListViewItem GetSelectedEntity()
        {
            return this.lstVwOptionSets.SelectedItems.OfType<OptionSetMetadataListViewItem>().Count() == 1
                ? this.lstVwOptionSets.SelectedItems.OfType<OptionSetMetadataListViewItem>().SingleOrDefault() : null;
        }

        private List<OptionSetMetadataListViewItem> GetSelectedEntitiesList()
        {
            return this.lstVwOptionSets.SelectedItems.OfType<OptionSetMetadataListViewItem>().ToList();
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

            if (service == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingOptionSetFormat2, service.ConnectionData.Name, optionSetName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionPublishingOptionSetFormat2, service.ConnectionData.Name, optionSetName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishOptionSetsAsync(new[] { optionSetName });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingOptionSetCompletedFormat2, service.ConnectionData.Name, optionSetName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingOptionSetFailedFormat2, service.ConnectionData.Name, optionSetName);
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

            ConnectionData connectionData = GetSelectedConnection();

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

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.OptionSet, entity.OptionSetMetadata.MetadataId.Value);
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
            var entitiesList = GetSelectedEntitiesList()
                .Select(e => e.OptionSetMetadata.MetadataId.Value);

            if (!entitiesList.Any())
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.OptionSet, entitiesList, null, withSelect);
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

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");
            }
        }

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , null
                , _commonConfig
                , (int)ComponentType.OptionSet
                , entity.OptionSetMetadata.MetadataId.Value
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

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.OptionSet
                , entity.OptionSetMetadata.MetadataId.Value
                , null
            );
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                LoadEntityNames(cmBEntityName, connectionData);

                await ShowExistingOptionSets();
            }
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            this._optionsControl.BindFileGenerationOptions(fileGenerationOptions);

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
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private async void hyperlinkCSharp_Click(object sender, RoutedEventArgs e)
        {
            OptionSetMetadataListViewItem item = GetItemFromRoutedDataContext<OptionSetMetadataListViewItem>(e);

            if (item == null)
            {
                return;
            }

            await CreateCSharpFile(new[] { item.OptionSetMetadata });
        }

        private async void hyperlinkJavaScript_Click(object sender, RoutedEventArgs e)
        {
            OptionSetMetadataListViewItem item = GetItemFromRoutedDataContext<OptionSetMetadataListViewItem>(e);

            if (item == null)
            {
                return;
            }

            await CreateJavaScriptFile(new[] { item.OptionSetMetadata });
        }

        private async void hyperlinkPublishOptionSet_Click(object sender, RoutedEventArgs e)
        {
            OptionSetMetadataListViewItem item = GetItemFromRoutedDataContext<OptionSetMetadataListViewItem>(e);

            if (item == null)
            {
                return;
            }

            await PublishOptionSetAsync(item.OptionSetMetadata.Name);
        }

        #region Clipboard

        private void mIClipboardCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<OptionSetMetadataListViewItem>(e, ent => ent.LogicalName);
        }

        private void mIClipboardCopyDisplayName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<OptionSetMetadataListViewItem>(e, ent => ent.DisplayName);
        }

        private void mIClipboardCopyOptionSetId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<OptionSetMetadataListViewItem>(e, ent => ent.OptionSetMetadata.MetadataId.ToString());
        }

        #endregion Clipboard
    }
}