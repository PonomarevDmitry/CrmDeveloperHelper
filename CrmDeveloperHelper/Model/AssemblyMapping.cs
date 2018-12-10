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
        public string LocalAssemblyPath { get; set; }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            if (!string.IsNullOrEmpty(this.AssemblyName))
            {
                str.Append(this.AssemblyName);
            }

            if (str.Length > 0) { str.Append(" - "); }

            str.Append(LocalAssemblyPath);

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