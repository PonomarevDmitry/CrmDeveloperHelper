using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public static class WindowHelper
    {
        public static void OpenEntityMetadataWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filterEntityName = null
            , string filePath = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportEntityMetadata(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        , entityMetadataList
                        , null
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenEntityAttributeExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowEntityAttributeExplorer(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenEntityKeyExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowEntityKeyExplorer(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenEntityRelationshipOneToManyExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowEntityRelationshipOneToManyExplorer(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenEntityRelationshipManyToManyExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowEntityRelationshipManyToManyExplorer(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenEntitySecurityRolesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filterEntityName = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowEntitySecurityRoleExplorer(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , entityMetadataList
                        , filterEntityName
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenApplicationRibbonWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportRibbon(
                        iWriteToOutput
                        , service
                        , commonConfig
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSystemFormWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            , string selection = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportSystemForm(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        , string.Empty
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSavedQueryWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            , string selection = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportSavedQuery(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        , string.Empty
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSavedQueryVisualizationWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            , string selection = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportSavedQueryVisualization(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        , string.Empty
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenWorkflowWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            , string selection = null
            )
        {

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportWorkflow(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , filterEntityName
                        , selection
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenTraceReaderWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            )
        {

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowTraceReader(
                        iWriteToOutput
                        , service
                        , commonConfig
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenPluginTreeWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter = null
            , string pluginTypeFilter = null
            , string messageFilter = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowPluginTree(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , entityFilter
                        , pluginTypeFilter
                        , messageFilter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSdkMessageTreeWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter = null
            , string messageFilter = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowSdkMessageTree(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , entityFilter
                        , messageFilter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSdkMessageRequestTreeWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter = null
            , string messageFilter = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowSdkMessageRequestTree(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , entityFilter
                        , messageFilter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSystemUsersExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filter = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowSystemUserExplorer(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , entityMetadataList
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenTeamsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filter = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowTeamExplorer(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , entityMetadataList
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenRolesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filter = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowRoleExplorer(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , entityMetadataList
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSolutionComponentDependenciesWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , int componentType
            , Guid objectId
            , string selection = null
            )
        {
            var worker = new Thread(() =>
            {
                try
                {
                    var form = new WindowExplorerSolutionComponentDependencies(
                        iWriteToOutput
                        , service
                        , descriptor
                        , commonConfig
                        , componentType
                        , objectId
                        , selection
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    iWriteToOutput.WriteErrorToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenExplorerSolutionWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , int? componentType
            , Guid? objectId
            , EnvDTE.SelectedItem selectedItem
            )
        {
            var worker = new Thread(() =>
            {
                try
                {
                    var form = new WindowExplorerSolution(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , componentType
                        , objectId
                        , selectedItem
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    iWriteToOutput.WriteErrorToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSolutionComponentDependenciesWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , string selection = null
            )
        {
            var worker = new Thread(() =>
            {
                try
                {
                    var form = new WindowExplorerSolutionComponents(iWriteToOutput, service, descriptor, commonConfig, solutionUniqueName, selection);

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    iWriteToOutput.WriteErrorToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenGlobalOptionSetsWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<OptionSetMetadata> optionSets
            , string filePath = null
            , string selection = null
            )
        {

            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportGlobalOptionSets(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , optionSets
                        , filePath
                        , selection
                      );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenExportReportWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportReport(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , selection
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenExportSiteMapWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportSiteMap(
                        iWriteToOutput
                        , service
                        , commonConfig
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenExportWebResourcesWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportWebResource(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , selection
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenPluginTypeWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportPluginType(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , selection
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenPluginAssemblyWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportPluginAssembly(
                        iWriteToOutput
                        , service
                        , commonConfig
                        , selection
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowExportOrganization(
                        iWriteToOutput
                        , service
                        , commonConfig
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenSolutionImageWindow(
            IWriteToOutput iWriteToOutput
            , ConnectionData connectionData
            , CommonConfiguration commonConfig
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowSolutionImage(
                        iWriteToOutput
                        , commonConfig
                        , connectionData
                        , null
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationDifferenceImageWindow(
            IWriteToOutput iWriteToOutput
            , ConnectionData connectionData
            , CommonConfiguration commonConfig
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationDifferenceImage(
                        iWriteToOutput
                        , commonConfig
                        , connectionData
                        , null
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerWindow(
            IWriteToOutput iWriteToOutput
            , ConnectionConfiguration crmConfig
            , CommonConfiguration commonConfig
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparer(
                        iWriteToOutput
                        , crmConfig
                        , commonConfig
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerEntityMetadataWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string entityFilter = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerEntityMetadata(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , entityFilter
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerApplicationRibbonWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerApplicationRibbon(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerGlobalOptionSetsWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter = null
            )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerGlobalOptionSets(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , filter
                        );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerSystemFormWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity = null
            , string filter = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerSystemForm(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , filterEntity
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerSavedQueryWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity = null
            , string filter = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerSavedQuery(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , filterEntity
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerSavedQueryVisualizationWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity = null
            , string filter = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerSavedQueryVisualization(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , filterEntity
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerWorkflowWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity = null
            , string filter = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerWorkflow(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , filterEntity
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerReportWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerReport(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerSiteMapWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerSiteMap(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerWebResourcesWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerWebResources(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static void OpenOrganizationComparerPluginAssemblyWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter = null
        )
        {
            System.Threading.Thread worker = new System.Threading.Thread(() =>
            {
                try
                {
                    var form = new WindowOrganizationComparerPluginAssembly(
                        iWriteToOutput
                        , commonConfig
                        , connection1
                        , connection2
                        , filter
                    );

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            });

            worker.SetApartmentState(System.Threading.ApartmentState.STA);

            worker.Start();
        }

        public static bool IsDefinedExplorer(ComponentType componentType)
        {
            switch (componentType)
            {
                case ComponentType.Entity:
                case ComponentType.Attribute:
                case ComponentType.OptionSet:
                case ComponentType.EntityRelationship:
                case ComponentType.EntityKey:
                case ComponentType.Role:
                case ComponentType.Organization:
                case ComponentType.SavedQuery:
                case ComponentType.SavedQueryVisualization:
                case ComponentType.SystemForm:
                case ComponentType.Workflow:
                case ComponentType.Report:
                case ComponentType.WebResource:
                case ComponentType.SiteMap:
                case ComponentType.PluginType:
                case ComponentType.PluginAssembly:
                case ComponentType.SdkMessageProcessingStep:
                case ComponentType.SdkMessageProcessingStepImage:
                case ComponentType.SdkMessage:
                case ComponentType.SdkMessageFilter:
                case ComponentType.SdkMessagePair:
                case ComponentType.SdkMessageRequest:
                case ComponentType.SdkMessageRequestField:
                case ComponentType.SdkMessageResponse:
                case ComponentType.SdkMessageResponseField:
                case ComponentType.RibbonCustomization:
                    return true;
            }

            return false;
        }

        public static void OpenComponentExplorer(
            ComponentType componentType
            , IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string componentName
            , string parameter
        )
        {
            switch (componentType)
            {
                case ComponentType.Entity:
                    OpenEntityMetadataWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.Attribute:
                    OpenEntityAttributeExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.OptionSet:
                    OpenGlobalOptionSetsWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.EntityRelationship:
                    if (string.Equals(parameter, typeof(OneToManyRelationshipMetadata).Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        OpenEntityRelationshipOneToManyExplorer(iWriteToOutput, service, commonConfig, componentName);
                    }
                    else if (string.Equals(parameter, typeof(ManyToManyRelationshipMetadata).Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        OpenEntityRelationshipManyToManyExplorer(iWriteToOutput, service, commonConfig, componentName);
                    }
                    break;

                case ComponentType.EntityKey:
                    OpenEntityKeyExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.Role:
                    OpenRolesExplorer(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.Organization:
                    OpenOrganizationExplorer(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SavedQuery:
                    OpenSavedQueryWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SavedQueryVisualization:
                    OpenSavedQueryVisualizationWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SystemForm:
                    OpenSystemFormWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.Workflow:
                    OpenWorkflowWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.Report:
                    OpenExportReportWindow(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.WebResource:
                    OpenExportWebResourcesWindow(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.SiteMap:
                    OpenExportSiteMapWindow(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.PluginType:
                    OpenPluginTypeWindow(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.PluginAssembly:
                    OpenPluginAssemblyWindow(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.SdkMessageProcessingStep:
                    OpenPluginTreeWindow(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SdkMessageProcessingStepImage:
                    OpenPluginTreeWindow(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SdkMessage:
                    OpenSdkMessageTreeWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SdkMessageFilter:
                    OpenSdkMessageTreeWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SdkMessagePair:
                    OpenSdkMessageRequestTreeWindow(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SdkMessageRequest:
                    OpenSdkMessageRequestTreeWindow(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SdkMessageRequestField:
                    OpenSdkMessageRequestTreeWindow(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SdkMessageResponse:
                    OpenSdkMessageRequestTreeWindow(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SdkMessageResponseField:
                    OpenSdkMessageRequestTreeWindow(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.RibbonCustomization:
                    OpenApplicationRibbonWindow(iWriteToOutput, service, commonConfig);
                    break;
            }
        }
    }
}