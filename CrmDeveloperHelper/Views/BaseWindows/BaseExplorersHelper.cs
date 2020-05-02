using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class BaseExplorersHelper
    {
        protected readonly IWriteToOutput _iWriteToOutput;
        protected readonly CommonConfiguration _commonConfig;

        protected readonly Func<string> _getEntityName;
        protected readonly Func<string> _getGlobalOptionSetName;
        protected readonly Func<string> _getWorkflowName;
        protected readonly Func<string> _getSystemFormName;
        protected readonly Func<string> _getSavedQueryName;
        protected readonly Func<string> _getSavedQueryVisualizationName;
        protected readonly Func<string> _getSiteMapName;
        protected readonly Func<string> _getReportName;
        protected readonly Func<string> _getWebResourceName;
        protected readonly Func<string> _getPluginAssemblyName;

        protected BaseExplorersHelper(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , Func<string> getEntityName
            , Func<string> getGlobalOptionSetName
            , Func<string> getWorkflowName
            , Func<string> getSystemFormName
            , Func<string> getSavedQueryName
            , Func<string> getSavedQueryVisualizationName
            , Func<string> getSiteMapName
            , Func<string> getReportName
            , Func<string> getWebResourceName
            , Func<string> getPluginAssemblyName
        )
        {
            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
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

        protected string GetEntityName()
        {
            if (_getEntityName != null)
            {
                return _getEntityName();
            }

            return string.Empty;
        }

        protected string GetGlobalOptionSetName()
        {
            if (_getGlobalOptionSetName != null)
            {
                return _getGlobalOptionSetName();
            }

            return string.Empty;
        }

        protected string GetWorkflowName()
        {
            if (_getWorkflowName != null)
            {
                return _getWorkflowName();
            }

            return string.Empty;
        }

        protected string GetSystemFormName()
        {
            if (_getSystemFormName != null)
            {
                return _getSystemFormName();
            }

            return string.Empty;
        }

        protected string GetSavedQueryName()
        {
            if (_getSavedQueryName != null)
            {
                return _getSavedQueryName();
            }

            return string.Empty;
        }

        protected string GetSavedQueryVisualizationName()
        {
            if (_getSavedQueryVisualizationName != null)
            {
                return _getSavedQueryVisualizationName();
            }

            return string.Empty;
        }

        protected string GetSiteMapName()
        {
            if (_getSiteMapName != null)
            {
                return _getSiteMapName();
            }

            return string.Empty;
        }

        protected string GetReportName()
        {
            if (_getReportName != null)
            {
                return _getReportName();
            }

            return string.Empty;
        }

        protected string GetWebResourceName()
        {
            if (_getWebResourceName != null)
            {
                return _getWebResourceName();
            }

            return string.Empty;
        }

        protected string GetPluginAssemblyName()
        {
            if (_getPluginAssemblyName != null)
            {
                return _getPluginAssemblyName();
            }

            return string.Empty;
        }
    }
}
