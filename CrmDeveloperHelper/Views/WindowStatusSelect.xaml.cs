using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
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
    public partial class WindowStatusSelect : WindowWithOutputAndCommonConfig
    {
        protected readonly string _entityName;

        protected readonly IOrganizationServiceExtented _service;

        private EntityMetadata _entityMetadata;

        private readonly List<StatusCodeViewItem> _sourceStatusCodes = new List<StatusCodeViewItem>();

        private readonly ObservableCollection<StatusCodeViewItem> _itemsSource = new ObservableCollection<StatusCodeViewItem>();

        public StatusCodeViewItem SelectedStatusCodeViewItem { get; private set; }

        public WindowStatusSelect(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string entityName
        ) : base(iWriteToOutput, commonConfig)
        {
            IncreaseInit();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this.Name = string.Format("WindowStatusSelect_{0}", entityName);
            lstVwStatusCodes.Name = string.Format("lstVwStatusCodes{0}", entityName);

            this._service = service;
            this._entityName = entityName;

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            lstVwStatusCodes.ItemsSource = _itemsSource;

            FillExplorersMenuItems();

            DecreaseInit();

            txtBFilter.SelectionStart = txtBFilter.Text.Length;
            txtBFilter.SelectionLength = 0;
            txtBFilter.Focus();

            RetrieveEntityInformation();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, () => Task.FromResult(_service)
                , getEntityName: () => this._entityName
            );

            explorersHelper.FillExplorers(miExplorers);
        }

        private async Task RetrieveEntityInformation()
        {
            try
            {
                ToggleControls(false, Properties.OutputStrings.GettingEntityMetadataFormat1, _entityName);

                var repositoryEntityMetadata = new EntityMetadataRepository(_service);

                this._entityMetadata = await repositoryEntityMetadata.GetEntityMetadataWithAttributesAsync(_entityName);

                ToggleControls(true, Properties.OutputStrings.GettingEntityMetadataCompletedFormat1, _entityName);

                if (this._entityMetadata != null
                    && this._entityMetadata.Attributes != null
                )
                {
                    var stateCodeAttributeMetadata = this._entityMetadata.Attributes.OfType<StateAttributeMetadata>().FirstOrDefault();
                    var statusCodeAttributeMetadata = this._entityMetadata.Attributes.OfType<StatusAttributeMetadata>().FirstOrDefault();

                    if (stateCodeAttributeMetadata != null
                        && stateCodeAttributeMetadata.OptionSet != null
                        && stateCodeAttributeMetadata.OptionSet.Options != null
                        && statusCodeAttributeMetadata != null
                        && statusCodeAttributeMetadata.OptionSet != null
                        && statusCodeAttributeMetadata.OptionSet.Options != null
                    )
                    {
                        foreach (var statusCodeOption in statusCodeAttributeMetadata
                            .OptionSet
                            .Options
                            .OfType<StatusOptionMetadata>()
                            .OrderBy(o => o.State)
                            .ThenBy(o => o.Value)
                        )
                        {
                            var stateCodeOption = stateCodeAttributeMetadata.OptionSet.Options.OfType<StateOptionMetadata>().FirstOrDefault(o => o.Value == statusCodeOption.State);

                            if (stateCodeOption != null)
                            {
                                string stateName = CreateFileHandler.GetLocalizedLabel(stateCodeOption.Label);
                                string statusName = CreateFileHandler.GetLocalizedLabel(statusCodeOption.Label);

                                _sourceStatusCodes.Add(new StatusCodeViewItem(
                                    stateCodeOption.Value.Value
                                    , stateName

                                    , statusCodeOption.Value.Value
                                    , statusName

                                    , stateCodeOption.Label
                                    , statusCodeOption.Label

                                    , statusCodeOption
                                ));
                            }
                        }
                    }
                }

                FilterStatusCodes();
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void FilterStatusCodes()
        {
            ToggleControls(false, Properties.OutputStrings.FilteringStatusCodesFormat1, _entityName);

            this.lstVwStatusCodes.Dispatcher.Invoke(() =>
            {
                _itemsSource.Clear();
            });

            string textName = string.Empty;

            this.txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim();
            });

            var list = _sourceStatusCodes.AsEnumerable();

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (int.TryParse(textName, out int tempInt))
                {
                    list = list.Where(ent => ent.StatusCode.ToString().StartsWith(textName));
                }
                else
                {
                    list = list
                    .Where(ent =>
                        (
                            ent.StateCodeLabel != null
                            && ent.StateCodeLabel.LocalizedLabels != null
                            && ent.StateCodeLabel.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )
                        || (
                            ent.StatusCodeLabel != null
                            && ent.StatusCodeLabel.LocalizedLabels != null
                            && ent.StatusCodeLabel.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )
                    );
                }
            }

            this.lstVwStatusCodes.Dispatcher.Invoke(() =>
            {
                foreach (var item in list.OrderBy(o => o.StateCode).ThenBy(o => o.StatusCode))
                {
                    _itemsSource.Add(item);
                }
            });

            ToggleControls(true, Properties.OutputStrings.FilteringStatusCodesCompletedFormat1, _entityName);
        }

        protected void UpdateStatus(string format, params object[] args)
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

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar
                , this.btnSelectStatus
            );

            UpdateButtonsEnable();
        }

        protected void UpdateButtonsEnable()
        {
            this.lstVwStatusCodes.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwStatusCodes.SelectedItems.Count > 0;

                    UIElement[] list = { btnSelectStatus };

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

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterStatusCodes();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwStatusCodes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                StatusCodeViewItem item = GetItemFromRoutedDataContext<StatusCodeViewItem>(e);

                if (item != null)
                {
                    SelectStatusCodeAction(item);
                }
            }
        }

        private void lstVwStatusCodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void SelectStatusCodeAction(StatusCodeViewItem statusItem)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (statusItem == null
                || statusItem.StatusOptionMetadata == null
            )
            {
                return;
            }

            this.SelectedStatusCodeViewItem = statusItem;

            this.DialogResult = true;

            this.Close();
        }

        private StatusCodeViewItem GetSelectedStatusCode()
        {
            return this.lstVwStatusCodes.SelectedItems.OfType<StatusCodeViewItem>().Count() == 1
                ? this.lstVwStatusCodes.SelectedItems.OfType<StatusCodeViewItem>().SingleOrDefault() : null;
        }

        private void btnSelectStatus_Click(object sender, RoutedEventArgs e)
        {
            var statusItem = GetSelectedStatusCode();

            if (statusItem == null)
            {
                return;
            }

            SelectStatusCodeAction(statusItem);
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            FilterStatusCodes();
        }

        private void mIOpenEntityInstanceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityMetadataInWeb(_entityName);
        }

        private void mIOpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityInstanceListInWeb(_entityName);
        }
    }
}