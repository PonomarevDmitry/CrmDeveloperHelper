using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class PluginTreeViewItem
    {
        public string Name { get; set; }

        public BitmapImage Image { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }

        public string Tooltip { get; set; }

        public List<Guid> Message { get; set; }

        public List<Guid> MessageFilter { get; set; }

        public Guid? PluginAssembly { get; set; }

        public Guid? PluginType { get; set; }

        public Guid? Step { get; set; }

        public Guid? StepImage { get; set; }

        public ObservableCollection<PluginTreeViewItem> Items { get; private set; }

        public PluginTreeViewItem()
        {
            this.Items = new ObservableCollection<PluginTreeViewItem>();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}