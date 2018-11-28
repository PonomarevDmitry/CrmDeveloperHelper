using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.ObjectModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    internal static class EntityFileNameFormatter
    {
        private static string GetDateString()
        {
            return DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss");
        }

        private const string WorkflowFormatFile = "{0}.{1} Workflow {2} - {3} - {4} at {5}.{6}";

        internal static ReadOnlyCollection<string> WorkflowIgnoreFields = new ReadOnlyCollection<string>(new[] { Workflow.Schema.Attributes.xaml, Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Attributes.clientdata });

        internal static string GetWorkflowFileName(string connectionName, string entityName, string category, string name, string fieldTitle, string extension)
        {
            return string.Format(WorkflowFormatFile, connectionName, entityName, category, name, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        internal static ReadOnlyCollection<string> WebResourceIgnoreFields = new ReadOnlyCollection<string>(new[] { WebResource.Schema.Attributes.content, WebResource.Schema.Attributes.dependencyxml });

        private const string WebResourceFormatFile = "{0}.{1} {2} at {3}.{4}";

        internal static string GetWebResourceFileName(string connectionName, string name, string fieldTitle, string extension)
        {
            return string.Format(WebResourceFormatFile, connectionName, name, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        internal static ReadOnlyCollection<string> SystemFormIgnoreFields = new ReadOnlyCollection<string>(new[] { SystemForm.Schema.Attributes.formxml });

        private const string SystemFormFormatFile = "{0}.{1} SystemForm {2} - {3} at {4}.{5}";

        internal static string GetSystemFormFileName(string connectionName, string entityName, string name, string fieldTitle, string extension)
        {
            return string.Format(SystemFormFormatFile, connectionName, entityName, name, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string SolutionFormatFile = "{0}.Solution {1} - {2} at {3}.{4}";

        private const string SolutionFormatFileMultiple = "{0}.Solutions {1} - {2} - {3} at {4}.{5}";

        internal static string GetSolutionFileName(string connectionName, string uniquename, string fieldTitle, string extension = "txt")
        {
            return string.Format(SolutionFormatFile, connectionName, uniquename, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        internal static string GetSolutionMultipleFileName(string connectionName, string uniquename1, string uniquename2, string fieldTitle, string extension = "txt")
        {
            return string.Format(SolutionFormatFileMultiple, connectionName, uniquename1, uniquename2, fieldTitle, GetDateString(), extension.Trim('.'));
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

        internal static ReadOnlyCollection<string> SiteMapIgnoreFields = new ReadOnlyCollection<string>(new[] { SiteMap.Schema.Attributes.sitemapxml });

        internal static string GetSiteMapFileName(string connectionName, string name, Guid id, string fieldTitle, string extension)
        {
            return string.Format(SiteMapFormatFile, connectionName, name, id, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string SavedQueryVisualizationFormatFile = "{0}.{1} SystemChart {2} - {3} at {4}.{5}";

        internal static ReadOnlyCollection<string> SavedQueryVisualizationIgnoreFields = new ReadOnlyCollection<string>(new[] { SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Attributes.presentationdescription });

        internal static string GetSavedQueryVisualizationFileName(string connectionName, string entityName, string name, string fieldTitle, string extension)
        {
            return string.Format(SavedQueryVisualizationFormatFile, connectionName, entityName, name, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string SavedQueryFormatFile = "{0}.{1} SavedQuery {2} - {3} at {4}.{5}";

        internal static ReadOnlyCollection<string> SavedQueryIgnoreFields = new ReadOnlyCollection<string>(new[] { SavedQuery.Schema.Attributes.fetchxml, SavedQuery.Schema.Attributes.layoutxml, SavedQuery.Schema.Attributes.columnsetxml });

        internal static string GetSavedQueryFileName(string connectionName, string entityName, string name, string fieldTitle, string extension)
        {
            return string.Format(SavedQueryFormatFile, connectionName, entityName, name, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string ApplicationRibbonFormatFile = "{0}.ApplicationRibbon at {1}.{2}";

        internal static string GetApplicationRibbonFileName(string connectionName, string extension = "xml")
        {
            return string.Format(ApplicationRibbonFormatFile, connectionName, GetDateString(), extension.Trim('.'));
        }

        private const string ApplicationRibbonFormatFileFieldTitle = "{0}.ApplicationRibbon - {1} at {2}.{3}";

        internal static string GetApplicationRibbonFileName(string connectionName, string fieldTitle, string extension)
        {
            return string.Format(ApplicationRibbonFormatFileFieldTitle, connectionName, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string ApplicationRibbonDiffXmlFormatFile = "{0}.ApplicationRibbonDiffXml at {1}.xml";

        internal static string GetApplicationRibbonDiffXmlFileName(string connectionName)
        {
            return string.Format(ApplicationRibbonDiffXmlFormatFile, connectionName, GetDateString());
        }

        private const string ApplicationRibbonDiffXmlFormatFileFieldTitle = "{0}.ApplicationRibbonDiffXml - {1} at {2}.{3}";

        internal static string GetApplicationRibbonDiffXmlFileName(string connectionName, string fieldTitle, string extension)
        {
            return string.Format(ApplicationRibbonDiffXmlFormatFileFieldTitle, connectionName, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string EntityRibbonFormatFile = "{0}.{1}.Ribbon at {2}.{3}";

        internal static string GetEntityRibbonFileName(string connectionName, string entityName, string extension = "xml")
        {
            return string.Format(EntityRibbonFormatFile, connectionName, entityName, GetDateString(), extension.Trim('.'));
        }

        private const string EntityRibbonFormatFileFieldTitle = "{0}.{1}.Ribbon - {2} at {3}.{4}";

        internal static string GetEntityRibbonFileName(string connectionName, string entityName, string fieldTitle, string extension)
        {
            return string.Format(EntityRibbonFormatFileFieldTitle, connectionName, entityName, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string EntityRibbonDiffXmlFormatFile = "{0}.{1}.RibbonDiffXml at {2}.xml";

        internal static string GetEntityRibbonDiffXmlFileName(string connectionName, string entityName)
        {
            return string.Format(EntityRibbonDiffXmlFormatFile, connectionName, entityName, GetDateString());
        }

        private const string EntityRibbonDiffXmlFormatFileFieldTitle = "{0}.{1}.RibbonDiffXml - {2} at {3}.{4}";

        internal static string GetEntityRibbonDiffXmlFileName(string connectionName, string entityName, string fieldTitle, string extension)
        {
            return string.Format(EntityRibbonDiffXmlFormatFileFieldTitle, connectionName, entityName, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string ReportFormatFile = "{0}.Report {1} - {2} - {3} at {4}.{5}";

        internal static ReadOnlyCollection<string> ReportIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            Report.Schema.Attributes.bodytext
            , Report.Schema.Attributes.originalbodytext
            , Report.Schema.Attributes.bodybinary
            , Report.Schema.Attributes.defaultfilter
            , Report.Schema.Attributes.customreportxml
            , Report.Schema.Attributes.schedulexml
            , Report.Schema.Attributes.queryinfo
        });

        internal static string GetReportFileName(string connectionName, string name, Guid id, string fieldTitle, string extension)
        {
            return string.Format(ReportFormatFile, connectionName, name, id.ToString(), fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string PluginTypeFormatFileTxt = "{0}.PluginType {1} - {2} at {3}.txt";

        internal static string GetPluginTypeFileName(string connectionName, string name, string fieldTitle)
        {
            return string.Format(PluginTypeFormatFileTxt, connectionName, name, fieldTitle, GetDateString());
        }

        private const string PluginAssemblyFormatFileTxt = "{0}.PluginAssembly {1} - {2} at {3}.{4}";

        internal static ReadOnlyCollection<string> PluginAssemblyIgnoreFields = new ReadOnlyCollection<string>(new[] { PluginAssembly.Schema.Attributes.content });

        internal static string GetPluginAssemblyFileName(string connectionName, string name, string fieldTitle, string extension)
        {
            return string.Format(PluginAssemblyFormatFileTxt, connectionName, name, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string OrganizationFormatFile = "{0}.Organization {1} - {2} at {3}.{4}";

        internal static ReadOnlyCollection<string> OrganizationIgnoreFields = new ReadOnlyCollection<string>(new[]
        {
            Organization.Schema.Attributes.defaultemailsettings
            , Organization.Schema.Attributes.externalpartycorrelationkeys
            , Organization.Schema.Attributes.externalpartyentitysettings
            , Organization.Schema.Attributes.featureset
            , Organization.Schema.Attributes.kmsettings
            , Organization.Schema.Attributes.referencesitemapxml
            , Organization.Schema.Attributes.sitemapxml
            , Organization.Schema.Attributes.defaultthemedata
            , Organization.Schema.Attributes.highcontrastthemedata
            , Organization.Schema.Attributes.slapausestates
        });

        internal static string GetOrganizationFileName(string connectionName, string name, string fieldTitle, string extension)
        {
            return string.Format(OrganizationFormatFile, connectionName, name, fieldTitle, GetDateString(), extension.Trim('.'));
        }

        private const string PluginConfigurationFormatFileTxt = "{0} {1} - Description.{2}";

        internal static string GetPluginConfigurationFileName(string fileName, string fieldTitle, string extension)
        {
            return string.Format(PluginConfigurationFormatFileTxt, fileName, fieldTitle, extension.Trim('.'));
        }

        private const string MessageFormatFileTxt = "{0}.Message {1} - {2} at {3}.txt";

        internal static string GetMessageFileName(string connectionName, string name, string fieldTitle)
        {
            return string.Format(MessageFormatFileTxt, connectionName, name, fieldTitle, GetDateString());
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

        internal static string GetEntityName(string connectionName, Entity entity, string fieldTitle, string extension)
        {
            switch (entity.LogicalName.ToLower())
            {
                case Workflow.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<Workflow>();

                        return GetWorkflowFileName(connectionName, ent.PrimaryEntity, ent.FormattedValues[Workflow.Schema.Attributes.category], ent.Name, fieldTitle, extension.Trim('.'));
                    }

                case WebResource.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<WebResource>();

                        return GetWebResourceFileName(connectionName, ent.Name, fieldTitle, extension.Trim('.'));
                    }

                case Report.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<Report>();

                        return GetReportFileName(connectionName, ent.Name, ent.Id, fieldTitle, extension.Trim('.'));
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
                            return GetApplicationRibbonFileName(connectionName, fieldTitle, extension.Trim('.'));
                        }
                        else
                        {
                            return GetEntityRibbonFileName(connectionName, ent.Entity, fieldTitle, extension.Trim('.'));
                        }
                    }

                case PluginAssembly.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<PluginAssembly>();

                        return GetPluginAssemblyFileName(connectionName, ent.Name, fieldTitle, extension.Trim('.'));
                    }

                case PluginType.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<PluginType>();

                        return GetPluginTypeFileName(connectionName, ent.Name, fieldTitle);
                    }

                case SystemForm.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SystemForm>();

                        return GetSystemFormFileName(connectionName, ent.ObjectTypeCode, ent.Name, fieldTitle, extension.Trim('.'));
                    }

                case SavedQuery.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SavedQuery>();

                        return GetSavedQueryFileName(connectionName, ent.ReturnedTypeCode, ent.Name, fieldTitle, extension.Trim('.'));
                    }

                case SavedQueryVisualization.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SavedQueryVisualization>();

                        return GetSavedQueryFileName(connectionName, ent.PrimaryEntityTypeCode, ent.Name, fieldTitle, extension.Trim('.'));
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

                        return GetOrganizationFileName(connectionName, ent.Name, fieldTitle, extension.Trim('.'));
                    }

                case SiteMap.EntityLogicalName:
                    {
                        var ent = entity.ToEntity<SiteMap>();

                        return GetSiteMapFileName(connectionName, ent.SiteMapName, ent.Id, fieldTitle, extension.Trim('.'));
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

                //case SdkMessageResponse.EntityLogicalName:
                //    {
                //        var ent = entity.ToEntity<SdkMessageResponse>();

                //        return get(connectionName, ent.Id.ToString(), fieldTitle);
                //    }

                default:
                    break;
            }

            return string.Format("{0}.Entity {1} - {2} - {3} at {4}.{5}", connectionName, entity.LogicalName, entity.Id, fieldTitle, GetDateString(), extension.Trim('.'));
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

        
    }
}