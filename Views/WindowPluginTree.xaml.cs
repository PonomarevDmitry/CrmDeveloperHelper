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
    public partial class WindowPluginTree : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private int _init = 0;

        private View _currentView = View.ByEntity;

        private BitmapImage _imagePluginAssembly;
        private BitmapImage _imageEntity;
        private BitmapImage _imageImage;
        private BitmapImage _imageMessage;
        private BitmapImage _imagePluginType;
        private BitmapImage _imageStage;
        private BitmapImage _imageStep;
        private BitmapImage _imageStepDisabled;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private enum View
        {
            ByEntity = 0,
            ByAssembly = 1,
            ByMessage = 2,
        }

        bool _controlsEnabled = true;

        public WindowPluginTree(
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

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            LoadImages();

            LoadConfiguration();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBEntityName.Text = selection;
            }

            txtBEntityName.SelectionLength = 0;
            txtBEntityName.SelectionStart = txtBEntityName.Text.Length;

            txtBEntityName.Focus();

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (service != null)
            {
                ShowExistingPlugins();
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
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

        private const string paramEntityName = "EntityName";
        private const string paramMessage = "Message";
        private const string paramPluginTypeName = "PluginTypeName";

        private const string paramPreValidationStage = "PreValidationStage";
        private const string paramPreStage = "PreStage";
        private const string paramPostSynchStage = "PostSynchStage";
        private const string paramPostAsynchStage = "PostAsynchStage";

        private const string paramView = "View";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            if (string.IsNullOrEmpty(this.txtBEntityName.Text))
            {
                this.txtBEntityName.Text = winConfig.GetValueString(paramEntityName);
            }

            this.txtBMessageFilter.Text = winConfig.GetValueString(paramMessage);
            this.txtBPluginTypeFilter.Text = winConfig.GetValueString(paramPluginTypeName);

            this.chBStagePreValidation.IsChecked = winConfig.GetValueBool(paramPreValidationStage).GetValueOrDefault();
            this.chBStagePre.IsChecked = winConfig.GetValueBool(paramPreStage).GetValueOrDefault();
            this.chBStagePostSync.IsChecked = winConfig.GetValueBool(paramPostSynchStage).GetValueOrDefault();
            this.chBStagePostAsync.IsChecked = winConfig.GetValueBool(paramPostAsynchStage).GetValueOrDefault();

            {
                var temp = winConfig.GetValueString(paramView);

                if (!string.IsNullOrEmpty(temp))
                {
                    if (Enum.TryParse<View>(temp, out View tempView))
                    {
                        this._currentView = tempView;

                        bool lastEnabled = this._controlsEnabled;

                        this._controlsEnabled = true;

                        rBViewByEntity.IsChecked = rBViewByAssembly.IsChecked = rBViewByMessage.IsChecked = false;

                        switch (this._currentView)
                        {
                            case View.ByAssembly:
                                rBViewByAssembly.IsChecked = true;
                                break;
                            case View.ByMessage:
                                rBViewByMessage.IsChecked = true;
                                break;
                            case View.ByEntity:
                            default:
                                rBViewByEntity.IsChecked = true;
                                break;
                        }

                        this._controlsEnabled = lastEnabled;
                    }
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictString[paramEntityName] = this.txtBEntityName.Text.Trim();
            winConfig.DictString[paramMessage] = this.txtBMessageFilter.Text.Trim();
            winConfig.DictString[paramPluginTypeName] = this.txtBPluginTypeFilter.Text.Trim();

            winConfig.DictBool[paramPreValidationStage] = this.chBStagePreValidation.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPreStage] = this.chBStagePre.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPostSynchStage] = this.chBStagePostSync.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPostAsynchStage] = this.chBStagePostAsync.IsChecked.GetValueOrDefault();

            winConfig.DictString[paramView] = this._currentView.ToString();
        }

        private void LoadImages()
        {
            this._imagePluginAssembly = this.Resources["ImagePluginAssembly"] as BitmapImage;
            this._imageEntity = this.Resources["ImageEntity"] as BitmapImage;
            this._imageImage = this.Resources["ImageImage"] as BitmapImage;
            this._imageMessage = this.Resources["ImageMessage"] as BitmapImage;
            this._imagePluginType = this.Resources["ImagePluginType"] as BitmapImage;
            this._imageStage = this.Resources["ImageStage"] as BitmapImage;
            this._imageStep = this.Resources["ImageStep"] as BitmapImage;
            this._imageStepDisabled = this.Resources["ImageStepDisabled"] as BitmapImage;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingPlugins();
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

        private async Task ShowExistingPlugins()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading plugins...");

            this.trVPluginTree.ItemsSource = null;

            string entityName = string.Empty;
            string messageName = string.Empty;
            string pluginTypeName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                entityName = txtBEntityName.Text.Trim();
                messageName = txtBMessageFilter.Text.Trim();
                pluginTypeName = txtBPluginTypeFilter.Text.Trim();

            });

            var stages = GetStages();

            PluginSearchResult search = null;

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    PluginSearchRepository repository = new PluginSearchRepository(service);

                    search = await repository.FindAllAsync(stages, pluginTypeName, messageName, entityName);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            if (search != null)
            {
                ObservableCollection<PluginTreeViewItem> list = LoadPlugins(search, _currentView);

                this.trVPluginTree.Dispatcher.Invoke(() =>
                {
                    this.trVPluginTree.BeginInit();

                    this.trVPluginTree.ItemsSource = list;

                    this.trVPluginTree.EndInit();
                });
            }

            UpdateStatus("Loading completed.");

            ToggleControls(true);
        }

        public List<PluginStage> GetStages()
        {
            List<PluginStage> result = new List<PluginStage>();

            this.Dispatcher.Invoke(() =>
            {
                if (chBStagePreValidation.IsChecked.GetValueOrDefault())
                {
                    result.Add(PluginStage.PreValidation);
                }

                if (chBStagePre.IsChecked.GetValueOrDefault())
                {
                    result.Add(PluginStage.Pre);
                }

                if (chBStagePostSync.IsChecked.GetValueOrDefault())
                {
                    result.Add(PluginStage.PostSynch);
                }

                if (chBStagePostAsync.IsChecked.GetValueOrDefault())
                {
                    result.Add(PluginStage.PostAsych);
                }
            });

            return result;
        }

        private ObservableCollection<PluginTreeViewItem> LoadPlugins(PluginSearchResult search, View currentView)
        {
            ObservableCollection<PluginTreeViewItem> list = null;

            switch (currentView)
            {
                case View.ByAssembly:
                    list = FillTreeByAssembly(search);
                    break;

                case View.ByMessage:
                    list = FillTreeByMessage(search);
                    break;

                case View.ByEntity:
                default:
                    list = FillTreeByEntity(search);
                    break;
            }

            ExpandNodes(list);

            return list;
        }

        private ObservableCollection<PluginTreeViewItem> FillTreeByEntity(PluginSearchResult result)
        {
            ObservableCollection<PluginTreeViewItem> list = new ObservableCollection<PluginTreeViewItem>();

            var groupsByEnity = result.SdkMessageProcessingStep.GroupBy(ent => ent.PrimaryObjectTypeCodeName).OrderBy(gr => gr.Key);

            foreach (var grEntity in groupsByEnity)
            {
                PluginTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);

                list.Add(nodeEntity);

                var groupsByMessages = grEntity.GroupBy(ent => ent.SdkMessageId?.Name ?? "Unknown").OrderBy(mess => mess.Key, new MessageComparer());

                foreach (var mess in groupsByMessages)
                {
                    PluginTreeViewItem nodeMessage = CreateNodeMessage(mess.Key, mess);

                    AddTreeNode(nodeEntity, nodeMessage);

                    var groupsStage = mess.GroupBy(ent => ent.Stage.Value).OrderBy(item => item.Key);

                    foreach (var stage in groupsStage)
                    {
                        PluginTreeViewItem nodeStage = null;
                        PluginTreeViewItem nodePostAsynch = null;

                        foreach (var step in stage.OrderBy(s => s.Mode.Value).ThenBy(s => s.Rank).ThenBy(s => s.Name))
                        {
                            PluginTreeViewItem nodeTarget = null;

                            if (step.Mode.Value == 1)
                            {
                                if (nodePostAsynch == null)
                                {
                                    nodePostAsynch = CreateNodeStage(stage.Key, step.Mode.Value);

                                    AddTreeNode(nodeMessage, nodePostAsynch);
                                }

                                nodeTarget = nodePostAsynch;
                            }
                            else
                            {
                                if (nodeStage == null)
                                {
                                    nodeStage = CreateNodeStage(stage.Key, step.Mode.Value);

                                    AddTreeNode(nodeMessage, nodeStage);
                                }

                                nodeTarget = nodeStage;
                            }

                            PluginTreeViewItem nodeStep = CreateNodeStep(step);

                            AddTreeNode(nodeTarget, nodeStep);

                            var queryImage = from image in result.SdkMessageProcessingStepImage
                                             where image.SdkMessageProcessingStepId != null
                                             where image.SdkMessageProcessingStepId.Id == step.Id
                                             orderby image.ImageType.Value
                                             select image;

                            foreach (var image in queryImage)
                            {
                                PluginTreeViewItem nodeImage = CreateNodeImage(image);

                                AddTreeNode(nodeStep, nodeImage);
                            }

                            ExpandNode(nodeStep);
                        }

                        if (nodeStage != null)
                        {
                            ExpandNode(nodeStage);
                        }

                        if (nodePostAsynch != null)
                        {
                            ExpandNode(nodePostAsynch);
                        }
                    }

                    ExpandNode(nodeMessage);
                }
            }

            return list;
        }

        private ObservableCollection<PluginTreeViewItem> FillTreeByMessage(PluginSearchResult result)
        {
            ObservableCollection<PluginTreeViewItem> list = new ObservableCollection<PluginTreeViewItem>();

            var groupsByMessage = result.SdkMessageProcessingStep.GroupBy(ent => ent.SdkMessageId?.Name ?? "Unknown").OrderBy(mess => mess.Key, new MessageComparer());

            foreach (var grMessage in groupsByMessage)
            {
                var nodeMessage = CreateNodeMessage(grMessage.Key, grMessage);

                list.Add(nodeMessage);

                var groupsByEntity = grMessage.GroupBy(ent => ent.PrimaryObjectTypeCodeName).OrderBy(e => e.Key);

                foreach (var grEntity in groupsByEntity)
                {
                    PluginTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);

                    AddTreeNode(nodeMessage, nodeEntity);

                    var groupsStage = grEntity.GroupBy(ent => ent.Stage.Value).OrderBy(item => item.Key);

                    foreach (var stage in groupsStage)
                    {
                        PluginTreeViewItem nodeStage = null;
                        PluginTreeViewItem nodePostAsynch = null;

                        foreach (var step in stage.OrderBy(s => s.Mode.Value).ThenBy(s => s.Rank).ThenBy(s => s.Name))
                        {
                            PluginTreeViewItem nodeTarget = null;

                            if (step.Mode.Value == 1)
                            {
                                if (nodePostAsynch == null)
                                {
                                    nodePostAsynch = CreateNodeStage(stage.Key, step.Mode.Value);

                                    AddTreeNode(nodeEntity, nodePostAsynch);
                                }

                                nodeTarget = nodePostAsynch;
                            }
                            else
                            {
                                if (nodeStage == null)
                                {
                                    nodeStage = CreateNodeStage(stage.Key, step.Mode.Value);

                                    AddTreeNode(nodeEntity, nodeStage);
                                }

                                nodeTarget = nodeStage;
                            }

                            PluginTreeViewItem nodeStep = CreateNodeStep(step);

                            AddTreeNode(nodeTarget, nodeStep);

                            var queryImage = from image in result.SdkMessageProcessingStepImage
                                             where image.SdkMessageProcessingStepId != null
                                             where image.SdkMessageProcessingStepId.Id == step.Id
                                             orderby image.ImageType.Value
                                             select image;

                            foreach (var image in queryImage)
                            {
                                PluginTreeViewItem nodeImage = CreateNodeImage(image);

                                AddTreeNode(nodeStep, nodeImage);
                            }

                            ExpandNode(nodeStep);
                        }

                        if (nodeStage != null)
                        {
                            ExpandNode(nodeStage);
                        }

                        if (nodePostAsynch != null)
                        {
                            ExpandNode(nodePostAsynch);
                        }
                    }
                }
            }

            return list;
        }

        private ObservableCollection<PluginTreeViewItem> FillTreeByAssembly(PluginSearchResult result)
        {
            ObservableCollection<PluginTreeViewItem> list = new ObservableCollection<PluginTreeViewItem>();

            var groupsByAssembly = result.SdkMessageProcessingStep.GroupBy(ent => ent.PluginAssemblyName).OrderBy(gr => gr.Key);

            foreach (var grAssembly in groupsByAssembly)
            {
                PluginTreeViewItem nodeAssembly = CreateNodeAssembly(grAssembly.Key, grAssembly.FirstOrDefault(e => e.PluginAssemblyId != null)?.PluginAssemblyId);

                list.Add(nodeAssembly);

                var groupsByPluginType = grAssembly.GroupBy(ent => ent.EventHandler?.Name ?? "Unknown").OrderBy(gr => gr.Key);

                foreach (var grPluginType in groupsByPluginType)
                {
                    PluginTreeViewItem nodePluginType = CreateNodePluginType(grPluginType.Key, grPluginType.FirstOrDefault(e => e.EventHandler != null)?.EventHandler?.Id);

                    AddTreeNode(nodeAssembly, nodePluginType);

                    var groupsByEnity = grPluginType.GroupBy(ent => ent.PrimaryObjectTypeCodeName).OrderBy(gr => gr.Key);

                    foreach (var grEntity in groupsByEnity)
                    {
                        var nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);

                        AddTreeNode(nodePluginType, nodeEntity);

                        var groupsByMessages = grEntity.GroupBy(ent => ent.SdkMessageId?.Name ?? "Unknown").OrderBy(mess => mess.Key, new MessageComparer());

                        foreach (var mess in groupsByMessages)
                        {
                            PluginTreeViewItem nodeMessage = CreateNodeMessage(mess.Key, mess);

                            AddTreeNode(nodeEntity, nodeMessage);

                            var groupsStage = mess.GroupBy(ent => ent.Stage.Value).OrderBy(item => item.Key);

                            foreach (var stage in groupsStage)
                            {
                                PluginTreeViewItem nodeStage = null;
                                PluginTreeViewItem nodePostAsynch = null;

                                foreach (var step in stage.OrderBy(s => s.Mode.Value).ThenBy(s => s.Rank).ThenBy(s => s.Name))
                                {
                                    PluginTreeViewItem nodeTarget = null;

                                    if (step.Mode.Value == 1)
                                    {
                                        if (nodePostAsynch == null)
                                        {
                                            nodePostAsynch = CreateNodeStage(stage.Key, step.Mode.Value);

                                            AddTreeNode(nodeMessage, nodePostAsynch);
                                        }

                                        nodeTarget = nodePostAsynch;
                                    }
                                    else
                                    {
                                        if (nodeStage == null)
                                        {
                                            nodeStage = CreateNodeStage(stage.Key, step.Mode.Value);

                                            AddTreeNode(nodeMessage, nodeStage);
                                        }

                                        nodeTarget = nodeStage;
                                    }

                                    PluginTreeViewItem nodeStep = CreateNodeStep(step);

                                    AddTreeNode(nodeTarget, nodeStep);

                                    var queryImage = from image in result.SdkMessageProcessingStepImage
                                                     where image.SdkMessageProcessingStepId != null
                                                     where image.SdkMessageProcessingStepId.Id == step.Id
                                                     orderby image.ImageType.Value
                                                     select image;

                                    foreach (var image in queryImage)
                                    {
                                        PluginTreeViewItem nodeImage = CreateNodeImage(image);

                                        AddTreeNode(nodeStep, nodeImage);
                                    }

                                    ExpandNode(nodeStep);
                                }

                                if (nodeStage != null)
                                {
                                    ExpandNode(nodeStage);
                                }

                                if (nodePostAsynch != null)
                                {
                                    ExpandNode(nodePostAsynch);
                                }
                            }

                            ExpandNode(nodeMessage);
                        }

                        ExpandNode(nodeEntity);
                    }
                }
            }

            return list;
        }

        private void AddTreeNode(PluginTreeViewItem node, PluginTreeViewItem childNode)
        {
            node.Items.Add(childNode);
        }

        private void ExpandNode(PluginTreeViewItem node)
        {
            node.IsExpanded = true;
        }

        private PluginTreeViewItem CreateNodeAssembly(string assemblyName, Guid? idPluginAssembly)
        {
            var nodeMessage = new PluginTreeViewItem()
            {
                Name = assemblyName,
                Image = _imagePluginAssembly,

                PluginAssembly = idPluginAssembly,
            };

            return nodeMessage;
        }

        private PluginTreeViewItem CreateNodeImage(Entities.SdkMessageProcessingStepImage image)
        {
            string nameImage = GetImageName(image);
            string tooltipImage = GetImageTooltip(image);

            var nodeImage = new PluginTreeViewItem()
            {
                Name = nameImage,
                Tooltip = tooltipImage,
                Image = _imageImage,

                StepImage = image.Id
            };

            return nodeImage;
        }

        private PluginTreeViewItem CreateNodeStep(Entities.SdkMessageProcessingStep step)
        {
            string nameStep = GetStepName(step);
            string tooltipStep = GetStepTooltip(step);

            var nodeStep = new PluginTreeViewItem()
            {
                Name = nameStep,
                Tooltip = tooltipStep,
                Image = _imageStep,

                Step = step.Id,
            };

            nodeStep.Image = step.StateCode == 0 ? _imageStep : _imageStepDisabled;

            return nodeStep;
        }

        private PluginTreeViewItem CreateNodeStage(int stage, int mode)
        {
            string name = SdkMessageProcessingStepRepository.GetStageName(stage, mode);

            var nodeStage = new PluginTreeViewItem()
            {
                Name = name,
                Image = _imageStage,
            };

            return nodeStage;
        }

        private PluginTreeViewItem CreateNodePluginType(string pluginTypeName, Guid? idPluginType)
        {
            var nodeType = new PluginTreeViewItem()
            {
                Name = pluginTypeName,
                Image = _imagePluginType,

                PluginType = idPluginType,
            };

            return nodeType;
        }

        private PluginTreeViewItem CreateNodeMessage(string message, IEnumerable<SdkMessageProcessingStep> steps)
        {
            var nodeMessage = new PluginTreeViewItem()
            {
                Name = message,
                Image = _imageMessage,

                Message = steps.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Id).Distinct().ToList(),

                MessageFilter = steps.Where(s => s.SdkMessageFilterId != null).Select(s => s.SdkMessageFilterId.Id).Distinct().ToList(),
            };

            return nodeMessage;
        }

        private PluginTreeViewItem CreateNodeEntity(string entityName, IEnumerable<SdkMessageProcessingStep> steps)
        {
            var nodeMessage = new PluginTreeViewItem()
            {
                Name = entityName,
                Image = _imageEntity,

                MessageFilter = steps.Where(s => s.SdkMessageFilterId != null).Select(s => s.SdkMessageFilterId.Id).Distinct().ToList(),
            };

            return nodeMessage;
        }

        private string GetStepTooltip(Entities.SdkMessageProcessingStep step)
        {
            if (string.IsNullOrEmpty(step.FilteringAttributes))
            {
                return null;
            }

            StringBuilder tooltipStep = new StringBuilder();

            tooltipStep.AppendLine("Filtering:");

            foreach (string item in step.FilteringAttributesStrings)
            {
                tooltipStep.AppendLine().Append(item);
            }

            return tooltipStep.ToString();
        }

        private string GetStepName(Entities.SdkMessageProcessingStep step)
        {
            StringBuilder nameStep = new StringBuilder();

            if (step.Rank.HasValue)
            {
                nameStep.AppendFormat("{0}. ", step.Rank.ToString());
            }

            nameStep.Append(step.EventHandler?.Name ?? "Unknown");

            if (!string.IsNullOrEmpty(step.FilteringAttributes))
            {
                nameStep.AppendFormat("      Filtering: {0}", step.FilteringAttributesStringsSorted);
            }

            return nameStep.ToString();
        }

        private string GetImageTooltip(Entities.SdkMessageProcessingStepImage image)
        {
            if (string.IsNullOrEmpty(image.Attributes1))
            {
                return null;
            }

            StringBuilder tooltipImage = new StringBuilder();

            tooltipImage.AppendLine("Attributes:");

            foreach (string item in image.Attributes1Strings)
            {
                tooltipImage.AppendLine().Append(item);
            }

            return tooltipImage.ToString();
        }

        private string GetImageName(Entities.SdkMessageProcessingStepImage image)
        {
            StringBuilder nameImage = new StringBuilder();

            if (image.ImageType != null)
            {
                if (image.ImageType.Value == 0)
                {
                    nameImage.Append("PreImage");
                }
                else if (image.ImageType.Value == 1)
                {
                    nameImage.Append("PostImage");
                }
                else if (image.ImageType.Value == 2)
                {
                    nameImage.Append("BothImage");
                }
            }

            if (!string.IsNullOrEmpty(image.EntityAlias))
            {
                if (nameImage.Length > 0) { nameImage.Append(", "); }

                nameImage.Append(image.EntityAlias);
            }

            if (!string.IsNullOrEmpty(image.Name))
            {
                if (nameImage.Length > 0) { nameImage.Append(", "); }

                nameImage.Append(image.Name);
            }

            if (nameImage.Length > 0) { nameImage.Append(", "); }

            if (!string.IsNullOrEmpty(image.Attributes1))
            {
                nameImage.AppendFormat("Attributes: {0}", image.Attributes1StringsSorted);
            }
            else
            {
                nameImage.Append("Attributes: All");
            }

            return nameImage.ToString();
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

            ToggleControl(this.tSBCollapseAll, enabled);
            ToggleControl(this.tSBExpandAll, enabled);
            ToggleControl(this.menuView, enabled);

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

        private void ToggleControl(UIElement c, bool enabled)
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
            this.trVPluginTree.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled
                                        && this.trVPluginTree.SelectedItem != null
                                        && this.trVPluginTree.SelectedItem is PluginTreeViewItem
                                        && CanCreateDescription(this.trVPluginTree.SelectedItem as PluginTreeViewItem);

                    UIElement[] list = { tSBCreateDescription };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }

                    tSBCreateDescription.Content = GetCreateDescription(this.trVPluginTree.SelectedItem as PluginTreeViewItem);
                }
                catch (Exception)
                {
                }
            });
        }

        private string GetCreateDescription(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "Create Description";
            }

            if (item.PluginAssembly.HasValue)
            {
                return "Create Plugin Assembly Description";
            }

            if (item.PluginType.HasValue)
            {
                return "Create Plugin Type Description";
            }

            if (item.Message != null && item.Message.Any())
            {
                return "Create Message Description";
            }

            if (item.MessageFilter != null && item.MessageFilter.Any())
            {
                return "Create Message Filter Description";
            }

            if (item.Step.HasValue)
            {
                return "Create Step Description";
            }

            if (item.StepImage.HasValue)
            {
                return "Create Image Description";
            }

            return "Create Description";
        }

        private bool CanCreateDescription(PluginTreeViewItem item)
        {
            return item.PluginAssembly.HasValue
                || item.PluginType.HasValue
                || (item.Message != null && item.Message.Any())
                || (item.MessageFilter != null && item.MessageFilter.Any())
                || item.Step.HasValue
                || item.StepImage.HasValue
                ;
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingPlugins();
            }
        }

        private PluginTreeViewItem GetSelectedEntity()
        {
            PluginTreeViewItem result = null;

            if (this.trVPluginTree.SelectedItem != null
                && this.trVPluginTree.SelectedItem is PluginTreeViewItem
                )
            {
                result = this.trVPluginTree.SelectedItem as PluginTreeViewItem;
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void trVPluginTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateButtonsEnable();
        }

        private void rBViewByEntity_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByEntity)
            {
                this._currentView = View.ByEntity;

                ShowExistingPlugins();
            }
        }

        private void rBViewByAssembly_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByAssembly)
            {
                this._currentView = View.ByAssembly;

                ShowExistingPlugins();
            }
        }

        private void rBViewByMessage_Checked(object sender, RoutedEventArgs e)
        {
            if (this._currentView != View.ByMessage)
            {
                this._currentView = View.ByMessage;

                ShowExistingPlugins();
            }
        }

        private void tSBCollapseAll_Click(object sender, RoutedEventArgs e)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ChangeExpandedAll(false);
        }

        private void tSBExpandAll_Click(object sender, RoutedEventArgs e)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ChangeExpandedAll(true);
        }

        private void ChangeExpandedAll(bool isExpanded)
        {
            trVPluginTree.BeginInit();

            if (trVPluginTree.Items != null)
            {
                foreach (PluginTreeViewItem item in trVPluginTree.Items)
                {
                    RecursiveExpandedAll(item, isExpanded);
                }
            }

            trVPluginTree.EndInit();
        }

        private void RecursiveExpandedAll(PluginTreeViewItem item, bool isExpanded)
        {
            item.IsExpanded = isExpanded;

            foreach (var child in item.Items)
            {
                RecursiveExpandedAll(child, isExpanded);
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
                return;
            }

            if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                return;
            }

            await CreateDescription(node);
        }

        private async Task CreateDescription(PluginTreeViewItem node)
        {
            if (!CanCreateDescription(node))
            {
                return;
            }

            ToggleControls(false);

            var service = await GetService();

            StringBuilder result = new StringBuilder();

            string fileName = string.Empty;

            if (node.PluginAssembly.HasValue)
            {
                var repository = new PluginAssemblyRepository(service);
                var pluginAssembly = await repository.GetAssemblyByIdAsync(node.PluginAssembly.Value);

                fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, pluginAssembly.Name, "Description", "txt");

                {
                    PluginAssemblyDescriptionHandler handler = new PluginAssemblyDescriptionHandler(service, service.ConnectionData.GetConnectionInfo());

                    string desc = await handler.CreateDescriptionAsync(pluginAssembly.Id, pluginAssembly.Name, DateTime.Now);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(pluginAssembly, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (node.PluginType.HasValue)
            {
                var repository = new PluginTypeRepository(service);
                var pluginType = await repository.GetPluginTypeByIdAsync(node.PluginType.Value);

                fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, pluginType.TypeName, "Description");

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

                {
                    var repStep = new SdkMessageProcessingStepRepository(service);
                    var repImage = new SdkMessageProcessingStepImageRepository(service);
                    var repSecure = new SdkMessageProcessingStepSecureConfigRepository(service);

                    var allSteps = await repStep.GetAllStepsByPluginTypeAsync(pluginType.PluginTypeId.Value);
                    var queryImage = await repImage.GetImagesByPluginTypeAsync(pluginType.PluginTypeId.Value);
                    var listSecure = await repSecure.GetAllSdkMessageProcessingStepSecureConfigAsync();

                    var desc = await PluginTypeDescriptionHandler.CreateDescriptionAsync(
                        pluginType.PluginTypeId.Value
                        , allSteps
                        , queryImage
                        , listSecure
                        );

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(pluginType, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (node.Step.HasValue)
            {
                var repository = new SdkMessageProcessingStepRepository(service);
                var step = await repository.GetStepByIdAsync(node.Step.Value);

                fileName = EntityFileNameFormatter.GetPluginStepFileName(service.ConnectionData.Name, step.Name, "Description");

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

                {
                    var repImage = new SdkMessageProcessingStepImageRepository(service);

                    var queryImage = await repImage.GetStepImagesAsync(step.Id);
                    SdkMessageProcessingStepSecureConfig enSecure = null;

                    if (step.SdkMessageProcessingStepSecureConfigId != null)
                    {
                        var repSecure = new SdkMessageProcessingStepSecureConfigRepository(service);

                        enSecure = await repSecure.GetSecureByIdAsync(step.SdkMessageProcessingStepSecureConfigId.Id);
                    }

                    var desc = await PluginTypeDescriptionHandler.GetStepDescriptionAsync(step, enSecure, queryImage);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(step, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (node.StepImage.HasValue)
            {
                var repository = new SdkMessageProcessingStepImageRepository(service);
                SdkMessageProcessingStepImage stepImage = await repository.GetStepImageByIdAsync(node.StepImage.Value);

                fileName = EntityFileNameFormatter.GetPluginImageFileName(service.ConnectionData.Name, stepImage.Name, "Description");

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

                {
                    var desc = PluginTypeDescriptionHandler.GetImageDescription(null, stepImage);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(stepImage, null, service.ConnectionData);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);
                    }
                }
            }

            if (node.Message != null && node.Message.Any())
            {
                var repository = new SdkMessageRepository(service);
                List<SdkMessage> listMessages = await repository.GetMessageByIdsAsync(node.Message.ToArray());

                fileName = EntityFileNameFormatter.GetMessageFileName(service.ConnectionData.Name, node.Name, "Description");

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

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

            if (node.MessageFilter != null && node.MessageFilter.Any())
            {
                var repository = new SdkMessageFilterRepository(service);
                List<SdkMessageFilter> listMessages = await repository.GetMessageFiltersByIdsAsync(node.MessageFilter.ToArray());

                fileName = EntityFileNameFormatter.GetMessageFilterFileName(service.ConnectionData.Name, node.Name, "Description");

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

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

            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, result.ToString(), new UTF8Encoding(false));

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("End creating file at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }

            UpdateStatus("Operation is completed.");

            ToggleControls(true);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                trVPluginTree.ItemsSource = null;
            });

            if (!_controlsEnabled)
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
                ShowExistingPlugins();
            }
        }
    }
}