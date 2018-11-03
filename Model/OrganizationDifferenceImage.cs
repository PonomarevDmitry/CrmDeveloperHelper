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
    public class OrganizationDifferenceImage
    {
        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public SolutionImage Connection1Image { get; set; }

        [DataMember]
        public SolutionImage Connection2Image { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        [DataMember]
        public string ExecuteUserDomainName { get; set; }

        [DataMember]
        public string ExecuteUserName { get; set; }

        [DataMember]
        public List<OrganizationDifferenceImageComponent> DifferentComponents { get; set; }

        public string FilePath { get; set; }

        public OrganizationDifferenceImage()
        {
            this.DifferentComponents = new List<OrganizationDifferenceImageComponent>();
        }

        public void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(OrganizationDifferenceImage));

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

        public static Task<OrganizationDifferenceImage> LoadAsync(string filePath)
        {
            return Task.Run(() => Load(filePath));
        }

        private static OrganizationDifferenceImage Load(string filePath)
        {
            OrganizationDifferenceImage result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(OrganizationDifferenceImage));

                try
                {
                    using (var sr = File.OpenRead(filePath))
                    {
                        result = ser.ReadObject(sr) as OrganizationDifferenceImage;
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
    }
}
