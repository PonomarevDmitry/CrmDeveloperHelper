using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class AttributeMetadataViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly string[] _names =
        {
            nameof(IsChanged)
            , nameof(IsGlobalFilterEnabled)
            , nameof(IsSortableEnabled)
            , nameof(IsCustomizable)
            , nameof(IsRenameable)
            , nameof(IsValidForAdvancedFind)
            , nameof(IsValidForForm)
            , nameof(IsRequiredForForm)
            , nameof(IsValidForGrid)
            , nameof(RequiredLevel)
            , nameof(CanModifyAdditionalSettings)
            , nameof(IsDataSourceSecret)
            , nameof(IsSecured)
            , nameof(IsAuditEnabled)
        };

        private AttributeMetadata _AttributeMetadata;
        public AttributeMetadata AttributeMetadata
        {
            get => _AttributeMetadata;
            private set
            {
                _AttributeMetadata = value;

                this._initialIsAuditEnabled = AttributeMetadata.IsAuditEnabled?.Value;
                this._initialIsGlobalFilterEnabled = AttributeMetadata.IsGlobalFilterEnabled?.Value;
                this._initialIsSortableEnabled = AttributeMetadata.IsSortableEnabled?.Value;
                this._initialIsCustomizable = AttributeMetadata.IsCustomizable?.Value;
                this._initialIsRenameable = AttributeMetadata.IsRenameable?.Value;
                this._initialIsValidForAdvancedFind = AttributeMetadata.IsValidForAdvancedFind?.Value;
                this._initialCanModifyAdditionalSettings = AttributeMetadata.CanModifyAdditionalSettings?.Value;

                this._initialRequiredLevel = AttributeMetadata.RequiredLevel?.Value;

                this._initialIsSecured = AttributeMetadata.IsSecured.GetValueOrDefault();

                this._initialIsValidForForm = AttributeMetadata.IsValidForForm.GetValueOrDefault();
                this._initialIsRequiredForForm = AttributeMetadata.IsRequiredForForm.GetValueOrDefault();
                this._initialIsValidForGrid = AttributeMetadata.IsValidForGrid.GetValueOrDefault();
                this._initialIsDataSourceSecret = AttributeMetadata.IsDataSourceSecret.GetValueOrDefault();

                foreach (var name in _names)
                {
                    OnPropertyChanged(name);
                }
            }
        }

        public string LogicalName => AttributeMetadata.LogicalName;

        public string DisplayName { get; private set; }

        private bool? _initialIsAuditEnabled;
        private bool? _initialIsGlobalFilterEnabled;
        private bool? _initialIsSortableEnabled;
        private bool? _initialIsCustomizable;
        private bool? _initialIsRenameable;
        private bool? _initialIsValidForAdvancedFind;
        private bool? _initialCanModifyAdditionalSettings;

        private AttributeRequiredLevel? _initialRequiredLevel;

        private bool _initialIsSecured;

        private bool _initialIsDataSourceSecret;
        private bool _initialIsValidForForm;
        private bool _initialIsRequiredForForm;
        private bool _initialIsValidForGrid;

        public AttributeMetadataViewItem(AttributeMetadata attributeMetadata)
        {
            LoadMetadata(attributeMetadata);
        }

        public void LoadMetadata(AttributeMetadata attributeMetadata)
        {
            this.DisplayName = CreateFileHandler.GetLocalizedLabel(attributeMetadata.DisplayName);

            this.AttributeMetadata = attributeMetadata;

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
            if (_initialIsAuditEnabled != AttributeMetadata.IsAuditEnabled?.Value)
            {
                return true;
            }

            if (_initialIsGlobalFilterEnabled != AttributeMetadata.IsGlobalFilterEnabled?.Value)
            {
                return true;
            }

            if (_initialIsSortableEnabled != AttributeMetadata.IsSortableEnabled?.Value)
            {
                return true;
            }

            if (_initialIsCustomizable != AttributeMetadata.IsCustomizable?.Value)
            {
                return true;
            }

            if (_initialIsRenameable != AttributeMetadata.IsRenameable?.Value)
            {
                return true;
            }

            if (_initialRequiredLevel != AttributeMetadata.RequiredLevel?.Value)
            {
                return true;
            }

            if (_initialCanModifyAdditionalSettings != AttributeMetadata.CanModifyAdditionalSettings?.Value)
            {
                return true;
            }

            if (_initialIsValidForAdvancedFind != AttributeMetadata.IsValidForAdvancedFind?.Value)
            {
                return true;
            }

            if (_initialIsValidForForm != AttributeMetadata.IsValidForForm.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsRequiredForForm != AttributeMetadata.IsRequiredForForm.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsValidForGrid != AttributeMetadata.IsValidForGrid.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsDataSourceSecret != AttributeMetadata.IsDataSourceSecret.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsSecured != AttributeMetadata.IsSecured.GetValueOrDefault())
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

        public bool IsGlobalFilterEnabledCanBeChanged => (AttributeMetadata.IsGlobalFilterEnabled?.CanBeChanged).GetValueOrDefault();

        public bool IsGlobalFilterEnabled
        {
            get => (AttributeMetadata.IsGlobalFilterEnabled?.Value).GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsGlobalFilterEnabled == null || !AttributeMetadata.IsGlobalFilterEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsGlobalFilterEnabled));
                this.AttributeMetadata.IsGlobalFilterEnabled.Value = value;
                this.OnPropertyChanged(nameof(IsGlobalFilterEnabled));
            }
        }

        public bool IsSortableEnabledCanBeChanged => (AttributeMetadata.IsSortableEnabled?.CanBeChanged).GetValueOrDefault();

        public bool IsSortableEnabled
        {
            get => (AttributeMetadata.IsSortableEnabled?.Value).GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsSortableEnabled == null || !AttributeMetadata.IsSortableEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsSortableEnabled));
                this.AttributeMetadata.IsSortableEnabled.Value = value;
                this.OnPropertyChanged(nameof(IsSortableEnabled));
            }
        }

        public bool IsCustomizableCanBeChanged => (AttributeMetadata.IsCustomizable?.CanBeChanged).GetValueOrDefault();

        public bool IsCustomizable
        {
            get => (AttributeMetadata.IsCustomizable?.Value).GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsCustomizable == null || !AttributeMetadata.IsCustomizable.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsCustomizable));
                this.AttributeMetadata.IsCustomizable.Value = value;
                this.OnPropertyChanged(nameof(IsCustomizable));
            }
        }

        public bool IsRenameableCanBeChanged => (AttributeMetadata.IsRenameable?.CanBeChanged).GetValueOrDefault();

        public bool IsRenameable
        {
            get => (AttributeMetadata.IsRenameable?.Value).GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsRenameable == null || !AttributeMetadata.IsRenameable.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsRenameable));
                this.AttributeMetadata.IsRenameable.Value = value;
                this.OnPropertyChanged(nameof(IsRenameable));
            }
        }

        public bool IsValidForAdvancedFindCanBeChanged => (AttributeMetadata.IsValidForAdvancedFind?.CanBeChanged).GetValueOrDefault();

        public bool IsValidForAdvancedFind
        {
            get => (AttributeMetadata.IsValidForAdvancedFind?.Value).GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsValidForAdvancedFind == null || !AttributeMetadata.IsValidForAdvancedFind.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsValidForAdvancedFind));
                this.AttributeMetadata.IsValidForAdvancedFind.Value = value;
                this.OnPropertyChanged(nameof(IsValidForAdvancedFind));
            }
        }

        public bool IsValidForForm
        {
            get => AttributeMetadata.IsValidForForm.GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsValidForForm == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsValidForForm));
                this.AttributeMetadata.IsValidForForm = value;
                this.OnPropertyChanged(nameof(IsValidForForm));
            }
        }

        public bool IsRequiredForForm
        {
            get => AttributeMetadata.IsRequiredForForm.GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsRequiredForForm == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsRequiredForForm));
                this.AttributeMetadata.IsRequiredForForm = value;
                this.OnPropertyChanged(nameof(IsRequiredForForm));
            }
        }

        public bool IsValidForGrid
        {
            get => AttributeMetadata.IsValidForGrid.GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsValidForGrid == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsValidForGrid));
                this.AttributeMetadata.IsValidForGrid = value;
                this.OnPropertyChanged(nameof(IsValidForGrid));
            }
        }

        public bool RequiredLevelCanBeChanged => (AttributeMetadata.RequiredLevel?.CanBeChanged).GetValueOrDefault();

        public AttributeRequiredLevel RequiredLevel
        {
            get => (AttributeMetadata.RequiredLevel?.Value).GetValueOrDefault();
            set
            {
                if (AttributeMetadata.RequiredLevel == null || !AttributeMetadata.RequiredLevel.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(RequiredLevel));
                this.AttributeMetadata.RequiredLevel.Value = value;
                this.OnPropertyChanged(nameof(RequiredLevel));
            }
        }

        public bool CanModifyAdditionalSettingsCanBeChanged => (AttributeMetadata.CanModifyAdditionalSettings?.CanBeChanged).GetValueOrDefault();

        public bool CanModifyAdditionalSettings
        {
            get => (AttributeMetadata.CanModifyAdditionalSettings?.Value).GetValueOrDefault();
            set
            {
                if (AttributeMetadata.CanModifyAdditionalSettings == null || !AttributeMetadata.CanModifyAdditionalSettings.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanModifyAdditionalSettings));
                this.AttributeMetadata.CanModifyAdditionalSettings.Value = value;
                this.OnPropertyChanged(nameof(CanModifyAdditionalSettings));
            }
        }

        public bool IsDataSourceSecret
        {
            get => AttributeMetadata.IsDataSourceSecret.GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsDataSourceSecret == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsDataSourceSecret));
                this.AttributeMetadata.IsDataSourceSecret = value;
                this.OnPropertyChanged(nameof(IsDataSourceSecret));
            }
        }

        public bool IsSecured
        {
            get => AttributeMetadata.IsSecured.GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsSecured == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsSecured));
                this.AttributeMetadata.IsSecured = value;
                this.OnPropertyChanged(nameof(IsSecured));
            }
        }

        public bool IsAuditEnabledCanBeChanged => (AttributeMetadata.IsAuditEnabled?.CanBeChanged).GetValueOrDefault();

        public bool IsAuditEnabled
        {
            get => (AttributeMetadata.IsAuditEnabled?.Value).GetValueOrDefault();
            set
            {
                if (AttributeMetadata.IsAuditEnabled == null || !AttributeMetadata.IsAuditEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsAuditEnabled));
                this.AttributeMetadata.IsAuditEnabled.Value = value;
                this.OnPropertyChanged(nameof(IsAuditEnabled));
            }
        }
    }
}