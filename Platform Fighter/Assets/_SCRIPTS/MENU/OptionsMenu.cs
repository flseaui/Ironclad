﻿using System;
using ATTRIBUTES;
using DATA;

namespace MENU
{
    [MenuType(Types.Menu.OptionsMenu)]
    public class OptionsMenu : Menu
    {
        protected override void SwitchToThis(params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}