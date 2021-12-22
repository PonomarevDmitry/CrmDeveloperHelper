using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class TeamTemplate
    {
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Access Rights
        ///     (Russian - 1049): Права доступа
        /// 
        /// Description:
        ///     (English - United States - 1033): Default access rights mask for the access teams associated with entity instances.
        ///     (Russian - 1049): Маска прав доступа по умолчанию для групп доступа, связанных с экземплярами сущностей.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalName(Schema.Attributes.defaultaccessrightsmask)]
        [System.ComponentModel.Description("Access Rights")]
        [System.Diagnostics.DebuggerNonUserCode()]
        public Schema.OptionSets.DefaultAccessRightsMask? DefaultAccessRightsMaskEnum
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                if (this.DefaultAccessRightsMask.HasValue)
                {
                    return (Schema.OptionSets.DefaultAccessRightsMask)this.DefaultAccessRightsMask.Value;
                }

                return null;
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging(nameof(DefaultAccessRightsMaskEnum));
                this.OnPropertyChanging(nameof(DefaultAccessRightsMask));

                if (value == null)
                {
                    this.SetAttributeValue(Schema.Attributes.defaultaccessrightsmask, null);
                }
                else
                {
                    this.SetAttributeValue(Schema.Attributes.defaultaccessrightsmask, (int)value);
                }

                this.OnPropertyChanged(nameof(DefaultAccessRightsMask));
                this.OnPropertyChanged(nameof(DefaultAccessRightsMaskEnum));
            }
        }

        public static partial class Schema
        {
            public static partial class OptionSets
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Access Rights
                ///     (Russian - 1049): Права доступа
                /// 
                /// Description:
                ///     (English - United States - 1033): Default access rights mask for the access teams associated with entity instances.
                ///     (Russian - 1049): Маска прав доступа по умолчанию для групп доступа, связанных с экземплярами сущностей.
                [System.ComponentModel.Description("DefaultAccessRightsMask")]
                [System.ComponentModel.TypeConverter("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
                [Flags]
                public enum DefaultAccessRightsMask
                {
                    [System.ComponentModel.Description("None")]
                    [System.Runtime.Serialization.EnumMember()]
                    None = 0,

                    [System.ComponentModel.Description("Read")]
                    [System.Runtime.Serialization.EnumMember()]
                    Read = 1,

                    [System.ComponentModel.Description("Update")]
                    [System.Runtime.Serialization.EnumMember()]
                    Update = 2,

                    [System.ComponentModel.Description("Append")]
                    [System.Runtime.Serialization.EnumMember()]
                    Append = 4,

                    [System.ComponentModel.Description("AppendTo")]
                    [System.Runtime.Serialization.EnumMember()]
                    AppendTo = 16,

                    [System.ComponentModel.Description("Delete")]
                    [System.Runtime.Serialization.EnumMember()]
                    Delete = 65536, // 2^16

                    [System.ComponentModel.Description("Share")]
                    [System.Runtime.Serialization.EnumMember()]
                    Share = 262144, // 2^18

                    [System.ComponentModel.Description("Assign")]
                    [System.Runtime.Serialization.EnumMember()]
                    Assign = 524288, // 2^19
                }
            }
        }
    }
}
