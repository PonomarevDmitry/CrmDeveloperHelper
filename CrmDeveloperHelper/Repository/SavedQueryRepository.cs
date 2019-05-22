﻿using Microsoft.Crm.Sdk;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

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
                        new ConditionExpression(SavedQuery.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQuery.Schema.Attributes.returnedtypecode, OrderType.Ascending),
                    new OrderExpression(SavedQuery.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SavedQuery.Schema.Attributes.returnedtypecode, ConditionOperator.Equal, filterEntity));
            }

            return _service.RetrieveMultipleAll<SavedQuery>(query);
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

        public SavedQuery GetById(Guid idSavedQuery, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SavedQuery.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SavedQuery.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSavedQuery),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SavedQuery>()).SingleOrDefault() : null;
        }

        public static string GetFieldNameByXmlRoot(string rootNodeName)
        {
            if (string.Equals(rootNodeName, "fetch", StringComparison.InvariantCultureIgnoreCase))
            {
                return SavedQuery.Schema.Attributes.fetchxml;
            }
            else if (string.Equals(rootNodeName, "grid", StringComparison.InvariantCultureIgnoreCase))
            {
                return SavedQuery.Schema.Attributes.layoutxml;
            }
            else if (string.Equals(rootNodeName, "columnset", StringComparison.InvariantCultureIgnoreCase))
            {
                return SavedQuery.Schema.Attributes.columnsetxml;
            }

            throw new ArgumentException(string.Format("Unknown xml root node name - {0}", rootNodeName));
        }

        public static string GetFieldTitleByXmlRoot(string rootNodeName)
        {
            if (string.Equals(rootNodeName, "fetch", StringComparison.InvariantCultureIgnoreCase))
            {
                return "FetchXml";
            }
            else if (string.Equals(rootNodeName, "grid", StringComparison.InvariantCultureIgnoreCase))
            {
                return "LayoutXml";
            }
            else if (string.Equals(rootNodeName, "columnset", StringComparison.InvariantCultureIgnoreCase))
            {
                return "ColumnSetXml";
            }

            throw new ArgumentException(string.Format("Unknown xml root node name - {0}", rootNodeName));
        }

        public static Task<bool> ValidateXmlDocumentAsync(ConnectionData connectionData, IWriteToOutput iWriteToOutput, XDocument doc, string fieldTitle)
        {
            return Task.Run(() => ValidateXmlDocument(connectionData, iWriteToOutput, doc, fieldTitle));
        }

        private static bool ValidateXmlDocument(ConnectionData connectionData, IWriteToOutput iWriteToOutput, XDocument doc, string fieldTitle)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            {
                var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaFetch);

                if (schemasResources != null)
                {
                    foreach (var fileName in schemasResources)
                    {
                        Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        using (StreamReader reader = new StreamReader(info.Stream))
                        {
                            schemas.Add("", XmlReader.Create(reader));
                        }
                    }
                }
            }

            List<ValidationEventArgs> errors = new List<ValidationEventArgs>();

            doc.Validate(schemas, (o, e) =>
            {
                errors.Add(e);
            });

            if (errors.Count > 0)
            {
                iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.TextIsNotValidForFieldFormat1, fieldTitle);

                foreach (var item in errors)
                {
                    iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    iWriteToOutput.WriteToOutput(connectionData, string.Empty);
                    iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.XmlValidationMessageFormat2, item.Severity, item.Message);
                    iWriteToOutput.WriteErrorToOutput(connectionData, item.Exception);
                }

                iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return errors.Count == 0;
        }

        public Task<List<SavedQuery>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<SavedQuery> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
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
                        new ConditionExpression(SavedQuery.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQuery.Schema.Attributes.returnedtypecode, OrderType.Ascending),
                    new OrderExpression(SavedQuery.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<SavedQuery>(query);
        }

        public Task<List<SavedQuery>> GetListForEntitiesAsync(string[] entities, ColumnSet columnSet)
        {
            return Task.Run(() => GetListForEntities(entities, columnSet));
        }

        private List<SavedQuery> GetListForEntities(string[] entities, ColumnSet columnSet)
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
                        new ConditionExpression(SavedQuery.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                        new ConditionExpression(SavedQuery.Schema.Attributes.returnedtypecode, ConditionOperator.In, entities),
                    },
                },

                Orders =
                {
                    new OrderExpression(SavedQuery.Schema.Attributes.returnedtypecode, OrderType.Ascending),
                    new OrderExpression(SavedQuery.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<SavedQuery>(query);
        }
    }
}