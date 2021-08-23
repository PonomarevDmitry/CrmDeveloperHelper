using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerOperationMultiSelect : WindowBase
    {
        private readonly List<OrganizationComparerOperationSelectItem> _source = new List<OrganizationComparerOperationSelectItem>();

        private readonly ObservableCollection<OrganizationComparerOperationSelectItem> _sourceDataGrid = new ObservableCollection<OrganizationComparerOperationSelectItem>();

        private readonly Dictionary<OrganizationComparerOperation, OrganizationComparerOperation> _doubles = new Dictionary<OrganizationComparerOperation, OrganizationComparerOperation>()
        {
            { OrganizationComparerOperation.ApplicationRibbonsWithDetails, OrganizationComparerOperation.ApplicationRibbons },

            { OrganizationComparerOperation.EntityRibbonsWithDetails, OrganizationComparerOperation.EntityRibbons },

            { OrganizationComparerOperation.WebResourcesWithDetails, OrganizationComparerOperation.WebResources },

            { OrganizationComparerOperation.WorkflowsAttributesWithDetails, OrganizationComparerOperation.WorkflowsAttributes },
        };

        public WindowOrganizationComparerOperationMultiSelect()
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            lstVwAttributes.ItemsSource = _sourceDataGrid;

            List<OrganizationComparerOperation> allOperations = Enum.GetValues(typeof(OrganizationComparerOperation)).OfType<OrganizationComparerOperation>().ToList();

            foreach (var operation in allOperations)
            {
                var item = new OrganizationComparerOperationSelectItem(operation, EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(operation));

                _source.Add(item);
            }

            foreach (var item in _source)
            {
                _sourceDataGrid.Add(item);
            }

            this.DecreaseInit();

            txtBFilter.SelectionStart = txtBFilter.Text.Length;
            txtBFilter.SelectionLength = 0;
            txtBFilter.Focus();
        }

        public class OrganizationComparerOperationSelectItem : INotifyPropertyChanging, INotifyPropertyChanged
        {
            public OrganizationComparerOperation Operation { get; private set; }

            public string DisplayName { get; private set; }

            private bool _IsChecked = false;
            public bool IsChecked
            {
                get => _IsChecked;
                set
                {
                    if (_IsChecked == value)
                    {
                        return;
                    }

                    this.OnPropertyChanging(nameof(IsChecked));
                    this._IsChecked = value;
                    this.OnPropertyChanged(nameof(IsChecked));
                }
            }

            public OrganizationComparerOperationSelectItem(OrganizationComparerOperation operation, string displayName)
            {
                this.Operation = operation;

                this.DisplayName = displayName;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public event PropertyChangingEventHandler PropertyChanging;

            private void OnPropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private void OnPropertyChanging(string propertyName)
            {
                this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterOperations();
            }
        }

        private void FilterOperations()
        {
            _sourceDataGrid.Clear();

            var list = _source.AsEnumerable();

            var textName = txtBFilter.Text.Trim();

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                list = list.Where(ent => ent.DisplayName.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1);
            }

            foreach (var item in list)
            {
                _sourceDataGrid.Add(item);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectAllAttributesInDataGrid(IEnumerable<OrganizationComparerOperationSelectItem> items, bool isSelected)
        {
            this.IncreaseInit();

            foreach (var item in items)
            {
                item.IsChecked = isSelected;
            }

            this.DecreaseInit();
        }

        private void hypLinkSelectAll_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            SelectAllAttributesInDataGrid(_source, true);
        }

        private void hypLinkDeselectAll_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            SelectAllAttributesInDataGrid(_source, false);
        }

        private void hypLinkSelectFiltered_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            SelectAllAttributesInDataGrid(lstVwAttributes.Items.OfType<OrganizationComparerOperationSelectItem>(), true);
        }

        private void hypLinkDeselectFiltered_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            SelectAllAttributesInDataGrid(lstVwAttributes.Items.OfType<OrganizationComparerOperationSelectItem>(), false);
        }

        public List<OrganizationComparerOperation> GetOperations()
        {
            var result = new List<OrganizationComparerOperation>();

            if (!this._source.Any(o => o.IsChecked))
            {
                return result;
            }

            result.AddRange(this._source.Where(o => o.IsChecked).Select(o => o.Operation).OrderBy(o => o));

            foreach (var operation in _doubles.Keys)
            {
                if (result.Contains(operation) && result.Contains(_doubles[operation]))
                {
                    result.Remove(_doubles[operation]);
                }
            }

            return result;
        }

        private readonly HashSet<OrganizationComparerOperation> _operationsEntities = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.Entities
            , OrganizationComparerOperation.EntitiesByAudit
            , OrganizationComparerOperation.EntitiesPrivileges

            , OrganizationComparerOperation.EntityLabels
            , OrganizationComparerOperation.EntityMaps
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsEntitiesObjects = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.SystemForms
            , OrganizationComparerOperation.SystemViews
            , OrganizationComparerOperation.SystemCharts
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsEntitiesAll = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.Entities
            , OrganizationComparerOperation.EntitiesByAudit
            , OrganizationComparerOperation.EntitiesPrivileges

            , OrganizationComparerOperation.EntityLabels
            , OrganizationComparerOperation.EntityMaps

            , OrganizationComparerOperation.SystemForms
            , OrganizationComparerOperation.SystemViews
            , OrganizationComparerOperation.SystemCharts

            , OrganizationComparerOperation.DisplayStrings

            , OrganizationComparerOperation.EntityRibbons
            , OrganizationComparerOperation.EntityRibbonsWithDetails
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsEntitiesAllWithoutRibbons = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.Entities
            , OrganizationComparerOperation.EntitiesByAudit
            , OrganizationComparerOperation.EntitiesPrivileges

            , OrganizationComparerOperation.EntityLabels
            , OrganizationComparerOperation.EntityMaps

            , OrganizationComparerOperation.SystemForms
            , OrganizationComparerOperation.SystemViews
            , OrganizationComparerOperation.SystemCharts

            , OrganizationComparerOperation.DisplayStrings
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsTemplates = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.EmailTemplates
            , OrganizationComparerOperation.MailMergeTemplates
            , OrganizationComparerOperation.KBArticleTemplates
            , OrganizationComparerOperation.ContractTemplates
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsConnectionRoles = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.ConnectionRoles
            , OrganizationComparerOperation.ConnectionRoleCategories
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsPluginInformation = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.PluginAssemblies
            , OrganizationComparerOperation.PluginTypes

            , OrganizationComparerOperation.PluginStepsByStates

            , OrganizationComparerOperation.PluginSteps
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsSecurity = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.SecurityRoles
            , OrganizationComparerOperation.FieldSecurityProfiles
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsWorkflows = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.WorkflowsAttributes
            , OrganizationComparerOperation.WorkflowsAttributesWithDetails
            , OrganizationComparerOperation.WorkflowsStates
        };

        private readonly HashSet<OrganizationComparerOperation> _operationsRibbons = new HashSet<OrganizationComparerOperation>()
        {
            OrganizationComparerOperation.EntityRibbons
            , OrganizationComparerOperation.EntityRibbonsWithDetails

            , OrganizationComparerOperation.ApplicationRibbons
            , OrganizationComparerOperation.ApplicationRibbonsWithDetails
        };

        private void SelectOperationsInDataGridOnly(HashSet<OrganizationComparerOperation> operations, bool isSelected)
        {
            this.IncreaseInit();

            foreach (var item in _source)
            {
                if (operations.Contains(item.Operation))
                {
                    item.IsChecked = isSelected;
                }
                else
                {
                    item.IsChecked = false;
                }
            }

            this.DecreaseInit();
        }

        private void SelectOperationsInDataGrid(HashSet<OrganizationComparerOperation> operations, bool isSelected)
        {
            this.IncreaseInit();

            foreach (var item in _source)
            {
                if (operations.Contains(item.Operation))
                {
                    item.IsChecked = isSelected;
                }
            }

            this.DecreaseInit();
        }

        #region CheckOnly

        private void mICheckOnlyEntities_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsEntities, true);
        }

        private void mICheckOnlyEntitiesObjects_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsEntitiesObjects, true);
        }

        private void mICheckOnlyEntitiesAllWithoutRibbons_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsEntitiesAllWithoutRibbons, true);
        }

        private void mICheckOnlyEntitiesAll_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsEntitiesAll, true);
        }

        private void mICheckOnlyTemplates_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsTemplates, true);
        }

        private void mICheckOnlyConnectionRoles_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsConnectionRoles, true);
        }

        private void mICheckOnlyPluginInformation_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsPluginInformation, true);
        }

        private void mICheckOnlySecurity_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsSecurity, true);
        }

        private void mICheckOnlyWorkflows_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGridOnly(_operationsWorkflows, true);
        }

        #endregion CheckOnly

        #region Check

        private void mICheckEntities_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsEntities, true);
        }

        private void mICheckEntitiesObjects_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsEntitiesObjects, true);
        }

        private void mICheckEntitiesAllWithoutRibbons_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsEntitiesAllWithoutRibbons, true);
        }

        private void mICheckEntitiesAll_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsEntitiesAll, true);
        }

        private void mICheckTemplates_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsTemplates, true);
        }

        private void mICheckConnectionRoles_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsConnectionRoles, true);
        }

        private void mICheckPluginInformation_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsPluginInformation, true);
        }

        private void mICheckSecurity_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsSecurity, true);
        }

        private void mICheckWorkflows_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsWorkflows, true);
        }

        #endregion Check

        #region UnCheck

        private void mIUncheckEntities_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsEntities, false);
        }

        private void mIUncheckEntitiesObjects_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsEntitiesObjects, false);
        }

        private void mIUncheckEntitiesAll_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsEntitiesAll, false);
        }

        private void mIUncheckPluginInformation_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsPluginInformation, false);
        }

        private void mIUncheckTemplates_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsTemplates, false);
        }

        private void mIUncheckConnectionRoles_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsConnectionRoles, false);
        }

        private void mIUncheckSecurity_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsSecurity, false);
        }

        private void mIUncheckWorkflows_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsWorkflows, false);
        }

        private void mIUncheckRibbons_Click(object sender, RoutedEventArgs e)
        {
            SelectOperationsInDataGrid(_operationsRibbons, false);
        }

        #endregion UnCheck
    }
}