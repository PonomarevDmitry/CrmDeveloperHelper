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
    public partial class WindowExplorerSdkMessageFilter : WindowWithSolutionComponentDescriptor
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        public WindowExplorerSdkMessageFilter(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string filterEntity
            , string filterMessage
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            LoadFromConfig();

            cmBEntityName.Text = filterEntity;
            txtBMessageFilter.Text = filterMessage;

            FillEntityNames(service.ConnectionData);

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwMessageFilters.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            FocusOnComboBoxTextBox(cmBEntityName);

            this.DecreaseInit();

            var task = ShowExistingMessageFilters();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getEntityName: GetEntityName
                , getMessageName: GetMessageName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getEntityName: GetEntityName
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            mIOpenMessageExplorer.Click += explorersHelper.miMessageExplorer_Click;
            mIOpenPluginTree.Click += explorersHelper.miPluginTree_Click;
            mIOpenMessageFilterTree.Click += explorersHelper.miMessageFilterTree_Click;
            mIOpenMessageRequestTree.Click += explorersHelper.miMessageRequestTree_Click;

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageExplorer_Click, nameof(mIOpenMessageExplorer));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miPluginTree_Click, nameof(mIOpenPluginTree));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterTree_Click, nameof(mIOpenMessageFilterTree));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageRequestTree_Click, nameof(mIOpenMessageRequestTree));
            }
        }

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.PrimaryObjectTypeCode ?? cmBEntityName.Text.Trim();
        }

        private string GetMessageName()
        {
            var entity = GetSelectedEntity();

            return entity?.MessageName ?? txtBMessageFilter.Text.Trim();
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

        private async Task ShowExistingMessageFilters()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingSdkMessageFilter);

            string entityName = string.Empty;
            string messageName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();

                entityName = cmBEntityName.Text?.Trim();
                messageName = txtBMessageFilter.Text.Trim();
            });

            IEnumerable<SdkMessageFilter> list = Enumerable.Empty<SdkMessageFilter>();

            try
            {
                if (service != null)
                {
                    var repository = new SdkMessageFilterRepository(service);

                    list = await repository.GetAllSdkMessageFiltersWithMessageAsync(messageName, entityName
                        , new ColumnSet
                        (
                            SdkMessageFilter.Schema.Attributes.primaryobjecttypecode
                            , SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode
                            , SdkMessageFilter.Schema.Attributes.availability
                            , SdkMessageFilter.Schema.Attributes.customizationlevel
                            , SdkMessageFilter.Schema.Attributes.ismanaged
                            , SdkMessageFilter.Schema.Attributes.workflowsdkstepenabled
                            , SdkMessageFilter.Schema.Attributes.isvisible
                            , SdkMessageFilter.Schema.Attributes.iscustomprocessingstepallowed
                            , SdkMessageFilter.Schema.Attributes.restrictionlevel
                            , SdkMessageFilter.Schema.Attributes.sdkmessageid
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadSdkMessageFilters(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingSdkMessageFilterCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string MessageName => SdkMessageFilter.MessageName;

            public string MessageCategoryName => SdkMessageFilter.MessageCategoryName;

            public string PrimaryObjectTypeCode => SdkMessageFilter.PrimaryObjectTypeCode;

            public string SecondaryObjectTypeCode => SdkMessageFilter.SecondaryObjectTypeCode;

            public int? Availability => SdkMessageFilter.Availability;

            public Entities.GlobalOptionSets.AvailabilityEnum? AvailabilityEnum => SdkMessageFilter.AvailabilityEnum;

            public int? CustomizationLevel => SdkMessageFilter.CustomizationLevel;

            public int? RestrictionLevel => SdkMessageFilter.RestrictionLevel;

            public bool IsManaged => SdkMessageFilter.IsManaged.GetValueOrDefault();

            public bool IsVisible => SdkMessageFilter.IsVisible.GetValueOrDefault();

            public bool IsCustomProcessingStepAllowed => SdkMessageFilter.IsCustomProcessingStepAllowed.GetValueOrDefault();

            public bool WorkflowSdkStepEnabled => SdkMessageFilter.WorkflowSdkStepEnabled.GetValueOrDefault();

            public SdkMessageFilter SdkMessageFilter { get; }

            public EntityViewItem(SdkMessageFilter entity)
            {
                this.SdkMessageFilter = entity;
            }
        }

        private void LoadSdkMessageFilters(IEnumerable<SdkMessageFilter> results)
        {
            this.lstVwMessageFilters.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.MessageCategoryName, MessageComparer.Comparer)
                    .ThenBy(ent => ent.MessageName, MessageComparer.Comparer)
                    .ThenBy(ent => ent.PrimaryObjectTypeCode)
                )
                {
                    var item = new EntityViewItem(entity);

                    _itemsSource.Add(item);
                }

                if (this.lstVwMessageFilters.Items.Count == 1)
                {
                    this.lstVwMessageFilters.SelectedItem = this.lstVwMessageFilters.Items[0];
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
            this.lstVwMessageFilters.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwMessageFilters.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportMessageFilter };

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
                await ShowExistingMessageFilters();
            }
        }

        private SdkMessageFilter GetSelectedEntity()
        {
            return this.lstVwMessageFilters.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwMessageFilters.SelectedItems.OfType<EntityViewItem>().Select(e => e.SdkMessageFilter).SingleOrDefault() : null;
        }

        private List<SdkMessageFilter> GetSelectedEntitiesList()
        {
            return this.lstVwMessageFilters.SelectedItems.OfType<EntityViewItem>().Select(e => e.SdkMessageFilter).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void lstVwMessageFilters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    await ExecuteAction(item.SdkMessageFilter.Id, item.SdkMessageFilter.PrimaryObjectTypeCode, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idSdkMessageFilter, string entityName)
        {
            await PerformExportEntityDescription(folder, idSdkMessageFilter, entityName);
        }

        private void lstVwMessageFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idSdkMessageFilter, string entityName, Func<string, Guid, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            try
            {
                await action(folder, idSdkMessageFilter, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.PrimaryObjectTypeCode, PerformExportEntityDescription);
        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.PrimaryObjectTypeCode, PerformEntityEditor);
        }

        private async void mIDeleteMessageFilter_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.PrimaryObjectTypeCode, PerformDeleteEntity);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idSdkMessageFilter, string entityName)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            var repository = new SdkMessageFilterRepository(service);

            var message = await repository.GetByIdAsync(idSdkMessageFilter);

            string fileName = EntityFileNameFormatter.GetMessageFilterFileName(service.ConnectionData.Name, entityName, EntityFileNameFormatter.Headers.EntityDescription);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, message, service.ConnectionData);

            this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                , service.ConnectionData.Name
                , message.LogicalName
                , filePath
            );

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        private async Task PerformEntityEditor(string folder, Guid idSdkMessageFilter, string entityName)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, SdkMessageFilter.EntityLogicalName, idSdkMessageFilter);
        }

        private async Task PerformDeleteEntity(string folder, Guid idSdkMessageFilter, string entityName)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, SdkMessageFilter.EntityLogicalName, entityName);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, SdkMessageFilter.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, SdkMessageFilter.EntityLogicalName, idSdkMessageFilter);

                    await service.DeleteAsync(SdkMessageFilter.EntityLogicalName, idSdkMessageFilter);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, SdkMessageFilter.EntityLogicalName);

                await ShowExistingMessageFilters();
            }
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingMessageFilters();
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SdkMessageFilter, entity.Id);
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
                .Select(e => e.Id);

            if (!entitiesList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = GetSolutionComponentDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.SdkMessageFilter, entitiesList, null, withSelect);
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

                FillLastSolutionItems(connectionData, items, true, AddMessageToCrmSolutionLast_Click, "contMnAddMessageToSolutionLast");
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
                , (int)ComponentType.SdkMessageFilter
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
                , (int)ComponentType.SdkMessageFilter
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
                FillEntityNames(connectionData);
                await ShowExistingMessageFilters();
            }
        }

        private void FillEntityNames(ConnectionData connectionData)
        {
            cmBEntityName.Dispatcher.Invoke(() =>
            {
                LoadEntityNames(cmBEntityName, connectionData);
            });
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private void lstVwMessageFilters_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwMessageFilters_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteMessageFilter_Click(sender, e);
        }

        #region Clipboard

        private void mIClipboardCopyMessageName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.MessageCategoryName);
        }

        private void mIClipboardCopyMessageCategoryName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.MessageCategoryName);
        }

        private void mIClipboardCopyMessageId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SdkMessageFilter.SdkMessageId?.Id.ToString());
        }

        private void mIClipboardCopyPrimaryEntityName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.PrimaryObjectTypeCode);
        }

        private void mIClipboardCopySecondaryEntityName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SecondaryObjectTypeCode);
        }

        private void mIClipboardCopyMessageFilterId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SdkMessageFilter.Id.ToString());
        }

        #endregion Clipboard

        private async void AddMessageToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddMessageToSolution(true, null);
        }

        private async void AddMessageToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddMessageToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddMessageToSolution(bool withSelect, string solutionUniqueName)
        {
            var messagesList = GetSelectedEntitiesList()
                .Where(e => e.SdkMessageId != null)
                .Select(e => e.SdkMessageId.Id)
                .Distinct()
                .ToList();

            if (!messagesList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = GetSolutionComponentDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.SdkMessage, messagesList, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }
    }
}