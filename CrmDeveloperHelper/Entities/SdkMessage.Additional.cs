namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class SdkMessage
    {
        public static partial class Schema
        {
            public static partial class Instances
            {
                public const string ExportFieldTranslation = "ExportFieldTranslation";

                public const string RetrieveMultiple = "RetrieveMultiple";

                public const string Retrieve = "Retrieve";
            }
        }

        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Availability
        ///     (Russian - 1049): Доступность
        /// 
        /// Description:
        ///     (English - United States - 1033): Identifies where a method will be exposed. 0 - Server, 1 - Client, 2 - both.
        ///     (Russian - 1049): Указывает, где будет применен метод. 0 - сервер, 1 - клиент, 2 - оба.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessage.Schema.Attributes.availability)]
        [System.ComponentModel.DescriptionAttribute("Availability")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Nullable<Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets.AvailabilityEnum> AvailabilityEnum
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                System.Nullable<int> nullableInt = this.GetAttributeValue<System.Nullable<int>>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessage.Schema.Attributes.availability);
                if (((nullableInt.HasValue)
                            && System.Enum.IsDefined(typeof(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets.AvailabilityEnum), nullableInt.Value)))
                {
                    return ((Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets.AvailabilityEnum)(System.Enum.ToObject(typeof(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets.AvailabilityEnum), nullableInt.Value)));
                }
                else
                {
                    return null;
                }
            }
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            set
            {
                this.OnPropertyChanging(nameof(AvailabilityEnum));
                this.OnPropertyChanging(nameof(Availability));
                if ((value.HasValue))
                {
                    this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessage.Schema.Attributes.availability, ((int)value.Value));
                }
                else
                {
                    this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessage.Schema.Attributes.availability, null);
                }
                this.OnPropertyChanged(nameof(Availability));
                this.OnPropertyChanged(nameof(AvailabilityEnum));
            }
        }
    }
}