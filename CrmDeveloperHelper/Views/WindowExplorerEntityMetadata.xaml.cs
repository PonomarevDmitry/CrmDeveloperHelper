using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Implementations;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerEntityMetadata : WindowWithConnectionList
    {
        private readonly Popup _popupExportXmlOptions;
        private readonly Popup _popupFileGenerationEntityMetadataOptions;
        private readonly Popup _popupFileGenerationJavaScriptOptions;
        private readonly Popup _popupEntityMetadataFilter;

        private readonly FileGenerationEntityMetadataCSharpOptionsControl _optionsControlEntityMetadata;
        private readonly FileGenerationEntityMetadataJavaScriptOptionsControl _optionsControlJavaScript;
        private readonly EntityMetadataFilter _entityMetadataFilter;

        private readonly string _filePath;
        private readonly bool _isJavaScript;
        private readonly bool _blockNotMetadata;

        private readonly EnvDTE.SelectedItem _selectedItem;

        private readonly ObservableCollection<EntityMetadataListViewItem> _itemsSource;

        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();

        public WindowExplorerEntityMetadata(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , IEnumerable<EntityMetadata> allEntities
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

            this._blockNotMetadata = selectedItem != null || !string.IsNullOrEmpty(_filePath);

            if (allEntities != null)
            {
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = allEntities;
            }

            var child = new ExportEntityMetadataOptionsControl(_commonConfig);
            child.CloseClicked += optionsExportXmlOptions_CloseClicked;
            this._popupExportXmlOptions = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            _optionsControlEntityMetadata = new FileGenerationEntityMetadataCSharpOptionsControl();
            _optionsControlEntityMetadata.CloseClicked += this.optionsControlFileGeneration_CloseClicked;
            this._popupFileGenerationEntityMetadataOptions = new Popup
            {
                Child = _optionsControlEntityMetadata,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            _optionsControlJavaScript = new FileGenerationEntityMetadataJavaScriptOptionsControl();
            _optionsControlJavaScript.CloseClicked += this.optionsControlJavaScript_CloseClicked;
            this._popupFileGenerationJavaScriptOptions = new Popup
            {
                Child = _optionsControlJavaScript,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            _entityMetadataFilter = new EntityMetadataFilter();
            _entityMetadataFilter.CloseClicked += this.entityMetadataFilter_CloseClicked;
            this._popupEntityMetadataFilter = new Popup
            {
                Child = _entityMetadataFilter,

                PlacementTarget = lblFilterEnitity,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };
            _popupEntityMetadataFilter.Closed += this.popupEntityMetadataFilter_Closed;

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

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            if (allEntities != null)
            {
                var list = allEntities.AsEnumerable();

                list = FilterList(list, filterEntity);

                LoadEntities(list);
            }
            else if (service != null)
            {
                var task = ShowExistingEntities();
            }
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService, _selectedItem
                , getEntityName: GetEntityName
                , getEntityMetadataList: GetEntityMetadataList
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getEntityName: GetEntityName
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

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.LogicalName;
        }

        private IEnumerable<EntityMetadata> GetEntityMetadataList(Guid connectionId)
        {
            if (_cacheEntityMetadata.ContainsKey(connectionId))
            {
                return _cacheEntityMetadata[connectionId];
            }

            return null;
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            FileGenerationConfiguration.SaveConfiguration();

            (cmBCurrentConnection.SelectedItem as ConnectionData)?.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;
        }

        private async Task ShowExistingEntities()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingEntities);

            this.Dispatcher.Invoke(() =>
            {
                _itemsSource.Clear();
            });

            string textName = string.Empty;

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

                this.Dispatcher.Invoke(() =>
                {
                    textName = txtBFilterEnitity.Text.Trim().ToLower();
                });
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            list = FilterList(list, textName);

            LoadEntities(list);

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingEntitiesCompletedFormat1, list.Count());
        }

        private IEnumerable<EntityMetadata> FilterList(IEnumerable<EntityMetadata> list, string textName)
        {
            list = _entityMetadataFilter.FilterList(list);

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (int.TryParse(textName, out int tempInt))
                {
                    list = list.Where(ent => ent.ObjectTypeCode == tempInt);
                }
                else if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.MetadataId == tempGuid);
                }
                else
                {
                    list = list.Where(ent =>
                        ent.LogicalName.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                        ||
                        (
                            ent.DisplayName != null
                            && ent.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )

                    //|| (ent.Description != null && ent.Description.LocalizedLabels
                    //    .Where(l => !string.IsNullOrEmpty(l.Label))
                    //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                    //|| (ent.DisplayCollectionName != null && ent.DisplayCollectionName.LocalizedLabels
                    //    .Where(l => !string.IsNullOrEmpty(l.Label))
                    //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))
                    );
                }
            }

            return list;
        }

        private void LoadEntities(IEnumerable<EntityMetadata> results)
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(e => e.IsIntersect)
                    .ThenBy(e => e.LogicalName)
                )
                {
                    EntityMetadataListViewItem item = new EntityMetadataListViewItem(entity);

                    _itemsSource.Add(item);
                }

                if (this.lstVwEntities.Items.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwEntities.SelectedItems.Count > 0;

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

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingEntities();
            }
        }

        private EntityMetadataListViewItem GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().Count() == 1
                ? this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().SingleOrDefault() : null;
        }

        private List<EntityMetadataListViewItem> GetSelectedEntitiesList()
        {
            return this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void miCreateCSharpFileSchemaMetadata_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, ExecuteCreateEntityMetadataFileCSharpSchemaAsync);
        }

        private async void miCreateCSharpFileProxyClass_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, ExecuteCreateEntityMetadataFileCSharpProxyClassAsync);
        }

        private async void miCreateCSharpFileSchemaMetadataAndProxyClass_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, ExecuteCreateEntityMetadataFileCSharpSchemaAndProxyAsync);
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityMetadataListViewItem entity = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

                if (entity != null)
                {
                    ConnectionData connectionData = GetSelectedConnection();

                    if (connectionData != null)
                    {
                        connectionData.OpenEntityMetadataInWeb(entity.EntityMetadata.MetadataId.Value);
                    }
                }
            }
        }

        private async Task ExecuteActionAsync(EntityMetadataListViewItem entityMetadata, Func<string, EntityMetadataListViewItem, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, entityMetadata);
        }

        private async Task ExecuteActionOnListAsync(IEnumerable<EntityMetadataListViewItem> entityMetadataList, Func<string, IEnumerable<EntityMetadataListViewItem>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, entityMetadataList);
        }

        private async Task ExecuteCreateEntityMetadataFileCSharpSchemaAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var repositoryEntityMetadata = new EntityMetadataRepository(service);
            IMetadataProviderService metadataProviderService = new MetadataProviderService(repositoryEntityMetadata);

            await CreateEntityMetadataFileCSharpSchemaAsync(folder, entityMetadata, metadataProviderService, service);
        }

        private async Task ExecuteCreateEntityMetadataFileCSharpProxyClassAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var repositoryEntityMetadata = new EntityMetadataRepository(service);
            IMetadataProviderService metadataProviderService = new MetadataProviderService(repositoryEntityMetadata);

            await CreateEntityMetadataFileCSharpProxyClassAsync(folder, entityMetadata, metadataProviderService, service.ConnectionData, repositoryEntityMetadata);
        }

        private async Task ExecuteCreateEntityMetadataFileCSharpSchemaAndProxyAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var repositoryEntityMetadata = new EntityMetadataRepository(service);
            IMetadataProviderService metadataProviderService = new MetadataProviderService(repositoryEntityMetadata);

            await CreateEntityMetadataFileCSharpSchemaAsync(folder, entityMetadata, metadataProviderService, service);

            await CreateEntityMetadataFileCSharpProxyClassAsync(folder, entityMetadata, metadataProviderService, service.ConnectionData, repositoryEntityMetadata);
        }

        #region Creating CSharp Metadata Files

        private async Task CreateEntityMetadataFileCSharpSchemaAsync(string folder, EntityMetadataListViewItem entityMetadata, IMetadataProviderService metadataProviderService, IOrganizationServiceExtented service)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForEntityFormat1, entityMetadata.LogicalName);

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var config = CreateFileCSharpConfiguration.CreateForSchemaEntity(fileGenerationOptions);

                string fileName = CreateFileWithEntityMetadataCSharpHandler.CreateFileNameForSchema(service.ConnectionData, entityMetadata.EntityMetadata.SchemaName, this._selectedItem != null);

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                if (!_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
                INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
                ITypeMappingService typeMappingService = new TypeMappingService(fileGenerationOptions.NamespaceClassesCSharp);
                ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);

                ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

                var stringBuilder = new StringBuilder();

                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    var handler = new CreateFileWithEntityMetadataCSharpHandler(stringWriter, config, service, _iWriteToOutput, codeGenerationServiceProvider);

                    await handler.CreateFileAsync(entityMetadata.LogicalName);
                }

                File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

                AddFileToVSProject(_selectedItem, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedEntityMetadataFileFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);
        }

        private async Task CreateEntityMetadataFileCSharpProxyClassAsync(string folder, EntityMetadataListViewItem entityMetadata, IMetadataProviderService metadataProviderService, ConnectionData connectionData, EntityMetadataRepository repositoryEntityMetadata)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, Properties.OperationNames.CreatingFileForEntityFormat2, connectionData.Name, entityMetadata.LogicalName);

            ToggleControls(connectionData, false, Properties.OutputStrings.CreatingFileForEntityFormat1, entityMetadata.LogicalName);

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var config = CreateFileCSharpConfiguration.CreateForProxyClass(fileGenerationOptions);

                string fileName = CreateFileWithEntityMetadataCSharpHandler.CreateFileNameForProxy(connectionData, entityMetadata.EntityMetadata.SchemaName, this._selectedItem != null);

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                if (!_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
                INamingService namingService = new NamingService(connectionData.ServiceContextName, config);
                ITypeMappingService typeMappingService = new TypeMappingService(fileGenerationOptions.NamespaceClassesCSharp);
                ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);

                ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

                var entityMetadataFull = await repositoryEntityMetadata.GetEntityMetadataAsync(entityMetadata.LogicalName);

                var options = new CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    VerbatimOrder = true,
                };

                await codeGenerationService.WriteEntityFileAsync(entityMetadataFull, filePath, fileGenerationOptions.NamespaceClassesCSharp, options);

                AddFileToVSProject(_selectedItem, filePath);

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionCreatedEntityMetadataFileFormat3, connectionData.Name, entityMetadata.LogicalName, filePath);

                this._iWriteToOutput.PerformAction(connectionData, filePath);

                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

                ToggleControls(connectionData, true, Properties.OutputStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);

                ToggleControls(connectionData, true, Properties.OutputStrings.CreatingFileForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(connectionData, Properties.OperationNames.CreatingFileForEntityFormat2, connectionData.Name, entityMetadata.LogicalName);
        }

        #endregion Creating CSharp Metadata Files

        private async void miCreateJavaScriptFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, CreateEntityMetadataFileJavaScriptAsync);
        }

        private async Task CreateEntityMetadataFileJavaScriptAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForEntityFormat1, entityMetadata.LogicalName);

            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            var config = new CreateFileJavaScriptConfiguration(fileGenerationOptions);

            string fileName = string.Format("{0}.{1}.entitymetadata.generated.js", service.ConnectionData.Name, entityMetadata.LogicalName);

            if (this._selectedItem != null)
            {
                fileName = string.Format("{0}.entitymetadata.generated.js", entityMetadata.LogicalName);
            }

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (_isJavaScript && !string.IsNullOrEmpty(_filePath))
            {
                filePath = _filePath;
            }

            try
            {
                var stringBuilder = new StringBuilder();

                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    var handler = new CreateFileWithEntityMetadataJavaScriptHandler(stringWriter, config, service, _iWriteToOutput);

                    await handler.CreateFileAsync(entityMetadata.LogicalName);
                }

                File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedEntityMetadataFileFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                AddFileToVSProject(_selectedItem, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);
        }

        private void miCreateNewEntity_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataCreateUrlInWeb();
            }
        }

        private async void miCreateNewEntityInstance_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, CreateNewEntityInstanceAsync);
        }

        private async Task CreateNewEntityInstanceAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, entityMetadata.EntityMetadata.LogicalName);
        }

        private async void miCreateFileAttributesDependentComponents_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, StartCreateFileWithAttibuteDependentAsync);
        }

        private async Task StartCreateFileWithAttibuteDependentAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_blockNotMetadata)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            string operation = string.Format(Properties.OperationNames.CreatingFileWithAttributesDependentComponentsFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForEntityFormat1, entityMetadata.LogicalName);

            try
            {
                bool allComponents = _commonConfig.AttributesDependentComponentsAllComponents;

                string fileName = string.Format("{0}.{1} attributes dependent components at {2}.txt", service.ConnectionData.Name, entityMetadata.LogicalName, EntityFileNameFormatter.GetDateString());
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var descriptor = new SolutionComponentDescriptor(service);
                descriptor.SetSettings(_commonConfig);
                var dependencyRepository = new DependencyRepository(service);
                var descriptorHandler = new DependencyDescriptionHandler(descriptor);

                var handler = new EntityAttributesDependentComponentsHandler(dependencyRepository, descriptor, descriptorHandler);

                string message = await handler.CreateFileAsync(entityMetadata.LogicalName, filePath, allComponents);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, message);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        #region Create EntityXml File

        private async void miExportEntityXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, CreateEntityXmlFileAsync);
        }

        private async Task CreateEntityXmlFileAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_blockNotMetadata)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.GettingEntityXmlFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForEntityFormat1, entityMetadata.LogicalName);

            try
            {
                var repository = new EntityMetadataRepository(service);

                var fileName = string.Format("{0}.{1} - EntityXml at {2}.xml", service.ConnectionData.Name, entityMetadata.LogicalName, EntityFileNameFormatter.GetDateString());
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await repository.ExportEntityXmlAsync(entityMetadata.LogicalName, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedEntityXmlFileFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.GettingEntityXmlFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);
        }

        #endregion Create EntityXml File

        #region Create EntityMetadata Xml File

        private async void miExportEntityMetadataXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, CreateEntityMetadataXmlFileAsync);
        }

        private async Task CreateEntityMetadataXmlFileAsync(string folder, EntityMetadataListViewItem entityMetadataViewItem)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_blockNotMetadata)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.GettingEntityXmlFormat2, service.ConnectionData.Name, entityMetadataViewItem.LogicalName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForEntityFormat1, entityMetadataViewItem.LogicalName);

            try
            {
                var repository = new EntityMetadataRepository(service);

                var entityMetadata = await repository.GetEntityMetadataAsync(entityMetadataViewItem.LogicalName);

                var serializer = new DataContractSerializer(typeof(EntityMetadata));

                byte[] fileBody = null;

                using (var memoryStream = new MemoryStream())
                {
                    try
                    {
                        var settings = new XmlWriterSettings
                        {
                            Indent = true,
                            Encoding = Encoding.UTF8
                        };

                        using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                        {
                            serializer.WriteObject(xmlWriter, entityMetadata);
                            xmlWriter.Flush();
                        }

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        fileBody = memoryStream.ToArray();
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);

                        fileBody = null;
                    }
                }

                var fileName = string.Format("{0}.{1} - EntityMetadata at {2}.xml", service.ConnectionData.Name, entityMetadataViewItem.LogicalName, EntityFileNameFormatter.GetDateString());
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllBytes(filePath, fileBody);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedEntityXmlFileFormat3, service.ConnectionData.Name, entityMetadataViewItem.LogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityCompletedFormat1, entityMetadataViewItem.LogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityFailedFormat1, entityMetadataViewItem.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.GettingEntityXmlFormat2, service.ConnectionData.Name, entityMetadataViewItem.LogicalName);
        }

        #endregion Create EntityMetadata Xml File

        #region Publish Entity

        private async void miPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entitiesList = GetSelectedEntitiesList();

            if (!entitiesList.Any())
            {
                return;
            }

            await ExecuteActionOnListAsync(entitiesList, PublishEntityAsync);
        }

        protected Task PublishEntityAsync(string folder, IEnumerable<EntityMetadataListViewItem> entityMetadataList)
        {
            return base.PublishEntityAsync(GetSelectedConnection(), entityMetadataList.Select(e => e.EntityMetadata.LogicalName));
        }

        #endregion Publish Entity

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingEntities();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            Popup[] _popupArray = new Popup[] { _popupEntityMetadataFilter, _popupExportXmlOptions, _popupFileGenerationEntityMetadataOptions, _popupFileGenerationJavaScriptOptions };

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

        private void miOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.EntityMetadata.MetadataId.Value);
            }
        }

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                this._iWriteToOutput.OpenFetchXmlFile(connectionData, _commonConfig, entity.EntityMetadata.LogicalName);
            }
        }

        private void miOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(entity.LogicalName);
            }
        }

        private void miOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Entity, entity.EntityMetadata.MetadataId.Value);
            }
        }

        private async void AddToCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void AddToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void AddToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async Task AddToSolution(bool withSelect, string solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior)
        {
            var entitiesList = GetSelectedEntitiesList()
                .Select(e => e.EntityMetadata.MetadataId.Value);

            if (!entitiesList.Any())
            {
                return;
            }

            _commonConfig.Save();

            await AddEntityMetadataToSolution(
                GetSelectedConnection()
                , entitiesList
                , withSelect
                , solutionUniqueName
                , rootComponentBehavior
            );
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddToSolutionLastIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddToSolutionLastDoNotIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddToSolutionLastIncludeAsShellOnly");

                ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddToSolutionLast");

                ActivateControls(items, !_blockNotMetadata, "blockNotMetadata");
            }
        }

        private async void miOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Entity, entity.EntityMetadata.MetadataId.Value, null);
        }

        private async void miOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Entity
                , entity.EntityMetadata.MetadataId.Value
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
                UpdateButtonsEnable();

                await ShowExistingEntities();
            }
        }

        private void miExportXmlOptions_Click(object sender, RoutedEventArgs e)
        {
            _popupExportXmlOptions.IsOpen = true;
            _popupExportXmlOptions.Child.Focus();
        }

        private void optionsExportXmlOptions_CloseClicked(object sender, EventArgs e)
        {
            if (_popupExportXmlOptions.IsOpen)
            {
                _popupExportXmlOptions.IsOpen = false;
                this.Focus();
            }
        }

        private void miFileGenerationEntityMetadataOptions_Click(object sender, RoutedEventArgs e)
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            this._optionsControlEntityMetadata.BindFileGenerationOptions(fileGenerationOptions);

            _popupFileGenerationEntityMetadataOptions.IsOpen = true;
            _popupFileGenerationEntityMetadataOptions.Child.Focus();
        }

        private void optionsControlFileGeneration_CloseClicked(object sender, EventArgs e)
        {
            if (_popupFileGenerationEntityMetadataOptions.IsOpen)
            {
                _popupFileGenerationEntityMetadataOptions.IsOpen = false;
                this.Focus();
            }
        }

        private void miFileGenerationJavaScriptOptions_Click(object sender, RoutedEventArgs e)
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            this._optionsControlJavaScript.BindFileGenerationOptions(fileGenerationOptions);

            _popupFileGenerationJavaScriptOptions.IsOpen = true;
            _popupFileGenerationJavaScriptOptions.Child.Focus();
        }

        private void optionsControlJavaScript_CloseClicked(object sender, EventArgs e)
        {
            if (_popupFileGenerationJavaScriptOptions.IsOpen)
            {
                _popupFileGenerationJavaScriptOptions.IsOpen = false;
                this.Focus();
            }
        }

        private void btnEntityMetadataFilter_Click(object sender, RoutedEventArgs e)
        {
            _popupEntityMetadataFilter.IsOpen = true;
            _popupEntityMetadataFilter.Child.Focus();
        }

        private async void popupEntityMetadataFilter_Closed(object sender, EventArgs e)
        {
            if (_entityMetadataFilter.FilterChanged)
            {
                await ShowExistingEntities();
            }
        }

        private void entityMetadataFilter_CloseClicked(object sender, EventArgs e)
        {
            if (_popupEntityMetadataFilter.IsOpen)
            {
                _popupEntityMetadataFilter.IsOpen = false;
                this.Focus();
            }
        }

        #region Ribbon

        private async Task ExportEntityRibbon(IOrganizationServiceExtented service, string folder, EntityMetadataListViewItem entityMetadata)
        {
            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingRibbonForEntityFormat1, entityMetadata.LogicalName);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                string ribbonXml = await repository.ExportEntityRibbonAsync(entityMetadata.LogicalName, _commonConfig.GetRibbonLocationFilters());

                ribbonXml = ContentComparerHelper.FormatXmlByConfiguration(
                    ribbonXml
                    , _commonConfig
                    , XmlOptionsControls.RibbonXmlOptions
                    , entityName: entityMetadata.LogicalName
                );

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityMetadata.LogicalName);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityRibbonFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingRibbonForEntityCompletedFormat1, entityMetadata.LogicalName);
        }

        private async Task ExportEntityRibbonDiffXml(IOrganizationServiceExtented service, string folder, EntityMetadataListViewItem entityMetadata)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingRibbonDiffXmlForEntityFormat1, entityMetadata.LogicalName);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            try
            {
                string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entityMetadata.EntityMetadata, null);

                if (string.IsNullOrEmpty(ribbonDiffXml))
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entityMetadata.LogicalName);
                    return;
                }

                ribbonDiffXml = ContentComparerHelper.FormatXmlByConfiguration(
                    ribbonDiffXml
                    , _commonConfig
                    , XmlOptionsControls.RibbonXmlOptions
                    , schemaName: AbstractDynamicCommandXsdSchemas.RibbonSchema
                    , entityName: entityMetadata.LogicalName
                );

                {
                    string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entityMetadata.LogicalName);
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityRibbonDiffXmlFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingRibbonDiffXmlForEntityCompletedFormat1, entityMetadata.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entityMetadata.LogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entityMetadata.LogicalName);
        }

        private async void miExportEntityRibbonAndRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, PerformExportEntityRibbonAndRibbonDiffXml);
        }

        private async Task PerformExportEntityRibbonAndRibbonDiffXml(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_blockNotMetadata)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            await ExportEntityRibbon(service, folder, entityMetadata);

            await ExportEntityRibbonDiffXml(service, folder, entityMetadata);
        }

        private async void miExportEntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, PerformExportEntityRibbon);
        }

        private async Task PerformExportEntityRibbon(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_blockNotMetadata)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            await ExportEntityRibbon(service, folder, entityMetadata);
        }

        private async void miExportEntityRibbonArchive_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, PerformExportEntityRibbonArchive);
        }

        private async Task PerformExportEntityRibbonArchive(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_blockNotMetadata)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingRibbonForEntityFormat1, entityMetadata.LogicalName);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                var bodyEntityRibbon = await repository.ExportEntityRibbonByteArrayAsync(entityMetadata.LogicalName, _commonConfig.GetRibbonLocationFilters());

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityMetadata.LogicalName, FileExtension.zip);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllBytes(filePath, bodyEntityRibbon);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityRibbonFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingRibbonForEntityCompletedFormat1, entityMetadata.LogicalName);
        }

        private async void miExportEntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, PerformExportEntityRibbonDiffXml);
        }

        private async Task PerformExportEntityRibbonDiffXml(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_blockNotMetadata)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            await ExportEntityRibbonDiffXml(service, folder, entityMetadata);
        }

        private async void miUpdateEntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, PerformUpdateEntityRibbonDiffXml);
        }

        private async Task PerformUpdateEntityRibbonDiffXml(string folder, EntityMetadataListViewItem entity)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (_blockNotMetadata)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var newText = string.Empty;

            {
                bool? dialogResult = false;

                var title = string.Format("RibbonDiffXml for {0}", entity.LogicalName);

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + title, title, string.Empty);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingRibbonDiffXmlForEntityCanceledFormat2, service.ConnectionData.Name, entity.LogicalName);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }

            newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

            UpdateStatus(service.ConnectionData, Properties.OutputStrings.ValidatingRibbonDiffXmlForEntityFormat1, entity.LogicalName);

            if (!ContentComparerHelper.TryParseXmlDocument(newText, out var doc))
            {
                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.TextIsNotValidXml);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            bool validateResult = await RibbonCustomizationRepository.ValidateXmlDocumentAsync(service.ConnectionData, _iWriteToOutput, doc);

            if (!validateResult)
            {
                var dialogResult = MessageBoxResult.Cancel;

                this.Dispatcher.Invoke(() =>
                {
                    dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                });

                if (dialogResult != MessageBoxResult.OK)
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ValidatingRibbonDiffXmlForEntityFailedFormat1, entity.LogicalName);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }

            var repositoryPublisher = new PublisherRepository(service);
            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            if (publisherDefault == null)
            {
                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.NotFoundedDefaultPublisher);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionUpdatingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entity.LogicalName);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            try
            {
                await repositoryRibbonCustomization.PerformUpdateRibbonDiffXml(_iWriteToOutput, _commonConfig, doc, entity.EntityMetadata, null);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingRibbonDiffXmlForEntityCompletedFormat2, service.ConnectionData.Name, entity.LogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingRibbonDiffXmlForEntityFailedFormat2, service.ConnectionData.Name, entity.LogicalName);
            }
        }

        #endregion Ribbon

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private async void hyperlinkCSharpMetadata_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataListViewItem entity = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, ExecuteCreateEntityMetadataFileCSharpSchemaAsync);
        }

        private async void hyperlinkCSharpProxy_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataListViewItem entity = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, ExecuteCreateEntityMetadataFileCSharpProxyClassAsync);
        }

        private async void hyperlinkCSharpMetadataAndProxy_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataListViewItem entity = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, ExecuteCreateEntityMetadataFileCSharpSchemaAndProxyAsync);
        }

        private async void hyperlinkJavaScript_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataListViewItem entity = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity, CreateEntityMetadataFileJavaScriptAsync);
        }

        private async void hyperlinkPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataListViewItem entity = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

            if (entity == null)
            {
                return;
            }

            await ExecuteActionOnListAsync(new[] { entity }, PublishEntityAsync);
        }

        private async void mICreateFormEntityJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity, JavaScriptObjectType.JsonObject, PerformCreateEntityJavaScriptFileForm);
        }

        private async void mICreateFormEntityJavaScriptFileAnonymousConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity, JavaScriptObjectType.AnonymousConstructor, PerformCreateEntityJavaScriptFileForm);
        }

        private async void mICreateFormEntityJavaScriptFileTypeConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity, JavaScriptObjectType.TypeConstructor, PerformCreateEntityJavaScriptFileForm);
        }

        private async void mICreateRibbonEntityJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity, JavaScriptObjectType.JsonObject, PerformCreateEntityJavaScriptFileRibbon);
        }

        private async void mICreateRibbonEntityJavaScriptFileAnonymousConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity, JavaScriptObjectType.AnonymousConstructor, PerformCreateEntityJavaScriptFileRibbon);
        }

        private async void mICreateRibbonEntityJavaScriptFileTypeConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity, JavaScriptObjectType.TypeConstructor, PerformCreateEntityJavaScriptFileRibbon);
        }

        private async Task ExecuteJavaScriptObjectTypeAsync(EntityMetadataListViewItem entityMetadata, JavaScriptObjectType javaScriptObjectType, Func<string, EntityMetadataListViewItem, JavaScriptObjectType, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, entityMetadata, javaScriptObjectType);
        }

        private async Task PerformCreateEntityJavaScriptFileForm(string folder, EntityMetadataListViewItem entityMetadata, JavaScriptObjectType javaScriptObjectType)
        {
            string formTypeName = "form";
            string formTypeConstructorName = "Form";

            await PerformCreateEntityJavaScriptFile(folder, entityMetadata, javaScriptObjectType, formTypeName, formTypeConstructorName);
        }

        private async Task PerformCreateEntityJavaScriptFileRibbon(string folder, EntityMetadataListViewItem entityMetadata, JavaScriptObjectType javaScriptObjectType)
        {
            string formTypeName = "ribbon";
            string formTypeConstructorName = "Ribbon";

            await PerformCreateEntityJavaScriptFile(folder, entityMetadata, javaScriptObjectType, formTypeName, formTypeConstructorName);
        }

        private async Task PerformCreateEntityJavaScriptFile(string folder, EntityMetadataListViewItem entityMetadata, JavaScriptObjectType javaScriptObjectType, string formTypeName, string formTypeConstructorName)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForEntityFormat1, entityMetadata.LogicalName);

            string objectName = string.Format("{0}_{1}", entityMetadata.LogicalName, formTypeName);
            string constructorName = string.Format("{0}{1}", entityMetadata.LogicalName, formTypeConstructorName);

            string fileName = string.Format("{0}.{1}_{2}.js", service.ConnectionData.Name, entityMetadata.LogicalName, formTypeName);

            if (this._selectedItem != null)
            {
                fileName = string.Format("{0}_{1}.js", entityMetadata.LogicalName, formTypeName);
            }

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var config = new CreateFileJavaScriptConfiguration(fileGenerationOptions);

                string filePath = Path.Combine(folder, fileName);

                if (this._selectedItem != null)
                {
                    filePath = FileOperations.CheckFilePathUnique(filePath);
                }

                var stringBuilder = new StringBuilder();

                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    var handler = new CreateFormTabsJavaScriptHandler(stringWriter, config, javaScriptObjectType, service);

                    await handler.WriteContentAsync(entityMetadata.EntityMetadata, objectName, constructorName, null);
                }

                File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

                AddFileToVSProject(_selectedItem, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionCreatedEntityMetadataFileFormat3, service.ConnectionData.Name, entityMetadata.LogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.LogicalName);
        }

        private async void mIClearEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _cacheEntityMetadata.Remove(connectionData.ConnectionId);

                UpdateButtonsEnable();

                await ShowExistingEntities();
            }
        }

        #region Clipboard Entity

        private void mIClipboardEntityCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataListViewItem>(e, ent => ent.LogicalName);
        }

        private void mIClipboardEntityCopyDisplayName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataListViewItem>(e, ent => ent.DisplayName);
        }

        private void mIClipboardEntityCopyObjectTypeCode_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataListViewItem>(e, ent => ent.ObjectTypeCode.ToString());
        }

        private void mIClipboardEntityCopyEntityMetadataId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataListViewItem>(e, ent => ent.EntityMetadata.MetadataId.ToString());
        }

        #endregion Clipboard Entity

        private void miEntityOperations_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            var items = menuItem.Items.OfType<Control>();

            ActivateControls(items, !_blockNotMetadata, "blockNotMetadata");
        }
    }
}