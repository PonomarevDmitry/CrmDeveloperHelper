using Microsoft.Crm.Sdk.Messages;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleOtherPrivilegeViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly string[] _names =
        {
            nameof(IsChanged)
            , nameof(Right)
        };

        public Privilege Privilege { get; private set; }

        public Role Role { get; private set; }

        public string RoleName => Role.Name;

        public string BusinessUnitName => Role.BusinessUnitId?.Name;

        public string RoleTemplateName => Role.RoleTemplateName;

        public bool IsManaged => (Role?.IsManaged).GetValueOrDefault();

        public bool IsCustomizable => (Role?.IsCustomizable?.Value).GetValueOrDefault();

        public bool IsCustomizableCanBeChanged => (Role?.IsCustomizable?.CanBeChanged).GetValueOrDefault();

        private PrivilegeDepthExtended _initialRight;

        public RoleOtherPrivilegeViewItem(Role role, IEnumerable<RolePrivileges> rolePrivileges, Privilege privilege)
        {
            LoadData(role, rolePrivileges, privilege);
        }

        public void LoadData(Role role, IEnumerable<RolePrivileges> rolePrivileges, Privilege privilege)
        {
            this.Role = role;
            this.Privilege = privilege;

            this._Right = this._initialRight = GetPrivilegeLevel(rolePrivileges);

            this.OnPropertyChanging(nameof(IsChanged));
            this.IsChanged = false;
            this.OnPropertyChanged(nameof(IsChanged));
        }

        private PrivilegeDepthExtended GetPrivilegeLevel(IEnumerable<RolePrivileges> rolePrivileges)
        {
            var rolePrivilege = rolePrivileges.FirstOrDefault(p => p.PrivilegeId == this.Privilege.PrivilegeId);

            if (rolePrivilege != null && rolePrivilege.PrivilegeDepthMask.HasValue)
            {
                var privilegeDepth = RolePrivilegesRepository.ConvertMaskToPrivilegeDepth(rolePrivilege.PrivilegeDepthMask.Value);

                if (privilegeDepth.HasValue)
                {
                    return (PrivilegeDepthExtended)privilegeDepth.Value;
                }
            }

            return PrivilegeDepthExtended.None;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!string.Equals(propertyName, nameof(IsChanged)))
            {
                var val = CalculateIsChanged();

                if (val != this.IsChanged)
                {
                    this.OnPropertyChanging(nameof(IsChanged));
                    this.IsChanged = val;
                    this.OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        private bool CalculateIsChanged()
        {
            if (_initialRight != _Right)
            {
                return true;
            }

            return false;
        }

        private void OnPropertyChanging(string propertyName)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public bool IsChanged { get; private set; }

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

        public IEnumerable<PrivilegeDepthExtended> RightOptions => EntityPrivilegeViewItem.GetPrivilegeDepthsByAvailability(Privilege.CanBeBasic.GetValueOrDefault(), Privilege.CanBeLocal.GetValueOrDefault(), Privilege.CanBeDeep.GetValueOrDefault(), Privilege.CanBeGlobal.GetValueOrDefault());

        private static PrivilegeDepthExtended[] _optionsDefault = new PrivilegeDepthExtended[] { PrivilegeDepthExtended.None };

        public List<RolePrivilege> GetAddRolePrivilege()
        {
            Dictionary<Guid, PrivilegeDepth> privilegesAdd = new Dictionary<Guid, PrivilegeDepth>();

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
            HashSet<Guid> privilegesRemove = new HashSet<Guid>();

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