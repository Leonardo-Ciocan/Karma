﻿using System;
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
using Windows.UI.Text;

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

                hub.SectionsInViewChanged += (a, b) =>
                {
                    int index = int.Parse(hub.SectionsInView.First().Tag as string) ;
                    foreach (PivotIcon t in navigator.Children) t.Selected = false;
                    //(navigator.Children[index] as PivotIcon).Selected = true;
                    switch (index)
                    {
                        case 0:
                            {


                                header.Text = "Your information";

                                if (! ((this.BottomAppBar as CommandBar).PrimaryCommands.Contains(btnRecents)))
                                    (this.BottomAppBar as CommandBar).PrimaryCommands.Add(btnRecents);
                                pivotInfo.Selected = true;
                                break;
                            }
                        case 1:
                            {
                                (this.BottomAppBar as CommandBar).PrimaryCommands.Remove(btnRecents);
                                header.Text = "Habits";
                                pivotHabits.Selected = true;
                                break;
                            }
                        case 2:
                            {
                                header.Text = "To-do";
                                pivotToDo.Selected = true;
                                break;
                            }
                        case 3:
                            {
                                header.Text = "Rewards";
                                pivotRewards.Selected = true;
                                break;
                            }
                    }
                };

                

                foreach (PivotIcon t in navigator.Children)
                {
                    t.Tapped += (About, b) =>
                    {
                        //TODO: finish this
                        int index = int.Parse(t.Tag as string);
                        var sections = hub.Sections[index];
                        hub.ScrollToSection(sections);
                        foreach (PivotIcon tx in navigator.Children) tx.Selected = false;
                        t.Selected = true;
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

                    header_Copy.Text = User.Current.TotalCoins.ToString();
                
                }
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
            karmaPanel.Children.Insert(0,new TextBlock { FontSize = 20, Text = n.ToString() , FontWeight = FontWeights.Light , FontFamily = new FontFamily("Segoe WP")});
            karmaPanel.Children.Remove(karmaPanel.Children.ElementAt(1));
        }


        private async void review(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=218bb6cb-bd37-4f36-8e17-3809298e8821"));
            //Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=27721LeonardoCiocan.Karma_55ag5hpdedhr2"));
            //var mailto = new Uri("mailto:?to=leonardo.ciocan@outlook.com&subject=Karma feedback");
            //await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        private void clearRecents(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            User.Current.Logs.Clear();
            User.Current.RaisePropertyChanged("TotalCoins");
        }



        
    }
}