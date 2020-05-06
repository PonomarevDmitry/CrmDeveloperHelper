using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class ConnectionDataUrlGenerator
    {
        private readonly IOrganizationServiceExtented _service;

        public ConnectionDataUrlGenerator(IOrganizationServiceExtented service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

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

            if (string.IsNullOrEmpty(this._service.ConnectionData.PublicUrl))
            {
                return false;
            }

            uri = this._service.ConnectionData.PublicUrl.TrimEnd('/');

            return true;
        }

        public void OpenSolutionComponentInWeb(ComponentType componentType, Guid objectId)
        {
            string uri = GetSolutionComponentUrl(componentType, objectId);

            if (!IsValidUri(uri)) { return; }

            System.Diagnostics.Process.Start(uri);
        }

        public string GetSolutionComponentUrl(ComponentType componentType, Guid objectId)
        {
            string uriEnd = GetSolutionComponentRelativeUrl(componentType, objectId);

            if (string.IsNullOrEmpty(uriEnd))
            {
                return null;
            }

            if (!TryGetPublicUrl(out string publicUrl))
            {
                return null;
            }

            var uri = publicUrl + "/" + uriEnd.TrimStart('/');
            return uri;
        }

        private string GetSolutionComponentRelativeUrl(ComponentType componentType, Guid objectId)
        {
            switch (componentType)
            {
                case ComponentType.SavedQueryVisualization:
                    {
                        var respositorySolution = new SolutionRepository(this._service);

                        var chart = _service.RetrieveByQuery<SavedQueryVisualization>(SavedQueryVisualization.EntityLogicalName, objectId, new ColumnSet(SavedQueryVisualization.Schema.Attributes.primaryentitytypecode));

                        if (chart != null && !string.IsNullOrEmpty(chart.PrimaryEntityTypeCode))
                        {
                            if ((this._service.ConnectionData.IntellisenseData?.Entities?.ContainsKey(chart.PrimaryEntityTypeCode)).GetValueOrDefault())
                            {
                                var linkedEntityObjectCode = this._service.ConnectionData.IntellisenseData.Entities[chart.PrimaryEntityTypeCode].ObjectTypeCode;

                                if (linkedEntityObjectCode.HasValue)
                                {
                                    return $"/main.aspx?appSolutionId=%7b{Solution.Schema.InstancesUniqueId.DefaultId}%7d&extraqs=etc%3d{linkedEntityObjectCode}%26id%3d%7b{objectId}%7d&pagetype=vizdesigner";
                                }
                            }
                        }
                    }
                    break;

                case ComponentType.SystemForm:
                    {
                        var respositorySolution = new SolutionRepository(this._service);

                        var systemform = _service.RetrieveByQuery<SystemForm>(SystemForm.EntityLogicalName, objectId, new ColumnSet(SystemForm.Schema.Attributes.type, SystemForm.Schema.Attributes.objecttypecode));

                        if (systemform != null && systemform.Type != null)
                        {
                            if (systemform.Type.Value == (int)SystemForm.Schema.OptionSets.type.Task_Flow_Form_9)
                            {
                                var workflowRepository = new WorkflowRepository(_service);

                                var linkedWorkflow = workflowRepository.FindLinkedWorkflow(systemform.Id, new ColumnSet(false));

                                if (linkedWorkflow != null)
                                {
                                    return this.GetSolutionComponentRelativeUrl(ComponentType.Workflow, linkedWorkflow.Id);
                                }
                            }
                            else
                            {
                                return _service.ConnectionData.GetSystemFormRelativeUrl(systemform.ObjectTypeCode, systemform.Id, systemform.Type.Value);
                            }
                        }
                    }
                    break;

                case ComponentType.SiteMap:
                    {
                        var respositorySolution = new SolutionRepository(this._service);

                        //designer/sitemap/FD140AAF-4DF4-11DD-BD17-0019B9312238/39983702-960A-E711-80DD-00155D018C04#/SiteMapHome/39983702-960a-e711-80dd-00155d018c04

                        return $"/designer/sitemap/{Solution.Schema.InstancesUniqueId.DefaultId}/{objectId}#/SiteMapHome/{objectId}";
                    }

                case ComponentType.AttributeMap:
                    {
                        var attributeMap = _service.RetrieveByQuery<AttributeMap>(AttributeMap.EntityLogicalName, objectId, new ColumnSet(AttributeMap.Schema.Attributes.entitymapid));

                        if (attributeMap != null && attributeMap.EntityMapId != null)
                        {
                            return this.GetSolutionComponentRelativeUrl(ComponentType.EntityMap, attributeMap.EntityMapId.Id);
                        }
                    }
                    break;

                case ComponentType.DisplayStringMap:
                    {
                        var displayMap = _service.RetrieveByQuery<DisplayStringMap>(DisplayStringMap.EntityLogicalName, objectId, new ColumnSet(DisplayStringMap.Schema.Attributes.displaystringid));

                        if (displayMap != null && displayMap.DisplayStringId.HasValue)
                        {
                            return this.GetSolutionComponentRelativeUrl(ComponentType.DisplayString, displayMap.DisplayStringId.Value);
                        }
                    }
                    break;

                case ComponentType.RolePrivileges:
                    {
                        var entity = _service.RetrieveByQuery<RolePrivileges>(RolePrivileges.EntityLogicalName, objectId, new ColumnSet(RolePrivileges.Schema.Attributes.roleid));

                        if (entity != null && entity.RoleId.HasValue)
                        {
                            return this.GetSolutionComponentRelativeUrl(ComponentType.Role, entity.RoleId.Value);
                        }
                    }
                    break;

                case ComponentType.FieldPermission:
                    {
                        var entity = _service.RetrieveByQuery<FieldPermission>(FieldPermission.EntityLogicalName, objectId, new ColumnSet(FieldPermission.Schema.Attributes.fieldsecurityprofileid));

                        if (entity != null && entity.FieldSecurityProfileId != null)
                        {
                            return this.GetSolutionComponentRelativeUrl(ComponentType.FieldSecurityProfile, entity.FieldSecurityProfileId.Id);
                        }
                    }
                    break;

                case ComponentType.ReportEntity:
                    {
                        var entity = _service.RetrieveByQuery<ReportEntity>(ReportEntity.EntityLogicalName, objectId, new ColumnSet(ReportEntity.Schema.Attributes.reportid));

                        if (entity != null && entity.ReportId != null)
                        {
                            return this.GetSolutionComponentRelativeUrl(ComponentType.Report, entity.ReportId.Id);
                        }
                    }
                    break;

                case ComponentType.ReportCategory:
                    {
                        var entity = _service.RetrieveByQuery<ReportCategory>(ReportCategory.EntityLogicalName, objectId, new ColumnSet(ReportCategory.Schema.Attributes.reportid));

                        if (entity != null && entity.ReportId != null)
                        {
                            return this.GetSolutionComponentRelativeUrl(ComponentType.Report, entity.ReportId.Id);
                        }
                    }
                    break;

                case ComponentType.ReportVisibility:
                    {
                        var entity = _service.RetrieveByQuery<ReportVisibility>(ReportVisibility.EntityLogicalName, objectId, new ColumnSet(ReportVisibility.Schema.Attributes.reportid));

                        if (entity != null && entity.ReportId != null)
                        {
                            return this.GetSolutionComponentRelativeUrl(ComponentType.Report, entity.ReportId.Id);
                        }
                    }
                    break;

                case ComponentType.AppModule:
                    {
                        var respositorySolution = new SolutionRepository(this._service);

                        // /designer/app/47FB3607-13DA-E811-8114-001DD8B71D68/7A536683-A60D-E811-8105-001DD8B71D68#/AppDesignerCanvas/7a536683-a60d-e811-8105-001dd8b71d68

                        return $"/designer/app/{Solution.Schema.InstancesUniqueId.DefaultId}/{objectId}#/AppDesignerCanvas/{objectId}";
                    }

                case ComponentType.AppModuleRoles:
                    {
                        var appModuleRoles = _service.RetrieveByQuery<AppModuleRoles>(AppModuleRoles.EntityLogicalName, objectId, new ColumnSet(AppModuleRoles.Schema.Attributes.appmoduleid));

                        if (appModuleRoles != null && appModuleRoles.AppModuleId != null)
                        {
                            return this.GetSolutionComponentRelativeUrl(ComponentType.AppModule, appModuleRoles.AppModuleId.Id);
                        }
                    }
                    break;

                    //case ComponentType.RibbonCommand:
                    //   return $"";
                    //case ComponentType.RibbonContextGroup:
                    //   return $"";
                    //case ComponentType.RibbonCustomization:
                    //   return $"";
                    //case ComponentType.RibbonRule:
                    //   return $"";
                    //case ComponentType.RibbonTabToCommandMap:
                    //   return $"";
                    //case ComponentType.RibbonDiff:
                    //   return $"";

                    //case ComponentType.SLAItem:
                    //   return $"";











                    //case ComponentType.Relationship:
                    //   return $"";
                    //case ComponentType.AttributePicklistValue:
                    //   return $"";
                    //case ComponentType.AttributeLookupValue:
                    //   return $"";
                    //case ComponentType.ViewAttribute:
                    //   return $"";
                    //case ComponentType.LocalizedLabel:
                    //   return $"";
                    //case ComponentType.RelationshipExtraCondition:
                    //   return $"";

                    //case ComponentType.EntityRelationshipRole:
                    //   return $"";
                    //case ComponentType.EntityRelationshipRelationships:
                    //   return $"";

                    //case ComponentType.ManagedProperty:
                    //   return $"";

                    //case ComponentType.Form:
                    //   return $"";
                    //case ComponentType.Organization:
                    //   return $"";

                    //case ComponentType.Attachment:
                    //   return $"";

                    //case ComponentType.PluginType:
                    //   return $"";
                    //case ComponentType.PluginAssembly:
                    //   return $"";

                    //case ComponentType.SDKMessageProcessingStep:
                    //   return $"";
                    //case ComponentType.SDKMessageProcessingStepImage:
                    //   return $"";

                    //case ComponentType.ServiceEndpoint:
                    //   return $"";
                    //case ComponentType.RoutingRule:
                    //   return $"";
                    //case ComponentType.RoutingRuleItem:
                    //   return $"";

                    //case ComponentType.ConvertRule:
                    //   return $"";
                    //case ComponentType.ConvertRuleItem:
                    //   return $"";
                    //case ComponentType.HierarchyRule:
                    //   return $"";
                    //case ComponentType.MobileOfflineProfileItem:
                    //   return $"";
                    //case ComponentType.SimilarityRule:
                    //   return $"";
            }

            return this._service.ConnectionData.GetSolutionComponentRelativeUrl(componentType, objectId);
        }
    }
}
