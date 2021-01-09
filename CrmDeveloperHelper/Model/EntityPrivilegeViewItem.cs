using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityPrivilegeViewItem : INotifyPropertyChanging, INotifyPropertyChanged
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

        public EntityMetadata EntityMetadata { get; private set; }

        public string LogicalName => EntityMetadata.LogicalName;

        public bool IsIntersect => EntityMetadata.IsIntersect.GetValueOrDefault();

        public int ObjectTypeCode => EntityMetadata.ObjectTypeCode.GetValueOrDefault();

        public string DisplayName { get; private set; }

        public bool IsCustomizable { get; private set; }

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

        public EntityPrivilegeViewItem(EntityMetadata entityMetadata, IEnumerable<RolePrivilege> rolePrivileges, bool isCustomizable = false)
        {
            this.IsCustomizable = isCustomizable;
            LoadData(entityMetadata, rolePrivileges);
        }

        private Dictionary<PrivilegeType, SecurityPrivilegeMetadata> _availablePrivilegesTypes = new Dictionary<PrivilegeType, SecurityPrivilegeMetadata>();

        public void LoadData(EntityMetadata entityMetadata, IEnumerable<RolePrivilege> rolePrivileges)
        {
            this.EntityMetadata = entityMetadata;
            this._availablePrivilegesTypes.Clear();
            this.DisplayName = CreateFileHandler.GetLocalizedLabel(entityMetadata.DisplayName);

            this._CreateRight = this._initialCreate = GetPrivilegeLevel(rolePrivileges, PrivilegeType.Create);
            this._ReadRight = this._initialRead = GetPrivilegeLevel(rolePrivileges, PrivilegeType.Read);
            this._UpdateRight = this._initialUpdate = GetPrivilegeLevel(rolePrivileges, PrivilegeType.Write);
            this._DeleteRight = this._initialDelete = GetPrivilegeLevel(rolePrivileges, PrivilegeType.Delete);

            this._AppendRight = this._initialAppend = GetPrivilegeLevel(rolePrivileges, PrivilegeType.Append);
            this._AppendToRight = this._initialAppendTo = GetPrivilegeLevel(rolePrivileges, PrivilegeType.AppendTo);

            this._ShareRight = this._initialShare = GetPrivilegeLevel(rolePrivileges, PrivilegeType.Share);
            this._AssignRight = this._initialAssign = GetPrivilegeLevel(rolePrivileges, PrivilegeType.Assign);

            this.OnPropertyChanging(nameof(IsChanged));
            this.IsChanged = false;
            this.OnPropertyChanged(nameof(IsChanged));
        }

        private PrivilegeDepthExtended GetPrivilegeLevel(IEnumerable<RolePrivilege> rolePrivileges, PrivilegeType privilegeType)
        {
            if (privilegeType != PrivilegeType.None
                && EntityMetadata != null
                && EntityMetadata.Privileges != null
                && EntityMetadata.Privileges.Any()
                )
            {
                var privilege = EntityMetadata.Privileges.FirstOrDefault(p => p.PrivilegeType == privilegeType);

                if (privilege != null)
                {
                    this._availablePrivilegesTypes.Add(privilegeType, privilege);

                    var rolePrivilege = rolePrivileges.FirstOrDefault(p => p.PrivilegeId == privilege.PrivilegeId);

                    if (rolePrivilege != null)
                    {
                        return (PrivilegeDepthExtended)rolePrivilege.Depth;
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

        public bool IsChanged { get; private set; }

        //public PrivilegeDepthExtended CreateRight

        private PrivilegeDepthExtended _CreateRight;
        public PrivilegeDepthExtended CreateRight
        {
            get => AvailableCreate ? _CreateRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableCreate || _CreateRight == value)
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
                if (!AvailableRead || _ReadRight == value)
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
                if (!AvailableUpdate || _UpdateRight == value)
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
                if (!AvailableDelete || _DeleteRight == value
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
                if (!AvailableAppend || _AppendRight == value)
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
                if (!AvailableAppendTo || _AppendToRight == value)
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
                if (!AvailableShare || _ShareRight == value)
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
                if (!AvailableAssign || _AssignRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AssignRight));
                this._AssignRight = value;
                this.OnPropertyChanged(nameof(AssignRight));
            }
        }

        public void FillChangedPrivileges(Dictionary<Guid, PrivilegeDepth> privilegesAdd, HashSet<Guid> privilegesRemove)
        {
            SetPrivilegeLevel(this._initialCreate, this._CreateRight, PrivilegeType.Create, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._initialRead, this._ReadRight, PrivilegeType.Read, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._initialUpdate, this._UpdateRight, PrivilegeType.Write, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._initialDelete, this._DeleteRight, PrivilegeType.Delete, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._initialAppend, this._AppendRight, PrivilegeType.Append, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._initialAppendTo, this._AppendToRight, PrivilegeType.AppendTo, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._initialAssign, this._AssignRight, PrivilegeType.Assign, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._initialShare, this._ShareRight, PrivilegeType.Share, privilegesAdd, privilegesRemove);
        }

        private void SetPrivilegeLevel(PrivilegeDepthExtended initialValue
            , PrivilegeDepthExtended currentValue
            , PrivilegeType privilegeType
            , Dictionary<Guid, PrivilegeDepth> privilegesAdd
            , HashSet<Guid> privilegesRemove
            )
        {
            if (currentValue == initialValue)
            {
                return;
            }

            if (privilegeType == PrivilegeType.None
                || !_availablePrivilegesTypes.ContainsKey(privilegeType)
            )
            {
                return;
            }

            var privilege = _availablePrivilegesTypes[privilegeType];

            if (privilege == null)
            {
                return;
            }

            if (currentValue == PrivilegeDepthExtended.None)
            {
                privilegesRemove.Add(privilege.PrivilegeId);
            }
            else
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

        public IEnumerable<PrivilegeDepthExtended> CreateOptions => ReturnOptions(PrivilegeType.Create);

        public IEnumerable<PrivilegeDepthExtended> ReadOptions => ReturnOptions(PrivilegeType.Read);

        public IEnumerable<PrivilegeDepthExtended> UpdateOptions => ReturnOptions(PrivilegeType.Write);

        public IEnumerable<PrivilegeDepthExtended> DeleteOptions => ReturnOptions(PrivilegeType.Delete);

        public IEnumerable<PrivilegeDepthExtended> AppendOptions => ReturnOptions(PrivilegeType.Append);

        public IEnumerable<PrivilegeDepthExtended> AppendToOptions => ReturnOptions(PrivilegeType.AppendTo);

        public IEnumerable<PrivilegeDepthExtended> AssignOptions => ReturnOptions(PrivilegeType.Assign);

        public IEnumerable<PrivilegeDepthExtended> ShareOptions => ReturnOptions(PrivilegeType.Share);

        private static readonly PrivilegeDepthExtended[] _optionsDefault = new PrivilegeDepthExtended[] { PrivilegeDepthExtended.None };

        private static ConcurrentDictionary<Tuple<bool, bool, bool, bool>, PrivilegeDepthExtended[]> _optionsCache = new ConcurrentDictionary<Tuple<bool, bool, bool, bool>, PrivilegeDepthExtended[]>();

        private PrivilegeDepthExtended[] ReturnOptions(PrivilegeType privilegeType)
        {
            if (privilegeType != PrivilegeType.None && this.EntityMetadata != null && this.EntityMetadata.Privileges != null)
            {
                var privilege = this.EntityMetadata.Privileges.FirstOrDefault(p => p.PrivilegeType == privilegeType);

                if (privilege != null)
                {
                    return GetPrivilegeDepthsByAvailability(privilege.CanBeBasic, privilege.CanBeLocal, privilege.CanBeDeep, privilege.CanBeGlobal);
                }
            }

            return _optionsDefault;
        }

        public static PrivilegeDepthExtended[] GetPrivilegeDepthsByAvailability(bool canBeBasic, bool canBeLocal, bool canBeDeep, bool canBeGlobal)
        {
            var key = Tuple.Create(canBeBasic, canBeLocal, canBeDeep, canBeGlobal);

            if (_optionsCache.ContainsKey(key))
            {
                return _optionsCache[key];
            }

            PrivilegeDepthExtended[] result = ConstructNewArray(canBeBasic, canBeLocal, canBeDeep, canBeGlobal);

            _optionsCache.TryAdd(key, result);

            return result;
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
    }
}
