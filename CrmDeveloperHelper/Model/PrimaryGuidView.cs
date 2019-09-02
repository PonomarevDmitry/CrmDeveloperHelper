using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class PrimaryGuidView : IComparable, IComparable<PrimaryGuidView>, IEquatable<PrimaryGuidView>
    {
        public string LogicalName { get; private set; }

        public Guid Id { get; private set; }

        public ConnectionData ConnectionData { get; private set; }

        public PrimaryGuidView(ConnectionData connectionData, string logicalName, Guid idValue)
        {
            this.ConnectionData = connectionData;
            this.LogicalName = logicalName;
            this.Id = idValue;
        }

        public string Url => this.ConnectionData.GetEntityInstanceUrl(this.LogicalName, this.Id);

        public int CompareTo(PrimaryGuidView other)
        {
            if (other == null)
            {
                return -1;
            }

            if (this.ConnectionData != other.ConnectionData)
            {
                return this.ConnectionData.Name.CompareTo(other.ConnectionData.Name);
            }

            return this.Id.CompareTo(other.Id);
        }

        public int CompareTo(object obj)
        {
            return this.CompareTo(obj as PrimaryGuidView);
        }

        public bool Equals(PrimaryGuidView other)
        {
            if (this.ConnectionData != other.ConnectionData)
            {
                return false;
            }

            if (other == null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override string ToString()
        {
            return this.Id.ToString();
        }
    }
}
