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
    public abstract class BasePrivilegeViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging(string propertyName)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        #region GetPrivilegeLevel

        protected static PrivilegeDepthExtended GetPrivilegeLevel(
            IEnumerable<SecurityPrivilegeMetadata> privilegesEnum
            , IEnumerable<RolePrivilege> rolePrivileges
            , Dictionary<PrivilegeType, SecurityPrivilegeMetadata> availablePrivilegesTypes
            , PrivilegeType privilegeType
        )
        {
            if (privilegeType != PrivilegeType.None
                && privilegesEnum != null
                && privilegesEnum.Any()
            )
            {
                var privilege = privilegesEnum.FirstOrDefault(p => p.PrivilegeType == privilegeType);

                if (privilege != null)
                {
                    availablePrivilegesTypes.Add(privilegeType, privilege);

                    var rolePrivilege = rolePrivileges.FirstOrDefault(p => p.PrivilegeId == privilege.PrivilegeId);

                    if (rolePrivilege != null)
                    {
                        return (PrivilegeDepthExtended)rolePrivilege.Depth;
                    }
                }
            }

            return PrivilegeDepthExtended.None;
        }

        protected static PrivilegeDepthExtended GetPrivilegeLevel(
            IEnumerable<SecurityPrivilegeMetadata> privilegesEnum
            , IEnumerable<RolePrivileges> rolePrivileges
            , Dictionary<PrivilegeType, SecurityPrivilegeMetadata> availablePrivilegesTypes
            , PrivilegeType privilegeType
        )
        {
            if (privilegeType != PrivilegeType.None
                && privilegesEnum != null
                && privilegesEnum.Any()
            )
            {
                var privilege = privilegesEnum.FirstOrDefault(p => p.PrivilegeType == privilegeType);

                if (privilege != null)
                {
                    availablePrivilegesTypes.Add(privilegeType, privilege);

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

        protected static PrivilegeDepthExtended GetPrivilegeLevel(Guid privilegeId, IEnumerable<RolePrivilege> rolePrivileges)
        {
            var rolePrivilege = rolePrivileges.FirstOrDefault(p => p.PrivilegeId == privilegeId);

            if (rolePrivilege != null)
            {
                return (PrivilegeDepthExtended)rolePrivilege.Depth;
            }

            return PrivilegeDepthExtended.None;
        }

        protected static PrivilegeDepthExtended GetPrivilegeLevel(Guid privilegeId, IEnumerable<RolePrivileges> rolePrivileges)
        {
            var rolePrivilege = rolePrivileges.FirstOrDefault(p => p.PrivilegeId == privilegeId);

            if (rolePrivilege != null && rolePrivilege.PrivilegeDepthMask.HasValue)
            {
                var privilegeDepth = RolePrivilegesRepository.ConvertMaskToPrivilegeDepth(rolePrivilege.PrivilegeDepthMask.Value);

                if (privilegeDepth.HasValue)
                {
                    return (PrivilegeDepthExtended)privilegeDepth.Value;
                }
            }

            return PrivilegeDepthExtended.None;
        }

        #endregion GetPrivilegeLevel

        protected static void SetPrivilegeLevel(
            Dictionary<PrivilegeType, SecurityPrivilegeMetadata> availablePrivilegesTypes
            , PrivilegeDepthExtended initialValue
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
                || !availablePrivilegesTypes.ContainsKey(privilegeType)
            )
            {
                return;
            }

            var privilege = availablePrivilegesTypes[privilegeType];

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

        protected static void SetPrivilegeLevel(
            Dictionary<PrivilegeType, SecurityPrivilegeMetadata> availablePrivilegesTypes
            , PrivilegeDepthExtended? initialValue
            , PrivilegeDepthExtended? currentValue
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
                || !availablePrivilegesTypes.ContainsKey(privilegeType)
            )
            {
                return;
            }

            var privilege = availablePrivilegesTypes[privilegeType];

            if (privilege == null)
            {
                return;
            }

            if (currentValue == PrivilegeDepthExtended.None)
            {
                privilegesRemove.Add(privilege.PrivilegeId);
            }
            else if (currentValue.HasValue)
            {
                if (privilegesAdd.ContainsKey(privilege.PrivilegeId))
                {
                    privilegesAdd[privilege.PrivilegeId] = (PrivilegeDepth)Math.Max((int)currentValue.Value, (int)privilegesAdd[privilege.PrivilegeId]);
                }
                else
                {
                    privilegesAdd.Add(privilege.PrivilegeId, (PrivilegeDepth)currentValue.Value);
                }
            }
        }

        #region GetPrivilegeDepthsByAvailability

        private static readonly PrivilegeDepthExtended[] _optionsDefault = new PrivilegeDepthExtended[] { PrivilegeDepthExtended.None };

        private static ConcurrentDictionary<Tuple<bool, bool, bool, bool>, PrivilegeDepthExtended[]> _optionsCache = new ConcurrentDictionary<Tuple<bool, bool, bool, bool>, PrivilegeDepthExtended[]>();

        protected static PrivilegeDepthExtended[] GetPrivilegeDepthsByAvailability(bool canBeBasic, bool canBeLocal, bool canBeDeep, bool canBeGlobal)
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

        protected static PrivilegeDepthExtended[] ReturnOptions(PrivilegeType privilegeType, IEnumerable<SecurityPrivilegeMetadata> privilegesEnum)
        {
            if (privilegeType != PrivilegeType.None && privilegesEnum != null)
            {
                var privilege = privilegesEnum.FirstOrDefault(p => p.PrivilegeType == privilegeType);

                if (privilege != null)
                {
                    return GetPrivilegeDepthsByAvailability(privilege.CanBeBasic, privilege.CanBeLocal, privilege.CanBeDeep, privilege.CanBeGlobal);
                }
            }

            return _optionsDefault;
        }

        #endregion GetPrivilegeDepthsByAvailability
    }
}
