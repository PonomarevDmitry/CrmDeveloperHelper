using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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
#warning ToDo
                        //return $"/main.aspx?extraqs=etc={linkedEntityObjectCode}&id={objectId}&pagetype=vizdesigner";
                    }
                    break;

                case ComponentType.SystemForm:
                    break;

                //case ComponentType.AttributeMap:
                //   return $"";

                //case ComponentType.FieldPermission:
                //   return $"";

                //case ComponentType.RolePrivileges:
                //   return $"";

                //case ComponentType.DisplayStringMap:
                //   return $"";
                //case ComponentType.ReportEntity:
                //   return $"";
                //case ComponentType.ReportCategory:
                //   return $"";
                //case ComponentType.ReportVisibility:
                //   return $"";

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

                //case ComponentType.SiteMap:
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
