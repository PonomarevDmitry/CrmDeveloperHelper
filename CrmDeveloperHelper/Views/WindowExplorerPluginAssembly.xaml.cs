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
    public partial class WindowExplorerPluginAssembly : WindowWithSolutionComponentDescriptor
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        public WindowExplorerPluginAssembly(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string selection
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

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

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingPluginAssemblies();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getPluginAssemblyName: GetPluginAssemblyName
                , getPluginTypeName: GetPluginAssemblyName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getPluginAssemblyName: GetPluginAssemblyName
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            mIOpenPluginTree.Click += explorersHelper.miPluginTree_Click;
            mIOpenPluginTypeExplorer.Click += explorersHelper.miPluginTypes_Click;

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miPluginTree_Click, nameof(mIOpenPluginTree));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miPluginTypes_Click, nameof(mIOpenPluginTypeExplorer));
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

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;
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

        private async Task ShowExistingPluginAssemblies()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingPluginAssemblies);

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
                    list = await repository.GetPluginAssembliesAsync(textName
                        , new ColumnSet
                        (
                            PluginAssembly.Schema.Attributes.name
                            , PluginAssembly.Schema.Attributes.version
                            , PluginAssembly.Schema.Attributes.culture
                            , PluginAssembly.Schema.Attributes.publickeytoken
                            , PluginAssembly.Schema.Attributes.iscustomizable
                            , PluginAssembly.Schema.Attributes.ismanaged
                            , PluginAssembly.Schema.Attributes.ishidden
                            , PluginAssembly.Schema.Attributes.username
                            , PluginAssembly.Schema.Attributes.ispasswordset
                            , PluginAssembly.Schema.Attributes.authtype
                            , PluginAssembly.Schema.Attributes.isolationmode
                            , PluginAssembly.Schema.Attributes.sourcetype
                            , PluginAssembly.Schema.Attributes.description
                            , PluginAssembly.Schema.Attributes.path
                            , PluginAssembly.Schema.Attributes.url
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadPluginAssemblies(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingPluginAssembliesCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string Name => PluginAssembly.Name;

            public string Version => PluginAssembly.Version;

            public string Culture => PluginAssembly.Culture;

            public string PublicKeyToken => PluginAssembly.PublicKeyToken;

            public bool IsCustomizable => PluginAssembly.IsCustomizable.Value;

            public bool IsManaged => PluginAssembly.IsManaged.GetValueOrDefault();

            public bool IsHidden => PluginAssembly.IsHidden.Value;

            public string UserName => PluginAssembly.UserName;

            public bool IsPasswordSet => PluginAssembly.IsPasswordSet.GetValueOrDefault();

            public string AuthType { get; }

            public string IsolationMode { get; }

            public string SourceType { get; }

            public string Description => PluginAssembly.Description;

            public bool HasDescription => !string.IsNullOrEmpty(PluginAssembly.Description);

            public string Path => PluginAssembly.Path;

            public string Url => PluginAssembly.Url;

            public PluginAssembly PluginAssembly { get; }

            public EntityViewItem(PluginAssembly entity)
            {
                entity.FormattedValues.TryGetValue(PluginAssembly.Schema.Attributes.authtype, out var authtype);
                entity.FormattedValues.TryGetValue(PluginAssembly.Schema.Attributes.isolationmode, out var isolationmode);
                entity.FormattedValues.TryGetValue(PluginAssembly.Schema.Attributes.sourcetype, out var sourcetype);

                this.AuthType = authtype;
                this.IsolationMode = isolationmode;
                this.SourceType = sourcetype;

                this.PluginAssembly = entity;
            }
        }

        private void LoadPluginAssemblies(IEnumerable<PluginAssembly> results)
        {
            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results.OrderBy(ent => ent.Name))
                {
                    var item = new EntityViewItem(entity);

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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, btnNewPluginAssembly);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwPluginAssemblies.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwPluginAssemblies.SelectedItems.Count > 0;

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

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingPluginAssemblies();
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

        private async void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    await ExecuteAction(item.PluginAssembly.Id, item.PluginAssembly.Name, PerformExportMouseDoubleClick);
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
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            try
            {
                await action(folder, idAssembly, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idPluginAssembly, string name)
        {
            await PerformExportAssemblyDescription(folder, idPluginAssembly, name);
        }

        private async void mICreatePluginAssemblyDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformExportAssemblyDescription);
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformExportEntityDescription);
        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformEntityEditor);
        }

        private async void mIDeletePluginAssembly_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformDeleteEntity);
        }

        private async void mIExportPluginAssemblyBinaryContent_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, ExecuteExportAssembly);
        }

        private async Task PerformExportAssemblyDescription(string folder, Guid idPluginAssembly, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingPluginAssebmltyDescriptionFormat1, name);

            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, name, "Description", FileExtension.txt);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            PluginAssemblyDescriptionHandler handler = new PluginAssemblyDescriptionHandler(service, service.ConnectionData.GetConnectionInfo());

            await handler.CreateFileWithDescriptionAsync(filePath, idPluginAssembly, name, DateTime.Now);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, PluginAssembly.Schema.EntityLogicalName, name, "Description", filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingPluginAssebmltyDescriptionCompletedFormat1, name);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idPluginAssembly, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            var repository = new PluginAssemblyRepository(service);

            var assembly = await repository.GetAssemblyByIdRetrieveRequestAsync(idPluginAssembly);

            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, name, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, assembly, service.ConnectionData);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                , service.ConnectionData.Name
                , assembly.LogicalName
                , filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        private async Task PerformEntityEditor(string folder, Guid idPluginAssembly, string name)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, PluginAssembly.EntityLogicalName, idPluginAssembly);
        }

        private async Task PerformDeleteEntity(string folder, Guid idPluginAssembly, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, PluginAssembly.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, PluginAssembly.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, PluginAssembly.EntityLogicalName, idPluginAssembly);

                    await service.DeleteAsync(PluginAssembly.EntityLogicalName, idPluginAssembly);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, PluginAssembly.EntityLogicalName);

                await ShowExistingPluginAssemblies();
            }
        }

        private async Task ExecuteExportAssembly(string folder, Guid idAssembly, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingPluginAssemblyBodyBinaryFormat1, name);

            var repository = new PluginAssemblyRepository(service);

            var assembly = await repository.GetAssemblyByIdRetrieveRequestAsync(idAssembly, new ColumnSet(PluginAssembly.Schema.Attributes.content));

            string fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, name, "Content", FileExtension.dll);
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

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingPluginAssemblyBodyBinaryCompletedFormat1, name);
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingPluginAssemblies();
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.PluginAssembly, entity.Id);
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
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = GetSolutionComponentDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.PluginAssembly, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void mIAddAssemblyStepsToSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddAssemblyStepsToSolution(true, null);
        }

        private async void mIAddAssemblyStepsToSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyStepsToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddAssemblyStepsToSolution(bool withSelect, string solutionUniqueName)
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

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SdkMessageProcessingStep, steps.Select(s => s.Id), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void mICompareWithLocalAssembly_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.Name, PerformComparingAssembly);
        }

        private async Task PerformComparingAssembly(string folder, Guid idPluginAssembly, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingPluginAssemblyWithLocalAssemblyFormat1, name);

            var controller = new PluginController(_iWriteToOutput);

            string filePath = await controller.SelecteFileCreateFileWithAssemblyComparing(folder, service, idPluginAssembly, name, null);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingPluginAssemblyWithLocalAssemblyCompletedFormat1, name);
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

            var assembly = await repository.GetAssemblyByIdRetrieveRequestAsync(entity.Id);

            WindowHelper.OpenPluginAssemblyUpdateWindow(
                this._iWriteToOutput
                , service
                , assembly
                , null
            );
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

                FillLastSolutionItems(connectionData, items, true, mIAddAssemblyStepsToSolutionLast_Click, "contMnAddPluginAssemblyStepsToSolutionLast");
            }
        }

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , null
                , _commonConfig
                , (int)ComponentType.PluginAssembly
                , entity.Id
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

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.PluginAssembly
                , entity.Id
                , null
            );
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                await ShowExistingPluginAssemblies();
            }
        }

        private async void btnNewPluginAssembly_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var assembly = new PluginAssembly();

            var form = new WindowPluginAssembly(_iWriteToOutput, service, assembly, null, null);

            if (form.ShowDialog().GetValueOrDefault())
            {
                await ShowExistingPluginAssemblies();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private void lstVwPluginAssemblies_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwPluginAssemblies_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeletePluginAssembly_Click(sender, e);
        }

        #region Clipboard

        private void mIClipboardCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.Name);
        }

        private void mIClipboardCopyVersion_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.Version);
        }

        private void mIClipboardCopyCulture_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.Culture);
        }

        private void mIClipboardCopyPublicKeyToken_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.PublicKeyToken);
        }

        private void mIClipboardCopyPluginAssemblyId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.PluginAssembly.Id.ToString());
        }

        #endregion Clipboard
    }
}