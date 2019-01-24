using System;
using UnityEngine;

namespace NETWORKING
{
    public static class Events
    {
        public static Action<NetworkIdentity, InputChange[], Vector2, bool> OnInputsChanged;
        public static Action<NetworkIdentity> OnMatchJoined;

        // Users steam id, Recievers steam id, current frame
        public static Action<float> OnPingSent;
        
        public static Action<float, ulong> OnPingCalculated;
    }
}