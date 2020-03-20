using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class EntityMetadataPropertiesViewItem : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly string[] _names =
        {
            nameof(IsChanged)

            , nameof(IsAuditEnabled)
            , nameof(IsCustomizable)
            , nameof(IsRenameable)
            , nameof(CanModifyAdditionalSettings)
            , nameof(IsValidForQueue)
            , nameof(IsConnectionsEnabled)
            , nameof(IsMappable)
            , nameof(IsDuplicateDetectionEnabled)
            , nameof(CanCreateAttributes)
            , nameof(CanCreateForms)
            , nameof(CanCreateViews)
            , nameof(CanCreateCharts)
            , nameof(CanBeRelatedEntityInRelationship)
            , nameof(CanBePrimaryEntityInRelationship)
            , nameof(CanBeInManyToMany)
            , nameof(CanBeInCustomEntityAssociation)
            , nameof(CanEnableSyncToExternalSearchIndex)
            , nameof(CanChangeHierarchicalRelationship)
            , nameof(CanChangeTrackingBeEnabled)
            , nameof(IsMailMergeEnabled)
            , nameof(IsVisibleInMobile)
            , nameof(IsVisibleInMobileClient)
            , nameof(IsReadOnlyInMobileClient)
            , nameof(IsOfflineInMobileClient)


            , nameof(IsQuickCreateEnabled)
            , nameof(IsReadingPaneEnabled)
            , nameof(ChangeTrackingEnabled)
            , nameof(HasActivities)
            , nameof(HasNotes)
            , nameof(UsesBusinessDataLabelTable)
            , nameof(IsEnabledForExternalChannels)
            , nameof(SyncToExternalSearchIndex)
            , nameof(IsActivity)
            , nameof(AutoCreateAccessTeams)
            , nameof(IsMSTeamsIntegrationEnabled)
            , nameof(IsDocumentRecommendationsEnabled)
            , nameof(IsBPFEntity)
            , nameof(IsSLAEnabled)
            , nameof(IsKnowledgeManagementEnabled)
            , nameof(IsInteractionCentricEnabled)
            , nameof(IsOneNoteIntegrationEnabled)
            , nameof(IsDocumentManagementEnabled)
            , nameof(EntityHelpUrlEnabled)
            , nameof(AutoRouteToOwnerQueue)
            , nameof(IsActivityParty)
            , nameof(IsAvailableOffline)
            , nameof(HasFeedback)
            , nameof(IsBusinessProcessEnabled)
            , nameof(IsSolutionAware)
        };

        private EntityMetadata _EntityMetadata;
        public EntityMetadata EntityMetadata
        {
            get => _EntityMetadata;
            private set
            {
                _EntityMetadata = value;

                this._initialIsAuditEnabled = EntityMetadata.IsAuditEnabled?.Value;
                this._initialIsCustomizable = EntityMetadata.IsCustomizable?.Value;
                this._initialIsRenameable = EntityMetadata.IsRenameable?.Value;
                this._initialCanModifyAdditionalSettings = EntityMetadata.CanModifyAdditionalSettings?.Value;
                this._initialIsValidForQueue = EntityMetadata.IsValidForQueue?.Value;
                this._initialIsConnectionsEnabled = EntityMetadata.IsConnectionsEnabled?.Value;
                this._initialIsMappable = EntityMetadata.IsMappable?.Value;
                this._initialIsDuplicateDetectionEnabled = EntityMetadata.IsDuplicateDetectionEnabled?.Value;
                this._initialCanCreateAttributes = EntityMetadata.CanCreateAttributes?.Value;
                this._initialCanCreateForms = EntityMetadata.CanCreateForms?.Value;
                this._initialCanCreateViews = EntityMetadata.CanCreateViews?.Value;
                this._initialCanCreateCharts = EntityMetadata.CanCreateCharts?.Value;
                this._initialCanBeRelatedEntityInRelationship = EntityMetadata.CanBeRelatedEntityInRelationship?.Value;
                this._initialCanBePrimaryEntityInRelationship = EntityMetadata.CanBePrimaryEntityInRelationship?.Value;
                this._initialCanBeInManyToMany = EntityMetadata.CanBeInManyToMany?.Value;
                this._initialCanBeInCustomEntityAssociation = EntityMetadata.CanBeInCustomEntityAssociation?.Value;
                this._initialCanEnableSyncToExternalSearchIndex = EntityMetadata.CanEnableSyncToExternalSearchIndex?.Value;
                this._initialCanChangeHierarchicalRelationship = EntityMetadata.CanChangeHierarchicalRelationship?.Value;
                this._initialCanChangeTrackingBeEnabled = EntityMetadata.CanChangeTrackingBeEnabled?.Value;
                this._initialIsMailMergeEnabled = EntityMetadata.IsMailMergeEnabled?.Value;
                this._initialIsVisibleInMobile = EntityMetadata.IsVisibleInMobile?.Value;
                this._initialIsVisibleInMobileClient = EntityMetadata.IsVisibleInMobileClient?.Value;
                this._initialIsReadOnlyInMobileClient = EntityMetadata.IsReadOnlyInMobileClient?.Value;
                this._initialIsOfflineInMobileClient = EntityMetadata.IsOfflineInMobileClient?.Value;

                //this._initial = EntityMetadata.IsAuditEnabled?.Value;




                this._initialIsQuickCreateEnabled = EntityMetadata.IsQuickCreateEnabled.GetValueOrDefault();
                this._initialIsReadingPaneEnabled = EntityMetadata.IsReadingPaneEnabled.GetValueOrDefault();
                this._initialChangeTrackingEnabled = EntityMetadata.ChangeTrackingEnabled.GetValueOrDefault();
                this._initialHasActivities = EntityMetadata.HasActivities.GetValueOrDefault();
                this._initialHasNotes = EntityMetadata.HasNotes.GetValueOrDefault();
                this._initialUsesBusinessDataLabelTable = EntityMetadata.UsesBusinessDataLabelTable.GetValueOrDefault();
                this._initialIsEnabledForExternalChannels = EntityMetadata.IsEnabledForExternalChannels.GetValueOrDefault();
                this._initialSyncToExternalSearchIndex = EntityMetadata.SyncToExternalSearchIndex.GetValueOrDefault();
                this._initialIsActivity = EntityMetadata.IsActivity.GetValueOrDefault();
                this._initialAutoCreateAccessTeams = EntityMetadata.AutoCreateAccessTeams.GetValueOrDefault();
                this._initialIsMSTeamsIntegrationEnabled = EntityMetadata.IsMSTeamsIntegrationEnabled.GetValueOrDefault();
                this._initialIsDocumentRecommendationsEnabled = EntityMetadata.IsDocumentRecommendationsEnabled.GetValueOrDefault();
                this._initialIsBPFEntity = EntityMetadata.IsBPFEntity.GetValueOrDefault();
                this._initialIsSLAEnabled = EntityMetadata.IsSLAEnabled.GetValueOrDefault();
                this._initialIsKnowledgeManagementEnabled = EntityMetadata.IsKnowledgeManagementEnabled.GetValueOrDefault();
                this._initialIsInteractionCentricEnabled = EntityMetadata.IsInteractionCentricEnabled.GetValueOrDefault();
                this._initialIsOneNoteIntegrationEnabled = EntityMetadata.IsOneNoteIntegrationEnabled.GetValueOrDefault();
                this._initialIsDocumentManagementEnabled = EntityMetadata.IsDocumentManagementEnabled.GetValueOrDefault();
                this._initialEntityHelpUrlEnabled = EntityMetadata.EntityHelpUrlEnabled.GetValueOrDefault();
                this._initialAutoRouteToOwnerQueue = EntityMetadata.AutoRouteToOwnerQueue.GetValueOrDefault();
                this._initialIsActivityParty = EntityMetadata.IsActivityParty.GetValueOrDefault();
                this._initialIsAvailableOffline = EntityMetadata.IsAvailableOffline.GetValueOrDefault();
                this._initialHasFeedback = EntityMetadata.HasFeedback.GetValueOrDefault();
                this._initialIsBusinessProcessEnabled = EntityMetadata.IsBusinessProcessEnabled.GetValueOrDefault();
                this._initialIsSolutionAware = EntityMetadata.IsSolutionAware.GetValueOrDefault();


                //this._initialIsValidForGrid = EntityMetadata.IsValidForGrid.GetValueOrDefault();

                foreach (var name in _names)
                {
                    OnPropertyChanged(name);
                }
            }
        }

        public string LogicalName => EntityMetadata.LogicalName;

        public string DisplayName { get; private set; }

        public EntityMetadataPropertiesViewItem(EntityMetadata entityMetadata)
        {
            LoadMetadata(entityMetadata);
        }

        public void LoadMetadata(EntityMetadata entityMetadata)
        {
            this.DisplayName = CreateFileHandler.GetLocalizedLabel(entityMetadata.DisplayName);

            this.EntityMetadata = entityMetadata;

            this.OnPropertyChanging(nameof(IsChanged));
            this.IsChanged = false;
            this.OnPropertyChanged(nameof(IsChanged));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!string.Equals(propertyName, nameof(IsChanged)))
            {
                var val = CalculateIsChanged();

                if (val != this.IsChanged)
                {
                    this.OnPropertyChanging(nameof(IsChanged));
                    this.IsChanged = val;
                    this.OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        private bool CalculateIsChanged()
        {
            #region bool? Properties

            if (_initialIsReadingPaneEnabled != EntityMetadata.IsReadingPaneEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsQuickCreateEnabled != EntityMetadata.IsQuickCreateEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialChangeTrackingEnabled != EntityMetadata.ChangeTrackingEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialHasActivities != EntityMetadata.HasActivities.GetValueOrDefault())
            {
                return true;
            }

            if (_initialHasNotes != EntityMetadata.HasNotes.GetValueOrDefault())
            {
                return true;
            }

            if (_initialUsesBusinessDataLabelTable != EntityMetadata.UsesBusinessDataLabelTable.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsEnabledForExternalChannels != EntityMetadata.IsEnabledForExternalChannels.GetValueOrDefault())
            {
                return true;
            }

            if (_initialSyncToExternalSearchIndex != EntityMetadata.SyncToExternalSearchIndex.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsActivity != EntityMetadata.IsActivity.GetValueOrDefault())
            {
                return true;
            }

            if (_initialAutoCreateAccessTeams != EntityMetadata.AutoCreateAccessTeams.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsMSTeamsIntegrationEnabled != EntityMetadata.IsMSTeamsIntegrationEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsDocumentRecommendationsEnabled != EntityMetadata.IsDocumentRecommendationsEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsBPFEntity != EntityMetadata.IsBPFEntity.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsSLAEnabled != EntityMetadata.IsSLAEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsKnowledgeManagementEnabled != EntityMetadata.IsKnowledgeManagementEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsInteractionCentricEnabled != EntityMetadata.IsInteractionCentricEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsOneNoteIntegrationEnabled != EntityMetadata.IsOneNoteIntegrationEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsDocumentManagementEnabled != EntityMetadata.IsDocumentManagementEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialEntityHelpUrlEnabled != EntityMetadata.EntityHelpUrlEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialAutoRouteToOwnerQueue != EntityMetadata.AutoRouteToOwnerQueue.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsActivityParty != EntityMetadata.IsActivityParty.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsAvailableOffline != EntityMetadata.IsAvailableOffline.GetValueOrDefault())
            {
                return true;
            }

            if (_initialHasFeedback != EntityMetadata.HasFeedback.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsBusinessProcessEnabled != EntityMetadata.IsBusinessProcessEnabled.GetValueOrDefault())
            {
                return true;
            }

            if (_initialIsSolutionAware != EntityMetadata.IsSolutionAware.GetValueOrDefault())
            {
                return true;
            }

            //if (_initialHasNotes != EntityMetadata.HasNotes.GetValueOrDefault())
            //{
            //    return true;
            //}
            //if (_initialHasNotes != EntityMetadata.HasNotes.GetValueOrDefault())
            //{
            //    return true;
            //}

            #endregion bool? Properties

            #region ManagedProperties

            if (_initialIsAuditEnabled != EntityMetadata.IsAuditEnabled?.Value)
            {
                return true;
            }

            if (_initialIsCustomizable != EntityMetadata.IsCustomizable?.Value)
            {
                return true;
            }

            if (_initialIsRenameable != EntityMetadata.IsRenameable?.Value)
            {
                return true;
            }

            if (_initialCanModifyAdditionalSettings != EntityMetadata.CanModifyAdditionalSettings?.Value)
            {
                return true;
            }

            if (_initialIsValidForQueue != EntityMetadata.IsValidForQueue?.Value)
            {
                return true;
            }

            if (_initialIsConnectionsEnabled != EntityMetadata.IsConnectionsEnabled?.Value)
            {
                return true;
            }

            if (_initialIsMappable != EntityMetadata.IsMappable?.Value)
            {
                return true;
            }

            if (_initialIsDuplicateDetectionEnabled != EntityMetadata.IsDuplicateDetectionEnabled?.Value)
            {
                return true;
            }

            if (_initialCanCreateAttributes != EntityMetadata.CanCreateAttributes?.Value)
            {
                return true;
            }

            if (_initialCanCreateForms != EntityMetadata.CanCreateForms?.Value)
            {
                return true;
            }

            if (_initialCanCreateViews != EntityMetadata.CanCreateViews?.Value)
            {
                return true;
            }

            if (_initialCanCreateCharts != EntityMetadata.CanCreateCharts?.Value)
            {
                return true;
            }

            if (_initialCanBeRelatedEntityInRelationship != EntityMetadata.CanBeRelatedEntityInRelationship?.Value)
            {
                return true;
            }

            if (_initialCanBePrimaryEntityInRelationship != EntityMetadata.CanBePrimaryEntityInRelationship?.Value)
            {
                return true;
            }

            if (_initialCanBeInManyToMany != EntityMetadata.CanBeInManyToMany?.Value)
            {
                return true;
            }

            if (_initialCanBeInCustomEntityAssociation != EntityMetadata.CanBeInCustomEntityAssociation?.Value)
            {
                return true;
            }

            if (_initialCanEnableSyncToExternalSearchIndex != EntityMetadata.CanEnableSyncToExternalSearchIndex?.Value)
            {
                return true;
            }

            if (_initialCanChangeHierarchicalRelationship != EntityMetadata.CanChangeHierarchicalRelationship?.Value)
            {
                return true;
            }

            if (_initialCanChangeTrackingBeEnabled != EntityMetadata.CanChangeTrackingBeEnabled?.Value)
            {
                return true;
            }

            if (_initialIsMailMergeEnabled != EntityMetadata.IsMailMergeEnabled?.Value)
            {
                return true;
            }

            if (_initialIsVisibleInMobile != EntityMetadata.IsVisibleInMobile?.Value)
            {
                return true;
            }

            if (_initialIsVisibleInMobileClient != EntityMetadata.IsVisibleInMobileClient?.Value)
            {
                return true;
            }

            if (_initialIsReadOnlyInMobileClient != EntityMetadata.IsReadOnlyInMobileClient?.Value)
            {
                return true;
            }

            //if (_initial != EntityMetadata.IsGlobalFilterEnabled?.Value)
            //{
            //    return true;
            //}

            //if (_initial != EntityMetadata.IsGlobalFilterEnabled?.Value)
            //{
            //    return true;
            //}

            //if (_initial != EntityMetadata.IsGlobalFilterEnabled?.Value)
            //{
            //    return true;
            //}

            #endregion ManagedProperties

            return false;
        }

        private void OnPropertyChanging(string propertyName)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public bool IsChanged { get; private set; }

        #region bool? Properties

        private bool? _initialIsQuickCreateEnabled;

        public bool IsQuickCreateEnabled
        {
            get => EntityMetadata.IsQuickCreateEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsQuickCreateEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsQuickCreateEnabled));
                this.EntityMetadata.IsQuickCreateEnabled = value;
                this.OnPropertyChanged(nameof(IsQuickCreateEnabled));
            }
        }

        private bool? _initialIsReadingPaneEnabled;

        public bool IsReadingPaneEnabled
        {
            get => EntityMetadata.IsReadingPaneEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsReadingPaneEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsReadingPaneEnabled));
                this.EntityMetadata.IsReadingPaneEnabled = value;
                this.OnPropertyChanged(nameof(IsReadingPaneEnabled));
            }
        }

        private bool? _initialChangeTrackingEnabled;

        public bool ChangeTrackingEnabled
        {
            get => EntityMetadata.ChangeTrackingEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.ChangeTrackingEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ChangeTrackingEnabled));
                this.EntityMetadata.ChangeTrackingEnabled = value;
                this.OnPropertyChanged(nameof(ChangeTrackingEnabled));
            }
        }

        private bool? _initialHasActivities;

        public bool HasActivities
        {
            get => EntityMetadata.HasActivities.GetValueOrDefault();
            set
            {
                if (EntityMetadata.HasActivities == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(HasActivities));
                this.EntityMetadata.HasActivities = value;
                this.OnPropertyChanged(nameof(HasActivities));
            }
        }

        private bool? _initialHasNotes;

        public bool HasNotes
        {
            get => EntityMetadata.HasNotes.GetValueOrDefault();
            set
            {
                if (EntityMetadata.HasNotes == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(HasNotes));
                this.EntityMetadata.HasNotes = value;
                this.OnPropertyChanged(nameof(HasNotes));
            }
        }

        private bool? _initialUsesBusinessDataLabelTable;

        public bool UsesBusinessDataLabelTable
        {
            get => EntityMetadata.UsesBusinessDataLabelTable.GetValueOrDefault();
            set
            {
                if (EntityMetadata.UsesBusinessDataLabelTable == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(UsesBusinessDataLabelTable));
                this.EntityMetadata.UsesBusinessDataLabelTable = value;
                this.OnPropertyChanged(nameof(UsesBusinessDataLabelTable));
            }
        }

        private bool? _initialIsEnabledForExternalChannels;

        public bool IsEnabledForExternalChannels
        {
            get => EntityMetadata.IsEnabledForExternalChannels.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsEnabledForExternalChannels == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsEnabledForExternalChannels));
                this.EntityMetadata.IsEnabledForExternalChannels = value;
                this.OnPropertyChanged(nameof(IsEnabledForExternalChannels));
            }
        }

        private bool? _initialSyncToExternalSearchIndex;

        public bool SyncToExternalSearchIndex
        {
            get => EntityMetadata.SyncToExternalSearchIndex.GetValueOrDefault();
            set
            {
                if (EntityMetadata.SyncToExternalSearchIndex == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(SyncToExternalSearchIndex));
                this.EntityMetadata.SyncToExternalSearchIndex = value;
                this.OnPropertyChanged(nameof(SyncToExternalSearchIndex));
            }
        }

        private bool? _initialIsActivity;

        public bool IsActivity
        {
            get => EntityMetadata.IsActivity.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsActivity == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsActivity));
                this.EntityMetadata.IsActivity = value;
                this.OnPropertyChanged(nameof(IsActivity));
            }
        }

        private bool? _initialAutoCreateAccessTeams;

        public bool AutoCreateAccessTeams
        {
            get => EntityMetadata.AutoCreateAccessTeams.GetValueOrDefault();
            set
            {
                if (EntityMetadata.AutoCreateAccessTeams == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AutoCreateAccessTeams));
                this.EntityMetadata.AutoCreateAccessTeams = value;
                this.OnPropertyChanged(nameof(AutoCreateAccessTeams));
            }
        }

        private bool? _initialIsMSTeamsIntegrationEnabled;

        public bool IsMSTeamsIntegrationEnabled
        {
            get => EntityMetadata.IsMSTeamsIntegrationEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsMSTeamsIntegrationEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsMSTeamsIntegrationEnabled));
                this.EntityMetadata.IsMSTeamsIntegrationEnabled = value;
                this.OnPropertyChanged(nameof(IsMSTeamsIntegrationEnabled));
            }
        }

        private bool? _initialIsDocumentRecommendationsEnabled;

        public bool IsDocumentRecommendationsEnabled
        {
            get => EntityMetadata.IsDocumentRecommendationsEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsDocumentRecommendationsEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsDocumentRecommendationsEnabled));
                this.EntityMetadata.IsDocumentRecommendationsEnabled = value;
                this.OnPropertyChanged(nameof(IsDocumentRecommendationsEnabled));
            }
        }

        private bool? _initialIsBPFEntity;

        public bool IsBPFEntity
        {
            get => EntityMetadata.IsBPFEntity.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsBPFEntity == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsBPFEntity));
                this.EntityMetadata.IsBPFEntity = value;
                this.OnPropertyChanged(nameof(IsBPFEntity));
            }
        }

        private bool? _initialIsSLAEnabled;

        public bool IsSLAEnabled
        {
            get => EntityMetadata.IsSLAEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsSLAEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsSLAEnabled));
                this.EntityMetadata.IsSLAEnabled = value;
                this.OnPropertyChanged(nameof(IsSLAEnabled));
            }
        }

        private bool? _initialIsKnowledgeManagementEnabled;

        public bool IsKnowledgeManagementEnabled
        {
            get => EntityMetadata.IsKnowledgeManagementEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsKnowledgeManagementEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsKnowledgeManagementEnabled));
                this.EntityMetadata.IsKnowledgeManagementEnabled = value;
                this.OnPropertyChanged(nameof(IsKnowledgeManagementEnabled));
            }
        }

        private bool? _initialIsInteractionCentricEnabled;

        public bool IsInteractionCentricEnabled
        {
            get => EntityMetadata.IsInteractionCentricEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsInteractionCentricEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsInteractionCentricEnabled));
                this.EntityMetadata.IsInteractionCentricEnabled = value;
                this.OnPropertyChanged(nameof(IsInteractionCentricEnabled));
            }
        }

        private bool? _initialIsOneNoteIntegrationEnabled;

        public bool IsOneNoteIntegrationEnabled
        {
            get => EntityMetadata.IsOneNoteIntegrationEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsOneNoteIntegrationEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsOneNoteIntegrationEnabled));
                this.EntityMetadata.IsOneNoteIntegrationEnabled = value;
                this.OnPropertyChanged(nameof(IsOneNoteIntegrationEnabled));
            }
        }

        private bool? _initialIsDocumentManagementEnabled;

        public bool IsDocumentManagementEnabled
        {
            get => EntityMetadata.IsDocumentManagementEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsDocumentManagementEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsDocumentManagementEnabled));
                this.EntityMetadata.IsDocumentManagementEnabled = value;
                this.OnPropertyChanged(nameof(IsDocumentManagementEnabled));
            }
        }

        private bool? _initialEntityHelpUrlEnabled;

        public bool EntityHelpUrlEnabled
        {
            get => EntityMetadata.EntityHelpUrlEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.EntityHelpUrlEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(EntityHelpUrlEnabled));
                this.EntityMetadata.EntityHelpUrlEnabled = value;
                this.OnPropertyChanged(nameof(EntityHelpUrlEnabled));
            }
        }

        private bool? _initialAutoRouteToOwnerQueue;

        public bool AutoRouteToOwnerQueue
        {
            get => EntityMetadata.AutoRouteToOwnerQueue.GetValueOrDefault();
            set
            {
                if (EntityMetadata.AutoRouteToOwnerQueue == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AutoRouteToOwnerQueue));
                this.EntityMetadata.AutoRouteToOwnerQueue = value;
                this.OnPropertyChanged(nameof(AutoRouteToOwnerQueue));
            }
        }

        private bool? _initialIsActivityParty;

        public bool IsActivityParty
        {
            get => EntityMetadata.IsActivityParty.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsActivityParty == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsActivityParty));
                this.EntityMetadata.IsActivityParty = value;
                this.OnPropertyChanged(nameof(IsActivityParty));
            }
        }

        private bool? _initialIsAvailableOffline;

        public bool IsAvailableOffline
        {
            get => EntityMetadata.IsAvailableOffline.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsAvailableOffline == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsAvailableOffline));
                this.EntityMetadata.IsAvailableOffline = value;
                this.OnPropertyChanged(nameof(IsAvailableOffline));
            }
        }

        private bool? _initialHasFeedback;

        public bool HasFeedback
        {
            get => EntityMetadata.HasFeedback.GetValueOrDefault();
            set
            {
                if (EntityMetadata.HasFeedback == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(HasFeedback));
                this.EntityMetadata.HasFeedback = value;
                this.OnPropertyChanged(nameof(HasFeedback));
            }
        }

        private bool? _initialIsBusinessProcessEnabled;

        public bool IsBusinessProcessEnabled
        {
            get => EntityMetadata.IsBusinessProcessEnabled.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsBusinessProcessEnabled == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsBusinessProcessEnabled));
                this.EntityMetadata.IsBusinessProcessEnabled = value;
                this.OnPropertyChanged(nameof(IsBusinessProcessEnabled));
            }
        }

        private bool? _initialIsSolutionAware;

        public bool IsSolutionAware
        {
            get => EntityMetadata.IsSolutionAware.GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsSolutionAware == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsSolutionAware));
                this.EntityMetadata.IsSolutionAware = value;
                this.OnPropertyChanged(nameof(IsSolutionAware));
            }
        }

        //private bool? _initialIsSecured;

        //public bool IsSecured
        //{
        //    get => EntityMetadata.IsSecured.GetValueOrDefault();
        //    set
        //    {
        //        if (EntityMetadata.IsSecured == value)
        //        {
        //            return;
        //        }

        //        this.OnPropertyChanging(nameof(IsSecured));
        //        this.EntityMetadata.IsSecured = value;
        //        this.OnPropertyChanged(nameof(IsSecured));
        //    }
        //}

        #endregion bool? Properties

        #region ManagedProperties

        private bool? _initialIsAuditEnabled;

        public bool IsAuditEnabledCanBeChanged => (EntityMetadata.IsAuditEnabled?.CanBeChanged).GetValueOrDefault();

        public bool IsAuditEnabled
        {
            get => (EntityMetadata.IsAuditEnabled?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsAuditEnabled == null || !EntityMetadata.IsAuditEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsAuditEnabled));
                this.EntityMetadata.IsAuditEnabled.Value = value;
                this.OnPropertyChanged(nameof(IsAuditEnabled));
            }
        }

        private bool? _initialCanModifyAdditionalSettings;

        public bool CanModifyAdditionalSettingsCanBeChanged => (EntityMetadata.CanModifyAdditionalSettings?.CanBeChanged).GetValueOrDefault();

        public bool CanModifyAdditionalSettings
        {
            get => (EntityMetadata.CanModifyAdditionalSettings?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanModifyAdditionalSettings == null || !EntityMetadata.CanModifyAdditionalSettings.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanModifyAdditionalSettings));
                this.EntityMetadata.CanModifyAdditionalSettings.Value = value;
                this.OnPropertyChanged(nameof(CanModifyAdditionalSettings));
            }
        }

        private bool? _initialIsCustomizable;
        public bool IsCustomizableCanBeChanged => (EntityMetadata.IsCustomizable?.CanBeChanged).GetValueOrDefault();

        public bool IsCustomizable
        {
            get => (EntityMetadata.IsCustomizable?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsCustomizable == null || !EntityMetadata.IsCustomizable.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsCustomizable));
                this.EntityMetadata.IsCustomizable.Value = value;
                this.OnPropertyChanged(nameof(IsCustomizable));
            }
        }

        private bool? _initialIsRenameable;

        public bool IsRenameableCanBeChanged => (EntityMetadata.IsRenameable?.CanBeChanged).GetValueOrDefault();

        public bool IsRenameable
        {
            get => (EntityMetadata.IsRenameable?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsRenameable == null || !EntityMetadata.IsRenameable.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsRenameable));
                this.EntityMetadata.IsRenameable.Value = value;
                this.OnPropertyChanged(nameof(IsRenameable));
            }
        }

        private bool? _initialIsValidForQueue;

        public bool IsValidForQueueCanBeChanged => (EntityMetadata.IsValidForQueue?.CanBeChanged).GetValueOrDefault();

        public bool IsValidForQueue
        {
            get => (EntityMetadata.IsValidForQueue?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsValidForQueue == null || !EntityMetadata.IsValidForQueue.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsValidForQueue));
                this.EntityMetadata.IsValidForQueue.Value = value;
                this.OnPropertyChanged(nameof(IsValidForQueue));
            }
        }

        private bool? _initialIsConnectionsEnabled;

        public bool IsConnectionsEnabledCanBeChanged => (EntityMetadata.IsConnectionsEnabled?.CanBeChanged).GetValueOrDefault();

        public bool IsConnectionsEnabled
        {
            get => (EntityMetadata.IsConnectionsEnabled?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsConnectionsEnabled == null || !EntityMetadata.IsConnectionsEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsConnectionsEnabled));
                this.EntityMetadata.IsConnectionsEnabled.Value = value;
                this.OnPropertyChanged(nameof(IsConnectionsEnabled));
            }
        }

        private bool? _initialIsMappable;

        public bool IsMappableCanBeChanged => (EntityMetadata.IsMappable?.CanBeChanged).GetValueOrDefault();

        public bool IsMappable
        {
            get => (EntityMetadata.IsMappable?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsMappable == null || !EntityMetadata.IsMappable.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsMappable));
                this.EntityMetadata.IsMappable.Value = value;
                this.OnPropertyChanged(nameof(IsMappable));
            }
        }

        private bool? _initialIsDuplicateDetectionEnabled;

        public bool IsDuplicateDetectionEnabledCanBeChanged => (EntityMetadata.IsDuplicateDetectionEnabled?.CanBeChanged).GetValueOrDefault();

        public bool IsDuplicateDetectionEnabled
        {
            get => (EntityMetadata.IsDuplicateDetectionEnabled?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsDuplicateDetectionEnabled == null || !EntityMetadata.IsDuplicateDetectionEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsDuplicateDetectionEnabled));
                this.EntityMetadata.IsDuplicateDetectionEnabled.Value = value;
                this.OnPropertyChanged(nameof(IsDuplicateDetectionEnabled));
            }
        }

        private bool? _initialCanCreateAttributes;

        public bool CanCreateAttributesCanBeChanged => (EntityMetadata.CanCreateAttributes?.CanBeChanged).GetValueOrDefault();

        public bool CanCreateAttributes
        {
            get => (EntityMetadata.CanCreateAttributes?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanCreateAttributes == null || !EntityMetadata.CanCreateAttributes.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanCreateAttributes));
                this.EntityMetadata.CanCreateAttributes.Value = value;
                this.OnPropertyChanged(nameof(CanCreateAttributes));
            }
        }

        private bool? _initialCanCreateForms;

        public bool CanCreateFormsCanBeChanged => (EntityMetadata.CanCreateForms?.CanBeChanged).GetValueOrDefault();

        public bool CanCreateForms
        {
            get => (EntityMetadata.CanCreateForms?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanCreateForms == null || !EntityMetadata.CanCreateForms.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanCreateForms));
                this.EntityMetadata.CanCreateForms.Value = value;
                this.OnPropertyChanged(nameof(CanCreateForms));
            }
        }

        private bool? _initialCanCreateViews;

        public bool CanCreateViewsCanBeChanged => (EntityMetadata.CanCreateViews?.CanBeChanged).GetValueOrDefault();

        public bool CanCreateViews
        {
            get => (EntityMetadata.CanCreateViews?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanCreateViews == null || !EntityMetadata.CanCreateViews.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanCreateViews));
                this.EntityMetadata.CanCreateViews.Value = value;
                this.OnPropertyChanged(nameof(CanCreateViews));
            }
        }

        private bool? _initialCanCreateCharts;

        public bool CanCreateChartsCanBeChanged => (EntityMetadata.CanCreateCharts?.CanBeChanged).GetValueOrDefault();

        public bool CanCreateCharts
        {
            get => (EntityMetadata.CanCreateCharts?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanCreateCharts == null || !EntityMetadata.CanCreateCharts.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanCreateCharts));
                this.EntityMetadata.CanCreateCharts.Value = value;
                this.OnPropertyChanged(nameof(CanCreateCharts));
            }
        }

        private bool? _initialCanBeRelatedEntityInRelationship;

        public bool CanBeRelatedEntityInRelationshipCanBeChanged => (EntityMetadata.CanBeRelatedEntityInRelationship?.CanBeChanged).GetValueOrDefault();

        public bool CanBeRelatedEntityInRelationship
        {
            get => (EntityMetadata.CanBeRelatedEntityInRelationship?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanBeRelatedEntityInRelationship == null || !EntityMetadata.CanBeRelatedEntityInRelationship.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanBeRelatedEntityInRelationship));
                this.EntityMetadata.CanBeRelatedEntityInRelationship.Value = value;
                this.OnPropertyChanged(nameof(CanBeRelatedEntityInRelationship));
            }
        }

        private bool? _initialCanBePrimaryEntityInRelationship;

        public bool CanBePrimaryEntityInRelationshipCanBeChanged => (EntityMetadata.CanBePrimaryEntityInRelationship?.CanBeChanged).GetValueOrDefault();

        public bool CanBePrimaryEntityInRelationship
        {
            get => (EntityMetadata.CanBePrimaryEntityInRelationship?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanBePrimaryEntityInRelationship == null || !EntityMetadata.CanBePrimaryEntityInRelationship.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanBePrimaryEntityInRelationship));
                this.EntityMetadata.CanBePrimaryEntityInRelationship.Value = value;
                this.OnPropertyChanged(nameof(CanBePrimaryEntityInRelationship));
            }
        }

        private bool? _initialCanBeInManyToMany;

        public bool CanBeInManyToManyCanBeChanged => (EntityMetadata.CanBeInManyToMany?.CanBeChanged).GetValueOrDefault();

        public bool CanBeInManyToMany
        {
            get => (EntityMetadata.CanBeInManyToMany?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanBeInManyToMany == null || !EntityMetadata.CanBeInManyToMany.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanBeInManyToMany));
                this.EntityMetadata.CanBeInManyToMany.Value = value;
                this.OnPropertyChanged(nameof(CanBeInManyToMany));
            }
        }

        private bool? _initialCanBeInCustomEntityAssociation;

        public bool CanBeInCustomEntityAssociationCanBeChanged => (EntityMetadata.CanBeInCustomEntityAssociation?.CanBeChanged).GetValueOrDefault();

        public bool CanBeInCustomEntityAssociation
        {
            get => (EntityMetadata.CanBeInCustomEntityAssociation?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanBeInCustomEntityAssociation == null || !EntityMetadata.CanBeInCustomEntityAssociation.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanBeInCustomEntityAssociation));
                this.EntityMetadata.CanBeInCustomEntityAssociation.Value = value;
                this.OnPropertyChanged(nameof(CanBeInCustomEntityAssociation));
            }
        }

        private bool? _initialCanEnableSyncToExternalSearchIndex;

        public bool CanEnableSyncToExternalSearchIndexCanBeChanged => (EntityMetadata.CanEnableSyncToExternalSearchIndex?.CanBeChanged).GetValueOrDefault();

        public bool CanEnableSyncToExternalSearchIndex
        {
            get => (EntityMetadata.CanEnableSyncToExternalSearchIndex?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanEnableSyncToExternalSearchIndex == null || !EntityMetadata.CanEnableSyncToExternalSearchIndex.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanEnableSyncToExternalSearchIndex));
                this.EntityMetadata.CanEnableSyncToExternalSearchIndex.Value = value;
                this.OnPropertyChanged(nameof(CanEnableSyncToExternalSearchIndex));
            }
        }

        private bool? _initialCanChangeHierarchicalRelationship;

        public bool CanChangeHierarchicalRelationshipCanBeChanged => (EntityMetadata.CanChangeHierarchicalRelationship?.CanBeChanged).GetValueOrDefault();

        public bool CanChangeHierarchicalRelationship
        {
            get => (EntityMetadata.CanChangeHierarchicalRelationship?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanChangeHierarchicalRelationship == null || !EntityMetadata.CanChangeHierarchicalRelationship.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanChangeHierarchicalRelationship));
                this.EntityMetadata.CanChangeHierarchicalRelationship.Value = value;
                this.OnPropertyChanged(nameof(CanChangeHierarchicalRelationship));
            }
        }

        private bool? _initialCanChangeTrackingBeEnabled;

        public bool CanChangeTrackingBeEnabledCanBeChanged => (EntityMetadata.CanChangeTrackingBeEnabled?.CanBeChanged).GetValueOrDefault();

        public bool CanChangeTrackingBeEnabled
        {
            get => (EntityMetadata.CanChangeTrackingBeEnabled?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.CanChangeTrackingBeEnabled == null || !EntityMetadata.CanChangeTrackingBeEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(CanChangeTrackingBeEnabled));
                this.EntityMetadata.CanChangeTrackingBeEnabled.Value = value;
                this.OnPropertyChanged(nameof(CanChangeTrackingBeEnabled));
            }
        }

        private bool? _initialIsMailMergeEnabled;

        public bool IsMailMergeEnabledCanBeChanged => (EntityMetadata.IsMailMergeEnabled?.CanBeChanged).GetValueOrDefault();

        public bool IsMailMergeEnabled
        {
            get => (EntityMetadata.IsMailMergeEnabled?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsMailMergeEnabled == null || !EntityMetadata.IsMailMergeEnabled.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsMailMergeEnabled));
                this.EntityMetadata.IsMailMergeEnabled.Value = value;
                this.OnPropertyChanged(nameof(IsMailMergeEnabled));
            }
        }

        private bool? _initialIsVisibleInMobile;

        public bool IsVisibleInMobileCanBeChanged => (EntityMetadata.IsVisibleInMobile?.CanBeChanged).GetValueOrDefault();

        public bool IsVisibleInMobile
        {
            get => (EntityMetadata.IsVisibleInMobile?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsVisibleInMobile == null || !EntityMetadata.IsVisibleInMobile.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsVisibleInMobile));
                this.EntityMetadata.IsVisibleInMobile.Value = value;
                this.OnPropertyChanged(nameof(IsVisibleInMobile));
            }
        }

        private bool? _initialIsVisibleInMobileClient;

        public bool IsVisibleInMobileClientCanBeChanged => (EntityMetadata.IsVisibleInMobileClient?.CanBeChanged).GetValueOrDefault();

        public bool IsVisibleInMobileClient
        {
            get => (EntityMetadata.IsVisibleInMobileClient?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsVisibleInMobileClient == null || !EntityMetadata.IsVisibleInMobileClient.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsVisibleInMobileClient));
                this.EntityMetadata.IsVisibleInMobileClient.Value = value;
                this.OnPropertyChanged(nameof(IsVisibleInMobileClient));
            }
        }

        private bool? _initialIsReadOnlyInMobileClient;

        public bool IsReadOnlyInMobileClientCanBeChanged => (EntityMetadata.IsReadOnlyInMobileClient?.CanBeChanged).GetValueOrDefault();

        public bool IsReadOnlyInMobileClient
        {
            get => (EntityMetadata.IsReadOnlyInMobileClient?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsReadOnlyInMobileClient == null || !EntityMetadata.IsReadOnlyInMobileClient.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsReadOnlyInMobileClient));
                this.EntityMetadata.IsReadOnlyInMobileClient.Value = value;
                this.OnPropertyChanged(nameof(IsReadOnlyInMobileClient));
            }
        }

        private bool? _initialIsOfflineInMobileClient;

        public bool IsOfflineInMobileClientCanBeChanged => (EntityMetadata.IsOfflineInMobileClient?.CanBeChanged).GetValueOrDefault();

        public bool IsOfflineInMobileClient
        {
            get => (EntityMetadata.IsOfflineInMobileClient?.Value).GetValueOrDefault();
            set
            {
                if (EntityMetadata.IsOfflineInMobileClient == null || !EntityMetadata.IsOfflineInMobileClient.CanBeChanged)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IsOfflineInMobileClient));
                this.EntityMetadata.IsOfflineInMobileClient.Value = value;
                this.OnPropertyChanged(nameof(IsOfflineInMobileClient));
            }
        }

        //private bool? _initial;

        //public bool CanBeChanged => (EntityMetadata.IsValidForQueue?.CanBeChanged).GetValueOrDefault();

        //public bool IsValidForQueue
        //{
        //    get => (EntityMetadata.IsValidForQueue?.Value).GetValueOrDefault();
        //    set
        //    {
        //        if (EntityMetadata.IsValidForQueue == null || !EntityMetadata.IsValidForQueue.CanBeChanged)
        //        {
        //            return;
        //        }

        //        this.OnPropertyChanging(nameof(IsValidForQueue));
        //        this.EntityMetadata.IsValidForQueue.Value = value;
        //        this.OnPropertyChanged(nameof(IsValidForQueue));
        //    }
        //}

        #endregion ManagedProperties
    }
}
