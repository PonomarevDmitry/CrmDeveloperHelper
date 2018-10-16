using Microsoft.Crm.Sdk;
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
    /// <summary>
    /// Репозиторий для работы с SavedQuery
    /// </summary>
    public class SavedQueryRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SavedQueryRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SavedQuery>> GetListAsync(string filterEntity, ColumnSet columnSet)
        {
            return Task.Run(() => GetList(filterEntity, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SavedQuery> GetList(string filterEntity, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SavedQuery.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQuery.Schema.Attributes.componentstate, ConditionOperator.In, 0, 1),
                        new ConditionExpression(SavedQuery.Schema.Attributes.statecode, ConditionOperator.Equal, 0),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQuery.Schema.Attributes.returnedtypecode, OrderType.Ascending),
                    new OrderExpression(SavedQuery.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SavedQuery.Schema.Attributes.returnedtypecode, ConditionOperator.Equal, filterEntity));
            }

            var result = new List<SavedQuery>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SavedQuery>()));

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

        internal static string GetQueryTypeName(int queryType)
        {
            //public const int MainApplicationView = 0;
            //public const int AdvancedSearch = 1;
            //public const int SubGrid = 2;
            //public const int QuickFindSearch = 4;
            //public const int Reporting = 8;
            //public const int OfflineFilters = 16;
            //public const int LookupView = 64;
            //public const int SMAppointmentBookView = 128;
            //public const int OutlookFilters = 256;
            //public const int AddressBookFilters = 512;
            //public const int MainApplicationViewWithoutSubject = 1024;
            //public const int SavedQueryTypeOther = 2048;
            //public const int InteractiveWorkflowView = 4096;
            //public const int OfflineTemplate = 8192;
            //public const int CustomDefinedView = 16384;
            //public const int ExportFieldTranslationsView = 65536;
            //public const int OutlookTemplate = 131072;

            switch (queryType)
            {
                case SavedQueryQueryType.MainApplicationView:
                    return "Main Application View";

                case SavedQueryQueryType.AdvancedSearch:
                    return "Advanced Search";

                case SavedQueryQueryType.SubGrid:
                    return "SubGrid";

                case SavedQueryQueryType.QuickFindSearch:
                    return "Quick Find Search";

                case SavedQueryQueryType.Reporting:
                    return "Reporting";

                case SavedQueryQueryType.OfflineFilters:
                    return "Offline Filters";

                case SavedQueryQueryType.LookupView:
                    return "Lookup View";

                case SavedQueryQueryType.SMAppointmentBookView:
                    return "SM Appointment Book View";

                case SavedQueryQueryType.OutlookFilters:
                    return "Outlook Filters";

                case SavedQueryQueryType.AddressBookFilters:
                    return "Address Book Filters";

                case SavedQueryQueryType.MainApplicationViewWithoutSubject:
                    return "Main Application View Without Subject";

                case SavedQueryQueryType.SavedQueryTypeOther:
                    return "Saved Query Type Other";

                case SavedQueryQueryType.InteractiveWorkflowView:
                    return "Interactive Workflow View";

                case SavedQueryQueryType.OfflineTemplate:
                    return "Offline Template";

                case SavedQueryQueryType.CustomDefinedView:
                    return "Custom Defined View";

                case SavedQueryQueryType.ExportFieldTranslationsView:
                    return "Export Field Translations View";

                case SavedQueryQueryType.OutlookTemplate:
                    return "Outlook Template";
            }

            //UserQueryQueryType

            return queryType.ToString();
        }

        public Task<SavedQuery> GetByIdAsync(Guid idSavedQuery, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSavedQuery, columnSet));
        }

        private SavedQuery GetById(Guid idSavedQuery, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SavedQuery.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQuery.PrimaryIdAttribute, ConditionOperator.Equal, idSavedQuery),
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SavedQuery>()).FirstOrDefault();
        }
    }
}
