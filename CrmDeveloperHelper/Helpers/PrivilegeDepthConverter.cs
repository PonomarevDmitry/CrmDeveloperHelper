using Microsoft.Crm.Sdk.Messages;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class PrivilegeDepthConverter : IValueConverter
    {
        private BitmapImage _imageNone;
        private BitmapImage _imageBasic;
        private BitmapImage _imageLocal;
        private BitmapImage _imageDeep;
        private BitmapImage _imageGlobal;

        public PrivilegeDepthConverter()
        {
            this._imageNone = new BitmapImage(new Uri(@"pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/PrivilegeDepths/ico_18_role_X.gif", UriKind.Absolute))
            {
                DecodePixelHeight = 16,
                DecodePixelWidth = 16,
            };

            this._imageBasic = new BitmapImage(new Uri(@"pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/PrivilegeDepths/ico_18_role_B.gif", UriKind.Absolute))
            {
                DecodePixelHeight = 16,
                DecodePixelWidth = 16,
            };
            this._imageLocal = new BitmapImage(new Uri(@"pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/PrivilegeDepths/ico_18_role_L.gif", UriKind.Absolute))
            {
                DecodePixelHeight = 16,
                DecodePixelWidth = 16,
            };
            this._imageDeep = new BitmapImage(new Uri(@"pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/PrivilegeDepths/ico_18_role_D.gif", UriKind.Absolute))
            {
                DecodePixelHeight = 16,
                DecodePixelWidth = 16,
            };
            this._imageGlobal = new BitmapImage(new Uri(@"pack://application:,,,/Nav.Common.VSPackages.CrmDeveloperHelper;component/Resources/PrivilegeDepths/ico_18_role_G.gif", UriKind.Absolute))
            {
                DecodePixelHeight = 16,
                DecodePixelWidth = 16,
            };
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return _imageNone;
            }

            if (value is string)
            {
                return _imageNone;
            }

            if (!(value is PrivilegeDepthExtended depth))
            {
                return _imageNone;
            }

            switch (depth)
            {
                case PrivilegeDepthExtended.Basic:
                    return _imageBasic;
                case PrivilegeDepthExtended.Local:
                    return _imageLocal;
                case PrivilegeDepthExtended.Deep:
                    return _imageDeep;
                case PrivilegeDepthExtended.Global:
                    return _imageGlobal;

                case PrivilegeDepthExtended.None:
                default:
                    return _imageNone;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        #endregion IValueConverter Members
    }
}
