using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;

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
        ) : base(commandService, PackageGuids.guidDynamicDefaultSiteMapCommandSet, baseIdStart, ListDefaultSiteMaps.Length)
        {
        }

        protected override IList<string> GetElementSourceCollection()
        {
            return ListDefaultSiteMaps;
        }

        protected override string GetElementName(string siteMapName)
        {
            return siteMapName;
        }
    }
}
