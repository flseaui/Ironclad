using System;
using UnityEngine;

namespace NETWORKING
{
    public static class Events
    {
        public static Action<NetworkIdentity, Vector2, bool> OnEntityMoved;
        public static Action<NetworkIdentity, bool> OnEntitySpawned;
        public static Action<NetworkIdentity, P2PInputSet.InputChange[], bool> OnInputsChanged;
    }
}