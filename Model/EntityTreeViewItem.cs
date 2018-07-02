using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityTreeViewItem
    {
        public string Name { get; set; }

        public Guid? WebResourceId { get; set; }

        public BitmapImage Image { get; set; }

        public ObservableCollection<EntityTreeViewItem> Items { get; private set; }

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }

        public string Description { get; set; }

        public EntityTreeViewItem()
        {
            this.Items = new ObservableCollection<EntityTreeViewItem>();
        }

        public EntityTreeViewItem(string name, Guid? idWebResource, BitmapImage image)
            : this()
        {
            this.Name = name;
            this.WebResourceId = idWebResource;
            this.Image = image;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}