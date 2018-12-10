using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class SiteMapIntellisenseData
    {
        private const int _loadPeriodInMinutes = 5;

        private string FilePath { get; set; }

        public DateTime? NextLoadFileDate { get; set; }

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
        public ConcurrentDictionary<Guid, SystemForm> Dashboards { get; private set; }

        public SiteMapIntellisenseData()
        {
            ClearData();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
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
                this.Dashboards = new ConcurrentDictionary<Guid, SystemForm>();
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

            this.Dashboards = new ConcurrentDictionary<Guid, SystemForm>();
        }

        public void LoadDataFromSiteMap(XDocument docSiteMap)
        {
            if (docSiteMap == null)
            {
                return;
            }

            this.NextLoadFileDate = DateTime.Now.AddMinutes(_loadPeriodInMinutes);

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

            this.NextLoadFileDate = DateTime.Now.AddMinutes(_loadPeriodInMinutes);

            foreach (var item in systemForms)
            {
                if (!this.Dashboards.ContainsKey(item.Id))
                {
                    this.Dashboards.TryAdd(item.Id, item);
                }
            }
        }
    }
}