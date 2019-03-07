using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class PluginTreeViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
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

        private bool _IsActive;
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                if (_IsActive == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsActive));
                this._IsActive = value;
                this.OnPropertyChanged(nameof(IsActive));
            }
        }

        private string _Tooltip;
        public string Tooltip
        {
            get
            {
                return _Tooltip;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_Tooltip == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Tooltip));
                this._Tooltip = value;
                this.OnPropertyChanged(nameof(Tooltip));
            }
        }

        public BitmapImage ImageActive { get; set; }

        public BitmapImage ImageInactive { get; set; }

        public string MessageName { get; set; }

        public List<Guid> MessageList { get; set; }

        public List<Guid> MessageFilterList { get; set; }

        public string EntityLogicalName { get; set; }

        public Guid? PluginAssembly { get; set; }

        public Guid? PluginType { get; set; }

        public Guid? Step { get; set; }

        public Guid? StepImage { get; set; }

        public Guid? Workflow { get; set; }

        public ComponentType? ComponentType { get; private set; }

        public PluginTreeViewItem Parent { get; set; }

        public ObservableCollection<PluginTreeViewItem> Items { get; private set; }

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

        public PluginTreeViewItem(ComponentType? componentType)
        {
            this.ComponentType = componentType;

            this.Items = new ObservableCollection<PluginTreeViewItem>();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public Guid? GetId()
        {
            if (this.PluginAssembly.HasValue && this.ComponentType == Entities.ComponentType.PluginAssembly)
            {
                return this.PluginAssembly.Value;
            }

            if (this.PluginType.HasValue && this.ComponentType == Entities.ComponentType.PluginType)
            {
                return this.PluginType.Value;
            }

            if (this.Step.HasValue && this.ComponentType == Entities.ComponentType.SdkMessageProcessingStep)
            {
                return this.Step.Value;
            }

            if (this.StepImage.HasValue && this.ComponentType == Entities.ComponentType.SdkMessageProcessingStepImage)
            {
                return this.StepImage.Value;
            }

            if (this.Workflow.HasValue && this.ComponentType == Entities.ComponentType.Workflow)
            {
                return this.Workflow.Value;
            }

            return null;
        }

        public void CorrectImage()
        {
            if (this.IsActive && this.ImageActive != null)
            {
                this.Image = ImageActive;
            }
            else if (!this.IsActive && this.ImageInactive != null)
            {
                this.Image = ImageInactive;
            }
        }
    }
}