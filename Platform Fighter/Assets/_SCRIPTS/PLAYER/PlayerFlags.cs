using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerFlags : MonoBehaviour
    {
        private Dictionary<Types.PlayerFlags, Types.FlagState> _flags;

        private void Awake()
        {
            _flags = new Dictionary<Types.PlayerFlags, Types.FlagState>();
            foreach (var flag in Enum.GetValues(typeof(Types.PlayerFlags)).Cast<Types.PlayerFlags>())
                _flags.Add(flag, Types.FlagState.Resolved);
        }

        public bool CheckFlag(Types.PlayerFlags flag, Action action)
        {
            if (_flags[flag] == Types.FlagState.Pending)
            {
                _flags[flag] = Types.FlagState.Resolved;
                action();
                return true;
            }

            return false;
        }

        public void RaiseFlag(Types.PlayerFlags flag)
        {
            _flags[flag] = Types.FlagState.Pending;
        }
    }
}