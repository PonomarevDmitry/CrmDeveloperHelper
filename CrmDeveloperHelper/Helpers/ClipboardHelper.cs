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
            var thread = new Thread(() => Clipboard.SetText(text));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        public static void SetFileDropList(StringCollection stringCollection)
        {
            var thread = new Thread(() => Clipboard.SetFileDropList(stringCollection));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
