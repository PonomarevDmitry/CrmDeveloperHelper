using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.PluginExtraction
{
    [DataContract]
    public class PluginStep
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int? StateCode { get; set; }

        [DataMember]
        public string StateCodeName { get; set; }

        [DataMember]
        public int? StatusCode { get; set; }

        [DataMember]
        public string StatusCodeName { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string PrimaryEntity { get; set; }

        [DataMember]
        public string SecondaryEntity { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string RunInUserContext { get; set; }

        [DataMember]
        public int? Stage { get; set; }

        [DataMember]
        public string StageName { get; set; }

        [DataMember]
        public int? ExecutionMode { get; set; }

        [DataMember]
        public string ExecutionModeName { get; set; }

        [DataMember]
        public int? ExecutionOrder { get; set; }

        [DataMember]
        public string SupportedDeployment { get; set; }

        [DataMember]
        public int? SupportedDeploymentCode { get; set; }

        [DataMember]
        public string UnsecureConfiguration { get; set; }

        [DataMember]
        public string SecureConfiguration { get; set; }

        [DataMember]
        public string InvocationSource { get; set; }

        [DataMember]
        public int? InvocationSourceCode { get; set; }

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
        public bool? AsyncAutoDelete { get; set; }

        [DataMember]
        public List<string> FilteringAttributes { get; set; }

        [DataMember]
        public List<PluginImage> PluginImages { get; set; }

        public PluginStep()
        {
            this.PluginImages = new List<PluginImage>();
            this.FilteringAttributes = new List<string>();
        }

        internal static PluginStep GetObject(
            Entities.SdkMessageProcessingStep entStep
            , Entities.SdkMessage entMessage
            , Entities.SdkMessageFilter entFilter
            , Entities.SdkMessageProcessingStepSecureConfig entSecure
            )
        {
            var result = new PluginStep();

            result.Id = entStep.Id;

            result.Message = entMessage.Name;

            result.Name = entStep.Name;

            result.Description = entStep.Description;

            if (entFilter != null)
            {
                result.PrimaryEntity = entFilter.PrimaryObjectTypeCode;
                result.SecondaryEntity = entFilter.SecondaryObjectTypeCode;
            }
            else
            {
                result.PrimaryEntity = "none";
                result.SecondaryEntity = "none";
            }

            result.StateCode = (int)entStep.StateCode.GetValueOrDefault();
            result.StatusCode = entStep.StatusCode.Value;

            result.StateCodeName = entStep.FormattedValues[Entities.SdkMessageProcessingStep.Schema.Attributes.statecode];
            result.StatusCodeName = entStep.FormattedValues[Entities.SdkMessageProcessingStep.Schema.Attributes.statuscode];

            result.Stage = entStep.Stage.Value;
            result.StageName = entStep.FormattedValues[Entities.SdkMessageProcessingStep.Schema.Attributes.stage];

            result.ExecutionMode = entStep.Mode.Value;
            result.ExecutionModeName = entStep.FormattedValues[Entities.SdkMessageProcessingStep.Schema.Attributes.mode];

            result.AsyncAutoDelete = entStep.AsyncAutoDelete;

            result.ExecutionOrder = entStep.Rank;

            var user = entStep.ImpersonatingUserId;

            if (user != null)
            {
                result.RunInUserContext = user.Name;
            }
            else
            {
                result.RunInUserContext = "Calling User";
            }

            result.SupportedDeployment = entStep.FormattedValues[Entities.SdkMessageProcessingStep.Schema.Attributes.supporteddeployment];
            result.SupportedDeploymentCode = entStep.SupportedDeployment.Value;

            result.InvocationSource = entStep.FormattedValues[Entities.SdkMessageProcessingStep.Schema.Attributes.invocationsource];
#pragma warning disable CS0612 // 'SdkMessageProcessingStep.InvocationSource' is obsolete
            result.InvocationSourceCode = entStep.InvocationSource?.Value;
#pragma warning restore CS0612 // 'SdkMessageProcessingStep.InvocationSource' is obsolete

            result.CreatedBy = entStep.CreatedBy.Name;
            result.CreatedOn = entStep.CreatedOn;

            result.ModifiedBy = entStep.ModifiedBy.Name;
            result.ModifiedOn = entStep.ModifiedOn;

            result.ComponentState = entStep.FormattedValues[Entities.SdkMessageProcessingStep.Schema.Attributes.componentstate];
            result.ComponentStateCode = entStep.ComponentState.Value;

            result.UnsecureConfiguration = entStep.Configuration;

            if (entSecure != null)
            {
                result.SecureConfiguration = entSecure.SecureConfig;
            }

            result.FilteringAttributes.AddRange(entStep.FilteringAttributesStrings);

            return result;
        }
    }
}