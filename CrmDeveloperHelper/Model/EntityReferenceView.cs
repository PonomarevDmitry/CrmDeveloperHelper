using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityReferenceView : PrimaryGuidView
    {
        public string Name { get; private set; }

        public EntityReferenceView(ConnectionData connectionData, string logicalName, Guid idValue, string name)
            : base(connectionData, logicalName, idValue)
        {
            this.Name = name;

            if (string.IsNullOrEmpty(name))
            {
                this.Name = string.Format("{0} - {1}", LogicalName, Id);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
