using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class Translation
    {
        [DataMember]
        public List<TranslationDisplayString> DisplayStrings { get; set; }

        [DataMember]
        public List<LocalizedLabel> LocalizedLabels { get; set; }

        public Translation()
        {
            this.DisplayStrings = new List<TranslationDisplayString>();
            this.LocalizedLabels = new List<LocalizedLabel>();
        }

        public static Translation Get(string dataPath)
        {
            Translation result = null;

            if (File.Exists(dataPath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Translation));

                try
                {
                    using (var sr = File.OpenRead(dataPath))
                    {
                        result = ser.ReadObject(sr) as Translation;
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);

                    FileOperations.CreateBackUpFile(dataPath, ex);

                    result = null;
                }
            }

            return result;
        }

        public static void Save(string filePath, Translation translation)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            DataContractSerializer ser = new DataContractSerializer(typeof(Translation));

            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    ser.WriteObject(memoryStream, translation);

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
    }
}