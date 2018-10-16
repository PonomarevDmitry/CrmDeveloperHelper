using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction
{
    [DataContract]
    public class PluginAssembly
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SourceType { get; set; }

        [DataMember]
        public string IsolationMode { get; set; }

        [DataMember]
        public string ComponentState { get; set; }

        [DataMember]
        public int? ComponentStateCode { get; set; }

        [DataMember]
        public DateTime? CreatedOn { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime? ModifiedOn { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public List<PluginType> PluginTypes { get; set; }

        public PluginAssembly()
        {
            this.PluginTypes = new List<PluginType>();
        }

        internal static PluginAssembly GetObject(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.PluginAssembly entAssembly)
        {
            var result = new PluginAssembly();

            result.Name = entAssembly.Name;

            //result.IsolationMode = GetIsolationModeName(entAssembly.IsolationMode.Value);
            result.IsolationMode = entAssembly.FormattedValues[Entities.PluginAssembly.Schema.Attributes.isolationmode];

            //result.ComponentState = PluginAssembly.GetComponentStateName(entAssembly.ComponentState.Value);
            result.ComponentState = entAssembly.FormattedValues[Entities.PluginAssembly.Schema.Attributes.componentstate];
            result.ComponentStateCode = entAssembly.ComponentState.Value;

            //result.SourceType = GetSourceTypeName(entAssembly.SourceType.Value);

            result.SourceType = entAssembly.FormattedValues[Entities.PluginAssembly.Schema.Attributes.sourcetype];

            result.CreatedBy = entAssembly.CreatedBy.Name;
            result.CreatedOn = entAssembly.CreatedOn;

            result.ModifiedBy = entAssembly.ModifiedBy.Name;
            result.ModifiedOn = entAssembly.ModifiedOn;

            return result;
        }

        //private static string GetSourceTypeName(int sourcetype)
        //{
        //    switch (sourcetype)
        //    {
        //        case 0:
        //            return "Database";

        //        case 1:
        //            return "Disk";

        //        case 2:
        //            return "Normal";

        //        default:
        //            return string.Format("Unknown {0}", sourcetype);
        //    }
        //}

        //private static string GetIsolationModeName(int isolationMode)
        //{
        //    switch (isolationMode)
        //    {
        //        case 1:
        //            return "None";

        //        case 2:
        //            return "Sandbox";

        //        default:
        //            return string.Format("Unknown {0}", isolationMode);
        //    }
        //}

        //public static string GetComponentStateName(int componentstate)
        //{
        //    switch (componentstate)
        //    {
        //        case 0:
        //            return "Published";

        //        case 1:
        //            return "Unpublished";

        //        case 2:
        //            return "Deleted";

        //        case 3:
        //            return "Deleted_Unpublished";

        //        default:
        //            return string.Format("Unknown {0}", componentstate);
        //    }
        //}
    }
}