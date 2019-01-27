using System.Reflection;
using ATTRIBUTES;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.BlankMenu)]
    public abstract class Menu : MonoBehaviour
    {
        public Types.Menu Type { get; private set; }

        protected void Awake()
        {
            Type = GetType().GetCustomAttribute<MenuTypeAttribute>().MenuType;
            MenuManager.Instance.MenuStateChanged += SetupMenu;
            MenuManager.Instance.Menus.Add(Type, this);
        }

        private void SetupMenu(object sender, MenuManager.MenuChangedEventArgs e)
        {
            if (e.Menu != Type) return;

            SwitchToThis(e.Args);
        }

        protected abstract void SwitchToThis(params string[] args);
    }
}