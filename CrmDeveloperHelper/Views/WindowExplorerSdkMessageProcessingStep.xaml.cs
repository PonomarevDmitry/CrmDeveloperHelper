using Microsoft.Xrm.Sdk;
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
    public partial class WindowExplorerSdkMessageProcessingStep : WindowWithMessageFilters
    {
        private readonly ObservableCollection<SdkMessageProcessingStepViewItem> _itemsSource = new ObservableCollection<SdkMessageProcessingStepViewItem>();

        public WindowExplorerSdkMessageProcessingStep(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string entityFilter
            , string pluginTypeFilter
            , string messageFilter
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            cmBEntityName.Text = entityFilter;
            txtBPluginTypeFilter.Text = pluginTypeFilter;
            txtBMessageFilter.Text = messageFilter;

            LoadFromConfig();

            FillEntityNames(service.ConnectionData);

            cmBStatusCode.ItemsSource = new EnumBindingSourceExtension(typeof(SdkMessageProcessingStep.Schema.OptionSets.statuscode?)).ProvideValue(null) as IEnumerable;

            LoadConfiguration();

            FocusOnComboBoxTextBox(cmBEntityName);

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            lstVwPluginSteps.ItemsSource = _itemsSource;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingPluginSteps();
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

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterExplorer_Click, "mIOpenMessageFilterExplorer");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miPluginTree_Click, "mIOpenPluginTree");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageFilterTree_Click, "mIOpenMessageFilterTree");

                AddMenuItemClickHandler(listContextMenu, explorersHelper.miMessageRequestTree_Click, "mIOpenMessageRequestTree");
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
            base.OnClosed(e);

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;
        }

        private const string paramEntityName = "EntityName";
        private const string paramMessage = "Message";
        private const string paramPluginTypeName = "PluginTypeName";

        private const string paramPreValidationStage = "PreValidationStage";
        private const string paramPreStage = "PreStage";
        private const string paramPostSynchStage = "PostSynchStage";
        private const string paramPostAsynchStage = "PostAsynchStage";
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

            var statusValue = winConfig.GetValueInt(paramStatusCode);
            if (statusValue != -1)
            {
                var item = cmBStatusCode.Items.OfType<SdkMessageProcessingStep.Schema.OptionSets.statuscode?>().FirstOrDefault(e => (int)e == statusValue);
                if (item != null)
                {
                    cmBStatusCode.SelectedItem = item;
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

            var statusValue = -1;

            {
                if (cmBStatusCode.SelectedItem is SdkMessageProcessingStep.Schema.OptionSets.statuscode comboBoxItem)
                {
                    statusValue = (int)comboBoxItem;
                }
            }

            winConfig.DictInt[paramStatusCode] = statusValue;
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingPluginSteps();
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

        private async Task ShowExistingPluginSteps()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingPlugins);

            string entityNameFilter = string.Empty;
            string messageNameFilter = string.Empty;
            string pluginTypeNameFilter = string.Empty;

            SdkMessageProcessingStep.Schema.OptionSets.statuscode? statuscode = null;
            List<PluginStage> stages = null;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSource.Clear();

                entityNameFilter = cmBEntityName.Text?.Trim();
                messageNameFilter = txtBMessageFilter.Text.Trim();
                pluginTypeNameFilter = txtBPluginTypeFilter.Text.Trim();

                if (cmBStatusCode.SelectedItem is SdkMessageProcessingStep.Schema.OptionSets.statuscode comboBoxItem)
                {
                    statuscode = comboBoxItem;
                }

                stages = GetStages();
            });

            IEnumerable<SdkMessageProcessingStep> listSteps = Enumerable.Empty<SdkMessageProcessingStep>();

            try
            {
                if (service != null)
                {
                    var repository = new SdkMessageProcessingStepRepository(service);

                    listSteps = await repository.FindSdkMessageProcessingStepWithEntityNameAsync(entityNameFilter, stages, pluginTypeNameFilter, messageNameFilter, statuscode);

                    base.StartGettingSdkMessageFilters(service);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                ToggleControls(service.ConnectionData, true, string.Empty);
            }

            this.lstVwPluginSteps.Dispatcher.Invoke(() =>
            {
                foreach (var entity in listSteps
                    .OrderBy(ent => ent.PrimaryObjectTypeCodeName)
                    .ThenBy(ent => ent.SdkMessageId?.Name, MessageComparer.Comparer)
                    .ThenBy(ent => ent.StageEnum)
                    .ThenBy(ent => ent.ModeEnum)
                    .ThenBy(ent => ent.Rank)
                    .ThenBy(ent => ent.EventHandler?.Name)
                    .ThenBy(ent => ent.Name)
                )
                {
                    var item = new SdkMessageProcessingStepViewItem(entity);

                    _itemsSource.Add(item);
                }

                if (this.lstVwPluginSteps.Items.Count == 1)
                {
                    this.lstVwPluginSteps.SelectedItem = this.lstVwPluginSteps.Items[0];
                }
            });

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingPluginsCompleted);
        }

        private class SdkMessageProcessingStepViewItem
        {
            public string MessageName => SdkMessageProcessingStep.SdkMessageId?.Name;

            public string MessageCategoryName => SdkMessageProcessingStep.MessageCategoryName;

            public string PrimaryObjectTypeCode => SdkMessageProcessingStep.PrimaryObjectTypeCodeName;

            public string SecondaryObjectTypeCode => SdkMessageProcessingStep.SecondaryObjectTypeCodeName;

            public string PluginTypeName => SdkMessageProcessingStep.EventHandler?.Name;

            public Guid? PluginTypeId => SdkMessageProcessingStep.EventHandler?.Id;

            public string PluginAssemblyName => SdkMessageProcessingStep.PluginAssemblyName;

            public Guid? PluginAssemblyId => SdkMessageProcessingStep.PluginAssemblyId;

            public string Stage { get; }

            public string Mode { get; }

            public int? Rank => SdkMessageProcessingStep.Rank;

            public string Name => SdkMessageProcessingStep.Name;

            public string Description => SdkMessageProcessingStep.Description;

            public bool HasDescription => !string.IsNullOrEmpty(SdkMessageProcessingStep.Description);

            public string StateCode { get; }

            public string StatusCode { get; }

            public SdkMessageProcessingStep SdkMessageProcessingStep { get; }

            public SdkMessageProcessingStepViewItem(SdkMessageProcessingStep step)
            {
                this.SdkMessageProcessingStep = step;

                if (step.StateCodeEnum.HasValue)
                {
                    this.StateCode = EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(step.StateCodeEnum.Value);
                }

                if (step.StatusCodeEnum.HasValue)
                {
                    this.StatusCode = EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(step.StatusCodeEnum.Value);
                }

                this.Stage = SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value);

                if (step.ModeEnum.HasValue)
                {
                    this.Mode = EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(step.ModeEnum.Value);
                }
            }
        }

        public List<PluginStage> GetStages()
        {
            var result = new List<PluginStage>();

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

        private static string GetImageTooltip(SdkMessageProcessingStepImage imageEntity)
        {
            var tooltipImage = new StringBuilder();

            if (!string.IsNullOrEmpty(imageEntity.Name))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendFormat("Name: {0}", imageEntity.Name);
            }

            if (!string.IsNullOrEmpty(imageEntity.Description))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendFormat("Description: {0}", imageEntity.Description);
            }

            if (!string.IsNullOrEmpty(imageEntity.MessagePropertyName))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendFormat("MessagePropertyName: {0}", imageEntity.MessagePropertyName);
            }

            if (!string.IsNullOrEmpty(imageEntity.RelatedAttributeName))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendFormat("RelatedAttributeName: {0}", imageEntity.RelatedAttributeName);
            }

            if (!string.IsNullOrEmpty(imageEntity.Attributes1))
            {
                if (tooltipImage.Length > 0)
                {
                    tooltipImage.AppendLine();
                }

                tooltipImage.AppendLine("Attributes:");

                foreach (string item in imageEntity.Attributes1Strings)
                {
                    tooltipImage.AppendLine().Append(item);
                }
            }

            if (tooltipImage.Length > 0)
            {
                return tooltipImage.ToString();
            }
            else
            {
                return null;
            }
        }

        private static string GetImageName(SdkMessageProcessingStepImage imageEntity)
        {
            StringBuilder nameImage = new StringBuilder();

            if (imageEntity.ImageType != null)
            {
                if (imageEntity.ImageType.Value == (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.PreImage_0)
                {
                    nameImage.Append("PreImage");
                }
                else if (imageEntity.ImageType.Value == (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.PostImage_1)
                {
                    nameImage.Append("PostImage");
                }
                else if (imageEntity.ImageType.Value == (int)SdkMessageProcessingStepImage.Schema.OptionSets.imagetype.Both_2)
                {
                    nameImage.Append("BothImage");
                }
            }

            if (!string.IsNullOrEmpty(imageEntity.EntityAlias))
            {
                if (nameImage.Length > 0) { nameImage.Append(", "); }

                nameImage.Append(imageEntity.EntityAlias);
            }

            if (!string.IsNullOrEmpty(imageEntity.Name))
            {
                if (nameImage.Length > 0) { nameImage.Append(", "); }

                nameImage.Append(imageEntity.Name);
            }

            if (nameImage.Length > 0) { nameImage.Append(", "); }

            if (!string.IsNullOrEmpty(imageEntity.Attributes1))
            {
                nameImage.AppendFormat("Attributes: {0}", imageEntity.Attributes1StringsSorted);
            }
            else
            {
                nameImage.Append("Attributes: All");
            }

            return nameImage.ToString();
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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, tSBRegisterAssembly);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwPluginSteps.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                        && this.lstVwPluginSteps.SelectedItem != null
                        && this.lstVwPluginSteps.SelectedItem is SdkMessageProcessingStepViewItem;

                    UIElement[] list = { tSBCreateDescription };

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

        private static string GetChangeStateName(SdkMessageProcessingStepViewItem item)
        {
            if (item == null)
            {
                return "ChangeState";
            }

            var action = (item.SdkMessageProcessingStep.StatusCodeEnum == SdkMessageProcessingStep.Schema.OptionSets.statuscode.Enabled_0_Enabled_1)
                ? "Deactivate" : "Activate";

            return $"{action} Step";
        }

        private async void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingPluginSteps();
            }
        }

        private async void cmBStatusCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowExistingPluginSteps();
        }

        private SdkMessageProcessingStepViewItem GetSelectedEntity()
        {
            return this.lstVwPluginSteps.SelectedItems.OfType<SdkMessageProcessingStepViewItem>().Count() == 1
                ? this.lstVwPluginSteps.SelectedItems.OfType<SdkMessageProcessingStepViewItem>().SingleOrDefault() : null;
        }

        private List<SdkMessageProcessingStepViewItem> GetSelectedEntitiesList()
        {
            return this.lstVwPluginSteps.SelectedItems.OfType<SdkMessageProcessingStepViewItem>().ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwPluginSteps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
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

        private async Task CreateDescription(SdkMessageProcessingStepViewItem node)
        {
            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.CreatingFileWithDescriptionFormat1, service.ConnectionData.Name);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingDescription);

            StringBuilder result = new StringBuilder();

            string fileName = string.Empty;

            bool appendConnectionInfo = true;

            var repository = new SdkMessageProcessingStepRepository(service);
            var step = await repository.GetStepByIdAsync(node.SdkMessageProcessingStep.Id);

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
                string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(step, service.ConnectionData);

                if (!string.IsNullOrEmpty(desc))
                {
                    if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

                    result.AppendLine(desc);
                }
            }

            //if (node.StepImageId.HasValue && node.ComponentType == ComponentType.SdkMessageProcessingStepImage)
            //{
            //    var repository = new SdkMessageProcessingStepImageRepository(service);
            //    SdkMessageProcessingStepImage stepImage = await repository.GetStepImageByIdAsync(node.StepImageId.Value);

            //    if (string.IsNullOrEmpty(fileName))
            //    {
            //        fileName = EntityFileNameFormatter.GetPluginImageFileName(service.ConnectionData.Name, stepImage.Name, "Description");
            //    }

            //    if (appendConnectionInfo)
            //    {
            //        if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }
            //        result.AppendLine(service.ConnectionData.GetConnectionInfo());

            //        appendConnectionInfo = false;
            //    }

            //    {
            //        var desc = PluginTypeDescriptionHandler.GetImageDescription(null, stepImage);

            //        if (!string.IsNullOrEmpty(desc))
            //        {
            //            if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

            //            result.AppendLine(desc);
            //        }
            //    }

            //    {
            //        string desc = await EntityDescriptionHandler.GetEntityDescriptionAsync(stepImage, service.ConnectionData);

            //        if (!string.IsNullOrEmpty(desc))
            //        {
            //            if (result.Length > 0) { result.AppendLine().AppendLine().AppendLine(); }

            //            result.AppendLine(desc);
            //        }
            //    }
            //}

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

                await ShowExistingPluginSteps();
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

            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            var items = contextMenu.Items.OfType<Control>();

            bool isEntity = nodeItem.PrimaryObjectTypeCode.IsValidEntityName();

            bool isPluginAssembly = nodeItem.PluginAssemblyId.HasValue;
            bool isStep = true;

            //bool isStepImage = nodeItem.StepId.HasValue && nodeItem.ComponentType == ComponentType.SdkMessageProcessingStepImage;

            ConnectionData connectionData = GetSelectedConnection();

            ActivateControls(items, isPluginAssembly, "contMnAddPluginAssemblyToSolution", "contMnAddPluginAssemblyToSolutionLast");
            FillLastSolutionItems(connectionData, items, isPluginAssembly, AddAssemblyToCrmSolutionLast_Click, "contMnAddPluginAssemblyToSolutionLast");

            ActivateControls(items, isStep, "contMnAddToSolution", "contMnAddToSolutionLast");
            FillLastSolutionItems(connectionData, items, isStep, AddStepToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            ActivateControls(items, isPluginAssembly, "contMnAddPluginTypeStepsToSolution", "contMnAddPluginTypeStepsToSolutionLast");
            FillLastSolutionItems(connectionData, items, isPluginAssembly, mIAddPluginTypeStepsToSolutionLast_Click, "contMnAddPluginTypeStepsToSolutionLast");

            ActivateControls(items, isPluginAssembly, "contMnAddPluginAssemblyStepsToSolution", "contMnAddPluginAssemblyStepsToSolutionLast", "contMnCompareWithLocalAssembly");
            FillLastSolutionItems(connectionData, items, isPluginAssembly, mIAddAssemblyStepsToSolutionLast_Click, "contMnAddPluginAssemblyStepsToSolutionLast");

            ActivateControls(items, isEntity, "contMnEntity");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastDoNotIncludeSubcomponents");
            FillLastSolutionItems(connectionData, items, isEntity, AddEntityToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityToSolutionLastIncludeAsShellOnly");
            ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityToSolutionLast");

            var selectedSteps = GetSelectedEntitiesList();

            var hasEnabledSteps = selectedSteps.Any(s => s.SdkMessageProcessingStep.StatusCodeEnum == SdkMessageProcessingStep.Schema.OptionSets.statuscode.Enabled_0_Enabled_1);
            var hasDisabledSteps = selectedSteps.Any(s => s.SdkMessageProcessingStep.StatusCodeEnum == SdkMessageProcessingStep.Schema.OptionSets.statuscode.Disabled_1_Disabled_2);

            ActivateControls(items, hasDisabledSteps, "miActivateSteps");
            ActivateControls(items, hasEnabledSteps, "miDeactivateSteps");

            CheckSeparatorVisible(items);
        }

        private async void mICreateDescription_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            await CreateDescription(nodeItem);
        }

        #region Entity Handlers

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null
                || !nodeItem.PrimaryObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(nodeItem.PrimaryObjectTypeCode);
            }
        }

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.PrimaryObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                this._iWriteToOutput.OpenFetchXmlFile(connectionData, _commonConfig, entity.PrimaryObjectTypeCode);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null
                || !nodeItem.PrimaryObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(nodeItem.PrimaryObjectTypeCode);
            }
        }

        private async void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null
                || !nodeItem.PrimaryObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            var entityName = nodeItem.PrimaryObjectTypeCode;

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
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null
                || !nodeItem.PrimaryObjectTypeCode.IsValidEntityName()
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

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.PrimaryObjectTypeCode);

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
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null
                || !nodeItem.PrimaryObjectTypeCode.IsValidEntityName()
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

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.PrimaryObjectTypeCode);

            if (!idMetadata.HasValue)
            {
                return;
            }

            connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Entity, idMetadata.Value);
        }

        private async void miOpenEntityDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null
                || !nodeItem.PrimaryObjectTypeCode.IsValidEntityName()
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

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.PrimaryObjectTypeCode);

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
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null
                || !nodeItem.PrimaryObjectTypeCode.IsValidEntityName()
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

            var idMetadata = connectionData.GetEntityMetadataId(nodeItem.PrimaryObjectTypeCode);

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

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SdkMessageProcessingStep, nodeItem.SdkMessageProcessingStep.Id);
        }

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            _commonConfig.Save();

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.SdkMessageProcessingStep, nodeItem.SdkMessageProcessingStep.Id, null);
        }

        private async void miOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.SdkMessageProcessingStep
                , nodeItem.SdkMessageProcessingStep.Id
                , null
            );
        }

        private async void AddStepToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddStepToSolution(true, null);
        }

        private async void AddStepToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
            )
            {
                await AddStepToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddStepToSolution(bool withSelect, string solutionUniqueName)
        {
            var stepList = GetSelectedEntitiesList()
                .Select(s => s.SdkMessageProcessingStep.Id)
                .ToList();

            if (!stepList.Any())
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

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.SdkMessageProcessingStep, stepList, null, withSelect);
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
            var pluginTypesList = GetSelectedEntitiesList()
                .Where(s => s.PluginTypeId.HasValue)
                .Select(s => s.PluginTypeId.Value)
                .Distinct()
                .ToList();

            if (!pluginTypesList.Any())
            {
                return;
            }

            var service = await GetService();

            var repository = new SdkMessageProcessingStepRepository(service);

            var steps = new List<SdkMessageProcessingStep>();

            foreach (var idPluginType in pluginTypesList)
            {
                steps.AddRange(await repository.GetAllStepsByPluginTypeAsync(idPluginType));
            }

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
            var assembliesList = GetSelectedEntitiesList()
                .Where(s => s.PluginAssemblyId.HasValue)
                .Select(s => s.PluginAssemblyId.Value)
                .Distinct()
                .ToList();

            if (!assembliesList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.PluginAssembly, assembliesList, null, withSelect);
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
            var pluginAssembliesList = GetSelectedEntitiesList()
                .Where(s => s.PluginAssemblyId.HasValue)
                .Select(s => s.PluginAssemblyId.Value)
                .Distinct()
                .ToList();

            if (!pluginAssembliesList.Any())
            {
                return;
            }

            var service = await GetService();

            var repository = new SdkMessageProcessingStepRepository(service);

            var steps = new List<SdkMessageProcessingStep>();

            foreach (var idPluginAssembly in pluginAssembliesList)
            {
                steps.AddRange(await repository.GetAllStepsByPluginAssemblyAsync(idPluginAssembly));
            }

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
            SdkMessageProcessingStepViewItem nodeStep = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            await ExecuteAddingNewPluginStep(nodeStep);
        }

        private async Task ExecuteAddingNewPluginStep(SdkMessageProcessingStepViewItem nodeStep)
        {
            var service = await GetService();

            var step = new SdkMessageProcessingStep()
            {
                EventHandler = nodeStep.SdkMessageProcessingStep.EventHandler,
            };

            List<SdkMessageFilter> filters = await GetSdkMessageFiltersAsync(service);

            var form = new WindowSdkMessageProcessingStep(_iWriteToOutput, service, filters, step);

            if (form.ShowDialog().GetValueOrDefault())
            {
                await ShowExistingPluginSteps();
            }
        }

        private async void mIAddPluginStepImage_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeStep = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeStep == null
                || !nodeStep.PrimaryObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            await ExecuteAddingPluginStepImage(nodeStep);
        }

        private async Task ExecuteAddingPluginStepImage(SdkMessageProcessingStepViewItem nodeStep)
        {
            var service = await GetService();

            var image = new SdkMessageProcessingStepImage()
            {
                SdkMessageProcessingStepId = nodeStep.SdkMessageProcessingStep.ToEntityReference(),
            };

            var form = new WindowSdkMessageProcessingStepImage(_iWriteToOutput, service, image, nodeStep.PrimaryObjectTypeCode, nodeStep.MessageName);

            if (form.ShowDialog().GetValueOrDefault())
            {
                //var repositoryImage = new SdkMessageProcessingStepImageRepository(service);

                //image = await repositoryImage.GetStepImageByIdAsync(form.Image.Id);

                //this.lstVwPluginSteps.Dispatcher.Invoke(() =>
                //{
                //    SdkMessageProcessingStepViewItem nodeImage = new SdkMessageProcessingStepViewItem(ComponentType.SdkMessageProcessingStepImage);

                //    FillNodeImageInformation(nodeImage, image, nodeStep.EntityLogicalName, nodeStep.MessageName, nodeStep.PluginTypeId, nodeStep.PluginTypeName, nodeStep.PluginAssemblyId, nodeStep.PluginAssemblyName);

                //    nodeStep.Items.Add(nodeImage);
                //    nodeImage.Parent = nodeStep;

                //    nodeStep.IsExpanded = true;

                //    nodeImage.IsSelected = true;
                //    nodeImage.IsExpanded = true;
                //});
            }
        }

        private async void mIUpdateSdkMessageProcessingStep_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            await ExecuteUpdateSdkMessageProcessingStep(nodeItem);
        }

        private async Task ExecuteUpdateSdkMessageProcessingStep(SdkMessageProcessingStepViewItem nodeItem)
        {
            var service = await GetService();

            List<SdkMessageFilter> filters = await GetSdkMessageFiltersAsync(service);

            var repositoryStep = new SdkMessageProcessingStepRepository(service);

            var step = await repositoryStep.GetStepByIdAsync(nodeItem.SdkMessageProcessingStep.Id);

            var form = new WindowSdkMessageProcessingStep(_iWriteToOutput, service, filters, step);

            if (form.ShowDialog().GetValueOrDefault())
            {
                await ShowExistingPluginSteps();
            }
        }

        private async void mIEditInEditor_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, nodeItem.SdkMessageProcessingStep.LogicalName, nodeItem.SdkMessageProcessingStep.Id);
        }


        private async void miActivateSteps_Click(object sender, RoutedEventArgs e)
        {
            await ChangeStepsState(SdkMessageProcessingStep.Schema.OptionSets.statecode.Enabled_0, SdkMessageProcessingStep.Schema.OptionSets.statuscode.Enabled_0_Enabled_1);
        }

        private async void miDeactivateSteps_Click(object sender, RoutedEventArgs e)
        {
            await ChangeStepsState(SdkMessageProcessingStep.Schema.OptionSets.statecode.Disabled_1, SdkMessageProcessingStep.Schema.OptionSets.statuscode.Disabled_1_Disabled_2);
        }

        private async Task ChangeStepsState(SdkMessageProcessingStep.Schema.OptionSets.statecode stateCode, SdkMessageProcessingStep.Schema.OptionSets.statuscode statusCode)
        {
            var selectedSteps = GetSelectedEntitiesList()
                .Where(e => e.SdkMessageProcessingStep.StatusCodeEnum != statusCode)
                .ToList();

            if (!selectedSteps.Any())
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ChangingEntityStateFormat1, SdkMessageProcessingStep.EntityLogicalName);

            foreach (var step in selectedSteps)
            {
                try
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.ChangingEntityStateFormat1, step.Name);

                    await service.ExecuteAsync<Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest()
                    {
                        EntityMoniker = step.SdkMessageProcessingStep.ToEntityReference(),
                        State = new OptionSetValue((int)stateCode),
                        Status = new OptionSetValue((int)statusCode),
                    });
                }
                catch (Exception ex)
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.ChangingEntityStateFailedFormat1, SdkMessageProcessingStep.EntityLogicalName);

                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateCompletedFormat1, SdkMessageProcessingStep.EntityLogicalName);

            await ShowExistingPluginSteps();
        }

        private async void mIDeleteSdkMessageProcessingStep_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            await TryDeleteSdkMessageProcessingStep(nodeItem);
        }

        private async Task TryDeleteSdkMessageProcessingStep(SdkMessageProcessingStepViewItem nodeItem)
        {
            EntityReference referenceToDelete = nodeItem.SdkMessageProcessingStep.ToEntityReference();

            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, SdkMessageProcessingStep.EntityLogicalName, nodeItem.Name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                try
                {
                    await service.DeleteAsync(referenceToDelete.LogicalName, referenceToDelete.Id);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                await ShowExistingPluginSteps();
            }
        }

        private async void mICompareWithLocalAssembly_Click(object sender, RoutedEventArgs e)
        {
            SdkMessageProcessingStepViewItem nodeItem = GetItemFromRoutedDataContext<SdkMessageProcessingStepViewItem>(e);

            if (nodeItem == null || !nodeItem.PluginAssemblyId.HasValue)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingPluginAssemblyWithLocalAssemblyFormat1, nodeItem.Name);

            var controller = new PluginController(_iWriteToOutput);

            string filePath = await controller.SelecteFileCreateFileWithAssemblyComparing(_commonConfig.FolderForExport, service, nodeItem.PluginAssemblyId.Value, nodeItem.Name, null);

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
                await ShowExistingPluginSteps();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private void lstVwPluginSteps_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private async void lstVwPluginStepsDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (lstVwPluginSteps.SelectedItem != null && lstVwPluginSteps.SelectedItem is SdkMessageProcessingStepViewItem nodeItem)
            {
                e.Handled = true;
                await TryDeleteSdkMessageProcessingStep(nodeItem);
            }
        }

        private async void lstVwPluginStepsNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (lstVwPluginSteps.SelectedItem != null && lstVwPluginSteps.SelectedItem is SdkMessageProcessingStepViewItem nodeItem)
            {
                e.Handled = true;

                await ExecuteAddingPluginStepImage(nodeItem);
            }
        }

        private async void lstVwPluginStepsOpenProperties_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (lstVwPluginSteps.SelectedItem != null && lstVwPluginSteps.SelectedItem is SdkMessageProcessingStepViewItem nodeItem)
            {
                e.Handled = true;
                await ExecuteUpdateSdkMessageProcessingStep(nodeItem);
            }
        }

        private async void lstVwPluginSteps_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var nodeItem = GetSelectedEntity();

            if (nodeItem == null)
            {
                return;
            }

            await ExecuteUpdateSdkMessageProcessingStep(nodeItem);
        }
    }
}