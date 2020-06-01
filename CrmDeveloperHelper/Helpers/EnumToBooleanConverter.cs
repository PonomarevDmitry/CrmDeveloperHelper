using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Data;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is Enum enumParameter)
            {
                var result = enumParameter.Equals(value);

                return result;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType.IsEnum
                && parameter is Enum enumParameter
                && (value is bool? || value is bool)
            )
            {
                var valueBool = (bool?)value;

                object result = null;

                if (valueBool.GetValueOrDefault())
                {
                    result = enumParameter;
                }
                else
                {
                    Array enumValues = GetTypeArray(targetType);

                    result = enumValues.Cast<object>().FirstOrDefault(o => !enumParameter.Equals(o));
                }

                return result;
            }

            return Binding.DoNothing;
        }

        private readonly static ConcurrentDictionary<Type, Array> _knownTypeSources = new ConcurrentDictionary<Type, Array>();

        private static Array GetTypeArray(Type enumType)
        {
            if (_knownTypeSources.ContainsKey(enumType))
            {
                return _knownTypeSources[enumType];
            }

            var result = CreateTypeArray(enumType);

            _knownTypeSources.TryAdd(enumType, result);

            return result;
        }

        private static Array CreateTypeArray(Type enumType)
        {
            Type actualEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

            if (!actualEnumType.IsEnum)
                throw new ArgumentException("Type must be for an Enum.");

            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == enumType)
                return enumValues;

            object[] tempArray = new object[enumValues.Length + 1];
            tempArray[0] = string.Empty;
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}