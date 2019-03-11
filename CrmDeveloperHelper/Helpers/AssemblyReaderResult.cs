using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    [DataContract]
    public sealed class AssemblyReaderResult
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string Culture { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string PublicKeyToken { get; set; }

        [DataMember]
        public List<string> Plugins { get; set; }

        [DataMember]
        public List<string> Workflows { get; set; }

        public byte[] Content { get; set; }

        public AssemblyReaderResult()
        {
            this.Plugins = new List<string>();
            this.Workflows = new List<string>();
        }
    }
}