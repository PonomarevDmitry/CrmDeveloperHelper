﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandConnectionPair : AbstractDynamicCommand<Tuple<ConnectionData, ConnectionData>>
    {
        private readonly string _formatButtonName;

        protected AbstractDynamicCommandConnectionPair(OleMenuCommandService commandService, int baseIdStart, string formatButtonName)
            : base(commandService, PackageGuids.guidDynamicConnectionPairCommandSet, baseIdStart, ConnectionData.CountConnectionPairQuickList)
        {
            this._formatButtonName = formatButtonName;
        }

        protected override IList<Tuple<ConnectionData, ConnectionData>> GetElementSourceCollection()
        {
            var connectionConfig = ConnectionConfiguration.Get();

            return connectionConfig.GetConnectionPairsByGroup();
        }

        protected override string GetElementName(Tuple<ConnectionData, ConnectionData> connectionDataPair)
        {
            return string.Format(_formatButtonName, connectionDataPair.Item1.Name, connectionDataPair.Item2.Name);
        }
    }
}