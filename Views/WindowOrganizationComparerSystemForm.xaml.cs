using Microsoft.Xrm.Sdk.Query;
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
    public partial class WindowOrganizationComparerSystemForm : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private string _filterEntity;

        private Dictionary<Guid, IOrganizationServiceExtented> _cacheService = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _сacheDescription = new Dictionary<Guid, SolutionComponentDescriptor>();

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private bool _controlsEnabled = true;

        private int _init = 0;

        public WindowOrganizationComparerSystemForm(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = connection1.ConnectionConfiguration;
            this._filterEntity = filterEntity;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            tSDDBConnection1.Header = string.Format("Export from {0}", connection1.Name);
            tSDDBConnection2.Header = string.Format("Export from {0}", connection2.Name);

            this.Resources["ConnectionName1"] = string.Format("Create from {0}", connection1.Name);
            this.Resources["ConnectionName2"] = string.Format("Create from {0}", connection2.Name);

            LoadFromConfig();

            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            if (string.IsNullOrEmpty(_filterEntity))
            {
                btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
                btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;
            }

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSystemForms.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = _connectionConfig.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = _connectionConfig.Connections;
            cmBConnection2.SelectedItem = connection2;

            _init--;

            ShowExistingSystemForms();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

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

        private async Task<IOrganizationServiceExtented> GetService1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_cacheService.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput("Connection to CRM.");
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

                    _cacheService[connectionData.ConnectionId] = service;
                }

                return _cacheService[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task<IOrganizationServiceExtented> GetService2()
        {
            ConnectionData connectionData = null;

            cmBConnection2.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_cacheService.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput("Connection to CRM.");
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

                    _cacheService[connectionData.ConnectionId] = service;
                }

                return _cacheService[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task<SolutionComponentDescriptor> GetDescriptor1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            if (!_сacheDescription.ContainsKey(connectionData.ConnectionId))
            {
                var service = await GetService1();

                if (service != null)
                {
                    _сacheDescription[connectionData.ConnectionId] = new SolutionComponentDescriptor(_iWriteToOutput, service, true);
                }
            }

            return _сacheDescription[connectionData.ConnectionId];
        }

        private async Task<SolutionComponentDescriptor> GetDescriptor2()
        {
            ConnectionData connectionData = null;

            cmBConnection2.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            if (!_сacheDescription.ContainsKey(connectionData.ConnectionId))
            {
                var service = await GetService2();

                if (service != null)
                {
                    _сacheDescription[connectionData.ConnectionId] = new SolutionComponentDescriptor(_iWriteToOutput, service, true);
                }
            }

            return _сacheDescription[connectionData.ConnectionId];
        }

        private async void ShowExistingSystemForms()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading forms...");

            this._itemsSource.Clear();

            IEnumerable<LinkedEntities<SystemForm>> list = Enumerable.Empty<LinkedEntities<SystemForm>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var columnSet = new ColumnSet(SystemForm.Schema.Attributes.name, SystemForm.Schema.Attributes.objecttypecode, SystemForm.Schema.Attributes.type);

                    var temp = new List<LinkedEntities<SystemForm>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new SystemFormRepository(service1);
                        var repository2 = new SystemFormRepository(service2);

                        var task1 = repository1.GetListAsync(this._filterEntity, columnSet);
                        var task2 = repository2.GetListAsync(this._filterEntity, columnSet);

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

                        var list1 = await repository1.GetListAsync(this._filterEntity, columnSet);

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
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            var textName = string.Empty;

            txtBFilterEnitity.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            this._iWriteToOutput.WriteToOutput("Found {0} forms", list.Count());

            LoadEntities(list);

            UpdateStatus(string.Format("{0} forms loaded.", list.Count()));

            ToggleControls(true);
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
                        var type1 = ent.Entity1?.ObjectTypeCode.ToLower() ?? string.Empty;
                        var name1 = ent.Entity1?.Name.ToLower() ?? string.Empty;

                        var type2 = ent.Entity2?.ObjectTypeCode.ToLower() ?? string.Empty;
                        var name2 = ent.Entity2?.Name.ToLower() ?? string.Empty;

                        return type1.Contains(textName)
                            || name1.Contains(textName)
                            || type2.Contains(textName)
                            || name2.Contains(textName)
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

            public LinkedEntities<SystemForm> Link { get; private set; }

            public EntityViewItem(string entityName, string formType, LinkedEntities<SystemForm> link, string formName1, string formName2)
            {
                this.EntityName = entityName;
                this.FormName1 = formName1;
                this.FormName2 = formName2;
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
                    var item = new EntityViewItem(link.Entity1.ObjectTypeCode, link.Entity1.FormattedValues[SystemForm.Schema.Attributes.type], link, link.Entity1.Name, link.Entity2?.Name);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSystemForms.Items.Count == 1)
                {
                    this.lstVwSystemForms.SelectedItem = this.lstVwSystemForms.Items[0];
                }
            });
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

            ToggleControl(this.toolStrip, enabled);

            ToggleProgressBar(enabled);

            if (enabled)
            {
                UpdateButtonsEnable();
            }
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
            this.lstVwSystemForms.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwSystemForms.SelectedItems.Count > 0;

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

        private EntityViewItem GetSelectedEntity()
        {
            EntityViewItem result = null;

            if (this.lstVwSystemForms.SelectedItems.Count == 1
                && this.lstVwSystemForms.SelectedItems[0] != null
                && this.lstVwSystemForms.SelectedItems[0] is EntityViewItem
                )
            {
                result = (this.lstVwSystemForms.SelectedItems[0] as EntityViewItem);
            }

            return result;
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
            if (!_controlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
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

            action(linked, showAllways);
        }

        private Task<string> CreateFileAsync(string connectionName, string entityName, string name, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(connectionName, entityName, name, fieldTitle, xmlContent));
        }

        private string CreateFile(string connectionName, string entityName, string name, string fieldTitle, string xmlContent)
        {
            string fileName = EntityFileNameFormatter.GetSystemFormFileName(connectionName, entityName, name, fieldTitle, "xml");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, Encoding.UTF8);

                    this._iWriteToOutput.WriteToOutput("{0} System Form {1} {2} exported to {3}", connectionName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("{0} System Form {1} {2} is empty.", connectionName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            return filePath;
        }

        private Task<string> CreateDescriptionFileAsync(string connectionName, string entityName, string name, string fieldName, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionName, entityName, name, fieldName, description));
        }

        private string CreateDescriptionFile(string connectionName, string entityName, string name, string fieldName, string description)
        {
            string fileName = EntityFileNameFormatter.GetSystemFormFileName(connectionName, entityName, name, fieldName, "txt");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, Encoding.UTF8);

                    this._iWriteToOutput.WriteToOutput("{0} System Form Entity Description {1} {2} exported to {3}", connectionName, name, fieldName, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput("{0} System Form Entity Description {1} {2} is empty.", connectionName, name, fieldName);
                this._iWriteToOutput.ActivateOutputWindow();
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

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SystemForm.Schema.Attributes.formxml, "FormXml");
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
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

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
                    string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData.Name, systemForm1.ObjectTypeCode, systemForm1.Name, "EntityDescription", desc1);
                    string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData.Name, systemForm2.ObjectTypeCode, systemForm2.Name, "EntityDescription", desc2);

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        this._iWriteToOutput.ProcessStartProgramComparer(this._commonConfig, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(filePath1, _commonConfig);

                        this._iWriteToOutput.PerformAction(filePath2, _commonConfig);
                    }
                }
            }

            ToggleControls(true);
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
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null)
            {
                var descriptor1 = await GetDescriptor1();
                var descriptor2 = await GetDescriptor2();

                var handler1 = new FormDescriptionHandler(descriptor1, new DependencyRepository(service1));
                var handler2 = new FormDescriptionHandler(descriptor2, new DependencyRepository(service2));

                var repository1 = new SystemFormRepository(service1);
                var repository2 = new SystemFormRepository(service2);

                var systemForm1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                var systemForm2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                string formDescritpion1 = await handler1.GetFormDescriptionAsync(XElement.Parse(systemForm1.FormXml), systemForm1.ObjectTypeCode, systemForm1.Id, systemForm1.Name, systemForm1.FormattedValues[SystemForm.Schema.Attributes.type]);
                string formDescritpion2 = await handler2.GetFormDescriptionAsync(XElement.Parse(systemForm2.FormXml), systemForm2.ObjectTypeCode, systemForm2.Id, systemForm2.Name, systemForm2.FormattedValues[SystemForm.Schema.Attributes.type]);

                if (showAllways || formDescritpion1 != formDescritpion2)
                {
                    string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData.Name, linked.Entity1.ObjectTypeCode, linked.Entity1.Name, "FormDescription", formDescritpion1);
                    string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData.Name, linked.Entity2.ObjectTypeCode, linked.Entity2.Name, "FormDescription", formDescritpion2);

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        this._iWriteToOutput.ProcessStartProgramComparer(this._commonConfig, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(filePath1, _commonConfig);

                        this._iWriteToOutput.PerformAction(filePath2, _commonConfig);
                    }
                }
            }

            ToggleControls(true);
        }

        private void mIShowDifferenceFormXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SystemForm.Schema.Attributes.formxml, "FormXml", PerformShowingDifferenceSingleXmlAsync);
        }

        private void ExecuteActionLinked(LinkedEntities<SystemForm> linked, bool showAllways, string fieldName, string fieldTitle, Func<LinkedEntities<SystemForm>, bool, string, string, Action<XElement>, Task> action, Action<XElement> actionXml = null)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
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

            action(linked, showAllways, fieldName, fieldTitle, actionXml);
        }

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<SystemForm> linked, bool showAllways, string fieldName, string fieldTitle, Action<XElement> action = null)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

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

                if (showAllways || !ContentCoparerHelper.CompareXML(xml1, xml2, false, action).IsEqual)
                {
                    string filePath1 = await CreateFileAsync(service1.ConnectionData.Name, linked.Entity1.ObjectTypeCode, linked.Entity1.Name, fieldTitle, xml1);
                    string filePath2 = await CreateFileAsync(service2.ConnectionData.Name, linked.Entity2.ObjectTypeCode, linked.Entity2.Name, fieldTitle, xml2);

                    if (File.Exists(filePath1) && File.Exists(filePath2))
                    {
                        this._iWriteToOutput.ProcessStartProgramComparer(this._commonConfig, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                    }
                    else
                    {
                        this._iWriteToOutput.PerformAction(filePath1, _commonConfig);

                        this._iWriteToOutput.PerformAction(filePath2, _commonConfig);
                    }
                }
            }

            ToggleControls(true);
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
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null)
            {
                var descriptor1 = await GetDescriptor1();
                var descriptor2 = await GetDescriptor2();

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

                        if (!Directory.Exists(_commonConfig.FolderForExport))
                        {
                            Directory.CreateDirectory(_commonConfig.FolderForExport);
                        }

                        foreach (var resName in list)
                        {
                            var webresource1 = await webResourceRepository1.FindByNameAsync(resName, ".js");
                            var webresource2 = await webResourceRepository2.FindByNameAsync(resName, ".js");

                            if (webresource1 != null && webresource2 != null)
                            {
                                this._iWriteToOutput.WriteToOutput("Web-resource founded by name: {0}", resName);

                                var contentWebResource1 = webresource1.Content ?? string.Empty;
                                var contentWebResource2 = webresource2.Content ?? string.Empty;

                                if (contentWebResource1 != contentWebResource2)
                                {
                                    string localFilePath1 = await CreateWebResourceAsync(_commonConfig.FolderForExport, service1.ConnectionData.Name, resName, webresource1);
                                    string localFilePath2 = await CreateWebResourceAsync(_commonConfig.FolderForExport, service2.ConnectionData.Name, resName, webresource2);

                                    if (File.Exists(localFilePath1) && File.Exists(localFilePath2))
                                    {
                                        this._iWriteToOutput.ProcessStartProgramComparer(this._commonConfig, localFilePath1, localFilePath2, Path.GetFileName(localFilePath1), Path.GetFileName(localFilePath2));
                                    }
                                    else
                                    {
                                        this._iWriteToOutput.PerformAction(localFilePath1, _commonConfig);

                                        this._iWriteToOutput.PerformAction(localFilePath2, _commonConfig);
                                    }
                                }
                            }
                            else
                            {
                                this._iWriteToOutput.WriteToOutput("Web-resource not founded in CRM: {0}", resName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }

            ToggleControls(true);
        }

        private Task<string> CreateWebResourceAsync(string folder, string connectionName, string resName, WebResource webresource)
        {
            return Task.Run(() => CreateWebResource(folder, connectionName, resName, webresource));
        }

        private string CreateWebResource(string folder, string connectionName, string resName, WebResource webresource)
        {
            if (webresource == null)
            {
                return string.Empty;
            }

            this._iWriteToOutput.WriteToOutput("Web-resource founded by name: {0}", resName);

            this._iWriteToOutput.WriteToOutput("Starting downloading {0}", webresource.Name);

            string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

            var contentWebResource = webresource.Content ?? string.Empty;

            var array = Convert.FromBase64String(contentWebResource);

            string localFileName = string.Format("{0}.{1}", connectionName, webResourceFileName);
            string localFilePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(localFileName));

            File.WriteAllBytes(localFilePath, array);

            this._iWriteToOutput.WriteToOutput("Web-resource '{0}' has downloaded to {1}.", webresource.Name, localFilePath);

            return localFilePath;
        }

        private void ExecuteActionEntityField(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, Task> action)
        {
            if (!_controlsEnabled)
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

            action(idSystemForm, getService, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFileAsync(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service = await getService();

            if (service != null)
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

                string xmlContent = systemForm.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }

            ToggleControls(true);
        }

        private void ExecuteActionEntityDescription(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!_controlsEnabled)
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

            action(idSystemForm, getService);
        }

        private async Task PerformExportEntityDescriptionToFileAsync(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service = await getService();

            if (service != null)
            {
                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(systemForm, EntityFileNameFormatter.SystemFormIgnoreFields, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, "EntityDescription", description);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }

            ToggleControls(true);
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

        private void chBAllEntities_Checked(object sender, RoutedEventArgs e)
        {
            ShowExistingSystemForms();
        }

        private void mIExportSystemForm1FormDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionSystemFormWithFormDescription(link.Link.Entity1.Id, GetService1, GetDescriptor1, PerformExportFormDescriptionToFileAsync);
        }

        private void mIExportSystemForm2FormDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionSystemFormWithFormDescription(link.Link.Entity2.Id, GetService2, GetDescriptor2, PerformExportFormDescriptionToFileAsync);
        }

        private async Task PerformExportFormDescriptionToFileAsync(Guid idSystemForm, Func<Task<IOrganizationServiceExtented>> getService, Func<Task<SolutionComponentDescriptor>> getDescription)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service = await getService();
            var descriptor = await getDescription();

            if (service != null && descriptor != null)
            {
                var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(idSystemForm, new ColumnSet(true));

                string formXml = systemForm.FormXml;

                if (!string.IsNullOrEmpty(formXml))
                {
                    try
                    {
                        XElement doc = XElement.Parse(formXml);

                        string formDescritpion = await handler.GetFormDescriptionAsync(doc, systemForm.ObjectTypeCode, idSystemForm, systemForm.Name, systemForm.FormattedValues[SystemForm.Schema.Attributes.type]);

                        string filePath = await CreateDescriptionFileAsync(service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name, "FormDescription", formDescritpion);

                        this._iWriteToOutput.WriteToOutput("SystemForm {0} Form Description exported to {0}", systemForm.Name, filePath);

                        this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput("SystemForm {0} FormXml is empty.", systemForm.Name);
                    this._iWriteToOutput.ActivateOutputWindow();
                }
            }

            ToggleControls(true);
        }

        private void mIExportSystemForm1FormXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntityField(link.Link.Entity1.Id, GetService1, SystemForm.Schema.Attributes.formxml, "FormXml", PerformExportXmlToFileAsync);
        }

        private void mIExportSystemForm2FormXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntityField(link.Link.Entity2.Id, GetService2, SystemForm.Schema.Attributes.formxml, "FormXml", PerformExportXmlToFileAsync);
        }

        private void mIExportSystemForm1DownloadWebResouces_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionSystemFormWithFormDescription(link.Link.Entity1.Id, GetService1, GetDescriptor1, PerformDownloadWebResourcesAsync);
        }

        private void mIExportSystemForm2DownloadWebResouces_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionSystemFormWithFormDescription(link.Link.Entity2.Id, GetService2, GetDescriptor2, PerformDownloadWebResourcesAsync);
        }

        private void ExecuteActionSystemFormWithFormDescription(Guid systemFormId, Func<Task<IOrganizationServiceExtented>> getService, Func<Task<SolutionComponentDescriptor>> getDescriptor, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Func<Task<SolutionComponentDescriptor>>, Task> action)
        {
            if (!_controlsEnabled)
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

            action(systemFormId, getService, getDescriptor);
        }

        private async Task PerformDownloadWebResourcesAsync(Guid systemFormId, Func<Task<IOrganizationServiceExtented>> getService, Func<Task<SolutionComponentDescriptor>> getDescriptor)
        {
            ToggleControls(false);

            UpdateStatus("Start download form's web resources.");

            var service = await getService();
            var descriptor = await getDescriptor();

            if (service != null && descriptor != null)
            {
                var handler = new FormDescriptionHandler(descriptor, new DependencyRepository(service));

                var repository = new SystemFormRepository(service);

                var systemForm = await repository.GetByIdAsync(systemFormId, new ColumnSet(true));

                string formXml = systemForm.FormXml;

                WebResourceRepository webResourceRepository = new WebResourceRepository(service);

                List<string> files = new List<string>();

                if (!Directory.Exists(_commonConfig.FolderForExport))
                {
                    Directory.CreateDirectory(_commonConfig.FolderForExport);
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

                            var filePath = await CreateWebResourceAsync(_commonConfig.FolderForExport, service.ConnectionData.Name, resName, webresource);

                            if (!string.IsNullOrEmpty(filePath))
                            {
                                files.Add(filePath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput("System Form {0} FormXml is empty.", systemForm.Name);
                    this._iWriteToOutput.ActivateOutputWindow();
                }

                foreach (var filePath in files)
                {
                    this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                }
            }

            UpdateStatus("Operation is completed.");

            ToggleControls(true);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSystemForms();
            }

            base.OnKeyDown(e);
        }

        private void btnClearEntityFilter_Click(object sender, RoutedEventArgs e)
        {
            this._filterEntity = null;

            btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
            btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;

            ShowExistingSystemForms();
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();

                if (!_controlsEnabled)
                {
                    return;
                }

                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    tSDDBConnection1.Header = string.Format("Export from {0}", connection1.Name);
                    tSDDBConnection2.Header = string.Format("Export from {0}", connection2.Name);

                    this.Resources["ConnectionName1"] = string.Format("Create from {0}", connection1.Name);
                    this.Resources["ConnectionName2"] = string.Format("Create from {0}", connection2.Name);

                    ShowExistingSystemForms();
                }
            });
        }
        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
        }

        private async void btnCompareRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerRibbonWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity.EntityName);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity.EntityName);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity.EntityName);
        }

        private async void btnCreateMetadataFile1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, null, null);
        }

        private async void btnExportRibbon1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRibbonWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, null);
        }

        private async void btnGlobalOptionSets1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            var service = await GetService1();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , null
                , string.Empty
                , string.Empty
                );
        }

        private async void btnSystemForms1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnSavedQuery1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnSavedChart1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnWorkflows1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnAttributesDependentComponent1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenAttributesDependentComponentWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, null);
        }

        private async void btnPluginTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnMessageTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnMessageRequestTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnCreateMetadataFile2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, null, null);
        }

        private async void btnExportRibbon2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRibbonWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, null);
        }

        private async void btnGlobalOptionSets2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            var service = await GetService2();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , null
                , string.Empty
                , string.Empty
                );
        }

        private async void btnSystemForms2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnSavedQuery2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnSavedChart2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnWorkflows2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnAttributesDependentComponent2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenAttributesDependentComponentWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, null);
        }

        private async void btnPluginTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnMessageTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnMessageRequestTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }
    }
}