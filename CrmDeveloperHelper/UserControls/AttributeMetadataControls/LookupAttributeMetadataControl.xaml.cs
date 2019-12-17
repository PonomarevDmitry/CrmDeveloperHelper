using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class LookupAttributeMetadataControl : UserControl, IAttributeMetadataControl<LookupAttributeMetadata>
    {
        private readonly IWriteToOutput _iWriteToOutput;
        private readonly IOrganizationServiceExtented _service;

        public LookupAttributeMetadata AttributeMetadata { get; }

        private readonly EntityReference _initialValue;

        private readonly bool _fillAllways;

        private EntityReference currentValue;

        public LookupAttributeMetadataControl(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, bool fillAllways, LookupAttributeMetadata attributeMetadata, EntityReference initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._service = service;
            this._iWriteToOutput = iWriteToOutput;

            this._initialValue = initialValue;
            this._fillAllways = fillAllways;
            this.AttributeMetadata = attributeMetadata;

            SetEntityReference(_initialValue);

            btnRemoveControl.IsEnabled = _fillAllways;

            btnRemoveControl.Visibility = btnRemoveControl.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            chBChanged.Visibility = _fillAllways ? Visibility.Collapsed : Visibility.Visible;

            btnRestore.IsEnabled = !_fillAllways;
            btnRestore.Visibility = btnRestore.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetEntityReference(EntityReference entityReferenceValue)
        {
            this.currentValue = entityReferenceValue;

            chBChanged.IsChecked = !IsEntityReferenceEquals(this.currentValue, _initialValue);

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

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            var hasValue = this.currentValue != null;

            var separatorVisible = btnRestore.IsEnabled && hasValue;

            sepCopy.IsEnabled = separatorVisible;
            sepCopy.Visibility = separatorVisible ? Visibility.Visible : Visibility.Collapsed;

            btnOpenEntityInWeb.IsEnabled
                = btnCopyEntityId.IsEnabled
                = btnCopyEntityLogicalName.IsEnabled
                = btnCopyEntityName.IsEnabled
                = btnCopyEntityUrl.IsEnabled = hasValue;

            btnOpenEntityInWeb.Visibility
                = btnCopyEntityId.Visibility
                = btnCopyEntityLogicalName.Visibility
                = btnCopyEntityName.Visibility
                = btnCopyEntityUrl.Visibility = hasValue ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool IsEntityReferenceEquals(EntityReference entityReference1, EntityReference entityReference2)
        {
            if (entityReference1 == null
                && entityReference2 == null
            )
            {
                return true;
            }
            else if (entityReference1 != null
                && entityReference2 == null
            )
            {
                return false;
            }
            else if (entityReference1 == null
                && entityReference2 != null
            )
            {
                return false;
            }
            else if (entityReference1 != null
                && entityReference2 != null
            )
            {
                return string.Equals(entityReference1.LogicalName, entityReference2.LogicalName, StringComparison.InvariantCultureIgnoreCase)
                    && entityReference1.Id == entityReference2.Id
                    ;
            }

            return false;
        }

        public void AddAttribute(Entity entity)
        {
            entity.Attributes[AttributeMetadata.LogicalName] = this.currentValue;
        }

        public void AddChangedAttribute(Entity entity)
        {
            if (_fillAllways || !IsEntityReferenceEquals(this.currentValue, _initialValue))
            {
                entity.Attributes[AttributeMetadata.LogicalName] = this.currentValue;
            }
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
            var targets = AttributeMetadata.Targets.OrderBy(s => s).ToList();

            var form = new WindowSelectEntityReference(this._iWriteToOutput, _service, targets);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            SetEntityReference(form.SelectedEntityReference);
        }

        public event EventHandler RemoveControlClicked;

        private void btnRemoveControl_Click(object sender, RoutedEventArgs e)
        {
            RemoveControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            SetEntityReference(_initialValue);
        }

        private void btnActions_Click(object sender, RoutedEventArgs e)
        {
            if (btnActions.ContextMenu == null)
            {
                return;
            }

            //btnActions.ContextMenu.Width = btnActions.ActualWidth;

            btnActions.ContextMenu.PlacementTarget = btnActions;
            btnActions.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            ContextMenuService.SetPlacement(btnActions, System.Windows.Controls.Primitives.PlacementMode.Bottom);

            btnActions.ContextMenu.IsOpen = true;
        }

        private void btnCopyEntityLogicalName_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentValue == null)
            {
                return;
            }

            ClipboardHelper.SetText(this.currentValue.LogicalName);
        }

        private void btnCopyEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentValue == null)
            {
                return;
            }

            ClipboardHelper.SetText(this.currentValue.Name);
        }

        private void btnCopyEntityId_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentValue == null)
            {
                return;
            }

            ClipboardHelper.SetText(this.currentValue.Id.ToString());
        }

        private void btnCopyEntityUrl_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentValue == null)
            {
                return;
            }

            var url = _service.ConnectionData.GetEntityInstanceUrl(this.currentValue.LogicalName, this.currentValue.Id);

            ClipboardHelper.SetText(url);
        }

        private void btnOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentValue == null)
            {
                return;
            }

            _service.ConnectionData.OpenEntityInstanceInWeb(this.currentValue.LogicalName, this.currentValue.Id);
        }
    }
}