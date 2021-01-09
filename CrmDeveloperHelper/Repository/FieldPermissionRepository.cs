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
    public class FieldPermissionRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public FieldPermissionRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<FieldPermission>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        private List<FieldPermission> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = FieldPermission.EntityLogicalName,

                NoLock = true,

                ColumnSet = ColumnSetInstances.AllColumns,

                Orders =
                {
                    new OrderExpression(FieldPermission.Schema.Attributes.entityname, OrderType.Ascending),
                    new OrderExpression(FieldPermission.Schema.Attributes.attributelogicalname, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<FieldPermission>(query);
        }
    }
}
