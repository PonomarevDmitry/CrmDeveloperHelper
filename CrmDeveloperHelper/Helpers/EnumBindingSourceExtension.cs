using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Windows.Markup;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private readonly static ConcurrentDictionary<Tuple<Type, bool, bool>, Array> _knownTypeSources = new ConcurrentDictionary<Tuple<Type, bool, bool>, Array>();

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

        public bool SortByIntValue { get; set; }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type enumType)
        {
            this.EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this._enumType == null)
                throw new InvalidOperationException("The EnumType must be specified.");

            var key = Tuple.Create(_enumType, this.SortByName, this.SortByIntValue);

            if (_knownTypeSources.ContainsKey(key))
            {
                return _knownTypeSources[key];
            }

            var result = GetTypeArray(_enumType, this.SortByName, this.SortByIntValue);

            _knownTypeSources.TryAdd(key, result);

            return result;
        }

        private static Array GetTypeArray(Type enumType, bool sortByName, bool sortByIntValue)
        {
            Type actualEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (sortByName)
            {
                Array.Sort(enumValues, EnumSorterByName.Comparer);
            }
            else if (sortByIntValue)
            {
                Array.Sort(enumValues, EnumSorterByValue.Comparer);
            }

            if (actualEnumType == enumType)
                return enumValues;

            object[] tempArray = new object[enumValues.Length + 1];
            tempArray[0] = string.Empty;
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }

        private class EnumSorterByValue : IComparer
        {
            private EnumSorterByValue()
            {

            }

            public static EnumSorterByValue Comparer { get; } = new EnumSorterByValue();

            public int Compare(object x, object y)
            {
                int xValue = Convert.ToInt32(x);
                int yValue = Convert.ToInt32(y);

                return xValue.CompareTo(yValue);
            }
        }

        private class EnumSorterByName : IComparer
        {
            private EnumSorterByName()
            {

            }

            public static EnumSorterByName Comparer { get; } = new EnumSorterByName();

            public int Compare(object x, object y)
            {
                return x.ToString().CompareTo(y.ToString());
            }
        }
    }
}