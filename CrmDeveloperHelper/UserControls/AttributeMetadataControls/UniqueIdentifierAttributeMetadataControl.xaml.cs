using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class UniqueIdentifierAttributeMetadataControl : UserControl, IAttributeMetadataControl<AttributeMetadata>
    {
        public AttributeMetadata AttributeMetadata { get; private set; }

        private readonly Guid? _initialValue;

        private readonly bool _fillAllways;

        public UniqueIdentifierAttributeMetadataControl(bool fillAllways, AttributeMetadata attributeMetadata, Guid? initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._fillAllways = fillAllways;
            this.AttributeMetadata = attributeMetadata;

            txtBValue.Text = _initialValue.ToString();

            btnRemoveControl.IsEnabled = _fillAllways;

            btnRemoveControl.Visibility = btnRemoveControl.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            chBChanged.Visibility = _fillAllways ? Visibility.Collapsed : Visibility.Visible;

            btnRestore.IsEnabled = !_fillAllways;
            btnRestore.Visibility = btnRestore.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtBValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentValue = GetGuidValue();

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        private Guid? GetGuidValue()
        {
            var value = txtBValue.Text.Trim();

            if (Guid.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetGuidValue();

            if (this._fillAllways || currentValue != _initialValue)
            {
                entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            txtBValue.Focus();

            base.OnGotFocus(e);
        }

        public event EventHandler RemoveControlClicked;

        private void btnRemoveControl_Click(object sender, RoutedEventArgs e)
        {
            RemoveControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            txtBValue.Text = _initialValue.ToString();
        }
    }
}