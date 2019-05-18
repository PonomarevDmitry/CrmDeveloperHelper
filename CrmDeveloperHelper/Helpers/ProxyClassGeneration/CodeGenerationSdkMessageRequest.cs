using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public sealed class CodeGenerationSdkMessageRequest
    {
        public Guid Id { get; }

        public CodeGenerationSdkMessagePair MessagePair { get; }

        public string Name { get; }

        public Dictionary<int, Entities.SdkMessageRequestField> RequestFields { get; }

        public CodeGenerationSdkMessageRequest(CodeGenerationSdkMessagePair message, Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.MessagePair = message;
            this.RequestFields = new Dictionary<int, Entities.SdkMessageRequestField>();
        }

        internal void Fill(IEnumerable<Entities.SdkMessageRequestField> sdkMessageRequestFields)
        {
            foreach (var requestField in sdkMessageRequestFields)
            {
                if (requestField.Position.HasValue && !this.RequestFields.ContainsKey(requestField.Position.Value))
                {
                    this.RequestFields.Add(requestField.Position.Value, requestField);
                }
            }
        }
    }
}
