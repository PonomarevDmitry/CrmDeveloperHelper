using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class LabelString
    {
        [DataMember]
        public int LanguageCode { get; set; }

        [DataMember]
        public string Locale { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}