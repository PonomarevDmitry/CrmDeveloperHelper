using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowTreePlugin : WindowWithMessageFilters
    {
        private readonly ObservableCollection<PluginTreeViewItem> _pluginTree = new ObservableCollection<PluginTreeViewItem>();

        private BitmapImage _imageRefresh;

        private BitmapImage _imagePluginAssembly;
        private BitmapImage _imagePluginType;

        private BitmapImage _imageEntity;

        private BitmapImage _imageMessage;
        private BitmapImage _imageMessageCategory;

        private BitmapImage _imageStep;
        private BitmapImage _imageStepDisabled;
        private BitmapImage _imageImage;

        private BitmapImage _imageStage;
        private BitmapImage _imageMode;

        private BitmapImage _imageBusinessRule;
        private BitmapImage _imageBusinessProcess;
        private BitmapImage _imageWorkflowActivity;

        private readonly List<GroupingProperty> _currentGrouping = new List<GroupingProperty>()
        {
            GroupingProperty.PluginAssembly
            , GroupingProperty.PluginType
            , GroupingProperty.Entity
            , GroupingProperty.Message
        };

        private readonly List<List<GroupingProperty>> _predifinedView = new List<List<GroupingProperty>>()
        {
            new List<GroupingProperty>()
            {
                GroupingProperty.PluginAssembly
                , GroupingProperty.PluginType
                , GroupingProperty.Entity
                , GroupingProperty.Message
            }

            , new List<GroupingProperty>()
            {
                GroupingProperty.PluginAssembly
                , GroupingProperty.PluginType
                , GroupingProperty.Message
                , GroupingProperty.Entity
            }

            , new List<GroupingProperty>()
            {
                GroupingProperty.Entity
                , GroupingProperty.Message
                , GroupingProperty.PluginType
                , GroupingProperty.Stage
            }

            , new List<GroupingProperty>()
            {
                GroupingProperty.Message
                , GroupingProperty.Entity
                , GroupingProperty.PluginType
                , GroupingProperty.Stage
            }
        };

        private readonly List<GroupingProperty> _groupingPropertiesAll = Enum.GetValues(typeof(GroupingProperty)).OfType<GroupingProperty>().ToList();

        private readonly Dictionary<GroupingProperty, RequestGroupBuilder> _propertyGroups = new Dictionary<GroupingProperty, RequestGroupBuilder>();

        public WindowTreePlugin(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string entityFilter
            , string pluginTypeFilter
            , string messageFilter
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            cmBEntityName.Text = entityFilter;
            txtBPluginTypeFilter.Text = pluginTypeFilter;
            txtBMessageFilter.Text = messageFilter;

            LoadFromConfig();

            FillEntityNames(service.ConnectionData);

            cmBStatusCode.ItemsSource = new EnumBindingSourceExtension(typeof(SdkMessageProcessingStep.Schema.OptionSets.statuscode?)).ProvideValue(null) as IEnumerable;

            LoadImages();

            LoadConfiguration();

            FillPredefinedViews();

            FillViewGroups();

            FocusOnComboBoxTextBox(cmBEntityName);

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            trVPluginTree.ItemsSource = _pluginTree;

            FillExplorersMenuItems();

            this.DecreaseInit();

            ShowExistingPlugins();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getEntityName: GetEntityName
                , getMessageName: GetMessageName
                , getPluginTypeName: GetPluginTypeName
                , getPluginAssemblyName: GetPluginAssemblyName
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

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterTree_Click, "mIOpenMessageFilterTree");

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

        private string GetPluginTypeName()
        {
            var entity = GetSelectedEntity();

            return entity?.PluginTypeName ?? txtBPluginTypeFilter.Text.Trim();
        }

        private string GetPluginAssemblyName()
        {
            var entity = GetSelectedEntity();

            return entity?.PluginAssemblyName ?? txtBPluginTypeFilter.Text.Trim();
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
        private const string paramPluginTypeName = "PluginTypeName";

        private const string paramPreValidationStage = "PreValidationStage";
        private const string paramPreStage = "PreStage";
        private const string paramPostSynchStage = "PostSynchStage";
        private const string paramPostAsynchStage = "PostAsynchStage";
        private const string paramBusinessRules = "BusinessRules";
        private const string paramBusinessProcesses = "BusinessProcesses";
        private const string paramStatusCode = "StatusCode";

        private const string paramGrouping = "Grouping";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            if (string.IsNullOrEmpty(this.cmBEntityName.Text)
                && string.IsNullOrEmpty(this.txtBPluginTypeFilter.Text)
                && string.IsNullOrEmpty(this.txtBMessageFilter.Text)
                )
            {
                this.cmBEntityName.Text = winConfig.GetValueString(paramEntityName);
                this.txtBPluginTypeFilter.Text = winConfig.GetValueString(paramPluginTypeName);
                this.txtBMessageFilter.Text = winConfig.GetValueString(paramMessage);
            }

            this.chBStagePreValidation.IsChecked = winConfig.GetValueBool(paramPreValidationStage).GetValueOrDefault();
            this.chBStagePre.IsChecked = winConfig.GetValueBool(paramPreStage).GetValueOrDefault();
            this.chBStagePostSync.IsChecked = winConfig.GetValueBool(paramPostSynchStage).GetValueOrDefault();
            this.chBStagePostAsync.IsChecked = winConfig.GetValueBool(paramPostAsynchStage).GetValueOrDefault();

            this.chBBusinessRules.IsChecked = winConfig.GetValueBool(paramBusinessRules).GetValueOrDefault();

            this.chBBusinessProcesses.IsChecked = winConfig.GetValueBool(paramBusinessProcesses).GetValueOrDefault();

            var statusValue = winConfig.GetValueInt(paramStatusCode);
            if (statusValue != -1)
            {
                var item = cmBStatusCode.Items.OfType<SdkMessageProcessingStep.Schema.OptionSets.statuscode?>().FirstOrDefault(e => (int)e == statusValue);
                if (item != null)
                {
                    cmBStatusCode.SelectedItem = item;
                }
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
            winConfig.DictString[paramPluginTypeName] = this.txtBPluginTypeFilter.Text.Trim();

            winConfig.DictBool[paramPreValidationStage] = this.chBStagePreValidation.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPreStage] = this.chBStagePre.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPostSynchStage] = this.chBStagePostSync.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPostAsynchStage] = this.chBStagePostAsync.IsChecked.GetValueOrDefault();

            winConfig.DictBool[paramBusinessRules] = this.chBBusinessRules.IsChecked.GetValueOrDefault();

            winConfig.DictBool[paramBusinessProcesses] = this.chBBusinessProcesses.IsChecked.GetValueOrDefault();

            winConfig.DictString[paramGrouping] = string.Join(",", _currentGrouping.Select(g => g.ToString()));

            var statusValue = -1;

            {
                if (cmBStatusCode.SelectedItem is SdkMessageProcessingStep.Schema.OptionSets.statuscode comboBoxItem)
                {
                    statusValue = (int)comboBoxItem;
                }
            }

            winConfig.DictInt[paramStatusCode] = statusValue;
        }

        private void LoadImages()
        {
            this._imageRefresh = this.Resources["ImageRefresh"] as BitmapImage;

            this._imagePluginAssembly = this.Resources["ImagePluginAssembly"] as BitmapImage;
            this._imagePluginType = this.Resources["ImagePluginType"] as BitmapImage;

            this._imageEntity = this.Resources["ImageEntity"] as BitmapImage;

            this._imageMessage = this.Resources["ImageMessage"] as BitmapImage;
            this._imageMessageCategory = this.Resources["ImageMessageCategory"] as BitmapImage;

            this._imageStep = this.Resources["ImageStep"] as BitmapImage;
            this._imageStepDisabled = this.Resources["ImageStepDisabled"] as BitmapImage;
            this._imageImage = this.Resources["ImageImage"] as BitmapImage;

            this._imageStage = this.Resources["ImageStage"] as BitmapImage;
            this._imageMode = this.Resources["ImageMode"] as BitmapImage;

            this._imageBusinessRule = this.Resources["ImageBusinessRule"] as BitmapImage;
            this._imageBusinessProcess = this.Resources["ImageBusinessProcess"] as BitmapImage;
            this._imageWorkflowActivity = this.Resources["ImageWorkflowActivity"] as BitmapImage;
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingPlugins();
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

            PluginAssembly,

            PluginType,

            Stage,

            Mode,
        }

        private void FillPredefinedViews()
        {
            mIView.Items.Clear();

            for (int i = 0; i < _predifinedView.Count; i++)
            {
                var view = _predifinedView[i];

                var menuItem = new MenuItem()
                {
                    Header = string.Join(" -> ", view.Select(g => g.ToString())),
                    Tag = view,
                };

                menuItem.Click += this.mIPredefinedView_Click;

                mIView.Items.Add(menuItem);
            }

            UpdatePredefinedViewCheck();
        }

        private async void mIPredefinedView_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            if (!(menuItem.Tag is List<GroupingProperty> grouping))
            {
                return;
            }

            if (_currentGrouping.SequenceEqual(grouping))
            {
                return;
            }

            SetGrouping(grouping);

            UpdateGroupingVisibility();

            this._currentGrouping.Clear();

            this._currentGrouping.AddRange(grouping);

            UpdatePredefinedViewCheck();

            await ShowExistingPlugins();
        }

        private void UpdatePredefinedViewCheck()
        {
            foreach (MenuItem item in mIView.Items)
            {
                if (item.Tag is List<GroupingProperty> grouping)
                {
                    item.IsChecked = this._currentGrouping.SequenceEqual(grouping);
                }
            }
        }

        private void FillViewGroups()
        {
            _propertyGroups.Clear();

            _propertyGroups[GroupingProperty.Entity] = new RequestGroupBuilder()
            {
                GroupingProperty = GroupingProperty.Entity,
                FillTree = FillTreeByEntity,
                GroupSteps = GroupStepsByEntity,
            };

            _propertyGroups[GroupingProperty.Message] = new RequestGroupBuilder()
            {
                GroupingProperty = GroupingProperty.Message,
                FillTree = FillTreeByMessage,
                GroupSteps = GroupStepsByMessage,
            };

            _propertyGroups[GroupingProperty.MessageCategory] = new RequestGroupBuilder()
            {
                GroupingProperty = GroupingProperty.MessageCategory,
                FillTree = FillTreeByMessageCategory,
                GroupSteps = GroupStepsByMessageCategory,
            };

            _propertyGroups[GroupingProperty.PluginAssembly] = new RequestGroupBuilder()
            {
                GroupingProperty = GroupingProperty.PluginAssembly,
                FillTree = FillTreeByPluginAssembly,
                GroupSteps = GroupStepsByPluginAssembly,
            };

            _propertyGroups[GroupingProperty.PluginType] = new RequestGroupBuilder()
            {
                GroupingProperty = GroupingProperty.PluginType,
                FillTree = FillTreeByPluginType,
                GroupSteps = GroupStepsByPluginType,
            };

            _propertyGroups[GroupingProperty.Stage] = new RequestGroupBuilder()
            {
                GroupingProperty = GroupingProperty.Stage,
                FillTree = FillTreeByStage,
                GroupSteps = GroupStepsByStage,
            };

            _propertyGroups[GroupingProperty.Mode] = new RequestGroupBuilder()
            {
                GroupingProperty = GroupingProperty.Mode,
                FillTree = FillTreeByMode,
                GroupSteps = GroupStepsByMode,
            };

            mICustomView.Items.Clear();

            mICustomView.SubmenuClosed += this.mICustomView_SubmenuClosed;

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

            menuItemRefreshView.Click += this.mICustomView_SubmenuClosed;

            mICustomView.Items.Add(menuItemRefreshView);
            mICustomView.Items.Add(new Separator());

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

                mICustomView.Items.Add(comboBox);
            }

            this.DecreaseInit();

            UpdateGroupingVisibility();
        }

        private async void mICustomView_SubmenuClosed(object sender, RoutedEventArgs e)
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

                UpdatePredefinedViewCheck();

                await ShowExistingPlugins();
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

            foreach (var comboBox in mICustomView.Items.OfType<ComboBox>())
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

            foreach (var comboBox in mICustomView.Items.OfType<ComboBox>())
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

            foreach (var comboBox in mICustomView.Items.OfType<ComboBox>())
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

        private void SetGrouping(List<GroupingProperty> grouping)
        {
            foreach (var comboBox in mICustomView.Items.OfType<ComboBox>())
            {
                comboBox.SelectedIndex = 0;
            }

            var index = 0;

            foreach (var comboBox in mICustomView.Items.OfType<ComboBox>())
            {
                if (index < grouping.Count)
                {
                    comboBox.SelectedItem = grouping[index];
                }

                index++;
            }
        }

        private class RequestGroupBuilder
        {
            public GroupingProperty GroupingProperty { get; set; }

            public Func<
                IOrganizationServiceExtented
                , List<PluginStage>
                , string
                , string
                , string
                , SdkMessageProcessingStep.Schema.OptionSets.statuscode?
                , IEnumerable<RequestGroupBuilder>

                , Task> // Func Result
                FillTree
            { get; set; }

            public Func<
                IOrganizationServiceExtented
                , PluginSearchResult
                , IEnumerable<SdkMessageProcessingStep>
                , IEnumerable<RequestGroupBuilder>
                , IEnumerable<Action<PluginTreeViewItem>>

                , IEnumerable<PluginTreeViewItem> // Func Result
                > GroupSteps
            { get; set; }

            public override string ToString() => this.GroupingProperty.ToString();
        }

        #region Entity

        private async Task FillTreeByEntity(
            IOrganizationServiceExtented service
            , List<PluginStage> stages
            , string pluginTypeNameFilter
            , string messageNameFilter
            , string entityNameFilter
            , SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<RequestGroupBuilder> requestGroups
        )
        {
            PluginSearchRepository repository = new PluginSearchRepository(service);

            PluginSearchResult search = await repository.FindAllAsync(stages, pluginTypeNameFilter, messageNameFilter, entityNameFilter, statuscode);

            foreach (var nodeEntity in GroupStepsByEntity(
                service
                , search
                , search.SdkMessageProcessingStep
                , requestGroups
                , new Action<PluginTreeViewItem>[0]
            ))
            {
                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(nodeEntity);
                });
            }

            //var groupsList = steps.GroupBy(s => s.PrimaryObjectTypeCodeName, StringComparer.InvariantCultureIgnoreCase);

            //List<ProcessingStep> processingSteps = new List<ProcessingStep>();

            //processingSteps.AddRange(searchResult.SdkMessageProcessingStep.Select(e => new ProcessingStep(e.PrimaryObjectTypeCodeName, e.SdkMessageId?.Name ?? "Unknown", e.Stage?.Value, e.Mode?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1, e.Rank, e.Name, e)));

            //var repWorkflow = new WorkflowRepository(service);

            //if (withBusinessProcesses)
            //{
            //    IEnumerable<Workflow> businessProcesses = await repWorkflow.GetListAsync(entityNameFilter, (int)Workflow.Schema.OptionSets.category.Workflow_0, null, _columnSetWorkflow);

            //    foreach (var item in businessProcesses)
            //    {
            //        if ("Create".StartsWith(messageNameFilter, StringComparison.InvariantCultureIgnoreCase) && item.TriggerOnCreate.GetValueOrDefault())
            //        {
            //            processingSteps.Add(new ProcessingStep(item.PrimaryEntity, "Create", item.CreateStage?.Value ?? (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40, item.Mode?.Value == (int)Workflow.Schema.OptionSets.mode.Background_0, item.Rank, item.Name, item));
            //        }

            //        if ("Update".StartsWith(messageNameFilter, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(item.TriggerOnUpdateAttributeList))
            //        {
            //            processingSteps.Add(new ProcessingStep(item.PrimaryEntity, "Update", item.UpdateStage?.Value ?? (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40, item.Mode?.Value == (int)Workflow.Schema.OptionSets.mode.Background_0, item.Rank, item.Name, item));
            //        }

            //        if ("Delete".StartsWith(messageNameFilter, StringComparison.InvariantCultureIgnoreCase) && item.TriggerOnDelete.GetValueOrDefault())
            //        {
            //            processingSteps.Add(new ProcessingStep(item.PrimaryEntity, "Delete", item.DeleteStage?.Value ?? (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40, item.Mode?.Value == (int)Workflow.Schema.OptionSets.mode.Background_0, item.Rank, item.Name, item));
            //        }
            //    }
            //}

            //var listEntities = new HashSet<string>(processingSteps.Select(ent => ent.EntityName), StringComparer.InvariantCultureIgnoreCase);

            //IEnumerable<Workflow> businessRules = null;

            //if (withBusinessRules)
            //{
            //    businessRules = await repWorkflow.GetListAsync(entityNameFilter, (int)Workflow.Schema.OptionSets.category.Business_Rule_2, null, _columnSetWorkflow);

            //    foreach (var item in businessRules)
            //    {
            //        listEntities.Add(item.PrimaryEntity);
            //    }
            //}

            //foreach (var entityName in listEntities.OrderBy(s => s))
            //{
            //    var grEntity = processingSteps.Where(e => string.Equals(e.EntityName, entityName, StringComparison.InvariantCultureIgnoreCase));

            //    PluginTreeViewItem nodeEntity = CreateNodeMessageFilter(entityName, null, grEntity.Where(e => e.Entity is SdkMessageProcessingStep).Select(e => e.Entity as SdkMessageProcessingStep));

            //    //if (businessRules != null)
            //    //{
            //    //    var entityRules = businessRules.Where(w => string.Equals(w.PrimaryEntity, entityName, StringComparison.InvariantCultureIgnoreCase));

            //    //    if (entityRules.Any())
            //    //    {
            //    //        var nodeRules = CreateNodeBusinessRules(entityName);

            //    //        nodeEntity.Items.Add(nodeRules);
            //    //        nodeRules.Parent = nodeEntity;

            //    //        foreach (var item in entityRules.OrderBy(w => w.Name))
            //    //        {
            //    //            var nodeRule = CreateNodeBusinessRule(item);

            //    //            nodeRules.Items.Add(nodeRule);
            //    //            nodeRule.Parent = nodeRules;
            //    //        }
            //    //    }
            //    //}

            //    var groupsByMessages = grEntity.GroupBy(e => e.Message).OrderBy(mess => mess.Key, MessageComparer.Comparer);

            //    foreach (var grMessage in groupsByMessages)
            //    {
            //        PluginTreeViewItem nodeMessage = CreateNodeMessage(grMessage.Key, grMessage.Where(e => e.Entity is SdkMessageProcessingStep).Select(e => e.Entity as SdkMessageProcessingStep));

            //        nodeEntity.Items.Add(nodeMessage);
            //        nodeMessage.Parent = nodeEntity;

            //        var groupsStage = grMessage.GroupBy(ent => ent.Stage.Value).OrderBy(item => item.Key);

            //        foreach (var stage in groupsStage)
            //        {
            //            PluginTreeViewItem nodeStage = null;
            //            PluginTreeViewItem nodePostAsynch = null;

            //            foreach (var step in stage.OrderBy(s => s.AsyncMode).ThenBy(s => s.Rank).ThenBy(s => s.Name))
            //            {
            //                PluginTreeViewItem nodeTarget = null;

            //                if (step.AsyncMode)
            //                {
            //                    if (nodePostAsynch == null)
            //                    {
            //                        nodePostAsynch = CreateNodeStage(stage.Key, step.AsyncMode ? 1 : 0);

            //                        nodeMessage.Items.Add(nodePostAsynch);
            //                        nodePostAsynch.Parent = nodeMessage;
            //                    }

            //                    nodeTarget = nodePostAsynch;
            //                }
            //                else
            //                {
            //                    if (nodeStage == null)
            //                    {
            //                        nodeStage = CreateNodeStage(stage.Key, step.AsyncMode ? 1 : 0);

            //                        nodeMessage.Items.Add(nodeStage);
            //                        nodeStage.Parent = nodeMessage;
            //                    }

            //                    nodeTarget = nodeStage;
            //                }

            //                PluginTreeViewItem nodeStep = CreateNodeStep(step.Entity, searchResult.SdkMessageProcessingStepImage);

            //                nodeTarget.Items.Add(nodeStep);
            //                nodeStep.Parent = nodeTarget;

            //                nodeStep.IsExpanded = true;
            //            }

            //            if (nodeStage != null)
            //            {
            //                nodeStage.IsExpanded = true;
            //            }

            //            if (nodePostAsynch != null)
            //            {
            //                nodePostAsynch.IsExpanded = true;
            //            }
            //        }

            //        nodeMessage.IsExpanded = true;
            //    }


            //}
        }

        private IEnumerable<PluginTreeViewItem> GroupStepsByEntity(
            IOrganizationServiceExtented service
            , PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<PluginTreeViewItem>> actionsOnChilds
        )
        {
            var groupsList = steps.GroupBy(s => s.PrimaryObjectTypeCodeName, StringComparer.InvariantCultureIgnoreCase).OrderBy(k => k.Key);

            foreach (var grEntity in groupsList)
            {
                var nodeEntity = new PluginTreeViewItem(ComponentType.Entity)
                {
                    Name = grEntity.Key,
                    Image = _imageEntity,

                    EntityLogicalName = grEntity.Key,
                };

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(nodeEntity);
                }

                var newActionOnChilds = actionsOnChilds.Union(new Action<PluginTreeViewItem>[] { n => n.EntityLogicalName = grEntity.Key });

                RecursiveGroup(service, nodeEntity, search, grEntity, requestGroups, newActionOnChilds);

                yield return nodeEntity;
            }
        }

        #endregion Entity

        #region Message

        private async Task FillTreeByMessage(
            IOrganizationServiceExtented service
            , List<PluginStage> stages
            , string pluginTypeNameFilter
            , string messageNameFilter
            , string entityNameFilter
            , SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<RequestGroupBuilder> requestGroups
        )
        {
            PluginSearchRepository repository = new PluginSearchRepository(service);

            PluginSearchResult search = await repository.FindAllAsync(stages, pluginTypeNameFilter, messageNameFilter, entityNameFilter, statuscode);

            foreach (var nodeMessage in GroupStepsByMessage(
                service
                , search
                , search.SdkMessageProcessingStep
                , requestGroups
                , new Action<PluginTreeViewItem>[0]
            ))
            {
                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(nodeMessage);
                });
            }
        }

        private IEnumerable<PluginTreeViewItem> GroupStepsByMessage(
            IOrganizationServiceExtented service
            , PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<PluginTreeViewItem>> actionsOnChilds
        )
        {
            var groupsList = steps.GroupBy(ent => ent.SdkMessageId?.Name ?? "Unknown").OrderBy(mess => mess.Key, MessageComparer.Comparer);

            foreach (var grMessage in groupsList)
            {
                var nodeMessage = new PluginTreeViewItem(ComponentType.SdkMessage)
                {
                    Name = grMessage.Key,
                    Image = _imageMessage,

                    MessageName = grMessage.Key,
                };

                nodeMessage.MessageList.AddRange(steps.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Id).Distinct());

                nodeMessage.MessageFilterList.AddRange(steps.Where(s => s.SdkMessageFilterId != null).Select(s => s.SdkMessageFilterId.Id).Distinct());

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(nodeMessage);
                }

                var newActionOnChilds = actionsOnChilds.Union(new Action<PluginTreeViewItem>[] { n => n.MessageName = grMessage.Key });

                RecursiveGroup(service, nodeMessage, search, grMessage, requestGroups, newActionOnChilds);

                yield return nodeMessage;
            }
        }

        #endregion Message

        #region Message Category

        private async Task FillTreeByMessageCategory(
            IOrganizationServiceExtented service
            , List<PluginStage> stages
            , string pluginTypeNameFilter
            , string messageNameFilter
            , string entityNameFilter
            , SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<RequestGroupBuilder> requestGroups
        )
        {
            PluginSearchRepository repository = new PluginSearchRepository(service);

            PluginSearchResult search = await repository.FindAllAsync(stages, pluginTypeNameFilter, messageNameFilter, entityNameFilter, statuscode);

            foreach (var nodeEntity in GroupStepsByMessageCategory(
                service
                , search
                , search.SdkMessageProcessingStep
                , requestGroups
                , new Action<PluginTreeViewItem>[0]
            ))
            {
                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(nodeEntity);
                });
            }
        }

        private IEnumerable<PluginTreeViewItem> GroupStepsByMessageCategory(
            IOrganizationServiceExtented service
            , PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<PluginTreeViewItem>> actionsOnChilds
        )
        {
            var groupsList = steps.GroupBy(ent => ent.MessageCategoryName ?? "Unknown").OrderBy(mess => mess.Key, MessageComparer.Comparer);

            foreach (var grMessageCategory in groupsList)
            {
                var nodeMessageCategory = new PluginTreeViewItem(ComponentType.SdkMessage)
                {
                    Name = grMessageCategory.Key,
                    Image = _imageMessageCategory,

                    MessageCategoryName = grMessageCategory.Key,
                };

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(nodeMessageCategory);
                }

                var newActionOnChilds = actionsOnChilds.Union(new Action<PluginTreeViewItem>[] { n => n.MessageCategoryName = grMessageCategory.Key });

                RecursiveGroup(service, nodeMessageCategory, search, grMessageCategory, requestGroups, newActionOnChilds);

                yield return nodeMessageCategory;
            }
        }

        #endregion Message Category

        #region PluginAssembly

        private async Task FillTreeByPluginAssembly(
            IOrganizationServiceExtented service
            , List<PluginStage> stages
            , string pluginTypeNameFilter
            , string messageNameFilter
            , string entityNameFilter
            , SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<RequestGroupBuilder> requestGroups
        )
        {
            var repositoryPluginType = new PluginTypeRepository(service);
            var pluginTypeList = await repositoryPluginType.GetPluginTypesAsync(pluginTypeNameFilter, null);

            var repository = new PluginSearchRepository(service);
            PluginSearchResult search = await repository.FindAllAsync(stages, pluginTypeNameFilter, messageNameFilter, entityNameFilter, statuscode);

            var groupAssemblyList = pluginTypeList.GroupBy(p => new { p.PluginAssemblyId.Id, Name = p.AssemblyName }).OrderBy(a => a.Key.Name);

            var stepsByPluginAssemblyDict = search.SdkMessageProcessingStep.GroupBy(s => s.PluginAssemblyId).ToDictionary(s => s.Key);

            foreach (var grPluginAssembly in groupAssemblyList)
            {
                var nodePluginAssembly = new PluginTreeViewItem(ComponentType.PluginAssembly)
                {
                    Name = grPluginAssembly.Key.Name,
                    Image = _imagePluginAssembly,

                    PluginAssemblyId = grPluginAssembly.Key.Id,
                    PluginAssemblyName = grPluginAssembly.Key.Name,
                };

                if (requestGroups.Any() && requestGroups.First().GroupingProperty == GroupingProperty.PluginType)
                {
                    var stepsByPluginTypeDict = search.SdkMessageProcessingStep.GroupBy(s => s.EventHandler.Id).ToDictionary(s => s.Key);

                    foreach (var pluginType in grPluginAssembly
                        .OrderBy(p => p.IsWorkflowActivity.GetValueOrDefault())
                        .ThenBy(p => p.TypeName)
                    )
                    {
                        var nodePluginType = new PluginTreeViewItem(ComponentType.PluginType)
                        {
                            Name = pluginType.TypeName,
                            Image = pluginType.IsWorkflowActivity.GetValueOrDefault() ? _imageWorkflowActivity : _imagePluginType,

                            PluginAssemblyId = pluginType.PluginAssemblyId?.Id,
                            PluginAssemblyName = pluginType.AssemblyName,

                            PluginTypeId = pluginType.PluginTypeId,
                            PluginTypeName = pluginType.TypeName,

                            IsWorkflowActivity = pluginType.IsWorkflowActivity.GetValueOrDefault(),
                        };

                        nodePluginAssembly.Items.Add(nodePluginType);
                        nodePluginType.Parent = nodePluginAssembly;

                        if (stepsByPluginTypeDict.ContainsKey(pluginType.Id))
                        {
                            var steps = stepsByPluginTypeDict[pluginType.Id];

                            var newActionOnChilds = new Action<PluginTreeViewItem>[]
                            {
                                n =>
                                {
                                    n.PluginAssemblyId = pluginType.PluginAssemblyId?.Id;
                                    n.PluginAssemblyName = pluginType.AssemblyName;

                                    n.PluginTypeId = pluginType.PluginTypeId;
                                    n.PluginTypeName = pluginType.TypeName;
                                }
                            };

                            RecursiveGroup(service, nodePluginType, search, steps, requestGroups.Skip(1), newActionOnChilds);
                        }
                    }
                }
                else
                {
                    var newActionOnChilds = new Action<PluginTreeViewItem>[]
                    {
                        n =>
                        {
                            n.PluginAssemblyId = grPluginAssembly.Key.Id;
                            n.PluginAssemblyName = grPluginAssembly.Key.Name;
                        }
                    };

                    if (stepsByPluginAssemblyDict.ContainsKey(grPluginAssembly.Key.Id))
                    {
                        var steps = stepsByPluginAssemblyDict[grPluginAssembly.Key.Id];

                        RecursiveGroup(service, nodePluginAssembly, search, steps, requestGroups, newActionOnChilds);
                    }
                }

                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(nodePluginAssembly);
                });
            }
        }

        private IEnumerable<PluginTreeViewItem> GroupStepsByPluginAssembly(
            IOrganizationServiceExtented service
            , PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<PluginTreeViewItem>> actionsOnChilds
        )
        {
            var groupsList = steps
                .GroupBy(ent => new
                {
                    ent.PluginAssemblyName,
                    ent.PluginAssemblyId
                })
                .OrderBy(s => s.Key.PluginAssemblyName)
                ;

            foreach (var grPluginAssembly in groupsList)
            {
                var nodePluginAssembly = new PluginTreeViewItem(ComponentType.PluginAssembly)
                {
                    Name = grPluginAssembly.Key.PluginAssemblyName,
                    Image = _imagePluginAssembly,

                    PluginAssemblyId = grPluginAssembly.Key.PluginAssemblyId,
                    PluginAssemblyName = grPluginAssembly.Key.PluginAssemblyName,
                };

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(nodePluginAssembly);
                }

                var newActionOnChilds = actionsOnChilds.Union(new Action<PluginTreeViewItem>[]
                {
                    n =>
                    {
                        n.PluginAssemblyId = grPluginAssembly.Key.PluginAssemblyId;
                        n.PluginAssemblyName = grPluginAssembly.Key.PluginAssemblyName;
                    }
                });

                RecursiveGroup(service, nodePluginAssembly, search, grPluginAssembly, requestGroups, newActionOnChilds);

                yield return nodePluginAssembly;
            }
        }

        #endregion PluginAssembly

        #region PluginType

        private async Task FillTreeByPluginType(
            IOrganizationServiceExtented service
            , List<PluginStage> stages
            , string pluginTypeNameFilter
            , string messageNameFilter
            , string entityNameFilter
            , SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<RequestGroupBuilder> requestGroups
        )
        {
            var repositoryPluginType = new PluginTypeRepository(service);
            var pluginTypeList = await repositoryPluginType.GetPluginTypesAsync(pluginTypeNameFilter, null);

            var repository = new PluginSearchRepository(service);
            PluginSearchResult search = await repository.FindAllAsync(stages, pluginTypeNameFilter, messageNameFilter, entityNameFilter, statuscode);

            var stepsByPluginTypeDict = search.SdkMessageProcessingStep.GroupBy(s => s.EventHandler.Id).ToDictionary(s => s.Key);

            foreach (var pluginType in pluginTypeList
                .OrderBy(p => p.IsWorkflowActivity.GetValueOrDefault())
                .ThenBy(p => p.AssemblyName)
                .ThenBy(p => p.TypeName)
            )
            {
                var nodePluginType = new PluginTreeViewItem(ComponentType.PluginType)
                {
                    Name = pluginType.TypeName,
                    Image = pluginType.IsWorkflowActivity.GetValueOrDefault() ? _imageWorkflowActivity : _imagePluginType,

                    PluginAssemblyId = pluginType.PluginAssemblyId?.Id,
                    PluginAssemblyName = pluginType.AssemblyName,

                    PluginTypeId = pluginType.PluginTypeId,
                    PluginTypeName = pluginType.TypeName,

                    IsWorkflowActivity = pluginType.IsWorkflowActivity.GetValueOrDefault(),
                };

                if (stepsByPluginTypeDict.ContainsKey(pluginType.Id))
                {
                    var steps = stepsByPluginTypeDict[pluginType.Id];

                    var newActionOnChilds = new Action<PluginTreeViewItem>[]
                    {
                        n =>
                        {
                            n.PluginTypeId = pluginType.PluginTypeId;
                            n.PluginTypeName = pluginType.TypeName;

                            n.PluginAssemblyId = pluginType.PluginAssemblyId?.Id;
                            n.PluginAssemblyName = pluginType.AssemblyName;
                        }
                    };

                    RecursiveGroup(service, nodePluginType, search, steps, requestGroups, newActionOnChilds);
                }

                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(nodePluginType);
                });
            }
        }

        private IEnumerable<PluginTreeViewItem> GroupStepsByPluginType(
            IOrganizationServiceExtented service
            , PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<PluginTreeViewItem>> actionsOnChilds
        )
        {
            var groupsList = steps
                .GroupBy(ent => new
                {
                    PluginTypeName = ent.EventHandler.Name,
                    PluginTypeId = ent.EventHandler.Id,
                    ent.PluginAssemblyName,
                    ent.PluginAssemblyId
                })
                .OrderBy(s => s.Key.PluginAssemblyName)
                .ThenBy(s => s.Key.PluginTypeName)
                ;

            foreach (var grPluginType in groupsList)
            {
                var nodePluginType = new PluginTreeViewItem(ComponentType.PluginType)
                {
                    Name = grPluginType.Key.PluginTypeName,
                    Image = _imagePluginType,

                    PluginAssemblyId = grPluginType.Key.PluginAssemblyId,
                    PluginAssemblyName = grPluginType.Key.PluginAssemblyName,

                    PluginTypeId = grPluginType.Key.PluginTypeId,
                    PluginTypeName = grPluginType.Key.PluginTypeName,

                    IsWorkflowActivity = false,
                };

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(nodePluginType);
                }

                var newActionOnChilds = actionsOnChilds.Union(new Action<PluginTreeViewItem>[]
                {
                    n =>
                    {
                        n.PluginAssemblyId = grPluginType.Key.PluginAssemblyId;
                        n.PluginAssemblyName = grPluginType.Key.PluginAssemblyName;

                        n.PluginTypeId = grPluginType.Key.PluginTypeId;
                        n.PluginTypeName = grPluginType.Key.PluginTypeName;
                    }
                });

                RecursiveGroup(service, nodePluginType, search, grPluginType, requestGroups, actionsOnChilds);

                yield return nodePluginType;
            }
        }

        #endregion PluginType

        #region Stage

        private async Task FillTreeByStage(
            IOrganizationServiceExtented service
            , List<PluginStage> stages
            , string pluginTypeNameFilter
            , string messageNameFilter
            , string entityNameFilter
            , SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<RequestGroupBuilder> requestGroups
        )
        {
            PluginSearchRepository repository = new PluginSearchRepository(service);

            PluginSearchResult search = await repository.FindAllAsync(stages, pluginTypeNameFilter, messageNameFilter, entityNameFilter, statuscode);

            foreach (var grStage in GroupStepsByStage(
                service
                , search
                , search.SdkMessageProcessingStep
                , requestGroups
                , new Action<PluginTreeViewItem>[0]
            ))
            {
                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(grStage);
                });
            }
        }

        private IEnumerable<PluginTreeViewItem> GroupStepsByStage(
            IOrganizationServiceExtented service
            , PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<PluginTreeViewItem>> actionsOnChilds
        )
        {
            var groupsList = steps.GroupBy(ent => new { Stage = ent.Stage.Value, Mode = ent.Mode.Value }).OrderBy(s => s.Key.Stage).ThenBy(s => s.Key.Mode);

            foreach (var grStage in groupsList)
            {
                string name = SdkMessageProcessingStepRepository.GetStageName(grStage.Key.Stage, grStage.Key.Mode);

                var nodeStage = new PluginTreeViewItem(null)
                {
                    Name = name,
                    Image = _imageStage,
                };

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(nodeStage);
                }

                RecursiveGroup(service, nodeStage, search, grStage, requestGroups, actionsOnChilds);

                yield return nodeStage;
            }
        }

        #endregion Stage

        #region Mode

        private async Task FillTreeByMode(
            IOrganizationServiceExtented service
            , List<PluginStage> stages
            , string pluginTypeNameFilter
            , string messageNameFilter
            , string entityNameFilter
            , SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode
            , IEnumerable<RequestGroupBuilder> requestGroups
        )
        {
            PluginSearchRepository repository = new PluginSearchRepository(service);

            PluginSearchResult search = await repository.FindAllAsync(stages, pluginTypeNameFilter, messageNameFilter, entityNameFilter, statuscode);

            foreach (var grMode in GroupStepsByMode(
                service
                , search
                , search.SdkMessageProcessingStep
                , requestGroups
                , new Action<PluginTreeViewItem>[0]
            ))
            {
                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(grMode);
                });
            }
        }

        private IEnumerable<PluginTreeViewItem> GroupStepsByMode(
            IOrganizationServiceExtented service
            , PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<PluginTreeViewItem>> actionsOnChilds
        )
        {
            var groupsList = steps.GroupBy(ent => ent.ModeEnum.Value).OrderBy(s => s.Key);

            foreach (var grMode in groupsList)
            {
                string name = Helpers.EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(grMode.Key);

                var nodeMode = new PluginTreeViewItem(null)
                {
                    Name = name,
                    Image = _imageMode,
                };

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(nodeMode);
                }

                RecursiveGroup(service, nodeMode, search, grMode, requestGroups, actionsOnChilds);

                yield return nodeMode;
            }
        }

        #endregion Mode

        private void RecursiveGroup(
            IOrganizationServiceExtented service
            , PluginTreeViewItem nodeParent
            , PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<RequestGroupBuilder> requestGroups
            , IEnumerable<Action<PluginTreeViewItem>> newActionOnChilds
        )
        {
            if (requestGroups.Any())
            {
                var groupBuilder = requestGroups.First();

                foreach (var node in groupBuilder.GroupSteps(
                    service
                    , search
                    , steps
                    , requestGroups.Skip(1)
                    , newActionOnChilds
                ))
                {
                    nodeParent.Items.Add(node);
                    node.Parent = nodeParent;
                }
            }
            else
            {
                foreach (var node in GetAllSteps(search, steps, newActionOnChilds))
                {
                    nodeParent.Items.Add(node);
                    node.Parent = nodeParent;
                }
            }
        }

        private IEnumerable<PluginTreeViewItem> GetAllSteps(
            PluginSearchResult search
            , IEnumerable<SdkMessageProcessingStep> steps
            , IEnumerable<Action<PluginTreeViewItem>> actionsOnChilds
        )
        {
            foreach (var step in steps
                .OrderBy(s => s.Stage.Value)
                .ThenBy(s => s.Mode.Value)
                .ThenBy(s => s.Rank)
                .ThenBy(s => s.Name)
            )
            {
                PluginTreeViewItem nodeStep = CreateNodeStep(step, search.SdkMessageProcessingStepImage);

                foreach (var action in actionsOnChilds)
                {
                    action?.Invoke(nodeStep);
                }

                yield return nodeStep;
            }
        }

        private async Task ShowExistingPlugins()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingPlugins);

            string entityName = string.Empty;
            string messageName = string.Empty;
            string pluginTypeName = string.Empty;

            bool withBusinessRules = false;
            bool withBusinessProcesses = false;
            SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode = null;

            try
            {

                this.Dispatcher.Invoke(() =>
                {
                    this.trVPluginTree.BeginInit();

                    _pluginTree.Clear();

                    entityName = cmBEntityName.Text?.Trim();
                    messageName = txtBMessageFilter.Text.Trim();
                    pluginTypeName = txtBPluginTypeFilter.Text.Trim();
                    withBusinessRules = chBBusinessRules.IsChecked.GetValueOrDefault();
                    withBusinessProcesses = chBBusinessProcesses.IsChecked.GetValueOrDefault();

                    if (cmBStatusCode.SelectedItem is SdkMessageProcessingStep.Schema.OptionSets.statuscode comboBoxItem)
                    {
                        statuscode = comboBoxItem;
                    }
                });

                var stages = GetStages();

                if (service != null)
                {
                    List<RequestGroupBuilder> requestGroups = _currentGrouping.Select(g => _propertyGroups[g]).ToList();

                    var first = requestGroups.First();

                    await first.FillTree(service, stages, pluginTypeName, messageName, entityName, statuscode, requestGroups.Skip(1));

                    ExpandNodes(_pluginTree);

                    base.StartGettingSdkMessageFilters(service);
                }

                this.trVPluginTree.Dispatcher.Invoke(() =>
                {
                    this.trVPluginTree.EndInit();
                });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingPluginsCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }


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

        private class ProcessingStep
        {
            public string EntityName { get; private set; }

            public string Message { get; private set; }

            public int? Stage { get; private set; }

            public bool AsyncMode { get; private set; }

            public int? Rank { get; private set; }

            public string Name { get; private set; }

            public Entity Entity { get; private set; }

            public ProcessingStep(string entityName, string message, int? stage, bool asyncMode, int? rank, string name, Entity entity)
            {
                this.EntityName = entityName;
                this.Message = message;
                this.Stage = stage;
                this.AsyncMode = asyncMode;
                this.Rank = rank;
                this.Name = name;
                this.Entity = entity;
            }
        }

        private ColumnSet _columnSetWorkflow = new ColumnSet
        (
            Workflow.Schema.Attributes.workflowid
            , Workflow.Schema.Attributes.name
            , Workflow.Schema.Attributes.category

            , Workflow.Schema.Attributes.primaryentity
            , Workflow.Schema.Attributes.statecode
            , Workflow.Schema.Attributes.statuscode

            , Workflow.Schema.Attributes.type
            , Workflow.Schema.Attributes.businessprocesstype

            , Workflow.Schema.Attributes.rank
            , Workflow.Schema.Attributes.mode

            , Workflow.Schema.Attributes.createstage
            , Workflow.Schema.Attributes.updatestage
            , Workflow.Schema.Attributes.deletestage

            , Workflow.Schema.Attributes.triggeroncreate
            , Workflow.Schema.Attributes.triggeronupdateattributelist
            , Workflow.Schema.Attributes.triggerondelete
        );

        private PluginTreeViewItem CreateNodeStep(Entity entity, IEnumerable<SdkMessageProcessingStepImage> images)
        {
            PluginTreeViewItem nodeStep = null;

            if (entity is SdkMessageProcessingStep step)
            {
                nodeStep = new PluginTreeViewItem(ComponentType.SdkMessageProcessingStep);

                FillNodeStepInformation(nodeStep, step);

                var queryImage = from image in images
                                 where image.SdkMessageProcessingStepId != null
                                 where image.SdkMessageProcessingStepId.Id == step.Id
                                 orderby image.ImageType.Value
                                 select image;

                foreach (var image in queryImage)
                {
                    PluginTreeViewItem nodeImage = new PluginTreeViewItem(ComponentType.SdkMessageProcessingStepImage);

                    FillNodeImageInformation(nodeImage, image, step.PrimaryObjectTypeCodeName, step.SdkMessageId?.Name, step.EventHandler?.Id, step.EventHandler?.Name, step.PluginAssemblyId, step.PluginAssemblyName);

                    nodeStep.Items.Add(nodeImage);
                    nodeImage.Parent = nodeStep;
                }
            }
            else if (entity is Workflow workflow)
            {
                string nameStep = GetStepName(workflow.Rank, workflow.Name, workflow.TriggerOnUpdateAttributeList);

                nodeStep = new PluginTreeViewItem(ComponentType.Workflow)
                {
                    Name = nameStep,

                    WorkflowId = workflow.Id,

                    IsActive = workflow.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1,

                    ImageActive = _imageBusinessProcess,
                    ImageInactive = _imageStepDisabled,
                };

                nodeStep.Image = workflow.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1 ? _imageBusinessProcess : _imageStepDisabled;
            }

            return nodeStep;
        }

        private void FillNodeStepInformation(PluginTreeViewItem nodeStep, SdkMessageProcessingStep step)
        {
            string nameStep = GetStepName(step.Rank, step.EventHandler?.Name ?? "Unknown", step.FilteringAttributesStringsSorted);
            string tooltipStep = GetStepTooltip(step);

            nodeStep.Name = nameStep;
            nodeStep.Tooltip = tooltipStep;
            nodeStep.EntityLogicalName = step.PrimaryObjectTypeCodeName;
            nodeStep.PluginAssemblyId = step.PluginAssemblyId;
            nodeStep.PluginAssemblyName = step.PluginAssemblyName;
            nodeStep.PluginTypeId = step.EventHandler?.Id;
            nodeStep.MessageName = step.SdkMessageId?.Name;
            nodeStep.StepId = step.Id;
            nodeStep.IsActive = step.StateCodeEnum == SdkMessageProcessingStep.Schema.OptionSets.statecode.Enabled_0;
            nodeStep.ImageActive = _imageStep;
            nodeStep.ImageInactive = _imageStepDisabled;

            nodeStep.Image = step.StateCodeEnum == SdkMessageProcessingStep.Schema.OptionSets.statecode.Enabled_0 ? _imageStep : _imageStepDisabled;
        }

        private void FillNodeImageInformation(
            PluginTreeViewItem nodeImage
            , SdkMessageProcessingStepImage image
            , string entityName
            , string messageName
            , Guid? idPluginType
            , string pluginTypeName
            , Guid? idPluginAssembly
            , string pluginAssemblyName
        )
        {
            string nameImage = GetImageName(image);
            string tooltipImage = GetImageTooltip(image);

            nodeImage.Name = nameImage;
            nodeImage.Tooltip = tooltipImage;
            nodeImage.Image = _imageImage;
            nodeImage.EntityLogicalName = entityName;
            nodeImage.MessageName = messageName;
            nodeImage.PluginAssemblyId = idPluginAssembly;
            nodeImage.PluginAssemblyName = pluginAssemblyName;
            nodeImage.PluginTypeId = idPluginType;
            nodeImage.PluginTypeName = pluginTypeName;
            nodeImage.StepId = image.SdkMessageProcessingStepImageId;
            nodeImage.StepImageId = image.Id;
        }

        private PluginTreeViewItem CreateNodeBusinessRules(string entityName)
        {
            var result = new PluginTreeViewItem(null)
            {
                Name = string.Format("BusinessRules {0}", entityName),
                Image = _imageBusinessRule,

                EntityLogicalName = entityName,
            };

            return result;
        }

        private PluginTreeViewItem CreateNodeBusinessRule(Entities.Workflow businessRule)
        {
            var nodeStep = new PluginTreeViewItem(ComponentType.Workflow)
            {
                Name = businessRule.Name,

                WorkflowId = businessRule.Id,

                IsActive = businessRule.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1,

                ImageActive = _imageStep,
                ImageInactive = _imageStepDisabled,
            };

            nodeStep.Image = businessRule.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1 ? _imageStep : _imageStepDisabled;

            return nodeStep;
        }

        private string GetStepTooltip(SdkMessageProcessingStep step)
        {
            StringBuilder tooltipStep = new StringBuilder();

            if (!string.IsNullOrEmpty(step.Name))
            {
                if (tooltipStep.Length > 0)
                {
                    tooltipStep.AppendLine();
                }

                tooltipStep.AppendFormat("Name: {0}", step.Name);
            }

            if (!string.IsNullOrEmpty(step.Description))
            {
                if (tooltipStep.Length > 0)
                {
                    tooltipStep.AppendLine();
                }

                tooltipStep.AppendFormat("Description: {0}", step.Description);
            }

            if (!string.IsNullOrEmpty(step.FilteringAttributes))
            {
                if (tooltipStep.Length > 0)
                {
                    tooltipStep.AppendLine();
                }

                tooltipStep.AppendLine("Filtering:");

                foreach (string item in step.FilteringAttributesStrings)
                {
                    tooltipStep.AppendLine().Append(item);
                }
            }

            return tooltipStep.ToString();
        }

        private string GetStepName(int? rank, string name, string filteringAttributesStringsSorted)
        {
            StringBuilder nameStep = new StringBuilder();

            if (rank.HasValue)
            {
                nameStep.AppendFormat("{0}. ", rank.ToString());
            }

            nameStep.Append(name);

            if (!string.IsNullOrEmpty(filteringAttributesStringsSorted))
            {
                nameStep.AppendFormat("      Filtering: {0}", filteringAttributesStringsSorted);
            }

            return nameStep.ToString();
        }

        private string GetImageTooltip(SdkMessageProcessingStepImage image)
        {
            StringBuilder tooltipImage = new StringBuilder();

            if (!string.IsNullOrEmpty(image.Name))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendFormat("Name: {0}", image.Name);
            }

            if (!string.IsNullOrEmpty(image.Description))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendFormat("Description: {0}", image.Description);
            }

            if (!string.IsNullOrEmpty(image.MessagePropertyName))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendFormat("MessagePropertyName: {0}", image.MessagePropertyName);
            }

            if (!string.IsNullOrEmpty(image.RelatedAttributeName))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendFormat("RelatedAttributeName: {0}", image.RelatedAttributeName);
            }

            if (!string.IsNullOrEmpty(image.Attributes1))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendLine("Attributes:");

                foreach (string item in image.Attributes1Strings)
                {
                    tooltipImage.AppendLine().Append(item);
                }
            }

            if (tooltipImage.Length > 0)
            {
                return tooltipImage.ToString();
            }

            return null;
        }

        private string GetImageName(SdkMessageProcessingStepImage image)
        {
            StringBuilder nameImage = new StringBuilder();

            if (image.ImageType != null)
            {
                if (image.ImageType.Value == (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.PreImage_0)
                {
                    nameImage.Append("PreImage");
                }
                else if (image.ImageType.Value == (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.PostImage_1)
                {
                    nameImage.Append("PostImage");
                }
                else if (image.ImageType.Value == (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.Both_2)
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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, tSBCollapseAll, tSBExpandAll, tSBRegisterAssembly, menuView, menuCustomView);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.trVPluginTree.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                                        && this.trVPluginTree.SelectedItem != null
                                        && this.trVPluginTree.SelectedItem is PluginTreeViewItem
                                        && CanCreateDescription(this.trVPluginTree.SelectedItem as PluginTreeViewItem);

                    UIElement[] list = { tSBCreateDescription };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }

                    tSBCreateDescription.Content = GetCreateDescriptionName(this.trVPluginTree.SelectedItem as PluginTreeViewItem);
                }
                catch (Exception)
                {
                }
            });
        }

        private string GetCreateDescriptionName(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "Create Description";
            }

            if (item.PluginAssemblyId.HasValue && item.ComponentType == ComponentType.PluginAssembly)
            {
                return "Create Plugin Assembly Description";
            }

            if (item.PluginTypeId.HasValue && item.ComponentType == ComponentType.PluginType)
            {
                return "Create Plugin Type Description";
            }

            if (item.StepId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return "Create Step Description";
            }

            if (item.StepImageId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                return "Create Image Description";
            }

            if (item.WorkflowId.HasValue && item.ComponentType == ComponentType.Workflow)
            {
                return "Create Workflow Description";
            }

            if (item.MessageList != null && item.MessageList.Any())
            {
                return "Create Message Description";
            }

            if (item.MessageFilterList != null && item.MessageFilterList.Any())
            {
                return "Create Message Filter Description";
            }

            return "Create Description";
        }

        private string GetUpdateName(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "Update";
            }

            if (item.PluginAssemblyId.HasValue && item.ComponentType == ComponentType.PluginAssembly)
            {
                return "Update Plugin Assembly";
            }

            if (item.StepId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return "Update Step";
            }

            if (item.StepImageId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                return "Update Image";
            }

            return "Update";
        }

        private string GetEditName(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "Edit in Editor";
            }

            if (item.PluginAssemblyId.HasValue && item.ComponentType == ComponentType.PluginAssembly)
            {
                return "Edit Plugin Assembly in Editor";
            }

            if (item.PluginTypeId.HasValue && item.ComponentType == ComponentType.PluginType)
            {
                return "Edit Plugin Type in Editor";
            }

            if (item.StepId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return "Edit Step in Editor";
            }

            if (item.StepImageId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                return "Edit Image in Editor";
            }

            if (item.WorkflowId.HasValue && item.ComponentType == ComponentType.Workflow)
            {
                return "Edit Workflow in Editor";
            }

            return "Edit in Editor";
        }

        private string GetChangeStateName(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "ChangeState";
            }

            var action = item.IsActive ? "Deactivate" : "Activate";

            if (item.StepId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return $"{action} Step";
            }

            if (item.WorkflowId.HasValue && item.ComponentType == ComponentType.Workflow)
            {
                return $"{action} Workflow";
            }

            return "ChangeState";
        }

        private string GetDeleteName(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "Delete";
            }

            if (item.PluginAssemblyId.HasValue && item.ComponentType == ComponentType.PluginAssembly)
            {
                return "Delete Plugin Assembly";
            }

            if (item.PluginTypeId.HasValue && item.ComponentType == ComponentType.PluginType)
            {
                return "Delete Plugin Type";
            }

            if (item.StepId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return "Delete Step";
            }

            if (item.StepImageId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                return "Delete Image";
            }

            return "Delete";
        }

        private bool CanCreateDescription(PluginTreeViewItem item)
        {
            return ((item.MessageList != null && item.MessageList.Any()) && item.ComponentType == ComponentType.SdkMessage)
                || ((item.MessageFilterList != null && item.MessageFilterList.Any()) && item.ComponentType == ComponentType.SdkMessageFilter)

                || (item.PluginAssemblyId.HasValue && item.ComponentType == ComponentType.PluginAssembly)
                || (item.PluginTypeId.HasValue && item.ComponentType == ComponentType.PluginType)

                || (item.StepId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
                || (item.StepImageId.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)

                || (item.WorkflowId.HasValue && item.ComponentType == ComponentType.Workflow)
                ;
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingPlugins();
            }
        }

        private void cmBStatusCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingPlugins();
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

        private void tSBCollapseAll_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(_pluginTree, false);
        }

        private void tSBExpandAll_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(_pluginTree, true);
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

            this.trVPluginTree.Dispatcher.Invoke(() =>
            {
                this.trVPluginTree.BeginInit();
            });

            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;

                ChangeExpandedInTreeViewItemsRecursive(item.Items, isExpanded);
            }

            this.trVPluginTree.Dispatcher.Invoke(() =>
            {
                this.trVPluginTree.EndInit();
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

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingDescription);

            StringBuilder result = new StringBuilder();

            string fileName = string.Empty;

            bool appendConnectionInfo = true;

            if (node.PluginAssemblyId.HasValue && node.ComponentType == ComponentType.PluginAssembly)
            {
                var repository = new PluginAssemblyRepository(service);
                var pluginAssembly = await repository.GetAssemblyByIdRetrieveRequestAsync(node.PluginAssemblyId.Value);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetPluginAssemblyFileName(service.ConnectionData.Name, pluginAssembly.Name, "Description", "txt");
                }

                {
                    PluginAssemblyDescriptionHandler handler = new PluginAssemblyDescriptionHandler(service, service.ConnectionData.GetConnectionInfo());

                    string desc = await handler.CreateDescriptionAsync(pluginAssembly.Id, pluginAssembly.Name, DateTime.Now);

                    if (!string.IsNullOrEmpty(desc))
                    {
                        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                        result.AppendLine(desc);

                        appendConnectionInfo = false;
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

            if (node.PluginTypeId.HasValue && node.ComponentType == ComponentType.PluginType)
            {
                var repository = new PluginTypeRepository(service);
                var pluginType = await repository.GetPluginTypeByIdAsync(node.PluginTypeId.Value);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, pluginType.TypeName, "Description");
                }

                if (appendConnectionInfo)
                {
                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());

                    appendConnectionInfo = false;
                }

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

            if (node.StepId.HasValue && node.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                var repository = new SdkMessageProcessingStepRepository(service);
                var step = await repository.GetStepByIdAsync(node.StepId.Value);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetPluginStepFileName(service.ConnectionData.Name, step.Name, "Description");
                }

                if (appendConnectionInfo)
                {
                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());

                    appendConnectionInfo = false;
                }

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

            if (node.StepImageId.HasValue && node.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                var repository = new SdkMessageProcessingStepImageRepository(service);
                SdkMessageProcessingStepImage stepImage = await repository.GetStepImageByIdAsync(node.StepImageId.Value);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetPluginImageFileName(service.ConnectionData.Name, stepImage.Name, "Description");
                }

                if (appendConnectionInfo)
                {
                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());

                    appendConnectionInfo = false;
                }

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

            if (node.MessageList != null && node.MessageList.Any())
            {
                var repository = new SdkMessageRepository(service);
                List<SdkMessage> listMessages = await repository.GetMessageByIdsAsync(node.MessageList.ToArray());

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetMessageFileName(service.ConnectionData.Name, node.Name, "Description");
                }

                if (appendConnectionInfo)
                {
                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());

                    appendConnectionInfo = false;
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

            if (node.MessageFilterList != null && node.MessageFilterList.Any())
            {
                var repository = new SdkMessageFilterRepository(service);
                List<SdkMessageFilter> listMessages = await repository.GetMessageFiltersByIdsAsync(node.MessageFilterList.ToArray());

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetMessageFilterFileName(service.ConnectionData.Name, node.Name, "Description");
                }

                if (appendConnectionInfo)
                {
                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());

                    appendConnectionInfo = false;
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

            if (node.WorkflowId.HasValue && node.ComponentType == ComponentType.Workflow)
            {
                var repository = new WorkflowRepository(service);
                var workflow = await repository.GetByIdAsync(node.WorkflowId.Value);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetWorkflowFileName(
                        service.ConnectionData.Name
                        , workflow.Name
                        , workflow.FormattedValues[Workflow.Schema.Attributes.category]
                        , workflow.Name
                        , "Description"
                        , "txt"
                        );
                }

                if (appendConnectionInfo)
                {
                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
                    result.AppendLine(service.ConnectionData.GetConnectionInfo());

                    appendConnectionInfo = false;
                }

                {
                    string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(workflow, new List<string>() { Workflow.Schema.Attributes.xaml }, service.ConnectionData);

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

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectionData connectionData = null;

            this.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                FillEntityNames(connectionData);

                ShowExistingPlugins();
            }
        }

        private void FillEntityNames(ConnectionData connectionData)
        {
            cmBEntityName.Dispatcher.Invoke(() =>
            {
                LoadEntityNames(cmBEntityName, connectionData);
            });
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

            bool isPluginAssembly = nodeItem.PluginAssemblyId.HasValue && nodeItem.ComponentType == ComponentType.PluginAssembly;
            bool isPluginType = nodeItem.PluginTypeId.HasValue && nodeItem.ComponentType == ComponentType.PluginType;
            bool isStep = nodeItem.StepId.HasValue && nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep;
            bool isStepImage = nodeItem.StepId.HasValue && nodeItem.ComponentType == ComponentType.SdkMessageProcessingStepImage;

            bool isWorkflow = nodeItem.WorkflowId.HasValue && nodeItem.ComponentType == ComponentType.Workflow;

            bool showDependentComponents = nodeItem.GetId().HasValue && nodeItem.ComponentType.HasValue;

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            ActivateControls(items, CanCreateDescription(nodeItem), "contMnCreateDescription");
            SetControlsName(items, GetCreateDescriptionName(nodeItem), "contMnCreateDescription");

            ActivateControls(items, isPluginAssembly || isStep || isStepImage, "contMnUpdate");
            SetControlsName(items, GetUpdateName(nodeItem), "contMnUpdate");

            ActivateControls(items, isWorkflow || isStep, "contMnChangeState");
            SetControlsName(items, GetChangeStateName(nodeItem), "contMnChangeState");

            ActivateControls(items, isPluginAssembly || isPluginType || isStep || isStepImage || isWorkflow, "contMnDelete");
            SetControlsName(items, GetDeleteName(nodeItem), "contMnDelete");

            ActivateControls(items, isPluginType, "contMnAddPluginStep");
            ActivateControls(items, isStep && isEntity, "contMnAddPluginStepImage");

            ActivateControls(items, isWorkflow, "contMnOpenInWeb", "contMnWorkflowExlorer");

            ActivateControls(items, showDependentComponents && !isMessage && !isMessageFilter, "contMnEditor");
            SetControlsName(items, GetEditName(nodeItem), "contMnEditor");

            ActivateControls(items, showDependentComponents, "contMnDependentComponents");

            ActivateControls(items, isMessage || isEntity, "contMnSdkMessage", "mIOpenMessageExplorer", "mIOpenMessageFilterTree", "mIOpenMessageRequestTree");

            ActivateControls(items, nodeItem.PluginAssemblyId.HasValue && nodeItem.ComponentType != ComponentType.PluginAssembly, "contMnAddPluginAssemblyToSolution", "contMnAddPluginAssemblyToSolutionLast");
            FillLastSolutionItems(connectionData, items, nodeItem.PluginAssemblyId.HasValue && nodeItem.ComponentType != ComponentType.PluginAssembly, AddAssemblyToCrmSolutionLast_Click, "contMnAddPluginAssemblyToSolutionLast");

            ActivateControls(items, isMessage || isMessageFilter || isPluginAssembly || isStep || isWorkflow, "contMnAddToSolution", "contMnAddToSolutionLast");
            FillLastSolutionItems(connectionData, items, isMessage || isMessageFilter || isPluginAssembly || isStep || isWorkflow, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            ActivateControls(items, nodeItem.PluginTypeId.HasValue, "contMnAddPluginTypeStepsToSolution", "contMnAddPluginTypeStepsToSolutionLast");
            FillLastSolutionItems(connectionData, items, nodeItem.PluginTypeId.HasValue, mIAddPluginTypeStepsToSolutionLast_Click, "contMnAddPluginTypeStepsToSolutionLast");

            ActivateControls(items, nodeItem.PluginAssemblyId.HasValue, "contMnAddPluginAssemblyStepsToSolution", "contMnAddPluginAssemblyStepsToSolutionLast", "contMnCompareWithLocalAssembly");
            FillLastSolutionItems(connectionData, items, nodeItem.PluginAssemblyId.HasValue, mIAddAssemblyStepsToSolutionLast_Click, "contMnAddPluginAssemblyStepsToSolutionLast");

            ActivateControls(items, isEntity, "contMnEntity");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastDoNotIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityToSolutionLastIncludeAsShellOnly");
            ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityToSolutionLast");

            CheckSeparatorVisible(items);
        }

        private void mICreateDescription_Click(object sender, RoutedEventArgs e)
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

            CreateDescription(nodeItem);
        }

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                || !nodeItem.WorkflowId.HasValue
            )
            {
                return;
            }

            var service = await GetService();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Workflow, nodeItem.WorkflowId.Value);
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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(nodeItem.EntityLogicalName);
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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

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

            await AddEntityMetadataToSolution(
                connectionData
                , new[] { idMetadata.Value }
                , withSelect
                , solutionUniqueName
                , rootComponentBehavior
            );
        }

        #endregion Entity Handlers

        private async void mIOpenWorkflowExplorer_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null
                || nodeItem.ComponentType != ComponentType.Workflow
                || !nodeItem.WorkflowId.HasValue
                )
            {
                return;
            }

            var service = await GetService();

            var repository = new WorkflowRepository(service);
            var workflow = await repository.GetByIdAsync(nodeItem.WorkflowId.Value, new ColumnSet(Workflow.Schema.Attributes.name, Workflow.Schema.Attributes.primaryentity));

            string entityName = string.Empty;

            if (workflow.PrimaryEntity.IsValidEntityName())
            {
                entityName = workflow.PrimaryEntity;
            }

            WindowHelper.OpenWorkflowExplorer(_iWriteToOutput, service, _commonConfig, entityName, workflow.Name);
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

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

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

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
                    WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)nodeItem.ComponentType.Value, id.Value, null);
                }
            }
        }

        private async void miOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

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

        private async void mIAddPluginTypeStepsToSolution_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null || !nodeItem.PluginTypeId.HasValue)
            {
                return;
            }

            await AddPluginTypeStepsToSolution(nodeItem.PluginTypeId.Value, true, null);
        }

        private async void mIAddPluginTypeStepsToSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null || !nodeItem.PluginTypeId.HasValue)
            {
                return;
            }

            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddPluginTypeStepsToSolution(nodeItem.PluginTypeId.Value, false, solutionUniqueName);
            }
        }

        private async Task AddPluginTypeStepsToSolution(Guid idPluginType, bool withSelect, string solutionUniqueName)
        {
            var service = await GetService();

            var repository = new SdkMessageProcessingStepRepository(service);

            var steps = await repository.GetAllStepsByPluginTypeAsync(idPluginType);

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

        private async void AddAssemblyToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null || !nodeItem.PluginAssemblyId.HasValue)
            {
                return;
            }

            await AddPluginTypeStepsToSolution(nodeItem.PluginAssemblyId.Value, true, null);
        }

        private async void AddAssemblyToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null || !nodeItem.PluginAssemblyId.HasValue)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyToSolution(nodeItem.PluginAssemblyId.Value, false, solutionUniqueName);
            }
        }

        private async Task AddAssemblyToSolution(Guid idPluginAssembly, bool withSelect, string solutionUniqueName)
        {
            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.PluginAssembly, new[] { idPluginAssembly }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void mIAddAssemblyStepsToSolution_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null || !nodeItem.PluginAssemblyId.HasValue)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyStepsToSolution(nodeItem.PluginAssemblyId.Value, false, solutionUniqueName);
            }
        }

        private async void mIAddAssemblyStepsToSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null || !nodeItem.PluginAssemblyId.HasValue)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyStepsToSolution(nodeItem.PluginAssemblyId.Value, false, solutionUniqueName);
            }
        }

        private async Task AddAssemblyStepsToSolution(Guid idPluginAssembly, bool withSelect, string solutionUniqueName)
        {
            var service = await GetService();

            var repository = new SdkMessageProcessingStepRepository(service);

            var steps = await repository.GetAllStepsByPluginAssemblyAsync(idPluginAssembly);

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

        private async void mIAddPluginStep_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodePluginType = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodePluginType == null
                || nodePluginType.ComponentType != ComponentType.PluginType
                || !nodePluginType.PluginTypeId.HasValue
            )
            {
                return;
            }

            await ExecuteAddingNewPluginStep(nodePluginType);
        }

        private async Task ExecuteAddingNewPluginStep(PluginTreeViewItem nodePluginType)
        {
            var service = await GetService();

            var step = new SdkMessageProcessingStep()
            {
                EventHandler = new EntityReference(PluginType.EntityLogicalName, nodePluginType.PluginTypeId.Value),
            };

            List<SdkMessageFilter> filters = await GetSdkMessageFiltersAsync(service);

            var form = new WindowSdkMessageProcessingStep(_iWriteToOutput, service, filters, step);

            if (form.ShowDialog().GetValueOrDefault())
            {
                var repositoryStep = new SdkMessageProcessingStepRepository(service);

                step = await repositoryStep.GetStepByIdAsync(form.Step.Id);

                this.trVPluginTree.Dispatcher.Invoke(() =>
                {
                    PluginTreeViewItem nodeStep = new PluginTreeViewItem(ComponentType.SdkMessageProcessingStep);

                    FillNodeStepInformation(nodeStep, step);

                    nodePluginType.Items.Add(nodeStep);
                    nodeStep.Parent = nodePluginType;

                    nodeStep.IsSelected = true;
                    nodeStep.IsExpanded = true;
                });
            }
        }

        private async void mIAddPluginStepImage_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeStep = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeStep == null
                || nodeStep.ComponentType != ComponentType.SdkMessageProcessingStep
                || !nodeStep.StepId.HasValue
                || !nodeStep.EntityLogicalName.IsValidEntityName()
            )
            {
                return;
            }

            await ExecuteAddingPluginStepImage(nodeStep);
        }

        private async Task ExecuteAddingPluginStepImage(PluginTreeViewItem nodeStep)
        {
            var service = await GetService();

            var image = new SdkMessageProcessingStepImage()
            {
                SdkMessageProcessingStepId = new EntityReference(SdkMessageProcessingStep.EntityLogicalName, nodeStep.StepId.Value),
            };

            var form = new WindowSdkMessageProcessingStepImage(_iWriteToOutput, service, image, nodeStep.EntityLogicalName, nodeStep.MessageName);

            if (form.ShowDialog().GetValueOrDefault())
            {
                var repositoryImage = new SdkMessageProcessingStepImageRepository(service);

                image = await repositoryImage.GetStepImageByIdAsync(form.Image.Id);

                this.trVPluginTree.Dispatcher.Invoke(() =>
                {
                    PluginTreeViewItem nodeImage = new PluginTreeViewItem(ComponentType.SdkMessageProcessingStepImage);

                    FillNodeImageInformation(nodeImage, image, nodeStep.EntityLogicalName, nodeStep.MessageName, nodeStep.PluginTypeId, nodeStep.PluginTypeName, nodeStep.PluginAssemblyId, nodeStep.PluginAssemblyName);

                    nodeStep.Items.Add(nodeImage);
                    nodeImage.Parent = nodeStep;

                    nodeStep.IsExpanded = true;

                    nodeImage.IsSelected = true;
                    nodeImage.IsExpanded = true;
                });
            }
        }

        private async void mIUpdateSdkObject_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            await ExecuteUpdateSdkObject(nodeItem);
        }

        private async Task ExecuteUpdateSdkObject(PluginTreeViewItem nodeItem)
        {
            var service = await GetService();

            if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStepImage && nodeItem.StepImageId.HasValue)
            {
                var repositoryImage = new SdkMessageProcessingStepImageRepository(service);

                var image = await repositoryImage.GetStepImageByIdAsync(nodeItem.StepImageId.Value);

                var form = new WindowSdkMessageProcessingStepImage(_iWriteToOutput, service, image, nodeItem.EntityLogicalName, nodeItem.MessageName);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    image = await repositoryImage.GetStepImageByIdAsync(form.Image.Id);

                    this.trVPluginTree.Dispatcher.Invoke(() =>
                    {
                        FillNodeImageInformation(nodeItem, image, nodeItem.EntityLogicalName, nodeItem.MessageName, nodeItem.PluginTypeId, nodeItem.PluginTypeName, nodeItem.PluginAssemblyId, nodeItem.PluginAssemblyName);

                        this.trVPluginTree.UpdateLayout();
                    });
                }
            }
            else if (nodeItem.ComponentType == ComponentType.PluginAssembly && nodeItem.PluginAssemblyId.HasValue)
            {
                var repository = new PluginAssemblyRepository(service);

                var pluginAssembly = await repository.GetAssemblyByIdRetrieveRequestAsync(nodeItem.PluginAssemblyId.Value);

                var form = new WindowPluginAssembly(_iWriteToOutput, service, pluginAssembly, null, null);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    await ShowExistingPlugins();
                }
            }
            else if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep && nodeItem.StepId.HasValue)
            {
                List<SdkMessageFilter> filters = await GetSdkMessageFiltersAsync(service);

                var repositoryStep = new SdkMessageProcessingStepRepository(service);

                var step = await repositoryStep.GetStepByIdAsync(nodeItem.StepId.Value);

                var form = new WindowSdkMessageProcessingStep(_iWriteToOutput, service, filters, step);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    step = await repositoryStep.GetStepByIdAsync(form.Step.Id);

                    this.trVPluginTree.Dispatcher.Invoke(() =>
                    {
                        FillNodeStepInformation(nodeItem, step);

                        this.trVPluginTree.UpdateLayout();
                    });
                }
            }
        }

        private async void mIEditInEditor_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ComponentType? componentType = nodeItem.ComponentType;
            Guid? id = nodeItem.GetId();

            if (componentType.HasValue && id.HasValue)
            {
                SolutionComponent.GetComponentTypeEntityName((int)componentType.Value, out var componentEntityName, out _);

                if (!string.IsNullOrEmpty(componentEntityName))
                {
                    var service = await GetService();

                    _commonConfig.Save();

                    WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, componentEntityName, id.Value);
                }
            }
        }

        private async void mIChangeStateSdkObject_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            EntityReference referenceToChangeState = null;
            int? state = null;
            int? status = null;

            if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep
                && nodeItem.StepId.HasValue
            )
            {
                referenceToChangeState = new EntityReference(SdkMessageProcessingStep.EntityLogicalName, nodeItem.StepId.Value);

                state = nodeItem.IsActive ? (int)SdkMessageProcessingStep.Schema.OptionSets.statecode.Disabled_1 : (int)SdkMessageProcessingStep.Schema.OptionSets.statecode.Enabled_0;
                status = nodeItem.IsActive ? (int)SdkMessageProcessingStep.Schema.OptionSets.statuscode.Disabled_1_Disabled_2 : (int)SdkMessageProcessingStep.Schema.OptionSets.statuscode.Enabled_0_Enabled_1;
            }
            else if (nodeItem.ComponentType == ComponentType.Workflow
               && nodeItem.WorkflowId.HasValue
            )
            {
                referenceToChangeState = new EntityReference(Workflow.EntityLogicalName, nodeItem.WorkflowId.Value);

                state = nodeItem.IsActive ? (int)Workflow.Schema.OptionSets.statecode.Draft_0 : (int)Workflow.Schema.OptionSets.statecode.Activated_1;
                status = nodeItem.IsActive ? (int)Workflow.Schema.OptionSets.statuscode.Draft_0_Draft_1 : (int)Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2;
            }

            if (referenceToChangeState != null
                && state.HasValue
                && status.HasValue
            )
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ChangingEntityStateFormat1, referenceToChangeState.LogicalName);

                try
                {
                    await service.ExecuteAsync<Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest()
                    {
                        EntityMoniker = referenceToChangeState,
                        State = new OptionSetValue(state.Value),
                        Status = new OptionSetValue(status.Value),
                    });

                    nodeItem.IsActive = !nodeItem.IsActive;

                    nodeItem.CorrectImage();

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateCompletedFormat1, referenceToChangeState.LogicalName);
                }
                catch (Exception ex)
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateFailedFormat1, referenceToChangeState.LogicalName);

                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }
        }

        private async void mIDeleteSdkObject_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            await TryDeleteSdkObject(nodeItem);
        }

        private async Task TryDeleteSdkObject(PluginTreeViewItem nodeItem)
        {
            EntityReference referenceToDelete = null;

            if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStepImage && nodeItem.StepImageId.HasValue)
            {
                referenceToDelete = new EntityReference(SdkMessageProcessingStepImage.EntityLogicalName, nodeItem.StepImageId.Value);
            }
            else if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep && nodeItem.StepId.HasValue)
            {
                referenceToDelete = new EntityReference(SdkMessageProcessingStep.EntityLogicalName, nodeItem.StepId.Value);
            }
            else if (nodeItem.ComponentType == ComponentType.PluginType && nodeItem.PluginTypeId.HasValue)
            {
                referenceToDelete = new EntityReference(PluginType.EntityLogicalName, nodeItem.PluginTypeId.Value);
            }
            else if (nodeItem.ComponentType == ComponentType.PluginAssembly && nodeItem.PluginAssemblyId.HasValue)
            {
                referenceToDelete = new EntityReference(PluginAssembly.EntityLogicalName, nodeItem.PluginAssemblyId.Value);
            }
            else if (nodeItem.ComponentType == ComponentType.Workflow && nodeItem.WorkflowId.HasValue)
            {
                referenceToDelete = new EntityReference(Workflow.EntityLogicalName, nodeItem.WorkflowId.Value);
            }

            if (referenceToDelete != null)
            {
                string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, nodeItem.ComponentType, nodeItem.Name);

                if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    var service = await GetService();

                    try
                    {
                        await service.DeleteAsync(referenceToDelete.LogicalName, referenceToDelete.Id);

                        if (nodeItem.Parent != null)
                        {
                            nodeItem.Parent.Items.Remove(nodeItem);
                            CheckChildNodes(nodeItem.Parent);
                        }
                        else if (trVPluginTree.ItemsSource != null
                            && trVPluginTree.ItemsSource is ObservableCollection<PluginTreeViewItem> list
                            && list.Contains(nodeItem)
                        )
                        {
                            var index = list.IndexOf(nodeItem) - 1;
                            list.Remove(nodeItem);

                            if (0 <= index && index < list.Count)
                            {
                                list[index].IsSelected = true;
                            }
                        }

                        trVPluginTree.UpdateLayout();
                    }
                    catch (Exception ex)
                    {
                        _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                        _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                    }
                }
            }
        }

        private void CheckChildNodes(PluginTreeViewItem nodeItem)
        {
            if (nodeItem == null
                || nodeItem.ComponentType == ComponentType.PluginAssembly
                || nodeItem.ComponentType == ComponentType.PluginType
                || nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep
            )
            {
                return;
            }

            if (nodeItem.Items.Count == 0 && nodeItem.Parent != null)
            {
                nodeItem.Parent.Items.Remove(nodeItem);

                CheckChildNodes(nodeItem.Parent);
            }
        }

        private async void mICompareWithLocalAssembly_Click(object sender, RoutedEventArgs e)
        {
            PluginTreeViewItem nodeItem = GetItemFromRoutedDataContext<PluginTreeViewItem>(e);

            if (nodeItem == null || !nodeItem.PluginAssemblyId.HasValue)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingPluginAssemblyWithLocalAssemblyFormat1, nodeItem.Name);

            var controller = new PluginController(_iWriteToOutput);

            string filePath = await controller.CreateFileWithAssemblyComparing(_commonConfig.FolderForExport, service, nodeItem.PluginAssemblyId.Value, nodeItem.Name, null);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingPluginAssemblyWithLocalAssemblyCompletedFormat1, nodeItem.Name);
        }

        private async void tSBRegisterAssembly_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteRegisterNewAssembly();
        }

        private async Task ExecuteRegisterNewAssembly()
        {
            var service = await GetService();

            var pluginAssembly = new PluginAssembly()
            {
            };

            var form = new WindowPluginAssembly(_iWriteToOutput, service, pluginAssembly, null, null);

            if (form.ShowDialog().GetValueOrDefault())
            {
                await ShowExistingPlugins();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void treeViewCopy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (trVPluginTree.SelectedItem != null && trVPluginTree.SelectedItem is PluginTreeViewItem nodeItem)
            {
                e.Handled = true;

                ClipboardHelper.SetText(nodeItem.Name);
            }
        }

        private async void trVPluginTreeDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (trVPluginTree.SelectedItem != null && trVPluginTree.SelectedItem is PluginTreeViewItem nodeItem)
            {
                e.Handled = true;
                await TryDeleteSdkObject(nodeItem);
            }
        }

        private async void trVPluginTreeNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (trVPluginTree.SelectedItem != null && trVPluginTree.SelectedItem is PluginTreeViewItem nodeItem)
            {
                if (nodeItem.ComponentType == ComponentType.PluginType && nodeItem.PluginTypeId.HasValue)
                {
                    e.Handled = true;

                    await ExecuteAddingNewPluginStep(nodeItem);
                }
                else if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep && nodeItem.StepId.HasValue)
                {
                    e.Handled = true;

                    await ExecuteAddingPluginStepImage(nodeItem);
                }
                else if (nodeItem.ComponentType == ComponentType.PluginAssembly)
                {
                    e.Handled = true;

                    await ExecuteRegisterNewAssembly();
                }
            }
        }

        private async void trVPluginTreeOpenProperties_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (trVPluginTree.SelectedItem != null && trVPluginTree.SelectedItem is PluginTreeViewItem nodeItem)
            {
                if ((nodeItem.ComponentType == ComponentType.SdkMessageProcessingStepImage && nodeItem.StepImageId.HasValue)
                    || (nodeItem.ComponentType == ComponentType.PluginAssembly && nodeItem.PluginAssemblyId.HasValue)
                    || (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep && nodeItem.StepId.HasValue)
                )
                {
                    e.Handled = true;
                    await ExecuteUpdateSdkObject(nodeItem);
                }
            }
        }
    }
}