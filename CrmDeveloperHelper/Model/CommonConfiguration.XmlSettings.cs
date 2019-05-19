using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class CommonConfiguration
    {
        private bool _ExportXmlAttributeOnNewLine;
        [DataMember]
        public bool ExportXmlAttributeOnNewLine
        {
            get => _ExportXmlAttributeOnNewLine;
            set
            {
                this.OnPropertyChanging(nameof(ExportXmlAttributeOnNewLine));
                this._ExportXmlAttributeOnNewLine = value;
                this.OnPropertyChanged(nameof(ExportXmlAttributeOnNewLine));
            }
        }

        private bool _SetXmlSchemasDuringExport = true;
        [DataMember]
        public bool SetXmlSchemasDuringExport
        {
            get => _SetXmlSchemasDuringExport;
            set
            {
                this.OnPropertyChanging(nameof(SetXmlSchemasDuringExport));
                this._SetXmlSchemasDuringExport = value;
                this.OnPropertyChanged(nameof(SetXmlSchemasDuringExport));
            }
        }

        private bool _SortFormXmlElements = false;
        [DataMember]
        public bool SortFormXmlElements
        {
            get => _SortFormXmlElements;
            set
            {
                this.OnPropertyChanging(nameof(SortFormXmlElements));
                this._SortFormXmlElements = value;
                this.OnPropertyChanged(nameof(SortFormXmlElements));
            }
        }

        private bool _SortXmlAttributes = false;
        [DataMember]
        public bool SortXmlAttributes
        {
            get => _SortXmlAttributes;
            set
            {
                this.OnPropertyChanging(nameof(SortXmlAttributes));
                this._SortXmlAttributes = value;
                this.OnPropertyChanged(nameof(SortXmlAttributes));
            }
        }

        private bool _SetIntellisenseContext = true;
        [DataMember]
        public bool SetIntellisenseContext
        {
            get => _SetIntellisenseContext;
            set
            {
                this.OnPropertyChanging(nameof(SetIntellisenseContext));
                this._SetIntellisenseContext = value;
                this.OnPropertyChanged(nameof(SetIntellisenseContext));
            }
        }

        private void LoadFromDiskXml(CommonConfiguration diskData)
        {
            this.ExportXmlAttributeOnNewLine = diskData.ExportXmlAttributeOnNewLine;
            this.SetXmlSchemasDuringExport = diskData.SetXmlSchemasDuringExport;
            this.SetIntellisenseContext = diskData.SetIntellisenseContext;
            this.SortXmlAttributes = diskData.SortXmlAttributes;
            this.SortFormXmlElements = diskData.SortFormXmlElements;
        }
    }
}
