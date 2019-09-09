using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class StatusCodeViewItem
    {
        public int StateCode { get; private set; }

        public string StateCodeName { get; private set; }

        public int StatusCode { get; private set; }

        public string StatusCodeName { get; private set; }

        public Microsoft.Xrm.Sdk.Label StateCodeLabel { get; private set; }

        public Microsoft.Xrm.Sdk.Label StatusCodeLabel { get; private set; }

        public StatusOptionMetadata StatusOptionMetadata { get; private set; }

        public StatusCodeViewItem(
            int stateCode
            , string stateCodeName
            , int statusCode
            , string statusCodeName
            , Microsoft.Xrm.Sdk.Label stateCodeLabel
            , Microsoft.Xrm.Sdk.Label statusCodeLabel
            , StatusOptionMetadata statusOptionMetadata
        )
        {
            this.StateCode = stateCode;
            this.StateCodeName = stateCodeName;

            this.StatusCode = statusCode;
            this.StatusCodeName = statusCodeName;

            this.StateCodeLabel = stateCodeLabel;
            this.StatusCodeLabel = statusCodeLabel;

            this.StatusOptionMetadata = statusOptionMetadata;
        }
    }
}
