using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace KarmaApp
{
    public sealed partial class HabitHub : UserControl
    {
        TextBox txt;
        public HabitHub()
        {
            this.InitializeComponent();
            this.Loaded += HabitHub_Loaded;
            txt = textBox.InnerTextBox;
        }


        void HabitHub_Loaded(object sender, RoutedEventArgs e)
        {
            txt.KeyDown += (a, b) =>
            {
                if (b.Key == Windows.System.VirtualKey.Enter)
                {
                    (DataContext as User).Habits.Add(new Habit { Name = txt.Text, Positive = true, Value = User.Current.DefaultValue });
                    b.Handled = true;
                    User.Current.Save();
                    scroller.Focus(FocusState.Programmatic);
                    //txt.IsEnabled = false;
                    //txt.IsEnabled = true;
                    //this.Focus(FocusState.Programmatic);
                    textBox.Text = "";
                    
                }
            };
        }
    }
}
