using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowTextField : WindowBase
    {
        public string FieldText { get; private set; }

        public WindowTextField(string windowTitle, string labelTitle, string fieldText, bool isReadOnly = false)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this.Title = windowTitle;
            lblText.Content = labelTitle;
            txtBFieldText.Text = fieldText;

            if (isReadOnly)
            {
                txtBFieldText.IsReadOnly = true;
                txtBFieldText.IsReadOnlyCaretVisible = true;

                btnOk.Visibility = Visibility.Collapsed;
                btnOk.IsEnabled = false;

                btnCancel.Content = "Close";
            }

            txtBFieldText.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void ClickOkButton()
        {
            string text = txtBFieldText.Text.Trim();

            this.FieldText = text;

            this.DialogResult = true;

            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            ClickOkButton();
        }
    }
}
