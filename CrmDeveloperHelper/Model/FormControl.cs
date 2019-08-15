using System;
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

        public enum FormControlType
        {
            UnknownControl,

            String,

            Lookup,

            OptionSet,

            Decimal,

            Money,

            MultiString,

            Date,

            Boolean,

            SubGrid,

            EditableSubGrid,

            Integer,

            WebResource,

            QuickViewForm,

            NotesControl,

            StatusCode,

            BooleanCheckBox,

            MultiSelectPicklist,

            IFrame,

            WebSiteUrl,

            EmailAddress,

            TickerSymbol,

            Duration,

            LanguageCode,

            RelatedQuickViewForm,

            PartyList,
            RegardingObject,
        }

        public FormControlType GetControlType()
        {
            if (Guid.TryParse(this.ClassId, out var tempGuid))
            {
                if (tempGuid == Guid.Parse("{4273EDBD-AC1D-40d3-9FB2-095C621B552D}"))
                {
                    return FormControlType.String;
                }
                else if (tempGuid == Guid.Parse("{270BD3DB-D9AF-4782-9025-509E298DEC0A}"))
                {
                    return FormControlType.Lookup;
                }
                else if (tempGuid == Guid.Parse("{3EF39988-22BB-4f0b-BBBE-64B5A3748AEE}"))
                {
                    return FormControlType.OptionSet;
                }
                else if (tempGuid == Guid.Parse("{C3EFE0C3-0EC6-42be-8349-CBD9079DFD8E}"))
                {
                    return FormControlType.Decimal;
                }
                else if (tempGuid == Guid.Parse("{533B9E00-756B-4312-95A0-DC888637AC78}"))
                {
                    return FormControlType.Money;
                }
                else if (tempGuid == Guid.Parse("{E0DECE4B-6FC8-4a8f-A065-082708572369}"))
                {
                    return FormControlType.MultiString;
                }
                else if (tempGuid == Guid.Parse("{5B773807-9FB2-42db-97C3-7A91EFF8ADFF}"))
                {
                    return FormControlType.Date;
                }
                else if (tempGuid == Guid.Parse("{67FAC785-CD58-4f9f-ABB3-4B7DDC6ED5ED}"))
                {
                    return FormControlType.Boolean;
                }
                else if (tempGuid == Guid.Parse("{E7A81278-8635-4d9e-8D4D-59480B391C5B}"))
                {
                    return FormControlType.SubGrid;
                }
                else if (tempGuid == Guid.Parse("{C6D124CA-7EDA-4a60-AEA9-7FB8D318B68F}"))
                {
                    return FormControlType.Integer;
                }
                else if (tempGuid == Guid.Parse("{9FDF5F91-88B1-47f4-AD53-C11EFC01A01D}"))
                {
                    return FormControlType.WebResource;
                }
                else if (tempGuid == Guid.Parse("{5C5600E0-1D6E-4205-A272-BE80DA87FD42}"))
                {
                    return FormControlType.QuickViewForm;
                }
                else if (tempGuid == Guid.Parse("{06375649-c143-495e-a496-c962e5b4488e}"))
                {
                    return FormControlType.NotesControl;
                }
                else if (tempGuid == Guid.Parse("{5D68B988-0661-4db2-BC3E-17598AD3BE6C}"))
                {
                    return FormControlType.StatusCode;
                }
                else if (tempGuid == Guid.Parse("{B0C6723A-8503-4fd7-BB28-C8A06AC933C2}"))
                {
                    return FormControlType.BooleanCheckBox;
                }
                else if (tempGuid == Guid.Parse("{4AA28AB7-9C13-4F57-A73D-AD894D048B5F}"))
                {
                    return FormControlType.MultiSelectPicklist;
                }
                else if (tempGuid == Guid.Parse("{FD2A7985-3187-444e-908D-6624B21F69C0}"))
                {
                    return FormControlType.IFrame;
                }
                else if (tempGuid == Guid.Parse("{71716B6C-711E-476c-8AB8-5D11542BFB47}"))
                {
                    return FormControlType.WebSiteUrl;
                }
                else if (tempGuid == Guid.Parse("{ADA2203E-B4CD-49be-9DDF-234642B43B52}"))
                {
                    return FormControlType.EmailAddress;
                }
                else if (tempGuid == Guid.Parse("{1E1FC551-F7A8-43af-AC34-A8DC35C7B6D4}"))
                {
                    return FormControlType.TickerSymbol;
                }
                else if (tempGuid == Guid.Parse("{AA987274-CE4E-4271-A803-66164311A958}"))
                {
                    return FormControlType.Duration;
                }
                else if (tempGuid == Guid.Parse("{B634828E-C390-444A-AFE6-E07315D9D970}"))
                {
                    return FormControlType.LanguageCode;
                }
                else if (tempGuid == Guid.Parse("{B68B05F0-A46D-43F8-843B-917920AF806A}"))
                {
                    return FormControlType.RelatedQuickViewForm;
                }
                else if (tempGuid == Guid.Parse("{CBFB742C-14E7-4a17-96BB-1A13F7F64AA2}"))
                {
                    return FormControlType.PartyList;
                }
                else if (tempGuid == Guid.Parse("{F3015350-44A2-4aa0-97B5-00166532B5E9}"))
                {
                    return FormControlType.RegardingObject;
                }
                //else if (tempGuid == Guid.Parse(""))
                //{
                //    return FormControlType.;
                //}
                //else if (tempGuid == Guid.Parse(""))
                //{
                //    return FormControlType.;
                //}
                //else if (tempGuid == Guid.Parse(""))
                //{
                //    return FormControlType.;
                //}
                //else if (tempGuid == Guid.Parse(""))
                //{
                //    return FormControlType.;
                //}
                //else if (tempGuid == Guid.Parse(""))
                //{
                //    return FormControlType.;
                //}


                //else if (tempGuid == Guid.Parse(""))
                //{
                //    return FormControlType.;
                //}
            }

            return FormControlType.UnknownControl;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}