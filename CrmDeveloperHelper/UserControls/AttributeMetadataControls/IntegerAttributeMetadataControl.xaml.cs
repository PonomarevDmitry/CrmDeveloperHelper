using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class IntegerAttributeMetadataControl : UserControl, IAttributeMetadataControl<IntegerAttributeMetadata>
    {
        public IntegerAttributeMetadata AttributeMetadata { get; private set; }

        private readonly int? _initialValue;

        public IntegerAttributeMetadataControl(IntegerAttributeMetadata attributeMetadata, int? initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this.AttributeMetadata = attributeMetadata;

            txtBValue.Text = initialValue.ToString();
        }

        private void txtBValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentValue = GetIntValue();

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        private int? GetIntValue()
        {
            var value = txtBValue.Text.Trim();

            if (int.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetIntValue();

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