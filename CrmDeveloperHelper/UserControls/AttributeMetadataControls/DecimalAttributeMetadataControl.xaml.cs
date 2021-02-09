using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class DecimalAttributeMetadataControl : UserControl, IAttributeMetadataControl<DecimalAttributeMetadata>
    {
        public DecimalAttributeMetadata AttributeMetadata { get; }

        private readonly decimal? _initialValue;

        private readonly bool _allwaysAddToEntity;

        public DecimalAttributeMetadataControl(DecimalAttributeMetadata attributeMetadata, decimal? initialValue, bool allwaysAddToEntity, bool showRestoreButton)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._allwaysAddToEntity = allwaysAddToEntity;
            this.AttributeMetadata = attributeMetadata;

            txtBValue.Text = _initialValue.ToString();

            Views.WindowBase.SetElementsEnabled(allwaysAddToEntity, btnRemoveControl);

            Views.WindowBase.SetElementsEnabled(showRestoreButton, btnRestore, chBChanged);
        }

        private void txtBValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentValue = GetDecimalValue();

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        private decimal? GetDecimalValue()
        {
            var value = txtBValue.Text.Trim();

            if (decimal.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }

        public void AddAttribute(Entity entity)
        {
            var currentValue = GetDecimalValue();

            entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetDecimalValue();

            if (this._allwaysAddToEntity || currentValue != _initialValue)
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