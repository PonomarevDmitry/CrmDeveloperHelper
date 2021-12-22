using Microsoft.Crm.Sdk.Messages;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class OtherPrivilegeRolePrivilegeViewItem : BaseSinglePrivilegeViewItem
    {
        public Role Role { get; private set; }

        public Privilege Privilege { get; private set; }

        public string RoleName => Role.Name;

        public string BusinessUnitName => Role.BusinessUnitId?.Name;

        public string RoleTemplateName => Role.RoleTemplateName;

        public bool IsManaged => (Role?.IsManaged).GetValueOrDefault();

        public bool IsCustomizable => (Role?.IsCustomizable?.Value).GetValueOrDefault();

        public bool IsCustomizableCanBeChanged => (Role?.IsCustomizable?.CanBeChanged).GetValueOrDefault();

        private PrivilegeDepthExtended _initialRight;

        public OtherPrivilegeRolePrivilegeViewItem(Role role, IEnumerable<RolePrivileges> rolePrivileges, Privilege privilege)
        {
            LoadData(role, rolePrivileges, privilege);
        }

        public void LoadData(Role role, IEnumerable<RolePrivileges> rolePrivileges, Privilege privilege)
        {
            this.Role = role;
            this.Privilege = privilege;

            this._Right = this._initialRight = GetPrivilegeLevel(this.Privilege.PrivilegeId.Value, rolePrivileges);

            this.OnPropertyChanging(nameof(IsChanged));
            this.IsChanged = false;
            this.OnPropertyChanged(nameof(IsChanged));
        }

        protected override bool CalculateIsChanged()
        {
            if (_initialRight != _Right)
            {
                return true;
            }

            return false;
        }

        private PrivilegeDepthExtended _Right;
        public PrivilegeDepthExtended Right
        {
            get => _Right;
            set
            {
                if (_Right == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Right));
                this._Right = value;
                this.OnPropertyChanged(nameof(Right));
            }
        }

        public IEnumerable<PrivilegeDepthExtended> RightOptions => GetPrivilegeDepthsByAvailability(
            Privilege.CanBeBasic.GetValueOrDefault()
            , Privilege.CanBeLocal.GetValueOrDefault()
            , Privilege.CanBeDeep.GetValueOrDefault()
            , Privilege.CanBeGlobal.GetValueOrDefault()
        );

        public List<RolePrivilege> GetAddRolePrivilege()
        {
            var privilegesAdd = new Dictionary<Guid, PrivilegeDepth>();

            FillAddPrivilege(this._initialRight, this._Right, privilegesAdd);

            return privilegesAdd.Select(d => new RolePrivilege()
            {
                PrivilegeId = d.Key,
                Depth = d.Value,
            }).ToList();
        }

        private void FillAddPrivilege(
            PrivilegeDepthExtended initialValue
            , PrivilegeDepthExtended currentValue
            , Dictionary<Guid, PrivilegeDepth> privilegesAdd
        )
        {
            if (currentValue == initialValue || this.Privilege == null)
            {
                return;
            }

            if (currentValue != PrivilegeDepthExtended.None)
            {
                if (privilegesAdd.ContainsKey(this.Privilege.Id))
                {
                    privilegesAdd[this.Privilege.Id] = (PrivilegeDepth)Math.Max((int)currentValue, (int)privilegesAdd[this.Privilege.Id]);
                }
                else
                {
                    privilegesAdd.Add(this.Privilege.Id, (PrivilegeDepth)currentValue);
                }
            }
        }

        public List<RolePrivilege> GetRemoveRolePrivilege()
        {
            var privilegesRemove = new HashSet<Guid>();

            FillRemovePrivilege(this._initialRight, this._Right, privilegesRemove);

            return privilegesRemove.Select(p => new RolePrivilege()
            {
                PrivilegeId = p,
            }).ToList();
        }

        private void FillRemovePrivilege(
            PrivilegeDepthExtended initialValue
            , PrivilegeDepthExtended currentValue
            , HashSet<Guid> privilegesRemove
        )
        {
            if (currentValue == initialValue || this.Privilege == null)
            {
                return;
            }

            if (currentValue == PrivilegeDepthExtended.None)
            {
                privilegesRemove.Add(this.Privilege.Id);
            }
        }
    }
}