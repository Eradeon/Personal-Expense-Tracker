using System;
using System.Windows.Media;

namespace Personal_Expense_Tracker.Model
{
    internal class ThemeColour
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Colour { get; set; }
        public SolidColorBrush ColourBrush { get; set; }
        public bool IsColourValid { get; set; }

        public ThemeColour(string key, string name, string displayName, string colour, SolidColorBrush colourBrush, bool isColourValid)
        {
            Key = key;
            Name = name;
            DisplayName = displayName;
            Colour = colour;
            ColourBrush = colourBrush;
            IsColourValid = isColourValid;
        }
    }
}
