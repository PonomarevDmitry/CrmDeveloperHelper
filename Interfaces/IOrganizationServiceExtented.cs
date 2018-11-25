using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IOrganizationServiceExtented : IOrganizationService
    {
        string CurrentServiceEndpoint { get; }

        ConnectionData ConnectionData { get; }

        ConnectionDataUrlGenerator UrlGenerator { get; }

        bool IsRequestExists(string requestName);

        T RetrieveByQuery<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity;

        Task<T> RetrieveByQueryAsync<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity;

        Task<Guid> CreateAsync(Entity entity);

        Task UpdateAsync(Entity entity);

        Task DeleteAsync(string entityName, Guid id);
    }
}