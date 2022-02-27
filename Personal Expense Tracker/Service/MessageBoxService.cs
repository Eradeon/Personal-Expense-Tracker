using System;
using System.Windows.Threading;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Service
{
    internal enum MessageType
    {
        Information,
        Warning,
        Error
    }

    internal class MessageBoxService
    {
        private readonly HomeViewModel _homeViewModel;

        private DispatcherTimer timer = new DispatcherTimer();

        public MessageBoxService(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;

            timer.Tick += OnTimerTick;
        }

        public void ShowMessageBox(MessageType messageType, string message)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                _homeViewModel.MessageBoxVisible = false;
                _homeViewModel.IsMessageBoxClosing = false;
            }

            _homeViewModel.MessageBoxType = messageType;
            _homeViewModel.MessageText = message;
            _homeViewModel.MessageBoxVisible = true;

            if (messageType != MessageType.Error)
            {
                timer.Interval = TimeSpan.FromMilliseconds(4850);
                timer.Start();
            }
        }

        public void HideMessageBox()
        {
            if (!_homeViewModel.IsMessageBoxClosing)
            {
                timer.Stop();
                timer.Interval = TimeSpan.FromMilliseconds(150);
                timer.Start();
                _homeViewModel.IsMessageBoxClosing = true;
            }
            else
            {
                timer.Stop();
                _homeViewModel.MessageBoxVisible = false;
                _homeViewModel.IsMessageBoxClosing = false;
            }
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            HideMessageBox();
        }
    }
}
