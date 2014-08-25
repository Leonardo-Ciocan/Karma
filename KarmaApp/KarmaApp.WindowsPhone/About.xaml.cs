using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace KarmaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        public About()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            /*Windows.Phone.UI.Input.HardwareButtons.BackPressed += (a, b) =>
            {


                
            };*/
            this.DataContext = User.Current;

            btnReset.Tapped += async (a, b) =>
            {
                var diag = new MessageDialog("Are you sure you want to delete all data?");
                diag.Commands.Add(new UICommand("delete all" , (x) => {
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

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }

}
