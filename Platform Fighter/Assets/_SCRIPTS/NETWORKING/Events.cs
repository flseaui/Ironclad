using System;
using UnityEngine;

namespace NETWORKING
{
    public static class Events
    {
        public static Action<NetworkIdentity, InputChange[], Vector2, bool> OnInputsChanged;
        public static Action<NetworkIdentity> OnGameStarted;

        public static Action<int> OnPingSent;
        public static Action<int, ulong> OnPingCalculated;
    }
}