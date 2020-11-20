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
    public class SiteMapRepository : IEntitySaver
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SiteMapRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
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

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SiteMap.EntityPrimaryIdAttribute, ConditionOperator.NotNull),
                    },
                },
            };

            return _service.RetrieveMultipleAll<SiteMap>(query);
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
                        new ConditionExpression(SiteMap.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSiteMap),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SiteMap>()).SingleOrDefault() : null;
        }

        public Task<SiteMap> FindByExactNameAsync(string sitemapName, ColumnSet columnSet)
        {
            return Task.Run(() => FindByExactName(sitemapName, columnSet));
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

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SiteMap>()).SingleOrDefault() : null;
        }

        public static Task<bool> ValidateXmlDocumentAsync(ConnectionData connectionData, IWriteToOutput iWriteToOutput, XDocument doc)
        {
            return Task.Run(() => ValidateXmlDocument(connectionData, iWriteToOutput, doc));
        }

        private static bool ValidateXmlDocument(ConnectionData connectionData, IWriteToOutput iWriteToOutput, XDocument doc)
        {
            ContentComparerHelper.ClearRoot(doc);

            XmlSchemaSet schemas = new XmlSchemaSet();

            {
                var schemasResources = AbstractDynamicCommandXsdSchemas.GetXsdSchemas(AbstractDynamicCommandXsdSchemas.SiteMapXmlSchema);

                if (schemasResources != null)
                {
                    foreach (var fileName in schemasResources)
                    {
                        Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        using (StreamReader reader = new StreamReader(info.Stream))
                        {
                            schemas.Add(string.Empty, XmlReader.Create(reader));
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
                iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.TextIsNotValidForFieldFormat1, AbstractDynamicCommandXsdSchemas.SiteMapXmlSchema);

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

        public Task<List<SiteMap>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<SiteMap> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SiteMap.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SiteMap.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(SiteMap.Schema.Attributes.sitemapnameunique, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<SiteMap>(query);
        }

        public async Task<Guid> UpsertAsync(Entity entity, Action<string> updateStatus)
        {
            var idEntity = await _service.UpsertAsync(entity);

            var siteMap = await GetByIdAsync(idEntity, new ColumnSet(SiteMap.Schema.Attributes.sitemapname));

            var repositoryPublish = new PublishActionsRepository(_service);

            updateStatus(string.Format(Properties.OutputStrings.InConnectionPublishingSiteMapFormat3, _service.ConnectionData.Name, siteMap.SiteMapName, idEntity.ToString()));

            await repositoryPublish.PublishSiteMapsAsync(new[] { idEntity });

            updateStatus(string.Format(Properties.OutputStrings.InConnectionPublishingSiteMapCompletedFormat3, _service.ConnectionData.Name, siteMap.SiteMapName, idEntity.ToString()));

            return idEntity;
        }
    }
}
