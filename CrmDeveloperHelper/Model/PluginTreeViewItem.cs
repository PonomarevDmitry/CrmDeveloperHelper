using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        private bool _IsChecked;
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                if (_IsChecked == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsChecked));
                this._IsChecked = value;
                this.OnPropertyChanged(nameof(IsChecked));
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

        private bool _IsWorkflowActivity;
        public bool IsWorkflowActivity
        {
            get
            {
                return _IsWorkflowActivity;
            }
            set
            {
                if (_IsWorkflowActivity == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsWorkflowActivity));
                this._IsWorkflowActivity = value;
                this.OnPropertyChanged(nameof(IsWorkflowActivity));
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

        public string MessageCategoryName { get; set; }

        public List<Guid> MessageList { get; }

        public List<Guid> MessageFilterList { get; }

        public string EntityLogicalName { get; set; }

        public Guid? PluginAssemblyId { get; set; }

        public Guid? PluginTypeId { get; set; }

        public Guid? StepId { get; set; }

        public Guid? StepImageId { get; set; }

        public Guid? WorkflowId { get; set; }

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

            this.MessageList = new List<Guid>();

            this.MessageFilterList = new List<Guid>();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public Guid? GetId()
        {
            if (this.PluginAssemblyId.HasValue && this.ComponentType == Entities.ComponentType.PluginAssembly)
            {
                return this.PluginAssemblyId.Value;
            }

            if (this.PluginTypeId.HasValue && this.ComponentType == Entities.ComponentType.PluginType)
            {
                return this.PluginTypeId.Value;
            }

            if (this.StepId.HasValue && this.ComponentType == Entities.ComponentType.SdkMessageProcessingStep)
            {
                return this.StepId.Value;
            }

            if (this.StepImageId.HasValue && this.ComponentType == Entities.ComponentType.SdkMessageProcessingStepImage)
            {
                return this.StepImageId.Value;
            }

            if (this.WorkflowId.HasValue && this.ComponentType == Entities.ComponentType.Workflow)
            {
                return this.WorkflowId.Value;
            }

            if (this.MessageList.Count == 1 && this.ComponentType == Entities.ComponentType.SdkMessage)
            {
                return this.MessageList.First();
            }

            if (this.MessageFilterList.Count == 1 && this.ComponentType == Entities.ComponentType.SdkMessageFilter)
            {
                return this.MessageFilterList.First();
            }

            return null;
        }

        public IEnumerable<Guid> GetIdEnumerable()
        {
            if (this.PluginAssemblyId.HasValue && this.ComponentType == Entities.ComponentType.PluginAssembly)
            {
                yield return this.PluginAssemblyId.Value;
            }

            if (this.PluginTypeId.HasValue && this.ComponentType == Entities.ComponentType.PluginType)
            {
                yield return this.PluginTypeId.Value;
            }

            if (this.StepId.HasValue && this.ComponentType == Entities.ComponentType.SdkMessageProcessingStep)
            {
                yield return this.StepId.Value;
            }

            if (this.StepImageId.HasValue && this.ComponentType == Entities.ComponentType.SdkMessageProcessingStepImage)
            {
                yield return this.StepImageId.Value;
            }

            if (this.WorkflowId.HasValue && this.ComponentType == Entities.ComponentType.Workflow)
            {
                yield return this.WorkflowId.Value;
            }

            if (this.ComponentType == Entities.ComponentType.SdkMessage)
            {
                foreach (var item in this.MessageList)
                {
                    yield return item;
                }
            }

            if (this.ComponentType == Entities.ComponentType.SdkMessageFilter)
            {
                foreach (var item in this.MessageFilterList)
                {
                    yield return item;
                }
            }
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