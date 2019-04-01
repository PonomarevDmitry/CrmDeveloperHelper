using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class OtherPrivilegeViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly string[] _names =
        {
            nameof(IsChanged)
            , nameof(Right)
        };

        public Privilege Privilege { get; private set; }

        public string Name => Privilege.Name;

        public string EntityLogicalName => Privilege.LinkedEntitiesSorted;

        public AccessRights? PrivilegeAccessRights { get; private set; }

        public string PrivilegeType => PrivilegeAccessRights.ToString();

        public bool IsCustomizable { get; private set; }

        private readonly PrivilegeDepthExtended _initialRight;

        public OtherPrivilegeViewItem(Privilege privilege, IEnumerable<RolePrivilege> rolePrivileges, bool isCustomizable = false)
        {
            this.IsCustomizable = isCustomizable;
            this.Privilege = privilege;

            this._Right = this._initialRight = GetPrivilegeLevel(rolePrivileges);

            if (privilege.AccessRight.HasValue 
                && Enum.IsDefined(typeof(AccessRights), privilege.AccessRight.Value)
            )
            {
                this.PrivilegeAccessRights = (AccessRights)privilege.AccessRight.Value;
            }

            this._IsChanged = false;
        }

        private PrivilegeDepthExtended GetPrivilegeLevel(IEnumerable<RolePrivilege> rolePrivileges)
        {
            var rolePrivilege = rolePrivileges.FirstOrDefault(p => p.PrivilegeId == this.Privilege.PrivilegeId);

            if (rolePrivilege != null)
            {
                return (PrivilegeDepthExtended)rolePrivilege.Depth;
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
                this.IsChanged = CalculateIsChanged();
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

        private bool _IsChanged = false;
        public bool IsChanged
        {
            get => _IsChanged;
            set
            {
                if (_IsChanged == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsChanged));
                this._IsChanged = value;
                this.OnPropertyChanged(nameof(IsChanged));
            }
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

        public void FillChangedPrivileges(List<RolePrivilege> privilegesAdd, List<RolePrivilege> privilegesRemove)
        {
            if (this._Right == this._initialRight)
            {
                return;
            }

            if (this._Right == PrivilegeDepthExtended.None)
            {
                privilegesRemove.Add(new RolePrivilege()
                {
                    PrivilegeId = Privilege.PrivilegeId.Value,
                });
            }
            else
            {
                privilegesAdd.Add(new RolePrivilege()
                {
                    PrivilegeId = Privilege.PrivilegeId.Value,
                    Depth = (PrivilegeDepth)this._Right,
                });
            }
        }

        public PrivilegeDepthExtended[] RightOptions => EntityPrivilegeViewItem.GetPrivilegeDepthsByAvailability(Privilege.CanBeBasic.GetValueOrDefault(), Privilege.CanBeLocal.GetValueOrDefault(), Privilege.CanBeDeep.GetValueOrDefault(), Privilege.CanBeGlobal.GetValueOrDefault());
    }
}
