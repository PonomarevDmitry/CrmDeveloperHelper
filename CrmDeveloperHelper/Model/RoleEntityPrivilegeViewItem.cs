using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleEntityPrivilegeViewItem : BaseEntityPrivilegeViewItem
    {
        public EntityMetadata EntityMetadata { get; private set; }

        public string LogicalName => EntityMetadata.LogicalName;

        public bool IsIntersect => EntityMetadata.IsIntersect.GetValueOrDefault();

        public int ObjectTypeCode => EntityMetadata.ObjectTypeCode.GetValueOrDefault();

        public string DisplayName { get; private set; }

        private readonly bool _IsCustomizable = false;
        public override bool IsCustomizable => _IsCustomizable;

        public RoleEntityPrivilegeViewItem(EntityMetadata entityMetadata, IEnumerable<RolePrivilege> rolePrivileges, bool isCustomizable = false)
        {
            this.EntityMetadata = entityMetadata;
            this._IsCustomizable = isCustomizable;
            this.DisplayName = CreateFileHandler.GetLocalizedLabel(entityMetadata.DisplayName);

            LoadData(rolePrivileges, entityMetadata?.Privileges);
        }

        public void FillChangedPrivileges(Dictionary<Guid, PrivilegeDepth> privilegesAdd, HashSet<Guid> privilegesRemove)
        {
            SetPrivilegeLevel(this._availablePrivilegesTypes, this.InitialCreateRight, this._createRight, PrivilegeType.Create, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this.InitialReadRight, this._readRight, PrivilegeType.Read, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this.InitialUpdateRight, this._updateRight, PrivilegeType.Write, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this.InitialDeleteRight, this._deleteRight, PrivilegeType.Delete, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._availablePrivilegesTypes, this.InitialAppendRight, this._appendRight, PrivilegeType.Append, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this.InitialAppendToRight, this._appendToRight, PrivilegeType.AppendTo, privilegesAdd, privilegesRemove);

            SetPrivilegeLevel(this._availablePrivilegesTypes, this.InitialAssignRight, this._assignRight, PrivilegeType.Assign, privilegesAdd, privilegesRemove);
            SetPrivilegeLevel(this._availablePrivilegesTypes, this.InitialShareRight, this._shareRight, PrivilegeType.Share, privilegesAdd, privilegesRemove);
        }
    }
}
