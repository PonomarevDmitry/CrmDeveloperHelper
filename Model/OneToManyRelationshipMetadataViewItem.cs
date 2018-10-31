using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class OneToManyRelationshipMetadataViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly string[] _names =
        {
            nameof(IsChanged)
            //, nameof(IsGlobalFilterEnabled)
        };

        private OneToManyRelationshipMetadata _OneToManyRelationshipMetadata;
        public OneToManyRelationshipMetadata OneToManyRelationshipMetadata
        {
            get => _OneToManyRelationshipMetadata;
            private set
            {
                _OneToManyRelationshipMetadata = value;

                foreach (var name in _names)
                {
                    OnPropertyChanged(name);
                }
            }
        }

        public string SchemaName => OneToManyRelationshipMetadata.SchemaName;

        public string DisplayName { get; private set; }

        public OneToManyRelationshipMetadataViewItem(OneToManyRelationshipMetadata oneToManyRelationshipMetadata)
        {
            LoadMetadata(oneToManyRelationshipMetadata);
        }

        public void LoadMetadata(OneToManyRelationshipMetadata oneToManyRelationshipMetadata)
        {
            this.DisplayName = CreateFileHandler.GetLocalizedLabel(oneToManyRelationshipMetadata.AssociatedMenuConfiguration?.Label);

            this.OneToManyRelationshipMetadata = oneToManyRelationshipMetadata;

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

        public bool IsManaged => OneToManyRelationshipMetadata.IsManaged.GetValueOrDefault();
        public bool IsCustomizable => (OneToManyRelationshipMetadata.IsCustomizable?.Value).GetValueOrDefault();
        public bool IsCustomRelationship => OneToManyRelationshipMetadata.IsCustomRelationship.GetValueOrDefault();
        public bool IsValidForAdvancedFind => OneToManyRelationshipMetadata.IsValidForAdvancedFind.GetValueOrDefault();
        public SecurityTypes? SecurityTypes => OneToManyRelationshipMetadata.SecurityTypes;
        public RelationshipType RelationshipType => OneToManyRelationshipMetadata.RelationshipType;

        //public AssociatedMenuConfiguration AssociatedMenuConfiguration { get; set; }
        //public CascadeConfiguration CascadeConfiguration { get; set; }
        //public string ReferencedAttribute { get; set; }
        //public string ReferencedEntity { get; set; }
        //public string ReferencingAttribute { get; set; }
        //public string ReferencingEntity { get; set; }
        //public bool? IsHierarchical { get; set; }
        //public string ReferencedEntityNavigationPropertyName { get; set; }
        //public string ReferencingEntityNavigationPropertyName { get; set; }

        public string ReferencedEntity => OneToManyRelationshipMetadata.ReferencedEntity;
        public string ReferencedAttribute => OneToManyRelationshipMetadata.ReferencedAttribute;
        public string ReferencingEntity => OneToManyRelationshipMetadata.ReferencingEntity;
        public string ReferencingAttribute => OneToManyRelationshipMetadata.ReferencingAttribute;
        public bool IsHierarchical => OneToManyRelationshipMetadata.IsHierarchical.GetValueOrDefault();
        public string ReferencedEntityNavigationPropertyName => OneToManyRelationshipMetadata.ReferencedEntityNavigationPropertyName;
        public string ReferencingEntityNavigationPropertyName => OneToManyRelationshipMetadata.ReferencingEntityNavigationPropertyName;

        //public CascadeType? Assign { get; set; }
        //public CascadeType? Delete { get; set; }
        //public CascadeType? Merge { get; set; }
        //public CascadeType? Reparent { get; set; }
        //public CascadeType? Share { get; set; }
        //public CascadeType? Unshare { get; set; }
        //public CascadeType? RollupView { get; set; }

        public CascadeType? Assign => OneToManyRelationshipMetadata.CascadeConfiguration?.Assign;
        public CascadeType? Delete => OneToManyRelationshipMetadata.CascadeConfiguration?.Delete;
        public CascadeType? Merge => OneToManyRelationshipMetadata.CascadeConfiguration?.Merge;
        public CascadeType? Reparent => OneToManyRelationshipMetadata.CascadeConfiguration?.Reparent;
        public CascadeType? Share => OneToManyRelationshipMetadata.CascadeConfiguration?.Share;
        public CascadeType? Unshare => OneToManyRelationshipMetadata.CascadeConfiguration?.Unshare;
        public CascadeType? RollupView => OneToManyRelationshipMetadata.CascadeConfiguration?.RollupView;

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

        public AssociatedMenuBehavior? Behavior => OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.Behavior;
        public AssociatedMenuGroup? Group => OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.Group;
        public int? Order => OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.Order;
        public bool AssociatedMenuConfigurationIsCustomizable => (OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.IsCustomizable).GetValueOrDefault();
        public string Icon => OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.Icon;
        public Guid? ViewId => OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.ViewId;
        public bool AvailableOffline => (OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.AvailableOffline).GetValueOrDefault();
        public string MenuId => OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.MenuId;
        public string QueryApi => OneToManyRelationshipMetadata.AssociatedMenuConfiguration?.QueryApi;
    }
}