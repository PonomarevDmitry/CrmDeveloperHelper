using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class SdkMessageRequestTreeViewItem
    {
        public string Name { get; set; }

        public BitmapImage Image { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }

        public string Tooltip { get; set; }

        public List<Guid> Message { get; set; }

        public Guid? SdkMessageRequest { get; set; }

        public Guid? SdkMessageResponse { get; set; }

        public ObservableCollection<SdkMessageRequestTreeViewItem> Items { get; private set; }

        public SdkMessageRequestTreeViewItem()
        {
            this.Items = new ObservableCollection<SdkMessageRequestTreeViewItem>();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}