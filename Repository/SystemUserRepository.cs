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

        public Task<List<SystemUser>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        private List<SystemUser> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SystemUser.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Orders =
                {
                    new OrderExpression(Report.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SystemUser>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SystemUser>()));

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