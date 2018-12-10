namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("stringmap")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "5.0.9689.1985")]
    public partial class StringMap : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public StringMap() :
            base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "stringmap";

        public const int EntityTypeCode = 1043;

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

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("attributename")]
        public string AttributeName
        {
            get
            {
                return this.GetAttributeValue<string>("attributename");
            }
            set
            {
                this.OnPropertyChanging("AttributeName");
                this.SetAttributeValue("attributename", value);
                this.OnPropertyChanged("AttributeName");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("attributevalue")]
        public System.Nullable<int> AttributeValue
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("attributevalue");
            }
            set
            {
                this.OnPropertyChanging("AttributeValue");
                this.SetAttributeValue("attributevalue", value);
                this.OnPropertyChanged("AttributeValue");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("displayorder")]
        public System.Nullable<int> DisplayOrder
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("displayorder");
            }
            set
            {
                this.OnPropertyChanging("DisplayOrder");
                this.SetAttributeValue("displayorder", value);
                this.OnPropertyChanged("DisplayOrder");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("langid")]
        public System.Nullable<int> LangId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("langid");
            }
            set
            {
                this.OnPropertyChanging("LangId");
                this.SetAttributeValue("langid", value);
                this.OnPropertyChanged("LangId");
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

        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("stringmapid")]
        public System.Nullable<System.Guid> StringMapId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("stringmapid");
            }
            set
            {
                this.OnPropertyChanging("StringMapId");
                this.SetAttributeValue("stringmapid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("StringMapId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("value")]
        public string Value
        {
            get
            {
                return this.GetAttributeValue<string>("value");
            }
            set
            {
                this.OnPropertyChanging("Value");
                this.SetAttributeValue("value", value);
                this.OnPropertyChanged("Value");
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
