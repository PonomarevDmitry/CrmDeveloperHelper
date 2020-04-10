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
    public partial class WindowExplorerOrganization : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly Popup _optionsPopup;

        public WindowExplorerOrganization(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.OrganizationXmlOptions | XmlOptionsControls.SiteMapXmlOptions);
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

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

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

            if (connectionData == null)
            {
                return null;
            }

            if (_connectionCache.ContainsKey(connectionData.ConnectionId))
            {
                return _connectionCache[connectionData.ConnectionId];
            }

            ToggleControls(connectionData, false, string.Empty);

            try
            {
                var service = await QuickConnection.ConnectAndWriteToOutputAsync(_iWriteToOutput, connectionData);

                if (service != null)
                {
                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return service;
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                ToggleControls(connectionData, true, string.Empty);
            }

            return null;
        }

        private async Task ShowExistingOrganizations()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingOrganizations);

            this._itemsSource.Clear();

            IEnumerable<Organization> list = Enumerable.Empty<Organization>();

            try
            {
                if (service != null)
                {
                    var repository = new OrganizationRepository(service);
                    list = await repository.GetListAsync();
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadSavedOrganizations(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingOrganizationsCompletedFormat1, list.Count());
        }

        private static IEnumerable<Organization> FilterList(IEnumerable<Organization> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                list = list.Where(ent =>
                {
                    return ent.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1;
                });
            }

            return list;
        }

        private class EntityViewItem
        {
            public string Name => Organization.Name;

            public string IsDisabled { get; }

            public Organization Organization { get; private set; }

            public EntityViewItem(Organization organization)
            {
                organization.FormattedValues.TryGetValue(Organization.Schema.Attributes.isdisabled, out var isDisabled);

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
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwOrganizations.Items.Count == 1)
                {
                    this.lstVwOrganizations.SelectedItem = this.lstVwOrganizations.Items[0];
                }
            });
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
            this.lstVwOrganizations.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwOrganizations.SelectedItems.Count > 0;

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

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
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
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

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

            action(folder, organization);
        }

        private Task<string> CreateFileAsync(string folder, string name, string fieldTitle, string siteMapXml)
        {
            return Task.Run(async () => await CreateFile(folder, name, fieldTitle, siteMapXml));
        }

        private async Task<string> CreateFile(string folder, string name, string fieldTitle, string siteMapXml)
        {
            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetOrganizationFileName(service.ConnectionData.Name, name, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(siteMapXml))
            {
                try
                {
                    string schemaName = string.Empty;
                    var xmlOptions = XmlOptionsControls.OrganizationXmlOptions;

                    if (string.Equals(fieldTitle, Organization.Schema.Attributes.sitemapxml, StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(fieldTitle, Organization.Schema.Attributes.referencesitemapxml, StringComparison.InvariantCultureIgnoreCase)
                        )
                    {
                        schemaName = AbstractDynamicCommandXsdSchemas.SchemaSiteMapXml;
                        xmlOptions = XmlOptionsControls.SiteMapXmlOptions;
                    }

                    siteMapXml = ContentComparerHelper.FormatXmlByConfiguration(
                        siteMapXml
                        , _commonConfig
                        , xmlOptions
                        , schemaName: schemaName
                    );

                    File.WriteAllText(filePath, siteMapXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Organization.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Organization.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
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

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.defaultemailsettings, Organization.Schema.Headers.defaultemailsettings);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.externalpartycorrelationkeys, Organization.Schema.Headers.externalpartycorrelationkeys);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.externalpartyentitysettings, Organization.Schema.Headers.externalpartyentitysettings);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.featureset, Organization.Schema.Headers.featureset);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.kmsettings, Organization.Schema.Headers.kmsettings);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.referencesitemapxml, Organization.Schema.Headers.referencesitemapxml);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.sitemapxml, Organization.Schema.Headers.sitemapxml);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.defaultthemedata, Organization.Schema.Headers.defaultthemedata);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.highcontrastthemedata, Organization.Schema.Headers.highcontrastthemedata);

            await PerformExportXmlToFile(folder, organization, Organization.Schema.Attributes.slapausestates, Organization.Schema.Headers.slapausestates);
        }

        private void ExecuteActionEntity(Organization organization, string fieldName, string fieldTitle, Func<string, Organization, string, string, Task> action)
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

            action(folder, organization, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Organization organization, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                string xmlContent = organization.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, organization.Name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Organization organization, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

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
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

                if (ContentComparerHelper.TryParseXml(newText, out var doc))
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

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
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

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity, PerformEntityEditor);
        }

        private async Task PerformEntityEditor(string folder, Organization organization)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, Organization.EntityLogicalName, organization.Id);
        }

        private async Task PerformExportEntityDescription(string folder, Organization organization)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetOrganizationFileName(service.ConnectionData.Name, organization.Name, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, organization, EntityFileNameFormatter.OrganizationIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , organization.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionFailed);
            }
        }

        private void mIExportOrganizationDefaultEmailSettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.defaultemailsettings, Organization.Schema.Headers.defaultemailsettings, PerformExportXmlToFile);
        }

        private void mIExportOrganizationExternalPartyCorrelationKeys_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.externalpartycorrelationkeys, Organization.Schema.Headers.externalpartycorrelationkeys, PerformExportXmlToFile);
        }

        private void mIExportOrganizationExternalPartyEntitySettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.externalpartyentitysettings, Organization.Schema.Headers.externalpartyentitysettings, PerformExportXmlToFile);
        }

        private void mIExportOrganizationFeatureSet_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.featureset, Organization.Schema.Headers.featureset, PerformExportXmlToFile);
        }

        private void mIExportOrganizationKMSettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.kmsettings, Organization.Schema.Headers.kmsettings, PerformExportXmlToFile);
        }

        private void mIExportOrganizationReferenceSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.referencesitemapxml, Organization.Schema.Headers.referencesitemapxml, PerformExportXmlToFile);
        }

        private void mIExportOrganizationSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.sitemapxml, Organization.Schema.Headers.sitemapxml, PerformExportXmlToFile);
        }

        private void mIExportOrganizationDefaultThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.defaultthemedata, Organization.Schema.Headers.defaultthemedata, PerformExportXmlToFile);
        }

        private void mIExportOrganizationHighContrastThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.highcontrastthemedata, Organization.Schema.Headers.highcontrastthemedata, PerformExportXmlToFile);
        }

        private void mIExportOrganizationSlaPauseStates_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.slapausestates, Organization.Schema.Headers.slapausestates, PerformExportXmlToFile);
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
                , Organization.Schema.Attributes.sitemapxml, Organization.Schema.Headers.sitemapxml
                , Organization.Schema.Attributes.referencesitemapxml, Organization.Schema.Headers.referencesitemapxml
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
                , Organization.Schema.Attributes.defaultthemedata, Organization.Schema.Headers.defaultthemedata
                , Organization.Schema.Attributes.highcontrastthemedata, Organization.Schema.Headers.highcontrastthemedata
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

            action(folder, organization, fieldName1, fieldTitle1, fieldName2, fieldTitle2);
        }

        private async Task PerformShowingDirfference(
            string folder
            , Organization organization
            , string fieldName1, string fieldTitle1
            , string fieldName2, string fieldTitle2
            )
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ShowingDifferenceForFieldsFormat2, fieldTitle1, fieldTitle2);

            string xmlContent1 = organization.GetAttributeValue<string>(fieldName1);
            string filePath1 = await CreateFileAsync(folder, organization.Name, fieldTitle1, xmlContent1);

            string xmlContent2 = organization.GetAttributeValue<string>(fieldName2);
            string filePath2 = await CreateFileAsync(folder, organization.Name, fieldTitle2, xmlContent2);

            if (File.Exists(filePath1) && File.Exists(filePath2))
            {
                this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
            }
            else
            {
                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath1);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath2);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ShowingDifferenceForFieldsCompletedFormat2, fieldTitle1, fieldTitle2);
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingOrganizations();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            Popup[] _popupArray = new Popup[] { _optionsPopup };

            foreach (var popup in _popupArray)
            {
                if (popup.IsOpen)
                {
                    popup.IsOpen = false;
                    e.Handled = true;

                    return false;
                }
            }

            return true;
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (!this.IsControlsEnabled)
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

            ExecuteActionEntity(entity, Organization.Schema.Attributes.defaultemailsettings, Organization.Schema.Headers.defaultemailsettings, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationExternalPartyCorrelationKeys_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.externalpartycorrelationkeys, Organization.Schema.Headers.externalpartycorrelationkeys, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationExternalPartyEntitySettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.externalpartyentitysettings, Organization.Schema.Headers.externalpartyentitysettings, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationFeatureSet_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.featureset, Organization.Schema.Headers.featureset, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationKMSettings_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.kmsettings, Organization.Schema.Headers.kmsettings, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationReferenceSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.referencesitemapxml, Organization.Schema.Headers.referencesitemapxml, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.sitemapxml, Organization.Schema.Headers.sitemapxml, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationDefaultThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.defaultthemedata, Organization.Schema.Headers.defaultthemedata, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationHighContrastThemeData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.highcontrastthemedata, Organization.Schema.Headers.highcontrastthemedata, PerformUpdateEntityField);
        }

        private void mIUpdateOrganizationSlaPauseStates_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity, Organization.Schema.Attributes.slapausestates, Organization.Schema.Headers.slapausestates, PerformUpdateEntityField);
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

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }
    }
}