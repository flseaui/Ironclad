using System;
using UnityEngine;

namespace NETWORKING
{
    public static class Events
    {
        public static Action<NetworkIdentity, Vector2, bool> OnEntityMoved;
    }
}