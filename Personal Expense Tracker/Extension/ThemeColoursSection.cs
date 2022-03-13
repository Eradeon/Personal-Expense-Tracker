using System;
using System.Configuration;

namespace Personal_Expense_Tracker.Extension
{
    internal class ThemeColoursSection : ConfigurationSection
    {
        [ConfigurationProperty("themeColours")]
        public ThemeColoursCollection ThemeColours
        {
            get { return (ThemeColoursCollection)this["themeColours"]; }
        }
    }

    internal class ThemeColoursCollection : ConfigurationElementCollection
    {
        public new ThemeColourElement this[string key]
        {
            get { return (ThemeColourElement)BaseGet(key); }
            set
            {
                if (BaseGet(key) != null)
                    base[key] = value;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ThemeColourElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ThemeColourElement)element).Key;
        }
    }

    internal class ThemeColourElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get { return (string)this["displayName"]; }
            set { this["displayName"] = value; }
        }

        [ConfigurationProperty("colourCode", IsRequired = true)]
        public string ColourCode
        {
            get { return (string)this["colourCode"]; }
            set { this["colourCode"] = value; }
        }
    }
}
