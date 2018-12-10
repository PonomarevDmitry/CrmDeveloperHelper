using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class ManyToManyRelationshipMetadataViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly string[] _names =
        {
            nameof(IsChanged)
            //, nameof(IsGlobalFilterEnabled)
        };

        private ManyToManyRelationshipMetadata _ManyToManyRelationshipMetadata;
        public ManyToManyRelationshipMetadata ManyToManyRelationshipMetadata
        {
            get => _ManyToManyRelationshipMetadata;
            private set
            {
                _ManyToManyRelationshipMetadata = value;

                foreach (var name in _names)
                {
                    OnPropertyChanged(name);
                }
            }
        }

        public string SchemaName => ManyToManyRelationshipMetadata.SchemaName;

        public ManyToManyRelationshipMetadataViewItem(ManyToManyRelationshipMetadata manyToManyRelationshipMetadata)
        {
            LoadMetadata(manyToManyRelationshipMetadata);
        }

        public void LoadMetadata(ManyToManyRelationshipMetadata manyToManyRelationshipMetadata)
        {
            this.ManyToManyRelationshipMetadata = manyToManyRelationshipMetadata;

            this._IsChanged = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!string.Equals(propertyName, nameof(IsChanged)))
            {
                this.IsChanged = CalculateIsChanged();
            }
        }

        private bool CalculateIsChanged()
        {
            return false;
        }

        private void OnPropertyChanging(string propertyName)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        private bool _IsChanged = false;
        public bool IsChanged
        {
            get => _IsChanged;
            set
            {
                if (_IsChanged == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsChanged));
                this._IsChanged = value;
                this.OnPropertyChanged(nameof(IsChanged));
            }
        }

        //public bool? IsCustomRelationship { get; set; }
        //public BooleanManagedProperty IsCustomizable { get; set; }
        //public bool? IsValidForAdvancedFind { get; set; }
        //public string SchemaName { get; set; }
        //public SecurityTypes? SecurityTypes { get; set; }
        //public bool? IsManaged { get; }
        //public RelationshipType RelationshipType { get; }
        //public string IntroducedVersion { get; }

        public bool IsManaged => ManyToManyRelationshipMetadata.IsManaged.GetValueOrDefault();
        public bool IsCustomizable => (ManyToManyRelationshipMetadata.IsCustomizable?.Value).GetValueOrDefault();
        public bool IsCustomRelationship => ManyToManyRelationshipMetadata.IsCustomRelationship.GetValueOrDefault();
        public bool IsValidForAdvancedFind => ManyToManyRelationshipMetadata.IsValidForAdvancedFind.GetValueOrDefault();
        public SecurityTypes? SecurityTypes => ManyToManyRelationshipMetadata.SecurityTypes;
        public RelationshipType RelationshipType => ManyToManyRelationshipMetadata.RelationshipType;


        //public AssociatedMenuConfiguration Entity1AssociatedMenuConfiguration { get; set; }
        //public AssociatedMenuConfiguration Entity2AssociatedMenuConfiguration { get; set; }
        //public string Entity1LogicalName { get; set; }
        //public string Entity2LogicalName { get; set; }
        //public string IntersectEntityName { get; set; }
        //public string Entity1IntersectAttribute { get; set; }
        //public string Entity2IntersectAttribute { get; set; }
        //public string Entity1NavigationPropertyName { get; set; }
        //public string Entity2NavigationPropertyName { get; set; }

        public string Entity1LogicalName => ManyToManyRelationshipMetadata.Entity1LogicalName;
        public string Entity2LogicalName => ManyToManyRelationshipMetadata.Entity2LogicalName;
        public string IntersectEntityName => ManyToManyRelationshipMetadata.IntersectEntityName;
        public string Entity1IntersectAttribute => ManyToManyRelationshipMetadata.Entity1IntersectAttribute;
        public string Entity2IntersectAttribute => ManyToManyRelationshipMetadata.Entity2IntersectAttribute;
        public string Entity1NavigationPropertyName => ManyToManyRelationshipMetadata.Entity1NavigationPropertyName;
        public string Entity2NavigationPropertyName => ManyToManyRelationshipMetadata.Entity2NavigationPropertyName;

        //public AssociatedMenuBehavior? Behavior { get; set; }
        //public AssociatedMenuGroup? Group { get; set; }
        //public Label Label { get; set; }
        //public int? Order { get; set; }
        //public bool IsCustomizable { get; }
        //public string Icon { get; }
        //public Guid ViewId { get; }
        //public bool AvailableOffline { get; }
        //public string MenuId { get; }
        //public string QueryApi { get; }

        public AssociatedMenuBehavior? Entity1AssociatedMenuBehavior => ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.Behavior;
        public AssociatedMenuGroup? Entity1AssociatedMenuGroup => ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.Group;
        public int? Entity1AssociatedMenuOrder => ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.Order;
        public bool Entity1AssociatedMenuIsCustomizable => (ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.IsCustomizable).GetValueOrDefault();
        public string Entity1AssociatedMenuIcon => ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.Icon;
        public Guid? Entity1AssociatedMenuViewId => ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.ViewId;
        public bool Entity1AssociatedMenuAvailableOffline => (ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.AvailableOffline).GetValueOrDefault();
        public string Entity1AssociatedMenuMenuId => ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.MenuId;
        public string Entity1AssociatedMenuQueryApi => ManyToManyRelationshipMetadata.Entity1AssociatedMenuConfiguration?.QueryApi;



        public AssociatedMenuBehavior? Entity2AssociatedMenuBehavior => ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.Behavior;
        public AssociatedMenuGroup? Entity2AssociatedMenuGroup => ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.Group;
        public int? Entity2AssociatedMenuOrder => ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.Order;
        public bool Entity2AssociatedMenuIsCustomizable => (ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.IsCustomizable).GetValueOrDefault();
        public string Entity2AssociatedMenuIcon => ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.Icon;
        public Guid? Entity2AssociatedMenuViewId => ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.ViewId;
        public bool Entity2AssociatedMenuAvailableOffline => (ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.AvailableOffline).GetValueOrDefault();
        public string Entity2AssociatedMenuMenuId => ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.MenuId;
        public string Entity2AssociatedMenuQueryApi => ManyToManyRelationshipMetadata.Entity2AssociatedMenuConfiguration?.QueryApi;
    }
}
