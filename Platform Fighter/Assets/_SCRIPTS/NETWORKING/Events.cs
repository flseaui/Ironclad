using System;
using UnityEngine;

namespace NETWORKING
{
    public static class Events
    {
        public static Action<NetworkIdentity, InputChange[], Vector2, bool> InputsChanged;
        public static Action<NetworkIdentity> GameStarted;
        public static Action<int, ulong> PingCalculated;
    }
}