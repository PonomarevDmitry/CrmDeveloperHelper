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

            Double,

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

        private static readonly Guid guidString = Guid.Parse("{4273EDBD-AC1D-40d3-9FB2-095C621B552D}");
        private static readonly Guid guidLookup = Guid.Parse("{270BD3DB-D9AF-4782-9025-509E298DEC0A}");
        private static readonly Guid guidOptionSet = Guid.Parse("{3EF39988-22BB-4f0b-BBBE-64B5A3748AEE}");
        private static readonly Guid guidDecimal = Guid.Parse("{C3EFE0C3-0EC6-42be-8349-CBD9079DFD8E}");
        private static readonly Guid guidMoney = Guid.Parse("{533B9E00-756B-4312-95A0-DC888637AC78}");
        private static readonly Guid guidMultiString = Guid.Parse("{E0DECE4B-6FC8-4a8f-A065-082708572369}");
        private static readonly Guid guidDate = Guid.Parse("{5B773807-9FB2-42db-97C3-7A91EFF8ADFF}");
        private static readonly Guid guidBoolean = Guid.Parse("{67FAC785-CD58-4f9f-ABB3-4B7DDC6ED5ED}");
        private static readonly Guid guidBooleanCheckBox = Guid.Parse("{B0C6723A-8503-4fd7-BB28-C8A06AC933C2}");
        private static readonly Guid guidDouble = Guid.Parse("{0D2C745A-E5A8-4c8f-BA63-C6D3BB604660}");
        private static readonly Guid guidSubGrid = Guid.Parse("{E7A81278-8635-4d9e-8D4D-59480B391C5B}");
        private static readonly Guid guidEditableSubGrid = Guid.Parse("{F9A8A302-114E-466A-B582-6771B2AE0D92}");
        private static readonly Guid guidInteger = Guid.Parse("{C6D124CA-7EDA-4a60-AEA9-7FB8D318B68F}");
        private static readonly Guid guidWebResource = Guid.Parse("{9FDF5F91-88B1-47f4-AD53-C11EFC01A01D}");
        private static readonly Guid guidQuickViewForm = Guid.Parse("{5C5600E0-1D6E-4205-A272-BE80DA87FD42}");
        private static readonly Guid guidNotesControl = Guid.Parse("{06375649-c143-495e-a496-c962e5b4488e}");
        private static readonly Guid guidStatusCode = Guid.Parse("{5D68B988-0661-4db2-BC3E-17598AD3BE6C}");
        private static readonly Guid guidMultiSelectPicklist = Guid.Parse("{4AA28AB7-9C13-4F57-A73D-AD894D048B5F}");
        private static readonly Guid guidIFrame = Guid.Parse("{FD2A7985-3187-444e-908D-6624B21F69C0}");
        private static readonly Guid guidWebSiteUrl = Guid.Parse("{71716B6C-711E-476c-8AB8-5D11542BFB47}");
        private static readonly Guid guidEmailAddress = Guid.Parse("{ADA2203E-B4CD-49be-9DDF-234642B43B52}");
        private static readonly Guid guidTickerSymbol = Guid.Parse("{1E1FC551-F7A8-43af-AC34-A8DC35C7B6D4}");
        private static readonly Guid guidDuration = Guid.Parse("{AA987274-CE4E-4271-A803-66164311A958}");
        private static readonly Guid guidLanguageCode = Guid.Parse("{B634828E-C390-444A-AFE6-E07315D9D970}");
        private static readonly Guid guidRelatedQuickViewForm = Guid.Parse("{B68B05F0-A46D-43F8-843B-917920AF806A}");
        private static readonly Guid guidPartyList = Guid.Parse("{CBFB742C-14E7-4a17-96BB-1A13F7F64AA2}");
        private static readonly Guid guidRegardingObject = Guid.Parse("{F3015350-44A2-4aa0-97B5-00166532B5E9}");

        //private static readonly Guid guid = ;

        public FormControlType GetControlType()
        {
            if (Guid.TryParse(this.ClassId, out var tempGuid))
            {
                if (tempGuid == guidString)
                {
                    return FormControlType.String;
                }
                else if (tempGuid == guidLookup)
                {
                    return FormControlType.Lookup;
                }
                else if (tempGuid == guidOptionSet)
                {
                    return FormControlType.OptionSet;
                }
                else if (tempGuid == guidDecimal)
                {
                    return FormControlType.Decimal;
                }
                else if (tempGuid == guidMoney)
                {
                    return FormControlType.Money;
                }
                else if (tempGuid == guidMultiString)
                {
                    return FormControlType.MultiString;
                }
                else if (tempGuid == guidDate)
                {
                    return FormControlType.Date;
                }
                else if (tempGuid == guidDouble)
                {
                    return FormControlType.Double;
                }
                else if (tempGuid == guidBoolean)
                {
                    return FormControlType.Boolean;
                }
                else if (tempGuid == guidBooleanCheckBox)
                {
                    return FormControlType.BooleanCheckBox;
                }
                else if (tempGuid == guidSubGrid)
                {
                    return FormControlType.SubGrid;
                }
                else if (tempGuid == guidEditableSubGrid)
                {
                    return FormControlType.EditableSubGrid;
                }
                else if (tempGuid == guidInteger)
                {
                    return FormControlType.Integer;
                }
                else if (tempGuid == guidWebResource)
                {
                    return FormControlType.WebResource;
                }
                else if (tempGuid == guidQuickViewForm)
                {
                    return FormControlType.QuickViewForm;
                }
                else if (tempGuid == guidNotesControl)
                {
                    return FormControlType.NotesControl;
                }
                else if (tempGuid == guidStatusCode)
                {
                    return FormControlType.StatusCode;
                }
                else if (tempGuid == guidMultiSelectPicklist)
                {
                    return FormControlType.MultiSelectPicklist;
                }
                else if (tempGuid == guidIFrame)
                {
                    return FormControlType.IFrame;
                }
                else if (tempGuid == guidWebSiteUrl)
                {
                    return FormControlType.WebSiteUrl;
                }
                else if (tempGuid == guidEmailAddress)
                {
                    return FormControlType.EmailAddress;
                }
                else if (tempGuid == guidTickerSymbol)
                {
                    return FormControlType.TickerSymbol;
                }
                else if (tempGuid == guidDuration)
                {
                    return FormControlType.Duration;
                }
                else if (tempGuid == guidLanguageCode)
                {
                    return FormControlType.LanguageCode;
                }
                else if (tempGuid == guidRelatedQuickViewForm)
                {
                    return FormControlType.RelatedQuickViewForm;
                }
                else if (tempGuid == guidPartyList)
                {
                    return FormControlType.PartyList;
                }
                else if (tempGuid == guidRegardingObject)
                {
                    return FormControlType.RegardingObject;
                }
                //else if (tempGuid == )
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