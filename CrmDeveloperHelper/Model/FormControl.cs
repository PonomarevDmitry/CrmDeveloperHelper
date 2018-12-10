using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class FormControl
    {
        public string Attribute { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ShowLabel { get; set; }

        public string ClassId { get; set; }

        public string Location { get; set; }

        public string Visible { get; set; }

        public string Disabled { get; set; }

        public string IndicationOfSubgrid { get; set; }

        public List<LabelString> Labels { get; private set; }

        public string Parameters { get; set; }

        public FormControl()
        {
            this.Labels = new List<LabelString>();
        }

        public string GetControlType()
        {
            switch (this.ClassId)
            {
                case "{4273EDBD-AC1D-40d3-9FB2-095C621B552D}":
                    return "string";

                case "{270BD3DB-D9AF-4782-9025-509E298DEC0A}":
                    return "lookup";

                case "{3EF39988-22BB-4f0b-BBBE-64B5A3748AEE}":
                    return "optionset";

                case "{C3EFE0C3-0EC6-42be-8349-CBD9079DFD8E}":
                    return "decimal";

                case "{533B9E00-756B-4312-95A0-DC888637AC78}":
                    return "money";

                case "{E0DECE4B-6FC8-4a8f-A065-082708572369}":
                    return "multistring";

                case "{5B773807-9FB2-42db-97C3-7A91EFF8ADFF}":
                    return "date";

                case "{67FAC785-CD58-4f9f-ABB3-4B7DDC6ED5ED}":
                    return "boolean";

                case "{E7A81278-8635-4d9e-8D4D-59480B391C5B}":
                    return "SubGrid";

                case "{C6D124CA-7EDA-4a60-AEA9-7FB8D318B68F}":
                    return "integer";

                case "{9FDF5F91-88B1-47f4-AD53-C11EFC01A01D}":
                    return "WebResource";

                case "{5C5600E0-1D6E-4205-A272-BE80DA87FD42}":
                    return "QuickViewForm";

                case "{06375649-c143-495e-a496-c962e5b4488e}":
                    return "NotesControl";

                case "{5D68B988-0661-4db2-BC3E-17598AD3BE6C}":
                    return "StatusCode";

                case "{B0C6723A-8503-4fd7-BB28-C8A06AC933C2}":
                    return "booleanCheckBox";

                default:
                    break;
            }

            return "UnknownControl";
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}