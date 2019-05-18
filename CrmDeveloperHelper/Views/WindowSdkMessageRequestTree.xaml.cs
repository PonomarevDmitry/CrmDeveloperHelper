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
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSdkMessageRequestTree : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly ObservableCollection<SdkMessageRequestTreeViewItem> _messageTree = new ObservableCollection<SdkMessageRequestTreeViewItem>();

        private View _currentView = View.Entity_Message_Namespace_Endpoint;

        private SdkMessageSearchResult _sdkMessageSearchResult = null;

        private readonly Dictionary<View, IEnumerable<RequestGroupBuilder>> _viewGroups = new Dictionary<View, IEnumerable<RequestGroupBuilder>>();

        private BitmapImage _imageEntity;
        private BitmapImage _imageMessage;
        private BitmapImage _imageField;
        private BitmapImage _imageRequest;
        private BitmapImage _imageResponse;
        private BitmapImage _imageNamespace;
        private BitmapImage _imageImageEndpoint;

        private readonly CommonConfiguration _commonConfig;

        public WindowSdkMessageRequestTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter
            , string messageFilter
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput ?? throw new ArgumentNullException(nameof(iWriteToOutput));
            this._commonConfig = commonConfig ?? throw new ArgumentNullException(nameof(commonConfig));

            _connectionCache[service.ConnectionData.ConnectionId] = service ?? throw new ArgumentNullException(nameof(service));

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            FillViewGroups();

            LoadFromConfig();

            LoadImages();

            LoadConfiguration();

            cmBEntityName.Text = entityFilter;
            txtBMessageFilter.Text = messageFilter;

            FocusOnComboBoxTextBox(cmBEntityName);

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            trVSdkMessageRequestTree.ItemsSource = _messageTree;

            this.DecreaseInit();

            ShowExistingSdkMessageRequests();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
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
                var tempViewName = winConfig.GetValueString(paramView);

                if (!string.IsNullOrEmpty(tempViewName))
                {
                    if (Enum.TryParse<View>(tempViewName, out View tempView))
                    {
                        this._currentView = tempView;

                        this.IncreaseInit();

                        foreach (var item in mIView.Items.OfType<RadioButton>())
                        {
                            item.IsChecked = false;
                        }

                        var selectedRadioButton = mIView.Items.OfType<RadioButton>().FirstOrDefault(r => r.Tag != null && r.Tag is View view && view == this._currentView);

                        if (selectedRadioButton != null)
                        {
                            selectedRadioButton.IsChecked = true;
                        }
                        else
                        {
                            mIView.Items.OfType<RadioButton>().First().IsChecked = true;
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
            this._imageField = this.Resources["ImageField"] as BitmapImage;
            this._imageRequest = this.Resources["ImageRequest"] as BitmapImage;
            this._imageResponse = this.Resources["ImageResponse"] as BitmapImage;
            this._imageNamespace = this.Resources["ImageNamespace"] as BitmapImage;
            this._imageImageEndpoint = this.Resources["ImageEndpoint"] as BitmapImage;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSdkMessageRequests();
            }

            base.OnKeyDown(e);
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

        private enum View
        {
            Entity_Message_Endpoint,
            Entity_Message_Endpoint_Namespace,

            Entity_Message_Namespace_Endpoint,

            Entity_Endpoint_Message,
            Entity_Endpoint_Message_Namespace,

            Message_Entity_Endpoint,
            Message_Entity_Endpoint_Namespace,

            Message_Entity_Namespace_Endpoint,

            Message_Endpoint_Entity,
            Message_Endpoint_Entity_Namespace,

            Endpoint_Entity_Message,
            Endpoint_Entity_Message_Namespace,

            Endpoint_Message_Entity,
            Endpoint_Message_Entity_Namespace,

            Endpoint_Namespace_Entity_Message,
            Endpoint_Namespace_Message_Entity,

            Namespace_Entity_Message_Endpoint,
            Namespace_Message_Entity_Endpoint,

            Namespace_Endpoint_Entity_Message,
            Namespace_Endpoint_Message_Entity,
        }

        private void FillViewGroups()
        {
            var groupByName = new Dictionary<string, RequestGroupBuilder>();

            groupByName["Entity"] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.PrimaryObjectTypeCode ?? "none",
                TreeNodeBuilder = CreateNodeEntity,
            };

            groupByName["Message"] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.SdkMessageName,
                TreeNodeBuilder = CreateNodeMessage,
                OrderComparer = new MessageComparer(),
            };

            groupByName["Namespace"] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.Namespace,
                TreeNodeBuilder = CreateNodeNamespace,
            };

            groupByName["Endpoint"] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.Endpoint,
                TreeNodeBuilder = CreateNodeEndpoint,
            };

            _viewGroups.Clear();
            mIView.Items.Clear();

            foreach (var view in Enum.GetValues(typeof(View)).OfType<View>())
            {
                string name = view.ToString();

                var split = name.Split('_');

                _viewGroups[view] = split.Select(n => groupByName[n]).ToArray();

                var groupsName = string.Join(" -> ", split);

                RadioButton radioButton = new RadioButton()
                {
                    GroupName = "View",
                    Content = string.Format("Display by {0}", groupsName),
                    IsChecked = false,
                    Tag = view,
                };

                radioButton.Checked += radioButton_Checked;

                mIView.Items.Add(radioButton);
            }

            mIView.Items.OfType<RadioButton>().First().IsChecked = true;

            foreach (var item in _viewGroups.Keys.OrderBy(v => (int)v))
            {
                FieldInfo fi = typeof(View).GetField(item.ToString());
                if (fi != null)
                {
                    var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    string groups = (attributes.Length > 0) ? attributes[0].Description : item.ToString();


                }
            }
        }

        private void RefreshTreeByView()
        {
            if (_sdkMessageSearchResult != null)
            {
                ShowExistingSdkMessageRequests();
            }
            else
            {
                FillTree();
            }
        }

        private void FillTree()
        {
            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                _messageTree.Clear();
            });

            if (_sdkMessageSearchResult == null)
            {
                return;
            }

            var list = GroupRequests(_sdkMessageSearchResult, _viewGroups[_currentView]);

            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                this.trVSdkMessageRequestTree.BeginInit();

                _messageTree.Clear();

                this.Dispatcher.Invoke(() =>
                {
                    foreach (var node in list)
                    {
                        _messageTree.Add(node);
                    }
                });

                this.trVSdkMessageRequestTree.EndInit();
            });
        }

        private async Task ShowExistingSdkMessageRequests()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingSdkMessageRequests);

            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                _messageTree.Clear();
            });

            string entityName = string.Empty;
            string messageName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                entityName = cmBEntityName.Text?.Trim();
                messageName = txtBMessageFilter.Text.Trim();
            });

            SdkMessageSearchResult search = new SdkMessageSearchResult();

            try
            {
                search.Requests = await new SdkMessageRequestRepository(service).GetListAsync(entityName, messageName
                    , new ColumnSet
                    (
                        SdkMessageRequest.Schema.Attributes.name
                        , SdkMessageRequest.Schema.Attributes.primaryobjecttypecode
                        , SdkMessageRequest.Schema.Attributes.sdkmessagepairid
                    )
                );
                search.RequestFields = await new SdkMessageRequestFieldRepository(service).GetListAsync(new ColumnSet(SdkMessageRequestField.Schema.Attributes.name, SdkMessageRequestField.Schema.Attributes.sdkmessagerequestid, SdkMessageRequestField.Schema.Attributes.clrparser, SdkMessageRequestField.Schema.Attributes.parser, SdkMessageRequestField.Schema.Attributes.position));

                search.Responses = await new SdkMessageResponseRepository(service).GetListAsync(new ColumnSet(SdkMessageResponse.Schema.Attributes.sdkmessagerequestid));
                search.ResponseFields = await new SdkMessageResponseFieldRepository(service).GetListAsync(new ColumnSet(SdkMessageResponseField.Schema.Attributes.name, SdkMessageResponseField.Schema.Attributes.sdkmessageresponseid, SdkMessageResponseField.Schema.Attributes.clrformatter, SdkMessageResponseField.Schema.Attributes.position));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            _sdkMessageSearchResult = search;

            FillTree();

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingSdkMessageRequestsCompleted);
        }

        private class RequestGroupBuilder
        {
            public Func<SdkMessageRequest, string> GroupFunc { get; set; }

            public IComparer<string> OrderComparer { get; set; }

            public Func<string, IEnumerable<SdkMessageRequest>, SdkMessageRequestTreeViewItem> TreeNodeBuilder { get; set; }
        }

        private List<SdkMessageRequestTreeViewItem> GroupRequests(SdkMessageSearchResult search, IEnumerable<RequestGroupBuilder> requestGroups)
        {
            List<SdkMessageRequestTreeViewItem> result = new List<SdkMessageRequestTreeViewItem>();

            result.AddRange(GroupRequestsRecursive(search, search.Requests, requestGroups));

            return result;
        }

        private IEnumerable<SdkMessageRequestTreeViewItem> GroupRequestsRecursive(SdkMessageSearchResult search, IEnumerable<SdkMessageRequest> requests, IEnumerable<RequestGroupBuilder> requestGroups)
        {
            if (!requestGroups.Any())
            {
                foreach (var node in GetAllRequests(search, requests))
                {
                    yield return node;
                }

                yield break;
            }

            var groupBuilder = requestGroups.First();

            var groupsList = requests.GroupBy(groupBuilder.GroupFunc);

            if (groupBuilder.OrderComparer != null)
            {
                groupsList = groupsList.OrderBy(e => e.Key, groupBuilder.OrderComparer);
            }
            else
            {
                groupsList = groupsList.OrderBy(e => e.Key);
            }

            foreach (var group in groupsList)
            {
                SdkMessageRequestTreeViewItem node = groupBuilder.TreeNodeBuilder(group.Key, group);

                foreach (var childNode in GroupRequestsRecursive(search, group, requestGroups.Skip(1)))
                {
                    node.Items.Add(childNode);
                }

                yield return node;
            }
        }

        private IEnumerable<SdkMessageRequestTreeViewItem> GetAllRequests(SdkMessageSearchResult search, IEnumerable<SdkMessageRequest> requests)
        {
            foreach (var sdkRequest in requests.OrderBy(s => s.Name))
            {
                SdkMessageRequestTreeViewItem nodeRequest = CreateNodeRequest(sdkRequest);

                {
                    var fields = search.RequestFields.Where(f => f.SdkMessageRequestId?.Id == sdkRequest.SdkMessageRequestId).OrderBy(f => f.Position).ThenBy(f => f.Name);

                    foreach (var field in fields)
                    {
                        SdkMessageRequestTreeViewItem nodeRequestField = CreateNodeRequestField(sdkRequest, field);

                        nodeRequest.Items.Add(nodeRequestField);
                    }

                    nodeRequest.IsExpanded = true;
                }

                yield return nodeRequest;

                var responses = search.Responses.Where(r => r.SdkMessageRequestId?.Id == sdkRequest.SdkMessageRequestId);

                foreach (var response in responses)
                {
                    SdkMessageRequestTreeViewItem nodeRespose = CreateNodeResponse(sdkRequest, sdkRequest.Name + " - Response", response);

                    var fields = search.ResponseFields.Where(f => f.SdkMessageResponseId?.Id == response.SdkMessageResponseId).OrderBy(f => f.Position).ThenBy(f => f.Name);

                    foreach (var field in fields)
                    {
                        SdkMessageRequestTreeViewItem nodeResposeField = CreateNodeResponseField(sdkRequest, field);

                        nodeRespose.Items.Add(nodeResposeField);
                    }

                    nodeRespose.IsExpanded = true;

                    yield return nodeRespose;
                }
            }
        }

        private SdkMessageRequestTreeViewItem CreateNodeMessage(string message, IEnumerable<SdkMessageRequest> steps)
        {
            var nodeMessage = new SdkMessageRequestTreeViewItem(ComponentType.SdkMessage)
            {
                Name = message,
                Image = _imageMessage,

                MessageName = message,
            };

            nodeMessage.MessageList.AddRange(steps.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Value).Distinct());

            return nodeMessage;
        }

        private SdkMessageRequestTreeViewItem CreateNodeEntity(string entityName, IEnumerable<SdkMessageRequest> steps)
        {
            var node = new SdkMessageRequestTreeViewItem(ComponentType.Entity)
            {
                Name = entityName,
                Image = _imageEntity,

                EntityLogicalName = entityName,
            };

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeNamespace(string name, IEnumerable<SdkMessageRequest> steps)
        {
            var node = new SdkMessageRequestTreeViewItem(null)
            {
                Name = name,
                Image = _imageNamespace,
            };

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeEndpoint(string name, IEnumerable<SdkMessageRequest> steps)
        {
            var node = new SdkMessageRequestTreeViewItem(null)
            {
                Name = name,
                Image = _imageImageEndpoint,
            };

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeRequest(SdkMessageRequest sdkRequest)
        {
            var node = new SdkMessageRequestTreeViewItem(ComponentType.SdkMessageRequest)
            {
                Name = sdkRequest.Name + " - Request",
                Image = _imageRequest,

                EntityLogicalName = sdkRequest.PrimaryObjectTypeCode,
                MessageName = sdkRequest.SdkMessageName,
                SdkMessagePair = sdkRequest.SdkMessagePairId?.Id,

                SdkMessageRequest = sdkRequest.SdkMessageRequestId,
            };

            if (sdkRequest.SdkMessageId.HasValue)
            {
                node.MessageList.Add(sdkRequest.SdkMessageId.Value);
            }

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeRequestField(SdkMessageRequest sdkRequest, SdkMessageRequestField field)
        {
            StringBuilder name = new StringBuilder();
            name.AppendFormat("{0}. {1}", field.Position, field.Name);

            if (!string.IsNullOrEmpty(field.ClrParser))
            {
                name.AppendFormat("      ({0})", field.ClrParser);
            }

            if (!string.IsNullOrEmpty(field.Parser))
            {
                name.AppendFormat("      ({0})", field.Parser);
            }

            var node = new SdkMessageRequestTreeViewItem(ComponentType.SdkMessageRequestField)
            {
                Name = name.ToString(),
                Image = _imageField,

                EntityLogicalName = sdkRequest.PrimaryObjectTypeCode,
                MessageName = sdkRequest.SdkMessageName,
                SdkMessagePair = sdkRequest.SdkMessagePairId?.Id,

                SdkMessageRequest = field.SdkMessageRequestId?.Id,

                SdkMessageRequestField = field.Id,
            };

            if (sdkRequest.SdkMessageId.HasValue)
            {
                node.MessageList.Add(sdkRequest.SdkMessageId.Value);
            }

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeResponse(SdkMessageRequest sdkRequest, string name, SdkMessageResponse response)
        {
            var node = new SdkMessageRequestTreeViewItem(ComponentType.SdkMessageResponse)
            {
                Name = name,
                Image = _imageResponse,

                EntityLogicalName = sdkRequest.PrimaryObjectTypeCode,
                MessageName = sdkRequest.SdkMessageName,
                SdkMessagePair = sdkRequest.SdkMessagePairId?.Id,

                SdkMessageResponse = response.SdkMessageResponseId,
            };

            if (sdkRequest.SdkMessageId.HasValue)
            {
                node.MessageList.Add(sdkRequest.SdkMessageId.Value);
            }

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeResponseField(SdkMessageRequest sdkRequest, SdkMessageResponseField field)
        {
            StringBuilder name = new StringBuilder();
            name.AppendFormat("{0}. {1}", field.Position, field.Name);

            if (!string.IsNullOrEmpty(field.ClrFormatter))
            {
                name.AppendFormat("      ({0})", field.ClrFormatter);
            }

            if (!string.IsNullOrEmpty(field.Formatter))
            {
                name.AppendFormat("      ({0})", field.Formatter);
            }

            var node = new SdkMessageRequestTreeViewItem(ComponentType.SdkMessageResponseField)
            {
                Name = name.ToString(),
                Image = _imageField,

                EntityLogicalName = sdkRequest.PrimaryObjectTypeCode,
                MessageName = sdkRequest.SdkMessageName,
                SdkMessagePair = sdkRequest.SdkMessagePairId?.Id,

                SdkMessageResponse = field.SdkMessageResponseId?.Id,
                SdkMessageResponseField = field.Id,
            };

            if (sdkRequest.SdkMessageId.HasValue)
            {
                node.MessageList.Add(sdkRequest.SdkMessageId.Value);
            }

            return node;
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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, this.tSBCollapseAll, this.tSBExpandAll, this.menuView);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                                        && this.trVSdkMessageRequestTree.SelectedItem != null
                                        && this.trVSdkMessageRequestTree.SelectedItem is SdkMessageRequestTreeViewItem
                                        && CanCreateDescription(this.trVSdkMessageRequestTree.SelectedItem as SdkMessageRequestTreeViewItem);

                    UIElement[] list = { tSBCreateDescription };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }

                    tSBCreateDescription.Content = GetCreateDescriptionName(this.trVSdkMessageRequestTree.SelectedItem as SdkMessageRequestTreeViewItem);
                }
                catch (Exception)
                {
                }
            });
        }

        private string GetCreateDescriptionName(SdkMessageRequestTreeViewItem item)
        {
            if (item == null)
            {
                return "Create Description";
            }

            if (item.SdkMessageRequest.HasValue)
            {
                return "Create SdkMessageRequest Description";
            }

            if (item.SdkMessageResponse.HasValue)
            {
                return "Create SdkMessageResponse Description";
            }

            if (item.MessageList != null && item.MessageList.Any())
            {
                return "Create Message Description";
            }

            return "Create Description";
        }

        private bool CanCreateDescription(SdkMessageRequestTreeViewItem item)
        {
            return
                (item.MessageList != null && item.MessageList.Any())
                || item.SdkMessageRequest.HasValue
                || item.SdkMessageResponse.HasValue
                ;
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSdkMessageRequests();
            }
        }

        private SdkMessageRequestTreeViewItem GetSelectedEntity()
        {
            SdkMessageRequestTreeViewItem result = null;

            if (this.trVSdkMessageRequestTree.SelectedItem != null
                && this.trVSdkMessageRequestTree.SelectedItem is SdkMessageRequestTreeViewItem
                )
            {
                result = this.trVSdkMessageRequestTree.SelectedItem as SdkMessageRequestTreeViewItem;
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void trVSdkMessageRequestTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateButtonsEnable();
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton
                && radioButton.Tag != null
                && radioButton.Tag is View view
            )
            {
                if (this._currentView != view)
                {
                    this._currentView = view;

                    RefreshTreeByView();
                }
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
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, true);
        }

        private void mICollapseNodes_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, false);
        }

        private void ChangeExpandedInTreeViewItems(IEnumerable<SdkMessageRequestTreeViewItem> items, bool isExpanded)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                this.trVSdkMessageRequestTree.BeginInit();
            });

            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;

                ChangeExpandedInTreeViewItemsRecursive(item.Items, isExpanded);
            }

            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                this.trVSdkMessageRequestTree.EndInit();
            });
        }

        private void ChangeExpandedInTreeViewItemsRecursive(IEnumerable<SdkMessageRequestTreeViewItem> items, bool isExpanded)
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

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, _commonConfig.FolderForExport);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await CreateDescription(node);
        }

        private async Task CreateDescription(SdkMessageRequestTreeViewItem node)
        {
            if (!CanCreateDescription(node))
            {
                return;
            }

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingDescription);

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
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(message, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (node.SdkMessageRequest.HasValue)
            {
                var repository = new SdkMessageRequestRepository(service);
                var request = await repository.GetByIdAsync(node.SdkMessageRequest.Value, new ColumnSet(true));

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetSdkMessageRequestFileName(service.ConnectionData.Name, node.Name, "Description");

                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());
                }

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(request, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }

                {
                    var repFields = new SdkMessageRequestFieldRepository(service);

                    var queryFields = await repFields.GetListByRequestIdAsync(request.Id, new ColumnSet(true));

                    foreach (var field in queryFields.OrderBy(f => f.Position).ThenBy(f => f.Name))
                    {
                        var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(field, null, service.ConnectionData);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                            result.AppendLine(desc);
                        }
                    }
                }
            }

            if (node.SdkMessageResponse.HasValue)
            {
                var repository = new SdkMessageResponseRepository(service);
                var Response = await repository.GetByIdAsync(node.SdkMessageResponse.Value, new ColumnSet(true));

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetSdkMessageResponseFileName(service.ConnectionData.Name, node.Name, "Description");

                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());
                }

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(Response, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }

                {
                    var repFields = new SdkMessageResponseFieldRepository(service);

                    var queryFields = await repFields.GetListByResponseIdAsync(Response.Id, new ColumnSet(true));

                    foreach (var field in queryFields.OrderBy(f => f.Position).ThenBy(f => f.Name))
                    {
                        var desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(field, null, service.ConnectionData);

                        if (!string.IsNullOrEmpty(desc))
                        {
                            if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                            result.AppendLine(desc);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, result.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingDescriptionCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectionData connectionData = null;

            this.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                trVSdkMessageRequestTree.ItemsSource = null;
            });

            if (connectionData != null)
            {
                FillEntityNames(connectionData);

                ShowExistingSdkMessageRequests();
            }
        }

        private void FillEntityNames(ConnectionData connectionData)
        {
            if (connectionData != null
                && connectionData.IntellisenseData != null
                && connectionData.IntellisenseData.Entities != null
                )
            {
                cmBEntityName.Dispatcher.Invoke(() =>
                {
                    string text = cmBEntityName.Text;

                    cmBEntityName.Items.Clear();

                    foreach (var item in connectionData.IntellisenseData.Entities.Keys.OrderBy(s => s))
                    {
                        cmBEntityName.Items.Add(item);
                    }

                    cmBEntityName.Text = text;
                });
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            var items = contextMenu.Items.OfType<Control>();

            bool isEntity = !string.IsNullOrEmpty(nodeItem.EntityLogicalName) && !string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase);

            bool isMessage = nodeItem.MessageList != null && nodeItem.MessageList.Any();

            bool showDependentComponents = nodeItem.GetId().HasValue && nodeItem.ComponentType.HasValue;
            bool hasIds = nodeItem.GetIdEnumerable().Any() && nodeItem.ComponentType.HasValue;

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            ActivateControls(items, CanCreateDescription(nodeItem), "contMnCreateDescription");
            SetControlsName(items, GetCreateDescriptionName(nodeItem), "contMnCreateDescription");

            ActivateControls(items, showDependentComponents, "contMnDependentComponents");

            ActivateControls(items, isMessage || isEntity, "contMnSdkMessage");

            ActivateControls(items, hasIds, "contMnAddIntoSolution", "contMnAddIntoSolutionLast");
            FillLastSolutionItems(connectionData, items, hasIds, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");

            ActivateControls(items, nodeItem.SdkMessagePair.HasValue, "contMnSdkMessagePair", "contMnSdkMessagePairAddIntoSolutionLast");
            FillLastSolutionItems(connectionData, items, nodeItem.SdkMessagePair.HasValue, mIAddSdkMessagePairIntoCrmSolutionLast_Click, "contMnSdkMessagePairAddIntoSolutionLast");

            //ActivateControls(items, nodeItem.PluginAssembly.HasValue, "contMnAddPluginAssemblyStepsIntoSolution", "contMnAddPluginAssemblyStepsIntoSolutionLast");
            //FillLastSolutionItems(connectionData, items, nodeItem.PluginAssembly.HasValue, mIAddAssemblyStepsIntoSolutionLast_Click, "contMnAddPluginAssemblyStepsIntoSolutionLast");

            ActivateControls(items, isEntity, "contMnEntity");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityIntoCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityIntoSolutionLastIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityIntoCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityIntoSolutionLastDoNotIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityIntoCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityIntoSolutionLastIncludeAsShellOnly");
            ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityIntoSolutionLast");

            CheckSeparatorVisible(items);
        }

        #region Entity Handlers

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                || string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(nodeItem.EntityLogicalName);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                || string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(nodeItem.EntityLogicalName);
            }
        }

        private async void mIOpenEntityExplorer_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                || string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var service = await GetService();

                WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, nodeItem.EntityLogicalName);
            }
        }

        private async void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                || string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            var entityName = nodeItem.EntityLogicalName;

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
        }

        private async void miOpenEntitySolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                || string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData == null)
            {
                return;
            }

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);

            if (!idMetadata.HasValue)
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
                , idMetadata.Value
                , null
            );
        }

        private void miOpenEntityDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                || string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

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

        private async void miOpenEntityDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                || string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData == null)
            {
                return;
            }

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);

            if (!idMetadata.HasValue)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Entity, idMetadata.Value, null);
        }

        private async void AddEntityIntoCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(e, true, null, RootComponentBehavior.IncludeSubcomponents);
        }

        private async void AddEntityIntoCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(e, true, null, RootComponentBehavior.DoNotIncludeSubcomponents);
        }

        private async void AddEntityIntoCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(e, true, null, RootComponentBehavior.IncludeAsShellOnly);
        }

        private async void AddEntityIntoCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(e, false, solutionUniqueName, RootComponentBehavior.IncludeSubcomponents);
            }
        }

        private async void AddEntityIntoCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(e, false, solutionUniqueName, RootComponentBehavior.DoNotIncludeSubcomponents);
            }
        }

        private async void AddEntityIntoCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(e, false, solutionUniqueName, RootComponentBehavior.IncludeAsShellOnly);
            }
        }

        private async Task AddEntityIntoSolution(RoutedEventArgs e, bool withSelect, string solutionUniqueName, RootComponentBehavior rootComponentBehavior)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                || string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData == null)
            {
                return;
            }

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);

            if (!idMetadata.HasValue)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { idMetadata.Value }, rootComponentBehavior, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        #endregion Entity Handlers

        #region SdkMessagePair Handlers

        private async void miOpenSdkMessagePairSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || !nodeItem.SdkMessagePair.HasValue
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.SdkMessagePair
                , nodeItem.SdkMessagePair.Value
                , null
            );
        }

        private void miOpenSdkMessagePairDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || !nodeItem.SdkMessagePair.HasValue
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData == null)
            {
                return;
            }

            connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SdkMessagePair, nodeItem.SdkMessagePair.Value);
        }

        private async void miOpenSdkMessagePairDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || !nodeItem.SdkMessagePair.HasValue
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.SdkMessagePair, nodeItem.SdkMessagePair.Value, null);
        }

        private async void mIAddSdkMessagePairIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddSdkMessagePairIntoSolution(e, true, null);
        }

        private async void mIAddSdkMessagePairIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddSdkMessagePairIntoSolution(e, false, solutionUniqueName);
            }
        }

        private async Task AddSdkMessagePairIntoSolution(RoutedEventArgs e, bool withSelect, string solutionUniqueName)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null
                || !nodeItem.SdkMessagePair.HasValue
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SdkMessagePair, new[] { nodeItem.SdkMessagePair.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        #endregion SdkMessagePair Handlers

        private void mICreateDescription_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            if (!CanCreateDescription(nodeItem))
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, _commonConfig.FolderForExport);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            CreateDescription(nodeItem);
        }

        private async void mIOpenPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(_iWriteToOutput, service, _commonConfig, nodeItem?.EntityLogicalName, null, nodeItem?.MessageName);
        }

        private async void mIOpenSdkMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(_iWriteToOutput, service, _commonConfig, nodeItem?.EntityLogicalName, nodeItem?.MessageName);
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

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

        private async void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            _commonConfig.Save();

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var service = await GetService();

                ComponentType? componentType = nodeItem.ComponentType;
                Guid? id = nodeItem.GetId();

                if (componentType.HasValue && id.HasValue)
                {
                    WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, null, _commonConfig, (int)nodeItem.ComponentType.Value, id.Value, null);
                }
            }
        }

        private async void miOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData == null)
            {
                return;
            }

            ComponentType? componentType = nodeItem.ComponentType;
            Guid? id = nodeItem.GetId();

            if (componentType.HasValue && id.HasValue)
            {
                _commonConfig.Save();

                var service = await GetService();

                WindowHelper.OpenExplorerSolutionWindow(
                    _iWriteToOutput
                    , service
                    , _commonConfig
                    , (int)componentType
                    , id.Value
                    , null
                );
            }
        }

        private async void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            await AddIntoSolution(nodeItem, true, null);
        }

        private async void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as SdkMessageRequestTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddIntoSolution(nodeItem, false, solutionUniqueName);
            }
        }

        private async Task AddIntoSolution(SdkMessageRequestTreeViewItem nodeItem, bool withSelect, string solutionUniqueName)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ComponentType? componentType = nodeItem.ComponentType;
            IEnumerable<Guid> idList = nodeItem.GetIdEnumerable();

            if (componentType.HasValue && idList.Any())
            {
                _commonConfig.Save();

                try
                {
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                    await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, componentType.Value, idList, null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
        }
    }
}