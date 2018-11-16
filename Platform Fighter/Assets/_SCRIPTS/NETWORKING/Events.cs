using System;
using UnityEngine;

namespace NETWORKING
{
    public static class Events
    {
        public static Action<NetworkIdentity, P2PInputSet.InputChange[], bool> OnInputsChanged;
        public static Action<NetworkIdentity> OnMatchJoined;
        public static Action<NetworkIdentity, int> OnPingSent;
    }
}