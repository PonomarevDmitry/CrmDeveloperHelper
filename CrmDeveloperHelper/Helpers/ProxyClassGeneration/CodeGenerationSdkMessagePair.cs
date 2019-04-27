using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public sealed class CodeGenerationSdkMessagePair
    {
        public Guid Id { get; }

        public string MessageNamespace { get; }

        public CodeGenerationSdkMessage Message { get; set; }

        public CodeGenerationSdkMessageRequest Request { get; set; }

        public CodeGenerationSdkMessageResponse Response { get; set; }

        public CodeGenerationSdkMessagePair(CodeGenerationSdkMessage message
            , Guid id
            , string messageNamespace
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequest request
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponse response
        )
        {
            this.Message = message;
            this.Id = id;
            this.MessageNamespace = messageNamespace;

            if (request != null)
            {
                this.Request = new CodeGenerationSdkMessageRequest(this, request.SdkMessageRequestId.Value, request.Name);
            }

            if (response != null)
            {
                this.Response = new CodeGenerationSdkMessageResponse(response.SdkMessageResponseId.Value);
            }
        }

        internal void Fill(
            IEnumerable<Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField> sdkMessageRequestFields
            , IEnumerable<Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField> sdkMessageResponseFields
        )
        {
            if (this.Request != null)
            {
                this.Request.Fill(sdkMessageRequestFields);
            }

            if (this.Response != null)
            {
                this.Response.Fill(sdkMessageResponseFields);
            }
        }
    }
}
