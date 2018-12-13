using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerFlags : MonoBehaviour
    {
        private Dictionary<Types.Flags, Types.FlagState> _flags;

        private void Awake()
        {
            _flags = new Dictionary<Types.Flags, Types.FlagState>();
            foreach (var flag in Enum.GetValues(typeof(Types.Flags)).Cast<Types.Flags>())
                _flags.Add(flag, Types.FlagState.Inactive);
        }

        public Types.FlagState GetFlagState(Types.Flags flag) => _flags[flag];
        
        public void SetFlagState(Types.Flags flag, Types.FlagState flagState) => _flags[flag] = flagState;

    }
}