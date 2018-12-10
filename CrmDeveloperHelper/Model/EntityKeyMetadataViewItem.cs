using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.ComponentModel;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
  public  class EntityKeyMetadataViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly string[] _names =
        {
            nameof(IsChanged)
            , nameof(IsCustomizable)
            , nameof(EntityKeyIndexStatus)
        };

        private EntityKeyMetadata _EntityKeyMetadata;
        public EntityKeyMetadata EntityKeyMetadata
        {
            get => _EntityKeyMetadata;
            private set
            {
                _EntityKeyMetadata = value;

                this._initialIsCustomizable = EntityKeyMetadata.IsCustomizable?.Value;

                this._initialIsKeyAttributes = EntityKeyMetadata.KeyAttributes;

                this._initialEntityKeyIndexStatus = EntityKeyMetadata.EntityKeyIndexStatus;

                foreach (var name in _names)
                {
                    OnPropertyChanged(name);
                }
            }
        }

        public string LogicalName => EntityKeyMetadata.LogicalName;

        public string DisplayName { get; private set; }

        private bool? _initialIsCustomizable;
        private string[] _initialIsKeyAttributes;

        private EntityKeyIndexStatus _initialEntityKeyIndexStatus;

        public EntityKeyMetadataViewItem(EntityKeyMetadata EntityKeyMetadata)
        {
            LoadMetadata(EntityKeyMetadata);
        }

        public void LoadMetadata(EntityKeyMetadata EntityKeyMetadata)
        {
            this.DisplayName = CreateFileHandler.GetLocalizedLabel(EntityKeyMetadata.DisplayName);

            this.EntityKeyMetadata = EntityKeyMetadata;

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
            if (!(_initialIsKeyAttributes?.SequenceEqual(EntityKeyMetadata.KeyAttributes)).GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsCustomizable != EntityKeyMetadata.IsCustomizable?.Value)
            {
                return true;
            }

            if (_initialEntityKeyIndexStatus != EntityKeyMetadata.EntityKeyIndexStatus)
            {
                return true;
            }

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

        public bool IsCustomizableCanBeChanged => (EntityKeyMetadata.IsCustomizable?.CanBeChanged).GetValueOrDefault();

        public bool IsCustomizable
        {
            get => (EntityKeyMetadata.IsCustomizable?.Value).GetValueOrDefault();
            set
            {
                if (EntityKeyMetadata.IsCustomizable == null || !EntityKeyMetadata.IsCustomizable.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsCustomizable));
                this.EntityKeyMetadata.IsCustomizable.Value = value;
                this.OnPropertyChanged(nameof(IsCustomizable));
            }
        }

        public EntityKeyIndexStatus EntityKeyIndexStatus => EntityKeyMetadata.EntityKeyIndexStatus;

        public string KeyAttributesString
        {
            get
            {
                if (this.EntityKeyMetadata == null
                    || this.EntityKeyMetadata.KeyAttributes == null
                    )
                {
                    return null;
                }

                return string.Join(",", this.EntityKeyMetadata.KeyAttributes.OrderBy(s => s));
            }
        }
    }
}
