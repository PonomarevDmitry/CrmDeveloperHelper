using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    class SdkMessageResponseFieldRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _Service { get; set; }

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageResponseFieldRepository(IOrganizationServiceExtented service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageResponseField>> GetListAsync(ColumnSet columnSet)
        {
            return Task.Run(() => GetList(columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SdkMessageResponseField> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageResponseField.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageResponseField>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageResponseField>()));

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

        public Task<SdkMessageResponseField> GetByIdAsync(Guid idSdkMessageResponseField, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSdkMessageResponseField, columnSet));
        }

        private SdkMessageResponseField GetById(Guid idSdkMessageResponseField, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageResponseField.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageResponseField.PrimaryIdAttribute, ConditionOperator.Equal, idSdkMessageResponseField),
                    },
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SdkMessageResponseField>()).SingleOrDefault();
        }

        public Task<List<SdkMessageResponseField>> GetListByResponseIdAsync(Guid idResponse, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByResponseId(idResponse, columnSet));
        }

        private List<SdkMessageResponseField> GetListByResponseId(Guid idResponse, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = SdkMessageResponseField.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageResponseField.Schema.Attributes.sdkmessageresponseid, ConditionOperator.Equal, idResponse),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageResponseField>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageResponseField>()));

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
