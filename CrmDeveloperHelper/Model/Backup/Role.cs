using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model.Backup
{
    [DataContract]
    public class Role
    {
        [DataMember(Order = 10)]
        public Guid Id { get; set; }

        [DataMember(Order = 20)]
        public Guid? TemplateId { get; set; }

        [DataMember(Order = 30)]
        public string Name { get; set; }

        [DataMember(Order = 40)]
        public List<RolePrivilege> RolePrivileges { get; set; }

        public Role()
        {
            this.RolePrivileges = new List<RolePrivilege>();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (this.RolePrivileges == null)
            {
                this.RolePrivileges = new List<RolePrivilege>();
            }
        }

        public Task SaveAsync(string filePath)
        {
            return Task.Run(() => Save(filePath));
        }

        private void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(Role));

            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        Encoding = Encoding.UTF8,
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
                    File.WriteAllBytes(filePath, fileBody);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);
                }
            }
        }

        public static Task<Role> LoadAsync(string filePath)
        {
            return Task.Run(() => Load(filePath));
        }

        private static Role Load(string filePath)
        {
            Role result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Role));

                try
                {
                    using (var sr = File.OpenRead(filePath))
                    {
                        result = ser.ReadObject(sr) as Role;
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
    }
}