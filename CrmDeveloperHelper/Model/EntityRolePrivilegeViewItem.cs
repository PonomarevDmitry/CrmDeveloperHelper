using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityRolePrivilegeViewItem : SinglePrivilegeViewItem
    {
        public Role Role { get; private set; }

        public string RoleName => Role.Name;

        public string BusinessUnitName => Role.BusinessUnitId?.Name;

        public string RoleTemplateName => Role.RoleTemplateName;

        public bool IsManaged => (Role?.IsManaged).GetValueOrDefault();

        public bool IsCustomizable => (Role?.IsCustomizable?.Value).GetValueOrDefault();

        public bool IsCustomizableCanBeChanged => (Role?.IsCustomizable?.CanBeChanged).GetValueOrDefault();

        private PrivilegeDepthExtended _initialCreate;
        private PrivilegeDepthExtended _initialRead;
        private PrivilegeDepthExtended _initialUpdate;
        private PrivilegeDepthExtended _initialDelete;
        private PrivilegeDepthExtended _initialAppend;
        private PrivilegeDepthExtended _initialAppendTo;
        private PrivilegeDepthExtended _initialShare;
        private PrivilegeDepthExtended _initialAssign;

        public bool AvailableCreate => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Create);
        public bool AvailableRead => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Read);
        public bool AvailableUpdate => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Write);
        public bool AvailableDelete => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Delete);
        public bool AvailableAppend => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Append);
        public bool AvailableAppendTo => _availablePrivilegesTypes.ContainsKey(PrivilegeType.AppendTo);
        public bool AvailableShare => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Share);
        public bool AvailableAssign => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Assign);

        public EntityRolePrivilegeViewItem(Role role, IEnumerable<RolePrivileges> rolePrivileges, IEnumerable<SecurityPrivilegeMetadata> entityPrivileges)
        {
            LoadData(role, rolePrivileges, entityPrivileges);
        }

        public IEnumerable<SecurityPrivilegeMetadata> EntityPrivileges { get; private set; }

        private Dictionary<PrivilegeType, SecurityPrivilegeMetadata> _availablePrivilegesTypes = new Dictionary<PrivilegeType, SecurityPrivilegeMetadata>();

        public void LoadData(Role role, IEnumerable<RolePrivileges> rolePrivileges, IEnumerable<SecurityPrivilegeMetadata> entityPrivileges)
        {
            this.Role = role;

            this.EntityPrivileges = entityPrivileges.ToArray();

            this._availablePrivilegesTypes.Clear();

            this._CreateRight = this._initialCreate = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Create);
            this._ReadRight = this._initialRead = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Read);
            this._UpdateRight = this._initialUpdate = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Write);
            this._DeleteRight = this._initialDelete = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Delete);

            this._AppendRight = this._initialAppend = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Append);
            this._AppendToRight = this._initialAppendTo = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.AppendTo);

            this._ShareRight = this._initialShare = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Share);
            this._AssignRight = this._initialAssign = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Assign);

            this.OnPropertyChanging(nameof(IsChanged));
            this.IsChanged = false;
            this.OnPropertyChanged(nameof(IsChanged));
        }

        protected override bool CalculateIsChanged()
        {
            if (_initialCreate != _CreateRight)
            {
                return true;
            }

            if (_initialRead != _ReadRight)
            {
                return true;
            }

            if (_initialUpdate != _UpdateRight)
            {
                return true;
            }

            if (_initialDelete != _DeleteRight)
            {
                return true;
            }

            if (_initialAppend != _AppendRight)
            {
                return true;
            }

            if (_initialAppendTo != _AppendToRight)
            {
                return true;
            }

            if (_initialShare != _ShareRight)
            {
                return true;
            }

            if (_initialAssign != _AssignRight)
            {
                return true;
            }

            return false;
        }

        private PrivilegeDepthExtended _CreateRight;
        public PrivilegeDepthExtended CreateRight
        {
            get => AvailableCreate ? _CreateRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableCreate || !IsCustomizable || _CreateRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CreateRight));
                this._CreateRight = value;
                this.OnPropertyChanged(nameof(CreateRight));
            }
        }

        private PrivilegeDepthExtended _ReadRight;
        public PrivilegeDepthExtended ReadRight
        {
            get => AvailableRead ? _ReadRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableRead || !IsCustomizable || _ReadRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ReadRight));
                this._ReadRight = value;
                this.OnPropertyChanged(nameof(ReadRight));
            }
        }

        private PrivilegeDepthExtended _UpdateRight;
        public PrivilegeDepthExtended UpdateRight
        {
            get => AvailableUpdate ? _UpdateRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableUpdate || !IsCustomizable || _UpdateRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(UpdateRight));
                this._UpdateRight = value;
                this.OnPropertyChanged(nameof(UpdateRight));
            }
        }

        private PrivilegeDepthExtended _DeleteRight;
        public PrivilegeDepthExtended DeleteRight
        {
            get => AvailableDelete ? _DeleteRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableDelete || !IsCustomizable || _DeleteRight == value
                    )
                {
                    return;
                }

                this.OnPropertyChanging(nameof(DeleteRight));
                this._DeleteRight = value;
                this.OnPropertyChanged(nameof(DeleteRight));
            }
        }

        private PrivilegeDepthExtended _AppendRight;
        public PrivilegeDepthExtended AppendRight
        {
            get => AvailableAppend ? _AppendRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAppend || !IsCustomizable || _AppendRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AppendRight));
                this._AppendRight = value;
                this.OnPropertyChanged(nameof(AppendRight));
            }
        }

        private PrivilegeDepthExtended _AppendToRight;
        public PrivilegeDepthExtended AppendToRight
        {
            get => AvailableAppendTo ? _AppendToRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAppendTo || !IsCustomizable || _AppendToRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AppendToRight));
                this._AppendToRight = value;
                this.OnPropertyChanged(nameof(AppendToRight));
            }
        }

        private PrivilegeDepthExtended _ShareRight;
        public PrivilegeDepthExtended ShareRight
        {
            get => AvailableShare ? _ShareRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableShare || !IsCustomizable || _ShareRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ShareRight));
                this._ShareRight = value;
                this.OnPropertyChanged(nameof(ShareRight));
            }
        }

        private PrivilegeDepthExtended _AssignRight;
        public PrivilegeDepthExtended AssignRight
        {
            get => AvailableAssign ? _AssignRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAssign || !IsCustomizable || _AssignRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AssignRight));
                this._AssignRight = value;
                this.OnPropertyChanged(nameof(AssignRight));
            }
        }

        public IEnumerable<PrivilegeDepthExtended> CreateOptions => ReturnOptions(PrivilegeType.Create, this.EntityPrivileges);

        public IEnumerable<PrivilegeDepthExtended> ReadOptions => ReturnOptions(PrivilegeType.Read, this.EntityPrivileges);

        public IEnumerable<PrivilegeDepthExtended> UpdateOptions => ReturnOptions(PrivilegeType.Write, this.EntityPrivileges);

        public IEnumerable<PrivilegeDepthExtended> DeleteOptions => ReturnOptions(PrivilegeType.Delete, this.EntityPrivileges);

        public IEnumerable<PrivilegeDepthExtended> AppendOptions => ReturnOptions(PrivilegeType.Append, this.EntityPrivileges);

        public IEnumerable<PrivilegeDepthExtended> AppendToOptions => ReturnOptions(PrivilegeType.AppendTo, this.EntityPrivileges);

        public IEnumerable<PrivilegeDepthExtended> AssignOptions => ReturnOptions(PrivilegeType.Assign, this.EntityPrivileges);

        public IEnumerable<PrivilegeDepthExtended> ShareOptions => ReturnOptions(PrivilegeType.Share, this.EntityPrivileges);

        public List<RolePrivilege> GetAddRolePrivilege()
        {
            Dictionary<Guid, PrivilegeDepth> result = new Dictionary<Guid, PrivilegeDepth>();

            FillAddPrivilege(this._initialCreate, this._CreateRight, PrivilegeType.Create, result);
            FillAddPrivilege(this._initialRead, this._ReadRight, PrivilegeType.Read, result);
            FillAddPrivilege(this._initialUpdate, this._UpdateRight, PrivilegeType.Write, result);
            FillAddPrivilege(this._initialDelete, this._DeleteRight, PrivilegeType.Delete, result);
            FillAddPrivilege(this._initialAppend, this._AppendRight, PrivilegeType.Append, result);
            FillAddPrivilege(this._initialAppendTo, this._AppendToRight, PrivilegeType.AppendTo, result);
            FillAddPrivilege(this._initialAssign, this._AssignRight, PrivilegeType.Assign, result);
            FillAddPrivilege(this._initialShare, this._ShareRight, PrivilegeType.Share, result);

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

            FillRemovePrivilege(this._initialCreate, this._CreateRight, PrivilegeType.Create, result);
            FillRemovePrivilege(this._initialRead, this._ReadRight, PrivilegeType.Read, result);
            FillRemovePrivilege(this._initialUpdate, this._UpdateRight, PrivilegeType.Write, result);
            FillRemovePrivilege(this._initialDelete, this._DeleteRight, PrivilegeType.Delete, result);
            FillRemovePrivilege(this._initialAppend, this._AppendRight, PrivilegeType.Append, result);
            FillRemovePrivilege(this._initialAppendTo, this._AppendToRight, PrivilegeType.AppendTo, result);
            FillRemovePrivilege(this._initialAssign, this._AssignRight, PrivilegeType.Assign, result);
            FillRemovePrivilege(this._initialShare, this._ShareRight, PrivilegeType.Share, result);

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