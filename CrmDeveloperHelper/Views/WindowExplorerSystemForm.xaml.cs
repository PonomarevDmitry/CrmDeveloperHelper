using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
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
    public partial class WindowExplorerSystemForm : WindowWithSolutionComponentDescriptor
    {
        private readonly EnvDTE.SelectedItem _selectedItem;

        private readonly ObservableCollection<SystemFormViewItem> _itemsSource;

        private readonly Popup _popupExportXmlOptions;
        private readonly Popup _popupFileGenerationJavaScriptOptions;

        private readonly FileGenerationEntityMetadataJavaScriptOptionsControl _optionsControlJavaScript;

        public WindowExplorerSystemForm(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string filterEntity
            , EnvDTE.SelectedItem selectedItem
            , string selection
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            this._selectedItem = selectedItem;

            LoadEntityNames(cmBEntityName, service.ConnectionData);

            cmBFormActivationState.ItemsSource = new EnumBindingSourceExtension(typeof(SystemForm.Schema.OptionSets.formactivationstate?)).ProvideValue(null) as IEnumerable;
            cmBFormActivationState.SelectedItem = SystemForm.Schema.OptionSets.formactivationstate.Active_1;

            cmBFormType.ItemsSource = new EnumBindingSourceExtension(typeof(SystemForm.Schema.OptionSets.type?)).ProvideValue(null) as IEnumerable;

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.FormXmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._popupExportXmlOptions = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            _optionsControlJavaScript = new FileGenerationEntityMetadataJavaScriptOptionsControl();
            _optionsControlJavaScript.CloseClicked += this.optionsControlJavaScript_CloseClicked;
            this._popupFileGenerationJavaScriptOptions = new Popup
            {
                Child = _optionsControlJavaScript,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            LoadFromConfig();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            cmBEntityName.Text = filterEntity;

            if (this._selectedItem != null)
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

            this._itemsSource = new ObservableCollection<SystemFormViewItem>();

            this.lstVwSystemForms.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingSystemForms();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService, _selectedItem
                , getEntityName: GetEntityName
                , getSystemFormName: GetSystemFormName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getEntityName: GetEntityName
                , getSystemFormName: GetSystemFormName
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

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

            return entity?.ObjectTypeCode;
        }

        private string GetSystemFormName()
        {
            var entity = GetSelectedEntity();

            return entity?.Name ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            FileGenerationConfiguration.SaveConfiguration();

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

        private async Task ShowExistingSystemForms()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingForms);

            string filterEntity = null;

            string entityName = string.Empty;
            SystemForm.Schema.OptionSets.formactivationstate? state = null;
            SystemForm.Schema.OptionSets.type? formType = null;

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();

                if (!string.IsNullOrEmpty(cmBEntityName.Text)
                    && cmBEntityName.Items.Contains(cmBEntityName.Text)
                )
                {
                    entityName = cmBEntityName.Text.Trim().ToLower();
                }

                {
                    if (cmBFormActivationState.SelectedItem is SystemForm.Schema.OptionSets.formactivationstate cmBFormActivationStateValue)
                    {
                        state = cmBFormActivationStateValue;
                    }
                }

                {
                    if (cmBFormType.SelectedItem is SystemForm.Schema.OptionSets.type cmBFormTypeValue)
                    {
                        formType = cmBFormTypeValue;
                    }
                }
            });

            if (connectionData.IsValidEntityName(entityName))
            {
                filterEntity = entityName;
            }

            IEnumerable<SystemForm> list = Enumerable.Empty<SystemForm>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new SystemFormRepository(service);

                    list = await repository.GetListAsync(filterEntity
                        , formType
                        , state
                        , new ColumnSet
                        (
                            SystemForm.Schema.Attributes.name
                            , SystemForm.Schema.Attributes.objecttypecode
                            , SystemForm.Schema.Attributes.type
                            , SystemForm.Schema.Attributes.iscustomizable
                            , SystemForm.Schema.Attributes.formactivationstate
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadSystemForms(list);

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingFormsCompletedFormat1, list.Count());
        }

        private static IEnumerable<SystemForm> FilterList(IEnumerable<SystemForm> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.Id == tempGuid || ent.FormIdUnique == tempGuid);
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        return ent.ObjectTypeCode.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1 || ent.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1;
                    });
                }
            }

            return list;
        }

        private class SystemFormViewItem
        {
            public string ObjectTypeCode => SystemForm.ObjectTypeCode;

            public string FormType { get; }

            public string Name => SystemForm.Name;

            public string FormActivationState { get; }

            public SystemForm SystemForm { get; }

            public SystemFormViewItem(SystemForm systemForm)
            {
                systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out var formType);
                systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.formactivationstate, out var formactivationstate);

                this.FormType = formType;
                this.FormActivationState = formactivationstate;

                this.SystemForm = systemForm;
            }
        }

        private void LoadSystemForms(IEnumerable<SystemForm> results)
        {
            this.lstVwSystemForms.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(ent => ent.ObjectTypeCode)
                    .ThenBy(ent => ent.Type?.Value)
                    .ThenBy(ent => ent.Name)
                )
                {
                    var item = new SystemFormViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSystemForms.Items.Count == 1)
                {
                    this.lstVwSystemForms.SelectedItem = this.lstVwSystemForms.Items[0];
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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSystemForms.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSystemForms.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportSystemForm, btnExportAll };

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

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingSystemForms();
            }
        }

        private async void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowExistingSystemForms();
        }

        private SystemForm GetSelectedEntity()
        {
            return this.lstVwSystemForms.SelectedItems.OfType<SystemFormViewItem>().Count() == 1
                ? this.lstVwSystemForms.SelectedItems.OfType<SystemFormViewItem>().Select(e => e.SystemForm).SingleOrDefault() : null;
        }

        private List<SystemForm> GetSelectedEntitiesList()
        {
            return this.lstVwSystemForms.SelectedItems.OfType<SystemFormViewItem>().Select(e => e.SystemForm).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

                if (item != null)
                {
                    var service = await GetService();

                    if (service != null)
                    {
                        service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, item.SystemForm.Id);
                    }
                }
            }
        }

        //private async Task PerformExportMouseDoubleClickAsync(string folder, Guid idSystemForm, string entityName, string name)
        //{
        //    await PerformExportFormDescriptionToFileAsync(folder, idSystemForm, entityName, name);
        //}

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteActionAsync(Guid idSystemForm, string entityName, string name, Func<string, Guid, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSystemForm, entityName, name);
        }

        private async Task ExecuteJavaScriptObjectTypeAsync(Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType, Func<string, Guid, string, string, JavaScriptObjectType, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSystemForm, entityName, name, javaScriptObjectType);
        }

        private Task<string> CreateFileAsync(string folder, Guid formId, string entityName, string name, string fieldTitle, FileExtension extension, string formXml)
        {
            return Task.Run(() => CreateFile(folder, formId, entityName, name, fieldTitle, extension, formXml));
        }

        private string CreateFile(string folder, Guid formId, string entityName, string name, string fieldTitle, FileExtension extension, string formXml)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetSystemFormFileName(connectionData.Name, entityName, name, fieldTitle, extension);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    File.WriteAllText(filePath, formXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, connectionData.Name, SystemForm.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, connectionData.Name, SystemForm.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private async void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformExportAllXmlAsync);
        }

        private async Task PerformExportAllXmlAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            await PerformExportEntityDescriptionAsync(folder, idSystemForm, entityName, name);

            await PerformExportFormDescriptionToFileAsync(folder, idSystemForm, entityName, name);

            await PerformExportXmlToFileAsync(folder, idSystemForm, entityName, name, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, FileExtension.xml);

            await PerformExportXmlToFileAsync(folder, idSystemForm, entityName, name, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, FileExtension.json);
        }

        private async Task ExecuteActionEntityAsync(Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, FileExtension extension, Func<string, Guid, string, string, string, string, FileExtension, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idSystemForm, entityName, name, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportXmlToFileAsync(string folder, Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, FileExtension extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(fieldName));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml, StringComparison.InvariantCultureIgnoreCase))
                {
                    xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                        xmlContent
                        , _commonConfig
                        , XmlOptionsControls.FormXmlOptions
                        , schemaName: AbstractDynamicCommandXsdSchemas.FormXmlSchema
                        , formId: idSystemForm
                        , entityName: entityName
                    );
                }
                else if (string.Equals(fieldName, SystemForm.Schema.Attributes.formjson, StringComparison.InvariantCultureIgnoreCase))
                {
                    xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                }

                string filePath = await CreateFileAsync(folder, idSystemForm, entityName, name, fieldTitle, extension, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idSystemForm, string entityName, string name, string fieldName, string fieldTitle, FileExtension extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionUpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(fieldName));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var tempDoc))
                    {
                        xmlContent = tempDoc.ToString();
                    }
                }
                else if (string.Equals(fieldName, SystemForm.Schema.Attributes.formjson, StringComparison.InvariantCultureIgnoreCase))
                {
                    xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                }

                {
                    string backUpXmlContent = xmlContent;

                    if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml, StringComparison.InvariantCultureIgnoreCase))
                    {
                        backUpXmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            backUpXmlContent
                            , _commonConfig
                            , XmlOptionsControls.FormXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.FormXmlSchema
                            , formId: idSystemForm
                            , entityName: entityName
                        );
                    }
                    else if (string.Equals(fieldName, SystemForm.Schema.Attributes.formjson, StringComparison.InvariantCultureIgnoreCase))
                    {
                        backUpXmlContent = ContentComparerHelper.FormatJson(backUpXmlContent);
                    }

                    await CreateFileAsync(folder, idSystemForm, entityName, name, fieldTitle + " BackUp", extension, backUpXmlContent);
                }

                var newText = string.Empty;

                {
                    bool? dialogResult = false;

                    this.Dispatcher.Invoke(() =>
                    {
                        var form = new WindowTextField("Enter " + fieldTitle, fieldTitle, xmlContent);

                        dialogResult = form.ShowDialog();

                        newText = form.FieldText;
                    });

                    if (dialogResult.GetValueOrDefault() == false)
                    {
                        ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                        return;
                    }
                }

                if (string.Equals(fieldName, SystemForm.Schema.Attributes.formxml))
                {
                    newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.ValidatingXmlForFieldFormat1, fieldName);

                    if (!ContentComparerHelper.TryParseXmlDocument(newText, out var doc))
                    {
                        ToggleControls(service.ConnectionData, true, Properties.OutputStrings.TextIsNotValidXml);

                        _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                        return;
                    }

                    bool validateResult = await SystemFormRepository.ValidateXmlDocumentAsync(service.ConnectionData, _iWriteToOutput, doc);

                    if (!validateResult)
                    {
                        var dialogResult = MessageBoxResult.Cancel;

                        this.Dispatcher.Invoke(() =>
                        {
                            dialogResult = MessageBox.Show(Properties.MessageBoxStrings.ContinueOperation, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        });

                        if (dialogResult != MessageBoxResult.OK)
                        {
                            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ValidatingXmlForFieldFailedFormat1, fieldName);
                            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                            return;
                        }
                    }

                    newText = doc.ToString(SaveOptions.DisableFormatting);
                }

                var updateEntity = new SystemForm
                {
                    Id = idSystemForm
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                var repositoryPublish = new PublishActionsRepository(service);

                UpdateStatus(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);
                await repositoryPublish.PublishDashboardsAsync(new[] { idSystemForm });

                if (entityName.IsValidEntityName())
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

                    await repositoryPublish.PublishEntitiesAsync(new[] { entityName });
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionUpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformExportEntityDescriptionAsync);
        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformEntityEditor);
        }

        private async void miActivateSystemForms_Click(object sender, RoutedEventArgs e)
        {
            await ChangeSystemFormsState(SystemForm.Schema.OptionSets.formactivationstate.Active_1);
        }

        private async void miDeactivateSystemForms_Click(object sender, RoutedEventArgs e)
        {
            await ChangeSystemFormsState(SystemForm.Schema.OptionSets.formactivationstate.Inactive_0);
        }

        private async Task ChangeSystemFormsState(SystemForm.Schema.OptionSets.formactivationstate stateCode)
        {
            var selectedSystemForms = GetSelectedEntitiesList()
                .Where(e => e.FormActivationStateEnum != stateCode)
                .ToList();

            if (!selectedSystemForms.Any())
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ChangingEntityStateFormat1, SystemForm.EntityLogicalName);

            foreach (var systemForm in selectedSystemForms)
            {
                try
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.ChangingEntityStateFormat1, systemForm.Name);

                    var updateEntity = new Entity(SystemForm.EntityLogicalName)
                    {
                        Id = systemForm.Id,
                    };

                    updateEntity.Attributes[SystemForm.Schema.Attributes.formactivationstate] = new OptionSetValue((int)stateCode);
                    await service.UpdateAsync(updateEntity);
                }
                catch (Exception ex)
                {
                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.ChangingEntityStateFailedFormat1, systemForm.Name);

                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            var hashEntities = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var systemForm in selectedSystemForms)
            {
                UpdateStatus(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingSystemFormFormat3, service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name);

                if (systemForm.ObjectTypeCode.IsValidEntityName())
                {
                    hashEntities.Add(systemForm.ObjectTypeCode);
                }
            }

            var repositoryPublish = new PublishActionsRepository(service);

            try
            {
                await repositoryPublish.PublishDashboardsAsync(selectedSystemForms.Select(f => f.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            if (hashEntities.Any())
            {
                UpdateStatus(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, string.Join(",", hashEntities.OrderBy(s => s)));

                try
                {
                    await repositoryPublish.PublishEntitiesAsync(hashEntities);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ChangingEntityStateCompletedFormat1, SystemForm.EntityLogicalName);

            await ShowExistingSystemForms();
        }

        private async void mIDeleteSystemForm_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformDeleteEntity);
        }

        private async Task PerformExportEntityDescriptionAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, entityName, name, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, ColumnSetInstances.AllColumns);

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, systemForm, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                    , service.ConnectionData.Name
                    , systemForm.LogicalName
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

        private async Task PerformEntityEditor(string folder, Guid idSystemForm, string entityName, string name)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, SystemForm.EntityLogicalName, idSystemForm);
        }

        private async Task PerformDeleteEntity(string folder, Guid idSystemForm, string entityName, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, SystemForm.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionDeletingEntityFormat2, service.ConnectionData.Name, SystemForm.EntityLogicalName);

            try
            {
                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, SystemForm.EntityLogicalName, idSystemForm);

                await service.DeleteAsync(SystemForm.EntityLogicalName, idSystemForm);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionDeletingEntityCompletedFormat2, service.ConnectionData.Name, SystemForm.EntityLogicalName);

            await ShowExistingSystemForms();
        }

        private async void mIPublishSystemForm_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformPublishSystemFormAsync);
        }

        private async Task PerformPublishSystemFormAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionPublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);

            var repository = new PublishActionsRepository(service);

            try
            {
                await repository.PublishDashboardsAsync(new[] { idSystemForm });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingSystemFormCompletedFormat3, service.ConnectionData.Name, entityName, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingSystemFormFailedFormat3, service.ConnectionData.Name, entityName, name);
            }

            if (entityName.IsValidEntityName())
            {
                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

                try
                {
                    await repository.PublishEntitiesAsync(new[] { entityName });

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
                }
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingSystemFormFormat3, service.ConnectionData.Name, entityName, name);
        }

        private async void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformPublishEntityAsync);
        }

        private async Task PerformPublishEntityAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, entityName);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityName);
        }

        private async void mIExportSystemFormDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformExportFormDescriptionToFileAsync);
        }

        private async Task PerformExportFormDescriptionToFileAsync(string folder, Guid idSystemForm, string entityName, string name)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingSystemFormDescriptionFormat2, entityName, name);

            var descriptor = GetSolutionComponentDescriptor(service);
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            string fileName = EntityFileNameFormatter.GetSystemFormFileName(service.ConnectionData.Name, entityName, name, "FormDescription", FileExtension.txt);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(SystemForm.Schema.Attributes.type, SystemForm.Schema.Attributes.formxml));

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    XElement doc = XElement.Parse(formXml);

                    string desc = await handler.GetFormDescriptionAsync(doc, entityName, idSystemForm, name, systemForm.FormattedValues[SystemForm.Schema.Attributes.type]);

                    File.WriteAllText(filePath, desc, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "FormDescription", filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingSystemFormDescriptionCompletedFormat2, entityName, name);
        }

        private async void mIExportSystemFormFormXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private async void mIExportSystemFormFormJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, FileExtension.json, PerformExportXmlToFileAsync);
        }

        private async void mIExportSystemFormWebResources_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformDownloadWebResources);
        }

        private async Task PerformDownloadWebResources(string folder, Guid idSystemForm, string entityName, string name)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DownloadingSystemFormWebResourcesFormat2, entityName, name);

            var descriptor = GetSolutionComponentDescriptor(service);
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(SystemForm.Schema.Attributes.formxml));

            string formXml = systemForm.FormXml;

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            List<string> files = new List<string>();

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    XElement doc = XElement.Parse(formXml);

                    List<string> webResources = handler.GetFormLibraries(doc);

                    foreach (var resName in webResources)
                    {
                        var webresource = await webResourceRepository.FindByNameAsync(resName, ".js");

                        var filePath = await CreateWebResourceAsync(folder, service.ConnectionData, resName, webresource);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            files.Add(filePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            foreach (var filePath in files)
            {
                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DownloadingSystemFormWebResourcesCompletedFormat2, entityName, name);
        }

        private Task<string> CreateWebResourceAsync(string folder, ConnectionData connectionData, string resName, WebResource webresource)
        {
            return Task.Run(() => CreateWebResource(folder, connectionData, resName, webresource));
        }

        private string CreateWebResource(string folder, ConnectionData connectionData, string resName, WebResource webresource)
        {
            if (webresource == null)
            {
                return string.Empty;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, "Web-resource founded by name: {0}", resName);

            this._iWriteToOutput.WriteToOutput(connectionData, "Starting downloading {0}", webresource.Name);

            string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

            var contentWebResource = webresource.Content ?? string.Empty;

            var array = Convert.FromBase64String(contentWebResource);

            string localFileName = string.Format("{0}.{1}", connectionData.Name, webResourceFileName);
            string localFilePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(localFileName));

            File.WriteAllBytes(localFilePath, array);

            this._iWriteToOutput.WriteToOutput(connectionData, "Web-resource '{0}' has downloaded to {1}.", webresource.Name, localFilePath);

            return localFilePath;
        }

        private async void mIExportSystemFormEntityJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.JsonObject, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private async void mIExportSystemFormEntityJavaScriptFileAnonymousConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.AnonymousConstructor, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private async void mIExportSystemFormEntityJavaScriptFileTypeConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.TypeConstructor, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private async Task PerformCreateEntityJavaScriptFileBasedOnForm(string folder, Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityJavaScriptFileOnFormFormat2, entityName, name);

            var descriptor = GetSolutionComponentDescriptor(service);
            var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, ColumnSetInstances.AllColumns);

            SystemFormRepository.GetTypeName(systemForm.TypeEnum, out var formTypeName, out var formTypeConstructorName);

            string objectName = string.Format("{0}_form_{1}", entityName, formTypeName);
            string constructorName = string.Format("{0}Form{1}", entityName, formTypeConstructorName);

            string fileName = string.Format("{0}.{1}_form_{2}.js", service.ConnectionData.Name, entityName, formTypeName);

            if (this._selectedItem != null)
            {
                fileName = string.Format("{0}_form_{1}.js", entityName, formTypeName);
            }

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                    var config = new CreateFileJavaScriptConfiguration(fileGenerationOptions);

                    XElement doc = XElement.Parse(formXml);

                    FormInformation formInfo = handler.GetFormInformation(doc);

                    systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out string typeName);

                    formInfo.FormId = systemForm.Id;
                    formInfo.FormName = systemForm.Name;
                    formInfo.FormType = systemForm.Type?.Value;
                    formInfo.FormTypeName = typeName;

                    string filePath = Path.Combine(folder, fileName);

                    if (this._selectedItem != null)
                    {
                        filePath = FileOperations.CheckFilePathUnique(filePath);
                    }

                    var stringBuilder = new StringBuilder();

                    using (var stringWriter = new StringWriter(stringBuilder))
                    {
                        var handlerCreate = new CreateFormTabsJavaScriptHandler(stringWriter, config, javaScriptObjectType, service);

                        await handlerCreate.WriteContentAsync(entityName, objectName, constructorName, formInfo);
                    }

                    File.WriteAllText(filePath, stringBuilder.ToString(), new UTF8Encoding(false));

                    AddFileToVSProject(_selectedItem, filePath);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "Entity Metadata", filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityJavaScriptFileOnFormCompletedFormat2, entityName, name);
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSystemForms();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            Popup[] _popupArray = new Popup[] { _popupExportXmlOptions };

            foreach (var popup in _popupArray)
            {
                if (popup.IsOpen)
                {
                    popup.IsOpen = false;
                    e.Handled = true;

                    return false;
                }
            }

            return true;
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.SystemForm, entity.Id);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, entity.Id);
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
            var entitiesList = GetSelectedEntitiesList()
                .Select(e => e.Id);

            if (!entitiesList.Any())
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var descriptor = GetSolutionComponentDescriptor(service);

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.SystemForm, entitiesList, null, withSelect);
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
            var entitiesList = GetSelectedEntitiesList();

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData == null)
            {
                return;
            }

            var entityMetadataIdList = entitiesList
                .Where(e => e.ObjectTypeCode.IsValidEntityName())
                .Select(e => connectionData.GetEntityMetadataId(e.ObjectTypeCode))
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList()
                ;

            if (!entityMetadataIdList.Any())
            {
                return;
            }

            await AddEntityMetadataToSolution(
                GetSelectedConnection()
                , entityMetadataIdList
                , withSelect
                , solutionUniqueName
                , rootComponentBehavior
            );
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

            SystemFormViewItem nodeItem = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            ActivateControls(items, (nodeItem.SystemForm.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            bool hasEntity = nodeItem.SystemForm.ObjectTypeCode.IsValidEntityName();
            ActivateControls(items, hasEntity, "contMnEntity");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddEntityToSolutionLastDoNotIncludeSubcomponents");

            FillLastSolutionItems(connectionData, items, hasEntity, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddEntityToSolutionLastIncludeAsShellOnly");

            ActivateControls(items, hasEntity && connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddEntityToSolutionLast");

            var selectedSystemForms = GetSelectedEntitiesList();

            var hasEnabledSystemForms = selectedSystemForms.Any(s => s.FormActivationStateEnum == SystemForm.Schema.OptionSets.formactivationstate.Active_1);
            var hasDisabledSystemForms = selectedSystemForms.Any(s => s.FormActivationStateEnum == SystemForm.Schema.OptionSets.formactivationstate.Inactive_0);

            ActivateControls(items, hasDisabledSystemForms, "miActivateSystemForms");
            ActivateControls(items, hasEnabledSystemForms, "miDeactivateSystemForms");
        }

        private void tSDDBExportSystemForm_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var systemForm = GetSelectedEntity();

            var items = tSDDBExportSystemForm.Items.OfType<Control>();

            ActivateControls(items, (systemForm?.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");

            var selectedSystemForms = GetSelectedEntitiesList();

            var hasEnabledSystemForms = selectedSystemForms.Any(s => s.FormActivationStateEnum == SystemForm.Schema.OptionSets.formactivationstate.Active_1);
            var hasDisabledSystemForms = selectedSystemForms.Any(s => s.FormActivationStateEnum == SystemForm.Schema.OptionSets.formactivationstate.Inactive_0);

            ActivateControls(items, hasDisabledSystemForms, "miActivateSystemForms");
            ActivateControls(items, hasEnabledSystemForms, "miDeactivateSystemForms");
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
            var descriptor = GetSolutionComponentDescriptor(service);

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , descriptor
                , _commonConfig
                , (int)ComponentType.SystemForm
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

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.SystemForm
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
                await ShowExistingSystemForms();
            }
        }

        private async void mIUpdateSystemFormFormXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, FileExtension.xml, PerformUpdateEntityField);
        }

        private async void mIUpdateSystemFormFormJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, FileExtension.json, PerformUpdateEntityField);
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.ObjectTypeCode);
            }
        }

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                this._iWriteToOutput.OpenFetchXmlFile(connectionData, _commonConfig, entity.ObjectTypeCode);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
                || !entity.ObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(entity.ObjectTypeCode);
            }
        }

        private void miExportXmlOptions_Click(object sender, RoutedEventArgs e)
        {
            this._popupExportXmlOptions.IsOpen = true;
            this._popupExportXmlOptions.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_popupExportXmlOptions.IsOpen)
            {
                _popupExportXmlOptions.IsOpen = false;
                this.Focus();
            }
        }

        private void miFileGenerationJavaScriptOptions_Click(object sender, RoutedEventArgs e)
        {
            var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

            this._optionsControlJavaScript.BindFileGenerationOptions(fileGenerationOptions);

            _popupFileGenerationJavaScriptOptions.IsOpen = true;
            _popupFileGenerationJavaScriptOptions.Child.Focus();
        }

        private void optionsControlJavaScript_CloseClicked(object sender, EventArgs e)
        {
            if (_popupFileGenerationJavaScriptOptions.IsOpen)
            {
                _popupFileGenerationJavaScriptOptions.IsOpen = false;
                this.Focus();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        #region Copy to Clipboard

        private async void mISystemFormCopyFormObjectsEnumsForJsonObject_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.JsonObject, PerformCopyFormObjectsEnums);
        }

        private async void mISystemFormCopyFormObjectsEnumsForTypeConstructor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.TypeConstructor, PerformCopyFormObjectsEnums);
        }

        private async Task PerformCopyFormObjectsEnums(string folder, Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormFormat2, entityName, name);

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(SystemForm.Schema.Attributes.formxml));

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                    var config = new CreateFileJavaScriptConfiguration(fileGenerationOptions);

                    var doc = XElement.Parse(formXml);

                    var descriptor = GetSolutionComponentDescriptor(service);
                    var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

                    FormInformation formInfo = handler.GetFormInformation(doc);

                    var stringBuilder = new StringBuilder();

                    using (var writer = new StringWriter(stringBuilder))
                    {
                        var handlerCreate = new CreateFormTabsJavaScriptHandler(writer, config, javaScriptObjectType, service);

                        handlerCreate.WriteContentOnlyForm(formInfo);
                    }

                    ClipboardHelper.SetText(stringBuilder.ToString().Trim(' ', '\r', '\n'));

                    //this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "Entity Metadata", filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormCompletedFormat2, entityName, name);
        }

        private async void mISystemFormCopyFormJavaScriptTag_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.TypeConstructor, PerformCopyFormJavaScriptTagBasedOnForm);
        }

        private async Task PerformCopyFormJavaScriptTagBasedOnForm(string folder, Guid idSystemForm, string entityName, string name, JavaScriptObjectType javaScriptObjectType)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormFormat2, entityName, name);

            var descriptor = GetSolutionComponentDescriptor(service);

            var repository = new SystemFormRepository(service);

            var systemForm = await repository.GetByIdAsync(idSystemForm, ColumnSetInstances.AllColumns);

            string formXml = systemForm.FormXml;

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    var fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();

                    var config = new CreateFileJavaScriptConfiguration(fileGenerationOptions);

                    var stringBuilder = new StringBuilder();

                    using (var writer = new StringWriter(stringBuilder))
                    {
                        var handlerCreate = new CreateFormTabsJavaScriptHandler(writer, config, javaScriptObjectType, service);

                        systemForm.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out string typeName);

                        handlerCreate.WriteFormJavaScriptTag(systemForm.ObjectTypeCode, systemForm.Id, systemForm.Name, systemForm.Type?.Value, typeName);
                    }

                    ClipboardHelper.SetText(stringBuilder.ToString().Trim(' ', '\r', '\n'));
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, SystemForm.Schema.Headers.formxml);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CopyingEntityJavaScriptContentOnFormCompletedFormat2, entityName, name);
        }

        private void mISystemFormCopyFormId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SystemFormViewItem>(e, ent => ent.SystemForm.Id.ToString());
        }

        private void mISystemFormCopyFormEntityName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SystemFormViewItem>(e, ent => ent.ObjectTypeCode);
        }

        private void mISystemFormCopyFormTypeCode_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SystemFormViewItem>(e, ent => ent.SystemForm.Type?.Value.ToString());
        }

        private void mISystemFormCopyFormTypeName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SystemFormViewItem>(e, ent => ent.FormType);
        }

        private void mISystemFormCopyFormName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SystemFormViewItem>(e, ent => ent.Name);
        }

        #endregion Copy to Clipboard

        private async void hyperlinkFormDescription_Click(object sender, RoutedEventArgs e)
        {
            SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformExportFormDescriptionToFileAsync);
        }

        private async void hyperlinkFormXml_Click(object sender, RoutedEventArgs e)
        {
            SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            await ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private async void hyperlinkFormJson_Click(object sender, RoutedEventArgs e)
        {
            SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            await ExecuteActionEntityAsync(entity.Id, entity.ObjectTypeCode, entity.Name, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, FileExtension.json, PerformExportXmlToFileAsync);
        }

        private async void hyperlinkJavaScriptFileJsonObject_Click(object sender, RoutedEventArgs e)
        {
            SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.JsonObject, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private async void hyperlinkJavaScriptFileAnonymousConstructor_Click(object sender, RoutedEventArgs e)
        {
            SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.AnonymousConstructor, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private async void hyperlinkJavaScriptFileTypeConstructor_Click(object sender, RoutedEventArgs e)
        {
            SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            await ExecuteJavaScriptObjectTypeAsync(entity.Id, entity.ObjectTypeCode, entity.Name, JavaScriptObjectType.TypeConstructor, PerformCreateEntityJavaScriptFileBasedOnForm);
        }

        private async void hyperlinkPublishSystemForm_Click(object sender, RoutedEventArgs e)
        {
            SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformPublishSystemFormAsync);
        }

        private async void hyperlinkPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            SystemFormViewItem item = GetItemFromRoutedDataContext<SystemFormViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.SystemForm;

            if (entity == null
                || !entity.ObjectTypeCode.IsValidEntityName()
            )
            {
                return;
            }

            await ExecuteActionAsync(entity.Id, entity.ObjectTypeCode, entity.Name, PerformPublishEntityAsync);
        }

        private void lstVwSystemForms_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwSystemForms_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteSystemForm_Click(sender, e);
        }
    }
}