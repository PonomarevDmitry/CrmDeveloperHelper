using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectEntityReference : WindowBase
    {
        protected readonly IWriteToOutput _iWriteToOutput;

        private readonly IOrganizationServiceExtented _service;

        private readonly Dictionary<string, EntityMetadata> _entityMetadataCache = new Dictionary<string, EntityMetadata>(StringComparer.InvariantCultureIgnoreCase);

        public EntityReference SelectedEntityReference { get; private set; }

        public WindowSelectEntityReference(
            IWriteToOutput outputWindow
            , IOrganizationServiceExtented service
            , IEnumerable<string> entityNames
        )
        {
            InitializeComponent();

            this._iWriteToOutput = outputWindow;
            this._service = service;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            cmBEntityName.Focus();

            LoadEntityNames(entityNames);

            LoadEntityMetadataAsync(entityNames);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadEntityNames(IEnumerable<string> entityNames)
        {
            string text = cmBEntityName.Text;

            cmBEntityName.Items.Clear();

            foreach (var item in entityNames.OrderBy(s => s))
            {
                cmBEntityName.Items.Add(item);
            }

            cmBEntityName.Text = text;

            if (cmBEntityName.Items.Count == 1)
            {
                var item = cmBEntityName.Items[0].ToString();

                cmBEntityName.IsEnabled = false;

                cmBEntityName.SelectedIndex = 0;
                cmBEntityName.Text = item;

                txtBEntityId.Focus();
            }
            else
            {
                cmBEntityName.SelectedIndex = 0;
            }
        }

        private async Task LoadEntityMetadataAsync(IEnumerable<string> entityNames)
        {
            if (!entityNames.Any())
            {
                return;
            }

            var repository = new EntityMetadataRepository(_service);

            var entities = await repository.GetEntitiesWithAttributesFullAsync(entityNames);

            foreach (var entityMetadata in entities)
            {
                _entityMetadataCache[entityMetadata.LogicalName] = entityMetadata;
            }
        }

        private void MakeOkClick()
        {
            StringBuilder message = new StringBuilder();

            string entityName = string.Empty;
            Guid? entityId = null;

            TryParseUrl(out var urlEntityName, out var urlObjectTypeCode, out var urlEntityId);

            string textEntityName = cmBEntityName.SelectedItem?.ToString().Trim(' ', '<', '>');
            string textEntityId = txtBEntityId.Text?.Trim(' ', '<', '>');

            if (!string.IsNullOrEmpty(textEntityName))
            {
                entityName = textEntityName;
            }

            if (string.IsNullOrEmpty(entityName)
                && !string.IsNullOrEmpty(urlEntityName)
                )
            {
                entityName = urlEntityName;
            }

            if (string.IsNullOrEmpty(entityName)
                && urlObjectTypeCode.HasValue
                )
            {
                if (_service != null
                    && _service.ConnectionData != null
                    && _service.ConnectionData.IntellisenseData != null
                    && _service.ConnectionData.IntellisenseData.Entities != null
                )
                {
                    var entityIntellisense = _service.ConnectionData.IntellisenseData.Entities.Values.FirstOrDefault(e => e.ObjectTypeCode == urlObjectTypeCode.Value);

                    if (entityIntellisense != null)
                    {
                        entityName = entityIntellisense.EntityLogicalName;
                    }
                }
            }

            if (string.IsNullOrEmpty(entityName))
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.EntityNameIsEmpty);
            }

            if (!string.IsNullOrEmpty(textEntityId)
                && Guid.TryParse(textEntityId, out Guid tempGuid)
            )
            {
                entityId = tempGuid;
            }
            else if (urlEntityId.HasValue)
            {
                entityId = urlEntityId;
            }

            if (!entityId.HasValue)
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.CannotParseGuid);
            }

            if (message.Length > 0)
            {
                MessageBox.Show(message.ToString(), Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.SelectedEntityReference = new EntityReference(entityName, entityId.Value);

            this.DialogResult = true;

            this.Close();
        }

        private void TryParseUrl(out string urlEntityName, out int? urlObjectTypeCode, out Guid? urlEntityId)
        {
            urlObjectTypeCode = null;
            urlEntityName = string.Empty;
            urlEntityId = null;

            var textUrl = txtBEntityUrl.Text.Trim();

            var split = textUrl.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var url in split)
            {
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }

                var temp = url;

                temp = temp.Trim(' ', '<', '>');

                if (string.IsNullOrEmpty(temp))
                {
                    continue;
                }

                if (Uri.TryCreate(temp, UriKind.Absolute, out var uri))
                {
                    var query = HttpUtility.ParseQueryString(uri.Query);

                    if (query.AllKeys.Contains("id", StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(query.Get("id")) && Guid.TryParse(query.Get("id"), out Guid guid))
                        {
                            urlEntityId = guid;
                        }
                    }

                    if (query.AllKeys.Contains("etn", StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(query.Get("etn")))
                        {
                            urlEntityName = query.Get("etn");
                        }
                    }

                    if (query.AllKeys.Contains("etc", StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(query.Get("etc")) && int.TryParse(query.Get("etc"), out int tempInt))
                        {
                            urlObjectTypeCode = tempInt;
                        }
                    }

                    return;
                }
            }
        }

        private void btnSelectEntity_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DataRowView item = WindowBase.GetItemFromRoutedDataContext<DataRowView>(e);

                if (item != null
                    && item[EntityDescriptionHandler.ColumnOriginalEntity] != null
                    && item[EntityDescriptionHandler.ColumnOriginalEntity] is Entity entity
                )
                {
                    cmBEntityName.SelectedItem = entity.LogicalName;
                    txtBEntityId.Text = entity.Id.ToString();

                    MakeOkClick();
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
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

            ToggleControl(this.tSProgressBar, this.btnSelect);

            UpdateButtonsEnable();
        }

        protected void UpdateButtonsEnable()
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwEntities.SelectedItems.Count > 0;

                    UIElement[] list = { btnSelect };

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

        private class EntityViewItem
        {
            public string Name => SavedQuery.Name;

            public SavedQuery SavedQuery { get; }

            public EntityViewItem(SavedQuery savedQuery)
            {
                this.SavedQuery = savedQuery;
            }

            public override string ToString()
            {
                return this.Name;
            }
        }

        private async void cmBEntityName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmBSearchView.Items.Clear();

            if (cmBEntityName.SelectedItem == null)
            {
                return;
            }

            string entityName = cmBEntityName.SelectedItem.ToString();

            var repository = new SavedQueryRepository(_service);

            var viewsList = await repository.GetListSearchQueriesAsync(entityName, new ColumnSet(true));

            foreach (var savedQuery in viewsList)
            {
                cmBSearchView.Items.Add(new EntityViewItem(savedQuery));
            }

            if (cmBSearchView.Items.Count > 0)
            {
                cmBSearchView.SelectedIndex = 0;
            }
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                PerformSearchAsync();
            }
        }

        private void txtBEntity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                MakeOkClick();
            }
        }

        private async Task PerformSearchAsync()
        {
            try
            {
                lstVwEntities.Items.DetachFromSourceCollection();
                lstVwEntities.ItemsSource = null;

                if (cmBEntityName.SelectedItem == null)
                {
                    return;
                }

                string entityName = cmBEntityName.SelectedItem.ToString();

                if (!_entityMetadataCache.ContainsKey(entityName))
                {
                    return;
                }

                var entityMetadata = _entityMetadataCache[entityName];

                var savedQuery = cmBSearchView.SelectedItem as EntityViewItem;

                if (savedQuery == null)
                {
                    return;
                }

                string searchText = txtBFilter.Text.Trim();

                if (!ContentComparerHelper.TryParseXml(savedQuery.SavedQuery.FetchXml, out _, out var fetchXmlDoc))
                {
                    return;
                }

                FillParametersInFetch(entityMetadata, fetchXmlDoc, searchText);

                var response = await _service.ExecuteAsync<RetrieveMultipleResponse>(new RetrieveMultipleRequest()
                {
                    Query = new FetchExpression(fetchXmlDoc.ToString()),
                });

                var columnsInFetch = EntityDescriptionHandler.GetColumnsFromFetch(_service.ConnectionData, fetchXmlDoc);

                var entityCollection = response.EntityCollection;

                var dataTable = EntityDescriptionHandler.ConvertEntityCollectionToDataTable(_service.ConnectionData, entityCollection, out Dictionary<string, string> columnMapping);

                this.Dispatcher.Invoke(() =>
                {
                    lstVwEntities.Columns.Clear();

                    foreach (var attributeName in columnsInFetch)
                    {
                        if (!columnMapping.ContainsKey(attributeName))
                        {
                            continue;
                        }

                        var dataColumnName = columnMapping[attributeName];

                        if (!dataTable.Columns.Contains(dataColumnName))
                        {
                            continue;
                        }

                        AddNewColumnInDataGrid(dataTable, attributeName, dataColumnName);
                    }

                    foreach (var attributeName in columnMapping.Keys)
                    {
                        if (columnsInFetch.Contains(attributeName))
                        {
                            continue;
                        }

                        var dataColumnName = columnMapping[attributeName];

                        if (!dataTable.Columns.Contains(dataColumnName))
                        {
                            continue;
                        }

                        AddNewColumnInDataGrid(dataTable, attributeName, dataColumnName);
                    }

                    lstVwEntities.ItemsSource = dataTable.DefaultView;
                });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void AddNewColumnInDataGrid(DataTable dataTable, string attributeName, string dataColumnName)
        {
            var dataColumn = dataTable.Columns[dataColumnName];

            if (dataColumn.DataType == typeof(EntityReferenceView)
                || dataColumn.DataType == typeof(PrimaryGuidView)
                )
            {
                var columnDGT = new DataGridHyperlinkColumn()
                {
                    Header = attributeName.Replace("_", "__"),
                    Width = DataGridLength.Auto,

                    SortMemberPath = dataColumnName,

                    ContentBinding = new Binding("[" + dataColumnName + "]")
                    {
                        Mode = BindingMode.OneTime,
                    },

                    Binding = new Binding("[" + dataColumnName + "].Url")
                    {
                        Mode = BindingMode.OneTime,
                    },
                };

                lstVwEntities.Columns.Add(columnDGT);
            }
            else
            {
                string format = null;

                if (dataColumn.DataType == typeof(decimal)
                   || dataColumn.DataType == typeof(double)
                   )
                {
                    format = "{0:F2}";
                }
                else if (dataColumn.DataType == typeof(DateTime))
                {
                    format = "{0:G}";
                }

                var columnDGT = new DataGridTextColumn()
                {
                    Header = attributeName.Replace("_", "__"),
                    Width = DataGridLength.Auto,

                    SortMemberPath = dataColumnName,

                    Binding = new Binding(dataColumnName)
                    {
                        Mode = BindingMode.OneTime,

                        StringFormat = format,
                        ConverterCulture = CultureInfo.CurrentCulture,
                    },
                };

                lstVwEntities.Columns.Add(columnDGT);
            }
        }

        private static void FillParametersInFetch(EntityMetadata entityMetadata, XElement fetchXmlDoc, string searchText)
        {
            var filterElements = fetchXmlDoc
                .DescendantsAndSelf()
                .Where(IsQuickFindFields)
                .ToList();

            foreach (var filter in filterElements)
            {
                filter.Attribute("isquickfindfields").Remove();

                var conditionElements = filter
                    .Descendants()
                    .Where(IsConditionElement)
                    .ToList();

                foreach (var condition in conditionElements)
                {
                    string attributeName = condition.Attribute("attribute")?.Value;
                    string value = condition.Attribute("value")?.Value;

                    if (string.IsNullOrEmpty(attributeName))
                    {
                        continue;
                    }

                    if (string.Equals(value, "{0}", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var attr = entityMetadata.Attributes.FirstOrDefault(a => string.Equals(a.LogicalName, attributeName, StringComparison.InvariantCultureIgnoreCase));

                        if (attr == null)
                        {
                            condition.Remove();
                            continue;
                        }

                        if (string.IsNullOrEmpty(searchText))
                        {
                            condition.Remove();
                            continue;
                        }

                        switch (attr.AttributeType.Value)
                        {
                            case AttributeTypeCode.Memo:
                            case AttributeTypeCode.String:

                                condition.Attribute("value").Value = searchText.Replace("*", "%") + "%";

                                break;

                            case AttributeTypeCode.Boolean:

                                if (bool.TryParse(searchText, out bool boolValue))
                                {
                                    condition.Attribute("value").Value = boolValue.ToString();
                                }
                                else
                                {
                                    condition.Remove();
                                }
                                break;

                            case AttributeTypeCode.Customer:
                            case AttributeTypeCode.Lookup:
                            case AttributeTypeCode.Owner:

                                if (entityMetadata.Attributes.FirstOrDefault(a => string.Equals(a.LogicalName, attributeName + "name", StringComparison.InvariantCultureIgnoreCase)) == null)
                                {
                                    condition.Remove();
                                }
                                else
                                {
                                    condition.Attribute("attribute").Value += "name";

                                    if (string.IsNullOrEmpty(searchText))
                                    {
                                        condition.Attribute("value").Value = "%";
                                    }
                                    else
                                    {
                                        condition.Attribute("value").Value = searchText.Replace("*", "%") + "%";
                                    }
                                }
                                break;

                            case AttributeTypeCode.DateTime:

                                if (DateTime.TryParse(searchText, out var dt))
                                {
                                    condition.Attribute("value").Value = dt.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    condition.Remove();
                                }
                                break;

                            case AttributeTypeCode.Decimal:
                            case AttributeTypeCode.Double:
                            case AttributeTypeCode.Money:

                                if (decimal.TryParse(searchText, out decimal decimalValue))
                                {
                                    condition.Attribute("value").Value = decimalValue.ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    condition.Remove();
                                }
                                break;

                            case AttributeTypeCode.Integer:

                                if (int.TryParse(searchText, out int intValue))
                                {
                                    condition.Attribute("value").Value = intValue.ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    condition.Remove();
                                }
                                break;

                            case AttributeTypeCode.Picklist:

                                if (attr is PicklistAttributeMetadata picklistAttribute)
                                {
                                    FillOptionSetValue(searchText, condition, picklistAttribute.OptionSet.Options);
                                }
                                break;

                            case AttributeTypeCode.State:

                                if (attr is StateAttributeMetadata stateAttributeMetadata)
                                {
                                    FillOptionSetValue(searchText, condition, stateAttributeMetadata.OptionSet.Options);
                                }
                                break;

                            case AttributeTypeCode.Status:

                                if (attr is StatusAttributeMetadata statusAttributeMetadata)
                                {
                                    FillOptionSetValue(searchText, condition, statusAttributeMetadata.OptionSet.Options);
                                }
                                break;
                        }
                    }
                    else if (string.Equals(value, "{1}", StringComparison.InvariantCultureIgnoreCase))
                    {

                    }
                }

                if (!filter.HasElements)
                {
                    filter.Remove();
                }
            }
        }

        private static void FillOptionSetValue(string searchText, XElement condition, OptionMetadataCollection options)
        {
            OptionMetadata opt = null;

            if (int.TryParse(searchText, out int tempInt))
            {
                opt = options.FirstOrDefault(o => o.Value == tempInt);
            }

            if (opt == null)
            {
                opt = options.FirstOrDefault(o => o.Label.LocalizedLabels.Any(l => string.Equals(l.Label, searchText, StringComparison.InvariantCultureIgnoreCase)));
            }

            if (opt != null)
            {
                condition.Attribute("value").Value = opt.Value.Value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                condition.Remove();
            }
        }

        private static bool IsConditionElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "condition", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsQuickFindFields(XElement element)
        {
            if (!string.Equals(element.Name.LocalName, "filter", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var attribute = element.Attribute("isquickfindfields");

            if (attribute == null)
            {
                return false;
            }

            if (!string.Equals(attribute.Value, "1", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private void btnActions_Click(object sender, RoutedEventArgs e)
        {
            if (btnActions.ContextMenu == null)
            {
                return;
            }

            //btnActions.ContextMenu.Width = btnActions.ActualWidth;

            btnActions.ContextMenu.PlacementTarget = btnActions;
            btnActions.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            ContextMenuService.SetPlacement(btnActions, System.Windows.Controls.Primitives.PlacementMode.Bottom);

            btnActions.ContextMenu.IsOpen = true;
        }

        private void btnOpenViewInWeb_Click(object sender, RoutedEventArgs e)
        {
            var savedQuery = cmBSearchView.SelectedItem as EntityViewItem;

            if (savedQuery == null)
            {
                return;
            }

            _service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQuery, savedQuery.SavedQuery.Id);
        }

        private void btnOpenViewListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var savedQuery = cmBSearchView.SelectedItem as EntityViewItem;

            if (savedQuery == null)
            {
                return;
            }

            _service.ConnectionData.OpenEntityInstanceListInWeb(savedQuery.SavedQuery.ReturnedTypeCode, savedQuery.SavedQuery.Id);
        }
    }
}
