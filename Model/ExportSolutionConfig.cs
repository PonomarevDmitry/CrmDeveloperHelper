using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class ExportSolutionConfig
    {
        public string ConnectionName { get; internal set; }

        public bool ExportAutoNumberingSettings { get; set; }

        public bool ExportCalendarSettings { get; set; }

        public bool ExportCustomizationSettings { get; set; }

        public bool ExportEmailTrackingSettings { get; set; }

        public bool ExportExternalApplications { get; set; }

        public string ExportFolder { get; internal set; }

        public bool ExportGeneralSettings { get; set; }

        public bool ExportIsvConfig { get; set; }

        public bool ExportMarketingSettings { get; set; }

        public bool ExportOutlookSynchronizationSettings { get; set; }

        public bool ExportRelationshipRoles { get; set; }

        public bool ExportSales { get; set; }

        public Guid IdSolution { get; internal set; }

        public bool Managed { get; set; }
    }
}