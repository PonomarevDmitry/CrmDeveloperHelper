using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public class CompareWindowsHelper : BaseExplorersHelper
    {
        private readonly Func<Tuple<ConnectionData, ConnectionData>> GetConnections;

        public CompareWindowsHelper(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , Func<Tuple<ConnectionData, ConnectionData>> getConnections
            , Func<string> getEntityName = null
            , Func<string> getGlobalOptionSetName = null
            , Func<string> getWorkflowName = null
            , Func<string> getSystemFormName = null
            , Func<string> getSavedQueryName = null
            , Func<string> getSavedQueryVisualizationName = null
            , Func<string> getSiteMapName = null
            , Func<string> getReportName = null
            , Func<string> getWebResourceName = null
            , Func<string> getPluginAssemblyName = null
        ) : base(iWriteToOutput, commonConfig
            , getEntityName
            , getGlobalOptionSetName
            , getWorkflowName
            , getSystemFormName
            , getSavedQueryName
            , getSavedQueryVisualizationName
            , getSiteMapName
            , getReportName
            , getWebResourceName
            , getPluginAssemblyName
        )
        {
            this.GetConnections = getConnections;
        }

        public void FillCompareWindows(ContextMenu contextMenu, params string[] uidList)
        {
            if (uidList == null || uidList.Length == 0)
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            FillCompareWindows(items, uidList);
        }

        public void FillCompareWindows(IEnumerable<Control> items, params string[] uidList)
        {
            if (uidList == null || uidList.Length == 0)
            {
                return;
            }

            HashSet<string> hash = new HashSet<string>(uidList, StringComparer.InvariantCultureIgnoreCase);

            IEnumerable<MenuItem> source = WindowBase.GetMenuItems(items);

            foreach (var item in source)
            {
                if (hash.Contains(item.Uid))
                {
                    FillCompareWindows(item);
                }
            }
        }

        public void FillCompareWindows(MenuItem miCompareOrganizations)
        {
            var miOrganizationComparer = new MenuItem()
            {
                Header = "Organization Comparer",
            };
            miOrganizationComparer.Click += miOrganizationComparer_Click;

            var miCompareMetadataFile = new MenuItem()
            {
                Header = "Entity Metadata",
            };
            miCompareMetadataFile.Click += miCompareMetadataFile_Click;

            var miCompareGlobalOptionSets = new MenuItem()
            {
                Header = "Global OptionSets",
            };
            miCompareGlobalOptionSets.Click += miCompareGlobalOptionSets_Click;

            var miCompareApplicationRibbon = new MenuItem()
            {
                Header = "ApplicationRibbon",
            };
            miCompareApplicationRibbon.Click += miCompareApplicationRibbon_Click;

            var miCompareSystemForms = new MenuItem()
            {
                Header = "Forms",
            };
            miCompareSystemForms.Click += miCompareSystemForms_Click;

            var miCompareSavedQuery = new MenuItem()
            {
                Header = "Views",
            };
            miCompareSavedQuery.Click += miCompareSavedQuery_Click;

            var miCompareSavedChart = new MenuItem()
            {
                Header = "Charts",
            };
            miCompareSavedChart.Click += miCompareSavedChart_Click;

            var miCompareWorkflows = new MenuItem()
            {
                Header = "Workflows",
            };
            miCompareWorkflows.Click += miCompareWorkflows_Click;

            var miCompareReports = new MenuItem()
            {
                Header = "Reports",
            };
            miCompareReports.Click += miCompareReports_Click;

            var miCompareSiteMaps = new MenuItem()
            {
                Header = "SiteMaps",
            };
            miCompareSiteMaps.Click += miCompareSiteMaps_Click;

            var miCompareWebResources = new MenuItem()
            {
                Header = "WebResources",
            };
            miCompareWebResources.Click += miCompareWebResources_Click;

            var miComparePluginAssemblies = new MenuItem()
            {
                Header = "Plugin Assemblies",
            };
            miComparePluginAssemblies.Click += miComparePluginAssemblies_Click;

            //<MenuItem Header="Organization Comparer" Click="miOrganizationComparer_Click"/>
            //<Separator/>
            //<MenuItem Header="Entity Metadata" Click="miCompareMetadataFile_Click"/>
            //<Separator/>
            //<MenuItem Header="Global OptionSets" Click="miCompareGlobalOptionSets_Click"/>
            //<Separator/>
            //<MenuItem Header="ApplicationRibbon" Click="miCompareApplicationRibbon_Click"/>
            //<Separator/>
            //<MenuItem Header="Forms" Click="miCompareSystemForms_Click"/>
            //<MenuItem Header="Views" Click="miCompareSavedQuery_Click"/>
            //<MenuItem Header="Charts" Click="miCompareSavedChart_Click"/>
            //<Separator/>
            //<MenuItem Header="Workflows" Click="miCompareWorkflows_Click"/>

            miCompareOrganizations.Items.Add(miOrganizationComparer);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareMetadataFile);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareGlobalOptionSets);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareApplicationRibbon);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareSystemForms);
            miCompareOrganizations.Items.Add(miCompareSavedQuery);
            miCompareOrganizations.Items.Add(miCompareSavedChart);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareWorkflows);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareReports);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareSiteMaps);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareWebResources);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miComparePluginAssemblies);
        }

        private void miOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, connectionTuple.Item1.ConnectionConfiguration, _commonConfig);
        }

        private void miCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, entityName);
        }

        private void miCompareApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2);
        }

        private void miCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var optionSetName = GetGlobalOptionSetName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, optionSetName, entityName);
        }

        private void miCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var systemFormName = GetSystemFormName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, entityName, systemFormName);
        }

        private void miCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var savedQueryName = GetSavedQueryName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, entityName, savedQueryName);
        }

        private void miCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var chartName = GetSavedQueryVisualizationName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, entityName, chartName);
        }

        private void miCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var workflowName = GetWorkflowName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, entityName, workflowName);
        }

        private void miCompareReports_Click(object sender, RoutedEventArgs e)
        {
            var reportName = GetReportName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerReportWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, reportName);
        }

        private void miCompareSiteMaps_Click(object sender, RoutedEventArgs e)
        {
            var siteMapName = GetSiteMapName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerSiteMapWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, siteMapName);
        }

        private void miCompareWebResources_Click(object sender, RoutedEventArgs e)
        {
            var webResourceName = GetWebResourceName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerWebResourcesWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, webResourceName);
        }

        private void miComparePluginAssemblies_Click(object sender, RoutedEventArgs e)
        {
            var pluginAssemblyName = GetPluginAssemblyName();

            _commonConfig.Save();

            var connectionTuple = GetConnections();

            WindowHelper.OpenOrganizationComparerPluginAssemblyWindow(this._iWriteToOutput, _commonConfig, connectionTuple.Item1, connectionTuple.Item2, pluginAssemblyName);
        }
    }
}