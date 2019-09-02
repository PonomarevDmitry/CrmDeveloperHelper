using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public partial class EntityReferenceMappingControl : UserControl
    {
        private readonly IWriteToOutput _iWriteToOutput;

        private readonly IOrganizationServiceExtented _service;

        public LookupAttributeMetadata AttributeMetadata { get; }

        private readonly EntityReference _entityReferenceConnection1;

        private readonly EntityReference _entityReferenceConnection2;

        public EntityReference CurrentValue { get; private set; }

        public EntityReferenceMappingControl(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , LookupAttributeMetadata attributeMetadata
            , EntityReference entityReferenceConnection1
            , EntityReference entityReferenceConnection2
        )
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._iWriteToOutput = iWriteToOutput;
            this._service = service;

            this._entityReferenceConnection1 = entityReferenceConnection1;
            this._entityReferenceConnection2 = entityReferenceConnection2;

            this.AttributeMetadata = attributeMetadata;

            txtBEntityReferenceConnection1EntityName.Text = this._entityReferenceConnection1.LogicalName;
            txtBEntityReferenceConnection1EntityId.Text = this._entityReferenceConnection1.Id.ToString();

            if (!string.IsNullOrEmpty(this._entityReferenceConnection1.Name))
            {
                txtBEntityReferenceConnection1Name.Text = this._entityReferenceConnection1.Name;
            }
            else
            {
                txtBEntityReferenceConnection1Name.IsEnabled = false;
                txtBEntityReferenceConnection1Name.Visibility = Visibility.Collapsed;
            }

            btnRestore.IsEnabled = _entityReferenceConnection2 != null;
            btnRestore.Visibility = btnRestore.IsEnabled ? Visibility.Visible : Visibility.Collapsed;

            SetEntityReference(entityReferenceConnection2);
        }

        private void SetEntityReference(EntityReference entityReferenceValue)
        {
            this.CurrentValue = entityReferenceValue;

            if (CurrentValue == null)
            {
                txtBCurrentValue.Text = string.Empty;
            }
            else if (!string.IsNullOrEmpty(CurrentValue.Name))
            {
                txtBCurrentValue.Text = CurrentValue.Name;
            }
            else
            {
                txtBCurrentValue.Text = string.Format("{0} - {1}", CurrentValue.LogicalName, CurrentValue.Id.ToString());
            }

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            var hasValue = this.CurrentValue != null;

            var separatorVisible = btnRestore.IsEnabled && hasValue;

            sepCopy.IsEnabled = separatorVisible;
            sepCopy.Visibility = separatorVisible ? Visibility.Visible : Visibility.Collapsed;

            btnCopyEntityId.IsEnabled
                = btnCopyEntityLogicalName.IsEnabled
                = btnCopyEntityName.IsEnabled
                = btnCopyEntityUrl.IsEnabled = hasValue;


            btnCopyEntityId.Visibility
                = btnCopyEntityLogicalName.Visibility
                = btnCopyEntityName.Visibility
                = btnCopyEntityUrl.Visibility = hasValue ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            txtBCurrentValue.Focus();

            base.OnGotFocus(e);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            SetEntityReference(null);
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            SetEntityReference(this._entityReferenceConnection2);
        }

        private void btnSetValue_Click(object sender, RoutedEventArgs e)
        {
            var form = new WindowSelectEntityReference(this._iWriteToOutput, _service, new[] { _entityReferenceConnection1.LogicalName });

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            SetEntityReference(form.SelectedEntityReference);
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
            if (this.CurrentValue == null)
            {
                return;
            }

            Clipboard.SetText(this.CurrentValue.LogicalName);
        }

        private void btnCopyEntityName_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentValue == null)
            {
                return;
            }

            Clipboard.SetText(this.CurrentValue.Name);
        }

        private void btnCopyEntityId_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentValue == null)
            {
                return;
            }

            Clipboard.SetText(this.CurrentValue.Id.ToString());
        }

        private void btnCopyEntityUrl_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentValue == null)
            {
                return;
            }

            var url = _service.ConnectionData.GetEntityInstanceUrl(this.CurrentValue.LogicalName, this.CurrentValue.Id);

            Clipboard.SetText(url);
        }
    }
}