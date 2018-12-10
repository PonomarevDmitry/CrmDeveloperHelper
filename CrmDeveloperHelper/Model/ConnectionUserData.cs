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
            get => _UserId;
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
            get => _Username;
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
            get => _Password;
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

        private string _Salt;

        [DataMember]
        public string Salt
        {
            get => _Salt;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                if (_Salt == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(Salt));
                this._Salt = value;
                this.OnPropertyChanged(nameof(Salt));
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
            FillRequiredFields();

            this.Password = Encryption.Encrypt(this.Password, this.Salt);
        }

        [OnSerialized]
        private void AfterSerialize(StreamingContext context)
        {
            AfterSerializeAndDeserialize(context);
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            AfterSerializeAndDeserialize(context);
        }

        private void AfterSerializeAndDeserialize(StreamingContext context)
        {
            FillRequiredFields();

            try
            {
                this.Password = Encryption.Decrypt(this.Password, this.Salt);
            }
            catch (Exception ex)
            {
                DTEHelper.Singleton?.WriteToOutput(Properties.OutputStrings.DecryptingPasswordErrorFormat1, this.Username);
                this.Password = "empty";
                DTEHelper.WriteExceptionToLog(ex);
            }
        }

        private void FillRequiredFields()
        {
            if (string.IsNullOrEmpty(this.Salt))
            {
                this.Salt = Encryption.GenerateSalt();
            }

            if (this.UserId == Guid.Empty)
            {
                this.UserId = Guid.NewGuid();
            }
        }
    }
}