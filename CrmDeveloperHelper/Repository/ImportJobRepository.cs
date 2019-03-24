using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class ImportJobRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        public ImportJobRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<ImportJob>> GetListAsync(string filter, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filter, columnSet));
        }

        private List<ImportJob> GetList(string filter, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = ImportJob.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(false),

                Orders =
                {
                    new OrderExpression(ImportJob.Schema.Attributes.startedon, OrderType.Descending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(ImportJob.Schema.Attributes.solutionname, ConditionOperator.Like, "%" + filter + "%"));
            }

            var result = new List<ImportJob>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<ImportJob>()));

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
                Helpers.DTEHelper.WriteExceptionToOutput(_service.ConnectionData, ex);
            }

            return result;
        }

        public Task<ImportJob> GetByIdAsync(Guid idImportJob, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idImportJob, columnSet));
        }

        private ImportJob GetById(Guid idImportJob, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = ImportJob.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ImportJob.PrimaryIdAttribute, ConditionOperator.Equal, idImportJob),
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<ImportJob>()).SingleOrDefault();
        }

        public async Task<string> GetFormattedResultsAsync(Guid idImportJob)
        {
            var importLogRequest = new RetrieveFormattedImportJobResultsRequest
            {
                ImportJobId = idImportJob
            };

            var importLogResponse = (RetrieveFormattedImportJobResultsResponse)await _service.ExecuteAsync(importLogRequest);

            return importLogResponse.FormattedResults;
        }
    }
}