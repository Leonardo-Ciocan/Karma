using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace KarmaApp
{
    public partial class Tutorial : Page
    {
        public Tutorial()
        {
            InitializeComponent();
            this.Loaded += Tutorial_Loaded;
        }

        void Tutorial_Loaded(object sender , RoutedEventArgs e)
        {

            flipView.SelectedIndex = -1;
            flipView.SelectedIndex = 0;

            beginBtn.Tapped += beginBtn_Tapped;

            StatusBar.GetForCurrentView().HideAsync();

            flipView.SelectionChanged += (a, b) =>
            {
                foreach (Ellipse el in dots.Children)
                {
                    el.Fill = null;
                }
                (dots.Children[flipView.SelectedIndex] as Ellipse).Fill =
                    new SolidColorBrush
                    {
                        Color = ((dots.Children[flipView.SelectedIndex] as Ellipse).Stroke as SolidColorBrush).Color,
                        Opacity = 0.6
                    }; 

                if (flipView.SelectedIndex == 3) dots.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                else dots.Visibility = Windows.UI.Xaml.Visibility.Visible;
            };

            skip.Tapped += beginBtn_Tapped;
            colorAnim.Begin();
        }

        void beginBtn_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}