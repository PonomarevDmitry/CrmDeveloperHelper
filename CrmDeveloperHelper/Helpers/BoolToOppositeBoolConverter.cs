using System;
using System.Windows.Data;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class BoolToOppositeBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            if (targetType != typeof(bool) && targetType != typeof(bool?))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;

            //if (value == null)
            //{
            //    return true;
            //}

            //if (targetType != typeof(bool) && targetType != typeof(bool?))
            //    throw new InvalidOperationException("The target must be a boolean");

            //return !(bool)value;
        }

        #endregion IValueConverter Members
    }
}