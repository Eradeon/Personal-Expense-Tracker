using System.Reflection;
using System.Windows;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: AssemblyTitle("Personal Expense Tracker")]
[assembly: AssemblyProduct("Personal Expense Tracker")]
[assembly: AssemblyCopyright("Copyright © 2022")]
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyCompany("Eradeon")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif