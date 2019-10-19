using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSdkMessageProcessingStep : WindowBase
    {
        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;

        private const string _impersonatingUserNone = "<Calling User>";

        public SdkMessageProcessingStep Step { get; private set; }

        private readonly List<SdkMessageFilter> _filters;

        private EntityReference _impersonatingUser = null;

        private string _eventHandlerName = null;

        public WindowSdkMessageProcessingStep(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , List<SdkMessageFilter> filters
            , SdkMessageProcessingStep step
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._filters = filters;

            this.Step = step;

            InitializeComponent();

            LoadFromConfig();

            tSSLblConnectionName.Content = _service.ConnectionData.Name;

            FillComboBoxMessages();

            LoadEntityStepProperties();

            this.DecreaseInit();

            FocusOnComboBoxTextBox(cmBMessageName);
        }

        private void LoadFromConfig()
        {
            WindowSettings winConfig = GetWindowsSettings();

            LoadFormSettings(winConfig);
        }

        protected override void LoadConfigurationInternal(WindowSettings winConfig)
        {
            base.LoadConfigurationInternal(winConfig);

            LoadFormSettings(winConfig);
        }

        private const string paramColumnGeneralInfoWidth = "ColumnGeneralInfoWidth";

        private void LoadFormSettings(WindowSettings winConfig)
        {
            if (winConfig.DictDouble.ContainsKey(paramColumnGeneralInfoWidth))
            {
                columnGeneralInfo.Width = new GridLength(winConfig.DictDouble[paramColumnGeneralInfoWidth]);
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictDouble[paramColumnGeneralInfoWidth] = columnGeneralInfo.Width.Value;
        }

        private void cmBMessageName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmBPrimaryEntity.Dispatcher.Invoke(() =>
            {
                var primaryText = cmBPrimaryEntity.Text;
                cmBPrimaryEntity.Items.Clear();
                cmBPrimaryEntity.Text = primaryText;
            });

            cmBSecondaryEntity.Dispatcher.Invoke(() =>
            {
                var secondaryText = cmBSecondaryEntity.Text;
                cmBSecondaryEntity.Items.Clear();
                cmBSecondaryEntity.Text = secondaryText;
            });

            this.Dispatcher.Invoke(() =>
            {
                btnSetAllAttributes.IsEnabled
                    = btnSelectAttributes.IsEnabled
                    = string.Equals(cmBMessageName.SelectedItem?.ToString(), "Update", StringComparison.InvariantCultureIgnoreCase);
            });

            if (cmBMessageName.SelectedItem == null)
            {
                return;
            }

            string selectedMessage = cmBMessageName.SelectedItem?.ToString();

            var messageFilters = _filters.AsEnumerable();

            if (!string.IsNullOrEmpty(selectedMessage))
            {
                messageFilters = messageFilters.Where(f => string.Equals(f.SdkMessageId.Name, selectedMessage, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!messageFilters.Any())
            {
                return;
            }

            {
                var hash = new HashSet<string>(messageFilters.Select(f => f.PrimaryObjectTypeCode), StringComparer.InvariantCultureIgnoreCase);

                cmBPrimaryEntity.Dispatcher.Invoke(() =>
                {
                    var primaryText = cmBPrimaryEntity.Text;

                    foreach (var entityName in hash.OrderBy(s => s))
                    {
                        cmBPrimaryEntity.Items.Add(entityName);
                    }

                    cmBPrimaryEntity.Text = primaryText;
                });
            }

            {
                var hash = new HashSet<string>(messageFilters.Select(f => f.SecondaryObjectTypeCode), StringComparer.InvariantCultureIgnoreCase);

                cmBSecondaryEntity.Dispatcher.Invoke(() =>
                {
                    var secondaryText = cmBSecondaryEntity.Text;

                    foreach (var entityName in hash.OrderBy(s => s))
                    {
                        cmBSecondaryEntity.Items.Add(entityName);
                    }

                    cmBSecondaryEntity.Text = secondaryText;
                });
            }
        }

        private void FillComboBoxMessages()
        {
            var hash = new HashSet<string>(this._filters.Select(f => f.SdkMessageId.Name), StringComparer.InvariantCultureIgnoreCase);

            var text = cmBMessageName.Text;

            cmBMessageName.Items.Clear();

            foreach (var message in hash.OrderBy(s => s))
            {
                cmBMessageName.Items.Add(message);
            }

            cmBMessageName.Text = text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadEntityStepProperties()
        {
            if (Step.EventHandler != null)
            {
                var nameColumn = PluginType.Schema.Attributes.name;

                if (string.Equals(Step.EventHandler.LogicalName, PluginType.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
                {
                    nameColumn = PluginType.Schema.Attributes.typename;
                }
                else if (string.Equals(Step.EventHandler.LogicalName, ServiceEndpoint.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
                {
                    nameColumn = ServiceEndpoint.Schema.Attributes.name;
                }

                var eventHandlerEntity = _service.RetrieveByQuery<Entity>(Step.EventHandler.LogicalName, Step.EventHandler.Id, new ColumnSet(nameColumn));

                if (eventHandlerEntity.Attributes.ContainsKey(nameColumn)
                    && eventHandlerEntity.Attributes[nameColumn] != null
                    && eventHandlerEntity.Attributes[nameColumn] is string name
                    && !string.IsNullOrEmpty(name)
                )
                {
                    _eventHandlerName = name;
                }
            }

            txtBEventHandler.Text = _eventHandlerName;

            cmBMessageName.Text = Step.SdkMessageId?.Name;
            if (cmBMessageName.Items.Contains(cmBMessageName.Text))
            {
                cmBMessageName.SelectedItem = cmBMessageName.Text;
            }

            cmBPrimaryEntity.Text = Step.PrimaryObjectTypeCodeName;
            if (cmBPrimaryEntity.Items.Contains(cmBPrimaryEntity.Text))
            {
                cmBPrimaryEntity.SelectedItem = cmBPrimaryEntity.Text;
            }

            cmBSecondaryEntity.Text = Step.SecondaryObjectTypeCodeName;
            if (cmBSecondaryEntity.Items.Contains(cmBSecondaryEntity.Text))
            {
                cmBSecondaryEntity.SelectedItem = cmBSecondaryEntity.Text;
            }

            txtBName.Text = Step.Name;

            if (Step.Rank.HasValue)
            {
                txtBExecutionOrder.Text = Step.Rank.ToString();
            }
            else
            {
                txtBExecutionOrder.Text = "1";
            }

            txtBDescription.Text = Step.Description;
            txtBUnSecureConfiguration.Text = Step.Configuration;

            txtBFilteringBAttributes.Text = Step.FilteringAttributesStringsSorted;
            txtBFilteringBAttributes.ToolTip = string.Join(System.Environment.NewLine, Step.FilteringAttributesStrings);

            if (Step.Stage?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40)
            {
                rBPostOperation.IsChecked = true;
            }
            else if (Step.Stage?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20)
            {
                rBPreOperation.IsChecked = true;
            }
            else if (Step.Stage?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10)
            {
                rBPreValidation.IsChecked = true;
            }
            else
            {
                rBPostOperation.IsChecked = true;
            }

            if (Step.Mode?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Synchronous_0)
            {
                rBSync.IsChecked = true;
            }
            else if (Step.Mode?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1)
            {
                rBAsync.IsChecked = true;
            }
            else
            {
                rBSync.IsChecked = true;
            }

            if (Step.SupportedDeployment?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Server_Only_0)
            {
                rBServer.IsChecked = true;
            }
            else if (Step.SupportedDeployment?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Microsoft_Dynamics_365_Client_for_Outlook_Only_1)
            {
                rBOffline.IsChecked = true;
            }
            else if (Step.SupportedDeployment?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Both_2)
            {
                rBDeploymentBoth.IsChecked = true;
            }
            else
            {
                rBServer.IsChecked = true;
            }

#pragma warning disable CS0612 // Type or member is obsolete
            if (Step.InvocationSource?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.invocationsource.Parent_0)
#pragma warning restore CS0612 // Type or member is obsolete
            {
                rBParent.IsChecked = true;
            }
#pragma warning disable CS0612 // Type or member is obsolete
            else if (Step.InvocationSource?.Value == (int)SdkMessageProcessingStep.Schema.OptionSets.invocationsource.Child_1)
#pragma warning restore CS0612 // Type or member is obsolete
            {
                rBChild.IsChecked = true;
            }
            else
            {
                rBParent.IsChecked = true;
            }

            chBDeleteAsyncOperationIfSuccessful.IsChecked = Step.AsyncAutoDelete.GetValueOrDefault();

            if (Step.SdkMessageProcessingStepSecureConfigId != null)
            {
                var config = _service.RetrieveByQuery<SdkMessageProcessingStepSecureConfig>(Step.SdkMessageProcessingStepSecureConfigId.LogicalName, Step.SdkMessageProcessingStepSecureConfigId.Id, new ColumnSet(SdkMessageProcessingStepSecureConfig.Schema.Attributes.secureconfig));

                if (config != null)
                {
                    txtBSecureConfiguration.Text = config.SecureConfig;
                }
            }

            this._impersonatingUser = Step.ImpersonatingUserId;

            if (Step.ImpersonatingUserId != null)
            {
                if (!string.IsNullOrEmpty(Step.ImpersonatingUserId.Name))
                {
                    txtBRunInUserContext.Text = Step.ImpersonatingUserId.Name;
                }
                else
                {
                    txtBRunInUserContext.Text = Step.ImpersonatingUserId.Id.ToString();
                }
            }
            else
            {
                txtBRunInUserContext.Text = _impersonatingUserNone;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await PerformSaveClick();
        }

        public int GetDeployment()
        {
            if (rBDeploymentBoth.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Both_2;
            }
            else if (rBServer.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Server_Only_0;
            }
            else if (rBOffline.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Microsoft_Dynamics_365_Client_for_Outlook_Only_1;
            }

            return (int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Server_Only_0;
        }

        public int GetMode()
        {
            if (rBSync.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Synchronous_0;
            }
            else if (rBAsync.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1;
            }

            return (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Synchronous_0;
        }

        public int GetStage()
        {
            if (rBPreValidation.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10;
            }
            else if (rBPreOperation.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20;
            }
            else if (rBPostOperation.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40;
            }

            return (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40;
        }

        public int GetInvocationSource()
        {
            if (rBParent.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.invocationsource.Parent_0;
            }
            else if (rBChild.IsChecked.GetValueOrDefault())
            {
                return (int)SdkMessageProcessingStep.Schema.OptionSets.invocationsource.Child_1;
            }

            return (int)SdkMessageProcessingStep.Schema.OptionSets.invocationsource.Parent_0;
        }

        private async Task PerformSaveClick()
        {
            string messageName = cmBMessageName.Text?.Trim();
            string primaryEntity = cmBPrimaryEntity.Text?.Trim();
            string secondaryEntity = cmBSecondaryEntity.Text?.Trim();

            if (string.IsNullOrEmpty(messageName))
            {
                MessageBox.Show(Properties.MessageBoxStrings.MessageNameIsEmpty, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtBExecutionOrder.Text.Trim(), out int rank))
            {
                string text = string.Format(Properties.MessageBoxStrings.TextCannotBeParsedToIntFormat1, txtBExecutionOrder.Text);
                MessageBox.Show(text, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(primaryEntity))
            {
                primaryEntity = "none";
            }
            if (string.IsNullOrEmpty(secondaryEntity))
            {
                secondaryEntity = "none";
            }

            var messageFilterCount = _filters.Count(f =>
                string.Equals(f.SdkMessageId.Name, messageName, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(f.PrimaryObjectTypeCode, primaryEntity, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(f.SecondaryObjectTypeCode, secondaryEntity, StringComparison.InvariantCultureIgnoreCase)
            );

            if (messageFilterCount != 1)
            {
                string text = string.Format(Properties.MessageBoxStrings.MessageFilterNotFindedForMessageNameAndEntitiesFormat3, messageName, primaryEntity, secondaryEntity);
                MessageBox.Show(text, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var messageFilter = _filters.Single(f =>
                string.Equals(f.SdkMessageId.Name, messageName, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(f.PrimaryObjectTypeCode, primaryEntity, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(f.SecondaryObjectTypeCode, secondaryEntity, StringComparison.InvariantCultureIgnoreCase)
            );

            this.Step.Name = txtBName.Text.Trim();
            this.Step.Description = txtBDescription.Text.Trim();

            this.Step.SdkMessageFilterId = messageFilter.ToEntityReference();
            this.Step.SdkMessageId = messageFilter.SdkMessageId;

            this.Step.Rank = rank;
            this.Step.Configuration = txtBUnSecureConfiguration.Text.Trim();

            var stage = GetStage();
            var invocationsource = GetInvocationSource();

            this.Step.Stage = new OptionSetValue(stage);

            if (stage == (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40)
            {
                var mode = GetMode();

                this.Step.Mode = new OptionSetValue(mode);

                if (mode == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1)
                {
                    this.Step.AsyncAutoDelete = chBDeleteAsyncOperationIfSuccessful.IsChecked.GetValueOrDefault();
                }
                else
                {
                    this.Step.AsyncAutoDelete = false;
                }
            }
            else
            {
                this.Step.Mode = new OptionSetValue((int)SdkMessageProcessingStep.Schema.OptionSets.mode.Synchronous_0);
                this.Step.AsyncAutoDelete = false;
            }

            if (!messageFilter.Availability.HasValue || messageFilter.Availability == (int)SdkMessageFilter.Schema.OptionSets.availability.Both)
            {
                var supporteddeployment = GetDeployment();

                this.Step.SupportedDeployment = new OptionSetValue(supporteddeployment);
            }
            else if (messageFilter.Availability == (int)SdkMessageFilter.Schema.OptionSets.availability.Server)
            {
                this.Step.SupportedDeployment = new OptionSetValue((int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Server_Only_0);
            }
            else if (messageFilter.Availability == (int)SdkMessageFilter.Schema.OptionSets.availability.Client)
            {
                this.Step.SupportedDeployment = new OptionSetValue((int)SdkMessageProcessingStep.Schema.OptionSets.supporteddeployment.Microsoft_Dynamics_365_Client_for_Outlook_Only_1);
            }

#pragma warning disable CS0612 // Type or member is obsolete
            this.Step.InvocationSource = new OptionSetValue(invocationsource);
#pragma warning restore CS0612 // Type or member is obsolete

            if (string.Equals(messageFilter.SdkMessageId.Name, "Update", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Step.FilteringAttributes = txtBFilteringBAttributes.Text.Trim();
            }
            else
            {
                this.Step.FilteringAttributes = string.Empty;
            }

            this.Step.ImpersonatingUserId = _impersonatingUser;

            string secureConfig = txtBSecureConfiguration.Text.Trim();
            EntityReference configToDelete = null;

            if (string.IsNullOrEmpty(secureConfig))
            {
                if (this.Step.SdkMessageProcessingStepSecureConfigId != null)
                {
                    configToDelete = this.Step.SdkMessageProcessingStepSecureConfigId;

                    this.Step.SdkMessageProcessingStepSecureConfigId = null;
                }
            }
            else
            {
                SdkMessageProcessingStepSecureConfig config = new SdkMessageProcessingStepSecureConfig()
                {
                    SecureConfig = secureConfig,
                };

                if (this.Step.SdkMessageProcessingStepSecureConfigId != null)
                {
                    config.Id = this.Step.SdkMessageProcessingStepSecureConfigId.Id;
                }

                ToggleControls(false, Properties.OutputStrings.UpdatingSdkMessageProcessingStepSecureConfigFormat1, _service.ConnectionData.Name);

                try
                {
                    config.Id = await _service.UpsertAsync(config);

                    ToggleControls(true, Properties.OutputStrings.UpdatingSdkMessageProcessingStepSecureConfigCompletedFormat1, _service.ConnectionData.Name);
                }
                catch (Exception ex)
                {
                    ToggleControls(true, Properties.OutputStrings.UpdatingSdkMessageProcessingStepSecureConfigFailedFormat1, _service.ConnectionData.Name);

                    _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                }

                if (config.Id != Guid.Empty)
                {
                    this.Step.SdkMessageProcessingStepSecureConfigId = config.ToEntityReference();
                }
            }

            ToggleControls(false, Properties.OutputStrings.UpdatingSdkMessageProcessingStepFormat1, _service.ConnectionData.Name);

            try
            {
                this.Step.Id = await _service.UpsertAsync(this.Step);

                if (configToDelete != null)
                {
                    ToggleControls(false, Properties.OutputStrings.DeletingSdkMessageProcessingStepSecureConfigFormat1, _service.ConnectionData.Name);

                    try
                    {
                        await _service.DeleteAsync(configToDelete.LogicalName, configToDelete.Id);

                        ToggleControls(true, Properties.OutputStrings.DeletingSdkMessageProcessingStepSecureConfigCompletedFormat1, _service.ConnectionData.Name);
                    }
                    catch (Exception ex)
                    {
                        ToggleControls(true, Properties.OutputStrings.DeletingSdkMessageProcessingStepSecureConfigFailedFormat1, _service.ConnectionData.Name);

                        _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                        _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                    }
                }

                ToggleControls(true, Properties.OutputStrings.UpdatingSdkMessageProcessingStepCompletedFormat1, _service.ConnectionData.Name);

                this.DialogResult = true;

                this.Close();
            }
            catch (Exception ex)
            {
                ToggleControls(true, Properties.OutputStrings.UpdatingSdkMessageProcessingStepFailedFormat1, _service.ConnectionData.Name);

                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
            }
        }

        private async void btnSelectAttributes_Click(object sender, RoutedEventArgs e)
        {
            string entityName = cmBPrimaryEntity.SelectedItem?.ToString();

            if (!entityName.IsValidEntityName())
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.GettingEntityMetadataFormat1, entityName);

            var repository = new EntityMetadataRepository(_service);

            var entityMetadata = await repository.GetEntityMetadataAsync(entityName);

            ToggleControls(true, Properties.OutputStrings.GettingEntityMetadataCompletedFormat1, entityName);

            if (entityMetadata == null)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.UpdatingStepFilteringAttributesFormat1, entityName);

            var form = new WindowAttributeMultiSelect(_iWriteToOutput
                , _service
                , entityMetadata
                , txtBFilteringBAttributes.Text.Trim()
            );

            if (form.ShowDialog().GetValueOrDefault())
            {
                txtBFilteringBAttributes.Text = form.GetAttributes();
            }

            ToggleControls(true, Properties.OutputStrings.UpdatingStepFilteringAttributesCompletedFormat1, entityName);
        }

        private void btnSetAllAttributes_Click(object sender, RoutedEventArgs e)
        {
            txtBFilteringBAttributes.Text = string.Empty;
        }

        private void rbStage_Checked(object sender, RoutedEventArgs e)
        {
            var isPost = rBPostOperation.IsChecked.GetValueOrDefault();

            rBSync.IsEnabled = rBAsync.IsEnabled = isPost;

            ChangeCheckBoxEnable();
        }

        private void rbMode_Checked(object sender, RoutedEventArgs e)
        {
            ChangeCheckBoxEnable();
        }

        private void ChangeCheckBoxEnable()
        {
            chBDeleteAsyncOperationIfSuccessful.IsEnabled = this.IsControlsEnabled && rBPostOperation.IsChecked.GetValueOrDefault() && rBAsync.IsChecked.GetValueOrDefault();
        }

        private void btnGenerateName_Click(object sender, RoutedEventArgs e)
        {
            string messageName = cmBMessageName.Text?.Trim();
            string primaryEntity = cmBPrimaryEntity.Text?.Trim();
            string secondaryEntity = cmBSecondaryEntity.Text?.Trim();

            var nameBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_eventHandlerName))
            {
                nameBuilder.AppendFormat("{0}: ", _eventHandlerName);
            }

            var stage = GetStage();
            var mode = GetMode();

            nameBuilder.Append(GetStageName(stage, mode) + " ");

            if (string.IsNullOrEmpty(messageName))
            {
                nameBuilder.Append("Not Specified of ");
            }
            else
            {
                nameBuilder.AppendFormat("{0} of ", messageName);
            }

            bool hasPrimaryEntity = false;

            if (primaryEntity.IsValidEntityName())
            {
                hasPrimaryEntity = true;

                nameBuilder.Append(primaryEntity);
            }

            if (secondaryEntity.IsValidEntityName())
            {
                if (hasPrimaryEntity)
                {
                    nameBuilder.AppendFormat(" and {0}", secondaryEntity);
                }
                else
                {
                    nameBuilder.Append(secondaryEntity);
                }
            }
            else if (!hasPrimaryEntity)
            {
                nameBuilder.Append("any Entity");
            }

            txtBName.Text = nameBuilder.ToString();
        }

        private static string GetStageName(int stage, int? mode)
        {
            StringBuilder result = new StringBuilder();

            switch (stage)
            {
                case (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_validation_10:
                    result.Append("PreValidation");
                    break;

                case (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Pre_operation_20:
                    result.Append("Pre");
                    break;

                case (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_40:
                case (int)SdkMessageProcessingStep.Schema.OptionSets.stage.Post_operation_Deprecated_50:
                    result.Append("Post");

                    if (mode == (int)SdkMessageProcessingStep.Schema.OptionSets.mode.Asynchronous_1)
                    {
                        result.Append("Asynch");
                    }

                    break;

                default:
                    result.Append("Other");
                    break;
            }

            return result.ToString();
        }

        private void btnSelectUser_Click(object sender, RoutedEventArgs e)
        {
            var repository = new SystemUserRepository(_service);

            Func<string, Task<IEnumerable<SystemUser>>> getter = (string filter) => repository.GetUsersAsync(filter, new ColumnSet(
                               SystemUser.Schema.Attributes.domainname
                               , SystemUser.Schema.Attributes.fullname
                               , SystemUser.Schema.Attributes.businessunitid
                               , SystemUser.Schema.Attributes.isdisabled
                               ));

            IEnumerable<DataGridColumn> columns = SystemUserRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<SystemUser>(_iWriteToOutput, _service.ConnectionData, SystemUser.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var user = form.SelectedEntity;

            this._impersonatingUser = user.ToEntityReference();
            txtBRunInUserContext.Text = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.DomainName;
        }

        private void btnClearUser_Click(object sender, RoutedEventArgs e)
        {
            this._impersonatingUser = null;
            txtBRunInUserContext.Text = _impersonatingUserNone;
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar
                , btnSave
                , btnClose
                , btnSelectUser
                , btnClearUser

                , cmBMessageName
                , cmBPrimaryEntity
                , cmBSecondaryEntity

                , txtBDescription
                , txtBExecutionOrder
                , txtBName
                , txtBSecureConfiguration
                , txtBUnSecureConfiguration

                , rBAsync
                , rBSync

                , rBPreValidation
                , rBPreOperation
                , rBPostOperation

                , rBParent
                , rBChild

                , rBOffline
                , rBDeploymentBoth
                , rBServer
            );

            ToggleControl(IsControlsEnabled && string.Equals(cmBMessageName.SelectedItem?.ToString(), "Update", StringComparison.InvariantCultureIgnoreCase)
                , btnSetAllAttributes
                , btnSelectAttributes
            );

            ChangeCheckBoxEnable();
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(_service.ConnectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }
    }
}