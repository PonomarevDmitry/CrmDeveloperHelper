﻿using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class WindowSettings
    {
        public string Path { get; set; }

        [DataMember]
        public double? Top { get; set; }

        [DataMember]
        public double? Left { get; set; }

        [DataMember]
        public double? Height { get; set; }

        [DataMember]
        public double? Width { get; set; }

        [DataMember]
        public System.Windows.WindowState WindowState { get; set; }

        [DataMember]
        public Dictionary<string, int> DictInt { get; private set; }

        [DataMember]
        public Dictionary<string, double> DictDouble { get; private set; }

        [DataMember]
        public Dictionary<string, bool> DictBool { get; private set; }

        [DataMember]
        public Dictionary<string, string> DictString { get; private set; }

        [DataMember]
        public Dictionary<string, double> GridViewColumnsWidths { get; private set; }

        public WindowSettings()
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

            if (this.DictDouble == null)
            {
                this.DictDouble = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (this.DictString == null)
            {
                this.DictString = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (this.GridViewColumnsWidths == null)
            {
                this.GridViewColumnsWidths = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        public static WindowSettings Get(string filePath)
        {
            WindowSettings result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(WindowSettings));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        try
                        {
                            using (var sr = File.OpenRead(filePath))
                            {
                                result = ser.ReadObject(sr) as WindowSettings;
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(null, ex);

                            FileOperations.CreateBackUpFile(filePath, ex);

                            result = new WindowSettings();
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }

                
            }
            else
            {
                result = new WindowSettings();
            }

            result.Path = filePath;

            result.DictBool.Remove(string.Empty);

            result.DictInt.Remove(string.Empty);

            result.DictString.Remove(string.Empty);

            return result;
        }

        private void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(WindowSettings));

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