using Microsoft.Xrm.Sdk;
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
    public partial class WindowStatusSelect : WindowBase
    {
        protected readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        protected readonly string _entityName;

        protected readonly IOrganizationServiceExtented _service;

        private EntityMetadata _entityMetadata;

        private readonly List<StatusCodeViewItem> _sourceStatusCodes = new List<StatusCodeViewItem>();

        private readonly ObservableCollection<StatusCodeViewItem> _itemsSource = new ObservableCollection<StatusCodeViewItem>();

        public StatusOptionMetadata SelectedStatusOptionMetadata { get; private set; }

        public WindowStatusSelect(
            IWriteToOutput outputWindow
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityName
        )
        {
            IncreaseInit();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this.Name = string.Format("WindowStatusSelect_{0}", entityName);
            lstVwStatusCodes.Name = string.Format("lstVwStatusCodes{0}", entityName);

            this._iWriteToOutput = outputWindow;
            this._service = service;
            this._commonConfig = commonConfig;
            this._entityName = entityName;

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            lstVwStatusCodes.ItemsSource = _itemsSource;

            DecreaseInit();

            txtBFilter.SelectionStart = txtBFilter.Text.Length;
            txtBFilter.SelectionLength = 0;
            txtBFilter.Focus();

            RetrieveEntityInformation();
        }

        private async Task RetrieveEntityInformation()
        {
            try
            {
                ToggleControls(false, Properties.WindowStatusStrings.GettingEntityMetadataFormat1, _entityName);

                var repositoryEntityMetadata = new EntityMetadataRepository(_service);

                this._entityMetadata = await repositoryEntityMetadata.GetEntityMetadataWithAttributesAsync(_entityName);

                ToggleControls(true, Properties.WindowStatusStrings.GettingEntityMetadataCompletedFormat1, _entityName);

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
            ToggleControls(false, Properties.WindowStatusStrings.FilteringStatusCodesFormat1, _entityName);

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
                                .Any(lbl => lbl.Label.ToLower().Contains(textName))
                        )
                        || (
                            ent.StatusCodeLabel != null
                            && ent.StatusCodeLabel.LocalizedLabels != null
                            && ent.StatusCodeLabel.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.ToLower().Contains(textName))
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

            ToggleControls(true, Properties.WindowStatusStrings.FilteringStatusCodesCompletedFormat1, _entityName);
        }

        private class StatusCodeViewItem
        {
            public int StateCode { get; private set; }

            public string StateCodeName { get; private set; }

            public int StatusCode { get; private set; }

            public string StatusCodeName { get; private set; }

            public Microsoft.Xrm.Sdk.Label StateCodeLabel { get; private set; }

            public Microsoft.Xrm.Sdk.Label StatusCodeLabel { get; private set; }

            public StatusOptionMetadata StatusOptionMetadata { get; private set; }

            public StatusCodeViewItem(
                int stateCode
                , string stateCodeName
                , int statusCode
                , string statusCodeName
                , Microsoft.Xrm.Sdk.Label stateCodeLabel
                , Microsoft.Xrm.Sdk.Label statusCodeLabel
                , StatusOptionMetadata statusOptionMetadata
            )
            {
                this.StateCode = stateCode;
                this.StateCodeName = stateCodeName;

                this.StatusCode = statusCode;
                this.StatusCodeName = statusCodeName;

                this.StateCodeLabel = stateCodeLabel;
                this.StatusCodeLabel = statusCodeLabel;

                this.StatusOptionMetadata = statusOptionMetadata;
            }
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
                if (((FrameworkElement)e.OriginalSource).DataContext is StatusCodeViewItem item)
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

            this.SelectedStatusOptionMetadata = statusItem.StatusOptionMetadata;

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                FilterStatusCodes();
            }

            base.OnKeyDown(e);
        }

        private void mIOpenEntityInstanceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityMetadataInWeb(_entityName);
        }

        private void mIOpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityInstanceListInWeb(_entityName);
        }

        #region Кнопки открытия других форм с информация о сущности.

        private void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, _service, _commonConfig, _entityName);
        }

        private void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, _service, _commonConfig, _entityName);
        }

        private void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, _service, _commonConfig, _entityName);
        }

        private void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, _service, _commonConfig, _entityName);
        }

        private void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, _service, _commonConfig, _entityName);
        }

        private void miEntitySecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, _service, _commonConfig);
        }

        private void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , _service
                , _commonConfig
                , null
                , string.Empty
                , string.Empty
                );
        }

        private void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, _service, _commonConfig, null, _entityName);
        }

        private void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, _service, _commonConfig, null, _entityName);
        }

        private void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, _service, _commonConfig, null, _entityName);
        }

        private void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, _service, _commonConfig, null, _entityName);
        }

        private void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, _service, _commonConfig, _entityName, string.Empty, string.Empty);
        }

        private void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, _service, _commonConfig, _entityName, string.Empty);
        }

        private void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, _service, _commonConfig, _entityName, string.Empty);
        }

        private void btnSiteMap_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenExportSiteMapWindow(this._iWriteToOutput, _service, _commonConfig);
        }

        private void btnWebResources_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenExportWebResourcesWindow(this._iWriteToOutput, _service, _commonConfig, string.Empty);
        }

        private void btnExportReport_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenExportReportWindow(this._iWriteToOutput, _service, _commonConfig, string.Empty);
        }

        private void btnPluginAssembly_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenPluginAssemblyWindow(this._iWriteToOutput, _service, _commonConfig, null);
        }

        private void btnPluginType_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            WindowHelper.OpenPluginTypeWindow(this._iWriteToOutput, _service, _commonConfig, null);
        }

        #endregion Кнопки открытия других форм с информация о сущности.
    }
}