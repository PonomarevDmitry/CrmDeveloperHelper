using Microsoft.Xrm.Sdk.Query;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerPluginAssembly : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        public WindowOrganizationComparerPluginAssembly(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            this.Resources["ConnectionName1"] = connection1.Name;
            this.Resources["ConnectionName2"] = connection2.Name;

            LoadFromConfig();

            if (!string.IsNullOrEmpty(filter))
            {
                txtBFilter.Text = filter;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwPluginAssemblies.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingPluginAssemblies();
        }

        private void FillExplorersMenuItems()
        {
            //var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1
            //    , getReportName: GetReportName1
            //);

            //var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2
            //    , getReportName: GetReportName2
            //);

            //explorersHelper1.FillExplorers(miExplorers1);
            //explorersHelper2.FillExplorers(miExplorers2);

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2())
                , getPluginAssemblyName: GetPluginAssemblyName
            );
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu contextMenu
            )
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    //if (string.Equals(item.Uid, "miExplorers1", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    explorersHelper1.FillExplorers(item);
                    //}
                    //else if (string.Equals(item.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    explorersHelper2.FillExplorers(item);
                    //}
                    //else
                    if (string.Equals(item.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase))
                    {
                        compareWindowsHelper.FillCompareWindows(item);
                    }
                }
            }
        }

        private string GetPluginAssemblyName()
        {
            var entity = GetSelectedEntity();

            return entity?.Name ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();

            BindingOperations.ClearAllBindings(cmBConnection1);
            cmBConnection1.Items.DetachFromSourceCollection();
            cmBConnection1.DataContext = null;
            cmBConnection1.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBConnection2);
            cmBConnection2.Items.DetachFromSourceCollection();
            cmBConnection2.DataContext = null;
            cmBConnection2.ItemsSource = null;
        }

        private ConnectionData GetConnection1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private ConnectionData GetConnection2()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService1()
        {
            return GetOrganizationService(GetConnection1());
        }

        private Task<IOrganizationServiceExtented> GetService2()
        {
            return GetOrganizationService(GetConnection2());
        }

        private async Task ShowExistingPluginAssemblies()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingPluginAssemblies);

            this._itemsSource.Clear();

            var textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<LinkedEntities<PluginAssembly>> list = Enumerable.Empty<LinkedEntities<PluginAssembly>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var columnSet = new ColumnSet(PluginAssembly.Schema.Attributes.pluginassemblyid, PluginAssembly.Schema.Attributes.name);

                    var temp = new List<LinkedEntities<PluginAssembly>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new PluginAssemblyRepository(service1);
                        var repository2 = new PluginAssemblyRepository(service2);

                        var task1 = repository1.GetPluginAssembliesAsync(textName, columnSet);
                        var task2 = repository2.GetPluginAssembliesAsync(textName, columnSet);

                        var list1 = await task1;
                        var list2 = await task2;

                        foreach (var assembly1 in list1)
                        {
                            var assembly2 = list2.FirstOrDefault(c => c.Name == assembly1.Name);

                            if (assembly2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedEntities<PluginAssembly>(assembly1, assembly2));
                        }
                    }
                    else
                    {
                        var repository1 = new PluginAssemblyRepository(service1);

                        var task1 = repository1.GetPluginAssembliesAsync(textName, columnSet);

                        var list1 = await task1;

                        foreach (var assembly1 in list1)
                        {
                            temp.Add(new LinkedEntities<PluginAssembly>(assembly1, null));
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            LoadEntities(list);
        }

        private class EntityViewItem
        {
            public string Name => Link.Entity1?.Name;

            public LinkedEntities<PluginAssembly> Link { get; private set; }

            public EntityViewItem(LinkedEntities<PluginAssembly> link)
            {
                this.Link = link;
            }
        }

        private void LoadEntities(IEnumerable<LinkedEntities<PluginAssembly>> results)
        {
            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                foreach (var link in results.OrderBy(ent => ent.Entity1.Name))
                {
                    var item = new EntityViewItem(link);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwPluginAssemblies.Items.Count == 1)
                {
                    this.lstVwPluginAssemblies.SelectedItem = this.lstVwPluginAssemblies.Items[0];
                }
            });

            ToggleControls(true, Properties.OutputStrings.LoadingPluginAssembliesCompletedFormat1, results.Count());
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(null, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            ToggleControls(enabled, statusFormat, args);
        }

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.cmBConnection1, this.cmBConnection2);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwPluginAssemblies.SelectedItems.Count > 0;

                    var item = (this.lstVwPluginAssemblies.SelectedItems[0] as EntityViewItem);

                    tSDDBShowDifference.IsEnabled = enabled && item.Link.Entity1 != null && item.Link.Entity2 != null;
                    tSDDBConnection1.IsEnabled = enabled && item.Link.Entity1 != null;
                    tSDDBConnection2.IsEnabled = enabled && item.Link.Entity2 != null;
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
                await ShowExistingPluginAssemblies();
            }
        }

        private EntityViewItem GetSelectedEntity()
        {
            return this.lstVwPluginAssemblies.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwPluginAssemblies.SelectedItems.OfType<EntityViewItem>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    ExecuteActionOnLinkedEntities(item.Link, false, PerformShowingDifferenceAllAsync);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteActionOnLinkedEntities(LinkedEntities<PluginAssembly> linked, bool showAllways, Func<LinkedEntities<PluginAssembly>, bool, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(linked, showAllways);
        }

        private void btnShowDifferenceAll_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnLinkedEntities(link.Link, false, PerformShowingDifferenceAllAsync);
        }

        private async Task PerformShowingDifferenceAllAsync(LinkedEntities<PluginAssembly> linked, bool showAllways)
        {
            await PerformShowingDifferenceAssemblyDescriptionAsync(linked, showAllways);

            //await PerformShowingDifferenceEntityDescriptionAsync(linked, showAllways);
        }

        private void mIShowDifferenceAssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnLinkedEntities(link.Link, true, PerformShowingDifferenceAssemblyDescriptionAsync);
        }

        private async Task PerformShowingDifferenceAssemblyDescriptionAsync(LinkedEntities<PluginAssembly> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferencePluginAssemblyDescriptionFormat1, linked.Entity1.Name);

            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null)
            {
                var handler1 = new PluginAssemblyDescriptionHandler(service1, service1.ConnectionData.GetConnectionInfo());
                var handler2 = new PluginAssemblyDescriptionHandler(service2, service2.ConnectionData.GetConnectionInfo());

                DateTime now = DateTime.Now;

                string desc1 = await handler1.CreateDescriptionAsync(linked.Entity1.Id, linked.Entity1.Name, now);
                string desc2 = await handler2.CreateDescriptionAsync(linked.Entity2.Id, linked.Entity2.Name, now);

                if (showAllways || desc1 != desc2)
                {
                    string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, linked.Entity1.Name, "Description", desc1);
                    string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, linked.Entity2.Name, "Description", desc2);

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                        this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                    }
                }
            }

            ToggleControls(true, Properties.OutputStrings.ShowingDifferencePluginAssemblyDescriptionCompletedFormat1, linked.Entity1.Name);
        }

        private void mIShowDifferenceEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnLinkedEntities(link.Link, true, PerformShowingDifferenceEntityDescriptionAsync);
        }

        private async Task PerformShowingDifferenceEntityDescriptionAsync(LinkedEntities<PluginAssembly> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceEntityDescription);

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var repository1 = new PluginAssemblyRepository(service1);
                    var repository2 = new PluginAssemblyRepository(service2);

                    var assembly1 = await repository1.GetAssemblyByIdRetrieveRequestAsync(linked.Entity1.Id, new ColumnSet(true));
                    var assembly2 = await repository2.GetAssemblyByIdRetrieveRequestAsync(linked.Entity2.Id, new ColumnSet(true));

                    var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(assembly1);
                    var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(assembly2);

                    if (showAllways || desc1 != desc2)
                    {
                        string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, assembly1.Name, EntityFileNameFormatter.Headers.EntityDescription, desc1);
                        string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, assembly2.Name, EntityFileNameFormatter.Headers.EntityDescription, desc2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionFailed);
            }
        }

        private void mIExportPluginAssembly1AssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionPluginAssemblyDescription(link.Link.Entity1.Id, link.Link.Entity1.Name, GetService1, PerformExportAssemblyDescriptionToFileAsync);
        }

        private void mIExportPluginAssembly2AssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionPluginAssemblyDescription(link.Link.Entity2.Id, link.Link.Entity2.Name, GetService2, PerformExportAssemblyDescriptionToFileAsync);
        }

        private void ExecuteActionPluginAssemblyDescription(
            Guid pluginAssemblyId
            , string assemblyName
            , Func<Task<IOrganizationServiceExtented>> getService
            , Func<Guid, string, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(pluginAssemblyId, assemblyName, getService);
        }

        private async Task PerformExportAssemblyDescriptionToFileAsync(Guid idAssembly, string assemblyName, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingPluginAssebmltyDescriptionFormat1, assemblyName);

            var service = await getService();

            if (service != null)
            {
                var repository = new PluginAssemblyRepository(service);

                var assembly = await repository.GetAssemblyByIdRetrieveRequestAsync(idAssembly, new ColumnSet(PluginAssembly.Schema.Attributes.name));

                PluginAssemblyDescriptionHandler handler = new PluginAssemblyDescriptionHandler(service, service.ConnectionData.GetConnectionInfo());

                string description = await handler.CreateDescriptionAsync(assembly.Id, assembly.Name, DateTime.Now);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, assembly.Name, "Description", description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.CreatingPluginAssebmltyDescriptionCompletedFormat1, assemblyName);
        }

        private void mIExportPluginAssembly1EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionPluginAssemblyDescription(link.Link.Entity1.Id, link.Link.Entity1.Name, GetService1, PerformExportEntityDescriptionToFileAsync);
        }

        private void mIExportPluginAssembly2EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionPluginAssemblyDescription(link.Link.Entity2.Id, link.Link.Entity2.Name, GetService2, PerformExportEntityDescriptionToFileAsync);
        }

        private void mIExportPluginAssembly1BinaryContent_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionPluginAssemblyDescription(link.Link.Entity1.Id, link.Link.Entity1.Name, GetService1, PerformDownloadBinaryContent);
        }

        private void mIExportPluginAssembly2BinaryContent_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionPluginAssemblyDescription(link.Link.Entity2.Id, link.Link.Entity2.Name, GetService2, PerformDownloadBinaryContent);
        }

        private async Task PerformDownloadBinaryContent(Guid pluginAssemblyId, string assemblyName, Func<Task<IOrganizationServiceExtented>> getService)
        {
            ToggleControls(false, Properties.OutputStrings.ExportingPluginAssemblyBodyBinaryFormat1, assemblyName);

            var service = await getService();

            if (service != null)
            {
                var repository = new PluginAssemblyRepository(service);

                var assembly = await repository.GetAssemblyByIdRetrieveRequestAsync(pluginAssemblyId, new ColumnSet(PluginAssembly.Schema.Attributes.content));

                string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, assembly.Name, "Content", FileExtension.dll);
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                var array = Convert.FromBase64String(assembly.Content);

                File.WriteAllBytes(filePath, array);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, PluginAssembly.Schema.EntityLogicalName, assembly.Name, "Content", filePath);

                if (File.Exists(filePath))
                {
                    if (_commonConfig.DefaultFileAction != FileAction.None)
                    {
                        this._iWriteToOutput.SelectFileInFolder(service.ConnectionData, filePath);
                    }
                }
            }

            ToggleControls(true, Properties.OutputStrings.ExportingPluginAssemblyBodyBinaryCompletedFormat1, assemblyName);
        }

        private async Task PerformExportEntityDescriptionToFileAsync(Guid pluginAssemblyId, string assemblyName, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new PluginAssemblyRepository(service);

                var assembly = await repository.GetAssemblyByIdRetrieveRequestAsync(pluginAssemblyId, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(assembly, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, assembly.Name, EntityFileNameFormatter.Headers.EntityDescription, description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        private Task<string> CreateDescriptionFileAsync(ConnectionData connectionData, string name, string fieldTitle, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionData, name, fieldTitle, description));
        }

        private string CreateDescriptionFile(ConnectionData connectionData, string name, string fieldTitle, string description)
        {
            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(connectionData.Name, name, fieldTitle, FileExtension.txt);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, connectionData.Name, PluginAssembly.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, connectionData.Name, PluginAssembly.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingPluginAssemblies();
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();

                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    this.Resources["ConnectionName1"] = connection1.Name;
                    this.Resources["ConnectionName2"] = connection2.Name;

                    UpdateButtonsEnable();

                    var task = ShowExistingPluginAssemblies();
                }
            });
        }

        private async void btnExportPluginAssembly1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenPluginAssemblyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.Name ?? txtBFilter.Text);
        }

        private async void btnExportPluginAssembly2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenPluginAssemblyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.Name ?? txtBFilter.Text);
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            EntityViewItem linkedEntityMetadata = GetItemFromRoutedDataContext<EntityViewItem>(e);

            var hasTwoEntities = linkedEntityMetadata != null
                && linkedEntityMetadata.Link != null
                && linkedEntityMetadata.Link.Entity1 != null
                && linkedEntityMetadata.Link.Entity2 != null;

            var hasSecondEntity = linkedEntityMetadata != null
                && linkedEntityMetadata.Link != null
                && linkedEntityMetadata.Link.Entity2 != null;

            var items = contextMenu.Items.OfType<Control>();

            ActivateControls(items, hasTwoEntities, "menuContextDifference");

            ActivateControls(items, hasSecondEntity, "menuContextConnection2");
        }
    }
}