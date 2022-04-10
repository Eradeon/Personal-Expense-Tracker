using System;
using Personal_Expense_Tracker.Model;

namespace Personal_Expense_Tracker.ViewModel
{
    public class StatisticCardViewModel : BaseViewModel
    {
        private readonly StatisticCard _statisticCard;

        public string Icon
        {
            get { return _statisticCard.Icon; }
            set { _statisticCard.Icon = value; RaisePropertyChanged(); }
        }

        public string Title
        {
            get { return _statisticCard.Title; }
            set { _statisticCard.Title = value; RaisePropertyChanged(); }
        }

        public string Value
        {
            get { return _statisticCard.Value; }
            set { _statisticCard.Value = value; RaisePropertyChanged(); }
        }

        public string SupportText
        {
            get { return _statisticCard.SupportText; }
            set { _statisticCard.SupportText = value; RaisePropertyChanged(); }
        }

        public string TooltipText
        {
            get { return _statisticCard.TooltipText; }
            set { _statisticCard.TooltipText = value; RaisePropertyChanged(); }
        }

        public StatisticCardViewModel(StatisticCard statisticCard)
        {
            _statisticCard = statisticCard;
        }
    }
}
