using Microsoft.Crm.Sdk.Messages;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleOtherPrivilegeViewItem : SinglePrivilegeViewItem
    {
        public Privilege Privilege { get; private set; }

        public string Name => Privilege.Name;

        public string EntityLogicalName => Privilege.LinkedEntitiesSorted;

        public AccessRights? PrivilegeAccessRights { get; private set; }

        public string PrivilegeType => PrivilegeAccessRights.ToString();

        public bool IsCustomizable { get; private set; }

        private readonly PrivilegeDepthExtended _initialRight;

        public RoleOtherPrivilegeViewItem(Privilege privilege, IEnumerable<RolePrivilege> rolePrivileges, bool isCustomizable = false)
        {
            this.IsCustomizable = isCustomizable;
            this.Privilege = privilege;

            this._Right = this._initialRight = GetPrivilegeLevel(this.Privilege.PrivilegeId.Value, rolePrivileges);

            if (privilege.AccessRight.HasValue
                && Enum.IsDefined(typeof(AccessRights), privilege.AccessRight.Value)
            )
            {
                this.PrivilegeAccessRights = (AccessRights)privilege.AccessRight.Value;
            }

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

        public void FillChangedPrivileges(Dictionary<Guid, PrivilegeDepth> privilegesAdd, HashSet<Guid> privilegesRemove)
        {
            if (this._Right == this._initialRight)
            {
                return;
            }

            if (this._Right == PrivilegeDepthExtended.None)
            {
                privilegesRemove.Add(Privilege.PrivilegeId.Value);
            }
            else
            {
                if (privilegesAdd.ContainsKey(Privilege.PrivilegeId.Value))
                {
                    privilegesAdd[Privilege.PrivilegeId.Value] = (PrivilegeDepth)Math.Max((int)this._Right, (int)privilegesAdd[Privilege.PrivilegeId.Value]);
                }
                else
                {
                    privilegesAdd.Add(Privilege.PrivilegeId.Value, (PrivilegeDepth)this._Right);
                }
            }
        }

        public IEnumerable<PrivilegeDepthExtended> RightOptions => GetPrivilegeDepthsByAvailability(
            Privilege.CanBeBasic.GetValueOrDefault()
            , Privilege.CanBeLocal.GetValueOrDefault()
            , Privilege.CanBeDeep.GetValueOrDefault()
            , Privilege.CanBeGlobal.GetValueOrDefault()
        );
    }
}
