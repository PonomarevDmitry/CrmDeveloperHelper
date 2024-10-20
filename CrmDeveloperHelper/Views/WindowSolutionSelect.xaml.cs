using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSolutionSelect : WindowWithSingleConnection
    {
        private readonly ObservableCollection<SolutionViewItem> _itemsSource;

        private object _syncObject = new object();

        public Solution SelectedSolution { get; private set; }

        public bool ForAllOther => chBForAllOther.IsChecked.GetValueOrDefault();

        public WindowSolutionSelect(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput, service)
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.LastSelectedSolutionsUniqueName, _syncObject);

            this._itemsSource = new ObservableCollection<SolutionViewItem>();

            this.lstVwSolutions.ItemsSource = _itemsSource;

            cmBFilter.DataContext = this._service.ConnectionData;
            cmBLastSelectedSolution.DataContext = this._service.ConnectionData;

            FocusOnComboBoxTextBox(cmBFilter);

            var task = ShowExistingSolutions();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (cmBLastSelectedSolution.SelectedIndex == -1 && cmBLastSelectedSolution.Items.Count > 0)
            {
                cmBLastSelectedSolution.SelectedIndex = 0;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            BindingOperations.ClearAllBindings(cmBFilter);
            BindingOperations.ClearAllBindings(cmBLastSelectedSolution);

            cmBFilter.Items.DetachFromSourceCollection();
            cmBFilter.DataContext = null;
            cmBFilter.ItemsSource = null;

            cmBLastSelectedSolution.Items.DetachFromSourceCollection();
            cmBLastSelectedSolution.DataContext = null;
            cmBLastSelectedSolution.ItemsSource = null;

            base.OnClosed(e);
        }

        private async Task ShowExistingSolutions()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingSolutions);

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();
            });

            IEnumerable<Solution> list = Enumerable.Empty<Solution>();

            try
            {
                var repository = new SolutionRepository(this._service);

                string textName = string.Empty;

                cmBFilter.Dispatcher.Invoke(() =>
                {
                    textName = cmBFilter.Text?.Trim().ToLower();
                });

                list = await repository.FindSolutionsVisibleUnmanagedAsync(textName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }

            this.lstVwSolutions.Dispatcher.Invoke(() =>
            {
                foreach (var solution in list)
                {
                    _itemsSource.Add(new SolutionViewItem(solution));
                }

                if (_itemsSource.Count == 1)
                {
                    this.lstVwSolutions.SelectedItem = this.lstVwSolutions.Items[0];
                }
            });

            ToggleControls(true, Properties.OutputStrings.LoadingSolutionsCompletedFormat1, list.Count());
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

            ToggleControl(this.tSProgressBar, this.btnSelectSolution, this.gridLastSelectedSolution);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSolutions.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwSolutions.SelectedItems.Count > 0;

                    UIElement[] list = { btnSelectSolution, btnOpenSolutionInWeb };

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

        private async void cmBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingSolutions();
            }
        }

        private Solution GetSelectedEntity()
        {
            return this.lstVwSolutions.SelectedItems.OfType<SolutionViewItem>().Count() == 1
                ? this.lstVwSolutions.SelectedItems.OfType<SolutionViewItem>().Select(e => e.Solution).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                SolutionViewItem item = GetItemFromRoutedDataContext<SolutionViewItem>(e);

                if (item != null && item.Solution != null)
                {
                    SelectSolutioAction(item.Solution);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void SelectSolutioAction(Solution solution)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (solution == null)
            {
                return;
            }

            this.SelectedSolution = solution;

            var text = cmBFilter.Text;

            this._service.ConnectionData.AddLastSelectedSolution(solution.UniqueName);

            _iWriteToOutput.WriteToOutputSolutionUri(_service.ConnectionData, solution.UniqueName, solution.Id);

            cmBFilter.Text = text;

            this.DialogResult = true;

            this.Close();
        }

        private void btnSelectSolution_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            SelectSolutioAction(entity);
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSolutions();
        }

        private async void btnSelectLastSolution_Click(object sender, RoutedEventArgs e)
        {
            string lastSolutionUniqueName = cmBLastSelectedSolution.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(lastSolutionUniqueName))
            {
                return;
            }

            var repository = new SolutionRepository(this._service);

            var lastSolution = await repository.GetSolutionByUniqueNameAsync(lastSolutionUniqueName);

            if (lastSolution == null)
            {
                return;
            }

            if (lastSolution.IsManaged.GetValueOrDefault() || !lastSolution.IsVisible.GetValueOrDefault())
            {
                return;
            }

            this.SelectedSolution = lastSolution;

            this._service.ConnectionData.AddLastSelectedSolution(lastSolution.UniqueName);

            _iWriteToOutput.WriteToOutputSolutionUri(_service.ConnectionData, lastSolution.UniqueName, lastSolution.Id);

            this.DialogResult = true;
        }

        private void btnOpenSolutionInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            this._service.ConnectionData.OpenSolutionInWeb(entity.Id);
        }

        private void mIOpenSolutionListInWeb_Click(object sender, RoutedEventArgs e)
        {
            this._service.ConnectionData.OpenCrmWebSite(OpenCrmWebSiteType.Solutions);
        }

        private void mIOpenCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            this._service.ConnectionData.OpenCrmWebSite(OpenCrmWebSiteType.Customization);
        }

        private void btnOpenComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenSolutionComponentsExplorer(this._iWriteToOutput, _service, null, commonConfig, entity.UniqueName, null);
        }

        public void ShowForAllOther()
        {
            sepForAllOther.IsEnabled = chBForAllOther.IsEnabled = true;
            sepForAllOther.Visibility = chBForAllOther.Visibility = Visibility.Visible;
        }

        private void mICreateNewSolutionInBrowser_Click(object sender, RoutedEventArgs e)
        {
            this._service.ConnectionData.OpenSolutionCreateInWeb();
        }

        private async void mICreateNewSolutionInEditor_Click(object sender, RoutedEventArgs e)
        {
            var commonConfig = CommonConfiguration.Get();

            var repositoryPublisher = new PublisherRepository(_service);

            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            var newSolution = new Solution()
            {
                FriendlyName = string.Empty,
                UniqueName = string.Empty,
                Version = "1.0.0.0",
                PublisherId = null,
            };

            if (publisherDefault != null)
            {
                newSolution.PublisherId = new Microsoft.Xrm.Sdk.EntityReference(publisherDefault.LogicalName, publisherDefault.Id)
                {
                    Name = publisherDefault.FriendlyName
                };
            }

            WindowHelper.OpenEntityEditor(_iWriteToOutput, _service, commonConfig, Solution.EntityLogicalName, newSolution);
        }

        private async void ClearUnmanagedSolution_Click(object sender, RoutedEventArgs e)
        {
            var solution = GetSelectedEntity();

            if (solution == null)
            {
                return;
            }

            string question = string.Format(Properties.MessageBoxStrings.ClearSolutionFormat1, solution.UniqueName);

            if (MessageBox.Show(question, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            try
            {

                ToggleControls(false, Properties.OutputStrings.InConnectionClearingSolutionFormat2, _service.ConnectionData.Name, solution.UniqueName);

                var commonConfig = CommonConfiguration.Get();

                var descriptor = new SolutionComponentDescriptor(_service);
                descriptor.SetSettings(commonConfig);

                var solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, descriptor);

                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, string.Empty);
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Creating backup Solution Components in '{0}'.", solution.UniqueName);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , solution.UniqueName
                        , "Components Backup"
                        , FileExtension.txt
                    );

                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solution.Id);

                    this._iWriteToOutput.WriteToOutput(_service.ConnectionData, "Created backup Solution Components in '{0}': {1}", solution.UniqueName, filePath);
                    this._iWriteToOutput.WriteToOutputFilePathUri(_service.ConnectionData, filePath);
                }

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , solution.UniqueName
                        , "SolutionImage Backup before Clearing"
                        , FileExtension.xml
                    );

                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    SolutionImage solutionImage = await solutionDescriptor.CreateSolutionImageAsync(solution.Id, solution.UniqueName);

                    await solutionImage.SaveAsync(filePath);
                }

                var repository = new SolutionComponentRepository(_service);

                await repository.ClearSolutionAsync(solution.UniqueName);

                ToggleControls(true, Properties.OutputStrings.InConnectionClearingSolutionCompletedFormat2, _service.ConnectionData.Name, solution.UniqueName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.InConnectionClearingSolutionFailedFormat2, _service.ConnectionData.Name, solution.UniqueName);
            }
        }

        private void cmBLastSelectedSolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSolution = cmBLastSelectedSolution.SelectedItem?.ToString();

            btnSelectLastSolution1.IsEnabled = btnSelectLastSolution2.IsEnabled = !string.IsNullOrEmpty(selectedSolution);
        }

        #region Clipboard

        private void mICopyEntityInstanceIdToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is SolutionViewItem entity)
                )
            {
                return;
            }

            ClipboardHelper.SetText(entity.Solution.Id.ToString());
        }

        private void mICopyEntityInstanceUrlToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is SolutionViewItem entity)
                )
            {
                return;
            }

            var url = _service.ConnectionData.GetSolutionUrl(entity.Solution.Id);

            ClipboardHelper.SetText(url);
        }

        private void mICopySolutionUniqueNameToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionViewItem>(e, ent => ent.UniqueName);
        }

        private void mICopySolutionFriendlyNameToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionViewItem>(e, ent => ent.FriendlyName);
        }

        private void mICopySolutionVersionToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionViewItem>(e, ent => ent.Version);
        }

        private void mICopySolutionPublisherNameToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionViewItem>(e, ent => ent.PublisherName);
        }

        private void mICopySolutionPrefixToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionViewItem>(e, ent => ent.Prefix);
        }

        private void mICopySolutionDescriptionToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<SolutionViewItem>(e, ent => ent.Description);
        }

        #endregion Clipboard
    }
}
