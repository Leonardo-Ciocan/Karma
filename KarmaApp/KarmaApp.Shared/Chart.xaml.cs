using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace KarmaApp
{
    public partial class Chart : UserControl
    {
        public Chart()
        {
            InitializeComponent();
            this.Loaded += Chart_Loaded;
            DataContextChanged += (a, b) =>
            {
                if (DataContext == null) return;
                draw();
                (DataContext as User).PropertyChanged += (c, d) =>
                {
                    if (d.PropertyName == "TotalCoins")
                    {
                        draw();
                    }
                };
            };
        }


        User user;
        double r;
        Polyline line = new Polyline { Stroke = new SolidColorBrush(Color.FromArgb(255, 246, 143, 0)), StrokeThickness =2 , StrokeLineJoin = PenLineJoin.Round };
        void Chart_Loaded(object sender, RoutedEventArgs e)
        {
            r = Math.Min(ActualHeight, ActualWidth) / 50.0;
            root.Children.Clear();
            root.Children.Add(line);
            user = (DataContext as User);
            draw();
        }

        
        public void draw()
        {
            user = DataContext as User;
            if (DataContext != null)
            {
                if (user.KarmaHistory.Count <= 0) return;
                line.Points.Clear();
                int max = user.KarmaHistory.Max();
                int min = user.KarmaHistory.Min();
                for (int x = 0; x < (DataContext as User).KarmaHistory.Count; x++)
                {
                    line.Points.Add(new Point(ActualWidth - (user.KarmaHistory.Count - x) * r, ActualHeight - (user.KarmaHistory[x] - min) / (Math.Max(0.001, max - min)) * ActualHeight));
                }
            }
        }

        public Brush LineColor
        {
            set { line.Stroke = value; }
            get { return line.Stroke; }
        }
    }
}
