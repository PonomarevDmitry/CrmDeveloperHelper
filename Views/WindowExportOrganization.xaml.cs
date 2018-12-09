using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportOrganization : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private Popup _optionsPopup;

        private int _init = 0;

        public WindowExportOrganization(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string connectionDataName
            )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.XmlFull);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            LoadFromConfig(commonConfig);

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwOrganizations.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (service != null)
            {
                ShowExistingOrganizations();
            }
        }

        private void LoadFromConfig(CommonConfiguration commonConfig)
        {
            cmBFileAction.DataContext = commonConfig;

            txtBFolder.DataContext = commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
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
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingOrganizations()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingOrganizations);

            this._itemsSource.Clear();

            IEnumerable<Organization> list = Enumerable.Empty<Organization>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new OrganizationRepository(service);
                    list = await repository.GetListAsync();
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadSavedOrganizations(list);
        }

        private static IEnumerable<Organization> FilterList(IEnumerable<Organization> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                list = list.Where(ent =>
                {
                    var name = ent.Name.ToLower();

                    return name.Contains(textName);
                });
            }

            return list;
        }

        private class EntityViewItem
        {
            public string Name { get; private set; }

            public string IsDisabled { get; private set; }

            public Organization Organization { get; private set; }

            public EntityViewItem(string name, string isDisabled, Organization organization)
            {
                this.Name = name;
                this.IsDisabled = isDisabled;
                this.Organization = organization;
            }
        }

        private void LoadSavedOrganizations(IEnumerable<Organization> results)
        {
            this.lstVwOrganizations.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results.OrderBy(ent => ent.Name))
                {
                    var item = new EntityViewItem(entity.Name, entity.FormattedValues[Organization.Schema.Attributes.isdisabled], entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwOrganizations.Items.Count == 1)
                {
                    this.lstVwOrganizations.SelectedItem = this.lstVwOrganizations.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingOrganizationsCompletedFormat1, results.Count());
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

            ToggleControl(cmBCurrentConnection, enabled);

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
            this.lstVwOrganizations.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwOrganizations.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportOrganization, btnExportAll };

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
                ShowExistingOrganizations();
            }
        }

        private Organization GetSelectedEntity()
        {
            return this.lstVwOrganizations.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwOrganizations.SelectedItems.OfType<EntityViewItem>().Select(e => e.Organization).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null)
                {
                    ExecuteAction(item.Organization, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Organization organization)
        {
            await PerformExportEntityDescription(folder, organization);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteAction(Organization organization, Func<string, Organization, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
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

            action(folder, organization);
        }

        private Task<string> CreateFileAsync(string folder, string name, string fieldTitle, string xmlContent)
        {
            return Task.Run(async () => await CreateFile(folder, name, fieldTitle, xmlContent));
        }

        private async Task<string> CreateFile(string folder, string name, string fieldTitle, string xmlContent)
        {
            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetOrganizationFileName(service.ConnectionData.Name, name, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (string.Equals(fieldTitle, Organization.Schema.Attributes.sitemapxml, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(fieldTitle, Organization.Schema.Attributes.referencesitemapxml, StringComparison.OrdinalIgnoreCase)
                        )
                    {
                        if (_commonConfig.SetXmlSchemasDuringExport)
                        {
                            var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaSiteMapXml);

                            if (schemasResources != null)
                            {
                                xmlContent = ContentCoparerHelper.SetXsdSchema(xmlContent, schemasResources);
                            }
                        }
                    }

                    xmlContent = ContentCoparerHelper.FormatXml(xmlContent, _commonConfig.ExportXmlAttributeOnNewLine);

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Organization.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Organization.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            return filePath;
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Organization organization)
        {
            await PerformExportEntityDescription(folder, organization);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.defaultemailsettings, "DefaultEmailSettings");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.externalpartycorrelationkeys, "ExternalPartyCorrelationKeys");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.externalpartyentitysettings, "ExternalPartyEntitySettings");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.featureset, "FeatureSet");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.kmsettings, "KMSettings");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.referencesitemapxml, "ReferenceSiteMapXml");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.sitemapxml, "SiteMapXml");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.defaultthemedata, "DefaultThemeData");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.highcontrastthemedata, "HighContrastThemeData");

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.slapausestates, "SlaPauseStates");
        }

        private void ExecuteActionEntity(Organization organization, string fieldName, string fieldTitle, Func<string, Organization, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
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

            action(folder, organization, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Organization organization, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                string xmlContent = organization.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, organization.Name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Organization organization, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                string xmlContent = organization.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, organization.Name, fieldTitle + " BackUp", xmlContent);

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
                    ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                newText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

                if (ContentCoparerHelper.TryParseXml(newText, out var doc))
                {
                    newText = doc.ToString(SaveOptions.DisableFormatting);
                }

                var updateEntity = new Organization
                {
                    Id = organization.Id
                };
                updateEntity.Attributes[fieldName] = newText;
                await service.UpdateAsync(updateEntity);

                organization.Attributes[fieldName] = newText;

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformExportEntityDescription);
        }

        private async Task PerformExportEntityDescription(string folder, Organization organization)
        {
            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                var service = await GetService();

                string fileName = EntityFileNameFormatter.GetOrganizationFileName(service.ConnectionData.Name, organization.Name, "EntityDescription", "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, organization, EntityFileNameFormatter.OrganizationIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , organization.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private void mIExportOrganizationDefaultEmailSettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.defaultemailsettings, "DefaultEmailSettings", PerformExportXmlToFile);
        }

        private void mIExportOrganizationExternalPartyCorrelationKeys_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.externalpartycorrelationkeys, "ExternalPartyCorrelationKeys", PerformExportXmlToFile);
        }

        private void mIExportOrganizationExternalPartyEntitySettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.externalpartyentitysettings, "ExternalPartyEntitySettings", PerformExportXmlToFile);
        }

        private void mIExportOrganizationFeatureSet_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.featureset, "FeatureSet", PerformExportXmlToFile);
        }

        private void mIExportOrganizationKMSettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.kmsettings, "KMSettings", PerformExportXmlToFile);
        }

        private void mIExportOrganizationReferenceSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.referencesitemapxml, "ReferenceSiteMapXml", PerformExportXmlToFile);
        }

        private void mIExportOrganizationSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.sitemapxml, "SiteMapXml", PerformExportXmlToFile);
        }

        private void mIExportOrganizationDefaultThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.defaultthemedata, "DefaultThemeData", PerformExportXmlToFile);
        }

        private void mIExportOrganizationHighContrastThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.highcontrastthemedata, "HighContrastThemeData", PerformExportXmlToFile);
        }

        private void mIExportOrganizationSlaPauseStates_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.slapausestates, "SlaPauseStates", PerformExportXmlToFile);
        }

        private void mIExportOrganizationShowDifferenceSiteMapXmlAndReferenceSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteDifference(
                entity
                , Organization.Schema.Attributes.sitemapxml, "SiteMapXml"
                , Organization.Schema.Attributes.referencesitemapxml, "ReferenceSiteMapXml"
                , PerformShowingDirfference
                );
        }

        private void mIExportOrganizationShowDifferenceDefaultThemeDataAndHighContrastThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteDifference(
                entity
                , Organization.Schema.Attributes.defaultthemedata, "DefaultThemeData"
                , Organization.Schema.Attributes.highcontrastthemedata, "HighContrastThemeData"
                , PerformShowingDirfference
                );
        }

        private void ExecuteDifference(
            Organization organization
            , string fieldName1
            , string fieldTitle1
            , string fieldName2
            , string fieldTitle2
            , Func<string, Organization, string, string, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
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

            action(folder, organization, fieldName1, fieldTitle1, fieldName2, fieldTitle2);
        }

        private async Task PerformShowingDirfference(
            string folder
            , Organization organization
            , string fieldName1, string fieldTitle1
            , string fieldName2, string fieldTitle2
            )
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceForFieldsFormat2, fieldTitle1, fieldTitle2);

            string xmlContent1 = organization.GetAttributeValue<string>(fieldName1);
            string filePath1 = await CreateFileAsync(folder, organization.Name, fieldTitle1, xmlContent1);

            string xmlContent2 = organization.GetAttributeValue<string>(fieldName2);
            string filePath2 = await CreateFileAsync(folder, organization.Name, fieldTitle2, xmlContent2);

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(filePath1);

                this._iWriteToOutput.PerformAction(filePath2);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceForFieldsCompletedFormat2, fieldTitle1, fieldTitle2);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingOrganizations();
            }

            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    if (_optionsPopup.IsOpen)
                    {
                        _optionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                }
            }

            base.OnKeyDown(e);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ShowExistingOrganizations();
            }
        }

        private void mIUpdateOrganizationDefaultEmailSettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.defaultemailsettings, "DefaultEmailSettings", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationExternalPartyCorrelationKeys_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.externalpartycorrelationkeys, "ExternalPartyCorrelationKeys", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationExternalPartyEntitySettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.externalpartyentitysettings, "ExternalPartyEntitySettings", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationFeatureSet_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.featureset, "FeatureSet", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationKMSettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.kmsettings, "KMSettings", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationReferenceSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.referencesitemapxml, "ReferenceSiteMapXml", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.sitemapxml, "SiteMapXml", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationDefaultThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.defaultthemedata, "DefaultThemeData", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationHighContrastThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.highcontrastthemedata, "HighContrastThemeData", PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationSlaPauseStates_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.slapausestates, "SlaPauseStates", PerformUpdateEntityField);
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
        {
            this._optionsPopup.IsOpen = true;
            this._optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
                this.Focus();
            }
        }
    }
}