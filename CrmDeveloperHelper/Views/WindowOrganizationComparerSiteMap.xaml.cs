using Microsoft.Xrm.Sdk.Query;
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

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerSiteMap : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowOrganizationComparerSiteMap(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.SiteMapXmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            this.Resources["ConnectionName1"] = connection1.Name;
            this.Resources["ConnectionName2"] = connection2.Name;

            LoadFromConfig();

            if (!string.IsNullOrEmpty(filter))
            {
                txtBFilter.Text = filter;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSiteMaps.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingSiteMaps();
        }

        private void FillExplorersMenuItems()
        {
            //var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1
            //    , getReportName: GetReportName1
            //);

            //var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2
            //    , getReportName: GetReportName2
            //);

            //explorersHelper1.FillExplorers(miExplorers1);
            //explorersHelper2.FillExplorers(miExplorers2);

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2())
                , getSiteMapName: GetSiteMapName
            );
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu contextMenu
            )
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    //if (string.Equals(item.Uid, "miExplorers1", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    explorersHelper1.FillExplorers(item);
                    //}
                    //else if (string.Equals(item.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    explorersHelper2.FillExplorers(item);
                    //}
                    //else
                    if (string.Equals(item.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase))
                    {
                        compareWindowsHelper.FillCompareWindows(item);
                    }
                }
            }
        }

        private string GetSiteMapName()
        {
            var entity = GetSelectedEntity();

            return entity?.SiteMapName ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            BindingOperations.ClearAllBindings(cmBConnection1);
            cmBConnection1.Items.DetachFromSourceCollection();
            cmBConnection1.DataContext = null;
            cmBConnection1.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBConnection2);
            cmBConnection2.Items.DetachFromSourceCollection();
            cmBConnection2.DataContext = null;
            cmBConnection2.ItemsSource = null;
        }

        private ConnectionData GetConnection1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private ConnectionData GetConnection2()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService1()
        {
            return GetOrganizationService(GetConnection1());
        }

        private Task<IOrganizationServiceExtented> GetService2()
        {
            return GetOrganizationService(GetConnection2());
        }

        private async Task ShowExistingSiteMaps()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingSiteMaps);

            this._itemsSource.Clear();

            IEnumerable<LinkedEntities<SiteMap>> list = Enumerable.Empty<LinkedEntities<SiteMap>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var columnSet = new ColumnSet(SiteMap.EntityPrimaryIdAttribute, SiteMap.Schema.Attributes.sitemapname, SiteMap.Schema.Attributes.sitemapnameunique);

                    List<LinkedEntities<SiteMap>> temp = new List<LinkedEntities<SiteMap>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new SiteMapRepository(service1);
                        var repository2 = new SiteMapRepository(service2);

                        var task1 = repository1.GetListAsync(columnSet);
                        var task2 = repository2.GetListAsync(columnSet);

                        var list1 = await task1;
                        var list2 = await task2;

                        foreach (var sitemap1 in list1)
                        {
                            var sitemap2 = list2.FirstOrDefault(c => string.Equals(c.SiteMapNameUnique ?? string.Empty, sitemap1.SiteMapNameUnique ?? string.Empty));

                            if (sitemap2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedEntities<SiteMap>(sitemap1, sitemap2));
                        }
                    }
                    else
                    {
                        var repository1 = new SiteMapRepository(service1);

                        var task1 = repository1.GetListAsync(columnSet);

                        var list1 = await task1;

                        foreach (var sitemap1 in list1)
                        {
                            temp.Add(new LinkedEntities<SiteMap>(sitemap1, null));
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            var textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadEntities(list);
        }

        private static IEnumerable<LinkedEntities<SiteMap>> FilterList(IEnumerable<LinkedEntities<SiteMap>> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent =>
                        ent.Entity1?.Id == tempGuid
                        || ent.Entity2?.Id == tempGuid
                        || ent.Entity1?.SiteMapIdUnique == tempGuid
                        || ent.Entity2?.SiteMapIdUnique == tempGuid
                    );
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var nameUnique1 = ent.Entity1?.SiteMapName ?? string.Empty;
                        var name1 = ent.Entity1?.SiteMapNameUnique ?? string.Empty;

                        var nameUnique2 = ent.Entity2?.SiteMapName ?? string.Empty;
                        var name2 = ent.Entity2?.SiteMapNameUnique ?? string.Empty;

                        return nameUnique1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || nameUnique2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            ;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string SiteMapName { get; private set; }

            public string SiteMapNameUnique { get; private set; }

            public Guid? SiteMap1Id { get; private set; }

            public Guid? SiteMap2Id { get; private set; }

            public LinkedEntities<SiteMap> Link { get; private set; }

            public EntityViewItem(string sitemapName, string siteMapNameUnique, Guid? id1, Guid? id2, LinkedEntities<SiteMap> link)
            {
                this.SiteMapName = sitemapName;
                this.SiteMapNameUnique = siteMapNameUnique;

                this.SiteMap1Id = id1;
                this.SiteMap2Id = id2;

                this.Link = link;
            }
        }

        private void LoadEntities(IEnumerable<LinkedEntities<SiteMap>> results)
        {
            this.lstVwSiteMaps.Dispatcher.Invoke(() =>
            {
                foreach (var link in results
                      .OrderBy(ent => ent.Entity1.SiteMapName)
                      .ThenBy(ent => ent.Entity1.SiteMapNameUnique)
                      .ThenBy(ent => ent.Entity1.Id)
                      .ThenBy(ent => ent.Entity2?.Id)
                  )
                {
                    var item = new EntityViewItem(link.Entity1.SiteMapName, link.Entity1.SiteMapNameUnique, link.Entity1.Id, link.Entity2?.Id, link);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSiteMaps.Items.Count == 1)
                {
                    this.lstVwSiteMaps.SelectedItem = this.lstVwSiteMaps.Items[0];
                }
            });

            ToggleControls(true, Properties.OutputStrings.LoadingSiteMapsCompletedFormat1, results.Count());
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(null, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            ToggleControls(enabled, statusFormat, args);
        }

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.cmBConnection1, this.cmBConnection2);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSiteMaps.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSiteMaps.SelectedItems.Count > 0;

                    var item = (this.lstVwSiteMaps.SelectedItems[0] as EntityViewItem);

                    tSDDBShowDifference.IsEnabled = enabled && item.Link.Entity1 != null && item.Link.Entity2 != null;
                    tSDDBConnection1.IsEnabled = enabled && item.Link.Entity1 != null;
                    tSDDBConnection2.IsEnabled = enabled && item.Link.Entity2 != null;
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
                await ShowExistingSiteMaps();
            }
        }

        private EntityViewItem GetSelectedEntity()
        {
            return this.lstVwSiteMaps.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwSiteMaps.SelectedItems.OfType<EntityViewItem>().SingleOrDefault() : null;
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
                    ExecuteAction(item.Link, false, PerformMouseDoubleClickAsync);
                }
            }
        }

        private async Task PerformMouseDoubleClickAsync(LinkedEntities<SiteMap> linked, bool showAllways)
        {
            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteAction(LinkedEntities<SiteMap> linked, bool showAllways, Func<LinkedEntities<SiteMap>, bool, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(linked, showAllways);
        }

        private Task<string> CreateFileAsync(ConnectionData connectionData, string name, string nameUnique, Guid id, string fieldTitle, string siteMapXml)
        {
            return Task.Run(() => CreateFile(connectionData, name, nameUnique, id, fieldTitle, siteMapXml));
        }

        private string CreateFile(ConnectionData connectionData, string name, string nameUnique, Guid id, string fieldTitle, string siteMapXml)
        {
            string fileName = EntityFileNameFormatter.GetSiteMapFileName(connectionData.Name, name, id, fieldTitle, "xml");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(siteMapXml))
            {
                try
                {
                    siteMapXml = ContentComparerHelper.FormatXmlByConfiguration(
                        siteMapXml
                        , _commonConfig
                        , XmlOptionsControls.SiteMapXmlOptions
                        , schemaName: AbstractDynamicCommandXsdSchemas.SchemaSiteMapXml
                        , siteMapUniqueName: nameUnique ?? string.Empty
                    );

                    File.WriteAllText(filePath, siteMapXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private Task<string> CreateDescriptionFileAsync(ConnectionData connectionData, string name, Guid id, string fieldTitle, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionData, name, id, fieldTitle, description));
        }

        private string CreateDescriptionFile(ConnectionData connectionData, string name, Guid id, string fieldTitle, string description)
        {
            string fileName = EntityFileNameFormatter.GetSiteMapFileName(connectionData.Name, name, id, fieldTitle, "txt");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SiteMap.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private void btnShowDifferenceAll_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteAction(link.Link, false, PerformShowingDifferenceAllAsync);
        }

        private async Task PerformShowingDifferenceAllAsync(LinkedEntities<SiteMap> linked, bool showAllways)
        {
            await PerformShowingDifferenceDescriptionAsync(linked, showAllways);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml);
        }

        private void mIShowDifferenceSiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml, PerformShowingDifferenceSingleXmlAsync);
        }

        private void ExecuteActionLinked(LinkedEntities<SiteMap> linked, bool showAllways, string fieldName, string fieldTitle, Func<LinkedEntities<SiteMap>, bool, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(linked, showAllways, fieldName, fieldTitle);
        }

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<SiteMap> linked, bool showAllways, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceXmlForFieldFormat1, fieldName);

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var repository1 = new SiteMapRepository(service1);
                    var repository2 = new SiteMapRepository(service2);

                    var sitemap1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var sitemap2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    string xml1 = sitemap1.GetAttributeValue<string>(fieldName);
                    string xml2 = sitemap2.GetAttributeValue<string>(fieldName);

                    if (showAllways || !ContentComparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        string filePath1 = await CreateFileAsync(service1.ConnectionData, sitemap1.SiteMapName, sitemap1.SiteMapNameUnique, sitemap1.Id, fieldTitle, xml1);

                        string filePath2 = await CreateFileAsync(service2.ConnectionData, sitemap2.SiteMapName, sitemap2.SiteMapNameUnique, sitemap2.Id, fieldTitle, xml2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceXmlForFieldCompletedFormat1, fieldName);

            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceXmlForFieldFailedFormat1, fieldName);
            }
        }

        private void ExecuteActionEntity(Guid idSiteMap, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idSiteMap, getService, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFileAsync(Guid idSiteMap, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new SiteMapRepository(service);

                var sitemap = await repository.GetByIdAsync(idSiteMap, new ColumnSet(true));

                string xmlContent = sitemap.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData, sitemap.SiteMapName, sitemap.SiteMapNameUnique, sitemap.Id, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);

        }

        private void mIShowDifferenceEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteAction(link.Link, true, PerformShowingDifferenceDescriptionAsync);
        }

        private async Task PerformShowingDifferenceDescriptionAsync(LinkedEntities<SiteMap> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceEntityDescription);

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var repository1 = new SiteMapRepository(service1);
                    var repository2 = new SiteMapRepository(service2);

                    var sitemap1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var sitemap2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(sitemap1, EntityFileNameFormatter.SiteMapIgnoreFields);
                    var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(sitemap2, EntityFileNameFormatter.SiteMapIgnoreFields);

                    if (showAllways || desc1 != desc2)
                    {
                        string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, sitemap1.SiteMapName, sitemap1.Id, EntityFileNameFormatter.Headers.EntityDescription, desc1);
                        string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, sitemap2.SiteMapName, sitemap2.Id, EntityFileNameFormatter.Headers.EntityDescription, desc2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionFailed);
            }
        }

        private void ExecuteActionDescription(Guid idSiteMap, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idSiteMap, getService);
        }

        private async Task PerformExportDescriptionToFileAsync(Guid idSiteMap, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new SiteMapRepository(service);

                var sitemap = await repository.GetByIdAsync(idSiteMap, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(sitemap, EntityFileNameFormatter.SiteMapIgnoreFields, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, sitemap.SiteMapName, sitemap.Id, EntityFileNameFormatter.Headers.EntityDescription, description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        private void mIExportSiteMap1EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity1.Id, GetService1, PerformExportDescriptionToFileAsync);
        }

        private void mIExportSiteMap2EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity2.Id, GetService2, PerformExportDescriptionToFileAsync);
        }

        private void mIExportSiteMap1SiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml, PerformExportXmlToFileAsync);
        }

        private void mIExportSiteMap2SiteMapXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SiteMap.Schema.Attributes.sitemapxml, SiteMap.Schema.Headers.sitemapxml, PerformExportXmlToFileAsync);
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSiteMaps();
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
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();

                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    this.Resources["ConnectionName1"] = connection1.Name;
                    this.Resources["ConnectionName2"] = connection2.Name;

                    UpdateButtonsEnable();

                    var task = ShowExistingSiteMaps();
                }
            });
        }

        private async void btnExportSiteMap1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenExportSiteMapExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnExportSiteMap2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenExportSiteMapExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            EntityViewItem linkedEntityMetadata = GetItemFromRoutedDataContext<EntityViewItem>(e);

            var hasTwoEntities = linkedEntityMetadata != null
                && linkedEntityMetadata.Link != null
                && linkedEntityMetadata.Link.Entity1 != null
                && linkedEntityMetadata.Link.Entity2 != null;

            var hasSecondEntity = linkedEntityMetadata != null
                && linkedEntityMetadata.Link != null
                && linkedEntityMetadata.Link.Entity2 != null;

            var items = contextMenu.Items.OfType<Control>();

            ActivateControls(items, hasTwoEntities, "menuContextDifference", "miCompareOrganizations");

            ActivateControls(items, hasSecondEntity, "menuContextConnection2");
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

        private async void mIConnection1OpenSolutionComponentInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService1();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SiteMap, entity.Link.Entity1.Id);
            }
        }

        private async void mIConnection2OpenSolutionComponentInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService2();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SiteMap, entity.Link.Entity2.Id);
            }
        }
    }
}