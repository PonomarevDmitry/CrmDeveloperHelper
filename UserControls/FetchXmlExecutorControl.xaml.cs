using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for FetchXmlExecutorControl.xaml
    /// </summary>
    public partial class FetchXmlExecutorControl : UserControl
    {
        private readonly object sysObjectConnections = new object();

        private ConnectionConfiguration _connectionConfig;

        private Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private bool _controlsEnabled = true;

        public event EventHandler<EventArgs> ConnectionChanged;

        private NavigateUriMultiValueConverter _navigateUriConverter = new NavigateUriMultiValueConverter();

        private TabItem _selectedItem;

        private void OnConnectionChanged()
        {
            this.ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public string FilePath { get; private set; }

        public ConnectionData ConnectionData
        {
            get
            {
                return cmBCurrentConnection.SelectedItem as ConnectionData;
            }
        }

        private const string paramColumnParametersWidth = "ColumnParametersWidth";
        private const string paramColumnFetchTextWidth = "ColumnFetchTextWidth";

        public FetchXmlExecutorControl()
        {
            InitializeComponent();

            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

            var settings = GetUserControlSettings();

            if (settings.DictDouble.ContainsKey(paramColumnParametersWidth)
                && settings.DictDouble.ContainsKey(paramColumnFetchTextWidth)
                )
            {
                var widthParameters = settings.DictDouble[paramColumnParametersWidth];
                var widthFetchText = settings.DictDouble[paramColumnFetchTextWidth];

                if (widthParameters + widthFetchText > 0)
                {
                    var tempParamerts = widthParameters * 100 / (widthParameters + widthFetchText);
                    var tempFetchText = widthFetchText * 100 / (widthParameters + widthFetchText);

                    columnParameters.Width = new GridLength(tempParamerts, GridUnitType.Star);
                    columnFetchText.Width = new GridLength(tempFetchText, GridUnitType.Star);
                }
            }

            tabControl.Loaded += TabControl_Loaded;
        }

        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                tabControl.SelectedItem = _selectedItem;
            }
        }

        private UserControlSettings GetUserControlSettings()
        {
            var name = this.Name;

            if (string.IsNullOrEmpty(name))
            {
                name = this.GetType().Name;
            }

            return FileOperations.GetUserControlSettings(name);
        }

        public void DetachFromSourceCollection()
        {
            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            var settings = GetUserControlSettings();
            settings.DictDouble[paramColumnParametersWidth] = columnParameters.Width.Value;
            settings.DictDouble[paramColumnFetchTextWidth] = columnFetchText.Width.Value;
            settings.Save();

            _connectionConfig.Save();
        }

        public void SetSource(string filePath, ConnectionData connectionData)
        {
            if (!string.IsNullOrEmpty(this.FilePath))
            {
                return;
            }

            this.FilePath = filePath;
            this._connectionConfig = connectionData.ConnectionConfiguration;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            BindCollections(connectionData);

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = connectionData;

            ClearGridAndTextBox();

            this.dGrParameters.DataContext = cmBCurrentConnection;
        }

        private void btnExecuteFetchXml_Click(object sender, RoutedEventArgs e)
        {
            this.Execute();
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }


        public async Task Execute()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExecutingFetch);

            ClearGridAndTextBox();

            if (!TryLoadFileText())
            {
                ToggleControls(true, Properties.WindowStatusStrings.FileNotExists);
                return;
            }

            if (!(cmBCurrentConnection.SelectedItem is ConnectionData connectionData))
            {
                txtBErrorText.Text = Properties.WindowStatusStrings.ConnectionIsNotSelected;

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                this._selectedItem = tbErrorText;

                ToggleControls(true, Properties.WindowStatusStrings.ConnectionIsNotSelected);

                return;
            }

            var fileText = txtBFetchXml.Text.Trim();

            if (!ContentCoparerHelper.TryParseXml(fileText, out var exception, out var doc))
            {
                StringBuilder text = new StringBuilder();

                text.AppendLine(Properties.WindowStatusStrings.FileTextIsNotValidXml);
                text.AppendFormat("File: {0}", this.FilePath).AppendLine();
                text.AppendLine();
                text.AppendLine();

                var description = DTEHelper.GetExceptionDescription(exception);

                text.AppendLine(description);

                txtBErrorText.Text = text.ToString();

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                this._selectedItem = tbErrorText;

                ToggleControls(true, Properties.WindowStatusStrings.FileTextIsNotValidXml);

                return;
            }

            txtBFetchXml.Text = doc.ToString();

            if (CheckParametersAndReturnHasNew(doc, connectionData))
            {
                ToggleControls(true, Properties.WindowStatusStrings.FillNewParameters);

                return;
            }

            _connectionConfig.Save();

            FillParametersValues(doc, connectionData);

            UpdateStatus(Properties.WindowStatusStrings.ExecutingFetch);

            await ExecuteFetchAsync(doc, connectionData);
        }

        private bool TryLoadFileText()
        {
            if (!File.Exists(this.FilePath))
            {
                txtBErrorText.Text = string.Format(Properties.MessageBoxStrings.FileNotExistsFormat1, FilePath);

                tbErrorText.IsEnabled = true;
                tbErrorText.Visibility = Visibility.Visible;

                tbErrorText.IsSelected = true;
                tbErrorText.Focus();

                this._selectedItem = tbErrorText;

                UpdateStatus(Properties.WindowStatusStrings.FileNotExists);

                return false;
            }

            var fileText = File.ReadAllText(this.FilePath);

            txtBFetchXml.Text = fileText;

            return true;
        }

        private Task ExecuteFetchAsync(XElement fetchXml, ConnectionData connectionData)
        {
            return Task.Run(async () => await ExecuteFetch(fetchXml, connectionData));
        }

        private async Task ExecuteFetch(XElement fetchXml, ConnectionData connectionData)
        {
            EntityCollection entityCollection = null;

            try
            {
                var service = await GetServiceAsync(connectionData);

                var request = new RetrieveMultipleRequest()
                {
                    Query = new FetchExpression(fetchXml.ToString()),
                };

                var response = (RetrieveMultipleResponse)service.Execute(request);

                entityCollection = response.EntityCollection;

                LoadData(connectionData, entityCollection, fetchXml);

                ToggleControls(true, Properties.WindowStatusStrings.FetchQueryExecutedSuccessfullyFormat1, entityCollection.Entities.Count);
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(() =>
                {
                    StringBuilder text = new StringBuilder();

                    text.AppendLine(Properties.WindowStatusStrings.FetchExecutionError);
                    text.AppendLine();

                    var description = DTEHelper.GetExceptionDescription(ex);

                    text.AppendLine(description);

                    txtBErrorText.Text = text.ToString();

                    tbErrorText.IsEnabled = true;
                    tbErrorText.Visibility = Visibility.Visible;

                    tbErrorText.IsSelected = true;
                    tbErrorText.Focus();

                    this._selectedItem = tbErrorText;

                    ToggleControls(true, Properties.WindowStatusStrings.FetchExecutionError);
                });

                return;
            }
        }

        private void LoadData(ConnectionData connectionData, EntityCollection entityCollection, XElement fetchXml)
        {
            var columnsInFetch = GetColumns(connectionData, fetchXml);

            var dataTable = EntityCollectionToDataTable(connectionData, entityCollection, out Dictionary<string, string> columnMapping);

            this.Dispatcher.Invoke(() =>
            {
                dGrResults.Columns.Clear();

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

                    AddNewColumnInDataGrid(dataTable, attributeName, dataColumnName, connectionData);
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

                    AddNewColumnInDataGrid(dataTable, attributeName, dataColumnName, connectionData);
                }

                dGrResults.ItemsSource = dataTable.DefaultView;

                tabControl.SelectedItem = tbResults;

                tbResults.IsEnabled = true;
                tbResults.Visibility = Visibility.Visible;

                tbResults.IsSelected = true;
                tbResults.Focus();

                this._selectedItem = tbResults;
            });
        }

        private void AddNewColumnInDataGrid(DataTable dataTable, string attributeName, string dataColumnName, ConnectionData connectionData)
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

                    Binding = new MultiBinding()
                    {
                        Mode = BindingMode.OneTime,

                        Converter = _navigateUriConverter,
                        ConverterParameter = connectionData,

                        Bindings =
                        {
                            new Binding("[" + dataColumnName + "].LogicalName"),
                            new Binding("[" + dataColumnName + "].Id"),
                        },
                    },
                };

                dGrResults.Columns.Add(columnDGT);
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

                dGrResults.Columns.Add(columnDGT);
            }
        }

        private void dGrResults_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = e.OriginalSource as Hyperlink;

            if (link != null && link.NavigateUri != null && !string.IsNullOrEmpty(link.NavigateUri.AbsoluteUri))
            {
                System.Diagnostics.Process.Start(link.NavigateUri.AbsoluteUri);
            }
        }

        private class NavigateUriMultiValueConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (values == null || values.Length != 2)
                {
                    return Binding.DoNothing;
                }

                if (values[0] is string entityName
                    && values[1] is Guid id
                    && parameter is ConnectionData connectionData
                    )
                {
                    var url = connectionData.GetEntityInstanceUrl(entityName, id);

                    if (Uri.TryCreate(url, UriKind.Absolute, out var result))
                    {
                        return result;
                    }
                }

                return null;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
            {
                return new object[] { Binding.DoNothing };
            }
        }

        private List<string> GetColumns(ConnectionData connectionData, XElement fetchXml)
        {
            List<string> result = new List<string>();

            var linkElements = fetchXml.DescendantsAndSelf().Where(n => IsLinkElement(n)).ToList();

            {
                var entityElements = fetchXml.DescendantsAndSelf().Where(IsAttributeOrAllElement).ToList();

                foreach (var attributeNode in entityElements)
                {
                    if (IsAttributeElement(attributeNode))
                    {
                        var attrAlias = attributeNode.Attribute("alias");

                        if (attrAlias != null && !string.IsNullOrEmpty(attrAlias.Value))
                        {
                            result.Add(attrAlias.Value);
                        }
                        else
                        {
                            string name = attributeNode.Attribute("name").Value;

                            var parentElement = attributeNode.Ancestors().FirstOrDefault(IsEntityOrLinkElement);

                            if (parentElement != null)
                            {
                                var parentAlias = parentElement.Attribute("alias");

                                if (parentAlias != null && !string.IsNullOrEmpty(parentAlias.Value))
                                {
                                    name = parentAlias.Value + "." + name;
                                }
                                else if (IsLinkElement(parentElement))
                                {
                                    if (linkElements.Contains(parentElement))
                                    {
                                        var index = linkElements.IndexOf(parentElement) + 1;

                                        name = parentElement.Attribute("name")?.Value + index.ToString() + "." + name;
                                    }
                                }
                            }

                            result.Add(name);
                        }
                    }
                    else if (IsAllAttributesElement(attributeNode))
                    {
                        var parentElement = attributeNode.Ancestors().FirstOrDefault(IsEntityOrLinkElement);

                        if (parentElement != null)
                        {
                            var parentAlias = parentElement.Attribute("alias");
                            var parentEntityName = parentElement.Attribute("name");

                            string parentPrefixName = string.Empty;

                            if (parentAlias != null && !string.IsNullOrEmpty(parentAlias.Value))
                            {
                                parentPrefixName = parentAlias.Value + ".";
                            }
                            else if (IsLinkElement(parentElement))
                            {
                                if (linkElements.Contains(parentElement))
                                {
                                    var index = linkElements.IndexOf(parentElement) + 1;

                                    parentPrefixName = parentElement.Attribute("name")?.Value + index.ToString();
                                }
                            }

                            var intellisenseData = connectionData.IntellisenseData;

                            if (intellisenseData != null
                                && intellisenseData.Entities != null
                                && intellisenseData.Entities.ContainsKey(parentEntityName.Value)
                                && intellisenseData.Entities[parentEntityName.Value].Attributes != null
                                )
                            {
                                foreach (var attrName in intellisenseData.Entities[parentEntityName.Value].Attributes.Keys)
                                {
                                    string name = parentPrefixName + attrName;

                                    result.Add(name);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsEntityOrLinkElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "entity", StringComparison.OrdinalIgnoreCase)
                || string.Equals(element.Name.LocalName, "link-entity", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsLinkElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "link-entity", StringComparison.OrdinalIgnoreCase);
        }

        private class PrimaryGuidView : IComparable, IComparable<PrimaryGuidView>, IEquatable<PrimaryGuidView>
        {
            public string LogicalName { get; private set; }

            public Guid Id { get; private set; }

            public PrimaryGuidView(string logicalName, Guid idValue)
            {
                this.LogicalName = logicalName;
                this.Id = idValue;
            }

            public int CompareTo(PrimaryGuidView other)
            {
                if (other == null)
                {
                    return -1;
                }

                return this.Id.CompareTo(other.Id);
            }

            public int CompareTo(object obj)
            {
                return this.CompareTo(obj as PrimaryGuidView);
            }

            public bool Equals(PrimaryGuidView other)
            {
                if (other == null)
                {
                    return false;
                }

                return this.Id == other.Id;
            }

            public override string ToString()
            {
                return this.Id.ToString();
            }
        }

        private class EntityReferenceView : PrimaryGuidView
        {
            public string Name { get; private set; }

            public EntityReferenceView(string logicalName, Guid idValue, string name)
                : base(logicalName, idValue)
            {
                this.Name = name;

                if (string.IsNullOrEmpty(name))
                {
                    this.Name = string.Format("{0} - {1}", LogicalName, Id);
                }
            }

            public override string ToString()
            {
                return this.Name;
            }
        }

        private const string _columnOriginalEntity = "columnOriginalEntity______";

        private static DataTable EntityCollectionToDataTable(ConnectionData connectionData, EntityCollection entityCollection, out Dictionary<string, string> columnMapping)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(_columnOriginalEntity, typeof(Entity));

            columnMapping = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var entity in entityCollection.Entities)
            {
                DataRow row = dataTable.NewRow();

                row[_columnOriginalEntity] = entity;

                foreach (string attributeName in entity.Attributes.Keys)
                {
                    var value = entity.Attributes[attributeName];

                    if (value == null || EntityDescriptionHandler.GetUnderlyingValue(value) == null)
                    {
                        continue;
                    }

                    if (connectionData.IntellisenseData != null
                        && connectionData.IntellisenseData.Entities != null
                        && connectionData.IntellisenseData.Entities.ContainsKey(entity.LogicalName)
                        && value is Guid idValue
                        && string.Equals(connectionData.IntellisenseData.Entities[entity.LogicalName].EntityPrimaryIdAttribute, attributeName, StringComparison.InvariantCultureIgnoreCase)
                        )
                    {
                        value = new PrimaryGuidView(entity.LogicalName, idValue);
                    }

                    string columnName = string.Format("{0}___{1}", entity.LogicalName, attributeName.Replace(".", "_"));

                    if (value is AliasedValue aliasedValue)
                    {
                        columnName = string.Format("{0}___{1}___{2}", attributeName.Replace(".", "_"), aliasedValue.EntityLogicalName, aliasedValue.AttributeLogicalName);

                        if (connectionData.IntellisenseData != null
                            && connectionData.IntellisenseData.Entities != null
                            && connectionData.IntellisenseData.Entities.ContainsKey(aliasedValue.EntityLogicalName)
                            && aliasedValue.Value is Guid refIdValue
                            && string.Equals(connectionData.IntellisenseData.Entities[aliasedValue.EntityLogicalName].EntityPrimaryIdAttribute, aliasedValue.AttributeLogicalName, StringComparison.InvariantCultureIgnoreCase)
                        )
                        {
                            value = new PrimaryGuidView(aliasedValue.EntityLogicalName, refIdValue);
                        }
                    }

                    value = EntityDescriptionHandler.GetUnderlyingValue(value);

                    if (value is EntityReference entityReference)
                    {
                        value = new EntityReferenceView(entityReference.LogicalName, entityReference.Id, entityReference.Name);
                    }

                    if (value is Money money)
                    {
                        value = money.Value;
                    }

                    if (value is DateTime dateTime)
                    {
                        value = dateTime.ToLocalTime();
                    }

                    if (value is OptionSetValue optionSetValue)
                    {
                        value = (entity.FormattedValues != null && entity.FormattedValues.ContainsKey(attributeName) ? string.Format("{0} - ", entity.FormattedValues[attributeName]) : string.Empty) + optionSetValue.Value.ToString();
                    }

                    if (value is BooleanManagedProperty booleanManagedProperty)
                    {
                        value = string.Format("{0,-5}        CanBeChanged = {1,-5}", booleanManagedProperty.Value, booleanManagedProperty.CanBeChanged);
                    }

                    if (dataTable.Columns.IndexOf(columnName) == -1)
                    {
                        if (!columnMapping.ContainsKey(attributeName))
                        {
                            columnMapping.Add(attributeName, columnName);
                        }

                        dataTable.Columns.Add(columnName, value.GetType());
                    }

                    row[columnName] = value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private async Task<IOrganizationServiceExtented> GetServiceAsync(ConnectionData connectionData)
        {
            if (connectionData != null)
            {
                if (!_connectionCache.ContainsKey(connectionData.ConnectionId))
                {
                    DTEHelper.Singleton?.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    DTEHelper.Singleton?.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    DTEHelper.Singleton?.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private void FillParametersValues(XElement doc, ConnectionData connectionData)
        {
            var entityElements = doc.DescendantsAndSelf().Where(IsConditonElement).ToList();

            foreach (var entity in entityElements)
            {
                var attrValue = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "value", StringComparison.InvariantCultureIgnoreCase));

                if (attrValue != null && !string.IsNullOrEmpty(attrValue.Value))
                {
                    var valueText = attrValue.Value;

                    if (valueText.StartsWith("@")
                        || (valueText.StartsWith("{") && valueText.EndsWith("}") && !Guid.TryParse(valueText, out _))
                        )
                    {
                        var parameter = connectionData.FetchXmlRequestParameterList.FirstOrDefault(p => string.Equals(p.Name, valueText));

                        if (parameter != null)
                        {
                            attrValue.Value = parameter.Value;
                        }
                    }
                }
            }
        }

        private bool CheckParametersAndReturnHasNew(XElement doc, ConnectionData connectionData)
        {
            bool hasNew = false;

            var entityElements = doc.DescendantsAndSelf().Where(IsConditonElement).ToList();

            foreach (var entity in entityElements)
            {
                var attrValue = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "value", StringComparison.InvariantCultureIgnoreCase));

                if (attrValue != null && !string.IsNullOrEmpty(attrValue.Value))
                {
                    var valueText = attrValue.Value;

                    if (valueText.StartsWith("@")
                        || (valueText.StartsWith("{") && valueText.EndsWith("}") && !Guid.TryParse(valueText, out _))
                        )
                    {
                        var parameter = connectionData.FetchXmlRequestParameterList.FirstOrDefault(p => string.Equals(p.Name, valueText));

                        if (parameter == null)
                        {
                            var newParameter = new FetchXmlRequestParameter()
                            {
                                Name = valueText,
                            };

                            connectionData.FetchXmlRequestParameterList.Add(newParameter);

                            if (!hasNew)
                            {
                                hasNew = true;

                                dGrParameters.SelectedItem = newParameter;
                            }
                        }
                    }
                }
            }

            return hasNew;
        }

        private static bool IsAttributeOrAllElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "attribute", StringComparison.OrdinalIgnoreCase)
                || string.Equals(element.Name.LocalName, "all-attributes", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAttributeElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "attribute", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAllAttributesElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "all-attributes", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsConditonElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "condition", StringComparison.OrdinalIgnoreCase);
        }

        private void ClearGridAndTextBox()
        {
            tbFetchXml.IsSelected = true;
            tbFetchXml.Focus();

            txtBErrorText.Text = string.Empty;

            tbErrorText.Visibility = tbResults.Visibility = Visibility.Collapsed;
            tbErrorText.IsEnabled = tbResults.IsEnabled = false;

            dGrResults.Columns.Clear();
            dGrResults.Items.DetachFromSourceCollection();
            dGrResults.ItemsSource = null;

            this._selectedItem = tbFetchXml;

            UpdateStatus(string.Empty);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                ClearGridAndTextBox();
                
                if (cmBCurrentConnection.SelectedItem is ConnectionData connectionData)
                {
                    BindCollections(connectionData);
                }
            });

            OnConnectionChanged();

            if (!_controlsEnabled)
            {
                return;
            }
        }

        private void BindCollections(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return;
            }

            if (!_syncCacheObjects.ContainsKey(connectionData.ConnectionId))
            {
                _syncCacheObjects.Add(connectionData.ConnectionId, new object());

                BindingOperations.EnableCollectionSynchronization(connectionData.FetchXmlRequestParameterList, _syncCacheObjects[connectionData.ConnectionId]);
            }
        }

        private void dGrResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (((FrameworkElement)e.OriginalSource).DataContext is DataRowView item
                    && item[_columnOriginalEntity] != null
                    && item[_columnOriginalEntity] is Entity entity
                    )
                {
                    this.ConnectionData?.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
                }
            }
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(this.btnExecuteFetchXml, enabled);
            ToggleControl(this.btnExecuteFetchXml2, enabled);

            ToggleControl(this.dGrParameters, enabled);

            ToggleControl(cmBCurrentConnection, enabled);

            ToggleProgressBar(enabled);
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

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            this.ConnectionData?.OpenEntityMetadataInWeb(entity.LogicalName);
        }

        private async void mIOpenEntityMetadataWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                if (DTEHelper.Singleton != null)
                {
                    var commonConfig = CommonConfiguration.Get();

                    Views.WindowHelper.OpenEntityMetadataWindow(DTEHelper.Singleton, service, commonConfig, entity.LogicalName, null, null);
                }
            }
        }

        private void mICopyEntityId_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            Clipboard.SetText(entity.Id.ToString());
        }

        private void mICopyEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            Clipboard.SetText(entity.LogicalName);
        }

        private void mICopyEntityUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            var url = this.ConnectionData?.GetEntityInstanceUrl(entity.LogicalName, entity.Id);

            Clipboard.SetText(url);
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                var entityFull = service.RetrieveByQuery<Entity>(entity.LogicalName, entity.Id, new ColumnSet(true));

                var commonConfig = CommonConfiguration.Get();

                string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, "EntityDescription", "txt");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, null, service.ConnectionData);

                DTEHelper.Singleton?.WriteToOutput(Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , entityFull.LogicalName
                    , filePath);

                DTEHelper.Singleton?.PerformAction(filePath);
            }
        }

        private bool TryFindEntityFromDataRowView(RoutedEventArgs e, out Entity entity)
        {
            entity = null;

            DataRowView item = null;

            if (e.OriginalSource is FrameworkElement framework
                && framework.DataContext is DataRowView dataRowView
                )
            {
                item = dataRowView;
            }
            else if (e.OriginalSource is FrameworkContentElement frameworkContent
                && frameworkContent.DataContext is DataRowView dataRowView2
                )
            {
                item = dataRowView2;
            }

            if (item != null
                && item[_columnOriginalEntity] != null
                && item[_columnOriginalEntity] is Entity result
                )
            {
                entity = result;

                return true;
            }

            return false;
        }

        private void mIOpenEntityReferenceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityNameFromEntityReference(e, out string entityName, out Guid entityId))
            {
                return;
            }

            this.ConnectionData?.OpenEntityMetadataInWeb(entityName);
        }

        private async void mIOpenEntityReferenceMetadataWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityNameFromEntityReference(e, out string entityName, out var entityId))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                if (DTEHelper.Singleton != null)
                {
                    var commonConfig = CommonConfiguration.Get();

                    Views.WindowHelper.OpenEntityMetadataWindow(DTEHelper.Singleton, service, commonConfig, entityName, null, null);
                }
            }
        }

        private void mICopyEntityReferenceId_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityNameFromEntityReference(e, out string entityName, out var entityId))
            {
                return;
            }

            Clipboard.SetText(entityId.ToString());
        }

        private void mICopyEntityReferenceName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityNameFromEntityReference(e, out string entityName, out var entityId))
            {
                return;
            }

            Clipboard.SetText(entityName);
        }

        private void mICopyEntityReferenceUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindNavigateUriFromHyperlink(e, out var navigateUri))
            {
                return;
            }

            Clipboard.SetText(navigateUri.AbsoluteUri);
        }

        private async void mICreateEntityReferenceDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityNameFromEntityReference(e, out string entityName, out var entityId))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                var entityFull = service.RetrieveByQuery<Entity>(entityName, entityId, new ColumnSet(true));

                var commonConfig = CommonConfiguration.Get();

                string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, "EntityDescription", "txt");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, null, service.ConnectionData);

                DTEHelper.Singleton?.WriteToOutput(Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , entityFull.LogicalName
                    , filePath);

                DTEHelper.Singleton?.PerformAction(filePath);
            }
        }

        private bool TryFindEntityNameFromEntityReference(RoutedEventArgs e, out string entityName, out Guid entityId)
        {
            entityName = null;
            entityId = Guid.Empty;

            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return false;
            }

            if (!(ItemsControl.ItemsControlFromItemContainer(menuItem) is ContextMenu contextMenu)
                || contextMenu.PlacementTarget == null
                || !(contextMenu.PlacementTarget is TextBlock textBlock)
                || !(contextMenu.DataContext is DataRowView dataRowView)
                || !(textBlock.Parent is DataGridCell cell)
                )
            {
                return false;
            }

            if (string.IsNullOrEmpty(cell.Column.SortMemberPath))
            {
                return false;
            }

            if (!dataRowView.Row.Table.Columns.Contains(cell.Column.SortMemberPath))
            {
                return false;
            }

            if (!(dataRowView[cell.Column.SortMemberPath] is PrimaryGuidView value))
            {
                return false;
            }

            entityName = value.LogicalName;
            entityId = value.Id;

            return true;
        }

        private bool TryFindNavigateUriFromHyperlink(RoutedEventArgs e, out Uri navigateUri)
        {
            navigateUri = null;

            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return false;
            }

            if (!(ItemsControl.ItemsControlFromItemContainer(menuItem) is ContextMenu contextMenu)
                || contextMenu.PlacementTarget == null
                || !(contextMenu.PlacementTarget is TextBlock textBlock)
                )
            {
                return false;
            }

            var link = textBlock.Inlines.OfType<Hyperlink>().FirstOrDefault();

            if (link == null
               || link.NavigateUri == null
               || string.IsNullOrEmpty(link.NavigateUri.AbsoluteUri)
               )
            {
                return false;
            }

            navigateUri = link.NavigateUri;

            return true;
        }

        private void btnCopyJavaScriptToClipboardCode_Click(object sender, RoutedEventArgs e)
        {
            ConvertFetchXmlToJavaScriptCodeInternal();
        }

        private void ConvertFetchXmlToJavaScriptCodeInternal()
        {
            var fileText = txtBFetchXml.Text.Trim();

            string jsCode = ContentCoparerHelper.FormatToJavaScript("fetchXml", fileText);

            Clipboard.SetText(jsCode);
        }
    }
}