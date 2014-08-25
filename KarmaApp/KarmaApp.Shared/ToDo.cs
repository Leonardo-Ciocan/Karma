﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KarmaApp
{
    public class ToDo : INotifyPropertyChanged
    {
        public int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

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

        public bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            User.Changes = true;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}