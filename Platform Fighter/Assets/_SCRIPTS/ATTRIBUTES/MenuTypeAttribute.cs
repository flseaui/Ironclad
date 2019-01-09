using System;
using DATA;

namespace ATTRIBUTES
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MenuTypeAttribute : Attribute
    {
        public MenuTypeAttribute(Types.Menu menuType)
        {
            MenuType = menuType;
        }

        public Types.Menu MenuType { get; set; }
    }
}