using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class DateTimeAttributeMetadataControl : UserControl, IAttributeMetadataControl<DateTimeAttributeMetadata>
    {
        public DateTimeAttributeMetadata AttributeMetadata { get; }

        private readonly DateTime? _initialValue;

        private readonly bool _allwaysAddToEntity;

        public DateTimeAttributeMetadataControl(DateTimeAttributeMetadata attributeMetadata, DateTime? initialValue, bool allwaysAddToEntity, bool showRestoreButton)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._allwaysAddToEntity = allwaysAddToEntity;
            this.AttributeMetadata = attributeMetadata;

            if (_initialValue.HasValue)
            {
                dPValue.SelectedDate = _initialValue.Value;
            }

            Views.WindowBase.SetElementsEnabled(allwaysAddToEntity, btnRemoveControl);

            Views.WindowBase.SetElementsEnabled(showRestoreButton, btnRestore, chBChanged);
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

        public void AddAttribute(Entity entity)
        {
            var currentValue = dPValue.SelectedDate;

            entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = dPValue.SelectedDate;

            if (this._allwaysAddToEntity || currentValue != _initialValue)
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