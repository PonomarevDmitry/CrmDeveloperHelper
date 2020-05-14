using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class RibbonGroupTemplate
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public List<string> TemplateAliases { get; private set; } = new List<string>();
    }
}