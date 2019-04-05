using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class BooleanAttributeMetadataControl : UserControl, IAttributeMetadataControl<BooleanAttributeMetadata>
    {
        public BooleanAttributeMetadata AttributeMetadata { get; private set; }

        private readonly bool? _initialValue;

        private readonly bool _fillAllways;

        public BooleanAttributeMetadataControl(bool fillAllways, BooleanAttributeMetadata attributeMetadata, bool? initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._fillAllways = fillAllways;
            this.AttributeMetadata = attributeMetadata;

            FillComboBox();

            btnRemoveControl.IsEnabled = _fillAllways;
            chBChanged.IsEnabled = !_fillAllways;

            btnRemoveControl.Visibility = btnRemoveControl.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            chBChanged.Visibility = chBChanged.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void FillComboBox()
        {
            var falseItem = CreateComboBoxItem(AttributeMetadata.OptionSet.FalseOption, false);
            var trueItem = CreateComboBoxItem(AttributeMetadata.OptionSet.TrueOption, true);

            cmBValue.Items.Clear();
            cmBValue.Items.Add("<Null>");
            cmBValue.Items.Add(falseItem);
            cmBValue.Items.Add(trueItem);

            if (_initialValue.HasValue)
            {
                cmBValue.SelectedItem = _initialValue.Value ? trueItem : falseItem;
            }
            else
            {
                cmBValue.SelectedIndex = 0;
            }
        }

        private ComboBoxItem CreateComboBoxItem(OptionMetadata item, bool value)
        {
            StringBuilder name = new StringBuilder();

            name.Append(value);

            var label = CreateFileHandler.GetLocalizedLabel(item.Label);
            var description = CreateFileHandler.GetLocalizedLabel(item.Description);

            if (!string.IsNullOrEmpty(label))
            {
                name.AppendFormat(" - {0}", label);
            }
            else if (!string.IsNullOrEmpty(description))
            {
                name.AppendFormat(" - {0}", description);
            }

            var newItem = new ComboBoxItem()
            {
                Content = name.ToString(),
                Tag = value,
            };

            cmBValue.Items.Add(newItem);

            return newItem;
        }

        private void cmBValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentValue = GetBoolValue();

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        private bool? GetBoolValue()
        {
            if (cmBValue.SelectedItem is ComboBoxItem item
                && item.Tag != null
                && item.Tag is bool optionSetValue
            )
            {
                return optionSetValue;
            }

            return null;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetBoolValue();

            if (this._fillAllways || currentValue != _initialValue)
            {
                entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            cmBValue.Focus();

            base.OnGotFocus(e);
        }

        public event EventHandler RemoveControlClicked;

        private void btnRemoveControl_Click(object sender, RoutedEventArgs e)
        {
            RemoveControlClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}