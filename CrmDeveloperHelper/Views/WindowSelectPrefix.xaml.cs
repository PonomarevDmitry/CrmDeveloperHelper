using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectPrefix : WindowBase
    {
        /// <summary>
        /// Настройки расширения
        /// </summary>
        public string Prefix { get; private set; }

        private readonly Func<string, bool> _checker;
        private readonly string _message;

        public WindowSelectPrefix(string windowTitle, string labelTitle, Func<string, bool> checker = null, string message = null)
        {
            InitializeComponent();

            this.Title = windowTitle;
            this._checker = checker;
            this._message = message;
            lblText.Content = labelTitle;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            txtBPrefix.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void ClickOkButton()
        {
            string text = txtBPrefix.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show(Properties.MessageBoxStrings.PrefixIsEmpty, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isValid = _checker?.Invoke(text) ?? true;

            if (!isValid)
            {
                MessageBox.Show(_message, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Prefix = text;

            this.DialogResult = true;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            ClickOkButton();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                ClickOkButton();
            }

            base.OnKeyDown(e);
        }
    }
}
