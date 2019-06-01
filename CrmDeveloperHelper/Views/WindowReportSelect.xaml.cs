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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowReportSelect : WindowBase
    {
        private readonly IWriteToOutput _iWriteToOutput;

        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        private string _fileExtension;

        /// <summary>
        /// Выбранный файл
        /// </summary>
        private SelectedFile _file;

        private Guid? _lastLinkedReport;

        /// <summary>
        /// ИД залинкованного веб-ресурса
        /// </summary>
        public Guid? SelectedReportId { get; private set; }

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        public WindowReportSelect(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SelectedFile selectedFile
            , Guid? lastLinkedReport
        )
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._lastLinkedReport = lastLinkedReport;
            this._service = service;
            this._file = selectedFile;
            this._fileExtension = selectedFile.Extension;

            this.tSSLblConnectionName.Content = _service.ConnectionData.Name;

            txtBCurrentFile.Text = selectedFile.FriendlyFilePath;

            btnSelectLastLink.IsEnabled = false;

            btnSelectLastLink.Visibility = lblLastLink.Visibility = txtBLastLink.Visibility = Visibility.Collapsed;

            txtBFilter.Text = selectedFile.Name;

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwReports.ItemsSource = _itemsSource;

            if (_service != null)
            {
                ShowExistingReports();
            }
        }

        private async Task ShowExistingReports()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingReports);

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            List<Report> list = null;

            try
            {
                var repository = new ReportRepository(this._service);

                if (this._lastLinkedReport.HasValue && string.IsNullOrEmpty(txtBLastLink.Text))
                {
                    var report = await repository.GetByIdAsync(this._lastLinkedReport.Value
                        , new ColumnSet(
                            Report.Schema.Attributes.name
                            , Report.Schema.Attributes.ispersonal
                            , Report.Schema.Attributes.ownerid
                            , Report.Schema.Attributes.reporttypecode
                            , Report.Schema.Attributes.filename
                            , Report.Schema.Attributes.iscustomizable
                            , Report.Schema.Attributes.description
                        ));

                    if (report != null)
                    {
                        string tempName = string.Format("{0} - {1} - {2}", report.FileName, report.Name, report.FormattedValues[Report.Schema.Attributes.reporttypecode]);

                        txtBLastLink.Dispatcher.Invoke(() =>
                        {
                            txtBLastLink.Text = tempName;
                        });

                        this.Dispatcher.Invoke(() =>
                        {
                            btnSelectLastLink.IsEnabled = lblLastLink.IsEnabled = txtBLastLink.IsEnabled = true;
                            btnSelectLastLink.Visibility = lblLastLink.Visibility = txtBLastLink.Visibility = Visibility.Visible;
                        });
                    }
                }

                list = await repository.GetListAsync(textName, new ColumnSet(true));
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);

                list = new List<Report>();
            }

            LoadReports(list);
        }

        private class EntityViewItem
        {
            public string Name => Report.Name;

            public string FileName => Report.FileName;

            public string ReportTypeCode { get; }

            public string Owner => Report.OwnerId?.Name;

            public string IsPersonal { get; }

            public string Description => Report.Description;

            public bool HasDescription => !string.IsNullOrEmpty(Report.Description);

            public Report Report { get; }

            public EntityViewItem(Report report)
            {
                report.FormattedValues.TryGetValue(Report.Schema.Attributes.reporttypecode, out var reporttypecode);
                report.FormattedValues.TryGetValue(Report.Schema.Attributes.ispersonal, out var ispersonal);

                this.ReportTypeCode = reporttypecode;
                this.IsPersonal = ispersonal;

                this.Report = report;
            }
        }

        private void LoadReports(IEnumerable<Report> results)
        {
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                foreach (var report in results)
                {
                    var item = new EntityViewItem(report);

                    _itemsSource.Add(item);
                }

                if (this.lstVwReports.Items.Count == 1)
                {
                    this.lstVwReports.SelectedItem = this.lstVwReports.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingReportsCompletedFormat1, results.Count());
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

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.btnSelectReport, this.btnSelectLastLink);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwReports.SelectedItems.Count > 0;

                    UIElement[] list = { btnSelectReport };

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
                ShowExistingReports();
            }
        }

        private Report GetSelectedEntity()
        {
            return this.lstVwReports.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwReports.SelectedItems.OfType<EntityViewItem>().Select(e => e.Report).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (((FrameworkElement)e.OriginalSource).DataContext is EntityViewItem item)
                {
                    this.SelectedReportId = item.Report.Id;

                    this.DialogResult = true;

                    this.Close();
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void btnSelectReport_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            this.SelectedReportId = entity.Id;

            this.DialogResult = true;

            this.Close();
        }

        private void btnSelectLastLink_Click(object sender, RoutedEventArgs e)
        {
            if (this._lastLinkedReport.HasValue)
            {
                this.SelectedReportId = this._lastLinkedReport.Value;

                this.DialogResult = true;

                this.Close();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingReports();
            }

            base.OnKeyDown(e);
        }

        private void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Report, entity.Id);
        }

        private void mICreateNewReport_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenReportCreateInWeb();
        }
    }
}