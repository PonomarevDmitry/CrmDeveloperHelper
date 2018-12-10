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
        private IOrganizationServiceExtented _Service { get; set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску решений.
        /// </summary>
        /// <param name="service"></param>
        public SolutionRepository(IOrganizationServiceExtented service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
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
                        LinkToAttributeName = Publisher.PrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },
                Orders =
                {
                    new OrderExpression(Solution.Schema.Attributes.installedon, OrderType.Descending),
                },
                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                var filter = query.Criteria.AddFilter(LogicalOperator.Or);
                filter.Conditions.Add(new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Like, "%" + name + "%"));
                filter.Conditions.Add(new ConditionExpression(Solution.Schema.Attributes.friendlyname, ConditionOperator.Like, "%" + name + "%"));
            }

            var result = new List<Solution>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Solution>()));

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
                        new ConditionExpression(Solution.PrimaryIdAttribute, ConditionOperator.Equal, id)
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
                        LinkToAttributeName = Publisher.PrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<Solution>()).First();
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
                        LinkToAttributeName = Publisher.PrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<Solution>()).SingleOrDefault();
        }

        public Task<List<Solution>> GetSolutionsAllAsync(string name = null, int? componentType = null, Guid? objectId = null)
        {
            return Task.Run(() => GetSolutionsAll(name, componentType, objectId));
        }

        private List<Solution> GetSolutionsAll(string name, int? componentType, Guid? objectId)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = Solution.EntityLogicalName,
                ColumnSet = new ColumnSet(true),

                //Criteria =
                //{
                //    Conditions =
                //    {
                //        new ConditionExpression("isvisible", ConditionOperator.Equal, true)
                //    }
                //},

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        EntityAlias = Solution.Schema.Attributes.publisherid,

                        LinkFromEntityName = Solution.EntityLogicalName,
                        LinkFromAttributeName = Solution.Schema.Attributes.publisherid,

                        LinkToEntityName = Publisher.EntityLogicalName,
                        LinkToAttributeName = Publisher.PrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },

                Orders =
                {
                    new OrderExpression(Solution.Schema.Attributes.installedon, OrderType.Descending),
                },
                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                var filter = query.Criteria.AddFilter(LogicalOperator.Or);
                filter.Conditions.Add(new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Like, "%" + name + "%"));
                filter.Conditions.Add(new ConditionExpression(Solution.Schema.Attributes.friendlyname, ConditionOperator.Like, "%" + name + "%"));
            }

            if (objectId.HasValue && componentType.HasValue)
            {
                query.LinkEntities.Add(new LinkEntity()
                {
                    LinkFromEntityName = Solution.EntityLogicalName,
                    LinkFromAttributeName = Solution.PrimaryIdAttribute,

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

            var result = new List<Solution>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Solution>()));

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
                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<Solution>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Solution>()));

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
                        LinkToAttributeName = Publisher.PrimaryIdAttribute,

                        Columns = new ColumnSet(Publisher.Schema.Attributes.customizationprefix)
                    }
                },
                Orders =
                {
                    new OrderExpression(Solution.Schema.Attributes.installedon, OrderType.Descending),
                },
                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                var filter = query.Criteria.AddFilter(LogicalOperator.Or);
                filter.Conditions.Add(new ConditionExpression(Solution.Schema.Attributes.uniquename, ConditionOperator.Like, "%" + name + "%"));
                filter.Conditions.Add(new ConditionExpression(Solution.Schema.Attributes.friendlyname, ConditionOperator.Like, "%" + name + "%"));
            }

            var result = new List<Solution>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Solution>()));

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