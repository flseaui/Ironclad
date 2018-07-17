using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    public abstract class Menu : MonoBehaviour
    {
        public virtual Types.Menu MenuType => Types.Menu.BLANK_MENU;

        private void Awake()
        {
            MenuManager.Instance.MenuStateChanged += SetupMenu;
        }

        private void SetupMenu(object sender, MenuManager.MenuChangedEventArgs e)
        {
            if (!e.Menu.Equals(MenuType)) return;

            SwitchToMenu();
        }

        protected abstract void SwitchToMenu();
    }
}