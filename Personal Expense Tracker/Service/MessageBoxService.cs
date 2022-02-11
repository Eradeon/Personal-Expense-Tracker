using System;
using System.Windows.Threading;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Service
{
    internal class MessageBoxService
    {
        private readonly MainViewModel _mainViewModel;

        private DispatcherTimer timer = new DispatcherTimer();

        public MessageBoxService(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            timer.Tick += OnTimerTick;
        }

        public void ShowMessageBox(MessageType messageType, string message)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                _mainViewModel.MessageBoxVisible = false;
                _mainViewModel.IsMessageBoxClosing = false;
            }

            _mainViewModel.MessageBoxType = messageType;
            _mainViewModel.MessageText = message;
            _mainViewModel.MessageBoxVisible = true;

            if (messageType != MessageType.Error)
            {
                timer.Interval = TimeSpan.FromMilliseconds(4850);
                timer.Start();
            }
        }

        public void HideMessageBox()
        {
            if (!_mainViewModel.IsMessageBoxClosing)
            {
                timer.Stop();
                timer.Interval = TimeSpan.FromMilliseconds(150);
                timer.Start();
                _mainViewModel.IsMessageBoxClosing = true;
            }
            else
            {
                timer.Stop();
                _mainViewModel.MessageBoxVisible = false;
                _mainViewModel.IsMessageBoxClosing = false;
            }
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            HideMessageBox();
        }
    }
}
