using Microsoft.Xrm.Sdk.Query;
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
    public partial class WindowExportPluginType : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private int _init = 0;

        public WindowExportPluginType(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection
            )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(_iWriteToOutput, service, true);

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwPluginTypes.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (service != null)
            {
                ShowExistingPluginTypes();
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
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

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

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(_iWriteToOutput, service, true);
                }

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingPluginTypes()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading Plugin Types...");

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<PluginType> list = Enumerable.Empty<PluginType>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new PluginTypeRepository(service);
                    list = await repository.GetPluginTypesAsync(textName, new ColumnSet(PluginType.Schema.Attributes.typename, PluginType.Schema.Attributes.pluginassemblyid));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            LoadPluginTypes(list);
        }

        private class EntityViewItem
        {
            public string PluginTypeName { get; private set; }

            public PluginType PluginType { get; private set; }

            public EntityViewItem(string pluginTypeName, PluginType entity)
            {
                this.PluginTypeName = pluginTypeName;
                this.PluginType = entity;
            }
        }

        private void LoadPluginTypes(IEnumerable<PluginType> results)
        {
            this._iWriteToOutput.WriteToOutput("Found {0} Plugin Types.", results.Count());

            this.lstVwPluginTypes.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results.OrderBy(ent => ent.TypeName))
                {
                    var item = new EntityViewItem(entity.TypeName, entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwPluginTypes.Items.Count == 1)
                {
                    this.lstVwPluginTypes.SelectedItem = this.lstVwPluginTypes.Items[0];
                }
            });

            UpdateStatus(string.Format("{0} Plugin Types loaded.", results.Count()));

            ToggleControls(true);
        }

        private void UpdateStatus(string msg)
        {
            this.statusBar.Dispatcher.Invoke(() =>
            {
                this.tSSLStatusMessage.Content = msg;
            });
        }

        private void ToggleControls(bool enabled)
        {
            this._controlsEnabled = enabled;

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
            this.lstVwPluginTypes.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwPluginTypes.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportPluginType, btnExportAll };

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
                ShowExistingPluginTypes();
            }
        }

        private PluginType GetSelectedEntity()
        {
            PluginType result = null;

            if (this.lstVwPluginTypes.SelectedItems.Count == 1
                && this.lstVwPluginTypes.SelectedItems[0] != null
                && this.lstVwPluginTypes.SelectedItems[0] is EntityViewItem
                )
            {
                result = (this.lstVwPluginTypes.SelectedItems[0] as EntityViewItem).PluginType;
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null && item.PluginType != null)
                {
                    ExecuteAction(item.PluginType.Id, item.PluginType.TypeName, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idPluginType, string name)
        {
            await PerformExportPluginTypeDescription(folder, idPluginType, name);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idPluginType, string name, Func<string, Guid, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!_controlsEnabled)
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

            await action(folder, idPluginType, name);
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idPluginType, string name)
        {
            await PerformExportPluginTypeDescription(folder, idPluginType, name);

            await PerformExportEntityDescription(folder, idPluginType, name);
        }

        private void mICreatePluginTypeDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformExportPluginTypeDescription);
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformExportEntityDescription);
        }

        private async Task PerformExportPluginTypeDescription(string folder, Guid idPluginType, string name)
        {
            ToggleControls(false);

            UpdateStatus("Start exporting PluginType Description.");

            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, name, "Description");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var repStep = new SdkMessageProcessingStepRepository(service);
            var repImage = new SdkMessageProcessingStepImageRepository(service);
            var repSecure = new SdkMessageProcessingStepSecureConfigRepository(service);

            var allSteps = await repStep.GetAllStepsByPluginTypeAsync(idPluginType);
            var queryImage = await repImage.GetImagesByPluginTypeAsync(idPluginType);
            var listSecure = await repSecure.GetAllSdkMessageProcessingStepSecureConfigAsync();

            bool hasDescription = await PluginTypeDescriptionHandler.CreateFileWithDescriptionAsync(
                service.ConnectionData.GetConnectionInfo()
                , filePath
                , idPluginType
                , name
                , allSteps
                , queryImage
                , listSecure
                );

            if (hasDescription)
            {
                this._iWriteToOutput.WriteToOutput("PluginType {0} Description exported to {1}", name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("PluginType {0} Description is empty.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            UpdateStatus("Operation is completed.");

            ToggleControls(true);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idPluginType, string name)
        {
            ToggleControls(false);

            UpdateStatus("Start exporting PluginType Entity Description.");

            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, name, "EntityDescription");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var repository = new PluginTypeRepository(service);

            var pluginType = await repository.GetPluginTypeByIdAsync(idPluginType);

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, pluginType, null, service.ConnectionData);

            this._iWriteToOutput.WriteToOutput("Plugin Type {0} Entity Description exported to {1}", name, filePath);

            this._iWriteToOutput.PerformAction(filePath, _commonConfig);

            this._iWriteToOutput.WriteToOutput("End creating file at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            UpdateStatus("Operation is completed.");

            ToggleControls(true);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingPluginTypes();
            }

            base.OnKeyDown(e);
        }

        private void mIExportPlugiTypeDependentComponents_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformCreatingFileWithDependentComponents);
        }

        private void mIExportPlugiTypeRequiredComponents_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformCreatingFileWithRequiredComponents);
        }

        private void mIExportPlugiTypeDependenciesForDelete_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformCreatingFileWithDependenciesForDelete);
        }

        private async Task PerformCreatingFileWithDependentComponents(string folder, Guid idPluginType, string name)
        {
            var service = await GetService();
            var descriptor = await GetDescriptor();

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            this._iWriteToOutput.WriteToOutput("Starting downloading {0}", name);

            var removeWrongFromName = FileOperations.RemoveWrongSymbols(name);

            var dependencyRepository = new DependencyRepository(service);

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.PluginType, idPluginType);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                string fileName = EntityFileNameFormatter.GetPluginTypeFileName(
                    service.ConnectionData.Name
                    , removeWrongFromName
                    , "Dependent Components"
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("PluginType {0} Dependent Components exported to {1}", name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("PluginType {0} has no Dependent Components.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        private async Task PerformCreatingFileWithRequiredComponents(string folder, Guid idPluginType, string name)
        {
            this._iWriteToOutput.WriteToOutput("Starting downloading {0}", name);

            var removeWrongFromName = FileOperations.RemoveWrongSymbols(name);

            var service = await GetService();
            var descriptor = await GetDescriptor();

            var dependencyRepository = new DependencyRepository(service);

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var coll = await dependencyRepository.GetRequiredComponentsAsync((int)ComponentType.PluginType, idPluginType);

            string description = await descriptorHandler.GetDescriptionRequiredAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                string fileName = EntityFileNameFormatter.GetPluginTypeFileName(
                    service.ConnectionData.Name
                    , removeWrongFromName
                    , "Required Components"
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("PluginType {0} Required Components exported to {1}", name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("PluginType {0} has no Required Components.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        private async Task PerformCreatingFileWithDependenciesForDelete(string folder, Guid idPluginType, string name)
        {
            var service = await GetService();
            var descriptor = await GetDescriptor();

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            this._iWriteToOutput.WriteToOutput("Starting downloading {0}", name);

            var removeWrongFromName = FileOperations.RemoveWrongSymbols(name);

            var dependencyRepository = new DependencyRepository(service);

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var coll = await dependencyRepository.GetDependenciesForDeleteAsync((int)ComponentType.PluginType, idPluginType);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                string fileName = EntityFileNameFormatter.GetPluginTypeFileName(
                    service.ConnectionData.Name
                    , removeWrongFromName
                    , "Dependencies For Delete"
                    );

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("PluginType {0} Dependencies For Delete exported to {1}", name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("PluginType {0} has no Dependencies For Delete.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        private async void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.PluginType, entity.Id);
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

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                _commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        var contr = new SolutionController(this._iWriteToOutput);

                        contr.ExecuteAddingComponentesIntoSolution(connectionData, _commonConfig, solutionUniqueName, ComponentType.PluginAssembly, new[] { entity.PluginAssemblyId.Id }, null, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                });

                backWorker.Start();
            }
        }

        private async void mIAddAssemblyStepsIntoSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddAssemblyStepsIntoSolution(true, null);
        }

        private async void mIAddAssemblyStepsIntoSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyStepsIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddAssemblyStepsIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            var repository = new SdkMessageProcessingStepRepository(service);

            var steps = await repository.GetAllStepsByPluginAssemblyAsync(entity.PluginAssemblyId.Id);

            if (!steps.Any())
            {
                return;
            }

            _commonConfig.Save();

            var backWorker = new Thread(() =>
            {
                try
                {
                    this._iWriteToOutput.ActivateOutputWindow();

                    var contr = new SolutionController(this._iWriteToOutput);

                    contr.ExecuteAddingComponentesIntoSolution(service.ConnectionData, _commonConfig, solutionUniqueName, ComponentType.SDKMessageProcessingStep, steps.Select(s => s.Id), null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            });

            backWorker.Start();
        }

        private async void mIAddPluginTypeStepsIntoSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddPluginTypeStepsIntoSolution(true, null);
        }

        private async void mIAddPluginTypeStepsIntoSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddPluginTypeStepsIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddPluginTypeStepsIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            var repository = new SdkMessageProcessingStepRepository(service);

            var steps = await repository.GetAllStepsByPluginTypeAsync(entity.Id);

            if (!steps.Any())
            {
                return;
            }

            _commonConfig.Save();

            var backWorker = new Thread(() =>
            {
                try
                {
                    this._iWriteToOutput.ActivateOutputWindow();

                    var contr = new SolutionController(this._iWriteToOutput);

                    contr.ExecuteAddingComponentesIntoSolution(service.ConnectionData, _commonConfig, solutionUniqueName, ComponentType.SDKMessageProcessingStep, steps.Select(s => s.Id), null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            });

            backWorker.Start();
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                string[] commands = { "contMnAddPluginAssemblyIntoSolutionLast", "contMnAddPluginTypeStepsIntoSolutionLast", "contMnAddPluginAssemblyStepsIntoSolutionLast" };
                Action<object, RoutedEventArgs>[] actions = { AddIntoCrmSolutionLast_Click, mIAddPluginTypeStepsIntoSolutionLast_Click, mIAddAssemblyStepsIntoSolutionLast_Click };

                for (int index = 0; index < commands.Length; index++)
                {
                    var lastSolution = items.FirstOrDefault(i => string.Equals(i.Uid, commands[index], StringComparison.InvariantCultureIgnoreCase));

                    if (lastSolution != null)
                    {
                        lastSolution.Items.Clear();

                        lastSolution.IsEnabled = false;
                        lastSolution.Visibility = Visibility.Collapsed;

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

                                menuItem.Click += new RoutedEventHandler(actions[index]);

                                lastSolution.Items.Add(menuItem);
                            }
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
            var descriptor = await GetDescriptor();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , service
                , descriptor
                , _commonConfig
                , (int)ComponentType.PluginType
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
                , (int)ComponentType.PluginType
                , entity.Id
            );
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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ShowExistingPluginTypes();
            }
        }
    }
}