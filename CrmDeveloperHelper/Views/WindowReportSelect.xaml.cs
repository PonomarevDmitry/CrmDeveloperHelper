using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowReportSelect : WindowBase
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

        private Guid? _lastLinkedReport;

        /// <summary>
        /// ИД залинкованного веб-ресурса
        /// </summary>
        public Guid? SelectedReportId { get; private set; }

        private ObservableCollection<EntityViewItem> _itemsSource;

        private bool _controlsEnabled = true;

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
            if (!_controlsEnabled)
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
                    var report = await repository.GetByIdAsync(this._lastLinkedReport.Value, new ColumnSet(Report.Schema.Attributes.name, Report.Schema.Attributes.filename, Report.Schema.Attributes.reporttypecode));

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
                this._iWriteToOutput.WriteErrorToOutput(ex);

                list = new List<Report>();
            }

            LoadReports(list);
        }

        private class EntityViewItem
        {
            public string ReportName { get; private set; }

            public string FileName { get; private set; }

            public string ReportType { get; private set; }

            public string Owner { get; private set; }

            public string ViewableBy { get; private set; }

            public Report Report { get; private set; }

            public EntityViewItem(string name, string filename, string reportType, string owner, string ispersonal, Report report)
            {
                this.ReportName = name;
                this.FileName = filename;
                this.ReportType = reportType;
                this.Owner = owner;
                this.ViewableBy = ispersonal;
                this.Report = report;
            }
        }

        private void LoadReports(IEnumerable<Report> results)
        {
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                foreach (var report in results)
                {
                    string reportType = report.FormattedValues[Report.Schema.Attributes.reporttypecode];

                    string owner = string.Empty;
                    if (report.OwnerId != null)
                    {
                        owner = report.OwnerId.Name;
                    }

                    string ispersonal = report.FormattedValues[Report.Schema.Attributes.ispersonal];

                    var item = new EntityViewItem(report.Name, report.FileName, reportType, owner, ispersonal, report);

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

            ToggleControl(this.btnSelectReport, enabled);
            ToggleControl(this.btnSelectLastLink, enabled);

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
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwReports.SelectedItems.Count > 0;

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
