using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class ConnectionData
    {
        private static bool IsValidUri(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return false;
            }


            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri tmp))
            {
                return false;
            }

            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
        }

        private bool TryGetPublicUrl(out string uri)
        {
            uri = null;

            if (string.IsNullOrEmpty(this.PublicUrl))
            {
                return false;
            }

            uri = this.PublicUrl.TrimEnd('/');

            return true;
        }

        public void OpenCrmWebSite(OpenCrmWebSiteType crmWebSiteType)
        {
            var uri = GetOpenCrmWebSiteUrl(crmWebSiteType);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetOpenCrmWebSiteUrl(OpenCrmWebSiteType crmWebSiteType)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            string relativeUrl = string.Empty;

            if (crmWebSiteType != OpenCrmWebSiteType.CrmWebApplication)
            {
                relativeUrl = GetRelativeUrl(crmWebSiteType);

                if (!string.IsNullOrEmpty(relativeUrl))
                {
                    relativeUrl = '/' + relativeUrl.TrimStart('/');
                }
            }

            var uri = publicUrl.TrimEnd('/') + relativeUrl;

            return uri;
        }

        private string GetRelativeUrl(OpenCrmWebSiteType crmWebSiteType)
        {
            switch (crmWebSiteType)
            {
                case OpenCrmWebSiteType.AdvancedFind:
                    return "/main.aspx?pagetype=advancedfind";

                case OpenCrmWebSiteType.Solutions:
                    //return "/tools/systemcustomization/systemCustomization.aspx?pid=11&web=true";
                    return "/tools/Solution/home_solution.aspx?etc=7100";

                case OpenCrmWebSiteType.Customization:
                    return "/tools/systemcustomization/systemCustomization.aspx";

                case OpenCrmWebSiteType.SystemUsers:
                    return GetEntityListRelativeUrl(SystemUser.EntityLogicalName);

                case OpenCrmWebSiteType.Teams:
                    return GetEntityListRelativeUrl(Team.EntityLogicalName);

                case OpenCrmWebSiteType.Roles:
                    return GetEntityListRelativeUrl(Role.EntityLogicalName);

                case OpenCrmWebSiteType.Security:
                    return "/tools/AdminSecurity/adminsecurity_area.aspx";

                case OpenCrmWebSiteType.Workflows:
                    return GetEntityListRelativeUrl(Workflow.EntityLogicalName);

                case OpenCrmWebSiteType.SystemJobs:
                    return "/tools/business/home_asyncoperation.aspx";

                case OpenCrmWebSiteType.Audit:
                    return "/tools/audit/audit_area.aspx";

                case OpenCrmWebSiteType.Administration:
                    return "/tools/Admin/admin.aspx";

                case OpenCrmWebSiteType.EngagementHub:
                    return "/engagementhub.aspx";

                case OpenCrmWebSiteType.Business:
                    return "/tools/business/business.aspx";

                case OpenCrmWebSiteType.Templates:
                    return "/tools/templates/templates.aspx";

                case OpenCrmWebSiteType.ProductCatalog:
                    return "/tools/productcatalog/productcatalog.aspx";

                case OpenCrmWebSiteType.ServiceManagement:
                    return "/tools/servicemanagement/servicemanagement.aspx";

                case OpenCrmWebSiteType.DataManagement:
                    return "/tools/DataManagement/datamanagement.aspx";

                case OpenCrmWebSiteType.Social:
                    return "/tools/social/social_area.aspx";

                case OpenCrmWebSiteType.Calendar:
                    return "/workplace/home_calendar.aspx";

                case OpenCrmWebSiteType.MobileOffline:
                    return "/tools/mobileoffline/mobileoffline.aspx";

                case OpenCrmWebSiteType.ExternAppManagement:
                    return "/tools/externappmanagement/externappmanagement.aspx";

                case OpenCrmWebSiteType.AppsForCrm:
                    return "/tools/appsforcrm/AppForOutlookAdminSettings.aspx";

                case OpenCrmWebSiteType.RelationshipIntelligence:
                    return "/_static/tools/RelationshipIntelligenceConfig/UnifiedConfig.html";

                case OpenCrmWebSiteType.MicrosoftFlow:
                    return "/tools/MicrosoftFlow/FlowTemplatesPage.aspx";

                case OpenCrmWebSiteType.AppModule:
                    return "/tools/AppModuleContainer/applandingtilepage.aspx";

                case OpenCrmWebSiteType.AppointmentBook:
                    return "/sm/home_apptbook.aspx";
            }

            return null;
        }

        public void OpenSolutionInWeb(Guid idSolution)
        {
            string uri = GetSolutionUrl(idSolution);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetSolutionUrl(Guid idSolution)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + string.Format("/tools/solution/edit.aspx?id={0}", idSolution);
        }

        public void OpenSolutionCreateInWeb()
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return;
            }

            string uri = publicUrl + "/tools/solution/edit.aspx";

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public void OpenReportCreateInWeb()
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return;
            }

            string uri = publicUrl + "/CRMReports/reportproperty.aspx";

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public void OpenEntityInstanceInWeb(string entityName, Guid entityId)
        {
            string uri = GetEntityInstanceUrl(entityName, entityId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetEntityInstanceUrl(string entityName, Guid entityId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            var componentType = GetSolutionComponentType(entityName);

            if (componentType.HasValue)
            {
                string uriEnd = GetSolutionComponentRelativeUrl(componentType.Value, entityId);

                if (!string.IsNullOrEmpty(uriEnd))
                {
                    return publicUrl + "/" + uriEnd.TrimStart('/');
                }
            }

            return string.Format(GetEntityInstanceUrlFormat(entityName), entityId);
        }

        public string GetEntityInstanceUrlFormat(string entityName)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + GetEntityInstanceRelativeUrlFormat(entityName);
        }

        public string GetEntityInstanceRelativeUrlFormat(string entityName)
        {
            if (string.Equals(entityName, "asyncoperation", StringComparison.InvariantCultureIgnoreCase))
            {
                return "/tools/asyncoperation/edit.aspx?id={0}";
            }

            if (string.Equals(entityName, BusinessUnit.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
            {
                return "/biz/business/edit.aspx?id={0}";
            }

            return "/main.aspx?etn=" + entityName + "&pagetype=entityrecord&id={0}";
        }

        public void OpenEntityListInWeb(string entityName)
        {
            string uri = GetEntityListUrl(entityName);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetEntityListUrl(string entityName)
        {
            return string.Format(GetEntityListUrlFormat(), entityName);
        }

        public string GetEntityListUrlFormat()
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + "/main.aspx?etn={0}&pagetype=entitylist";
        }

        public string GetEntityListRelativeUrl(string entityName)
        {
            return string.Format("/main.aspx?etn={0}&pagetype=entitylist", entityName);
        }

        private static ComponentType? GetSolutionComponentType(string entityName)
        {
            var componentTypeName = entityName;

            if (string.Equals(componentTypeName, "template", StringComparison.OrdinalIgnoreCase))
            {
                componentTypeName = "EmailTemplate";
            }

            if (Enum.TryParse<ComponentType>(componentTypeName, true, out ComponentType componentType))
            {
                return componentType;
            }

            return null;
        }

        public Guid? GetEntityMetadataId(string entityName)
        {
            if (this.IntellisenseData.Entities != null
                && this.IntellisenseData.Entities.ContainsKey(entityName)
                && this.IntellisenseData.Entities[entityName].MetadataId.HasValue
                )
            {
                return this.IntellisenseData.Entities[entityName].MetadataId.Value;
            }

            return null;
        }

        public void OpenEntityMetadataInWeb(Guid entityMetadataId)
        {
            string uri = GetEntityMetadataUrl(entityMetadataId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public void OpenEntityMetadataInWeb(string entityName)
        {
            if (this.IntellisenseData.Entities != null
                && this.IntellisenseData.Entities.ContainsKey(entityName)
                && this.IntellisenseData.Entities[entityName].MetadataId.HasValue
                )
            {
                OpenEntityMetadataInWeb(this.IntellisenseData.Entities[entityName].MetadataId.Value);
            }
        }

        public string GetEntityMetadataUrl(Guid entityId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + GetSolutionComponentRelativeUrl(ComponentType.Entity, entityId);
        }

        public void OpenAttributeMetadataInWeb(Guid entityId, Guid attributeId)
        {
            string uri = GetAttributeMetadataUrl(entityId, attributeId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetAttributeMetadataUrl(Guid entityId, Guid attributeId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + GetAttributeMetadataRelativeUrl(entityId, attributeId);
        }

        public string GetAttributeMetadataRelativeUrl(Guid entityId, Guid attributeId)
        {
            return $"/tools/systemcustomization/attributes/manageAttribute.aspx?attributeId={attributeId}&entityId={entityId}";
        }

        public void OpenEntityKeyMetadataInWeb(Guid entityId, Guid entityKeyId)
        {
            string uri = GetEntityKeyMetadataUrl(entityId, entityKeyId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetEntityKeyMetadataUrl(Guid entityId, Guid entityKeyId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + GetEntityKeyMetadataRelativeUrl(entityId, entityKeyId);
        }

        public string GetEntityKeyMetadataRelativeUrl(Guid entityId, Guid entityKeyId)
        {
            return $"/tools/systemcustomization/AlternateKeys/manageAlternateKeys.aspx?entityId={entityId}&entityKeyId={entityKeyId}";
        }

        public void OpenRelationshipMetadataInWeb(Guid entityId, Guid relationId)
        {
            string uri = GetRelationshipMetadataUrl(entityId, relationId);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetRelationshipMetadataUrl(Guid entityId, Guid relationId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + GetRelationshipMetadataRelativeUrl(entityId, relationId);
        }

        public string GetRelationshipMetadataRelativeUrl(Guid entityId, Guid relationId)
        {
            return $"/tools/systemcustomization/relationships/manageRelationship.aspx?entityId={entityId}&entityRelationshipId={relationId}";
        }

        public void OpenGlobalOptionSetInWeb(Guid idGlobalOptionSet)
        {
            string uri = GetGlobalOptionSetUrl(idGlobalOptionSet);

            if (!IsValidUri(uri)) return;

            System.Diagnostics.Process.Start(uri);
        }

        public string GetGlobalOptionSetUrl(Guid idGlobalOptionSet)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            return publicUrl + GetSolutionComponentRelativeUrl(ComponentType.OptionSet, idGlobalOptionSet);
        }

        //public void OpenSolutionComponentInWeb(ComponentType componentType, Guid objectId)
        //{
        //    string uri = GetSolutionComponentUrl(componentType, objectId);

        //    if (!IsValidUri(uri)) { return; }

        //    System.Diagnostics.Process.Start(uri);
        //}

        //public string GetSolutionComponentUrl(ComponentType componentType, Guid objectId)
        //{
        //    string uriEnd = GetSolutionComponentRelativeUrl(componentType, objectId);

        //    if (string.IsNullOrEmpty(uriEnd))
        //    {
        //        return null;
        //    }

        //    if (!TryGetPublicUrl(out string publicUrl))
        //    {
        //        return null;
        //    }

        //    var uri = publicUrl + "/" + uriEnd.TrimStart('/');
        //    return uri;
        //}

        public string GetSolutionComponentRelativeUrl(ComponentType componentType, Guid objectId)
        {
            switch (componentType)
            {
                case ComponentType.SavedQuery:
                    return $"/tools/vieweditor/viewManager.aspx?id={objectId}";

                case ComponentType.Workflow:
                    return $"/sfa/workflow/edit.aspx?id={objectId}";

                case ComponentType.Entity:
                    return $"/tools/systemcustomization/entities/manageentity.aspx?id={objectId}";

                case ComponentType.Attribute:
                    {
                        if (this.IntellisenseData != null)
                        {
                            var entityMetadataId = (from ent in this.IntellisenseData?.Entities.Values
                                                    where ent.MetadataId.HasValue
                                                    from attr in ent.Attributes?.Values
                                                    where attr.MetadataId == objectId
                                                    select ent.MetadataId).FirstOrDefault();

                            if (entityMetadataId.HasValue)
                            {
                                return GetAttributeMetadataRelativeUrl(entityMetadataId.Value, objectId);
                            }
                        }
                    }
                    break;

                case ComponentType.EntityKey:
                    {
                        if (this.IntellisenseData != null)
                        {
                            var entityMetadataId = (from ent in this.IntellisenseData?.Entities.Values
                                                    where ent.MetadataId.HasValue
                                                    from key in ent.Keys?.Values
                                                    where key.MetadataId == objectId
                                                    select ent.MetadataId).FirstOrDefault();

                            if (entityMetadataId.HasValue)
                            {
                                return GetEntityKeyMetadataRelativeUrl(entityMetadataId.Value, objectId);
                            }
                        }
                    }
                    break;

                case ComponentType.EntityRelationship:
                    {
                        if (this.IntellisenseData != null)
                        {
                            {
                                var entityMetadataId = (from ent in this.IntellisenseData?.Entities.Values
                                                        where ent.MetadataId.HasValue
                                                        from rel in ent.ManyToOneRelationships?.Values
                                                        where rel.MetadataId == objectId
                                                        select ent.MetadataId).FirstOrDefault();

                                if (entityMetadataId.HasValue)
                                {
                                    return GetRelationshipMetadataRelativeUrl(entityMetadataId.Value, objectId);
                                }
                            }

                            {
                                var entityMetadataId = (from ent in this.IntellisenseData?.Entities.Values
                                                        where ent.MetadataId.HasValue
                                                        from rel in ent.ManyToManyRelationships?.Values
                                                        where rel.MetadataId == objectId
                                                        select ent.MetadataId).FirstOrDefault();

                                if (entityMetadataId.HasValue)
                                {
                                    return GetRelationshipMetadataRelativeUrl(entityMetadataId.Value, objectId);
                                }
                            }
                        }
                    }
                    break;

                case ComponentType.OptionSet:
                    return $"/tools/systemcustomization/optionset/optionset.aspx?id={objectId}";

                case ComponentType.WebResource:
                    return $"/main.aspx?etc=9333&id={objectId}&pagetype=webresourceedit";

                case ComponentType.Report:
                    return $"/CRMReports/reportproperty.aspx?id={objectId}";

                case ComponentType.EntityMap:
                    return $"/tools/systemcustomization/relationships/mappings/mappingList.aspx?mappingId={objectId}";

                case ComponentType.DisplayString:
                    return $"/tools/systemcustomization/displaystrings/edit.aspx?id={objectId}";

                case ComponentType.FieldSecurityProfile:
                    return $"/biz/fieldsecurityprofiles/edit.aspx?id={objectId}";

                case ComponentType.Role:
                    return $"/biz/roles/edit.aspx?id={objectId}";

                case ComponentType.ConnectionRole:
                    return $"/connections/connectionroles/edit.aspx?id={objectId}";
            }

            SolutionComponent.GetComponentTypeEntityName((int)componentType, out string entityName, out string entityIdName);

            if (!string.IsNullOrEmpty(entityName))
            {
                return string.Format(GetEntityInstanceRelativeUrlFormat(entityName), objectId);
            }

            return null;
        }

        internal void OpenSolutionComponentDependentComponentsInWeb(ComponentType componentType, Guid objectId)
        {
            if (!TryGetPublicUrl(out string publicUrl))
            {
                return;
            }

            var entityTypeCode = SolutionComponent.GetComponentTypeObjectTypeCode(componentType);

            if (!entityTypeCode.HasValue)
            {
                return;
            }

            var uri = publicUrl + $"/tools/dependency/dependencyviewdialog.aspx?dType=1&objectid={objectId}&objecttype={entityTypeCode}&operationtype=showdependency";

            if (!IsValidUri(uri)) { return; }

            System.Diagnostics.Process.Start(uri);
        }
    }
}
