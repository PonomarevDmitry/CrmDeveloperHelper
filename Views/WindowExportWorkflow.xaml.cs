using Microsoft.Xrm.Sdk.Metadata;
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
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportWorkflow : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private string _filterEntity;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private int _init = 0;

        public WindowExportWorkflow(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , string selection
            )
        {
            BeginLoadConfig();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;
            this._filterEntity = filterEntity;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            var attributeCategory = _descriptorCache[service.ConnectionData.ConnectionId].MetadataSource.GetAttributeMetadata(Workflow.EntityLogicalName, Workflow.Schema.Attributes.category);
            var attributeMode = _descriptorCache[service.ConnectionData.ConnectionId].MetadataSource.GetAttributeMetadata(Workflow.EntityLogicalName, Workflow.Schema.Attributes.mode);

            FillComboBoxCategoryAndMode(attributeCategory, attributeMode);

            LoadFromConfig();

            LoadConfiguration();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            if (string.IsNullOrEmpty(_filterEntity))
            {
                btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
                btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwWorkflows.ItemsSource = _itemsSource;

            EndLoadConfig();

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            if (service != null)
            {
                ShowExistingWorkflows();
            }
        }

        private void BeginLoadConfig()
        {
            ++_init;
        }

        private void EndLoadConfig()
        {
            --_init;
        }

        private void FillComboBoxCategoryAndMode(AttributeMetadata attributeCategory, AttributeMetadata attributeMode)
        {
            BeginLoadConfig();

            {
                int? category = null;

                if (cmBCategory.SelectedItem is ComboBoxItem comboBoxItem && comboBoxItem.Tag is int valueTempInt)
                {
                    category = valueTempInt;
                }

                cmBCategory.Items.Clear();

                cmBCategory.Items.Add(string.Empty);

                if (attributeCategory != null && attributeCategory is PicklistAttributeMetadata picklist)
                {
                    foreach (var item in picklist.OptionSet.Options.Where(o => o.Value.HasValue))
                    {
                        var value = new ComboBoxItem()
                        {
                            Tag = item.Value.Value,

                            Content = item.Label.UserLocalizedLabel.Label
                        };

                        cmBCategory.Items.Add(value);

                        if (category.HasValue && category.Value == item.Value.Value)
                        {
                            cmBCategory.SelectedItem = value;
                        }
                    }
                }
            }

            {
                int? mode = null;

                if (cmBMode.SelectedItem is ComboBoxItem comboBoxItem && comboBoxItem.Tag is int valueTempInt)
                {
                    mode = valueTempInt;
                }

                cmBMode.Items.Clear();

                cmBMode.Items.Add(string.Empty);

                if (attributeMode != null && attributeMode is PicklistAttributeMetadata picklist)
                {
                    foreach (var item in picklist.OptionSet.Options.Where(o => o.Value.HasValue))
                    {
                        var value = new ComboBoxItem()
                        {
                            Tag = item.Value.Value,

                            Content = item.Label.UserLocalizedLabel.Label
                        };

                        cmBMode.Items.Add(value);

                        if (mode.HasValue && mode.Value == item.Value.Value)
                        {
                            cmBMode.SelectedItem = value;
                        }
                    }
                }
            }

            EndLoadConfig();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        private const string paramCategory = "Category";
        private const string paramMode = "Mode";

        private void LoadConfiguration()
        {
            WindowSettings winConfig = this.GetWindowsSettings();

            var categoryValue = winConfig.GetValueInt(paramCategory);
            if (categoryValue != -1)
            {
                var item = cmBCategory.Items.OfType<ComboBoxItem>().FirstOrDefault(e => (int)e.Tag == categoryValue);
                if (item != null)
                {
                    cmBCategory.SelectedItem = item;
                }
            }

            var modeValue = winConfig.GetValueInt(paramMode);
            if (modeValue != -1)
            {
                var item = cmBMode.Items.OfType<ComboBoxItem>().FirstOrDefault(e => (int)e.Tag == modeValue);
                if (item != null)
                {
                    cmBMode.SelectedItem = item;
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            var categoryValue = -1;

            {
                if (cmBCategory.SelectedItem is ComboBoxItem comboBoxItem
                        && comboBoxItem.Tag is int value
                        )
                {
                    categoryValue = value;
                }
            }

            var modeValue = -1;

            {
                if (cmBMode.SelectedItem is ComboBoxItem comboBoxItem
                        && comboBoxItem.Tag is int value
                        )
                {
                    modeValue = value;
                }
            }

            winConfig.DictInt[paramCategory] = categoryValue;
            winConfig.DictInt[paramMode] = modeValue;
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

        private async Task<SolutionComponentDescriptor> GetDescriptor()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_descriptorCache.ContainsKey(connectionData.ConnectionId))
                {
                    var service = await GetService();

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);
                }

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingWorkflows()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingWorkflows);

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            int? category = null;

            cmBCategory.Dispatcher.Invoke(() =>
            {
                if (cmBCategory.SelectedItem is ComboBoxItem comboBoxItem && comboBoxItem.Tag is int value)
                {
                    category = value;
                }
            });

            int? mode = null;

            cmBMode.Dispatcher.Invoke(() =>
            {
                if (cmBMode.SelectedItem is ComboBoxItem comboBoxItem && comboBoxItem.Tag is int value)
                {
                    mode = value;
                }
            });

            IEnumerable<Workflow> list = Enumerable.Empty<Workflow>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    WorkflowRepository repository = new WorkflowRepository(service);
                    list = await repository.GetListAsync(this._filterEntity, category, mode
                        , new ColumnSet(
                            Workflow.Schema.Attributes.category
                            , Workflow.Schema.Attributes.name
                            , Workflow.Schema.Attributes.mode
                            , Workflow.Schema.Attributes.uniquename
                            , Workflow.Schema.Attributes.primaryentity
                            , Workflow.Schema.Attributes.iscustomizable
                            , Workflow.Schema.Attributes.statuscode
                        ));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            list = FilterList(list, textName);

            LoadWorkflows(list);
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
                        var type = ent.PrimaryEntity?.ToLower() ?? string.Empty;
                        var name = ent.Name?.ToLower() ?? string.Empty;
                        var nameUnique = ent.UniqueName?.ToLower() ?? string.Empty;

                        return type.Contains(textName) || name.Contains(textName) || nameUnique.Contains(textName);
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string EntityName { get; private set; }

            public string Category { get; private set; }

            public string ModeName { get; private set; }

            public string StatusName { get; private set; }

            public string WorkflowName { get; private set; }

            public Workflow Workflow { get; private set; }

            public EntityViewItem(string entityName, string workflowName, string category, string modeName, string statusName, Workflow workflow)
            {
                this.EntityName = entityName;
                this.WorkflowName = workflowName;
                this.Category = category;
                this.ModeName = modeName;
                this.Workflow = workflow;
                this.StatusName = statusName;
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
                    string name = entity.Name;

                    var uniqueName = entity.UniqueName;

                    if (!string.IsNullOrEmpty(uniqueName))
                    {
                        name += string.Format("    (UniqueName \"{0}\")", uniqueName);
                    }

                    string category = entity.FormattedValues[Workflow.Schema.Attributes.category];
                    string mode = entity.FormattedValues[Workflow.Schema.Attributes.mode];
                    string status = entity.FormattedValues[Workflow.Schema.Attributes.statuscode];

                    var item = new EntityViewItem(entity.PrimaryEntity, name, category, mode, status, entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwWorkflows.Items.Count == 1)
                {
                    this.lstVwWorkflows.SelectedItem = this.lstVwWorkflows.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingWorkflowsCompletedFormat1, results.Count());
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

            ToggleControl(cmBCurrentConnection, enabled);
            ToggleControl(cmBCategory, enabled);
            ToggleControl(cmBMode, enabled);

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

        private void ToggleControl(Control c, bool enabled)
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
            this.lstVwWorkflows.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwWorkflows.SelectedItems.Count > 0;

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

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null)
                {
                    ExecuteAction(item.Workflow.Id, item.Workflow.PrimaryEntity, item.Workflow.Name, item.Workflow.FormattedValues[Workflow.Schema.Attributes.category], PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idWorkflow, string entityName, string name, string category)
        {
            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.xaml, "Xaml");

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.inputparameters, "InputParameters");

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.clientdata, "ClientData");
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idWorkflow, string entityName, string name, string category, Func<string, Guid, string, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, idWorkflow, entityName, name, category);
        }

        private Task<string> CreateFileAsync(string folder, string entityName, string category, string name, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, entityName, category, name, fieldTitle, xmlContent));
        }

        private string CreateFile(string folder, string entityName, string category, string name, string fieldTitle, string xmlContent)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetWorkflowFileName(connectionData.Name, entityName, category, name, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
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

                    xmlContent = ContentCoparerHelper.ClearXml(xmlContent, replacer.FullfillLabelsForWorkflow, ContentCoparerHelper.RenameClasses, WorkflowUsedEntitiesHandler.ReplaceGuids);

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
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

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.xaml, "Xaml");

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.inputparameters, "InputParameters");

            await PerformExportXmlToFile(folder, idWorkflow, entityName, name, category, Workflow.Schema.Attributes.clientdata, "ClientData");

            //await PerformExportCorrectedXmlToFile(folder, workflow, Workflow.Schema.Attributes.xaml, "CorrectedXaml);
        }

        private async Task ExecuteActionEntity(Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, idWorkflow, entityName, name, category, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var service = await GetService();

                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, entityName, category, name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
               WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }
                }

                string filePath = await CreateFileAsync(folder, entityName, category, name, fieldTitle + " BackUp", xmlContent);

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
                    ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                {
                    if (ContentCoparerHelper.TryParseXml(newText, out var doc))
                    {
                        newText = doc.ToString(SaveOptions.DisableFormatting);
                    }
                }

                var updateEntity = new Workflow
                {
                    Id = idWorkflow
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private async Task PerformExportCorrectedXmlToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingCorrectedXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var service = await GetService();

                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                string filePath = await CreateCorrectedFileAsync(folder, entityName, category, name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingCorrectedXmlFieldToFileCompletedFormat1, fieldTitle);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingCorrectedXmlFieldToFileFailedFormat1, fieldTitle);
            }
        }

        private async Task PerformExportUsedEntitesToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.AnalizingWorkflowFormat2, entityName, name);

            try
            {
                var service = await GetService();

                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent) && ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                {
                    string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, fieldTitle, "txt");
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    var workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, new SolutionComponentDescriptor(service, true));

                    var stringBuider = new StringBuilder();

                    await workflowDescriptor.GetDescriptionUsedEntitiesInWorkflowAsync(stringBuider, idWorkflow);

                    File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);

                    this._iWriteToOutput.PerformAction(filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow();
                }

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingWorkflowCompletedFormat2, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingWorkflowFailedFormat2, entityName, name);
            }
        }

        private async Task PerformExportUsedNotExistsEntitesToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.AnalizingWorkflowFormat2, entityName, name);

            try
            {
                var service = await GetService();

                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent) && ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                {
                    string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, fieldTitle, "txt");
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    WorkflowUsedEntitiesDescriptor workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, new SolutionComponentDescriptor(service, true));

                    var stringBuider = new StringBuilder();

                    await workflowDescriptor.GetDescriptionUsedNotExistsEntitiesInWorkflowAsync(stringBuider, idWorkflow);

                    File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);

                    this._iWriteToOutput.PerformAction(filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow();
                }

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingWorkflowCompletedFormat2, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingWorkflowFailedFormat2, entityName, name);
            }
        }

        private async Task PerformExportCreatedOrUpdatedEntitiesToFile(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.AnalizingWorkflowFormat2, entityName, name);

            try
            {
                var service = await GetService();

                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent) && ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                {
                    string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, fieldTitle, "txt");
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    WorkflowUsedEntitiesDescriptor workflowDescriptor = new WorkflowUsedEntitiesDescriptor(_iWriteToOutput, service, new SolutionComponentDescriptor(service, true));

                    var stringBuider = new StringBuilder();

                    await workflowDescriptor.GetDescriptionEntitesAndAttributesInWorkflowAsync(stringBuider, idWorkflow);

                    File.WriteAllText(filePath, stringBuider.ToString(), new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);

                    this._iWriteToOutput.PerformAction(filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow();
                }

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingWorkflowCompletedFormat2, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.AnalizingWorkflowFailedFormat2, entityName, name);
            }
        }

        private void mIExportWorkflowXaml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, "Xaml", PerformExportXmlToFile);
        }

        private void mIExportWorkflowCorrectedXaml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, "CorrectedXaml", PerformExportCorrectedXmlToFile);
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

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.inputparameters, "InputParameters", PerformExportXmlToFile);
        }

        private void mIExportWorkflowClientData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.clientdata, "ClientData", PerformExportXmlToFile);
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

        private async void mIExportWorkflowShowDifferenceXamlAndCorrectedXaml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await PerformDifferenceXamlAndCorrectedXaml(folder, entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, "Xaml", "CorrectedXaml");
        }

        private async Task PerformDifferenceXamlAndCorrectedXaml(string folder, Guid idWorkflow, string entityName, string name, string category, string fieldName, string fieldTitle1, string fieldTitle2)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceForCorrectedFieldFormat1, fieldName);

            var service = await GetService();

            WorkflowRepository repository = new WorkflowRepository(service);

            Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(fieldName));

            string xmlContent = workflow.GetAttributeValue<string>(fieldName);

            string filePath1 = await CreateFileAsync(folder, entityName, category, name, fieldTitle1, xmlContent);
            string filePath2 = await CreateCorrectedFileAsync(folder, entityName, category, name, fieldTitle2, xmlContent);

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(filePath1);

                this._iWriteToOutput.PerformAction(filePath2);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceForCorrectedFieldCompletedFormat1, fieldName);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idWorkflow, string entityName, string name, string category)
        {
            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                var service = await GetService();

                string fileName = EntityFileNameFormatter.GetWorkflowFileName(service.ConnectionData.Name, entityName, category, name, "EntityDescription", "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                WorkflowRepository repository = new WorkflowRepository(service);

                Workflow workflow = await repository.GetByIdAsync(idWorkflow, new ColumnSet(true));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, workflow, EntityFileNameFormatter.WorkflowIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , workflow.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private void btnClearEntityFilter_Click(object sender, RoutedEventArgs e)
        {
            this._filterEntity = null;

            btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
            btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;

            ShowExistingWorkflows();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingWorkflows();
            }

            base.OnKeyDown(e);
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Workflow, entity.Id);
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
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenCrmWebSite(OpenCrmWebSiteType.Workflows);
            }
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.PrimaryEntity)
                || string.Equals(entity.PrimaryEntity, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.PrimaryEntity);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.PrimaryEntity)
                || string.Equals(entity.PrimaryEntity, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityListInWeb(entity.PrimaryEntity);
            }
        }

        private async void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddIntoSolution(true, null);
        }

        private async void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Workflow, new[] { entity.WorkflowId.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private async void mIAddEntityIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null);
        }

        private async void mIAddEntityIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddEntityIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || string.IsNullOrEmpty(entity.PrimaryEntity)
                || string.Equals(entity.PrimaryEntity, "none", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var entityMetadataId = connectionData.GetEntityMetadataId(entity.PrimaryEntity);

                if (entityMetadataId.HasValue)
                {
                    _commonConfig.Save();

                    var service = await GetService();
                    var descriptor = await GetDescriptor();

                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entityMetadataId.Value }, null, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
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

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            ActivateControls(items, (nodeItem.Workflow.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            bool hasEntity = !string.IsNullOrEmpty(nodeItem.Workflow.PrimaryEntity) && !string.Equals(nodeItem.Workflow.PrimaryEntity, "none", StringComparison.InvariantCultureIgnoreCase);
            ActivateControls(items, hasEntity, "contMnEntity");
            FillLastSolutionItems(connectionData, items, hasEntity, mIAddEntityIntoCrmSolutionLast_Click, "contMnAddEntityIntoSolutionLast");
        }

        private void tSDDBExportWorkflow_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            ActivateControls(tSDDBExportWorkflow.Items.OfType<Control>(), (nodeItem.Workflow.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity, null, null);
        }

        private async void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity);
        }

        private async void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity);
        }

        private async void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity);
        }

        private async void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity);
        }

        private async void miEntitySecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, entity?.PrimaryEntity);
        }

        private async void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity, string.Empty);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity, string.Empty);
        }

        private async void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity, string.Empty);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity, string.Empty, string.Empty);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity, string.Empty);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.PrimaryEntity, string.Empty);
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntity);
        }

        private async void btnCompareApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntity);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntity);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntity);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntity);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.PrimaryEntity);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private void cmBCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ShowExistingWorkflows();
        }

        private async void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , service
                , descriptor
                , _commonConfig
                , (int)ComponentType.Workflow
                , entity.Id
                , null
                );
        }

        private async void mIOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionWindow(
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

            if (_init > 0 || !_controlsEnabled)
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
                var descriptor = await GetDescriptor();

                var attributeCategory = descriptor.MetadataSource.GetAttributeMetadata(Workflow.EntityLogicalName, Workflow.Schema.Attributes.category);
                var attributeMode = descriptor.MetadataSource.GetAttributeMetadata(Workflow.EntityLogicalName, Workflow.Schema.Attributes.mode);

                FillComboBoxCategoryAndMode(attributeCategory, attributeMode);

                ShowExistingWorkflows();
            }
        }

        private void mIUpdateWorkflowXaml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.xaml, "Xaml", PerformUpdateEntityField);
        }

        private void mIUpdateWorkflowInputParameters_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.inputparameters, "InputParameters", PerformUpdateEntityField);
        }

        private void mIUpdateWorkflowClientData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.PrimaryEntity, entity.Name, entity.FormattedValues[Workflow.Schema.Attributes.category], Workflow.Schema.Attributes.clientdata, "ClientData", PerformUpdateEntityField);
        }
    }
}