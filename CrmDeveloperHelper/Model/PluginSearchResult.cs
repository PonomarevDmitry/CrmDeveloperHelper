using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class PluginSearchResult
    {
        //public List<PluginAssembly> PluginAssembly { get; set; }

        //public List<PluginType> PluginType { get; set; }

        //public List<SdkMessage> SdkMessage { get; set; }

        //public List<SdkMessageFilter> SdkMessageFilter { get; set; }

        public List<SdkMessageProcessingStep> SdkMessageProcessingStep { get; set; }

        public List<SdkMessageProcessingStepImage> SdkMessageProcessingStepImage { get; set; }

        //public List<SdkMessageProcessingStepSecureConfig> SdkMessageProcessingStepSecureConfig { get; set; }
    }
}