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

        public UniqueIdentifierAttributeMetadataControl(AttributeMetadata attributeMetadata, Guid? initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this.AttributeMetadata = attributeMetadata;

            txtBValue.Text = initialValue.ToString();
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

            if (currentValue != _initialValue)
            {
                entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            txtBValue.Focus();

            base.OnGotFocus(e);
        }
    }
}