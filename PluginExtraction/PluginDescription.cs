using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction
{
    [DataContract]
    public class PluginDescription
    {
        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public string DiscoveryService { get; set; }

        [DataMember]
        public string OrganizationService { get; set; }

        [DataMember]
        public string PublicUrl { get; set; }

        [DataMember]
        public string Organization { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        [DataMember]
        public string CRMConnectionUserName { get; set; }

        [DataMember]
        public string ExecuteUserDomainName { get; set; }

        [DataMember]
        public string ExecuteUserName { get; set; }

        [DataMember]
        public List<PluginAssembly> PluginAssemblies { get; set; }

        public string FilePath { get; set; }

        public PluginDescription()
        {
            this.PluginAssemblies = new List<PluginAssembly>();
        }

        public void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(PluginDescription));

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

        public static Task<PluginDescription> LoadAsync(string filePath)
        {
            return Task.Run(() => Load(filePath));
        }

        private static PluginDescription Load(string filePath)
        {
            PluginDescription result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(PluginDescription));

                try
                {
                    using (var sr = File.OpenRead(filePath))
                    {
                        result = ser.ReadObject(sr) as PluginDescription;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    result = null;
                }
            }

            return result;
        }

        public string GetConnectionInfo()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Connection to CRM:        PublicUrl: {0}", this.PublicUrl).AppendLine();
            result.AppendFormat("DiscoveryService:         {0}", this.DiscoveryService).AppendLine();
            result.AppendFormat("OrganizationService:      {0}", this.OrganizationService);

            if (!string.IsNullOrEmpty(this.FilePath))
            {
                result.AppendLine().AppendLine(this.FilePath);
            }

            return result.ToString();
        }
    }
}