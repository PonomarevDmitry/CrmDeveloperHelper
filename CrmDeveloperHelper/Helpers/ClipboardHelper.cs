using System;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class ClipboardHelper
    {
        public static void SetText(string text)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    Clipboard.SetText(text);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public static void SetFileDropList(StringCollection stringCollection)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    Clipboard.SetFileDropList(stringCollection);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
