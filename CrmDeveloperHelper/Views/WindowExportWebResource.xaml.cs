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
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportWebResource : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        public WindowExportWebResource(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection
            )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

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

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

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

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
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
                , { (int)WebResource.Schema.OptionSets.webresourcetype.PNG_format_5, this.Resources["ImagePng"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.JPG_format_6, this.Resources["ImageImages"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.GIF_format_7, this.Resources["ImageGif"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.Silverlight_XAP_8, this.Resources["ImageXml"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.Style_Sheet_XSL_9, this.Resources["ImageXml"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.ICO_format_10, this.Resources["ImageImages"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.Vector_format_SVG_11, this.Resources["ImageSvg"] as BitmapImage }
                , { (int)WebResource.Schema.OptionSets.webresourcetype.String_RESX_12, this.Resources["ImageResx"] as BitmapImage }
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

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service);
                }

                _descriptorCache[connectionData.ConnectionId].SetSettings(_commonConfig);

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingWebResources()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingWebResources);

            this.trVWebResources.ItemsSource = null;

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<WebResource> list = Enumerable.Empty<WebResource>();

            try
            {
                if (service != null)
                {
                    WebResourceRepository repository = new WebResourceRepository(service);
                    list = await repository.GetListAllAsync(textName, new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype, WebResource.Schema.Attributes.ismanaged, WebResource.Schema.Attributes.ishidden, WebResource.Schema.Attributes.iscustomizable));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadWebResources(list);

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingWebResourcesCompletedFormat1, list.Count());
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

                EntityTreeViewItem tn = new EntityTreeViewItem(name, item.Item2, image);

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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                                        && this.trVWebResources.SelectedItem != null
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
            if (this.trVWebResources.SelectedItem != null
                && this.trVWebResources.SelectedItem is EntityTreeViewItem result
                )
            {
                return result;
            }

            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task ExecuteAction(Guid idWebResource, string name, Func<string, Guid, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
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

            await PerformExportXmlToFile(folder, idWebResource, name, WebResource.Schema.Attributes.dependencyxml, WebResource.Schema.Headers.dependencyxml, "xml");

            await PerformExportXmlToFile(folder, idWebResource, name, WebResource.Schema.Attributes.contentjson, WebResource.Schema.Headers.contentjson, "json");
        }

        private async Task ExecuteActionEntity(Guid idWebResource, string name, string fieldName, string fieldTitle, string extension, Func<string, Guid, string, string, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(folder))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, folder);
                folder = FileOperations.GetDefaultFolderForExportFilePath();
            }

            await action(folder, idWebResource, name, fieldName, fieldTitle, extension);
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

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformEntityEditor);
        }

        private void mIDeleteWebResource_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformDeleteWebResource);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idWebResource, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(service);

                var webresource = await webResourceRepository.FindByIdAsync(idWebResource, new ColumnSet(true));

                string fileName = EntityFileNameFormatter.GetWebResourceFileName(service.ConnectionData.Name, name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, webresource, EntityFileNameFormatter.WebResourceIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData
                    , Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , webresource.LogicalName
                    , filePath
                );

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private async Task PerformEntityEditor(string folder, Guid idWebResource, string name)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, WebResource.EntityLogicalName, idWebResource);
        }

        private async Task PerformDeleteWebResource(string folder, Guid idWebResource, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, WebResource.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.DeletingEntitiesFormat2, service.ConnectionData.Name, WebResource.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, WebResource.EntityLogicalName, idWebResource);

                    await service.DeleteAsync(WebResource.EntityLogicalName, idWebResource);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.DeletingEntitiesCompletedFormat2, service.ConnectionData.Name, WebResource.EntityLogicalName);

                ShowExistingWebResources();
            }
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
            await PerformExportWebResourceContent(folder, idWebResource, name);

            //await PerformExportEntityDescription(folder, idWebResource, name);

            //await PerformExportXmlToFile(folder, idWebResource, name, WebResource.Schema.Attributes.dependencyxml, "DependencyXml);
        }

        private async Task PerformExportWebResourceContent(string folder, Guid idWebResource, string name)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingWebResourceContentFormat1, name);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(service);

                var webresource = await webResourceRepository.FindByIdAsync(idWebResource, new ColumnSet(WebResource.Schema.Attributes.content, WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype));

                if (webresource != null && !string.IsNullOrEmpty(webresource.Content))
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Starting downloading {0}", name);

                    string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

                    var contentWebResource = webresource.Content ?? string.Empty;

                    var array = Convert.FromBase64String(contentWebResource);

                    string fileName = string.Format("{0}.{1}", service.ConnectionData.Name, webResourceFileName);
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, array);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Web-resource '{0}' has downloaded to {1}.", name, filePath);

                    this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, "Web-resource not founded in CRM: {0}", name);
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingWebResourceContentCompletedFormat1, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingWebResourceContentFailedFormat1, name);
            }
        }

        private void mIExportWebResourceDependencyXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.dependencyxml, WebResource.Schema.Headers.dependencyxml, "xml", PerformExportXmlToFile);
        }

        private void mIExportWebResourceContentJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.contentjson, WebResource.Schema.Headers.contentjson, "json", PerformExportXmlToFile);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idWebResource, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(service);

                var webresource = await webResourceRepository.FindByIdAsync(idWebResource, new ColumnSet(fieldName));

                string xmlContent = webresource.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, name, fieldTitle, xmlContent, extension);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldTitle);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExportingXmlFieldToFileFailedFormat1, fieldTitle);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idWebResource, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(service);

                var webresource = await webResourceRepository.FindByIdAsync(idWebResource, new ColumnSet(true));

                string xmlContent = webresource.GetAttributeValue<string>(fieldName);

                if (string.Equals(fieldName, WebResource.Schema.Attributes.content, StringComparison.InvariantCultureIgnoreCase))
                {
                    string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

                    var contentWebResource = webresource.Content ?? string.Empty;

                    var array = Convert.FromBase64String(contentWebResource);

                    string fileName = string.Format("{0}.{1} BackUp at {2}{3}", service.ConnectionData.Name, Path.GetFileNameWithoutExtension(webResourceFileName), DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"), Path.GetExtension(webResourceFileName));
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, array);

                    var encodings = ContentCoparerHelper.GetFileEncoding(array);

                    xmlContent = encodings.First().GetString(array);
                }
                else
                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    string filePath = await CreateFileAsync(folder, name, fieldTitle + " BackUp", xmlContent, extension);
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
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                if (string.Equals(fieldName, WebResource.Schema.Attributes.content, StringComparison.InvariantCultureIgnoreCase))
                {
                    var encoding = new UTF8Encoding(false);

                    var bytes = encoding.GetBytes(newText);

                    newText = Convert.ToBase64String(bytes);
                }
                else
                {
                    if (ContentCoparerHelper.TryParseXml(newText, out var doc))
                    {
                        newText = doc.ToString(SaveOptions.DisableFormatting);
                    }
                }

                var updateEntity = new WebResource
                {
                    Id = idWebResource
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.PublishingWebResourceFormat2, service.ConnectionData.Name, name);

                {
                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishWebResourcesAsync(new[] { idWebResource });
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private async Task PerformUpdateEntityFieldFromFile(string folder, Guid idWebResource, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(service);

                var webresource = await webResourceRepository.FindByIdAsync(idWebResource, new ColumnSet(true));

                var allExtensions = WebResourceRepository.GetTypeAllExtensions(webresource.WebResourceType.Value);

                string filter = string.Format("({0})|{1}"
                    , string.Join(";", allExtensions.Select(s => string.Format("{0}", s)))
                    , string.Join(";", allExtensions.Select(s => string.Format("*{0}", s)))
                );

                bool? dialogResult = false;
                string selectedFilePath = string.Empty;

                this.Dispatcher.Invoke(() =>
                {
                    var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = filter,
                        FilterIndex = 1,
                        RestoreDirectory = true,
                        Multiselect = false,
                    };

                    dialogResult = openFileDialog1.ShowDialog();

                    selectedFilePath = openFileDialog1.FileName;
                });

                if (string.IsNullOrEmpty(selectedFilePath)
                    || dialogResult.GetValueOrDefault() == false
                    || !File.Exists(selectedFilePath)
                )
                {
                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                byte[] bytes = File.ReadAllBytes(selectedFilePath);

                {
                    string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

                    var contentWebResource = webresource.Content ?? string.Empty;

                    var array = Convert.FromBase64String(contentWebResource);

                    string fileName = string.Format("{0}.{1} BackUp at {2}{3}", service.ConnectionData.Name, Path.GetFileNameWithoutExtension(webResourceFileName), DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"), Path.GetExtension(webResourceFileName));
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, array);
                }
                

                var updateEntity = new WebResource
                {
                    Id = idWebResource
                };
                updateEntity.Attributes[fieldName] = Convert.ToBase64String(bytes);

                await service.UpdateAsync(updateEntity);

                UpdateStatus(service.ConnectionData, Properties.WindowStatusStrings.PublishingWebResourceFormat2, service.ConnectionData.Name, name);

                {
                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishWebResourcesAsync(new[] { idWebResource });
                }

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private Task<string> CreateFileAsync(string folder, string name, string fieldTitle, string xmlContent, string extension)
        {
            return Task.Run(() => CreateFile(folder, name, fieldTitle, xmlContent, extension));
        }

        private string CreateFile(string folder, string name, string fieldTitle, string xmlContent, string extension)
        {
            name = Path.GetFileName(name);

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetWebResourceFileName(connectionData.Name, name, fieldTitle, extension);
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

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, WebResource.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, WebResource.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
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

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            if (contextMenu.PlacementTarget is TreeViewItem node)
            {
                node.IsSelected = true;
            }

            var items = contextMenu.Items.OfType<Control>();

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityTreeViewItem;

            ActivateControls(items, (nodeItem.WebResource?.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
        }

        private void tSDDBExportWebResource_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var nodeItem = GetSelectedEntity();

            ActivateControls(tSDDBExportWebResource.Items.OfType<Control>(), (nodeItem?.WebResource?.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.WebResource, entity.WebResourceId.Value);
            }
        }

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
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

            var service = await GetService();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.WebResource, entity.WebResourceId.Value);
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
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.WebResource, new[] { entity.WebResourceId.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
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
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWebResourcesWindow(
                _iWriteToOutput
                , _commonConfig
                , service.ConnectionData
                , service.ConnectionData
                , entity?.WebResource?.Name ?? txtBFilter.Text
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
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.trVWebResources.ItemsSource = null;
            });

            if (!this.IsControlsEnabled)
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

        private void mIUpdateWebResourceDependencyXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.dependencyxml, WebResource.Schema.Headers.dependencyxml, "xml", PerformUpdateEntityField);
        }

        private void mIUpdateWebResourceContent_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.content, WebResource.Schema.Headers.content, "js", PerformUpdateEntityField);
        }

        private void mIUpdateWebResourceContentFromFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.content, WebResource.Schema.Headers.content, "js", PerformUpdateEntityFieldFromFile);
        }

        private void mIUpdateWebResourceContentJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.contentjson, WebResource.Schema.Headers.contentjson, "json", PerformUpdateEntityField);
        }

        private void btnPublishWebResource_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformPublishWebResource);
        }

        private async Task PerformPublishWebResource(string folder, Guid idWebResource, string name)
        {
            var service = await GetService();

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingWebResourceFormat2, service.ConnectionData.Name, name);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingWebResourceFormat2, service.ConnectionData.Name, name);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishWebResourcesAsync(new[] { idWebResource });

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingWebResourceCompletedFormat2, service.ConnectionData.Name, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingWebResourceFailedFormat2, service.ConnectionData.Name, name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingWebResourceFormat2, service.ConnectionData.Name, name);
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}