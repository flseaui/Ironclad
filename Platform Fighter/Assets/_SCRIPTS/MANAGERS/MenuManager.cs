using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MENU;
using MISC;
using UnityEngine;
using Types = DATA.Types;

namespace MANAGERS
{
    public class MenuManager : Singleton<MenuManager>, INotifyPropertyChanged
    {
        public delegate void MenuChangedEventHandler(object sender, MenuChangedEventArgs e);

        private Types.Menu _menuState;
        private Types.Menu _previousMenuState;

        [SerializeField] private Types.Menu _startingMenu = Types.Menu.BlankMenu;

        public Dictionary<Types.Menu, Menu> Menus = new Dictionary<Types.Menu, Menu>();

        public Types.Menu MenuState
        {
            get => _menuState;
            set
            {
                if (value == _menuState) return;

                _previousMenuState = _menuState;
                _menuState = value;

                if (_previousMenuState != value)
                {
                    Menus[_menuState].transform.FindObjectsWithTag("MenuPanel").FirstOrDefault()?.SetActive(true);
                    Menus[_previousMenuState].transform.FindObjectsWithTag("MenuPanel").FirstOrDefault()
                        ?.SetActive(false);
                }

                OnPropertyChanged();
                OnMenuStateChanged(new MenuChangedEventArgs(_menuState));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SwitchToMenuWithArgs(Types.Menu menu, params string[] args)
        {
            if (menu == _menuState) return;

            _previousMenuState = _menuState;
            _menuState = menu;

            if (_previousMenuState != menu)
            {
                Menus[_menuState].transform.FindObjectsWithTag("MenuPanel").FirstOrDefault()?.SetActive(true);
                Menus[_previousMenuState].transform.FindObjectsWithTag("MenuPanel").FirstOrDefault()?.SetActive(false);
            }

            OnPropertyChanged();
            OnMenuStateChanged(new MenuChangedEventArgs(_menuState, args));
        }

        public void SwitchToPreviousMenu()
        {
            MenuState = _previousMenuState;
        }

        private void Awake()
        {
            _menuState = _startingMenu;
            _previousMenuState = _startingMenu;
        }

        private void OnMenuStateChanged(MenuChangedEventArgs e)
        {
            var handler = MenuStateChanged;
            handler?.Invoke(this, e);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, e);
        }

        // [CallerMemberName] sets propertyName to the name of the caller to the method
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public event MenuChangedEventHandler MenuStateChanged;

        public class MenuChangedEventArgs : EventArgs
        {
            public MenuChangedEventArgs(Types.Menu menu, params string[] args)
            {
                Menu = menu;
                Args = args;
            }

            public MenuChangedEventArgs(Types.Menu menu)
            {
                Menu = menu;
                Args = new string[0];
            }

            public Types.Menu Menu { get; }

            public string[] Args { get; }
        }
    }
}