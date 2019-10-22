namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleEditorLayoutEntity
    {
        public int EntityCode { get; private set; }

        public string Name { get; private set; }

        public string LocId { get; private set; }

        public RoleEditorLayoutEntity(string name, int entityCode, string locId)
        {
            this.Name = name;
            this.EntityCode = entityCode;
            this.LocId = locId;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Name, this.EntityCode);
        }
    }
}