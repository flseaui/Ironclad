using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MISC;
using UnityEngine;
using Types = DATA.Types;

namespace MANAGERS
{
    public class MenuManager : Singleton<MenuManager>, INotifyPropertyChanged
    {
        private Types.Menu _menuState;

        public Types.Menu MenuState
        {
            get => _menuState;
            set
            {
                if (value == _menuState) return;

                _menuState = value;
                OnPropertyChanged();
                OnMenuStateChanged(new MenuChangedEventArgs(_menuState));
            }
        }

        protected void OnMenuStateChanged(MenuChangedEventArgs e)
        {
            var handler = MenuStateChanged;
            handler?.Invoke(this, e);
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, e);
        }

        // [CallerMemberName] sets propertyName to the name of the caller to the method
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public class MenuChangedEventArgs : EventArgs
        {
            public MenuChangedEventArgs(Types.Menu menu)
            {
                Menu = menu;
            }

            public Types.Menu Menu { get; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void MenuChangedEventHandler(object sender, MenuChangedEventArgs e);
        public event MenuChangedEventHandler MenuStateChanged;
    }
}