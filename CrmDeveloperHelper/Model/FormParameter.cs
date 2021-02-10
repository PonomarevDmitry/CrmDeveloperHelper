namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class FormParameter
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public FormParameter(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
