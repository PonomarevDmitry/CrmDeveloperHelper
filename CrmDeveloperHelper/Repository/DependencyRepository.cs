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

        public Task<List<Dependency>> GetSolutionMissingDependenciesAsync(string solutionUniqueName)
        {
            return Task.Run(() => GetSolutionMissingDependencies(solutionUniqueName));
        }

        private List<Dependency> GetSolutionMissingDependencies(string solutionUniqueName)
        {
            var request = new RetrieveMissingDependenciesRequest
            {
                SolutionUniqueName = solutionUniqueName
            };

            var response = (RetrieveMissingDependenciesResponse)_service.Execute(request);

            var listComponent = response.EntityCollection.Entities.Select(e => e.ToEntity<Dependency>()).ToList();

            return listComponent;
        }

        public Task<List<Dependency>> GetSolutionDependenciesForUninstallAsync(string solutionUniqueName)
        {
            return Task.Run(() => GetSolutionDependenciesForUninstall(solutionUniqueName));
        }

        private List<Dependency> GetSolutionDependenciesForUninstall(string solutionUniqueName)
        {
            var request = new RetrieveDependenciesForUninstallRequest
            {
                SolutionUniqueName = solutionUniqueName
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
            };

            return _service.RetrieveMultipleAll<Dependency>(query);
        }

        public Task<List<SolutionComponent>> GetSolutionAllRequiredComponentsAsync(Guid solutionId, string solutionUniqueName)
        {
            return Task.Run(() => GetSolutionAllRequiredComponents(solutionId, solutionUniqueName));
        }

        private List<SolutionComponent> GetSolutionAllRequiredComponents(Guid solutionId, string solutionUniqueName)
        {
            var request = new RetrieveMissingDependenciesRequest
            {
                SolutionUniqueName = solutionUniqueName,
            };

            var response = (RetrieveMissingDependenciesResponse)_service.Execute(request);

            var listComponent = response.EntityCollection.Entities.Select(e => e.ToEntity<Dependency>()).ToList();

            GetAllRequiredComponents(listComponent);

            HashSet<Tuple<int, Guid>> result = new HashSet<Tuple<int, Guid>>();

            foreach (var item in listComponent.Where(d => d.RequiredComponentObjectId.HasValue && d.RequiredComponentType != null))
            {
                result.Add(Tuple.Create(item.RequiredComponentType.Value, item.RequiredComponentObjectId.Value));
            }

            var repository = new SolutionComponentRepository(_service);

            var components = repository.GetSolutionComponents(solutionId, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

            foreach (var item in components.Where(c => c.ObjectId.HasValue && c.ComponentType != null))
            {
                var key = Tuple.Create(item.ComponentType.Value, item.ObjectId.Value);

                result.Remove(key);
            }

            return result.Select(c => new SolutionComponent()
            {
                ComponentType = new OptionSetValue(c.Item1),
                ObjectId = c.Item2,
            }).ToList();
        }

        private void GetAllRequiredComponents(List<Dependency> listComponent)
        {
            var currentList = new HashSet<Guid>(listComponent.Where(c => c.RequiredComponentNodeId != null).Select(c => c.RequiredComponentNodeId.Id));

            HashSet<Guid> componentsToRequest = new HashSet<Guid>(currentList);

            while (componentsToRequest.Any())
            {
                var requiredForItemList = this.GetRequiredComponents(componentsToRequest.ToArray());

                componentsToRequest.Clear();

                foreach (var item in requiredForItemList.Where(d => d.RequiredComponentNodeId != null))
                {
                    if (currentList.Add(item.RequiredComponentNodeId.Id))
                    {
                        listComponent.Add(item);
                        componentsToRequest.Add(item.RequiredComponentNodeId.Id);
                    }
                }
            }
        }

        private List<Dependency> GetRequiredComponents(Guid[] idNodes)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = Dependency.EntityLogicalName,

                NoLock = true,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Dependency.Schema.Attributes.dependentcomponentnodeid, ConditionOperator.In, idNodes),
                    },
                },
            };

            return _service.RetrieveMultipleAll<Dependency>(query);
        }
    }
}
