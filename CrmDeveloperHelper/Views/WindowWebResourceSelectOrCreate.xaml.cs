using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowWebResourceSelectOrCreate : WindowBase
    {
        private IWriteToOutput _iWriteToOutput;

        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

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

        private readonly ConnectionData _connectionData;

        private bool _controlsEnabled = true;

        public WindowWebResourceSelectOrCreate(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , ConnectionData connectionData
            , SelectedFile selectedFile
            , Guid? lastLinkedWebResource
            )
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            btnSelectLastLink.IsEnabled = lblLastLink.IsEnabled = txtBLastLink.IsEnabled = sepLastLink.IsEnabled = false;
            btnSelectLastLink.Visibility = lblLastLink.Visibility = txtBLastLink.Visibility = sepLastLink.Visibility = Visibility.Collapsed;

            this._iWriteToOutput = iWriteToOutput;
            this._service = service;
            this._file = selectedFile;
            this._fileExtension = selectedFile.Extension;
            this._connectionData = connectionData;

            this.tSSLblConnectionName.Content = this._connectionData.Name;

            txtBCurrentFile.Text = selectedFile.FriendlyFilePath;

            LoadImages();

            txtBFilter.Text = selectedFile.Name;

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            if (_service != null)
            {
                ShowExistingWebResources(lastLinkedWebResource);
            }
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
            if (!_controlsEnabled)
            {
                return;
            }
            
            ToggleControls(false, Properties.WindowStatusStrings.LoadingWebResources);

            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                this.trVWebResources.ItemsSource = null;
                this.trVWebResources.Items.Clear();
            });

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            List<WebResource> list = null;

            WebResourceRepository repository = new WebResourceRepository(this._service);

            try
            {
                list = await repository.GetListAllAsync(textName, new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype, WebResource.Schema.Attributes.ismanaged, WebResource.Schema.Attributes.ishidden));

                if (lastLinkedWebResource.HasValue && this._lastWebResource == null)
                {
                    this._lastWebResource = await repository.FindByIdAsync(lastLinkedWebResource.Value);

                    string name = this._lastWebResource?.Name;

                    bool isEnabled = this._lastWebResource != null;

                    Visibility visibility = isEnabled ? Visibility.Visible : Visibility.Collapsed;

                    this.Dispatcher.Invoke(() =>
                    {
                        txtBLastLink.Text = name;

                        btnSelectLastLink.IsEnabled = lblLastLink.IsEnabled = txtBLastLink.IsEnabled = sepLastLink.IsEnabled = isEnabled;
                        btnSelectLastLink.Visibility = lblLastLink.Visibility = txtBLastLink.Visibility = sepLastLink.Visibility = visibility;

                        toolStrip.UpdateLayout();
                    });
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                list = new List<WebResource>();
            }

            LoadWebResources(list);
            
            ToggleControls(true, Properties.WindowStatusStrings.LoadingWebResourcesCompletedFormat1, list.Count());
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
            this.trVWebResources.Dispatcher.Invoke(() =>
            {
                try
                {
                    btnCreateNewWebResource.IsEnabled = _controlsEnabled;

                    bool enabled = _controlsEnabled
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

        private void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingWebResources();
            }
        }

        private Guid? GetSelectedEntity()
        {
            Guid? result = null;

            if (this.trVWebResources.SelectedItem != null
                && this.trVWebResources.SelectedItem is EntityTreeViewItem
                )
            {
                result = (this.trVWebResources.SelectedItem as EntityTreeViewItem).WebResourceId;
            }

            return result;
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
                if (((FrameworkElement)e.OriginalSource).DataContext is EntityTreeViewItem item && item.WebResourceId != null)
                {
                    this.SelectedWebResourceId = item.WebResourceId;

                    this.DialogResult = true;
                }
            }
        }

        private void btnSelectWebResource_Click(object sender, RoutedEventArgs e)
        {
            var idWebResource = GetSelectedEntity();

            if (idWebResource == null)
            {
                return;
            }

            this.SelectedWebResourceId = idWebResource.Value;

            this.DialogResult = true;
        }

        private async void btnCreateNewWebResource_Click(object sender, RoutedEventArgs e)
        {
            if (!_controlsEnabled)
            {
                return;
            }
            
            ToggleControls(false, Properties.WindowStatusStrings.PreparingCreatingNewWebResource);

            var extension = _file.Extension;

            if (string.IsNullOrEmpty(extension) || !WebResourceRepository.IsSupportedExtension(extension))
            {
                ToggleControls(true, Properties.WindowStatusStrings.CreatingNewWebResourceDeniedFormat1, extension);

                var message = string.Format(Properties.MessageBoxStrings.FileExtensionIsNotAllowedForWebResourceFormat1, extension);

                MessageBox.Show(message, Properties.MessageBoxStrings.InformationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Solution solution = null;

            if (!string.IsNullOrEmpty(_connectionData.LastSelectedSolutionsUniqueName.FirstOrDefault()) && this.ForAllOther)
            {
                var repositorySolution = new SolutionRepository(_service);

                solution = await repositorySolution.GetSolutionByUniqueNameAsync(_connectionData.LastSelectedSolutionsUniqueName.FirstOrDefault());
            }

            if (solution == null)
            {
                var formSelectSolution = new WindowSolutionSelect(_iWriteToOutput, _service);
                formSelectSolution.ShowForAllOther();

                var dialogResult = formSelectSolution.ShowDialog().GetValueOrDefault();

                _connectionData.AddLastSelectedSolution(formSelectSolution.SelectedSolution?.UniqueName);

                if (!dialogResult)
                {
                    this.ForAllOther = false;

                    ToggleControls(true, Properties.WindowStatusStrings.CreatingNewWebResourceCanceled);
                    return;
                }

                solution = formSelectSolution.SelectedSolution;

                _connectionData.AddLastSelectedSolution(solution?.UniqueName);
                this.ForAllOther = !string.IsNullOrEmpty(_connectionData.LastSelectedSolutionsUniqueName.FirstOrDefault()) && formSelectSolution.ForAllOther;
            }

            if (solution == null)
            {
                this.ForAllOther = false;
                ToggleControls(true, Properties.WindowStatusStrings.SolutionForCreatingNewWebResouceNotSelected);
                
                return;
            }

            this._iWriteToOutput.WriteToOutputSolutionUri(_connectionData, solution.UniqueName, solution.Id);

            var formWebResourceInfo = new WindowWebResourceCreate(_file.FileName, _file.FriendlyFilePath, solution.UniqueName, solution.PublisherCustomizationPrefix);

            {
                var dialogResult = formWebResourceInfo.ShowDialog().GetValueOrDefault();

                if (!dialogResult)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.CreatingNewWebResourceCanceled);
                    return;
                }
            }

            var name = formWebResourceInfo.WebResourceName;
            var displayName = formWebResourceInfo.WebResourceDisplayName;
            var description = formWebResourceInfo.WebResourceDescription;

            name = WebResourceRepository.GenerateWebResouceName(name, solution.PublisherCustomizationPrefix);

            UpdateStatus(Properties.WindowStatusStrings.CreatingNewWebResourceFormat1, name);

            WebResourceRepository repository = new WebResourceRepository(_service);

            try
            {
                Guid id = await repository.CreateNewWebResourceAsync(name, displayName, description, extension, solution.UniqueName);

                this.SelectedWebResourceId = id;

                this.DialogResult = true;

                this.Close();

                ToggleControls(true, Properties.WindowStatusStrings.CreatingNewWebResourceCompletedFormat1, name);
            }
            catch (Exception xE)
            {
                ToggleControls(true, Properties.WindowStatusStrings.CreatingNewWebResourceFailed, name);

                this._iWriteToOutput.WriteErrorToOutput(xE);
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingWebResources();
            }

            base.OnKeyDown(e);
        }
    }
}
