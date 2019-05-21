using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        private readonly static ConcurrentDictionary<Type, ConcurrentDictionary<string, string>> _knownTypeValueDescriptions = new ConcurrentDictionary<Type, ConcurrentDictionary<string, string>>();

        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    var valueType = value.GetType();

                    if (!_knownTypeValueDescriptions.ContainsKey(valueType))
                    {
                        _knownTypeValueDescriptions.TryAdd(valueType, new ConcurrentDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase));
                    }

                    var currentTypeDescriptions = _knownTypeValueDescriptions[valueType];

                    var valueString = value.ToString();

                    if (currentTypeDescriptions.ContainsKey(valueString))
                    {
                        return currentTypeDescriptions[valueString];
                    }

                    string description = valueString;

                    FieldInfo fi = valueType.GetField(valueString);
                    if (fi != null)
                    {
                        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (attributes.Length > 0)
                        {
                            description = attributes[0].Description;
                        }
                    }

                    currentTypeDescriptions.TryAdd(valueString, description);

                    return description;
                }

                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}