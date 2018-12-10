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
                        LinkToAttributeName = ConnectionRole.PrimaryIdAttribute,

                        EntityAlias = ConnectionRole.EntityLogicalName,

                        Columns = new ColumnSet(ConnectionRole.Schema.Attributes.name),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<ConnectionRoleAssociation>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<ConnectionRoleAssociation>()));

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
