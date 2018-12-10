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
    public class PublisherRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _Service { get; set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску издателей.
        /// </summary>
        /// <param name="service"></param>
        public PublisherRepository(IOrganizationServiceExtented service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        private Task<List<Publisher>> GetSolutionPublisherAsync(Guid solutionId)
        {
            return Task.Run(() => GetSolutionPublisher(solutionId));
        }

        private List<Publisher> GetSolutionPublisher(Guid solutionId)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Publisher.EntityLogicalName,

                ColumnSet = new ColumnSet(Publisher.Schema.Attributes.customizationprefix),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = Publisher.EntityLogicalName,
                        LinkFromAttributeName = Publisher.PrimaryIdAttribute,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.Schema.Attributes.publisherid,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(Solution.PrimaryIdAttribute, ConditionOperator.Equal, solutionId)
                            }
                        }
                    }
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<Publisher>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Publisher>()));

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

        public Task<Publisher> GetDefaultPublisherAsync()
        {
            return Task.Run(() => GetDefaultPublisher());
        }

        private Publisher GetDefaultPublisher()
        {
            var organization = _Service.Retrieve(Organization.EntityLogicalName, _Service.ConnectionData.OrganizationId.Value, new ColumnSet(Organization.Schema.Attributes.name)).ToEntity<Organization>();

            var defaultPublisherName = "DefaultPublisher" + organization.Name;

            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Publisher.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Publisher.Schema.Attributes.uniquename, ConditionOperator.Equal, defaultPublisherName),
                    },
                },

                Orders =
                {
                    new OrderExpression(Publisher.Schema.Attributes.createdon, OrderType.Ascending),
                },
            };

            var coll = _Service.RetrieveMultiple(query).Entities;

            var result = coll.Count == 1 ? coll.Select(e => e.ToEntity<Publisher>()).SingleOrDefault() : null;

            if (result != null)
            {
                return result;
            }

            query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Publisher.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Publisher.Schema.Attributes.uniquename, ConditionOperator.Like, "DefaultPublisher%"),
                    },
                },

                Orders =
                {
                    new OrderExpression(Publisher.Schema.Attributes.createdon, OrderType.Ascending),
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<Publisher>()).FirstOrDefault();
        }
    }
}
