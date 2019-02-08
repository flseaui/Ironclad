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
                _flags.Add(flag, Types.FlagState.Inactive);
        }

        public Types.FlagState GetFlagState(Types.PlayerFlags flag)
        {
            return _flags[flag];
        }

        public void SetFlagState(Types.PlayerFlags flag, Types.FlagState flagState)
        {
            _flags[flag] = flagState;
        }
    }
}