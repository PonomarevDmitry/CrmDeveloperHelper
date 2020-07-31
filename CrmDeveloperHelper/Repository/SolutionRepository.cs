using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SolutionRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public SolutionRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Solution>> FindSolutionsVisibleUnmanagedAsync(string name = null)
        {
            return Task.Run(() => FindSolutionsVisibleUnmanaged(name));
        }

        private List<Solution> FindSolutionsVisibleUnmanaged(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Solution.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Solution.Schema.Attributes.ismanaged, ConditionOperator.Equal, false),
                        new ConditionExpression(Solution.Schema.Attributes.isvisible, ConditionOperator.Equal, true)
                    }
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        EntityAlias = Solution.Schema.Attributes.publisherid,

                        LinkFromEntityName = Solution.EntityLogicalName,
                        LinkFromAttributeName = Solution.Schema.Attributes.publisherid,

                        LinkToEntityName = Publisher.EntityLogicalName,
                        LinkToAttributeName = Publisher.EntityPrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },

                Orders =
                {
                    new OrderExpression(Solution.Schema.Attributes.installedon, OrderType.Descending),
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                query.Criteria.Filters.Add(new FilterExpression(LogicalOperator.Or)
                {
                    Conditions =
                    {
                        new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Like, "%" + name + "%"),
                        new ConditionExpression(Solution.Schema.Attributes.friendlyname, ConditionOperator.Like, "%" + name + "%"),
                    },
                });
            }

            return _service.RetrieveMultipleAll<Solution>(query);
        }

        public Task<Solution> GetSolutionByIdAsync(Guid id)
        {
            return Task.Run(() => GetSolutionById(id));
        }

        private Solution GetSolutionById(Guid id)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Solution.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Solution.EntityPrimaryIdAttribute, ConditionOperator.Equal, id)
                    }
                },
                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        EntityAlias = Solution.Schema.Attributes.publisherid,

                        LinkFromEntityName = Solution.EntityLogicalName,
                        LinkFromAttributeName = Solution.Schema.Attributes.publisherid,

                        LinkToEntityName = Publisher.EntityLogicalName,
                        LinkToAttributeName = Publisher.EntityPrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Solution>()).SingleOrDefault() : null;
        }

        public Task<Solution> GetSolutionByUniqueNameAsync(string uniqueName)
        {
            return Task.Run(() => GetSolutionByUniqueName(uniqueName));
        }

        public Solution GetSolutionByUniqueName(string uniqueName)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Solution.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Equal, uniqueName)
                    }
                },
                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        EntityAlias = Solution.Schema.Attributes.publisherid,

                        LinkFromEntityName = Solution.EntityLogicalName,
                        LinkFromAttributeName = Solution.Schema.Attributes.publisherid,

                        LinkToEntityName = Publisher.EntityLogicalName,
                        LinkToAttributeName = Publisher.EntityPrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Solution>()).SingleOrDefault() : null;
        }

        public Task<List<Solution>> GetSolutionsAllAsync(string name = null, int? componentType = null, Guid? objectId = null)
        {
            return Task.Run(() => GetSolutionsAll(name, componentType, objectId));
        }

        private List<Solution> GetSolutionsAll(string name, int? componentType, Guid? objectId)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = Solution.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        EntityAlias = Solution.Schema.Attributes.publisherid,

                        LinkFromEntityName = Solution.EntityLogicalName,
                        LinkFromAttributeName = Solution.Schema.Attributes.publisherid,

                        LinkToEntityName = Publisher.EntityLogicalName,
                        LinkToAttributeName = Publisher.EntityPrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },

                Orders =
                {
                    new OrderExpression(Solution.Schema.Attributes.installedon, OrderType.Descending),
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                if (Guid.TryParse(name, out Guid id))
                {
                    query.Criteria.Conditions.Add(new ConditionExpression(Solution.Schema.Attributes.solutionid, ConditionOperator.Equal, id));
                }
                else
                {
                    query.Criteria.Filters.Add(new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Like, "%" + name + "%"),
                            new ConditionExpression(Solution.Schema.Attributes.friendlyname, ConditionOperator.Like, "%" + name + "%"),
                        },
                    });
                }
            }

            if (objectId.HasValue && componentType.HasValue)
            {
                query.LinkEntities.Add(new LinkEntity()
                {
                    LinkFromEntityName = Solution.EntityLogicalName,
                    LinkFromAttributeName = Solution.EntityPrimaryIdAttribute,

                    LinkToEntityName = SolutionComponent.EntityLogicalName,
                    LinkToAttributeName = SolutionComponent.Schema.Attributes.solutionid,

                    LinkCriteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(SolutionComponent.Schema.Attributes.componenttype, ConditionOperator.Equal, componentType.Value),
                            new ConditionExpression(SolutionComponent.Schema.Attributes.objectid, ConditionOperator.Equal, objectId.Value),
                        },
                    },
                });
            }

            return _service.RetrieveMultipleAll<Solution>(query);
        }

        public Task<List<Solution>> GetSolutionsVisibleUnmanagedAsync(IEnumerable<string> uniqueNames)
        {
            return Task.Run(() => GetSolutionsVisibleUnmanaged(uniqueNames));
        }

        public List<Solution> GetSolutionsVisibleUnmanaged(IEnumerable<string> uniqueNames)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Solution.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Solution.Schema.Attributes.ismanaged, ConditionOperator.Equal, false),
                        new ConditionExpression(Solution.Schema.Attributes.isvisible, ConditionOperator.Equal, true),
                        new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.In, uniqueNames.ToArray()),
                    }
                },
            };

            return _service.RetrieveMultipleAll<Solution>(query);
        }

        public Task<List<Solution>> GetListSolutionsUnmanagedAsync(string name = null)
        {
            return Task.Run(() => GetListSolutionsUnmanaged(name));
        }

        private List<Solution> GetListSolutionsUnmanaged(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Solution.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Solution.Schema.Attributes.ismanaged, ConditionOperator.Equal, false),
                    }
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        EntityAlias = Solution.Schema.Attributes.publisherid,

                        LinkFromEntityName = Solution.EntityLogicalName,
                        LinkFromAttributeName = Solution.Schema.Attributes.publisherid,

                        LinkToEntityName = Publisher.EntityLogicalName,
                        LinkToAttributeName = Publisher.EntityPrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },

                Orders =
                {
                    new OrderExpression(Solution.Schema.Attributes.installedon, OrderType.Descending),
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                query.Criteria.Filters.Add(new FilterExpression(LogicalOperator.Or)
                {
                    Conditions =
                    {
                        new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Like, "%" + name + "%"),
                        new ConditionExpression(Solution.Schema.Attributes.friendlyname, ConditionOperator.Like, "%" + name + "%"),
                    },
                });
            }

            return _service.RetrieveMultipleAll<Solution>(query);
        }
    }
}