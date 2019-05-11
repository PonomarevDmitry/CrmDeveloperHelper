using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityTreeViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Guid? WebResourceId { get; set; }

        public WebResource WebResource { get; set; }

        public ObservableCollection<EntityTreeViewItem> Items { get; private set; }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_Name == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Name));
                this._Name = value;
                this.OnPropertyChanged(nameof(Name));
            }
        }

        private BitmapImage _Image;
        public BitmapImage Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (_Image == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Image));
                this._Image = value;
                this.OnPropertyChanged(nameof(Image));
            }
        }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                if (_IsExpanded == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsExpanded));
                this._IsExpanded = value;
                this.OnPropertyChanged(nameof(IsExpanded));
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsSelected));
                this._IsSelected = value;
                this.OnPropertyChanged(nameof(IsSelected));
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_Description == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Description));
                this._Description = value;
                this.OnPropertyChanged(nameof(Description));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        public EntityTreeViewItem()
        {
            this.Items = new ObservableCollection<EntityTreeViewItem>();
        }

        public EntityTreeViewItem(string name, WebResource webResource, BitmapImage image)
            : this()
        {
            this.Name = name;
            this.WebResourceId = webResource?.Id;
            this.WebResource = webResource;
            this.Image = image;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}