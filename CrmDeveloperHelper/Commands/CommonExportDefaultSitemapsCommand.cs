using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportDefaultSitemapsCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => _package;

        private const int _baseIdStart = PackageIds.CommonExportDefaultSitemapsCommandId;

        internal static string[] ListDefaultSitemaps { get; private set; } = new string[]
        {
            "2011"
            , "2013"
            , "2015"
            , "2015SP1"
            , "2016"
            , "2016SP1"
            , "365.8.2"
        };

        private CommonExportDefaultSitemapsCommand(Package package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                for (int i = 0; i < ListDefaultSitemaps.Length; i++)
                {
                    CommandID menuCommandID = new CommandID(PackageGuids.guidDynamicCommandSet, _baseIdStart + i);

                    OleMenuCommand menuCommand = new OleMenuCommand(menuItemCallback, menuCommandID);

                    menuCommand.Enabled = menuCommand.Visible = true;

                    menuCommand.Text = ListDefaultSitemaps[i];

                    commandService.AddCommand(menuCommand);
                }
            }
        }

        public static CommonExportDefaultSitemapsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportDefaultSitemapsCommand(package);
        }

        private void menuItemCallback(object sender, EventArgs e)
        {
            try
            {
                OleMenuCommand menuCommand = sender as OleMenuCommand;
                if (menuCommand == null)
                {
                    return;
                }

                EnvDTE80.DTE2 applicationObject = ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                if (applicationObject == null)
                {
                    return;
                }

                int index = menuCommand.CommandID.ID - _baseIdStart;

                if (0 <= index && index < ListDefaultSitemaps.Length)
                {
                    string selectedSitemap = ListDefaultSitemaps[index];

                    DTEHelper helper = DTEHelper.Create(applicationObject);

                    helper.HandleExportDefaultSitemap(selectedSitemap);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}
