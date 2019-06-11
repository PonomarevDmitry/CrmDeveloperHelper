using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandAddObjectToSolutionLast : AbstractDynamicCommand<string>
    {
        private readonly Collection<string> EmptyCollection = new Collection<string>();

        public AbstractDynamicCommandAddObjectToSolutionLast(
            OleMenuCommandService commandService
            , int baseIdStart
        ) : base(
            commandService
            , baseIdStart
            , ConnectionData.CountLastSolutions
        )
        {

        }

        protected override ICollection<string> GetElementSourceCollection()
        {
            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig != null && connectionConfig.CurrentConnectionData != null)
            {
                return connectionConfig.CurrentConnectionData.LastSelectedSolutionsUniqueName;
            }

            return EmptyCollection;
        }

        protected override string GetElementName(string solutionUniqueName)
        {
            return solutionUniqueName;
        }
    }
}
