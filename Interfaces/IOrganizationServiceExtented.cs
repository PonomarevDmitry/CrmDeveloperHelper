using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IOrganizationServiceExtented : IOrganizationService
    {
        string CurrentServiceEndpoint { get; }

        ConnectionData ConnectionData { get; }

        bool IsRequestExists(string requestName);

        T RetrieveByQuery<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity;
    }
}