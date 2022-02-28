using System;
using System.Windows.Threading;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Stores
{
    internal enum MessageType
    {
        Information,
        Warning,
        Error
    }

    internal class MessageBoxStore
    {
        private DispatcherTimer timer = new DispatcherTimer();

        public event Action? MessageBoxUpdated;

        #region Properties
        private bool _messageBoxVisible;
        public bool MessageBoxVisible
        {
            get { return _messageBoxVisible; }
            set { _messageBoxVisible = value; }
        }

        private bool _isMessageBoxClosing;
        public bool IsMessageBoxClosing
        {
            get { return _isMessageBoxClosing; }
            set { _isMessageBoxClosing = value; }
        }

        private MessageType _messageBoxType;
        public MessageType MessageBoxType
        {
            get { return _messageBoxType; }
            set { _messageBoxType = value; }
        }

        private string _messageBoxText = string.Empty;
        public string MessageBoxText
        {
            get { return _messageBoxText; }
            set { _messageBoxText = value; }
        }
        #endregion Properties

        public MessageBoxStore()
        {
            timer.Tick += OnTimerTick;
        }

        private void OnMessageBoxUpdated()
        {
            MessageBoxUpdated?.Invoke();
        }

        public void ShowMessageBox(MessageType messageType, string message)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                MessageBoxVisible = false;
                IsMessageBoxClosing = false;
                OnMessageBoxUpdated();
            }

            MessageBoxType = messageType;
            MessageBoxText = message;
            MessageBoxVisible = true;

            if (messageType != MessageType.Error)
            {
                timer.Interval = TimeSpan.FromMilliseconds(4850);
                timer.Start();
            }

            OnMessageBoxUpdated();
        }

        public void HideMessageBox()
        {
            if (!IsMessageBoxClosing)
            {
                timer.Stop();
                timer.Interval = TimeSpan.FromMilliseconds(150);
                timer.Start();
                IsMessageBoxClosing = true;
            }
            else
            {
                timer.Stop();
                MessageBoxVisible = false;
                IsMessageBoxClosing = false;
            }

            OnMessageBoxUpdated();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            HideMessageBox();
        }
    }
}
