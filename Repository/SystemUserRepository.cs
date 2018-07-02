using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class SystemUserRepository
    {
        private IOrganizationServiceExtented _service;

        public SystemUserRepository(IOrganizationServiceExtented _service)
        {
            this._service = _service;
        }

        internal EntityReference FindUser(string fullname)
        {
            QueryExpression query = new QueryExpression()
            {
                ColumnSet = new ColumnSet(false),

                EntityName = SystemUser.EntityLogicalName,

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Schema.Attributes.fullname, ConditionOperator.Equal, fullname),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntityReference()).FirstOrDefault();
        }
    }
}
