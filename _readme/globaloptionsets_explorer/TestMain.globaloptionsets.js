if (typeof (GlobalOptionSets) == 'undefined') {
    GlobalOptionSets = { __namespace: true };
}

GlobalOptionSets.activity_mailmergetypecodeEnum = {

    'Appointment_4201': { 'value': 4201, 'text': 'Appointment', 'name1033': 'Appointment', 'name1049': 'Встреча' },
    'Email_4202': { 'value': 4202, 'text': 'Email', 'name1033': 'Email', 'name1049': 'Электронная почта' },
    'Fax_4204': { 'value': 4204, 'text': 'Fax', 'name1033': 'Fax', 'name1049': 'Факс' },
    'Letter_4207': { 'value': 4207, 'text': 'Letter', 'name1033': 'Letter', 'name1049': 'Письмо' },
    'Phone_Call_4210': { 'value': 4210, 'text': 'Phone Call', 'name1033': 'Phone Call', 'name1049': 'Звонок' },
    'Email_via_Mail_Merge_42020': { 'value': 42020, 'text': 'Email via Mail Merge', 'name1033': 'Email via Mail Merge', 'name1049': 'Электронная почта через слияние почты' },
    'Fax_via_Mail_Merge_42040': { 'value': 42040, 'text': 'Fax via Mail Merge', 'name1033': 'Fax via Mail Merge', 'name1049': 'Факс через слияние' },
    'Letter_via_Mail_Merge_42070': { 'value': 42070, 'text': 'Letter via Mail Merge', 'name1033': 'Letter via Mail Merge', 'name1049': 'Письмо через слияние' },
};

GlobalOptionSets.activity_typecodeEnum = {

    'Appointment_4201': { 'value': 4201, 'text': 'Appointment', 'name1033': 'Appointment', 'name1049': 'Встреча' },
    'Email_4202': { 'value': 4202, 'text': 'Email', 'name1033': 'Email', 'name1049': 'Электронная почта' },
    'Fax_4204': { 'value': 4204, 'text': 'Fax', 'name1033': 'Fax', 'name1049': 'Факс' },
    'Letter_4207': { 'value': 4207, 'text': 'Letter', 'name1033': 'Letter', 'name1049': 'Письмо' },
    'Phone_Call_4210': { 'value': 4210, 'text': 'Phone Call', 'name1033': 'Phone Call', 'name1049': 'Звонок' },
};

// ComponentType:   Attribute (2)            Count: 18
//     AttributeName                                  DisplayName      IsCustomizable    Behavior
//     activitypointer.activitytypecode               Activity Type    False             IncludeSubcomponents
//     appointment.activitytypecode                   Activity Type    True              IncludeSubcomponents
//     bulkoperation.activitytypecode                 Activity Type    True              IncludeSubcomponents
//     campaignactivity.activitytypecode              Activity Type    True              IncludeSubcomponents
//     campaignresponse.activitytypecode              Activity Type    True              IncludeSubcomponents
//     email.activitytypecode                         Activity Type    True              IncludeSubcomponents
//     fax.activitytypecode                           Activity Type    True              IncludeSubcomponents
//     incidentresolution.activitytypecode            Activity Type    True              IncludeSubcomponents
//     letter.activitytypecode                        Activity Type    True              IncludeSubcomponents
//     opportunityclose.activitytypecode              Activity Type    True              IncludeSubcomponents
//     orderclose.activitytypecode                    Activity Type    True              IncludeSubcomponents
//     phonecall.activitytypecode                     Activity Type    True              IncludeSubcomponents
//     quoteclose.activitytypecode                    Activity Type    True              IncludeSubcomponents
//     recurringappointmentmaster.activitytypecode    Activity Type    True              IncludeSubcomponents
//     serviceappointment.activitytypecode            Activity Type    True              IncludeSubcomponents
//     socialactivity.activitytypecode                Activity Type    True              IncludeSubcomponents
//     task.activitytypecode                          Activity Type    True              IncludeSubcomponents
//     untrackedemail.activitytypecode                Activity Type    True              IncludeSubcomponents
// GlobalOptionSets.activitypointer_activitytypecodeEnum

// ComponentType:   Attribute (2)            Count: 10
//     AttributeName                              DisplayName          IsCustomizable    Behavior
//     activitypointer.deliveryprioritycode       Delivery Priority    False             IncludeSubcomponents
//     bulkoperation.deliveryprioritycode         Delivery Priority    True              IncludeSubcomponents
//     campaignactivity.deliveryprioritycode      Delivery Priority    True              IncludeSubcomponents
//     campaignresponse.deliveryprioritycode      Delivery Priority    True              IncludeSubcomponents
//     email.deliveryprioritycode                 Delivery Priority    True              IncludeSubcomponents
//     incidentresolution.deliveryprioritycode    Delivery Priority    True              IncludeSubcomponents
//     opportunityclose.deliveryprioritycode      Delivery Priority    True              IncludeSubcomponents
//     orderclose.deliveryprioritycode            Delivery Priority    True              IncludeSubcomponents
//     quoteclose.deliveryprioritycode            Delivery Priority    True              IncludeSubcomponents
//     serviceappointment.deliveryprioritycode    Delivery Priority    True              IncludeSubcomponents
GlobalOptionSets.activitypointer_deliveryprioritycodeEnum = {

    'Low_0': { 'value': 0, 'text': 'Low', 'displayOrder': 1, 'name1033': 'Low', 'name1049': 'Низкий' },
    'Normal_1': { 'value': 1, 'text': 'Normal', 'displayOrder': 2, 'name1033': 'Normal', 'name1049': 'Обычный' },
    'High_2': { 'value': 2, 'text': 'High', 'displayOrder': 3, 'name1033': 'High', 'name1049': 'Высокий' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName            IsCustomizable    Behavior
//     none          Dialog      Confirm Addition    False             IncludeSubcomponents
GlobalOptionSets.addlistcampaignEnum = {

    'To_the_campaign_only_0': { 'value': 0, 'text': 'To the campaign only.', 'name1033': 'To the campaign only.', 'name1049': 'Только в кампанию.' },
    'To_the_campaign_and_all_undistributed_campaign_activities_1': { 'value': 1, 'text': 'To the campaign and all undistributed campaign activities.', 'name1033': 'To the campaign and all undistributed campaign activities.', 'name1049': 'В кампанию и все нераспределенные действия кампании.' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName                        DisplayName            IsCustomizable    Behavior
//     characteristic.characteristictype    Characteristic Type    True              IncludeSubcomponents
GlobalOptionSets.bookableresourcecharacteristictypeEnum = {

    'Skill_1': { 'value': 1, 'text': 'Skill', 'displayOrder': 1, 'name1033': 'Skill', 'name1049': 'Навык' },
    'Certification_2': { 'value': 2, 'text': 'Certification', 'displayOrder': 2, 'name1033': 'Certification', 'name1049': 'Сертификация' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName               DisplayName    IsCustomizable    Behavior
//     lead.budgetstatus           Budget         True              IncludeSubcomponents
//     opportunity.budgetstatus    Budget         True              IncludeSubcomponents
GlobalOptionSets.budgetstatusEnum = {

    'No_Committed_Budget_0': { 'value': 0, 'text': 'No Committed Budget', 'displayOrder': 1, 'name1033': 'No Committed Budget', 'name1049': 'Нет выделенного бюджета' },
    'May_Buy_1': { 'value': 1, 'text': 'May Buy', 'displayOrder': 2, 'name1033': 'May Buy', 'name1049': 'Могут купить' },
    'Can_Buy_2': { 'value': 2, 'text': 'Can Buy', 'displayOrder': 3, 'name1033': 'Can Buy', 'name1049': 'Могут купить' },
    'Will_Buy_3': { 'value': 3, 'text': 'Will Buy', 'displayOrder': 4, 'name1033': 'Will Buy', 'name1049': 'Купят' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName            IsCustomizable    Behavior
//     none          Dialog      BulkEmail Dialog    False             IncludeSubcomponents
GlobalOptionSets.bulkemail_recipientsEnum = {

    'Selected_records_on_current_page_1': { 'value': 1, 'text': 'Selected records on current page', 'name1033': 'Selected records on current page' },
    'All_records_on_current_page_2': { 'value': 2, 'text': 'All records on current page', 'name1033': 'All records on current page' },
    'All_records_on_all_pages_3': { 'value': 3, 'text': 'All records on all pages', 'name1033': 'All records on all pages' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                    IsCustomizable    Behavior
//     none          Dialog      Dialog for case settings    True              IncludeSubcomponents
GlobalOptionSets.cascadecaseclosurepreferenceEnum = {

    'Close_all_child_cases_when_parent_case_is_closed_0': { 'value': 0, 'text': 'Close all child cases when parent case is closed', 'name1033': 'Close all child cases when parent case is closed', 'name1049': 'Закрывать все дочерние обращения при закрытии родительского' },
    'Don_t_allow_parent_case_closure_until_all_child_cases_are_closed_1': { 'value': 1, 'text': 'Don\'t allow parent case closure until all child cases are closed', 'name1033': 'Don\'t allow parent case closure until all child cases are closed', 'name1049': 'Запретить закрытие родительского обращения, пока не будут закрыты все дочерние' },
};

// ComponentType:   Attribute (2)            Count: 95
//     Default Description for Unknowned Solution Components:
//     0513aa19-b341-427e-b229-a681d79d6a1a    IncludeSubcomponents
//     9c64a506-22b6-484d-99d8-637001b3be7b    IncludeSubcomponents
//     ba7ad728-ef8c-45e7-a579-b51d9b2783d2    IncludeSubcomponents
//     AttributeName                                           DisplayName        IsCustomizable    Behavior
//     activitymimeattachment.componentstate                   Component State    True              IncludeSubcomponents
//     advancedsimilarityrule.componentstate                   Component State    False             IncludeSubcomponents
//     appconfig.componentstate                                Component State    False             IncludeSubcomponents
//     appconfiginstance.componentstate                        Component State    False             IncludeSubcomponents
//     appmodule.componentstate                                Component State    False             IncludeSubcomponents
//     appmoduleroles.componentstate                           Component State    False             IncludeSubcomponents
//     attributemap.componentstate                             Component State    False             IncludeSubcomponents
//     channelaccessprofile.componentstate                     Component State    False             IncludeSubcomponents
//     channelaccessprofileentityaccesslevel.componentstate    Component State    True              IncludeSubcomponents
//     channelaccessprofilerule.componentstate                 Component State    False             IncludeSubcomponents
//     channelaccessprofileruleitem.componentstate             Component State    False             IncludeSubcomponents
//     channelproperty.componentstate                          Component State    False             IncludeSubcomponents
//     channelpropertygroup.componentstate                     Component State    False             IncludeSubcomponents
//     columnmapping.componentstate                            Component State    False             IncludeSubcomponents
//     complexcontrol.componentstate                           Component State    False             IncludeSubcomponents
//     connectionrole.componentstate                           Component State    True              IncludeSubcomponents
//     contracttemplate.componentstate                         Component State    True              IncludeSubcomponents
//     convertrule.componentstate                              Component State    False             IncludeSubcomponents
//     convertruleitem.componentstate                          Component State    False             IncludeSubcomponents
//     customcontrol.componentstate                            Component State    False             IncludeSubcomponents
//     customcontroldefaultconfig.componentstate               Component State    False             IncludeSubcomponents
//     customcontrolresource.componentstate                    Component State    False             IncludeSubcomponents
//     dependencyfeature.componentstate                        Component State    True              IncludeSubcomponents
//     displaystring.componentstate                            Component State    False             IncludeSubcomponents
//     displaystringmap.componentstate                         Component State    False             IncludeSubcomponents
//     emailsignature.componentstate                           Component State    True              IncludeSubcomponents
//     entitydataprovider.componentstate                       Component State    False             IncludeSubcomponents
//     entitydatasource.componentstate                         Component State    False             IncludeSubcomponents
//     entitymap.componentstate                                Component State    False             IncludeSubcomponents
//     fieldpermission.componentstate                          Component State    False             IncludeSubcomponents
//     fieldsecurityprofile.componentstate                     Component State    False             IncludeSubcomponents
//     globalsearchconfiguration.componentstate                Component State    True              IncludeSubcomponents
//     hierarchyrule.componentstate                            Component State    False             IncludeSubcomponents
//     importentitymapping.componentstate                      Component State    False             IncludeSubcomponents
//     importmap.componentstate                                Component State    False             IncludeSubcomponents
//     kbarticletemplate.componentstate                        Component State    False             IncludeSubcomponents
//     knowledgesearchmodel.componentstate                     Component State    False             IncludeSubcomponents
//     lookupmapping.componentstate                            Component State    False             IncludeSubcomponents
//     mailmergetemplate.componentstate                        Component State    True              IncludeSubcomponents
//     mobileofflineprofile.componentstate                     Component State    False             IncludeSubcomponents
//     mobileofflineprofileitem.componentstate                 Component State    False             IncludeSubcomponents
//     mobileofflineprofileitemassociation.componentstate      Component State    False             IncludeSubcomponents
//     navigationsetting.componentstate                        Component State    False             IncludeSubcomponents
//     organizationui.componentstate                           Component State    False             IncludeSubcomponents
//     ownermapping.componentstate                             Component State    False             IncludeSubcomponents
//     picklistmapping.componentstate                          Component State    False             IncludeSubcomponents
//     pluginassembly.componentstate                           Component State    False             IncludeSubcomponents
//     plugintype.componentstate                               Component State    False             IncludeSubcomponents
//     privilege.componentstate                                Component State    False             IncludeSubcomponents
//     privilegeobjecttypecodes.componentstate                 Component State    False             IncludeSubcomponents
//     processtrigger.componentstate                           Component State    False             IncludeSubcomponents
//     report.componentstate                                   Component State    True              IncludeSubcomponents
//     reportcategory.componentstate                           Component State    True              IncludeSubcomponents
//     reportentity.componentstate                             Component State    False             IncludeSubcomponents
//     reportvisibility.componentstate                         Component State    False             IncludeSubcomponents
//     ribboncommand.componentstate                            Component State    False             IncludeSubcomponents
//     ribboncontextgroup.componentstate                       Component State    False             IncludeSubcomponents
//     ribboncustomization.componentstate                      Component State    False             IncludeSubcomponents
//     ribbondiff.componentstate                               Component State    False             IncludeSubcomponents
//     ribbonrule.componentstate                               Component State    False             IncludeSubcomponents
//     ribbontabtocommandmap.componentstate                    Component State    False             IncludeSubcomponents
//     role.componentstate                                     Component State    True              IncludeSubcomponents
//     roleprivileges.componentstate                           Component State    True              IncludeSubcomponents
//     routingrule.componentstate                              Component State    True              IncludeSubcomponents
//     routingruleitem.componentstate                          Component State    False             IncludeSubcomponents
//     savedquery.componentstate                               Component State    True              IncludeSubcomponents
//     savedqueryvisualization.componentstate                  Component State    True              IncludeSubcomponents
//     sdkmessage.componentstate                               Component State    False             IncludeSubcomponents
//     sdkmessagefilter.componentstate                         Component State    False             IncludeSubcomponents
//     sdkmessagepair.componentstate                           Component State    False             IncludeSubcomponents
//     sdkmessageprocessingstep.componentstate                 Component State    False             IncludeSubcomponents
//     sdkmessageprocessingstepimage.componentstate            Component State    False             IncludeSubcomponents
//     sdkmessagerequest.componentstate                        Component State    False             IncludeSubcomponents
//     sdkmessagerequestfield.componentstate                   Component State    False             IncludeSubcomponents
//     sdkmessageresponse.componentstate                       Component State    False             IncludeSubcomponents
//     sdkmessageresponsefield.componentstate                  Component State    False             IncludeSubcomponents
//     serviceendpoint.componentstate                          Component State    False             IncludeSubcomponents
//     similarityrule.componentstate                           Component State    False             IncludeSubcomponents
//     sitemap.componentstate                                  Component State    False             IncludeSubcomponents
//     sla.componentstate                                      Component State    True              IncludeSubcomponents
//     slaitem.componentstate                                  Component State    True              IncludeSubcomponents
//     syncattributemapping.componentstate                     Component State    False             IncludeSubcomponents
//     syncattributemappingprofile.componentstate              Component State    False             IncludeSubcomponents
//     systemform.componentstate                               Component State    False             IncludeSubcomponents
//     template.componentstate                                 Component State    True              IncludeSubcomponents
//     textanalyticsentitymapping.componentstate               Component State    False             IncludeSubcomponents
//     topicmodelconfiguration.componentstate                  Component State    False             IncludeSubcomponents
//     transformationmapping.componentstate                    Component State    False             IncludeSubcomponents
//     transformationparametermapping.componentstate           Component State    False             IncludeSubcomponents
//     webresource.componentstate                              Component State    False             IncludeSubcomponents
//     webwizard.componentstate                                Component State    False             IncludeSubcomponents
//     workflow.componentstate                                 Component State    True              IncludeSubcomponents
GlobalOptionSets.componentstateEnum = {

    'Published_0': { 'value': 0, 'text': 'Published', 'name1033': 'Published', 'name1049': 'Опубликовано' },
    'Unpublished_1': { 'value': 1, 'text': 'Unpublished', 'name1033': 'Unpublished', 'name1049': 'Неопубликованный' },
    'Deleted_2': { 'value': 2, 'text': 'Deleted', 'name1033': 'Deleted', 'name1049': 'Удалено' },
    'Deleted_Unpublished_3': { 'value': 3, 'text': 'Deleted Unpublished', 'name1033': 'Deleted Unpublished', 'name1049': 'Удален неопубликованным' },
};

// ComponentType:   Attribute (2)            Count: 6
//     AttributeName                              DisplayName                         IsCustomizable    Behavior
//     dependency.dependentcomponenttype                                              False             IncludeSubcomponents
//     dependency.requiredcomponenttype                                               False             IncludeSubcomponents
//     dependencynode.componenttype               Type Code                           False             IncludeSubcomponents
//     invaliddependency.existingcomponenttype    Existing Object's Component Type    False             IncludeSubcomponents
//     invaliddependency.missingcomponenttype     Type Code                           False             IncludeSubcomponents
//     solutioncomponent.componenttype            Object Type Code                    False             IncludeSubcomponents
GlobalOptionSets.componenttypeEnum = {

    'Entity_1': { 'value': 1, 'text': 'Entity', 'displayOrder': 1, 'name1033': 'Entity', 'name1049': 'Сущность' },
    'Attribute_2': { 'value': 2, 'text': 'Attribute', 'displayOrder': 2, 'name1033': 'Attribute', 'name1049': 'Атрибут' },
    'Relationship_3': { 'value': 3, 'text': 'Relationship', 'displayOrder': 3, 'name1033': 'Relationship', 'name1049': 'Отношение' },
    'Attribute_Picklist_Value_4': { 'value': 4, 'text': 'Attribute Picklist Value', 'displayOrder': 4, 'name1033': 'Attribute Picklist Value', 'name1049': 'Значение поля выбора атрибута' },
    'Attribute_Lookup_Value_5': { 'value': 5, 'text': 'Attribute Lookup Value', 'displayOrder': 5, 'name1033': 'Attribute Lookup Value', 'name1049': 'Значение подстановки атрибута' },
    'View_Attribute_6': { 'value': 6, 'text': 'View Attribute', 'displayOrder': 6, 'name1033': 'View Attribute', 'name1049': 'Просмотр атрибута' },
    'Localized_Label_7': { 'value': 7, 'text': 'Localized Label', 'displayOrder': 7, 'name1033': 'Localized Label', 'name1049': 'Локализованная надпись' },
    'Relationship_Extra_Condition_8': { 'value': 8, 'text': 'Relationship Extra Condition', 'displayOrder': 8, 'name1033': 'Relationship Extra Condition', 'name1049': 'Дополнительное условие отношения' },
    'Option_Set_9': { 'value': 9, 'text': 'Option Set', 'displayOrder': 9, 'name1033': 'Option Set', 'name1049': 'Набор параметров' },
    'Entity_Relationship_10': { 'value': 10, 'text': 'Entity Relationship', 'displayOrder': 10, 'name1033': 'Entity Relationship', 'name1049': 'Отношение сущности' },
    'Entity_Relationship_Role_11': { 'value': 11, 'text': 'Entity Relationship Role', 'displayOrder': 11, 'name1033': 'Entity Relationship Role', 'name1049': 'Роль отношения сущности' },
    'Entity_Relationship_Relationships_12': { 'value': 12, 'text': 'Entity Relationship Relationships', 'displayOrder': 12, 'name1033': 'Entity Relationship Relationships', 'name1049': 'Отношения отношения сущности' },
    'Managed_Property_13': { 'value': 13, 'text': 'Managed Property', 'displayOrder': 13, 'name1033': 'Managed Property', 'name1049': 'Управляемое свойство' },
    'Entity_Key_14': { 'value': 14, 'text': 'Entity Key', 'displayOrder': 14, 'name1033': 'Entity Key', 'name1049': 'Ключ сущности' },
    'Privilege_16': { 'value': 16, 'text': 'Privilege', 'displayOrder': 15, 'name1033': 'Privilege', 'name1049': 'Привилегия' },
    'PrivilegeObjectTypeCode_17': { 'value': 17, 'text': 'PrivilegeObjectTypeCode', 'displayOrder': 16, 'name1033': 'PrivilegeObjectTypeCode' },
    'Role_20': { 'value': 20, 'text': 'Role', 'displayOrder': 17, 'name1033': 'Role', 'name1049': 'Роль' },
    'Role_Privilege_21': { 'value': 21, 'text': 'Role Privilege', 'displayOrder': 18, 'name1033': 'Role Privilege', 'name1049': 'Привилегия роли' },
    'Display_String_22': { 'value': 22, 'text': 'Display String', 'displayOrder': 19, 'name1033': 'Display String', 'name1049': 'Отображаемая строка' },
    'Display_String_Map_23': { 'value': 23, 'text': 'Display String Map', 'displayOrder': 20, 'name1033': 'Display String Map', 'name1049': 'Сопоставление отображаемой строки' },
    'Form_24': { 'value': 24, 'text': 'Form', 'displayOrder': 21, 'name1033': 'Form', 'name1049': 'Форма' },
    'Organization_25': { 'value': 25, 'text': 'Organization', 'displayOrder': 22, 'name1033': 'Organization', 'name1049': 'Предприятие' },
    'Saved_Query_26': { 'value': 26, 'text': 'Saved Query', 'displayOrder': 23, 'name1033': 'Saved Query', 'name1049': 'Сохраненный запрос' },
    'Workflow_29': { 'value': 29, 'text': 'Workflow', 'displayOrder': 24, 'name1033': 'Workflow', 'name1049': 'Бизнес-процесс' },
    'Report_31': { 'value': 31, 'text': 'Report', 'displayOrder': 25, 'name1033': 'Report', 'name1049': 'Отчет' },
    'Report_Entity_32': { 'value': 32, 'text': 'Report Entity', 'displayOrder': 26, 'name1033': 'Report Entity', 'name1049': 'Сущность отчета' },
    'Report_Category_33': { 'value': 33, 'text': 'Report Category', 'displayOrder': 27, 'name1033': 'Report Category', 'name1049': 'Категория отчета' },
    'Report_Visibility_34': { 'value': 34, 'text': 'Report Visibility', 'displayOrder': 28, 'name1033': 'Report Visibility', 'name1049': 'Отображение отчета' },
    'Attachment_35': { 'value': 35, 'text': 'Attachment', 'displayOrder': 29, 'name1033': 'Attachment', 'name1049': 'Вложение' },
    'Email_Template_36': { 'value': 36, 'text': 'Email Template', 'displayOrder': 30, 'name1033': 'Email Template', 'name1049': 'Шаблон электронной почты' },
    'Contract_Template_37': { 'value': 37, 'text': 'Contract Template', 'displayOrder': 31, 'name1033': 'Contract Template', 'name1049': 'Шаблон контракта' },
    'KB_Article_Template_38': { 'value': 38, 'text': 'KB Article Template', 'displayOrder': 32, 'name1033': 'KB Article Template', 'name1049': 'Шаблон статьи базы знаний' },
    'Mail_Merge_Template_39': { 'value': 39, 'text': 'Mail Merge Template', 'displayOrder': 33, 'name1033': 'Mail Merge Template', 'name1049': 'Шаблон слияния' },
    'Duplicate_Rule_44': { 'value': 44, 'text': 'Duplicate Rule', 'displayOrder': 34, 'name1033': 'Duplicate Rule', 'name1049': 'Правило поиска дубликатов' },
    'Duplicate_Rule_Condition_45': { 'value': 45, 'text': 'Duplicate Rule Condition', 'displayOrder': 35, 'name1033': 'Duplicate Rule Condition', 'name1049': 'Условие правила обнаружения повторяющихся данных' },
    'Entity_Map_46': { 'value': 46, 'text': 'Entity Map', 'displayOrder': 36, 'name1033': 'Entity Map', 'name1049': 'Сопоставление сущностей' },
    'Attribute_Map_47': { 'value': 47, 'text': 'Attribute Map', 'displayOrder': 37, 'name1033': 'Attribute Map', 'name1049': 'Сопоставление атрибутов' },
    'Ribbon_Command_48': { 'value': 48, 'text': 'Ribbon Command', 'displayOrder': 38, 'name1033': 'Ribbon Command', 'name1049': 'Команда ленты' },
    'Ribbon_Context_Group_49': { 'value': 49, 'text': 'Ribbon Context Group', 'displayOrder': 39, 'name1033': 'Ribbon Context Group', 'name1049': 'Контекстная группа ленты' },
    'Ribbon_Customization_50': { 'value': 50, 'text': 'Ribbon Customization', 'displayOrder': 40, 'name1033': 'Ribbon Customization', 'name1049': 'Настройка ленты' },
    'Ribbon_Rule_52': { 'value': 52, 'text': 'Ribbon Rule', 'displayOrder': 41, 'name1033': 'Ribbon Rule', 'name1049': 'Правило ленты' },
    'Ribbon_Tab_To_Command_Map_53': { 'value': 53, 'text': 'Ribbon Tab To Command Map', 'displayOrder': 42, 'name1033': 'Ribbon Tab To Command Map', 'name1049': 'Сопоставление вкладки ленты с командой' },
    'Ribbon_Diff_55': { 'value': 55, 'text': 'Ribbon Diff', 'displayOrder': 43, 'name1033': 'Ribbon Diff', 'name1049': 'Различие ленты' },
    'Saved_Query_Visualization_59': { 'value': 59, 'text': 'Saved Query Visualization', 'displayOrder': 44, 'name1033': 'Saved Query Visualization', 'name1049': 'Сохраненная визуализация запроса' },
    'System_Form_60': { 'value': 60, 'text': 'System Form', 'displayOrder': 45, 'name1033': 'System Form', 'name1049': 'Системная форма' },
    'Web_Resource_61': { 'value': 61, 'text': 'Web Resource', 'displayOrder': 46, 'name1033': 'Web Resource', 'name1049': 'Веб-ресурс' },
    'Site_Map_62': { 'value': 62, 'text': 'Site Map', 'displayOrder': 47, 'name1033': 'Site Map', 'name1049': 'Карта сайта' },
    'Connection_Role_63': { 'value': 63, 'text': 'Connection Role', 'displayOrder': 48, 'name1033': 'Connection Role', 'name1049': 'Роль подключения' },
    'Complex_Control_64': { 'value': 64, 'text': 'Complex Control', 'displayOrder': 49, 'name1033': 'Complex Control', 'name1049': 'Сложный элемент управления' },
    'Field_Security_Profile_70': { 'value': 70, 'text': 'Field Security Profile', 'displayOrder': 50, 'name1033': 'Field Security Profile', 'name1049': 'Профиль безопасности поля' },
    'Field_Permission_71': { 'value': 71, 'text': 'Field Permission', 'displayOrder': 51, 'name1033': 'Field Permission', 'name1049': 'Разрешение поля' },
    'Plugin_Type_90': { 'value': 90, 'text': 'Plugin Type', 'displayOrder': 52, 'name1033': 'Plugin Type', 'name1049': 'Тип подключаемого модуля' },
    'Plugin_Assembly_91': { 'value': 91, 'text': 'Plugin Assembly', 'displayOrder': 53, 'name1033': 'Plugin Assembly', 'name1049': 'Сборка подключаемого модуля' },
    'SDK_Message_Processing_Step_92': { 'value': 92, 'text': 'SDK Message Processing Step', 'displayOrder': 54, 'name1033': 'SDK Message Processing Step', 'name1049': 'Шаг обработки сообщения SDK' },
    'SDK_Message_Processing_Step_Image_93': { 'value': 93, 'text': 'SDK Message Processing Step Image', 'displayOrder': 55, 'name1033': 'SDK Message Processing Step Image', 'name1049': 'Образ шага обработки сообщения SDK' },
    'Service_Endpoint_95': { 'value': 95, 'text': 'Service Endpoint', 'displayOrder': 56, 'name1033': 'Service Endpoint', 'name1049': 'Конечная точка сервиса' },
    'Routing_Rule_150': { 'value': 150, 'text': 'Routing Rule', 'displayOrder': 57, 'name1033': 'Routing Rule', 'name1049': 'Правило маршрутизации' },
    'Routing_Rule_Item_151': { 'value': 151, 'text': 'Routing Rule Item', 'displayOrder': 58, 'name1033': 'Routing Rule Item', 'name1049': 'Элемент правила маршрутизации' },
    'SLA_152': { 'value': 152, 'text': 'SLA', 'displayOrder': 59, 'name1033': 'SLA', 'name1049': 'Соглашение об уровне обслуживания' },
    'SLA_Item_153': { 'value': 153, 'text': 'SLA Item', 'displayOrder': 60, 'name1033': 'SLA Item', 'name1049': 'Элемент SLA' },
    'Convert_Rule_154': { 'value': 154, 'text': 'Convert Rule', 'displayOrder': 61, 'name1033': 'Convert Rule', 'name1049': 'Правило преобразования' },
    'Convert_Rule_Item_155': { 'value': 155, 'text': 'Convert Rule Item', 'displayOrder': 62, 'name1033': 'Convert Rule Item', 'name1049': 'Элемент правила преобразования' },
    'Hierarchy_Rule_65': { 'value': 65, 'text': 'Hierarchy Rule', 'displayOrder': 63, 'name1033': 'Hierarchy Rule', 'name1049': 'Правило иерархии' },
    'Mobile_Offline_Profile_161': { 'value': 161, 'text': 'Mobile Offline Profile', 'displayOrder': 64, 'name1033': 'Mobile Offline Profile', 'name1049': 'Профиль Mobile Offline' },
    'Mobile_Offline_Profile_Item_162': { 'value': 162, 'text': 'Mobile Offline Profile Item', 'displayOrder': 65, 'name1033': 'Mobile Offline Profile Item', 'name1049': 'Элемент профиля Mobile Offline' },
    'Similarity_Rule_165': { 'value': 165, 'text': 'Similarity Rule', 'displayOrder': 66, 'name1033': 'Similarity Rule', 'name1049': 'Правило подобия' },
    'Custom_Control_66': { 'value': 66, 'text': 'Custom Control', 'displayOrder': 67, 'name1033': 'Custom Control', 'name1049': 'Пользовательский элемент управления' },
    'Custom_Control_Default_Config_68': { 'value': 68, 'text': 'Custom Control Default Config', 'displayOrder': 68, 'name1033': 'Custom Control Default Config', 'name1049': 'Конфигурация пользовательского элемента управления по умолчанию' },
    'Data_Source_Mapping_166': { 'value': 166, 'text': 'Data Source Mapping', 'displayOrder': 69, 'name1033': 'Data Source Mapping', 'name1049': 'Сопоставление источника данных' },
    'SDKMessage_201': { 'value': 201, 'text': 'SDKMessage', 'displayOrder': 70, 'name1033': 'SDKMessage' },
    'SDKMessageFilter_202': { 'value': 202, 'text': 'SDKMessageFilter', 'displayOrder': 71, 'name1033': 'SDKMessageFilter' },
    'SdkMessagePair_203': { 'value': 203, 'text': 'SdkMessagePair', 'displayOrder': 72, 'name1033': 'SdkMessagePair' },
    'SdkMessageRequest_204': { 'value': 204, 'text': 'SdkMessageRequest', 'displayOrder': 73, 'name1033': 'SdkMessageRequest' },
    'SdkMessageRequestField_205': { 'value': 205, 'text': 'SdkMessageRequestField', 'displayOrder': 74, 'name1033': 'SdkMessageRequestField' },
    'SdkMessageResponse_206': { 'value': 206, 'text': 'SdkMessageResponse', 'displayOrder': 75, 'name1033': 'SdkMessageResponse' },
    'SdkMessageResponseField_207': { 'value': 207, 'text': 'SdkMessageResponseField', 'displayOrder': 76, 'name1033': 'SdkMessageResponseField' },
    'WebWizard_210': { 'value': 210, 'text': 'WebWizard', 'displayOrder': 77, 'name1033': 'WebWizard' },
    'Index_18': { 'value': 18, 'text': 'Index', 'displayOrder': 78, 'name1033': 'Index', 'name1049': 'Индекс' },
    'Import_Map_208': { 'value': 208, 'text': 'Import Map', 'displayOrder': 79, 'name1033': 'Import Map', 'name1049': 'Сопоставление для импорта' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName              DisplayName                 IsCustomizable    Behavior
//     connectionrole.category    Connection Role Category    True              IncludeSubcomponents
// ComponentType:   ConnectionRole (63)            Count: 33
//     Name                            IsCustomizable    Behavior
//     Account Manager                 True              IncludeSubcomponents
//     Associated Product              True              IncludeSubcomponents
//     Champion                        True              IncludeSubcomponents
//     Child                           True              IncludeSubcomponents
//     Colleague                       True              IncludeSubcomponents
//     Decision Maker                  True              IncludeSubcomponents
//     Delivery Professional           True              IncludeSubcomponents
//     Economic Buyer                  True              IncludeSubcomponents
//     Employee                        True              IncludeSubcomponents
//     Employer                        True              IncludeSubcomponents
//     End User                        True              IncludeSubcomponents
//     Former Employee                 True              IncludeSubcomponents
//     Former Employer                 True              IncludeSubcomponents
//     Friend                          True              IncludeSubcomponents
//     Industry Expert                 True              IncludeSubcomponents
//     Influencer                      True              IncludeSubcomponents
//     Knowledge Article               True              IncludeSubcomponents
//     Parent                          True              IncludeSubcomponents
//     Partner                         True              IncludeSubcomponents
//     Primary Article                 True              IncludeSubcomponents
//     Primary Case                    True              IncludeSubcomponents
//     Referral                        True              IncludeSubcomponents
//     Referred by                     True              IncludeSubcomponents
//     Related Article                 True              IncludeSubcomponents
//     Related case                    True              IncludeSubcomponents
//     Sales Professional              True              IncludeSubcomponents
//     Service Professional            True              IncludeSubcomponents
//     Spouse/Partner                  True              IncludeSubcomponents
//     Stakeholder                     True              IncludeSubcomponents
//     Technical Buyer                 True              IncludeSubcomponents
//     Technical Sales Professional    True              IncludeSubcomponents
//     Territory Default Pricelist     True              IncludeSubcomponents
//     Territory Manager               True              IncludeSubcomponents
GlobalOptionSets.connectionrole_categoryEnum = {

    'Business_1': { 'value': 1, 'text': 'Business', 'displayOrder': 1, 'name1033': 'Business', 'name1049': 'Бизнес' },
    'Family_2': { 'value': 2, 'text': 'Family', 'displayOrder': 2, 'name1033': 'Family', 'name1049': 'Семья' },
    'Social_3': { 'value': 3, 'text': 'Social', 'displayOrder': 3, 'name1033': 'Social', 'name1049': 'Общественный' },
    'Sales_4': { 'value': 4, 'text': 'Sales', 'displayOrder': 4, 'name1033': 'Sales', 'name1049': 'Продажи' },
    'Other_5': { 'value': 5, 'text': 'Other', 'displayOrder': 5, 'name1033': 'Other', 'name1049': 'Прочее' },
    'Stakeholder_1000': { 'value': 1000, 'text': 'Stakeholder', 'displayOrder': 6, 'name1033': 'Stakeholder', 'name1049': 'Заинтересованное лицо' },
    'Sales_Team_1001': { 'value': 1001, 'text': 'Sales Team', 'displayOrder': 7, 'name1033': 'Sales Team', 'name1049': 'ГРУППА ПРОДАЖ' },
    'Service_1002': { 'value': 1002, 'text': 'Service', 'displayOrder': 8, 'name1033': 'Service', 'name1049': 'Сервис' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                            IsCustomizable    Behavior
//     none          Dialog      Convert Campaign Response Dialog    False             IncludeSubcomponents
GlobalOptionSets.convert_campaign_response_deactivate_statusEnum = {

    'Closed_1': { 'value': 1, 'text': 'Closed', 'name1033': 'Closed', 'name1049': 'Закрыто' },
    'Cancelled_2': { 'value': 2, 'text': 'Cancelled', 'name1033': 'Cancelled', 'name1049': 'Отменено' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                            IsCustomizable    Behavior
//     none          Dialog      Convert Campaign Response Dialog    False             IncludeSubcomponents
GlobalOptionSets.convert_campaign_response_optionEnum = {

    'Create_a_lead_1': { 'value': 1, 'text': 'Create a lead', 'name1033': 'Create a lead', 'name1049': 'Создать интерес' },
    'Convert_to_an_existing_lead_2': { 'value': 2, 'text': 'Convert to an existing lead', 'name1033': 'Convert to an existing lead', 'name1049': 'Преобразовать в существующий интерес' },
    'Create_a_quote_order_or_opportunity_for_an_account_or_contact_3': { 'value': 3, 'text': 'Create a quote, order, or opportunity for an account or contact', 'name1033': 'Create a quote, order, or opportunity for an account or contact', 'name1049': 'Создать предложение с расценками, заказ или возможную сделку для организации или контакта' },
    'Close_response_4': { 'value': 4, 'text': 'Close response', 'name1033': 'Close response', 'name1049': 'Закрыть отклик от кампании' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                            IsCustomizable    Behavior
//     none          Dialog      Convert Campaign Response Dialog    False             IncludeSubcomponents
GlobalOptionSets.convert_campaign_response_sales_entity_typeEnum = {

    'Opportunity_1': { 'value': 1, 'text': 'Opportunity', 'name1033': 'Opportunity', 'name1049': 'Возможная сделка' },
    'Order_2': { 'value': 2, 'text': 'Order', 'name1033': 'Order', 'name1049': 'Заказ' },
    'Quote_3': { 'value': 3, 'text': 'Quote', 'name1033': 'Quote', 'name1049': 'Предложение с расценками' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                                    IsCustomizable    Behavior
//     none          Dialog      Convert Campaign Response To Lead Dialog    False             IncludeSubcomponents
GlobalOptionSets.convert_campaign_response_to_lead_disqualify_statusEnum = {

    'Lost_4': { 'value': 4, 'text': 'Lost', 'name1033': 'Lost', 'name1049': 'Потерян' },
    'Cannot_Contact_5': { 'value': 5, 'text': 'Cannot Contact', 'name1033': 'Cannot Contact', 'name1049': 'Невозможно связаться' },
    'No_Longer_Interested_6': { 'value': 6, 'text': 'No Longer Interested', 'name1033': 'No Longer Interested', 'name1049': 'Больше не интересуется' },
    'Canceled_7': { 'value': 7, 'text': 'Canceled', 'name1033': 'Canceled', 'name1049': 'Отменен' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                                    IsCustomizable    Behavior
//     none          Dialog      Convert Campaign Response To Lead Dialog    False             IncludeSubcomponents
GlobalOptionSets.convert_campaign_response_to_lead_optionEnum = {

    'Qualify_and_convert_into_the_following_records_1': { 'value': 1, 'text': 'Qualify and convert into the following records', 'name1033': 'Qualify and convert into the following records', 'name1049': 'Квалифицировать и преобразовать в записи' },
    'Disqualify_2': { 'value': 2, 'text': 'Disqualify', 'name1033': 'Disqualify', 'name1049': 'Дисквалифицировать' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                                    IsCustomizable    Behavior
//     none          Dialog      Convert Campaign Response To Lead Dialog    False             IncludeSubcomponents
GlobalOptionSets.convert_campaign_response_to_lead_qualify_statusEnum = {

    'Qualified_3': { 'value': 3, 'text': 'Qualified', 'name1033': 'Qualified', 'name1049': 'Квалифицированный' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                             DisplayName       IsCustomizable    Behavior
//     channelpropertygroup.regardingtypecode    Regarding Type    False             IncludeSubcomponents
//     convertrule.sourcechanneltypecode         Source Type       False             IncludeSubcomponents
GlobalOptionSets.convertrule_channelactivityEnum = {

    'Phone_Call_4210': { 'value': 4210, 'text': 'Phone Call', 'displayOrder': 1, 'name1033': 'Phone Call', 'name1049': 'Звонок' },
    'Email_4202': { 'value': 4202, 'text': 'Email', 'displayOrder': 2, 'name1033': 'Email', 'name1049': 'Электронная почта' },
    'Appointment_4201': { 'value': 4201, 'text': 'Appointment', 'displayOrder': 3, 'name1033': 'Appointment', 'name1049': 'Встреча' },
    'Task_4212': { 'value': 4212, 'text': 'Task', 'displayOrder': 4, 'name1033': 'Task', 'name1049': 'Задача' },
    'Social_Activity_4216': { 'value': 4216, 'text': 'Social Activity', 'displayOrder': 5, 'name1033': 'Social Activity', 'name1049': 'Действие социальной сети' },
    'Service_Activity_4214': { 'value': 4214, 'text': 'Service Activity', 'displayOrder': 6, 'name1033': 'Service Activity', 'name1049': 'Действие сервиса' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                               DisplayName        IsCustomizable    Behavior
//     dependency.dependencytype                   Dependency Type    False             IncludeSubcomponents
//     invaliddependency.existingdependencytype    Weight             False             IncludeSubcomponents
GlobalOptionSets.dependencytypeEnum = {

    'None_0': { 'value': 0, 'text': 'None', 'displayOrder': 1, 'name1033': 'None', 'name1049': 'Нет' },
    'Solution_Internal_1': { 'value': 1, 'text': 'Solution Internal', 'displayOrder': 2, 'name1033': 'Solution Internal', 'name1049': 'Внутри решения' },
    'Published_2': { 'value': 2, 'text': 'Published', 'displayOrder': 3, 'name1033': 'Published', 'name1049': 'Опубликовано' },
    'Unpublished_4': { 'value': 4, 'text': 'Unpublished', 'displayOrder': 4, 'name1033': 'Unpublished', 'name1049': 'Неопубликованный' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                                        DisplayName                         IsCustomizable    Behavior
//     emailserverprofile.incomingauthenticationprotocol    Incoming Authentication Protocol    True              IncludeSubcomponents
//     emailserverprofile.outgoingauthenticationprotocol    Outgoing Authentication Protocol    True              IncludeSubcomponents
GlobalOptionSets.emailserverprofile_authenticationprotocolEnum = {

    'Auto_Detect_0': { 'value': 0, 'text': 'Auto Detect', 'displayOrder': 1, 'name1033': 'Auto Detect', 'name1049': 'Автоматическое обнаружение' },
    'Negotiate_1': { 'value': 1, 'text': 'Negotiate', 'displayOrder': 2, 'name1033': 'Negotiate', 'name1049': 'Согласовывать' },
    'NTLM_2': { 'value': 2, 'text': 'NTLM', 'displayOrder': 3, 'name1033': 'NTLM' },
    'Basic_3': { 'value': 3, 'text': 'Basic', 'displayOrder': 4, 'name1033': 'Basic', 'name1049': 'Базовая' },
};

// ComponentType:   Attribute (2)            Count: 6
//     AttributeName                               DisplayName                 IsCustomizable    Behavior
//     fieldpermission.cancreate                   Can create the attribute    False             IncludeSubcomponents
//     fieldpermission.canread                     Can Read the attribute      False             IncludeSubcomponents
//     fieldpermission.canupdate                   Can Update the attribute    False             IncludeSubcomponents
//     principalattributeaccessmap.createaccess                                False             IncludeSubcomponents
//     principalattributeaccessmap.readaccess                                  False             IncludeSubcomponents
//     principalattributeaccessmap.updateaccess                                False             IncludeSubcomponents
GlobalOptionSets.field_security_permission_typeEnum = {

    'Not_Allowed_0': { 'value': 0, 'text': 'Not Allowed', 'displayOrder': 1, 'name1033': 'Not Allowed', 'name1049': 'Недопустимо' },
    'Allowed_4': { 'value': 4, 'text': 'Allowed', 'displayOrder': 2, 'name1033': 'Allowed', 'name1049': 'Допустимо' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                  IsCustomizable    Behavior
//     none          Dialog      Device Settings Dialog    False             IncludeSubcomponents
GlobalOptionSets.flipswitch_optionsEnum = {

    'Off_0': { 'value': 0, 'text': 'Off', 'name1033': 'Off', 'name1049': 'Выкл.' },
    'On_1': { 'value': 1, 'text': 'On', 'name1033': 'On', 'name1049': 'Вкл.' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                              DisplayName              IsCustomizable    Behavior
//     account.gbc_multiselectpicklist            Multi Select Picklist    True              IncludeSubcomponents
//     gbc_entity_test.gbc_multiselectpicklist    Multi Select Picklist    True              IncludeSubcomponents
GlobalOptionSets.gbc_multiselectEnum = {

    'One_865240000': { 'value': 865240000, 'text': 'One', 'displayOrder': 1, 'name1033': 'One' },
    'Two_865240001': { 'value': 865240001, 'text': 'Two', 'displayOrder': 2, 'name1033': 'Two' },
    'Three_865240002': { 'value': 865240002, 'text': 'Three', 'displayOrder': 3, 'name1033': 'Three' },
    'Four_865240003': { 'value': 865240003, 'text': 'Four', 'displayOrder': 4, 'name1033': 'Four' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName        DisplayName      IsCustomizable    Behavior
//     goal.fiscalperiod    Fiscal Period    True              IncludeSubcomponents
GlobalOptionSets.goal_fiscalperiodEnum = {

    'Quarter_1_1': { 'value': 1, 'text': 'Quarter 1', 'displayOrder': 1, 'name1033': 'Quarter 1', 'name1049': 'Квартал 1' },
    'Quarter_2_2': { 'value': 2, 'text': 'Quarter 2', 'displayOrder': 2, 'name1033': 'Quarter 2', 'name1049': 'Квартал 2' },
    'Quarter_3_3': { 'value': 3, 'text': 'Quarter 3', 'displayOrder': 3, 'name1033': 'Quarter 3', 'name1049': 'Квартал 3' },
    'Quarter_4_4': { 'value': 4, 'text': 'Quarter 4', 'displayOrder': 4, 'name1033': 'Quarter 4', 'name1049': 'Квартал 4' },
    'January_101': { 'value': 101, 'text': 'January', 'displayOrder': 5, 'name1033': 'January', 'name1049': 'Январь' },
    'February_102': { 'value': 102, 'text': 'February', 'displayOrder': 6, 'name1033': 'February', 'name1049': 'Февраль' },
    'March_103': { 'value': 103, 'text': 'March', 'displayOrder': 7, 'name1033': 'March', 'name1049': 'Март' },
    'April_104': { 'value': 104, 'text': 'April', 'displayOrder': 8, 'name1033': 'April', 'name1049': 'Апрель' },
    'May_105': { 'value': 105, 'text': 'May', 'displayOrder': 9, 'name1033': 'May', 'name1049': 'Май' },
    'June_106': { 'value': 106, 'text': 'June', 'displayOrder': 10, 'name1033': 'June', 'name1049': 'Июнь' },
    'July_107': { 'value': 107, 'text': 'July', 'displayOrder': 11, 'name1033': 'July', 'name1049': 'Июль' },
    'August_108': { 'value': 108, 'text': 'August', 'displayOrder': 12, 'name1033': 'August', 'name1049': 'Август' },
    'September_109': { 'value': 109, 'text': 'September', 'displayOrder': 13, 'name1033': 'September', 'name1049': 'Сентябрь' },
    'October_110': { 'value': 110, 'text': 'October', 'displayOrder': 14, 'name1033': 'October', 'name1049': 'Октябрь' },
    'November_111': { 'value': 111, 'text': 'November', 'displayOrder': 15, 'name1033': 'November', 'name1049': 'Ноябрь' },
    'December_112': { 'value': 112, 'text': 'December', 'displayOrder': 16, 'name1033': 'December', 'name1049': 'Декабрь' },
    'Semester_1_201': { 'value': 201, 'text': 'Semester 1', 'displayOrder': 17, 'name1033': 'Semester 1', 'name1049': 'Семестр 1' },
    'Semester_2_202': { 'value': 202, 'text': 'Semester 2', 'displayOrder': 18, 'name1033': 'Semester 2', 'name1049': 'Семестр 2' },
    'Annual_301': { 'value': 301, 'text': 'Annual', 'displayOrder': 19, 'name1033': 'Annual', 'name1049': 'Годовой' },
    'P1_401': { 'value': 401, 'text': 'P1', 'displayOrder': 20, 'name1033': 'P1', 'name1049': 'П1' },
    'P2_402': { 'value': 402, 'text': 'P2', 'displayOrder': 21, 'name1033': 'P2', 'name1049': 'П2' },
    'P3_403': { 'value': 403, 'text': 'P3', 'displayOrder': 22, 'name1033': 'P3', 'name1049': 'П3' },
    'P4_404': { 'value': 404, 'text': 'P4', 'displayOrder': 23, 'name1033': 'P4', 'name1049': 'П4' },
    'P5_405': { 'value': 405, 'text': 'P5', 'displayOrder': 24, 'name1033': 'P5', 'name1049': 'П5' },
    'P6_406': { 'value': 406, 'text': 'P6', 'displayOrder': 25, 'name1033': 'P6', 'name1049': 'П6' },
    'P7_407': { 'value': 407, 'text': 'P7', 'displayOrder': 26, 'name1033': 'P7', 'name1049': 'П7' },
    'P8_408': { 'value': 408, 'text': 'P8', 'displayOrder': 27, 'name1033': 'P8', 'name1049': 'П8' },
    'P9_409': { 'value': 409, 'text': 'P9', 'displayOrder': 28, 'name1033': 'P9', 'name1049': 'П9' },
    'P10_410': { 'value': 410, 'text': 'P10', 'displayOrder': 29, 'name1033': 'P10', 'name1049': 'П10' },
    'P11_411': { 'value': 411, 'text': 'P11', 'displayOrder': 30, 'name1033': 'P11', 'name1049': 'П11' },
    'P12_412': { 'value': 412, 'text': 'P12', 'displayOrder': 31, 'name1033': 'P12', 'name1049': 'П12' },
    'P13_413': { 'value': 413, 'text': 'P13', 'displayOrder': 32, 'name1033': 'P13', 'name1049': 'П13' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName      DisplayName    IsCustomizable    Behavior
//     goal.fiscalyear    Fiscal Year    True              IncludeSubcomponents
GlobalOptionSets.goal_fiscalyearEnum = {

    'FY2038_2038': { 'value': 2038, 'text': 'FY2038', 'displayOrder': 1, 'name1033': 'FY2038', 'name1049': 'ФГ2038' },
    'FY2037_2037': { 'value': 2037, 'text': 'FY2037', 'displayOrder': 2, 'name1033': 'FY2037', 'name1049': 'ФГ2037' },
    'FY2036_2036': { 'value': 2036, 'text': 'FY2036', 'displayOrder': 3, 'name1033': 'FY2036', 'name1049': 'ФГ2036' },
    'FY2035_2035': { 'value': 2035, 'text': 'FY2035', 'displayOrder': 4, 'name1033': 'FY2035', 'name1049': 'ФГ2035' },
    'FY2034_2034': { 'value': 2034, 'text': 'FY2034', 'displayOrder': 5, 'name1033': 'FY2034', 'name1049': 'ФГ2034' },
    'FY2033_2033': { 'value': 2033, 'text': 'FY2033', 'displayOrder': 6, 'name1033': 'FY2033', 'name1049': 'ФГ2033' },
    'FY2032_2032': { 'value': 2032, 'text': 'FY2032', 'displayOrder': 7, 'name1033': 'FY2032', 'name1049': 'ФГ2032' },
    'FY2031_2031': { 'value': 2031, 'text': 'FY2031', 'displayOrder': 8, 'name1033': 'FY2031', 'name1049': 'ФГ2031' },
    'FY2030_2030': { 'value': 2030, 'text': 'FY2030', 'displayOrder': 9, 'name1033': 'FY2030', 'name1049': 'ФГ2030' },
    'FY2029_2029': { 'value': 2029, 'text': 'FY2029', 'displayOrder': 10, 'name1033': 'FY2029', 'name1049': 'ФГ2029' },
    'FY2028_2028': { 'value': 2028, 'text': 'FY2028', 'displayOrder': 11, 'name1033': 'FY2028', 'name1049': 'ФГ2028' },
    'FY2027_2027': { 'value': 2027, 'text': 'FY2027', 'displayOrder': 12, 'name1033': 'FY2027', 'name1049': 'ФГ2027' },
    'FY2026_2026': { 'value': 2026, 'text': 'FY2026', 'displayOrder': 13, 'name1033': 'FY2026', 'name1049': 'ФГ2026' },
    'FY2025_2025': { 'value': 2025, 'text': 'FY2025', 'displayOrder': 14, 'name1033': 'FY2025', 'name1049': 'ФГ2025' },
    'FY2024_2024': { 'value': 2024, 'text': 'FY2024', 'displayOrder': 15, 'name1033': 'FY2024', 'name1049': 'ФГ2024' },
    'FY2023_2023': { 'value': 2023, 'text': 'FY2023', 'displayOrder': 16, 'name1033': 'FY2023', 'name1049': 'ФГ2023' },
    'FY2022_2022': { 'value': 2022, 'text': 'FY2022', 'displayOrder': 17, 'name1033': 'FY2022', 'name1049': 'ФГ2022' },
    'FY2021_2021': { 'value': 2021, 'text': 'FY2021', 'displayOrder': 18, 'name1033': 'FY2021', 'name1049': 'ФГ2021' },
    'FY2020_2020': { 'value': 2020, 'text': 'FY2020', 'displayOrder': 19, 'name1033': 'FY2020', 'name1049': 'ФГ2020' },
    'FY2019_2019': { 'value': 2019, 'text': 'FY2019', 'displayOrder': 20, 'name1033': 'FY2019', 'name1049': 'ФГ2019' },
    'FY2018_2018': { 'value': 2018, 'text': 'FY2018', 'displayOrder': 21, 'name1033': 'FY2018', 'name1049': 'ФГ2018' },
    'FY2017_2017': { 'value': 2017, 'text': 'FY2017', 'displayOrder': 22, 'name1033': 'FY2017', 'name1049': 'ФГ2017' },
    'FY2016_2016': { 'value': 2016, 'text': 'FY2016', 'displayOrder': 23, 'name1033': 'FY2016', 'name1049': 'ФГ2016' },
    'FY2015_2015': { 'value': 2015, 'text': 'FY2015', 'displayOrder': 24, 'name1033': 'FY2015', 'name1049': 'ФГ2015' },
    'FY2014_2014': { 'value': 2014, 'text': 'FY2014', 'displayOrder': 25, 'name1033': 'FY2014', 'name1049': 'ФГ2014' },
    'FY2013_2013': { 'value': 2013, 'text': 'FY2013', 'displayOrder': 26, 'name1033': 'FY2013', 'name1049': 'ФГ2013' },
    'FY2012_2012': { 'value': 2012, 'text': 'FY2012', 'displayOrder': 27, 'name1033': 'FY2012', 'name1049': 'ФГ2012' },
    'FY2011_2011': { 'value': 2011, 'text': 'FY2011', 'displayOrder': 28, 'name1033': 'FY2011', 'name1049': 'ФГ2011' },
    'FY2010_2010': { 'value': 2010, 'text': 'FY2010', 'displayOrder': 29, 'name1033': 'FY2010', 'name1049': 'ФГ2010' },
    'FY2009_2009': { 'value': 2009, 'text': 'FY2009', 'displayOrder': 30, 'name1033': 'FY2009', 'name1049': 'ФГ2009' },
    'FY2008_2008': { 'value': 2008, 'text': 'FY2008', 'displayOrder': 31, 'name1033': 'FY2008', 'name1049': 'ФГ2008' },
    'FY2007_2007': { 'value': 2007, 'text': 'FY2007', 'displayOrder': 32, 'name1033': 'FY2007', 'name1049': 'ФГ2007' },
    'FY2006_2006': { 'value': 2006, 'text': 'FY2006', 'displayOrder': 33, 'name1033': 'FY2006', 'name1049': 'ФГ2006' },
    'FY2005_2005': { 'value': 2005, 'text': 'FY2005', 'displayOrder': 34, 'name1033': 'FY2005', 'name1049': 'ФГ2005' },
    'FY2004_2004': { 'value': 2004, 'text': 'FY2004', 'displayOrder': 35, 'name1033': 'FY2004', 'name1049': 'ФГ2004' },
    'FY2003_2003': { 'value': 2003, 'text': 'FY2003', 'displayOrder': 36, 'name1033': 'FY2003', 'name1049': 'ФГ2003' },
    'FY2002_2002': { 'value': 2002, 'text': 'FY2002', 'displayOrder': 37, 'name1033': 'FY2002', 'name1049': 'ФГ2002' },
    'FY2001_2001': { 'value': 2001, 'text': 'FY2001', 'displayOrder': 38, 'name1033': 'FY2001', 'name1049': 'ФГ2001' },
    'FY2000_2000': { 'value': 2000, 'text': 'FY2000', 'displayOrder': 39, 'name1033': 'FY2000', 'name1049': 'ФГ2000' },
    'FY1999_1999': { 'value': 1999, 'text': 'FY1999', 'displayOrder': 40, 'name1033': 'FY1999', 'name1049': 'ФГ1999' },
    'FY1998_1998': { 'value': 1998, 'text': 'FY1998', 'displayOrder': 41, 'name1033': 'FY1998', 'name1049': 'ФГ1998' },
    'FY1997_1997': { 'value': 1997, 'text': 'FY1997', 'displayOrder': 42, 'name1033': 'FY1997', 'name1049': 'ФГ1997' },
    'FY1996_1996': { 'value': 1996, 'text': 'FY1996', 'displayOrder': 43, 'name1033': 'FY1996', 'name1049': 'ФГ1996' },
    'FY1995_1995': { 'value': 1995, 'text': 'FY1995', 'displayOrder': 44, 'name1033': 'FY1995', 'name1049': 'ФГ1995' },
    'FY1994_1994': { 'value': 1994, 'text': 'FY1994', 'displayOrder': 45, 'name1033': 'FY1994', 'name1049': 'ФГ1994' },
    'FY1993_1993': { 'value': 1993, 'text': 'FY1993', 'displayOrder': 46, 'name1033': 'FY1993', 'name1049': 'ФГ1993' },
    'FY1992_1992': { 'value': 1992, 'text': 'FY1992', 'displayOrder': 47, 'name1033': 'FY1992', 'name1049': 'ФГ1992' },
    'FY1991_1991': { 'value': 1991, 'text': 'FY1991', 'displayOrder': 48, 'name1033': 'FY1991', 'name1049': 'ФГ1991' },
    'FY1990_1990': { 'value': 1990, 'text': 'FY1990', 'displayOrder': 49, 'name1033': 'FY1990', 'name1049': 'ФГ1990' },
    'FY1989_1989': { 'value': 1989, 'text': 'FY1989', 'displayOrder': 50, 'name1033': 'FY1989', 'name1049': 'ФГ1989' },
    'FY1988_1988': { 'value': 1988, 'text': 'FY1988', 'displayOrder': 51, 'name1033': 'FY1988', 'name1049': 'ФГ1988' },
    'FY1987_1987': { 'value': 1987, 'text': 'FY1987', 'displayOrder': 52, 'name1033': 'FY1987', 'name1049': 'ФГ1987' },
    'FY1986_1986': { 'value': 1986, 'text': 'FY1986', 'displayOrder': 53, 'name1033': 'FY1986', 'name1049': 'ФГ1986' },
    'FY1985_1985': { 'value': 1985, 'text': 'FY1985', 'displayOrder': 54, 'name1033': 'FY1985', 'name1049': 'ФГ1985' },
    'FY1984_1984': { 'value': 1984, 'text': 'FY1984', 'displayOrder': 55, 'name1033': 'FY1984', 'name1049': 'ФГ1984' },
    'FY1983_1983': { 'value': 1983, 'text': 'FY1983', 'displayOrder': 56, 'name1033': 'FY1983', 'name1049': 'ФГ1983' },
    'FY1982_1982': { 'value': 1982, 'text': 'FY1982', 'displayOrder': 57, 'name1033': 'FY1982', 'name1049': 'ФГ1982' },
    'FY1981_1981': { 'value': 1981, 'text': 'FY1981', 'displayOrder': 58, 'name1033': 'FY1981', 'name1049': 'ФГ1981' },
    'FY1980_1980': { 'value': 1980, 'text': 'FY1980', 'displayOrder': 59, 'name1033': 'FY1980', 'name1049': 'ФГ1980' },
    'FY1979_1979': { 'value': 1979, 'text': 'FY1979', 'displayOrder': 60, 'name1033': 'FY1979', 'name1049': 'ФГ1979' },
    'FY1978_1978': { 'value': 1978, 'text': 'FY1978', 'displayOrder': 61, 'name1033': 'FY1978', 'name1049': 'ФГ1978' },
    'FY1977_1977': { 'value': 1977, 'text': 'FY1977', 'displayOrder': 62, 'name1033': 'FY1977', 'name1049': 'ФГ1977' },
    'FY1976_1976': { 'value': 1976, 'text': 'FY1976', 'displayOrder': 63, 'name1033': 'FY1976', 'name1049': 'ФГ1976' },
    'FY1975_1975': { 'value': 1975, 'text': 'FY1975', 'displayOrder': 64, 'name1033': 'FY1975', 'name1049': 'ФГ1975' },
    'FY1974_1974': { 'value': 1974, 'text': 'FY1974', 'displayOrder': 65, 'name1033': 'FY1974', 'name1049': 'ФГ1974' },
    'FY1973_1973': { 'value': 1973, 'text': 'FY1973', 'displayOrder': 66, 'name1033': 'FY1973', 'name1049': 'ФГ1973' },
    'FY1972_1972': { 'value': 1972, 'text': 'FY1972', 'displayOrder': 67, 'name1033': 'FY1972', 'name1049': 'ФГ1972' },
    'FY1971_1971': { 'value': 1971, 'text': 'FY1971', 'displayOrder': 68, 'name1033': 'FY1971', 'name1049': 'ФГ1971' },
    'FY1970_1970': { 'value': 1970, 'text': 'FY1970', 'displayOrder': 69, 'name1033': 'FY1970', 'name1049': 'ФГ1970' },
};

// ComponentType:   Attribute (2)            Count: 3
//     AttributeName                         DisplayName    IsCustomizable    Behavior
//     entitlementchannel.channel            Name           False             IncludeSubcomponents
//     entitlementtemplatechannel.channel    Name           False             IncludeSubcomponents
//     incident.caseorigincode               Origin         True              IncludeSubcomponents
GlobalOptionSets.incident_caseorigincodeEnum = {

    'Phone_1': { 'value': 1, 'text': 'Phone', 'displayOrder': 1, 'name1033': 'Phone', 'name1049': 'Телефон' },
    'Email_2': { 'value': 2, 'text': 'Email', 'displayOrder': 2, 'name1033': 'Email', 'name1049': 'Электронная почта' },
    'Web_3': { 'value': 3, 'text': 'Web', 'displayOrder': 3, 'name1033': 'Web', 'name1049': 'Интернет' },
    'Facebook_2483': { 'value': 2483, 'text': 'Facebook', 'displayOrder': 4, 'name1033': 'Facebook' },
    'Twitter_3986': { 'value': 3986, 'text': 'Twitter', 'displayOrder': 5, 'name1033': 'Twitter' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                       DisplayName              IsCustomizable    Behavior
//     lead.initialcommunication           Initial Communication    True              IncludeSubcomponents
//     opportunity.initialcommunication    Initial Communication    True              IncludeSubcomponents
GlobalOptionSets.initialcommunicationEnum = {

    'Contacted_0': { 'value': 0, 'text': 'Contacted', 'displayOrder': 1, 'name1033': 'Contacted', 'name1049': 'Связь установлена' },
    'Not_Contacted_1': { 'value': 1, 'text': 'Not Contacted', 'displayOrder': 2, 'name1033': 'Not Contacted', 'name1049': 'Контактов не было' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                     IsCustomizable    Behavior
//     none          Dialog      Publish Knowledge Article    False             IncludeSubcomponents
GlobalOptionSets.knowledgearticle_expirationstateEnum = {

    'Published_3': { 'value': 3, 'text': 'Published', 'name1033': 'Published', 'name1049': 'Опубликовано' },
    'Expired_4': { 'value': 4, 'text': 'Expired', 'name1033': 'Expired', 'name1049': 'Истекло' },
    'Archived_5': { 'value': 5, 'text': 'Archived', 'name1033': 'Archived', 'name1049': 'Архивировано' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName      DisplayName    IsCustomizable    Behavior
//     lead.salesstage    Sales Stage    True              IncludeSubcomponents
GlobalOptionSets.lead_salesstageEnum = {

    'Qualify_0': { 'value': 0, 'text': 'Qualify', 'displayOrder': 1, 'name1033': 'Qualify', 'name1049': 'Квалифицировать' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName            DisplayName         IsCustomizable    Behavior
//     goal.amountdatatype      Amount Data Type    True              IncludeSubcomponents
//     metric.amountdatatype    Amount Data Type    True              IncludeSubcomponents
GlobalOptionSets.metric_goaltypeEnum = {

    'Money_0': { 'value': 0, 'text': 'Money', 'displayOrder': 1, 'name1033': 'Money', 'name1049': 'Деньги' },
    'Decimal_1': { 'value': 1, 'text': 'Decimal', 'displayOrder': 2, 'name1033': 'Decimal', 'name1049': 'Десятичный' },
    'Integer_2': { 'value': 2, 'text': 'Integer', 'displayOrder': 3, 'name1033': 'Integer', 'name1049': 'Целое число' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName                                      DisplayName    IsCustomizable    Behavior
//     mobileofflineprofileitem.selectedentitytypecode    Entity         False             IncludeSubcomponents
GlobalOptionSets.mobileofflineenabledentitiesEnum = {

    'Account_1': { 'value': 1, 'text': 'Account', 'displayOrder': 1, 'name1033': 'Account', 'name1049': 'Организация' },
    'Contact_2': { 'value': 2, 'text': 'Contact', 'displayOrder': 2, 'name1033': 'Contact', 'name1049': 'Контакт' },
    'Note_5': { 'value': 5, 'text': 'Note', 'displayOrder': 3, 'name1033': 'Note', 'name1049': 'Примечание' },
    'User_8': { 'value': 8, 'text': 'User', 'displayOrder': 4, 'name1033': 'User', 'name1049': 'Пользователь' },
    'Team_9': { 'value': 9, 'text': 'Team', 'displayOrder': 5, 'name1033': 'Team', 'name1049': 'Рабочая группа' },
    'Attachment_1001': { 'value': 1001, 'text': 'Attachment', 'displayOrder': 6, 'name1033': 'Attachment', 'name1049': 'Вложение' },
    'Queue_2020': { 'value': 2020, 'text': 'Queue', 'displayOrder': 7, 'name1033': 'Queue', 'name1049': 'Очередь' },
    'Queue_Item_2029': { 'value': 2029, 'text': 'Queue Item', 'displayOrder': 8, 'name1033': 'Queue Item', 'name1049': 'Элемент очереди' },
    'Appointment_4201': { 'value': 4201, 'text': 'Appointment', 'displayOrder': 9, 'name1033': 'Appointment', 'name1049': 'Встреча' },
    'Email_4202': { 'value': 4202, 'text': 'Email', 'displayOrder': 10, 'name1033': 'Email', 'name1049': 'Электронная почта' },
    'Task_4212': { 'value': 4212, 'text': 'Task', 'displayOrder': 11, 'name1033': 'Task', 'name1049': 'Задача' },
    'SLA_KPI_Instance_9752': { 'value': 9752, 'text': 'SLA KPI Instance', 'displayOrder': 12, 'name1033': 'SLA KPI Instance', 'name1049': 'Экземпляр KPI по SLA' },
    'AccountLeads_16': { 'value': 16, 'text': 'AccountLeads', 'displayOrder': 13, 'name1033': 'AccountLeads' },
    'ContactLeads_22': { 'value': 22, 'text': 'ContactLeads', 'displayOrder': 14, 'name1033': 'ContactLeads' },
    'Lead_4': { 'value': 4, 'text': 'Lead', 'displayOrder': 15, 'name1033': 'Lead' },
    'Product_1024': { 'value': 1024, 'text': 'Product', 'displayOrder': 16, 'name1033': 'Product', 'name1049': 'Продукт' },
    'Entitlement_9700': { 'value': 9700, 'text': 'Entitlement', 'displayOrder': 17, 'name1033': 'Entitlement', 'name1049': 'Объем обслуживания' },
    'Entitlement_Contact_7272': { 'value': 7272, 'text': 'Entitlement Contact', 'displayOrder': 18, 'name1033': 'Entitlement Contact', 'name1049': 'Контакт объема обслуживания' },
    'Entitlement_Product_6363': { 'value': 6363, 'text': 'Entitlement Product', 'displayOrder': 19, 'name1033': 'Entitlement Product', 'name1049': 'Продукт объема обслуживания' },
    'Entitlement_Template_Product_4545': { 'value': 4545, 'text': 'Entitlement Template Product', 'displayOrder': 20, 'name1033': 'Entitlement Template Product', 'name1049': 'Продукт шаблона объема обслуживания' },
    'Case_112': { 'value': 112, 'text': 'Case', 'displayOrder': 21, 'name1033': 'Case', 'name1049': 'Обращение' },
    'Incident_KnowledgeBaseRecord_9931': { 'value': 9931, 'text': 'Incident KnowledgeBaseRecord', 'displayOrder': 22, 'name1033': 'Incident KnowledgeBaseRecord', 'name1049': 'Запись базы знаний инцидентов' },
    'Phone_To_Case_Process_952': { 'value': 952, 'text': 'Phone To Case Process', 'displayOrder': 23, 'name1033': 'Phone To Case Process', 'name1049': 'Преобразование звонка в обращение' },
    'Competitor_Address_1004': { 'value': 1004, 'text': 'Competitor Address', 'displayOrder': 24, 'name1033': 'Competitor Address', 'name1049': 'Адрес конкурента' },
    'Competitor_Product_1006': { 'value': 1006, 'text': 'Competitor Product', 'displayOrder': 25, 'name1033': 'Competitor Product', 'name1049': 'Продукт конкурента' },
    'LeadCompetitors_24': { 'value': 24, 'text': 'LeadCompetitors', 'displayOrder': 26, 'name1033': 'LeadCompetitors' },
    'LeadProduct_27': { 'value': 27, 'text': 'LeadProduct', 'displayOrder': 27, 'name1033': 'LeadProduct' },
    'Lead_To_Opportunity_Sales_Process_954': { 'value': 954, 'text': 'Lead To Opportunity Sales Process', 'displayOrder': 28, 'name1033': 'Lead To Opportunity Sales Process', 'name1049': 'Преобразование интереса в возможную сделку' },
    'Opportunity_3': { 'value': 3, 'text': 'Opportunity', 'displayOrder': 29, 'name1033': 'Opportunity' },
    'OpportunityCompetitors_25': { 'value': 25, 'text': 'OpportunityCompetitors', 'displayOrder': 30, 'name1033': 'OpportunityCompetitors' },
    'Opportunity_Product_1083': { 'value': 1083, 'text': 'Opportunity Product', 'displayOrder': 31, 'name1033': 'Opportunity Product' },
    'Opportunity_Sales_Process_953': { 'value': 953, 'text': 'Opportunity Sales Process', 'displayOrder': 32, 'name1033': 'Opportunity Sales Process', 'name1049': 'Преобразование возможной сделки в продажу' },
    'Competitor_123': { 'value': 123, 'text': 'Competitor', 'displayOrder': 33, 'name1033': 'Competitor', 'name1049': 'Конкурент' },
};

GlobalOptionSets.msdyn_bookableresourcetypeEnum = {

    'Generic_1': { 'value': 1, 'text': 'Generic', 'name1033': 'Generic', 'name1049': 'Универсальный' },
    'Contact_2': { 'value': 2, 'text': 'Contact', 'name1033': 'Contact', 'name1049': 'Контакт' },
    'User_3': { 'value': 3, 'text': 'User', 'name1033': 'User', 'name1049': 'Пользователь' },
    'Equipment_4': { 'value': 4, 'text': 'Equipment', 'name1033': 'Equipment', 'name1049': 'Оборудование' },
    'Account_5': { 'value': 5, 'text': 'Account', 'name1033': 'Account', 'name1049': 'Организация' },
    'Crew_6': { 'value': 6, 'text': 'Crew', 'name1033': 'Crew', 'name1049': 'Команда' },
    'Facility_7': { 'value': 7, 'text': 'Facility', 'name1033': 'Facility', 'name1049': 'Помещение' },
    'Pool_8': { 'value': 8, 'text': 'Pool', 'name1033': 'Pool', 'name1049': 'Пул' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName       DisplayName    IsCustomizable    Behavior
//     lead.need           Need           True              IncludeSubcomponents
//     opportunity.need    Need           True              IncludeSubcomponents
GlobalOptionSets.needEnum = {

    'Must_have_0': { 'value': 0, 'text': 'Must have', 'displayOrder': 1, 'name1033': 'Must have', 'name1049': 'Обязан' },
    'Should_have_1': { 'value': 1, 'text': 'Should have', 'displayOrder': 2, 'name1033': 'Should have', 'name1049': 'Должен' },
    'Good_to_have_2': { 'value': 2, 'text': 'Good to have', 'displayOrder': 3, 'name1033': 'Good to have', 'name1049': 'Хорошо' },
    'No_need_3': { 'value': 3, 'text': 'No need', 'displayOrder': 4, 'name1033': 'No need', 'name1049': 'Не требуется' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName             DisplayName    IsCustomizable    Behavior
//     opportunity.salesstage    Sales Stage    True              IncludeSubcomponents
GlobalOptionSets.opportunity_salesstageEnum = {

    'Qualify_0': { 'value': 0, 'text': 'Qualify', 'displayOrder': 1, 'name1033': 'Qualify', 'name1049': 'Квалифицировать' },
    'Develop_1': { 'value': 1, 'text': 'Develop', 'displayOrder': 2, 'name1033': 'Develop', 'name1049': 'Развить' },
    'Propose_2': { 'value': 2, 'text': 'Propose', 'displayOrder': 3, 'name1033': 'Propose', 'name1049': 'Предложить' },
    'Close_3': { 'value': 3, 'text': 'Close', 'displayOrder': 4, 'name1033': 'Close', 'name1049': 'Закрыть' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName                             DisplayName    IsCustomizable    Behavior
//     savedorginsightsconfiguration.lookback    Lookback       False             IncludeSubcomponents
GlobalOptionSets.orginsightsconfiguration_lookbackEnum = {

    'V_2H_1': { 'value': 1, 'text': '2H', 'displayOrder': 1, 'name1033': '2H', 'name1049': '2 ч' },
    'V_48H_2': { 'value': 2, 'text': '48H', 'displayOrder': 2, 'name1033': '48H', 'name1049': '48 ч' },
    'V_7D_3': { 'value': 3, 'text': '7D', 'displayOrder': 3, 'name1033': '7D', 'name1049': '7 д' },
    'V_30D_4': { 'value': 4, 'text': '30D', 'displayOrder': 4, 'name1033': '30D', 'name1049': '30 д' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName                               DisplayName    IsCustomizable    Behavior
//     savedorginsightsconfiguration.plotoption    Plot Option    False             IncludeSubcomponents
GlobalOptionSets.orginsightsconfiguration_plotoptionEnum = {

    'Line_1': { 'value': 1, 'text': 'Line', 'displayOrder': 1, 'name1033': 'Line', 'name1049': 'Линейная' },
    'Column_2': { 'value': 2, 'text': 'Column', 'displayOrder': 2, 'name1033': 'Column', 'name1049': 'Гистограмма' },
    'Area_3': { 'value': 3, 'text': 'Area', 'displayOrder': 3, 'name1033': 'Area', 'name1049': 'С областями' },
    'Pie_4': { 'value': 4, 'text': 'Pie', 'displayOrder': 4, 'name1033': 'Pie', 'name1049': 'Круговая' },
    'Bar_5': { 'value': 5, 'text': 'Bar', 'displayOrder': 5, 'name1033': 'Bar', 'name1049': 'Линейчатая' },
    'Donut_6': { 'value': 6, 'text': 'Donut', 'displayOrder': 6, 'name1033': 'Donut', 'name1049': 'Круговая' },
    'Infocard_7': { 'value': 7, 'text': 'Infocard', 'displayOrder': 7, 'name1033': 'Infocard' },
    'List_8': { 'value': 8, 'text': 'List', 'displayOrder': 8, 'name1033': 'List', 'name1049': 'Список' },
    'DoubleDonut_9': { 'value': 9, 'text': 'DoubleDonut', 'displayOrder': 9, 'name1033': 'DoubleDonut', 'name1049': 'Двойная круговая' },
    'LinearGauge_10': { 'value': 10, 'text': 'LinearGauge', 'displayOrder': 10, 'name1033': 'LinearGauge', 'name1049': 'Линейная измерительная' },
    'Bubble_11': { 'value': 11, 'text': 'Bubble', 'displayOrder': 11, 'name1033': 'Bubble', 'name1049': 'Пузырьковая' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName                  IsCustomizable    Behavior
//     none          Dialog      Device Settings Dialog    False             IncludeSubcomponents
GlobalOptionSets.photo_resolutionEnum = {

    'Device_Default_0': { 'value': 0, 'text': 'Device Default', 'name1033': 'Device Default', 'name1049': 'По умолчанию для устройства' },
    'V_640_x_480_1': { 'value': 1, 'text': '640 x 480', 'name1033': '640 x 480' },
    'V_1024_x_768_2': { 'value': 2, 'text': '1024 x 768', 'name1033': '1024 x 768' },
    'V_1600_x_1200_3': { 'value': 3, 'text': '1600 x 1200', 'name1033': '1600 x 1200' },
    'V_2048_x_1536_4': { 'value': 4, 'text': '2048 x 1536', 'name1033': '2048 x 1536' },
    'V_2592_x_1936_5': { 'value': 5, 'text': '2592 x 1936', 'name1033': '2592 x 1936' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                                     DisplayName       IsCustomizable    Behavior
//     principalsyncattributemap.defaultsyncdirection    Sync Direction    False             IncludeSubcomponents
//     principalsyncattributemap.syncdirection           Sync Direction    False             IncludeSubcomponents
GlobalOptionSets.principalsyncattributemapping_syncdirectionEnum = {

    'None_0': { 'value': 0, 'text': 'None', 'displayOrder': 1, 'name1033': 'None', 'name1049': 'Нет' },
    'ToExchange_1': { 'value': 1, 'text': 'ToExchange', 'displayOrder': 2, 'name1033': 'ToExchange' },
    'ToCRM_2': { 'value': 2, 'text': 'ToCRM', 'displayOrder': 3, 'name1033': 'ToCRM' },
    'Bidirectional_3': { 'value': 3, 'text': 'Bidirectional', 'displayOrder': 4, 'name1033': 'Bidirectional', 'name1049': 'Двунаправленный' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName                 DisplayName       IsCustomizable    Behavior
//     processstage.stagecategory    Stage Category    True              IncludeSubcomponents
// ComponentType:   Workflow (29)            Count: 10
//     Entity              Category                 Name                                                                                 Type             Scope           Mode          StatusCode    IsCustomizable    Behavior
//     appointment         Business Process Flow    After Meeting    (UniqueName "after_meeting")                                        Task Flow        Organization    Background    Activated     True              IncludeSubcomponents
//     contact             Business Process Flow    Update Contact    (UniqueName "update_contact")                                      Task Flow        Organization    Background    Activated     True              IncludeSubcomponents
//     gbc_entity_test     Business Process Flow    Test    (UniqueName "gbc_test")                                                      Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
//     incident            Business Process Flow    Phone to Case Process    (UniqueName "phonetocaseprocess")                           Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
//     knowledgearticle    Business Process Flow    Expired Process    (UniqueName "expiredprocess")                                     Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
//     knowledgearticle    Business Process Flow    New Process    (UniqueName "newprocess")                                             Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
//     knowledgearticle    Business Process Flow    Translation Process    (UniqueName "translationprocess")                             Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
//     lead                Business Process Flow    Lead to Opportunity Sales Process    (UniqueName "leadtoopportunitysalesprocess")    Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
//     opportunity         Business Process Flow    Follow up with Opportunity    (UniqueName "make_contact_on_opportunity")             Task Flow        Organization    Background    Activated     True              IncludeSubcomponents
//     opportunity         Business Process Flow    Opportunity Sales Process    (UniqueName "opportunitysalesprocess")                  Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
GlobalOptionSets.processstage_categoryEnum = {

    'Qualify_0': { 'value': 0, 'text': 'Qualify', 'displayOrder': 1, 'name1033': 'Qualify', 'name1049': 'Квалифицировать' },
    'Develop_1': { 'value': 1, 'text': 'Develop', 'displayOrder': 2, 'name1033': 'Develop', 'name1049': 'Развить' },
    'Propose_2': { 'value': 2, 'text': 'Propose', 'displayOrder': 3, 'name1033': 'Propose', 'name1049': 'Предложить' },
    'Close_3': { 'value': 3, 'text': 'Close', 'displayOrder': 4, 'name1033': 'Close', 'name1049': 'Закрыть' },
    'Identify_4': { 'value': 4, 'text': 'Identify', 'displayOrder': 5, 'name1033': 'Identify', 'name1049': 'Определить' },
    'Research_5': { 'value': 5, 'text': 'Research', 'displayOrder': 6, 'name1033': 'Research', 'name1049': 'Исследование' },
    'Resolve_6': { 'value': 6, 'text': 'Resolve', 'displayOrder': 7, 'name1033': 'Resolve', 'name1049': 'Разрешить' },
    'Approval_7': { 'value': 7, 'text': 'Approval', 'displayOrder': 8, 'name1033': 'Approval', 'name1049': 'Утверждение' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                  DisplayName         IsCustomizable    Behavior
//     lead.purchaseprocess           Purchase Process    True              IncludeSubcomponents
//     opportunity.purchaseprocess    Purchase Process    True              IncludeSubcomponents
GlobalOptionSets.purchaseprocessEnum = {

    'Individual_0': { 'value': 0, 'text': 'Individual', 'displayOrder': 1, 'name1033': 'Individual', 'name1049': 'Отдельное лицо' },
    'Committee_1': { 'value': 1, 'text': 'Committee', 'displayOrder': 2, 'name1033': 'Committee', 'name1049': 'Комитет' },
    'Unknown_2': { 'value': 2, 'text': 'Unknown', 'displayOrder': 3, 'name1033': 'Unknown', 'name1049': 'Неизвестно' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                    DisplayName           IsCustomizable    Behavior
//     lead.purchasetimeframe           Purchase Timeframe    True              IncludeSubcomponents
//     opportunity.purchasetimeframe    Purchase Timeframe    True              IncludeSubcomponents
GlobalOptionSets.purchasetimeframeEnum = {

    'Immediate_0': { 'value': 0, 'text': 'Immediate', 'displayOrder': 1, 'name1033': 'Immediate', 'name1049': 'Немедленно' },
    'This_Quarter_1': { 'value': 1, 'text': 'This Quarter', 'displayOrder': 2, 'name1033': 'This Quarter', 'name1049': 'Этот квартал' },
    'Next_Quarter_2': { 'value': 2, 'text': 'Next Quarter', 'displayOrder': 3, 'name1033': 'Next Quarter', 'name1049': 'Следующий квартал' },
    'This_Year_3': { 'value': 3, 'text': 'This Year', 'displayOrder': 4, 'name1033': 'This Year', 'name1049': 'Этот год' },
    'Unknown_4': { 'value': 4, 'text': 'Unknown', 'displayOrder': 5, 'name1033': 'Unknown', 'name1049': 'Неизвестно' },
};

// ComponentType:   Attribute (2)            Count: 8
//     AttributeName                          DisplayName       IsCustomizable    Behavior
//     invoice.pricingerrorcode               Pricing Error     True              IncludeSubcomponents
//     invoicedetail.pricingerrorcode         Pricing Error     True              IncludeSubcomponents
//     opportunity.pricingerrorcode           Pricing Error     True              IncludeSubcomponents
//     opportunityproduct.pricingerrorcode    Pricing Error     True              IncludeSubcomponents
//     quote.pricingerrorcode                 Pricing Error     True              IncludeSubcomponents
//     quotedetail.pricingerrorcode           Pricing Error     True              IncludeSubcomponents
//     salesorder.pricingerrorcode            Pricing Error     True              IncludeSubcomponents
//     salesorderdetail.pricingerrorcode      Pricing Error     True              IncludeSubcomponents
GlobalOptionSets.qooi_pricingerrorcodeEnum = {

    'None_0': { 'value': 0, 'text': 'None', 'displayOrder': 1, 'name1033': 'None', 'name1049': 'Нет' },
    'Detail_Error_1': { 'value': 1, 'text': 'Detail Error', 'displayOrder': 2, 'name1033': 'Detail Error', 'name1049': 'Сведения об ошибке' },
    'Missing_Price_Level_2': { 'value': 2, 'text': 'Missing Price Level', 'displayOrder': 3, 'name1033': 'Missing Price Level', 'name1049': 'Отсутствует уровень цены' },
    'Inactive_Price_Level_3': { 'value': 3, 'text': 'Inactive Price Level', 'displayOrder': 4, 'name1033': 'Inactive Price Level', 'name1049': 'Неактивный уровень цен' },
    'Missing_Quantity_4': { 'value': 4, 'text': 'Missing Quantity', 'displayOrder': 5, 'name1033': 'Missing Quantity', 'name1049': 'Отсутствует количество' },
    'Missing_Unit_Price_5': { 'value': 5, 'text': 'Missing Unit Price', 'displayOrder': 6, 'name1033': 'Missing Unit Price', 'name1049': 'Отсутствует цена за единицу' },
    'Missing_Product_6': { 'value': 6, 'text': 'Missing Product', 'displayOrder': 7, 'name1033': 'Missing Product', 'name1049': 'Отсутствует продукт' },
    'Invalid_Product_7': { 'value': 7, 'text': 'Invalid Product', 'displayOrder': 8, 'name1033': 'Invalid Product', 'name1049': 'Недопустимый продукт' },
    'Missing_Pricing_Code_8': { 'value': 8, 'text': 'Missing Pricing Code', 'displayOrder': 9, 'name1033': 'Missing Pricing Code', 'name1049': 'Отсутствует код ценообразования' },
    'Invalid_Pricing_Code_9': { 'value': 9, 'text': 'Invalid Pricing Code', 'displayOrder': 10, 'name1033': 'Invalid Pricing Code', 'name1049': 'Недопустимый код ценообразования' },
    'Missing_UOM_10': { 'value': 10, 'text': 'Missing UOM', 'displayOrder': 11, 'name1033': 'Missing UOM', 'name1049': 'Отсутствует единица измерения' },
    'Product_Not_In_Price_Level_11': { 'value': 11, 'text': 'Product Not In Price Level', 'displayOrder': 12, 'name1033': 'Product Not In Price Level', 'name1049': 'Продукт отсутствует в уровне цены' },
    'Missing_Price_Level_Amount_12': { 'value': 12, 'text': 'Missing Price Level Amount', 'displayOrder': 13, 'name1033': 'Missing Price Level Amount', 'name1049': 'Отсутствует объем уровня цены' },
    'Missing_Price_Level_Percentage_13': { 'value': 13, 'text': 'Missing Price Level Percentage', 'displayOrder': 14, 'name1033': 'Missing Price Level Percentage', 'name1049': 'Отсутствует процент уровня цены' },
    'Missing_Price_14': { 'value': 14, 'text': 'Missing Price', 'displayOrder': 15, 'name1033': 'Missing Price', 'name1049': 'Отсутствует цена' },
    'Missing_Current_Cost_15': { 'value': 15, 'text': 'Missing Current Cost', 'displayOrder': 16, 'name1033': 'Missing Current Cost', 'name1049': 'Отсутствует текущая стоимость' },
    'Missing_Standard_Cost_16': { 'value': 16, 'text': 'Missing Standard Cost', 'displayOrder': 17, 'name1033': 'Missing Standard Cost', 'name1049': 'Отсутствует нормативная стоимость' },
    'Invalid_Price_Level_Amount_17': { 'value': 17, 'text': 'Invalid Price Level Amount', 'displayOrder': 18, 'name1033': 'Invalid Price Level Amount', 'name1049': 'Недопустимый объем уровня цены' },
    'Invalid_Price_Level_Percentage_18': { 'value': 18, 'text': 'Invalid Price Level Percentage', 'displayOrder': 19, 'name1033': 'Invalid Price Level Percentage', 'name1049': 'Недопустимый процент уровня цены' },
    'Invalid_Price_19': { 'value': 19, 'text': 'Invalid Price', 'displayOrder': 20, 'name1033': 'Invalid Price', 'name1049': 'Недопустимая цена' },
    'Invalid_Current_Cost_20': { 'value': 20, 'text': 'Invalid Current Cost', 'displayOrder': 21, 'name1033': 'Invalid Current Cost', 'name1049': 'Недопустимая текущая стоимость' },
    'Invalid_Standard_Cost_21': { 'value': 21, 'text': 'Invalid Standard Cost', 'displayOrder': 22, 'name1033': 'Invalid Standard Cost', 'name1049': 'Недопустимая нормативная стоимость' },
    'Invalid_Rounding_Policy_22': { 'value': 22, 'text': 'Invalid Rounding Policy', 'displayOrder': 23, 'name1033': 'Invalid Rounding Policy', 'name1049': 'Недопустимое правило округления' },
    'Invalid_Rounding_Option_23': { 'value': 23, 'text': 'Invalid Rounding Option', 'displayOrder': 24, 'name1033': 'Invalid Rounding Option', 'name1049': 'Недопустимый тип округления' },
    'Invalid_Rounding_Amount_24': { 'value': 24, 'text': 'Invalid Rounding Amount', 'displayOrder': 25, 'name1033': 'Invalid Rounding Amount', 'name1049': 'Недопустимая величина округления' },
    'Price_Calculation_Error_25': { 'value': 25, 'text': 'Price Calculation Error', 'displayOrder': 26, 'name1033': 'Price Calculation Error', 'name1049': 'Ошибка расчета цены' },
    'Invalid_Discount_Type_26': { 'value': 26, 'text': 'Invalid Discount Type', 'displayOrder': 27, 'name1033': 'Invalid Discount Type', 'name1049': 'Недопустимый тип скидки' },
    'Discount_Type_Invalid_State_27': { 'value': 27, 'text': 'Discount Type Invalid State', 'displayOrder': 28, 'name1033': 'Discount Type Invalid State', 'name1049': 'Недопустимое состояние типа скидки' },
    'Invalid_Discount_28': { 'value': 28, 'text': 'Invalid Discount', 'displayOrder': 29, 'name1033': 'Invalid Discount', 'name1049': 'Недопустимая скидка' },
    'Invalid_Quantity_29': { 'value': 29, 'text': 'Invalid Quantity', 'displayOrder': 30, 'name1033': 'Invalid Quantity', 'name1049': 'Недопустимое количество' },
    'Invalid_Pricing_Precision_30': { 'value': 30, 'text': 'Invalid Pricing Precision', 'displayOrder': 31, 'name1033': 'Invalid Pricing Precision', 'name1049': 'Недопустимая точность ценообразования' },
    'Missing_Product_Default_UOM_31': { 'value': 31, 'text': 'Missing Product Default UOM', 'displayOrder': 32, 'name1033': 'Missing Product Default UOM', 'name1049': 'Отсутствует единица измерения продукта по умолчанию' },
    'Missing_Product_UOM_Schedule_32': { 'value': 32, 'text': 'Missing Product UOM Schedule ', 'displayOrder': 33, 'name1033': 'Missing Product UOM Schedule ', 'name1049': 'Отсутствует перечень единиц измерения продукта ' },
    'Inactive_Discount_Type_33': { 'value': 33, 'text': 'Inactive Discount Type', 'displayOrder': 34, 'name1033': 'Inactive Discount Type', 'name1049': 'Неактивный тип скидки' },
    'Invalid_Price_Level_Currency_34': { 'value': 34, 'text': 'Invalid Price Level Currency', 'displayOrder': 35, 'name1033': 'Invalid Price Level Currency', 'name1049': 'Недопустимая валюта уровня цены' },
    'Price_Attribute_Out_Of_Range_35': { 'value': 35, 'text': 'Price Attribute Out Of Range', 'displayOrder': 36, 'name1033': 'Price Attribute Out Of Range', 'name1049': 'Значение атрибута "Цена" выходит за границы допустимого диапазона' },
    'Base_Currency_Attribute_Overflow_36': { 'value': 36, 'text': 'Base Currency Attribute Overflow', 'displayOrder': 37, 'name1033': 'Base Currency Attribute Overflow', 'name1049': 'Переполнение атрибута "Базовая валюта"' },
    'Base_Currency_Attribute_Underflow_37': { 'value': 37, 'text': 'Base Currency Attribute Underflow', 'displayOrder': 38, 'name1033': 'Base Currency Attribute Underflow', 'name1049': 'Недостаточное заполнение атрибута "Базовая валюта"' },
};

// ComponentType:   Attribute (2)            Count: 4
//     AttributeName                         DisplayName     IsCustomizable    Behavior
//     invoicedetail.producttypecode         Product type    True              IncludeSubcomponents
//     opportunityproduct.producttypecode    Product type    True              IncludeSubcomponents
//     quotedetail.producttypecode           Product type    True              IncludeSubcomponents
//     salesorderdetail.producttypecode      Product type    True              IncludeSubcomponents
GlobalOptionSets.qooiproduct_producttypeEnum = {

    'Product_1': { 'value': 1, 'text': 'Product', 'displayOrder': 1, 'name1033': 'Product', 'name1049': 'Продукт' },
    'Bundle_2': { 'value': 2, 'text': 'Bundle', 'displayOrder': 2, 'name1033': 'Bundle', 'name1049': 'Набор' },
    'Required_Bundle_Product_3': { 'value': 3, 'text': 'Required Bundle Product', 'displayOrder': 3, 'name1033': 'Required Bundle Product', 'name1049': 'Обязательный продукт набора' },
    'Optional_Bundle_Product_4': { 'value': 4, 'text': 'Optional Bundle Product', 'displayOrder': 4, 'name1033': 'Optional Bundle Product', 'name1049': 'Дополнительный продукт набора' },
    'Project_based_Service_5': { 'value': 5, 'text': 'Project-based Service', 'displayOrder': 5, 'name1033': 'Project-based Service', 'name1049': 'Служба на основе проекта' },
};

// ComponentType:   Attribute (2)            Count: 4
//     AttributeName                                     DisplayName               IsCustomizable    Behavior
//     invoicedetail.propertyconfigurationstatus         Property Configuration    True              IncludeSubcomponents
//     opportunityproduct.propertyconfigurationstatus    Property Configuration    True              IncludeSubcomponents
//     quotedetail.propertyconfigurationstatus           Property Configuration    True              IncludeSubcomponents
//     salesorderdetail.propertyconfigurationstatus      Property Configuration    True              IncludeSubcomponents
GlobalOptionSets.qooiproduct_propertiesconfigurationstatusEnum = {

    'Edit_0': { 'value': 0, 'text': 'Edit', 'displayOrder': 1, 'name1033': 'Edit', 'name1049': 'Изменить' },
    'Rectify_1': { 'value': 1, 'text': 'Rectify', 'displayOrder': 2, 'name1033': 'Rectify', 'name1049': 'Уточнить' },
    'Not_Configured_2': { 'value': 2, 'text': 'Not Configured', 'displayOrder': 3, 'name1033': 'Not Configured', 'name1049': 'Не настроено' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                             DisplayName      IsCustomizable    Behavior
//     recurrencerule.monthofyear                Month Of Year    False             IncludeSubcomponents
//     recurringappointmentmaster.monthofyear    Month Of Year    True              IncludeSubcomponents
GlobalOptionSets.recurrencerule_monthofyearEnum = {

    'Invalid_Month_Of_Year_0': { 'value': 0, 'text': 'Invalid Month Of Year', 'displayOrder': 1, 'name1033': 'Invalid Month Of Year', 'name1049': 'Недействительный месяц года' },
    'January_1': { 'value': 1, 'text': 'January', 'displayOrder': 2, 'name1033': 'January', 'name1049': 'Январь' },
    'February_2': { 'value': 2, 'text': 'February', 'displayOrder': 3, 'name1033': 'February', 'name1049': 'Февраль' },
    'March_3': { 'value': 3, 'text': 'March', 'displayOrder': 4, 'name1033': 'March', 'name1049': 'Март' },
    'April_4': { 'value': 4, 'text': 'April', 'displayOrder': 5, 'name1033': 'April', 'name1049': 'Апрель' },
    'May_5': { 'value': 5, 'text': 'May', 'displayOrder': 6, 'name1033': 'May', 'name1049': 'Май' },
    'June_6': { 'value': 6, 'text': 'June', 'displayOrder': 7, 'name1033': 'June', 'name1049': 'Июнь' },
    'July_7': { 'value': 7, 'text': 'July', 'displayOrder': 8, 'name1033': 'July', 'name1049': 'Июль' },
    'August_8': { 'value': 8, 'text': 'August', 'displayOrder': 9, 'name1033': 'August', 'name1049': 'Август' },
    'September_9': { 'value': 9, 'text': 'September', 'displayOrder': 10, 'name1033': 'September', 'name1049': 'Сентябрь' },
    'October_10': { 'value': 10, 'text': 'October', 'displayOrder': 11, 'name1033': 'October', 'name1049': 'Октябрь' },
    'November_11': { 'value': 11, 'text': 'November', 'displayOrder': 12, 'name1033': 'November', 'name1049': 'Ноябрь' },
    'December_12': { 'value': 12, 'text': 'December', 'displayOrder': 13, 'name1033': 'December', 'name1049': 'Декабрь' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName            DisplayName      IsCustomizable    Behavior
//     incident.servicestage    Service Stage    True              IncludeSubcomponents
GlobalOptionSets.servicestageEnum = {

    'Identify_0': { 'value': 0, 'text': 'Identify', 'displayOrder': 1, 'name1033': 'Identify', 'name1049': 'Определить' },
    'Research_1': { 'value': 1, 'text': 'Research', 'displayOrder': 2, 'name1033': 'Research', 'name1049': 'Исследование' },
    'Resolve_2': { 'value': 2, 'text': 'Resolve', 'displayOrder': 3, 'name1033': 'Resolve', 'name1049': 'Разрешить' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName                      DisplayName               IsCustomizable    Behavior
//     sharepointsite.validationstatus    Last Validation Status    True              IncludeSubcomponents
GlobalOptionSets.sharepoint_validationstatusEnum = {

    'Not_Validated_1': { 'value': 1, 'text': 'Not Validated', 'displayOrder': 1, 'name1033': 'Not Validated', 'name1049': 'Не проверен' },
    'In_Progress_2': { 'value': 2, 'text': 'In Progress', 'displayOrder': 2, 'name1033': 'In Progress', 'name1049': 'Выполняется' },
    'Invalid_3': { 'value': 3, 'text': 'Invalid', 'displayOrder': 3, 'name1033': 'Invalid', 'name1049': 'Проверен' },
    'Valid_4': { 'value': 4, 'text': 'Valid', 'displayOrder': 4, 'name1033': 'Valid', 'name1049': 'Действителен' },
    'Could_not_validate_5': { 'value': 5, 'text': 'Could not validate', 'displayOrder': 5, 'name1033': 'Could not validate', 'name1049': 'Не удалось проверить' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName                               DisplayName               IsCustomizable    Behavior
//     sharepointsite.validationstatuserrorcode    Additional Information    True              IncludeSubcomponents
GlobalOptionSets.sharepoint_validationstatusreasonEnum = {

    'This_record_s_URL_has_not_been_validated_1': { 'value': 1, 'text': 'This record\'s URL has not been validated.', 'displayOrder': 1, 'name1033': 'This record\'s URL has not been validated.', 'name1049': 'Этот URL-адрес записи не был проверен.' },
    'This_record_s_URL_is_valid_2': { 'value': 2, 'text': 'This record\'s URL is valid.', 'displayOrder': 2, 'name1033': 'This record\'s URL is valid.', 'name1049': 'Этот URL-адрес записи действителен.' },
    'This_record_s_URL_is_not_valid_3': { 'value': 3, 'text': 'This record\'s URL is not valid.', 'displayOrder': 3, 'name1033': 'This record\'s URL is not valid.', 'name1049': 'Этот URL-адрес записи недействителен.' },
    'The_URL_schemes_of_Microsoft_Dynamics_365_and_SharePoint_are_different_4': { 'value': 4, 'text': 'The URL schemes of Microsoft Dynamics 365 and SharePoint are different.', 'displayOrder': 4, 'name1033': 'The URL schemes of Microsoft Dynamics 365 and SharePoint are different.', 'name1049': 'Схемы URL-адресов Microsoft Dynamics 365 и SharePoint отличаются.' },
    'The_URL_could_not_be_accessed_because_of_Internet_Explorer_security_settings_5': { 'value': 5, 'text': 'The URL could not be accessed because of Internet Explorer security settings.', 'displayOrder': 5, 'name1033': 'The URL could not be accessed because of Internet Explorer security settings.', 'name1049': 'Не удалось получить URL-адрес из-за параметров безопасности Internet Explorer.' },
    'Authentication_failure_6': { 'value': 6, 'text': 'Authentication failure.', 'displayOrder': 6, 'name1033': 'Authentication failure.', 'name1049': 'Ошибка при проверке подлинности.' },
    'Invalid_certificates_7': { 'value': 7, 'text': 'Invalid certificates.', 'displayOrder': 7, 'name1033': 'Invalid certificates.', 'name1049': 'Недопустимые сертификаты.' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                              DisplayName               IsCustomizable    Behavior
//     sharepointdocument.documentlocationtype    Document Location Type    True              IncludeSubcomponents
//     sharepointdocumentlocation.locationtype    Location Type             True              IncludeSubcomponents
GlobalOptionSets.sharepointdocumentlocation_locationtypeEnum = {

    'General_0': { 'value': 0, 'text': 'General', 'displayOrder': 1, 'name1033': 'General', 'name1049': 'Общее' },
    'Dedicated_for_OneNote_Integration_1': { 'value': 1, 'text': 'Dedicated for OneNote Integration', 'displayOrder': 2, 'name1033': 'Dedicated for OneNote Integration', 'name1049': 'Выделенное для интеграции с OneNote' },
};

// ComponentType:   Attribute (2)            Count: 3
//     AttributeName                             DisplayName          IsCustomizable    Behavior
//     sharepointdocument.servicetype            Document Location    False             IncludeSubcomponents
//     sharepointdocumentlocation.servicetype    Service Type         False             IncludeSubcomponents
//     sharepointsite.servicetype                Service Type         False             IncludeSubcomponents
GlobalOptionSets.sharepointsite_servicetypeEnum = {

    'SharePoint_0': { 'value': 0, 'text': 'SharePoint', 'displayOrder': 1, 'name1033': 'SharePoint' },
    'OneDrive_1': { 'value': 1, 'text': 'OneDrive', 'displayOrder': 2, 'name1033': 'OneDrive' },
    'Shared_with_me_2': { 'value': 2, 'text': 'Shared with me', 'displayOrder': 3, 'name1033': 'Shared with me', 'name1049': 'Мне предоставлен доступ' },
};

// ComponentType:   SystemForm (60)            Count: 1
//     EntityName    FormType    FormName           IsCustomizable    Behavior
//     none          Dialog      CreateSlaDialog    False             IncludeSubcomponents
GlobalOptionSets.sla_slaenabledentitiesEnum = {

    'Case_112': { 'value': 112, 'text': 'Case', 'name1033': 'Case', 'name1049': 'Обращение' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                     DisplayName    IsCustomizable    Behavior
//     incident.messagetypecode          Received As    True              IncludeSubcomponents
//     socialactivity.postmessagetype    Received As    True              IncludeSubcomponents
GlobalOptionSets.socialactivity_postmessagetypeEnum = {

    'Public_Message_0': { 'value': 0, 'text': 'Public Message', 'displayOrder': 1, 'name1033': 'Public Message', 'name1049': 'Общедоступное сообщение' },
    'Private_Message_1': { 'value': 1, 'text': 'Private Message', 'displayOrder': 2, 'name1033': 'Private Message', 'name1049': 'Личное сообщение' },
};

// ComponentType:   Attribute (2)            Count: 11
//     AttributeName                   DisplayName       IsCustomizable    Behavior
//     activitypointer.community       Social Channel    True              IncludeSubcomponents
//     bulkoperation.community         Social Channel    True              IncludeSubcomponents
//     campaignactivity.community      Social Channel    True              IncludeSubcomponents
//     campaignresponse.community      Social Channel    True              IncludeSubcomponents
//     incidentresolution.community    Social Channel    True              IncludeSubcomponents
//     opportunityclose.community      Social Channel    True              IncludeSubcomponents
//     orderclose.community            Social Channel    True              IncludeSubcomponents
//     quoteclose.community            Social Channel    True              IncludeSubcomponents
//     serviceappointment.community    Social Channel    True              IncludeSubcomponents
//     socialactivity.community        Social Channel    True              IncludeSubcomponents
//     socialprofile.community         Social Channel    True              IncludeSubcomponents
GlobalOptionSets.socialprofile_communityEnum = {

    'Facebook_1': { 'value': 1, 'text': 'Facebook', 'displayOrder': 1, 'name1033': 'Facebook' },
    'Twitter_2': { 'value': 2, 'text': 'Twitter', 'displayOrder': 2, 'name1033': 'Twitter' },
    'Other_0': { 'value': 0, 'text': 'Other', 'displayOrder': 3, 'name1033': 'Other', 'name1049': 'Прочее' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                                DisplayName       IsCustomizable    Behavior
//     syncattributemapping.defaultsyncdirection    Sync Direction    False             IncludeSubcomponents
//     syncattributemapping.syncdirection           Sync Direction    False             IncludeSubcomponents
GlobalOptionSets.syncattributemapping_syncdirectionEnum = {

    'None_0': { 'value': 0, 'text': 'None', 'displayOrder': 1, 'name1033': 'None', 'name1049': 'Нет' },
    'ToExchange_1': { 'value': 1, 'text': 'ToExchange', 'displayOrder': 2, 'name1033': 'ToExchange' },
    'ToCRM_2': { 'value': 2, 'text': 'ToCRM', 'displayOrder': 3, 'name1033': 'ToCRM' },
    'Bidirectional_3': { 'value': 3, 'text': 'Bidirectional', 'displayOrder': 4, 'name1033': 'Bidirectional', 'name1049': 'Двунаправленный' },
};

// ComponentType:   Attribute (2)            Count: 1
//     AttributeName     DisplayName    IsCustomizable    Behavior
//     workflow.runas    Run As User    True              IncludeSubcomponents
GlobalOptionSets.workflow_runasEnum = {

    'Owner_0': { 'value': 0, 'text': 'Owner', 'displayOrder': 1, 'name1033': 'Owner', 'name1049': 'Ответственный' },
    'Calling_User_1': { 'value': 1, 'text': 'Calling User', 'displayOrder': 2, 'name1033': 'Calling User', 'name1049': 'Вызывающий пользователь' },
};

// ComponentType:   Attribute (2)            Count: 3
//     AttributeName           DisplayName     IsCustomizable    Behavior
//     workflow.createstage    Create Stage    True              IncludeSubcomponents
//     workflow.deletestage    Delete stage    True              IncludeSubcomponents
//     workflow.updatestage    Update Stage    True              IncludeSubcomponents
GlobalOptionSets.workflow_stageEnum = {

    'Pre_operation_20': { 'value': 20, 'text': 'Pre-operation', 'displayOrder': 1, 'name1033': 'Pre-operation', 'name1049': 'Перед основной операцией внутри транзакции' },
    'Post_operation_40': { 'value': 40, 'text': 'Post-operation', 'displayOrder': 2, 'name1033': 'Post-operation', 'name1049': 'После основной операции внутри транзакции' },
};

// ComponentType:   Attribute (2)            Count: 2
//     AttributeName                                      DisplayName    IsCustomizable    Behavior
//     workflowlog.childworkflowinstanceobjecttypecode    Entity         False             IncludeSubcomponents
//     workflowlog.objecttypecode                         Entity         False             IncludeSubcomponents
GlobalOptionSets.workflowlog_objecttypecodeEnum = {

    'System_Job_4700': { 'value': 4700, 'text': 'System Job', 'displayOrder': 1, 'name1033': 'System Job', 'name1049': 'Системное задание' },
    'Workflow_Session_4710': { 'value': 4710, 'text': 'Workflow Session', 'displayOrder': 2, 'name1033': 'Workflow Session', 'name1049': 'Сеанс бизнес-процесса' },
};
