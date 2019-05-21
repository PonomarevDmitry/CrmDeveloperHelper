using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    /// <summary>
    /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy"/>
    /// </summary>
    public interface IOrganizationServiceExtented : IOrganizationService
    {
        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.CurrentServiceEndpoint"/>
        /// </summary>
        string CurrentServiceEndpoint { get; }

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.ConnectionData"/>
        /// </summary>
        ConnectionData ConnectionData { get; }

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.UrlGenerator"/>
        /// </summary>
        ConnectionDataUrlGenerator UrlGenerator { get; }

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.IsRequestExists"/>
        /// </summary>
        bool IsRequestExists(string requestName);

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.Retrieve"/>
        /// </summary>
        T Retrieve<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity;

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.RetrieveAsync"/>
        /// </summary>
        Task<T> RetrieveAsync<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity;

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.RetrieveByQuery"/>
        /// </summary>
        T RetrieveByQuery<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity;

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.RetrieveByQueryAsync"/>
        /// </summary>
        Task<T> RetrieveByQueryAsync<T>(string entityName, Guid id, ColumnSet columnSet) where T : Entity;

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.CreateAsync"/>
        /// </summary>
        Task<Guid> CreateAsync(Entity entity);

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.UpdateAsync"/>
        /// </summary>
        Task UpdateAsync(Entity entity);

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.DeleteAsync"/>
        /// </summary>
        Task DeleteAsync(string entityName, Guid id);

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.UpsertAsync"/>
        /// </summary>
        Task<Guid> UpsertAsync(Entity entity);

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.ExecuteAsync"/>
        /// </summary>
        Task<T> ExecuteAsync<T>(OrganizationRequest request) where T : OrganizationResponse;

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.RetrieveMultipleAll"/>
        /// </summary>
        List<T> RetrieveMultipleAll<T>(QueryExpression query) where T : Entity;

        /// <summary>
        /// Default Implementation <see cref="Model.OrganizationServiceExtentedProxy.RetrieveMultipleAllAsync"/>
        /// </summary>
        Task<List<T>> RetrieveMultipleAllAsync<T>(QueryExpression query) where T : Entity;
    }
}