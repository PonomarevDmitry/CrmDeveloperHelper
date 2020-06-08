using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandDefaultSiteMap : AbstractDynamicCommand<string>
    {
        private static string[] ListDefaultSiteMaps { get; } = new string[]
        {
            "2011"
            , "2013"
            , "2015"
            , "2015SP1"
            , "2016"
            , "2016SP1"
            , "365.8.2"
        };

        protected AbstractDynamicCommandDefaultSiteMap(
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
            return ListDefaultSiteMaps;
        }

        protected override string GetElementName(string siteMapName)
        {
            return siteMapName;
        }
    }
}
