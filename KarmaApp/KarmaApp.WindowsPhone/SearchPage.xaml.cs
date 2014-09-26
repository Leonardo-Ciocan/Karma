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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace KarmaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
            Loaded += SearchPage_Loaded;
        }

        void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (User.Current.HideToDos)
            {
                todo_header.Visibility = Visibility.Collapsed;
                todo_panel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (User.Current.SearchedHabits == null) User.Current.SearchedHabits = new System.Collections.ObjectModel.ObservableCollection<Habit>();
            if (User.Current.SearchedToDos == null) User.Current.SearchedToDos = new System.Collections.ObjectModel.ObservableCollection<ToDo>();
            if (User.Current.SearchedRewards == null) User.Current.SearchedRewards = new System.Collections.ObjectModel.ObservableCollection<Reward>();
            DataContext = User.Current;
        }

        private void search_term(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            

            User.Current.SearchedHabits.Clear();
            foreach(Habit hx in User.Current.Habits.Where((h) => h.Name.Contains(search.Text)).ToList()){
                User.Current.SearchedHabits.Add(hx);
            }

            User.Current.SearchedToDos.Clear();
            foreach (ToDo hx in User.Current.ToDos.Where((h) => h.Name.Contains(search.Text)).ToList())
            {
                User.Current.SearchedToDos.Add(hx);
            }

            User.Current.SearchedRewards.Clear();
            foreach (Reward hx in User.Current.Rewards.Where((h) => h.Name.Contains(search.Text)).ToList())
            {
                User.Current.SearchedRewards.Add(hx);
            }

        }
    }
}
