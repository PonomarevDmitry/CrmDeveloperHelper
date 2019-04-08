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
    public class SolutionDifferenceImage
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
        public string Solution1Name { get; set; }

        [DataMember]
        public string Solution2Name { get; set; }

        [DataMember]
        public List<SolutionImageComponent> OnlySolution1Components { get; set; }

        [DataMember]
        public List<SolutionImageComponent> OnlySolution2Components { get; set; }

        [DataMember]
        public List<SolutionImageComponent> CommonComponents { get; set; }

        public string FilePath { get; set; }

        public SolutionDifferenceImage()
        {
            this.CommonComponents = new List<SolutionImageComponent>();

            this.OnlySolution1Components = new List<SolutionImageComponent>();
            this.OnlySolution2Components = new List<SolutionImageComponent>();
        }

        public virtual Task SaveAsync(string filePath)
        {
            return Task.Run(() => Save(filePath));
        }

        private void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(SolutionDifferenceImage));

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

        public static Task<SolutionDifferenceImage> LoadAsync(string filePath)
        {
            return Task.Run(() => Load(filePath));
        }

        private static SolutionDifferenceImage Load(string filePath)
        {
            SolutionDifferenceImage result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(SolutionDifferenceImage));

                try
                {
                    using (var sr = File.OpenRead(filePath))
                    {
                        result = ser.ReadObject(sr) as SolutionDifferenceImage;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);

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
