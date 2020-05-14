using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class ConnectionRibbonIntellisenseData
    {
        public Guid ConnectionId { get; set; }

        public RibbonIntellisenseData ApplicationRibbonData { get; set; }

        public ConcurrentDictionary<string, RibbonIntellisenseData> EntitiesRibbonData { get; private set; }

        public ConnectionRibbonIntellisenseData()
        {
            CreateCollectionsIfNeeded();
        }

        private void CreateCollectionsIfNeeded()
        {
            if (this.ApplicationRibbonData == null)
            {
                this.ApplicationRibbonData = new RibbonIntellisenseData();
            }

            if (EntitiesRibbonData == null)
            {
                this.EntitiesRibbonData = new ConcurrentDictionary<string, RibbonIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        public void GetDataFromDisk()
        {
            CreateCollectionsIfNeeded();

            var task = Task.Run(() =>
            {
                string directoryConnection = FileOperations.GetConnectionIntellisenseDataFolderPath(this.ConnectionId);

                var applicationRibbonFilePath = Path.Combine(directoryConnection, "ApplicationRibbon.zip");

                if (File.Exists(applicationRibbonFilePath))
                {
                    var ribbonData = RibbonIntellisenseData.Get(applicationRibbonFilePath, string.Empty);

                    if (ribbonData != null)
                    {
                        this.ApplicationRibbonData = ribbonData;
                    }
                }
            });

            string directoryEntityRibbons = FileOperations.GetConnectionIntellisenseDataFolderPathRibbons(this.ConnectionId);

            var directoryInfo = new DirectoryInfo(directoryEntityRibbons);

            if (directoryInfo.Exists)
            {
                var files = directoryInfo.GetFiles("*.zip");

                Parallel.ForEach(files, file =>
                {
                    string entityName = Path.GetFileNameWithoutExtension(file.FullName).ToLower();

                    var ribbonData = RibbonIntellisenseData.Get(file.FullName, entityName);

                    if (ribbonData != null)
                    {
                        if (!this.EntitiesRibbonData.ContainsKey(ribbonData.EntityLogicalName))
                        {
                            this.EntitiesRibbonData.TryAdd(ribbonData.EntityLogicalName, ribbonData);
                        }
                        else
                        {
                            this.EntitiesRibbonData[ribbonData.EntityLogicalName] = ribbonData;
                        }
                    }
                });
            }
        }
    }
}