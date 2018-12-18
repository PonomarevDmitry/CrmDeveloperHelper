using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract(Name = "ConnectionConfiguration")]
    internal class ConnectionConfigurationBackward
    {
        [DataMember]
        public List<ConnectionData> Connections { get; set; }

        [DataMember]
        public List<ConnectionData> ArchiveConnections { get; set; }
    }
}