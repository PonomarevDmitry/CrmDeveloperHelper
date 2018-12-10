using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
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

        public string DisplayName { get; private set; }

        private PrivilegeDepth? _initialCreate;
        private PrivilegeDepth? _initialRead;
        private PrivilegeDepth? _initialUpdate;
        private PrivilegeDepth? _initialDelete;
        private PrivilegeDepth? _initialAppend;
        private PrivilegeDepth? _initialAppendTo;
        private PrivilegeDepth? _initialShare;
        private PrivilegeDepth? _initialAssign;

        public bool AvailableCreate => _availablePrivilegesTypes.Contains(PrivilegeType.Create);
        public bool AvailableRead => _availablePrivilegesTypes.Contains(PrivilegeType.Read);
        public bool AvailableUpdate => _availablePrivilegesTypes.Contains(PrivilegeType.Write);
        public bool AvailableDelete => _availablePrivilegesTypes.Contains(PrivilegeType.Delete);
        public bool AvailableAppend => _availablePrivilegesTypes.Contains(PrivilegeType.Append);
        public bool AvailableAppendTo => _availablePrivilegesTypes.Contains(PrivilegeType.AppendTo);
        public bool AvailableShare => _availablePrivilegesTypes.Contains(PrivilegeType.Share);
        public bool AvailableAssign => _availablePrivilegesTypes.Contains(PrivilegeType.Assign);

        public EntityPrivilegeViewItem(EntityMetadata entityMetadata, IEnumerable<RolePrivilege> rolePrivileges)
        {
            LoadData(entityMetadata, rolePrivileges);
        }

        private HashSet<PrivilegeType> _availablePrivilegesTypes = new HashSet<PrivilegeType>();

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

            this._IsChanged = false;
        }

        private PrivilegeDepth? GetPrivilegeLevel(IEnumerable<RolePrivilege> rolePrivileges, PrivilegeType privilegeType)
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
                    this._availablePrivilegesTypes.Add(privilegeType);

                    var rolePrivilege = rolePrivileges.FirstOrDefault(p => p.PrivilegeId == privilege.PrivilegeId);

                    if (rolePrivilege != null)
                    {
                        return rolePrivilege.Depth;
                    }
                }
            }

            return null;
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

        //public PrivilegeDepth? CreateRight

        private PrivilegeDepth? _CreateRight;
        public PrivilegeDepth? CreateRight
        {
            get => AvailableCreate ? _CreateRight : null;
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

        private PrivilegeDepth? _ReadRight;
        public PrivilegeDepth? ReadRight
        {
            get => AvailableRead ? _ReadRight : null;
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

        private PrivilegeDepth? _UpdateRight;
        public PrivilegeDepth? UpdateRight
        {
            get => AvailableUpdate ? _UpdateRight : null;
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

        private PrivilegeDepth? _DeleteRight;
        public PrivilegeDepth? DeleteRight
        {
            get => AvailableDelete ? _DeleteRight : null;
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

        private PrivilegeDepth? _AppendRight;
        public PrivilegeDepth? AppendRight
        {
            get => AvailableAppend ? _AppendRight : null;
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

        private PrivilegeDepth? _AppendToRight;
        public PrivilegeDepth? AppendToRight
        {
            get => AvailableAppendTo ? _AppendToRight : null;
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

        private PrivilegeDepth? _ShareRight;
        public PrivilegeDepth? ShareRight
        {
            get => AvailableShare ? _ShareRight : null;
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

        private PrivilegeDepth? _AssignRight;
        public PrivilegeDepth? AssignRight
        {
            get => AvailableAssign ? _AssignRight : null;
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
    }
}
