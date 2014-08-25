using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace KarmaApp
{
    public sealed partial class RewardControl : UserControl
    {
        public RewardControl()
        {
            this.InitializeComponent();
            this.Loaded += RewardControl_Loaded;
        }

        bool open = false;
        void RewardControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddCoins.Tapped += (a, b) =>
            {
                if (User.Current.TotalCoins - (DataContext as Reward).Value < 0) return;
                User.Current.TotalCoins -= (DataContext as Reward).Value;
                Log newLog = new Log
                {
                    Positive = false,
                    Value = (DataContext as Reward).Value,
                    Name = (DataContext as Reward).Name,
                    Time = DateTime.Now
                };
                User.Current.Log(newLog);
            };

            btnEdit.Tapped += (c, d) =>
            {
                open = !open;
                editor.Visibility = (open) ? Visibility.Visible :Visibility.Collapsed;
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
            User.Current.Rewards.Remove(DataContext as Reward);
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
