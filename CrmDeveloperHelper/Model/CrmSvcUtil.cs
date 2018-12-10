using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class CrmSvcUtil : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private Guid _Id;

        [DataMember]
        public Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Id));
                this._Id = value;
                this.OnPropertyChanged(nameof(Id));
            }
        }

        private string _Path;

        [DataMember]
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_Path == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Path));
                this._Path = value;
                this.OnPropertyChanged(nameof(Path));
            }
        }

        private string _Version;

        [DataMember]
        public string Version
        {
            get
            {
                return _Version;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_Version == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Version));
                this._Version = value;
                this.OnPropertyChanged(nameof(Version));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Path, this.Version);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            if (!string.IsNullOrEmpty(this.Path) && File.Exists(this.Path))
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(this.Path);
                this.Version = string.Format("{0}.{1}.{2}.{3}", versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductPrivatePart, versionInfo.ProductBuildPart);
            }
        }
    }
}