using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public abstract class BaseEntityPrivilegeViewItem : BaseSinglePrivilegeViewItem
    {
        public abstract bool IsCustomizable { get; }

        public PrivilegeDepthExtended InitialCreateRight { get; private set; }

        public PrivilegeDepthExtended InitialReadRight { get; private set; }

        public PrivilegeDepthExtended InitialUpdateRight { get; private set; }

        public PrivilegeDepthExtended InitialDeleteRight { get; private set; }

        public PrivilegeDepthExtended InitialAppendRight { get; private set; }

        public PrivilegeDepthExtended InitialAppendToRight { get; private set; }

        public PrivilegeDepthExtended InitialShareRight { get; private set; }

        public PrivilegeDepthExtended InitialAssignRight { get; private set; }

        public bool AvailableCreate => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Create);

        public bool AvailableRead => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Read);

        public bool AvailableUpdate => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Write);

        public bool AvailableDelete => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Delete);

        public bool AvailableAppend => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Append);

        public bool AvailableAppendTo => _availablePrivilegesTypes.ContainsKey(PrivilegeType.AppendTo);

        public bool AvailableShare => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Share);

        public bool AvailableAssign => _availablePrivilegesTypes.ContainsKey(PrivilegeType.Assign);

        public bool AvailablePrivilegeType(PrivilegeType privilegeType) => _availablePrivilegesTypes.ContainsKey(privilegeType);

        public IEnumerable<SecurityPrivilegeMetadata> EntityPrivileges { get; private set; }

        private readonly Dictionary<PrivilegeType, IEnumerable<PrivilegeDepthExtended>> _privilegeOptions = new Dictionary<PrivilegeType, IEnumerable<PrivilegeDepthExtended>>();

        protected readonly Dictionary<PrivilegeType, SecurityPrivilegeMetadata> _availablePrivilegesTypes = new Dictionary<PrivilegeType, SecurityPrivilegeMetadata>();

        protected override bool CalculateIsChanged()
        {
            if (InitialCreateRight != _createRight)
            {
                return true;
            }

            if (InitialReadRight != _readRight)
            {
                return true;
            }

            if (InitialUpdateRight != _updateRight)
            {
                return true;
            }

            if (InitialDeleteRight != _deleteRight)
            {
                return true;
            }

            if (InitialAppendRight != _appendRight)
            {
                return true;
            }

            if (InitialAppendToRight != _appendToRight)
            {
                return true;
            }

            if (InitialShareRight != _shareRight)
            {
                return true;
            }

            if (InitialAssignRight != _assignRight)
            {
                return true;
            }

            return false;
        }

        protected void LoadData(IEnumerable<RolePrivileges> rolePrivileges, IEnumerable<SecurityPrivilegeMetadata> entityPrivileges)
        {
            this.EntityPrivileges = entityPrivileges.ToArray();

            this._availablePrivilegesTypes.Clear();

            this._createRight = this.InitialCreateRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Create);
            this._readRight = this.InitialReadRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Read);
            this._updateRight = this.InitialUpdateRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Write);
            this._deleteRight = this.InitialDeleteRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Delete);

            this._appendRight = this.InitialAppendRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Append);
            this._appendToRight = this.InitialAppendToRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.AppendTo);

            this._shareRight = this.InitialShareRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Share);
            this._assignRight = this.InitialAssignRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Assign);

            this.OnPropertyChanging(nameof(IsChanged));
            this.IsChanged = false;
            this.OnPropertyChanged(nameof(IsChanged));
        }

        protected void LoadData(IEnumerable<RolePrivilege> rolePrivileges, IEnumerable<SecurityPrivilegeMetadata> entityPrivileges)
        {
            this.EntityPrivileges = entityPrivileges.ToArray();

            this._availablePrivilegesTypes.Clear();

            this._createRight = this.InitialCreateRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Create);
            this._readRight = this.InitialReadRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Read);
            this._updateRight = this.InitialUpdateRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Write);
            this._deleteRight = this.InitialDeleteRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Delete);

            this._appendRight = this.InitialAppendRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Append);
            this._appendToRight = this.InitialAppendToRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.AppendTo);

            this._shareRight = this.InitialShareRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Share);
            this._assignRight = this.InitialAssignRight = GetPrivilegeLevel(entityPrivileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Assign);

            this.OnPropertyChanging(nameof(IsChanged));
            this.IsChanged = false;
            this.OnPropertyChanged(nameof(IsChanged));
        }

        protected PrivilegeDepthExtended _createRight;
        public PrivilegeDepthExtended CreateRight
        {
            get => AvailableCreate ? _createRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableCreate || !IsCustomizable || !CreateOptions.Contains(value) || _createRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CreateRight));
                this._createRight = value;
                this.OnPropertyChanged(nameof(CreateRight));
            }
        }

        protected PrivilegeDepthExtended _readRight;
        public PrivilegeDepthExtended ReadRight
        {
            get => AvailableRead ? _readRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableRead || !IsCustomizable || !ReadOptions.Contains(value) || _readRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ReadRight));
                this._readRight = value;
                this.OnPropertyChanged(nameof(ReadRight));
            }
        }

        protected PrivilegeDepthExtended _updateRight;
        public PrivilegeDepthExtended UpdateRight
        {
            get => AvailableUpdate ? _updateRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableUpdate || !IsCustomizable || !UpdateOptions.Contains(value) || _updateRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(UpdateRight));
                this._updateRight = value;
                this.OnPropertyChanged(nameof(UpdateRight));
            }
        }

        protected PrivilegeDepthExtended _deleteRight;
        public PrivilegeDepthExtended DeleteRight
        {
            get => AvailableDelete ? _deleteRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableDelete || !IsCustomizable || !DeleteOptions.Contains(value) || _deleteRight == value
                    )
                {
                    return;
                }

                this.OnPropertyChanging(nameof(DeleteRight));
                this._deleteRight = value;
                this.OnPropertyChanged(nameof(DeleteRight));
            }
        }

        protected PrivilegeDepthExtended _appendRight;
        public PrivilegeDepthExtended AppendRight
        {
            get => AvailableAppend ? _appendRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAppend || !IsCustomizable || !AppendOptions.Contains(value) || _appendRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AppendRight));
                this._appendRight = value;
                this.OnPropertyChanged(nameof(AppendRight));
            }
        }

        protected PrivilegeDepthExtended _appendToRight;
        public PrivilegeDepthExtended AppendToRight
        {
            get => AvailableAppendTo ? _appendToRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAppendTo || !IsCustomizable || !AppendToOptions.Contains(value) || _appendToRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AppendToRight));
                this._appendToRight = value;
                this.OnPropertyChanged(nameof(AppendToRight));
            }
        }

        protected PrivilegeDepthExtended _shareRight;
        public PrivilegeDepthExtended ShareRight
        {
            get => AvailableShare ? _shareRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableShare || !IsCustomizable || !ShareOptions.Contains(value) || _shareRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ShareRight));
                this._shareRight = value;
                this.OnPropertyChanged(nameof(ShareRight));
            }
        }

        protected PrivilegeDepthExtended _assignRight;
        public PrivilegeDepthExtended AssignRight
        {
            get => AvailableAssign ? _assignRight : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAssign || !IsCustomizable || !AssignOptions.Contains(value) || _assignRight == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AssignRight));
                this._assignRight = value;
                this.OnPropertyChanged(nameof(AssignRight));
            }
        }

        public IEnumerable<PrivilegeDepthExtended> GetPrivilegeOptions(PrivilegeType privilegeType)
        {
            if (privilegeType != PrivilegeType.None && this.EntityPrivileges != null)
            {
                if (!_privilegeOptions.ContainsKey(privilegeType))
                {
                    _privilegeOptions[privilegeType] = ReturnOptions(privilegeType, this.EntityPrivileges);
                }

                return _privilegeOptions[privilegeType];
            }
            else
            {
                return _optionsDefault;
            }
        }

        public IEnumerable<PrivilegeDepthExtended> CreateOptions => GetPrivilegeOptions(PrivilegeType.Create);

        public IEnumerable<PrivilegeDepthExtended> ReadOptions => GetPrivilegeOptions(PrivilegeType.Read);

        public IEnumerable<PrivilegeDepthExtended> UpdateOptions => GetPrivilegeOptions(PrivilegeType.Write);

        public IEnumerable<PrivilegeDepthExtended> DeleteOptions => GetPrivilegeOptions(PrivilegeType.Delete);

        public IEnumerable<PrivilegeDepthExtended> AppendOptions => GetPrivilegeOptions(PrivilegeType.Append);

        public IEnumerable<PrivilegeDepthExtended> AppendToOptions => GetPrivilegeOptions(PrivilegeType.AppendTo);

        public IEnumerable<PrivilegeDepthExtended> AssignOptions => GetPrivilegeOptions(PrivilegeType.Assign);

        public IEnumerable<PrivilegeDepthExtended> ShareOptions => GetPrivilegeOptions(PrivilegeType.Share);

        public void SetPrivilegeRight(PrivilegeType privilegeType, PrivilegeDepthExtended privilegeDepth)
        {
            if (!this.AvailablePrivilegeType(privilegeType))
            {
                return;
            }

            if (!this.GetPrivilegeOptions(privilegeType).Contains(privilegeDepth))
            {
                return;
            }

            switch (privilegeType)
            {
                case PrivilegeType.Create:
                    if (this.AvailableCreate && this.CreateOptions.Contains(privilegeDepth))
                    {
                        this.CreateRight = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Read:
                    if (this.AvailableRead && this.ReadOptions.Contains(privilegeDepth))
                    {
                        this.ReadRight = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Write:
                    if (this.AvailableUpdate && this.UpdateOptions.Contains(privilegeDepth))
                    {
                        this.UpdateRight = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Delete:
                    if (this.AvailableDelete && this.DeleteOptions.Contains(privilegeDepth))
                    {
                        this.DeleteRight = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Assign:
                    if (this.AvailableAssign && this.AssignOptions.Contains(privilegeDepth))
                    {
                        this.AssignRight = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Share:
                    if (this.AvailableShare && this.ShareOptions.Contains(privilegeDepth))
                    {
                        this.ShareRight = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Append:
                    if (this.AvailableAppend && this.AppendOptions.Contains(privilegeDepth))
                    {
                        this.AppendRight = privilegeDepth;
                    }
                    break;

                case PrivilegeType.AppendTo:
                    if (this.AvailableAppendTo && this.AppendToOptions.Contains(privilegeDepth))
                    {
                        this.AppendToRight = privilegeDepth;
                    }
                    break;

                case PrivilegeType.None:
                default:
                    break;
            }
        }

        public bool EqualsInitialPrivilegeDepthValue(PrivilegeType privilegeType, PrivilegeDepthExtended privilegeDepthValue)
        {
            if (!this.AvailablePrivilegeType(privilegeType))
            {
                return false;
            }

            switch (privilegeType)
            {
                case PrivilegeType.Create:
                    return this.InitialCreateRight == privilegeDepthValue;

                case PrivilegeType.Read:
                    return this.InitialReadRight == privilegeDepthValue;

                case PrivilegeType.Write:
                    return this.InitialUpdateRight == privilegeDepthValue;

                case PrivilegeType.Delete:
                    return this.InitialDeleteRight == privilegeDepthValue;

                case PrivilegeType.Assign:
                    return this.InitialAssignRight == privilegeDepthValue;

                case PrivilegeType.Share:
                    return this.InitialShareRight == privilegeDepthValue;

                case PrivilegeType.Append:
                    return this.InitialAppendRight == privilegeDepthValue;

                case PrivilegeType.AppendTo:
                    return this.InitialAppendToRight == privilegeDepthValue;

                case PrivilegeType.None:
                default:
                    break;
            }

            return false;
        }

        public bool EqualsCurrentPrivilegeDepthValue(PrivilegeType privilegeType, PrivilegeDepthExtended privilegeDepthValue)
        {
            if (!this.AvailablePrivilegeType(privilegeType))
            {
                return false;
            }

            switch (privilegeType)
            {
                case PrivilegeType.Create:
                    return this.CreateRight == privilegeDepthValue;

                case PrivilegeType.Read:
                    return this.ReadRight == privilegeDepthValue;

                case PrivilegeType.Write:
                    return this.UpdateRight == privilegeDepthValue;

                case PrivilegeType.Delete:
                    return this.DeleteRight == privilegeDepthValue;

                case PrivilegeType.Assign:
                    return this.AssignRight == privilegeDepthValue;

                case PrivilegeType.Share:
                    return this.ShareRight == privilegeDepthValue;

                case PrivilegeType.Append:
                    return this.AppendRight == privilegeDepthValue;

                case PrivilegeType.AppendTo:
                    return this.AppendToRight == privilegeDepthValue;

                case PrivilegeType.None:
                default:
                    break;
            }

            return false;
        }
    }
}
