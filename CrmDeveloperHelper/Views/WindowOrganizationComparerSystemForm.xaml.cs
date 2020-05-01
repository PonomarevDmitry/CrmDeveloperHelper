using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerSystemForm : WindowWithConnectionList
    {
        private readonly Dictionary<Guid, SolutionComponentDescriptor> _cacheDescription = new Dictionary<Guid, SolutionComponentDescriptor>();

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowOrganizationComparerSystemForm(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity
            , string filter
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            LoadEntityNames(cmBEntityName, connection1, connection2);

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.FormXmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            this.Resources["ConnectionName1"] = connection1.Name;
            this.Resources["ConnectionName2"] = connection2.Name;

            LoadFromConfig();

            if (!string.IsNullOrEmpty(filter))
            {
                txtBFilter.Text = filter;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            cmBEntityName.Text = filterEntity;

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSystemForms.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            FillExplorersMenuItems();

            this.DecreaseInit();

            ShowExistingSystemForms();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1
                , getEntityName: GetEntityName
                , getSystemFormName: GetSystemFormName1
            );

            var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2
                , getEntityName: GetEntityName
                , getSystemFormName: GetSystemFormName2
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2())
                , getEntityName: GetEntityName
                , getSystemFormName: GetSystemFormName1
            );

            explorersHelper1.FillExplorers(miExplorers1);
            explorersHelper2.FillExplorers(miExplorers2);

            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu contextMenu
            )
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    if (string.Equals(item.Uid, "miExplorers1", StringComparison.InvariantCultureIgnoreCase))
                    {
                        explorersHelper1.FillExplorers(item);
                    }
                    else if (string.Equals(item.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase))
                    {
                        explorersHelper2.FillExplorers(item);
                    }
                    else if (string.Equals(item.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase))
                    {
                        compareWindowsHelper.FillCompareWindows(item);
                    }
                }
            }
        }

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.EntityName;
        }

        private string GetSystemFormName1()
        {
            var entity = GetSelectedEntity();

            return entity?.FormName1;
        }

        private string GetSystemFormName2()
        {
            var entity = GetSelectedEntity();

            return entity?.FormName2;
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

            BindingOperations.ClearAllBindings(cmBConnection1);
            cmBConnection1.Items.DetachFromSourceCollection();
            cmBConnection1.DataContext = null;
            cmBConnection1.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBConnection2);
            cmBConnection2.Items.DetachFromSourceCollection();
            cmBConnection2.DataContext = null;
            cmBConnection2.ItemsSource = null;

            base.OnClosed(e);
        }

        private ConnectionData GetConnection1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private ConnectionData GetConnection2()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService1()
        {
            return GetOrganizationService(GetConnection1());
        }

        private Task<IOrganizationServiceExtented> GetService2()
        {
            return GetOrganizationService(GetConnection2());
        }

        private SolutionComponentDescriptor GetDescriptor(IOrganizationServiceExtented service)
        {
            if (!_cacheDescription.ContainsKey(service.ConnectionData.ConnectionId))
            {
                _cacheDescription[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service);
            }

            return _cacheDescription[service.ConnectionData.ConnectionId];
        }

        private async Task ShowExistingSystemForms()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingForms);

            this._itemsSource.Clear();

            IEnumerable<LinkedEntities<SystemForm>> list = Enumerable.Empty<LinkedEntities<SystemForm>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    string entityName = string.Empty;
                    SystemForm.Schema.OptionSets.formactivationstate? state = null;

                    this.Dispatcher.Invoke(() =>
                    {
                        if (!string.IsNullOrEmpty(cmBEntityName.Text)
                            && cmBEntityName.Items.Contains(cmBEntityName.Text)
                        )
                        {
                            entityName = cmBEntityName.Text.Trim().ToLower();
                        }

                        if (cmBFormActivationState.SelectedItem is SystemForm.Schema.OptionSets.formactivationstate comboBoxItem)
                        {
                            state = comboBoxItem;
                        }
                    });

                    string filterEntity = null;

                    if (service1.ConnectionData.IsValidEntityName(entityName)
                        && service2.ConnectionData.IsValidEntityName(entityName)
                    )
                    {
                        filterEntity = entityName;
                    }

                    var columnSet = new ColumnSet
                    (
                        SystemForm.Schema.Attributes.name
                        , SystemForm.Schema.Attributes.objecttypecode
                        , SystemForm.Schema.Attributes.type
                        , SystemForm.Schema.Attributes.formactivationstate
                    );

                    var temp = new List<LinkedEntities<SystemForm>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new SystemFormRepository(service1);
                        var repository2 = new SystemFormRepository(service2);

                        var task1 = repository1.GetListAsync(filterEntity, state, columnSet);
                        var task2 = repository2.GetListAsync(filterEntity, state, columnSet);

                        var list1 = await task1;
                        var list2 = await task2;

                        foreach (var form1 in list1)
                        {
                            var form2 = list2.FirstOrDefault(form => form.Id == form1.Id);

                            if (form2 == null)
                            {
                                continue;
                            }

                            var item = new LinkedEntities<SystemForm>(form1, form2);

                            temp.Add(item);
                        }
                    }
                    else
                    {
                        var repository1 = new SystemFormRepository(service1);

                        var list1 = await repository1.GetListAsync(filterEntity, state, columnSet);

                        foreach (var form1 in list1)
                        {
                            var item = new LinkedEntities<SystemForm>(form1, null);

                            temp.Add(item);
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            var textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadEntities(list);

            ToggleControls(true, Properties.OutputStrings.LoadingFormsCompletedFormat1, list.Count());
        }

        private static IEnumerable<LinkedEntities<SystemForm>> FilterList(IEnumerable<LinkedEntities<SystemForm>> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent =>
                        ent.Entity1?.Id == tempGuid
                        || ent.Entity2?.Id == tempGuid
                        || ent.Entity1?.FormIdUnique == tempGuid
                        || ent.Entity2?.FormIdUnique == tempGuid
                    );
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var type1 = ent.Entity1?.ObjectTypeCode ?? string.Empty;
                        var name1 = ent.Entity1?.Name ?? string.Empty;

                        var type2 = ent.Entity2?.ObjectTypeCode ?? string.Empty;
                        var name2 = ent.Entity2?.Name ?? string.Empty;

                        return type1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || type2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            ;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string EntityName { get; private set; }

            public string FormType { get; private set; }

            public string FormName1 { get; private set; }

            public string FormName2 { get; private set; }

            public string FormState1 { get; private set; }

            public string FormState2 { get; private set; }

            public LinkedEntities<SystemForm> Link { get; private set; }

            public EntityViewItem(string entityName, string formType, LinkedEntities<SystemForm> link
                , string formName1, string formName2
                , string formState1, string formState2
            )
            {
                this.EntityName = entityName;
                this.FormName1 = formName1;
                this.FormName2 = formName2;
                this.FormState1 = formState1;
                this.FormState2 = formState2;
                this.FormType = formType;
                this.Link = link;
            }
        }

        private void LoadEntities(IEnumerable<LinkedEntities<SystemForm>> results)
        {
            this.lstVwSystemForms.Dispatcher.Invoke(() =>
            {
                foreach (var link in results
                      .OrderBy(ent => ent.Entity1.ObjectTypeCode)
                      .ThenBy(ent => ent.Entity1.Type.Value)
                      .ThenBy(ent => ent.Entity1.Name)
                      .ThenBy(ent => ent.Entity1.Name)
                  )
                {
                    link.Entity1.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.type, out var formType);

                    var formState2 = string.Empty;

                    link.Entity1.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.formactivationstate, out var formState1);

                    if (link.Entity2 != null)
                    {
                        link.Entity2.FormattedValues.TryGetValue(SystemForm.Schema.Attributes.formactivationstate, out formState2);
                    }

                    var item = new EntityViewItem(link.Entity1.ObjectTypeCode, formType, link
                        , link.Entity1.Name, link.Entity2?.Name
                        , formState1, formState2
                    );

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSystemForms.Items.Count == 1)
                {
                    this.lstVwSystemForms.SelectedItem = this.lstVwSystemForms.Items[0];
                }
            });
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(null, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            ToggleControls(enabled, statusFormat, args);
        }

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.cmBConnection1, this.cmBConnection2);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSystemForms.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSystemForms.SelectedItems.Count > 0;

                    var item = (this.lstVwSystemForms.SelectedItems[0] as EntityViewItem);

                    tSDDBShowDifference.IsEnabled = enabled && item.Link.Entity1 != null && item.Link.Entity2 != null;
                    tSDDBConnection1.IsEnabled = enabled && item.Link.Entity1 != null;
                    tSDDBConnection2.IsEnabled = enabled && item.Link.Entity2 != null;
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
                ShowExistingSystemForms();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingSystemForms();
        }

        private EntityViewItem GetSelectedEntity()
        {
            return this.lstVwSystemForms.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwSystemForms.SelectedItems.OfType<EntityViewItem>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    ExecuteActionOnPair(item.Link, false, PerformShowingDifferenceAllAsync);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteActionOnPair(LinkedEntities<SystemForm> linked, bool showAllways, Func<LinkedEntities<SystemForm>, bool, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(linked, showAllways);
        }

        private Task<string> CreateFileAsync(ConnectionData connectionData, Guid idSystemForm, string entityName, string name, string fieldTitle, string extension, string formXml)
        {
            return Task.Run(() => CreateFile(connectionData, idSystemForm, entityName, name, fieldTitle, extension, formXml));
        }

        private string CreateFile(ConnectionData connectionData, Guid idSystemForm, string entityName, string name, string fieldTitle, string extension, string formXml)
        {
            string fileName = EntityFileNameFormatter.GetSystemFormFileName(connectionData.Name, entityName, name, fieldTitle, extension);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(formXml))
            {
                try
                {
                    if (string.Equals(extension, "xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        formXml = ContentComparerHelper.FormatXmlByConfiguration(
                            formXml
                            , _commonConfig
                            , XmlOptionsControls.FormXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.SchemaFormXml
                            , formId: idSystemForm
                            , entityName: entityName
                        );
                    }
                    else if (string.Equals(extension, "json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        formXml = ContentComparerHelper.FormatJson(formXml);
                    }

                    File.WriteAllText(filePath, formXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SystemForm.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;

                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SystemForm.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private Task<string> CreateDescriptionFileAsync(ConnectionData connectionData, string entityName, string name, string fieldTitle, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionData, entityName, name, fieldTitle, description));
        }

        private string CreateDescriptionFile(ConnectionData connectionData, string entityName, string name, string fieldTitle, string description)
        {
            string fileName = EntityFileNameFormatter.GetSystemFormFileName(connectionData.Name, entityName, name, fieldTitle, "txt");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SystemForm.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SystemForm.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private void btnShowDifferenceAll_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnPair(link.Link, false, PerformShowingDifferenceAllAsync);
        }

        private async Task PerformShowingDifferenceAllAsync(LinkedEntities<SystemForm> linked, bool showAllways)
        {
            await PerformShowingDifferenceFormDescriptionAsync(linked, showAllways);

            await PerformShowingDifferenceEntityDescriptionAsync(linked, showAllways);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, "xml");

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, "json");
        }

        private void mIShowDifferenceEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnPair(link.Link, true, PerformShowingDifferenceEntityDescriptionAsync);
        }

        private async Task PerformShowingDifferenceEntityDescriptionAsync(LinkedEntities<SystemForm> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceEntityDescription);

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var repository1 = new SystemFormRepository(service1);
                    var repository2 = new SystemFormRepository(service2);

                    var systemForm1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var systemForm2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(systemForm1, EntityFileNameFormatter.SystemFormIgnoreFields);
                    var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(systemForm2, EntityFileNameFormatter.SystemFormIgnoreFields);

                    if (showAllways || desc1 != desc2)
                    {
                        string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, systemForm1.ObjectTypeCode, systemForm1.Name, EntityFileNameFormatter.Headers.EntityDescription, desc1);
                        string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, systemForm2.ObjectTypeCode, systemForm2.Name, EntityFileNameFormatter.Headers.EntityDescription, desc2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(null, filePath1);

                            this._iWriteToOutput.PerformAction(null, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionFailed);
            }
        }

        private void mIShowDifferenceFormDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnPair(link.Link, true, PerformShowingDifferenceFormDescriptionAsync);
        }

        private async Task PerformShowingDifferenceFormDescriptionAsync(LinkedEntities<SystemForm> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceFormDescriptionFormat2, linked.Entity1.ObjectTypeCode, linked.Entity1.Name);

            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null)
            {
                var descriptor1 = GetDescriptor(service1);
                var descriptor2 = GetDescriptor(service2);

                var handler1 = new FormDescriptionHandler(descriptor1, new DependencyRepository(service1))
                {
                    WithManagedInfo = false,
                    WithDependentComponents = false,
                };
                var handler2 = new FormDescriptionHandler(descriptor2, new DependencyRepository(service2))
                {
                    WithManagedInfo = false,
                    WithDependentComponents = false,
                };

                var repository1 = new SystemFormRepository(service1);
                var repository2 = new SystemFormRepository(service2);

                var systemForm1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                var systemForm2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                string formDescritpion1 = await handler1.GetFormDescriptionAsync(XElement.Parse(systemForm1.FormXml), systemForm1.ObjectTypeCode, systemForm1.Id, systemForm1.Name, systemForm1.FormattedValues[SystemForm.Schema.Attributes.type]);
                string formDescritpion2 = await handler2.GetFormDescriptionAsync(XElement.Parse(systemForm2.FormXml), systemForm2.ObjectTypeCode, systemForm2.Id, systemForm2.Name, systemForm2.FormattedValues[SystemForm.Schema.Attributes.type]);

                if (showAllways || formDescritpion1 != formDescritpion2)
                {
                    string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, linked.Entity1.ObjectTypeCode, linked.Entity1.Name, "FormDescription", formDescritpion1);
                    string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, linked.Entity2.ObjectTypeCode, linked.Entity2.Name, "FormDescription", formDescritpion2);

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                        this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                    }
                }
            }

            ToggleControls(true, Properties.OutputStrings.ShowingDifferenceFormDescriptionCompletedFormat2, linked.Entity1.ObjectTypeCode, linked.Entity1.Name);
        }

        private void mIShowDifferenceFormXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, "xml", PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceFormJson_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, "json", PerformShowingDifferenceSingleXmlAsync);
        }

        private void ExecuteActionLinked(LinkedEntities<SystemForm> linked, bool showAllways, string fieldName, string fieldTitle, string extension, Func<LinkedEntities<SystemForm>, bool, string, string, string, Action<XElement>, Task> action, Action<XElement> actionXml = null)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(linked, showAllways, fieldName, fieldTitle, extension, actionXml);
        }

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<SystemForm> linked, bool showAllways, string fieldName, string fieldTitle, string extension, Action<XElement> action = null)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceXmlForFieldFormat1, fieldName);

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var repository1 = new SystemFormRepository(service1);
                    var repository2 = new SystemFormRepository(service2);

                    var systemForm1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(fieldName));
                    var systemForm2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(fieldName));

                    string xml1 = systemForm1.GetAttributeValue<string>(fieldName);
                    string xml2 = systemForm2.GetAttributeValue<string>(fieldName);

                    if (showAllways || !ContentComparerHelper.CompareXML(xml1, xml2, false, action).IsEqual)
                    {
                        string filePath1 = await CreateFileAsync(service1.ConnectionData, linked.Entity1.Id, linked.Entity1.ObjectTypeCode, linked.Entity1.Name, fieldTitle, extension, xml1);
                        string filePath2 = await CreateFileAsync(service2.ConnectionData, linked.Entity2.Id, linked.Entity2.ObjectTypeCode, linked.Entity2.Name, fieldTitle, extension, xml2);

                        if (!File.Exists(filePath1))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service1.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm1.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (!File.Exists(filePath2))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service2.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm2.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceXmlForFieldCompletedFormat1, fieldName);

            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceXmlForFieldFailedFormat1, fieldName);
            }
        }

        private void mIShowDifferenceWebResouces_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnPair(link.Link, true, PerformShowingDifferenceWebResourcesAsync);
        }

        private async Task PerformShowingDifferenceWebResourcesAsync(LinkedEntities<SystemForm> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceFormWebResourcesFormat2, linked.Entity1.ObjectTypeCode, linked.Entity1.Name);

            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null)
            {
                var descriptor1 = GetDescriptor(service1);
                var descriptor2 = GetDescriptor(service2);

                var repository1 = new SystemFormRepository(service1);
                var repository2 = new SystemFormRepository(service2);

                var systemForm1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(SystemForm.Schema.Attributes.formxml));
                var systemForm2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(SystemForm.Schema.Attributes.formxml));

                string formXml1 = systemForm1.FormXml ?? string.Empty;
                string formXml2 = systemForm2.FormXml ?? string.Empty;

                WebResourceRepository webResourceRepository1 = new WebResourceRepository(service1);
                WebResourceRepository webResourceRepository2 = new WebResourceRepository(service2);

                try
                {
                    if (!string.IsNullOrEmpty(formXml1) && !string.IsNullOrEmpty(formXml2))
                    {
                        XElement doc1 = XElement.Parse(formXml1);
                        XElement doc2 = XElement.Parse(formXml2);

                        List<string> webResources1 = new FormDescriptionHandler(descriptor1, new DependencyRepository(service1)).GetFormLibraries(doc1);
                        List<string> webResources2 = new FormDescriptionHandler(descriptor2, new DependencyRepository(service2)).GetFormLibraries(doc2);

                        List<string> list = webResources1.Intersect(webResources2).ToList();

                        foreach (var resName in list)
                        {
                            var webresource1 = await webResourceRepository1.FindByNameAsync(resName, ".js");
                            var webresource2 = await webResourceRepository2.FindByNameAsync(resName, ".js");

                            if (webresource1 != null && webresource2 != null)
                            {
                                this._iWriteToOutput.WriteToOutput(null, "Web-resource founded by name: {0}", resName);

                                var contentWebResource1 = webresource1.Content ?? string.Empty;
                                var contentWebResource2 = webresource2.Content ?? string.Empty;

                                if (contentWebResource1 != contentWebResource2)
                                {
                                    string localFilePath1 = await CreateWebResourceAsync(_commonConfig.FolderForExport, service1.ConnectionData, resName, webresource1);
                                    string localFilePath2 = await CreateWebResourceAsync(_commonConfig.FolderForExport, service2.ConnectionData, resName, webresource2);

                                    if (File.Exists(localFilePath1) && File.Exists(localFilePath2))
                                    {
                                        this._iWriteToOutput.ProcessStartProgramComparerAsync(localFilePath1, localFilePath2, Path.GetFileName(localFilePath1), Path.GetFileName(localFilePath2));
                                    }
                                    else
                                    {
                                        this._iWriteToOutput.PerformAction(service1.ConnectionData, localFilePath1);

                                        this._iWriteToOutput.PerformAction(service2.ConnectionData, localFilePath2);
                                    }
                                }
                            }
                            else
                            {
                                this._iWriteToOutput.WriteToOutput(null, "Web-resource not founded in CRM: {0}", resName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(null, ex);
                }
            }

            ToggleControls(true, Properties.OutputStrings.ShowingDifferenceFormWebResourcesCompletedFormat2, linked.Entity1.ObjectTypeCode, linked.Entity1.Name);
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

        private void ExecuteActionEntityField(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, string extension, Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idSystemForm, getService, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportXmlToFileAsync(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData, idSystemForm, systemForm.ObjectTypeCode, systemForm.Name, fieldTitle, extension, xmlContent);

                if (!File.Exists(filePath))
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, systemForm.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(null);
                }

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
        }

        private void ExecuteActionEntityDescription(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idSystemForm, getService);
        }

        private async Task PerformExportEntityDescriptionToFileAsync(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(systemForm, EntityFileNameFormatter.SystemFormIgnoreFields, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, systemForm.ObjectTypeCode, systemForm.Name, EntityFileNameFormatter.Headers.EntityDescription, description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        private void mIExportSystemForm1EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntityDescription(link.Link.Entity1.Id, GetService1, PerformExportEntityDescriptionToFileAsync);
        }

        private void mIExportSystemForm2EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntityDescription(link.Link.Entity2.Id, GetService2, PerformExportEntityDescriptionToFileAsync);
        }

        private void mIExportSystemForm1FormDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionSystemFormWithFormDescription(link.Link.Entity1.Id, link.Link.Entity1.ObjectTypeCode, link.Link.Entity1.Name, GetService1, PerformExportFormDescriptionToFileAsync);
        }

        private void mIExportSystemForm2FormDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionSystemFormWithFormDescription(link.Link.Entity2.Id, link.Link.Entity2.ObjectTypeCode, link.Link.Entity2.Name, GetService2, PerformExportFormDescriptionToFileAsync);
        }

        private async Task PerformExportFormDescriptionToFileAsync(Guid idSystemForm, string entityName, string name, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingSystemFormDescriptionFormat2, entityName, name);

            var service = await getService();
            var descriptor = GetDescriptor(service);

            if (service != null && descriptor != null)
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

                string formXml = systemForm.FormXml;

                if (!string.IsNullOrEmpty(formXml))
                {
                    try
                    {
                        XElement doc = XElement.Parse(formXml);

                        var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

                        string formDescritpion = await handler.GetFormDescriptionAsync(doc, systemForm.ObjectTypeCode, idSystemForm, systemForm.Name, systemForm.FormattedValues[SystemForm.Schema.Attributes.type]);

                        string filePath = await CreateDescriptionFileAsync(service.ConnectionData, systemForm.ObjectTypeCode, systemForm.Name, "FormDescription", formDescritpion);

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "FormDescription", filePath);

                        this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "FormXml");
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }
            }

            ToggleControls(true, Properties.OutputStrings.CreatingSystemFormDescriptionCompletedFormat2, entityName, name);
        }

        private void mIExportSystemForm1FormXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntityField(link.Link.Entity1.Id, GetService1, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, "xml", PerformExportXmlToFileAsync);
        }

        private void mIExportSystemForm2FormXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntityField(link.Link.Entity2.Id, GetService2, SystemForm.Schema.Attributes.formxml, SystemForm.Schema.Headers.formxml, "xml", PerformExportXmlToFileAsync);
        }

        private void mIExportSystemForm1FormJson_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntityField(link.Link.Entity1.Id, GetService1, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, "json", PerformExportXmlToFileAsync);
        }

        private void mIExportSystemForm2FormJson_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntityField(link.Link.Entity2.Id, GetService2, SystemForm.Schema.Attributes.formjson, SystemForm.Schema.Headers.formjson, "json", PerformExportXmlToFileAsync);
        }

        private void mIExportSystemForm1DownloadWebResouces_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionSystemFormWithFormDescription(link.Link.Entity1.Id, link.Link.Entity1.ObjectTypeCode, link.Link.Entity1.Name, GetService1, PerformDownloadWebResourcesAsync);
        }

        private void mIExportSystemForm2DownloadWebResouces_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionSystemFormWithFormDescription(link.Link.Entity2.Id, link.Link.Entity2.ObjectTypeCode, link.Link.Entity2.Name, GetService2, PerformDownloadWebResourcesAsync);
        }

        private void ExecuteActionSystemFormWithFormDescription(
            Guid systemFormId
            , string entityName
            , string name
            , Func<Task<IOrganizationServiceExtented>> getService
            , Func<Guid, string, string, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(systemFormId, entityName, name, getService);
        }

        private async Task PerformDownloadWebResourcesAsync(Guid systemFormId, string entityName, string name, Func<Task<IOrganizationServiceExtented>> getService)
        {
            ToggleControls(false, Properties.OutputStrings.DownloadingSystemFormWebResourcesFormat2, entityName, name);

            var service = await getService();
            var descriptor = GetDescriptor(service);

            if (service != null && descriptor != null)
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(systemFormId, new ColumnSet(true));

                string formXml = systemForm.FormXml;

                WebResourceRepository webResourceRepository = new WebResourceRepository(service);

                List<string> files = new List<string>();

                if (!string.IsNullOrEmpty(formXml))
                {
                    try
                    {
                        XElement doc = XElement.Parse(formXml);

                        List<string> webResources = new FormDescriptionHandler(descriptor, new DependencyRepository(service)).GetFormLibraries(doc);

                        foreach (var resName in webResources)
                        {
                            var webresource = await webResourceRepository.FindByNameAsync(resName, ".js");

                            var filePath = await CreateWebResourceAsync(_commonConfig.FolderForExport, service.ConnectionData, resName, webresource);

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
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SystemForm.Schema.EntityLogicalName, name, "FormXml");
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                foreach (var filePath in files)
                {
                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
            }

            ToggleControls(true, Properties.OutputStrings.DownloadingSystemFormWebResourcesCompletedFormat2, entityName, name);
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingSystemForms();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            Popup[] _popupArray = new Popup[] { _optionsPopup };

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

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();

                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    this.Resources["ConnectionName1"] = connection1.Name;
                    this.Resources["ConnectionName2"] = connection2.Name;

                    LoadEntityNames(cmBEntityName, connection1, connection2);

                    UpdateButtonsEnable();

                    ShowExistingSystemForms();
                }
            });
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            EntityViewItem linkedEntityMetadata = GetItemFromRoutedDataContext<EntityViewItem>(e);

            var items = contextMenu.Items.OfType<Control>();

            foreach (var menuContextDifference in items.Where(i =>
                string.Equals(i.Uid, "menuContextDifference", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(i.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase)
            ))
            {
                menuContextDifference.IsEnabled = false;
                menuContextDifference.Visibility = Visibility.Collapsed;

                if (linkedEntityMetadata != null
                     && linkedEntityMetadata.Link != null
                     && linkedEntityMetadata.Link.Entity1 != null
                     && linkedEntityMetadata.Link.Entity2 != null
                )
                {
                    menuContextDifference.IsEnabled = true;
                    menuContextDifference.Visibility = Visibility.Visible;
                }
            }

            foreach (var menuContextConnection2 in items.Where(i =>
                string.Equals(i.Uid, "menuContextConnection2", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(i.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase)
            ))
            {
                menuContextConnection2.IsEnabled = false;
                menuContextConnection2.Visibility = Visibility.Collapsed;

                if (linkedEntityMetadata != null
                    && linkedEntityMetadata.Link != null
                    && linkedEntityMetadata.Link.Entity2 != null
                )
                {
                    menuContextConnection2.IsEnabled = true;
                    menuContextConnection2.Visibility = Visibility.Visible;
                }
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

        private async void miConnection1OpenEntityMetadataInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService1();

            service.ConnectionData.OpenEntityMetadataInWeb(entity.EntityName);
        }

        private async void miConnection2OpenEntityMetadataInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService2();

            service.ConnectionData.OpenEntityMetadataInWeb(entity.EntityName);
        }

        private async void miConnection1OpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService1();

            service.ConnectionData.OpenEntityInstanceListInWeb(entity.EntityName);
        }

        private async void miConnection2OpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService2();

            service.ConnectionData.OpenEntityInstanceListInWeb(entity.EntityName);
        }

        private async void mIConnection1OpenSolutionComponentInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService1();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, entity.Link.Entity1.Id);
            }
        }

        private async void mIConnection2OpenSolutionComponentInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService2();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SystemForm, entity.Link.Entity2.Id);
            }
        }
    }
}