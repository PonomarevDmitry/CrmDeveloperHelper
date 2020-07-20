using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public enum FileExtension
    {
        txt,

        xml,

        js,

        json,

        zip,

        sql,

        dll
    }

    internal static class EntityFileNameFormatter
    {
        public static partial class Headers
        {
            public const string EntityDescription = "EntityDescription";

            public const string Backup = "Backup";
        }

        private static partial class Ext
        {
            public const string txt = "txt";

            public const string xml = "xml";

            public const string js = "js";

            public const string json = "json";

            public const string zip = "zip";

            public const string sql = "sql";

            public const string dll = "dll";
        }

        public static string ToStr(this FileExtension extension)
        {
            switch (extension)
            {
                case FileExtension.txt:
                    return Ext.txt;

                case FileExtension.xml:
                    return Ext.xml;

                case FileExtension.json:
                    return Ext.json;

                case FileExtension.sql:
                    return Ext.sql;

                case FileExtension.zip:
                    return Ext.zip;

                case FileExtension.dll:
                    return Ext.dll;
            }

            throw new ArgumentOutOfRangeException(extension.ToString());
        }

        private static string GetDateString()
        {
            return DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss");
        }

        private const string WorkflowFormatFile = "{0}.{1} Workflow {2} - {3} - {4} at {5}.{6}";

        internal static string GetWorkflowFileName(string connectionName, string entityName, string category, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(WorkflowFormatFile, connectionName, entityName, category, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string WebResourceFormatFile = "{0}.{1} {2} at {3}.{4}";

        internal static string GetWebResourceFileName(string connectionName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(WebResourceFormatFile, connectionName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string CustomControlFormatFile = "{0}.{1} {2} at {3}.{4}";

        internal static string GetCustomControlFileName(string connectionName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(CustomControlFormatFile, connectionName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string SystemFormFormatFile = "{0}.{1} SystemForm {2} - {3} at {4}.{5}";

        internal static string GetSystemFormFileName(string connectionName, string entityName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(SystemFormFormatFile, connectionName, entityName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string SolutionFormatFile = "{0}.Solution {1} - {2} at {3}.{4}";

        private const string SolutionFormatFileMultiple = "{0}.Solutions {1} - {2} - {3} at {4}.{5}";

        internal static string GetSolutionFileName(string connectionName, string uniquename, string fieldTitle, FileExtension extension)
        {
            return string.Format(SolutionFormatFile, connectionName, uniquename, fieldTitle, GetDateString(), extension.ToStr());
        }

        internal static string GetSolutionMultipleFileName(string connectionName, string uniquename1, string uniquename2, string fieldTitle, FileExtension extension = FileExtension.txt)
        {
            return string.Format(SolutionFormatFileMultiple, connectionName, uniquename1, uniquename2, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string DifferenceImageFormatFile = "OrgCompare {0} OrganizationDifferenceImage at {1}.xml";

        internal static string GetDifferenceImageFileName(string organizationsString)
        {
            return string.Format(DifferenceImageFormatFile, organizationsString, GetDateString());
        }

        private const string DifferenceConnectionsFileNameFormat = "OrgCompare {0} at {1} {2}.txt";

        internal static string GetDifferenceConnectionsForFieldFileName(string organizationsString, string fieldTitle)
        {
            return string.Format(DifferenceConnectionsFileNameFormat, organizationsString, GetDateString(), fieldTitle);
        }

        private const string SiteMapFormatFile = "{0}.SiteMap{1} {2} - {3} at {4}.{5}";

        internal static string GetSiteMapFileName(string connectionName, string name, Guid id, string fieldTitle, FileExtension extension)
        {
            return string.Format(SiteMapFormatFile, connectionName, name, id, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string SavedQueryVisualizationFormatFile = "{0}.{1} SystemChart {2} - {3} at {4}.{5}";

        internal static string GetSavedQueryVisualizationFileName(string connectionName, string entityName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(SavedQueryVisualizationFormatFile, connectionName, entityName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string SavedQueryFormatFile = "{0}.{1} SavedQuery {2} - {3} at {4}.{5}";

        internal static string GetSavedQueryFileName(string connectionName, string entityName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(SavedQueryFormatFile, connectionName, entityName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string ApplicationRibbonFormatFile = "{0}.ApplicationRibbon at {1}.{2}";

        internal static string GetApplicationRibbonFileName(string connectionName, FileExtension extension = FileExtension.xml)
        {
            return string.Format(ApplicationRibbonFormatFile, connectionName, GetDateString(), extension.ToStr());
        }

        private const string ApplicationRibbonFormatFileFieldTitle = "{0}.ApplicationRibbon - {1} at {2}.{3}";

        internal static string GetApplicationRibbonFileName(string connectionName, string fieldTitle, FileExtension extension)
        {
            return string.Format(ApplicationRibbonFormatFileFieldTitle, connectionName, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string ApplicationRibbonDiffXmlFormatFile = "{0}.ApplicationRibbonDiffXml at {1}.xml";

        internal static string GetApplicationRibbonDiffXmlFileName(string connectionName)
        {
            return string.Format(ApplicationRibbonDiffXmlFormatFile, connectionName, GetDateString());
        }

        private const string ApplicationRibbonDiffXmlFormatFileFieldTitle = "{0}.ApplicationRibbonDiffXml - {1} at {2}.{3}";

        internal static string GetApplicationRibbonDiffXmlFileName(string connectionName, string fieldTitle, FileExtension extension)
        {
            return string.Format(ApplicationRibbonDiffXmlFormatFileFieldTitle, connectionName, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string EntityRibbonFormatFile = "{0}.{1}.Ribbon at {2}.{3}";

        internal static string GetEntityRibbonFileName(string connectionName, string entityName, FileExtension extension = FileExtension.xml)
        {
            return string.Format(EntityRibbonFormatFile, connectionName, entityName, GetDateString(), extension.ToStr());
        }

        private const string EntityRibbonFormatFileFieldTitle = "{0}.{1}.Ribbon - {2} at {3}.{4}";

        internal static string GetEntityRibbonFileName(string connectionName, string entityName, string fieldTitle, FileExtension extension)
        {
            return string.Format(EntityRibbonFormatFileFieldTitle, connectionName, entityName, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string EntityRibbonDiffXmlFormatFile = "{0}.{1}.RibbonDiffXml at {2}.xml";

        internal static string GetEntityRibbonDiffXmlFileName(string connectionName, string entityName)
        {
            return string.Format(EntityRibbonDiffXmlFormatFile, connectionName, entityName, GetDateString());
        }

        private const string EntityRibbonDiffXmlFormatFileFieldTitle = "{0}.{1}.RibbonDiffXml - {2} at {3}.{4}";

        internal static string GetEntityRibbonDiffXmlFileName(string connectionName, string entityName, string fieldTitle, FileExtension extension)
        {
            return string.Format(EntityRibbonDiffXmlFormatFileFieldTitle, connectionName, entityName, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string ReportFormatFile = "{0}.Report {1} - {2} - {3} at {4}.{5}";

        internal static string GetReportFileName(string connectionName, string name, Guid id, string fieldTitle, FileExtension extension)
        {
            return GetReportFileName(connectionName, name, id, fieldTitle, extension.ToStr());
        }

        internal static string GetReportFileName(string connectionName, string name, Guid id, string fieldTitle, string extension)
        {
            return string.Format(ReportFormatFile, connectionName, name, id.ToString(), fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string PluginTypeFormatFileTxt = "{0}.PluginType {1} - {2} at {3}.{4}";

        internal static string GetPluginTypeFileName(string connectionName, string name, string fieldTitle, FileExtension extension = FileExtension.txt)
        {
            return string.Format(PluginTypeFormatFileTxt, connectionName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string PluginAssemblyFormatFileTxt = "{0}.PluginAssembly {1} - {2} at {3}.{4}";

        internal static string GetPluginAssemblyFileName(string connectionName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(PluginAssemblyFormatFileTxt, connectionName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string OrganizationFormatFile = "{0}.Organization {1} - {2} at {3}.{4}";

        internal static string GetOrganizationFileName(string connectionName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(OrganizationFormatFile, connectionName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string PluginConfigurationFormatFileTxt = "{0} {1} - Description.{2}";

        internal static string GetPluginConfigurationFileName(string fileName, string fieldTitle, FileExtension extension)
        {
            return string.Format(PluginConfigurationFormatFileTxt, fileName, fieldTitle, extension.ToStr());
        }

        private const string MessageFormatFileTxtFormat5 = "{0}.Message {1} - {2} at {3}.{4}";

        internal static string GetMessageFileName(string connectionName, string name, string fieldTitle, FileExtension extension = FileExtension.txt)
        {
            return string.Format(MessageFormatFileTxtFormat5, connectionName, name, fieldTitle, GetDateString(), extension);
        }

        private const string MessageFilterFormatFileTxt = "{0}.MessageFilter {1} - {2} at {3}.txt";

        internal static string GetMessageFilterFileName(string connectionName, string name, string fieldTitle)
        {
            return string.Format(MessageFilterFormatFileTxt, connectionName, name, fieldTitle, GetDateString());
        }

        private const string PluginStepFormatFileTxt = "{0}.PluginStep {1} - {2} at {3}.txt";

        internal static string GetPluginStepFileName(string connectionName, string name, string fieldTitle)
        {
            return string.Format(PluginStepFormatFileTxt, connectionName, name, fieldTitle, GetDateString());
        }

        private const string PluginImageFormatFileTxt = "{0}.PluginImage {1} - {2} at {3}.txt";

        internal static string GetPluginImageFileName(string connectionName, string name, string fieldTitle)
        {
            return string.Format(PluginImageFormatFileTxt, connectionName, name, fieldTitle, GetDateString());
        }

        private const string SdkMessageRequestFormatFileTxt = "{0}.SdkMessageRequest {1} - {2} at {3}.txt";

        internal static string GetSdkMessageRequestFileName(string connectionName, string name, string fieldTitle)
        {
            return string.Format(SdkMessageRequestFormatFileTxt, connectionName, name, fieldTitle, GetDateString());
        }

        private const string SdkMessageResponseFormatFileTxt = "{0}.SdkMessageResponse {1} - {2} at {3}.txt";

        internal static string GetSdkMessageResponseFileName(string connectionName, string name, string fieldTitle)
        {
            return string.Format(SdkMessageResponseFormatFileTxt, connectionName, name, fieldTitle, GetDateString());
        }

        private const string RoleFormatFileTxt = "{0}.Role {1} - {2} at {3}.{4}";

        internal static string GetRoleFileName(string connectionName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(RoleFormatFileTxt, connectionName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string SystemUserFormatFileTxt = "{0}.SystemUser {1} - {2} at {3}.{4}";

        internal static string GetSystemUserFileName(string connectionName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(SystemUserFormatFileTxt, connectionName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string TeamFormatFileTxt = "{0}.Team {1} - {2} at {3}.{4}";

        internal static string GetTeamFileName(string connectionName, string name, string fieldTitle, FileExtension extension)
        {
            return string.Format(TeamFormatFileTxt, connectionName, name, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string ImportJobFormatFileTxt = "{0}.ImportJob {1}{2} - {3} at {4}.{5}";

        internal static string GetImportJobFileName(string connectionName, string solutionName, DateTime? createdOn, string fieldTitle, FileExtension extension)
        {
            string createdOnStr = string.Empty;

            if (createdOn.HasValue)
            {
                createdOnStr = string.Format(" at {0:yyyy.MM.dd HH-mm-ss}", createdOn.Value);
            }

            return string.Format(ImportJobFormatFileTxt, connectionName, solutionName, createdOnStr, fieldTitle, GetDateString(), extension.ToStr());
        }

        internal static string GetEntityName(string connectionName, Entity entity, string fieldTitle, FileExtension extension)
        {
            switch (entity.LogicalName.ToLower())
            {
                case Workflow.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<Workflow>();

                        return GetWorkflowFileName(connectionName, ent.PrimaryEntity, ent.FormattedValues[Workflow.Schema.Attributes.category], ent.Name, fieldTitle, extension);
                    }

                case WebResource.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<WebResource>();

                        return GetWebResourceFileName(connectionName, ent.Name, fieldTitle, extension);
                    }

                case Report.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<Report>();

                        return GetReportFileName(connectionName, ent.Name, ent.Id, fieldTitle, extension);
                    }

                case SdkMessageProcessingStep.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SdkMessageProcessingStep>();

                        return GetPluginStepFileName(connectionName, ent.Name, fieldTitle);
                    }

                case SdkMessageProcessingStepImage.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SdkMessageProcessingStepImage>();

                        return GetPluginImageFileName(connectionName, ent.Name, fieldTitle);
                    }

                case RibbonCustomization.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<RibbonCustomization>();

                        if (string.IsNullOrEmpty(ent.Entity))
                        {
                            return GetApplicationRibbonFileName(connectionName, fieldTitle, extension);
                        }
                        else
                        {
                            return GetEntityRibbonFileName(connectionName, ent.Entity, fieldTitle, extension);
                        }
                    }

                case PluginAssembly.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<PluginAssembly>();

                        return GetPluginAssemblyFileName(connectionName, ent.Name, fieldTitle, extension);
                    }

                case PluginType.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<PluginType>();

                        return GetPluginTypeFileName(connectionName, ent.Name, fieldTitle);
                    }

                case SystemForm.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SystemForm>();

                        return GetSystemFormFileName(connectionName, ent.ObjectTypeCode, ent.Name, fieldTitle, extension);
                    }

                case SavedQuery.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SavedQuery>();

                        return GetSavedQueryFileName(connectionName, ent.ReturnedTypeCode, ent.Name, fieldTitle, extension);
                    }

                case SavedQueryVisualization.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SavedQueryVisualization>();

                        return GetSavedQueryVisualizationFileName(connectionName, ent.PrimaryEntityTypeCode, ent.Name, fieldTitle, extension);
                    }

                case SdkMessage.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SdkMessage>();

                        return GetMessageFileName(connectionName, ent.Name, fieldTitle);
                    }

                case SdkMessageFilter.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SdkMessageFilter>();

                        return GetMessageFilterFileName(connectionName, ent.PrimaryObjectTypeCode, fieldTitle);
                    }

                case Organization.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<Organization>();

                        return GetOrganizationFileName(connectionName, ent.Name, fieldTitle, extension);
                    }

                case SiteMap.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SiteMap>();

                        return GetSiteMapFileName(connectionName, ent.SiteMapName, ent.Id, fieldTitle, extension);
                    }

                case SdkMessageRequest.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SdkMessageRequest>();

                        return GetSdkMessageRequestFileName(connectionName, ent.Name, fieldTitle);
                    }

                case SdkMessageResponse.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SdkMessageResponse>();

                        return GetSdkMessageResponseFileName(connectionName, ent.Id.ToString(), fieldTitle);
                    }

                case Solution.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<Solution>();

                        return GetSolutionFileName(connectionName, ent.UniqueName, fieldTitle, extension);
                    }

                case Role.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<Role>();

                        return GetRoleFileName(connectionName, ent.Name, fieldTitle, extension);
                    }

                case SystemUser.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SystemUser>();

                        return GetSystemUserFileName(connectionName, ent.FullName, fieldTitle, extension);
                    }

                case Team.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<Team>();

                        return GetTeamFileName(connectionName, ent.Name, fieldTitle, extension);
                    }

                case ImportJob.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<ImportJob>();

                        return GetImportJobFileName(connectionName, ent.SolutionName, ent.CreatedOn, fieldTitle, extension);
                    }

                //case SdkEntity.EntityLogicalName:
                //    {
                //        var ent = entity.ToEntity<SdkEntity>();

                //        return GetSavedQueryVisualizationFileName(connectionName, ent, fieldTitle, extension);
                //    }

                default:
                    break;
            }

            return string.Format("{0}.Entity {1} - {2} - {3} at {4}.{5}", connectionName, entity.LogicalName, entity.Id, fieldTitle, GetDateString(), extension.ToStr());
        }

        private const string FindingCRMObjectsByUniqueidentifierFileNameFormat3 = "{0}.Finding CRM Objects by Uniqueidentifier {1} at {2}.txt";

        public static string GetFindingCRMObjectsByUniqueidentifierFileName(string connectionName, Guid entityId)
        {
            return string.Format(FindingCRMObjectsByUniqueidentifierFileNameFormat3, connectionName, entityId, GetDateString());
        }

        private const string FindingCRMObjectsByIdFileNameFormat3 = "{0}.Finding CRM Objects by Id {1} at {2}.txt";

        public static string GetFindingCRMObjectsByIdFileName(string connectionName, Guid entityId)
        {
            return string.Format(FindingCRMObjectsByIdFileNameFormat3, connectionName, entityId, GetDateString());
        }

        private const string CheckEntityNamesForPrefixFileNameFormat3 = "{0}.Check Entity Names for prefix {1} at {2}.txt";

        public static string GetCheckEntityNamesForPrefixFileName(string connectionName, string prefix)
        {
            return string.Format(CheckEntityNamesForPrefixFileNameFormat3, connectionName, prefix, GetDateString());
        }

        private const string ComparingRolePrivilegesInEntitiesFileNameFormat3 = "{0}.Comparing RolePrivileges for {1} and {2} at {3}.txt";

        public static string ComparingRolePrivilegesInEntitiesFileName(string connectionName, string name1, string name2)
        {
            string[] arr = new string[] { name1, name2 };

            Array.Sort(arr);

            return string.Format(ComparingRolePrivilegesInEntitiesFileNameFormat3, connectionName, arr[0], arr[1], GetDateString());
        }
    }
}