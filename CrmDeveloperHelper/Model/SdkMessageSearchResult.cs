using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class SdkMessageSearchResult
    {
        public List<SdkMessageRequest> Requests { get; set; }

        public List<SdkMessageResponse> Responses { get; set; }

        public SdkMessageSearchResult()
        {
            this.Requests = new List<SdkMessageRequest>();

            this.Responses = new List<SdkMessageResponse>();
        }
    }
}