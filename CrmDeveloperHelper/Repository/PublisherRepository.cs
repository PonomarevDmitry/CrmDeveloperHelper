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
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория функция по поиску издателей.
        /// </summary>
        /// <param name="service"></param>
        public PublisherRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
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
                        LinkFromAttributeName = Publisher.EntityPrimaryIdAttribute,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.Schema.Attributes.publisherid,

                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(Solution.EntityPrimaryIdAttribute, ConditionOperator.Equal, solutionId)
                            }
                        }
                    }
                },
            };

            return _service.RetrieveMultipleAll<Publisher>(query);
        }

        public Task<Publisher> GetDefaultPublisherAsync()
        {
            return Task.Run(() => GetDefaultPublisher());
        }

        private Publisher GetDefaultPublisher()
        {
            var organization = _service.Retrieve(Organization.EntityLogicalName, _service.ConnectionData.OrganizationId.Value, new ColumnSet(Organization.Schema.Attributes.name)).ToEntity<Organization>();

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

            var coll = _service.RetrieveMultiple(query).Entities;

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

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<Publisher>()).FirstOrDefault();
        }
    }
}
