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
                NoLock = true,

                EntityName = SdkMessageProcessingStepSecureConfig.EntityLogicalName,

                ColumnSet = new ColumnSet(true),
            };

            return _service.RetrieveMultipleAll<SdkMessageProcessingStepSecureConfig>(query);
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
                        new ConditionExpression(SdkMessageProcessingStepSecureConfig.EntityPrimaryIdAttribute, ConditionOperator.Equal, id),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SdkMessageProcessingStepSecureConfig>()).SingleOrDefault() : null;
        }
    }
}
