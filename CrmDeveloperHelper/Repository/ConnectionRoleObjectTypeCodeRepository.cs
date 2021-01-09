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
    public class ConnectionRoleObjectTypeCodeRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public ConnectionRoleObjectTypeCodeRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<ConnectionRoleObjectTypeCode>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        private List<ConnectionRoleObjectTypeCode> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = ConnectionRoleObjectTypeCode.EntityLogicalName,

                NoLock = true,

                ColumnSet = ColumnSetInstances.AllColumns,
            };

            return _service.RetrieveMultipleAll<ConnectionRoleObjectTypeCode>(query);
        }
    }
}
