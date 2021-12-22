using Microsoft.Crm.Sdk.Messages;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleLinkedOtherPrivilegeViewItem : BaseLinkedPrivilegeViewItem
    {
        public string Name => (Privilege1 ?? Privilege2).Name;

        public bool IsCustomizable1 { get; private set; }

        public bool IsCustomizable2 { get; private set; }

        public bool RightDifferent => Privilege1 != null && Privilege2 != null && _Right1 != _Right2;

        public RoleLinkedOtherPrivilegeViewItem(
            Privilege privilege1
            , Privilege privilege2
            , IEnumerable<RolePrivilege> rolePrivileges1
            , IEnumerable<RolePrivilege> rolePrivileges2
            , bool isCustomizable1 = false
            , bool isCustomizable2 = false
        )
        {
            this.IsCustomizable1 = isCustomizable1;
            this.IsCustomizable2 = isCustomizable2;

            LoadData1(privilege1, rolePrivileges1);
            LoadData2(privilege2, rolePrivileges2);

            this.OnPropertyChanging(nameof(IsChanged1));
            this.IsChanged1 = false;
            this.OnPropertyChanged(nameof(IsChanged1));

            this.OnPropertyChanging(nameof(IsChanged2));
            this.IsChanged2 = false;
            this.OnPropertyChanged(nameof(IsChanged2));
        }

        #region Privilege1

        protected override bool CalculateIsChanged1()
        {
            if (_initialRight1 != _Right1)
            {
                return true;
            }

            return false;
        }

        public Privilege Privilege1 { get; private set; }

        public string EntityLogicalName1 => Privilege1.LinkedEntitiesSorted;

        public AccessRights? PrivilegeAccessRights1 { get; private set; }

        public string PrivilegeType1 => PrivilegeAccessRights1.ToString();

        private PrivilegeDepthExtended _initialRight1;

        private void LoadData1(Privilege privilege1, IEnumerable<RolePrivilege> rolePrivileges1)
        {
            this.Privilege1 = privilege1;
            this._Right1 = this._initialRight1 = GetPrivilegeLevel(this.Privilege1.PrivilegeId.Value, rolePrivileges1);

            if (privilege1.AccessRight.HasValue
                && Enum.IsDefined(typeof(AccessRights), privilege1.AccessRight.Value)
            )
            {
                this.PrivilegeAccessRights1 = (AccessRights)privilege1.AccessRight.Value;
            }
            else
            {
                this.PrivilegeAccessRights1 = null;
            }
        }

        private PrivilegeDepthExtended _Right1;
        public PrivilegeDepthExtended Right1
        {
            get => _Right1;
            set
            {
                if (_Right1 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Right1));
                this._Right1 = value;
                this.OnPropertyChanged(nameof(Right1));
            }
        }

        public IEnumerable<PrivilegeDepthExtended> RightOptions1 => GetPrivilegeDepthsByAvailability(
            Privilege1.CanBeBasic.GetValueOrDefault()
            , Privilege1.CanBeLocal.GetValueOrDefault()
            , Privilege1.CanBeDeep.GetValueOrDefault()
            , Privilege1.CanBeGlobal.GetValueOrDefault()
        );

        public void FillChangedPrivileges1(Dictionary<Guid, PrivilegeDepth> privilegesAdd, HashSet<Guid> privilegesRemove)
        {
            if (Privilege1 == null)
            {
                return;
            }

            if (this._Right1 == this._initialRight1)
            {
                return;
            }

            if (this._Right1 == PrivilegeDepthExtended.None)
            {
                privilegesRemove.Add(Privilege1.PrivilegeId.Value);
            }
            else
            {
                if (privilegesAdd.ContainsKey(Privilege1.PrivilegeId.Value))
                {
                    privilegesAdd[Privilege1.PrivilegeId.Value] = (PrivilegeDepth)Math.Max((int)this._Right1, (int)privilegesAdd[Privilege1.PrivilegeId.Value]);
                }
                else
                {
                    privilegesAdd.Add(Privilege1.PrivilegeId.Value, (PrivilegeDepth)this._Right1);
                }
            }
        }

        #endregion Privilege1

        #region Privilege2

        protected override bool CalculateIsChanged2()
        {
            if (_initialRight2 != _Right2)
            {
                return true;
            }

            return false;
        }

        public Privilege Privilege2 { get; private set; }

        public string EntityLogicalName2 => Privilege2.LinkedEntitiesSorted;

        public AccessRights? PrivilegeAccessRights2 { get; private set; }

        public string PrivilegeType2 => PrivilegeAccessRights2.ToString();

        private PrivilegeDepthExtended? _initialRight2;

        private void LoadData2(Privilege privilege2, IEnumerable<RolePrivilege> rolePrivileges2)
        {
            this.Privilege2 = privilege2;

            if (privilege2 != null)
            {
                this._Right2 = this._initialRight2 = GetPrivilegeLevel(this.Privilege2.PrivilegeId.Value, rolePrivileges2);

                if (privilege2.AccessRight.HasValue
                    && Enum.IsDefined(typeof(AccessRights), privilege2.AccessRight.Value)
                )
                {
                    this.PrivilegeAccessRights2 = (AccessRights)privilege2.AccessRight.Value;
                }
                else
                {
                    this.PrivilegeAccessRights2 = null;
                }
            }
            else
            {
                this._Right2 = this._initialRight2 = null;

                this.PrivilegeAccessRights2 = null;
            }
        }

        private PrivilegeDepthExtended? _Right2;
        public PrivilegeDepthExtended? Right2
        {
            get => _Right2;
            set
            {
                if (_Right2 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Right2));
                this._Right2 = value;
                this.OnPropertyChanged(nameof(Right2));
            }
        }

        public IEnumerable<PrivilegeDepthExtended> RightOptions2 => GetPrivilegeDepthsByAvailability(
            Privilege2.CanBeBasic.GetValueOrDefault()
            , Privilege2.CanBeLocal.GetValueOrDefault()
            , Privilege2.CanBeDeep.GetValueOrDefault()
            , Privilege2.CanBeGlobal.GetValueOrDefault()
        );

        public void FillChangedPrivileges2(Dictionary<Guid, PrivilegeDepth> privilegesAdd, HashSet<Guid> privilegesRemove)
        {
            if (Privilege2 == null)
            {
                return;
            }

            if (this._Right2 == this._initialRight2)
            {
                return;
            }

            if (this._Right2 == PrivilegeDepthExtended.None)
            {
                privilegesRemove.Add(Privilege2.PrivilegeId.Value);
            }
            else
            {
                if (privilegesAdd.ContainsKey(Privilege2.PrivilegeId.Value))
                {
                    privilegesAdd[Privilege2.PrivilegeId.Value] = (PrivilegeDepth)Math.Max((int)this._Right2, (int)privilegesAdd[Privilege2.PrivilegeId.Value]);
                }
                else
                {
                    privilegesAdd.Add(Privilege2.PrivilegeId.Value, (PrivilegeDepth)this._Right2);
                }
            }
        }

        #endregion Privilege2
    }
}
