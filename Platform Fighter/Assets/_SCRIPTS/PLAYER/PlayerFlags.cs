using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerFlags : MonoBehaviour
    {
        private Dictionary<Types.Flags, bool> _flags;

        private void Awake()
        {
            _flags = new Dictionary<Types.Flags, bool>();
            foreach (var flag in Enum.GetValues(typeof(Types.Flags)).Cast<Types.Flags>())
            {
                _flags.Add(flag, new bool());
            }
        }

        public bool FlagState(Types.Flags flag) => _flags[flag];

    }
}