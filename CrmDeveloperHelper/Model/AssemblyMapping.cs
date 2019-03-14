using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class AssemblyMapping
    {
        [DataMember]
        public string AssemblyName { get; set; }

        [DataMember]
        public List<string> LocalAssemblyPathList { get; set; }

        public AssemblyMapping()
        {
            this.LocalAssemblyPathList = new List<string>();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (this.LocalAssemblyPathList == null)
            {
                this.LocalAssemblyPathList = new List<string>();
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            if (!string.IsNullOrEmpty(this.AssemblyName))
            {
                str.Append(this.AssemblyName);
            }

            if (str.Length > 0) { str.Append(" - "); }

            str.Append(LocalAssemblyPathList.Count);

            if (str.Length > 0)
            {
                return str.ToString();
            }
            else
            {
                return base.ToString();
            }
        }
    }
}