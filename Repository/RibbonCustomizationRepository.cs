using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class RibbonCustomizationRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public RibbonCustomizationRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<RibbonCustomization>> GetListAsync()
        {
            return Task.Run(() => GetList());
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<RibbonCustomization> GetList()
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = RibbonCustomization.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RibbonCustomization.Schema.Attributes.entity, ConditionOperator.NotNull),
                    },
                },

                Orders =
                {
                    new OrderExpression(RibbonCustomization.Schema.Attributes.entity, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<RibbonCustomization>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<RibbonCustomization>()));

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

        public Task ExportApplicationRibbon(RibbonLocationFilters filter, string filePath)
        {
            return Task.Run(() => ExportingApplicationRibbon(filter, filePath));
        }

        private void ExportingApplicationRibbon(RibbonLocationFilters filter, string filePath)
        {
            RetrieveApplicationRibbonRequest appribReq = new RetrieveApplicationRibbonRequest();
            RetrieveApplicationRibbonResponse appribResp = (RetrieveApplicationRibbonResponse)_service.Execute(appribReq);

            var array = FileOperations.UnzipRibbon(appribResp.CompressedApplicationRibbonXml);

            File.WriteAllBytes(filePath, array);
        }

        public Task ExportEntityRibbon(string entityName, RibbonLocationFilters filter, string filePath, CommonConfiguration commonConfig)
        {
            return Task.Run(() => ExportingEntityRibbon(entityName, filter, filePath, commonConfig));
        }

        private void ExportingEntityRibbon(string entityName, RibbonLocationFilters filter, string filePath, CommonConfiguration commonConfig)
        {
            RetrieveEntityRibbonRequest entRibReq = new RetrieveEntityRibbonRequest()
            {
                RibbonLocationFilter = filter,
                EntityName = entityName,
            };

            RetrieveEntityRibbonResponse entRibResp = (RetrieveEntityRibbonResponse)_service.Execute(entRibReq);

            var byteXml = FileOperations.UnzipRibbon(entRibResp.CompressedEntityXml);

            if (commonConfig != null && commonConfig.ExportRibbonXmlXmlAttributeOnNewLine)
            {
                XElement doc = null;

                using (MemoryStream memStream = new MemoryStream())
                {
                    memStream.Write(byteXml, 0, byteXml.Length);

                    memStream.Position = 0;

                    doc = XElement.Load(memStream);
                }

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.Encoding = Encoding.UTF8;

                using (MemoryStream memStream = new MemoryStream())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(memStream, settings))
                    {
                        doc.Save(xmlWriter);
                    }

                    memStream.Position = 0;

                    byteXml = memStream.ToArray();
                }
            }

            File.WriteAllBytes(filePath, byteXml);
        }
    }
}
