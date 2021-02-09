using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class EntityNameAttributeMetadataControl : UserControl, IAttributeMetadataControl<EntityNameAttributeMetadata>
    {
        private readonly ConnectionData _connectionData;

        private readonly string _initialValue;

        private readonly bool _allwaysAddToEntity;

        public EntityNameAttributeMetadata AttributeMetadata { get; }

        public EntityNameAttributeMetadataControl(ConnectionData connectionData, EntityNameAttributeMetadata attributeMetadata, string initialValue, bool allwaysAddToEntity, bool showRestoreButton)
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(initialValue))
            {
                initialValue = null;
            }

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._initialValue = initialValue;
            this._allwaysAddToEntity = allwaysAddToEntity;
            this._connectionData = connectionData;

            this.AttributeMetadata = attributeMetadata;

            FillComboBox();

            Views.WindowBase.SetElementsEnabled(allwaysAddToEntity, btnRemoveControl);

            Views.WindowBase.SetElementsEnabled(showRestoreButton, btnRestore, chBChanged);
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

            if (_connectionData.IsValidEntityName(_initialValue))
            {
                currentItem = new ComboBoxItem()
                {
                    Content = _initialValue,
                    Tag = _initialValue,
                };

                cmBValue.Items.Add(currentItem);
            }

            if (_connectionData.EntitiesIntellisenseData != null
                && _connectionData.EntitiesIntellisenseData.Entities != null
            )
            {
                var entityArray = _connectionData.EntitiesIntellisenseData.Entities.Values.ToList().OrderBy(e => e.IsIntersectEntity).ThenBy(e => e.EntityLogicalName).ToArray();

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

            if (this._allwaysAddToEntity || !string.Equals(currentValue, _initialValue, StringComparison.InvariantCultureIgnoreCase))
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
