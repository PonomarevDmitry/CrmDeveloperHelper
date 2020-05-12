using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

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

        private object _syncObjectEntitiesIntellisense = new object();

        private object _syncObjectWebResourceIntellisense = new object();

        private object _syncObjectRequests = new object();

        private FileSystemWatcher _watcher = null;

        public string Path { get; private set; }

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
                UpdateNameWithCurrentMark();
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
                UpdateNameWithCurrentMark();
                this.OnPropertyChanged(nameof(Name));
            }
        }

        public string NameWithCurrentMark { get; private set; }

        private void UpdateNameWithCurrentMark()
        {
            this.NameWithCurrentMark = _Name + (this._IsCurrentConnection ? " (Current)" : string.Empty);
        }

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
        public DateTime? OrganizationInformationExpirationDate { get; set; }

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
        public ObservableCollection<FetchXmlRequestParameter> FetchXmlRequestParameterList { get; private set; }

        [DataMember]
        public ObservableCollection<ExportSolutionProfile> ExportSolutionProfileList { get; private set; }

        private int _ExportSolutionProfileListSelectedIndex = -1;
        [DataMember]
        public int ExportSolutionProfileListSelectedIndex
        {
            get => _ExportSolutionProfileListSelectedIndex;
            set
            {
                if (_ExportSolutionProfileListSelectedIndex == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionProfileListSelectedIndex));
                this._ExportSolutionProfileListSelectedIndex = value;
                this.OnPropertyChanged(nameof(ExportSolutionProfileListSelectedIndex));
            }
        }

        private ConcurrentDictionary<string, bool> _knownRequests = new ConcurrentDictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

        public string[] PluginAssemblyProperties { get; set; }

        public string[] EntityMetadataProperties { get; set; }

        /// <summary>
        /// Конструктор данных
        /// </summary>
        public ConnectionData()
        {
            this.Mappings = new List<FileMapping>();
            this.AssemblyMappings = new List<AssemblyMapping>();

            this.LastSelectedSolutionsUniqueName = new ObservableCollection<string>();
            this.LastSolutionExportFolders = new ObservableCollection<string>();
            this.FetchXmlRequestParameterList = new ObservableCollection<FetchXmlRequestParameter>();
            this.ExportSolutionProfileList = new ObservableCollection<ExportSolutionProfile>();

            this.TraceReaderLastFilters = new ObservableCollection<string>();
            this.TraceReaderLastFolders = new ObservableCollection<string>();
            this.TraceReaderSelectedFolders = new ObservableCollection<string>();

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
            if (_syncObjectEntitiesIntellisense == null) { _syncObjectEntitiesIntellisense = new object(); }
            if (_syncObjectWebResourceIntellisense == null) { _syncObjectWebResourceIntellisense = new object(); }
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

            if (this.FetchXmlRequestParameterList == null)
            {
                this.FetchXmlRequestParameterList = new ObservableCollection<FetchXmlRequestParameter>();
            }

            if (this.ExportSolutionProfileList == null)
            {
                this.ExportSolutionProfileList = new ObservableCollection<ExportSolutionProfile>();
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

            if (this.ExportSolutionProfileList.Count == 0)
            {
                this.ExportSolutionProfileList.Add(new ExportSolutionProfile()
                {
                    Name = "Default",
                });
            }

            UpdateNameWithCurrentMark();
        }

        public Task LoadIntellisenseAsync()
        {
            return Task.Run(() => LoadIntellisense());
        }

        private void LoadIntellisense()
        {
            this.IntellisenseData.ConnectionId = this.ConnectionId;
            this.WebResourceIntellisenseData.ConnectionId = this.ConnectionId;

            var task1 = Task.Run(() => this.IntellisenseData.GetDataFromDisk());
            var task2 = Task.Run(() => this.WebResourceIntellisenseData.GetDataFromDisk());

            AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            AppDomain.CurrentDomain.DomainUnload -= CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_ProcessExit;
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            this.IntellisenseData.Save();
            this.WebResourceIntellisenseData.Save();
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
            if (string.IsNullOrEmpty(assemblyName)
                || string.IsNullOrEmpty(localAssemblyPath)
            )
            {
                return;
            }

            {
                var mappingList = this.AssemblyMappings.Where(x => string.Equals(x.AssemblyName, assemblyName, StringComparison.InvariantCultureIgnoreCase)).ToList();

                while (mappingList.Count > 1)
                {
                    this.AssemblyMappings.Remove(mappingList[mappingList.Count - 1]);
                }
            }

            var mapping = this.AssemblyMappings.FirstOrDefault(x => x.AssemblyName.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            if (mapping != null)
            {
                var indexItem = 0;

                while (indexItem != -1 && mapping.LocalAssemblyPathList.Contains(localAssemblyPath, StringComparer.InvariantCultureIgnoreCase))
                {
                    indexItem = mapping.LocalAssemblyPathList.FindIndex(s => string.Equals(s, localAssemblyPath, StringComparison.InvariantCultureIgnoreCase));

                    if (indexItem != -1)
                    {
                        mapping.LocalAssemblyPathList.RemoveAt(indexItem);
                    }
                }

                mapping.LocalAssemblyPathList.Insert(0, localAssemblyPath);
            }
            else
            {
                this.AssemblyMappings.Add(new AssemblyMapping()
                {
                    AssemblyName = assemblyName,
                    LocalAssemblyPathList =
                    {
                        localAssemblyPath
                    },
                });
            }
        }

        public string GetLastAssemblyPath(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                return null;
            }

            var mapping = this.AssemblyMappings.FirstOrDefault(x => x.AssemblyName.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            return mapping?.LocalAssemblyPathList?.FirstOrDefault();
        }

        public IEnumerable<string> GetAssemblyPaths(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                yield break;
            }

            var mapping = this.AssemblyMappings.FirstOrDefault(x => x.AssemblyName.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            if (mapping != null)
            {
                foreach (var path in mapping.LocalAssemblyPathList.OrderBy(s => s))
                {
                    yield return path;
                }
            }
        }

        //public bool RemoveAssemblyMapping(string assemblyName)
        //{
        //    bool result = false;

        //    if (string.IsNullOrEmpty(assemblyName))
        //    {
        //        return result;
        //    }

        //    var mapping = this.AssemblyMappings.FirstOrDefault(x => x.AssemblyName.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

        //    if (mapping != null)
        //    {
        //        this.AssemblyMappings.Remove(mapping);

        //        result = true;
        //    }

        //    return result;
        //}

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

                    return new HashSet<string>(keys, StringComparer.InvariantCultureIgnoreCase);
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
                lock (_syncObjectEntitiesIntellisense)
                {
                    if (_intellisense == null)
                    {
                        _intellisense = new ConnectionIntellisenseData();
                    }
                }

                _intellisense.ConnectionId = this.ConnectionId;

                return _intellisense;
            }
        }

        private ConnectionWebResourceIntellisenseData _webResourceIntellisenseData;

        public ConnectionWebResourceIntellisenseData WebResourceIntellisenseData
        {
            get
            {
                lock (_syncObjectWebResourceIntellisense)
                {
                    if (_webResourceIntellisenseData == null)
                    {
                        _webResourceIntellisenseData = new ConnectionWebResourceIntellisenseData(this.ConnectionId);
                    }
                }

                _webResourceIntellisenseData.ConnectionId = this.ConnectionId;

                return _webResourceIntellisenseData;
            }
        }

        public static ConnectionData Get(Guid connectionId)
        {
            string filePath = FileOperations.GetConnectionDataFilePath(connectionId);

            ConnectionData result = GetFromPath(filePath);

            if (result != null)
            {
                result.Path = filePath;
            }

            return result;
        }

        public static ConnectionData GetFromPath(string filePath)
        {
            ConnectionData result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionData));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as ConnectionData;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);

                        FileOperations.CreateBackUpFile(filePath, ex);

                        result = null;
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Сохранение конфигурации в папку
        /// </summary>
        /// <param name="filePath"></param>
        private void Save(string filePath)
        {
            this.Path = filePath;

            DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionData));

            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        Encoding = Encoding.UTF8
                    };

                    using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
                    {
                        ser.WriteObject(xmlWriter, this);
                        xmlWriter.Flush();
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    fileBody = memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    Helpers.DTEHelper.WriteExceptionToLog(ex);

                    fileBody = null;
                }
            }

            if (fileBody != null)
            {
                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        if (_watcher != null)
                        {
                            _watcher.EnableRaisingEvents = false;
                        }

                        try
                        {
                            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                stream.Write(fileBody, 0, fileBody.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToLog(ex);
                        }

                        if (_watcher != null)
                        {
                            _watcher.EnableRaisingEvents = true;
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(this.Path)
                && this.ConnectionId != Guid.Empty
                )
            {
                this.Path = FileOperations.GetConnectionDataFilePath(this.ConnectionId);
            }

            this.Save(this.Path);
        }

        public void StartWatchFile()
        {
            if (_watcher != null)
            {
                return;
            }

            _watcher = new FileSystemWatcher()
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                Path = System.IO.Path.GetDirectoryName(this.Path),
                Filter = System.IO.Path.GetFileName(this.Path),
            };

            _watcher.Changed += _watcher_Changed;

            _watcher.EnableRaisingEvents = true;
        }

        public void StopWatchFile()
        {
            if (_watcher == null)
            {
                return;
            }

            _watcher.EnableRaisingEvents = false;

            _watcher.Changed -= _watcher_Changed;
            _watcher.Changed -= _watcher_Changed;

            _watcher.Dispose();

            _watcher = null;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var diskData = Get(this.ConnectionId);

            LoadFromDisk(diskData);
        }

        private void LoadFromDisk(ConnectionData diskData)
        {
            this.IsReadOnly = diskData.IsReadOnly;
            this.Name = diskData.Name;
            this.GroupName = diskData.GroupName;
            this.DiscoveryUrl = diskData.DiscoveryUrl;
            this.UniqueOrgName = diskData.UniqueOrgName;
            this.OrganizationUrl = diskData.OrganizationUrl;
            this.PublicUrl = diskData.PublicUrl;
            this.OrganizationVersion = diskData.OrganizationVersion;
            this.FriendlyName = diskData.FriendlyName;
            this.OrganizationId = diskData.OrganizationId;
            this.OrganizationState = diskData.OrganizationState;
            this.UrlName = diskData.UrlName;
            this.DefaultLanguage = diskData.DefaultLanguage;
            this.InstalledLanguagePacks = diskData.InstalledLanguagePacks;
            this.BaseCurrency = diskData.BaseCurrency;

            this.ServiceContextName = diskData.ServiceContextName;
            this.InteractiveLogin = diskData.InteractiveLogin;
            this.GenerateActions = diskData.GenerateActions;
            this.SelectedCrmSvcUtil = diskData.SelectedCrmSvcUtil;
            this.SelectSolutionFilter = diskData.SelectSolutionFilter;
            this.ExplorerSolutionFilter = diskData.ExplorerSolutionFilter;
            this.TraceReaderFilter = diskData.TraceReaderFilter;
            this.TraceReaderFolder = diskData.TraceReaderFolder;
            this.UserId = diskData.UserId;
            this.OrganizationInformationExpirationDate = diskData.OrganizationInformationExpirationDate;

            this.Mappings.Clear();
            this.Mappings.AddRange(diskData.Mappings);

            this.AssemblyMappings.Clear();
            this.AssemblyMappings.AddRange(diskData.AssemblyMappings);

            this.LastSelectedSolutionsUniqueName.Clear();
            foreach (var item in diskData.LastSelectedSolutionsUniqueName)
            {
                this.LastSelectedSolutionsUniqueName.Add(item);
            }

            this.LastSolutionExportFolders.Clear();
            foreach (var item in diskData.LastSolutionExportFolders)
            {
                this.LastSolutionExportFolders.Add(item);
            }

            this.FetchXmlRequestParameterList.Clear();
            foreach (var item in diskData.FetchXmlRequestParameterList)
            {
                this.FetchXmlRequestParameterList.Add(item);
            }

            this.ExportSolutionProfileList.Clear();
            foreach (var item in diskData.ExportSolutionProfileList)
            {
                this.ExportSolutionProfileList.Add(item);
            }

            if (this.ExportSolutionProfileList.Count == 0)
            {
                this.ExportSolutionProfileList.Add(new ExportSolutionProfile()
                {
                    Name = "Default",
                });
            }

            //this.Utils.Clear();
            //foreach (var item in diskData.Utils)
            //{
            //    this.Utils.Add(item);
            //}
        }
    }
}