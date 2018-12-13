using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RolePrivilegeViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly string[] _names =
        {
            nameof(IsChanged)
            , nameof(CreateRight)
            , nameof(ReadRight)
            , nameof(UpdateRight)
            , nameof(DeleteRight)
            , nameof(AppendRight)
            , nameof(AppendToRight)
            , nameof(ShareRight)
            , nameof(AssignRight)
        };

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

        public bool AvailableCreate => _availablePrivilegesTypes.Contains(PrivilegeType.Create);
        public bool AvailableRead => _availablePrivilegesTypes.Contains(PrivilegeType.Read);
        public bool AvailableUpdate => _availablePrivilegesTypes.Contains(PrivilegeType.Write);
        public bool AvailableDelete => _availablePrivilegesTypes.Contains(PrivilegeType.Delete);
        public bool AvailableAppend => _availablePrivilegesTypes.Contains(PrivilegeType.Append);
        public bool AvailableAppendTo => _availablePrivilegesTypes.Contains(PrivilegeType.AppendTo);
        public bool AvailableShare => _availablePrivilegesTypes.Contains(PrivilegeType.Share);
        public bool AvailableAssign => _availablePrivilegesTypes.Contains(PrivilegeType.Assign);

        public RolePrivilegeViewItem(Role role, IEnumerable<RolePrivileges> rolePrivileges, IEnumerable<SecurityPrivilegeMetadata> entityPrivileges)
        {
            LoadData(role, rolePrivileges, entityPrivileges);
        }

        public SecurityPrivilegeMetadata[] EntityPrivileges { get; private set; }

        private HashSet<PrivilegeType> _availablePrivilegesTypes = new HashSet<PrivilegeType>();

        public void LoadData(Role role, IEnumerable<RolePrivileges> rolePrivileges, IEnumerable<SecurityPrivilegeMetadata> entityPrivileges)
        {
            this.Role = role;
            this.EntityPrivileges = entityPrivileges.ToArray();
            this._availablePrivilegesTypes.Clear();

            this._CreateRight = this._initialCreate = GetPrivilegeLevel(rolePrivileges, entityPrivileges, PrivilegeType.Create);
            this._ReadRight = this._initialRead = GetPrivilegeLevel(rolePrivileges, entityPrivileges, PrivilegeType.Read);
            this._UpdateRight = this._initialUpdate = GetPrivilegeLevel(rolePrivileges, entityPrivileges, PrivilegeType.Write);
            this._DeleteRight = this._initialDelete = GetPrivilegeLevel(rolePrivileges, entityPrivileges, PrivilegeType.Delete);

            this._AppendRight = this._initialAppend = GetPrivilegeLevel(rolePrivileges, entityPrivileges, PrivilegeType.Append);
            this._AppendToRight = this._initialAppendTo = GetPrivilegeLevel(rolePrivileges, entityPrivileges, PrivilegeType.AppendTo);

            this._ShareRight = this._initialShare = GetPrivilegeLevel(rolePrivileges, entityPrivileges, PrivilegeType.Share);
            this._AssignRight = this._initialAssign = GetPrivilegeLevel(rolePrivileges, entityPrivileges, PrivilegeType.Assign);

            this._IsChanged = false;
        }

        private PrivilegeDepthExtended GetPrivilegeLevel(IEnumerable<RolePrivileges> rolePrivileges, IEnumerable<SecurityPrivilegeMetadata> entityPrivileges, PrivilegeType privilegeType)
        {
            if (privilegeType != PrivilegeType.None)
            {
                var privilege = entityPrivileges.FirstOrDefault(p => p.PrivilegeType == privilegeType);

                if (privilege != null)
                {
                    this._availablePrivilegesTypes.Add(privilegeType);

                    var rolePrivilege = rolePrivileges.FirstOrDefault(p => p.PrivilegeId == privilege.PrivilegeId);

                    if (rolePrivilege != null && rolePrivilege.PrivilegeDepthMask.HasValue)
                    {
                        var privilegeDepth = RolePrivilegesRepository.ConvertMaskToPrivilegeDepth(rolePrivilege.PrivilegeDepthMask.Value);

                        if (privilegeDepth.HasValue)
                        {
                            return (PrivilegeDepthExtended)privilegeDepth.Value;
                        }
                    }
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
                this.IsChanged = CalculateIsChanged();
            }
        }

        private bool CalculateIsChanged()
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

        public PrivilegeDepthExtended[] CreateOptions => ReturnOptions(PrivilegeType.Create);

        public PrivilegeDepthExtended[] ReadOptions => ReturnOptions(PrivilegeType.Read);

        public PrivilegeDepthExtended[] UpdateOptions => ReturnOptions(PrivilegeType.Write);

        public PrivilegeDepthExtended[] DeleteOptions => ReturnOptions(PrivilegeType.Delete);

        public PrivilegeDepthExtended[] AppendOptions => ReturnOptions(PrivilegeType.Append);

        public PrivilegeDepthExtended[] AppendToOptions => ReturnOptions(PrivilegeType.AppendTo);

        public PrivilegeDepthExtended[] AssignOptions => ReturnOptions(PrivilegeType.Assign);

        public PrivilegeDepthExtended[] ShareOptions => ReturnOptions(PrivilegeType.Share);

        private static PrivilegeDepthExtended[] _optionsDefault = new PrivilegeDepthExtended[] { PrivilegeDepthExtended.None };

        private static ConcurrentDictionary<Tuple<bool, bool, bool, bool>, PrivilegeDepthExtended[]> _optionsCache = new ConcurrentDictionary<Tuple<bool, bool, bool, bool>, PrivilegeDepthExtended[]>();

        private PrivilegeDepthExtended[] ReturnOptions(PrivilegeType privilegeType)
        {
            if (privilegeType != PrivilegeType.None && this.EntityPrivileges != null)
            {
                var privilege = this.EntityPrivileges.FirstOrDefault(p => p.PrivilegeType == privilegeType);

                if (privilege != null)
                {
                    var key = Tuple.Create(privilege.CanBeBasic, privilege.CanBeLocal, privilege.CanBeDeep, privilege.CanBeGlobal);

                    if (_optionsCache.ContainsKey(key))
                    {
                        return _optionsCache[key];
                    }

                    PrivilegeDepthExtended[] result = ConstructNewArray(privilege.CanBeBasic, privilege.CanBeLocal, privilege.CanBeDeep, privilege.CanBeGlobal);

                    _optionsCache.TryAdd(key, result);

                    return result;
                }
            }

            return _optionsDefault;
        }

        private static PrivilegeDepthExtended[] ConstructNewArray(bool canBeBasic, bool canBeLocal, bool canBeDeep, bool canBeGlobal)
        {
            List<PrivilegeDepthExtended> result = new List<PrivilegeDepthExtended>() { PrivilegeDepthExtended.None };

            if (canBeBasic)
            {
                result.Add(PrivilegeDepthExtended.Basic);
            }

            if (canBeLocal)
            {
                result.Add(PrivilegeDepthExtended.Local);
            }

            if (canBeDeep)
            {
                result.Add(PrivilegeDepthExtended.Deep);
            }

            if (canBeGlobal)
            {
                result.Add(PrivilegeDepthExtended.Global);
            }

            return result.ToArray();
        }

        public List<RolePrivilege> GetAddRolePrivilege()
        {
            List<RolePrivilege> result = new List<RolePrivilege>();

            FillAddPrivilege(this._initialCreate, this._CreateRight, PrivilegeType.Create, result);
            FillAddPrivilege(this._initialRead, this._ReadRight, PrivilegeType.Read, result);
            FillAddPrivilege(this._initialUpdate, this._UpdateRight, PrivilegeType.Write, result);
            FillAddPrivilege(this._initialDelete, this._DeleteRight, PrivilegeType.Delete, result);
            FillAddPrivilege(this._initialAppend, this._AppendRight, PrivilegeType.Append, result);
            FillAddPrivilege(this._initialAppendTo, this._AppendToRight, PrivilegeType.AppendTo, result);
            FillAddPrivilege(this._initialAssign, this._AssignRight, PrivilegeType.Assign, result);
            FillAddPrivilege(this._initialShare, this._ShareRight, PrivilegeType.Share, result);

            return result;
        }

        private void FillAddPrivilege(PrivilegeDepthExtended initialValue
            , PrivilegeDepthExtended currentValue
            , PrivilegeType privilegeType
            , List<RolePrivilege> privilegesAdd
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
                privilegesAdd.Add(new RolePrivilege()
                {
                    PrivilegeId = privilege.PrivilegeId,
                    Depth = (PrivilegeDepth)currentValue,
                });
            }
        }

        public List<RolePrivilege> GetRemoveRolePrivilege()
        {
            List<RolePrivilege> result = new List<RolePrivilege>();

            FillRemovePrivilege(this._initialCreate, this._CreateRight, PrivilegeType.Create, result);
            FillRemovePrivilege(this._initialRead, this._ReadRight, PrivilegeType.Read, result);
            FillRemovePrivilege(this._initialUpdate, this._UpdateRight, PrivilegeType.Write, result);
            FillRemovePrivilege(this._initialDelete, this._DeleteRight, PrivilegeType.Delete, result);
            FillRemovePrivilege(this._initialAppend, this._AppendRight, PrivilegeType.Append, result);
            FillRemovePrivilege(this._initialAppendTo, this._AppendToRight, PrivilegeType.AppendTo, result);
            FillRemovePrivilege(this._initialAssign, this._AssignRight, PrivilegeType.Assign, result);
            FillRemovePrivilege(this._initialShare, this._ShareRight, PrivilegeType.Share, result);

            return result;
        }

        private void FillRemovePrivilege(PrivilegeDepthExtended initialValue
            , PrivilegeDepthExtended currentValue
            , PrivilegeType privilegeType
            , List<RolePrivilege> privilegesRemove
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
                privilegesRemove.Add(new RolePrivilege()
                {
                    PrivilegeId = privilege.PrivilegeId,
                });
            }
        }
    }
}