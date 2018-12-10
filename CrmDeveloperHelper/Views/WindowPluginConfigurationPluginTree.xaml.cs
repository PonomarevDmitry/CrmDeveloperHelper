using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction;
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
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowPluginConfigurationPluginTree : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput = null;

        private View _currentView = View.ByEntity;

        private PluginDescription _pluginDescription = null;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private bool _controlsEnabled = true;

        private enum View
        {
            ByEntity = 0,
            ByAssembly = 1,
            ByMessage = 2,
        }

        private BitmapImage _imagePluginAssembly;
        private BitmapImage _imageEntity;
        private BitmapImage _imageImage;
        private BitmapImage _imageMessage;
        private BitmapImage _imagePluginType;
        private BitmapImage _imageStage;
        private BitmapImage _imageStep;
        private BitmapImage _imageStepDisabled;

        public WindowPluginConfigurationPluginTree(
            IWriteToOutput iWriteToOutput
            , ConnectionData connectionData
            , CommonConfiguration commonConfig
            , string filePath
            )
        {
            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = connectionData.ConnectionConfiguration;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            LoadImages();

            LoadConfiguration();

            txtBEntityName.SelectionLength = 0;
            txtBEntityName.SelectionStart = txtBEntityName.Text.Length;

            txtBEntityName.Focus();

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = connectionData;

            if (!string.IsNullOrEmpty(filePath))
            {
                LoadPluginConfiguration(filePath);
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

            cmBCurrentConnection.DataContext = null;
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

            this.txtBEntityName.Text = winConfig.GetValueString(paramEntityName);
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

        private async Task LoadPluginConfiguration(string filePath)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            txtBFilePath.Text = string.Empty;

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                this._pluginDescription = await PluginDescription.LoadAsync(filePath);
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);

                this._pluginDescription = null;
            }

            txtBFilePath.Dispatcher.Invoke(() =>
            {
                if (this._pluginDescription != null)
                {
                    txtBFilePath.Text = filePath;
                }
                else
                {
                    txtBFilePath.Text = string.Empty;
                }
            });

            ShowExistingPlugins();
        }

        private void ShowExistingPlugins()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingPluginConfiguration);

            this.trVPluginTree.ItemsSource = null;
            this.trVPluginTree.Items.Clear();

            if (this._pluginDescription != null)
            {
                ObservableCollection<StepFullInfo> list = LoadPlugins(this._pluginDescription, _currentView);

                this.trVPluginTree.Dispatcher.Invoke(() =>
                {
                    this.trVPluginTree.BeginInit();

                    this.trVPluginTree.ItemsSource = list;

                    this.trVPluginTree.EndInit();
                });
            }

            ToggleControls(true, Properties.WindowStatusStrings.LoadingPluginConfigurationCompleted);
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
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        public class StepFullInfo
        {
            public string Name { get; set; }

            public BitmapImage Image { get; set; }

            public bool IsExpanded { get; set; }

            public bool IsSelected { get; set; }

            public string Tooltip { get; set; }

            public PluginDescription PluginDescription { get; set; }

            public PluginAssembly PluginAssembly { get; set; }

            public PluginType PluginType { get; set; }

            public PluginStep PluginStep { get; set; }

            public PluginImage PluginImage { get; set; }

            public ObservableCollection<StepFullInfo> Items { get; private set; }

            public StepFullInfo()
            {
                this.Items = new ObservableCollection<StepFullInfo>();
            }
        }

        private ObservableCollection<StepFullInfo> LoadPlugins(PluginDescription pluginDescription, View currentView)
        {
            List<StepFullInfo> filterPlugins = FilterPlugins(pluginDescription);

            ObservableCollection<StepFullInfo> list = null;

            switch (currentView)
            {
                case View.ByAssembly:
                    list = FillTreeByAssembly(filterPlugins);
                    break;

                case View.ByMessage:
                    list = FillTreeByMessage(filterPlugins);
                    break;

                case View.ByEntity:
                default:
                    list = FillTreeByEntity(filterPlugins);
                    break;
            }

            ExpandNodes(list);

            return list;
        }

        private List<StepFullInfo> FilterPlugins(PluginDescription pluginDescription)
        {
            IEnumerable<StepFullInfo> steps = null;

            if (this._pluginDescription != null)
            {
                steps = (from a in this._pluginDescription.PluginAssemblies
                         from p in a.PluginTypes
                         from s in p.PluginSteps
                         select new StepFullInfo()
                         {
                             PluginDescription = this._pluginDescription,
                             PluginAssembly = a,
                             PluginType = p,
                             PluginStep = s,
                         }
                        );
            }
            else
            {
                steps = new List<StepFullInfo>();
            }

            string entityName = string.Empty;
            string messageName = string.Empty;
            string pluginTypeName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                entityName = txtBEntityName.Text.Trim();
                messageName = txtBMessageFilter.Text.Trim();
                pluginTypeName = txtBPluginTypeFilter.Text.Trim();

            });

            if (!string.IsNullOrEmpty(pluginTypeName))
            {
                steps = steps.Where(s => s.PluginType.TypeName.StartsWith(pluginTypeName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(messageName))
            {
                steps = steps.Where(s => s.PluginStep.Message.StartsWith(messageName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(entityName))
            {
                steps = steps.Where(s => s.PluginStep.PrimaryEntity.StartsWith(entityName, StringComparison.OrdinalIgnoreCase));
            }

            var stages = GetStages();

            if (stages.Count != 0 && stages.Count != 4)
            {
                steps = steps.Where(s => stages.Any(stage => IsFromStage(s, stage)));
            }

            return steps.ToList();
        }

        private bool IsFromStage(StepFullInfo s, PluginStage stage)
        {
            switch (stage)
            {
                case PluginStage.PreValidation:
                    return s.PluginStep.Stage == 10;

                case PluginStage.Pre:
                    return s.PluginStep.Stage == 20;

                case PluginStage.PostSynch:
                    return s.PluginStep.Stage == 40 && s.PluginStep.ExecutionMode == 0;

                case PluginStage.PostAsych:
                    return s.PluginStep.Stage == 40 && s.PluginStep.ExecutionMode == 1;

                default:
                    break;
            }

            return false;
        }

        private ObservableCollection<StepFullInfo> FillTreeByEntity(List<StepFullInfo> result)
        {
            ObservableCollection<StepFullInfo> list = new ObservableCollection<StepFullInfo>();

            var groupsByEnity = result.GroupBy(ent => ent.PluginStep.PrimaryEntity).OrderBy(gr => gr.Key);

            foreach (var grEntity in groupsByEnity)
            {
                StepFullInfo nodeEntity = CreateNodeEntity(grEntity.Key);

                list.Add(nodeEntity);

                var groupsByMessages = grEntity.GroupBy(ent => ent.PluginStep.Message).OrderBy(mess => mess.Key, new MessageComparer());

                foreach (var mess in groupsByMessages)
                {
                    StepFullInfo nodeMessage = CreateNodeMessage(mess.Key);

                    AddStepFullInfo(nodeEntity, nodeMessage);

                    var groupsStage = mess.GroupBy(ent => ent.PluginStep.Stage.Value).OrderBy(item => item.Key);

                    foreach (var stage in groupsStage)
                    {
                        StepFullInfo nodeStage = null;
                        StepFullInfo nodePostAsynch = null;

                        foreach (var step in stage.OrderBy(s => s.PluginStep.ExecutionMode.Value).ThenBy(s => s.PluginStep.ExecutionOrder).ThenBy(s => s.PluginStep.Name))
                        {
                            StepFullInfo nodeTarget = null;

                            if (step.PluginStep.ExecutionMode.Value == 1)
                            {
                                if (nodePostAsynch == null)
                                {
                                    nodePostAsynch = CreateNodeStage(stage.Key, step.PluginStep.ExecutionMode.Value);

                                    AddStepFullInfo(nodeMessage, nodePostAsynch);
                                }

                                nodeTarget = nodePostAsynch;
                            }
                            else
                            {
                                if (nodeStage == null)
                                {
                                    nodeStage = CreateNodeStage(stage.Key, step.PluginStep.ExecutionMode.Value);

                                    AddStepFullInfo(nodeMessage, nodeStage);
                                }

                                nodeTarget = nodeStage;
                            }

                            StepFullInfo nodeStep = CreateNodeStep(step);

                            AddStepFullInfo(nodeTarget, nodeStep);

                            var queryImage = from image in step.PluginStep.PluginImages
                                             orderby image.ImageType.Value
                                             select image;

                            foreach (var image in queryImage)
                            {
                                StepFullInfo nodeImage = CreateNodeImage(image);

                                AddStepFullInfo(nodeStep, nodeImage);
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

        private ObservableCollection<StepFullInfo> FillTreeByMessage(List<StepFullInfo> result)
        {
            ObservableCollection<StepFullInfo> list = new ObservableCollection<StepFullInfo>();

            var groupsByMessage = result.GroupBy(ent => ent.PluginStep.Message).OrderBy(mess => mess.Key, new MessageComparer());

            foreach (var grMessage in groupsByMessage)
            {
                var nodeMessage = CreateNodeMessage(grMessage.Key);

                list.Add(nodeMessage);

                var groupsByEntity = grMessage.GroupBy(ent => ent.PluginStep.PrimaryEntity).OrderBy(e => e.Key);

                foreach (var grEntity in groupsByEntity)
                {
                    StepFullInfo nodeEntity = CreateNodeEntity(grEntity.Key);

                    AddStepFullInfo(nodeMessage, nodeEntity);

                    var groupsStage = grEntity.GroupBy(ent => ent.PluginStep.Stage.Value).OrderBy(item => item.Key);

                    foreach (var stage in groupsStage)
                    {
                        StepFullInfo nodeStage = null;
                        StepFullInfo nodePostAsynch = null;

                        foreach (var step in stage.OrderBy(s => s.PluginStep.ExecutionMode.Value).ThenBy(s => s.PluginStep.ExecutionOrder).ThenBy(s => s.PluginStep.Name))
                        {
                            StepFullInfo nodeTarget = null;

                            if (step.PluginStep.ExecutionMode.Value == 1)
                            {
                                if (nodePostAsynch == null)
                                {
                                    nodePostAsynch = CreateNodeStage(stage.Key, step.PluginStep.ExecutionMode.Value);

                                    AddStepFullInfo(nodeEntity, nodePostAsynch);
                                }

                                nodeTarget = nodePostAsynch;
                            }
                            else
                            {
                                if (nodeStage == null)
                                {
                                    nodeStage = CreateNodeStage(stage.Key, step.PluginStep.ExecutionMode.Value);

                                    AddStepFullInfo(nodeEntity, nodeStage);
                                }

                                nodeTarget = nodeStage;
                            }

                            StepFullInfo nodeStep = CreateNodeStep(step);

                            AddStepFullInfo(nodeTarget, nodeStep);

                            var queryImage = from image in step.PluginStep.PluginImages
                                             orderby image.ImageType.Value
                                             select image;

                            foreach (var image in queryImage)
                            {
                                StepFullInfo nodeImage = CreateNodeImage(image);

                                AddStepFullInfo(nodeStep, nodeImage);
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

        private ObservableCollection<StepFullInfo> FillTreeByAssembly(List<StepFullInfo> result)
        {
            ObservableCollection<StepFullInfo> list = new ObservableCollection<StepFullInfo>();

            var groupsByAssembly = result.GroupBy(ent => ent.PluginAssembly.Name).OrderBy(gr => gr.Key);

            foreach (var grAssembly in groupsByAssembly)
            {
                var assemblyStep = grAssembly.FirstOrDefault();

                var nodeAssembly = CreateNodeAssembly(assemblyStep);

                list.Add(nodeAssembly);

                var groupsByPluginType = grAssembly.GroupBy(ent => ent.PluginType.TypeName).OrderBy(gr => gr.Key);

                foreach (var grPluginType in groupsByPluginType)
                {
                    var pluginTypeStep = grPluginType.FirstOrDefault();

                    var nodePluginType = CreateNodePluginType(pluginTypeStep);

                    AddStepFullInfo(nodeAssembly, nodePluginType);

                    var groupsByEnity = grPluginType.GroupBy(ent => ent.PluginStep.PrimaryEntity).OrderBy(gr => gr.Key);

                    foreach (var grEntity in groupsByEnity)
                    {
                        var nodeEntity = CreateNodeEntity(grEntity.Key);

                        AddStepFullInfo(nodePluginType, nodeEntity);

                        var groupsByMessages = grEntity.GroupBy(ent => ent.PluginStep.Message).OrderBy(mess => mess.Key, new MessageComparer());

                        foreach (var mess in groupsByMessages)
                        {
                            StepFullInfo nodeMessage = CreateNodeMessage(mess.Key);

                            AddStepFullInfo(nodeEntity, nodeMessage);

                            var groupsStage = mess.GroupBy(ent => ent.PluginStep.Stage.Value).OrderBy(item => item.Key);

                            foreach (var stage in groupsStage)
                            {
                                StepFullInfo nodeStage = null;
                                StepFullInfo nodePostAsynch = null;

                                foreach (var step in stage.OrderBy(s => s.PluginStep.ExecutionMode).ThenBy(s => s.PluginStep.ExecutionOrder).ThenBy(s => s.PluginStep.Name))
                                {
                                    StepFullInfo nodeTarget = null;

                                    if (step.PluginStep.ExecutionMode.Value == 1)
                                    {
                                        if (nodePostAsynch == null)
                                        {
                                            nodePostAsynch = CreateNodeStage(stage.Key, step.PluginStep.ExecutionMode.Value);

                                            AddStepFullInfo(nodeMessage, nodePostAsynch);
                                        }

                                        nodeTarget = nodePostAsynch;
                                    }
                                    else
                                    {
                                        if (nodeStage == null)
                                        {
                                            nodeStage = CreateNodeStage(stage.Key, step.PluginStep.ExecutionMode.Value);

                                            AddStepFullInfo(nodeMessage, nodeStage);
                                        }

                                        nodeTarget = nodeStage;
                                    }

                                    StepFullInfo nodeStep = CreateNodeStep(step);

                                    AddStepFullInfo(nodeTarget, nodeStep);

                                    var queryImage = from image in step.PluginStep.PluginImages
                                                     orderby image.ImageType.Value
                                                     select image;

                                    foreach (var image in queryImage)
                                    {
                                        StepFullInfo nodeImage = CreateNodeImage(image);

                                        AddStepFullInfo(nodeStep, nodeImage);
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

        private void AddStepFullInfo(StepFullInfo node, StepFullInfo childNode)
        {
            node.Items.Add(childNode);
        }

        private StepFullInfo CreateNodeAssembly(StepFullInfo assemblyStep)
        {
            var nodeMessage = new StepFullInfo()
            {
                Name = assemblyStep.PluginAssembly.Name,
                Image = _imagePluginAssembly,

                PluginDescription = assemblyStep.PluginDescription,
                PluginAssembly = assemblyStep.PluginAssembly,
            };

            return nodeMessage;
        }

        private StepFullInfo CreateNodeImage(PluginImage image)
        {
            string nameImage = GetImageName(image);
            string tooltipImage = GetImageTooltip(image);

            var nodeImage = new StepFullInfo()
            {
                Name = nameImage,
                Image = _imageImage,

                Tooltip = tooltipImage,
            };

            return nodeImage;
        }

        private StepFullInfo CreateNodeStep(StepFullInfo step)
        {
            string nameStep = GetStepName(step);
            string tooltipStep = GetStepTooltip(step);

            step.Name = nameStep;
            step.Tooltip = tooltipStep;

            step.Image = step.PluginStep.StateCode == 0 ? _imageStep : _imageStepDisabled;

            return step;
        }

        private StepFullInfo CreateNodeStage(int stage, int mode)
        {
            string temp = Nav.Common.VSPackages.CrmDeveloperHelper.Repository.SdkMessageProcessingStepRepository.GetStageName(stage, mode);

            var nodeStage = new StepFullInfo()
            {
                Name = temp,
                Image = _imageStage,
            };

            return nodeStage;
        }

        private StepFullInfo CreateNodePluginType(StepFullInfo pluginTypeStep)
        {
            var nodeType = new StepFullInfo()
            {
                Name = pluginTypeStep.PluginType.TypeName,

                Image = _imagePluginType,

                PluginDescription = pluginTypeStep.PluginDescription,
                PluginAssembly = pluginTypeStep.PluginAssembly,
                PluginType = pluginTypeStep.PluginType,
            };

            return nodeType;
        }

        private StepFullInfo CreateNodeMessage(string message)
        {
            var nodeMessage = new StepFullInfo()
            {
                Name = message,
                Image = _imageMessage,
            };

            return nodeMessage;
        }

        private StepFullInfo CreateNodeEntity(string entityName)
        {
            var nodeMessage = new StepFullInfo()
            {
                Name = entityName,
                Image = _imageEntity,
            };

            return nodeMessage;
        }

        private string GetStepTooltip(StepFullInfo step)
        {
            if (!step.PluginStep.FilteringAttributes.Any())
            {
                return null;
            }

            StringBuilder tooltipStep = new StringBuilder();

            tooltipStep.AppendLine("Filtering:");

            foreach (string item in step.PluginStep.FilteringAttributes.OrderBy(s => s))
            {
                tooltipStep.AppendLine().AppendFormat("  " + item);
            }

            return tooltipStep.ToString();
        }

        private string GetStepName(StepFullInfo step)
        {
            StringBuilder nameStep = new StringBuilder();

            if (step.PluginStep.ExecutionOrder.HasValue)
            {
                nameStep.AppendFormat("{0}. ", step.PluginStep.ExecutionOrder.ToString());
            }

            nameStep.Append(step.PluginType.TypeName);

            if (step.PluginStep.FilteringAttributes.Count > 0)
            {
                nameStep.AppendFormat("    Filtering: {0}", string.Join(",", step.PluginStep.FilteringAttributes.OrderBy(s => s)));
            }

            return nameStep.ToString();
        }

        private string GetImageTooltip(PluginImage image)
        {
            if (!image.Attributes.Any())
            {
                return null;
            }

            StringBuilder tooltipImage = new StringBuilder();

            tooltipImage.AppendLine("Attributes:");

            foreach (string item in image.Attributes.OrderBy(s => s))
            {
                tooltipImage.AppendLine().Append("  " + item);
            }

            return tooltipImage.ToString();
        }

        private string GetImageName(PluginImage image)
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

            if (image.Attributes.Any())
            {
                nameImage.AppendFormat("Attributes: {0}", string.Join(",", image.Attributes.OrderBy(s => s)));
            }
            else
            {
                nameImage.Append("Attributes: All");
            }

            return nameImage.ToString();
        }

        private void ExpandNodes(ObservableCollection<StepFullInfo> list)
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

        private void ExpandNode(StepFullInfo node)
        {
            node.IsExpanded = true;
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSBCollapseAll, enabled);
            ToggleControl(this.tSBExpandAll, enabled);
            ToggleControl(this.menuView, enabled);
            ToggleControl(this.tSBLoadPluginConfiguraion, enabled);

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
                    {
                        bool enabled = this._controlsEnabled
                                            && this.trVPluginTree.SelectedItem != null
                                            && this.trVPluginTree.SelectedItem is StepFullInfo
                                            && (this.trVPluginTree.SelectedItem as StepFullInfo).PluginType != null;

                        UIElement[] list = { tSBCreateDescription };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }

                    {
                        ConnectionData connectionData = null;

                        cmBCurrentConnection.Dispatcher.Invoke(() =>
                        {
                            connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                        });

                        bool enabled = this._controlsEnabled
                                            && connectionData != null
                                            && connectionData.IsReadOnly == false
                                            && this.trVPluginTree.SelectedItem != null
                                            && this.trVPluginTree.SelectedItem is StepFullInfo
                                            && (this.trVPluginTree.SelectedItem as StepFullInfo).PluginAssembly != null;

                        UIElement[] list = { tSBRegisterSteps };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled && !connectionData.IsReadOnly;
                        }
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingPlugins();
            }
        }

        private StepFullInfo GetSelectedEntity()
        {
            StepFullInfo result = null;

            if (this.trVPluginTree.SelectedItem != null
                && this.trVPluginTree.SelectedItem is StepFullInfo
                )
            {
                result = this.trVPluginTree.SelectedItem as StepFullInfo;
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
                foreach (StepFullInfo item in trVPluginTree.Items)
                {
                    RecursiveExpandedAll(item, isExpanded);
                }
            }

            trVPluginTree.EndInit();
        }

        private void RecursiveExpandedAll(StepFullInfo item, bool isExpanded)
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

        private async Task CreateDescription(StepFullInfo node)
        {
            if (node.PluginType == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.CreatingFileWithDescription);

            ToggleControls(false, Properties.WindowStatusStrings.CreatingDescription);

            StringBuilder result = new StringBuilder();

            string fileName = string.Empty;

            if (node.PluginType != null)
            {
                const string _formatFileTxt = "{0} {1} - Description.txt";

                fileName = string.Format(
                    _formatFileTxt
                  , Path.GetFileNameWithoutExtension(node.PluginDescription.FilePath)
                  , node.PluginType.TypeName);

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                {
                    PluginTypeConfigurationDescriptionHandler handler = new PluginTypeConfigurationDescriptionHandler();

                    var desc = await handler.CreateDescriptionAsync(node.PluginType);

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

                this._iWriteToOutput.PerformAction(filePath);
            }

            ToggleControls(true, Properties.WindowStatusStrings.CreatingDescriptionCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.CreatingFileWithDescription);
        }

        private void tSBLoadPluginConfiguraion_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = string.Empty;
            var t = new Thread(() =>
            {
                try
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "Plugin Configuration (.xml)|*.xml",
                        FilterIndex = 1,
                        RestoreDirectory = true
                    };

                    if (openFileDialog1.ShowDialog().GetValueOrDefault())
                    {
                        selectedPath = openFileDialog1.FileName;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();

            if (!string.IsNullOrEmpty(selectedPath))
            {
                LoadPluginConfiguration(selectedPath);
            }
        }

        private async void tSBRegisterSteps_Click(object sender, RoutedEventArgs e)
        {
            var step = GetSelectedEntity();

            if (step == null)
            {
                return;
            }

            string message = string.Empty;

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (step.PluginStep != null)
            {
                message = string.Format(Properties.MessageBoxStrings.RegisterPluginStepFormat1, connectionData?.Name);
            }
            else if (step.PluginType != null)
            {
                message = string.Format(Properties.MessageBoxStrings.RegisterAllStepsForPluginTypeFormat1, connectionData?.Name);
            }
            else if (step.PluginAssembly != null)
            {
                message = string.Format(Properties.MessageBoxStrings.RegisterAllStepsForPluginAssemblyFormat1, connectionData?.Name);
            }

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                await RegisterSteps(step);
            }
        }

        private async Task RegisterSteps(StepFullInfo step)
        {
            if (step.PluginType == null)
            {
                return;
            }

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData.IsReadOnly)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.RegisteringPluginStepsFormat1, connectionData.Name);

            if (connectionData != null && connectionData.IsReadOnly == false)
            {
                var service = await GetService();

                var helper = new RegistrerPluginHelper(service);

                try
                {
                    string filePath = string.Empty;

                    if (step.PluginStep != null)
                    {
                        filePath = await helper.RegisterPluginsForPluginStepAsync(this._commonConfig.FolderForExport, step.PluginAssembly.Name, step.PluginType.TypeName, step.PluginStep);
                    }
                    else if (step.PluginType != null)
                    {
                        filePath = await helper.RegisterPluginsForPluginTypeAsync(this._commonConfig.FolderForExport, step.PluginAssembly.Name, step.PluginType);
                    }
                    else if (step.PluginAssembly != null)
                    {
                        filePath = await helper.RegisterPluginsForAssemblyAsync(this._commonConfig.FolderForExport, step.PluginAssembly);
                    }

                    if (File.Exists(filePath))
                    {
                        this._iWriteToOutput.PerformAction(filePath);
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }

            ToggleControls(true, Properties.WindowStatusStrings.RegisteringPluginStepsCompletedFormat1, connectionData.Name);
        }
    }
}