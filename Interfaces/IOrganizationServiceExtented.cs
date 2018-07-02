using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IOrganizationServiceExtented : IOrganizationService
    {
        string CurrentServiceEndpoint { get; }

        ConnectionData ConnectionData { get; }

        bool IsRequestExists(string requestName);
    }
}