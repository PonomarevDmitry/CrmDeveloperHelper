using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerWorkflow : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowExplorerWorkflow(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string filterEntity
            , string selection
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            LoadEntityNames(cmBEntityName, service.ConnectionData);

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.WorkflowXmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            cmBCategory.ItemsSource = new EnumBindingSourceExtension(typeof(Workflow.Schema.OptionSets.category?)).ProvideValue(null) as IEnumerable;
            cmBMode.ItemsSource = new EnumBindingSourceExtension(typeof(Workflow.Schema.OptionSets.mode?)).ProvideValue(null) as IEnumerable;
            cmBStatusCode.ItemsSource = new EnumBindingSourceExtension(typeof(Workflow.Schema.OptionSets.statuscode?)).ProvideValue(null) as IEnumerable;

            LoadFromConfig();

            LoadConfiguration();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            cmBEntityName.Text = filterEntity;

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwWorkflows.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            if (service != null)
            {
                ShowExistingWorkflows();
            }
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getEntityName: GetEntityName
                , getWorkflowName: GetWorkflowName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getEntityName: GetEntityName
                , getWorkflowName: GetWorkflowName
            );

            explorersHelper.FillExplorers(miExplorers);
            explorersHelper.FillExplorers(miExplorers2);

            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations2);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));
            }
        }

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.PrimaryEntity;
        }

        private string GetWorkflowName()
        {
            var entity = GetSelectedEntity();

            return entity?.Name ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        private const string paramCategory = "Category";
        private const string paramMode = "Mode";
        private const string paramStatusCode = "StatusCode";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            var categoryValue = winConfig.GetValueInt(paramCategory);
            if (categoryValue != -1)
            {
                var item = cmBCategory.Items.OfType<Workflow.Schema.OptionSets.category?>().FirstOrDefault(e => (int)e == categoryValue);
                if (item != null)
                {
                    cmBCategory.SelectedItem = item;
                }
            }

            var modeValue = winConfig.GetValueInt(paramMode);
            if (modeValue != -1)
            {
                var item = cmBMode.Items.OfType<Workflow.Schema.OptionSets.mode?>().FirstOrDefault(e => (int)e == modeValue);
                if (item != null)
                {
                    cmBMode.SelectedItem = item;
                }
            }

            var modeStatusCodeValue = winConfig.GetValueInt(paramStatusCode);
            if (modeStatusCodeValue != -1)
            {
                var item = cmBStatusCode.Items.OfType<Workflow.Schema.OptionSets.statuscode?>().FirstOrDefault(e => (int)e == modeStatusCodeValue);
                if (item != null)
                {
                    cmBStatusCode.SelectedItem = item;
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            var categoryValue = -1;

            {
                if (cmBCategory.SelectedItem is Workflow.Schema.OptionSets.category comboBoxItem)
                {
                    categoryValue = (int)comboBoxItem;
                }
            }

            var modeValue = -1;

            {
                if (cmBMode.SelectedItem is Workflow.Schema.OptionSets.mode comboBoxItem)
                {
                    modeValue = (int)comboBoxItem;
                }
            }

            var modeStatusCodeValue = -1;

            {
                if (cmBStatusCode.SelectedItem is Workflow.Schema.OptionSets.statuscode comboBoxItem)
                {
                    modeStatusCodeValue = (int)comboBoxItem;
                }
            }

            winConfig.DictInt[paramCategory] = categoryValue;
            winConfig.DictInt[paramMode] = modeValue;
            winConfig.DictInt[paramStatusCode] = modeValue;
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

        private async Task ShowExistingWorkflows()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingWorkflows);

            this._itemsSource.Clear();

            string entityName = string.Empty;
            Workflow.Schema.OptionSets.category? category = null;
            Workflow.Schema.OptionSets.mode? mode = null;
            Workflow.Schema.OptionSets.statuscode? statuscode = null;

            this.Dispatcher.Invoke(() =>
            {
                {
                    if (cmBCategory.SelectedItem is Workflow.Schema.OptionSets.category comboBoxItem)
                    {
                        category = comboBoxItem;
                    }
                }

                {
                    if (cmBMode.SelectedItem is Workflow.Schema.OptionSets.mode comboBoxItem)
                    {
                        mode = comboBoxItem;
                    }
                }

                {
                    if (cmBMode.SelectedItem is Workflow.Schema.OptionSets.statuscode comboBoxItem)
                    {
                        statuscode = comboBoxItem;
                    }
                }

                if (!string.IsNullOrEmpty(cmBEntityName.Text)
                    && cmBEntityName.Items.Contains(cmBEntityName.Text)
                )
                {
                    entityName = cmBEntityName.Text.Trim().ToLower();
                }
            });

            string filterEntity = null;

            if (service.ConnectionData.IsValidEntityName(entityName))
            {
                filterEntity = entityName;
            }

            IEnumerable<Workflow> list = Enumerable.Empty<Workflow>();

            try
            {
                if (service != null)
                {
                    WorkflowRepository repository = new WorkflowRepository(service);
                    list = await repository.GetListAsync(
                        filterEntity
                        , category
                        , mode
                        , statuscode
                        , new ColumnSet(
                            Workflow.Schema.Attributes.category
                            , Workflow.Schema.Attributes.name
                            , Workflow.Schema.Attributes.mode
                            , Workflow.Schema.Attributes.uniquename
                            , Workflow.Schema.Attributes.primaryentity
                            , Workflow.Schema.Attributes.iscustomizable
                            , Workflow.Schema.Attributes.statecode
                            , Workflow.Schema.Attributes.statuscode
                        ));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadWorkflows(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingWorkflowsCompletedFormat1, list.Count());
        }

        private static IEnumerable<Workflow> FilterList(IEnumerable<Workflow> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.Id == tempGuid || ent.WorkflowIdUnique == tempGuid);
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var type = ent.PrimaryEntity ?? string.Empty;
                        var name = ent.Name ?? string.Empty;
                        var nameUnique = ent.UniqueName ?? string.Empty;

                        return type.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || nameUnique.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            ;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string PrimaryEntity => Workflow.PrimaryEntity;

            public string Name => Workflow.Name;

            public string UniqueName => Workflow.UniqueName;

            public string Category { get; }

            public string Mode { get; }

            public string StatusCode { get; }

            public Workflow Workflow { get; }

            public EntityViewItem(Workflow workflow)
            {
                workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out var category);
                workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.mode, out var mode);
                workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out var statuscode);

                this.Category = category;
                this.Mode = mode;
                this.StatusCode = statuscode;

                this.Workflow = workflow;
            }
        }

        private void LoadWorkflows(IEnumerable<Workflow> results)
        {
            this.lstVwWorkflows.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.PrimaryEntity)
                    .ThenBy(ent => ent.Category?.Value)
                    .ThenBy(ent => ent.Name)
                )
                {
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwWorkflows.Items.Count == 1)
                {
                    this.lstVwWorkflows.SelectedItem = this.lstVwWorkflows.Items[0];
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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, cmBCategory, cmBMode);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwWorkflows.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwWorkflows.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportWorkflow, btnExportAll };

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

        private void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingWorkflows();
            }
        }

        private Workflow GetSelectedEntity()
        {
            return this.lstVwWorkflows.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwWorkflows.SelectedItems.OfType<EntityViewItem>().Select(e => e.Workflow).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    var service = await GetService();

                    if (service != null)
                    {
                        service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Workflow, item.Workflow.Id);
                    }
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idWorkflow, string entityName, string name, string category)
        {
            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml);

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters);

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata);

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idWorkflow, string entityName, string name, string category, Func<string, Guid, string, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idWorkflow, entityName, name, category);
        }

        private Task<string> CreateFileAsync(string folder, string entityName, string category, string name, string fieldTitle, string xmlContent, string extension)
        {
            return Task.Run(() => CreateFile(folder, entityName, category, name, fieldTitle, xmlContent, extension));
        }

        private string CreateFile(string folder, string entityName, string category, string name, string fieldTitle, string xmlContent, string extension)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetWorkflowFileName(connectionData.Name, entityName, category, name, fieldTitle, extension);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private async Task<string> CreateCorrectedFileAsync(string folder, string entityName, string category, string name, string fieldTitle, string xmlContent)
        {
            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    var replacer = new LabelReplacer(await TranslationRepository.GetDefaultTranslationFromCacheAsync(service.ConnectionData.ConnectionId, service));

                    xmlContent = ContentComparerHelper.GetCorrectedXaml(xmlContent, replacer);

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            return filePath;
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idWorkflow, string entityName, string name, string category)
        {
            await PerformExportEntityDescription(folder, idWorkflow, entityName, name, category);

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml);

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters);

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata);

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata);
        }

        private async Task ExecuteActionEntity(Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idWorkflow, entityName, name, category, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                string extension = "json";

                if (ContentComparerHelper.TryParseXml(xmlContent, out var _))
                {
                    extension = "xml";

                    xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                        xmlContent
                        , _commonConfig
                        , XmlOptionsControls.WorkflowXmlOptions
                        , workflowId: idWorkflow
                    );
                }
                else
                {
                    xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                }

                string filePath = await CreateFileAsync(folder, entityName, category, name, fieldTitle, xmlContent, extension);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(true));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                string extension = "json";

                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var _))
                    {
                        extension = "xml";

                        xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            xmlContent
                            , _commonConfig
                            , XmlOptionsControls.WorkflowXmlOptions
                            , workflowId: idWorkflow
                        );
                    }
                    else
                    {
                        xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                    }

                    await CreateFileAsync(folder, entityName, category, name, fieldTitle + " BackUp", xmlContent, extension);
                }

                var newText = string.Empty;
                bool? dialogResult = false;

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + fieldTitle, fieldTitle, xmlContent);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                {
                    if (ContentComparerHelper.TryParseXml(newText, out var doc))
                    {
                        ContentComparerHelper.ClearRoot(doc);

                        newText = doc.ToString(SaveOptions.DisableFormatting);
                    }
                }

                if (workflow.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1)
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.DeactivatingWorkflowFormat2, service.ConnectionData.Name, workflow.Name);

                    await service.ExecuteAsync<SetStateResponse>(new SetStateRequest()
                    {
                        EntityMoniker = workflow.ToEntityReference(),
                        State = new OptionSetValue((int)Workflow.Schema.OptionSets.statecode.Draft_0),
                        Status = new OptionSetValue((int)Workflow.Schema.OptionSets.statuscode.Draft_0_Draft_1),
                    });
                }

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

                var updateEntity = new Workflow
                {
                    Id = idWorkflow
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                if (workflow.StateCodeEnum == Workflow.Schema.OptionSets.statecode.Activated_1)
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.ActivatingWorkflowFormat2, service.ConnectionData.Name, workflow.Name);

                    await service.ExecuteAsync<SetStateResponse>(new SetStateRequest()
                    {
                        EntityMoniker = workflow.ToEntityReference(),
                        State = new OptionSetValue((int)Workflow.Schema.OptionSets.statecode.Activated_1),
                        Status = new OptionSetValue((int)Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2),
                    });
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private async Task PerformExportCorrectedXmlToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingCorrectedXmlFieldToFileFormat1, fieldTitle);

            try
            {
                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                    xmlContent
                    , _commonConfig
                    , XmlOptionsControls.WorkflowXmlOptions
                    , workflowId: idWorkflow
                );

                string filePath = await CreateCorrectedFileAsync(folder, entityName, category, name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingCorrectedXmlFieldToFileCompletedFormat1, fieldTitle);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingCorrectedXmlFieldToFileFailedFormat1, fieldTitle);
            }
        }

        private async Task PerformExportUsedEntitesToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.AnalizingWorkflowFormat2, entityName, name);

            try
            {
                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent) && ContentComparerHelper.TryParseXml(xmlContent, out var doc))
                {
                    string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, fieldTitle, "txt");
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, new SolutionComponentDescriptor(service));

                    var stringBuider = new StringBuilder();

                    await workflowDescriptor.GetDescriptionUsedEntitiesInWorkflowAsync(stringBuider, idWorkflow);

                    File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.AnalizingWorkflowCompletedFormat2, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.AnalizingWorkflowFailedFormat2, entityName, name);
            }
        }

        private async Task PerformExportUsedNotExistsEntitesToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.AnalizingWorkflowFormat2, entityName, name);

            try
            {
                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent) && ContentComparerHelper.TryParseXml(xmlContent, out var doc))
                {
                    string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, fieldTitle, "txt");
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    WorkflowUsedEntitiesDescriptor workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, new SolutionComponentDescriptor(service));

                    var stringBuider = new StringBuilder();

                    await workflowDescriptor.GetDescriptionUsedNotExistsEntitiesInWorkflowAsync(stringBuider, idWorkflow);

                    File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.AnalizingWorkflowCompletedFormat2, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.AnalizingWorkflowFailedFormat2, entityName, name);
            }
        }

        private async Task PerformExportCreatedOrUpdatedEntitiesToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.AnalizingWorkflowFormat2, entityName, name);

            try
            {
                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent) && ContentComparerHelper.TryParseXml(xmlContent, out var doc))
                {
                    string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, fieldTitle, "txt");
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    WorkflowUsedEntitiesDescriptor workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, new SolutionComponentDescriptor(service));

                    var stringBuider = new StringBuilder();

                    await workflowDescriptor.GetDescriptionEntitesAndAttributesInWorkflowAsync(stringBuider, idWorkflow);

                    File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.AnalizingWorkflowCompletedFormat2, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.AnalizingWorkflowFailedFormat2, entityName, name);
            }
        }

        private void mIExportWorkflowXaml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml, PerformExportXmlToFile);
        }

        private void mIExportWorkflowCorrectedXaml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.CorrectedXaml, PerformExportCorrectedXmlToFile);
        }

        private void mIExportWorkflowUsedEntities_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, "UsedEntities", PerformExportUsedEntitesToFile);
        }

        private void mIExportWorkflowUsedNotExistsEntities_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, "UsedNotExistsEntities", PerformExportUsedNotExistsEntitesToFile);
        }

        private void mIExportWorkflowCreatedOrUpdatedEntities_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, "CreatedOrUpdatedEntities", PerformExportCreatedOrUpdatedEntitiesToFile);
        }

        private void mIExportWorkflowInputParameters_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters, PerformExportXmlToFile);
        }

        private void mIExportWorkflowClientData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata, PerformExportXmlToFile);
        }

        private void mIExportWorkflowUIData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata, PerformExportXmlToFile);
        }

        private void mIExportWorkflowProcessRoleAssignment_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.processroleassignment, Workflow.Schema.Headers.processroleassignment, PerformExportXmlToFile);
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], PerformExportEntityDescription);
        }

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], PerformEntityEditor);
        }

        private async void mIChangeStateWorkflow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], PerformChangeStateWorkflow);
        }

        private void mIDeleteWorkflow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], PerformDeleteEntity);
        }

        private async void mIExportWorkflowShowDifferenceXamlAndCorrectedXaml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await PerformDifferenceXamlAndCorrectedXaml(folder, entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml, Workflow.Schema.Headers.CorrectedXaml);
        }

        private async Task PerformDifferenceXamlAndCorrectedXaml(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle1, string fieldTitle2)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ShowingDifferenceForCorrectedFieldFormat1, fieldName);

            WorkflowRepository repository = new WorkflowRepository(service);

            Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

            string xmlContent = workflow.GetAttributeValue<string>(fieldName);

            xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                xmlContent
                , _commonConfig
                , XmlOptionsControls.WorkflowXmlOptions
                , workflowId: idWorkflow
            );

            string filePath1 = await CreateFileAsync(folder, entityName, category, name, fieldTitle1, xmlContent, "xml");
            string filePath2 = await CreateCorrectedFileAsync(folder, entityName, category, name, fieldTitle2, xmlContent);

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath1);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath2);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ShowingDifferenceForCorrectedFieldCompletedFormat1, fieldName);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idWorkflow, string entityName, string name, string category)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(true));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, workflow, EntityFileNameFormatter.WorkflowIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , workflow.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionFailed);
            }
        }

        private async Task PerformEntityEditor(string folder, Guid idWorkflow, string entityName, string name, string category)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, Workflow.EntityLogicalName, idWorkflow);
        }

        private async Task PerformDeleteEntity(string folder, Guid idWorkflow, string entityName, string name, string category)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, Workflow.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, Workflow.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, Workflow.EntityLogicalName, idWorkflow);

                    await service.DeleteAsync(Workflow.EntityLogicalName, idWorkflow);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, Workflow.EntityLogicalName);

                ShowExistingWorkflows();
            }
        }

        private async Task PerformChangeStateWorkflow(string folder, Guid idWorkflow, string entityName, string name, string category)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ChangingEntityStateFormat1, Workflow.EntityLogicalName);

            var repository = new WorkflowRepository(service);

            var workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(true));

            int state = workflow.StatusCodeEnum == Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2 ? (int)Workflow.Schema.OptionSets.statecode.Draft_0 : (int)Workflow.Schema.OptionSets.statecode.Activated_1;
            int status = workflow.StatusCodeEnum == Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2 ? (int)Workflow.Schema.OptionSets.statuscode.Draft_0_Draft_1 : (int)Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2;

            try
            {
                await service.ExecuteAsync<Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest()
                {
                    EntityMoniker = new EntityReference(Workflow.EntityLogicalName, idWorkflow),
                    State = new OptionSetValue(state),
                    Status = new OptionSetValue(status),
                });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateCompletedFormat1, Workflow.EntityLogicalName);
            }
            catch (Exception ex)
            {
                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateFailedFormat1, Workflow.EntityLogicalName);

                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ShowExistingWorkflows();
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingWorkflows();
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Workflow, entity.Id);
            }
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
        {
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

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Workflow, entity.WorkflowId.Value);
            }
        }

        private void mIOpenListInWeb_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenCrmWebSite(OpenCrmWebSiteType.Workflows);
            }
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.PrimaryEntity.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.PrimaryEntity);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.PrimaryEntity.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(entity.PrimaryEntity);
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
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Workflow, new[] { entity.WorkflowId.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void AddToCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void AddToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void AddToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async Task AddEntityToSolution(bool withSelect, string solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.PrimaryEntity.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                var entityMetadataId = connectionData.GetEntityMetadataId(entity.PrimaryEntity);

                if (entityMetadataId.HasValue)
                {
                    _commonConfig.Save();

                    var service = await GetService();

                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                        await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entityMetadataId.Value }, rootComponentBehavior, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            ConnectionData connectionData = GetSelectedConnection();

            FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            EntityViewItem nodeItem = GetItemFromRoutedDataContext<EntityViewItem>(e);

            ActivateControls(items, (nodeItem.Workflow.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            bool hasEntity = nodeItem.Workflow.PrimaryEntity.IsValidEntityName();
            ActivateControls(items, hasEntity, "contMnEntity");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastDoNotIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityToSolutionLastIncludeAsShellOnly");

            ActivateControls(items, hasEntity && connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityToSolutionLast");

            SetControlsName(items, GetChangeStateName(nodeItem.Workflow), "contMnChangeState");
        }

        private string GetChangeStateName(Workflow workflow)
        {
            if (workflow == null)
            {
                return "ChangeState";
            }

            return workflow.StatusCodeEnum == Workflow.Schema.OptionSets.statuscode.Activated_1_Activated_2 ? "Deactivate Workflow" : "Activate Workflow";
        }

        private void tSDDBExportWorkflow_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var workflow = GetSelectedEntity();

            ActivateControls(tSDDBExportWorkflow.Items.OfType<Control>(), (workflow?.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            SetControlsName(tSDDBExportWorkflow.Items.OfType<Control>(), GetChangeStateName(workflow), "contMnChangeState");
        }

        private void cmBCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ShowExistingWorkflows();
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
                , (int)ComponentType.Workflow
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
                , (int)ComponentType.Workflow
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
                LoadEntityNames(cmBEntityName, connectionData);

                await ShowExistingWorkflows();
            }
        }

        private void mIUpdateWorkflowXaml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml, PerformUpdateEntityField);
        }

        private void mIUpdateWorkflowInputParameters_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters, PerformUpdateEntityField);
        }

        private void mIUpdateWorkflowClientData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata, PerformUpdateEntityField);
        }

        private void mIUpdateWorkflowUIData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata, PerformUpdateEntityField);
        }

        private void mIUpdateWorkflowProcessRoleAssignment_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.processroleassignment, Workflow.Schema.Headers.processroleassignment, PerformUpdateEntityField);
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private void hyperlinkXaml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Workflow;

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml, PerformExportXmlToFile);
        }

        private void hyperlinkInputParameters_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Workflow;

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters, PerformExportXmlToFile);
        }

        private void hyperlinkClientData_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Workflow;

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata, PerformExportXmlToFile);
        }

        private void hyperlinkUIData_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Workflow;

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata, PerformExportXmlToFile);
        }

        private void hyperlinkProcessRoleAssignment_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Workflow;

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.processroleassignment, Workflow.Schema.Headers.processroleassignment, PerformExportXmlToFile);
        }

        private void hyperlinkCorrectedXaml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Workflow;

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.CorrectedXaml, PerformExportCorrectedXmlToFile);
        }

        private void lstVwWorkflows_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwWorkflows_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteWorkflow_Click(sender, e);
        }
    }
}