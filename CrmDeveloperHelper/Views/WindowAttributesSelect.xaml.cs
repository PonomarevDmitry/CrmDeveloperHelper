using Microsoft.Xrm.Sdk;
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
    public partial class WindowAttributesSelect : WindowBase
    {
        private IWriteToOutput _iWriteToOutput;

        private bool _controlsEnabled = true;

        private readonly EntityMetadata _entityMetadata;

        private readonly IOrganizationServiceExtented _service;

        private List<AttributeSelectItem> _source = new List<AttributeSelectItem>();

        private ObservableCollection<AttributeSelectItem> _sourceDataGrid = new ObservableCollection<AttributeSelectItem>();

        public WindowAttributesSelect(
            IWriteToOutput outputWindow
            , IOrganizationServiceExtented service
            , EntityMetadata entityMetadata
            , string selectedAttributes
        )
        {
            InitializeComponent();

            lstVwAttributes.ItemsSource = _sourceDataGrid;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = outputWindow;
            this._service = service;
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
                _source.Add(new AttributeSelectItem(attr)
                {
                    IsSelected = selectedAttributesColl.Contains(attr.LogicalName),
                });
            }

            chBAllAttributes.IsChecked = !_source.Any(a => a.IsSelected);

            foreach (var item in _source.OrderBy(a => a.LogicalName))
            {
                _sourceDataGrid.Add(item);
            }

            txtBFilter.SelectionStart = txtBFilter.Text.Length;
            txtBFilter.SelectionLength = 0;
            txtBFilter.Focus();
        }

        public class AttributeSelectItem : INotifyPropertyChanging, INotifyPropertyChanged
        {
            public AttributeMetadata AttributeMetadata { get; private set; }

            public string LogicalName => AttributeMetadata.LogicalName;

            public string DisplayName { get; private set; }

            public string AttributeTypeName => AttributeMetadata.AttributeType.ToString();

            private bool _IsSelected = false;
            public bool IsSelected
            {
                get => _IsSelected;
                set
                {
                    if (_IsSelected == value)
                    {
                        return;
                    }

                    this.OnPropertyChanging(nameof(IsSelected));
                    this._IsSelected = value;
                    this.OnPropertyChanged(nameof(IsSelected));
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

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(enabled, this.tSProgressBar);
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
                        ent.LogicalName.ToLower().Contains(textName)
                        || (
                        ent.AttributeMetadata.DisplayName != null
                        && ent.AttributeMetadata.DisplayName.LocalizedLabels != null
                        && ent.AttributeMetadata.DisplayName.LocalizedLabels
                            .Where(l => !string.IsNullOrEmpty(l.Label))
                            .Any(lbl => lbl.Label.ToLower().Contains(textName))
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

            FillLastSolutionItems(_service.ConnectionData, items, true, AddAttributeIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");
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

        private async void AddAttributeIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddAttributeIntoSolution(true, null);
        }

        private async void AddAttributeIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddAttributeIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddAttributeIntoSolution(bool withSelect, string solutionUniqueName)
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

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, _service, null, commonConfig, solutionUniqueName, ComponentType.Attribute, attributeList.Select(item => item.AttributeMetadata.MetadataId.Value).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void mIAttributeOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenExplorerSolutionWindow(
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

        private void mIAttributeOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            var commonConfig = CommonConfiguration.Get();

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, _service, null, commonConfig, (int)ComponentType.Attribute, attribute.AttributeMetadata.MetadataId.Value, null);
        }

        public string GetAttributes()
        {
            if (chBAllAttributes.IsChecked.GetValueOrDefault())
            {
                return string.Empty;
            }

            if (this._source.Count(a => a.IsSelected) == this._source.Count)
            {
                return string.Empty;
            }

            return string.Join(",", this._source.Where(a => a.IsSelected).OrderBy(a => a.LogicalName).Select(a => a.LogicalName));
        }

        private void chBAllAttributes_Checked(object sender, RoutedEventArgs e)
        {
            bool allAttributes = chBAllAttributes.IsChecked.GetValueOrDefault();

            lstVwAttributes.IsReadOnly = allAttributes;

            btnSelectAll.IsEnabled = !allAttributes;
            btnDeselectAll.IsEnabled = !allAttributes;
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in _sourceDataGrid)
            {
                item.IsSelected = true;
            }
        }

        private void btnDeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in _sourceDataGrid)
            {
                item.IsSelected = false;
            }
        }
    }
}