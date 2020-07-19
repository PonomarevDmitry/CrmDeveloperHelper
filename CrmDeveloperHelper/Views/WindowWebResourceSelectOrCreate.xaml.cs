using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
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
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowWebResourceSelectOrCreate : WindowWithSingleConnection
    {
        private readonly CommonConfiguration _commonConfig;

        private string _fileExtension;

        /// <summary>
        /// Выбранный файл
        /// </summary>
        private SelectedFile _file;

        /// <summary>
        /// Последний веб-ресурс, с которым сравнивался файл.
        /// </summary>
        private WebResource _lastWebResource;

        /// <summary>
        /// ИД залинкованного веб-ресурса
        /// </summary>
        public Guid? SelectedWebResourceId { get; private set; }

        public bool ShowNext { get; private set; }

        public bool ForAllOther { get; private set; }

        private readonly ObservableCollection<EntityTreeViewItem> _webResourceTree = new ObservableCollection<EntityTreeViewItem>();

        public WindowWebResourceSelectOrCreate(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SelectedFile selectedFile
            , Guid? lastLinkedWebResource
        ) : base(iWriteToOutput, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            btnSelectLastLink.IsEnabled = gridLastLink.IsEnabled = sepLastLink.IsEnabled = false;
            btnSelectLastLink.Visibility = gridLastLink.Visibility = sepLastLink.Visibility = Visibility.Collapsed;

            this._file = selectedFile;
            this._fileExtension = selectedFile.Extension;
            this._commonConfig = CommonConfiguration.Get();

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            txtBCurrentFile.Text = selectedFile.FriendlyFilePath;

            LoadImages();

            txtBFilter.Text = selectedFile.Name;

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            trVWebResources.ItemsSource = _webResourceTree;

            txtBFilter.Focus();

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingWebResources(lastLinkedWebResource);
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, () => Task.FromResult(_service)
                , getWebResourceName: GetWebResourceName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(_service.ConnectionData, _service.ConnectionData)
                , getWebResourceName: GetWebResourceName
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

        private string GetWebResourceName()
        {
            var entity = GetSelectedEntity();

            return entity?.WebResource?.Name ?? txtBFilter.Text.Trim();
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

        private async Task ShowExistingWebResources(Guid? lastLinkedWebResource = null)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingWebResources);

            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                _webResourceTree.Clear();
            });

            string textName = string.Empty;
            bool? hidden = null;
            bool? managed = null;
            int? webResourceType = null;

            this.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();

                if (cmBManaged.SelectedItem is ComboBoxItem comboBoxItemManaged
                    && comboBoxItemManaged.Tag != null
                    && comboBoxItemManaged.Tag is bool boolManaged
                )
                {
                    managed = boolManaged;
                }

                if (cmBHidden.SelectedItem is ComboBoxItem comboBoxItemHidden
                    && comboBoxItemHidden.Tag != null
                    && comboBoxItemHidden.Tag is bool boolHidden
                )
                {
                    hidden = boolHidden;
                }

                if (cmBType.SelectedItem is WebResource.Schema.OptionSets.webresourcetype webresourcetype)
                {
                    webResourceType = (int)webresourcetype;
                }
            });

            List<WebResource> list = null;

            WebResourceRepository repository = new WebResourceRepository(this._service);

            try
            {
                list = await repository.GetListAllAsync(
                    textName
                    , webResourceType
                    , managed
                    , hidden
                    , new ColumnSet
                    (
                        WebResource.Schema.Attributes.name
                        , WebResource.Schema.Attributes.webresourcetype
                        , WebResource.Schema.Attributes.ismanaged
                        , WebResource.Schema.Attributes.ishidden
                    )
                );

                if (lastLinkedWebResource.HasValue && this._lastWebResource == null)
                {
                    this._lastWebResource = await repository.GetByIdAsync(lastLinkedWebResource.Value);

                    string name = this._lastWebResource?.Name;

                    bool isEnabled = this._lastWebResource != null;

                    Visibility visibility = isEnabled ? Visibility.Visible : Visibility.Collapsed;

                    this.Dispatcher.Invoke(() =>
                    {
                        txtBLastLink.Text = name;

                        btnSelectLastLink.IsEnabled = gridLastLink.IsEnabled = sepLastLink.IsEnabled = isEnabled;
                        btnSelectLastLink.Visibility = gridLastLink.Visibility = sepLastLink.Visibility = visibility;

                        toolStrip.UpdateLayout();
                    });
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                list = new List<WebResource>();
            }

            LoadWebResources(list);

            ToggleControls(true, Properties.OutputStrings.LoadingWebResourcesCompletedFormat1, list.Count());
        }

        private void LoadWebResources(IEnumerable<WebResource> results)
        {
            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                this.trVWebResources.BeginInit();
            });

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

                this.trVWebResources.Dispatcher.Invoke(() =>
                {
                    _webResourceTree.Add(node);
                });
            }

            ExpandNode(_webResourceTree);

            this.trVWebResources.Dispatcher.Invoke(() =>
            {
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

                if (result.Length > 0)
                {
                    tn.Description = result.ToString();
                }

                node.Items.Add(tn);
            }

            if (node.Items.Count > 0)
            {
                node.Description = node.Items.Count.ToString();
            }
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

        protected override void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(tSProgressBar
                , cmBType
                , cmBManaged
                , cmBHidden
            );

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                try
                {
                    btnCreateNewWebResource.IsEnabled = this.IsControlsEnabled;

                    bool enabled = this.IsControlsEnabled
                                    && this.trVWebResources.SelectedItem != null
                                    && this.trVWebResources.SelectedItem is EntityTreeViewItem
                                    && (this.trVWebResources.SelectedItem as EntityTreeViewItem).WebResourceId != null;

                    UIElement[] list = { btnSelectWebResource };

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
                await ShowExistingWebResources();
            }
        }

        private EntityTreeViewItem GetSelectedEntity()
        {
            if (this.trVWebResources.SelectedItem != null
                && this.trVWebResources.SelectedItem is EntityTreeViewItem entity
            )
            {
                return entity;
            }

            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void trVWebResources_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateButtonsEnable();
        }

        private void trVWebResources_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityTreeViewItem item = GetItemFromRoutedDataContext<EntityTreeViewItem>(e);

                if (item != null && item.WebResourceId != null)
                {
                    this.SelectedWebResourceId = item.WebResourceId;

                    this.DialogResult = true;
                }
            }
        }

        private void btnSelectWebResource_Click(object sender, RoutedEventArgs e)
        {
            var webResource = GetSelectedEntity();

            if (webResource == null || !webResource.WebResourceId.HasValue)
            {
                return;
            }

            this.SelectedWebResourceId = webResource.WebResourceId.Value;

            this.DialogResult = true;
        }

        private async void btnCreateNewWebResource_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.PreparingCreatingNewWebResource);

            var extension = _file.Extension;

            if (string.IsNullOrEmpty(extension) || !WebResourceRepository.IsSupportedExtension(extension))
            {
                ToggleControls(true, Properties.OutputStrings.CreatingNewWebResourceDeniedFormat1, extension);

                var message = string.Format(Properties.MessageBoxStrings.FileExtensionIsNotAllowedForWebResourceFormat1, extension);

                MessageBox.Show(message, Properties.MessageBoxStrings.InformationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Solution solution = null;

            if (!string.IsNullOrEmpty(_service.ConnectionData.LastSelectedSolutionsUniqueName.FirstOrDefault()) && this.ForAllOther)
            {
                var repositorySolution = new SolutionRepository(_service);

                solution = await repositorySolution.GetSolutionByUniqueNameAsync(_service.ConnectionData.LastSelectedSolutionsUniqueName.FirstOrDefault());
            }

            if (solution == null)
            {
                var formSelectSolution = new WindowSolutionSelect(_iWriteToOutput, _service);
                formSelectSolution.ShowForAllOther();

                var dialogResult = formSelectSolution.ShowDialog().GetValueOrDefault();

                _service.ConnectionData.AddLastSelectedSolution(formSelectSolution.SelectedSolution?.UniqueName);

                if (!dialogResult)
                {
                    this.ForAllOther = false;

                    ToggleControls(true, Properties.OutputStrings.CreatingNewWebResourceCanceled);
                    return;
                }

                solution = formSelectSolution.SelectedSolution;

                _service.ConnectionData.AddLastSelectedSolution(solution?.UniqueName);
                this.ForAllOther = !string.IsNullOrEmpty(_service.ConnectionData.LastSelectedSolutionsUniqueName.FirstOrDefault()) && formSelectSolution.ForAllOther;
            }

            if (solution == null)
            {
                this.ForAllOther = false;
                ToggleControls(true, Properties.OutputStrings.SolutionForCreatingNewWebResouceNotSelected);

                return;
            }

            this._iWriteToOutput.WriteToOutputSolutionUri(_service.ConnectionData, solution.UniqueName, solution.Id);

            var formWebResourceInfo = new WindowWebResourceCreate(_file.FileName, _file.FriendlyFilePath, solution.UniqueName, solution.PublisherCustomizationPrefix);

            {
                var dialogResult = formWebResourceInfo.ShowDialog().GetValueOrDefault();

                if (!dialogResult)
                {
                    ToggleControls(true, Properties.OutputStrings.CreatingNewWebResourceCanceled);
                    return;
                }
            }

            var name = formWebResourceInfo.WebResourceName;
            var displayName = formWebResourceInfo.WebResourceDisplayName;
            var description = formWebResourceInfo.WebResourceDescription;

            name = WebResourceRepository.GenerateWebResouceName(name, solution.PublisherCustomizationPrefix);

            UpdateStatus(Properties.OutputStrings.CreatingNewWebResourceFormat1, name);

            WebResourceRepository repository = new WebResourceRepository(_service);

            try
            {
                Guid id = await repository.CreateNewWebResourceAsync(name, displayName, description, extension, solution.UniqueName);

                this.SelectedWebResourceId = id;

                this.DialogResult = true;

                this.Close();

                ToggleControls(true, Properties.OutputStrings.CreatingNewWebResourceCompletedFormat1, name);
            }
            catch (Exception ex)
            {
                ToggleControls(true, Properties.OutputStrings.CreatingNewWebResourceFailed, name);

                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void btnSelectLastLink_Click(object sender, RoutedEventArgs e)
        {
            if (this._lastWebResource != null)
            {
                this.SelectedWebResourceId = this._lastWebResource.Id;

                this.DialogResult = true;
            }
        }

        private void btnSkipFile_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedWebResourceId = null;

            this.ShowNext = true;

            this.DialogResult = false;
        }

        public void ShowSkipButton()
        {
            sepSkipFile.IsEnabled = btnSkipFile.IsEnabled = true;
            sepSkipFile.Visibility = btnSkipFile.Visibility = Visibility.Visible;
        }

        public void ShowCreateButton(bool allForOther)
        {
            this.ForAllOther = allForOther;

            sepCreateNewWebResource.IsEnabled = btnCreateNewWebResource.IsEnabled = true;
            sepCreateNewWebResource.Visibility = btnCreateNewWebResource.Visibility = Visibility.Visible;
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingWebResources();
        }

        private async void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            await ShowExistingWebResources();
        }

        #region Expand

        private void mIExpandNodes_Click(object sender, RoutedEventArgs e)
        {
            EntityTreeViewItem nodeItem = GetItemFromRoutedDataContext<EntityTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, true);
        }

        private void mICollapseNodes_Click(object sender, RoutedEventArgs e)
        {
            EntityTreeViewItem nodeItem = GetItemFromRoutedDataContext<EntityTreeViewItem>(e);

            if (nodeItem == null)
            {
                return;
            }

            ChangeExpandedInTreeViewItems(new[] { nodeItem }, false);
        }

        private void hypLinkExpandAll_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ChangeExpandedInTreeViewItems(_webResourceTree, true);
        }

        private void hypLinkCollapseAll_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ChangeExpandedInTreeViewItems(_webResourceTree, false);
        }

        private void ChangeExpandedInTreeViewItems(IEnumerable<EntityTreeViewItem> items, bool isExpanded)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                this.trVWebResources.BeginInit();
            });

            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;

                ChangeExpandedInTreeViewItemsRecursive(item.Items, isExpanded);
            }

            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                this.trVWebResources.EndInit();
            });
        }

        private void ChangeExpandedInTreeViewItemsRecursive(IEnumerable<EntityTreeViewItem> items, bool isExpanded)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;

                ChangeExpandedInTreeViewItemsRecursive(item.Items, isExpanded);
            }
        }

        #endregion

        #region Context Menu

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

            FillLastSolutionItems(_service.ConnectionData, items, true, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            EntityTreeViewItem nodeItem = GetItemFromRoutedDataContext<EntityTreeViewItem>(e);

            ActivateControls(items, (nodeItem.WebResource?.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
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

            _service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.WebResource, entity.WebResourceId.Value);
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

            _service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.WebResource, entity.WebResourceId.Value);
        }

        private async void mIOpenContent_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformExportWebResourceContent);
        }

        private async Task ExecuteAction(Guid idWebResource, string name, Func<string, Guid, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idWebResource, name);
        }

        private async Task ExecuteActionEntity(Guid idWebResource, string name, string fieldName, string fieldTitle, string extension, Func<string, Guid, string, string, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idWebResource, name, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportWebResourceContent(string folder, Guid idWebResource, string name)
        {
            ToggleControls(false, Properties.OutputStrings.ExportingWebResourceContentFormat1, name);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(_service);

                var webresource = await webResourceRepository.GetByIdAsync(idWebResource, new ColumnSet(WebResource.Schema.Attributes.content, WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype));

                if (webresource != null && !string.IsNullOrEmpty(webresource.Content))
                {
                    this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Starting downloading {0}", name);

                    string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

                    var contentWebResource = webresource.Content ?? string.Empty;

                    var array = Convert.FromBase64String(contentWebResource);

                    string fileName = string.Format("{0}.{1}", _service.ConnectionData.Name, webResourceFileName);
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, array);

                    this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Web-resource '{0}' has downloaded to {1}.", name, filePath);

                    this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Web-resource not founded in CRM: {0}", name);
                }

                ToggleControls(true, Properties.OutputStrings.ExportingWebResourceContentCompletedFormat1, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.ExportingWebResourceContentFailedFormat1, name);
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

            var descriptor = new SolutionComponentDescriptor(_service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, _service, descriptor, _commonConfig, solutionUniqueName, ComponentType.WebResource, new[] { entity.WebResourceId.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || !entity.WebResourceId.HasValue)
            {
                return;
            }

            _commonConfig.Save();

            var descriptor = new SolutionComponentDescriptor(_service);

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , _service
                , descriptor
                , _commonConfig
                , (int)ComponentType.WebResource
                , entity.WebResourceId.Value
                , null
            );
        }

        private async void mIExportWebResourceDependencyXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.dependencyxml, WebResource.Schema.Headers.dependencyxml, "xml", PerformExportXmlToFile);
        }

        private async void mIExportWebResourceContentJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.contentjson, WebResource.Schema.Headers.contentjson, "json", PerformExportXmlToFile);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idWebResource, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(_service);

                var webresource = await webResourceRepository.GetByIdAsync(idWebResource, new ColumnSet(true));

                string xmlContent = webresource.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(xmlContent))
                {
                    _commonConfig.Save();

                    if (string.Equals(fieldName, WebResource.Schema.Attributes.dependencyxml, StringComparison.InvariantCultureIgnoreCase))
                    {
                        xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            xmlContent
                            , _commonConfig
                            , XmlOptionsControls.WebResourceDependencyXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                            , webResourceName: webresource.Name
                        );
                    }
                    else if (string.Equals(fieldName, WebResource.Schema.Attributes.contentjson, StringComparison.InvariantCultureIgnoreCase))
                    {
                        xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                    }
                }

                string filePath = await CreateFileAsync(folder, name, fieldTitle, xmlContent, extension);

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldTitle);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldTitle);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idWebResource, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.UpdatingFieldFormat2, _service.ConnectionData.Name, fieldName);

            try
            {
                _commonConfig.Save();

                WebResourceRepository webResourceRepository = new WebResourceRepository(_service);

                var webresource = await webResourceRepository.GetByIdAsync(idWebResource, new ColumnSet(true));

                string xmlContent = webresource.GetAttributeValue<string>(fieldName);

                if (string.Equals(fieldName, WebResource.Schema.Attributes.content, StringComparison.InvariantCultureIgnoreCase))
                {
                    string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

                    var contentWebResource = webresource.Content ?? string.Empty;

                    var array = Convert.FromBase64String(contentWebResource);

                    string fileName = string.Format("{0}.{1} BackUp at {2}{3}", _service.ConnectionData.Name, Path.GetFileNameWithoutExtension(webResourceFileName), DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"), Path.GetExtension(webResourceFileName));
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, array);

                    var encodings = ContentComparerHelper.GetFileEncoding(array);

                    xmlContent = encodings.First().GetString(array);
                }
                else
                {
                    if (string.Equals(fieldName, WebResource.Schema.Attributes.dependencyxml, StringComparison.InvariantCultureIgnoreCase))
                    {
                        xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            xmlContent
                            , _commonConfig
                            , XmlOptionsControls.WebResourceDependencyXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlSchema
                            , webResourceName: webresource.Name
                        );
                    }

                    await CreateFileAsync(folder, name, fieldTitle + " BackUp", xmlContent, extension);
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
                    ToggleControls(true, Properties.OutputStrings.UpdatingFieldFailedFormat2, _service.ConnectionData.Name, fieldName);
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
                    if (string.Equals(fieldName, WebResource.Schema.Attributes.dependencyxml, StringComparison.InvariantCultureIgnoreCase))
                    {
                        newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

                        if (ContentComparerHelper.TryParseXml(newText, out var doc))
                        {
                            newText = doc.ToString(SaveOptions.DisableFormatting);
                        }
                    }
                }

                var updateEntity = new WebResource
                {
                    Id = idWebResource
                };
                updateEntity.Attributes[fieldName] = newText;

                await _service.UpdateAsync(updateEntity);

                UpdateStatus(Properties.OutputStrings.PublishingWebResourceFormat2, _service.ConnectionData.Name, name);

                {
                    var repositoryPublish = new PublishActionsRepository(_service);

                    await repositoryPublish.PublishWebResourcesAsync(new[] { idWebResource });
                }

                ToggleControls(true, Properties.OutputStrings.UpdatingFieldCompletedFormat2, _service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.UpdatingFieldFailedFormat2, _service.ConnectionData.Name, fieldName);
            }
        }

        private async Task PerformUpdateEntityFieldFromFile(string folder, Guid idWebResource, string name, string fieldName, string fieldTitle, string extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.UpdatingFieldFormat2, _service.ConnectionData.Name, fieldName);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(_service);

                var webresource = await webResourceRepository.GetByIdAsync(idWebResource, new ColumnSet(true));

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
                    ToggleControls(true, Properties.OutputStrings.UpdatingFieldFailedFormat2, _service.ConnectionData.Name, fieldName);
                    return;
                }

                byte[] bytes = File.ReadAllBytes(selectedFilePath);

                {
                    string webResourceFileName = WebResourceRepository.GetWebResourceFileName(webresource);

                    var contentWebResource = webresource.Content ?? string.Empty;

                    var array = Convert.FromBase64String(contentWebResource);

                    string fileName = string.Format("{0}.{1} BackUp at {2}{3}", _service.ConnectionData.Name, Path.GetFileNameWithoutExtension(webResourceFileName), DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss"), Path.GetExtension(webResourceFileName));
                    string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, array);
                }


                var updateEntity = new WebResource
                {
                    Id = idWebResource
                };
                updateEntity.Attributes[fieldName] = Convert.ToBase64String(bytes);

                await _service.UpdateAsync(updateEntity);

                UpdateStatus(Properties.OutputStrings.PublishingWebResourceFormat2, _service.ConnectionData.Name, name);

                {
                    var repositoryPublish = new PublishActionsRepository(_service);

                    await repositoryPublish.PublishWebResourcesAsync(new[] { idWebResource });
                }

                ToggleControls(true, Properties.OutputStrings.UpdatingFieldCompletedFormat2, _service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.UpdatingFieldFailedFormat2, _service.ConnectionData.Name, fieldName);
            }
        }

        private Task<string> CreateFileAsync(string folder, string name, string fieldTitle, string xmlContent, string extension)
        {
            return Task.Run(() => CreateFile(folder, name, fieldTitle, xmlContent, extension));
        }

        private string CreateFile(string folder, string name, string fieldTitle, string xmlContent, string extension)
        {
            name = Path.GetFileName(name);

            string fileName = EntityFileNameFormatter.GetWebResourceFileName(_service.ConnectionData.Name, name, fieldTitle, extension);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, _service.ConnectionData.Name, WebResource.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, _service.ConnectionData.Name, WebResource.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
            }

            return filePath;
        }

        private async void mIUpdateWebResourceDependencyXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.dependencyxml, WebResource.Schema.Headers.dependencyxml, "xml", PerformUpdateEntityField);
        }

        private async void mIUpdateWebResourceContent_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.content, WebResource.Schema.Headers.content, "js", PerformUpdateEntityField);
        }

        private async void mIUpdateWebResourceContentFromFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.content, WebResource.Schema.Headers.content, "js", PerformUpdateEntityFieldFromFile);
        }

        private async void mIUpdateWebResourceContentJson_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.WebResourceId.Value, entity.Name, WebResource.Schema.Attributes.contentjson, WebResource.Schema.Headers.contentjson, "json", PerformUpdateEntityField);
        }

        private async void btnPublishWebResource_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformPublishWebResource);
        }

        private async Task PerformPublishWebResource(string folder, Guid idWebResource, string name)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(_service.ConnectionData, Properties.OperationNames.PublishingWebResourceFormat2, _service.ConnectionData.Name, name);

            ToggleControls(false, Properties.OutputStrings.PublishingWebResourceFormat2, _service.ConnectionData.Name, name);

            try
            {
                var repository = new PublishActionsRepository(_service);

                await repository.PublishWebResourcesAsync(new[] { idWebResource });

                ToggleControls(true, Properties.OutputStrings.PublishingWebResourceCompletedFormat2, _service.ConnectionData.Name, name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.PublishingWebResourceFailedFormat2, _service.ConnectionData.Name, name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(_service.ConnectionData, Properties.OperationNames.PublishingWebResourceFormat2, _service.ConnectionData.Name, name);
        }

        private void mIOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || !entity.WebResourceId.HasValue)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , _service
                , _commonConfig
                , (int)ComponentType.WebResource
                , entity.WebResourceId.Value
                , null
            );
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformExportEntityDescription);
        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformEntityEditorAsync);
        }

        private async void mIDeleteWebResource_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformDeleteWebResource);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idWebResource, string name)
        {
            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                WebResourceRepository webResourceRepository = new WebResourceRepository(_service);

                var webresource = await webResourceRepository.GetByIdAsync(idWebResource, new ColumnSet(true));

                string fileName = EntityFileNameFormatter.GetWebResourceFileName(_service.ConnectionData.Name, name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, webresource, _service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData
                    , Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , _service.ConnectionData.Name
                    , webresource.LogicalName
                    , filePath
                );

                this._iWriteToOutput.PerformAction(_service.ConnectionData, filePath);

                ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionFailed);
            }
        }

        private Task PerformEntityEditorAsync(string folder, Guid idWebResource, string name)
        {
            return Task.Run(() => PerformEntityEditor(folder, idWebResource, name));
        }

        private void PerformEntityEditor(string folder, Guid idWebResource, string name)
        {
            _commonConfig.Save();

            var repositoryPublish = new PublishActionsRepository(_service);

            WindowHelper.OpenEntityEditor(_iWriteToOutput, _service, _commonConfig, WebResource.EntityLogicalName, idWebResource);
        }

        private async Task PerformDeleteWebResource(string folder, Guid idWebResource, string name)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, WebResource.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                ToggleControls(false, Properties.OutputStrings.DeletingEntityFormat2, _service.ConnectionData.Name, WebResource.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(_service.ConnectionData, WebResource.EntityLogicalName, idWebResource);

                    await _service.DeleteAsync(WebResource.EntityLogicalName, idWebResource);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                }

                ToggleControls(true, Properties.OutputStrings.DeletingEntityCompletedFormat2, _service.ConnectionData.Name, WebResource.EntityLogicalName);

                await ShowExistingWebResources();
            }
        }

        private async void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null || entity.WebResourceId == null)
            {
                return;
            }

            await ExecuteAction(entity.WebResourceId.Value, entity.Name, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idWebResource, string name)
        {
            await PerformExportEntityDescription(folder, idWebResource, name);

            await PerformExportWebResourceContent(folder, idWebResource, name);

            await PerformExportXmlToFile(folder, idWebResource, name, WebResource.Schema.Attributes.dependencyxml, WebResource.Schema.Headers.dependencyxml, "xml");

            await PerformExportXmlToFile(folder, idWebResource, name, WebResource.Schema.Attributes.contentjson, WebResource.Schema.Headers.contentjson, "json");
        }

        #endregion Context Menu

        private void trVWebResources_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void trVWebResources_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteWebResource_Click(sender, e);
        }
    }
}