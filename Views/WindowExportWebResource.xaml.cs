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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportWebResource : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private bool _controlsEnabled = true;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private int _init = 0;

        public WindowExportWebResource(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection
            )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(_iWriteToOutput, service, true);

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            LoadImages();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (service != null)
            {
                ShowExistingWebResources();
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private Dictionary<int, BitmapImage> _typeImageMapping = null;

        private BitmapImage _folderImage;

        private void LoadImages()
        {
            _typeImageMapping = new Dictionary<int, BitmapImage>()
            {
                { (int)WebResource.Schema.OptionSets.webresourcetype.Webpage_HTML_1, this.Resources["ImageHtml"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.Style_Sheet_CSS_2, this.Resources["ImageCss"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.Script_JScript_3, this.Resources["ImageJS"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.Data_XML_4, this.Resources["ImageXml"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.PNG_format_5, this.Resources["ImageImages"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.JPG_format_6, this.Resources["ImageImages"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.GIF_format_7, this.Resources["ImageImages"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.Silverlight_XAP_8, this.Resources["ImageXml"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.Style_Sheet_XSL_9, this.Resources["ImageXml"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.ICO_format_10, this.Resources["ImageImages"] as BitmapImage }
            };

            this._folderImage = this.Resources["ImageFolder"] as BitmapImage;
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
                    _iWriteToOutput.WriteToOutput("Connection to CRM.");
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

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

            if (!_descriptorCache.ContainsKey(connectionData.ConnectionId))
            {
                var service = await GetService();

                _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(_iWriteToOutput, service, true);
            }

            return _descriptorCache[connectionData.ConnectionId];
        }

        private async void ShowExistingWebResources()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading webresources...");

            this.trVWebResources.ItemsSource = null;

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<WebResource> list = Enumerable.Empty<WebResource>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    WebResourceRepository repository = new WebResourceRepository(service);
                    list = await repository.GetListAllAsync(textName, new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype, WebResource.Schema.Attributes.ismanaged, WebResource.Schema.Attributes.ishidden));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            this._iWriteToOutput.WriteToOutput("Found {0} webresources.", list.Count());

            LoadWebResources(list);

            UpdateStatus(string.Format("{0} webresources loaded.", list.Count()));

            ToggleControls(true);
        }

        private void LoadWebResources(IEnumerable<WebResource> results)
        {
            ObservableCollection<EntityTreeViewItem> list = new ObservableCollection<EntityTreeViewItem>();

            var groupList = results
                    .GroupBy(a => a.WebResourceType.Value)
                    .OrderBy(a => a.Key);

            foreach (var group in groupList)
            {
                var groupName = group.First().FormattedValues[WebResource.Schema.Attributes.webresourcetype];

                BitmapImage image = null;

                if (_typeImageMapping.ContainsKey(group.Key))
                {
                    image = _typeImageMapping[group.Key];
                }

                var node = new EntityTreeViewItem(groupName, null, image);

                var nodeEntity = TreeNodeEntity.Convert(group);

                FullfillTreeNode(node, nodeEntity, image);

                list.Add(node);
            }

            ExpandNode(list);

            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                this.trVWebResources.BeginInit();

                this.trVWebResources.ItemsSource = list;

                this.trVWebResources.EndInit();
            });
        }

        private void ExpandNode(ObservableCollection<EntityTreeViewItem> list)
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
                    ExpandNode(list[0].Items);
                }
            }
            else if (list.Count > 0)
            {
                list[0].IsSelected = true;
            }
        }

        private void FullfillTreeNode(EntityTreeViewItem node, TreeNodeEntity nodeEntity, BitmapImage image)
        {
            foreach (var item in nodeEntity.SubNodes.OrderBy(s => s.Name))
            {
                EntityTreeViewItem directoryNode = new EntityTreeViewItem(item.Name, null, _folderImage);

                node.Items.Add(directoryNode);

                FullfillTreeNode(directoryNode, item, image);
            }

            foreach (var item in nodeEntity.Entities.OrderBy(s => s.Item1))
            {
                string name = item.Item1;

                EntityTreeViewItem tn = new EntityTreeViewItem(name, item.Item2.Id, image);

                if (item != null)
                {
                    StringBuilder result = new StringBuilder();

                    if (item.Item2.IsManaged.GetValueOrDefault())
                    {
                        if (result.Length > 0) { result.Append(" "); }

                        result.Append("Managed");
                    }

                    if (item.Item2.IsHidden.Value)
                    {
                        if (result.Length > 0) { result.Append("    "); }

                        result.Append("Hidden");
                    }

                    tn.Description = result.ToString();
                }

                node.Items.Add(tn);
            }

            if (node.Items.Count > 0)
            {
                node.Description = node.Items.Count.ToString();
            }
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

            ToggleControl(cmBCurrentConnection, enabled);

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
            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.trVWebResources.SelectedItem != null
                                        && this.trVWebResources.SelectedItem is EntityTreeViewItem
                                        && (this.trVWebResources.SelectedItem as EntityTreeViewItem).WebResourceId != null;

                    UIElement[] list = { tSDDBExportWebResource, btnExportAll };

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
                ShowExistingWebResources();
            }
        }

        private EntityTreeViewItem GetSelectedEntity()
        {
            EntityTreeViewItem result = null;

            if (this.trVWebResources.SelectedItem != null
                && this.trVWebResources.SelectedItem is EntityTreeViewItem
                )
            {
                result = (this.trVWebResources.SelectedItem as EntityTreeViewItem);
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void ExecuteAction(Guid idWebResource, string name, Func<string, Guid, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!_controlsEnabled)
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

            await action(folder, idWebResource, name);
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idWebResource, string name)
        {
            await PerformExportEntityDescription(folder, idWebResource, name);

            await PerformExportWebResourceContent(folder, idWebResource, name);

            await PerformExportXmlToFile(folder, idWebResource, name, WebResource.Schema.Attributes.dependencyxml, "DependencyXml");
        }

        private async void ExecuteActionEntity(Guid idWebResource, string name, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!_controlsEnabled)
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

            await action(folder, idWebResource, name, fieldName, fieldTitle);
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformExportEntityDescription);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idWebResource, string name)
        {
            ToggleControls(false);

            UpdateStatus("Exporting WebResource Entity Description...");

            var service = await GetService();

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            var webresource = await webResourceRepository.FindByIdAsync(idWebResource, new ColumnSet(true));

            string fileName = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, name, "EntityDescription", "txt");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, webresource, EntityFileNameFormatter.WebResourceIgnoreFields);

            this._iWriteToOutput.WriteToOutput("WebResource Entity Description exported to {0}", filePath);

            this._iWriteToOutput.PerformAction(filePath, _commonConfig);

            this._iWriteToOutput.WriteToOutput("End creating file at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            UpdateStatus("Operation is completed.");

            ToggleControls(true);
        }

        private void trVWebResources_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateButtonsEnable();
        }

        private void trVWebResources_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityTreeViewItem;

                if (item != null && item.WebResourceId.HasValue)
                {
                    ExecuteAction(item.WebResourceId.Value, item.Name, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idWebResource, string name)
        {
            //await PerformExportEntityDescription(folder, idWebResource, name);

            await PerformExportWebResourceContent(folder, idWebResource, name);

            //await PerformExportXmlToFile(folder, idWebResource, name, WebResource.Schema.Attributes.dependencyxml, "DependencyXml);
        }

        private async Task PerformExportWebResourceContent(string folder, Guid idWebResource, string name)
        {
            ToggleControls(false);

            UpdateStatus("Exporting WebResource Content...");

            var service = await GetService();

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            var webresource = await webResourceRepository.FindByIdAsync(idWebResource, new ColumnSet(WebResource.Schema.Attributes.content, WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype));

            if (webresource != null && !string.IsNullOrEmpty(webresource.Content))
            {
                this._iWriteToOutput.WriteToOutput("Starting downloading {0}", name);

                string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

                var contentWebResource = webresource.Content ?? string.Empty;

                var array = Convert.FromBase64String(contentWebResource);

                string fileName = string.Format("{0}.{1}", service.ConnectionData.Name, webResourceFileName);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllBytes(filePath, array);

                this._iWriteToOutput.WriteToOutput("Web-resource '{0}' has downloaded to {1}.", name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("Web-resource not founded in CRM: {0}", name);
            }

            this._iWriteToOutput.WriteToOutput("End creating file at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            UpdateStatus("Operation is completed.");

            ToggleControls(true);
        }

        private void mIExportWebResourceDependencyXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.dependencyxml, "DependencyXml", PerformExportXmlToFile);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idWebResource, string name, string fieldName, string fieldTitle)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus(string.Format("Exporting WebResource {0}...", fieldTitle));

            var service = await GetService();

            WebResourceRepository webResourceRepository = new WebResourceRepository(service);

            var webresource = await webResourceRepository.FindByIdAsync(idWebResource, new ColumnSet(fieldName));

            string xmlContent = webresource.GetAttributeValue<string>(fieldName);

            string filePath = await CreateFileAsync(folder, name, fieldTitle, xmlContent);

            this._iWriteToOutput.PerformAction(filePath, _commonConfig);

            UpdateStatus("Operation is completed.");

            ToggleControls(true);
        }

        private Task<string> CreateFileAsync(string folder, string name, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, name, fieldTitle, xmlContent));
        }

        private string CreateFile(string folder, string name, string fieldTitle, string xmlContent)
        {
            name = Path.GetFileName(name);

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            string fileName = EntityFileNameFormatter.GetWebResourceFileName(connectionData.Name, name, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, Encoding.UTF8);

                    this._iWriteToOutput.WriteToOutput("{0} WebResource {1} {2} exported to {3}", connectionData.Name, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("WebResource {0} {1} is empty.", name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            return filePath;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingWebResources();
            }

            base.OnKeyDown(e);
        }

        private void mIExportWebResourceDependentComponents_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformCreatingFileWithDependentComponents);
        }

        private async Task PerformCreatingFileWithDependentComponents(string folder, Guid idWebResource, string name)
        {
            this._iWriteToOutput.WriteToOutput("Starting downloading {0}", name);

            var removeWrongFromName = FileOperations.RemoveWrongSymbols(name);

            var service = await GetService();
            var descriptor = await GetDescriptor();

            var dependencyRepository = new DependencyRepository(service);

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var coll = await dependencyRepository.GetDependentComponentsAsync((int)ComponentType.WebResource, idWebResource);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                string fileName = EntityFileNameFormatter.GetWebResourceFileName(
                    service.ConnectionData.Name
                    , removeWrongFromName
                    , "Dependent Components"
                    , "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("Web-resource {0} Dependent Components exported to {1}", name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("Web-resource {0} has no Dependent Components.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        private void mIExportWebResourceRequiredComponents_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformCreatingFileWithRequiredComponents);
        }

        private async Task PerformCreatingFileWithRequiredComponents(string folder, Guid idWebResource, string name)
        {
            this._iWriteToOutput.WriteToOutput("Starting downloading {0}", name);

            var removeWrongFromName = FileOperations.RemoveWrongSymbols(name);

            var service = await GetService();
            var descriptor = await GetDescriptor();

            var dependencyRepository = new DependencyRepository(service);

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var coll = await dependencyRepository.GetRequiredComponentsAsync((int)ComponentType.WebResource, idWebResource);

            string description = await descriptorHandler.GetDescriptionRequiredAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                string fileName = EntityFileNameFormatter.GetWebResourceFileName(
                    service.ConnectionData.Name
                    , removeWrongFromName
                    , "Required Components"
                    , "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("Web-resource {0} Required Components exported to {1}", name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("Web-resource {0} has no Required Components.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        private void mIExportWebResourceDependenciesForDelete_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformCreatingFileWithDependenciesForDelete);
        }

        private async Task PerformCreatingFileWithDependenciesForDelete(string folder, Guid idWebResource, string name)
        {
            this._iWriteToOutput.WriteToOutput("Starting downloading {0}", name);

            var removeWrongFromName = FileOperations.RemoveWrongSymbols(name);

            var service = await GetService();
            var descriptor = await GetDescriptor();

            var dependencyRepository = new DependencyRepository(service);

            var descriptorHandler = new DependencyDescriptionHandler(descriptor);

            var coll = await dependencyRepository.GetDependenciesForDeleteAsync((int)ComponentType.WebResource, idWebResource);

            string description = await descriptorHandler.GetDescriptionDependentAsync(coll);

            if (!string.IsNullOrEmpty(description))
            {
                string fileName = EntityFileNameFormatter.GetWebResourceFileName(
                    service.ConnectionData.Name
                    , removeWrongFromName
                    , "Dependencies For Delete"
                    , "txt");

                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                File.WriteAllText(filePath, description, Encoding.UTF8);

                this._iWriteToOutput.WriteToOutput("Web-resource {0} Dependencies For Delete exported to {1}", name, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            else
            {
                this._iWriteToOutput.WriteToOutput("Web-resource {0} has no Dependencies For Delete.", name);
                this._iWriteToOutput.ActivateOutputWindow();
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                if (contextMenu.PlacementTarget is TreeViewItem node)
                {
                    node.IsSelected = true;
                }

                var items = contextMenu.Items.OfType<MenuItem>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                var lastSoluiton = items.FirstOrDefault(i => string.Equals(i.Uid, "contMnAddIntoSolutionLast", StringComparison.InvariantCultureIgnoreCase));

                if (lastSoluiton != null)
                {
                    lastSoluiton.Items.Clear();

                    lastSoluiton.IsEnabled = false;
                    lastSoluiton.Visibility = Visibility.Collapsed;

                    if (connectionData != null)
                    {
                        bool addIntoSolutionLast = connectionData.LastSelectedSolutionsUniqueName.Any();

                        lastSoluiton.IsEnabled = addIntoSolutionLast;
                        lastSoluiton.Visibility = addIntoSolutionLast ? Visibility.Visible : Visibility.Collapsed;

                        foreach (var uniqueName in connectionData.LastSelectedSolutionsUniqueName)
                        {
                            var menuItem = new MenuItem()
                            {
                                Header = uniqueName.Replace("_", "__"),
                                Tag = uniqueName,
                            };

                            menuItem.Click += AddIntoCrmSolutionLast_Click;

                            lastSoluiton.Items.Add(menuItem);
                        }
                    }
                }
            }
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            if (!entity.WebResourceId.HasValue)
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.WebResource, entity.WebResourceId.Value);
            }
        }

        private void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            if (!entity.WebResourceId.HasValue)
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
                connectionData.OpenSolutionComponentInWeb(ComponentType.WebResource, entity.WebResourceId.Value, null, null);
            }
        }

        private void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            AddIntoSolution(true, null);
        }

        private void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is string solutionUniqueName
                )
            {
                AddIntoSolution(false, solutionUniqueName);
            }
        }

        private void AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
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
                _commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        var contr = new SolutionController(this._iWriteToOutput);

                        contr.ExecuteAddingComponentesIntoSolution(connectionData, _commonConfig, solutionUniqueName, ComponentType.WebResource, new[] { entity.WebResourceId.Value }, null, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                });

                backWorker.Start();
            }
        }

        private async void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || !entity.WebResourceId.HasValue)
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
                , (int)ComponentType.WebResource
                , entity.WebResourceId.Value
                , null
                );
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareWebResources_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWebResourcesWindow(
                _iWriteToOutput
                , _commonConfig
                , service.ConnectionData
                , service.ConnectionData
            );
        }

        private async void mIOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || !entity.WebResourceId.HasValue)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.WebResource
                , entity.WebResourceId.Value
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this.trVWebResources.ItemsSource = null;
            });

            if (!_controlsEnabled)
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
                ShowExistingWebResources();
            }
        }
    }
}