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

        public DateTimeAttributeMetadataControl(DateTimeAttributeMetadata attributeMetadata, DateTime? initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this.AttributeMetadata = attributeMetadata;
            dPValue.SelectedDate = initialValue;
        }

        private void dPValue_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentValue = dPValue.SelectedDate;

            chBChanged.IsChecked = currentValue != _initialValue;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = dPValue.SelectedDate;

            if (currentValue != _initialValue)
            {
                entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            dPValue.Focus();

            base.OnGotFocus(e);
        }
    }
}