using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class ConnectionUserData : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private Guid _UserId;

        [DataMember]
        public Guid UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                if (_UserId == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(UserId));
                this._UserId = value;
                this.OnPropertyChanged(nameof(UserId));
            }
        }

        private string _Username;

        /// <summary>
        /// Пользователь для подключения
        /// </summary>
        [DataMember]
        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_Username == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Username));
                this._Username = value;
                this.OnPropertyChanged(nameof(Username));
            }
        }

        private string _Password;

        /// <summary>
        /// Пароль для подключения
        /// </summary>
        [DataMember]
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_Password == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Password));
                this._Password = value;
                this.OnPropertyChanged(nameof(Password));
            }
        }

        public override string ToString()
        {
            return this.Username;
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

        [OnSerializing]
        private void BeforeSerializing(StreamingContext context)
        {
            this.Password = Encryption.Encrypt(this.Password, Encryption.EncryptionKey);

            if (this.UserId == Guid.Empty)
            {
                this.UserId = Guid.NewGuid();
            }
        }

        [OnSerialized]
        private void AfterSerialize(StreamingContext context)
        {
            this.Password = Encryption.Decrypt(this.Password, Encryption.EncryptionKey);
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            if (this.UserId == Guid.Empty)
            {
                this.UserId = Guid.NewGuid();
            }

            this.Password = Encryption.Decrypt(this.Password, Encryption.EncryptionKey);
        }
    }
}