using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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
    public class SitemapRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _Service { get; set; }

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SitemapRepository(IOrganizationServiceExtented service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SiteMap>> GetListAsync(ColumnSet columnSet = null)
        {
            return Task.Run(() => GetList(columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        private List<SiteMap> GetList(ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,
                EntityName = SiteMap.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            List<SiteMap> result = new List<SiteMap>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SiteMap>()));

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
                Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<SiteMap> GetByIdAsync(Guid idSiteMap, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSiteMap, columnSet));
        }

        private SiteMap GetById(Guid idSiteMap, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SiteMap.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SiteMap.PrimaryIdAttribute, ConditionOperator.Equal, idSiteMap),
                    },
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SiteMap>()).SingleOrDefault();
        }

        public SiteMap FindByExactName(string sitemapName, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SiteMap.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),
            };

            if (!string.IsNullOrEmpty(sitemapName))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SiteMap.Schema.Attributes.sitemapnameunique, ConditionOperator.Equal, sitemapName));
            }
            else
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SiteMap.Schema.Attributes.sitemapnameunique, ConditionOperator.Null));
            }

            var coll = _Service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SiteMap>()).SingleOrDefault() : null;
        }

        public static Task<bool> ValidateXmlDocumentAsync(IWriteToOutput iWriteToOutput, XDocument doc)
        {
            return Task.Run(() => ValidateXmlDocument(iWriteToOutput, doc));
        }

        private static bool ValidateXmlDocument(IWriteToOutput iWriteToOutput, XDocument doc)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            {
                var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaSiteMapXml);

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
                iWriteToOutput.WriteToOutput(Properties.OutputStrings.TextIsNotValidForFieldFormat1, "SiteMapXml");

                foreach (var item in errors)
                {
                    iWriteToOutput.WriteToOutput(string.Empty);
                    iWriteToOutput.WriteToOutput(string.Empty);
                    iWriteToOutput.WriteToOutput(Properties.OutputStrings.XmlValidationMessageFormat2, item.Severity, item.Message);
                    iWriteToOutput.WriteErrorToOutput(item.Exception);
                }

                iWriteToOutput.ActivateOutputWindow();
            }

            return errors.Count == 0;
        }
    }
}
