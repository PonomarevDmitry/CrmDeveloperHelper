using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class DateTimeAttributeMetadataControl : UserControl, IAttributeMetadataControl<DateTimeAttributeMetadata>
    {
        public DateTimeAttributeMetadata AttributeMetadata { get; private set; }

        private readonly DateTime? _initialValue;

        private readonly bool _fillAllways;

        public DateTimeAttributeMetadataControl(bool fillAllways, DateTimeAttributeMetadata attributeMetadata, DateTime? initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._fillAllways = fillAllways;
            this.AttributeMetadata = attributeMetadata;

            if (_initialValue.HasValue)
            {
                dPValue.SelectedDate = _initialValue.Value;
            }

            btnRemoveControl.IsEnabled = _fillAllways;

            btnRemoveControl.Visibility = btnRemoveControl.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            chBChanged.Visibility = _fillAllways ? Visibility.Collapsed : Visibility.Visible;

            btnRestore.IsEnabled = !_fillAllways;
            btnRestore.Visibility = btnRestore.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            dPValue.Focus();

            base.OnGotFocus(e);
        }

        private void DPValue_DateChanged(object sender, RoutedEventArgs e)
        {
            var currentValue = dPValue.SelectedDate;

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = dPValue.SelectedDate;

            if (this._fillAllways || currentValue != _initialValue)
            {
                entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
            }
        }

        public event EventHandler RemoveControlClicked;

        private void btnRemoveControl_Click(object sender, RoutedEventArgs e)
        {
            RemoveControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            dPValue.SelectedDate = null;
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (_initialValue.HasValue)
            {
                dPValue.SelectedDate = _initialValue.Value;
            }
        }
    }
}