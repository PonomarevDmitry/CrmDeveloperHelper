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
    public class SystemFormRepository : IEntitySaver
    {
        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _service;

        /// <summary>
        /// Конструктор репозитория
        /// </summary>
        /// <param name="service"></param>
        public SystemFormRepository(IOrganizationServiceExtented service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<List<SystemForm>> GetListAsync(string filterEntity = null, SystemForm.Schema.OptionSets.formactivationstate? state = null, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetList(filterEntity, state, columnSet));
        }

        /// <summary>
        /// Получить все представления
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<SystemForm> GetList(string filterEntity, SystemForm.Schema.OptionSets.formactivationstate? state, ColumnSet columnSet)
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
                        new ConditionExpression(SystemForm.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemForm.Schema.Attributes.objecttypecode, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.type, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (!string.IsNullOrEmpty(filterEntity))
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SystemForm.Schema.Attributes.objecttypecode, ConditionOperator.Equal, filterEntity));
            }

            if (state.HasValue)
            {
                query.Criteria.Conditions.Add(new ConditionExpression(SystemForm.Schema.Attributes.formactivationstate, ConditionOperator.Equal, (int)state.Value));
            }

            return _service.RetrieveMultipleAll<SystemForm>(query);
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
                        new ConditionExpression(SystemForm.EntityPrimaryIdAttribute, ConditionOperator.Equal, idSystemForm),
                    },
                },
            };

            var coll = _service.RetrieveMultiple(query).Entities;

            return coll.Count == 1 ? coll.Select(e => e.ToEntity<SystemForm>()).SingleOrDefault() : null;
        }

        public Task<List<SystemForm>> GetListByTypeAsync(int formType, ColumnSet columnSet)
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
                        new ConditionExpression(SystemForm.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
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
            };

            return _service.RetrieveMultipleAll<SystemForm>(query);
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

        public static void GetTypeName(SystemForm.Schema.OptionSets.type? formType, out string formTypeName, out string formTypeConstructorName)
        {
            formTypeName = "unknown";
            formTypeConstructorName = "Unknown";

            if (!formType.HasValue)
            {
                return;
            }

            switch (formType.Value)
            {
                case SystemForm.Schema.OptionSets.type.Dashboard_0:
                    formTypeName = "dashboard";
                    formTypeConstructorName = "Dashboard";
                    break;

                case SystemForm.Schema.OptionSets.type.AppointmentBook_1:
                    formTypeName = "appointment_book";
                    formTypeConstructorName = "AppointmentBook";
                    break;

                case SystemForm.Schema.OptionSets.type.Main_2:
                    formTypeName = "main";
                    formTypeConstructorName = "Main";
                    break;

                case SystemForm.Schema.OptionSets.type.MiniCampaignBO_3:
                    formTypeName = "mini_campaign_bo";
                    formTypeConstructorName = "MiniCampaignBO";
                    break;

                case SystemForm.Schema.OptionSets.type.Preview_4:
                    formTypeName = "preview";
                    formTypeConstructorName = "Preview";
                    break;

                case SystemForm.Schema.OptionSets.type.Mobile_Express_5:
                    formTypeName = "mobile_express";
                    formTypeConstructorName = "MobileExpress";
                    break;

                case SystemForm.Schema.OptionSets.type.Quick_View_Form_6:
                    formTypeName = "quick_view";
                    formTypeConstructorName = "QuickView";
                    break;

                case SystemForm.Schema.OptionSets.type.Quick_Create_7:
                    formTypeName = "quick";
                    formTypeConstructorName = "Quick";
                    break;

                case SystemForm.Schema.OptionSets.type.Dialog_8:
                    formTypeName = "dialog";
                    formTypeConstructorName = "Dialog";
                    break;

                case SystemForm.Schema.OptionSets.type.Task_Flow_Form_9:
                    formTypeName = "task_flow";
                    formTypeConstructorName = "TaskFlow";
                    break;

                case SystemForm.Schema.OptionSets.type.InteractionCentricDashboard_10:
                    formTypeName = "interaction_centric_dashboard";
                    formTypeConstructorName = "InteractionCentricDashboard";
                    break;

                case SystemForm.Schema.OptionSets.type.Card_11:
                    formTypeName = "card";
                    formTypeConstructorName = "Card";
                    break;

                case SystemForm.Schema.OptionSets.type.Main_Interactive_experience_12:
                    formTypeName = "main_interactive_experience";
                    formTypeConstructorName = "MainInteractiveExperience";
                    break;

                case SystemForm.Schema.OptionSets.type.Other_100:
                    formTypeName = "other";
                    formTypeConstructorName = "Other";
                    break;

                case SystemForm.Schema.OptionSets.type.MainBackup_101:
                    formTypeName = "main_backup";
                    formTypeConstructorName = "MainBackup";
                    break;

                case SystemForm.Schema.OptionSets.type.AppointmentBookBackup_102:
                    formTypeName = "appointment_book_backup";
                    formTypeConstructorName = "AppointmentBookBackup";
                    break;

                case SystemForm.Schema.OptionSets.type.Power_BI_Dashboard_103:
                    formTypeName = "power_bi_dashboard";
                    formTypeConstructorName = "PowerBIDashboard";
                    break;
            }
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
                var schemasResources = AbstractDynamicCommandXsdSchemas.GetXsdSchemas(AbstractDynamicCommandXsdSchemas.FormXmlSchema);

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
                iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.TextIsNotValidForFieldFormat1, AbstractDynamicCommandXsdSchemas.FormXmlSchema);

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

        public Task<List<SystemForm>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<SystemForm> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
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
                        new ConditionExpression(SystemForm.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemForm.Schema.Attributes.objecttypecode, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.type, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<SystemForm>(query);
        }

        public Task<List<SystemForm>> GetListForEntitiesAsync(string[] entities, ColumnSet columnSet)
        {
            return Task.Run(() => GetListForEntities(entities, columnSet));
        }

        private List<SystemForm> GetListForEntities(string[] entities, ColumnSet columnSet)
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
                        new ConditionExpression(SystemForm.Schema.Attributes.componentstate, ConditionOperator.In
                            , (int)Entities.GlobalOptionSets.componentstate.Published_0
                            , (int)Entities.GlobalOptionSets.componentstate.Unpublished_1
                        ),
                        new ConditionExpression(SystemForm.Schema.Attributes.objecttypecode, ConditionOperator.In, entities),
                    },
                },

                Orders =
                {
                    new OrderExpression(SystemForm.Schema.Attributes.objecttypecode, OrderType.Ascending),
                    new OrderExpression(SystemForm.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            return _service.RetrieveMultipleAll<SystemForm>(query);
        }

        public async Task<Guid> UpsertAsync(Entity entity, Action<string> updateStatus)
        {
            var idEntity = await _service.UpsertAsync(entity);

            var systemForm = await GetByIdAsync(idEntity, new ColumnSet(SystemForm.Schema.Attributes.objecttypecode, SystemForm.Schema.Attributes.name));

            var repositoryPublish = new PublishActionsRepository(_service);

            updateStatus(string.Format(Properties.OutputStrings.PublishingSystemFormFormat3, _service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name));

            await repositoryPublish.PublishDashboardsAsync(new[] { idEntity });

            updateStatus(string.Format(Properties.OutputStrings.PublishingSystemFormCompletedFormat3, _service.ConnectionData.Name, systemForm.ObjectTypeCode, systemForm.Name));

            if (systemForm.ObjectTypeCode.IsValidEntityName())
            {
                updateStatus(string.Format(Properties.OutputStrings.PublishingEntitiesFormat2, _service.ConnectionData.Name, systemForm.ObjectTypeCode));

                await repositoryPublish.PublishEntitiesAsync(new[] { systemForm.ObjectTypeCode });

                updateStatus(string.Format(Properties.OutputStrings.PublishingEntitiesCompletedFormat2, _service.ConnectionData.Name, systemForm.ObjectTypeCode));
            }

            return idEntity;
        }
    }
}
