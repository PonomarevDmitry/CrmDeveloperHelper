namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class PrivilegeObjectTypeCodes : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PrivilegeObjectTypeCodes() :
            base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "privilegeobjecttypecodes";

        public const int EntityTypeCode = 31;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objecttypecode")]
        public string ObjectTypeCode
        {
            get
            {
                return this.GetAttributeValue<string>("objecttypecode");
            }
            set
            {
                this.OnPropertyChanging("ObjectTypeCode");
                this.SetAttributeValue("objecttypecode", value);
                this.OnPropertyChanged("ObjectTypeCode");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("privilegeid")]
        public Microsoft.Xrm.Sdk.EntityReference PrivilegeId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("privilegeid");
            }
            set
            {
                this.OnPropertyChanging("PrivilegeId");
                this.SetAttributeValue("privilegeid", value);
                this.OnPropertyChanged("PrivilegeId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("privilegeobjecttypecodeid")]
        public System.Nullable<System.Guid> PrivilegeObjectTypeCodeId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("privilegeobjecttypecodeid");
            }
            set
            {
                this.OnPropertyChanging("PrivilegeObjectTypeCodeId");
                this.SetAttributeValue("privilegeobjecttypecodeid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("PrivilegeObjectTypeCodeId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }
    }
}
