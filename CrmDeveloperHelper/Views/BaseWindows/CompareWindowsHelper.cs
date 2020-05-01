using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public class CompareWindowsHelper
    {
        private readonly IWriteToOutput _iWriteToOutput;
        private readonly CommonConfiguration _commonConfig;

        private readonly Func<ConnectionData> GetConnection1;
        private readonly Func<ConnectionData> GetConnection2;

        private readonly Func<string> _getEntityName;
        private readonly Func<string> _getGlobalOptionSetName;
        private readonly Func<string> _getWorkflowName;
        private readonly Func<string> _getSystemFormName;
        private readonly Func<string> _getSavedQueryName;
        private readonly Func<string> _getSavedQueryVisualizationName;
        private readonly Func<string> _getSiteMapName;
        private readonly Func<string> _getReportName;
        private readonly Func<string> _getWebResourceName;
        private readonly Func<string> _getPluginAssemblyName;

        public CompareWindowsHelper(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , Func<ConnectionData> getConnection1
            , Func<ConnectionData> getConnection2
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
        )
        {
            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this.GetConnection1 = getConnection1;
            this.GetConnection2 = getConnection2;

            this._getEntityName = getEntityName;
            this._getGlobalOptionSetName = getGlobalOptionSetName;
            this._getWorkflowName = getWorkflowName;
            this._getSystemFormName = getSystemFormName;
            this._getSavedQueryName = getSavedQueryName;
            this._getSavedQueryVisualizationName = getSavedQueryVisualizationName;
            this._getSiteMapName = getSiteMapName;
            this._getReportName = getReportName;
            this._getWebResourceName = getWebResourceName;
            this._getPluginAssemblyName = getPluginAssemblyName;
        }

        private string GetEntityName()
        {
            if (_getEntityName != null)
            {
                return _getEntityName();
            }

            return string.Empty;
        }

        private string GetGlobalOptionSetName()
        {
            if (_getGlobalOptionSetName != null)
            {
                return _getGlobalOptionSetName();
            }

            return string.Empty;
        }

        private string GetWorkflowName()
        {
            if (_getWorkflowName != null)
            {
                return _getWorkflowName();
            }

            return string.Empty;
        }

        private string GetSystemFormName()
        {
            if (_getSystemFormName != null)
            {
                return _getSystemFormName();
            }

            return string.Empty;
        }

        private string GetSavedQueryName()
        {
            if (_getSavedQueryName != null)
            {
                return _getSavedQueryName();
            }

            return string.Empty;
        }

        private string GetSavedQueryVisualizationName()
        {
            if (_getSavedQueryVisualizationName != null)
            {
                return _getSavedQueryVisualizationName();
            }

            return string.Empty;
        }

        private string GetSiteMapName()
        {
            if (_getSiteMapName != null)
            {
                return _getSiteMapName();
            }

            return string.Empty;
        }

        private string GetReportName()
        {
            if (_getReportName != null)
            {
                return _getReportName();
            }

            return string.Empty;
        }

        private string GetWebResourceName()
        {
            if (_getWebResourceName != null)
            {
                return _getWebResourceName();
            }

            return string.Empty;
        }

        private string GetPluginAssemblyName()
        {
            if (_getPluginAssemblyName != null)
            {
                return _getPluginAssemblyName();
            }

            return string.Empty;
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

            var connection1 = GetConnection1();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, connection1.ConnectionConfiguration, _commonConfig);
        }

        private void miCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, entityName);
        }

        private void miCompareApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, connection1, connection2);
        }

        private void miCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var optionSetName = GetGlobalOptionSetName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, optionSetName, entityName);
        }

        private void miCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var systemFormName = GetSystemFormName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, entityName, systemFormName);
        }

        private void miCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var savedQueryName = GetSavedQueryName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, entityName, savedQueryName);
        }

        private void miCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var chartName = GetSavedQueryVisualizationName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, entityName, chartName);
        }

        private void miCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var workflowName = GetWorkflowName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, entityName, workflowName);
        }

        private void miCompareReports_Click(object sender, RoutedEventArgs e)
        {
            var reportName = GetReportName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerReportWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, reportName);
        }

        private void miCompareSiteMaps_Click(object sender, RoutedEventArgs e)
        {
            var siteMapName = GetSiteMapName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerSiteMapWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, siteMapName);
        }

        private void miCompareWebResources_Click(object sender, RoutedEventArgs e)
        {
            var webResourceName = GetWebResourceName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerWebResourcesWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, webResourceName);
        }

        private void miComparePluginAssemblies_Click(object sender, RoutedEventArgs e)
        {
            var pluginAssemblyName = GetPluginAssemblyName();

            _commonConfig.Save();

            var connection1 = GetConnection1();
            var connection2 = GetConnection2();

            WindowHelper.OpenOrganizationComparerPluginAssemblyWindow(this._iWriteToOutput, _commonConfig, connection1, connection2, pluginAssemblyName);
        }
    }
}