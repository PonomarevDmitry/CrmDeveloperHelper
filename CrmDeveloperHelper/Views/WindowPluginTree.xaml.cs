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
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowPluginTree : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly Dictionary<Guid, Task> _cacheTaskGettingMessageFilters = new Dictionary<Guid, Task>();

        private readonly Dictionary<Guid, List<SdkMessageFilter>> _cacheMessageFilters = new Dictionary<Guid, List<SdkMessageFilter>>();

        private readonly ObservableCollection<PluginTreeViewItem> _pluginTree = new ObservableCollection<PluginTreeViewItem>();

        private View _currentView = View.ByEntity;

        private BitmapImage _imagePluginAssembly;
        private BitmapImage _imageEntity;
        private BitmapImage _imageImage;
        private BitmapImage _imageMessage;
        private BitmapImage _imagePluginType;
        private BitmapImage _imageStage;
        private BitmapImage _imageStep;
        private BitmapImage _imageStepDisabled;
        private BitmapImage _imageBusinessRule;
        private BitmapImage _imageBusinessProcess;
        private BitmapImage _imageWorkflowActivity;

        private readonly CommonConfiguration _commonConfig;

        private enum View
        {
            ByEntity = 0,
            ByAssembly = 1,
            ByMessage = 2,
        }

        public WindowPluginTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter
            , string pluginTypeFilter
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

            LoadFromConfig();

            FillEntityNames(service.ConnectionData);

            LoadImages();

            LoadConfiguration();

            cmBEntityName.Text = entityFilter;
            txtBPluginTypeFilter.Text = pluginTypeFilter;
            txtBMessageFilter.Text = messageFilter;

            FocusOnComboBoxTextBox(cmBEntityName);

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            trVPluginTree.ItemsSource = _pluginTree;

            this.DecreaseInit();

            ShowExistingPlugins();
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

        private const string paramView = "View";

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

            {
                var temp = winConfig.GetValueString(paramView);

                if (!string.IsNullOrEmpty(temp))
                {
                    if (Enum.TryParse<View>(temp, out View tempView))
                    {
                        this._currentView = tempView;

                        this.IncreaseInit();

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
            winConfig.DictString[paramPluginTypeName] = this.txtBPluginTypeFilter.Text.Trim();

            winConfig.DictBool[paramPreValidationStage] = this.chBStagePreValidation.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPreStage] = this.chBStagePre.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPostSynchStage] = this.chBStagePostSync.IsChecked.GetValueOrDefault();
            winConfig.DictBool[paramPostAsynchStage] = this.chBStagePostAsync.IsChecked.GetValueOrDefault();

            winConfig.DictBool[paramBusinessRules] = this.chBBusinessRules.IsChecked.GetValueOrDefault();

            winConfig.DictBool[paramBusinessProcesses] = this.chBBusinessProcesses.IsChecked.GetValueOrDefault();

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
            this._imageBusinessRule = this.Resources["ImageBusinessRule"] as BitmapImage;
            this._imageBusinessProcess = this.Resources["ImageBusinessProcess"] as BitmapImage;
            this._imageWorkflowActivity = this.Resources["ImageWorkflowActivity"] as BitmapImage;
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

        private async Task ShowExistingPlugins()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingPlugins);

            this.trVPluginTree.Dispatcher.Invoke(() =>
            {
                _pluginTree.Clear();

                this.trVPluginTree.BeginInit();
            });

            string entityName = string.Empty;
            string messageName = string.Empty;
            string pluginTypeName = string.Empty;

            bool withBusinessRules = false;
            bool withBusinessProcesses = false;

            this.Dispatcher.Invoke(() =>
            {
                entityName = cmBEntityName.Text?.Trim();
                messageName = txtBMessageFilter.Text.Trim();
                pluginTypeName = txtBPluginTypeFilter.Text.Trim();
                withBusinessRules = chBBusinessRules.IsChecked.GetValueOrDefault();
                withBusinessProcesses = chBBusinessProcesses.IsChecked.GetValueOrDefault();
            });

            var stages = GetStages();

            try
            {
                if (service != null)
                {
                    PluginSearchRepository repository = new PluginSearchRepository(service);

                    switch (_currentView)
                    {
                        case View.ByAssembly:
                            await FillTreeByAssembly(service, stages, pluginTypeName, messageName, entityName);
                            break;

                        case View.ByMessage:
                            await FillTreeByMessage(service, stages, pluginTypeName, messageName, entityName);
                            break;

                        case View.ByEntity:
                        default:
                            await FillTreeByEntity(service, stages, pluginTypeName, messageName, entityName, withBusinessRules, withBusinessProcesses);
                            break;
                    }

                    ExpandNodes(_pluginTree);

                    if (!_cacheMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        if (!_cacheTaskGettingMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
                        {
                            _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId] = GetSdkMessageFiltersAsync(service);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this.trVPluginTree.Dispatcher.Invoke(() =>
            {
                this.trVPluginTree.EndInit();
            });

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingPluginsCompleted);
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

        private async Task FillTreeByEntity(
            IOrganizationServiceExtented service
            , List<PluginStage> stages
            , string pluginTypeNameFilter
            , string messageNameFilter
            , string entityNameFilter
            , bool withBusinessRules
            , bool withBusinessProcesses
            )
        {
            PluginSearchRepository repository = new PluginSearchRepository(service);

            PluginSearchResult searchResult = await repository.FindAllAsync(stages, pluginTypeNameFilter, messageNameFilter, entityNameFilter);

            List<ProcessingStep> processingSteps = new List<ProcessingStep>();

            processingSteps.AddRange(searchResult.SdkMessageProcessingStep.Select(e => new ProcessingStep(e.PrimaryObjectTypeCodeName, e.SdkMessageId?.Name ?? "Unknown", e.Stage?.Value, e.Mode?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1, e.Rank, e.Name, e)));

            var repWorkflow = new WorkflowRepository(service);

            if (withBusinessProcesses)
            {
                IEnumerable<Workflow> businessProcesses = await repWorkflow.GetListAsync(entityNameFilter, (int)Workflow.Schema.OptionSets.category.Workflow_0, null, _columnSetWorkflow);

                foreach (var item in businessProcesses)
                {
                    if ("Create".StartsWith(messageNameFilter, StringComparison.InvariantCultureIgnoreCase) && item.TriggerOnCreate.GetValueOrDefault())
                    {
                        processingSteps.Add(new ProcessingStep(item.PrimaryEntity, "Create", item.CreateStage?.Value ?? (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40, item.Mode?.Value == (int)Workflow.Schema.OptionSets.mode.Background_0, item.Rank, item.Name, item));
                    }

                    if ("Update".StartsWith(messageNameFilter, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(item.TriggerOnUpdateAttributeList))
                    {
                        processingSteps.Add(new ProcessingStep(item.PrimaryEntity, "Update", item.UpdateStage?.Value ?? (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40, item.Mode?.Value == (int)Workflow.Schema.OptionSets.mode.Background_0, item.Rank, item.Name, item));
                    }

                    if ("Delete".StartsWith(messageNameFilter, StringComparison.InvariantCultureIgnoreCase) && item.TriggerOnDelete.GetValueOrDefault())
                    {
                        processingSteps.Add(new ProcessingStep(item.PrimaryEntity, "Delete", item.DeleteStage?.Value ?? (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40, item.Mode?.Value == (int)Workflow.Schema.OptionSets.mode.Background_0, item.Rank, item.Name, item));
                    }
                }
            }

            var listEntities = new HashSet<string>(processingSteps.Select(ent => ent.EntityName), StringComparer.InvariantCultureIgnoreCase);

            IEnumerable<Workflow> businessRules = null;

            if (withBusinessRules)
            {
                businessRules = await repWorkflow.GetListAsync(entityNameFilter, (int)Workflow.Schema.OptionSets.category.Business_Rule_2, null, _columnSetWorkflow);

                foreach (var item in businessRules)
                {
                    listEntities.Add(item.PrimaryEntity);
                }
            }

            foreach (var entityName in listEntities.OrderBy(s => s))
            {
                var grEntity = processingSteps.Where(e => string.Equals(e.EntityName, entityName, StringComparison.InvariantCultureIgnoreCase));

                PluginTreeViewItem nodeEntity = CreateNodeEntity(entityName, grEntity.Where(e => e.Entity is SdkMessageProcessingStep).Select(e => e.Entity as SdkMessageProcessingStep));

                if (businessRules != null)
                {
                    var entityRules = businessRules.Where(w => string.Equals(w.PrimaryEntity, entityName, StringComparison.InvariantCultureIgnoreCase));

                    if (entityRules.Any())
                    {
                        var nodeRules = CreateNodeBusinessRules(entityName);

                        nodeEntity.Items.Add(nodeRules);
                        nodeRules.Parent = nodeEntity;

                        foreach (var item in entityRules.OrderBy(w => w.Name))
                        {
                            var nodeRule = CreateNodeBusinessRule(item);

                            nodeRules.Items.Add(nodeRule);
                            nodeRule.Parent = nodeRules;
                        }
                    }
                }

                var groupsByMessages = grEntity.GroupBy(e => e.Message).OrderBy(mess => mess.Key, new MessageComparer());

                foreach (var grMessage in groupsByMessages)
                {
                    PluginTreeViewItem nodeMessage = CreateNodeMessage(grMessage.Key, grMessage.Where(e => e.Entity is SdkMessageProcessingStep).Select(e => e.Entity as SdkMessageProcessingStep));

                    nodeEntity.Items.Add(nodeMessage);
                    nodeMessage.Parent = nodeEntity;

                    var groupsStage = grMessage.GroupBy(ent => ent.Stage.Value).OrderBy(item => item.Key);

                    foreach (var stage in groupsStage)
                    {
                        PluginTreeViewItem nodeStage = null;
                        PluginTreeViewItem nodePostAsynch = null;

                        foreach (var step in stage.OrderBy(s => s.AsyncMode).ThenBy(s => s.Rank).ThenBy(s => s.Name))
                        {
                            PluginTreeViewItem nodeTarget = null;

                            if (step.AsyncMode)
                            {
                                if (nodePostAsynch == null)
                                {
                                    nodePostAsynch = CreateNodeStage(stage.Key, step.AsyncMode ? 1 : 0);

                                    nodeMessage.Items.Add(nodePostAsynch);
                                    nodePostAsynch.Parent = nodeMessage;
                                }

                                nodeTarget = nodePostAsynch;
                            }
                            else
                            {
                                if (nodeStage == null)
                                {
                                    nodeStage = CreateNodeStage(stage.Key, step.AsyncMode ? 1 : 0);

                                    nodeMessage.Items.Add(nodeStage);
                                    nodeStage.Parent = nodeMessage;
                                }

                                nodeTarget = nodeStage;
                            }

                            PluginTreeViewItem nodeStep = CreateNodeStep(step.Entity, searchResult.SdkMessageProcessingStepImage);

                            nodeTarget.Items.Add(nodeStep);
                            nodeStep.Parent = nodeTarget;

                            nodeStep.IsExpanded = true;
                        }

                        if (nodeStage != null)
                        {
                            nodeStage.IsExpanded = true;
                        }

                        if (nodePostAsynch != null)
                        {
                            nodePostAsynch.IsExpanded = true;
                        }
                    }

                    nodeMessage.IsExpanded = true;
                }

                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(nodeEntity);
                });
            }
        }

        private async Task FillTreeByMessage(IOrganizationServiceExtented service, List<PluginStage> stages, string pluginTypeName, string messageName, string entityName)
        {
            PluginSearchRepository repository = new PluginSearchRepository(service);

            PluginSearchResult searchResult = await repository.FindAllAsync(stages, pluginTypeName, messageName, entityName);

            var groupsByMessage = searchResult.SdkMessageProcessingStep.GroupBy(ent => ent.SdkMessageId?.Name ?? "Unknown").OrderBy(mess => mess.Key, new MessageComparer());

            foreach (var grMessage in groupsByMessage)
            {
                var nodeMessage = CreateNodeMessage(grMessage.Key, grMessage);

                var groupsByEntity = grMessage.GroupBy(ent => ent.PrimaryObjectTypeCodeName).OrderBy(e => e.Key);

                foreach (var grEntity in groupsByEntity)
                {
                    PluginTreeViewItem nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);

                    nodeMessage.Items.Add(nodeEntity);
                    nodeEntity.Parent = nodeMessage;

                    var groupsStage = grEntity.GroupBy(ent => ent.Stage.Value).OrderBy(item => item.Key);

                    foreach (var stage in groupsStage)
                    {
                        PluginTreeViewItem nodeStage = null;
                        PluginTreeViewItem nodePostAsynch = null;

                        foreach (var step in stage.OrderBy(s => s.Mode.Value).ThenBy(s => s.Rank).ThenBy(s => s.Name))
                        {
                            PluginTreeViewItem nodeTarget = null;

                            if (step.Mode.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1)
                            {
                                if (nodePostAsynch == null)
                                {
                                    nodePostAsynch = CreateNodeStage(stage.Key, step.Mode.Value);

                                    nodeEntity.Items.Add(nodePostAsynch);
                                    nodePostAsynch.Parent = nodeEntity;
                                }

                                nodeTarget = nodePostAsynch;
                            }
                            else
                            {
                                if (nodeStage == null)
                                {
                                    nodeStage = CreateNodeStage(stage.Key, step.Mode.Value);

                                    nodeEntity.Items.Add(nodeStage);
                                    nodeStage.Parent = nodeEntity;
                                }

                                nodeTarget = nodeStage;
                            }

                            PluginTreeViewItem nodeStep = CreateNodeStep(step, searchResult.SdkMessageProcessingStepImage);

                            nodeTarget.Items.Add(nodeStep);
                            nodeStep.Parent = nodeTarget;

                            nodeStep.IsExpanded = true;
                        }

                        if (nodeStage != null)
                        {
                            nodeStage.IsExpanded = true;
                        }

                        if (nodePostAsynch != null)
                        {
                            nodePostAsynch.IsExpanded = true;
                        }
                    }
                }

                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(nodeMessage);
                });
            }
        }

        private async Task FillTreeByAssembly(IOrganizationServiceExtented service, List<PluginStage> stages, string pluginTypeName, string messageName, string entityName)
        {
            var repositoryPluginType = new PluginTypeRepository(service);
            var repository = new PluginSearchRepository(service);

            var pluginTypeList = await repositoryPluginType.GetPluginTypesAsync(pluginTypeName, null);
            PluginSearchResult searchResult = await repository.FindAllAsync(stages, pluginTypeName, messageName, entityName);

            var assemblyList = pluginTypeList.GroupBy(p => new { p.PluginAssemblyId.Id, Name = p.AssemblyName });
            //var pluginTypeDict = pluginTypeList.GroupBy(p => p.PluginAssemblyId.Id).ToDictionary(p => p.Key);
            var stepsByPluginTypeDict = searchResult.SdkMessageProcessingStep.GroupBy(s => s.EventHandler.Id).ToDictionary(s => s.Key);

            foreach (var assemblyGroup in assemblyList.OrderBy(a => a.Key.Name))
            {
                PluginTreeViewItem nodeAssembly = CreateNodeAssembly(assemblyGroup.Key.Name, assemblyGroup.Key.Id);

                foreach (var pluginType in assemblyGroup.OrderBy(p => p.IsWorkflowActivity.GetValueOrDefault()).ThenBy(p => p.TypeName))
                {
                    PluginTreeViewItem nodePluginType = CreateNodePluginType(pluginType.TypeName, pluginType.Id, assemblyGroup.Key.Id, pluginType.IsWorkflowActivity.GetValueOrDefault());
                    nodeAssembly.Items.Add(nodePluginType);
                    nodePluginType.Parent = nodeAssembly;

                    if (stepsByPluginTypeDict.ContainsKey(pluginType.Id))
                    {
                        var groupsByEnity = stepsByPluginTypeDict[pluginType.Id].GroupBy(s => s.PrimaryObjectTypeCodeName).OrderBy(gr => gr.Key);

                        foreach (var grEntity in groupsByEnity)
                        {
                            var nodeEntity = CreateNodeEntity(grEntity.Key, grEntity);

                            nodePluginType.Items.Add(nodeEntity);
                            nodeEntity.Parent = nodePluginType;

                            var groupsByMessages = grEntity.GroupBy(ent => ent.SdkMessageId?.Name ?? "Unknown").OrderBy(mess => mess.Key, new MessageComparer());

                            foreach (var grMessage in groupsByMessages)
                            {
                                PluginTreeViewItem nodeMessage = CreateNodeMessage(grMessage.Key, grMessage);

                                nodeEntity.Items.Add(nodeMessage);
                                nodeMessage.Parent = nodeEntity;

                                var groupsStage = grMessage.GroupBy(ent => ent.Stage.Value).OrderBy(item => item.Key);

                                foreach (var stage in groupsStage)
                                {
                                    PluginTreeViewItem nodeStage = null;
                                    PluginTreeViewItem nodePostAsynch = null;

                                    foreach (var step in stage.OrderBy(s => s.Mode.Value).ThenBy(s => s.Rank).ThenBy(s => s.Name))
                                    {
                                        PluginTreeViewItem nodeTarget = null;

                                        if (step.Mode.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1)
                                        {
                                            if (nodePostAsynch == null)
                                            {
                                                nodePostAsynch = CreateNodeStage(stage.Key, step.Mode.Value);

                                                nodeMessage.Items.Add(nodePostAsynch);
                                                nodePostAsynch.Parent = nodeMessage;
                                            }

                                            nodeTarget = nodePostAsynch;
                                        }
                                        else
                                        {
                                            if (nodeStage == null)
                                            {
                                                nodeStage = CreateNodeStage(stage.Key, step.Mode.Value);

                                                nodeMessage.Items.Add(nodeStage);
                                                nodeStage.Parent = nodeMessage;
                                            }

                                            nodeTarget = nodeStage;
                                        }

                                        PluginTreeViewItem nodeStep = CreateNodeStep(step, searchResult.SdkMessageProcessingStepImage);

                                        nodeTarget.Items.Add(nodeStep);
                                        nodeStep.Parent = nodeTarget;

                                        nodeStep.IsExpanded = true;
                                    }

                                    if (nodeStage != null)
                                    {
                                        nodeStage.IsExpanded = true;
                                    }

                                    if (nodePostAsynch != null)
                                    {
                                        nodePostAsynch.IsExpanded = true;
                                    }
                                }

                                nodeMessage.IsExpanded = true;
                            }

                            nodeEntity.IsExpanded = true;
                        }
                    }
                }

                this.Dispatcher.Invoke(() =>
                {
                    _pluginTree.Add(nodeAssembly);
                });
            }
        }

        private PluginTreeViewItem CreateNodeAssembly(string assemblyName, Guid? idPluginAssembly)
        {
            var nodeAssembly = new PluginTreeViewItem(ComponentType.PluginAssembly)
            {
                Name = assemblyName,
                Image = _imagePluginAssembly,

                PluginAssembly = idPluginAssembly,
            };

            return nodeAssembly;
        }

        private PluginTreeViewItem CreateNodePluginType(string pluginTypeName, Guid? idPluginType, Guid? idPluginAssembly, bool isWorkflowActivity)
        {
            var nodeType = new PluginTreeViewItem(ComponentType.PluginType)
            {
                Name = pluginTypeName,
                Image = isWorkflowActivity ? _imageWorkflowActivity : _imagePluginType,

                PluginType = idPluginType,
                PluginAssembly = idPluginAssembly,

                IsWorkflowActivity = isWorkflowActivity,
            };

            return nodeType;
        }

        private PluginTreeViewItem CreateNodeStage(int stage, int mode)
        {
            string name = SdkMessageProcessingStepRepository.GetStageName(stage, mode);

            var nodeStage = new PluginTreeViewItem(null)
            {
                Name = name,
                Image = _imageStage,
            };

            return nodeStage;
        }

        private PluginTreeViewItem CreateNodeBusinessRule(Entities.Workflow businessRule)
        {
            var nodeStep = new PluginTreeViewItem(ComponentType.Workflow)
            {
                Name = businessRule.Name,

                Workflow = businessRule.Id,

                IsActive = businessRule.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1,

                ImageActive = _imageStep,
                ImageInactive = _imageStepDisabled,
            };

            nodeStep.Image = businessRule.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1 ? _imageStep : _imageStepDisabled;

            return nodeStep;
        }

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

                    FillNodeImageInformation(nodeImage, image, step.PrimaryObjectTypeCodeName, step.SdkMessageId?.Name, step.EventHandler?.Id, step.PluginAssemblyId);

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

                    Workflow = workflow.Id,

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
            nodeStep.PluginAssembly = step.PluginAssemblyId;
            nodeStep.PluginType = step.EventHandler?.Id;
            nodeStep.MessageName = step.SdkMessageId?.Name;
            nodeStep.Step = step.Id;
            nodeStep.IsActive = step.StateCodeEnum == SdkMessageProcessingStep.Schema.OptionSets.statecode.Enabled_0;
            nodeStep.ImageActive = _imageStep;
            nodeStep.ImageInactive = _imageStepDisabled;

            nodeStep.Image = step.StateCodeEnum == SdkMessageProcessingStep.Schema.OptionSets.statecode.Enabled_0 ? _imageStep : _imageStepDisabled;
        }

        private void FillNodeImageInformation(PluginTreeViewItem nodeImage, SdkMessageProcessingStepImage image, string entityName, string messageName, Guid? idPluginType, Guid? idPluginAssembly)
        {
            string nameImage = GetImageName(image);
            string tooltipImage = GetImageTooltip(image);

            nodeImage.Name = nameImage;
            nodeImage.Tooltip = tooltipImage;
            nodeImage.Image = _imageImage;
            nodeImage.EntityLogicalName = entityName;
            nodeImage.MessageName = messageName;
            nodeImage.PluginAssembly = idPluginAssembly;
            nodeImage.PluginType = idPluginType;
            nodeImage.Step = image.SdkMessageProcessingStepImageId;
            nodeImage.StepImage = image.Id;
        }

        private PluginTreeViewItem CreateNodeMessage(string message, IEnumerable<SdkMessageProcessingStep> steps)
        {
            var nodeMessage = new PluginTreeViewItem(ComponentType.SdkMessage)
            {
                Name = message,
                Image = _imageMessage,

                MessageName = message,

                MessageList = steps.Where(s => s.SdkMessageId != null).Select(s => s.SdkMessageId.Id).Distinct().ToList(),

                MessageFilterList = steps.Where(s => s.SdkMessageFilterId != null).Select(s => s.SdkMessageFilterId.Id).Distinct().ToList(),
            };

            return nodeMessage;
        }

        private PluginTreeViewItem CreateNodeEntity(string entityName, IEnumerable<SdkMessageProcessingStep> steps)
        {
            var nodeMessage = new PluginTreeViewItem(ComponentType.Entity)
            {
                Name = entityName,
                Image = _imageEntity,

                EntityLogicalName = entityName,

                MessageFilterList = steps.Where(s => s.SdkMessageFilterId != null).Select(s => s.SdkMessageFilterId.Id).Distinct().ToList(),
            };

            return nodeMessage;
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, tSBCollapseAll, tSBExpandAll, tSBRegisterAssembly, menuView);

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

            if (item.PluginAssembly.HasValue && item.ComponentType == ComponentType.PluginAssembly)
            {
                return "Create Plugin Assembly Description";
            }

            if (item.PluginType.HasValue && item.ComponentType == ComponentType.PluginType)
            {
                return "Create Plugin Type Description";
            }

            if (item.MessageList != null && item.MessageList.Any() && item.ComponentType == ComponentType.SdkMessage)
            {
                return "Create Message Description";
            }

            if (item.MessageFilterList != null && item.MessageFilterList.Any() && item.ComponentType == ComponentType.SdkMessage)
            {
                return "Create Message Filter Description";
            }

            if (item.Step.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return "Create Step Description";
            }

            if (item.StepImage.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                return "Create Image Description";
            }

            if (item.Workflow.HasValue && item.ComponentType == ComponentType.Workflow)
            {
                return "Create Workflow Description";
            }

            return "Create Description";
        }

        private string GetUpdateName(PluginTreeViewItem item)
        {
            if (item == null)
            {
                return "Update";
            }

            if (item.PluginAssembly.HasValue && item.ComponentType == ComponentType.PluginAssembly)
            {
                return "Update Plugin Assembly";
            }

            if (item.Step.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return "Update Step";
            }

            if (item.StepImage.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
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

            if (item.PluginAssembly.HasValue && item.ComponentType == ComponentType.PluginAssembly)
            {
                return "Edit Plugin Assembly in Editor";
            }

            if (item.PluginType.HasValue && item.ComponentType == ComponentType.PluginType)
            {
                return "Edit Plugin Type in Editor";
            }

            if (item.Step.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return "Edit Step in Editor";
            }

            if (item.StepImage.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                return "Edit Image in Editor";
            }

            if (item.Workflow.HasValue && item.ComponentType == ComponentType.Workflow)
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

            if (item.Step.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return $"{action} Step";
            }

            if (item.Workflow.HasValue && item.ComponentType == ComponentType.Workflow)
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

            if (item.PluginAssembly.HasValue && item.ComponentType == ComponentType.PluginAssembly)
            {
                return "Delete Plugin Assembly";
            }

            if (item.PluginType.HasValue && item.ComponentType == ComponentType.PluginType)
            {
                return "Delete Plugin Type";
            }

            if (item.Step.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                return "Delete Step";
            }

            if (item.StepImage.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                return "Delete Image";
            }

            return "Delete";
        }

        private bool CanCreateDescription(PluginTreeViewItem item)
        {
            return (item.PluginAssembly.HasValue && item.ComponentType == ComponentType.PluginAssembly)
                || (item.PluginType.HasValue && item.ComponentType == ComponentType.PluginType)
                || ((item.MessageList != null && item.MessageList.Any()) && item.ComponentType == ComponentType.SdkMessage)
                || ((item.MessageFilterList != null && item.MessageFilterList.Any()) && item.ComponentType == ComponentType.SdkMessage)
                || (item.Step.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStep)
                || (item.StepImage.HasValue && item.ComponentType == ComponentType.SdkMessageProcessingStepImage)
                || (item.Workflow.HasValue && item.ComponentType == ComponentType.Workflow)
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
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, true);
        }

        private void mICollapseNodes_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

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

            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;

                ChangeExpandedInTreeViewItems(item.Items, isExpanded);
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

        private async Task CreateDescription(PluginTreeViewItem node)
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

            if (node.PluginAssembly.HasValue && node.ComponentType == ComponentType.PluginAssembly)
            {
                var repository = new PluginAssemblyRepository(service);
                var pluginAssembly = await repository.GetAssemblyByIdAsync(node.PluginAssembly.Value);

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

            if (node.PluginType.HasValue && node.ComponentType == ComponentType.PluginType)
            {
                var repository = new PluginTypeRepository(service);
                var pluginType = await repository.GetPluginTypeByIdAsync(node.PluginType.Value);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetPluginTypeFileName(service.ConnectionData.Name, pluginType.TypeName, "Description");
                }

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

            if (node.Step.HasValue && node.ComponentType == ComponentType.SdkMessageProcessingStep)
            {
                var repository = new SdkMessageProcessingStepRepository(service);
                var step = await repository.GetStepByIdAsync(node.Step.Value);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetPluginStepFileName(service.ConnectionData.Name, step.Name, "Description");
                }

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

            if (node.StepImage.HasValue && node.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            {
                var repository = new SdkMessageProcessingStepImageRepository(service);
                SdkMessageProcessingStepImage stepImage = await repository.GetStepImageByIdAsync(node.StepImage.Value);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetPluginImageFileName(service.ConnectionData.Name, stepImage.Name, "Description");
                }

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

            if (node.MessageList != null && node.MessageList.Any())
            {
                var repository = new SdkMessageRepository(service);
                List<SdkMessage> listMessages = await repository.GetMessageByIdsAsync(node.MessageList.ToArray());

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetMessageFileName(service.ConnectionData.Name, node.Name, "Description");
                }

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

            if (node.MessageFilterList != null && node.MessageFilterList.Any())
            {
                var repository = new SdkMessageFilterRepository(service);
                List<SdkMessageFilter> listMessages = await repository.GetMessageFiltersByIdsAsync(node.MessageFilterList.ToArray());

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = EntityFileNameFormatter.GetMessageFilterFileName(service.ConnectionData.Name, node.Name, "Description");
                }

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

            if (node.Workflow.HasValue && node.ComponentType == ComponentType.Workflow)
            {
                var repository = new WorkflowRepository(service);
                var workflow = await repository.GetByIdAsync(node.Workflow.Value);

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

                if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                result.AppendLine(service.ConnectionData.GetConnectionInfo());

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

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingDescriptionCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectionData connectionData = null;

            this.Dispatcher.Invoke(() =>
            {
                trVPluginTree.ItemsSource = null;
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

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            var items = contextMenu.Items.OfType<Control>();

            bool isEntity = !string.IsNullOrEmpty(nodeItem.EntityLogicalName) && !string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase);

            bool isPluginAssembly = nodeItem.PluginAssembly.HasValue && nodeItem.ComponentType == ComponentType.PluginAssembly;
            bool isPluginType = nodeItem.PluginType.HasValue && nodeItem.ComponentType == ComponentType.PluginType;
            bool isStep = nodeItem.Step.HasValue && nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep;
            bool isStepImage = nodeItem.Step.HasValue && nodeItem.ComponentType == ComponentType.SdkMessageProcessingStepImage;

            bool isWorkflow = nodeItem.Workflow.HasValue && nodeItem.ComponentType == ComponentType.Workflow;

            bool showDependentComponents = nodeItem.GetId().HasValue || (isEntity && nodeItem.ComponentType == ComponentType.Entity);

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            ActivateControls(items, CanCreateDescription(nodeItem), "contMnCreateDescription");
            SetControlsName(items, GetCreateDescriptionName(nodeItem), "contMnCreateDescription");

            ActivateControls(items, isPluginAssembly || isStep || isStepImage, "contMnUpdate");
            SetControlsName(items, GetUpdateName(nodeItem), "contMnUpdate");

            ActivateControls(items, nodeItem.GetId().HasValue, "contMnEditor");
            SetControlsName(items, GetEditName(nodeItem), "contMnEditor");

            ActivateControls(items, isWorkflow || isStep, "contMnChangeState");
            SetControlsName(items, GetChangeStateName(nodeItem), "contMnChangeState");

            ActivateControls(items, isPluginAssembly || isPluginType || isStep || isStepImage, "contMnDelete");
            SetControlsName(items, GetDeleteName(nodeItem), "contMnDelete");

            ActivateControls(items, isPluginType, "contMnAddPluginStep");
            ActivateControls(items, isStep && isEntity, "contMnAddPluginStepImage");

            ActivateControls(items, isWorkflow, "contMnOpenInWeb", "contMnWorkflowExlorer");

            ActivateControls(items, isEntity, "contMnEntity");

            ActivateControls(items, showDependentComponents, "contMnDependentComponents");

            ActivateControls(items, isEntity || isPluginAssembly || isStep || isWorkflow, "contMnAddIntoSolution", "contMnAddIntoSolutionLast");

            ActivateControls(items, nodeItem.PluginType.HasValue, "contMnAddPluginTypeStepsIntoSolution", "contMnAddPluginTypeStepsIntoSolutionLast");

            ActivateControls(items, nodeItem.PluginAssembly.HasValue && nodeItem.ComponentType != ComponentType.PluginAssembly, "contMnAddPluginAssemblyIntoSolution", "contMnAddPluginAssemblyIntoSolutionLast");

            ActivateControls(items, nodeItem.PluginAssembly.HasValue, "contMnAddPluginAssemblyStepsIntoSolution", "contMnAddPluginAssemblyStepsIntoSolutionLast", "contMnCompareWithLocalAssembly");

            ActivateControls(items, nodeItem.ComponentType == ComponentType.SdkMessage && !string.IsNullOrEmpty(nodeItem.Name), "contMnSdkMessage");

            FillLastSolutionItems(connectionData, items, isEntity || isPluginAssembly || isStep || isWorkflow, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");

            FillLastSolutionItems(connectionData, items, nodeItem.PluginType.HasValue, mIAddPluginTypeStepsIntoSolutionLast_Click, "contMnAddPluginTypeStepsIntoSolutionLast");

            FillLastSolutionItems(connectionData, items, nodeItem.PluginAssembly.HasValue && nodeItem.ComponentType != ComponentType.PluginAssembly, AddAssemblyIntoCrmSolutionLast_Click, "contMnAddPluginAssemblyIntoSolutionLast");

            FillLastSolutionItems(connectionData, items, nodeItem.PluginAssembly.HasValue, mIAddAssemblyStepsIntoSolutionLast_Click, "contMnAddPluginAssemblyStepsIntoSolutionLast");

            CheckSeparatorVisible(items);
        }

        private void mICreateDescription_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

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

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null
                || !nodeItem.Workflow.HasValue
            )
            {
                return;
            }

            var service = await GetService();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Workflow, nodeItem.Workflow.Value);
            }
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

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
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

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
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

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

        private async void mIOpenSdkMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null
                || nodeItem.ComponentType != ComponentType.SdkMessage
                || string.IsNullOrEmpty(nodeItem.Name)
                )
            {
                return;
            }

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(_iWriteToOutput, service, _commonConfig, null, nodeItem.Name);
        }

        private async void mIOpenSdkMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null
                || nodeItem.ComponentType != ComponentType.SdkMessage
                || string.IsNullOrEmpty(nodeItem.Name)
                )
            {
                return;
            }

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(_iWriteToOutput, service, _commonConfig, null, nodeItem.Name);
        }

        private async void mIOpenWorkflowExplorer_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null
                || nodeItem.ComponentType != ComponentType.Workflow
                || !nodeItem.Workflow.HasValue
                )
            {
                return;
            }

            var service = await GetService();

            var repository = new WorkflowRepository(service);
            var workflow = await repository.GetByIdAsync(nodeItem.Workflow.Value, new ColumnSet(Workflow.Schema.Attributes.name, Workflow.Schema.Attributes.primaryentity));

            string entityName = string.Empty;

            if (!string.IsNullOrEmpty(workflow.PrimaryEntity)
                && !string.Equals(workflow.PrimaryEntity, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                entityName = workflow.PrimaryEntity;
            }

            WindowHelper.OpenWorkflowWindow(_iWriteToOutput, service, _commonConfig, entityName, workflow.Name);
        }

        private async void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null || string.IsNullOrEmpty(nodeItem.EntityLogicalName))
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

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ComponentType? componentType = nodeItem.ComponentType;
                Guid? id = nodeItem.GetId();

                if (componentType == ComponentType.Entity
                    && !string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                    && !string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    id = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);
                }

                if (componentType.HasValue && id.HasValue)
                {
                    connectionData.OpenSolutionComponentDependentComponentsInWeb(componentType.Value, id.Value);
                }
            }
        }

        private async void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

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

                if (componentType == ComponentType.Entity
                    && !string.IsNullOrEmpty(nodeItem.EntityLogicalName)
                    && !string.Equals(nodeItem.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    id = connectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);
                }

                if (componentType.HasValue && id.HasValue)
                {
                    WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, null, _commonConfig, (int)nodeItem.ComponentType.Value, id.Value, null);
                }
            }
        }

        private async void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            await AddIntoSolution(nodeItem, true, null);
        }

        private async void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

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

        private async Task AddIntoSolution(PluginTreeViewItem nodeItem, bool withSelect, string solutionUniqueName)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ComponentType? componentType = nodeItem.ComponentType;
            Guid? id = nodeItem.GetId();

            if (componentType == ComponentType.Entity && !string.IsNullOrEmpty(nodeItem.EntityLogicalName))
            {
                id = service.ConnectionData.GetEntityMetadataId(nodeItem.EntityLogicalName);
            }

            if (componentType.HasValue && id.HasValue)
            {
                _commonConfig.Save();

                try
                {
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                    await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, componentType.Value, new[] { id.Value }, null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
        }

        private async void mIAddPluginTypeStepsIntoSolution_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null || !nodeItem.PluginType.HasValue)
            {
                return;
            }

            await AddPluginTypeStepsIntoSolution(nodeItem.PluginType.Value, true, null);
        }

        private async void mIAddPluginTypeStepsIntoSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null || !nodeItem.PluginType.HasValue)
            {
                return;
            }

            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddPluginTypeStepsIntoSolution(nodeItem.PluginType.Value, false, solutionUniqueName);
            }
        }

        private async Task AddPluginTypeStepsIntoSolution(Guid idPluginType, bool withSelect, string solutionUniqueName)
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

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SdkMessageProcessingStep, steps.Select(s => s.Id), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void AddAssemblyIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null || !nodeItem.PluginAssembly.HasValue)
            {
                return;
            }

            await AddPluginTypeStepsIntoSolution(nodeItem.PluginAssembly.Value, true, null);
        }

        private async void AddAssemblyIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null || !nodeItem.PluginAssembly.HasValue)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyIntoSolution(nodeItem.PluginAssembly.Value, false, solutionUniqueName);
            }
        }

        private async Task AddAssemblyIntoSolution(Guid idPluginAssembly, bool withSelect, string solutionUniqueName)
        {
            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.PluginAssembly, new[] { idPluginAssembly }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void mIAddAssemblyStepsIntoSolution_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null || !nodeItem.PluginAssembly.HasValue)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyStepsIntoSolution(nodeItem.PluginAssembly.Value, false, solutionUniqueName);
            }
        }

        private async void mIAddAssemblyStepsIntoSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null || !nodeItem.PluginAssembly.HasValue)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                await AddAssemblyStepsIntoSolution(nodeItem.PluginAssembly.Value, false, solutionUniqueName);
            }
        }

        private async Task AddAssemblyStepsIntoSolution(Guid idPluginAssembly, bool withSelect, string solutionUniqueName)
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

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SdkMessageProcessingStep, steps.Select(s => s.Id), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void mIAddPluginStep_Click(object sender, RoutedEventArgs e)
        {
            var nodePluginType = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodePluginType == null
                || nodePluginType.ComponentType != ComponentType.PluginType
                || !nodePluginType.PluginType.HasValue
            )
            {
                return;
            }

            var service = await GetService();

            var step = new SdkMessageProcessingStep()
            {
                EventHandler = new EntityReference(PluginType.EntityLogicalName, nodePluginType.PluginType.Value),
            };

            List<SdkMessageFilter> filters = null;

            if (!_cacheMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
            {
                if (!_cacheTaskGettingMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId] = GetSdkMessageFiltersAsync(service);
                }

                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.GettingMessages);

                await _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId];

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.GettingMessagesCompleted);
            }

            filters = _cacheMessageFilters[service.ConnectionData.ConnectionId];

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
                });
            }
        }

        private async void mIAddPluginStepImage_Click(object sender, RoutedEventArgs e)
        {
            var nodeStep = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeStep == null
                || nodeStep.ComponentType != ComponentType.SdkMessageProcessingStep
                || !nodeStep.Step.HasValue
                || string.IsNullOrEmpty(nodeStep.EntityLogicalName)
                || string.Equals(nodeStep.EntityLogicalName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return;
            }

            var service = await GetService();

            var image = new SdkMessageProcessingStepImage()
            {
                SdkMessageProcessingStepId = new EntityReference(SdkMessageProcessingStep.EntityLogicalName, nodeStep.Step.Value),
            };

            var form = new WindowSdkMessageProcessingStepImage(_iWriteToOutput, service, image, nodeStep.EntityLogicalName, nodeStep.MessageName);

            if (form.ShowDialog().GetValueOrDefault())
            {
                var repositoryImage = new SdkMessageProcessingStepImageRepository(service);

                image = await repositoryImage.GetStepImageByIdAsync(form.Image.Id);

                this.trVPluginTree.Dispatcher.Invoke(() =>
                {
                    PluginTreeViewItem nodeImage = new PluginTreeViewItem(ComponentType.SdkMessageProcessingStepImage);

                    FillNodeImageInformation(nodeImage, image, nodeStep.EntityLogicalName, nodeStep.MessageName, nodeStep.PluginType, nodeStep.PluginAssembly);

                    nodeStep.Items.Add(nodeImage);
                    nodeImage.Parent = nodeStep;
                });
            }
        }

        private async void mIUpdateSdkObject_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            var service = await GetService();

            if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStepImage
                && nodeItem.StepImage.HasValue
            )
            {
                var repositoryImage = new SdkMessageProcessingStepImageRepository(service);

                var image = await repositoryImage.GetStepImageByIdAsync(nodeItem.StepImage.Value);

                var form = new WindowSdkMessageProcessingStepImage(_iWriteToOutput, service, image, nodeItem.EntityLogicalName, nodeItem.MessageName);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    image = await repositoryImage.GetStepImageByIdAsync(form.Image.Id);

                    this.trVPluginTree.Dispatcher.Invoke(() =>
                    {
                        FillNodeImageInformation(nodeItem, image, nodeItem.EntityLogicalName, nodeItem.MessageName, nodeItem.PluginType, nodeItem.PluginAssembly);

                        this.trVPluginTree.UpdateLayout();
                    });
                }
            }
            else if (nodeItem.ComponentType == ComponentType.PluginAssembly
                && nodeItem.PluginAssembly.HasValue
            )
            {
                var repository = new PluginAssemblyRepository(service);

                var pluginAssembly = await repository.GetAssemblyByIdAsync(nodeItem.PluginAssembly.Value);

                var form = new WindowPluginAssembly(_iWriteToOutput, service, pluginAssembly, null, null);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    ShowExistingPlugins();
                }
            }
            else if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep
                && nodeItem.Step.HasValue
            )
            {
                List<SdkMessageFilter> filters = null;

                if (!_cacheMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    if (!_cacheTaskGettingMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId] = GetSdkMessageFiltersAsync(service);
                    }

                    ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.GettingMessages);

                    await _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId];

                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.GettingMessagesCompleted);
                }

                filters = _cacheMessageFilters[service.ConnectionData.ConnectionId];

                var repositoryStep = new SdkMessageProcessingStepRepository(service);

                var step = await repositoryStep.GetStepByIdAsync(nodeItem.Step.Value);

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
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null
                || !nodeItem.GetId().HasValue
                || string.IsNullOrEmpty(nodeItem.GetEntityName())
            )
            {
                return;
            }

            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, nodeItem.GetEntityName(), nodeItem.GetId().Value);
        }

        private async void mIChangeStateSdkObject_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            EntityReference referenceToChangeState = null;
            int? state = null;
            int? status = null;

            if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep
                && nodeItem.Step.HasValue
            )
            {
                referenceToChangeState = new EntityReference(SdkMessageProcessingStep.EntityLogicalName, nodeItem.Step.Value);

                state = nodeItem.IsActive ? (int)SdkMessageProcessingStep.Schema.OptionSets.statecode.Disabled_1 : (int)SdkMessageProcessingStep.Schema.OptionSets.statecode.Enabled_0;
                status = nodeItem.IsActive ? (int)SdkMessageProcessingStep.Schema.OptionSets.statuscode.Disabled_1_Disabled_2 : (int)SdkMessageProcessingStep.Schema.OptionSets.statuscode.Enabled_0_Enabled_1;
            }
            else if (nodeItem.ComponentType == ComponentType.Workflow
               && nodeItem.Workflow.HasValue
            )
            {
                referenceToChangeState = new EntityReference(Workflow.EntityLogicalName, nodeItem.Workflow.Value);

                state = nodeItem.IsActive ? (int)Workflow.Schema.OptionSets.statecode.Draft_0 : (int)Workflow.Schema.OptionSets.statecode.Activated_1;
                status = nodeItem.IsActive ? (int)Workflow.Schema.OptionSets.statuscode.Draft_0_Draft_1 : (int)Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2;
            }

            if (referenceToChangeState != null
                && state.HasValue
                && status.HasValue
            )
            {
                var service = await GetService();

                try
                {
                    await service.ExecuteAsync(new Microsoft.Crm.Sdk.Messages.SetStateRequest()
                    {
                        EntityMoniker = referenceToChangeState,
                        State = new OptionSetValue(state.Value),
                        Status = new OptionSetValue(status.Value),
                    });

                    nodeItem.IsActive = !nodeItem.IsActive;

                    nodeItem.CorrectImage();
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }
        }

        private async void mIDeleteSdkObject_Click(object sender, RoutedEventArgs e)
        {
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null)
            {
                return;
            }

            EntityReference referenceToDelete = null;

            if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStepImage
                && nodeItem.StepImage.HasValue
            )
            {
                referenceToDelete = new EntityReference(SdkMessageProcessingStepImage.EntityLogicalName, nodeItem.StepImage.Value);
            }
            else if (nodeItem.ComponentType == ComponentType.SdkMessageProcessingStep
                && nodeItem.Step.HasValue
            )
            {
                referenceToDelete = new EntityReference(SdkMessageProcessingStep.EntityLogicalName, nodeItem.Step.Value);
            }
            else if (nodeItem.ComponentType == ComponentType.PluginType
                && nodeItem.PluginType.HasValue
            )
            {
                referenceToDelete = new EntityReference(PluginType.EntityLogicalName, nodeItem.PluginType.Value);
            }
            else if (nodeItem.ComponentType == ComponentType.PluginAssembly
               && nodeItem.PluginAssembly.HasValue
            )
            {
                referenceToDelete = new EntityReference(PluginAssembly.EntityLogicalName, nodeItem.PluginAssembly.Value);
            }
            else if (nodeItem.ComponentType == ComponentType.Workflow
               && nodeItem.Workflow.HasValue
            )
            {
                referenceToDelete = new EntityReference(Workflow.EntityLogicalName, nodeItem.Workflow.Value);
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
            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as PluginTreeViewItem;

            if (nodeItem == null || !nodeItem.PluginAssembly.HasValue)
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

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ComparingPluginAssemblyWithLocalAssemblyFormat1, nodeItem.Name);

            var controller = new PluginTypeDescriptionController(_iWriteToOutput);

            string filePath = await controller.CreateFileWithAssemblyComparing(_commonConfig.FolderForExport, service, nodeItem.PluginAssembly.Value, nodeItem.Name, null);

            this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingPluginAssemblyWithLocalAssemblyCompletedFormat1, nodeItem.Name);
        }

        private async void tSBRegisterAssembly_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var pluginAssembly = new PluginAssembly()
            {
            };

            var form = new WindowPluginAssembly(_iWriteToOutput, service, pluginAssembly, null, null);

            if (form.ShowDialog().GetValueOrDefault())
            {
                ShowExistingPlugins();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}