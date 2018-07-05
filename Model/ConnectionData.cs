using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using System;
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
    public class ConnectionData : INotifyPropertyChanging, INotifyPropertyChanged
    {
        internal const int CountConnectionToQuickList = 40;

        private object _syncObjectAttributes = new object();

        private object _syncObjectIntellisense = new object();

        private object _syncObjectRequests = new object();

        private bool _IsCurrentConnection;

        public ConnectionConfiguration ConnectionConfiguration { get; set; }

        public bool IsCurrentConnection
        {
            get
            {
                return _IsCurrentConnection;
            }
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
            get
            {
                return _IsReadOnly;
            }
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

        private string _GroupName;

        /// <summary>
        /// Название
        /// </summary>
        [DataMember]
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _DiscoveryUrl;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _UniqueOrgName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _OrganizationUrl;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _PublicUrl;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _OrganizationVersion;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _FriendlyName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _OrganizationId;
            }
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
            get
            {
                return _OrganizationState;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _UrlName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _DefaultLanguage;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _InstalledLanguagePacks;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _BaseCurrency;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _NameSpaceClasses;
            }
            set
            {
                this.OnPropertyChanging(nameof(NameSpaceClasses));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._NameSpaceClasses = value;
                this.OnPropertyChanged(nameof(NameSpaceClasses));
            }
        }

        private string _NameSpaceOptionSets;
        [DataMember]
        public string NameSpaceOptionSets
        {
            get
            {
                return _NameSpaceOptionSets;
            }
            set
            {
                this.OnPropertyChanging(nameof(NameSpaceOptionSets));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._NameSpaceOptionSets = value;
                this.OnPropertyChanged(nameof(NameSpaceOptionSets));
            }
        }

        private string _ServiceContextName;
        [DataMember]
        public string ServiceContextName
        {
            get
            {
                return _ServiceContextName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _InteractiveLogin;
            }
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
            get
            {
                return _GenerateActions;
            }
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
            get
            {
                return _SelectedCrmSvcUtil;
            }
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
            get
            {
                return _SelectSolutionFilter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _ExplorerSolutionFilter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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

        private string _ExportSolutionFilter;
        [DataMember]
        public string ExportSolutionFilter
        {
            get
            {
                return _ExportSolutionFilter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_ExportSolutionFilter == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionFilter));
                this._ExportSolutionFilter = value;
                this.OnPropertyChanged(nameof(ExportSolutionFilter));
            }
        }

        private string _ExportSolutionFolder;
        [DataMember]
        public string ExportSolutionFolder
        {
            get
            {
                return _ExportSolutionFolder;
            }
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

        private bool _ExportSolutionOverrideSolutionNameAndVersion;
        [DataMember]
        public bool ExportSolutionOverrideSolutionNameAndVersion
        {
            get
            {
                return _ExportSolutionOverrideSolutionNameAndVersion;
            }
            set
            {
                if (_ExportSolutionOverrideSolutionNameAndVersion == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionOverrideSolutionNameAndVersion));
                this._ExportSolutionOverrideSolutionNameAndVersion = value;
                this.OnPropertyChanged(nameof(ExportSolutionOverrideSolutionNameAndVersion));
            }
        }

        private bool _ExportSolutionOverrideSolutionDescription;
        [DataMember]
        public bool ExportSolutionOverrideSolutionDescription
        {
            get
            {
                return _ExportSolutionOverrideSolutionDescription;
            }
            set
            {
                if (_ExportSolutionOverrideSolutionDescription == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ExportSolutionOverrideSolutionDescription));
                this._ExportSolutionOverrideSolutionDescription = value;
                this.OnPropertyChanged(nameof(ExportSolutionOverrideSolutionDescription));
            }
        }

        private string _ExportSolutionOverrideUniqueName;
        [DataMember]
        public string ExportSolutionOverrideUniqueName
        {
            get
            {
                return _ExportSolutionOverrideUniqueName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _ExportSolutionOverrideDisplayName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _ExportSolutionOverrideVersion;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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
            get
            {
                return _ExportSolutionOverrideDescription;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
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

        private Guid? _UserId;
        [DataMember]
        public Guid? UserId
        {
            get
            {
                return _UserId;
            }
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
            get { return _user; }
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

        internal const int CountLastSolutions = 10;

        internal const int CountLastItems = 20;

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

            this.NameSpaceClasses = nameof(NameSpaceClasses);
            this.NameSpaceOptionSets = nameof(NameSpaceOptionSets);
            this.ServiceContextName = "XrmServiceContext";
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

        public ConnectionData CreateCopy()
        {
            ConnectionData result = new ConnectionData();

            result.GroupName = this.GroupName;

            result.DiscoveryUrl = this.DiscoveryUrl;
            result.OrganizationUrl = this.OrganizationUrl;
            result.UniqueOrgName = this.UniqueOrgName;

            result.NameSpaceClasses = this.NameSpaceClasses;
            result.NameSpaceOptionSets = this.NameSpaceOptionSets;
            result.ServiceContextName = this.ServiceContextName;

            result.ConnectionConfiguration = this.ConnectionConfiguration;

            result.UserId = this.UserId;
            result.User = this.User;

            result.ConnectionId = Guid.NewGuid();

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
                    return new HashSet<string>(intellisense.Entities[entityName].Attributes.Keys);
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
                .AppendFormat("Connection to CRM: {0}", this.GetDescription())
                .AppendLine()
                .AppendFormat("DiscoveryService: {0}", this.DiscoveryUrl)
                .AppendLine()
                .AppendFormat("OrganizationService: {0}", this.OrganizationUrl);

            return result.ToString();
        }

        public string GetConnectionInfo()
        {
            StringBuilder connectionInfo = new StringBuilder();

            connectionInfo.AppendFormat("Connection to CRM: {0}", this.GetPublicUrl()).AppendLine();
            connectionInfo.AppendFormat("DiscoveryService: {0}", this.DiscoveryUrl).AppendLine();
            connectionInfo.AppendFormat("OrganizationService: {0}", this.OrganizationUrl);

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

        private Dictionary<string, bool> _knownRequests = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

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
                    _knownRequests = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
                }
            }

            if (this.FetchXmlRequestParameterList == null)
            {
                this.FetchXmlRequestParameterList = new ObservableCollection<FetchXmlRequestParameter>();
            }
        }

        [OnSerializing]
        private void BeforeSerializing(StreamingContext context)
        {
            if (this.ConnectionId == Guid.Empty)
            {
                this.ConnectionId = Guid.NewGuid();
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

        #region Генерация url-адресов.

        private static bool IsValidUri(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return false;
            }

            Uri tmp;

            if (!Uri.TryCreate(url, UriKind.Absolute, out tmp))
            {
                return false;
            }

            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
        }

        private bool TryGetPublicUrl(out string uri)
        {
            uri = null;

            if (string.IsNullOrEmpty(this.PublicUrl))
            {
                return false;
            }

            uri = this.PublicUrl.TrimEnd('/');

            return true;
        }

        public void OpenCrmInWeb()
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return;
            }

            if (!IsValidUri(publicUrl)) return;

            System.Diagnostics.Process.Start(publicUrl);
        }

        public void OpenAdvancedFindInWeb()
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return;
            }

            string uri = publicUrl + "/main.aspx?pagetype=advancedfind";

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public void OpenSolutionInWeb(Guid idSolution)
        {
            string uri = GetSolutionUrl(idSolution);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetSolutionUrl(Guid idSolution)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + string.Format("/tools/solution/edit.aspx?id={0}", idSolution);
        }

        public void OpenSolutionCreateInWeb()
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return;
            }

            string uri = publicUrl + "/tools/solution/edit.aspx";

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public void OpenReportCreateInWeb()
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return;
            }

            string uri = publicUrl + "/CRMReports/reportproperty.aspx";

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public void OpenEntityListInWeb(string entityName)
        {
            string uri = GetEntityListUrl(entityName);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetEntityListUrl(string entityName)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + string.Format("/main.aspx?etn={0}&pagetype=entitylist", entityName);
        }

        public void OpenEntityInWeb(string entityName, Guid entityId)
        {
            string uri = GetEntityUrl(entityName, entityId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetEntityUrl(string entityName, Guid entityId)
        {
            var componentType = GetSolutionComponentType(entityName);

            if (componentType.HasValue)
            {
                var url = GetSolutionComponentUrl(componentType.Value, entityId, null, null);

                if (!string.IsNullOrEmpty(url))
                {
                    return url;
                }
            }

            return string.Format(GetEntityUrlFormat(), entityName, entityId);
        }

        public string GetEntityUrlFormat()
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + "/main.aspx?etn={0}&pagetype=entityrecord&id={1}";
        }

        private static ComponentType? GetSolutionComponentType(string entityName)
        {
            var componentTypeName = entityName;

            if (string.Equals(componentTypeName, "template", StringComparison.OrdinalIgnoreCase))
            {
                componentTypeName = "EmailTemplate";
            }

            if (Enum.TryParse<ComponentType>(componentTypeName, true, out ComponentType componentType))
            {
                return componentType;
            }

            return null;
        }

        public void OpenEntityMetadataInWeb(Guid entityMetadataId)
        {
            string uri = GetEntityMetadataUrl(entityMetadataId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public void OpenEntityMetadataInWeb(string entityName)
        {
            if (this.IntellisenseData.Entities != null
                && this.IntellisenseData.Entities.ContainsKey(entityName)
                && this.IntellisenseData.Entities[entityName].MetadataId.HasValue
                )
            {
                OpenEntityMetadataInWeb(this.IntellisenseData.Entities[entityName].MetadataId.Value);
            }
        }

        public string GetEntityMetadataUrl(Guid entityId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + GetSolutionComponentRelativeUrl(ComponentType.Entity, entityId, null, null);
        }

        public void OpenAttributeMetadataInWeb(Guid entityId, Guid attributeId)
        {
            string uri = GetAttributeMetadataUrl(entityId, attributeId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetAttributeMetadataUrl(Guid entityId, Guid attributeId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + $"/tools/systemcustomization/attributes/manageAttribute.aspx?attributeId={attributeId}&entityId={entityId}";
        }

        public void OpenRelationshipMetadataInWeb(Guid entityId, Guid relationId)
        {
            string uri = GetRelationshipMetadataUrl(entityId, relationId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetRelationshipMetadataUrl(Guid entityId, Guid relationId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + $"/tools/systemcustomization/relationships/manageRelationship.aspx?entityId={entityId}&entityRelationshipId={relationId}";
        }

        public void OpenGlobalOptionSetInWeb(Guid idGlobalOptionSet)
        {
            string uri = GetGlobalOptionSetUrl(idGlobalOptionSet);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetGlobalOptionSetUrl(Guid idGlobalOptionSet)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + GetSolutionComponentRelativeUrl(ComponentType.OptionSet, idGlobalOptionSet, null, null);
        }

        public void OpenSolutionComponentInWeb(ComponentType componentType, Guid objectId, string linkedEntityName, int? linkedEntityObjectCode)
        {
            string uri = GetSolutionComponentUrl(componentType, objectId, linkedEntityName, linkedEntityObjectCode);

            if (!IsValidUri(uri)) { return; }

            System.Diagnostics.Process.Start(uri);
        }

        public string GetSolutionComponentUrl(ComponentType componentType, Guid objectId, string linkedEntityName, int? linkedEntityObjectCode)
        {
            string uriEnd = GetSolutionComponentRelativeUrl(componentType, objectId, linkedEntityName, linkedEntityObjectCode);

            if (string.IsNullOrEmpty(uriEnd))
            {
                return null;
            }

            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            var uri = publicUrl + "/" + uriEnd.TrimStart('/');
            return uri;
        }

        private static string GetSolutionComponentRelativeUrl(ComponentType componentType, Guid objectId, string linkedEntityName, int? linkedEntityObjectCode)
        {
            switch (componentType)
            {
                case ComponentType.SavedQuery:
                    return $"/tools/vieweditor/viewManager.aspx?id={objectId}";

                case ComponentType.SavedQueryVisualization:
                    return $"/main.aspx?extraqs=etc={linkedEntityObjectCode}&id={objectId}&pagetype=vizdesigner";

                case ComponentType.SystemForm:
                    break;

                case ComponentType.Workflow:
                    return $"/sfa/workflow/edit.aspx?id={objectId}";

                case ComponentType.Entity:
                    return $"/tools/systemcustomization/entities/manageentity.aspx?id={objectId}";

                case ComponentType.OptionSet:
                    return $"/tools/systemcustomization/optionset/optionset.aspx?id={objectId}";

                case ComponentType.WebResource:
                    return $"/main.aspx?etc=9333&id={objectId}&pagetype=webresourceedit";

                case ComponentType.Report:
                    return $"/CRMReports/reportproperty.aspx?id={objectId}";

                case ComponentType.EntityMap:
                    return $"/tools/systemcustomization/relationships/mappings/mappingList.aspx?mappingId={objectId}";

                //case ComponentType.AttributeMap:
                //   return $"";

                case ComponentType.DisplayString:
                    return $"/tools/systemcustomization/displaystrings/edit.aspx?id={objectId}";

                case ComponentType.FieldSecurityProfile:
                    return $"/biz/fieldsecurityprofiles/edit.aspx?id={objectId}";

                case ComponentType.Role:
                    return $"/biz/roles/edit.aspx?id={objectId}";

                //case ComponentType.RolePrivileges:
                //   return $"";

                case ComponentType.ConnectionRole:
                    return $"/connections/connectionroles/edit.aspx?id={objectId}";


                //case ComponentType.Attribute:
                //   return $"";
                //case ComponentType.Relationship:
                //   return $"";
                //case ComponentType.AttributePicklistValue:
                //   return $"";
                //case ComponentType.AttributeLookupValue:
                //   return $"";
                //case ComponentType.ViewAttribute:
                //   return $"";
                //case ComponentType.LocalizedLabel:
                //   return $"";
                //case ComponentType.RelationshipExtraCondition:
                //   return $"";

                //case ComponentType.EntityRelationship:
                //   return $"";
                //case ComponentType.EntityRelationshipRole:
                //   return $"";
                //case ComponentType.EntityRelationshipRelationships:
                //   return $"";
                //case ComponentType.ManagedProperty:
                //   return $"";
                //case ComponentType.EntityKey:
                //   return $"";


                //case ComponentType.DisplayStringMap:
                //   return $"";
                //case ComponentType.Form:
                //   return $"";
                //case ComponentType.Organization:
                //   return $"";

                //case ComponentType.ReportEntity:
                //   return $"";
                //case ComponentType.ReportCategory:
                //   return $"";
                //case ComponentType.ReportVisibility:
                //   return $"";

                //case ComponentType.Attachment:
                //   return $"";
                //case ComponentType.EmailTemplate:
                //   return $"";
                //case ComponentType.ContractTemplate:
                //   return $"";
                //case ComponentType.KBArticleTemplate:
                //   return $"";
                //case ComponentType.MailMergeTemplate:
                //   return $"";


                //case ComponentType.DuplicateRule:
                //   return $"";
                //case ComponentType.DuplicateRuleCondition:
                //   return $"";


                //case ComponentType.RibbonCommand:
                //   return $"";
                //case ComponentType.RibbonContextGroup:
                //   return $"";
                //case ComponentType.RibbonCustomization:
                //   return $"";
                //case ComponentType.RibbonRule:
                //   return $"";
                //case ComponentType.RibbonTabToCommandMap:
                //   return $"";
                //case ComponentType.RibbonDiff:
                //   return $"";

                //case ComponentType.SiteMap:
                //   return $"";
                //case ComponentType.FieldPermission:
                //   return $"";
                //case ComponentType.PluginType:
                //   return $"";
                //case ComponentType.PluginAssembly:
                //   return $"";
                //case ComponentType.SDKMessageProcessingStep:
                //   return $"";
                //case ComponentType.SDKMessageProcessingStepImage:
                //   return $"";
                //case ComponentType.ServiceEndpoint:
                //   return $"";
                //case ComponentType.RoutingRule:
                //   return $"";
                //case ComponentType.RoutingRuleItem:
                //   return $"";
                //case ComponentType.SLA:
                //   return $"";
                //case ComponentType.SLAItem:
                //   return $"";
                //case ComponentType.ConvertRule:
                //   return $"";
                //case ComponentType.ConvertRuleItem:
                //   return $"";
                //case ComponentType.HierarchyRule:
                //   return $"";
                //case ComponentType.MobileOfflineProfile:
                //   return $"";
                //case ComponentType.MobileOfflineProfileItem:
                //   return $"";
                //case ComponentType.SimilarityRule:
                //   return $"";
                //case ComponentType.CustomControl:
                //   return $"";
                //case ComponentType.CustomControlDefaultConfig:
                //   return $"";

                default:
                    break;
            }

            return null;
        }

        internal void OpenSolutionComponentDependentComponentsInWeb(ComponentType componentType, Guid objectId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return;
            }

            int entityTypeCode = SolutionComponent.GetComponentTypeObjectTypeCode(componentType);

            var uri = publicUrl + $"/tools/dependency/dependencyviewdialog.aspx?dType=1&objectid={objectId}&objecttype={entityTypeCode}&operationtype=showdependency";

            if (!IsValidUri(uri)) { return; }

            System.Diagnostics.Process.Start(uri);
        }

        #endregion Генерация url-адресов.
    }
}