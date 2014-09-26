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
using Windows.UI.Text;

namespace KarmaApp
{
    
        

    public partial class MainPage : Page
    {
        HubSection todoHub;
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
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += (a, b) =>
            {
                if (Frame.CurrentSourcePageType != typeof(MainPage))
                {
                    b.Handled = true;
                }
                if (Frame.CanGoBack) Frame.GoBack();
            };
        }


        List<SolidColorBrush> Colors = new List<SolidColorBrush>
                {
                    (SolidColorBrush)App.Current.Resources["gray"],
                    (SolidColorBrush)App.Current.Resources["green"],
                    (SolidColorBrush)App.Current.Resources["blue"],
                    (SolidColorBrush)App.Current.Resources["orange"]
                };
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
                RefreshTodoVisibility();
            }


            if (!loaded)
            {
                todoHub = hub.Sections[2];
                loaded = true;

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

                me.SearchedHabits = new ObservableCollection<Habit>();
                me.SearchedToDos = new ObservableCollection<ToDo>();
                me.SearchedRewards = new ObservableCollection<Reward>();


                RefreshTodoVisibility();

                

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

                int lastindex = 0;
                //var LastState = hub.SectionsInView;
                hub.SectionsInViewChanged += (a, b) =>
                {
                    /*if (LastState == null) LastState = hub.SectionsInView;
                    var diff = hub.SectionsInView.Except(LastState);
                    LastState = hub.SectionsInView;
                    if (diff.Count() !=1) return;*/

                    int index = int.Parse(hub.SectionsInView[0].Tag as string) ;
                    
                    if(lastindex != index) movedTo(index);
                    lastindex = index;
                };

                

                foreach (PivotIcon t in navigator.Children)
                {
                    t.Tapped += (About, b) =>
                    {
                        //TODO: finish this
                        int index = int.Parse(t.Tag as string);
                        var sections = hub.Sections[index];
                        hub.ScrollToSection(sections);
                        movedTo(index);
                    };

                    header_Copy.Text = User.Current.TotalCoins.ToString();
                
                }
            }
        }

        public void RefreshTodoVisibility()
        {
            
            /*if (User.Current.HideToDos)
            {
                hub.Sections[3].Tag = "2";
                pivotToDo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                navigator.ColumnDefinitions[2].Width = new GridLength(0);
                if (hub.Sections.Count != 3)
                {
                    hub.Sections.Remove(hub.Sections[2]);
                }
                
            }
            else
            {
                hub.Sections[2].Tag = "3";
                pivotToDo.Visibility = Windows.UI.Xaml.Visibility.Visible;
                navigator.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                if (hub.Sections.Count != 4)
                {
                    var last = hub.Sections.Last();
                    hub.Sections.Remove(last);
                    hub.Sections.Add(todoHub);
                    hub.Sections.Add(last);
                }
            }*/

            if (User.Current.HideToDos)
            {
                pivotRewards.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                navigator.ColumnDefinitions[3].Width = new GridLength(0);
                if (hub.Sections.Count != 3)
                {
                    hub.Sections.Remove(hub.Sections[2]);
                }
                pivotToDo.Foreground = App.Current.Resources["orange"] as SolidColorBrush;
                pivotToDo.Icon = "";
            }
            else
            {
                pivotRewards.Visibility = Windows.UI.Xaml.Visibility.Visible;
                navigator.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Star);
                if (hub.Sections.Count != 4)
                {
                    var last = hub.Sections.Last();
                    hub.Sections.Remove(last);
                    hub.Sections.Add(todoHub);
                    hub.Sections.Add(last);
                }
                pivotToDo.Foreground = App.Current.Resources["blue"] as SolidColorBrush;
                pivotToDo.Icon = "";
            }
        }

        public void movedTo(int index)
        {
            foreach (PivotIcon t in navigator.Children) t.Selected = false;

            if (index == 2 && User.Current.HideToDos) index = 3;
            BottomAppBar.Background = Colors[index];
            //(navigator.Children[index] as PivotIcon).Selected = true;
            switch (index)
            {
                case 0:
                    {
                        header.Text = "Your information";

                        pivotInfo.Selected = true;
                        break;
                    }
                case 1:
                    {
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
                        (User.Current.HideToDos ? pivotToDo : pivotRewards).Selected = true;
                        break;
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

        private void search(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchPage));
        }



        
    }
}