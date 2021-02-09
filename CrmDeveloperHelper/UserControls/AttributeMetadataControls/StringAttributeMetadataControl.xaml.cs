using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class StringAttributeMetadataControl : UserControl, IAttributeMetadataControl<StringAttributeMetadata>
    {
        public StringAttributeMetadata AttributeMetadata { get; }

        private readonly string _initialValue;

        private readonly bool _allwaysAddToEntity;

        public StringAttributeMetadataControl(StringAttributeMetadata attributeMetadata, string initialValue, bool allwaysAddToEntity, bool showRestoreButton)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            if (string.IsNullOrEmpty(initialValue))
            {
                initialValue = string.Empty;
            }

            this._initialValue = initialValue;
            this._allwaysAddToEntity = allwaysAddToEntity;
            this.AttributeMetadata = attributeMetadata;

            txtBValue.Text = _initialValue;

            Views.WindowBase.SetElementsEnabled(allwaysAddToEntity, btnRemoveControl);

            Views.WindowBase.SetElementsEnabled(showRestoreButton, btnRestore, chBChanged);
        }

        private void txtBValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentValue = GetCurrentString();

            chBChanged.IsChecked = !string.Equals(currentValue, _initialValue);
        }

        private object GetCurrentString()
        {
            var currentValue = txtBValue.Text;

            if (string.IsNullOrEmpty(currentValue))
            {
                currentValue = string.Empty;
            }

            return currentValue;
        }

        public void AddAttribute(Entity entity)
        {
            var currentValue = GetCurrentString();

            entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetCurrentString();

            if (this._allwaysAddToEntity || !string.Equals(currentValue, _initialValue))
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
            txtBValue.Text = _initialValue;
        }
    }
}