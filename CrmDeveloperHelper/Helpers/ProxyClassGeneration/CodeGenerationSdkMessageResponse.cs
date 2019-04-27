using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public sealed class CodeGenerationSdkMessageResponse
    {
        public Guid Id { get; }

        public Dictionary<int, Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField> ResponseFields { get; }

        public CodeGenerationSdkMessageResponse(Guid id)
        {
            this.Id = id;
            this.ResponseFields = new Dictionary<int, Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField>();
        }

        internal void Fill(IEnumerable<Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField> sdkMessageResponseFields)
        {
            foreach (var responseField in sdkMessageResponseFields)
            {
                if (responseField.Position.HasValue && !this.ResponseFields.ContainsKey(responseField.Position.Value))
                {
                    this.ResponseFields.Add(responseField.Position.Value, responseField);
                }
            }
        }
    }
}
