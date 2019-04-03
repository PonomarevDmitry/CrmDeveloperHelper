using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class BigIntAttributeMetadataControl : UserControl, IAttributeMetadataControl<BigIntAttributeMetadata>
    {
        public BigIntAttributeMetadata AttributeMetadata { get; private set; }

        private readonly long? _initialValue;

        public BigIntAttributeMetadataControl(BigIntAttributeMetadata attributeMetadata, long? initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this.AttributeMetadata = attributeMetadata;
            txtBValue.Text = initialValue.ToString();
        }

        private void txtBValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentValue = GetLongValue();

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        private long? GetLongValue()
        {
            var value = txtBValue.Text.Trim();

            if (long.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetLongValue();

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