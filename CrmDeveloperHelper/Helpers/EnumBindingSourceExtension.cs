using System;
using System.Collections;
using System.Linq;
using System.Windows.Markup;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;
        public Type EnumType
        {
            get => this._enumType;
            set
            {
                if (value != this._enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    this._enumType = value;
                }
            }
        }

        public bool SortByName { get; set; }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type enumType)
        {
            this.EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this._enumType == null)
                throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (this.SortByName)
            {
                Array.Sort(enumValues, new EnumSorterByName());
            }

            if (actualEnumType == this._enumType)
                return enumValues;

            object[] tempArray = new object[enumValues.Length + 1];
            tempArray[0] = string.Empty;
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }

        private class EnumSorterByName : IComparer
        {
            public int Compare(object x, object y)
            {
                return x.ToString().CompareTo(y.ToString());
            }
        }
    }
}