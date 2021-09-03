using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleEntityPrivilegeViewItem : SinglePrivilegeViewItem
    {
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

        public RoleEntityPrivilegeViewItem(EntityMetadata entityMetadata, IEnumerable<RolePrivilege> rolePrivileges, bool isCustomizable = false)
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

            this._CreateRight = this._initialCreate = GetPrivilegeLevel(entityMetadata?.Privileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Create);
            this._ReadRight = this._initialRead = GetPrivilegeLevel(entityMetadata?.Privileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Read);
            this._UpdateRight = this._initialUpdate = GetPrivilegeLevel(entityMetadata?.Privileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Write);
            this._DeleteRight = this._initialDelete = GetPrivilegeLevel(entityMetadata?.Privileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Delete);

            this._AppendRight = this._initialAppend = GetPrivilegeLevel(entityMetadata?.Privileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Append);
            this._AppendToRight = this._initialAppendTo = GetPrivilegeLevel(entityMetadata?.Privileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.AppendTo);

            this._ShareRight = this._initialShare = GetPrivilegeLevel(entityMetadata?.Privileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Share);
            this._AssignRight = this._initialAssign = GetPrivilegeLevel(entityMetadata?.Privileges, rolePrivileges, _availablePrivilegesTypes, PrivilegeType.Assign);

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

        public IEnumerable<PrivilegeDepthExtended> CreateOptions => ReturnOptions(PrivilegeType.Create, this.EntityMetadata.Privileges);

        public IEnumerable<PrivilegeDepthExtended> ReadOptions => ReturnOptions(PrivilegeType.Read, this.EntityMetadata.Privileges);

        public IEnumerable<PrivilegeDepthExtended> UpdateOptions => ReturnOptions(PrivilegeType.Write, this.EntityMetadata.Privileges);

        public IEnumerable<PrivilegeDepthExtended> DeleteOptions => ReturnOptions(PrivilegeType.Delete, this.EntityMetadata.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AppendOptions => ReturnOptions(PrivilegeType.Append, this.EntityMetadata.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AppendToOptions => ReturnOptions(PrivilegeType.AppendTo, this.EntityMetadata.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AssignOptions => ReturnOptions(PrivilegeType.Assign, this.EntityMetadata.Privileges);

        public IEnumerable<PrivilegeDepthExtended> ShareOptions => ReturnOptions(PrivilegeType.Share, this.EntityMetadata.Privileges);

        public void FillChangedPrivileges(Dictionary<Guid, PrivilegeDepth> privilegesAdd, HashSet<Guid> privilegesRemove)
        {
            SetPrivilegeLevel(this._availablePrivilegesTypes, this._initialCreate, this._CreateRight, PrivilegeType.Create, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this._initialRead, this._ReadRight, PrivilegeType.Read, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this._initialUpdate, this._UpdateRight, PrivilegeType.Write, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this._initialDelete, this._DeleteRight, PrivilegeType.Delete, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._availablePrivilegesTypes, this._initialAppend, this._AppendRight, PrivilegeType.Append, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this._initialAppendTo, this._AppendToRight, PrivilegeType.AppendTo, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._availablePrivilegesTypes, this._initialAssign, this._AssignRight, PrivilegeType.Assign, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this._initialShare, this._ShareRight, PrivilegeType.Share, privilegesAdd, privilegesRemove);
        }
    }
}
