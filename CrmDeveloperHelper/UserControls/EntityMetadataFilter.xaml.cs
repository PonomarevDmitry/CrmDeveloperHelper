using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for EntityMetadataFilter.xaml
    /// </summary>
    public partial class EntityMetadataFilter : UserControl
    {
        public static TupleList<string, Func<EntityMetadata, bool>> _filters = new TupleList<string, Func<EntityMetadata, bool>>()
        {
              { nameof(EntityMetadata.IsQuickCreateEnabled)                 , e => e.IsQuickCreateEnabled.HasValue                && e.IsQuickCreateEnabled.Value                  }
            , { nameof(EntityMetadata.IsReadingPaneEnabled)                 , e => e.IsReadingPaneEnabled.HasValue                && e.IsReadingPaneEnabled.Value                  }
            , { nameof(EntityMetadata.IsValidForAdvancedFind)               , e => e.IsValidForAdvancedFind.HasValue              && e.IsValidForAdvancedFind.Value                }
            , { nameof(EntityMetadata.IsEnabledForTrace)                    , e => e.IsEnabledForTrace.HasValue                   && e.IsEnabledForTrace.Value                     }
            , { nameof(EntityMetadata.IsEnabledForCharts)                   , e => e.IsEnabledForCharts.HasValue                  && e.IsEnabledForCharts.Value                    }
            , { nameof(EntityMetadata.IsManaged)                            , e => e.IsManaged.HasValue                           && e.IsManaged.Value                             }
            , { nameof(EntityMetadata.IsIntersect)                          , e => e.IsIntersect.HasValue                         && e.IsIntersect.Value                           }
            , { nameof(EntityMetadata.IsImportable)                         , e => e.IsImportable.HasValue                        && e.IsImportable.Value                          }
            , { nameof(EntityMetadata.ChangeTrackingEnabled)                , e => e.ChangeTrackingEnabled.HasValue               && e.ChangeTrackingEnabled.Value                 }
            , { nameof(EntityMetadata.IsOptimisticConcurrencyEnabled)       , e => e.IsOptimisticConcurrencyEnabled.HasValue      && e.IsOptimisticConcurrencyEnabled.Value        }
            , { nameof(EntityMetadata.HasActivities)                        , e => e.HasActivities.HasValue                       && e.HasActivities.Value                         }
            , { nameof(EntityMetadata.HasNotes)                             , e => e.HasNotes.HasValue                            && e.HasNotes.Value                              }
            , { nameof(EntityMetadata.IsLogicalEntity)                      , e => e.IsLogicalEntity.HasValue                     && e.IsLogicalEntity.Value                       }
            , { nameof(EntityMetadata.UsesBusinessDataLabelTable)           , e => e.UsesBusinessDataLabelTable.HasValue          && e.UsesBusinessDataLabelTable.Value            }
            , { nameof(EntityMetadata.IsPrivate)                            , e => e.IsPrivate.HasValue                           && e.IsPrivate.Value                             }
            , { nameof(EntityMetadata.IsEnabledForExternalChannels)         , e => e.IsEnabledForExternalChannels.HasValue        && e.IsEnabledForExternalChannels.Value          }
            , { nameof(EntityMetadata.EnforceStateTransitions)              , e => e.EnforceStateTransitions.HasValue             && e.EnforceStateTransitions.Value               }
            , { nameof(EntityMetadata.IsStateModelAware)                    , e => e.IsStateModelAware.HasValue                   && e.IsStateModelAware.Value                     }
            , { nameof(EntityMetadata.SyncToExternalSearchIndex)            , e => e.SyncToExternalSearchIndex.HasValue           && e.SyncToExternalSearchIndex.Value             }
            , { nameof(EntityMetadata.IsActivity)                           , e => e.IsActivity.HasValue                          && e.IsActivity.Value                            }
            , { nameof(EntityMetadata.AutoCreateAccessTeams)                , e => e.AutoCreateAccessTeams.HasValue               && e.AutoCreateAccessTeams.Value                 }
            //, { nameof(EntityMetadata.IsMSTeamsIntegrationEnabled)          , e => e.IsMSTeamsIntegrationEnabled.HasValue         && e.IsMSTeamsIntegrationEnabled.Value           }
            , { nameof(EntityMetadata.IsDocumentRecommendationsEnabled)     , e => e.IsDocumentRecommendationsEnabled.HasValue    && e.IsDocumentRecommendationsEnabled.Value      }
            , { nameof(EntityMetadata.IsBPFEntity)                          , e => e.IsBPFEntity.HasValue                         && e.IsBPFEntity.Value                           }
            , { nameof(EntityMetadata.IsSLAEnabled)                         , e => e.IsSLAEnabled.HasValue                        && e.IsSLAEnabled.Value                          }
            , { nameof(EntityMetadata.IsKnowledgeManagementEnabled)         , e => e.IsKnowledgeManagementEnabled.HasValue        && e.IsKnowledgeManagementEnabled.Value          }
            , { nameof(EntityMetadata.IsInteractionCentricEnabled)          , e => e.IsInteractionCentricEnabled.HasValue         && e.IsInteractionCentricEnabled.Value           }
            , { nameof(EntityMetadata.IsOneNoteIntegrationEnabled)          , e => e.IsOneNoteIntegrationEnabled.HasValue         && e.IsOneNoteIntegrationEnabled.Value           }
            , { nameof(EntityMetadata.IsDocumentManagementEnabled)          , e => e.IsDocumentManagementEnabled.HasValue         && e.IsDocumentManagementEnabled.Value           }
            , { nameof(EntityMetadata.EntityHelpUrlEnabled)                 , e => e.EntityHelpUrlEnabled.HasValue                && e.EntityHelpUrlEnabled.Value                  }
            , { nameof(EntityMetadata.CanTriggerWorkflow)                   , e => e.CanTriggerWorkflow.HasValue                  && e.CanTriggerWorkflow.Value                    }
            , { nameof(EntityMetadata.AutoRouteToOwnerQueue)                , e => e.AutoRouteToOwnerQueue.HasValue               && e.AutoRouteToOwnerQueue.Value                 }
            , { nameof(EntityMetadata.IsActivityParty)                      , e => e.IsActivityParty.HasValue                     && e.IsActivityParty.Value                       }
            , { nameof(EntityMetadata.IsAvailableOffline)                   , e => e.IsAvailableOffline.HasValue                  && e.IsAvailableOffline.Value                    }
            , { nameof(EntityMetadata.IsChildEntity)                        , e => e.IsChildEntity.HasValue                       && e.IsChildEntity.Value                         }
            , { nameof(EntityMetadata.HasFeedback)                          , e => e.HasFeedback.HasValue                         && e.HasFeedback.Value                           }
            , { nameof(EntityMetadata.IsBusinessProcessEnabled)             , e => e.IsBusinessProcessEnabled.HasValue            && e.IsBusinessProcessEnabled.Value              }
            , { nameof(EntityMetadata.IsCustomEntity)                       , e => e.IsCustomEntity.HasValue                      && e.IsCustomEntity.Value                        }
            , { nameof(EntityMetadata.IsAIRUpdated)                         , e => e.IsAIRUpdated.HasValue                        && e.IsAIRUpdated.Value                          }
            //, { nameof(EntityMetadata.IsSolutionAware)                      , e => e.IsSolutionAware.HasValue                     && e.IsSolutionAware.Value                       }

            , { nameof(EntityMetadata.IsOfflineInMobileClient)              , e => e.IsOfflineInMobileClient != null              && e.IsOfflineInMobileClient.Value               }
            , { nameof(EntityMetadata.IsReadOnlyInMobileClient)             , e => e.IsReadOnlyInMobileClient != null             && e.IsReadOnlyInMobileClient.Value              }
            , { nameof(EntityMetadata.IsVisibleInMobileClient)              , e => e.IsVisibleInMobileClient != null              && e.IsVisibleInMobileClient.Value               }
            , { nameof(EntityMetadata.IsVisibleInMobile)                    , e => e.IsVisibleInMobile != null                    && e.IsVisibleInMobile.Value                     }
            , { nameof(EntityMetadata.IsMailMergeEnabled)                   , e => e.IsMailMergeEnabled != null                   && e.IsMailMergeEnabled.Value                    }
            , { nameof(EntityMetadata.CanChangeTrackingBeEnabled)           , e => e.CanChangeTrackingBeEnabled != null           && e.CanChangeTrackingBeEnabled.Value            }
            , { nameof(EntityMetadata.CanChangeHierarchicalRelationship)    , e => e.CanChangeHierarchicalRelationship != null    && e.CanChangeHierarchicalRelationship.Value     }
            , { nameof(EntityMetadata.CanModifyAdditionalSettings)          , e => e.CanModifyAdditionalSettings != null          && e.CanModifyAdditionalSettings.Value           }
            , { nameof(EntityMetadata.IsAuditEnabled)                       , e => e.IsAuditEnabled != null                       && e.IsAuditEnabled.Value                        }
            , { nameof(EntityMetadata.CanEnableSyncToExternalSearchIndex)   , e => e.CanEnableSyncToExternalSearchIndex != null   && e.CanEnableSyncToExternalSearchIndex.Value    }
            , { nameof(EntityMetadata.CanBeInCustomEntityAssociation)       , e => e.CanBeInCustomEntityAssociation != null       && e.CanBeInCustomEntityAssociation.Value        }
            , { nameof(EntityMetadata.CanBeInManyToMany)                    , e => e.CanBeInManyToMany != null                    && e.CanBeInManyToMany.Value                     }
            , { nameof(EntityMetadata.CanBePrimaryEntityInRelationship)     , e => e.CanBePrimaryEntityInRelationship != null     && e.CanBePrimaryEntityInRelationship.Value      }
            , { nameof(EntityMetadata.CanBeRelatedEntityInRelationship)     , e => e.CanBeRelatedEntityInRelationship != null     && e.CanBeRelatedEntityInRelationship.Value      }
            , { nameof(EntityMetadata.CanCreateCharts)                      , e => e.CanCreateCharts != null                      && e.CanCreateCharts.Value                       }
            , { nameof(EntityMetadata.CanCreateViews)                       , e => e.CanCreateViews != null                       && e.CanCreateViews.Value                        }
            , { nameof(EntityMetadata.CanCreateForms)                       , e => e.CanCreateForms != null                       && e.CanCreateForms.Value                        }
            , { nameof(EntityMetadata.CanCreateAttributes)                  , e => e.CanCreateAttributes != null                  && e.CanCreateAttributes.Value                   }
            , { nameof(EntityMetadata.IsDuplicateDetectionEnabled)          , e => e.IsDuplicateDetectionEnabled != null          && e.IsDuplicateDetectionEnabled.Value           }
            , { nameof(EntityMetadata.IsMappable)                           , e => e.IsMappable != null                           && e.IsMappable.Value                            }
            , { nameof(EntityMetadata.IsCustomizable)                       , e => e.IsCustomizable != null                       && e.IsCustomizable.Value                        }
            , { nameof(EntityMetadata.IsConnectionsEnabled)                 , e => e.IsConnectionsEnabled != null                 && e.IsConnectionsEnabled.Value                  }
            , { nameof(EntityMetadata.IsValidForQueue)                      , e => e.IsValidForQueue != null                      && e.IsValidForQueue.Value                       }
            , { nameof(EntityMetadata.IsRenameable)                         , e => e.IsRenameable != null                         && e.IsRenameable.Value                          }
        };

        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();

        public bool FilterChanged { get; private set; } = false;

        private const int _countInColumn = 20;

        public EntityMetadataFilter()
        {
            InitializeComponent();

            FillRoleEditorLayoutTabs();

            panelFilters.Children.Clear();

            StackPanel panel = null;

            foreach (var item in _filters.OrderBy(i => i.Item1))
            {
                var checkBox = new CheckBox()
                {
                    Name = item.Item1,
                    Content = item.Item1,
                    Tag = item.Item2,

                    IsThreeState = true,

                    IsChecked = null,
                };

                checkBox.Checked += this.CheckBox_Checked;
                checkBox.Indeterminate += this.CheckBox_Checked;
                checkBox.Unchecked += this.CheckBox_Checked;

                if (_checkBoxes.Count % _countInColumn == 0)
                {
                    panel = new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                    };

                    panelFilters.Children.Add(panel);
                }

                _checkBoxes.Add(checkBox);

                panel.Children.Add(checkBox);
            }

            this.FilterChanged = false;
        }

        private void FillRoleEditorLayoutTabs()
        {
            var tabs = RoleEditorLayoutTab.GetTabs();

            cmBRoleEditorLayoutTabs.Items.Clear();

            cmBRoleEditorLayoutTabs.Items.Add("All");

            foreach (var tab in tabs)
            {
                cmBRoleEditorLayoutTabs.Items.Add(tab);
            }

            cmBRoleEditorLayoutTabs.SelectedIndex = 0;
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.FilterChanged = true;
        }

        private void cmBRoleEditorLayoutTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.FilterChanged = true;
        }

        public event EventHandler<EventArgs> CloseClicked;

        private void OnCloseClicked()
        {
            CloseClicked?.Invoke(this, new EventArgs());
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    e.Handled = true;

                    OnCloseClicked();

                    return;
                }
            }

            base.OnKeyDown(e);
        }

        public IEnumerable<EntityMetadata> FilterList(IEnumerable<EntityMetadata> list)
        {
            return FilterList<EntityMetadata>(list, e => e);
        }

        public IEnumerable<T> FilterList<T>(IEnumerable<T> list, Func<T, EntityMetadata> selector)
        {
            this.FilterChanged = false;

            RoleEditorLayoutTab selectedTab = null;

            this.Dispatcher.Invoke(() =>
            {
                selectedTab = cmBRoleEditorLayoutTabs.SelectedItem as RoleEditorLayoutTab;
            });

            var funcs = GetFilters();

            var result = list;

            result = result.Where(e => funcs.All(f => f.Item1(selector(e)) == f.Item2));

            if (selectedTab != null)
            {
                result = result.Where(ent => selector(ent) != null && selector(ent).ObjectTypeCode.HasValue && selectedTab.EntitiesHash.Contains(selector(ent).ObjectTypeCode.Value));
            }

            return result;
        }

        private TupleList<Func<EntityMetadata, bool>, bool> GetFilters()
        {
            var funcs = new TupleList<Func<EntityMetadata, bool>, bool>();

            this.Dispatcher.Invoke(() =>
            {
                foreach (var checkbox in _checkBoxes.Where(c => c.IsChecked.HasValue))
                {
                    if (checkbox.Tag is Func<EntityMetadata, bool> func)
                    {
                        funcs.Add(func, checkbox.IsChecked.Value);
                    }
                }
            });

            return funcs;
        }

        private void hypLinkClear_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            foreach (var item in _checkBoxes)
            {
                item.IsChecked = null;
            }
        }
    }
}