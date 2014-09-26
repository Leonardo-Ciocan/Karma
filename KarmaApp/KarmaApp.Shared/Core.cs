using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using System.Diagnostics;
using Windows.Storage.Streams;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;

namespace KarmaApp
{
    public class Log
    {
        public int Value { get; set; }
        public bool Positive { get; set; }

        
        public string Name { get; set; }

        public DateTime Time { get; set;}
        public string Label
        {
            get { return String.Format("{0:f}", Time); }
        }
        /*public SolidColorBrush LogColor
        {
            get
            {
                return new SolidColorBrush(Positive ? Color.FromArgb(255, 101, 167, 101) : Color.FromArgb(255, 197, 36, 0));
            }
        }*/

        public Color LogColor
        {
            get
            {
                return (Positive ? Color.FromArgb(255, 101, 167, 101) : Color.FromArgb(255, 197, 36, 0));
            }
        }

        public TextAlignment LogAlignment
        {
            get { return true ? TextAlignment.Left : TextAlignment.Right; }
        }
    }

    public class User : INotifyPropertyChanged
    {
        public static User Current;
        public User()
        {
            Current = this;



           SearchedHabits = new ObservableCollection<Habit>();
           SearchedToDos = new ObservableCollection<ToDo>();
           SearchedRewards = new ObservableCollection<Reward>();
        }

        public ObservableCollection<Habit> Habits { get; set; }

        public ObservableCollection<Habit> SearchedHabits { get; set; }
        public ObservableCollection<ToDo> SearchedToDos { get; set; }
        public ObservableCollection<Reward> SearchedRewards { get; set; }
        public ObservableCollection<Reward> Rewards { get; set; }
        public ObservableCollection<Log> Logs { get; set; }
        public ObservableCollection<ToDo> ToDos { get; set; }

        public ObservableCollection<int> KarmaHistory { get; set; }

        

        public string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public bool _transparent;
        public bool Transparent
        {
            get { return _transparent; }
            set
            {
                _transparent = value;
                RaisePropertyChanged();
            }
        }

        public int _totalCoins = 0;
        public int TotalCoins
        {
            get { return _totalCoins; }
            set
            {
                _totalCoins = value;
                RaisePropertyChanged();
                RaisePropertyChanged("GoldCoins");
                RaisePropertyChanged("SilverCoins");
                RaisePropertyChanged("BronzeCoins");
            }
        }

        public int GoldCoins
        {
            get { return (int)Math.Floor(_totalCoins / 10000.0); }
        }

        public int SilverCoins
        {
            get { return (int)Math.Floor((_totalCoins - GoldCoins * 10000.0)/100.0); }
        }

        public int BronzeCoins
        {
            get { return _totalCoins - SilverCoins * 100; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        public void Log(Log s)
        {
            if (Logs.Count > 50) Logs.RemoveAt(0);
            Logs.Add(s);
            LogKarma(TotalCoins);
        }

        public void LogKarma(int k)
        {
            if (KarmaHistory.Count > 50) KarmaHistory.RemoveAt(0);
            KarmaHistory.Add(k);
        }

        

        StorageFolder folder = ApplicationData.Current.RoamingFolder;


        public async void Save()
        {
            StorageFile file = await folder.CreateFileAsync("user.data", CreationCollisionOption.OpenIfExists);
            string json = "";
            await Task.Factory.StartNew(() => { json = Newtonsoft.Json.JsonConvert.SerializeObject(this); });
            await FileIO.WriteTextAsync(file, json);
        }

        public async Task<User> Load()
        {
            try
            {
                StorageFile file = await folder.GetFileAsync("user.data");
                string json = await FileIO.ReadTextAsync(file);
                Current = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(json);
                return Current;
            }
            catch
            {
                return null;
            }
        }

        public static bool firstRun()
        {
            /*bool s;
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue<bool>("first", out s))
            {
                IsolatedStorageSettings.ApplicationSettings["first"] = true;
                IsolatedStorageSettings.ApplicationSettings.Save();
                return true;
            }
            else
            {
                return false;
            }*/
            object k;
            if (!ApplicationData.Current.RoamingSettings.Values.TryGetValue("firstRun", out k))
            {
                ApplicationData.Current.RoamingSettings.Values["firstRun"] = true;
                return true;
            }
            return false;
        }

        [Newtonsoft.Json.JsonIgnore]
        public int DefaultValue
        {
            get
            {
                object k;
                ApplicationData.Current.RoamingSettings.Values.TryGetValue("defaultValue", out k);
                if(k == null){
                    ApplicationData.Current.RoamingSettings.Values["defaultValue"] = 150;
                    return 150;
                    
                }
                else
                {
                    return (int)k;
                }
            }
            set{
                ApplicationData.Current.RoamingSettings.Values["defaultValue"] = value;
            }
        }



        [Newtonsoft.Json.JsonIgnore]
        public bool DeleteOnCheck
        {
            get
            {
                object k;
                if (!ApplicationData.Current.RoamingSettings.Values.TryGetValue("DeleteOnCheck", out k))
                {
                    ApplicationData.Current.RoamingSettings.Values["DeleteOnCheck"] = false;
                    return false;
                }
                else
                {
                    return (bool)k;
                }
            }
            set
            {
                ApplicationData.Current.RoamingSettings.Values["DeleteOnCheck"] = value;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public bool HideToDos
        {
            get
            {
                object k;
                if (!ApplicationData.Current.RoamingSettings.Values.TryGetValue("HideToDos", out k))
                {
                    ApplicationData.Current.RoamingSettings.Values["HideToDos"] = false;
                    return false;
                }
                else
                {
                    return (bool)k;
                }
            }
            set
            {
                ApplicationData.Current.RoamingSettings.Values["HideToDos"] = value;
            }
        }


        [Newtonsoft.Json.JsonIgnore]
        public static bool Changes;


    }
}
