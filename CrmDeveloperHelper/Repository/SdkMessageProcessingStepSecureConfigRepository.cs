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
    public class SdkMessageProcessingStepSecureConfigRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SdkMessageProcessingStepSecureConfigRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SdkMessageProcessingStepSecureConfig>> GetAllSdkMessageProcessingStepSecureConfigAsync()
        {
            return Task.Run(() => GetAllSdkMessageProcessingStepSecureConfig());
        }

        private List<SdkMessageProcessingStepSecureConfig> GetAllSdkMessageProcessingStepSecureConfig()
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SdkMessageProcessingStepSecureConfig.EntityLogicalName,
                ColumnSet = new ColumnSet(true),

                NoLock = true,

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SdkMessageProcessingStepSecureConfig>();

            try
            {
                while (true)
                {
                    var coll = _service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SdkMessageProcessingStepSecureConfig>()));

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

        public Task<SdkMessageProcessingStepSecureConfig> GetSecureByIdAsync(Guid id)
        {
            return Task.Run(() => GetSecureById(id));
        }

        private SdkMessageProcessingStepSecureConfig GetSecureById(Guid id)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SdkMessageProcessingStepSecureConfig.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStepSecureConfig.PrimaryIdAttribute, ConditionOperator.Equal, id),
                    },
                },
            };

            return _service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SdkMessageProcessingStepSecureConfig>()).SingleOrDefault();
        }

        //internal SdkMessageProcessingStepSecureConfig GetSecureConfig(Guid id)
        //{
        //    return _service.Retrieve(SdkMessageProcessingStepSecureConfig.EntityLogicalName, id, new ColumnSet(true)).ToEntity<SdkMessageProcessingStepSecureConfig>();
        //}
    }
}
