using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class TranslationDisplayString
    {
        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public string StringKey { get; set; }

        [DataMember]
        public List<LabelString> Labels { get; set; }

        public TranslationDisplayString()
        {
            this.Labels = new List<LabelString>();
        }

        public TranslationDisplayString(string entityName, string stringKey) : this()
        {
            this.EntityName = entityName;
            this.StringKey = stringKey;
        }

        public override string ToString()
        {
            return string.Format("{0}; {1}; {2}", EntityName, StringKey, this.Labels.Count);
        }
    }
}