using System;
using System.Collections.Generic;
using System.Linq;
using DATA;

namespace MISC
{
    public class GameFlags
    {
        private static GameFlags _instance;

        public static GameFlags Instance => _instance ?? (_instance = new GameFlags());

        private readonly Dictionary<Types.GameFlags, Types.FlagState> _flags;

        private GameFlags()
        {
            _flags = new Dictionary<Types.GameFlags, Types.FlagState>();
            foreach (var flag in Enum.GetValues(typeof(Types.GameFlags)).Cast<Types.GameFlags>())
                _flags.Add(flag, Types.FlagState.Resolved);
        }

        public bool CheckFlag(Types.GameFlags flag, Action action)
        {
            if (_flags[flag] == Types.FlagState.Pending)
            {
                _flags[flag] = Types.FlagState.Resolved;
                action();
                return true;
            }

            return false;
        }

        public void RaiseFlag(Types.GameFlags flag)
        {
            _flags[flag] = Types.FlagState.Pending;
        }
    }
}