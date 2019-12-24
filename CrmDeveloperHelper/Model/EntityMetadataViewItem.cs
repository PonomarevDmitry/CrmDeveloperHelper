﻿using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityMetadataViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static string[] _names =
        {
            nameof(IsChanged)
            , nameof(IsAuditEnabled)
            //, nameof(IsGlobalFilterEnabled)
            //, nameof(IsSortableEnabled)
            //, nameof(IsCustomizable)
            //, nameof(IsRenameable)
            //, nameof(IsValidForAdvancedFind)
            //, nameof(IsValidForForm)
            //, nameof(IsRequiredForForm)
            //, nameof(IsValidForGrid)
            //, nameof(RequiredLevel)
            //, nameof(CanModifyAdditionalSettings)
            //, nameof(IsDataSourceSecret)
            //, nameof(IsSecured)
        };

        private EntityMetadata _EntityMetadata;
        public EntityMetadata EntityMetadata
        {
            get => _EntityMetadata;
            private set
            {
                _EntityMetadata = value;

                this._initialIsAuditEnabled = EntityMetadata.IsAuditEnabled?.Value;

                foreach (var name in _names)
                {
                    OnPropertyChanged(name);
                }
            }
        }

        public string LogicalName => EntityMetadata.LogicalName;

        public string DisplayName { get; private set; }

        private bool? _initialIsAuditEnabled;

        public EntityMetadataViewItem(EntityMetadata entityMetadata)
        {
            LoadMetadata(entityMetadata);
        }

        public void LoadMetadata(EntityMetadata entityMetadata)
        {
            this.EntityMetadata = entityMetadata;
           
            this.DisplayName = CreateFileHandler.GetLocalizedLabel(entityMetadata.DisplayName);

            this.OnPropertyChanging(nameof(IsChanged));
            this.IsChanged = false;
            this.OnPropertyChanged(nameof(IsChanged));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!string.Equals(propertyName, nameof(IsChanged)))
            {
                var val = CalculateIsChanged();

                if (val != this.IsChanged)
                {
                    this.OnPropertyChanging(nameof(IsChanged));
                    this.IsChanged = val;
                    this.OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        private bool CalculateIsChanged()
        {
            if (_initialIsAuditEnabled != EntityMetadata.IsAuditEnabled?.Value)
            {
                return true;
            }

            return false;
        }

        private void OnPropertyChanging(string propertyName)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public bool IsChanged { get; private set; }

        public bool IsAuditEnabledCanBeChanged => (EntityMetadata.IsAuditEnabled?.CanBeChanged).GetValueOrDefault();

        public bool IsAuditEnabled
        {
            get
            {
                return (EntityMetadata.IsAuditEnabled?.Value).GetValueOrDefault();
            }
            set
            {
                if (EntityMetadata.IsAuditEnabled == null || !EntityMetadata.IsAuditEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsAuditEnabled));
                this.EntityMetadata.IsAuditEnabled.Value = value;
                this.OnPropertyChanged(nameof(IsAuditEnabled));
            }
        }
    }
}