using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class ExportSolutionProfile
    {
        private const int CountLastItems = 20;

        private string _Name;
        /// <summary>
        /// Название
        /// </summary>
        [DataMember]
        public string Name
        {
            get => _Name;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
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

        private bool _ExportAutoNumberingSettings;
        /// <summary>
        /// Экспортировать в решении настройки AutoNumbering
        /// </summary>
        [DataMember]
        public bool ExportAutoNumberingSettings
        {
            get => _ExportAutoNumberingSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportAutoNumberingSettings));
                this._ExportAutoNumberingSettings = value;
                this.OnPropertyChanged(nameof(ExportAutoNumberingSettings));
            }
        }

        private bool _ExportCalendarSettings;
        /// <summary>
        /// Экспортировать в решении настройки Calendar
        /// </summary>
        [DataMember]
        public bool ExportCalendarSettings
        {
            get => _ExportCalendarSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportCalendarSettings));
                this._ExportCalendarSettings = value;
                this.OnPropertyChanged(nameof(ExportCalendarSettings));
            }
        }

        private bool _ExportCustomizationSettings;
        /// <summary>
        /// Экспортировать в решении настройки Customization
        /// </summary>
        [DataMember]
        public bool ExportCustomizationSettings
        {
            get => _ExportCustomizationSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportCustomizationSettings));
                this._ExportCustomizationSettings = value;
                this.OnPropertyChanged(nameof(ExportCustomizationSettings));
            }
        }

        private bool _ExportEmailTrackingSettings;
        /// <summary>
        /// Экспортировать в решении настройки EmailTracking
        /// </summary>
        [DataMember]
        public bool ExportEmailTrackingSettings
        {
            get => _ExportEmailTrackingSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportEmailTrackingSettings));
                this._ExportEmailTrackingSettings = value;
                this.OnPropertyChanged(nameof(ExportEmailTrackingSettings));
            }
        }

        private bool _ExportExternalApplications;
        /// <summary>
        /// Экспортировать в решении настройки ExternalApplications
        /// </summary>
        [DataMember]
        public bool ExportExternalApplications
        {
            get => _ExportExternalApplications;
            set
            {
                this.OnPropertyChanging(nameof(ExportExternalApplications));
                this._ExportExternalApplications = value;
                this.OnPropertyChanged(nameof(ExportExternalApplications));
            }
        }

        private bool _ExportGeneralSettings;
        /// <summary>
        /// Экспортировать в решении настройки General
        /// </summary>
        [DataMember]
        public bool ExportGeneralSettings
        {
            get => _ExportGeneralSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportGeneralSettings));
                this._ExportGeneralSettings = value;
                this.OnPropertyChanged(nameof(ExportGeneralSettings));
            }
        }

        private bool _ExportIsvConfig;
        /// <summary>
        /// Экспортировать в решении настройки IsvConfig
        /// </summary>
        [DataMember]
        public bool ExportIsvConfig
        {
            get => _ExportIsvConfig;
            set
            {
                this.OnPropertyChanging(nameof(ExportIsvConfig));
                this._ExportIsvConfig = value;
                this.OnPropertyChanged(nameof(ExportIsvConfig));
            }
        }

        private bool _ExportMarketingSettings;
        /// <summary>
        /// Экспортировать в решении настройки Marketing
        /// </summary>
        [DataMember]
        public bool ExportMarketingSettings
        {
            get => _ExportMarketingSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportMarketingSettings));
                this._ExportMarketingSettings = value;
                this.OnPropertyChanged(nameof(ExportMarketingSettings));
            }
        }

        private bool _ExportOutlookSynchronizationSettings;
        /// <summary>
        /// Экспортировать в решении настройки OutlookSynchronization
        /// </summary>
        [DataMember]
        public bool ExportOutlookSynchronizationSettings
        {
            get => _ExportOutlookSynchronizationSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportOutlookSynchronizationSettings));
                this._ExportOutlookSynchronizationSettings = value;
                this.OnPropertyChanged(nameof(ExportOutlookSynchronizationSettings));
            }
        }

        private bool _ExportRelationshipRoles;
        /// <summary>
        /// Экспортировать в решении настройки RelationshipRoles
        /// </summary>
        [DataMember]
        public bool ExportRelationshipRoles
        {
            get => _ExportRelationshipRoles;
            set
            {
                this.OnPropertyChanging(nameof(ExportRelationshipRoles));
                this._ExportRelationshipRoles = value;
                this.OnPropertyChanged(nameof(ExportRelationshipRoles));
            }
        }

        private bool _ExportSales;
        /// <summary>
        /// Экспортировать в решении настройки Sales
        /// </summary>
        [DataMember]
        public bool ExportSales
        {
            get => _ExportSales;
            set
            {
                this.OnPropertyChanging(nameof(ExportSales));
                this._ExportSales = value;
                this.OnPropertyChanged(nameof(ExportSales));
            }
        }

        private string _ExportFolder;
        [DataMember]
        public string ExportFolder
        {
            get => _ExportFolder;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                value = value.Trim();

                if (_ExportFolder == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportFolder));
                this._ExportFolder = value;
                this.OnPropertyChanged(nameof(ExportFolder));
            }
        }

        private bool _IsOverrideSolutionNameAndVersion;
        [DataMember]
        public bool IsOverrideSolutionNameAndVersion
        {
            get => _IsOverrideSolutionNameAndVersion;
            set
            {
                if (_IsOverrideSolutionNameAndVersion == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsOverrideSolutionNameAndVersion));
                this._IsOverrideSolutionNameAndVersion = value;
                this.OnPropertyChanged(nameof(IsOverrideSolutionNameAndVersion));
            }
        }

        private bool _IsOverrideSolutionDescription;
        [DataMember]
        public bool IsOverrideSolutionDescription
        {
            get => _IsOverrideSolutionDescription;
            set
            {
                if (_IsOverrideSolutionDescription == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsOverrideSolutionDescription));
                this._IsOverrideSolutionDescription = value;
                this.OnPropertyChanged(nameof(IsOverrideSolutionDescription));
            }
        }

        private bool _IsCreateFolderForVersion;
        [DataMember]
        public bool IsCreateFolderForVersion
        {
            get => _IsCreateFolderForVersion;
            set
            {
                if (_IsCreateFolderForVersion == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsCreateFolderForVersion));
                this._IsCreateFolderForVersion = value;
                this.OnPropertyChanged(nameof(IsCreateFolderForVersion));
            }
        }

        private bool _IsCopyFileToClipBoard;
        [DataMember]
        public bool IsCopyFileToClipBoard
        {
            get => _IsCopyFileToClipBoard;
            set
            {
                if (_IsCopyFileToClipBoard == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsCopyFileToClipBoard));
                this._IsCopyFileToClipBoard = value;
                this.OnPropertyChanged(nameof(IsCopyFileToClipBoard));
            }
        }

        private string _OverrideUniqueName;
        [DataMember]
        public string OverrideUniqueName
        {
            get => _OverrideUniqueName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                if (_OverrideUniqueName == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(OverrideUniqueName));
                this._OverrideUniqueName = value;
                this.OnPropertyChanged(nameof(OverrideUniqueName));
            }
        }

        private string _OverrideDisplayName;
        [DataMember]
        public string OverrideDisplayName
        {
            get => _OverrideDisplayName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                if (_OverrideDisplayName == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(OverrideDisplayName));
                this._OverrideDisplayName = value;
                this.OnPropertyChanged(nameof(OverrideDisplayName));
            }
        }

        private string _OverrideVersion;
        [DataMember]
        public string OverrideVersion
        {
            get => _OverrideVersion;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                if (_OverrideVersion == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(OverrideVersion));
                this._OverrideVersion = value;
                this.OnPropertyChanged(nameof(OverrideVersion));
            }
        }

        private int _LastOverrideUniqueNameSelectedIndex = -1;
        [DataMember]
        public int LastOverrideUniqueNameSelectedIndex
        {
            get => _LastOverrideUniqueNameSelectedIndex;
            set
            {
                if (_LastOverrideUniqueNameSelectedIndex == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(LastOverrideUniqueNameSelectedIndex));
                this._LastOverrideUniqueNameSelectedIndex = value;
                this.OnPropertyChanged(nameof(LastOverrideUniqueNameSelectedIndex));
            }
        }

        private int _LastOverrideDisplayNameSelectedIndex = -1;
        [DataMember]
        public int LastOverrideDisplayNameSelectedIndex
        {
            get => _LastOverrideDisplayNameSelectedIndex;
            set
            {
                if (_LastOverrideDisplayNameSelectedIndex == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(LastOverrideDisplayNameSelectedIndex));
                this._LastOverrideDisplayNameSelectedIndex = value;
                this.OnPropertyChanged(nameof(LastOverrideDisplayNameSelectedIndex));
            }
        }

        private int _LastOverrideVersionSelectedIndex = -1;
        [DataMember]
        public int LastOverrideVersionSelectedIndex
        {
            get => _LastOverrideVersionSelectedIndex;
            set
            {
                if (_LastOverrideVersionSelectedIndex == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(LastOverrideVersionSelectedIndex));
                this._LastOverrideVersionSelectedIndex = value;
                this.OnPropertyChanged(nameof(LastOverrideVersionSelectedIndex));
            }
        }

        private string _OverrideDescription;
        [DataMember]
        public string OverrideDescription
        {
            get => _OverrideDescription;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                if (_OverrideDescription == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(OverrideDescription));
                this._OverrideDescription = value;
                this.OnPropertyChanged(nameof(OverrideDescription));
            }
        }

        private bool _IsManaged;
        [DataMember]
        public bool IsManaged
        {
            get => _IsManaged;
            set
            {
                this.OnPropertyChanging(nameof(IsManaged));
                this._IsManaged = value;
                this.OnPropertyChanged(nameof(IsManaged));
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

        [DataMember]
        public ObservableCollection<string> LastOverrideUniqueName { get; private set; }

        [DataMember]
        public ObservableCollection<string> LastOverrideDisplayName { get; private set; }

        [DataMember]
        public ObservableCollection<string> LastOverrideVersion { get; private set; }

        public ExportSolutionProfile()
        {
            this.LastOverrideUniqueName = new ObservableCollection<string>();
            this.LastOverrideDisplayName = new ObservableCollection<string>();
            this.LastOverrideVersion = new ObservableCollection<string>();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (this.LastOverrideUniqueName == null)
            {
                this.LastOverrideUniqueName = new ObservableCollection<string>();
            }

            if (this.LastOverrideDisplayName == null)
            {
                this.LastOverrideDisplayName = new ObservableCollection<string>();
            }

            if (this.LastOverrideVersion == null)
            {
                this.LastOverrideVersion = new ObservableCollection<string>();
            }
        }

        public void AddLastOverrideUniqueName(string uniqueName)
        {
            if (string.IsNullOrEmpty(uniqueName))
            {
                return;
            }

            if (this.LastOverrideUniqueName.Contains(uniqueName))
            {
                this.LastOverrideUniqueName.Move(LastOverrideUniqueName.IndexOf(uniqueName), 0);
            }
            else
            {
                this.LastOverrideUniqueName.Insert(0, uniqueName);

                while (this.LastOverrideUniqueName.Count > CountLastItems)
                {
                    this.LastOverrideUniqueName.RemoveAt(this.LastOverrideUniqueName.Count - 1);
                }
            }
        }

        public void AddLastOverrideDisplayName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                return;
            }

            if (this.LastOverrideDisplayName.Contains(displayName))
            {
                this.LastOverrideDisplayName.Move(LastOverrideDisplayName.IndexOf(displayName), 0);
            }
            else
            {
                this.LastOverrideDisplayName.Insert(0, displayName);

                while (this.LastOverrideDisplayName.Count > CountLastItems)
                {
                    this.LastOverrideDisplayName.RemoveAt(this.LastOverrideDisplayName.Count - 1);
                }
            }
        }

        public void AddLastOverrideVersion(string newVersion, string oldVersion)
        {
            if (this.LastOverrideVersion.Contains(newVersion))
            {
                this.LastOverrideVersion.Move(LastOverrideVersion.IndexOf(newVersion), 0);

                if (this.LastOverrideVersion.Contains(oldVersion))
                {
                    this.LastOverrideVersion.Remove(oldVersion);
                }
            }
            else if (this.LastOverrideVersion.Contains(oldVersion))
            {
                this.LastOverrideVersion.Move(LastOverrideVersion.IndexOf(oldVersion), 0);

                this.LastOverrideVersion[0] = newVersion;
            }
            else
            {
                this.LastOverrideVersion.Insert(0, newVersion);
            }

            while (this.LastOverrideVersion.Count > CountLastItems)
            {
                this.LastOverrideVersion.RemoveAt(this.LastOverrideVersion.Count - 1);
            }
        }

        public override string ToString() => Name;
    }
}