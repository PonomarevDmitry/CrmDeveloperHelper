using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;

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

        public string EntityLogicalName { get; set; }

        public Guid? PluginAssembly { get; set; }

        public Guid? PluginType { get; set; }

        public Guid? Step { get; set; }

        public Guid? StepImage { get; set; }

        public ComponentType? ComponentType { get; private set; }

        public ObservableCollection<PluginTreeViewItem> Items { get; private set; }

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

            return null;
        }
    }
}