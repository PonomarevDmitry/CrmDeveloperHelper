using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class DoubleAttributeMetadataControl : UserControl, IAttributeMetadataControl<DoubleAttributeMetadata>
    {
        public DoubleAttributeMetadata AttributeMetadata { get; private set; }

        private readonly double? _initialValue;

        public DoubleAttributeMetadataControl(DoubleAttributeMetadata attributeMetadata, double? initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this.AttributeMetadata = attributeMetadata;
            txtBValue.Text = initialValue.ToString();
        }

        private void txtBValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentValue = GetDoubleValue();

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        private double? GetDoubleValue()
        {
            var value = txtBValue.Text.Trim();

            if (double.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetDoubleValue();

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