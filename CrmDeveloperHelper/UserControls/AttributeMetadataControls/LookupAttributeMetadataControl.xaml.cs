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
    public partial class LookupAttributeMetadataControl : UserControl, IAttributeMetadataControl<LookupAttributeMetadata>
    {
        private readonly IOrganizationServiceExtented _service;

        public LookupAttributeMetadata AttributeMetadata { get; private set; }

        private readonly EntityReference _initialValue;

        private readonly bool _fillAllways;

        private EntityReference currentValue;

        public LookupAttributeMetadataControl(IOrganizationServiceExtented service, bool fillAllways, LookupAttributeMetadata attributeMetadata, EntityReference initialValue)
        {
            InitializeComponent();

            AttributeMetadataControlFactory.SetGroupBoxNameByAttributeMetadata(gbAttribute, attributeMetadata);

            this._service = service;

            this._initialValue = initialValue;
            this._fillAllways = fillAllways;
            this.AttributeMetadata = attributeMetadata;

            SetEntityReference(_initialValue);

            btnRemoveControl.IsEnabled = _fillAllways;

            btnRemoveControl.Visibility = btnRemoveControl.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            chBChanged.Visibility = _fillAllways ? Visibility.Collapsed : Visibility.Visible;
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

            var form = new WindowSelectEntityReference(_service.ConnectionData, targets);

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
    }
}