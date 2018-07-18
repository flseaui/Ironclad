using System.Reflection;
using ATTRIBUTES;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.BLANK_MENU)]
    public abstract class Menu : MonoBehaviour
    {
        protected void Awake()
        {
            MenuManager.Instance.MenuStateChanged += SetupMenu;
            MenuManager.Instance.Menus.Add(GetType().GetCustomAttribute<MenuTypeAttribute>().MenuType, this);
        }

        private void SetupMenu(object sender, MenuManager.MenuChangedEventArgs e)
        {
            if (!e.Menu.Equals(GetType().GetCustomAttribute<MenuTypeAttribute>().MenuType)) return;

            SwitchToThis();
        }

        protected abstract void SwitchToThis();
    }
}