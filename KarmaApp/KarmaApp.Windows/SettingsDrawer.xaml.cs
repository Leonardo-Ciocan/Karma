using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace KarmaApp
{
    public sealed partial class SettingsDrawer : SettingsFlyout
    {
        public SettingsDrawer()
        {
            this.InitializeComponent();
            this.DataContext = User.Current;

            btnReset.Tapped += async (a, b) =>
            {
                var diag = new MessageDialog("Are you sure you want to delete all data?");
                diag.Commands.Add(new UICommand("delete all", (x) =>
                {
                    User.Current.Habits.Clear();
                    User.Current.Rewards.Clear();
                    User.Current.ToDos.Clear();
                    User.Current.TotalCoins = 0;
                    User.Current.Logs.Clear();
                    User.Current.RaisePropertyChanged("TotalCoins");
                    User.Current.Save();
                }));
                diag.Commands.Add(new UICommand("cancel"));
                await diag.ShowAsync();
            };
        }
    }
}
