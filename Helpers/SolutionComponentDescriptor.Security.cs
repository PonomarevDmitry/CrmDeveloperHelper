using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private void GenerateDescriptionFieldSecurityProfile(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<FieldSecurityProfile>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            var table = new FormatTextTableHandler();
            table.SetHeader("Name", "Description", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var name = entity.Name;

                string desc = entity.Description;

                table.AddLine(name
                    , desc
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionFieldSecurityProfileSingle(FieldSecurityProfile entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string title = entity.Name;

                return string.Format("FieldSecurityProfile {0}    IsManaged {1}    SolutionName {2}"
                    , title
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionFieldPermission(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<FieldPermission>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            var table = new FormatTextTableHandler();
            table.SetHeader("FieldSecurityProfileName", "Attribute", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var name = entity.GetAttributeValue<AliasedValue>("fieldsecurityprofile.name").Value.ToString();

                string attr = string.Format("{0}.{1}"
                    , entity.EntityName
                    , entity.AttributeLogicalName
                    );

                table.AddLine(name
                    , attr
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionFieldPermissionSingle(FieldPermission entity, SolutionComponent component)
        {
            if (entity != null)
            {
                var name = entity.GetAttributeValue<AliasedValue>("fieldsecurityprofile.name").Value.ToString();

                string attr = string.Format("{0}.{1}"
                    , entity.EntityName
                    , entity.AttributeLogicalName
                    );

                return string.Format("FieldSecurityProfile {0}    Attribute {1}    IsManaged {2}    SolutionName {3}"
                    , name
                    , attr
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionRoles(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<Role>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("Name", "BusinessUnit", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var role in list)
            {
                var businessUnit = role.BusinessUnitId.Name;

                if (role.BusinessUnitParentBusinessUnit == null)
                {
                    businessUnit = "Root Organization";
                }

                table.AddLine(role.Name
                    , businessUnit
                    , role.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(role, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(role, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(role, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(role, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionRolesSingle(Entity entity, SolutionComponent component)
        {
            if (entity != null)
            {
                var role = entity.ToEntity<Role>();

                var businessUnit = role.BusinessUnitId.Name;

                if (!role.Attributes.Contains("businessunit.parentbusinessunitid"))
                {
                    businessUnit = "Root Organization";
                }

                return string.Format("Role {0}    BusinessUnit {1}    IsManaged {2}    SolutionName {3}"
                    , role.Name
                    , businessUnit
                    , role.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(role, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionRolePrivileges(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<RolePrivileges>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler table = new FormatTextTableHandler();
            table.SetHeader("Name", "BusinessUnit", "Privilege", "PrivilegeDepthMask", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var rolePriv in list.Select(e => e.ToEntity<RolePrivileges>()))
            {
                var roleName = rolePriv.GetAttributeValue<AliasedValue>("role.name").Value.ToString();
                var businessUnit = ((EntityReference)rolePriv.GetAttributeValue<AliasedValue>("role.businessunitid").Value).Name;

                if (!rolePriv.Attributes.Contains("businessunit.parentbusinessunitid"))
                {
                    businessUnit = "Root Organization";
                }

                table.AddLine(roleName
                    , businessUnit
                    , rolePriv.GetAttributeValue<AliasedValue>("privilege.name").Value.ToString()
                    , SecurityRolePrivilegesRepository.GetPrivilegeDepthMaskName(rolePriv.PrivilegeDepthMask.Value)
                    , rolePriv.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionRolePrivilegesSinge(Entity entity, SolutionComponent component)
        {
            if (entity != null)
            {
                var rolePriv = entity.ToEntity<RolePrivileges>();

                var roleName = rolePriv.GetAttributeValue<AliasedValue>("role.name").Value.ToString();
                var businessUnit = ((EntityReference)rolePriv.GetAttributeValue<AliasedValue>("role.businessunitid").Value).Name;

                if (!rolePriv.Attributes.Contains("businessunit.parentbusinessunitid"))
                {
                    businessUnit = "Root Organization";
                }

                return string.Format("Role {0}    BusinessUnit {1}    Privilege {2}    PrivilegeDepthMask {3}    IsManaged {4}    SolutionName {5}"
                    , roleName
                    , businessUnit
                    , rolePriv.GetAttributeValue<AliasedValue>("privilege.name").Value.ToString()
                    , SecurityRolePrivilegesRepository.GetPrivilegeDepthMaskName(rolePriv.PrivilegeDepthMask.Value)
                    , rolePriv.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(rolePriv, "solution.uniquename")
                    );
            }

            return component.ToString();
        }
    }
}