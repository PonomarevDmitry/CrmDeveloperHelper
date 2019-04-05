using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class LookupAttributeMetadataControl : UserControl, IAttributeMetadataControl<LookupAttributeMetadata>
    {
        public LookupAttributeMetadata AttributeMetadata { get; private set; }

        private readonly EntityReference _initialValue;

        private readonly bool _fillAllways;

        private EntityReference currentValue;

        public LookupAttributeMetadataControl(bool fillAllways, LookupAttributeMetadata attributeMetadata, EntityReference initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._fillAllways = fillAllways;
            this.AttributeMetadata = attributeMetadata;

            SetEntityReference(_initialValue);

            btnRemoveControl.IsEnabled = _fillAllways;
            chBChanged.IsEnabled = !_fillAllways;

            btnRemoveControl.Visibility = btnRemoveControl.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            chBChanged.Visibility = chBChanged.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetEntityReference(EntityReference entityReferenceValue)
        {
            this.currentValue = entityReferenceValue;

            if (currentValue == null)
            {
                txtBReferenceName.Text = string.Empty;
            }
            else if (!string.IsNullOrEmpty(currentValue.Name))
            {
                txtBReferenceName.Text = currentValue.Name;
            }
            else
            {
                txtBReferenceName.Text = string.Format("{0} - {1}", currentValue.LogicalName, currentValue.Id.ToString());
            }
        }

        public void AddChangedAttribute(Entity entity)
        {

        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            txtBReferenceName.Focus();

            base.OnGotFocus(e);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            SetEntityReference(null);
        }

        private void btnSetValue_Click(object sender, RoutedEventArgs e)
        {

        }

        public event EventHandler RemoveControlClicked;

        private void btnRemoveControl_Click(object sender, RoutedEventArgs e)
        {
            RemoveControlClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}