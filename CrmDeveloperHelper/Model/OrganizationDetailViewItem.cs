using Microsoft.Xrm.Sdk.Discovery;
using System;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class OrganizationDetailViewItem
    {
        public Uri DiscoveryUri { get; private set; }

        public OrganizationDetail Detail { get; private set; }

        public ConnectionUserData User { get; private set; }

        public string Tooltip { get; private set; }

        public Guid OrganizationId => this.Detail.OrganizationId;

        public string FriendlyName => this.Detail.FriendlyName;

        public string OrganizationVersion => this.Detail.OrganizationVersion;

        public string UrlName => this.Detail.UrlName;

        public string UniqueName => this.Detail.UniqueName;

        public OrganizationState State => this.Detail.State;

        public EndpointCollection Endpoints => this.Detail.Endpoints;

        public string OrganizationServiceEndpoint => this.Detail.Endpoints[EndpointType.OrganizationService];

        public string OrganizationDataServiceEndpoint => this.Detail.Endpoints[EndpointType.OrganizationDataService];

        public string WebApplicationEndpoint => this.Detail.Endpoints[EndpointType.WebApplication];

        public OrganizationDetailViewItem(Uri discoveryUri, ConnectionUserData user, OrganizationDetail detail)
        {
            this.DiscoveryUri = discoveryUri;
            this.User = user;
            this.Detail = detail;

            SetToolTip();
        }

        private void SetToolTip()
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(this.FriendlyName))
            {
                if (result.Length > 0) { result.AppendLine(); }

                result.AppendFormat("FriendlyName: {0}", this.FriendlyName);
            }

            if (!string.IsNullOrEmpty(this.UrlName))
            {
                if (result.Length > 0) { result.AppendLine(); }

                result.AppendFormat("UrlName: {0}", this.UrlName);
            }

            if (result.Length > 0) { result.AppendLine(); }
            result.AppendFormat("OrganizationId: {0}", this.OrganizationId);

            if (result.Length > 0) { result.AppendLine(); }
            result.AppendFormat("OrganizationState: {0}", this.State.ToString());

            if (!string.IsNullOrEmpty(this.WebApplicationEndpoint))
            {
                if (result.Length > 0) { result.AppendLine(); }

                result.Append(this.WebApplicationEndpoint);
            }

            if (!string.IsNullOrEmpty(this.OrganizationServiceEndpoint))
            {
                if (result.Length > 0) { result.AppendLine(); }

                result.Append(this.OrganizationServiceEndpoint);
            }

            if (!string.IsNullOrEmpty(this.OrganizationDataServiceEndpoint))
            {
                if (result.Length > 0) { result.AppendLine(); }

                result.Append(this.OrganizationDataServiceEndpoint);
            }

            if (result.Length > 0)
            {
                this.Tooltip = result.ToString();
            }
        }
    }
}