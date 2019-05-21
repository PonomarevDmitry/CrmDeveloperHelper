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
    public class ConnectionRoleAssociationRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public ConnectionRoleAssociationRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<ConnectionRoleAssociation>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        private List<ConnectionRoleAssociation> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = ConnectionRoleAssociation.EntityLogicalName,

                NoLock = true,

                ColumnSet = new ColumnSet(true),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        LinkFromEntityName = ConnectionRoleAssociation.EntityLogicalName,
                        LinkFromAttributeName = ConnectionRoleAssociation.Schema.Attributes.associatedconnectionroleid,

                        LinkToEntityName = ConnectionRole.EntityLogicalName,
                        LinkToAttributeName = ConnectionRole.EntityPrimaryIdAttribute,

                        EntityAlias = ConnectionRole.EntityLogicalName,

                        Columns = new ColumnSet(ConnectionRole.Schema.Attributes.name),
                    },
                },
            };

            return _service.RetrieveMultipleAll<ConnectionRoleAssociation>(query);
        }
    }
}
