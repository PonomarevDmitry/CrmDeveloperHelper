using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class MoneyAttributeMetadataControl : UserControl, IAttributeMetadataControl<MoneyAttributeMetadata>
    {
        public MoneyAttributeMetadata AttributeMetadata { get; }

        private readonly decimal? _initialValue;

        private readonly bool _allwaysAddToEntity;

        public MoneyAttributeMetadataControl(MoneyAttributeMetadata attributeMetadata, Money initialValue, bool allwaysAddToEntity, bool showRestoreButton)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue?.Value;
            this._allwaysAddToEntity = allwaysAddToEntity;
            this.AttributeMetadata = attributeMetadata;

            txtBValue.Text = this._initialValue.ToString();

            Views.WindowBase.SetElementsEnabledAndVisible(allwaysAddToEntity, btnRemoveControl);

            Views.WindowBase.SetElementsEnabledAndVisible(showRestoreButton, btnRestore);

            Views.WindowBase.SetElementsVisible(showRestoreButton, chBChanged);
        }

        private void txtBValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentValue = GetDecimalValue();

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        private int? GetDecimalValue()
        {
            var value = txtBValue.Text.Trim();

            if (int.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }

        public void AddAttribute(Entity entity)
        {
            var currentValue = GetDecimalValue();

            if (currentValue.HasValue)
            {
                entity.Attributes[AttributeMetadata.LogicalName] = new Money(currentValue.Value);
            }
            else
            {
                entity.Attributes[AttributeMetadata.LogicalName] = null;
            }
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetDecimalValue();

            if (this._allwaysAddToEntity || currentValue != _initialValue)
            {
                if (currentValue.HasValue)
                {
                    entity.Attributes[AttributeMetadata.LogicalName] = new Money(currentValue.Value);
                }
                else
                {
                    entity.Attributes[AttributeMetadata.LogicalName] = null;
                }
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