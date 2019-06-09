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
    public partial class WindowAttributeSelect : WindowBase
    {
        private readonly IWriteToOutput _iWriteToOutput;

        private readonly IOrganizationServiceExtented _service;

        private readonly Guid _entityMetadataId;

        private readonly List<AttributeSelectItem> _source = new List<AttributeSelectItem>();

        private readonly ObservableCollection<AttributeSelectItem> _sourceDataGrid = new ObservableCollection<AttributeSelectItem>();

        public AttributeMetadata SelectedAttributeMetadata { get; private set; }

        public WindowAttributeSelect(
            IWriteToOutput outputWindow
            , IOrganizationServiceExtented service
            , Guid entityMetadataId
            , IEnumerable<AttributeMetadata> attributeList
        )
        {
            this.IncreaseInit();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = outputWindow;
            this._service = service;
            this._entityMetadataId = entityMetadataId;

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            foreach (var attr in attributeList.Where(a => string.IsNullOrEmpty(a.AttributeOf)).OrderBy(a => a.LogicalName))
            {
                _source.Add(new AttributeSelectItem(attr));
            }

            foreach (var item in _source.OrderBy(a => a.LogicalName))
            {
                _sourceDataGrid.Add(item);
            }

            lstVwAttributes.ItemsSource = _sourceDataGrid;

            this.DecreaseInit();

            txtBFilter.SelectionStart = txtBFilter.Text.Length;
            txtBFilter.SelectionLength = 0;
            txtBFilter.Focus();
        }

        public class AttributeSelectItem
        {
            public AttributeMetadata AttributeMetadata { get; private set; }

            public string LogicalName => AttributeMetadata.LogicalName;

            public string DisplayName { get; private set; }

            public string AttributeTypeName => AttributeMetadata.AttributeType.ToString();

            public AttributeSelectItem(AttributeMetadata attributeMetadata)
            {
                this.AttributeMetadata = attributeMetadata;

                this.DisplayName = Helpers.CreateFileHandler.GetLocalizedLabel(attributeMetadata.DisplayName);
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
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar);

            UpdateButtonsEnable();
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
                        || (
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

            _service.ConnectionData.OpenAttributeMetadataInWeb(_entityMetadataId, attribute.AttributeMetadata.MetadataId.Value);
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

        private AttributeSelectItem GetSelectedAttributeMetadata()
        {
            return this.lstVwAttributes.SelectedItems.OfType<AttributeSelectItem>().Count() == 1
                ? this.lstVwAttributes.SelectedItems.OfType<AttributeSelectItem>().SingleOrDefault() : null;
        }

        private void btnSelectAttribute_Click(object sender, RoutedEventArgs e)
        {
            var attributeMetadataItem = GetSelectedAttributeMetadata();

            if (attributeMetadataItem == null
                || attributeMetadataItem.AttributeMetadata == null
            )
            {
                return;
            }

            SelectAttributeMetadataAction(attributeMetadataItem.AttributeMetadata);
        }

        private void SelectAttributeMetadataAction(AttributeMetadata attributeMetadata)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (attributeMetadata == null)
            {
                return;
            }

            this.SelectedAttributeMetadata = attributeMetadata;

            this.DialogResult = true;

            this.Close();
        }

        private void lstVwAttributes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (((FrameworkElement)e.OriginalSource).DataContext is AttributeSelectItem item)
                {
                    SelectAttributeMetadataAction(item.AttributeMetadata);
                }
            }
        }

        private void lstVwAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        protected void UpdateButtonsEnable()
        {
            this.lstVwAttributes.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = IsControlsEnabled && this.lstVwAttributes.SelectedItems.Count > 0;

                    UIElement[] list = { btnSelectAttribute };

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
    }
}