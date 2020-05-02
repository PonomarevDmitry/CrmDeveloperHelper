using Microsoft.Xrm.Sdk;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerPluginType : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, Task> _cacheTaskGettingMessageFilters = new Dictionary<Guid, Task>();

        private readonly Dictionary<Guid, List<SdkMessageFilter>> _cacheMessageFilters = new Dictionary<Guid, List<SdkMessageFilter>>();

        public WindowExplorerPluginType(
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

            this.lstVwPluginTypes.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            if (service != null)
            {
                ShowExistingPluginTypes();
            }
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getPluginAssemblyName: GetPluginAssemblyName
                , getPluginTypeName: GetPluginTypeName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getPluginAssemblyName: GetPluginAssemblyName
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));

                var items = listContextMenu.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    if (string.Equals(item.Uid, "mIOpenPluginTree", StringComparison.InvariantCultureIgnoreCase))
                    {
                        item.Click += explorersHelper.miPluginTree_Click;
                    }
                    else if (string.Equals(item.Uid, "mIOpenPluginAssemblyExplorer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        item.Click += explorersHelper.miPluginAssemblies_Click;
                    }
                }
            }
        }

        private string GetPluginAssemblyName()
        {
            var entity = GetSelectedEntity();

            return entity?.AssemblyName;
        }

        private string GetPluginTypeName()
        {
            var entity = GetSelectedEntity();

            return entity?.TypeName ?? txtBFilter.Text.Trim();
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

        private async Task ShowExistingPluginTypes()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingPluginTypes);

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<PluginType> list = Enumerable.Empty<PluginType>();

            try
            {
                if (service != null)
                {
                    var repository = new PluginTypeRepository(service);
                    list = await repository.GetPluginTypesAsync(textName
                        , new ColumnSet
                        (
                            PluginType.Schema.Attributes.typename
                            , PluginType.Schema.Attributes.name
                            , PluginType.Schema.Attributes.friendlyname
                            , PluginType.Schema.Attributes.assemblyname
                            , PluginType.Schema.Attributes.workflowactivitygroupname
                            , PluginType.Schema.Attributes.customworkflowactivityinfo
                            , PluginType.Schema.Attributes.description
                            , PluginType.Schema.Attributes.ismanaged
                            , PluginType.Schema.Attributes.isworkflowactivity
                            , PluginType.Schema.Attributes.pluginassemblyid
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadPluginTypes(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingPluginTypesCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string AssemblyName => PluginType.AssemblyName;

            public string TypeName => PluginType.TypeName;

            public string Name => PluginType.Name;

            public string FriendlyName => PluginType.FriendlyName;

            public string WorkflowActivityGroupName => PluginType.WorkflowActivityGroupName;

            public bool HasDescription => !string.IsNullOrEmpty(PluginType.Description);

            public string Description => PluginType.Description;

            public bool IsManaged => PluginType.IsManaged.GetValueOrDefault();

            public bool IsWorkflowActivity => PluginType.IsWorkflowActivity.GetValueOrDefault();

            public PluginType PluginType { get; }

            public EntityViewItem(PluginType entity)
            {
                this.PluginType = entity;
            }
        }

        private void LoadPluginTypes(IEnumerable<PluginType> results)
        {
            this.lstVwPluginTypes.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.AssemblyName)
                    .ThenBy(ent => ent.TypeName)
                    .ThenBy(ent => ent.FriendlyName)
                    .ThenBy(ent => ent.Id)
                )
                {
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwPluginTypes.Items.Count == 1)
                {
                    this.lstVwPluginTypes.SelectedItem = this.lstVwPluginTypes.Items[0];
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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwPluginTypes.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwPluginTypes.SelectedItems.Count > 0;

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
            return this.lstVwPluginTypes.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwPluginTypes.SelectedItems.OfType<EntityViewItem>().Select(e => e.PluginType).SingleOrDefault() : null;
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
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

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

            //await PerformExportEntityDescription(folder, idPluginType, name);
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

        private void mIExportCustomWorkflowActivityInfo_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.TypeName, PluginType.Schema.Attributes.customworkflowactivityinfo, PluginType.Schema.Headers.customworkflowactivityinfo, PerformExportXmlToFile);
        }

        private async Task ExecuteActionEntity(Guid idPluginType, string typeName, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idPluginType, typeName, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idPluginType, string typeName, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new PluginTypeRepository(service);

                var pluginType = await repository.GetByIdAsync(idPluginType, new ColumnSet(fieldName));

                string xmlContent = pluginType.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, typeName, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private Task<string> CreateFileAsync(string folder, string typeName, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, typeName, fieldTitle, xmlContent));
        }

        private string CreateFile(string folder, string typeName, string fieldTitle, string xmlContent)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetPluginTypeFileName(connectionData.Name, typeName, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, Workflow.Schema.EntityLogicalName, typeName, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Workflow.Schema.EntityLogicalName, typeName, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
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

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformEntityEditor);
        }

        private void mIDeletePluginType_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformDeleteEntity);
        }

        private async Task PerformExportPluginTypeDescription(string folder, Guid idPluginType, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingPluginTypeDescriptionFormat1, name);

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
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, PluginType.EntityLogicalName, name, "Description", filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, PluginType.EntityLogicalName, name, "Description");
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingPluginTypeDescriptionCompletedFormat1, name);
        }

        private async Task PerformEntityEditor(string folder, Guid idPluginType, string name)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, PluginType.EntityLogicalName, idPluginType);
        }

        private async Task PerformDeleteEntity(string folder, Guid idPluginType, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, PluginType.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, PluginType.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, PluginType.EntityLogicalName, idPluginType);

                    await service.DeleteAsync(PluginType.EntityLogicalName, idPluginType);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, PluginType.EntityLogicalName);

                ShowExistingPluginTypes();
            }
        }

        private async Task PerformExportEntityDescription(string folder, Guid idPluginType, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            string fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, name, EntityFileNameFormatter.Headers.EntityDescription);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var repository = new PluginTypeRepository(service);

            var pluginType = await repository.GetPluginTypeByIdAsync(idPluginType);

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, pluginType, null, service.ConnectionData);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                , service.ConnectionData.Name
                , pluginType.LogicalName
                , filePath);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingPluginTypes();
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

        private async void AddAssemblyToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddAssemblyToSolution(true, null);
        }

        private async void AddAssemblyToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddAssemblyToSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.PluginAssembly, new[] { entity.PluginAssemblyId.Id }, null, withSelect);
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

            var steps = await repository.GetAllStepsByPluginAssemblyAsync(entity.PluginAssemblyId.Id);

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

        private async void mIAddPluginTypeStepsToSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddPluginTypeStepsToSolution(true, null);
        }

        private async void mIAddPluginTypeStepsToSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddPluginTypeStepsToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddPluginTypeStepsToSolution(bool withSelect, string solutionUniqueName)
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

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, mIAddPluginTypeStepsToSolutionLast_Click, "contMnAddPluginTypeStepsToSolutionLast");

                FillLastSolutionItems(connectionData, items, true, AddAssemblyToCrmSolutionLast_Click, "contMnAddPluginAssemblyToSolutionLast");

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
                , (int)ComponentType.PluginType
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
                , (int)ComponentType.PluginType
                , entity.Id
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                ShowExistingPluginTypes();
            }
        }

        private void mIAddPluginStep_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.TypeName, PerformAddingPluginStep);
        }

        private async Task PerformAddingPluginStep(string folder, Guid idPluginType, string name)
        {
            var service = await GetService();

            List<SdkMessageFilter> filters = null;

            if (!_cacheMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
            {
                if (!_cacheTaskGettingMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId] = GetSdkMessageFiltersAsync(service);
                }

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.GettingMessages);

                await _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId];

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.GettingMessagesCompleted);
            }

            filters = _cacheMessageFilters[service.ConnectionData.ConnectionId];

            var step = new SdkMessageProcessingStep()
            {
                EventHandler = new EntityReference(PluginType.EntityLogicalName, idPluginType),
            };

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowSdkMessageProcessingStep(_iWriteToOutput, service, filters, step);

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

        private async Task GetSdkMessageFiltersAsync(IOrganizationServiceExtented service)
        {
            var repository = new SdkMessageFilterRepository(service);

            var filters = await repository.GetAllAsync(new ColumnSet(SdkMessageFilter.Schema.Attributes.sdkmessageid, SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode, SdkMessageFilter.Schema.Attributes.availability));

            if (!_cacheMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
            {
                _cacheMessageFilters.Add(service.ConnectionData.ConnectionId, filters);
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private void lstVwPluginTypes_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.ContinueRouting = false;
        }

        private void lstVwPluginTypes_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeletePluginType_Click(sender, e);
        }
    }
}