using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Personal_Expense_Tracker.Model;

namespace Personal_Expense_Tracker.ViewModel
{
    public class ThemeColourViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly ThemeColour _themeColour;

        public string Key
        {
            get { return _themeColour.Key; }
            set { _themeColour.Key = value; RaisePropertyChanged(); }
        }

        public string Name
        {
            get { return _themeColour.Name; }
            set { _themeColour.Name = value; RaisePropertyChanged(); }
        }

        public string DisplayName
        {
            get { return _themeColour.DisplayName; }
            set { _themeColour.DisplayName = value; RaisePropertyChanged(); }
        }

        public string Colour
        {
            get { return _themeColour.Colour; }
            set { _themeColour.Colour = value; RaisePropertyChanged(); }
        }

        public SolidColorBrush ColorBrush
        {
            get { return _themeColour.ColourBrush; }
            set { _themeColour.ColourBrush = value; RaisePropertyChanged(); }
        }

        public bool IsColourValid
        {
            get { return _themeColour.IsColourValid; }
            set { _themeColour.IsColourValid = value; RaisePropertyChanged(); }
        }

        #region Colour Validation
        public string Error { get; }

        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName);
            }
        }

        private string GetErrorForProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "Colour":
                    if (!IsHexColourCode(Colour))
                    {
                        IsColourValid = false;
                        return "Chyba: Neplatný hexadecimální kód.";
                    }
                    else
                    {
                        ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colour));
                        IsColourValid = true;
                        return string.Empty;
                    }

                default:
                    return string.Empty;
            }
        }
        #endregion Colour Validation

        public ThemeColourViewModel(ThemeColour themeColour)
        {
            _themeColour = themeColour;
            Error = string.Empty;
        }

        private bool IsHexColourCode(string input)
        {
            return Regex.Match(input, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success ? true : false;
        }
    }
}
