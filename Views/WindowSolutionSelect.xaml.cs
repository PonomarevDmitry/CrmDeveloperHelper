using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSolutionSelect : WindowBase
    {
        private IWriteToOutput _iWriteToOutput;

        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        private bool _controlsEnabled = true;
        private Solution _lastSolution;
        private ObservableCollection<EntityViewItem> _itemsSource;

        private object _syncObject = new object();

        public Solution SelectedSolution { get; private set; }

        public bool ForAllOther
        {
            get
            {
                return chBForAllOther.IsChecked.GetValueOrDefault();
            }
        }

        public WindowSolutionSelect(
            IWriteToOutput outputWindow
            , IOrganizationServiceExtented service
        )
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._service = service;
            this._iWriteToOutput = outputWindow;

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.LastSelectedSolutionsUniqueName, _syncObject);

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSolutions.ItemsSource = _itemsSource;

            cmBFilter.DataContext = this._service.ConnectionData;

            cmBFilter.Focus();

            if (_service != null)
            {
                ShowExistingSolutions(this._service.ConnectionData.LastSelectedSolutionsUniqueName.FirstOrDefault());
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            BindingOperations.ClearAllBindings(cmBFilter);

            cmBFilter.Items.DetachFromSourceCollection();
            cmBFilter.DataContext = null;
            cmBFilter.ItemsSource = null;

            base.OnClosed(e);
        }

        private async Task ShowExistingSolutions(string lastSolutionUniqueName = null)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading solutions...");

            this._itemsSource.Clear();

            List<Solution> list = null;

            try
            {
                SolutionRepository repository = new SolutionRepository(this._service);

                if (!string.IsNullOrEmpty(lastSolutionUniqueName) && this._lastSolution == null)
                {
                    this._lastSolution = await repository.GetSolutionByUniqueNameAsync(lastSolutionUniqueName);

                    if (this._lastSolution.IsManaged.GetValueOrDefault() || !this._lastSolution.IsVisible.GetValueOrDefault())
                    {
                        this._lastSolution = null;
                    }

                    var name = this._lastSolution?.UniqueName;

                    var isEnabled = this._lastSolution != null;

                    var visibility = isEnabled ? Visibility.Visible : Visibility.Collapsed;

                    toolStrip.Dispatcher.Invoke(() =>
                    {
                        txtBLastLink.Text = name;

                        btnSelectLastSolution.IsEnabled = lblLastLink.IsEnabled = txtBLastLink.IsEnabled = sepLastLink.IsEnabled = isEnabled;
                        btnSelectLastSolution.Visibility = lblLastLink.Visibility = txtBLastLink.Visibility = sepLastLink.Visibility = visibility;

                        toolStrip.UpdateLayout();
                    });
                }

                string textName = string.Empty;

                cmBFilter.Dispatcher.Invoke(() =>
                {
                    textName = cmBFilter.Text?.Trim().ToLower();
                });

                list = await repository.FindSolutionsVisibleUnmanagedAsync(textName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                list = new List<Solution>();
            }

            this._iWriteToOutput.WriteToOutput("Found {0} solutions.", list.Count());

            LoadSolutions(list);

            UpdateStatus(string.Format("{0} solutions loaded.", list.Count()));

            ToggleControls(true);
        }

        private class EntityViewItem
        {
            public string SolutionName { get; private set; }

            public string DisplayName { get; private set; }

            public string SolutionType { get; private set; }

            public DateTime? InstalledOn { get; private set; }

            public Solution Solution { get; private set; }

            public EntityViewItem(string solutionName, string displayName, string solutionType, DateTime? installedOn, Solution Solution)
            {
                this.SolutionName = solutionName;
                this.DisplayName = displayName;
                this.SolutionType = solutionType;
                this.Solution = Solution;
                this.InstalledOn = installedOn;
            }
        }

        private void LoadSolutions(IEnumerable<Solution> results)
        {
            this.lstVwSolutions.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results)
                {
                    _itemsSource.Add(new EntityViewItem(entity.UniqueName, entity.FriendlyName, entity.FormattedValues[Solution.Schema.Attributes.ismanaged], entity.InstalledOn?.ToLocalTime(), entity));
                }

                if (_itemsSource.Count == 1)
                {
                    this.lstVwSolutions.SelectedItem = this.lstVwSolutions.Items[0];
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
            this.lstVwSolutions.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwSolutions.SelectedItems.Count > 0;

                    UIElement[] list = { btnSelectSolution };

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

        private void cmBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSolutions();
            }
        }

        private Solution GetSelectedEntity()
        {
            Solution result = null;

            if (this.lstVwSolutions.SelectedItems.Count == 1
                && this.lstVwSolutions.SelectedItems[0] != null
                && this.lstVwSolutions.SelectedItems[0] is EntityViewItem
                )
            {
                result = (this.lstVwSolutions.SelectedItems[0] as EntityViewItem).Solution;
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
                if (((FrameworkElement)e.OriginalSource).DataContext is EntityViewItem item && item.Solution != null)
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
            if (!_controlsEnabled)
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSolutions();
            }

            base.OnKeyDown(e);
        }

        private void btnSelectLastSolution_Click(object sender, RoutedEventArgs e)
        {
            if (this._lastSolution != null)
            {
                this.SelectedSolution = this._lastSolution;

                this._service.ConnectionData.AddLastSelectedSolution(this._lastSolution.UniqueName);

                this.DialogResult = true;
            }
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

        private void btnOpenComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenSolutionComponentDependenciesWindow(this._iWriteToOutput, _service, null, commonConfig, entity.UniqueName, null);
        }

        public void ShowForAllOther()
        {
            sepForAllOther.IsEnabled = chBForAllOther.IsEnabled = true;
            sepForAllOther.Visibility = chBForAllOther.Visibility = Visibility.Visible;
        }

        private void mICreateNewSolution_Click(object sender, RoutedEventArgs e)
        {
            this._service.ConnectionData.OpenSolutionCreateInWeb();
        }

        private async void ClearUnmanagedSolution_Click(object sender, RoutedEventArgs e)
        {
            var solution = GetSelectedEntity();

            if (solution == null)
            {
                return;
            }

            string question = string.Format("Are you sure want to clear solution {0}?", solution.UniqueName);

            if (MessageBox.Show(question, "Question", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                ToggleControls(false);

                UpdateStatus("Start clearing solution.");

                var descriptor = new SolutionComponentDescriptor(_iWriteToOutput, _service, true);

                SolutionDescriptor solutionDescriptor = new SolutionDescriptor(_iWriteToOutput, _service, descriptor);

                var commonConfig = CommonConfiguration.Get();

                this._iWriteToOutput.WriteToOutput(string.Empty);
                this._iWriteToOutput.WriteToOutput("Creating backup Solution Components in '{0}'.", solution.UniqueName);

                {
                    string fileName = EntityFileNameFormatter.GetSolutionFileName(
                        _service.ConnectionData.Name
                        , solution.UniqueName
                        , "Components Backup"
                    );

                    string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    await solutionDescriptor.CreateFileWithSolutionComponentsAsync(filePath, solution.Id);

                    this._iWriteToOutput.WriteToOutput("Created backup Solution Components in '{0}': {1}", solution.UniqueName, filePath);
                    this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                }

                SolutionComponentRepository repository = new SolutionComponentRepository(_service);
                await repository.ClearSolutionAsync(solution.UniqueName);

                UpdateStatus("Operation is completed.");
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus("Operation failed.");
            }
            finally
            {
                ToggleControls(true);
            }
        }
    }
}