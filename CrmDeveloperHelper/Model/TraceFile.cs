using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class TraceFile
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string Name { get; set; }

        public DateTime LocalTime { get; set; }

        public string Categories { get; set; }

        public string CallStackOn { get; set; }

        public string ComputerName { get; set; }

        public string CRMVersion { get; set; }

        public string DeploymentType { get; set; }

        public string ScaleGroup { get; set; }

        public string ServerRole { get; set; }

        public override string ToString()
        {
            return this.FileName;
        }
    }
}