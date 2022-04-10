using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Service
{
    public class StatisticsService
    {
        private readonly BaseViewModel _viewModel;

        public StatisticsService(BaseViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        #region Home Statistics
        public void CalculateHomeStatistics()
        {
            HomeViewModel homeViewModel = (HomeViewModel)_viewModel;

            if (homeViewModel.ExpenseCollection != null)
            {
                if (homeViewModel.ExpenseCollection.Count > 0)
                {
                    GetCardMostFrequentedExpense(homeViewModel, homeViewModel.StatisticsCardsCollection[0]);
                    GetCardSumAndMeanExpense(homeViewModel, homeViewModel.StatisticsCardsCollection[1], homeViewModel.StatisticsCardsCollection[4]);
                    GetCardMinExpense(homeViewModel, homeViewModel.StatisticsCardsCollection[2]);
                    GetCardMaxExpense(homeViewModel, homeViewModel.StatisticsCardsCollection[3]);
                }
                else
                {
                    for (int i = 0; i < homeViewModel.StatisticsCardsCollection.Count; i++)
                    {
                        homeViewModel.StatisticsCardsCollection[i].Value = "N/A";
                        homeViewModel.StatisticsCardsCollection[i].SupportText = "N/A";
                    }
                }

                GetBarGraphData(homeViewModel);
            }
        }

        private void GetCardMostFrequentedExpense(HomeViewModel homeViewModel, StatisticCardViewModel mostFrequentedExpenseCard)
        {
            var query = homeViewModel.ExpenseCollection
                .GroupBy(s => s.Name)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .First();

            mostFrequentedExpenseCard.Value = query.Name;
            mostFrequentedExpenseCard.SupportText = "Položek: " + query.Count;
        }

        private void GetCardSumAndMeanExpense(HomeViewModel homeViewModel, StatisticCardViewModel sumExpenseCard, StatisticCardViewModel meanExpenseCard)
        {
            double sum = homeViewModel.ExpenseCollection.Sum(x => x.Amount);
            int days;

            if (homeViewModel.GroupByMonth)
                days = DateTime.DaysInMonth(homeViewModel.SelectedYear, homeViewModel.SelectedMonth + 1);
            else
                days = DateTime.IsLeapYear(homeViewModel.SelectedYear) ? 366 : 365;

            //Sum
            sumExpenseCard.Value = sum.ToString("C2");
            sumExpenseCard.SupportText = "Položek: " + homeViewModel.ExpenseCollection.Count;

            //Mean
            meanExpenseCard.Value = (sum / days).ToString("C2");
            meanExpenseCard.SupportText = "Za 1 z " + days + " dnů";
        }

        private void GetCardMinExpense(HomeViewModel homeViewModel, StatisticCardViewModel minExpenseCard)
        {
            double min = homeViewModel.ExpenseCollection.Min(x => x.Amount);

            var query = homeViewModel.ExpenseCollection
                .Where(x => x.Amount == min)
                .Select(g => new { Name = g.Name, Amount = g.Amount })
                .First();

            minExpenseCard.Value = query.Amount.ToString("C2");
            minExpenseCard.SupportText = query.Name;
        }

        private void GetCardMaxExpense(HomeViewModel homeViewModel, StatisticCardViewModel maxExpenseCard)
        {
            double max = homeViewModel.ExpenseCollection.Max(x => x.Amount);

            var query = homeViewModel.ExpenseCollection
                .Where(x => x.Amount == max)
                .Select(g => new { Name = g.Name, Amount = g.Amount })
                .First();

            maxExpenseCard.Value = query.Amount.ToString("C2");
            maxExpenseCard.SupportText = query.Name;
        }

        private void GetBarGraphData(HomeViewModel homeViewModel)
        {
            //Graph Data
            List<Tuple<string, string, int, GridLength, GridLength>> result = new List<Tuple<string, string, int, GridLength, GridLength>>();

            int baseAxis = 0;

            var query = homeViewModel.ExpenseCollection
                .GroupBy(s => s.Name)
                .Select(g => new { Name = g.Key, Amount = g.Sum(x => x.Amount), Count = g.Count() })
                .OrderByDescending(x => x.Amount)
                .AsEnumerable();

            if (query.Count() > 0)
            {
                baseAxis = RoundToNearest(query.First().Amount) / 4;

                foreach (var item in query)
                {
                    int pct = (int)Math.Round((item.Amount / (baseAxis * 4) * 100), MidpointRounding.AwayFromZero);

                    result.Add(new Tuple<string, string, int, GridLength, GridLength>
                    (
                        item.Name,
                        item.Amount.ToString("C2"),
                        item.Count,
                        new GridLength(pct, GridUnitType.Star),
                        new GridLength((100 - pct), GridUnitType.Star)
                    ));
                }
            }

            homeViewModel.GraphData = result;

            //Axis labels
            homeViewModel.XAxisLabels = new Tuple<int, int, int, int, int>
            (
                0,
                baseAxis,
                (baseAxis * 2),
                (baseAxis * 3),
                (baseAxis * 4)
            );
        }
        #endregion Home Statistics

        private int RoundToNearest(double number)
        {
            if (number >= 0 && number <= 10000)
                number = Math.Ceiling(number / 100) * 100;

            else if (number > 10000)
                number = Math.Ceiling(number / 1000) * 1000;

            try
            {
                return (int)number;
            }
            catch
            {
                return 0;
            }
        }
    }
}
