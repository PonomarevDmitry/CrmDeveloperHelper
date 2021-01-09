using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class DependencyNodeRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public DependencyNodeRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<DependencyNode>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        private List<DependencyNode> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = DependencyNode.EntityLogicalName,

                ColumnSet = columnSet ?? ColumnSetInstances.AllColumns,
            };

            return _service.RetrieveMultipleAll<DependencyNode>(query);
        }

        public Task<List<DependencyNode>> GetDistinctListUnknownComponentTypeAsync()
        {
            return Task.Run(() => GetDistinctListUnknownComponentType());
        }

        private List<DependencyNode> GetDistinctListUnknownComponentType()
        {
            var arrayComponentType = Enum.GetValues(typeof(ComponentType)).OfType<ComponentType>().Select(e => (int)e).ToArray();

            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = DependencyNode.EntityLogicalName,

                ColumnSet = new ColumnSet
                (
                    DependencyNode.Schema.Attributes.objectid
                    , DependencyNode.Schema.Attributes.componenttype
                ),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(DependencyNode.Schema.Attributes.objectid, ConditionOperator.NotNull),
                        new ConditionExpression(DependencyNode.Schema.Attributes.componenttype, ConditionOperator.NotNull),
                        new ConditionExpression(DependencyNode.Schema.Attributes.componenttype, ConditionOperator.NotIn, arrayComponentType),
                    },
                },
            };

            return _service.RetrieveMultipleAll<DependencyNode>(query);
        }

        public Task<List<DependencyNode>> GetDistinctListAsync()
        {
            return Task.Run(() => GetDistinctList());
        }

        private List<DependencyNode> GetDistinctList()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                Distinct = true,

                EntityName = DependencyNode.EntityLogicalName,

                ColumnSet = new ColumnSet
                (
                    DependencyNode.Schema.Attributes.objectid
                    , DependencyNode.Schema.Attributes.componenttype
                ),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(DependencyNode.Schema.Attributes.objectid, ConditionOperator.NotNull),
                        new ConditionExpression(DependencyNode.Schema.Attributes.componenttype, ConditionOperator.NotNull),
                    },
                },
            };

            return _service.RetrieveMultipleAll<DependencyNode>(query);
        }
    }
}