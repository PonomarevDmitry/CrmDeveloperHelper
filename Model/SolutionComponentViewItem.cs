using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class SolutionComponentViewItem
    {
        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public string ComponentType { get; private set; }

        public string IsManaged { get; private set; }

        public string IsCustomizable { get; private set; }

        public SolutionComponent SolutionComponent { get; private set; }

        public SolutionComponentViewItem(
            SolutionComponent solutionComponent
            , string name
            , string displayName
            , string componentType
            , string managed
            , string customizable
            )
        {
            this.SolutionComponent = solutionComponent;

            this.Name = name;
            this.DisplayName = displayName;
            this.ComponentType = componentType;
            this.IsManaged = managed;
            this.IsCustomizable = customizable;
        }
    }
}