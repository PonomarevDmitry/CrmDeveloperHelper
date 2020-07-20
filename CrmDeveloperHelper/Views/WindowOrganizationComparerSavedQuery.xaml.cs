using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections;
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
    public partial class WindowOrganizationComparerSavedQuery : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowOrganizationComparerSavedQuery(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity
            , string filter
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            LoadEntityNames(cmBEntityName, connection1, connection2);

            cmBStatusCode.ItemsSource = new EnumBindingSourceExtension(typeof(SavedQuery.Schema.OptionSets.statuscode?)).ProvideValue(null) as IEnumerable;
            cmBStatusCode.SelectedItem = SavedQuery.Schema.OptionSets.statuscode.Active_0_Active_1;

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.SavedQueryXmlOptions);
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

            cmBEntityName.Text = filterEntity;

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwSavedQueries.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingSavedQueries();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1
                , getEntityName: GetEntityName
                , getSavedQueryName: GetSavedQueryName1
            );

            var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2
                , getEntityName: GetEntityName
                , getSavedQueryName: GetSavedQueryName2
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2())
                , getEntityName: GetEntityName
                , getSavedQueryName: GetSavedQueryName1
            );

            explorersHelper1.FillExplorers(miExplorers1);
            explorersHelper2.FillExplorers(miExplorers2);

            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu contextMenu
            )
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    if (string.Equals(item.Uid, "miExplorers1", StringComparison.InvariantCultureIgnoreCase))
                    {
                        explorersHelper1.FillExplorers(item);
                    }
                    else if (string.Equals(item.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase))
                    {
                        explorersHelper2.FillExplorers(item);
                    }
                    else if (string.Equals(item.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase))
                    {
                        compareWindowsHelper.FillCompareWindows(item);
                    }
                }
            }
        }

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.EntityName;
        }

        private string GetSavedQueryName1()
        {
            var entity = GetSelectedEntity();

            return entity?.QueryName1 ?? txtBFilter.Text.Trim();
        }

        private string GetSavedQueryName2()
        {
            var entity = GetSelectedEntity();

            return entity?.QueryName2 ?? txtBFilter.Text.Trim();
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

        private async Task ShowExistingSavedQueries()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingSavedQueries);

            this._itemsSource.Clear();

            IEnumerable<LinkedEntities<SavedQuery>> list = Enumerable.Empty<LinkedEntities<SavedQuery>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    string entityName = string.Empty;
                    SavedQuery.Schema.OptionSets.statuscode? statuscode = null;

                    this.Dispatcher.Invoke(() =>
                    {
                        if (!string.IsNullOrEmpty(cmBEntityName.Text)
                            && cmBEntityName.Items.Contains(cmBEntityName.Text)
                        )
                        {
                            entityName = cmBEntityName.Text.Trim().ToLower();
                        }

                        if (cmBStatusCode.SelectedItem is SavedQuery.Schema.OptionSets.statuscode comboBoxItem)
                        {
                            statuscode = comboBoxItem;
                        }
                    });

                    string filterEntity = null;

                    if (service1.ConnectionData.IsValidEntityName(entityName)
                        && service2.ConnectionData.IsValidEntityName(entityName)
                    )
                    {
                        filterEntity = entityName;
                    }

                    var columnsSet = new ColumnSet(
                        SavedQuery.Schema.Attributes.name
                        , SavedQuery.Schema.Attributes.returnedtypecode
                        , SavedQuery.Schema.Attributes.querytype
                        , SavedQuery.Schema.Attributes.statuscode
                    );

                    var temp = new List<LinkedEntities<SavedQuery>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        SavedQueryRepository repository1 = new SavedQueryRepository(service1);
                        SavedQueryRepository repository2 = new SavedQueryRepository(service2);

                        var task1 = repository1.GetListAsync(filterEntity, statuscode, columnsSet);
                        var task2 = repository2.GetListAsync(filterEntity, statuscode, columnsSet);

                        var list1 = await task1;
                        var list2 = await task2;

                        foreach (var query1 in list1)
                        {
                            var query2 = list2.FirstOrDefault(query => query.Id == query1.Id);

                            if (query2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedEntities<SavedQuery>(query1, query2));
                        }
                    }
                    else
                    {
                        SavedQueryRepository repository1 = new SavedQueryRepository(service1);

                        var task1 = repository1.GetListAsync(filterEntity, statuscode, columnsSet);

                        var list1 = await task1;

                        foreach (var query1 in list1)
                        {
                            temp.Add(new LinkedEntities<SavedQuery>(query1, null));
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

            ToggleControls(true, Properties.OutputStrings.LoadingSavedQueriesCompletedFormat1, list.Count());
        }

        private static IEnumerable<LinkedEntities<SavedQuery>> FilterList(IEnumerable<LinkedEntities<SavedQuery>> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent =>
                        ent.Entity1?.Id == tempGuid
                        || ent.Entity2?.Id == tempGuid
                        || ent.Entity1?.SavedQueryIdUnique == tempGuid
                        || ent.Entity2?.SavedQueryIdUnique == tempGuid
                    );
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var type1 = ent.Entity1?.ReturnedTypeCode ?? string.Empty;
                        var name1 = ent.Entity1?.Name ?? string.Empty;

                        var type2 = ent.Entity2?.ReturnedTypeCode ?? string.Empty;
                        var name2 = ent.Entity2?.Name ?? string.Empty;

                        return type1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || type2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            ;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string EntityName { get; private set; }

            public string QueryType { get; private set; }

            public string QueryName1 { get; private set; }

            public string QueryName2 { get; private set; }

            public string QueryStatus1 { get; private set; }

            public string QueryStatus2 { get; private set; }

            public LinkedEntities<SavedQuery> Link { get; private set; }

            public EntityViewItem(string entityName, string queryType, LinkedEntities<SavedQuery> link
                , string queryName1, string queryName2
                , string queryStatus1, string queryStatus2
            )
            {
                this.EntityName = entityName;
                this.QueryName1 = queryName1;
                this.QueryName2 = queryName2;
                this.QueryType = queryType;
                this.Link = link;
                this.QueryStatus1 = queryStatus1;
                this.QueryStatus2 = queryStatus2;
            }
        }

        private void LoadEntities(IEnumerable<LinkedEntities<SavedQuery>> results)
        {
            this.lstVwSavedQueries.Dispatcher.Invoke(() =>
            {
                foreach (var link in results
                      .OrderBy(ent => ent.Entity1.ReturnedTypeCode)
                      .ThenBy(ent => ent.Entity1.QueryType)
                      .ThenBy(ent => ent.Entity1.Name)
                      .ThenBy(ent => ent.Entity2?.Name)
                  )
                {
                    string queryTypeName = SavedQueryRepository.GetQueryTypeName(link.Entity1.QueryType.Value);

                    link.Entity1.FormattedValues.TryGetValue(SavedQuery.Schema.Attributes.statuscode, out var queryStatus1);

                    var queryStatus2 = string.Empty;

                    if (link.Entity2 != null)
                    {
                        link.Entity2.FormattedValues.TryGetValue(SavedQuery.Schema.Attributes.statuscode, out queryStatus2);
                    }

                    var item = new EntityViewItem(link.Entity1.ReturnedTypeCode, queryTypeName, link
                        , link.Entity1.Name, link.Entity2?.Name
                        , queryStatus1, queryStatus2
                    );

                    this._itemsSource.Add(item);
                }

                if (this.lstVwSavedQueries.Items.Count == 1)
                {
                    this.lstVwSavedQueries.SelectedItem = this.lstVwSavedQueries.Items[0];
                }
            });
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
            this.lstVwSavedQueries.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSavedQueries.SelectedItems.Count > 0;

                    var item = (this.lstVwSavedQueries.SelectedItems[0] as EntityViewItem);

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
                await ShowExistingSavedQueries();
            }
        }

        private async void cmBStatusCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowExistingSavedQueries();
        }

        private EntityViewItem GetSelectedEntity()
        {
            return this.lstVwSavedQueries.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwSavedQueries.SelectedItems.OfType<EntityViewItem>().SingleOrDefault() : null;
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
                    ExecuteAction(item.Link, false, PerformShowingDifferenceAllAsync);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteAction(LinkedEntities<SavedQuery> linked, bool showAllways, Func<LinkedEntities<SavedQuery>, bool, Task> action)
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

        private Task<string> CreateFileAsync(ConnectionData connectionData, Guid savedQueryId, string entityName, string name, string fieldTitle, FileExtension extension, string xmlContent)
        {
            return Task.Run(() => CreateFile(connectionData, savedQueryId, entityName, name, fieldTitle, extension, xmlContent));
        }

        private string CreateFile(ConnectionData connectionData, Guid savedQueryId, string entityName, string name, string fieldTitle, FileExtension extension, string xmlContent)
        {
            string fileName = EntityFileNameFormatter.GetSavedQueryFileName(connectionData.Name, entityName, name, fieldTitle, extension);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (extension == FileExtension.xml)
                    {
                        xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                            xmlContent
                            , _commonConfig
                            , XmlOptionsControls.SavedQueryXmlOptions
                            , schemaName: AbstractDynamicCommandXsdSchemas.FetchSchema
                            , savedQueryId: savedQueryId
                            , entityName: entityName
                        );
                    }
                    else if (extension == FileExtension.json)
                    {
                        xmlContent = ContentComparerHelper.FormatJson(xmlContent);
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SavedQuery.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SavedQuery.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private Task<string> CreateDescriptionFileAsync(ConnectionData connectionData, string entityName, string name, string fieldTitle, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionData, entityName, name, fieldTitle, description));
        }

        private string CreateDescriptionFile(ConnectionData connectionData, string entityName, string name, string fieldTitle, string description)
        {
            string fileName = EntityFileNameFormatter.GetSavedQueryFileName(connectionData.Name, entityName, name, fieldTitle, FileExtension.txt);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SavedQuery.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SavedQuery.Schema.EntityLogicalName, name, fieldTitle);
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

        private async Task PerformShowingDifferenceAllAsync(LinkedEntities<SavedQuery> linked, bool showAllways)
        {
            await PerformShowingDifferenceEntityDescriptionAsync(linked, showAllways);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, ContentComparerHelper.RemoveLayoutObjectCode);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, FileExtension.xml);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, FileExtension.json);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, FileExtension.sql);
        }

        private void mIShowDifferenceFetchXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceLayoutXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, PerformShowingDifferenceSingleXmlAsync, ContentComparerHelper.RemoveLayoutObjectCode);
        }

        private void mIShowDifferenceColumnSetXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, FileExtension.xml, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceLayoutJson_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, FileExtension.json, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceOfflineSqlQuery_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, FileExtension.sql, PerformShowingDifferenceSingleXmlAsync);
        }

        private void ExecuteActionLinked(
            LinkedEntities<SavedQuery> linked
            , bool showAllways
            , string fieldName
            , string fieldTitle
            , FileExtension extension
            , Func<LinkedEntities<SavedQuery>, bool, string, string, FileExtension, Action<XElement>, Task> action, Action<XElement> actionXml = null
        )
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

            action(linked, showAllways, fieldName, fieldTitle, extension, actionXml);
        }

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<SavedQuery> linked, bool showAllways, string fieldName, string fieldTitle, FileExtension extension, Action<XElement> action = null)
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
                    var repository1 = new SavedQueryRepository(service1);
                    var repository2 = new SavedQueryRepository(service2);

                    var savedQuery1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var savedQuery2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    string xml1 = savedQuery1.GetAttributeValue<string>(fieldName);
                    string xml2 = savedQuery2.GetAttributeValue<string>(fieldName);

                    if (showAllways || !ContentComparerHelper.CompareXML(xml1, xml2, false, action).IsEqual)
                    {
                        string filePath1 = await CreateFileAsync(service1.ConnectionData, savedQuery1.Id, savedQuery1.ReturnedTypeCode, savedQuery1.Name, fieldTitle, extension, xml1);
                        string filePath2 = await CreateFileAsync(service2.ConnectionData, savedQuery2.Id, savedQuery2.ReturnedTypeCode, savedQuery2.Name, fieldTitle, extension, xml2);

                        if (!File.Exists(filePath1))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service1.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery1.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (!File.Exists(filePath2))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service2.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery2.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
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

        private void mIExportSavedQuery1FetchXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery2FetchXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Headers.fetchxml, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery1LayoutXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery2LayoutXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Headers.layoutxml, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery1ColumnSetXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery2ColumnSetXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQuery.Schema.Attributes.columnsetxml, SavedQuery.Schema.Headers.columnsetxml, FileExtension.xml, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery1LayoutJson_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, FileExtension.json, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery2LayoutJson_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQuery.Schema.Attributes.layoutjson, SavedQuery.Schema.Headers.layoutjson, FileExtension.json, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery1OfflineSqlQuery_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, FileExtension.sql, PerformExportXmlToFileAsync);
        }

        private void mIExportSavedQuery2OfflineSqlQuery_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQuery.Schema.Attributes.offlinesqlquery, SavedQuery.Schema.Headers.offlinesqlquery, FileExtension.sql, PerformExportXmlToFileAsync);
        }

        private void ExecuteActionEntity(
            Guid idsavedquery
            , Func<Task<IOrganizationServiceExtented>> getService
            , string fieldName
            , string fieldTitle
            , FileExtension extension
            , Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, FileExtension, Task> action
        )
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idsavedquery, getService, fieldName, fieldTitle, extension);
        }

        private async Task PerformExportXmlToFileAsync(Guid idSavedQuery, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, FileExtension extension)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new SavedQueryRepository(service);

                var savedQuery = await repository.GetByIdAsync(idSavedQuery, new ColumnSet(true));

                string xmlContent = savedQuery.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData, savedQuery.Id, savedQuery.ReturnedTypeCode, savedQuery.Name, fieldTitle, extension, xmlContent);

                if (!File.Exists(filePath))
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SavedQuery.Schema.EntityLogicalName, savedQuery.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(null);
                }

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

            ExecuteAction(link.Link, true, PerformShowingDifferenceEntityDescriptionAsync);
        }

        private async Task PerformShowingDifferenceEntityDescriptionAsync(LinkedEntities<SavedQuery> linked, bool showAllways)
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
                    var repository1 = new SavedQueryRepository(service1);
                    var repository2 = new SavedQueryRepository(service2);

                    var savedQuery1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var savedQuery2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(savedQuery1);
                    var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(savedQuery2);

                    if (showAllways || desc1 != desc2)
                    {
                        string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, savedQuery1.ReturnedTypeCode, savedQuery1.Name, EntityFileNameFormatter.Headers.EntityDescription, desc1);
                        string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, savedQuery2.ReturnedTypeCode, savedQuery2.Name, EntityFileNameFormatter.Headers.EntityDescription, desc2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
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

        private void ExecuteActionDescription(Guid idsavedquery, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idsavedquery, getService);
        }

        private async Task PerformExportDescriptionToFileAsync(Guid idSavedQuery, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new SavedQueryRepository(service);

                var savedQuery = await repository.GetByIdAsync(idSavedQuery, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(savedQuery, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, savedQuery.ReturnedTypeCode, savedQuery.Name, EntityFileNameFormatter.Headers.EntityDescription, description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        private void mIExportSavedQuery1EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity1.Id, GetService1, PerformExportDescriptionToFileAsync);
        }

        private void mIExportSavedQuery2EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity2.Id, GetService2, PerformExportDescriptionToFileAsync);
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSavedQueries();
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

                this.Resources["ConnectionName1"] = connection1?.Name;
                this.Resources["ConnectionName2"] = connection2?.Name;

                LoadEntityNames(cmBEntityName, connection1, connection2);

                UpdateButtonsEnable();

                var task = ShowExistingSavedQueries();
            });
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

            ActivateControls(items, hasSecondEntity, "menuContextConnection2", "miExplorers2");
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

        private async void miConnection1OpenEntityMetadataInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService1();

            service.ConnectionData.OpenEntityMetadataInWeb(entity.EntityName);
        }

        private async void miConnection2OpenEntityMetadataInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService2();

            service.ConnectionData.OpenEntityMetadataInWeb(entity.EntityName);
        }

        private async void miConnection1OpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService1();

            this._iWriteToOutput.OpenFetchXmlFile(service.ConnectionData, _commonConfig, entity.EntityName);
        }

        private async void miConnection2OpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService2();

            this._iWriteToOutput.OpenFetchXmlFile(service.ConnectionData, _commonConfig, entity.EntityName);
        }

        private async void miConnection1OpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService1();

            service.ConnectionData.OpenEntityInstanceListInWeb(entity.EntityName);
        }

        private async void miConnection2OpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService2();

            service.ConnectionData.OpenEntityInstanceListInWeb(entity.EntityName);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQuery, entity.Link.Entity1.Id);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQuery, entity.Link.Entity2.Id);
            }
        }
    }
}