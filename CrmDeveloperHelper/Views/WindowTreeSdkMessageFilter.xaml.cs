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
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowTreeSdkMessageFilter : WindowWithConnectionList
    {
        private readonly ObservableCollection<PluginTreeViewItem> _messageTree = new ObservableCollection<PluginTreeViewItem>();

        private View _currentView = View.ByEntity;

        private BitmapImage _imageEntity;
        private BitmapImage _imageMessage;

        private enum View
        {
            ByEntity = 0,
            ByMessage = 1,
        }

        public WindowTreeSdkMessageFilter(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string entityFilter
            , string messageFilter
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            cmBEntityName.Text = entityFilter;
            txtBMessageFilter.Text = messageFilter;

            LoadFromConfig();

            FillEntityNames(service.ConnectionData);

            LoadImages();

            LoadConfiguration();

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            trVMessageTree.ItemsSource = _messageTree;

            FillExplorersMenuItems();

            FocusOnComboBoxTextBox(cmBEntityName);

            this.DecreaseInit();

            var task = ShowExistingMessages();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getEntityName: GetEntityName
                , getMessageName: GetMessageName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miEntityMetadataExplorer_Click, "mIOpenEntityExplorer");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageProcessingStepExplorer_Click, "miMessageProcessingStepExplorer");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageExplorer_Click, "mIOpenMessageExplorer");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterExplorer_Click, "mIOpenMessageFilterExplorer");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miPluginTree_Click, "mIOpenPluginTree");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageRequestTree_Click, "mIOpenMessageRequestTree");
            }
        }

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.EntityLogicalName ?? cmBEntityName.Text.Trim();
        }

        private string GetMessageName()
        {
            var entity = GetSelectedEntity();

            return entity?.MessageName ?? txtBMessageFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;
        }

        private const string paramEntityName = "EntityName";
        private const string paramMessage = "Message";

        private const string paramView = "View";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            if (string.IsNullOrEmpty(this.cmBEntityName.Text)
                && string.IsNullOrEmpty(this.txtBMessageFilter.Text)
            )
            {
                this.cmBEntityName.Text = winConfig.GetValueString(paramEntityName);
                this.txtBMessageFilter.Text = winConfig.GetValueString(paramMessage);
            }

            {
                var temp = winConfig.GetValueString(paramView);

                if (!string.IsNullOrEmpty(temp))
                {
                    if (Enum.TryParse<View>(temp, out View tempView))
                    {
                        this._currentView = tempView;

                        this.IncreaseInit();

                        rBViewByEntity.IsChecked = rBViewByMessage.IsChecked = false;

                        switch (this._currentView)
                        {
                            case View.ByMessage:
                                rBViewByMessage.IsChecked = true;
                                break;
                            case View.ByEntity:
                            default:
                                rBViewByEntity.IsChecked = true;
                                break;
                        }

                        this.DecreaseInit();
                    }
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictString[paramEntityName] = this.cmBEntityName.Text?.Trim();
            winConfig.DictString[paramMessage] = this.txtBMessageFilter.Text.Trim();

            winConfig.DictString[paramView] = this._currentView.ToString();
        }

        private void LoadImages()
        {
            this._imageEntity = this.Resources["ImageEntity"] as BitmapImage;
            this._imageMessage = this.Resources["ImageMessage"] as BitmapImage;
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingMessages();
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

        private async Task ShowExistingMessages()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingSdkMessage);

            string entityName = string.Empty;
            string messageName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                this._messageTree.Clear();

                entityName = cmBEntityName.Text?.Trim();
                messageName = txtBMessageFilter.Text.Trim();
            });

            IEnumerable<SdkMessageFilter> search = Enumerable.Empty<SdkMessageFilter>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new SdkMessageFilterRepository(service);

                    search = await repository.GetAllSdkMessageFiltersWithMessageAsync(messageName, entityName
                        , new ColumnSet
                        (
                            SdkMessageFilter.Schema.EntityPrimaryIdAttribute
                            , SdkMessageFilter.Schema.Attributes.primaryobjecttypecode
                            , SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode
                            , SdkMessageFilter.Schema.Attributes.sdkmessageid
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            if (search != null)
            {
                this.trVMessageTree.Dispatcher.Invoke(() =>
                {
                    this.trVMessageTree.BeginInit();
                });

                switch (_currentView)
                {
                    case View.ByMessage:
                        FillTreeByMessage(search);
                        break;

                    case View.ByEntity:
                    default:
                        FillTreeByEntity(search);
                        break;
                }

                ExpandNodes(_messageTree);

                this.trVMessageTree.Dispatcher.Invoke(() =>
                {
                    this.trVMessageTree.EndInit();
                });
            }

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingSdkMessageCompleted);
        }

        private void FillTreeByEntity(IEnumerable<SdkMessageFilter> result)
        {
            var groupsByEnity = result.GroupBy(ent => ent.PrimaryObjectTypeCode).OrderBy(gr => gr.Key);

            foreach (var grEntity in groupsByEnity)
            {
                PluginTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, null, grEntity);

                var groupsByMessages = grEntity.GroupBy(ent => ent.MessageName).OrderBy(mess => mess.Key, MessageComparer.Comparer);

                foreach (var mess in groupsByMessages)
                {
                    PluginTreeViewItem nodeMessage = CreateNodeMessage(grEntity.Key, mess.Key, mess);

                    AddTreeNode(nodeEntity, nodeMessage);

                    nodeMessage.IsExpanded = true;
                }

                this.Dispatcher.Invoke(() =>
                {
                    _messageTree.Add(nodeEntity);
                });
            }
        }

        private void FillTreeByMessage(IEnumerable<SdkMessageFilter> result)
        {
            var groupsByMessage = result.GroupBy(ent => ent.MessageName).OrderBy(mess => mess.Key, MessageComparer.Comparer);

            foreach (var grMessage in groupsByMessage)
            {
                var nodeMessage = CreateNodeMessage(null, grMessage.Key, grMessage);

                var groupsByEntity = grMessage.GroupBy(ent => ent.PrimaryObjectTypeCode).OrderBy(e => e.Key);

                foreach (var grEntity in groupsByEntity)
                {
                    PluginTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grMessage.Key, grEntity);

                    AddTreeNode(nodeMessage, nodeEntity);
                }

                this.Dispatcher.Invoke(() =>
                {
                    _messageTree.Add(nodeMessage);
                });
            }
        }

        private void AddTreeNode(PluginTreeViewItem node, PluginTreeViewItem childNode)
        {
            node.Items.Add(childNode);
            childNode.Parent = node;
        }

        private PluginTreeViewItem CreateNodeMessage(string entityName, string message, IEnumerable<SdkMessageFilter> filters)
        {
            var nodeMessage = new PluginTreeViewItem(ComponentType.SdkMessage)
            {
                Name = message,
                Image = _imageMessage,

                EntityLogicalName = entityName,
                MessageName = message,
            };

            nodeMessage.MessageList.AddRange(filters.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Id).Distinct());

            nodeMessage.MessageFilterList.AddRange(filters.Where(s => s.SdkMessageFilterId.HasValue).Select(s => s.SdkMessageFilterId.Value).Distinct());

            return nodeMessage;
        }

        private PluginTreeViewItem CreateNodeEntity(string entityName, string messageName, IEnumerable<SdkMessageFilter> filters)
        {
            var nodeMessage = new PluginTreeViewItem(ComponentType.SdkMessageFilter)
            {
                Name = entityName,
                Image = _imageEntity,

                EntityLogicalName = entityName,
                MessageName = messageName,
            };

            nodeMessage.MessageFilterList.AddRange(filters.Where(s => s.SdkMessageFilterId.HasValue).Select(s => s.SdkMessageFilterId.Value).Distinct());

            return nodeMessage;
        }

        private void ExpandNodes(ObservableCollection<PluginTreeViewItem> list)
        {
            if (list.Count == 1)
            {
                list[0].IsExpanded = true;

                if (list[0].Items.Count == 0)
                {
                    list[0].IsSelected = true;
                }
                else
                {
                    ExpandNodes(list[0].Items);
                }
            }
            else if (list.Count > 0)
            {
                list[0].IsSelected = true;
            }
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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, this.tSBCollapseAll, this.tSBExpandAll, this.menuView);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.trVMessageTree.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                                        && this.trVMessageTree.SelectedItem != null
                                        && this.trVMessageTree.SelectedItem is PluginTreeViewItem
                                        && CanCreateDescription(this.trVMessageTree.SelectedItem as PluginTreeViewItem);

                    UIElement[] list = { tSBCreateDescription };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }

                    tSBCreateDescription.Content = GetCreateDescriptionName(this.trVMessageTree.SelectedItem as PluginTreeViewItem);
                }
                catch (Exception)
                {
                }
            });
        }

        private static string GetCreateDescriptionName(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "Create Description";
            }

            if (item.MessageList != null && item.MessageList.Any())
            {
                return "Create Message Description";
            }

            if (item.MessageFilterList != null && item.MessageFilterList.Any())
            {
                return "Create Message Filter Description";
            }

            if (item.StepId.HasValue)
            {
                return "Create Step Description";
            }

            if (item.StepImageId.HasValue)
            {
                return "Create Image Description";
            }

            return "Create Description";
        }

        private bool CanCreateDescription(PluginTreeViewItem item)
        {
            return (item.MessageList != null && item.MessageList.Any())
                || (item.MessageFilterList != null && item.MessageFilterList.Any())
                || item.StepId.HasValue
                || item.StepImageId.HasValue
                ;
        }

        private async void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingMessages();
            }
        }

        private PluginTreeViewItem GetSelectedEntity()
        {
            PluginTreeViewItem result = null;

            if (this.trVMessageTree.SelectedItem != null
                && this.trVMessageTree.SelectedItem is PluginTreeViewItem
                )
            {
                result = this.trVMessageTree.SelectedItem as PluginTreeViewItem;
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void trVMessageTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateButtonsEnable();
        }

        private async void rBViewByEntity_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByEntity)
            {
                this._currentView = View.ByEntity;

                await ShowExistingMessages();
            }
        }

        private async void rBViewByMessage_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByMessage)
            {
                this._currentView = View.ByMessage;

                await ShowExistingMessages();
            }
        }

        private void tSBCollapseAll_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(_messageTree, false);
        }

        private void tSBExpandAll_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(_messageTree, true);
        }

        private void mIExpandNodes_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, true);
        }

        private void mICollapseNodes_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, false);
        }

        private void ChangeExpandedInTreeViewItems(IEnumerable<PluginTreeViewItem> items, bool isExpanded)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            this.trVMessageTree.Dispatcher.Invoke(() =>
            {
                this.trVMessageTree.BeginInit();
            });

            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;

                ChangeExpandedInTreeViewItemsRecursive(item.Items, isExpanded);
            }

            this.trVMessageTree.Dispatcher.Invoke(() =>
            {
                this.trVMessageTree.EndInit();
            });
        }

        private void ChangeExpandedInTreeViewItemsRecursive(IEnumerable<PluginTreeViewItem> items, bool isExpanded)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;

                ChangeExpandedInTreeViewItemsRecursive(item.Items, isExpanded);
            }
        }

        private async void tSBCreateDescription_Click(object sender, RoutedEventArgs e)
        {
            var node = GetSelectedEntity();

            if (node == null)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            await CreateDescription(node);
        }

        private async Task CreateDescription(PluginTreeViewItem node)
        {
            if (!CanCreateDescription(node))
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingDescription);

            StringBuilder result = new StringBuilder();

            string fileName = string.Empty;

            if (node.MessageList != null && node.MessageList.Any())
            {
                var repository = new SdkMessageRepository(service);
                List<SdkMessage> listMessages = await repository.GetMessageByIdsAsync(node.MessageList.ToArray());

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetMessageFileName(service.ConnectionData.Name, node.Name, "Description");

                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());
                }

                foreach (var message in listMessages)
                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(message, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (node.MessageFilterList != null && node.MessageFilterList.Any())
            {
                var repository = new SdkMessageFilterRepository(service);
                List<SdkMessageFilter> listMessages = await repository.GetMessageFiltersByIdsAsync(node.MessageFilterList.ToArray());

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetMessageFilterFileName(service.ConnectionData.Name, node.Name, "Description");

                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());
                }

                foreach (var message in listMessages)
                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(message, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, result.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingDescriptionCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                FillEntityNames(connectionData);
                await ShowExistingMessages();
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

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            var items = contextMenu.Items.OfType<Control>();

            bool isEntity = nodeItem.EntityLogicalName.IsValidEntityName();

            bool isMessage = nodeItem.MessageList != null && nodeItem.MessageList.Any() && nodeItem.ComponentType == ComponentType.SdkMessage;
            bool isMessageFilter = nodeItem.MessageFilterList != null && nodeItem.MessageFilterList.Any() && nodeItem.ComponentType == ComponentType.SdkMessageFilter;

            bool showDependentComponents = nodeItem.GetId().HasValue && nodeItem.ComponentType.HasValue;

            ConnectionData connectionData = GetSelectedConnection();

            ActivateControls(items, CanCreateDescription(nodeItem), "contMnCreateDescription");
            SetControlsName(items, GetCreateDescriptionName(nodeItem), "contMnCreateDescription");

            ActivateControls(items, isMessage || isMessageFilter, "contMnAddToSolution", "contMnAddToSolutionLast");
            FillLastSolutionItems(connectionData, items, isMessage || isMessageFilter, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            ActivateControls(items, !isMessageFilter && nodeItem.MessageList != null && nodeItem.MessageList.Any(), "contMnAddToSolutionMessageFilter", "contMnAddToSolutionMessageFilterLast");
            FillLastSolutionItems(connectionData, items, !isMessageFilter && nodeItem.MessageList != null && nodeItem.MessageList.Any(), AddMessageFilterToCrmSolutionLast_Click, "contMnAddToSolutionMessageFilterLast");

            ActivateControls(items, showDependentComponents, "contMnDependentComponents");

            ActivateControls(items, isEntity, "contMnEntity");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastDoNotIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityToSolutionLastIncludeAsShellOnly");
            ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityToSolutionLast");

            CheckSeparatorVisible(items);
        }

        private async void AddToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            await AddToSolution(nodeItem, true, null);
        }

        private async void AddToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
            )
            {
                await AddToSolution(nodeItem, false, solutionUniqueName);
            }
        }

        private async Task AddToSolution(PluginTreeViewItem nodeItem, bool withSelect, string solutionUniqueName)
        {
            ComponentType? componentType = nodeItem.ComponentType;
            var idList = nodeItem.GetIdEnumerable();

            if (componentType.HasValue && idList.Any())
            {
                await AddComponentsToSolution(componentType.Value, idList, null, withSelect, solutionUniqueName);
            }
        }

        private async void AddMessageFilterToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            await AddMessageFilterToSolution(nodeItem, true, null);
        }

        private async void AddMessageFilterToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
            )
            {
                await AddMessageFilterToSolution(nodeItem, false, solutionUniqueName);
            }
        }

        private async Task AddMessageFilterToSolution(PluginTreeViewItem nodeItem, bool withSelect, string solutionUniqueName)
        {
            if (nodeItem.MessageFilterList != null && nodeItem.MessageFilterList.Any())
            {
                await AddComponentsToSolution(ComponentType.SdkMessageFilter, nodeItem.MessageFilterList, null, withSelect, solutionUniqueName);
            }
        }

        private async void mICreateDescription_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            if (!CanCreateDescription(nodeItem))
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            await CreateDescription(nodeItem);
        }

        private async void miOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            ComponentType? componentType = nodeItem.ComponentType;
            Guid? id = nodeItem.GetId();

            if (componentType.HasValue && id.HasValue)
            {
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
                    , (int)componentType
                    , id.Value
                    , null
                );
            }
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            ComponentType? componentType = nodeItem.ComponentType;
            Guid? id = nodeItem.GetId();

            if (componentType.HasValue && id.HasValue)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(componentType.Value, id.Value);
            }
        }

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ComponentType? componentType = nodeItem.ComponentType;
            Guid? id = nodeItem.GetId();

            _commonConfig.Save();

            if (componentType.HasValue && id.HasValue)
            {
                WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)nodeItem.ComponentType.Value, id.Value, null);
            }
        }

        #region Entity Handlers

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                || !nodeItem.EntityLogicalName.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(nodeItem.EntityLogicalName);
            }
        }

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.EntityLogicalName.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                this._iWriteToOutput.OpenFetchXmlFile(connectionData, _commonConfig, entity.EntityLogicalName);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                || !nodeItem.EntityLogicalName.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(nodeItem.EntityLogicalName);
            }
        }

        private async void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                || !nodeItem.EntityLogicalName.IsValidEntityName()
            )
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

            var entityName = nodeItem.EntityLogicalName;

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
        }

        private async void miOpenEntitySolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                || !nodeItem.EntityLogicalName.IsValidEntityName()
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);

            if (!idMetadata.HasValue)
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
                , (int)ComponentType.Entity
                , idMetadata.Value
                , null
            );
        }

        private void miOpenEntityDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                || !nodeItem.EntityLogicalName.IsValidEntityName()
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);

            if (!idMetadata.HasValue)
            {
                return;
            }

            connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Entity, idMetadata.Value);
        }

        private async void miOpenEntityDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                || !nodeItem.EntityLogicalName.IsValidEntityName()
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);

            if (!idMetadata.HasValue)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Entity, idMetadata.Value, null);
        }

        private async void AddEntityToCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(e, true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddEntityToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(e, true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddEntityToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(e, true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddEntityToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
            )
            {
                await AddEntityToSolution(e, false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void AddEntityToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
            )
            {
                await AddEntityToSolution(e, false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void AddEntityToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
            )
            {
                await AddEntityToSolution(e, false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async Task AddEntityToSolution(RoutedEventArgs e, bool withSelect, string solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                 || !nodeItem.EntityLogicalName.IsValidEntityName()
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);

            if (!idMetadata.HasValue)
            {
                return;
            }

            await AddComponentsToSolution(ComponentType.Entity, new[] { idMetadata.Value }, rootComponentBehavior, withSelect, solutionUniqueName);
        }

        #endregion Entity Handlers

        private async Task AddComponentsToSolution(ComponentType componentType, IEnumerable<Guid> idList, SolutionComponent.Schema.OptionSets.rootcomponentbehavior? rootComponentBehavior, bool withSelect, string solutionUniqueName)
        {
            if (!idList.Any())
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

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, componentType, idList, rootComponentBehavior, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void treeViewCopy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (trVMessageTree.SelectedItem != null && trVMessageTree.SelectedItem is PluginTreeViewItem nodeItem)
            {
                e.Handled = true;

                ClipboardHelper.SetText(nodeItem.Name);
            }
        }
    }
}