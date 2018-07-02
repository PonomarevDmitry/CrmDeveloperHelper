namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    /// <summary>
    /// Permission to perform an action in Microsoft CRM. The platform checks for the privilege and rejects the attempt if the user does not hold the privilege.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("privilege")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9154")]
    public partial class Privilege : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        public Privilege() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "privilege";

        public const string PrimaryIdAttribute = "privilegeid";

        public const string PrimaryNameAttribute = "name";

        public const int EntityTypeCode = 1023;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        [System.Diagnostics.DebuggerNonUserCode()]
        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        [System.Diagnostics.DebuggerNonUserCode()]
        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Rights a user has to an instance of an entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("accessright")]
        public System.Nullable<int> AccessRight
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("accessright");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("AccessRight");
                this.SetAttributeValue("accessright", value);
                this.OnPropertyChanged("AccessRight");
            }
        }

        /// <summary>
        /// Information that specifies whether the privilege applies to the user, the user's team, or objects shared by the user.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canbebasic")]
        public System.Nullable<bool> CanBeBasic
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("canbebasic");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("CanBeBasic");
                this.SetAttributeValue("canbebasic", value);
                this.OnPropertyChanged("CanBeBasic");
            }
        }

        /// <summary>
        /// Information that specifies whether the privilege applies to child business units of the business unit associated with the user.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canbedeep")]
        public System.Nullable<bool> CanBeDeep
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("canbedeep");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("CanBeDeep");
                this.SetAttributeValue("canbedeep", value);
                this.OnPropertyChanged("CanBeDeep");
            }
        }

        /// <summary>
        /// Information that specifies whether the privilege applies to the local reference of an external party.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canbeentityreference")]
        public System.Nullable<bool> CanBeEntityReference
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("canbeentityreference");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("CanBeEntityReference");
                this.SetAttributeValue("canbeentityreference", value);
                this.OnPropertyChanged("CanBeEntityReference");
            }
        }

        /// <summary>
        /// Information that specifies whether the privilege applies to the entire organization.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canbeglobal")]
        public System.Nullable<bool> CanBeGlobal
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("canbeglobal");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("CanBeGlobal");
                this.SetAttributeValue("canbeglobal", value);
                this.OnPropertyChanged("CanBeGlobal");
            }
        }

        /// <summary>
        /// Information that specifies whether the privilege applies to the user's business unit.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canbelocal")]
        public System.Nullable<bool> CanBeLocal
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("canbelocal");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("CanBeLocal");
                this.SetAttributeValue("canbelocal", value);
                this.OnPropertyChanged("CanBeLocal");
            }
        }

        /// <summary>
        /// Information that specifies whether the privilege applies to parent reference of the external party.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canbeparententityreference")]
        public System.Nullable<bool> CanBeParentEntityReference
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("canbeparententityreference");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("CanBeParentEntityReference");
                this.SetAttributeValue("canbeparententityreference", value);
                this.OnPropertyChanged("CanBeParentEntityReference");
            }
        }

        /// <summary>
        /// Name of the privilege.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the privilege.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("privilegeid")]
        public System.Nullable<System.Guid> PrivilegeId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("privilegeid");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("PrivilegeId");
                this.SetAttributeValue("privilegeid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("PrivilegeId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("privilegeid")]
        public override System.Guid Id
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return base.Id;
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.PrivilegeId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// Constructor for populating via LINQ queries given a LINQ anonymous type
        /// <param name="anonymousType">LINQ anonymous type.</param>
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        public Privilege(object anonymousType) :
                this()
        {
            foreach (var p in anonymousType.GetType().GetProperties())
            {
                var value = p.GetValue(anonymousType, null);
                var name = p.Name.ToLower();

                if (name.EndsWith("enum") && value.GetType().BaseType == typeof(System.Enum))
                {
                    value = new Microsoft.Xrm.Sdk.OptionSetValue((int)value);
                    name = name.Remove(name.Length - "enum".Length);
                }

                switch (name)
                {
                    case "id":
                        base.Id = (System.Guid)value;
                        Attributes["privilegeid"] = base.Id;
                        break;
                    case "privilegeid":
                        var id = (System.Nullable<System.Guid>)value;
                        if (id == null) { continue; }
                        base.Id = id.Value;
                        Attributes[name] = base.Id;
                        break;
                    case "formattedvalues":
                        // Add Support for FormattedValues
                        FormattedValues.AddRange((Microsoft.Xrm.Sdk.FormattedValueCollection)value);
                        break;
                    default:
                        Attributes[name] = value;
                        break;
                }
            }
        }
    }
}
