using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class BooleanManagedPropertyAttributeMetadataControl : UserControl, IAttributeMetadataControl<ManagedPropertyAttributeMetadata>
    {
        public ManagedPropertyAttributeMetadata AttributeMetadata { get; }

        private readonly BooleanManagedProperty _initialValue;

        private readonly bool _allwaysAddToEntity;

        public BooleanManagedPropertyAttributeMetadataControl(ManagedPropertyAttributeMetadata attributeMetadata, BooleanManagedProperty initialValue, bool allwaysAddToEntity, bool showRestoreButton)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._allwaysAddToEntity = allwaysAddToEntity;
            this.AttributeMetadata = attributeMetadata;

            FillComboBox();

            Views.WindowBase.SetElementsEnabledAndVisible(allwaysAddToEntity, btnRemoveControl);

            Views.WindowBase.SetElementsEnabledAndVisible(showRestoreButton, btnRestore);

            Views.WindowBase.SetElementsVisible(showRestoreButton, chBChanged);
        }

        private void FillComboBox()
        {
            cmBValue.Items.Clear();
            cmBValue.Items.Add("<Null>");
            cmBValue.Items.Add(false);
            cmBValue.Items.Add(true);

            if (_initialValue != null)
            {
                cmBValue.SelectedItem = _initialValue.Value;
                chBCanBeChanged.IsChecked = _initialValue.CanBeChanged;
            }
            else
            {
                cmBValue.SelectedIndex = 0;
                chBCanBeChanged.IsChecked = false;
            }
        }

        private void cmBValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChangedProperty();
        }

        private void ChBCanBeChanged_Checked(object sender, RoutedEventArgs e)
        {
            UpdateChangedProperty();
        }

        private void UpdateChangedProperty()
        {
            var currentValue = GetBooleanManagedPropertyValue();

            chBChanged.IsChecked = !IsEqualBooleanManagedProperty(currentValue, _initialValue);
        }

        private bool IsEqualBooleanManagedProperty(BooleanManagedProperty x, BooleanManagedProperty y)
        {
            if (x == null && y != null)
            {
                return false;
            }
            else if (x != null && y == null)
            {
                return false;
            }
            else if (x != null && y != null)
            {
                return x.Value == y.Value && x.CanBeChanged == y.CanBeChanged;
            }

            return true;
        }

        private BooleanManagedProperty GetBooleanManagedPropertyValue()
        {
            if (cmBValue.SelectedItem is bool item)
            {
                return new BooleanManagedProperty(item)
                {
                    CanBeChanged = chBCanBeChanged.IsChecked.GetValueOrDefault(),
                };
            }

            return null;
        }

        public void AddAttribute(Entity entity)
        {
            var currentValue = GetBooleanManagedPropertyValue();

            entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetBooleanManagedPropertyValue();

            if (this._allwaysAddToEntity
                || !IsEqualBooleanManagedProperty(currentValue, _initialValue)
            )
            {
                entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            cmBValue.Focus();

            base.OnGotFocus(e);
        }

        public event EventHandler RemoveControlClicked;

        private void btnRemoveControl_Click(object sender, RoutedEventArgs e)
        {
            RemoveControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (_initialValue != null)
            {
                cmBValue.SelectedItem = _initialValue.Value;
                chBCanBeChanged.IsChecked = _initialValue.CanBeChanged;
            }
            else
            {
                cmBValue.SelectedIndex = 0;
                chBCanBeChanged.IsChecked = false;
            }
        }
    }
}