using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class SiteMapIntellisenseData
    {
        private string FilePath { get; set; }

        [DataMember]
        public Guid ConnectionId { get; set; }

        [DataMember]
        public SortedSet<string> Urls { get; private set; }

        [DataMember]
        public SortedSet<string> ResourceIds { get; private set; }

        [DataMember]
        public SortedSet<string> DescriptionResourceIds { get; private set; }

        [DataMember]
        public SortedSet<string> ToolTipResourseIds { get; private set; }

        [DataMember]
        public SortedSet<string> Icons { get; private set; }

        [DataMember]
        public SortedSet<string> GetStartedPanePaths { get; private set; }

        [DataMember]
        public SortedSet<string> GetStartedPanePathOutlooks { get; private set; }

        [DataMember]
        public SortedSet<string> GetStartedPanePathAdmins { get; private set; }

        [DataMember]
        public SortedSet<string> GetStartedPanePathAdminOutlooks { get; private set; }

        [DataMember]
        public SortedSet<string> CheckExtensionProperties { get; private set; }

        [DataMember]
        public ConcurrentDictionary<Guid, SystemFormIntellisenseData> Dashboards { get; private set; }

        public SiteMapIntellisenseData()
        {
            ClearData();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            CreateCollectionIfNeaded();
        }

        private void CreateCollectionIfNeaded()
        {
            if (Urls == null)
            {
                this.Urls = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (ResourceIds == null)
            {
                this.ResourceIds = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (DescriptionResourceIds == null)
            {
                this.DescriptionResourceIds = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (ToolTipResourseIds == null)
            {
                this.ToolTipResourseIds = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (Icons == null)
            {
                this.Icons = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (GetStartedPanePaths == null)
            {
                this.GetStartedPanePaths = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (GetStartedPanePathOutlooks == null)
            {
                this.GetStartedPanePathOutlooks = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (GetStartedPanePathAdmins == null)
            {
                this.GetStartedPanePathAdmins = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (GetStartedPanePathAdminOutlooks == null)
            {
                this.GetStartedPanePathAdminOutlooks = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (CheckExtensionProperties == null)
            {
                this.CheckExtensionProperties = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (Dashboards == null)
            {
                this.Dashboards = new ConcurrentDictionary<Guid, SystemFormIntellisenseData>();
            }
        }

        public void ClearData()
        {
            this.Urls = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this.ResourceIds = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.DescriptionResourceIds = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.ToolTipResourseIds = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this.Icons = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this.GetStartedPanePaths = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.GetStartedPanePathOutlooks = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.GetStartedPanePathAdmins = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
            this.GetStartedPanePathAdminOutlooks = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this.CheckExtensionProperties = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this.Dashboards = new ConcurrentDictionary<Guid, SystemFormIntellisenseData>();
        }

        public void LoadDataFromSiteMap(XDocument docSiteMap)
        {
            if (docSiteMap == null)
            {
                return;
            }

            {
                var nodeSiteMaps = docSiteMap.XPathSelectElements("SiteMap");

                foreach (var node in nodeSiteMaps)
                {
                    if (node.Attribute("Url") != null
                        && !string.IsNullOrEmpty(node.Attribute("Url").Value)
                        && !node.Attribute("Url").Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        this.Urls.Add(node.Attribute("Url").Value);
                    }
                }
            }

            {
                var nodeAreas = docSiteMap.XPathSelectElements("SiteMap/Area");

                foreach (var node in nodeAreas)
                {
                    if (node.Attribute("Url") != null
                        && !string.IsNullOrEmpty(node.Attribute("Url").Value)
                        && !node.Attribute("Url").Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        this.Urls.Add(node.Attribute("Url").Value);
                    }

                    if (node.Attribute("Icon") != null
                        && !string.IsNullOrEmpty(node.Attribute("Icon").Value)
                        && !node.Attribute("Icon").Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        this.Icons.Add(node.Attribute("Icon").Value);
                    }

                    if (node.Attribute("ResourceId") != null
                        && !string.IsNullOrEmpty(node.Attribute("ResourceId").Value)
                    )
                    {
                        this.ResourceIds.Add(node.Attribute("ResourceId").Value);
                    }

                    if (node.Attribute("DescriptionResourceId") != null
                        && !string.IsNullOrEmpty(node.Attribute("DescriptionResourceId").Value)
                    )
                    {
                        this.DescriptionResourceIds.Add(node.Attribute("DescriptionResourceId").Value);
                    }

                    if (node.Attribute("ToolTipResourseId") != null
                        && !string.IsNullOrEmpty(node.Attribute("ToolTipResourseId").Value)
                    )
                    {
                        this.ToolTipResourseIds.Add(node.Attribute("ToolTipResourseId").Value);
                    }
                }
            }

            {
                var nodeGroups = docSiteMap.XPathSelectElements("SiteMap/Area/Group");

                foreach (var node in nodeGroups)
                {
                    if (node.Attribute("Url") != null
                        && !string.IsNullOrEmpty(node.Attribute("Url").Value)
                        && !node.Attribute("Url").Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        this.Urls.Add(node.Attribute("Url").Value);
                    }

                    if (node.Attribute("Icon") != null
                        && !string.IsNullOrEmpty(node.Attribute("Icon").Value)
                        && !node.Attribute("Icon").Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        this.Icons.Add(node.Attribute("Icon").Value);
                    }

                    if (node.Attribute("ResourceId") != null
                        && !string.IsNullOrEmpty(node.Attribute("ResourceId").Value
                    )
                    )
                    {
                        this.ResourceIds.Add(node.Attribute("ResourceId").Value);
                    }

                    if (node.Attribute("DescriptionResourceId") != null
                        && !string.IsNullOrEmpty(node.Attribute("DescriptionResourceId").Value)
                    )
                    {
                        this.DescriptionResourceIds.Add(node.Attribute("DescriptionResourceId").Value);
                    }

                    if (node.Attribute("ToolTipResourseId") != null
                        && !string.IsNullOrEmpty(node.Attribute("ToolTipResourseId").Value)
                    )
                    {
                        this.ToolTipResourseIds.Add(node.Attribute("ToolTipResourseId").Value);
                    }
                }
            }

            {
                var nodeSubAreas = docSiteMap.XPathSelectElements("SiteMap/Area/Group/SubArea");

                foreach (var node in nodeSubAreas)
                {
                    if (node.Attribute("Url") != null
                        && !string.IsNullOrEmpty(node.Attribute("Url").Value)
                        && !node.Attribute("Url").Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        this.Urls.Add(node.Attribute("Url").Value);
                    }

                    if (node.Attribute("Icon") != null
                        && !string.IsNullOrEmpty(node.Attribute("Icon").Value)
                        && !node.Attribute("Icon").Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        this.Icons.Add(node.Attribute("Icon").Value);
                    }

                    if (node.Attribute("OutlookShortcutIcon") != null
                        && !string.IsNullOrEmpty(node.Attribute("OutlookShortcutIcon").Value)
                        && !node.Attribute("OutlookShortcutIcon").Value.StartsWith("$webresource:", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        this.Icons.Add(node.Attribute("OutlookShortcutIcon").Value);
                    }

                    if (node.Attribute("ResourceId") != null
                        && !string.IsNullOrEmpty(node.Attribute("ResourceId").Value)
                    )
                    {
                        this.ResourceIds.Add(node.Attribute("ResourceId").Value);
                    }

                    if (node.Attribute("DescriptionResourceId") != null
                        && !string.IsNullOrEmpty(node.Attribute("DescriptionResourceId").Value)
                    )
                    {
                        this.DescriptionResourceIds.Add(node.Attribute("DescriptionResourceId").Value);
                    }

                    if (node.Attribute("ToolTipResourseId") != null
                        && !string.IsNullOrEmpty(node.Attribute("ToolTipResourseId").Value)
                    )
                    {
                        this.ToolTipResourseIds.Add(node.Attribute("ToolTipResourseId").Value);
                    }

                    if (node.Attribute("GetStartedPanePath") != null
                        && !string.IsNullOrEmpty(node.Attribute("GetStartedPanePath").Value)
                    )
                    {
                        this.GetStartedPanePaths.Add(node.Attribute("GetStartedPanePath").Value);
                    }

                    if (node.Attribute("GetStartedPanePathOutlook") != null
                        && !string.IsNullOrEmpty(node.Attribute("GetStartedPanePathOutlook").Value)
                    )
                    {
                        this.GetStartedPanePathOutlooks.Add(node.Attribute("GetStartedPanePathOutlook").Value);
                    }

                    if (node.Attribute("GetStartedPanePathAdmin") != null
                        && !string.IsNullOrEmpty(node.Attribute("GetStartedPanePathAdmin").Value)
                    )
                    {
                        this.GetStartedPanePathAdmins.Add(node.Attribute("GetStartedPanePathAdmin").Value);
                    }

                    if (node.Attribute("GetStartedPanePathAdminOutlook") != null
                        && !string.IsNullOrEmpty(node.Attribute("GetStartedPanePathAdminOutlook").Value)
                    )
                    {
                        this.GetStartedPanePathAdminOutlooks.Add(node.Attribute("GetStartedPanePathAdminOutlook").Value);
                    }

                    if (node.Attribute("CheckExtensionProperty") != null
                        && !string.IsNullOrEmpty(node.Attribute("CheckExtensionProperty").Value)
                    )
                    {
                        this.CheckExtensionProperties.Add(node.Attribute("CheckExtensionProperty").Value);
                    }
                }
            }
        }

        public void LoadDashboards(IEnumerable<SystemForm> systemForms)
        {
            if (!systemForms.Any())
            {
                return;
            }

            foreach (var item in systemForms)
            {
                if (!this.Dashboards.ContainsKey(item.Id))
                {
                    this.Dashboards.TryAdd(item.Id, new SystemFormIntellisenseData());
                }

                this.Dashboards[item.Id].LoadData(item);
            }
        }

        private static SiteMapIntellisenseData Get(Guid connectionId)
        {
            var filePath = GetFilePath(connectionId);

            SiteMapIntellisenseData result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(SiteMapIntellisenseData));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as SiteMapIntellisenseData;
                            result.FilePath = filePath;
                            result.ConnectionId = connectionId;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);

                        FileOperations.CreateBackUpFile(filePath, ex);
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }

            return result;
        }

        public void Save()
        {
            string filePath = GetFilePath(this.ConnectionId);

            if (!string.IsNullOrEmpty(FilePath))
            {
                filePath = FilePath;
            }

            this.Save(filePath);
        }

        private void Save(string filePath)
        {
            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(SiteMapIntellisenseData));

                    ser.WriteObject(memoryStream, this);

                    memoryStream.Seek(0, SeekOrigin.Begin);

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
                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

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
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }

        private static string GetFilePath(Guid connectionId)
        {
            string folderPath = FileOperations.GetConnectionIntellisenseDataFolderPath(connectionId);

            var fileName = "SiteMapIntellisenseData.xml";

            var filePath = Path.Combine(folderPath, fileName);

            return filePath;
        }

        private void MergeDataFromDisk(SiteMapIntellisenseData data)
        {
            if (data == null)
            {
                return;
            }

            CreateCollectionIfNeaded();

            LoadSortedSet(data, d => d.Urls);

            LoadSortedSet(data, d => d.ResourceIds);
            LoadSortedSet(data, d => d.DescriptionResourceIds);
            LoadSortedSet(data, d => d.ToolTipResourseIds);

            LoadSortedSet(data, d => d.Icons);

            LoadSortedSet(data, d => d.GetStartedPanePaths);
            LoadSortedSet(data, d => d.GetStartedPanePathOutlooks);
            LoadSortedSet(data, d => d.GetStartedPanePathAdmins);
            LoadSortedSet(data, d => d.GetStartedPanePathAdminOutlooks);

            LoadSortedSet(data, d => d.CheckExtensionProperties);

            foreach (var dashboardData in data.Dashboards.Values)
            {
                if (!this.Dashboards.ContainsKey(dashboardData.FormId.Value))
                {
                    this.Dashboards.TryAdd(dashboardData.FormId.Value, dashboardData);
                }
                else
                {
                    this.Dashboards[dashboardData.FormId.Value].MergeDataFromDisk(dashboardData);
                }
            }
        }

        private void LoadSortedSet(SiteMapIntellisenseData data, Func<SiteMapIntellisenseData, SortedSet<string>> getSortedSet)
        {
            var thisSortedSet = getSortedSet(this);
            var dataSortedSet = getSortedSet(data);

            foreach (var value in thisSortedSet)
            {
                if (!thisSortedSet.Contains(value))
                {
                    thisSortedSet.Add(value);
                }
            }
        }

        internal void GetDataFromDisk()
        {
            var data = Get(this.ConnectionId);

            if (data != null)
            {
                this.MergeDataFromDisk(data);
            }
        }
    }
}