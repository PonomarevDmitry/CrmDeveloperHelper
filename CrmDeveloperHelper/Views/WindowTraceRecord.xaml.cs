using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowTraceRecord : WindowBase
    {
        public TraceRecord TraceRecord { get; private set; }

        public event EventHandler<EventArgs> NextClicked;

        private void OnNextClicked()
        {
            NextClicked?.Invoke(this, new EventArgs());
        }

        public event EventHandler<EventArgs> PreviousClicked;

        private void OnPreviousClicked()
        {
            PreviousClicked?.Invoke(this, new EventArgs());
        }

        public WindowTraceRecord()
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            txtBDescription.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void tSBUp_Click(object sender, RoutedEventArgs e)
        {
            OnNextClicked();
        }

        private void tSBDown_Click(object sender, RoutedEventArgs e)
        {
            OnPreviousClicked();
        }

        internal void SetTraceRecordInformation(TraceRecord item)
        {
            this.TraceRecord = item;

            this.Title = string.Format("TraceRecord: {0:yyyy.MM.dd HH:mm:ss.fff}", item.Date);

            this.txtBDescription.Text = item.Description;

            txtBDescription.Focus();
        }

        private void txtBDescription_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != 0)
            {
                if (e.SystemKey == Key.Up || e.Key == Key.Up)
                {
                    e.Handled = true;

                    OnNextClicked();
                    return;
                }
                else if (e.SystemKey == Key.Down || e.Key == Key.Down)
                {
                    e.Handled = true;

                    OnPreviousClicked();
                    return;
                }
            }
        }
    }
}
