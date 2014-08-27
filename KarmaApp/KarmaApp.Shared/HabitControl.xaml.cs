using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace KarmaApp
{
    public sealed partial class HabitControl : UserControl
    {
        public HabitControl()
        {
            this.InitializeComponent();
            this.Loaded += HabitControl_Loaded;
            
        }

        bool open = false;
        void HabitControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddCoins.Tapped += (a, b) =>
            {
                User.Current.TotalCoins += (DataContext as Habit).Value * ((DataContext as Habit).Positive ? 1:-1);
                Log newLog = new Log
                {
                    Positive=(DataContext as Habit).Positive,
                    Value = (DataContext as Habit).Value,
                    Name = (DataContext as Habit).Name,
                    Time = DateTime.Now
                };
                User.Current.Log(newLog);
            };


            //root.Background = new SolidColorBrush((DataContext as Habit).Positive ? Color.FromArgb(255, 101, 167, 101) : Color.FromArgb(255, 197, 36, 0));


            btnEdit.Tapped += (c, d) =>
            {
                open = !open;
                editor.Visibility = (open) ? Visibility.Visible : Visibility.Collapsed;
                this.Height = open ? 285 : 50;

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

            AddCoins.PointerPressed += (a, b) =>
            {
                AddCoins.CapturePointer(b.Pointer);
                VisualStateManager.GoToState(this, "PointerDown", true);
                AddCoins.Opacity = 0.4;
            };

            AddCoins.PointerReleased += (a, b) =>
            {
                AddCoins.ReleasePointerCapture(b.Pointer);
                VisualStateManager.GoToState(this, "PointerUp", true);
                AddCoins.Opacity = 1;
            };

            
        }

        private void MenuFlyoutItem_Tapped(object sender, RoutedEventArgs e)
        {
            User.Current.Habits.Remove(DataContext as Habit);
            User.Current.Save();
        }
        private void ToggleSwitch_Tapped(object sender, RoutedEventArgs e)
        {
            root.Background = new SolidColorBrush((DataContext as Habit).Positive ? Color.FromArgb(255, 101, 167, 101) : Color.FromArgb(255, 197, 36, 0));

        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            //this.CapturePointer(e.Pointer);
            //VisualStateManager.GoToState(this, "PointerDown", true);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            //VisualStateManager.GoToState(this, "PointerUp", true);
            //this.ReleasePointerCapture(e.Pointer);
        }

        

    }
}
