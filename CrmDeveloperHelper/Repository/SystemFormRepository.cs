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
    public class SystemFormRepository
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private IOrganizationServiceExtented _Service { get; set; }

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SystemFormRepository(IOrganizationServiceExtented service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SystemForm>> GetListAsync(string filterEntity = null, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetList(filterEntity, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SystemForm> GetList(string filterEntity, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SystemForm.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemForm.Schema.Attributes.componentstate, ConditionOperator.In, 0, 1),
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemForm.Schema.Attributes.objecttypecode, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.type, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SystemForm.Schema.Attributes.objecttypecode, ConditionOperator.Equal, filterEntity));
            }

            var result = new List<SystemForm>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SystemForm>()));

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

            result = result.Where(ent =>
                !ent.Contains(SystemForm.Schema.Attributes.formactivationstate) || (ent.Contains(SystemForm.Schema.Attributes.formactivationstate) && ent.GetAttributeValue<OptionSetValue>(SystemForm.Schema.Attributes.formactivationstate).Value == 1)
                ).ToList();

            return result;
        }

        public Task<SystemForm> GetByIdAsync(Guid idSystemForm, ColumnSet columnSet)
        {
            return Task.Run(() => GetById(idSystemForm, columnSet));
        }

        private SystemForm GetById(Guid idSystemForm, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = SystemForm.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemForm.PrimaryIdAttribute, ConditionOperator.Equal, idSystemForm),
                    },
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<SystemForm>()).SingleOrDefault();
        }

        public Task<List<SystemForm>> GetListByTypeAsync(int formType, ColumnSet columnSet )
        {
            return Task.Run(() => GetListByType(formType, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SystemForm> GetListByType(int formType, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = SystemForm.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemForm.Schema.Attributes.componentstate, ConditionOperator.In, 0, 1),
                        new ConditionExpression(SystemForm.Schema.Attributes.type, ConditionOperator.Equal, formType),
                        new ConditionExpression(SystemForm.Schema.Attributes.formactivationstate, ConditionOperator.Equal, (int)SystemForm.Schema.OptionSets.formactivationstate.Active_1),

                        
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemForm.Schema.Attributes.objecttypecode, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.type, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<SystemForm>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<SystemForm>()));

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

        public static string GetFormTypeString(int? formtype)
        {
            if (!formtype.HasValue)
            {
                return string.Empty;
            }

            switch (formtype.Value)
            {
                case (int)SystemForm.Schema.OptionSets.type.AppointmentBook_1:
                    return "appointmentBook";

                case (int)SystemForm.Schema.OptionSets.type.Main_2:
                    return "main";
                
                case (int)SystemForm.Schema.OptionSets.type.Quick_View_Form_6:
                    return "quick";

                case (int)SystemForm.Schema.OptionSets.type.Quick_Create_7:
                    return "quickCreate";

                case (int)SystemForm.Schema.OptionSets.type.InteractionCentricDashboard_10:
                    return "icdashboardeditor";

                case (int)SystemForm.Schema.OptionSets.type.Card_11:
                    return "card";

                case (int)SystemForm.Schema.OptionSets.type.Main_Interactive_experience_12:
                    return "mainInteractionCentric";

                case (int)SystemForm.Schema.OptionSets.type.Dashboard_0:
                case (int)SystemForm.Schema.OptionSets.type.MiniCampaignBO_3:
                case (int)SystemForm.Schema.OptionSets.type.Preview_4:
                case (int)SystemForm.Schema.OptionSets.type.Mobile_Express_5:
                case (int)SystemForm.Schema.OptionSets.type.Dialog_8:
                case (int)SystemForm.Schema.OptionSets.type.Task_Flow_Form_9:

                case (int)SystemForm.Schema.OptionSets.type.Other_100:
                case (int)SystemForm.Schema.OptionSets.type.MainBackup_101:
                case (int)SystemForm.Schema.OptionSets.type.AppointmentBookBackup_102:
                case (int)SystemForm.Schema.OptionSets.type.Power_BI_Dashboard_103:
                    break;
            }

            return string.Empty;
        }

        public static Task<bool> ValidateXmlDocumentAsync(IWriteToOutput iWriteToOutput, XDocument doc)
        {
            return Task.Run(() => ValidateXmlDocument(iWriteToOutput, doc));
        }

        private static bool ValidateXmlDocument(IWriteToOutput iWriteToOutput, XDocument doc)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            {
                var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaFormXml);

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
                iWriteToOutput.WriteToOutput(Properties.OutputStrings.TextIsNotValidForFieldFormat1, "FormXml");

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
