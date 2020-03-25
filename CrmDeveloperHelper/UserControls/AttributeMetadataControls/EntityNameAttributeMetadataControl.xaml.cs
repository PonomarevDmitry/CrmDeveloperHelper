using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class EntityNameAttributeMetadataControl : UserControl, IAttributeMetadataControl<EntityNameAttributeMetadata>
    {
        private readonly IOrganizationServiceExtented _service;

        private readonly string _initialValue;

        private readonly bool _fillAllways;

        public EntityNameAttributeMetadata AttributeMetadata { get; }

        public EntityNameAttributeMetadataControl(IOrganizationServiceExtented service, bool fillAllways, EntityNameAttributeMetadata attributeMetadata, string initialValue)
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(initialValue))
            {
                initialValue = null;
            }

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._fillAllways = fillAllways;
            this._service = service;

            this.AttributeMetadata = attributeMetadata;

            FillComboBox();

            btnRemoveControl.IsEnabled = _fillAllways;

            btnRemoveControl.Visibility = btnRemoveControl.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            chBChanged.Visibility = _fillAllways ? Visibility.Collapsed : Visibility.Visible;

            btnRestore.IsEnabled = !_fillAllways;
            btnRestore.Visibility = btnRestore.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void FillComboBox()
        {
            cmBValue.Items.Clear();
            cmBValue.Items.Add("<Null>");

            var noneItem = new ComboBoxItem()
            {
                Content = "none - 0",
                Tag = "none",
            };

            cmBValue.Items.Add(noneItem);

            ComboBoxItem currentItem = null;

            if (string.Equals(_initialValue, "none", StringComparison.InvariantCultureIgnoreCase))
            {
                currentItem = noneItem;
            }

            if (_service.ConnectionData.IsValidEntityName(_initialValue))
            {
                currentItem = new ComboBoxItem()
                {
                    Content = _initialValue,
                    Tag = _initialValue,
                };

                cmBValue.Items.Add(currentItem);
            }

            if (_service.ConnectionData.IntellisenseData != null
                && _service.ConnectionData.IntellisenseData.Entities != null
            )
            {
                var entityArray = _service.ConnectionData.IntellisenseData.Entities.Values.ToList().OrderBy(e => e.IsIntersectEntity).ThenBy(e => e.EntityLogicalName).ToArray();

                foreach (var entityData in entityArray)
                {
                    var newItem = new ComboBoxItem()
                    {
                        Content = $"{entityData.EntityLogicalName} - {entityData.ObjectTypeCode}",
                        Tag = entityData.EntityLogicalName,
                    };

                    cmBValue.Items.Add(newItem);

                    if (string.Equals(entityData.EntityLogicalName, _initialValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        currentItem = newItem;
                    }
                }
            }

            if (currentItem != null)
            {
                cmBValue.SelectedItem = currentItem;
            }
            else
            {
                cmBValue.SelectedIndex = 0;
            }
        }

        private void cmBValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentValue = GetEntitNameValue();

            chBChanged.IsChecked = !string.Equals(currentValue, _initialValue, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetEntitNameValue()
        {
            if (cmBValue.SelectedItem is ComboBoxItem item
                && item.Tag != null
                && item.Tag is string entityName
            )
            {
                return entityName;
            }

            return null;
        }

        public void AddAttribute(Entity entity)
        {
            var currentValue = GetEntitNameValue();

            entity.Attributes[AttributeMetadata.LogicalName] = currentValue;
        }

        public void AddChangedAttribute(Entity entity)
        {
            var currentValue = GetEntitNameValue();

            if (this._fillAllways || !string.Equals(currentValue, _initialValue, StringComparison.InvariantCultureIgnoreCase))
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
            if (!string.IsNullOrEmpty(_initialValue))
            {
                foreach (var item in cmBValue.Items.OfType<ComboBoxItem>())
                {
                    if (item.Tag != null
                        && item.Tag is string entityName
                        && string.Equals(entityName, _initialValue, StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        cmBValue.SelectedItem = item;
                        return;
                    }
                }
            }
            else
            {
                cmBValue.SelectedIndex = 0;
            }
        }
    }
}