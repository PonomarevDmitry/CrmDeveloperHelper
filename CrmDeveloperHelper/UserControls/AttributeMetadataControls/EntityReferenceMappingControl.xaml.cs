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
        private readonly IOrganizationServiceExtented _service;

        public LookupAttributeMetadata AttributeMetadata { get; }

        private readonly EntityReference _entityReferenceConnection1;

        private readonly EntityReference _entityReferenceConnection2;

        public EntityReference CurrentValue { get; private set; }

        public EntityReferenceMappingControl(IOrganizationServiceExtented service
            , LookupAttributeMetadata attributeMetadata
            , EntityReference entityReferenceConnection1
            , EntityReference entityReferenceConnection2
        )
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

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
            var form = new WindowSelectEntityReference(_service.ConnectionData, new[] { _entityReferenceConnection1.LogicalName });

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            SetEntityReference(form.SelectedEntityReference);
        }
    }
}