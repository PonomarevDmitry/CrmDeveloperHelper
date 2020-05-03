using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class FileGenerationOptions
    {
        private string _NamespaceGlobalOptionSetsJavaScript;
        [DataMember]
        public string NamespaceGlobalOptionSetsJavaScript
        {
            get => _NamespaceGlobalOptionSetsJavaScript;
            set
            {
                this.OnPropertyChanging(nameof(NamespaceGlobalOptionSetsJavaScript));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NamespaceGlobalOptionSetsJavaScript = value;
                this.OnPropertyChanged(nameof(NamespaceGlobalOptionSetsJavaScript));
            }
        }

        private string _NamespaceClassesJavaScript;
        [DataMember]
        public string NamespaceClassesJavaScript
        {
            get => _NamespaceClassesJavaScript;
            set
            {
                this.OnPropertyChanging(nameof(NamespaceClassesJavaScript));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NamespaceClassesJavaScript = value;
                this.OnPropertyChanged(nameof(NamespaceClassesJavaScript));
            }
        }

        private string _NamespaceSdkMessagesJavaScript;
        [DataMember]
        public string NamespaceSdkMessagesJavaScript
        {
            get => _NamespaceSdkMessagesJavaScript;
            set
            {
                this.OnPropertyChanging(nameof(NamespaceSdkMessagesJavaScript));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NamespaceSdkMessagesJavaScript = value;
                this.OnPropertyChanged(nameof(NamespaceSdkMessagesJavaScript));
            }
        }

        private bool _JavaScriptAddFormTypeEnum = true;
        [DataMember]
        public bool JavaScriptAddFormTypeEnum
        {
            get => _JavaScriptAddFormTypeEnum;
            set
            {
                this.OnPropertyChanging(nameof(JavaScriptAddFormTypeEnum));
                this._JavaScriptAddFormTypeEnum = value;
                this.OnPropertyChanged(nameof(JavaScriptAddFormTypeEnum));
            }
        }

        private bool _JavaScriptAddRequiredLevelEnum = true;
        [DataMember]
        public bool JavaScriptAddRequiredLevelEnum
        {
            get => _JavaScriptAddRequiredLevelEnum;
            set
            {
                this.OnPropertyChanging(nameof(JavaScriptAddRequiredLevelEnum));
                this._JavaScriptAddRequiredLevelEnum = value;
                this.OnPropertyChanged(nameof(JavaScriptAddRequiredLevelEnum));
            }
        }

        private bool _JavaScriptAddSubmitModeEnum = true;
        [DataMember]
        public bool JavaScriptAddSubmitModeEnum
        {
            get => _JavaScriptAddSubmitModeEnum;
            set
            {
                this.OnPropertyChanging(nameof(JavaScriptAddSubmitModeEnum));
                this._JavaScriptAddSubmitModeEnum = value;
                this.OnPropertyChanged(nameof(JavaScriptAddSubmitModeEnum));
            }
        }

        private bool _JavaScriptAddConsoleFunctions = true;
        [DataMember]
        public bool JavaScriptAddConsoleFunctions
        {
            get => _JavaScriptAddConsoleFunctions;
            set
            {
                this.OnPropertyChanging(nameof(JavaScriptAddConsoleFunctions));
                this._JavaScriptAddConsoleFunctions = value;
                this.OnPropertyChanged(nameof(JavaScriptAddConsoleFunctions));
            }
        }

        private bool _GenerateJavaScriptGlobalOptionSet = false;
        [DataMember]
        public bool GenerateJavaScriptGlobalOptionSet
        {
            get => _GenerateJavaScriptGlobalOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateJavaScriptGlobalOptionSet));
                this._GenerateJavaScriptGlobalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateJavaScriptGlobalOptionSet));
            }
        }

        private bool _GenerateJavaScriptIntoSchemaClass = true;
        [DataMember]
        public bool GenerateJavaScriptIntoSchemaClass
        {
            get => _GenerateJavaScriptIntoSchemaClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateJavaScriptIntoSchemaClass));
                this._GenerateJavaScriptIntoSchemaClass = value;
                this.OnPropertyChanged(nameof(GenerateJavaScriptIntoSchemaClass));
            }
        }

        private void LoadFromDiskJavaScript(FileGenerationOptions diskData)
        {
            this.NamespaceClassesJavaScript = diskData.NamespaceClassesJavaScript;
            this.NamespaceGlobalOptionSetsJavaScript = diskData.NamespaceGlobalOptionSetsJavaScript;
            this.NamespaceSdkMessagesJavaScript = diskData.NamespaceSdkMessagesJavaScript;

            this.GenerateJavaScriptIntoSchemaClass = diskData.GenerateJavaScriptIntoSchemaClass;
            this.GenerateJavaScriptGlobalOptionSet = diskData.GenerateJavaScriptGlobalOptionSet;

            this.JavaScriptAddFormTypeEnum = diskData.JavaScriptAddFormTypeEnum;
            this.JavaScriptAddRequiredLevelEnum = diskData.JavaScriptAddRequiredLevelEnum;
            this.JavaScriptAddSubmitModeEnum = diskData.JavaScriptAddSubmitModeEnum;
            this.JavaScriptAddConsoleFunctions = diskData.JavaScriptAddConsoleFunctions;

        }
    }
}
