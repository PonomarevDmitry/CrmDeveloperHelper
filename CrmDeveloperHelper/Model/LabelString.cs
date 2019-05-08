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

        public string GetValueJavaScript()
        {
            if (!string.IsNullOrEmpty(this.Value))
            {
                return this.Value.Replace("'", @"\'");
            }

            return string.Empty;
        }
    }
}