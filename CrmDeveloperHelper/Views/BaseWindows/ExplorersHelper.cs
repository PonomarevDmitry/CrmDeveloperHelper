using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public class ExplorersHelper
    {
        private readonly IWriteToOutput _iWriteToOutput;
        private readonly CommonConfiguration _commonConfig;
        private readonly Func<Task<IOrganizationServiceExtented>> GetService;

        public ExplorersHelper(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , Func<Task<IOrganizationServiceExtented>> getService
        )
        {
            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this.GetService = getService;
        }

        public void FillExplorers(MenuItem miExplorers)
        {
            var miCreateMetadataFile = new MenuItem()
            {
                Header = "Metadata",
            };
            miCreateMetadataFile.Click += miCreateMetadataFile_Click;

            var miGlobalOptionSets = new MenuItem()
            {
                Header = "Global OptionSets",
            };
            miGlobalOptionSets.Click += miGlobalOptionSets_Click;

            var miSystemForms = new MenuItem()
            {
                Header = "Forms",
            };
            miSystemForms.Click += miSystemForms_Click;

            var miSavedQuery = new MenuItem()
            {
                Header = "Views",
            };
            miSavedQuery.Click += miSavedQuery_Click;

            var miSavedChart = new MenuItem()
            {
                Header = "Charts",
            };
            miSavedChart.Click += miSavedChart_Click;

            var miWorkflows = new MenuItem()
            {
                Header = "Workflows",
            };
            miWorkflows.Click += miWorkflows_Click;

            var miPluginTree = new MenuItem()
            {
                Header = "Plugin Tree",
            };
            miPluginTree.Click += miPluginTree_Click;

            var miMessageExplorer = new MenuItem()
            {
                Header = "Message Explorer",
            };
            miMessageExplorer.Click += miMessageExplorer_Click;

            var miMessageFilterTree = new MenuItem()
            {
                Header = "Message Filter Tree",
            };
            miMessageFilterTree.Click += miMessageFilterTree_Click;

            var miMessageRequestTree = new MenuItem()
            {
                Header = "Plugin Steps Required Components",
            };
            miMessageRequestTree.Click += miMessageRequestTree_Click;

            //<MenuItem Header="Metadata" Click="miCreateMetadataFile_Click" />
            //<MenuItem Header="Global OptionSets" Click="miGlobalOptionSets_Click"/>
            //<Separator/>
            //<MenuItem Header="Forms" Click="miSystemForms_Click" />
            //<MenuItem Header="Views" Click="miSavedQuery_Click" />
            //<MenuItem Header="Charts" Click="miSavedChart_Click" />
            //<Separator/>
            //<MenuItem Header="Workflows" Click="miWorkflows_Click" />
            //<Separator/>
            //<MenuItem Header="Plugin Tree" Click="miPluginTree_Click" />
            //<MenuItem Header="Message Explorer" Click="miMessageExplorer_Click" />
            //<MenuItem Header="Message Filter Tree" Click="miMessageFilterTree_Click"/>
            //<MenuItem Header="Message Request Tree" Click="miMessageRequestTree_Click"/>


            miExplorers.Items.Add(miCreateMetadataFile);
            miExplorers.Items.Add(miGlobalOptionSets);
            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miSystemForms);
            miExplorers.Items.Add(miSavedQuery);
            miExplorers.Items.Add(miSavedChart);
            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miWorkflows);
            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miPluginTree);
            miExplorers.Items.Add(miMessageExplorer);
            miExplorers.Items.Add(miMessageFilterTree);
            miExplorers.Items.Add(miMessageRequestTree);
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void miCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsExplorer(
                this._iWriteToOutput
                , service
                , _commonConfig
            );
        }

        private async void miSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
        }

        private async void miSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
        }

        private async void miWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
        }

        private async void miPluginTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTree(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty, string.Empty);
        }

        private async void miMessageExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miMessageFilterTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageFilterTree(this._iWriteToOutput, service, _commonConfig, string.Empty, string.Empty);
        }

        private async void miMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTree(this._iWriteToOutput, service, _commonConfig);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

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

            var miCompareRibbon = new MenuItem()
            {
                Header = "Application Ribbon",
            };
            miCompareRibbon.Click += miCompareRibbon_Click;

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

            //<MenuItem Header="Organization Comparer" Click="miOrganizationComparer_Click"/>
            //<Separator/>
            //<MenuItem Header="Entity Metadata" Click="miCompareMetadataFile_Click"/>
            //<Separator/>
            //<MenuItem Header="Global OptionSets" Click="miCompareGlobalOptionSets_Click"/>
            //<Separator/>
            //<MenuItem Header="Application Ribbon" Click="miCompareRibbon_Click"/>
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
            miCompareOrganizations.Items.Add(miCompareRibbon);
            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareSystemForms);
            miCompareOrganizations.Items.Add(miCompareSavedQuery);
            miCompareOrganizations.Items.Add(miCompareSavedChart);
            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareWorkflows);
        }

        private async void miOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void miCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }
    }
}
