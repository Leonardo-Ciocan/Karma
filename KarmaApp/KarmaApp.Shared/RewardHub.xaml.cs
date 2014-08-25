using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace KarmaApp
{
    public sealed partial class RewardHub : UserControl
    {
        TextBox txt;
        public RewardHub()
        {
            this.InitializeComponent();
            this.Loaded += RewardHub_Loaded;
            txt = textBox.InnerTextBox;
        }

        void RewardHub_Loaded(object sender, RoutedEventArgs e)
        {
            txt.KeyDown += (a, b) =>
            {
                if (b.Key == Windows.System.VirtualKey.Enter)
                {
                    (DataContext as User).Rewards.Add(new Reward { Name = txt.Text, Value = User.Current.DefaultValue });
                    b.Handled = true;
                    textBox.Text = "";
                    scroller.Focus(FocusState.Programmatic);
                    User.Current.Save(); txt.IsEnabled = false;
                    txt.IsEnabled = true;
                }
            };
        
        }
    }
}
