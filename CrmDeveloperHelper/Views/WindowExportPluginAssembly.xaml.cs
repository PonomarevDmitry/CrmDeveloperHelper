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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportPluginAssembly : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private int _init = 0;

        public WindowExportPluginAssembly(
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

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

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

            this.lstVwPluginAssemblies.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (service != null)
            {
                ShowExistingPluginAssemblies();
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

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service);
                }

                _descriptorCache[connectionData.ConnectionId].SetSettings(_commonConfig);

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingPluginAssemblies()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingPluginAssemblies);

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<PluginAssembly> list = Enumerable.Empty<PluginAssembly>();

            try
            {
                if (service != null)
                {
                    var repository = new PluginAssemblyRepository(service);
                    list = await repository.GetPluginAssembliesAsync(textName, new ColumnSet(PluginAssembly.Schema.Attributes.name));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadPluginAssemblies(list);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingPluginAssembliesCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string PluginAssemblyName { get; private set; }

            public PluginAssembly PluginAssembly { get; private set; }

            public EntityViewItem(string pluginAssemblyName, PluginAssembly entity)
            {
                this.PluginAssemblyName = pluginAssemblyName;
                this.PluginAssembly = entity;
            }
        }

        private void LoadPluginAssemblies(IEnumerable<PluginAssembly> results)
        {
            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results.OrderBy(ent => ent.Name))
                {
                    var item = new EntityViewItem(entity.Name, entity);

                    _itemsSource.Add(item);
                }

                if (this.lstVwPluginAssemblies.Items.Count == 1)
                {
                    this.lstVwPluginAssemblies.SelectedItem = this.lstVwPluginAssemblies.Items[0];
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
            this._controlsEnabled = enabled;

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(enabled, this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, btnNewPluginAssembly);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwPluginAssemblies.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportPluginAssembly, btnExportAll };

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
                ShowExistingPluginAssemblies();
            }
        }

        private PluginAssembly GetSelectedEntity()
        {
            return this.lstVwPluginAssemblies.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwPluginAssemblies.SelectedItems.OfType<EntityViewItem>().Select(e => e.PluginAssembly).SingleOrDefault() : null;
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

                if (item != null)
                {
                    ExecuteAction(item.PluginAssembly.Id, item.PluginAssembly.Name, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idPluginAssembly, string name)
        {
            await PerformExportAssemblyDescription(folder, idPluginAssembly, name);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idAssembly, string name, Func<string, Guid, string, Task> action)
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

            try
            {
                await action(folder, idAssembly, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idPluginAssembly, string name)
        {
            await PerformExportAssemblyDescription(folder, idPluginAssembly, name);

            //await PerformExportEntityDescription(folder, idPluginAssembly, name);
        }

        private void mICreatePluginAssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, PerformExportAssemblyDescription);
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, PerformExportEntityDescription);
        }

        private void mIExportPluginAssemblyBinaryContent_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, ExecuteExportAssembly);
        }

        private async Task PerformExportAssemblyDescription(string folder, Guid idPluginAssembly, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingPluginAssebmltyDescriptionFormat1, name);

            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, name, "Description", "txt");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            PluginAssemblyDescriptionHandler handler = new PluginAssemblyDescriptionHandler(service, service.ConnectionData.GetConnectionInfo());

            await handler.CreateFileWithDescriptionAsync(filePath, idPluginAssembly, name, DateTime.Now);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, PluginAssembly.Schema.EntityLogicalName, name, "Description", filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingPluginAssebmltyDescriptionCompletedFormat1, name);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idPluginAssembly, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingEntityDescription);

            var repository = new PluginAssemblyRepository(service);

            var assembly = await repository.GetAssemblyByIdAsync(idPluginAssembly);

            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, name, "EntityDescription", "txt");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, assembly, EntityFileNameFormatter.PluginAssemblyIgnoreFields, service.ConnectionData);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                , service.ConnectionData.Name
                , assembly.LogicalName
                , filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
        }

        private async Task ExecuteExportAssembly(string folder, Guid idAssembly, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingPluginAssemblyBodyBinaryFormat1, name);

            var repository = new PluginAssemblyRepository(service);

            var assembly = await repository.GetAssemblyByIdAsync(idAssembly, new ColumnSet(PluginAssembly.Schema.Attributes.content));

            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, name, "Content", "dll");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var array = Convert.FromBase64String(assembly.Content);

            File.WriteAllBytes(filePath, array);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, PluginAssembly.Schema.EntityLogicalName, name, "Content", filePath);

            if (File.Exists(filePath))
            {
                if (_commonConfig.DefaultFileAction != FileAction.None)
                {
                    this._iWriteToOutput.SelectFileInFolder(service.ConnectionData, filePath);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingPluginAssemblyBodyBinaryCompletedFormat1, name);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingPluginAssemblies();
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.PluginAssembly, entity.Id);
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
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.PluginAssembly, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
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

            var steps = await repository.GetAllStepsByPluginAssemblyAsync(entity.Id);

            if (!steps.Any())
            {
                return;
            }

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SdkMessageProcessingStep, steps.Select(s => s.Id), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private void mICompareWithLocalAssembly_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, PerformComparingAssembly);
        }

        private async Task PerformComparingAssembly(string folder, Guid idPluginAssembly, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ComparingPluginAssemblyWithLocalAssemblyFormat1, name);

            var controller = new PluginTypeDescriptionController(_iWriteToOutput);

            string filePath = await controller.CreateFileWithAssemblyComparing(folder, service, idPluginAssembly, name, null);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingPluginAssemblyWithLocalAssemblyCompletedFormat1, name);
        }

        private async void mIUpdatePluginAssembly_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            var repository = new PluginAssemblyRepository(service);

            var assembly = await repository.GetAssemblyByIdAsync(entity.Id);

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginAssembly(_iWriteToOutput, service, assembly, null);

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
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

                FillLastSolutionItems(connectionData, items, true, mIAddAssemblyStepsIntoSolutionLast_Click, "contMnAddPluginAssemblyStepsIntoSolutionLast");
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
                , (int)ComponentType.PluginAssembly
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
                , (int)ComponentType.PluginAssembly
                , entity.Id
                , null
            );
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnComparePluginAssemblies_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerPluginAssemblyWindow(
                _iWriteToOutput
                , _commonConfig
                , service.ConnectionData
                , service.ConnectionData
                , entity?.Name ?? txtBFilter.Text
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
                ShowExistingPluginAssemblies();
            }
        }

        private async void mIOpenPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , null
                , entity.Name
                , null
            );
        }

        private async void btnNewPluginAssembly_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var assembly = new PluginAssembly();

            var form = new WindowPluginAssembly(_iWriteToOutput, service, assembly, null);

            if (form.ShowDialog().GetValueOrDefault())
            {
                ShowExistingPluginAssemblies();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}