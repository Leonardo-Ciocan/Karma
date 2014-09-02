using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace KarmaApp
{
    public sealed partial class ToDoControl : UserControl
    {
        public ToDoControl()
        {
            this.InitializeComponent();
            this.Loaded += ToDoControl_Loaded;
            /*DataContextChanged += (a, b) =>
            {
                if(DataContext != null) this.Opacity = (DataContext as ToDo).Checked ? 0.15 : 1;
            };*/
        }
        bool open = false;
        void ToDoControl_Loaded(object sender, RoutedEventArgs e)
        {

            this.Opacity = (DataContext as ToDo).Checked ? 0.15 : 1;
            check.Tapped += (a, b) =>
            {
                b.Handled = true;
                this.Opacity = (DataContext as ToDo).Checked ? 0.15 : 1;
                Log newLog = new Log
                {
                    Positive = true,
                    Value = (DataContext as ToDo).Value,
                    Name = (DataContext as ToDo).Name,
                    Time = DateTime.Now
                };
                User.Current.Log(newLog);
               
                User.Current.TotalCoins += ((DataContext as ToDo).Value * (((DataContext as ToDo).Checked) ? 1 : -1));

                if (User.Current.DeleteOnCheck)
                {
                    User.Current.ToDos.Remove(DataContext as ToDo);
                    User.Current.Save();
                }

            };

            this.Tapped += (c, d) =>
            {
                open = !open;
                editor.Visibility = (open) ? Visibility.Visible : Visibility.Collapsed;
                this.Height = open ? 220 : 50;

            };
            Holding += (a, b) =>
            {
                FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(root);

                flyoutBase.ShowAt(this);
            };
            editor.PointerPressed += (a, b) =>
            {
                b.Handled = true;
            };
        }

        private void MenuFlyoutItem_Tapped(object sender, RoutedEventArgs e)
        {
            User.Current.ToDos.Remove(DataContext as ToDo);
            User.Current.Save();
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(this, "PointerDown", true);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerUp", true);
            this.ReleasePointerCapture(e.Pointer);
        }

    }
}
