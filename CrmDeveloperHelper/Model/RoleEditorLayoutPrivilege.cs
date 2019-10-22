using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleEditorLayoutPrivilege
    {
        public Guid id { get; private set; }

        public string Name { get; private set; }

        public string LocId { get; private set; }

        public RoleEditorLayoutPrivilege(Guid id, string name, string locId)
        {
            this.id = id;
            this.Name = name;
            this.LocId = locId;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}