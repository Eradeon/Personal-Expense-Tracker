using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Service
{
    internal class StatisticsService
    {
        private readonly HomeViewModel _homeViewModel;

        public StatisticsService(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public void CalculateStatistics()
        {
            if (_homeViewModel.ExpenseCollection != null)
            {
                GetMostFrequentedExpense();

                if (_homeViewModel.ExpenseCollection.Count > 0)
                {
                    GetSumAndMeanExpense();
                    GetMinMaxExpense();
                }
                else
                {
                    _homeViewModel.SumExpense = new Tuple<string, int>("0",0);
                    _homeViewModel.MeanExpense = new Tuple<string, string>("0","N/A");
                    _homeViewModel.MinExpense = new Tuple<string, string>("0", "N/A");
                    _homeViewModel.MaxExpense = new Tuple<string, string>("0", "N/A");
                }

                GetGraphData();
            }
        }

        private void GetMostFrequentedExpense()
        {
            List<Tuple<string, int>> result = new List<Tuple<string, int>>();

            var query = _homeViewModel.ExpenseCollection
                .GroupBy(s => s.Name)
                .OrderByDescending(g => g.Count())
                .Select(g => new { g.Key, Count = g.Count() })
                .Take(3)
                .AsQueryable();

            int count = query.Count();

            for (int i = 0; i < 3; i++)
            {
                if (i < count)
                    result.Add(new Tuple<string, int>(query.ElementAt(i).Key, query.ElementAt(i).Count));
                else
                    result.Add(new Tuple<string, int>("N/A",0));
            }

            _homeViewModel.MostFrequentedExpenses = result;
        }

        private void GetSumAndMeanExpense()
        {
            double sum = _homeViewModel.ExpenseCollection.Sum(x => x.Amount);
            int days;

            if (_homeViewModel.GroupByMonth)
                days = DateTime.DaysInMonth(_homeViewModel.SelectedYear, _homeViewModel.SelectedMonth + 1);
            else
                days = DateTime.IsLeapYear(_homeViewModel.SelectedYear) ? 366 : 365;

            _homeViewModel.SumExpense = new Tuple<string, int>
            (
                sum.ToString("C2"),
                _homeViewModel.ExpenseCollection.Count
            );

            _homeViewModel.MeanExpense = new Tuple<string, string>
            (
                (sum / days).ToString("C2"),
                string.Concat(1, "/", days, " dnů")
            );
        }

        private void GetMinMaxExpense()
        {
            double min = _homeViewModel.ExpenseCollection.Min(x => x.Amount);
            double max = _homeViewModel.ExpenseCollection.Max(x => x.Amount);

            _homeViewModel.MinExpense = _homeViewModel.ExpenseCollection
                .Where(x => x.Amount == min)
                .Select(g => new Tuple<string, string>(g.Amount.ToString("C2"), g.Name))
                .First();

            _homeViewModel.MaxExpense = _homeViewModel.ExpenseCollection
                .Where(x => x.Amount == max)
                .Select(g => new Tuple<string, string>(g.Amount.ToString("C2"), g.Name))
                .First();
        }

        private void GetGraphData()
        {
            //Graph Data
            List<Tuple<string, string, int, GridLength, GridLength>> result = new List<Tuple<string, string, int, GridLength, GridLength>>();

            int baseAxis = 0;

            if (_homeViewModel.SumExpense != null && _homeViewModel.SumExpense.Item1 != "0")
            {
                var query = _homeViewModel.ExpenseCollection
                    .GroupBy(s => s.Name)
                    .Select(g => new { Name = g.Key, Amount = g.Sum(x => x.Amount), Count = g.Count() })
                    .OrderByDescending(x => x.Amount)
                    .AsEnumerable();

                if (query.Count() > 0)
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
                        new GridLength((100-pct), GridUnitType.Star)
                    ));
                }
            }

            _homeViewModel.GraphData = result;

            //Axis labels
            _homeViewModel.XAxisLabels = new Tuple<int, int, int, int, int>
            (
                0,
                baseAxis,
                (baseAxis * 2),
                (baseAxis * 3),
                (baseAxis * 4)
            );
        }

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
