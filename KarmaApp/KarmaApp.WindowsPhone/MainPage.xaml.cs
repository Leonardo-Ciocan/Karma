using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Shapes;
using UIFragments;

namespace KarmaApp
{
    public partial class MainPage : Page
    {
        StatusBarProgressIndicator p = StatusBar.GetForCurrentView().ProgressIndicator;
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
           // Microsoft.Phone.Shell.SystemTray.SetBackgroundColor(this, Color.FromArgb(255,246, 143, 0));
           // Microsoft.Phone.Shell.SystemTray.SetForegroundColor(this, Color.FromArgb(255, 255,255,254));
           // Microsoft.Phone.Shell.SystemTray.SetProgressIndicator(this,p);
            StatusBar.GetForCurrentView().BackgroundColor = Color.FromArgb(255, 246, 143, 0);
            StatusBar.GetForCurrentView().ForegroundColor = Color.FromArgb(255, 255, 255, 254);
            StatusBar.GetForCurrentView().BackgroundOpacity = 1;
            StatusBar.GetForCurrentView().ProgressIndicator.Text = "Karma";
            p.ProgressValue = 0;
            //p.ShowAsync();
            StatusBar.GetForCurrentView().HideAsync();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }



        public bool loaded = false;
        public static bool changes = false;
        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {



                /*var toast = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

                //toast.TextBodyWrap.Text = "You have a new message";

    

            var schedToast = new ScheduledToastNotification(toast, DateTime.Now.AddSeconds(8), new TimeSpan(0, 0, 60), 0);

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(schedToast);*/

            if (User.Current != null)
            {
                tile.Background = (User.Current.Transparent) ? null : new SolidColorBrush(Color.FromArgb(255, 246, 143, 0));
                tileSmall.Background = (User.Current.Transparent) ? null : new SolidColorBrush(Color.FromArgb(255, 246, 143, 0));
            }
            if (!loaded)
            {
               

                ObservableCollection<Habit> ls = new ObservableCollection<Habit>();
                User me = new User();
                me.Habits = ls;
                me.Rewards = new ObservableCollection<Reward>();
                me.Logs = new ObservableCollection<Log>();
                me.ToDos = new ObservableCollection<ToDo>();
                me.KarmaHistory = new ObservableCollection<int>();
                User.Current = me;
                User u = await me.Load();
                if (u != null)
                {
                    DataContext = u;
                    User.Current = u;
                }
                else
                {
                    User.Current = me;
                    DataContext = me;
                }

                User.Current.PropertyChanged += (a, b) =>
                {
                    if (b.PropertyName == "TotalCoins")
                    {
                        p.Text = "Karma : " + User.Current.TotalCoins.ToString();
                        ChangeScore(User.Current.TotalCoins);
                        //chart.draw();
                    }
                    changes = true;

                    //try
                    //{
                    //    if (User.Current != null) User.Current.Save();
                    //}
                    //catch { }
                };

                DispatcherTimer dt = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                dt.Tick += (a, b) =>
                {

                    if (changes)
                    {
                        if (User.Current != null)
                        {

                            User.Current.Save();
                            TileManager.SaveAndPin(tile, tileSmall, "TILE");
                        }
                    }
                    changes = false;
                    //TileManager.SaveAndPin(tile, tileSmall, "tile");
                };
                dt.Start();

                /*hub.SectionsInViewChanged += (a, b) =>
                {
                    int index = int.Parse(hub.SectionsInView.First().Tag as string) ;
                    foreach (Ellipse t in navigator.Children) t.Fill = null;
                    (navigator.Children[index] as Ellipse).Fill = new SolidColorBrush(Colors.LightGray);
                    switch (index)
                    {
                        case 0:
                            {
                                header.Text = "Your information";
                                break;
                            }
                        case 1:
                            {
                                header.Text = "Habits";
                                break;
                            }
                        case 2:
                            {
                                header.Text = "To-do";
                                break;
                            }
                        case 3:
                            {
                                header.Text = "Rewards";
                                break;
                            }
                    }
                };

                

                foreach (Ellipse t in navigator.Children)
                {
                    t.Tapped += (About, b) =>
                    {
                        //TODO: finish this
                        int index = int.Parse(t.Tag as string);
                        var sections = hub.Sections[index];
                        hub.ScrollToSection(sections);
                        foreach (Ellipse tx in navigator.Children) tx.Fill = null;
                        t.Fill = new SolidColorBrush(Colors.LightGray);
                        switch (index)
                        {
                            case 0:
                                {
                                    header.Text = "Your information";
                                    break;
                                }
                            case 1:
                                {
                                    header.Text = "Habits";
                                    break;
                                }
                            case 2:
                                {
                                    header.Text = "To-do";
                                    break;
                                }
                            case 3:
                                {
                                    header.Text = "Rewards";
                                    break;
                                }
                        }
                    };
                
                }*/
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //var btn = (AppBarButton)sender;
            //ApplicationData.
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }


        public async void ChangeScore(int n)
        {
            karmaPanel.Children.Insert(0,new TextBlock { FontSize = 22.667, Text = n.ToString()});
            karmaPanel.Children.Remove(karmaPanel.Children.ElementAt(1));
        }

        private void clearRecentsTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            User.Current.Logs.Clear();
            User.Current.RaisePropertyChanged("TotalCoins");
        }



        
    }
}