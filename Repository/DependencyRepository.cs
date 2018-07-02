using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class DependencyRepository
    {

        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service { get; set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public DependencyRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Dependency>> GetDependentComponentsAsync(int componentType, Guid metadataId)
        {
            return Task.Run(() => GetDependentComponents(componentType, metadataId));
        }

        private List<Dependency> GetDependentComponents(int componentType, Guid metadataId)
        {
            var request = new RetrieveDependentComponentsRequest
            {
                ComponentType = componentType,
                ObjectId = metadataId
            };

            var response = (RetrieveDependentComponentsResponse)_service.Execute(request);

            var listComponent = response.EntityCollection.Entities.Select(e => e.ToEntity<Dependency>()).ToList();

            return listComponent;
        }

        public Task<List<Dependency>> GetRequiredComponentsAsync(int componentType, Guid metadataId)
        {
            return Task.Run(() => GetRequiredComponents(componentType, metadataId));
        }

        private List<Dependency> GetRequiredComponents(int componentType, Guid metadataId)
        {
            var request = new RetrieveRequiredComponentsRequest
            {
                ComponentType = componentType,
                ObjectId = metadataId
            };

            var response = (RetrieveRequiredComponentsResponse)_service.Execute(request);

            var listComponent = response.EntityCollection.Entities.Select(e => e.ToEntity<Dependency>()).ToList();

            return listComponent;
        }

        public Task<List<Dependency>> GetDependenciesForDeleteAsync(int componentType, Guid metadataId)
        {
            return Task.Run(() => GetDependenciesForDelete(componentType, metadataId));
        }

        private List<Dependency> GetDependenciesForDelete(int componentType, Guid metadataId)
        {
            var request = new RetrieveDependenciesForDeleteRequest
            {
                ComponentType = componentType,
                ObjectId = metadataId
            };

            var response = (RetrieveDependenciesForDeleteResponse)_service.Execute(request);

            var listComponent = response.EntityCollection.Entities.Select(e => e.ToEntity<Dependency>()).ToList();

            return listComponent;
        }

        public Task<List<Dependency>> GetSolutionMissingDependenciesAsync(string name)
        {
            return Task.Run(() => GetSolutionMissingDependencies(name));
        }

        private List<Dependency> GetSolutionMissingDependencies(string name)
        {
            var request = new RetrieveMissingDependenciesRequest
            {
                SolutionUniqueName = name
            };

            var response = (RetrieveMissingDependenciesResponse)_service.Execute(request);

            var listComponent = response.EntityCollection.Entities.Select(e => e.ToEntity<Dependency>()).ToList();

            return listComponent;
        }

        public Task<List<Dependency>> GetSolutionDependenciesForUninstallAsync(string name)
        {
            return Task.Run(() => GetSolutionDependenciesForUninstall(name));
        }

        private List<Dependency> GetSolutionDependenciesForUninstall(string name)
        {
            var request = new RetrieveDependenciesForUninstallRequest
            {
                SolutionUniqueName = name
            };

            var response = (RetrieveDependenciesForUninstallResponse)_service.Execute(request);

            var listComponent = response.EntityCollection.Entities.Select(e => e.ToEntity<Dependency>()).ToList();

            return listComponent;
        }

        public Task<List<Dependency>> GetDistinctListAsync()
        {
            return Task.Run(() => GetDistinctList());
        }

        private List<Dependency> GetDistinctList()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = Dependency.EntityLogicalName,

                ColumnSet = new ColumnSet
                (
                    Dependency.Schema.Attributes.dependentcomponentobjectid
                    , Dependency.Schema.Attributes.dependentcomponenttype
                    , Dependency.Schema.Attributes.requiredcomponentobjectid
                    , Dependency.Schema.Attributes.requiredcomponenttype
                ),

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            List<Dependency> result = new List<Dependency>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Dependency>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }
    }
}
