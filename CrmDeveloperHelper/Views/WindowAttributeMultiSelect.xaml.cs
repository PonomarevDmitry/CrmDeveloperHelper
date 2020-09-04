using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowAttributeMultiSelect : WindowWithSingleConnection
    {
        private readonly EntityMetadata _entityMetadata;

        private readonly List<AttributeSelectItem> _source = new List<AttributeSelectItem>();

        private readonly ObservableCollection<AttributeSelectItem> _sourceDataGrid = new ObservableCollection<AttributeSelectItem>();

        public WindowAttributeMultiSelect(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , EntityMetadata entityMetadata
            , string selectedAttributes
        ) : base(iWriteToOutput, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            lstVwAttributes.ItemsSource = _sourceDataGrid;

            this._entityMetadata = entityMetadata;

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            HashSet<string> selectedAttributesColl = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            if (!string.IsNullOrEmpty(selectedAttributes))
            {
                var split = selectedAttributes.Split(',');

                foreach (var item in split)
                {
                    selectedAttributesColl.Add(item);
                }
            }

            foreach (var attr in _entityMetadata.Attributes.Where(a => string.IsNullOrEmpty(a.AttributeOf)).OrderBy(a => a.LogicalName))
            {
                var item = new AttributeSelectItem(attr)
                {
                    IsChecked = selectedAttributesColl.Contains(attr.LogicalName),
                };

                item.PropertyChanged += Item_PropertyChanged;

                _source.Add(item);
            }

            foreach (var item in _source.OrderBy(a => a.LogicalName))
            {
                _sourceDataGrid.Add(item);
            }

            this.DecreaseInit();

            UpdateSelectedAttributesText();

            txtBFilter.SelectionStart = txtBFilter.Text.Length;
            txtBFilter.SelectionLength = 0;
            txtBFilter.Focus();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSelectedAttributesText();
        }

        private void UpdateSelectedAttributesText()
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            txtBSelectedAttributes.Dispatcher.Invoke(() =>
            {
                txtBSelectedAttributes.Text = GetAttributes();
            });
        }

        public class AttributeSelectItem : INotifyPropertyChanging, INotifyPropertyChanged
        {
            public AttributeMetadata AttributeMetadata { get; private set; }

            public string LogicalName => AttributeMetadata.LogicalName;

            public string DisplayName { get; private set; }

            public string AttributeTypeName => AttributeMetadata.AttributeType.ToString();

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

            public AttributeSelectItem(AttributeMetadata attributeMetadata)
            {
                this.AttributeMetadata = attributeMetadata;

                this.DisplayName = Helpers.CreateFileHandler.GetLocalizedLabel(attributeMetadata.DisplayName);
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

        protected override void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar);
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterAttributes();
            }
        }

        private void FilterAttributes()
        {
            _sourceDataGrid.Clear();

            var list = _source.AsEnumerable();

            var textName = txtBFilter.Text.Trim();

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.AttributeMetadata.MetadataId == tempGuid);
                }
                else
                {
                    list = list
                    .Where(ent =>
                        ent.LogicalName.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                        ||
                        (
                            ent.AttributeMetadata.DisplayName != null
                            && ent.AttributeMetadata.DisplayName.LocalizedLabels != null
                            && ent.AttributeMetadata.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )
                    );
                }
            }

            foreach (var item in list.OrderBy(a => a.LogicalName))
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

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            FillLastSolutionItems(_service.ConnectionData, items, true, AddAttributeToCrmSolutionLast_Click, "contMnAddToSolutionLast");
        }

        private AttributeSelectItem GetSelectedAttribute()
        {
            return this.lstVwAttributes.SelectedItems.OfType<AttributeSelectItem>().Count() == 1
                ? this.lstVwAttributes.SelectedItems.OfType<AttributeSelectItem>().SingleOrDefault() : null;
        }

        private List<AttributeSelectItem> GetSelectedAttributes()
        {
            List<AttributeSelectItem> result = this.lstVwAttributes.SelectedItems.OfType<AttributeSelectItem>().ToList();

            return result;
        }

        private void mIOpenAttributeInWeb_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            _service.ConnectionData.OpenAttributeMetadataInWeb(_entityMetadata.MetadataId.Value, attribute.AttributeMetadata.MetadataId.Value);
        }

        private async void AddAttributeToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddAttributeToSolution(true, null);
        }

        private async void AddAttributeToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddAttributeToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddAttributeToSolution(bool withSelect, string solutionUniqueName)
        {
            var attributeList = GetSelectedAttributes();

            if (attributeList == null || !attributeList.Any())
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, _service, null, commonConfig, solutionUniqueName, ComponentType.Attribute, attributeList.Select(item => item.AttributeMetadata.MetadataId.Value).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void mIAttributeOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , _service
                , commonConfig
                , (int)ComponentType.Attribute
                , attribute.AttributeMetadata.MetadataId.Value
                , null
            );
        }

        private void mIAttributeOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            _service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Attribute, attribute.AttributeMetadata.MetadataId.Value);
        }

        private void mIAttributeOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, _service, null, commonConfig, (int)ComponentType.Attribute, attribute.AttributeMetadata.MetadataId.Value, null);
        }

        public string GetAttributes()
        {
            if (!this._source.Any(a => a.IsChecked))
            {
                return string.Empty;
            }

            if (this._source.Count(a => a.IsChecked) == this._source.Count)
            {
                return string.Empty;
            }

            return string.Join(",", this._source.Where(a => a.IsChecked).OrderBy(a => a.LogicalName).Select(a => a.LogicalName));
        }

        private void SelectAllAttributesInDataGrid(IEnumerable<AttributeSelectItem> items, bool isSelected)
        {
            this.IncreaseInit();

            foreach (var item in items)
            {
                item.IsChecked = isSelected;
            }

            this.DecreaseInit();

            this.UpdateSelectedAttributesText();
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
            SelectAllAttributesInDataGrid(lstVwAttributes.Items.OfType<AttributeSelectItem>(), true);
        }

        private void hypLinkDeselectFiltered_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            SelectAllAttributesInDataGrid(lstVwAttributes.Items.OfType<AttributeSelectItem>(), false);
        }

        #region Clipboard

        private void mIClipboardCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<AttributeSelectItem>(e, ent => ent.LogicalName);
        }

        private void mIClipboardCopyDisplayName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<AttributeSelectItem>(e, ent => ent.DisplayName);
        }

        private void mIClipboardCopyAttributeType_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<AttributeSelectItem>(e, ent => ent.AttributeTypeName);
        }

        private void mIClipboardCopyAttributeId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<AttributeSelectItem>(e, ent => ent.AttributeMetadata.MetadataId.ToString());
        }

        #endregion Clipboard
    }
}