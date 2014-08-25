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
    public sealed partial class ToDoHub : UserControl
    {
        TextBox txt;
        public ToDoHub()
        {
            this.InitializeComponent();
            this.Loaded += ToDoHub_Loaded;
            txt = textBox.InnerTextBox;
        }

        void ToDoHub_Loaded(object sender,RoutedEventArgs e)
        {
            
            txt.KeyDown += (a, b) =>
            {
                if (b.Key == Windows.System.VirtualKey.Enter)
                {
                    (DataContext as User).ToDos.Add(new ToDo { Name = txt.Text, Value = User.Current.DefaultValue });
                    b.Handled = true;
                    textBox.Text = "";
                    User.Current.Save();
                    scroller.Focus(FocusState.Programmatic);
                    txt.IsEnabled = false;
                    txt.IsEnabled = true;
                }
            };
        }
    }
}
