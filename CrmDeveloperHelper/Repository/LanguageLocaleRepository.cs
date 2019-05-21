﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class LanguageLocaleRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public LanguageLocaleRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<LanguageLocale>> GetListAsync(params int[] types)
        {
            return Task.Run(() => GetList(types));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<LanguageLocale> GetList(params int[] types)
        {
            if (types == null)
            {
                return new List<LanguageLocale>();
            }

            if (types.Length == 0)
            {
                return new List<LanguageLocale>();
            }

            QueryExpression query = new QueryExpression()
            {
                EntityName = LanguageLocale.EntityLogicalName,

                NoLock = true,

                ColumnSet = new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(LanguageLocale.Schema.Attributes.localeid, ConditionOperator.In, types),
                    },
                },
            };

            return _service.RetrieveMultipleAll<LanguageLocale>(query);
        }
    }
}
