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
            , Entities.SdkMessageRequest request
            , Entities.SdkMessageResponse response
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
            IEnumerable<Entities.SdkMessageRequestField> sdkMessageRequestFields
            , IEnumerable<Entities.SdkMessageResponseField> sdkMessageResponseFields
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
