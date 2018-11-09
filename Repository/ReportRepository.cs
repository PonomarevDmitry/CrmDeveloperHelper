using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class ReportRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service { get; set; }

        /// <summary>
        /// Конструктор репозитория функция по поиску веб-ресурсов.
        /// </summary>
        /// <param name="service"></param>
        public ReportRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<Report>> GetListAsync(string name, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(name, columnSet));
        }

        private List<Report> GetList(string name, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,
                EntityName = Report.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        //new ConditionExpression(Report.Schema.Attributes.iscustomizable, ConditionOperator.Equal, true),
                        new ConditionExpression(Report.Schema.Attributes.reporttypecode, ConditionOperator.NotEqual, 3),
                    },
                },

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

            if (!string.IsNullOrEmpty(name))
            {
                FilterExpression filter = new FilterExpression(LogicalOperator.Or);

                filter.AddCondition(Report.Schema.Attributes.name, ConditionOperator.Like, "%" + name + "%");
                filter.AddCondition(Report.Schema.Attributes.filename, ConditionOperator.Like, "%" + name + "%");

                query.Criteria.Filters.Add(filter);
            }

            var result = new List<Report>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Report>()));

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

        public Task<List<Report>> GetListAllForCompareAsync()
        {
            return Task.Run(() => GetListAllForCompare());
        }

        private List<Report> GetListAllForCompare()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = Report.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

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

            //query.Criteria.AddCondition(Report.Schema.Attributes.iscustomizable, ConditionOperator.Equal, true);

            var result = new List<Report>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<Report>()));

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

        public Task<Report> FindAsync(string fileName)
        {
            return Task.Run(() => Find(fileName));
        }

        private Report Find(string fileName)
        {
            var result = SearchSingleByFileName(fileName);

            if (result == null)
            {
                result = SearchSingleByFileName(Path.GetFileNameWithoutExtension(fileName));
            }

            if (result == null)
            {
                result = SearchSingleByName(fileName);
            }

            if (result == null)
            {
                result = SearchSingleByName(Path.GetFileNameWithoutExtension(fileName));
            }

            return result;
        }

        public Task<Report> GetByIdAsync(Guid idReport, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetById(idReport, columnSet));
        }

        private Report GetById(Guid idReport, ColumnSet columnSet = null)
        {
            try
            {
                var request = new RetrieveRequest()
                {
                    ColumnSet = columnSet ?? new ColumnSet(true),

                    Target = new EntityReference(Report.EntityLogicalName, idReport),
                };

                var response = (RetrieveResponse)_service.Execute(request);

                return response.Entity.ToEntity<Report>();
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);

                return null;
            }
        }

        private Report SearchSingleByName(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Report.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Report.Schema.Attributes.name, ConditionOperator.Equal, name),
                        new ConditionExpression(Report.Schema.Attributes.reporttypecode, ConditionOperator.NotEqual, 3),
                    }
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Report>()).SingleOrDefault() : null;
        }

        private Report SearchSingleByFileName(string fileName)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Report.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Report.Schema.Attributes.filename, ConditionOperator.Equal, fileName),
                        new ConditionExpression(Report.Schema.Attributes.reporttypecode, ConditionOperator.NotEqual, 3),
                    }
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Report>()).SingleOrDefault() : null;
        }

        public Report FindReportBySignature(int lcid, Guid signatureId, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = Report.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Report.Schema.Attributes.signaturelcid, ConditionOperator.Equal, lcid),
                        new ConditionExpression(Report.Schema.Attributes.signatureid, ConditionOperator.Equal, signatureId),
                    }
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<Report>()).SingleOrDefault() : null;
        }
    }
}
