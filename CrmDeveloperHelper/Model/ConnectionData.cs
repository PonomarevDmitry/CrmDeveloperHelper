using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    /// <summary>
    /// Данные для сохранения веб-ресурсов в CRM
    /// </summary>
    [DataContract]
    public partial class ConnectionData : INotifyPropertyChanging, INotifyPropertyChanged
    {
        internal const int CountConnectionToQuickList = 40;

        internal const int CountLastSolutions = 15;

        private const int CountLastItems = 20;

        private object _syncObjectAttributes = new object();

        private object _syncObjectIntellisense = new object();

        private object _syncObjectRequests = new object();

        public ConnectionConfiguration ConnectionConfiguration { get; set; }

        private bool _IsCurrentConnection;

        public bool IsCurrentConnection
        {
            get => _IsCurrentConnection;
            set
            {
                if (_IsCurrentConnection == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsCurrentConnection));
                this._IsCurrentConnection = value;
                this.OnPropertyChanged(nameof(IsCurrentConnection));
            }
        }

        /// <summary>
        /// Уникальный номер
        /// </summary>
        [DataMember]
        public Guid ConnectionId { get; set; }

        private bool _IsReadOnly;
        /// <summary>
        /// Подключение только для чтения
        /// </summary>
        [DataMember]
        public bool IsReadOnly
        {
            get => _IsReadOnly;
            set
            {
                if (_IsReadOnly == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsReadOnly));
                this._IsReadOnly = value;
                this.OnPropertyChanged(nameof(IsReadOnly));
            }
        }

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

        public string NameWithCurrentMark => Name + (this.IsCurrentConnection ? " (Current)" : string.Empty);

        private string _GroupName;
        /// <summary>
        /// Название
        /// </summary>
        [DataMember]
        public string GroupName
        {
            get => _GroupName;
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

                this.OnPropertyChanging(nameof(GroupName));
                this._GroupName = value;
                this.OnPropertyChanged(nameof(GroupName));
            }
        }

        private string _DiscoveryUrl;
        /// <summary>
        /// Url для сервиса Discovery
        /// Пример https://CRM-host/XRMServices/2011/Discovery.svc
        /// </summary>
        [DataMember]
        public string DiscoveryUrl
        {
            get => _DiscoveryUrl;
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

                if (_DiscoveryUrl == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(DiscoveryUrl));
                this._DiscoveryUrl = value;
                this.OnPropertyChanged(nameof(DiscoveryUrl));
            }
        }

        private string _UniqueOrgName;
        /// <summary>
        /// Организация в CRM
        /// </summary>
        [DataMember]
        public string UniqueOrgName
        {
            get => _UniqueOrgName;
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

                if (_UniqueOrgName == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(UniqueOrgName));
                this._UniqueOrgName = value;
                this.OnPropertyChanged(nameof(UniqueOrgName));
            }
        }

        private string _OrganizationUrl;
        /// <summary>
        /// Url для сервиса Organization
        /// </summary>
        [DataMember]
        public string OrganizationUrl
        {
            get => _OrganizationUrl;
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

                if (_OrganizationUrl == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(OrganizationUrl));
                this._OrganizationUrl = value;
                this.OnPropertyChanged(nameof(OrganizationUrl));
            }
        }

        private string _PublicUrl;
        /// <summary>
        /// Публичный url для доступа к CRM
        /// </summary>
        [DataMember]
        public string PublicUrl
        {
            get => _PublicUrl;
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

                if (_PublicUrl == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(PublicUrl));
                this._PublicUrl = value;
                this.OnPropertyChanged(nameof(PublicUrl));
            }
        }

        private string _OrganizationVersion;
        /// <summary>
        /// Версия организации
        /// </summary>
        [DataMember]
        public string OrganizationVersion
        {
            get => _OrganizationVersion;
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

                if (_OrganizationVersion == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(OrganizationVersion));
                this._OrganizationVersion = value;
                this.OnPropertyChanged(nameof(OrganizationVersion));
            }
        }

        private string _FriendlyName;
        [DataMember]
        public string FriendlyName
        {
            get => _FriendlyName;
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

                if (_FriendlyName == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ToolTip));
                this.OnPropertyChanging(nameof(FriendlyName));
                this._FriendlyName = value;
                this.OnPropertyChanged(nameof(FriendlyName));
                this.OnPropertyChanged(nameof(ToolTip));
            }
        }

        private Guid? _OrganizationId;
        [DataMember]
        public Guid? OrganizationId
        {
            get => _OrganizationId;
            set
            {
                if (_OrganizationId == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ToolTip));
                this.OnPropertyChanging(nameof(OrganizationId));
                this._OrganizationId = value;
                this.OnPropertyChanged(nameof(OrganizationId));
                this.OnPropertyChanged(nameof(ToolTip));
            }
        }

        private string _OrganizationState;
        [DataMember]
        public string OrganizationState
        {
            get => _OrganizationState;
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

                if (_OrganizationState == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ToolTip));
                this.OnPropertyChanging(nameof(OrganizationState));
                this._OrganizationState = value;
                this.OnPropertyChanged(nameof(OrganizationState));
                this.OnPropertyChanged(nameof(ToolTip));
            }
        }

        private string _UrlName;
        [DataMember]
        public string UrlName
        {
            get => _UrlName;
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

                if (_UrlName == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ToolTip));
                this.OnPropertyChanging(nameof(UrlName));
                this._UrlName = value;
                this.OnPropertyChanged(nameof(UrlName));
                this.OnPropertyChanged(nameof(ToolTip));
            }
        }

        private string _DefaultLanguage;
        [DataMember]
        public string DefaultLanguage
        {
            get => _DefaultLanguage;
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

                if (_DefaultLanguage == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ToolTip));
                this.OnPropertyChanging(nameof(DefaultLanguage));
                this._DefaultLanguage = value;
                this.OnPropertyChanged(nameof(DefaultLanguage));
                this.OnPropertyChanged(nameof(ToolTip));
            }
        }

        private string _InstalledLanguagePacks;
        [DataMember]
        public string InstalledLanguagePacks
        {
            get => _InstalledLanguagePacks;
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

                if (_InstalledLanguagePacks == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ToolTip));
                this.OnPropertyChanging(nameof(InstalledLanguagePacks));
                this._InstalledLanguagePacks = value;
                this.OnPropertyChanged(nameof(InstalledLanguagePacks));
                this.OnPropertyChanged(nameof(ToolTip));
            }
        }

        private string _BaseCurrency;
        [DataMember]
        public string BaseCurrency
        {
            get => _BaseCurrency;
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

                if (_BaseCurrency == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ToolTip));
                this.OnPropertyChanging(nameof(BaseCurrency));
                this._BaseCurrency = value;
                this.OnPropertyChanged(nameof(BaseCurrency));
                this.OnPropertyChanged(nameof(ToolTip));
            }
        }

        private string _NameSpaceClasses;
        /// <summary>
        /// Пространство имен для класса с метаданными сущности
        /// </summary>
        [DataMember]
        public string NameSpaceClasses
        {
            get => _NameSpaceClasses;
            set
            {
                this.OnPropertyChanging(nameof(NameSpaceClasses));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NameSpaceClasses = value;
                this.OnPropertyChanged(nameof(NameSpaceClasses));
            }
        }

        private string _NameSpaceOptionSets;
        [DataMember]
        public string NameSpaceOptionSets
        {
            get => _NameSpaceOptionSets;
            set
            {
                this.OnPropertyChanging(nameof(NameSpaceOptionSets));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NameSpaceOptionSets = value;
                this.OnPropertyChanged(nameof(NameSpaceOptionSets));
            }
        }

        private string _ServiceContextName;
        [DataMember]
        public string ServiceContextName
        {
            get => _ServiceContextName;
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

                if (_ServiceContextName == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ServiceContextName));
                this._ServiceContextName = value;
                this.OnPropertyChanged(nameof(ServiceContextName));
            }
        }

        private bool _InteractiveLogin;
        [DataMember]
        public bool InteractiveLogin
        {
            get => _InteractiveLogin;
            set
            {
                if (_InteractiveLogin == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(InteractiveLogin));
                this._InteractiveLogin = value;
                this.OnPropertyChanged(nameof(InteractiveLogin));
            }
        }

        private bool _GenerateActions;
        [DataMember]
        public bool GenerateActions
        {
            get => _GenerateActions;
            set
            {
                if (_GenerateActions == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(GenerateActions));
                this._GenerateActions = value;
                this.OnPropertyChanged(nameof(GenerateActions));
            }
        }

        private Guid? _SelectedCrmSvcUtil;
        [DataMember]
        public Guid? SelectedCrmSvcUtil
        {
            get => _SelectedCrmSvcUtil;
            set
            {
                if (_SelectedCrmSvcUtil == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(SelectedCrmSvcUtil));
                this._SelectedCrmSvcUtil = value;
                this.OnPropertyChanged(nameof(SelectedCrmSvcUtil));
            }
        }

        private string _SelectSolutionFilter;
        [DataMember]
        public string SelectSolutionFilter
        {
            get => _SelectSolutionFilter;
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

                if (_SelectSolutionFilter == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(SelectSolutionFilter));
                this._SelectSolutionFilter = value;
                this.OnPropertyChanged(nameof(SelectSolutionFilter));
            }
        }

        private string _ExplorerSolutionFilter;
        [DataMember]
        public string ExplorerSolutionFilter
        {
            get => _ExplorerSolutionFilter;
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

                if (_ExplorerSolutionFilter == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExplorerSolutionFilter));
                this._ExplorerSolutionFilter = value;
                this.OnPropertyChanged(nameof(ExplorerSolutionFilter));
            }
        }

        private string _ExportSolutionFolder;
        [DataMember]
        public string ExportSolutionFolder
        {
            get => _ExportSolutionFolder;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                value = value.Trim();

                if (_ExportSolutionFolder == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionFolder));
                this._ExportSolutionFolder = value;
                this.OnPropertyChanged(nameof(ExportSolutionFolder));
            }
        }

        private bool _ExportSolutionIsOverrideSolutionNameAndVersion;
        [DataMember]
        public bool ExportSolutionIsOverrideSolutionNameAndVersion
        {
            get => _ExportSolutionIsOverrideSolutionNameAndVersion;
            set
            {
                if (_ExportSolutionIsOverrideSolutionNameAndVersion == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionIsOverrideSolutionNameAndVersion));
                this._ExportSolutionIsOverrideSolutionNameAndVersion = value;
                this.OnPropertyChanged(nameof(ExportSolutionIsOverrideSolutionNameAndVersion));
            }
        }

        private bool _ExportSolutionIsOverrideSolutionDescription;
        [DataMember]
        public bool ExportSolutionIsOverrideSolutionDescription
        {
            get => _ExportSolutionIsOverrideSolutionDescription;
            set
            {
                if (_ExportSolutionIsOverrideSolutionDescription == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionIsOverrideSolutionDescription));
                this._ExportSolutionIsOverrideSolutionDescription = value;
                this.OnPropertyChanged(nameof(ExportSolutionIsOverrideSolutionDescription));
            }
        }

        private bool _ExportSolutionIsCreateFolderForVersion;
        [DataMember]
        public bool ExportSolutionIsCreateFolderForVersion
        {
            get => _ExportSolutionIsCreateFolderForVersion;
            set
            {
                if (_ExportSolutionIsCreateFolderForVersion == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionIsCreateFolderForVersion));
                this._ExportSolutionIsCreateFolderForVersion = value;
                this.OnPropertyChanged(nameof(ExportSolutionIsCreateFolderForVersion));
            }
        }

        private bool _ExportSolutionIsCopyFileToClipBoard;
        [DataMember]
        public bool ExportSolutionIsCopyFileToClipBoard
        {
            get => _ExportSolutionIsCopyFileToClipBoard;
            set
            {
                if (_ExportSolutionIsCopyFileToClipBoard == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionIsCopyFileToClipBoard));
                this._ExportSolutionIsCopyFileToClipBoard = value;
                this.OnPropertyChanged(nameof(ExportSolutionIsCopyFileToClipBoard));
            }
        }

        private string _ExportSolutionOverrideUniqueName;
        [DataMember]
        public string ExportSolutionOverrideUniqueName
        {
            get => _ExportSolutionOverrideUniqueName;
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

                if (_ExportSolutionOverrideUniqueName == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionOverrideUniqueName));
                this._ExportSolutionOverrideUniqueName = value;
                this.OnPropertyChanged(nameof(ExportSolutionOverrideUniqueName));
            }
        }

        private string _ExportSolutionOverrideDisplayName;
        [DataMember]
        public string ExportSolutionOverrideDisplayName
        {
            get => _ExportSolutionOverrideDisplayName;
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

                if (_ExportSolutionOverrideDisplayName == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionOverrideDisplayName));
                this._ExportSolutionOverrideDisplayName = value;
                this.OnPropertyChanged(nameof(ExportSolutionOverrideDisplayName));
            }
        }

        private string _ExportSolutionOverrideVersion;
        [DataMember]
        public string ExportSolutionOverrideVersion
        {
            get => _ExportSolutionOverrideVersion;
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

                if (_ExportSolutionOverrideVersion == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionOverrideVersion));
                this._ExportSolutionOverrideVersion = value;
                this.OnPropertyChanged(nameof(ExportSolutionOverrideVersion));
            }
        }

        private string _ExportSolutionOverrideDescription;
        [DataMember]
        public string ExportSolutionOverrideDescription
        {
            get => _ExportSolutionOverrideDescription;
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

                if (_ExportSolutionOverrideDescription == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionOverrideDescription));
                this._ExportSolutionOverrideDescription = value;
                this.OnPropertyChanged(nameof(ExportSolutionOverrideDescription));
            }
        }

        private bool _ExportSolutionManaged;
        [DataMember]
        public bool ExportSolutionManaged
        {
            get
            {
                return _ExportSolutionManaged;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionManaged));
                this._ExportSolutionManaged = value;
                this.OnPropertyChanged(nameof(ExportSolutionManaged));
            }
        }

        private string _TraceReaderFilter;
        [DataMember]
        public string TraceReaderFilter
        {
            get => _TraceReaderFilter;
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

                if (_TraceReaderFilter == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(TraceReaderFilter));
                this._TraceReaderFilter = value;
                this.OnPropertyChanged(nameof(TraceReaderFilter));
            }
        }

        private string _TraceReaderFolder;
        [DataMember]
        public string TraceReaderFolder
        {
            get => _TraceReaderFolder;
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

                if (_TraceReaderFolder == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(TraceReaderFolder));
                this._TraceReaderFolder = value;
                this.OnPropertyChanged(nameof(TraceReaderFolder));
            }
        }

        [DataMember]
        public ObservableCollection<string> TraceReaderLastFilters { get; private set; }

        [DataMember]
        public ObservableCollection<string> TraceReaderLastFolders { get; private set; }

        [DataMember]
        public ObservableCollection<string> TraceReaderSelectedFolders { get; private set; }

        private Guid? _UserId;
        [DataMember]
        public Guid? UserId
        {
            get => _UserId;
            set
            {
                if (_UserId == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(GetUsername));
                this.OnPropertyChanging(nameof(User));
                this.OnPropertyChanging(nameof(UserId));
                this._UserId = value;
                this.OnPropertyChanged(nameof(UserId));
                this.OnPropertyChanged(nameof(User));
                this.OnPropertyChanged(nameof(GetUsername));
            }
        }

        [DataMember]
        public DateTime? OrganizationInformationExpirationDate;

        private ConnectionUserData _user;

        public ConnectionUserData User
        {
            get => _user;
            set
            {
                if (_user == value)
                {
                    return;
                }

                _user = value;
                UserId = value?.UserId;
            }
        }

        /// <summary>
        /// Кэшированные идетификаторы веб-ресурсов
        /// </summary>
        [DataMember]
        public List<FileMapping> Mappings { get; private set; }

        [DataMember]
        public List<AssemblyMapping> AssemblyMappings { get; private set; }

        [DataMember]
        public ObservableCollection<string> LastSelectedSolutionsUniqueName { get; private set; }

        [DataMember]
        public ObservableCollection<string> LastSolutionExportFolders { get; private set; }

        [DataMember]
        public ObservableCollection<string> LastExportSolutionOverrideUniqueName { get; private set; }

        [DataMember]
        public ObservableCollection<string> LastExportSolutionOverrideDisplayName { get; private set; }

        [DataMember]
        public ObservableCollection<string> LastExportSolutionOverrideVersion { get; private set; }

        [DataMember]
        public ObservableCollection<FetchXmlRequestParameter> FetchXmlRequestParameterList { get; private set; }

        private ConcurrentDictionary<string, bool> _knownRequests = new ConcurrentDictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Конструктор данных
        /// </summary>
        public ConnectionData()
        {
            this.Mappings = new List<FileMapping>();
            this.AssemblyMappings = new List<AssemblyMapping>();

            this.LastSelectedSolutionsUniqueName = new ObservableCollection<string>();
            this.LastSolutionExportFolders = new ObservableCollection<string>();
            this.LastExportSolutionOverrideUniqueName = new ObservableCollection<string>();
            this.LastExportSolutionOverrideDisplayName = new ObservableCollection<string>();
            this.LastExportSolutionOverrideVersion = new ObservableCollection<string>();
            this.FetchXmlRequestParameterList = new ObservableCollection<FetchXmlRequestParameter>();

            this.TraceReaderLastFilters = new ObservableCollection<string>();
            this.TraceReaderLastFolders = new ObservableCollection<string>();
            this.TraceReaderSelectedFolders = new ObservableCollection<string>();

            this.NameSpaceClasses = nameof(NameSpaceClasses);
            this.NameSpaceOptionSets = nameof(NameSpaceOptionSets);
            this.ServiceContextName = "XrmServiceContext";
        }

        [OnSerializing]
        private void BeforeSerializing(StreamingContext context)
        {
            if (this.ConnectionId == Guid.Empty)
            {
                this.ConnectionId = Guid.NewGuid();
            }
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (_syncObjectAttributes == null) { _syncObjectAttributes = new object(); }
            if (_syncObjectIntellisense == null) { _syncObjectIntellisense = new object(); }
            if (_syncObjectRequests == null) { _syncObjectRequests = new object(); }

            lock (_syncObjectRequests)
            {
                if (_knownRequests == null)
                {
                    _knownRequests = new ConcurrentDictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
                }
            }

            if (this.Mappings == null)
            {
                this.Mappings = new List<FileMapping>();
            }

            if (this.AssemblyMappings == null)
            {
                this.AssemblyMappings = new List<AssemblyMapping>();
            }

            if (this.LastSelectedSolutionsUniqueName == null)
            {
                this.LastSelectedSolutionsUniqueName = new ObservableCollection<string>();
            }

            if (this.LastSolutionExportFolders == null)
            {
                this.LastSolutionExportFolders = new ObservableCollection<string>();
            }

            if (this.LastExportSolutionOverrideUniqueName == null)
            {
                this.LastExportSolutionOverrideUniqueName = new ObservableCollection<string>();
            }

            if (this.LastExportSolutionOverrideDisplayName == null)
            {
                this.LastExportSolutionOverrideDisplayName = new ObservableCollection<string>();
            }

            if (this.LastExportSolutionOverrideVersion == null)
            {
                this.LastExportSolutionOverrideVersion = new ObservableCollection<string>();
            }

            if (this.FetchXmlRequestParameterList == null)
            {
                this.FetchXmlRequestParameterList = new ObservableCollection<FetchXmlRequestParameter>();
            }

            if (this.TraceReaderLastFilters == null)
            {
                this.TraceReaderLastFilters = new ObservableCollection<string>();
            }

            if (this.TraceReaderLastFolders == null)
            {
                this.TraceReaderLastFolders = new ObservableCollection<string>();
            }

            if (this.TraceReaderSelectedFolders == null)
            {
                this.TraceReaderSelectedFolders = new ObservableCollection<string>();
            }
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            if (this.ConnectionId == Guid.Empty)
            {
                this.ConnectionId = Guid.NewGuid();
            }

            var data = ConnectionIntellisenseData.Get(this.ConnectionId);

            if (data != null)
            {
                this.IntellisenseData.MergeDataFromDisk(data);
            }

            IntellisenseData.ConnectionId = this.ConnectionId;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanging(string propertyName)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public ConnectionData CreateCopy()
        {
            ConnectionData result = new ConnectionData
            {
                GroupName = this.GroupName,

                DiscoveryUrl = this.DiscoveryUrl,
                OrganizationUrl = this.OrganizationUrl,
                UniqueOrgName = this.UniqueOrgName,

                NameSpaceClasses = this.NameSpaceClasses,
                NameSpaceOptionSets = this.NameSpaceOptionSets,
                ServiceContextName = this.ServiceContextName,

                ConnectionConfiguration = this.ConnectionConfiguration,

                UserId = this.UserId,
                User = this.User,

                ConnectionId = Guid.NewGuid()
            };

            return result;
        }

        public void AddLastSelectedSolution(string solutionUniqueName)
        {
            if (string.IsNullOrEmpty(solutionUniqueName))
            {
                return;
            }

            if (this.LastSelectedSolutionsUniqueName.Contains(solutionUniqueName))
            {
                this.LastSelectedSolutionsUniqueName.Move(LastSelectedSolutionsUniqueName.IndexOf(solutionUniqueName), 0);
            }
            else
            {
                this.LastSelectedSolutionsUniqueName.Insert(0, solutionUniqueName);

                while (this.LastSelectedSolutionsUniqueName.Count > CountLastSolutions)
                {
                    this.LastSelectedSolutionsUniqueName.RemoveAt(this.LastSelectedSolutionsUniqueName.Count - 1);
                }
            }
        }

        public void AddTraceFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return;
            }

            if (this.TraceReaderLastFilters.Contains(filter))
            {
                this.TraceReaderLastFilters.Move(TraceReaderLastFilters.IndexOf(filter), 0);
            }
            else
            {
                this.TraceReaderLastFilters.Insert(0, filter);

                while (this.TraceReaderLastFilters.Count > CountLastItems)
                {
                    this.TraceReaderLastFilters.RemoveAt(this.TraceReaderLastFilters.Count - 1);
                }
            }
        }

        public void AddTraceLastFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (this.TraceReaderLastFolders.Contains(folder))
            {
                this.TraceReaderLastFolders.Move(TraceReaderLastFolders.IndexOf(folder), 0);
            }
            else
            {
                this.TraceReaderLastFolders.Insert(0, folder);

                while (this.TraceReaderLastFolders.Count > CountLastItems)
                {
                    this.TraceReaderLastFolders.RemoveAt(this.TraceReaderLastFolders.Count - 1);
                }
            }
        }

        public void AddLastSolutionExportFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            if (this.LastSolutionExportFolders.Contains(folderPath))
            {
                this.LastSolutionExportFolders.Move(LastSolutionExportFolders.IndexOf(folderPath), 0);
            }
            else
            {
                this.LastSolutionExportFolders.Insert(0, folderPath);

                while (this.LastSolutionExportFolders.Count > CountLastItems)
                {
                    this.LastSolutionExportFolders.RemoveAt(this.LastSolutionExportFolders.Count - 1);
                }
            }
        }

        public void AddLastExportSolutionOverrideUniqueName(string uniqueName)
        {
            if (string.IsNullOrEmpty(uniqueName))
            {
                return;
            }

            if (this.LastExportSolutionOverrideUniqueName.Contains(uniqueName))
            {
                this.LastExportSolutionOverrideUniqueName.Move(LastExportSolutionOverrideUniqueName.IndexOf(uniqueName), 0);
            }
            else
            {
                this.LastExportSolutionOverrideUniqueName.Insert(0, uniqueName);

                while (this.LastExportSolutionOverrideUniqueName.Count > CountLastItems)
                {
                    this.LastExportSolutionOverrideUniqueName.RemoveAt(this.LastExportSolutionOverrideUniqueName.Count - 1);
                }
            }
        }

        public void AddLastExportSolutionOverrideDisplayName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                return;
            }

            if (this.LastExportSolutionOverrideDisplayName.Contains(displayName))
            {
                this.LastExportSolutionOverrideDisplayName.Move(LastExportSolutionOverrideDisplayName.IndexOf(displayName), 0);
            }
            else
            {
                this.LastExportSolutionOverrideDisplayName.Insert(0, displayName);

                while (this.LastExportSolutionOverrideDisplayName.Count > CountLastItems)
                {
                    this.LastExportSolutionOverrideDisplayName.RemoveAt(this.LastExportSolutionOverrideDisplayName.Count - 1);
                }
            }
        }

        public void AddLastExportSolutionOverrideVersion(string newVersion, string oldVersion)
        {
            if (this.LastExportSolutionOverrideVersion.Contains(newVersion))
            {
                this.LastExportSolutionOverrideVersion.Move(LastExportSolutionOverrideVersion.IndexOf(newVersion), 0);

                if (this.LastExportSolutionOverrideVersion.Contains(oldVersion))
                {
                    this.LastExportSolutionOverrideVersion.Remove(oldVersion);
                }
            }
            else if (this.LastExportSolutionOverrideVersion.Contains(oldVersion))
            {
                this.LastExportSolutionOverrideVersion.Move(LastExportSolutionOverrideVersion.IndexOf(oldVersion), 0);

                this.LastExportSolutionOverrideVersion[0] = newVersion;
            }
            else
            {
                this.LastExportSolutionOverrideVersion.Insert(0, newVersion);
            }

            while (this.LastExportSolutionOverrideVersion.Count > CountLastItems)
            {
                this.LastExportSolutionOverrideVersion.RemoveAt(this.LastExportSolutionOverrideVersion.Count - 1);
            }
        }

        /// <summary>
        /// Добавляем или обновляем идентификатор объекта в CRM
        /// </summary>
        /// <param name="crmObjectId"></param>
        /// <param name="friendlyPath"></param>
        public void AddMapping(Guid crmObjectId, string friendlyPath)
        {
            if (crmObjectId == Guid.Empty)
                return;

            var mapping = this.Mappings.FirstOrDefault(x => x.SourceFilePath.Equals(friendlyPath, StringComparison.InvariantCultureIgnoreCase));

            if (mapping != null)
            {
                mapping.CRMObjectId = crmObjectId;
            }
            else
            {
                this.Mappings.Add(new FileMapping()
                {
                    CRMObjectId = crmObjectId,
                    SourceFilePath = friendlyPath
                });
            }
        }

        public bool RemoveMapping(string friendlyPath)
        {
            bool result = false;

            if (string.IsNullOrEmpty(friendlyPath))
            {
                return result;
            }

            var mapping = this.Mappings.FirstOrDefault(x => x.SourceFilePath.Equals(friendlyPath, StringComparison.InvariantCultureIgnoreCase));

            if (mapping != null)
            {
                this.Mappings.Remove(mapping);

                result = true;
            }

            return result;
        }

        public Guid? GetLastLinkForFile(string friendlyPath)
        {
            Guid? result = null;

            Guid objectId = Guid.Empty;

            if (GetGuidByPath(friendlyPath, out objectId))
            {
                result = objectId;
            }

            return result;
        }

        public void AddAssemblyMapping(string assemblyName, string localAssemblyPath)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                return;
            }

            if (string.IsNullOrEmpty(localAssemblyPath))
            {
                return;
            }

            var mapping = this.AssemblyMappings.FirstOrDefault(x => x.AssemblyName.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            if (mapping != null)
            {
                mapping.LocalAssemblyPath = localAssemblyPath;
            }
            else
            {
                this.AssemblyMappings.Add(new AssemblyMapping()
                {
                    AssemblyName = assemblyName,
                    LocalAssemblyPath = localAssemblyPath,
                });
            }
        }

        public bool RemoveAssemblyMapping(string assemblyName)
        {
            bool result = false;

            if (string.IsNullOrEmpty(assemblyName))
            {
                return result;
            }

            var mapping = this.AssemblyMappings.FirstOrDefault(x => x.AssemblyName.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            if (mapping != null)
            {
                this.AssemblyMappings.Remove(mapping);

                result = true;
            }

            return result;
        }

        public HashSet<string> GetEntityAttributes(string entityName)
        {
            lock (_syncObjectAttributes)
            {
                var intellisense = this.IntellisenseData;

                if (intellisense.Entities != null
                    && intellisense.Entities.ContainsKey(entityName)
                    && intellisense.Entities[entityName].Attributes != null
                    && intellisense.Entities[entityName].Attributes.Any()
                    )
                {
                    var keys = intellisense.Entities[entityName].Attributes.Keys.ToList();

                    return new HashSet<string>(keys);
                }
            }

            return null;
        }

        public EntityIntellisenseData GetEntityIntellisenseData(string entityName)
        {
            lock (_syncObjectAttributes)
            {
                var intellisense = this.IntellisenseData;

                if (intellisense.Entities != null
                    && intellisense.Entities.ContainsKey(entityName)
                    && intellisense.Entities[entityName].Attributes != null
                    && intellisense.Entities[entityName].Attributes.Any()
                    )
                {
                    return intellisense.Entities[entityName];
                }
            }

            return null;
        }

        public bool? IsRequestExists(string requestName)
        {
            lock (_syncObjectRequests)
            {
                if (_knownRequests.ContainsKey(requestName))
                {
                    return _knownRequests[requestName];
                }
            }

            return null;
        }

        public void SetRequestExistance(string requestName, bool isRequestExists)
        {
            lock (_syncObjectRequests)
            {
                _knownRequests[requestName] = isRequestExists;
            }
        }

        public string GetLastAssemblyPath(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                return null;
            }

            var mapping = this.AssemblyMappings.FirstOrDefault(x => x.AssemblyName.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            return mapping?.LocalAssemblyPath;
        }

        private bool GetGuidByPath(string friendlyPath, out Guid webResourceId)
        {
            bool result = false;

            webResourceId = Guid.Empty;

            var mapping = this.Mappings.FirstOrDefault(x => x.SourceFilePath.Equals(friendlyPath, StringComparison.InvariantCultureIgnoreCase));

            if (mapping != null)
            {
                webResourceId = mapping.CRMObjectId;

                result = true;
            }

            return result;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                return this.Name;
            }
            else
            {
                return base.ToString();
            }
        }

        public string GetDescription()
        {
            string user = GetUsername;
            string publicUrl = GetPublicUrl();
            string version = this.OrganizationVersion;
            if (string.IsNullOrEmpty(version))
            {
                version = "unknown";
            }

            string readOnly = string.Empty;

            if (IsReadOnly)
            {
                readOnly = "    ReadOnly";
            }

            return string.Format("Name: {0}    {1}    User: {2}    Version: {3}{4}", this.Name, publicUrl, user, version, readOnly);
        }

        public void LoadReadOnlyInformation(ConnectionData connectionData)
        {
            this.PublicUrl = connectionData.PublicUrl;
            this.OrganizationVersion = connectionData.OrganizationVersion;
            this.FriendlyName = connectionData.FriendlyName;
            this.OrganizationId = connectionData.OrganizationId;
            this.OrganizationState = connectionData.OrganizationState;
            this.UrlName = connectionData.UrlName;
        }

        public string GetDescriptionColumn()
        {
            string user = GetUsername;

            string publicUrl = GetPublicUrl();

            string version = this.OrganizationVersion;

            if (string.IsNullOrEmpty(version))
            {
                version = "unknown";
            }

            string readOnly = string.Empty;

            if (this.IsReadOnly)
            {
                readOnly = "    ReadOnly";
            }

            return string.Format("Name: {0}    {1}\r\nUser: {2}    Version: {3}{4}", this.Name, publicUrl, user, version, readOnly);
        }

        public string GetPublicUrl()
        {
            string publicUrl = this.PublicUrl;

            if (string.IsNullOrEmpty(publicUrl))
            {
                publicUrl = "none";
            }

            return string.Format("PublicUrl: {0}", publicUrl);
        }

        public string GetConnectionDescription()
        {
            StringBuilder result = new StringBuilder();

            result
                .AppendFormat("Connection to CRM:        {0}", this.GetDescription())
                .AppendLine()
                .AppendFormat("DiscoveryService:         {0}", this.DiscoveryUrl)
                .AppendLine()
                .AppendFormat("OrganizationService:      {0}", this.OrganizationUrl);

            return result.ToString();
        }

        public string GetConnectionInfo()
        {
            StringBuilder connectionInfo = new StringBuilder();

            connectionInfo.AppendFormat("Connection to CRM:        {0}", this.GetPublicUrl()).AppendLine();
            connectionInfo.AppendFormat("DiscoveryService:         {0}", this.DiscoveryUrl).AppendLine();
            connectionInfo.AppendFormat("OrganizationService:      {0}", this.OrganizationUrl);

            return connectionInfo.ToString();
        }

        public string GetUsername
        {
            get
            {
                string result = "DefaultWindows";

                if (this.User != null)
                {
                    result = this.User.Username;
                }

                return result;
            }
        }

        public string ToolTip
        {
            get
            {
                StringBuilder result = new StringBuilder();

                if (!string.IsNullOrEmpty(this.FriendlyName))
                {
                    if (result.Length > 0) { result.AppendLine(); }

                    result.AppendFormat("FriendlyName: {0}", this.FriendlyName);
                }

                if (this.OrganizationId.HasValue)
                {
                    if (result.Length > 0) { result.AppendLine(); }

                    result.AppendFormat("OrganizationId: {0}", this.OrganizationId);
                }

                if (!string.IsNullOrEmpty(this.OrganizationState))
                {
                    if (result.Length > 0) { result.AppendLine(); }

                    result.AppendFormat("OrganizationState: {0}", this.OrganizationState);
                }

                if (!string.IsNullOrEmpty(this.UrlName))
                {
                    if (result.Length > 0) { result.AppendLine(); }

                    result.AppendFormat("UrlName: {0}", this.UrlName);
                }

                if (!string.IsNullOrEmpty(this.BaseCurrency))
                {
                    if (result.Length > 0) { result.AppendLine(); }

                    result.AppendFormat("BaseCurrency: {0}", this.BaseCurrency);
                }

                if (!string.IsNullOrEmpty(this.DefaultLanguage))
                {
                    if (result.Length > 0) { result.AppendLine(); }

                    result.AppendFormat("DefaultLanguage: {0}", this.DefaultLanguage);
                }

                if (!string.IsNullOrEmpty(this.InstalledLanguagePacks))
                {
                    if (result.Length > 0) { result.AppendLine(); }

                    result.AppendFormat("InstalledLanguagePacks: {0}", this.InstalledLanguagePacks);
                }

                if (result.Length == 0)
                {
                    return null;
                }

                return result.ToString();
            }
        }

        private ConnectionIntellisenseData _intellisense;

        public ConnectionIntellisenseData IntellisenseData
        {
            get
            {
                lock (_syncObjectIntellisense)
                {
                    if (_intellisense == null)
                    {
                        _intellisense = new ConnectionIntellisenseData();

                        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
                        AppDomain.CurrentDomain.DomainUnload += CurrentDomain_ProcessExit;
                    }
                }

                _intellisense.ConnectionId = this.ConnectionId;

                return _intellisense;
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            this.IntellisenseData.Save();
        }
    }
}