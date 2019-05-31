using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
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

        private readonly Popup _optionsPopup;

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly string _filePath;
        private readonly bool _isJavaScript;
        private readonly bool _blockNotMetadata;

        private readonly EnvDTE.SelectedItem _selectedItem;

        private readonly ObservableCollection<EntityMetadataListViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();

        public WindowExportEntityMetadata(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , IEnumerable<EntityMetadata> allEntities
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

            this._blockNotMetadata = selectedItem != null || !string.IsNullOrEmpty(_filePath);

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            if (allEntities != null)
            {
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = allEntities;
            }

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

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

            this.DecreaseInit();

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
            txtBNamespaceClassesCSharp.DataContext = cmBCurrentConnection;
            txtBNamespaceClassesJavaScript.DataContext = cmBCurrentConnection;

            txtBNamespaceOptionSetsCSharp.DataContext = cmBCurrentConnection;
            txtBNamespaceOptionSetsJavaScript.DataContext = cmBCurrentConnection;

            txtBTypeConverterName.DataContext = cmBCurrentConnection;

            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

            (cmBCurrentConnection.SelectedItem as ConnectionData)?.Save();

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

            _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
            _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            try
            {
                var service = await QuickConnection.ConnectAsync(connectionData);

                if (service != null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    return service;
                }
                else
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                }
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

        private async Task ShowExistingEntities()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingEntities);

            _itemsSource.Clear();

            IEnumerable<EntityMetadata> list = Enumerable.Empty<EntityMetadata>();

            try
            {
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
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string textName = string.Empty;

            txtBFilterEnitity.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadEntities(list);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingEntitiesCompletedFormat1, list.Count());
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

        private async void miEntityPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, entityMetadataList);
        }

        private async void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, entityMetadataList, null);
        }

        private async void miGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , string.Empty
                , entity.EntityLogicalName
            );
        }

        private async void miSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
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

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
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

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
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

        private void miCreateCSharpFileSchemaMetadata_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, CreateEntityMetadataFileCSharpSchemaAsync);
        }

        private void miCreateCSharpFileProxyClass_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, CreateEntityMetadataFileCSharpProxyClassAsync);
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var entity = ((FrameworkElement)e.OriginalSource).DataContext as EntityMetadataListViewItem;

                if (entity != null)
                {
                    if (_isJavaScript && !string.IsNullOrEmpty(_filePath))
                    {
                        ExecuteActionAsync(entity, CreateEntityMetadataFileJavaScriptAsync);
                    }
                    else
                    {
                        ExecuteActionAsync(entity, CreateEntityMetadataFileCSharpSchemaAsync);
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

            await action(folder, entityMetadata);
        }

        private async Task CreateEntityMetadataFileCSharpSchemaAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.GenerateCommonIndentType, _commonConfig.GenerateCommonSpaceCount);

                var config = CreateFileCSharpConfiguration.CreateForSchemaEntity(service.ConnectionData.NamespaceClassesCSharp, service.ConnectionData.NamespaceOptionSetsCSharp, service.ConnectionData.TypeConverterName, _commonConfig);

                string fileName = string.Format("{0}.{1}.Generated.cs", service.ConnectionData.Name, entityMetadata.EntityMetadata.SchemaName);

                if (this._selectedItem != null)
                {
                    fileName = string.Format("{0}.Generated.cs", entityMetadata.EntityMetadata.SchemaName);
                }

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                if (!_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                var repository = new EntityMetadataRepository(service);

                ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
                INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
                ITypeMappingService typeMappingService = new TypeMappingService(service.ConnectionData.NamespaceClassesCSharp);
                ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);
                IMetadataProviderService metadataProviderService = new MetadataProviderService(repository);

                ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

                using (var handler = new CreateFileWithEntityMetadataCSharpHandler(config, service, _iWriteToOutput, codeGenerationServiceProvider))
                {
                    await handler.CreateFileAsync(filePath, entityMetadata.EntityLogicalName);
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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private async Task CreateEntityMetadataFileCSharpProxyClassAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.GenerateCommonIndentType, _commonConfig.GenerateCommonSpaceCount);

                var config = CreateFileCSharpConfiguration.CreateForProxyClass(service.ConnectionData.NamespaceClassesCSharp, service.ConnectionData.NamespaceOptionSetsCSharp, _commonConfig);

                string fileName = string.Format("{0}.{1}.cs", service.ConnectionData.Name, entityMetadata.EntityMetadata.SchemaName);

                if (this._selectedItem != null)
                {
                    fileName = string.Format("{0}.cs", entityMetadata.EntityMetadata.SchemaName);
                }

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                if (!_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                var repository = new EntityMetadataRepository(service);

                ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
                INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
                ITypeMappingService typeMappingService = new TypeMappingService(service.ConnectionData.NamespaceClassesCSharp);
                ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);
                IMetadataProviderService metadataProviderService = new MetadataProviderService(repository);

                ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

                var entityMetadataFull = await repository.GetEntityMetadataAsync(entityMetadata.EntityLogicalName);

                CodeGeneratorOptions options = new CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    VerbatimOrder = true,
                };

                await codeGenerationService.WriteEntityFileAsync(entityMetadataFull, filePath, service.ConnectionData.NamespaceClassesCSharp, options, codeGenerationServiceProvider);

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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private void miCreateJavaScriptFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, CreateEntityMetadataFileJavaScriptAsync);
        }

        private async Task CreateEntityMetadataFileJavaScriptAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            string tabSpacer = CreateFileHandler.GetTabSpacer(_commonConfig.GenerateCommonIndentType, _commonConfig.GenerateCommonSpaceCount);

            var config = new CreateFileJavaScriptConfiguration(
                tabSpacer
                , _commonConfig.GenerateSchemaEntityOptionSetsWithDependentComponents
                , _commonConfig.GenerateSchemaIntoSchemaClass
            );

            string fileName = string.Format("{0}.{1}.entitymetadata.generated.js", service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            if (this._selectedItem != null)
            {
                fileName = string.Format("{0}.entitymetadata.generated.js", entityMetadata.EntityLogicalName);
            }

            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (_isJavaScript && !string.IsNullOrEmpty(_filePath))
            {
                filePath = _filePath;
            }

            try
            {
                using (var handler = new CreateFileWithEntityMetadataJavaScriptHandler(config, service, _iWriteToOutput))
                {
                    await handler.CreateFileAsync(filePath, entityMetadata.EntityLogicalName);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityMetadataFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
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

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private void miCreateNewEntityInstance_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(entity, CreateNewEntityInstanceAsync);
        }

        private async Task CreateNewEntityInstanceAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, entityMetadata.EntityMetadata.LogicalName, Guid.Empty);
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

            string operation = string.Format(Properties.OperationNames.CreatingFileWithAttributesDependentComponentsFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                bool allComponents = _commonConfig.AttributesDependentComponentsAllComponents;

                string fileName = string.Format("{0}.{1} attributes dependent components at {2}.txt", service.ConnectionData.Name, entityMetadata.EntityLogicalName, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var descriptor = new SolutionComponentDescriptor(service);
                descriptor.SetSettings(_commonConfig);
                var dependencyRepository = new DependencyRepository(service);
                var descriptorHandler = new DependencyDescriptionHandler(descriptor);

                var handler = new EntityAttributesDependentComponentsHandler(dependencyRepository, descriptor, descriptorHandler);

                string message = await handler.CreateFileAsync(entityMetadata.EntityLogicalName, filePath, allComponents);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, message);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
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

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.GettingEntityXmlFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingFileForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                var repository = new EntityMetadataRepository(service);

                var fileName = string.Format("{0}.{1} - EntityXml at {2}.xml", service.ConnectionData.Name, entityMetadata.EntityLogicalName, DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"));
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await repository.ExportEntityXmlAsync(entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedEntityXmlFileForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingFileForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.GettingEntityXmlFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
        }

        private async Task PublishEntityAsync(string folder, EntityMetadataListViewItem entityMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityMetadata.EntityLogicalName });

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
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
                connectionData.OpenEntityInstanceListInWeb(entity.EntityLogicalName);
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

        private async void AddToCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, RootComponentBehavior.IncludeSubcomponents);
        }

        private async void AddToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, RootComponentBehavior.DoNotIncludeSubcomponents);
        }

        private async void AddToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, RootComponentBehavior.IncludeAsShellOnly);
        }

        private async void AddToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, RootComponentBehavior.IncludeSubcomponents);
            }
        }

        private async void AddToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, RootComponentBehavior.DoNotIncludeSubcomponents);
            }
        }

        private async void AddToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, RootComponentBehavior.IncludeAsShellOnly);
            }
        }

        private async Task AddToSolution(bool withSelect, string solutionUniqueName, RootComponentBehavior rootComponentBehavior)
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

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entity.EntityMetadata.MetadataId.Value }, rootComponentBehavior, withSelect);
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

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddToSolutionLastIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddToSolutionLastDoNotIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddToSolutionLastIncludeAsShellOnly");

                ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddToSolutionLast");
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

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Entity, entity.EntityMetadata.MetadataId.Value, null);
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

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                string ribbonXml = await repository.ExportEntityRibbonAsync(entityMetadata.EntityLogicalName, _commonConfig.GetRibbonLocationFilters());

                ribbonXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonXml, _commonConfig, XmlOptionsControls.RibbonFull
                    , ribbonEntityName: entityMetadata.EntityLogicalName
                    );

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityMetadata.EntityLogicalName);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, ribbonXml, new UTF8Encoding(false));

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
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

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat1, entityMetadata.EntityLogicalName);

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                var bodyEntityRibbon = await repository.ExportEntityRibbonByteArrayAsync(entityMetadata.EntityLogicalName, _commonConfig.GetRibbonLocationFilters());

                string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entityMetadata.EntityLogicalName, "zip");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllBytes(filePath, bodyEntityRibbon);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
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

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFormat1, entityMetadata.EntityLogicalName);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            try
            {
                string ribbonDiffXml = await repositoryRibbonCustomization.GetRibbonDiffXmlAsync(_iWriteToOutput, entityMetadata.EntityMetadata, null);

                if (string.IsNullOrEmpty(ribbonDiffXml))
                {
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entityMetadata.EntityLogicalName);
                    return;
                }

                ribbonDiffXml = ContentCoparerHelper.FormatXmlByConfiguration(ribbonDiffXml, _commonConfig, XmlOptionsControls.RibbonFull
                    , schemaName: CommonExportXsdSchemasCommand.SchemaRibbonXml
                    , ribbonEntityName: entityMetadata.EntityLogicalName
                    );

                {
                    string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entityMetadata.EntityLogicalName);
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityRibbonDiffXmlForConnectionFormat3, service.ConnectionData.Name, entityMetadata.EntityLogicalName, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityCompletedFormat1, entityMetadata.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entityMetadata.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entityMetadata.EntityLogicalName);
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

            var newText = string.Empty;

            var service = await GetService();

            {
                bool? dialogResult = false;

                var title = string.Format("RibbonDiffXml for {0}", entity.EntityLogicalName);

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + title, title, string.Empty);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityCanceledFormat2, service.ConnectionData.Name, entity.EntityLogicalName);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }

            newText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

            UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.ValidatingRibbonDiffXmlForEntityFormat1, entity.EntityLogicalName);

            if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
            {
                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.TextIsNotValidXml);
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
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ValidatingRibbonDiffXmlForEntityFailedFormat1, entity.EntityLogicalName);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    return;
                }
            }

            var repositoryPublisher = new PublisherRepository(service);
            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            if (publisherDefault == null)
            {
                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.NotFoundedDefaultPublisher);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityFormat2, service.ConnectionData.Name, entity.EntityLogicalName);

            var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

            try
            {
                await repositoryRibbonCustomization.PerformUpdateRibbonDiffXml(_iWriteToOutput, _commonConfig, doc, entity.EntityMetadata, null);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityCompletedFormat2, service.ConnectionData.Name, entity.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityFailedFormat2, service.ConnectionData.Name, entity.EntityLogicalName);
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}