using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SolutionComponentRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service { get; set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public SolutionComponentRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SolutionComponent>> GetSolutionComponentsAsync(Guid solutionId, ColumnSet columnSet)
        {
            return Task.Run(() => GetSolutionComponents(solutionId, columnSet));
        }

        private List<SolutionComponent> GetSolutionComponents(Guid solutionId, ColumnSet columnSet)
        {
            var query = new QueryExpression
            {
                NoLock = true,

                EntityName = SolutionComponent.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SolutionComponent.Schema.Attributes.solutionid, ConditionOperator.Equal, solutionId),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SolutionComponent>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SolutionComponent>()));

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

        public Task<List<SolutionComponent>> GetSolutionComponentsAsync(string solutionUniqueName, ColumnSet columnSet)
        {
            return Task.Run(() => GetSolutionComponents(solutionUniqueName, columnSet));
        }

        private List<SolutionComponent> GetSolutionComponents(string solutionUniqueName, ColumnSet columnSet)
        {
            var query = new QueryExpression
            {
                NoLock = true,

                EntityName = SolutionComponent.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = SolutionComponent.EntityLogicalName,
                        LinkFromAttributeName = SolutionComponent.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Equal, solutionUniqueName),
                            },
                        },
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SolutionComponent>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SolutionComponent>()));

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

        public Task<List<SolutionComponent>> GetSolutionComponentsForCollectionAsync(IEnumerable<Guid> solutionCollection, ColumnSet columnSet)
        {
            return Task.Run(() => GetSolutionComponentsForCollection(solutionCollection, columnSet));
        }

        private List<SolutionComponent> GetSolutionComponentsForCollection(IEnumerable<Guid> solutionCollection, ColumnSet columnSet)
        {
            var query = new QueryExpression
            {
                NoLock = true,

                EntityName = SolutionComponent.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SolutionComponent.Schema.Attributes.solutionid, ConditionOperator.In, solutionCollection.ToArray()),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SolutionComponent>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SolutionComponent>()));

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

        public Task<List<SolutionComponent>> GetSolutionComponentsByTypeAsync(Guid solutionId, ComponentType? componentType, ColumnSet columnSet)
        {
            return Task.Run(() => GetSolutionComponentsByType(solutionId, (int?)componentType, columnSet));
        }

        public Task<List<SolutionComponent>> GetSolutionComponentsByTypeAsync(Guid solutionId, int? componentType, ColumnSet columnSet)
        {
            return Task.Run(() => GetSolutionComponentsByType(solutionId, componentType, columnSet));
        }

        private List<SolutionComponent> GetSolutionComponentsByType(Guid solutionId, int? componentType, ColumnSet columnSet)
        {
            var query = new QueryExpression
            {
                NoLock = true,

                EntityName = SolutionComponent.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SolutionComponent.Schema.Attributes.solutionid, ConditionOperator.Equal, solutionId),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (componentType.HasValue)
            {
                query.Criteria.AddCondition(new ConditionExpression(SolutionComponent.Schema.Attributes.componenttype, ConditionOperator.Equal, componentType.Value));
            }

            var result = new List<SolutionComponent>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SolutionComponent>()));

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

        public Task AddSolutionComponentsAsync(string solutionTargetUniqueName, IEnumerable<SolutionComponent> componentesOnlyInSource)
        {
            return Task.Run(() => AddSolutionComponents(solutionTargetUniqueName, componentesOnlyInSource));
        }

        private void AddSolutionComponents(string solutionTargetUniqueName, IEnumerable<SolutionComponent> componentesOnlyInSource)
        {
            foreach (var component in componentesOnlyInSource.Where(c => c.ObjectId.HasValue && c.ComponentType != null).OrderBy(c => c.ComponentType.Value))
            {
                AddSolutionComponentRequest request = new AddSolutionComponentRequest()
                {
                    SolutionUniqueName = solutionTargetUniqueName,

                    ComponentType = component.ComponentType.Value,
                    ComponentId = component.ObjectId.Value,

                    AddRequiredComponents = false,
                };

                if (component.RootComponentBehavior != null)
                {
                    if (component.RootComponentBehavior.Value == (int)RootComponentBehavior.DoNotIncludeSubcomponents)
                    {
                        request.DoNotIncludeSubcomponents = true;
                    }
                    else if (component.RootComponentBehavior.Value == (int)RootComponentBehavior.IncludeAsShellOnly)
                    {
                        request.DoNotIncludeSubcomponents = true;
                        request.IncludedComponentSettingsValues = new string[0];
                    }
                }

                try
                {
                    var response = (AddSolutionComponentResponse)_service.Execute(request);
                }
                catch (Exception ex)
                {
                    Helpers.DTEHelper.WriteExceptionToOutput(ex);
                }
            }
        }

        public Task ClearSolutionAsync(string solutionUniqueName)
        {
            return Task.Run(() => ClearSolution(solutionUniqueName));
        }

        private void ClearSolution(string solutionUniqueName)
        {
            var components = this.GetSolutionComponents(solutionUniqueName, new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid, SolutionComponent.Schema.Attributes.rootcomponentbehavior));

            this.RemoveSolutionComponents(solutionUniqueName, components);
        }

        public Task RemoveSolutionComponentsAsync(string solutionUniqueName, IEnumerable<SolutionComponent> components)
        {
            return Task.Run(() => RemoveSolutionComponents(solutionUniqueName, components));
        }

        private void RemoveSolutionComponents(string solutionUniqueName, IEnumerable<SolutionComponent> components)
        {
            foreach (var component in components.Where(c => c.ObjectId.HasValue && c.ComponentType != null).OrderByDescending(c => c.ComponentType?.Value))
            {
                RemoveSolutionComponentRequest request = new RemoveSolutionComponentRequest()
                {
                    SolutionUniqueName = solutionUniqueName,

                    ComponentType = component.ComponentType.Value,
                    ComponentId = component.ObjectId.Value,
                };

                try
                {
                    var response = (RemoveSolutionComponentResponse)_service.Execute(request);
                }
                catch (Exception ex)
                {
                    Helpers.DTEHelper.WriteExceptionToOutput(ex);
                }
            }
        }

        public Task<List<SolutionComponent>> GetDistinctSolutionComponentsAsync()
        {
            return Task.Run(() => GetDistinctSolutionComponents());
        }

        private List<SolutionComponent> GetDistinctSolutionComponents()
        {
            var arrayComponentType = Enum.GetValues(typeof(ComponentType)).OfType<ComponentType>().Select(e => (int)e).ToArray();

            var query = new QueryExpression
            {
                NoLock = true,

                Distinct = true,

                EntityName = SolutionComponent.EntityLogicalName,

                ColumnSet = new ColumnSet(SolutionComponent.Schema.Attributes.componenttype, SolutionComponent.Schema.Attributes.objectid),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SolutionComponent.Schema.Attributes.objectid, ConditionOperator.NotNull),
                        new ConditionExpression(SolutionComponent.Schema.Attributes.componenttype, ConditionOperator.NotNull),
                        new ConditionExpression(SolutionComponent.Schema.Attributes.componenttype, ConditionOperator.NotIn, arrayComponentType),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SolutionComponent>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SolutionComponent>()));

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
                Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }
    }
}