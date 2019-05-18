using System;
using System.Linq;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public sealed class CodeGenerationSdkMessage
    {
        private const string EntityTypeName = "Microsoft.Xrm.Sdk.Entity,Microsoft.Xrm.Sdk";

        public string Name { get; }

        public Guid Id { get; }

        public bool IsPrivate { get; }

        public bool IsCustomAction { get; }

        public Dictionary<Guid, CodeGenerationSdkMessagePair> SdkMessagePairs { get; }

        public Dictionary<Guid, Entities.SdkMessageFilter> SdkMessageFilters { get; }

        public CodeGenerationSdkMessage(Guid id, string name, bool isPrivate)
          : this(id, name, isPrivate, 0)
        {
        }

        internal CodeGenerationSdkMessage(Guid id, string name, bool isPrivate, byte customizationLevel)
        {
            this.Id = id;
            this.IsPrivate = isPrivate;
            this.Name = name;
            this.IsCustomAction = customizationLevel != 0;

            this.SdkMessagePairs = new Dictionary<Guid, CodeGenerationSdkMessagePair>();
            this.SdkMessageFilters = new Dictionary<Guid, Entities.SdkMessageFilter>();
        }

        public bool IsGeneric(Entities.SdkMessageRequestField requestField)
        {
            if (string.Equals(requestField.ClrParser, EntityTypeName, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.SdkMessageFilters.Count > 1;
            }

            return false;
        }

        internal void Fill(
            IEnumerable<Entities.SdkMessageFilter> sdkMessageFilters
            , IEnumerable<Entities.SdkMessagePair> sdkMessagePairs

            , IEnumerable<Entities.SdkMessageRequest> sdkMessageRequests
            , IEnumerable<Entities.SdkMessageRequestField> sdkMessageRequestFields

            , IEnumerable<Entities.SdkMessageResponse> sdkMessageResponses
            , IEnumerable<Entities.SdkMessageResponseField> sdkMessageResponseFields
        )
        {
            foreach (var sdkMessagePair in sdkMessagePairs)
            {
                if (sdkMessagePair.SdkMessagePairId.HasValue && !this.SdkMessagePairs.ContainsKey(sdkMessagePair.SdkMessagePairId.Value))
                {
                    var request = sdkMessageRequests.FirstOrDefault(r => r.SdkMessagePairId?.Id == sdkMessagePair.SdkMessagePairId);
                    var response = sdkMessageResponses.FirstOrDefault(r => r.SdkMessageRequestId?.Id == request?.SdkMessageRequestId);

                    CodeGenerationSdkMessagePair codeSdkMessagePair = new CodeGenerationSdkMessagePair(this, sdkMessagePair.SdkMessagePairId.Value, sdkMessagePair.Namespace, request, response);
                    this.SdkMessagePairs.Add(sdkMessagePair.Id, codeSdkMessagePair);

                    var requestFields = sdkMessageRequestFields.Where(r => r.SdkMessageRequestId?.Id == request?.SdkMessageRequestId);
                    var responseFields = sdkMessageResponseFields.Where(r => r.SdkMessageResponseId?.Id == response?.SdkMessageResponseId);

                    codeSdkMessagePair.Fill(requestFields, responseFields);
                }
            }

            foreach (var sdkMessageFilter in sdkMessageFilters)
            {
                if (sdkMessageFilter.SdkMessageFilterId.HasValue && !this.SdkMessageFilters.ContainsKey(sdkMessageFilter.SdkMessageFilterId.Value))
                {
                    this.SdkMessageFilters.Add(sdkMessageFilter.SdkMessageFilterId.Value, sdkMessageFilter);
                }
            }
        }
    }
}
