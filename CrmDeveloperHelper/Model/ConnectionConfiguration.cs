using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class ConnectionConfiguration
    {
        private static object _syncObject = new object();
        private static object _syncObjectFile = new object();

        private static ConnectionConfiguration _singleton;

        /// <summary>
        /// Путь к файлу конфигурации
        /// </summary>
        private string Path { get; set; }

        private VSSolutionConfiguration _currentSolutionConfiguration;

        /// <summary>
        /// Текущее подключение к CRM
        /// </summary>
        public ConnectionData CurrentConnectionData
        {
            get
            {
                if (this._currentSolutionConfiguration != null && this._currentSolutionConfiguration.SelectedConnectionId.HasValue)
                {
                    foreach (var item in this.Connections)
                    {
                        item.IsCurrentConnection = false;
                    }

                    var connection = this.Connections.FirstOrDefault(c => c.ConnectionId == this._currentSolutionConfiguration.SelectedConnectionId.Value);

                    if (connection == null)
                    {
                        this._currentSolutionConfiguration.SelectedConnectionId = null;
                    }
                    else
                    {
                        connection.IsCurrentConnection = true;
                    }

                    return connection;
                }

                return null;
            }
        }

        /// <summary>
        /// Все подключения к CRM
        /// </summary>
        [DataMember]
        public ObservableCollection<ConnectionData> Connections { get; private set; }

        [DataMember]
        public ObservableCollection<ConnectionData> ArchiveConnections { get; private set; }

        [DataMember]
        public ObservableCollection<ConnectionUserData> Users { get; private set; }

        [DataMember]
        public VSSolutionConfiguration VSSolutionConfigurationWhenNoSolutionLoaded { get; private set; }

        [DataMember]
        public ObservableCollection<VSSolutionConfiguration> Solutions { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ConnectionConfiguration()
        {
            this.VSSolutionConfigurationWhenNoSolutionLoaded = new VSSolutionConfiguration();

            this.Connections = new ObservableCollection<ConnectionData>();
            this.ArchiveConnections = new ObservableCollection<ConnectionData>();
            this.Users = new ObservableCollection<ConnectionUserData>();
            this.Solutions = new ObservableCollection<VSSolutionConfiguration>();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (this.VSSolutionConfigurationWhenNoSolutionLoaded == null)
            {
                this.VSSolutionConfigurationWhenNoSolutionLoaded = new VSSolutionConfiguration();
            }

            if (this.Connections == null)
            {
                this.Connections = new ObservableCollection<ConnectionData>();
            }

            if (this.ArchiveConnections == null)
            {
                this.ArchiveConnections = new ObservableCollection<ConnectionData>();
            }

            if (this.Users == null)
            {
                this.Users = new ObservableCollection<ConnectionUserData>();
            }

            if (this.Solutions == null)
            {
                this.Solutions = new ObservableCollection<VSSolutionConfiguration>();
            }
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            foreach (ConnectionData connectionData in this.Connections)
            {
                connectionData.ConnectionConfiguration = this;

                connectionData.User = this.Users.FirstOrDefault(u => u.UserId == connectionData.UserId);
            }

            foreach (ConnectionData connectionData in this.ArchiveConnections)
            {
                connectionData.ConnectionConfiguration = this;

                connectionData.User = this.Users.FirstOrDefault(u => u.UserId == connectionData.UserId);
            }
        }

        /// <summary>
        /// Получение конфига из файла
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public static ConnectionConfiguration Get()
        {
            lock (_syncObject)
            {
                if (_singleton != null)
                {
                    var applicationObject = CrmDeveloperHelperPackage.ServiceProvider?.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                    _singleton.SetCurrentSolution(applicationObject?.Solution?.FullName);

                    return _singleton;
                }

                ConnectionConfiguration result = null;

                string filePath = FileOperations.GetConnectionConfigFilePath();

                if (File.Exists(filePath))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionConfiguration));

                    try
                    {
                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as ConnectionConfiguration;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(ex);

                        FileOperations.CreateBackUpFile(filePath, ex);

                        result = null;
                    }
                }

                result = result ?? new ConnectionConfiguration();

                result.Path = filePath;

                if (_singleton == null)
                {
                    _singleton = result;

                    var applicationObject = CrmDeveloperHelperPackage.ServiceProvider?.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                    result.SetCurrentSolution(applicationObject?.Solution?.FullName);
                }

                return _singleton;
            }
        }

        /// <summary>
        /// Сохранение конфигурации в папку
        /// </summary>
        /// <param name="filePath"></param>
        private void Save(string filePath)
        {
            this.Path = filePath;

            DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionConfiguration));

            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Encoding.UTF8;

                    using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
                    {
                        ser.WriteObject(xmlWriter, this);
                    }

                    fileBody = memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);

                    fileBody = null;
                }
            }

            if (fileBody != null)
            {
                lock (_syncObjectFile)
                {
                    try
                    {
                        try
                        {

                        }
                        finally
                        {
                            File.WriteAllBytes(filePath, fileBody);
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);
                    }
                }
            }
        }

        public void Save()
        {
            this.Save(this.Path);
        }

        private void SetCurrentSolution(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
            {
                this.VSSolutionConfigurationWhenNoSolutionLoaded = this.VSSolutionConfigurationWhenNoSolutionLoaded ?? new VSSolutionConfiguration();
                this._currentSolutionConfiguration = this.VSSolutionConfigurationWhenNoSolutionLoaded;
                return;
            }

            var solution = this.Solutions.FirstOrDefault(s => s.SolutionPath.Equals(solutionPath, StringComparison.InvariantCultureIgnoreCase));

            if (solution != null)
            {
                if (this._currentSolutionConfiguration != solution)
                {
                    this._currentSolutionConfiguration = solution;
                }
            }
            else
            {
                solution = new VSSolutionConfiguration()
                {
                    SolutionPath = solutionPath,
                };

                this.Solutions.Add(solution);

                this._currentSolutionConfiguration = solution;
            }
        }

        public void SetCurrentConnection(Guid? selectedConnection)
        {
            foreach (var item in this.Connections)
            {
                item.IsCurrentConnection = false;
            }

            if (_currentSolutionConfiguration == null)
            {
                this.VSSolutionConfigurationWhenNoSolutionLoaded = this.VSSolutionConfigurationWhenNoSolutionLoaded ?? new VSSolutionConfiguration();

                _currentSolutionConfiguration = this.VSSolutionConfigurationWhenNoSolutionLoaded;
            }

            var connection = this.Connections.FirstOrDefault(c => c.ConnectionId == selectedConnection);

            if (connection != null)
            {
                connection.IsCurrentConnection = true;
                _currentSolutionConfiguration.SelectedConnectionId = selectedConnection;
            }
            else
            {
                _currentSolutionConfiguration.SelectedConnectionId = null;
            }
        }

        public List<ConnectionData> GetConnectionsWithoutCurrent()
        {
            if (this.CurrentConnectionData == null)
            {
                return new List<ConnectionData>();
            }

            return this.Connections.Where(c => this.CurrentConnectionData.ConnectionId != c.ConnectionId).ToList();
        }

        public List<ConnectionData> GetConnectionsByGroupWithoutCurrent()
        {
            if (this.CurrentConnectionData == null || string.IsNullOrEmpty(this.CurrentConnectionData.GroupName))
            {
                return new List<ConnectionData>();
            }

            return this.Connections.Where(c =>
                    string.Equals(this.CurrentConnectionData.GroupName, c.GroupName, StringComparison.InvariantCultureIgnoreCase)
                    && this.CurrentConnectionData.ConnectionId != c.ConnectionId
                ).ToList();
        }

        public List<ConnectionData> GetConnectionsByGroupWithCurrent()
        {
            if (this.CurrentConnectionData == null)
            {
                return new List<ConnectionData>();
            }

            if (string.IsNullOrEmpty(this.CurrentConnectionData.GroupName))
            {
                return new List<ConnectionData>() { this.CurrentConnectionData };
            }

            return this.Connections.Where(c =>
                string.Equals(this.CurrentConnectionData.GroupName, c.GroupName, StringComparison.InvariantCultureIgnoreCase)
                ).ToList();
        }

        public TupleList<ConnectionData, ConnectionData> GetConnectionPairsByGroup()
        {
            var list = GetConnectionsByGroupWithCurrent();

            var result = new TupleList<ConnectionData, ConnectionData>();

            for (int index1 = 0; index1 < list.Count; index1++)
            {
                for (int index2 = index1 + 1; index2 < list.Count; index2++)
                {
                    result.Add(list[index1], list[index2]);
                }
            }

            return result;
        }
    }
}