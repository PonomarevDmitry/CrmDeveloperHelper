using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction
{
    [DataContract]
    public class PluginType
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsWorkflowActivity { get; set; }

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
        public List<PluginStep> PluginSteps { get; set; }

        public PluginType()
        {
            this.PluginSteps = new List<PluginStep>();
        }

        internal static PluginType GetObject(Entities.PluginType entPluginType)
        {
            var result = new PluginType();

            result.Name = entPluginType.Name;
            result.TypeName = entPluginType.TypeName;

            result.Description = entPluginType.Description;

            result.IsWorkflowActivity = entPluginType.IsWorkflowActivity.GetValueOrDefault();

            result.CreatedBy = entPluginType.CreatedBy.Name;
            result.CreatedOn = entPluginType.CreatedOn;

            result.ModifiedBy = entPluginType.ModifiedBy.Name;
            result.ModifiedOn = entPluginType.ModifiedOn;

            result.ComponentState = entPluginType.FormattedValues[Entities.PluginType.Schema.Attributes.componentstate];
            result.ComponentStateCode = entPluginType.ComponentState.Value;

            return result;
        }
    }
}