using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
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
        private int _init = 0;

        private EntityCollection _entityCollection;

        private IWriteToOutput _iWriteToOutput;

        private readonly object sysObjectConnections = new object();

        private readonly Dictionary<Guid, object> _syncCacheObjects = new Dictionary<Guid, object>();

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        public event EventHandler<EventArgs> ConnectionChanged;

        private TabItem _selectedItem;

        private void OnConnectionChanged()
        {
            this.ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public string FilePath { get; private set; }

        public ConnectionData ConnectionData => cmBCurrentConnection.SelectedItem as ConnectionData;

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

        protected bool IsControlsEnabled => this._init == 0;

        protected void ChangeInitByEnabled(bool enabled)
        {
            if (enabled)
            {
                this._init++;
            }
            else
            {
                this._init--;
            }
        }

        protected void IncreaseInit()
        {
            this._init++;
        }

        protected void DecreaseInit()
        {
            this._init--;
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
        }

        public void SetSource(string filePath, ConnectionData connectionData, IWriteToOutput iWriteToOutput)
        {
            if (!string.IsNullOrEmpty(this.FilePath))
            {
                return;
            }

            this.FilePath = filePath;
            this._iWriteToOutput = iWriteToOutput;

            BindingOperations.EnableCollectionSynchronization(connectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            BindCollections(connectionData);

            cmBCurrentConnection.ItemsSource = connectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = connectionData;

            ClearGridAndTextBox();

            this.dGrParameters.DataContext = cmBCurrentConnection;
        }

        private void btnExecuteFetchXml_Click(object sender, RoutedEventArgs e)
        {
            this.Execute();
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


        public async Task Execute()
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            ToggleControls(this.ConnectionData, false, Properties.WindowStatusStrings.ExecutingFetch);

            ClearGridAndTextBox();

            if (!TryLoadFileText())
            {
                ToggleControls(this.ConnectionData, true, Properties.WindowStatusStrings.FileNotExists);
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

                ToggleControls(this.ConnectionData, true, Properties.WindowStatusStrings.ConnectionIsNotSelected);

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

                ToggleControls(this.ConnectionData, true, Properties.WindowStatusStrings.FileTextIsNotValidXml);

                return;
            }

            txtBFetchXml.Text = doc.ToString();

            if (CheckParametersAndReturnHasNew(doc, connectionData))
            {
                ToggleControls(this.ConnectionData, true, Properties.WindowStatusStrings.FillNewParameters);

                return;
            }

            connectionData.Save();

            FillParametersValues(doc, connectionData);

            UpdateStatus(this.ConnectionData, Properties.WindowStatusStrings.ExecutingFetch);

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

                UpdateStatus(this.ConnectionData, Properties.WindowStatusStrings.FileNotExists);

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
            this._entityCollection = null;

            var service = await GetServiceAsync(connectionData);

            try
            {
                var request = new RetrieveMultipleRequest()
                {
                    Query = new FetchExpression(fetchXml.ToString()),
                };

                var response = (RetrieveMultipleResponse)service.Execute(request);

                this._entityCollection = response.EntityCollection;

                LoadData(connectionData, this._entityCollection, fetchXml);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.FetchQueryExecutedSuccessfullyFormat1, this._entityCollection.Entities.Count);
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

                    ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.FetchExecutionError);
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

                dGrResults.ItemsSource = dataTable.DefaultView;

                tabControl.SelectedItem = tbResults;

                tbResults.IsEnabled = true;
                tbResults.Visibility = Visibility.Visible;

                tbResults.IsSelected = true;
                tbResults.Focus();

                this._selectedItem = tbResults;
            });
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

            public ConnectionData ConnectionData { get; private set; }

            public PrimaryGuidView(ConnectionData connectionData, string logicalName, Guid idValue)
            {
                this.ConnectionData = connectionData;
                this.LogicalName = logicalName;
                this.Id = idValue;
            }

            public string Url => this.ConnectionData.GetEntityInstanceUrl(this.LogicalName, this.Id);

            public int CompareTo(PrimaryGuidView other)
            {
                if (other == null)
                {
                    return -1;
                }

                if (this.ConnectionData != other.ConnectionData)
                {
                    return this.ConnectionData.Name.CompareTo(other.ConnectionData.Name);
                }

                return this.Id.CompareTo(other.Id);
            }

            public int CompareTo(object obj)
            {
                return this.CompareTo(obj as PrimaryGuidView);
            }

            public bool Equals(PrimaryGuidView other)
            {
                if (this.ConnectionData != other.ConnectionData)
                {
                    return false;
                }

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

            public EntityReferenceView(ConnectionData connectionData, string logicalName, Guid idValue, string name)
                : base(connectionData, logicalName, idValue)
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
                        value = new PrimaryGuidView(connectionData, entity.LogicalName, idValue);
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
                            value = new PrimaryGuidView(connectionData, aliasedValue.EntityLogicalName, refIdValue);
                        }
                    }

                    value = EntityDescriptionHandler.GetUnderlyingValue(value);

                    if (value is EntityReference entityReference)
                    {
                        value = new EntityReferenceView(connectionData, entityReference.LogicalName, entityReference.Id, entityReference.Name);
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
                    ToggleControls(connectionData, false, string.Empty);

                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    ToggleControls(connectionData, true, string.Empty);
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

            UpdateStatus(this.ConnectionData, string.Empty);
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.OfType<ConnectionData>())
            {
                removed.Save();
            }

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                ClearGridAndTextBox();

                if (cmBCurrentConnection.SelectedItem is ConnectionData connectionData)
                {
                    BindCollections(connectionData);
                }
            });

            OnConnectionChanged();

            if (!IsControlsEnabled)
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(IsControlsEnabled
                , this.tSProgressBar
                , this.cmBCurrentConnection
                , this.tSProgressBar
                , this.btnExecuteFetchXml
                , this.btnExecuteFetchXml2
                , this.stBtnExecuteFetchXml
                , this.dGrParameters
            );

            ToggleControl(IsControlsEnabled && _entityCollection != null && _entityCollection.Entities.Count > 0
                , this.menuExecuteWorkflow
                , this.menuAssignToUser
                , this.menuAssignToTeam
                , this.menuTransferToConnection
            );
        }

        protected void ToggleControl(bool enabled, params Control[] controlsArray)
        {
            if (controlsArray == null || !controlsArray.Any())
            {
                return;
            }

            foreach (var control in controlsArray)
            {
                if (control == null)
                {
                    continue;
                }

                control.Dispatcher.Invoke(() =>
                {
                    if (control is TextBox textBox)
                    {
                        textBox.IsReadOnly = !enabled;
                    }
                    else if (control is ProgressBar progressBar)
                    {
                        progressBar.IsIndeterminate = !enabled;
                    }
                    else
                    {
                        control.IsEnabled = enabled;
                    }
                });
            }
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

                var commonConfig = CommonConfiguration.Get();

                Views.WindowHelper.OpenEntityMetadataWindow(_iWriteToOutput, service, commonConfig, null, entity.LogicalName, null);
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
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                var entityFull = service.RetrieveByQuery<Entity>(entity.LogicalName, entity.Id, new ColumnSet(true));

                var commonConfig = CommonConfiguration.Get();

                string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, null, service.ConnectionData);

                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , entityFull.LogicalName
                    , filePath);

                _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
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
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            this.ConnectionData?.OpenEntityMetadataInWeb(entityReferenceView.LogicalName);
        }

        private async void mIOpenEntityReferenceMetadataWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                var commonConfig = CommonConfiguration.Get();

                Views.WindowHelper.OpenEntityMetadataWindow(_iWriteToOutput, service, commonConfig, null, entityReferenceView.LogicalName, null);
            }
        }

        private void mICopyEntityReferenceId_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            Clipboard.SetText(entityReferenceView.Id.ToString());
        }

        private void mICopyEntityReferenceEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            Clipboard.SetText(entityReferenceView.LogicalName);
        }

        private void mICopyEntityReferenceName_Click(object sender, RoutedEventArgs e)
        {
            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            Clipboard.SetText(entityReferenceView.Name);
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
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                var entityFull = service.RetrieveByQuery<Entity>(entityReferenceView.LogicalName, entityReferenceView.Id, new ColumnSet(true));

                var commonConfig = CommonConfiguration.Get();

                string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, null, service.ConnectionData);

                _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , entityFull.LogicalName
                    , filePath);

                _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }
        }

        private bool TryFindEntityReferenceViewFromRow(RoutedEventArgs e, out EntityReferenceView entityReferenceView)
        {
            entityReferenceView = null;

            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return false;
            }

            ContextMenu contextMenu = GetContextMenuFromMenuItem(menuItem);

            if (contextMenu == null
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

            if (!(dataRowView[cell.Column.SortMemberPath] is EntityReferenceView value))
            {
                return false;
            }

            entityReferenceView = value;

            return true;
        }

        private ContextMenu GetContextMenuFromMenuItem(MenuItem menuItem)
        {
            ItemsControl result = menuItem;

            do
            {
                result = ItemsControl.ItemsControlFromItemContainer(result);

                if (result != null && result is ContextMenu contextMenu)
                {
                    return contextMenu;
                }

            } while (result != null);

            return null;
        }

        private bool TryFindNavigateUriFromHyperlink(RoutedEventArgs e, out Uri navigateUri)
        {
            navigateUri = null;

            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return false;
            }

            var contextMenu = GetContextMenuFromMenuItem(menuItem);

            if (contextMenu == null
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

        private IEnumerable<Guid> GetSelectedEntityIds()
        {
            HashSet<Guid> hash = new HashSet<Guid>();

            List<Guid> result = new List<Guid>();

            var selectedCells = dGrResults.SelectedCells.ToList();

            foreach (var cell in selectedCells)
            {
                if (cell.Item is DataRowView item
                    && item[_columnOriginalEntity] != null
                    && item[_columnOriginalEntity] is Entity entity
                    && entity.Id != Guid.Empty
                )
                {
                    if (hash.Add(entity.Id))
                    {
                        result.Add(entity.Id);
                    }
                }
            }

            return result;
        }

        private async void mIExecuteWorkflowOnEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            await ExecuteWorkfowOnEntities(entity.LogicalName, new[] { entity.Id });
        }

        private async void miExecuteWorkflowOnAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            await ExecuteWorkfowOnEntities(_entityCollection.EntityName, _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id));
        }

        private async void miExecuteWorkflowOnSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntityIds();

            if (!selectedEntityIds.Any())
            {
                return;
            }

            await ExecuteWorkfowOnEntities(_entityCollection.EntityName, selectedEntityIds);
        }

        private async Task ExecuteWorkfowOnEntities(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!entityIds.Any(id => id != Guid.Empty))
            {
                return;
            }

            var service = await GetServiceAsync(this.ConnectionData);

            var repository = new WorkflowRepository(service);

            Func<string, Task<IEnumerable<Workflow>>> getter = (string filter) => repository.GetListAsync(
                entityName
                , (int)Workflow.Schema.OptionSets.category.Workflow_0
                , null
                , new ColumnSet(
                    Workflow.Schema.Attributes.workflowid
                    , Workflow.Schema.Attributes.category
                    , Workflow.Schema.Attributes.name
                    , Workflow.Schema.Attributes.mode
                    , Workflow.Schema.Attributes.uniquename
                    , Workflow.Schema.Attributes.primaryentity
                    , Workflow.Schema.Attributes.iscustomizable
                    , Workflow.Schema.Attributes.statuscode
                )
            );

            IEnumerable<DataGridColumn> columns = WorkflowRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<Workflow>(_iWriteToOutput, service.ConnectionData, Workflow.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            Workflow workflow = form.SelectedEntity;

            string operationName = string.Format(Properties.OperationNames.ExecutingWorkflowFormat2, service.ConnectionData.Name, workflow.Name);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ExecutingWorkflowFormat2, service.ConnectionData.Name, workflow.Name);

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            foreach (var id in entityIds.Where(e => e != Guid.Empty).Distinct())
            {
                try
                {
                    var request = new ExecuteWorkflowRequest()
                    {
                        EntityId = id,
                        WorkflowId = workflow.Id,
                    };

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExecutingOnEntityWorkflowFormat1, workflow.Name);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.ExecuteAsync(request);

                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ExecutingWorkflowCompletedFormat2, service.ConnectionData.Name, workflow.Name);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        private async void mIAssignEntityToUser_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            await AssignEntitiesToUser(entity.LogicalName, new[] { entity.Id });
        }

        private async void miAssignToUserAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            await AssignEntitiesToUser(_entityCollection.EntityName, _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id));
        }

        private async void miAssignToUserSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntityIds();

            if (!selectedEntityIds.Any())
            {
                return;
            }

            await AssignEntitiesToUser(_entityCollection.EntityName, selectedEntityIds);
        }

        private async Task AssignEntitiesToUser(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!entityIds.Any(id => id != Guid.Empty))
            {
                return;
            }

            var service = await GetServiceAsync(this.ConnectionData);

            var repository = new SystemUserRepository(service);

            Func<string, Task<IEnumerable<SystemUser>>> getter = (string filter) => repository.GetUsersAsync(filter
                , new ColumnSet(
                    SystemUser.Schema.Attributes.domainname
                    , SystemUser.Schema.Attributes.fullname
                    , SystemUser.Schema.Attributes.businessunitid
                    , SystemUser.Schema.Attributes.isdisabled
                )
            );

            IEnumerable<DataGridColumn> columns = SystemUserRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<SystemUser>(_iWriteToOutput, service.ConnectionData, SystemUser.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            SystemUser user = form.SelectedEntity;

            string operationName = string.Format(Properties.OperationNames.AssigningEntitiesToUserFormat2, service.ConnectionData.Name, user.FullName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.AssigningEntitiesToUserFormat2, service.ConnectionData.Name, user.FullName);

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            foreach (var id in entityIds.Where(e => e != Guid.Empty).Distinct())
            {
                try
                {
                    var request = new AssignRequest()
                    {
                        Target = new EntityReference(entityName, id),
                        Assignee = user.ToEntityReference(),
                    };

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.AssigningEntityToUserFormat1, user.FullName);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.ExecuteAsync(request);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.AssigningEntitiesToUserCompletedFormat2, service.ConnectionData.Name, user.FullName);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        private async void mIAssignEntityToTeam_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (entity.Id == Guid.Empty)
            {
                return;
            }

            await AssignEntitiesToTeam(entity.LogicalName, new[] { entity.Id });
        }

        private async void miAssignToTeamAllEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
                || !_entityCollection.Entities.Any(en => en.Id != Guid.Empty)
            )
            {
                return;
            }

            await AssignEntitiesToTeam(_entityCollection.EntityName, _entityCollection.Entities.Where(en => en.Id != Guid.Empty).Select(en => en.Id));
        }

        private async void miAssignToTeamSelectedEntites_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            IEnumerable<Guid> selectedEntityIds = GetSelectedEntityIds();

            if (!selectedEntityIds.Any())
            {
                return;
            }

            await AssignEntitiesToTeam(_entityCollection.EntityName, selectedEntityIds);
        }

        private async Task AssignEntitiesToTeam(string entityName, IEnumerable<Guid> entityIds)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!entityIds.Any(id => id != Guid.Empty))
            {
                return;
            }

            var service = await GetServiceAsync(this.ConnectionData);

            var repository = new TeamRepository(service);

            Func<string, Task<IEnumerable<Team>>> getter = (string filter) => repository.GetOwnerTeamsAsync(filter
                , new ColumnSet(
                    Team.Schema.Attributes.name
                    , Team.Schema.Attributes.businessunitid
                    , Team.Schema.Attributes.isdefault
                )
            );

            IEnumerable<DataGridColumn> columns = TeamRepository.GetDataGridColumnOwner();

            var form = new WindowEntitySelect<Team>(_iWriteToOutput, service.ConnectionData, Team.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            Team team = form.SelectedEntity;

            string operationName = string.Format(Properties.OperationNames.AssigningEntitiesToTeamFormat2, service.ConnectionData.Name, team.Name);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.AssigningEntitiesToTeamFormat2, service.ConnectionData.Name, team.Name);

            _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

            foreach (var id in entityIds.Where(e => e != Guid.Empty).Distinct())
            {
                try
                {
                    var request = new AssignRequest()
                    {
                        Target = new EntityReference(entityName, id),
                        Assignee = team.ToEntityReference(),
                    };

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.AssigningEntityToTeamFormat1, team.Name);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, entityName, id);

                    await service.ExecuteAsync(request);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.AssigningEntitiesToTeamCompletedFormat2, service.ConnectionData.Name, team.Name);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        private void MITransferToConnection_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            mITransferAllEntitiesToConnection.Items.Clear();

            var connectionData = this.ConnectionData;

            if (connectionData != null)
            {
                var otherConnections = connectionData.ConnectionConfiguration.Connections.Where(c => c.ConnectionId != connectionData.ConnectionId).ToList();

                if (otherConnections.Any())
                {
                    foreach (var connection in otherConnections)
                    {
                        MenuItem menuItem = new MenuItem()
                        {
                            Header = connection.Name,
                            Tag = connection,
                        };
                        menuItem.Click += mITransferAllEntitiesToConnection_Click;

                        if (mITransferAllEntitiesToConnection.Items.Count > 0)
                        {
                            mITransferAllEntitiesToConnection.Items.Add(new Separator());
                        }

                        mITransferAllEntitiesToConnection.Items.Add(menuItem);
                    }
                }
            }
        }

        private async void mITransferAllEntitiesToConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityCollection == null
                || _entityCollection.Entities.Count == 0
            )
            {
                return;
            }

            if (!(sender is MenuItem menuItem)
                || menuItem.Tag == null
                || !(menuItem.Tag is ConnectionData targetConnectionData)
            )
            {
                return;
            }

            var sourceService = await GetServiceAsync(this.ConnectionData);

            var targetService = await GetServiceAsync(targetConnectionData);

            string entityName = _entityCollection.EntityName;

            EntityMetadataRepository repository = new EntityMetadataRepository(targetService);

            var targetEntityMetadata = await repository.GetEntityMetadataAsync(_entityCollection.EntityName);

            if (targetEntityMetadata == null)
            {
                return;
            }

            List<Entity> entities = _entityCollection.Entities.ToList();



        }

        private async void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityFromDataRowView(e, out var entity))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                var commonConfig = CommonConfiguration.Get();

                WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entity.LogicalName, entity.Id);
            }
        }

        private async void mIChangeEntityReferenceInEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (!TryFindEntityReferenceViewFromRow(e, out var entityReferenceView))
            {
                return;
            }

            if (this.ConnectionData != null)
            {
                var service = await GetServiceAsync(this.ConnectionData);

                var commonConfig = CommonConfiguration.Get();

                WindowHelper.OpenEntityEditor(_iWriteToOutput, service, commonConfig, entityReferenceView.LogicalName, entityReferenceView.Id);
            }
        }
    }
}