using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class SolutionImage
    {
        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public string ConnectionName { get; set; }

        [DataMember]
        public string ConnectionDiscoveryService { get; set; }

        [DataMember]
        public string ConnectionOrganizationService { get; set; }

        [DataMember]
        public string ConnectionPublicUrl { get; set; }

        [DataMember]
        public string ConnectionOrganizationName { get; set; }

        [DataMember]
        public string ConnectionSystemUserName { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        [DataMember]
        public string ExecuteUserDomainName { get; set; }

        [DataMember]
        public string ExecuteUserName { get; set; }

        [DataMember]
        public List<SolutionImageComponent> Components { get; set; }

        public string FilePath { get; set; }

        public SolutionImage()
        {
            this.Components = new List<SolutionImageComponent>();
        }

        public Task SaveAsync(string filePath)
        {
            return Task.Run(() => Save(filePath));
        }

        private void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(SolutionImage));

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

        public static Task<SolutionImage> LoadAsync(string filePath)
        {
            return Task.Run(() => Load(filePath));
        }

        private static SolutionImage Load(string filePath)
        {
            SolutionImage result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(SolutionImage));

                try
                {
                    using (var sr = File.OpenRead(filePath))
                    {
                        result = ser.ReadObject(sr) as SolutionImage;
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

            result.AppendFormat("Connection to CRM:        PublicUrl: {0}", this.ConnectionPublicUrl).AppendLine();
            result.AppendFormat("DiscoveryService:         {0}", this.ConnectionDiscoveryService).AppendLine();
            result.AppendFormat("OrganizationService:      {0}", this.ConnectionOrganizationService);

            if (!string.IsNullOrEmpty(this.FilePath))
            {
                result.AppendLine().AppendLine(this.FilePath);
            }

            return result.ToString();
        }
    }
}
