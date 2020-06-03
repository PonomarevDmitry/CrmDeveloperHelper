using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowTreeSdkMessageRequest : WindowWithConnectionList
    {
        private readonly ObservableCollection<SdkMessageRequestTreeViewItem> _messageTree = new ObservableCollection<SdkMessageRequestTreeViewItem>();

        private SdkMessageSearchResult _sdkMessageSearchResult = null;

        private readonly List<GroupingProperty> _currentGrouping = new List<GroupingProperty>()
        {
            GroupingProperty.Endpoint,
            GroupingProperty.Message,
            GroupingProperty.Entity,
        };

        private readonly List<GroupingProperty> _groupingPropertiesAll = Enum.GetValues(typeof(GroupingProperty)).OfType<GroupingProperty>().ToList();

        private readonly Dictionary<GroupingProperty, RequestGroupBuilder> _propertyGroups = new Dictionary<GroupingProperty, RequestGroupBuilder>();

        private BitmapImage _imageRefresh;
        private BitmapImage _imageEntity;
        private BitmapImage _imageMessage;
        private BitmapImage _imageMessageCategory;
        private BitmapImage _imageField;
        private BitmapImage _imageRequest;
        private BitmapImage _imageResponse;
        private BitmapImage _imageNamespace;
        private BitmapImage _imageImageEndpoint;

        private readonly string _filePath;
        private readonly bool _isJavaScript;
        private readonly EnvDTE.SelectedItem _selectedItem;

        private readonly Popup _optionsPopup;
        private readonly FileGenerationSdkMessageRequestsOptionsControl _optionsControl;

        public WindowTreeSdkMessageRequest(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service

            , string filePath
            , bool isJavaScript
            , EnvDTE.SelectedItem selectedItem

            , string entityFilter
            , string messageFilter
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._filePath = filePath;
            this._isJavaScript = isJavaScript;
            this._selectedItem = selectedItem;

            InitializeComponent();

            _optionsControl = new FileGenerationSdkMessageRequestsOptionsControl();
            _optionsControl.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = _optionsControl,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            cmBEntityName.Text = entityFilter;
            txtBMessageFilter.Text = messageFilter;

            LoadFromConfig();

            LoadImages();

            LoadConfiguration();

            FillViewGroups();

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

            FocusOnComboBoxTextBox(cmBEntityName);

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            trVSdkMessageRequestTree.ItemsSource = _messageTree;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingSdkMessageRequests();
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

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageExplorer_Click, "mIOpenMessageExplorer");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterExplorer_Click, "mIOpenMessageFilterExplorer");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miPluginTree_Click, "mIOpenPluginTree");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterTree_Click, "mIOpenMessageFilterTree");
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
            _commonConfig.Save();
            FileGenerationConfiguration.SaveConfiguration();

            (cmBCurrentConnection.SelectedItem as ConnectionData)?.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private const string paramEntityName = "EntityName";
        private const string paramMessage = "Message";
        private const string paramEndpoint = "Endpoint";
        private const string paramGrouping = "Grouping";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            if (string.IsNullOrEmpty(this.cmBEntityName.Text)
                && string.IsNullOrEmpty(this.txtBMessageFilter.Text)
                && string.IsNullOrEmpty(this.txtBEndpoint.Text)
            )
            {
                this.cmBEntityName.Text = winConfig.GetValueString(paramEntityName);
                this.txtBMessageFilter.Text = winConfig.GetValueString(paramMessage);
                this.txtBEndpoint.Text = winConfig.GetValueString(paramEndpoint);
            }

            {
                var tempGroupingName = winConfig.GetValueString(paramGrouping);

                if (!string.IsNullOrEmpty(tempGroupingName))
                {
                    List<GroupingProperty> list = new List<GroupingProperty>();

                    this.IncreaseInit();

                    var split = tempGroupingName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in split)
                    {
                        if (Enum.TryParse<GroupingProperty>(item, out var tempGrouping))
                        {
                            if (!list.Contains(tempGrouping))
                            {
                                list.Add(tempGrouping);
                            }
                        }
                    }

                    if (list.Any())
                    {
                        _currentGrouping.Clear();

                        _currentGrouping.AddRange(list);
                    }

                    this.DecreaseInit();
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictString[paramEntityName] = this.cmBEntityName.Text?.Trim();
            winConfig.DictString[paramMessage] = this.txtBMessageFilter.Text.Trim();
            winConfig.DictString[paramEndpoint] = this.txtBEndpoint.Text.Trim();

            winConfig.DictString[paramGrouping] = string.Join(",", _currentGrouping.Select(g => g.ToString()));
        }

        private void LoadImages()
        {
            this._imageRefresh = this.Resources["ImageRefresh"] as BitmapImage;

            this._imageEntity = this.Resources["ImageEntity"] as BitmapImage;
            this._imageMessage = this.Resources["ImageMessage"] as BitmapImage;
            this._imageMessageCategory = this.Resources["ImageMessageCategory"] as BitmapImage;
            this._imageField = this.Resources["ImageField"] as BitmapImage;
            this._imageRequest = this.Resources["ImageRequest"] as BitmapImage;
            this._imageResponse = this.Resources["ImageResponse"] as BitmapImage;
            this._imageNamespace = this.Resources["ImageNamespace"] as BitmapImage;
            this._imageImageEndpoint = this.Resources["ImageEndpoint"] as BitmapImage;
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSdkMessageRequests();
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

        private enum GroupingProperty
        {
            Entity,
            Message,
            MessageCategory,
            Endpoint,
            Namespace,
        }

        private void FillViewGroups()
        {
            _propertyGroups.Clear();

            _propertyGroups[GroupingProperty.Entity] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.PrimaryObjectTypeCode ?? "none",
                TreeNodeBuilder = CreateNodeEntity,

                ActionOnChildsByKey = (entityName, node) => node.EntityLogicalName = entityName,
            };

            _propertyGroups[GroupingProperty.Message] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.SdkMessageName,
                TreeNodeBuilder = CreateNodeMessage,

                ActionOnChildsByKey = (message, node) => node.MessageName = message,

                OrderComparer = MessageComparer.Comparer,
            };

            _propertyGroups[GroupingProperty.MessageCategory] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.SdkMessageCategoryName,
                TreeNodeBuilder = CreateNodeMessageCategory,

                ActionOnChildsByKey = (messageCategory, node) => node.MessageCategoryName = messageCategory,

                OrderComparer = MessageComparer.Comparer,
            };

            _propertyGroups[GroupingProperty.Namespace] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.Namespace,
                TreeNodeBuilder = CreateNodeNamespace,
            };

            _propertyGroups[GroupingProperty.Endpoint] = new RequestGroupBuilder()
            {
                GroupFunc = ent => ent.Endpoint,
                TreeNodeBuilder = CreateNodeEndpoint,
            };

            mIView.Items.Clear();

            mIView.SubmenuClosed += this.mIView_SubmenuClosed;

            var menuItemRefreshView = new MenuItem()
            {
                Header = "Refresh View",
                Icon = new Image()
                {
                    Height = 16,
                    Width = 16,
                    Source = _imageRefresh,
                },
            };

            menuItemRefreshView.Click += this.mIView_SubmenuClosed;

            mIView.Items.Add(menuItemRefreshView);
            mIView.Items.Add(new Separator());

            this.IncreaseInit();

            for (int i = 0; i < _groupingPropertiesAll.Count; i++)
            {
                var comboBox = new ComboBox()
                {
                    Visibility = Visibility.Collapsed,

                    VerticalContentAlignment = VerticalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Stretch,

                    HorizontalAlignment = HorizontalAlignment.Stretch,

                    Background = Brushes.White,
                };

                comboBox.Items.Add(string.Empty);

                foreach (var item in _groupingPropertiesAll)
                {
                    comboBox.Items.Add(item);
                }

                if (i < this._currentGrouping.Count)
                {
                    comboBox.SelectedItem = this._currentGrouping[i];
                }

                comboBox.SelectionChanged += this.comboBox_SelectionChanged;

                mIView.Items.Add(comboBox);
            }

            this.DecreaseInit();

            UpdateGroupingVisibility();
        }

        private async void mIView_SubmenuClosed(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            UpdateGroupingVisibility();

            var grouping = GetGrouping();

            if (!this._currentGrouping.SequenceEqual(grouping))
            {
                this._currentGrouping.Clear();

                this._currentGrouping.AddRange(grouping);

                await RefreshTreeByViewAsync();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            UpdateGroupingVisibility();
        }

        private void UpdateGroupingVisibility()
        {
            this.IncreaseInit();

            List<GroupingProperty> selectedProperties = new List<GroupingProperty>();

            foreach (var comboBox in mIView.Items.OfType<ComboBox>())
            {
                if (comboBox.SelectedItem is GroupingProperty groupingProperty)
                {
                    if (!selectedProperties.Contains(groupingProperty))
                    {
                        selectedProperties.Add(groupingProperty);
                    }
                }
            }

            HashSet<GroupingProperty> usedProperties = new HashSet<GroupingProperty>();

            int index = 0;

            foreach (var comboBox in mIView.Items.OfType<ComboBox>())
            {
                FillGroupingComboBox(usedProperties, comboBox);

                if (index < selectedProperties.Count)
                {
                    usedProperties.Add(selectedProperties[index]);

                    comboBox.SelectedItem = selectedProperties[index];

                    comboBox.Visibility = Visibility.Visible;
                }
                else if (index == selectedProperties.Count)
                {
                    comboBox.SelectedIndex = 0;
                    comboBox.Visibility = Visibility.Visible;
                }
                else
                {
                    comboBox.SelectedIndex = 0;
                    comboBox.Visibility = Visibility.Hidden;
                }

                index++;
            }

            this.DecreaseInit();
        }

        private void FillGroupingComboBox(HashSet<GroupingProperty> usedProperties, ComboBox comboBox)
        {
            comboBox.Items.Clear();

            comboBox.Items.Add(string.Empty);

            comboBox.SelectedIndex = 0;

            foreach (var item in _groupingPropertiesAll.Where(p => !usedProperties.Contains(p)))
            {
                comboBox.Items.Add(item);
            }
        }

        private List<GroupingProperty> GetGrouping()
        {
            List<GroupingProperty> result = new List<GroupingProperty>();

            foreach (var comboBox in mIView.Items.OfType<ComboBox>())
            {
                if (comboBox.SelectedItem is GroupingProperty groupingProperty)
                {
                    if (!result.Contains(groupingProperty))
                    {
                        result.Add(groupingProperty);
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private async Task RefreshTreeByViewAsync()
        {
            if (_sdkMessageSearchResult != null)
            {
                await FillTreeAsync();
            }
            else
            {
                await ShowExistingSdkMessageRequests();
            }
        }

        private Task FillTreeAsync()
        {
            return Task.Run(() => FillTree());
        }

        private void FillTree()
        {
            ConnectionData connectionData = null;

            this.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            ToggleControls(connectionData, false, Properties.OutputStrings.BuildingSdkMessageRequestTree);

            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                _messageTree.Clear();
            });

            if (_sdkMessageSearchResult != null)
            {
                var list = GroupRequests(_sdkMessageSearchResult, _currentGrouping.Select(g => _propertyGroups[g]));

                this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
                {
                    this.trVSdkMessageRequestTree.BeginInit();

                    _messageTree.Clear();

                    foreach (var node in list)
                    {
                        _messageTree.Add(node);
                    }

                    this.trVSdkMessageRequestTree.EndInit();
                });
            }

            ToggleControls(connectionData, true, Properties.OutputStrings.BuildingSdkMessageRequestTreeCompleted);
        }

        private async Task ShowExistingSdkMessageRequests()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingSdkMessageRequests);

            this.trVSdkMessageRequestTree.Dispatcher.Invoke(() =>
            {
                _messageTree.Clear();
            });

            string entityName = string.Empty;
            string messageName = string.Empty;
            string endpointName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                entityName = cmBEntityName.Text?.Trim();
                messageName = txtBMessageFilter.Text.Trim();
                endpointName = txtBEndpoint.Text.Trim();
            });

            SdkMessageSearchResult search = new SdkMessageSearchResult();

            try
            {
                search.Requests = await new SdkMessageRequestRepository(service).GetListAsync(entityName, messageName, endpointName
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

            await FillTreeAsync();

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingSdkMessageRequestsCompleted);
        }

        private class RequestGroupBuilder
        {
            public Func<SdkMessageRequest, string> GroupFunc { get; set; }

            public IComparer<string> OrderComparer { get; set; }

            public Action<string, SdkMessageRequestTreeViewItem> ActionOnChildsByKey { get; set; }

            public Func<string, IEnumerable<SdkMessageRequest>, SdkMessageRequestTreeViewItem> TreeNodeBuilder { get; set; }
        }

        private List<SdkMessageRequestTreeViewItem> GroupRequests(SdkMessageSearchResult search, IEnumerable<RequestGroupBuilder> requestGroups)
        {
            List<SdkMessageRequestTreeViewItem> result = new List<SdkMessageRequestTreeViewItem>();

            result.AddRange(GroupRequestsRecursive(search, search.Requests, requestGroups, new Action<SdkMessageRequestTreeViewItem>[0]));

            return result;
        }

        private IEnumerable<SdkMessageRequestTreeViewItem> GroupRequestsRecursive(
            SdkMessageSearchResult search
            , IEnumerable<SdkMessageRequest> requests
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<SdkMessageRequestTreeViewItem>> actionsOnChilds
        )
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

            var groupsList = requests.GroupBy(groupBuilder.GroupFunc, StringComparer.InvariantCultureIgnoreCase);

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

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(node);
                }

                var newActionOnChilds = actionsOnChilds;

                if (groupBuilder.ActionOnChildsByKey != null)
                {
                    newActionOnChilds = newActionOnChilds.Union(new Action<SdkMessageRequestTreeViewItem>[] { n => groupBuilder.ActionOnChildsByKey(group.Key, n) });
                }

                foreach (var childNode in GroupRequestsRecursive(search, group, requestGroups.Skip(1), newActionOnChilds))
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

        private SdkMessageRequestTreeViewItem CreateNodeMessageCategory(string messageCategory, IEnumerable<SdkMessageRequest> steps)
        {
            var nodeMessage = new SdkMessageRequestTreeViewItem(null)
            {
                Name = messageCategory,
                Image = _imageMessageCategory,

                MessageCategoryName = messageCategory,
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

            node.MessageList.AddRange(steps.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Value).Distinct());

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeNamespace(string name, IEnumerable<SdkMessageRequest> steps)
        {
            var node = new SdkMessageRequestTreeViewItem(null)
            {
                Name = name,
                Image = _imageNamespace,
            };

            node.MessageList.AddRange(steps.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Value).Distinct());

            return node;
        }

        private SdkMessageRequestTreeViewItem CreateNodeEndpoint(string name, IEnumerable<SdkMessageRequest> steps)
        {
            var node = new SdkMessageRequestTreeViewItem(null)
            {
                Name = name,
                Image = _imageImageEndpoint,
            };

            node.MessageList.AddRange(steps.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Value).Distinct());

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

            if (!string.IsNullOrEmpty(field.Parser)
                && !string.Equals(field.ClrParser, field.Parser, StringComparison.InvariantCultureIgnoreCase)
            )
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

            if (!string.IsNullOrEmpty(field.Formatter)
                && !string.Equals(field.ClrFormatter, field.Formatter, StringComparison.InvariantCultureIgnoreCase)
            )
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar
                , cmBCurrentConnection
                , btnSetCurrentConnection
                , this.tSBCollapseAll
                , this.tSBExpandAll
                , this.tSBRefresh
                , this.menuView
            );

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

        private async void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingSdkMessageRequests();
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
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, true);
        }

        private void mICollapseNodes_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

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

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingDescriptionCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.OfType<ConnectionData>())
            {
                removed.Save();
            }

            ConnectionData connectionData = null;

            this.Dispatcher.Invoke(() =>
            {
                connectionData = GetSelectedConnection();
            });

            if (connectionData != null)
            {
                _sdkMessageSearchResult = null;

                FillEntityNames(connectionData);

                await ShowExistingSdkMessageRequests();
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

            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

            var items = contextMenu.Items.OfType<Control>();

            bool hasEntityName = nodeItem.EntityLogicalName.IsValidEntityName();
            bool hasMessageName = !string.IsNullOrEmpty(nodeItem.MessageName);

            bool isSingleMessage = nodeItem.MessageList != null && nodeItem.MessageList.Count == 1;

            bool showDependentComponents = nodeItem.GetId().HasValue && nodeItem.ComponentType.HasValue;
            bool hasIds = nodeItem.GetIdEnumerable().Any() && nodeItem.ComponentType.HasValue;

            ConnectionData connectionData = GetSelectedConnection();

            ActivateControls(items, CanCreateDescription(nodeItem), "contMnCreateDescription");
            SetControlsName(items, GetCreateDescriptionName(nodeItem), "contMnCreateDescription");

            ActivateControls(items, showDependentComponents, "contMnDependentComponents");

            ActivateControls(items, hasMessageName || hasEntityName, "contMnSdkMessage", "mIOpenPluginTree", "mIOpenMessageExplorer", "mIOpenMessageFilterExplorer", "mIOpenMessageFilterTree");

            ActivateControls(items, isSingleMessage, "contMnSdkMessageProxyClass");

            ActivateControls(items, hasIds, "contMnAddToSolution", "contMnAddToSolutionLast");
            FillLastSolutionItems(connectionData, items, hasIds, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            ActivateControls(items, nodeItem.SdkMessagePair.HasValue, "contMnSdkMessagePair", "contMnSdkMessagePairAddToSolutionLast");
            FillLastSolutionItems(connectionData, items, nodeItem.SdkMessagePair.HasValue, mIAddSdkMessagePairToCrmSolutionLast_Click, "contMnSdkMessagePairAddToSolutionLast");

            //ActivateControls(items, nodeItem.PluginAssembly.HasValue, "contMnAddPluginAssemblyStepsToSolution", "contMnAddPluginAssemblyStepsToSolutionLast");
            //FillLastSolutionItems(connectionData, items, nodeItem.PluginAssembly.HasValue, mIAddAssemblyStepsToSolutionLast_Click, "contMnAddPluginAssemblyStepsToSolutionLast");

            ActivateControls(items, hasEntityName, "contMnEntity");
            FillLastSolutionItems(connectionData, items, hasEntityName, AddEntityToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, hasEntityName, AddEntityToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastDoNotIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, hasEntityName, AddEntityToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityToSolutionLastIncludeAsShellOnly");
            ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityToSolutionLast");

            CheckSeparatorVisible(items);
        }

        #region Entity Handlers

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            var entityName = nodeItem.EntityLogicalName;

            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
        }

        private async void miOpenEntitySolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            _commonConfig.Save();

            var service = await GetService();

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
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            _commonConfig.Save();

            var service = await GetService();

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
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            await AddEntityMetadataToSolution(
                connectionData
                , new[] { idMetadata.Value }
                , withSelect
                , solutionUniqueName
                , rootComponentBehavior
            );
        }

        #endregion Entity Handlers

        #region SdkMessagePair Handlers

        private async void miOpenSdkMessagePairSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            WindowHelper.OpenExplorerSolutionExplorer(
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
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SdkMessagePair, nodeItem.SdkMessagePair.Value);
        }

        private async void miOpenSdkMessagePairDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.SdkMessagePair, nodeItem.SdkMessagePair.Value, null);
        }

        private async void mIAddSdkMessagePairToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddSdkMessagePairToSolution(e, true, null);
        }

        private async void mIAddSdkMessagePairToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddSdkMessagePairToSolution(e, false, solutionUniqueName);
            }
        }

        private async Task AddSdkMessagePairToSolution(RoutedEventArgs e, bool withSelect, string solutionUniqueName)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SdkMessagePair, new[] { nodeItem.SdkMessagePair.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void mICreateRequestProxyClassCSharp_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            var service = await GetService();

            var sdkMessagePair = await new SdkMessagePairRepository(service).GetByIdAsync(nodeItem.SdkMessagePair.Value, new ColumnSet(true));

            var sdkMessage = await new SdkMessageRepository(service).GetByIdAsync(sdkMessagePair.SdkMessageId.Id, new ColumnSet(true));

            var sdkFilters = await new SdkMessageFilterRepository(service).GetListByMessageAsync(sdkMessage.Id, new ColumnSet(true));

            var sdkRequests = await new SdkMessageRequestRepository(service).GetListByPairAsync(sdkMessagePair.Id, new ColumnSet(true));
            var sdkRequestFields = await new SdkMessageRequestFieldRepository(service).GetListByPairAsync(sdkMessagePair.Id, new ColumnSet(true));

            var sdkResponses = await new SdkMessageResponseRepository(service).GetListByPairAsync(sdkMessagePair.Id, new ColumnSet(true));
            var sdkResponseFields = await new SdkMessageResponseFieldRepository(service).GetListByPairAsync(sdkMessagePair.Id, new ColumnSet(true));

            var codeMessage = new CodeGenerationSdkMessage(sdkMessage.Id, sdkMessage.Name, sdkMessage.IsPrivate.GetValueOrDefault(), sdkMessage.CustomizationLevel.GetValueOrDefault());
            codeMessage.Fill(sdkFilters, new[] { sdkMessagePair }, sdkRequests, sdkRequestFields, sdkResponses, sdkResponseFields);

            var codeMessagePair = codeMessage.SdkMessagePairs[sdkMessagePair.Id];

            string operation = string.Format(Properties.OperationNames.CreatingFileForSdkMessageRequestFormat2, service.ConnectionData.Name, codeMessagePair.Request.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForSdkMessageRequestFormat1, codeMessagePair.Request.Name);

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var config = CreateFileCSharpConfiguration.CreateForSdkMessageRequest(fileGenerationOptions);

                string fileName = string.Format("{0}.{1}.cs", service.ConnectionData.Name, codeMessagePair.Request.Name);

                if (this._selectedItem != null)
                {
                    fileName = string.Format("{0}.cs", codeMessagePair.Request.Name);
                }

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                if (!_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                var repository = new EntityMetadataRepository(service);

                ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
                INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
                ITypeMappingService typeMappingService = new TypeMappingService(fileGenerationOptions.NamespaceClassesCSharp);
                ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);
                IMetadataProviderService metadataProviderService = new MetadataProviderService(repository);

                ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

                var options = new CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    VerbatimOrder = true,
                };

                await codeGenerationService.WriteSdkMessagePairAsync(codeMessagePair, filePath, fileGenerationOptions.NamespaceSdkMessagesCSharp, options);

                AddFileToVSProject(_selectedItem, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedSdkMessageRequestFileForConnectionFormat3, service.ConnectionData.Name, codeMessagePair.Request.Name, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForSdkMessageRequestCompletedFormat1, codeMessagePair.Request.Name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForSdkMessageRequestFailedFormat1, codeMessagePair.Request.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        #endregion SdkMessagePair Handlers

        private async void mICreateDescription_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            _commonConfig.Save();

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                var service = await GetService();

                ComponentType? componentType = nodeItem.ComponentType;
                Guid? id = nodeItem.GetId();

                if (componentType.HasValue && id.HasValue)
                {
                    WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)nodeItem.ComponentType.Value, id.Value, null);
                }
            }
        }

        private async void miOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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
                _commonConfig.Save();

                var service = await GetService();

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

        private async void AddToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            await AddToSolution(nodeItem, true, null);
        }

        private async void AddToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

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

        private async Task AddToSolution(SdkMessageRequestTreeViewItem nodeItem, bool withSelect, string solutionUniqueName)
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

                    await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, componentType.Value, idList, null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
        }

        private async void mICreateMessageProxyClassCSharp_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageRequestTreeViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageRequestTreeViewItem>(e);

            if (nodeItem == null
                || nodeItem.MessageList == null
                || !nodeItem.MessageList.Any()
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            var service = await GetService();

            Guid idMessage = nodeItem.MessageList.First();

            var sdkMessage = await new SdkMessageRepository(service).GetByIdAsync(idMessage, new ColumnSet(true));

            var sdkFilters = await new SdkMessageFilterRepository(service).GetListByMessageAsync(idMessage, new ColumnSet(true));

            var sdkMessagePairs = await new SdkMessagePairRepository(service).GetListByMessageAsync(idMessage, new ColumnSet(true));

            var sdkRequests = await new SdkMessageRequestRepository(service).GetListByMessageAsync(idMessage, new ColumnSet(true));
            var sdkRequestFields = await new SdkMessageRequestFieldRepository(service).GetListByMessageAsync(idMessage, new ColumnSet(true));

            var sdkResponses = await new SdkMessageResponseRepository(service).GetListByMessageAsync(idMessage, new ColumnSet(true));
            var sdkResponseFields = await new SdkMessageResponseFieldRepository(service).GetListByMessageAsync(idMessage, new ColumnSet(true));

            var codeMessage = new CodeGenerationSdkMessage(sdkMessage.Id, sdkMessage.Name, sdkMessage.IsPrivate.GetValueOrDefault(), sdkMessage.CustomizationLevel.GetValueOrDefault());
            codeMessage.Fill(sdkFilters, sdkMessagePairs, sdkRequests, sdkRequestFields, sdkResponses, sdkResponseFields);

            string operation = string.Format(Properties.OperationNames.CreatingFileForSdkMessageRequestFormat2, service.ConnectionData.Name, codeMessage.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingFileForSdkMessageRequestFormat1, codeMessage.Name);

            try
            {
                var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                var config = CreateFileCSharpConfiguration.CreateForSdkMessageRequest(fileGenerationOptions);

                string fileName = string.Format("{0}.{1}.cs", service.ConnectionData.Name, codeMessage.Name);

                if (this._selectedItem != null)
                {
                    fileName = string.Format("{0}.cs", codeMessage.Name);
                }

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                if (!_isJavaScript && !string.IsNullOrEmpty(_filePath))
                {
                    filePath = _filePath;
                }

                var repository = new EntityMetadataRepository(service);

                ICodeGenerationService codeGenerationService = new CodeGenerationService(config);
                INamingService namingService = new NamingService(service.ConnectionData.ServiceContextName, config);
                ITypeMappingService typeMappingService = new TypeMappingService(fileGenerationOptions.NamespaceClassesCSharp);
                ICodeWriterFilterService codeWriterFilterService = new CodeWriterFilterService(config);
                IMetadataProviderService metadataProviderService = new MetadataProviderService(repository);

                ICodeGenerationServiceProvider codeGenerationServiceProvider = new CodeGenerationServiceProvider(typeMappingService, codeGenerationService, codeWriterFilterService, metadataProviderService, namingService);

                var options = new CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    VerbatimOrder = true,
                };

                await codeGenerationService.WriteSdkMessageAsync(codeMessage, filePath, fileGenerationOptions.NamespaceSdkMessagesCSharp, options);

                AddFileToVSProject(_selectedItem, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.CreatedSdkMessageRequestFileForConnectionFormat3, service.ConnectionData.Name, codeMessage.Name, filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, string.Empty);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForSdkMessageRequestCompletedFormat1, codeMessage.Name);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingFileForSdkMessageRequestFailedFormat1, codeMessage.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation);
        }

        private async void tSBRefresh_Click(object sender, RoutedEventArgs e)
        {
            await ShowExistingSdkMessageRequests();
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            this._optionsControl.BindFileGenerationOptions(fileGenerationOptions);

            this._optionsPopup.IsOpen = true;
            this._optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
                this.Focus();
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void treeViewCopy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (trVSdkMessageRequestTree.SelectedItem != null && trVSdkMessageRequestTree.SelectedItem is SdkMessageRequestTreeViewItem nodeItem)
            {
                e.Handled = true;

                ClipboardHelper.SetText(nodeItem.Name);
            }
        }
    }
}