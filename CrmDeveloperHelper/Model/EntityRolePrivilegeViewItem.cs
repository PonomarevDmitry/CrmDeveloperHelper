using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityRolePrivilegeViewItem : BaseEntityPrivilegeViewItem
    {
        public Role Role { get; private set; }

        public string RoleName => Role.Name;

        public string BusinessUnitName => Role.BusinessUnitId?.Name;

        public string RoleTemplateName => Role.RoleTemplateName;

        public bool IsManaged => (Role?.IsManaged).GetValueOrDefault();

        public override bool IsCustomizable => (Role?.IsCustomizable?.Value).GetValueOrDefault();

        public bool IsCustomizableCanBeChanged => (Role?.IsCustomizable?.CanBeChanged).GetValueOrDefault();

        public EntityRolePrivilegeViewItem(Role role, IEnumerable<RolePrivileges> rolePrivileges, IEnumerable<SecurityPrivilegeMetadata> entityPrivileges)
        {
            this.Role = role;

            LoadData(rolePrivileges, entityPrivileges);
        }

        public List<RolePrivilege> GetAddRolePrivilege()
        {
            Dictionary<Guid, PrivilegeDepth> result = new Dictionary<Guid, PrivilegeDepth>();

            FillAddPrivilege(this.InitialCreateRight, this._createRight, PrivilegeType.Create, result);
            FillAddPrivilege(this.InitialReadRight, this._readRight, PrivilegeType.Read, result);
            FillAddPrivilege(this.InitialUpdateRight, this._updateRight, PrivilegeType.Write, result);
            FillAddPrivilege(this.InitialDeleteRight, this._deleteRight, PrivilegeType.Delete, result);
            FillAddPrivilege(this.InitialAppendRight, this._appendRight, PrivilegeType.Append, result);
            FillAddPrivilege(this.InitialAppendToRight, this._appendToRight, PrivilegeType.AppendTo, result);
            FillAddPrivilege(this.InitialAssignRight, this._assignRight, PrivilegeType.Assign, result);
            FillAddPrivilege(this.InitialShareRight, this._shareRight, PrivilegeType.Share, result);

            return result.Select(d => new RolePrivilege()
            {
                PrivilegeId = d.Key,
                Depth = d.Value,
            }).ToList();
        }

        private void FillAddPrivilege(PrivilegeDepthExtended initialValue
            , PrivilegeDepthExtended currentValue
            , PrivilegeType privilegeType
            , Dictionary<Guid, PrivilegeDepth> privilegesAdd
        )
        {
            if (currentValue == initialValue)
            {
                return;
            }

            if (privilegeType == PrivilegeType.None
                || EntityPrivileges == null
                || !EntityPrivileges.Any()
            )
            {
                return;
            }

            var privilege = EntityPrivileges.FirstOrDefault(p => p.PrivilegeType == privilegeType);

            if (privilege == null)
            {
                return;
            }

            if (currentValue != PrivilegeDepthExtended.None)
            {
                if (privilegesAdd.ContainsKey(privilege.PrivilegeId))
                {
                    privilegesAdd[privilege.PrivilegeId] = (PrivilegeDepth)Math.Max((int)currentValue, (int)privilegesAdd[privilege.PrivilegeId]);
                }
                else
                {
                    privilegesAdd.Add(privilege.PrivilegeId, (PrivilegeDepth)currentValue);
                }
            }
        }

        public List<RolePrivilege> GetRemoveRolePrivilege()
        {
            HashSet<Guid> result = new HashSet<Guid>();

            FillRemovePrivilege(this.InitialCreateRight, this._createRight, PrivilegeType.Create, result);
            FillRemovePrivilege(this.InitialReadRight, this._readRight, PrivilegeType.Read, result);
            FillRemovePrivilege(this.InitialUpdateRight, this._updateRight, PrivilegeType.Write, result);
            FillRemovePrivilege(this.InitialDeleteRight, this._deleteRight, PrivilegeType.Delete, result);
            FillRemovePrivilege(this.InitialAppendRight, this._appendRight, PrivilegeType.Append, result);
            FillRemovePrivilege(this.InitialAppendToRight, this._appendToRight, PrivilegeType.AppendTo, result);
            FillRemovePrivilege(this.InitialAssignRight, this._assignRight, PrivilegeType.Assign, result);
            FillRemovePrivilege(this.InitialShareRight, this._shareRight, PrivilegeType.Share, result);

            return result.Select(p => new RolePrivilege()
            {
                PrivilegeId = p,
            }).ToList();
        }

        private void FillRemovePrivilege(PrivilegeDepthExtended initialValue
            , PrivilegeDepthExtended currentValue
            , PrivilegeType privilegeType
            , HashSet<Guid> privilegesRemove
        )
        {
            if (currentValue == initialValue)
            {
                return;
            }

            if (privilegeType == PrivilegeType.None
                || EntityPrivileges == null
                || !EntityPrivileges.Any()
                )
            {
                return;
            }

            var privilege = EntityPrivileges.FirstOrDefault(p => p.PrivilegeType == privilegeType);

            if (privilege == null)
            {
                return;
            }

            if (currentValue == PrivilegeDepthExtended.None)
            {
                privilegesRemove.Add(privilege.PrivilegeId);
            }
        }
    }
}