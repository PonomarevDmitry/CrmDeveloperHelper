using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class StringAttributeMetadataControl : UserControl, IAttributeMetadataControl<StringAttributeMetadata>
    {
        public StringAttributeMetadata AttributeMetadata { get; private set; }

        private readonly string _initialValue;

        public StringAttributeMetadataControl(StringAttributeMetadata attributeMetadata, string initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            if (string.IsNullOrEmpty(initialValue))
            {
                initialValue = string.Empty;
            }

            this._initialValue = initialValue;
            this.AttributeMetadata = attributeMetadata;
            txtBValue.Text = initialValue;
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

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetCurrentString();

            if (!string.Equals(currentValue, _initialValue))
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