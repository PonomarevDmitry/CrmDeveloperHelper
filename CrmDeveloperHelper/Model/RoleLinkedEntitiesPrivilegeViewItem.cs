using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleLinkedEntitiesPrivilegeViewItem : BaseLinkedPrivilegeViewItem
    {
        public string LogicalName => (EntityMetadata1 ?? EntityMetadata2).LogicalName;

        public bool IsIntersect1 => (EntityMetadata1?.IsIntersect).GetValueOrDefault();

        public bool IsIntersect2 => (EntityMetadata2?.IsIntersect).GetValueOrDefault();

        public RoleLinkedEntitiesPrivilegeViewItem(
            EntityMetadata entityMetadata1
            , EntityMetadata entityMetadata2
            , IEnumerable<RolePrivilege> rolePrivileges1
            , IEnumerable<RolePrivilege> rolePrivileges2
            , bool isCustomizable1 = false
            , bool isCustomizable2 = false
        )
        {
            this.IsCustomizable1 = isCustomizable1;
            this.IsCustomizable2 = isCustomizable2;

            LoadData1(entityMetadata1, rolePrivileges1);
            LoadData2(entityMetadata2, rolePrivileges2);

            this.OnPropertyChanging(nameof(IsChanged1));
            this.IsChanged1 = false;
            this.OnPropertyChanged(nameof(IsChanged1));

            this.OnPropertyChanging(nameof(IsChanged2));
            this.IsChanged2 = false;
            this.OnPropertyChanged(nameof(IsChanged2));
        }

        public bool AnyRightDifferent => CreateRightDifferent
            || ReadRightDifferent
            || UpdateRightDifferent
            || DeleteRightDifferent
            || AppendRightDifferent
            || AppendToRightDifferent
            || ShareRightDifferent
            || AssignRightDifferent
            ;

        public bool CreateRightDifferent => EntityMetadata1 != null && EntityMetadata2 != null && _CreateRight1 != CreateRight2;

        public bool ReadRightDifferent => EntityMetadata1 != null && EntityMetadata2 != null && _ReadRight1 != _ReadRight2;

        public bool UpdateRightDifferent => EntityMetadata1 != null && EntityMetadata2 != null && _UpdateRight1 != UpdateRight2;

        public bool DeleteRightDifferent => EntityMetadata1 != null && EntityMetadata2 != null && _DeleteRight1 != _DeleteRight2;

        public bool AppendRightDifferent => EntityMetadata1 != null && EntityMetadata2 != null && _AppendRight1 != _AppendRight2;

        public bool AppendToRightDifferent => EntityMetadata1 != null && EntityMetadata2 != null && _AppendToRight1 != _AppendToRight2;

        public bool ShareRightDifferent => EntityMetadata1 != null && EntityMetadata2 != null && _ShareRight1 != _ShareRight2;

        public bool AssignRightDifferent => EntityMetadata1 != null && EntityMetadata2 != null && _AssignRight1 != _AssignRight2;

        #region Entity1

        protected override bool CalculateIsChanged1()
        {
            if (_initialCreate1 != _CreateRight1)
            {
                return true;
            }

            if (_initialRead1 != _ReadRight1)
            {
                return true;
            }

            if (_initialUpdate1 != _UpdateRight1)
            {
                return true;
            }

            if (_initialDelete1 != _DeleteRight1)
            {
                return true;
            }

            if (_initialAppend1 != _AppendRight1)
            {
                return true;
            }

            if (_initialAppendTo1 != _AppendToRight1)
            {
                return true;
            }

            if (_initialShare1 != _ShareRight1)
            {
                return true;
            }

            if (_initialAssign1 != _AssignRight1)
            {
                return true;
            }

            return false;
        }

        public void LoadData1(EntityMetadata entityMetadata1, IEnumerable<RolePrivilege> rolePrivileges1)
        {
            this._availablePrivilegesTypes1.Clear();

            this.EntityMetadata1 = entityMetadata1;
            this.DisplayName1 = CreateFileHandler.GetLocalizedLabel(entityMetadata1.DisplayName);

            this._CreateRight1 = this._initialCreate1 = GetPrivilegeLevel(entityMetadata1?.Privileges, rolePrivileges1, _availablePrivilegesTypes1, PrivilegeType.Create);
            this._ReadRight1 = this._initialRead1 = GetPrivilegeLevel(entityMetadata1?.Privileges, rolePrivileges1, _availablePrivilegesTypes1, PrivilegeType.Read);
            this._UpdateRight1 = this._initialUpdate1 = GetPrivilegeLevel(entityMetadata1?.Privileges, rolePrivileges1, _availablePrivilegesTypes1, PrivilegeType.Write);
            this._DeleteRight1 = this._initialDelete1 = GetPrivilegeLevel(entityMetadata1?.Privileges, rolePrivileges1, _availablePrivilegesTypes1, PrivilegeType.Delete);

            this._AppendRight1 = this._initialAppend1 = GetPrivilegeLevel(entityMetadata1?.Privileges, rolePrivileges1, _availablePrivilegesTypes1, PrivilegeType.Append);
            this._AppendToRight1 = this._initialAppendTo1 = GetPrivilegeLevel(entityMetadata1?.Privileges, rolePrivileges1, _availablePrivilegesTypes1, PrivilegeType.AppendTo);

            this._ShareRight1 = this._initialShare1 = GetPrivilegeLevel(entityMetadata1?.Privileges, rolePrivileges1, _availablePrivilegesTypes1, PrivilegeType.Share);
            this._AssignRight1 = this._initialAssign1 = GetPrivilegeLevel(entityMetadata1?.Privileges, rolePrivileges1, _availablePrivilegesTypes1, PrivilegeType.Assign);
        }

        private Dictionary<PrivilegeType, SecurityPrivilegeMetadata> _availablePrivilegesTypes1 = new Dictionary<PrivilegeType, SecurityPrivilegeMetadata>();

        public EntityMetadata EntityMetadata1 { get; private set; }

        public int ObjectTypeCode1 => EntityMetadata1.ObjectTypeCode.GetValueOrDefault();

        public string DisplayName1 { get; private set; }

        public bool IsCustomizable1 { get; private set; }

        private PrivilegeDepthExtended _initialCreate1;
        private PrivilegeDepthExtended _initialRead1;
        private PrivilegeDepthExtended _initialUpdate1;
        private PrivilegeDepthExtended _initialDelete1;
        private PrivilegeDepthExtended _initialAppend1;
        private PrivilegeDepthExtended _initialAppendTo1;
        private PrivilegeDepthExtended _initialShare1;
        private PrivilegeDepthExtended _initialAssign1;

        public bool AvailableCreate1 => _availablePrivilegesTypes1.ContainsKey(PrivilegeType.Create);
        public bool AvailableRead1 => _availablePrivilegesTypes1.ContainsKey(PrivilegeType.Read);
        public bool AvailableUpdate1 => _availablePrivilegesTypes1.ContainsKey(PrivilegeType.Write);
        public bool AvailableDelete1 => _availablePrivilegesTypes1.ContainsKey(PrivilegeType.Delete);
        public bool AvailableAppend1 => _availablePrivilegesTypes1.ContainsKey(PrivilegeType.Append);
        public bool AvailableAppendTo1 => _availablePrivilegesTypes1.ContainsKey(PrivilegeType.AppendTo);
        public bool AvailableShare1 => _availablePrivilegesTypes1.ContainsKey(PrivilegeType.Share);
        public bool AvailableAssign1 => _availablePrivilegesTypes1.ContainsKey(PrivilegeType.Assign);

        private PrivilegeDepthExtended _CreateRight1;
        public PrivilegeDepthExtended CreateRight1
        {
            get => AvailableCreate1 ? _CreateRight1 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableCreate1 || _CreateRight1 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CreateRight1));
                this.OnPropertyChanging(nameof(CreateRightDifferent));
                this._CreateRight1 = value;
                this.OnPropertyChanged(nameof(CreateRightDifferent));
                this.OnPropertyChanged(nameof(CreateRight1));
            }
        }

        private PrivilegeDepthExtended _ReadRight1;
        public PrivilegeDepthExtended ReadRight1
        {
            get => AvailableRead1 ? _ReadRight1 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableRead1 || _ReadRight1 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ReadRight1));
                this.OnPropertyChanging(nameof(ReadRightDifferent));
                this._ReadRight1 = value;
                this.OnPropertyChanged(nameof(ReadRightDifferent));
                this.OnPropertyChanged(nameof(ReadRight1));
            }
        }

        private PrivilegeDepthExtended _UpdateRight1;
        public PrivilegeDepthExtended UpdateRight1
        {
            get => AvailableUpdate1 ? _UpdateRight1 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableUpdate1 || _UpdateRight1 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(UpdateRight1));
                this.OnPropertyChanging(nameof(UpdateRightDifferent));
                this._UpdateRight1 = value;
                this.OnPropertyChanged(nameof(UpdateRightDifferent));
                this.OnPropertyChanged(nameof(UpdateRight1));
            }
        }

        private PrivilegeDepthExtended _DeleteRight1;
        public PrivilegeDepthExtended DeleteRight1
        {
            get => AvailableDelete1 ? _DeleteRight1 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableDelete1 || _DeleteRight1 == value
                    )
                {
                    return;
                }

                this.OnPropertyChanging(nameof(DeleteRight1));
                this.OnPropertyChanging(nameof(DeleteRightDifferent));
                this._DeleteRight1 = value;
                this.OnPropertyChanged(nameof(DeleteRightDifferent));
                this.OnPropertyChanged(nameof(DeleteRight1));
            }
        }

        private PrivilegeDepthExtended _AppendRight1;
        public PrivilegeDepthExtended AppendRight1
        {
            get => AvailableAppend1 ? _AppendRight1 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAppend1 || _AppendRight1 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AppendRight1));
                this.OnPropertyChanging(nameof(AppendRightDifferent));
                this._AppendRight1 = value;
                this.OnPropertyChanged(nameof(AppendRightDifferent));
                this.OnPropertyChanged(nameof(AppendRight1));
            }
        }

        private PrivilegeDepthExtended _AppendToRight1;
        public PrivilegeDepthExtended AppendToRight1
        {
            get => AvailableAppendTo1 ? _AppendToRight1 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAppendTo1 || _AppendToRight1 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AppendToRight1));
                this.OnPropertyChanging(nameof(AppendToRightDifferent));
                this._AppendToRight1 = value;
                this.OnPropertyChanged(nameof(AppendToRightDifferent));
                this.OnPropertyChanged(nameof(AppendToRight1));
            }
        }

        private PrivilegeDepthExtended _ShareRight1;
        public PrivilegeDepthExtended ShareRight1
        {
            get => AvailableShare1 ? _ShareRight1 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableShare1 || _ShareRight1 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ShareRight1));
                this.OnPropertyChanging(nameof(ShareRightDifferent));
                this._ShareRight1 = value;
                this.OnPropertyChanged(nameof(ShareRightDifferent));
                this.OnPropertyChanged(nameof(ShareRight1));
            }
        }

        private PrivilegeDepthExtended _AssignRight1;
        public PrivilegeDepthExtended AssignRight1
        {
            get => AvailableAssign1 ? _AssignRight1 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAssign1 || _AssignRight1 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AssignRight1));
                this.OnPropertyChanging(nameof(AssignRightDifferent));
                this._AssignRight1 = value;
                this.OnPropertyChanged(nameof(AssignRightDifferent));
                this.OnPropertyChanged(nameof(AssignRight1));
            }
        }

        public IEnumerable<PrivilegeDepthExtended> CreateOptions1 => ReturnOptions(PrivilegeType.Create, this.EntityMetadata1?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> ReadOptions1 => ReturnOptions(PrivilegeType.Read, this.EntityMetadata1?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> UpdateOptions1 => ReturnOptions(PrivilegeType.Write, this.EntityMetadata1?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> DeleteOptions1 => ReturnOptions(PrivilegeType.Delete, this.EntityMetadata1?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AppendOptions1 => ReturnOptions(PrivilegeType.Append, this.EntityMetadata1?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AppendToOptions1 => ReturnOptions(PrivilegeType.AppendTo, this.EntityMetadata1?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AssignOptions1 => ReturnOptions(PrivilegeType.Assign, this.EntityMetadata1?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> ShareOptions1 => ReturnOptions(PrivilegeType.Share, this.EntityMetadata1?.Privileges);

        public void FillChangedPrivileges1(Dictionary<Guid, PrivilegeDepth> privilegesAdd, HashSet<Guid> privilegesRemove)
        {
            SetPrivilegeLevel(this._availablePrivilegesTypes1, this._initialCreate1, this._CreateRight1, PrivilegeType.Create, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes1, this._initialRead1, this._ReadRight1, PrivilegeType.Read, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes1, this._initialUpdate1, this._UpdateRight1, PrivilegeType.Write, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes1, this._initialDelete1, this._DeleteRight1, PrivilegeType.Delete, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._availablePrivilegesTypes1, this._initialAppend1, this._AppendRight1, PrivilegeType.Append, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes1, this._initialAppendTo1, this._AppendToRight1, PrivilegeType.AppendTo, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._availablePrivilegesTypes1, this._initialAssign1, this._AssignRight1, PrivilegeType.Assign, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes1, this._initialShare1, this._ShareRight1, PrivilegeType.Share, privilegesAdd, privilegesRemove);
        }

        #endregion Entity1

        #region Entity2

        protected override bool CalculateIsChanged2()
        {
            if (_initialCreate2 != _CreateRight2)
            {
                return true;
            }

            if (_initialRead2 != _ReadRight2)
            {
                return true;
            }

            if (_initialUpdate2 != _UpdateRight2)
            {
                return true;
            }

            if (_initialDelete2 != _DeleteRight2)
            {
                return true;
            }

            if (_initialAppend2 != _AppendRight2)
            {
                return true;
            }

            if (_initialAppendTo2 != _AppendToRight2)
            {
                return true;
            }

            if (_initialShare2 != _ShareRight2)
            {
                return true;
            }

            if (_initialAssign2 != _AssignRight2)
            {
                return true;
            }

            return false;
        }

        public void LoadData2(EntityMetadata entityMetadata2, IEnumerable<RolePrivilege> rolePrivileges2)
        {
            this._availablePrivilegesTypes2.Clear();

            this.EntityMetadata2 = entityMetadata2;

            if (entityMetadata2 != null)
            {
                this.DisplayName2 = CreateFileHandler.GetLocalizedLabel(entityMetadata2?.DisplayName);

                this._CreateRight2 = this._initialCreate2 = GetPrivilegeLevel(entityMetadata2?.Privileges, rolePrivileges2, _availablePrivilegesTypes2, PrivilegeType.Create);
                this._ReadRight2 = this._initialRead2 = GetPrivilegeLevel(entityMetadata2?.Privileges, rolePrivileges2, _availablePrivilegesTypes2, PrivilegeType.Read);
                this._UpdateRight2 = this._initialUpdate2 = GetPrivilegeLevel(entityMetadata2?.Privileges, rolePrivileges2, _availablePrivilegesTypes2, PrivilegeType.Write);
                this._DeleteRight2 = this._initialDelete2 = GetPrivilegeLevel(entityMetadata2?.Privileges, rolePrivileges2, _availablePrivilegesTypes2, PrivilegeType.Delete);

                this._AppendRight2 = this._initialAppend2 = GetPrivilegeLevel(entityMetadata2?.Privileges, rolePrivileges2, _availablePrivilegesTypes2, PrivilegeType.Append);
                this._AppendToRight2 = this._initialAppendTo2 = GetPrivilegeLevel(entityMetadata2?.Privileges, rolePrivileges2, _availablePrivilegesTypes2, PrivilegeType.AppendTo);

                this._ShareRight2 = this._initialShare2 = GetPrivilegeLevel(entityMetadata2?.Privileges, rolePrivileges2, _availablePrivilegesTypes2, PrivilegeType.Share);
                this._AssignRight2 = this._initialAssign2 = GetPrivilegeLevel(entityMetadata2?.Privileges, rolePrivileges2, _availablePrivilegesTypes2, PrivilegeType.Assign);
            }
            else
            {
                this.DisplayName2 = string.Empty;

                this._CreateRight2 = this._initialCreate2 = null;
                this._ReadRight2 = this._initialRead2 = null;
                this._UpdateRight2 = this._initialUpdate2 = null;
                this._DeleteRight2 = this._initialDelete2 = null;

                this._AppendRight2 = this._initialAppend2 = null;
                this._AppendToRight2 = this._initialAppendTo2 = null;

                this._ShareRight2 = this._initialShare2 = null;
                this._AssignRight2 = this._initialAssign2 = null;
            }
        }

        private Dictionary<PrivilegeType, SecurityPrivilegeMetadata> _availablePrivilegesTypes2 = new Dictionary<PrivilegeType, SecurityPrivilegeMetadata>();

        public EntityMetadata EntityMetadata2 { get; private set; }

        public int? ObjectTypeCode2 => EntityMetadata2.ObjectTypeCode.GetValueOrDefault();

        public string DisplayName2 { get; private set; }

        public bool? IsCustomizable2 { get; private set; }

        private PrivilegeDepthExtended? _initialCreate2;
        private PrivilegeDepthExtended? _initialRead2;
        private PrivilegeDepthExtended? _initialUpdate2;
        private PrivilegeDepthExtended? _initialDelete2;
        private PrivilegeDepthExtended? _initialAppend2;
        private PrivilegeDepthExtended? _initialAppendTo2;
        private PrivilegeDepthExtended? _initialShare2;
        private PrivilegeDepthExtended? _initialAssign2;

        public bool AvailableCreate2 => _availablePrivilegesTypes2.ContainsKey(PrivilegeType.Create);
        public bool AvailableRead2 => _availablePrivilegesTypes2.ContainsKey(PrivilegeType.Read);
        public bool AvailableUpdate2 => _availablePrivilegesTypes2.ContainsKey(PrivilegeType.Write);
        public bool AvailableDelete2 => _availablePrivilegesTypes2.ContainsKey(PrivilegeType.Delete);
        public bool AvailableAppend2 => _availablePrivilegesTypes2.ContainsKey(PrivilegeType.Append);
        public bool AvailableAppendTo2 => _availablePrivilegesTypes2.ContainsKey(PrivilegeType.AppendTo);
        public bool AvailableShare2 => _availablePrivilegesTypes2.ContainsKey(PrivilegeType.Share);
        public bool AvailableAssign2 => _availablePrivilegesTypes2.ContainsKey(PrivilegeType.Assign);

        private PrivilegeDepthExtended? _CreateRight2;
        public PrivilegeDepthExtended? CreateRight2
        {
            get => AvailableCreate2 ? _CreateRight2 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableCreate2 || _CreateRight2 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CreateRight2));
                this.OnPropertyChanging(nameof(CreateRightDifferent));
                this._CreateRight2 = value;
                this.OnPropertyChanged(nameof(CreateRightDifferent));
                this.OnPropertyChanged(nameof(CreateRight2));
            }
        }

        private PrivilegeDepthExtended? _ReadRight2;
        public PrivilegeDepthExtended? ReadRight2
        {
            get => AvailableRead2 ? _ReadRight2 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableRead2 || _ReadRight2 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ReadRight2));
                this.OnPropertyChanging(nameof(ReadRightDifferent));
                this._ReadRight2 = value;
                this.OnPropertyChanged(nameof(ReadRightDifferent));
                this.OnPropertyChanged(nameof(ReadRight2));
            }
        }

        private PrivilegeDepthExtended? _UpdateRight2;
        public PrivilegeDepthExtended? UpdateRight2
        {
            get => AvailableUpdate2 ? _UpdateRight2 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableUpdate2 || _UpdateRight2 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(UpdateRight2));
                this.OnPropertyChanging(nameof(UpdateRightDifferent));
                this._UpdateRight2 = value;
                this.OnPropertyChanged(nameof(UpdateRightDifferent));
                this.OnPropertyChanged(nameof(UpdateRight2));
            }
        }

        private PrivilegeDepthExtended? _DeleteRight2;
        public PrivilegeDepthExtended? DeleteRight2
        {
            get => AvailableDelete2 ? _DeleteRight2 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableDelete2 || _DeleteRight2 == value
                    )
                {
                    return;
                }

                this.OnPropertyChanging(nameof(DeleteRight2));
                this.OnPropertyChanging(nameof(DeleteRightDifferent));
                this._DeleteRight2 = value;
                this.OnPropertyChanged(nameof(DeleteRightDifferent));
                this.OnPropertyChanged(nameof(DeleteRight2));
            }
        }

        private PrivilegeDepthExtended? _AppendRight2;
        public PrivilegeDepthExtended? AppendRight2
        {
            get => AvailableAppend2 ? _AppendRight2 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAppend2 || _AppendRight2 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AppendRight2));
                this.OnPropertyChanging(nameof(AppendRightDifferent));
                this._AppendRight2 = value;
                this.OnPropertyChanged(nameof(AppendRightDifferent));
                this.OnPropertyChanged(nameof(AppendRight2));
            }
        }

        private PrivilegeDepthExtended? _AppendToRight2;
        public PrivilegeDepthExtended? AppendToRight2
        {
            get => AvailableAppendTo2 ? _AppendToRight2 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAppendTo2 || _AppendToRight2 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AppendToRight2));
                this.OnPropertyChanging(nameof(AppendToRightDifferent));
                this._AppendToRight2 = value;
                this.OnPropertyChanged(nameof(AppendToRightDifferent));
                this.OnPropertyChanged(nameof(AppendToRight2));
            }
        }

        private PrivilegeDepthExtended? _ShareRight2;
        public PrivilegeDepthExtended? ShareRight2
        {
            get => AvailableShare2 ? _ShareRight2 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableShare2 || _ShareRight2 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ShareRight2));
                this.OnPropertyChanging(nameof(ShareRightDifferent));
                this._ShareRight2 = value;
                this.OnPropertyChanged(nameof(ShareRightDifferent));
                this.OnPropertyChanged(nameof(ShareRight2));
            }
        }

        private PrivilegeDepthExtended? _AssignRight2;
        public PrivilegeDepthExtended? AssignRight2
        {
            get => AvailableAssign2 ? _AssignRight2 : PrivilegeDepthExtended.None;
            set
            {
                if (!AvailableAssign2 || _AssignRight2 == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AssignRight2));
                this.OnPropertyChanging(nameof(AssignRightDifferent));
                this._AssignRight2 = value;
                this.OnPropertyChanged(nameof(AssignRightDifferent));
                this.OnPropertyChanged(nameof(AssignRight2));
            }
        }

        public IEnumerable<PrivilegeDepthExtended> CreateOptions2 => ReturnOptions(PrivilegeType.Create, this.EntityMetadata2?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> ReadOptions2 => ReturnOptions(PrivilegeType.Read, this.EntityMetadata2?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> UpdateOptions2 => ReturnOptions(PrivilegeType.Write, this.EntityMetadata2?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> DeleteOptions2 => ReturnOptions(PrivilegeType.Delete, this.EntityMetadata2?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AppendOptions2 => ReturnOptions(PrivilegeType.Append, this.EntityMetadata2?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AppendToOptions2 => ReturnOptions(PrivilegeType.AppendTo, this.EntityMetadata2?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> AssignOptions2 => ReturnOptions(PrivilegeType.Assign, this.EntityMetadata2?.Privileges);

        public IEnumerable<PrivilegeDepthExtended> ShareOptions2 => ReturnOptions(PrivilegeType.Share, this.EntityMetadata2?.Privileges);

        public void FillChangedPrivileges2(Dictionary<Guid, PrivilegeDepth> privilegesAdd, HashSet<Guid> privilegesRemove)
        {
            SetPrivilegeLevel(this._availablePrivilegesTypes2, this._initialCreate2, this._CreateRight2, PrivilegeType.Create, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes2, this._initialRead2, this._ReadRight2, PrivilegeType.Read, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes2, this._initialUpdate2, this._UpdateRight2, PrivilegeType.Write, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes2, this._initialDelete2, this._DeleteRight2, PrivilegeType.Delete, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._availablePrivilegesTypes2, this._initialAppend2, this._AppendRight2, PrivilegeType.Append, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes2, this._initialAppendTo2, this._AppendToRight2, PrivilegeType.AppendTo, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._availablePrivilegesTypes2, this._initialAssign2, this._AssignRight2, PrivilegeType.Assign, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes2, this._initialShare2, this._ShareRight2, PrivilegeType.Share, privilegesAdd, privilegesRemove);
        }

        #endregion Entity2
    }
}
