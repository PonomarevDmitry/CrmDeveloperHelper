using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class UserControlSettings
    {
        public string Path { get; set; }

        [DataMember]
        public Dictionary<string, double> DictDouble { get; private set; }

        [DataMember]
        public Dictionary<string, int> DictInt { get; private set; }

        [DataMember]
        public Dictionary<string, bool> DictBool { get; private set; }

        [DataMember]
        public Dictionary<string, string> DictString { get; private set; }

        [DataMember]
        public Dictionary<string, double> GridViewColumnsWidths { get; private set; }

        public UserControlSettings()
        {
            this.DictBool = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
            this.DictInt = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            this.DictString = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            this.DictDouble = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);

            this.GridViewColumnsWidths = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (this.DictBool == null)
            {
                this.DictBool = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (this.DictInt == null)
            {
                this.DictInt = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (this.DictString == null)
            {
                this.DictString = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (this.DictDouble == null)
            {
                this.DictDouble = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (this.GridViewColumnsWidths == null)
            {
                this.GridViewColumnsWidths = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        public static UserControlSettings Get(string filePath)
        {
            UserControlSettings result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(UserControlSettings));

                try
                {
                    using (var sr = File.OpenRead(filePath))
                    {
                        result = ser.ReadObject(sr) as UserControlSettings;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    FileOperations.CreateBackUpFile(filePath, ex);

                    result = new UserControlSettings();
                }
            }
            else
            {
                result = new UserControlSettings();
            }

            result.Path = filePath;

            result.DictBool.Remove(string.Empty);

            result.DictInt.Remove(string.Empty);

            result.DictString.Remove(string.Empty);

            return result;
        }

        private void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(UserControlSettings));

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

        public void Save()
        {
            Save(this.Path);
        }

        public string GetValueString(string key)
        {
            if (this.DictString.ContainsKey(key))
            {
                return this.DictString[key];
            }

            return string.Empty;
        }

        public bool? GetValueBool(string key)
        {
            if (this.DictBool.ContainsKey(key))
            {
                return this.DictBool[key];
            }

            return null;
        }

        public int? GetValueInt(string key)
        {
            if (this.DictInt.ContainsKey(key))
            {
                return this.DictInt[key];
            }

            return null;
        }

        public double? GetValueDouble(string key)
        {
            if (this.DictDouble.ContainsKey(key))
            {
                return this.DictDouble[key];
            }

            return null;
        }
    }
}
