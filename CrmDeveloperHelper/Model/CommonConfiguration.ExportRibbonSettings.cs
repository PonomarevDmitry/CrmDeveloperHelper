using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class CommonConfiguration
    {
        private bool _ExportRibbonXmlForm = true;
        /// <summary>
        /// Экспортировать риббон расположения Form
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlForm
        {
            get => _ExportRibbonXmlForm;
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlForm));
                this._ExportRibbonXmlForm = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlForm));
            }
        }

        private bool _ExportRibbonXmlHomepageGrid = true;
        /// <summary>
        /// Экспортировать риббон расположения HomepageGrid 
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlHomepageGrid
        {
            get => _ExportRibbonXmlHomepageGrid;
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlHomepageGrid));
                this._ExportRibbonXmlHomepageGrid = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlHomepageGrid));
            }
        }

        private bool _ExportRibbonXmlSubGrid = true;
        /// <summary>
        /// Экспортировать риббон расположения SubGrid
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlSubGrid
        {
            get => _ExportRibbonXmlSubGrid;
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlSubGrid));
                this._ExportRibbonXmlSubGrid = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlSubGrid));
            }
        }

        private bool _SortRibbonCommandsAndRulesById = false;
        [DataMember]
        public bool SortRibbonCommandsAndRulesById
        {
            get => _SortRibbonCommandsAndRulesById;
            set
            {
                this.OnPropertyChanging(nameof(SortRibbonCommandsAndRulesById));
                this._SortRibbonCommandsAndRulesById = value;
                this.OnPropertyChanged(nameof(SortRibbonCommandsAndRulesById));
            }
        }

        private void LoadFromDiskExportRibbon(CommonConfiguration diskData)
        {
            this.ExportRibbonXmlForm = diskData.ExportRibbonXmlForm;
            this.ExportRibbonXmlHomepageGrid = diskData.ExportRibbonXmlHomepageGrid;
            this.ExportRibbonXmlSubGrid = diskData.ExportRibbonXmlSubGrid;

            this.SortRibbonCommandsAndRulesById = diskData.SortRibbonCommandsAndRulesById;
        }

        public Microsoft.Crm.Sdk.Messages.RibbonLocationFilters GetRibbonLocationFilters()
        {
            var filter = Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.All;

            if (this.ExportRibbonXmlForm || this.ExportRibbonXmlHomepageGrid || this.ExportRibbonXmlSubGrid)
            {
                filter = 0;

                if (this.ExportRibbonXmlForm)
                {
                    filter |= Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.Form;
                }

                if (this.ExportRibbonXmlHomepageGrid)
                {
                    filter |= Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.HomepageGrid;
                }

                if (this.ExportRibbonXmlSubGrid)
                {
                    filter |= Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.SubGrid;
                }
            }

            return filter;
        }
    }
}
